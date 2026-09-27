using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits RTI type-support registration for a collection or value typedef.</summary>
internal static class CollectionAliasTypeSupportEmitter
{
    public static void Emit(CompilationContext compilation, IdlTypedef declaration, string sourceIdlFileName)
    {
        var typeName = IdlNaming.EscapeIdentifier(declaration.Name);
        var writer = EmissionSupport.CreateSource(declaration.Namespace, EmissionSupport.TypeSupportUsings, sourceIdlFileName);

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
        writer.WriteLine($"public static {typeName}Support Instance {{ get; }} =");
        writer.Indent();
        writer.WriteLine($"ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{typeName}Support, {typeName}>();");
        writer.Unindent();
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(declaration.Namespace, $"{declaration.Name}Support"), writer.ToString()));
    }
}
