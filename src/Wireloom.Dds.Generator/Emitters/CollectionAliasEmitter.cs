using static Wireloom.IdlCompiler;

namespace Wireloom;

/// <summary>Emits collection and value typedef documents.</summary>
internal sealed class CollectionAliasEmitter
{
    public static void Emit(CompilationState compilation, IdlTypedef declaration, string sourceIdlFileName)
    {
        EmitTypedefDocuments(compilation, declaration, sourceIdlFileName);
    }

    internal static void EmitTypedefDocuments(CompilationState compilation, IdlTypedef declaration, string sourceIdlFileName)
    {
        var typeName = EscapeIdentifier(declaration.Name);
        string? element;

        if (declaration.IsString)
        {
            element = "string";
        }
        else if (declaration.IsCollection)
        {
            element = declaration.ElementType!;
        }
        else
        {
            element = declaration.Target;
        }

        if (!declaration.IsCollection && !declaration.IsString)
        {
            element = compilation.ResolveUnderlyingType(element, declaration.Namespace);
        }

        string? resolvedElement;
        if (declaration.IsString || IsStringType(element))
        {
            resolvedElement = "string";
        }
        else if (IsPrimitive(element))
        {
            resolvedElement = MapPrimitive(element);
        }
        else
        {
            resolvedElement = EscapeQualifiedIdentifier(ResolveTypeName(element, declaration.Namespace));
        }

        var elementReference = TypeReference(resolvedElement, declaration.Namespace);
        var requiresNullForgivingValueInitializer = !declaration.IsCollection && !IsPrimitive(element) && !compilation.IsEnum(element, declaration.Namespace);
        var typedefUsings = declaration.IsCollection ? DataTypeUsings.Concat(["System.Linq"]) : DataTypeUsings;

        var writer = CreateSource(declaration.Namespace, typedefUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> IDL typedef declared in <c>{sourceIdlFileName}</c>.");
        writer.OpenBlock($"public partial class {typeName} : IEquatable<{typeName}>");

        if (declaration.IsSequence)
        {
            var sequenceSummary = $"Gets the sequence value represented by this typedef.{(declaration.Bound is int bound ? $" Its maximum number of elements is <c>{bound}</c>." : string.Empty)}";
            writer.WriteXmlSummary(sequenceSummary);
            writer.WriteLine($"[Bound({declaration.Bound ?? 0})]");
            writer.WriteLine($"public ISequence<{elementReference}> Value {{ get; }} = null!;");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes an empty sequence typedef.");
            writer.OpenBlock($"public {typeName}()");
            writer.WriteLine($"Value = new Sequence<{elementReference}>();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with a sequence value.");
            writer.WriteXmlParam("Value", "The sequence value to store.");
            writer.OpenBlock($"public {typeName}(ISequence<{elementReference}> Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another sequence typedef.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine($"Value = new Sequence<{elementReference}>(other.Value);");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine("return Value.Count;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same sequence values as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.WriteLine("return other is not null");
            writer.Indent();
            writer.WriteLine("&& (ReferenceEquals(this, other) || Value.SequenceEqual(other.Value));");
            writer.Unindent();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this typedef.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        else if (declaration.IsArray)
        {
            var arrayElementIsAggregate = !IsPrimitive(element) && !IsCSharpPrimitive(element) && !compilation.IsEnum(element, declaration.Namespace);
            var arrayType = $"{elementReference}[{new string(',', declaration.Dimensions.Count - 1)}]";

            writer.WriteXmlSummary("Gets or sets the array value represented by this typedef.");
            writer.WriteLine($"public {arrayType} Value {{ get; set; }} = new {elementReference}[{string.Join(", ", declaration.Dimensions)}];");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes an empty array typedef.");
            writer.OpenBlock($"public {typeName}()");

            if (arrayElementIsAggregate)
            {
                ArraySourceEmitter.EmitAggregateInitialization(writer, "Value", elementReference, declaration.Dimensions);
            }

            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with an array value.");
            writer.WriteXmlParam("Value", "The array value to store.");
            writer.OpenBlock($"public {typeName}({arrayType} Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another array typedef.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine($"Value = ({arrayType})other.Value.Clone();");

            if (arrayElementIsAggregate)
            {
                ArraySourceEmitter.EmitAggregateCopy(writer, "Value", "other.Value", elementReference, declaration.Dimensions);
            }

            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine($"return Value.Cast<{elementReference}>().First().GetHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same array values as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (ReferenceEquals(this, other))");
            writer.WriteLine("return true;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine("return Value.Rank == other.Value.Rank");
            writer.Indent();
            writer.WriteLine($"&& Value.Cast<{elementReference}>().SequenceEqual(other.Value.Cast<{elementReference}>());");
            writer.Unindent();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this typedef.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        else
        {
            var valueSummary = $"Gets or sets the value represented by this typedef.{(declaration.IsString ? $" Its maximum length is <c>{declaration.StringBound}</c>." : string.Empty)}";
            writer.WriteXmlSummary(valueSummary);

            if (declaration.IsString)
            {
                writer.WriteLine($"[Bound({declaration.StringBound})]");
            }

            writer.WriteLine($"public {elementReference} Value {{ get; set; }}{(declaration.IsString ? " = string.Empty;" : requiresNullForgivingValueInitializer ? " = null!;" : string.Empty)}");
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes an empty typedef value.");
            writer.OpenBlock($"public {typeName}()");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes the typedef with a value.");
            writer.WriteXmlParam("Value", "The value to store.");
            writer.OpenBlock($"public {typeName}({elementReference} Value)");
            writer.WriteLine("this.Value = Value;");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Initializes a copy of another typedef value.");
            writer.WriteXmlParam("other", "The typedef to copy.");
            writer.OpenBlock($"public {typeName}({typeName}? other)");
            writer.OpenBlock("if (other is not null)");
            writer.WriteLine("Value = other.Value;");
            writer.CloseBlock();
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.OpenBlock("public override int GetHashCode()");
            writer.WriteLine("return Value.GetHashCode();");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlSummary("Determines whether this typedef has the same value as <paramref name=\"other\"/>.");
            writer.WriteXmlParam("other", "The typedef to compare.");
            writer.OpenBlock($"public bool Equals({typeName}? other)");
            writer.OpenBlock("if (other is null)");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (ReferenceEquals(this, other))");
            writer.WriteLine("return true;");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine("return Value.Equals(other.Value);");
            writer.CloseBlock();
            writer.BlankLine();

            writer.WriteXmlInheritdoc();
            writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
            writer.BlankLine();

            writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this typedef.");
            writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        }
        writer.CloseBlock();

        compilation.AddSource(new(CreateHintName(declaration.Namespace, declaration.Name), writer.ToString()));
        EmitAliasTypeSupport(compilation, declaration, resolvedElement, sourceIdlFileName);
    }

    private static void EmitAliasTypeSupport(CompilationState compilation, IdlTypedef declaration, string elementType, string sourceIdlFileName)
    {
        var typeName = EscapeIdentifier(declaration.Name);
        var implementation = declaration.Namespace is null ? "Implementation" : $"{declaration.Namespace}.Implementation";
        var runtime = declaration.Namespace is null ? typeName : $"{EscapeQualifiedIdentifier(declaration.Namespace)}.{typeName}";
        var elementIdlType = declaration.IsCollection ? declaration.ElementType! : elementType;
        var implementationElementType = TypeReference(elementType, implementation);
        var isString = declaration.IsString;
        var isAggregate = !declaration.IsCollection && !isString && !IsPrimitive(elementType) && !IsCSharpPrimitive(elementType) && !compilation.IsEnum(elementType, declaration.Namespace);
        var collectionElementIsAggregate = declaration.IsCollection &&
            !IsStringType(elementIdlType) &&
            !IsPrimitive(elementType) &&
            !IsCSharpPrimitive(elementType) &&
            !compilation.IsEnum(elementType, declaration.Namespace);

        string? collectionElementUnmanagedType = string.Empty;
        if (collectionElementIsAggregate)
        {
            collectionElementUnmanagedType = TypeReference(compilation.ResolveAliasNativeType(elementType, declaration.Namespace), implementation);
        }

        var writer = CreateSource(implementation, PluginUsings, sourceIdlFileName);
        writer.OpenBlock($"internal class {typeName}Plugin : InterpretedTypePlugin<{typeName}, {typeName}Unmanaged>");
        writer.OpenBlock($"internal {typeName}Plugin() : base(\"{runtime}\", isKeyed: false, CreateDynamicType(isPublic: false))");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteXmlSummary($"Creates the RTI dynamic type description for <see cref=\"{typeName}\"/>.");
        writer.WriteXmlParam("isPublic", "Whether the resulting dynamic type is publicly visible to RTI.");
        writer.WriteXmlReturns("The RTI dynamic type description.");
        writer.OpenBlock("public static DynamicType CreateDynamicType(bool isPublic = true)");
        writer.WriteLine("var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);");
        writer.WriteLine("var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;");
        writer.BlankLine();

        if (declaration.IsSequence)
        {
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", tsf.CreateSequenceWithAccessInfo(dtf, {GetDynamicElementType(elementIdlType, implementation)}, {declaration.Bound}));");
        }
        else if (declaration.IsArray)
        {
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", tsf.CreateArrayWithAccessInfo<{(collectionElementIsAggregate ? collectionElementUnmanagedType : TypeReference(elementType, implementation))}>(dtf, {GetDynamicElementType(elementIdlType, implementation)}, new uint[] {{ {string.Join(", ", declaration.Dimensions)} }}));");
        }
        else
        {
            string? dynamicType;

            if (isString)
            {
                if (declaration.IsWideString)
                {
                    dynamicType = $"dtf.CreateWideString({declaration.StringBound})";
                }
                else
                {
                    dynamicType = $"dtf.CreateString({declaration.StringBound})";
                }
            }
            else
            {
                dynamicType = $"dtf.GetPrimitiveType<{TypeReference(elementType, implementation)}>()";
            }

            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", {dynamicType});");
        }

        writer.CloseBlock();
        writer.CloseBlock();
        compilation.AddSource(new(CreateHintName(implementation, declaration.Name + "Plugin"), writer.ToString()));

        writer = CreateSource(implementation, UnmanagedTypeUsings, sourceIdlFileName);
        writer.OpenBlock($"public struct {typeName}Unmanaged : INativeTopicType<{typeName}>");
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
                nativeElementType = TypeReference(compilation.ResolveAliasNativeType(elementType, declaration.Namespace), implementation);
            }
        }

        string? collectionNative;
        if (declaration.IsSequence)
        {
            collectionNative = "NativeSeq";
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
            if (declaration.IsArray && collectionElementIsAggregate)
            {
                writer.WriteLine($"Value.Destroy<{implementationElementType}, {collectionElementUnmanagedType}>(dimension: {ArraySourceEmitter.ElementCount(declaration.Dimensions)}, optionalsOnly: optionalsOnly);");
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

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Copies native values into a managed typedef sample.");
        writer.WriteXmlParam("sample", "The managed typedef to populate.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void FromNative({typeName} sample, bool keysOnly = false)");

        if (declaration.IsSequence)
        {
            if (collectionElementIsAggregate)
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
            if (collectionElementIsAggregate)
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
                    writer.WriteLine($"Value = {NativeDefaultValue(elementType, implementationElementType)};");
                }
            }
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Copies a managed typedef sample into native storage.");
        writer.WriteXmlParam("sample", "The managed typedef to copy.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void ToNative({typeName} sample, bool keysOnly = false)");

        if (declaration.IsSequence)
        {
            if (collectionElementIsAggregate)
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
        compilation.AddSource(new GeneratedIdlSource(CreateHintName(implementation, $"{declaration.Name}Unmanaged"), writer.ToString()));

        writer = CreateSource(declaration.Namespace, TypeSupportUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides RTI Connext DDS type support for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public class {typeName}Support : TypeSupport<{typeName}>");

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{typeName}Support\"/> class.");
        writer.WriteLine($"public {typeName}Support() : base(");
        writer.Indent();
        writer.WriteLine($"new Implementation.{typeName}Plugin(),");
        writer.WriteLine($"new Lazy<DynamicType>(() => Implementation.{typeName}Plugin.CreateDynamicType(isPublic: true)))");
        writer.Unindent();
        writer.OpenBrace();
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Gets the cached RTI Connext DDS type-support instance.");
        writer.WriteLine($"public static {typeName}Support Instance {{ get; }} = ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{typeName}Support, {typeName}>();");
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(declaration.Namespace, $"{declaration.Name}Support"), writer.ToString()));
    }

    private static string NativeDefaultValue(string elementType, string implementationElementType)
    {
        if (!IsPrimitive(elementType))
        {
            return $"({implementationElementType})0";
        }

        return NormalizeIdlType(elementType) switch
        {
            "float" => "0F",
            "double" => "0D",
            "long double" => "(LongDouble)0",
            _ => "0"
        };
    }

    private static string GetDynamicElementType(string typeName, string? currentNamespace)
    {
        if (IsStringType(typeName))
        {
            var isWide = typeName.StartsWith("wstring", StringComparison.Ordinal);
            var bound = ParseStringBound(typeName);

            return isWide
                ? $"dtf.CreateWideString({bound})"
                : $"dtf.CreateString({bound})";
        }

        if (IsPrimitive(typeName))
        {
            return $"dtf.GetPrimitiveType<{MapPrimitive(typeName)}>()";
        }
        else
        {
            if (IsCSharpPrimitive(typeName))
            {
                return $"dtf.GetPrimitiveType<{typeName}>()";
            }
            else
            {
                return $"{TypeReference(EscapeQualifiedIdentifier(ResolveTypeName(typeName, null)), currentNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic)";
            }
        }
    }

    /// <summary>Determines whether a type is already expressed as a C# primitive keyword.</summary>
    private static bool IsCSharpPrimitive(string typeName) => typeName is
        "sbyte" or "byte" or "short" or "ushort" or "int" or "uint" or "long" or
        "ulong" or "char" or "bool" or "float" or "double";

    private static bool IsStringType(string? typeName) =>
        typeName is not null
        && (typeName.StartsWith("string", StringComparison.Ordinal) || typeName.StartsWith("wstring", StringComparison.Ordinal));

    private static int ParseStringBound(string typeName)
    {
        var open = typeName.IndexOf('<');
        return open < 0 || !int.TryParse(typeName[(open + 1)..^1].Trim(), out var bound)
            ? 255
            : bound;
    }
}

