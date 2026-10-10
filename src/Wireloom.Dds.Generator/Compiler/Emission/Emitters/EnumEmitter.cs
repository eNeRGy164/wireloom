using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits managed enum, plugin, and type-support documents.</summary>
internal static class EnumEmitter
{
    /// <summary>Emits the managed enum, plugin, and type-support documents.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlEnum declaration, string sourceIdlFileName)
    {
        var names = IdlNaming.CreateGeneratedTypeNames(declaration.Namespace, declaration.Name);
        var writer = EmissionSupport.CreateSource(declaration.Namespace, [], sourceIdlFileName, nullableContext: false);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> enumeration declared in <c>{sourceIdlFileName}</c>.");
        writer.OpenBlock($"public enum {names.ManagedTypeName}");

        for (var index = 0; index < declaration.Members.Count; index++)
        {
            if (index > 0)
            {
                writer.BlankLine();
            }

            var member = declaration.Members[index];
            var value = member.HasExplicitValue ? $" = {member.Value}" : string.Empty;

            writer.WriteXmlSummary($"The <c>{member.Name}</c> enumeration value.");
            writer.WriteLine($"{IdlNaming.EscapeIdentifier(member.Name)}{value},");
        }

        writer.CloseBlock();

        return [
            new GeneratedIdlSource(names.Managed.HintName, writer.ToString()),
            .. EmitPlugin(declaration, names, sourceIdlFileName),
            .. EmitTypeSupport(declaration, names, sourceIdlFileName)
        ];
    }

    private static IReadOnlyList<GeneratedIdlSource> EmitPlugin(IdlEnum declaration, GeneratedTypeNames names, string sourceIdlFileName)
    {
        var typeName = names.ManagedTypeName;
        var runtimeName = names.RuntimeTypeName;

        var writer = EmissionSupport.CreateSource(names.ImplementationNamespace, EmissionSupport.PluginUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI runtime plugin for the <see cref=\"{typeName}\"/> enumeration. This implementation detail is not intended for application code.");
        writer.OpenBlock($"internal class {typeName}Plugin : EnumTypePlugin");

        writer.WriteXmlSummary($"Initializes the RTI plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal {typeName}Plugin() : base(CreateDynamicType(isPublic: false))");
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary($"Creates the RTI dynamic type description for <see cref=\"{typeName}\"/>.");
        writer.WriteXmlParam("isPublic", "Whether the resulting dynamic type is publicly visible to RTI.");
        writer.WriteXmlReturns("The RTI dynamic type description.");
        writer.OpenBlock("internal static DynamicType CreateDynamicType(bool isPublic = true)");
        writer.WriteLine("var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);");
        writer.WriteLine("var enumType = dtf.BuildEnum()");
        writer.Indent();
        writer.WriteLine($".WithName(\"{runtimeName.Replace(".", "::")}\")");

        foreach (var member in declaration.Members)
        {
            writer.WriteLine($".AddMember(new EnumMember(\"{member.Name}\", {member.Value}))");
        }

        writer.WriteLine($".WithExtensibility(ExtensibilityKind.{declaration.Extensibility})");
        writer.WriteLine(".Create();");
        writer.Unindent();
        writer.BlankLine();
        writer.OpenBrace();
        writer.WriteLine("var annotations = new Annotations(");
        writer.Indent();
        writer.WriteLine("TypeKind.Enumeration,");
        writer.WriteLine($"defaultValue: new AnnotationParameterValue {{ EnumValue = {declaration.DefaultMember.Value} }},");
        writer.WriteLine("minValue: null,");
        writer.WriteLine("maxValue: null,");
        writer.WriteLine("unit: null);");
        writer.Unindent();
        writer.WriteLine("enumType.SetAnnotations(annotations);");
        writer.CloseBlock();
        writer.WriteLine("return enumType;");
        writer.CloseBlock();
        writer.CloseBlock();

        return [new GeneratedIdlSource(names.Plugin.HintName, writer.ToString())];
    }

    private static IReadOnlyList<GeneratedIdlSource> EmitTypeSupport(IdlEnum declaration, GeneratedTypeNames names, string sourceIdlFileName)
    {
        var typeName = names.ManagedTypeName;

        var writer = EmissionSupport.CreateSource(declaration.Namespace, EmissionSupport.TypeSupportUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides RTI Connext DDS type support for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public class {names.SupportTypeName} : TypeSupport<{typeName}>");

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{names.SupportTypeName}\"/> class.");
        writer.WriteLine($"public {names.SupportTypeName}() : base(");
        writer.Indent();
        writer.WriteLine($"new Implementation.{names.PluginTypeName}(),");
        writer.WriteLine($"new global::System.Lazy<DynamicType>(() => Implementation.{names.PluginTypeName}.CreateDynamicType(isPublic: true)))");
        writer.Unindent();
        writer.OpenBrace();
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Gets the shared type-support instance for this enumeration.");
        writer.WriteLine($"public static {names.SupportTypeName} Instance {{ get; }} = ");
        writer.Indent();
        writer.WriteLine($"ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{names.SupportTypeName}, {typeName}>();");
        writer.Unindent();
        writer.CloseBlock();

        return [new GeneratedIdlSource(names.Support.HintName, writer.ToString())];
    }
}
