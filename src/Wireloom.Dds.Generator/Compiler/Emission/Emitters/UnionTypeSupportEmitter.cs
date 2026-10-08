using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits native and type-support documents for generated unions.</summary>
internal static class UnionTypeSupportEmitter
{
    /// <summary>Emits the RTI native representation, plugin, and type support for an IDL union.</summary>
    /// <summary>Emits native, plugin, and type-support documents for a union.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlEmissionUnion declaration, GeneratedTypeNames names, string sourceIdlFileName)
    {
        var typeName = names.ManagedTypeName;
        var runtimeTypeName = names.RuntimeTypeName;
        var idlTypeName = names.IdlTypeName;
        var implementationTypeName = IdlNaming.TypeReference(typeName, names.Namespace, names.ImplementationNamespace);
        var supportTypeName = IdlNaming.TypeReference(typeName, declaration.Namespace, declaration.Namespace);
        var nativeDiscriminatorType = declaration.DiscriminatorIsEnum
            ? IdlNaming.ResolvedTypeReference(declaration.DiscriminatorCSharpType, names.ImplementationNamespace)
            : declaration.NativeDiscriminatorType;

        var writer = EmissionSupport.CreateSource(
            names.ImplementationNamespace,
            EmissionSupport.GetUnmanagedTypeUsings(declaration.Branches.Select(branch => branch.Plan.Type)),
            sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI native representation for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"public struct {names.UnmanagedTypeName} : INativeTopicType<{implementationTypeName}>");
        writer.WriteLine($"private {nativeDiscriminatorType} _discriminator;");
        writer.BlankLine();

        foreach (var branch in declaration.Branches)
        {
            writer.WriteLine($"private {branch.Plan.NativeStorageTypeFor(names.ImplementationNamespace)} {branch.Plan.EscapedName};");
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Releases native resources held by this union.");
        writer.WriteXmlParam("optionalsOnly", "Indicates whether only optional members should be released.");
        writer.OpenBlock("public void Destroy(bool optionalsOnly)");
        writer.OpenBlock("if (optionalsOnly)");
        writer.WriteLine("return;");
        writer.CloseBlock();

        var destroyableBranches = declaration.Branches
            .Select(branch => (Branch: branch, Statement: branch.Plan.BuildDestroyStatement(names.ImplementationNamespace, NativeFieldPrefix(branch.Plan, "optionalsOnly"))))
            .Where(item => item.Statement is not null)
            .ToArray();
        if (destroyableBranches.Length > 0)
        {
            writer.BlankLine();

            foreach (var (_, Statement) in destroyableBranches)
            {
                writer.WriteLine(Statement!);
            }
        }

        writer.CloseBlock();
        EmitUnionNativeConversion(writer, declaration, implementationTypeName, names.ImplementationNamespace, fromNative: true);
        EmitUnionNativeInitialization(writer, declaration, names.ImplementationNamespace);
        EmitUnionNativeConversion(writer, declaration, implementationTypeName, names.ImplementationNamespace, fromNative: false);
        writer.CloseBlock();

        var documents = new List<GeneratedIdlSource>
        {
            new(names.Unmanaged.HintName, writer.ToString())
        };

        writer = EmissionSupport.CreateSource(names.ImplementationNamespace, EmissionSupport.PluginUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI interpreted type plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal class {names.PluginTypeName} : InterpretedTypePlugin<{implementationTypeName}, {names.UnmanagedTypeName}>");
        writer.WriteXmlSummary($"Initializes the RTI plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal {names.PluginTypeName}() : base(\"{runtimeTypeName}\", isKeyed: false, CreateDynamicType(isPublic: false))");
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
            var dynamicType = branch.Plan.BuildDynamicTypeExpression(names.ImplementationNamespace);
            var labels = branch.IsDefault
                ? "new int[] { (int)UnionMember.DefaultLabel }"
                : $"new int[] {{ {string.Join(", ", branch.LabelValues)} }}";

            writer.WriteLine($"new UnionMember(\"{branch.Field.Name}\", {dynamicType}, {labels}, id: {index + 1}){(index == declaration.Branches.Count - 1 ? string.Empty : ",")}");
        }

        writer.CloseBlock(";");
        writer.BlankLine();
        writer.WriteLine($"var result = tsf.CreateTypeWithAccessInfo<{names.UnmanagedTypeName}>(");
        writer.Indent();
        writer.WriteLine("dtf.BuildUnion()");
        writer.Indent();

        if (declaration.DiscriminatorIsEnum)
        {
            string discriminatorTypeName;
            if (declaration.DiscriminatorEnumQualifiedName is { } qualifiedName)
            {
                discriminatorTypeName = IdlNaming.SupportTypeName(qualifiedName, declaration.Namespace);
            }
            else
            {
                discriminatorTypeName = declaration.DiscriminatorCSharpType;
            }

            var discriminatorType = IdlNaming.GeneratedSupportTypeReference(discriminatorTypeName, names.ImplementationNamespace);
            writer.WriteLine($".WithDiscriminator({discriminatorType}Support.Instance.GetDynamicTypeInternal(isPublic))");
        }
        else
        {
            writer.WriteLine($".WithDiscriminator(dtf.GetPrimitiveType<{declaration.PrimitiveDiscriminatorDynamicType}>())");
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

        documents.Add(new GeneratedIdlSource(names.Plugin.HintName, writer.ToString()));

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
        writer.WriteXmlSummary("Gets the shared type-support instance for this union.");
        writer.WriteXmlRemarks("Use this instance for generated type-support operations. The plugin and unmanaged representation are runtime implementation details.");
        writer.WriteLine($"public static {names.SupportTypeName} Instance {{ get; }} =");
        writer.Indent();
        writer.WriteLine($"ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<{names.SupportTypeName}, {supportTypeName}>();");
        writer.Unindent();
        writer.CloseBlock();

        documents.Add(new GeneratedIdlSource(names.Support.HintName, writer.ToString()));
        return documents;
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
            writer.WriteLine($"_discriminator = {declaration.NativeDiscriminatorWriteExpression("sample.Discriminator")};");
            writer.BlankLine();
        }

        writer.OpenBlock($"switch ({declaration.NativeDiscriminatorReadExpression("_discriminator")})");

        foreach (var branch in declaration.ExplicitBranches)
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {label}:");
            }

            writer.Indent();

            if (fromNative)
            {
                EmitManagedBranchInitialization(writer, declaration, branch);

                var nativeFieldPrefix = NativeFieldPrefix(branch.Plan, "sample", "keysOnly");
                var statement = branch.Plan.BuildFromNativeStatement(false, implementationNamespace, nativeFieldPrefix);

                if (branch.Labels.Count > 1)
                {
                    var valueExpression = branch.Plan.BuildFromNativeValueExpression(nativeFieldPrefix);
                    if (valueExpression is null)
                    {
                        writer.WriteLine(statement);
                        valueExpression = $"sample.{branch.Plan.EscapedName}";
                    }

                    statement = $"sample.{IdlNaming.EscapeIdentifier($"Set{branch.Field.Name}")}({valueExpression}, {declaration.NativeDiscriminatorReadExpression("_discriminator")});";
                }

                writer.WriteLine(statement);
            }
            else
            {
                writer.WriteLine(branch.Plan.BuildToNativeStatement(false, implementationNamespace, NativeFieldPrefix(branch.Plan, "sample", "keysOnly")));
            }

            writer.WriteLine("break;");
            writer.BlankLine();
            writer.Unindent();
        }

        if (!declaration.IsExhaustiveBoolean)
        {
            writer.WriteLine("default:");
            writer.Indent();

            var defaultBranch = declaration.DefaultBranch;
            if (defaultBranch is not null)
            {
                if (fromNative)
                {
                    EmitManagedBranchInitialization(writer, declaration, defaultBranch);
                    var nativeFieldPrefix = NativeFieldPrefix(defaultBranch.Plan, "sample", "keysOnly");
                    var statement = defaultBranch.Plan.BuildFromNativeStatement(false, implementationNamespace, nativeFieldPrefix);
                    var valueExpression = defaultBranch.Plan.BuildFromNativeValueExpression(nativeFieldPrefix);
                    if (valueExpression is null)
                    {
                        writer.WriteLine(statement);
                        valueExpression = $"sample.{defaultBranch.Plan.EscapedName}";
                    }

                    writer.WriteLine($"sample.{IdlNaming.EscapeIdentifier($"Set{defaultBranch.Field.Name}")}({valueExpression}, {declaration.NativeDiscriminatorReadExpression("_discriminator")});");
                }
                else
                {
                    writer.WriteLine(defaultBranch.Plan.BuildToNativeStatement(false, implementationNamespace, NativeFieldPrefix(defaultBranch.Plan, "sample", "keysOnly")));
                }
            }

            writer.WriteLine("break;");
            writer.Unindent();
        }

        writer.CloseBlock();
        writer.CloseBlock();
    }

    private static void EmitManagedBranchInitialization(GeneratedSourceWriter writer, IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        var initialization = branch.Plan.ManagedDefaultInitializationStatement;
        if (initialization is null)
        {
            return;
        }

        var discriminator = declaration.NativeDiscriminatorReadExpression("_discriminator");
        writer.OpenBlock($"if (sample.Discriminator != {discriminator})");
        writer.WriteLine($"sample.{initialization}");
        writer.CloseBlock();
        writer.BlankLine();
    }

    /// <summary>Emits default initialization for a union's native storage.</summary>
    private static void EmitUnionNativeInitialization(GeneratedSourceWriter writer, IdlEmissionUnion declaration, string implementationNamespace)
    {
        writer.BlankLine();

        var defaultDiscriminator = $"{IdlNaming.EscapeIdentifier(declaration.Name)}.DefaultDiscriminator";

        writer.WriteXmlSummary("Initializes this native union representation to its IDL default values.");
        writer.WriteXmlParam("allocatePointers", "Whether pointer members should be allocated.");
        writer.WriteXmlParam("allocateMemory", "Whether native memory should be allocated.");
        writer.OpenBlock("public void Initialize(bool allocatePointers = true, bool allocateMemory = true)");
        writer.WriteLine($"_discriminator = {declaration.NativeDiscriminatorWriteExpression(defaultDiscriminator)};");

        var initializationStatements = declaration.Branches
            .Select(branch => branch.Plan.UnionDefaultInitializationStatement(implementationNamespace, NativeFieldPrefix(branch.Plan, "allocatePointers", "allocateMemory")))
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

    private static string NativeFieldPrefix(MemberEmissionPlan field, params string[] parameterNames) =>
        parameterNames.Contains(field.Name, StringComparer.Ordinal) ? "this." : string.Empty;

}
