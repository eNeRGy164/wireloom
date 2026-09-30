using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Owns the output entry point for native/plugin/type-support documents.</summary>
internal static class TypeSupportEmitter
{
    public static void Emit(
        CompilationContext compilation,
        string name,
        string? currentNamespace,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        bool isRecursive) =>
        EmitDocuments(compilation, name, currentNamespace, fields, inheritedFields, extensibility, sourceIdlFileName, baseType, isRecursive);

    private static void EmitDocuments(
        CompilationContext compilation,
        string name,
        string? currentNamespace,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        bool isRecursive)
    {
        var typeName = IdlNaming.EscapeIdentifier(name);
        var supportName = IdlNaming.EscapeIdentifier($"{name}Support");
        var unmanagedName = IdlNaming.EscapeIdentifier($"{name}Unmanaged");
        var pluginName = IdlNaming.EscapeIdentifier($"{name}Plugin");
        var implementationNamespace = currentNamespace is null ? "Implementation" : $"{currentNamespace}.Implementation";
        var implementationTypeName = IdlNaming.TypeReference(name, currentNamespace, implementationNamespace);
        var supportTypeName = IdlNaming.TypeReference(name, currentNamespace, currentNamespace);
        var baseUnmanagedType = baseType is null ? null : EmissionSupport.GetUnmanagedType(baseType, implementationNamespace);

        var writer = EmissionSupport.CreateSource(implementationNamespace, EmissionSupport.UnmanagedTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI native representation for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public struct {unmanagedName} : INativeTopicType<{implementationTypeName}>");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine($"private {baseUnmanagedType} parent;");
        }

        NativeTypeEmitter.Emit(writer, implementationTypeName, fields, inheritedFields, implementationNamespace, baseUnmanagedType);
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(implementationNamespace, $"{name}Unmanaged"), writer.ToString()));

        DynamicTypeEmitter.EmitStructPlugin(compilation, name, currentNamespace, fields, inheritedFields, extensibility, sourceIdlFileName, baseType, isRecursive);

        writer = EmissionSupport.CreateSource(currentNamespace, EmissionSupport.TypeSupportUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides RTI Connext DDS type support for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public class {supportName} : TypeSupport<{supportTypeName}>");

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{supportName}\"/> class.");
        writer.WriteLine($"public {supportName}() : base(");
        writer.Indent();
        writer.WriteLine($"new Implementation.{pluginName}(),");
        writer.WriteLine($"new global::System.Lazy<DynamicType>(() => Implementation.{pluginName}.CreateDynamicType(isPublic: true)))");
        writer.Unindent();
        writer.OpenBrace();
        writer.CloseBlock();
        writer.BlankLine();

        var instanceAccessors = isRecursive ? "{ get; private set; }" : "{ get; }";
        writer.WriteXmlSummary("Gets the cached RTI Connext DDS type-support instance.");
        writer.WriteLine($"public static {supportName} Instance {instanceAccessors} =");
        writer.Indent();
        writer.WriteLine($"ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{supportName}, {supportTypeName}>();");
        writer.Unindent();

        if (isRecursive)
        {
            writer.BlankLine();

            writer.WriteXmlSummary("Gets or creates the recursive type-support instance without forcing its public dynamic type.");
            writer.OpenBlock($"internal static {supportName} GetOrCreateInstanceImpl()");
            writer.OpenBlock("if (Instance is null)");
            writer.WriteLine($"Instance = ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{supportName}, {supportTypeName}>();");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine("return Instance;");
            writer.CloseBlock();
        }

        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(currentNamespace, $"{name}Support"), writer.ToString()));
    }
}
