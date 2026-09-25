namespace Wireloom;

using static Wireloom.IdlCompiler;

internal sealed class DynamicTypeEmitter
{
    public static void EmitStructPlugin(
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
        var unmanagedName = EscapeIdentifier(name + "Unmanaged");
        var pluginName = EscapeIdentifier(name + "Plugin");
        var runtimeTypeName = currentNamespace is null ? typeName : $"{EscapeQualifiedIdentifier(currentNamespace)}.{typeName}";
        var idlTypeName = currentNamespace is null ? name : $"{currentNamespace.Replace(".", "::")}::{name}";
        var implementationNamespace = currentNamespace is null ? "Implementation" : $"{currentNamespace}.Implementation";

        var writer = CreateSource(implementationNamespace, PluginUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI interpreted type plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal class {pluginName} : InterpretedTypePlugin<{typeName}, {unmanagedName}>");

        if (isRecursive)
        {
            writer.WriteLine("private static DynamicType? dynamicType;");
            writer.WriteLine("private static bool isInitialized;");
            writer.BlankLine();
        }

        writer.OpenBlock($"internal {pluginName}() : base(\"{runtimeTypeName}\", isKeyed: {(inheritedFields.Concat(fields).Any(field => field.IsKey) ? "true" : "false")}, CreateDynamicType(isPublic: false))");

        if (isRecursive)
        {
            writer.WriteLine("isRecursiveType = true;");
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary($"Creates the RTI dynamic type description for <see cref=\"{typeName}\"/>.");
        writer.WriteXmlParam("isPublic", "Whether the resulting dynamic type is publicly visible to RTI.");
        writer.WriteXmlReturns("The RTI dynamic type description.");
        writer.OpenBlock("public static DynamicType CreateDynamicType(bool isPublic = true)");
        writer.WriteLine("var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);");
        writer.WriteLine("var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;");

        if (isRecursive)
        {
            writer.BlankLine();
            writer.OpenBlock("if (isPublic)");
            writer.WriteLine("throw new global::System.NotSupportedException(\"Recursive types don't support the property TypeSupport.DynamicType\");");
            writer.CloseBlock();
            writer.BlankLine();
            writer.OpenBlock("if (isInitialized)");
            writer.WriteLine($"dynamicType = tsf.CreateTypeWithAccessInfo<{unmanagedName}>(dtf.BuildStruct()\n    .WithExtensibility(ExtensibilityKind.{extensibility})\n    .WithName(\"{idlTypeName}\"));");
            writer.WriteLine("return dynamicType;");
            writer.CloseBlock();
            writer.WriteLine("isInitialized = true;");
            writer.BlankLine();
        }

        writer.BlankLine();
        writer.WriteLine("var members = new StructMember[]");
        writer.OpenBrace();

        for (var index = 0; index < fields.Count; index++)
        {
            var field = fields[index];
            var key = field.IsKey ? ", isKey: true" : string.Empty;
            var optional = field.IsOptional ? ", isOptional: true" : string.Empty;
            var comma = index == fields.Count - 1 ? string.Empty : ",";

            writer.WriteLine($"new StructMember(\"{field.Name}\", {field.BuildDynamicTypeExpression(implementationNamespace, runtimeTypeName, isRecursive)}{key}{optional}, id: {field.MemberId ?? inheritedFields.Count + index}){comma}");
        }

        writer.CloseBlock(";");
        writer.BlankLine();
        writer.WriteLine($"var result = tsf.CreateTypeWithAccessInfo<{unmanagedName}>(");
        writer.Indent();
        writer.WriteLine("dtf.BuildStruct()");
        writer.Indent();

        if (baseType is not null)
        {
            writer.WriteLine($".WithParent((StructType) {GetSupportType(baseType, implementationNamespace)}.GetDynamicTypeInternal(isPublic))");
        }

        writer.WriteLine($".WithExtensibility(ExtensibilityKind.{extensibility})");
        writer.WriteLine($".WithName(\"{idlTypeName}\")");
        writer.WriteLine(".AddMembers(members));");
        writer.Unindent();
        writer.Unindent();

        for (var index = 0; index < fields.Count; index++)
        {
            TypeSupportAnnotationEmitter.Emit(writer, index, fields[index]);
        }

        if (isRecursive)
        {
            writer.BlankLine();
            writer.OpenBlock("if (dynamicType != null && ((StructType)dynamicType).MemberCount == 0)");
            writer.WriteLine("tsf.SetStructMembers((StructType)dynamicType, members);");
            writer.CloseBlock();
            writer.WriteLine("isInitialized = false;");
        }

        writer.BlankLine();
        writer.WriteLine("return result;");
        writer.CloseBlock();
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(implementationNamespace, name + "Plugin"), writer.ToString()));
    }
}
