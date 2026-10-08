using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Owns the output entry point for native/plugin/type-support documents.</summary>
internal static class TypeSupportEmitter
{
    /// <summary>Emits native, plugin, and type-support documents for a data type.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(
        GeneratedTypeNames names,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        bool isRecursive) =>
        EmitDocuments(names, fields, inheritedFields, extensibility, sourceIdlFileName, baseType, isRecursive);

    private static IReadOnlyList<GeneratedIdlSource> EmitDocuments(
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

        var writer = EmissionSupport.CreateSource(
            names.ImplementationNamespace,
            EmissionSupport.GetUnmanagedTypeUsings(fields.Concat(inheritedFields).Select(field => field.Type)),
            sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI native representation for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public struct {names.UnmanagedTypeName} : INativeTopicType<{implementationTypeName}>");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine($"private {baseUnmanagedType} parent;");
        }

        NativeTypeEmitter.Emit(writer, implementationTypeName, fields, inheritedFields, names.ImplementationNamespace, baseUnmanagedType);
        writer.CloseBlock();

        var documents = new List<GeneratedIdlSource>
        {
            new(names.Unmanaged.HintName, writer.ToString())
        };

        documents.AddRange(DynamicTypeEmitter.EmitStructPlugin(names, fields, inheritedFields, extensibility, sourceIdlFileName, baseType, isRecursive));

        var dynamicTypeExample = isRecursive ? string.Empty : $"\nvar dynamicType = {names.SupportTypeName}.Instance.DynamicType;";

        writer = EmissionSupport.CreateSource(names.Namespace, EmissionSupport.TypeSupportUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides application-facing formatting, serialization, and dynamic-type utilities for <see cref=\"{typeName}\"/>. RTI uses its internal plugin and native representation to process samples.");
        writer.WriteXmlRemarks("The referenced API documentation targets RTI Connext 7.7.0, Wireloom's current compatibility baseline. Verify API details against the runtime version used by the consuming project.");
        writer.WriteXmlSeeAlso("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_csharp/classRti_1_1Dds_1_1Topics_1_1TypeSupport.html", "RTI Connext C# TypeSupport API (7.7.0)");
        writer.WriteXmlExample($"var sample = new {typeName}();\nvar text = {names.SupportTypeName}.Instance.ToString(sample);{dynamicTypeExample}\nvar serializer = {names.SupportTypeName}.Instance.CreateSerializer();");
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
        writer.WriteXmlSummary("Gets the shared type-support instance used by the generated utilities and DDS APIs for this type.");
        writer.WriteXmlRemarks("Use this property to format or serialize samples and to inspect the dynamic type. The generated plugin and unmanaged type are runtime implementation details.");
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

        documents.Add(new GeneratedIdlSource(names.Support.HintName, writer.ToString()));
        return documents;
    }
}
