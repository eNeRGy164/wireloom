using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits managed union documents and their support members.</summary>
internal static class UnionEmitter
{
    /// <summary>Emits the managed, native, plugin, and type-support documents for an IDL union.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlUnion declaration, string sourceIdlFileName) =>
        EmitUnionCore(EmissionTypeProjector.ToEmissionUnion(declaration), sourceIdlFileName);

    private static IReadOnlyList<GeneratedIdlSource> EmitUnionCore(IdlEmissionUnion declaration, string sourceIdlFileName)
    {
        var names = IdlNaming.CreateGeneratedTypeNames(declaration.Namespace, declaration.Name);
        var typeName = names.ManagedTypeName;
        var defaultBranch = declaration.DefaultBranch;

        string branchSelectionSummary;
        if (defaultBranch is not null)
        {
            branchSelectionSummary = $" An unmatched discriminator selects the default branch <see cref=\"{IdlNaming.EscapeIdentifier(defaultBranch.Field.Name)}\"/>.";
        }
        else if (declaration.IsExhaustiveBoolean)
        {
            branchSelectionSummary = " Every discriminator value selects a declared branch.";
        }
        else
        {
            branchSelectionSummary = " If it matches no declared label, no branch is active.";
        }


        var writer = EmissionSupport.CreateSource(declaration.Namespace, EmissionSupport.DataTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> DDS union declared in <c>{sourceIdlFileName}</c>. The discriminator selects a branch when it matches a declared label.{branchSelectionSummary}");
        writer.OpenBlock($"public partial class {typeName} : global::System.IEquatable<{typeName}>");

        foreach (var branch in declaration.Branches)
        {
            string? initializer;

            if (branch.Plan.CSharpType == "string")
            {
                initializer = " = string.Empty;";
            }
            else
            {
                initializer = branch.Plan.IsSequence || branch.Plan.IsArray || branch.Plan.IsAggregate ? " = null!;" : ";";
            }

            writer.WriteLine($"private {IdlNaming.TypeReference(branch.Plan.CSharpType, declaration.Namespace)} {ManagedBackingFieldName(declaration, branch)}{initializer}");
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Gets the discriminator that selects the active DDS union branch.");
        writer.WriteLine($"public {ManagedDiscriminatorType(declaration)} Discriminator {{ get; private set; }}");
        writer.BlankLine();

        writer.WriteXmlSummary("Gets the discriminator value used by the parameterless constructor.");
        writer.WriteLine($"public const {ManagedDiscriminatorType(declaration)} DefaultDiscriminator = {declaration.ManagedDefaultDiscriminator};");

        foreach (var branch in declaration.Branches)
        {
            EmitUnionBranchProperty(writer, declaration, branch);
        }

        writer.BlankLine();

        string constructorBranchSummary;
        if (defaultBranch is not null)
        {
            constructorBranchSummary = $"The discriminator selects a branch matching a declared label or falls back to the default branch <see cref=\"{IdlNaming.EscapeIdentifier(defaultBranch.Field.Name)}\"/>.";
        }
        else if (declaration.IsExhaustiveBoolean)
        {
            constructorBranchSummary = "The default discriminator selects a declared branch.";
        }
        else
        {
            constructorBranchSummary = "If the default discriminator matches no declared label, no branch is active.";
        }

        writer.WriteXmlSummary($"Initializes a new union with its RTI default discriminator. {constructorBranchSummary}");
        writer.OpenBlock($"public {typeName}()");
        writer.WriteLine("Discriminator = DefaultDiscriminator;");
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary($"Initializes a copy of another <see cref=\"{typeName}\"/> union.");
        writer.WriteXmlParam("other", "The union to copy.");
        writer.WriteXmlRemarks("When <paramref name=\"other\"/> is null, the constructor leaves the discriminator at its default and does not copy a branch value.");
        writer.OpenBlock($"public {typeName}({typeName}? other)");
        writer.OpenBlock("if (other is null)");
        writer.WriteLine("return;");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine("Discriminator = other.Discriminator;");
        writer.BlankLine();
        EmitUnionBranchSwitch(writer, declaration, "other.", "this", " = ");
        writer.CloseBlock();
        writer.BlankLine();

        if (defaultBranch is not null)
        {
            EmitDefaultBranchSetter(writer, declaration, defaultBranch);
            writer.BlankLine();
        }

        writer.WriteXmlSummary("Gets the value of the currently active union branch, if any.");
        var getReturns = defaultBranch is null
            ? "The concrete value of the active branch, or <see langword=\"null\"/> when the discriminator selects no declared branch."
            : $"The concrete value of the active branch, including the default branch <see cref=\"{IdlNaming.EscapeIdentifier(defaultBranch.Field.Name)}\"/> when no explicit label matches.";
        writer.WriteXmlReturns(getReturns);
        writer.OpenBlock("public object? Get()");
        EmitUnionReturnSwitch(writer, declaration);
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Computes a hash from the discriminator and, when a branch is active, its value.");
        writer.OpenBlock("public override int GetHashCode()");
        EmitUnionHashSwitch(writer, declaration);
        writer.CloseBlock();
        writer.BlankLine();

        var equalitySummary = defaultBranch is null
            ? "Determines whether this union has the same discriminator and active branch value as <paramref name=\"other\"/>. When neither discriminator selects a branch, equality depends on the discriminator alone."
            : "Determines whether this union has the same discriminator and active branch value as <paramref name=\"other\"/>.";
        writer.WriteXmlSummary(equalitySummary);
        writer.WriteXmlParam("other", "The union to compare.");
        writer.WriteXmlReturns("<see langword=\"true\"/> when both unions select equal values; otherwise <see langword=\"false\"/>.");
        writer.OpenBlock($"public bool Equals({typeName}? other)");
        writer.OpenBlock("if (other is null || Discriminator != other.Discriminator)");
        writer.WriteLine("return false;");
        writer.CloseBlock();
        writer.BlankLine();
        writer.OpenBlock("if (global::System.Object.ReferenceEquals(this, other))");
        writer.WriteLine("return true;");
        writer.CloseBlock();
        writer.BlankLine();
        EmitUnionEqualitySwitch(writer, declaration);
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlInheritdoc();
        writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
        writer.BlankLine();

        writer.WriteXmlSummary("Formats this union as readable text.");
        writer.WriteXmlReturns("A readable string formatted by the union's type-support instance.");
        writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        writer.CloseBlock();

        return [
            new GeneratedIdlSource(names.Managed.HintName, writer.ToString()),
            .. UnionTypeSupportEmitter.Emit(declaration, names, sourceIdlFileName)
        ];
    }

    /// <summary>Emits one public union branch and its discriminator guard.</summary>
    private static void EmitUnionBranchProperty(GeneratedSourceWriter writer, IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        writer.BlankLine();
        var branchSummary = branch.IsDefault
            ? "Gets or sets the default union branch, selected when the discriminator does not match an explicit case."
            : $"Gets or sets the union branch selected when <see cref=\"Discriminator\"/> is one of: <c>{XmlDocumentationEscaping.EscapeText(string.Join(", ", branch.Labels))}</c>.";

        writer.WriteXmlSummary(branchSummary);
        var branchRemarks = "Reading this property while another branch is active throws <see cref=\"global::System.InvalidOperationException\"/>. Assigning it stores the value and changes <see cref=\"Discriminator\"/> to a label for this branch.";
        if (branch.Plan.IsString)
        {
            var stringKind = branch.Plan.IsWideString ? "wide" : "narrow";
            var unit = MemberEmissionRenderer.StringBoundUnit(branch.Plan.IsWideString);
            branchRemarks += branch.Plan.IsBoundedString
                ? $" The bound on this {stringKind} IDL string is measured in {unit}. The generated C# property does not check the bound when assigned."
                : $" This unbounded {stringKind} IDL string has an effective limit of 255 {unit}. The generated C# property does not enforce the effective limit when assigned.";
        }

        writer.WriteXmlRemarks(branchRemarks);
        var branchException = branch.IsDefault
            ? "The discriminator selects an explicit branch."
            : "The discriminator selects another branch or selects no branch.";
        writer.WriteXmlException("global::System.InvalidOperationException", branchException);

        if (branch.Plan.IsString)
        {
            writer.WriteXmlSeeAlso("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Strings_and_Wide_Strings.htm", "RTI Connext 7.7.0 string and wide-string bounds");
        }

        if (branch.Plan.Bound is int bound)
        {
            writer.WriteLine($"[Bound({bound})]");
        }

        writer.OpenBlock($"public {IdlNaming.TypeReference(branch.Plan.CSharpType, declaration.Namespace)} {branch.Plan.EscapedName}");
        writer.OpenBlock("get");
        writer.OpenBlock($"if ({declaration.SelectionCondition(branch, negated: true)})");
        writer.WriteLine($"throw new global::System.InvalidOperationException(\"{branch.Field.Name} not selected\");");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine($"return {ManagedBackingFieldName(declaration, branch)};");
        writer.CloseBlock();
        writer.OpenBlock("set");
        writer.WriteLine($"{ManagedBackingFieldName(declaration, branch)} = value;");
        writer.BlankLine();
        writer.WriteLine($"Discriminator = {declaration.BranchDiscriminator(branch)};");
        writer.CloseBlock();
        writer.CloseBlock();

        if (branch.Labels.Count > 1)
        {
            EmitMultiLabelBranchSetter(writer, declaration, branch);
        }
    }

    /// <summary>Emits the explicit-discriminator setter required for a branch with multiple labels.</summary>
    private static void EmitMultiLabelBranchSetter(GeneratedSourceWriter writer, IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        var methodName = IdlNaming.EscapeIdentifier($"Set{branch.Field.Name}");
        var validLabels = declaration.SelectionCondition(branch, negated: false, discriminatorName: "discriminator");
        var inactiveBranch = declaration.Branches.FirstOrDefault(candidate => candidate != branch);
        var inactiveReadNote = inactiveBranch is null
            ? string.Empty
            : $"\n// Reading choice.{inactiveBranch.Plan.EscapedName} here throws InvalidOperationException because that branch is inactive.";

        writer.BlankLine();
        writer.WriteXmlSummary($"Sets the {branch.Field.Name} branch with an explicit discriminator value.");
        writer.WriteXmlParam("value", $"The value for the {branch.Field.Name} branch.");
        writer.WriteXmlParam("discriminator", "A discriminator value selecting this branch.");
        writer.WriteXmlRemarks($"Assigning the <c>{branch.Field.Name}</c> property selects the first label listed in its documentation. Use this method to select another valid label for the same branch.");
        writer.WriteXmlExample($"var choice = new {IdlNaming.TypeReference(declaration.Name, declaration.Namespace)}();\nchoice.{branch.Plan.EscapedName} = default!;\nvar activeValue = choice.{branch.Plan.EscapedName};\nchoice.{methodName}(default!, {branch.Labels[0]});\nactiveValue = choice.{branch.Plan.EscapedName};{inactiveReadNote}");
        writer.WriteXmlException("global::System.ArgumentException", "The discriminator does not select this branch.");
        writer.OpenBlock($"public void {methodName}({IdlNaming.TypeReference(branch.Plan.CSharpType, declaration.Namespace)} value, {ManagedDiscriminatorType(declaration)} discriminator)");
        writer.OpenBlock($"if (!({validLabels}))");
        writer.WriteLine($"throw new global::System.ArgumentException(\"Invalid discriminator value for {branch.Field.Name}\", nameof(discriminator));");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine($"{ManagedBackingFieldName(declaration, branch)} = value;");
        writer.BlankLine();
        writer.WriteLine("Discriminator = discriminator;");
        writer.CloseBlock();
    }

    /// <summary>Emits the explicit-discriminator setter required for a default branch.</summary>
    private static void EmitDefaultBranchSetter(GeneratedSourceWriter writer, IdlEmissionUnion declaration, UnionBranchEmissionPlan defaultBranch)
    {
        var methodName = IdlNaming.EscapeIdentifier($"Set{defaultBranch.Field.Name}");
        writer.WriteXmlSummary("Sets the default branch with an explicit discriminator value.");
        writer.WriteXmlParam("value", "The value for the default branch.");
        writer.WriteXmlParam("discriminator", "A discriminator value that does not select an explicit branch.");
        writer.WriteXmlException("global::System.ArgumentException", "The discriminator selects an explicit branch.");
        writer.OpenBlock($"public void {methodName}({IdlNaming.TypeReference(defaultBranch.Plan.CSharpType, declaration.Namespace)} value, {ManagedDiscriminatorType(declaration)} discriminator)");
        writer.OpenBlock($"if ({declaration.SelectionCondition(defaultBranch, negated: true, discriminatorName: "discriminator")})");
        writer.WriteLine($"throw new global::System.ArgumentException(\"Invalid discriminator value for {defaultBranch.Field.Name}\", nameof(discriminator));");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine($"{ManagedBackingFieldName(declaration, defaultBranch)} = value;");
        writer.BlankLine();
        writer.WriteLine("Discriminator = discriminator;");
        writer.CloseBlock();
    }

    /// <summary>Emits a switch that copies branch values between two union instances.</summary>
    private static void EmitUnionBranchSwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration, string source, string destination, string assignment)
    {
        writer.OpenBlock("switch (Discriminator)");

        foreach (var branch in declaration.ExplicitBranches)
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {declaration.ManagedDiscriminatorLabel(label)}:");
            }

            writer.Indent();
            var copiedValue = branch.Plan.BuildUnionCopyExpression(source);
            writer.WriteLine($"{destination}.{ManagedBackingFieldName(declaration, branch)}{assignment}{copiedValue};");
            writer.WriteLine("break;");
            writer.Unindent();
        }

        var defaultBranch = declaration.DefaultBranch;
        if (defaultBranch is null && declaration.IsExhaustiveBoolean)
        {
            writer.CloseBlock();
            return;
        }

        writer.WriteLine("default:");
        writer.Indent();

        if (defaultBranch is not null)
        {
            var copiedValue = defaultBranch.Plan.BuildUnionCopyExpression(source);
            writer.WriteLine($"{destination}.{ManagedBackingFieldName(declaration, defaultBranch)}{assignment}{copiedValue};");
        }

        writer.WriteLine("break;");
        writer.Unindent();
        writer.CloseBlock();
    }

    /// <summary>Emits a switch that returns the active public union branch.</summary>
    private static void EmitUnionReturnSwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration)
    {
        writer.WriteLine("return Discriminator switch");
        writer.OpenBrace();

        foreach (var branch in declaration.ExplicitBranches)
        {
            var labels = string.Join(" or ", branch.Labels.Select(declaration.ManagedDiscriminatorLabel));
            writer.WriteLine($"{labels} => {IdlNaming.EscapeIdentifier(branch.Field.Name)},");
        }

        var defaultBranch = declaration.DefaultBranch;
        if (!declaration.IsExhaustiveBoolean)
        {
            var fallback = defaultBranch is null ? "null" : IdlNaming.EscapeIdentifier(defaultBranch.Field.Name);
            writer.WriteLine($"_ => {fallback},");
        }

        writer.CloseBlock(";");
    }

    /// <summary>Emits a switch that hashes the discriminator and active union branch.</summary>
    private static void EmitUnionHashSwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration)
    {
        writer.WriteLine("return Discriminator switch");
        writer.OpenBrace();

        foreach (var branch in declaration.ExplicitBranches)
        {
            var labels = string.Join(" or ", branch.Labels.Select(declaration.ManagedDiscriminatorLabel));
            writer.WriteLine($"{labels} => global::System.HashCode.Combine(Discriminator, {branch.Plan.HashValue()}),");
        }

        var defaultBranch = declaration.DefaultBranch;
        string fallback;
        if (defaultBranch is not null)
        {
            fallback = $"global::System.HashCode.Combine(Discriminator, {IdlNaming.EscapeIdentifier(defaultBranch.Field.Name)})";
        }
        else
        {
            fallback = "global::System.HashCode.Combine(Discriminator)";
        }

        if (!declaration.IsExhaustiveBoolean)
        {
            writer.WriteLine($"_ => {fallback},");
        }

        writer.CloseBlock(";");
    }

    /// <summary>Emits a switch that compares the active union branch values.</summary>
    private static void EmitUnionEqualitySwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration)
    {
        writer.WriteLine("return Discriminator switch");
        writer.OpenBrace();

        foreach (var branch in declaration.ExplicitBranches)
        {
            var labels = string.Join(" or ", branch.Labels.Select(declaration.ManagedDiscriminatorLabel));
            var thisPrefix = ManagedFieldPrefix(branch.Plan, "other");
            writer.WriteLine($"{labels} => {branch.Plan.EqualityExpression(thisPrefix: thisPrefix)},");
        }

        var defaultBranch = declaration.DefaultBranch;
        var fallback = defaultBranch is null
            ? "true"
            : defaultBranch.Plan.EqualityExpression(thisPrefix: ManagedFieldPrefix(defaultBranch.Plan, "other"));
        if (!declaration.IsExhaustiveBoolean)
        {
            writer.WriteLine($"_ => {fallback},");
        }

        writer.CloseBlock(";");
    }

    private static string ManagedFieldPrefix(MemberEmissionPlan field, params string[] shadowedNames) =>
        shadowedNames.Contains(field.Name, StringComparer.Ordinal) || field.IsArrayLoopLocalCollision ? "this." : string.Empty;

    private static string ManagedBackingFieldName(IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        var occupied = new HashSet<string>(StringComparer.Ordinal)
        {
            IdlNaming.EscapeIdentifier(declaration.Name),
            "Discriminator",
            "DefaultDiscriminator",
            "Get",
            "GetHashCode",
            "Equals",
            "ToString"
        };
        occupied.UnionWith(declaration.Branches.Select(candidate => candidate.Plan.EscapedName));

        foreach (var candidateBranch in declaration.Branches)
        {
            if (candidateBranch.Labels.Count > 1 || candidateBranch.IsDefault)
            {
                occupied.Add(IdlNaming.EscapeIdentifier($"Set{candidateBranch.Field.Name}"));
            }
        }

        foreach (var candidateBranch in declaration.Branches)
        {
            var candidate = IdlNaming.EscapeIdentifier("_" + candidateBranch.Field.Name);
            while (!occupied.Add(candidate))
            {
                candidate = IdlNaming.EscapeIdentifier("_" + candidate);
            }

            if (ReferenceEquals(candidateBranch, branch))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException($"Union branch '{branch.Field.Name}' was not found.");
    }

    private static string ManagedDiscriminatorType(IdlEmissionUnion declaration)
    {
        if (!declaration.DiscriminatorIsEnum)
        {
            return declaration.DiscriminatorCSharpType;
        }

        return IdlNaming.ResolvedTypeReference(declaration.DiscriminatorCSharpType, declaration.Namespace);
    }
}
