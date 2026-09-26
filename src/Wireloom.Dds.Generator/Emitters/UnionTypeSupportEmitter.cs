namespace Wireloom;

using static IdlCompiler;

internal static class UnionTypeSupportEmitter
{
    /// <summary>Emits the RTI native representation, plugin, and type support for an IDL union.</summary>
    public static void Emit(CompilationState compilation, IdlEmissionUnion declaration, string sourceIdlFileName, string implementationNamespace)
    {
        var typeName = EscapeIdentifier(declaration.Name);
        var unmanagedName = EscapeIdentifier(declaration.Name + "Unmanaged");
        var pluginName = EscapeIdentifier(declaration.Name + "Plugin");
        var supportName = EscapeIdentifier(declaration.Name + "Support");
        var runtimeTypeName = declaration.Namespace is null ? typeName : $"{EscapeQualifiedIdentifier(declaration.Namespace)}.{typeName}";
        var idlTypeName = declaration.Namespace is null ? declaration.Name : $"{declaration.Namespace.Replace(".", "::")}::{declaration.Name}";

        var writer = CreateSource(implementationNamespace, UnmanagedTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI native representation for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public struct {unmanagedName} : INativeTopicType<{typeName}>");
        writer.WriteLine($"private {declaration.DiscriminatorCSharpType} _discriminator;");
        writer.BlankLine();

        foreach (var branch in declaration.Branches)
        {
            writer.WriteLine($"private {branch.Plan.NativeStorageTypeFor(implementationNamespace)} {branch.Plan.EscapedName};");
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Releases native resources held by this union.");
        writer.WriteXmlParam("optionalsOnly", "Indicates whether only optional members should be released.");
        writer.OpenBlock("public void Destroy(bool optionalsOnly)");
        writer.OpenBlock("if (optionalsOnly)");
        writer.WriteLine("return;");
        writer.CloseBlock();

        var destroyableBranches = declaration.Branches
            .Select(branch => (Branch: branch, Statement: branch.Plan.BuildDestroyStatement(implementationNamespace)))
            .Where(item => item.Statement is not null)
            .ToArray();
        if (destroyableBranches.Length > 0)
        {
            writer.BlankLine();

            foreach (var item in destroyableBranches)
            {
                writer.WriteLine(item.Statement!);
            }
        }

        writer.CloseBlock();
        EmitUnionNativeConversion(writer, declaration, typeName, implementationNamespace, fromNative: true);
        EmitUnionNativeInitialization(writer, declaration, implementationNamespace);
        EmitUnionNativeConversion(writer, declaration, typeName, implementationNamespace, fromNative: false);
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(implementationNamespace, $"{declaration.Name}Unmanaged"), writer.ToString()));

        writer = CreateSource(implementationNamespace, PluginUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI interpreted type plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal class {pluginName} : InterpretedTypePlugin<{typeName}, {unmanagedName}>");
        writer.OpenBlock($"internal {pluginName}() : base(\"{runtimeTypeName}\", isKeyed: false, CreateDynamicType(isPublic: false))");
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary($"Creates the RTI dynamic type description for <see cref=\"{typeName}\"/>.");
        writer.WriteXmlParam("isPublic", "Whether the resulting dynamic type is publicly visible to RTI.");
        writer.WriteXmlReturns("The RTI dynamic type description.");
        writer.OpenBlock("public static DynamicType CreateDynamicType(bool isPublic = true)");
        writer.WriteLine("var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);");
        writer.WriteLine("var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;");
        writer.BlankLine();
        writer.WriteLine("var members = new UnionMember[]");
        writer.OpenBrace();

        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            var branch = declaration.Branches[index];
            var dynamicType = branch.Plan.BuildDynamicTypeExpression(implementationNamespace);
            var labels = branch.IsDefault
                ? "new int[] { (int)UnionMember.DefaultLabel }"
                : $"new int[] {{ {string.Join(", ", branch.LabelValues)} }}";

            writer.WriteLine($"new UnionMember(\"{branch.Field.Name}\", {dynamicType}, {labels}, id: {index + 1}){(index == declaration.Branches.Count - 1 ? string.Empty : ",")}");
        }

        writer.CloseBlock(";");
        writer.BlankLine();
        writer.WriteLine($"var result = tsf.CreateTypeWithAccessInfo<{unmanagedName}>(");
        writer.Indent();
        writer.WriteLine("dtf.BuildUnion()");
        writer.Indent();

        if (declaration.DiscriminatorIsEnum)
        {
            writer.WriteLine($".WithDiscriminator({TypeReference(declaration.DiscriminatorCSharpType, implementationNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic))");
        }
        else
        {
            writer.WriteLine($".WithDiscriminator(dtf.GetPrimitiveType<{declaration.DiscriminatorCSharpType}>())");
        }

        writer.WriteLine($".WithExtensibility(ExtensibilityKind.{declaration.Extensibility})");
        writer.WriteLine($".WithName(\"{idlTypeName}\")");
        writer.WriteLine(".AddMembers(members));");
        writer.Unindent();
        writer.Unindent();

        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            TypeSupportAnnotationEmitter.Emit(writer, index, declaration.Branches[index].Plan);
        }

        writer.BlankLine();
        writer.WriteLine("return result;");
        writer.CloseBlock();
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(implementationNamespace, $"{declaration.Name}Plugin"), writer.ToString()));

        writer = CreateSource(declaration.Namespace, TypeSupportUsings, sourceIdlFileName);
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
        writer.WriteXmlSummary("Gets the cached RTI Connext DDS type-support instance.");
        writer.WriteLine($"public static {supportName} Instance {{ get; }} = ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{supportName}, {typeName}>();");
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(declaration.Namespace, $"{declaration.Name}Support"), writer.ToString()));
    }

    /// <summary>Emits native-to-managed or managed-to-native conversion for the selected branch.</summary>
    private static void EmitUnionNativeConversion(GeneratedSourceWriter writer, IdlEmissionUnion declaration, string typeName, string implementationNamespace, bool fromNative)
    {
        writer.BlankLine();

        if (fromNative)
        {
            writer.WriteXmlSummary("Copies the active native union branch into a managed sample.");
            writer.WriteXmlParam("sample", "The managed sample to populate.");
            writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
            writer.OpenBlock($"public void FromNative({typeName} sample, bool keysOnly = false)");
        }
        else
        {
            writer.WriteXmlSummary("Copies the active managed union branch into native storage.");
            writer.WriteXmlParam("sample", "The managed sample to copy.");
            writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
            writer.OpenBlock($"public void ToNative({typeName} sample, bool keysOnly = false)");
            writer.WriteLine("_discriminator = sample.Discriminator;");
            writer.BlankLine();
        }

        writer.OpenBlock("switch (_discriminator)");

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {label}:");
            }

            writer.Indent();

            if (fromNative)
            {
                EmitManagedBranchInitialization(writer, branch);

                var statement = branch.Plan.BuildFromNativeStatement(false, implementationNamespace);

                if (branch.Labels.Count > 1)
                {
                    var valueExpression = branch.Plan.BuildFromNativeValueExpression();
                    if (valueExpression is not null)
                    {
                        statement = $"sample.Set{EscapeIdentifier(branch.Field.Name)}({valueExpression}, _discriminator);";
                    }
                }

                writer.WriteLine(statement);
            }
            else
            {
                writer.WriteLine(branch.Plan.BuildToNativeStatement(false, implementationNamespace));
            }

            writer.WriteLine("break;");
            writer.BlankLine();
            writer.Unindent();
        }

        writer.WriteLine("default:");
        writer.Indent();

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        if (defaultBranch is not null)
        {
            if (fromNative)
            {
                EmitManagedBranchInitialization(writer, defaultBranch);
                writer.WriteLine(defaultBranch.Plan.BuildFromNativeStatement(false, implementationNamespace));
            }
            else
            {
                writer.WriteLine(defaultBranch.Plan.BuildToNativeStatement(false, implementationNamespace));
            }
        }

        writer.WriteLine("break;");
        writer.Unindent();
        writer.CloseBlock();
        writer.CloseBlock();
    }

    private static void EmitManagedBranchInitialization(GeneratedSourceWriter writer, UnionBranchEmissionPlan branch)
    {
        var initialization = branch.Plan.ManagedDefaultInitializationStatement;
        if (initialization is null)
        {
            return;
        }

        writer.OpenBlock("if (sample.Discriminator != _discriminator)");
        writer.WriteLine($"sample.{initialization}");
        writer.CloseBlock();
        writer.BlankLine();
    }

    /// <summary>Emits default initialization for a union's native storage.</summary>
    private static void EmitUnionNativeInitialization(GeneratedSourceWriter writer, IdlEmissionUnion declaration, string implementationNamespace)
    {
        writer.BlankLine();

        writer.WriteXmlSummary("Initializes this native union representation to its IDL default values.");
        writer.WriteXmlParam("allocatePointers", "Whether pointer members should be allocated.");
        writer.WriteXmlParam("allocateMemory", "Whether native memory should be allocated.");
        writer.OpenBlock("public void Initialize(bool allocatePointers = true, bool allocateMemory = true)");
        writer.WriteLine($"_discriminator = {EscapeIdentifier(declaration.Name)}.DefaultDiscriminator;");

        var initializationStatements = declaration.Branches
            .Select(branch => branch.Plan.UnionDefaultInitializationStatement(implementationNamespace))
            .ToArray();

        if (initializationStatements.Length > 0)
        {
            writer.BlankLine();

            for (var index = 0; index < initializationStatements.Length; index++)
            {
                if (index == initializationStatements.Length - 1 && index > 0)
                {
                    writer.BlankLine();
                }

                writer.WriteLine(initializationStatements[index]);
            }
        }

        writer.CloseBlock();
    }
}
