using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the native representation for a collection or value typedef.</summary>
internal static class CollectionAliasNativeEmitter
{
    public static void Emit(CompilationContext compilation, EmissionResult result, IdlTypedef declaration, CollectionAliasEmissionPlan plan, string sourceIdlFileName)
    {
        var elementType = plan.ElementType;
        var typeName = IdlNaming.EscapeIdentifier(declaration.Name);
        var implementation = declaration.Namespace is null ? "Implementation" : $"{declaration.Namespace}.Implementation";
        var implementationTypeName = IdlNaming.TypeReference(typeName, declaration.Namespace, implementation);
        var elementIdlType = plan.ElementIdlType;
        var implementationElementType = IdlNaming.TypeReference(elementType, implementation);
        var isString = plan.IsString;
        var isStringSequence = declaration.IsSequence && plan.IsString;
        var stringSequenceNativeType = isStringSequence && plan.IsWideString
            ? "NativeWstringSeq"
            : "NativeStringSeq";
        var stringSequenceBound = isStringSequence ? plan.StringBound : 0;
        var isAggregate = plan.IsAggregate;
        var isUnion = plan.IsUnion;
        var collectionElementIsAggregate = declaration.IsCollection &&
            plan.CollectionElementIsAggregate;

        var collectionElementUnmanagedType = string.Empty;
        if (collectionElementIsAggregate)
        {
            collectionElementUnmanagedType = IdlNaming.TypeReference(compilation.ResolveAliasNativeType(elementType, declaration.Namespace), implementation);
        }

        var writer = EmissionSupport.CreateSource(implementation, EmissionSupport.UnmanagedTypeUsings, sourceIdlFileName);

        writer.OpenBlock($"public struct {typeName}Unmanaged : INativeTopicType<{implementationTypeName}>");

        string? nativeElementType;
        if (declaration.IsCollection)
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
                nativeElementType = IdlNaming.TypeReference(compilation.ResolveAliasNativeType(elementIdlType, declaration.Namespace), implementation);
            }
        }

        string? collectionNative;
        if (declaration.IsSequence)
        {
            collectionNative = isStringSequence ? stringSequenceNativeType : "NativeSeq";
        }
        else
        {
            if (declaration.IsArray)
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

        if (declaration.IsCollection)
        {
            if (isStringSequence)
            {
                writer.OpenBlock("if (optionalsOnly)");
                writer.WriteLine("return;");
                writer.CloseBlock();
                writer.BlankLine();
                writer.WriteLine("Value.Destroy();");
            }
            else if (declaration.IsArray && collectionElementIsAggregate)
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

        if (declaration.IsSequence)
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
            if (declaration.IsArray)
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
                    writer.WriteLine("sample.Value = Value;");
                }
            }
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Initializes this native representation to its IDL default values.");
        writer.WriteXmlParam("allocatePointers", "Whether pointer members should be allocated.");
        writer.WriteXmlParam("allocateMemory", "Whether native memory should be allocated.");
        writer.OpenBlock("public void Initialize(bool allocatePointers = true, bool allocateMemory = true)");

        if (declaration.IsSequence)
        {
            if (isStringSequence)
            {
                writer.WriteLine($"Value.Initialize(max: {declaration.Bound}, absoluteMax: {declaration.Bound}, maxStrLen: {stringSequenceBound}, allocateMemory: allocateMemory);");
            }
            else if (collectionElementIsAggregate)
            {
                writer.WriteLine($"Value.Initialize<{implementationElementType}, {collectionElementUnmanagedType}>(max: {declaration.Bound}, absoluteMax: {declaration.Bound}, allocateMemory: allocateMemory);");
            }
            else
            {
                writer.WriteLine($"Value.Initialize<{implementationElementType}>(max: {declaration.Bound}, absoluteMax: {declaration.Bound}, allocateMemory: allocateMemory);");
            }
        }
        else
        {
            if (declaration.IsArray)
            {
                if (collectionElementIsAggregate)
                {
                    writer.WriteLine($"Value.Initialize<{implementationElementType}, {collectionElementUnmanagedType}>(dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)}, allocatePointers: allocatePointers, allocateMemory: allocateMemory);");
                }
                else
                {
                    writer.WriteLine($"Value.Initialize<{implementationElementType}>(dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)}, allocateMemory: allocateMemory);");
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
                    writer.WriteLine($"Value.Initialize(size: {declaration.StringBound}, allocateMemory: allocateMemory);");
                }
                else
                {
                    writer.WriteLine($"Value = {NativeDefaultValue(elementIdlType, elementType, implementationElementType, plan)};");
                }
            }
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Copies a managed typedef sample into native storage.");
        writer.WriteXmlParam("sample", "The managed typedef to copy.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void ToNative({implementationTypeName} sample, bool keysOnly = false)");

        if (declaration.IsSequence)
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
            if (declaration.IsArray)
            {
                if (collectionElementIsAggregate)
                {
                    writer.WriteLine($"Value.ToNative<{implementationElementType}, {collectionElementUnmanagedType}>(sample.Value, keysOnly: false, dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)});");
                }
                else
                {
                    writer.WriteLine($"Value.ToNative<{implementationElementType}>(sample.Value, dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)});");
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
                    writer.WriteLine("Value = sample.Value;");
                }
            }
        }

        writer.CloseBlock();
        writer.CloseBlock();
        result.Add(IdlNaming.CreateGeneratedName(implementation, $"{declaration.Name}Unmanaged"), writer.ToString());
    }

    private static string NativeDefaultValue(string elementIdlType, string elementType, string implementationElementType, CollectionAliasEmissionPlan plan)
    {
        var normalizedElementIdlType = IdlNaming.NormalizeIdlType(elementIdlType);
        if (normalizedElementIdlType is "boolean")
        {
            return "false";
        }

        if (normalizedElementIdlType is "char" or "wchar")
        {
            return "'\\0'";
        }

        if (normalizedElementIdlType is "long long" or "int64")
        {
            return "0L";
        }

        if (plan.NativeValueRequiresCast)
        {
            return $"({implementationElementType})0";
        }

        return IdlNaming.NormalizeIdlType(elementType) switch
        {
            "long" or "int32" => "0",
            "long long" or "int64" => "0L",
            "unsigned long" or "uint32" => "0U",
            "unsigned long long" or "uint64" => "0UL",
            "float" => "0.0F",
            "double" => "0.0D",
            "long double" => "(LongDouble)0",
            _ => "0"
        };
    }
}
