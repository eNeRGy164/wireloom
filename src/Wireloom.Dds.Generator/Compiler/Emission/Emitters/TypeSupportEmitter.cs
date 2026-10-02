using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Owns the output entry point for native/plugin/type-support documents.</summary>
internal static class TypeSupportEmitter
{
    public static void Emit(
        EmissionResult result,
        GeneratedTypeNames names,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        bool isRecursive) =>
        EmitDocuments(result, names, fields, inheritedFields, extensibility, sourceIdlFileName, baseType, isRecursive);

    private static void EmitDocuments(
        EmissionResult result,
        GeneratedTypeNames names,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        bool isRecursive)
    {
        var typeName = names.ManagedTypeName;
        var implementationTypeName = IdlNaming.TypeReference(typeName, names.Namespace, names.ImplementationNamespace);
        var supportTypeName = IdlNaming.TypeReference(typeName, names.Namespace, names.Namespace);
        var baseUnmanagedType = baseType is null ? null : EmissionSupport.GetUnmanagedType(baseType, names.ImplementationNamespace);

        var writer = EmissionSupport.CreateSource(names.ImplementationNamespace, EmissionSupport.UnmanagedTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI native representation for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public struct {names.UnmanagedTypeName} : INativeTopicType<{implementationTypeName}>");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine($"private {baseUnmanagedType} parent;");
        }

        NativeTypeEmitter.Emit(writer, implementationTypeName, fields, inheritedFields, names.ImplementationNamespace, baseUnmanagedType);
        writer.CloseBlock();

        result.Add(names.Unmanaged, writer.ToString());

        DynamicTypeEmitter.EmitStructPlugin(result, names, fields, inheritedFields, extensibility, sourceIdlFileName, baseType, isRecursive);

        writer = EmissionSupport.CreateSource(names.Namespace, EmissionSupport.TypeSupportUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides RTI Connext DDS type support for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public class {names.SupportTypeName} : TypeSupport<{supportTypeName}>");

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{names.SupportTypeName}\"/> class.");
        writer.WriteLine($"public {names.SupportTypeName}() : base(");
        writer.Indent();
        writer.WriteLine($"new Implementation.{names.PluginTypeName}(),");
        writer.WriteLine($"new global::System.Lazy<DynamicType>(() => Implementation.{names.PluginTypeName}.CreateDynamicType(isPublic: true)))");
        writer.Unindent();
        writer.OpenBrace();
        writer.CloseBlock();
        writer.BlankLine();

        var instanceAccessors = isRecursive ? "{ get; private set; }" : "{ get; }";
        writer.WriteXmlSummary("Gets the cached RTI Connext DDS type-support instance.");
        writer.WriteLine($"public static {names.SupportTypeName} Instance {instanceAccessors} =");
        writer.Indent();
        writer.WriteLine($"ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{names.SupportTypeName}, {supportTypeName}>();");
        writer.Unindent();

        if (isRecursive)
        {
            writer.BlankLine();

            writer.WriteXmlSummary("Gets or creates the recursive type-support instance without forcing its public dynamic type.");
            writer.OpenBlock($"internal static {names.SupportTypeName} GetOrCreateInstanceImpl()");
            writer.OpenBlock("if (Instance is null)");
            writer.WriteLine($"Instance = ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{names.SupportTypeName}, {supportTypeName}>();");
            writer.CloseBlock();
            writer.BlankLine();
            writer.WriteLine("return Instance;");
            writer.CloseBlock();
        }

        writer.CloseBlock();

        result.Add(names.Support, writer.ToString());
    }
}
