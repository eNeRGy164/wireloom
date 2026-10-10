using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the native representation for a collection or value typedef.</summary>
internal static class CollectionAliasNativeEmitter
{
    /// <summary>Emits the native typedef document.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlTypedef declaration, GeneratedTypeNames names, CollectionAliasEmissionPlan plan, string sourceIdlFileName)
    {
        var elementType = plan.ElementType;
        var typeName = names.ManagedTypeName;
        var implementationTypeName = IdlNaming.TypeReference(typeName, names.Namespace, names.ImplementationNamespace);
        var elementPlan = EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan);
        string implementationElementType;
        if (names.Namespace is null
            && elementPlan is not PrimitiveEmissionType
            && elementPlan is not StringEmissionType)
        {
            implementationElementType = IdlNaming.TypeReference(elementType, names.Namespace, names.ImplementationNamespace);
        }
        else
        {
            implementationElementType = IdlNaming.TypeReference(elementType, names.ImplementationNamespace);
        }
        var isString = plan.IsString;
        var isStringSequence = plan is { IsSequence: true, IsString: true };
        var stringSequenceNativeType = isStringSequence && plan.IsWideString
            ? "NativeWstringSeq"
            : "NativeStringSeq";
        var stringSequenceBound = isStringSequence ? plan.StringBound : 0;
        var isAggregate = plan.IsAggregate;
        var isUnion = plan.IsUnion;
        var collectionElementIsAggregate = plan is { IsCollection: true, CollectionElementIsAggregate: true };

        var collectionElementUnmanagedType = string.Empty;
        if (collectionElementIsAggregate)
        {
            collectionElementUnmanagedType = IdlNaming.TypeReference(plan.ElementNativeType, names.ImplementationNamespace);
        }

        var writer = EmissionSupport.CreateSource(
            names.ImplementationNamespace,
            EmissionSupport.GetUnmanagedTypeUsings([plan.ElementPlan]),
            sourceIdlFileName);

        writer.WriteXmlSummary($"Provides native storage and conversion operations for the {typeName} typedef. This type is used by RTI runtime integration and is not an application-facing value type.");
        writer.OpenBlock($"public struct {typeName}Unmanaged : INativeTopicType<{implementationTypeName}>");

        string? nativeElementType;
        if (plan.IsCollection)
        {
            nativeElementType = implementationElementType;
        }
        else
        {
            if (isString)
            {
                nativeElementType = declaration.IsWideString ? "NativeWstring" : "NativeString";
            }
            else
            {
                nativeElementType = IdlNaming.TypeReference(plan.ElementNativeType, names.ImplementationNamespace);
            }
        }

        string? collectionNative;
        if (plan.IsSequence)
        {
            collectionNative = isStringSequence ? stringSequenceNativeType : "NativeSeq";
        }
        else
        {
            if (plan.IsArray)
            {
                collectionNative = collectionElementIsAggregate ? "NativeManagedArray" : "NativeUnmanagedArray";
            }
            else
            {
                collectionNative = nativeElementType;
            }
        }

        writer.WriteLine($"private {collectionNative} Value;");
        writer.BlankLine();

        writer.WriteXmlSummary("Releases native resources held by this instance.");
        writer.WriteXmlParam("optionalsOnly", "Indicates whether only optional members should be released.");
        writer.OpenBlock("public void Destroy(bool optionalsOnly)");

        if (plan.IsCollection)
        {
            if (isStringSequence)
            {
                writer.OpenBlock("if (optionalsOnly)");
                writer.WriteLine("return;");
                writer.CloseBlock();
                writer.BlankLine();
                writer.WriteLine("Value.Destroy();");
            }
            else if (plan.IsArray && collectionElementIsAggregate)
            {
                writer.WriteLine($"Value.Destroy<{implementationElementType}, {collectionElementUnmanagedType}>(dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)}, optionalsOnly: optionalsOnly);");
            }
            else if (collectionElementIsAggregate)
            {
                writer.OpenBlock("if (optionalsOnly)");
                writer.WriteLine("return;");
                writer.CloseBlock();
                writer.BlankLine();
                writer.WriteLine($"Value.Destroy<{implementationElementType}, {collectionElementUnmanagedType}>(optionalsOnly);");
            }
            else
            {
                writer.WriteLine("Value.Destroy(optionalsOnly);");
            }
        }
        else if (isString)
        {
            writer.WriteLine("Value.Destroy();");
        }
        else if (isAggregate)
        {
            if (!isUnion)
            {
                writer.OpenBlock("if (optionalsOnly)");
                writer.WriteLine("return;");
                writer.CloseBlock();
                writer.BlankLine();
            }

            writer.WriteLine("Value.Destroy(optionalsOnly);");
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Copies native values into a managed typedef sample.");
        writer.WriteXmlParam("sample", "The managed typedef to populate.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void FromNative({implementationTypeName} sample, bool keysOnly = false)");

        if (plan.IsSequence)
        {
            if (isStringSequence)
            {
                writer.WriteLine("Value.FromNative(sample.Value);");
            }
            else if (collectionElementIsAggregate)
            {
                writer.WriteLine($"Value.FromNative<{implementationElementType}, {collectionElementUnmanagedType}>((Sequence<{implementationElementType}>)sample.Value);");
            }
            else
            {
                writer.WriteLine($"Value.FromNative((Sequence<{implementationElementType}>)sample.Value);");
            }
        }
        else
        {
            if (plan.IsArray)
            {
                if (collectionElementIsAggregate)
                {
                    writer.WriteLine($"Value.FromNative<{implementationElementType}, {collectionElementUnmanagedType}>(sample.Value, keysOnly: false, dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)});");
                }
                else
                {
                    writer.WriteLine($"Value.FromNative(sample.Value, dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)});");
                }
            }
            else
            {
                if (isAggregate)
                {
                    writer.WriteLine("Value.FromNative(sample.Value, keysOnly: false);");
                }
                else if (isString)
                {
                    writer.WriteLine("sample.Value = Value.FromNative();");
                }
                else
                {
                    writer.WriteLine($"sample.Value = {FromNativeValue(plan)};");
                }
            }
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Initializes this native representation to its IDL default values.");
        writer.WriteXmlParam("allocatePointers", "Whether pointer members should be allocated.");
        writer.WriteXmlParam("allocateMemory", "Whether native memory should be allocated.");
        writer.OpenBlock("public void Initialize(bool allocatePointers = true, bool allocateMemory = true)");

        if (plan.IsSequence)
        {
            if (isStringSequence)
            {
                writer.WriteLine($"Value.Initialize(max: {declaration.Bound}, absoluteMax: {declaration.Bound}, maxStrLen: {stringSequenceBound}, allocateMemory);");
            }
            else if (collectionElementIsAggregate)
            {
                writer.WriteLine($"Value.Initialize<{implementationElementType}, {collectionElementUnmanagedType}>(max: {declaration.Bound}, absoluteMax: {declaration.Bound}, allocateMemory);");
            }
            else
            {
                writer.WriteLine($"Value.Initialize<{implementationElementType}>(max: {declaration.Bound}, absoluteMax: {declaration.Bound}, allocateMemory);");
            }
        }
        else
        {
            if (plan.IsArray)
            {
                if (collectionElementIsAggregate)
                {
                    writer.WriteLine($"Value.Initialize<{implementationElementType}, {collectionElementUnmanagedType}>(dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)}, allocatePointers: allocatePointers, allocateMemory: allocateMemory);");
                }
                else
                {
                    writer.WriteLine($"Value.Initialize<{implementationElementType}>(dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)}, allocateMemory);");
                }
            }
            else
            {
                if (isAggregate)
                {
                    writer.WriteLine("Value.Initialize(allocatePointers, allocateMemory);");
                }
                else if (isString)
                {
                    writer.WriteLine($"Value.Initialize(size: {declaration.StringBound}, allocateMemory);");
                }
                else
                {
                    writer.WriteLine($"Value = {NativeDefaultValue(implementationElementType, plan)};");
                }
            }
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Copies a managed typedef sample into native storage.");
        writer.WriteXmlParam("sample", "The managed typedef to copy.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void ToNative({implementationTypeName} sample, bool keysOnly = false)");

        if (plan.IsSequence)
        {
            if (isStringSequence)
            {
                writer.WriteLine($"Value.ToNative(sample.Value, {stringSequenceBound});");
            }
            else if (collectionElementIsAggregate)
            {
                writer.WriteLine($"Value.ToNative<{implementationElementType}, {collectionElementUnmanagedType}>((Sequence<{implementationElementType}>)sample.Value);");
            }
            else
            {
                writer.WriteLine($"Value.ToNative((Sequence<{implementationElementType}>)sample.Value);");
            }
        }
        else
        {
            if (plan.IsArray)
            {
                if (collectionElementIsAggregate)
                {
                    writer.WriteLine($"Value.ToNative<{implementationElementType}, {collectionElementUnmanagedType}>(sample.Value, keysOnly: false, dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)});");
                }
                else
                {
                    writer.WriteLine($"Value.ToNative(sample.Value, dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)});");
                }
            }
            else
            {
                if (isAggregate)
                {
                    writer.WriteLine("Value.ToNative(sample.Value, keysOnly: false);");
                }
                else if (isString)
                {
                    writer.WriteLine($"Value.ToNative(sample.Value, {declaration.StringBound});");
                }
                else
                {
                    writer.WriteLine($"Value = {ToNativeValue(plan)};");
                }
            }
        }

        writer.CloseBlock();
        writer.CloseBlock();
        return [new GeneratedIdlSource(names.Unmanaged.HintName, writer.ToString())];
    }

    private static string NativeDefaultValue(string implementationElementType, CollectionAliasEmissionPlan plan)
    {
        var elementPlan = EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan);
        if (elementPlan is PrimitiveEmissionType primitive)
        {
            var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
            if (primitive.IdlName is "boolean" or "char" or "wchar" or "long long" or "int64")
            {
                return mapping.NativeDefaultLiteral;
            }

            if (!plan.NativeValueRequiresCast)
            {
                return mapping.UncastNativeDefaultLiteral;
            }
        }

        if (plan.NativeValueRequiresCast)
        {
            return $"({implementationElementType})0";
        }

        return "0";
    }

    private static string FromNativeValue(CollectionAliasEmissionPlan plan)
    {
        var elementPlan = EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan);
        if (elementPlan is not PrimitiveEmissionType primitive)
        {
            return "Value";
        }

        return PrimitiveTypeMapping.Resolve(primitive.IdlName).FromNativeExpression("Value");
    }

    private static string ToNativeValue(CollectionAliasEmissionPlan plan)
    {
        var elementPlan = EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan);
        if (elementPlan is not PrimitiveEmissionType primitive)
        {
            return "sample.Value";
        }

        return PrimitiveTypeMapping.Resolve(primitive.IdlName).ToNativeExpression("sample.Value");
    }
}
