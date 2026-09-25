namespace Wireloom;

using static IdlCompiler;

/// <summary>Emits managed enum, plugin, and type-support documents.</summary>
internal sealed class EnumEmitter
{
    public static void Emit(CompilationState compilation, IdlEnum declaration, string sourceIdlFileName)
    {
        var writer = CreateSource(declaration.Namespace, ["System"], sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> enumeration declared in <c>{sourceIdlFileName}</c>.");
        writer.OpenBlock($"public enum {EscapeIdentifier(declaration.Name)}");

        for (var index = 0; index < declaration.Members.Count; index++)
        {
            if (index > 0)
            {
                writer.BlankLine();
            }

            var member = declaration.Members[index];
            var value = member.HasExplicitValue ? $" = {member.Value}" : string.Empty;

            writer.WriteXmlSummary($"The <c>{member.Name}</c> enumeration value.");
            writer.WriteLine($"{EscapeIdentifier(member.Name)}{value},");
        }

        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(declaration.Namespace, declaration.Name), writer.ToString()));

        EmitPlugin(compilation, declaration, sourceIdlFileName);
        EmitTypeSupport(compilation, declaration, sourceIdlFileName);
    }

    private static void EmitPlugin(CompilationState compilation, IdlEnum declaration, string sourceIdlFileName)
    {
        var typeName = EscapeIdentifier(declaration.Name);
        var runtimeName = declaration.Namespace is null ? typeName : $"{EscapeQualifiedIdentifier(declaration.Namespace)}.{typeName}";
        var implementation = declaration.Namespace is null ? "Implementation" : $"{declaration.Namespace}.Implementation";

        var writer = CreateSource(implementation, PluginUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI interpreted type plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal class {typeName}Plugin : EnumTypePlugin");
        writer.OpenBlock($"internal {typeName}Plugin() : base(CreateDynamicType(isPublic: false))");
        writer.CloseBlock();
        writer.BlankLine();
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

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(implementation, $"{declaration.Name}Plugin"), writer.ToString()));
    }

    private static void EmitTypeSupport(CompilationState compilation, IdlEnum declaration, string sourceIdlFileName)
    {
        var typeName = EscapeIdentifier(declaration.Name);

        var writer = CreateSource(declaration.Namespace, TypeSupportUsings, sourceIdlFileName);

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

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(declaration.Namespace, declaration.Name + "Support"), writer.ToString()));
    }
}
