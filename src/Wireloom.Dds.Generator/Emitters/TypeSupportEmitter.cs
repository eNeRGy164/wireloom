using System.Collections.Generic;

namespace Wireloom;

using static Wireloom.IdlCompiler;

/// <summary>Owns the output entry point for native/plugin/type-support documents.</summary>
internal sealed class TypeSupportEmitter
{
    public static void Emit(
        CompilationState compilation,
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
        CompilationState compilation,
        string name,
        string? currentNamespace,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        bool isRecursive)
    {
        var typeName = EscapeIdentifier(name);
        var supportName = EscapeIdentifier(name + "Support");
        var unmanagedName = EscapeIdentifier(name + "Unmanaged");
        var pluginName = EscapeIdentifier(name + "Plugin");
        var runtimeTypeName = currentNamespace is null ? typeName : $"{EscapeQualifiedIdentifier(currentNamespace)}.{typeName}";
        var idlTypeName = currentNamespace is null ? name : $"{currentNamespace.Replace(".", "::")}::{name}";
        var implementationNamespace = currentNamespace is null ? "Implementation" : $"{currentNamespace}.Implementation";
        var baseUnmanagedType = baseType is null ? null : GetUnmanagedType(baseType, implementationNamespace);

        var writer = CreateSource(implementationNamespace, UnmanagedTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI native representation for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public struct {unmanagedName} : INativeTopicType<{typeName}>");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine($"private {baseUnmanagedType} parent;");
        }

        NativeTypeEmitter.Emit(
            writer,
            typeName,
            fields,
            inheritedFields,
            implementationNamespace,
            baseUnmanagedType);
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(implementationNamespace, $"{name}Unmanaged"), writer.ToString()));

        DynamicTypeEmitter.EmitStructPlugin(
           compilation,
           name,
           currentNamespace,
           fields,
           inheritedFields,
           extensibility,
           sourceIdlFileName,
           baseType,
           isRecursive);

        writer = CreateSource(currentNamespace, TypeSupportUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides RTI Connext DDS type support for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public class {supportName} : TypeSupport<{typeName}>");

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{supportName}\"/> class.");
        writer.WriteLine($"public {supportName}() : base(");
        writer.Indent();
        writer.WriteLine($"new Implementation.{pluginName}(),");
        writer.WriteLine($"new Lazy<DynamicType>(() => Implementation.{pluginName}.CreateDynamicType(isPublic: true)))");
        writer.Unindent();
        writer.OpenBrace();
        writer.CloseBlock();
        writer.BlankLine();

        var instanceAccessors = isRecursive ? "{ get; private set; }" : "{ get; }";
        writer.WriteXmlSummary("Gets the cached RTI Connext DDS type-support instance.");
        writer.WriteLine($"public static {supportName} Instance {instanceAccessors} =");
        writer.Indent();
        writer.WriteLine($"ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{supportName}, {typeName}>();");
        writer.Unindent();

        if (isRecursive)
        {
            writer.BlankLine();

            writer.WriteXmlSummary("Gets or creates the recursive type-support instance without forcing its public dynamic type.");
            writer.OpenBlock($"internal static {supportName} GetOrCreateInstanceImpl()");
            writer.OpenBlock("if (Instance == null)");
            writer.WriteLine($"Instance = ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{supportName}, {typeName}>();");
            writer.CloseBlock();
            writer.WriteLine("return Instance;");
            writer.CloseBlock();
        }

        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(currentNamespace, $"{name}Support"), writer.ToString()));
    }
}
