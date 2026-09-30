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
    public static void Emit(CompilationContext compilation, IdlUnion declaration, string sourceIdlFileName) =>
        EmitUnionCore(compilation, EmissionTypeProjector.ToEmissionUnion(declaration), sourceIdlFileName);

    private static void EmitUnionCore(CompilationContext compilation, IdlEmissionUnion declaration, string sourceIdlFileName)
    {
        var typeName = IdlNaming.EscapeIdentifier(declaration.Name);
        var implementationNamespace = declaration.Namespace is null ? "Implementation" : $"{declaration.Namespace}.Implementation";
        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);

        var writer = EmissionSupport.CreateSource(declaration.Namespace, EmissionSupport.DataTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> DDS union declared in <c>{sourceIdlFileName}</c>. Exactly one branch is selected by <see cref=\"Discriminator\"/>.");
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

        writer.WriteXmlSummary("Gets the discriminator value used to initialize this union.");
        writer.WriteLine($"public const {ManagedDiscriminatorType(declaration)} DefaultDiscriminator = {ManagedDefaultDiscriminator(declaration)};");

        foreach (var branch in declaration.Branches)
        {
            EmitUnionBranchProperty(writer, declaration, branch);
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Initializes a new union with its RTI default discriminator.");
        writer.OpenBlock($"public {typeName}()");
        writer.WriteLine("Discriminator = DefaultDiscriminator;");
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary($"Initializes a copy of another <see cref=\"{typeName}\"/> union.");
        writer.WriteXmlParam("other", "The union to copy.");
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

        writer.WriteXmlSummary("Gets the currently active union-branch value.");
        writer.WriteXmlReturns("The value of the branch selected by <see cref=\"Discriminator\"/>.");
        writer.OpenBlock("public object? Get()");
        EmitUnionReturnSwitch(writer, declaration);
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlInheritdoc();
        writer.OpenBlock("public override int GetHashCode()");
        EmitUnionHashSwitch(writer, declaration);
        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlSummary("Determines whether this union has the same discriminator and active branch value as <paramref name=\"other\"/>.");
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

        writer.WriteXmlSummary("Returns the RTI Connext DDS representation of this union.");
        writer.WriteLine($"public override string ToString() => {typeName}Support.Instance.ToString(this);");
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(declaration.Namespace, declaration.Name), writer.ToString()));

        UnionTypeSupportEmitter.Emit(compilation, declaration, sourceIdlFileName, implementationNamespace);
    }

    /// <summary>Emits one public union branch and its discriminator guard.</summary>
    private static void EmitUnionBranchProperty(GeneratedSourceWriter writer, IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        writer.BlankLine();
        var branchSummary = branch.IsDefault
            ? "Gets or sets the default union branch, selected when the discriminator does not match an explicit case."
            : $"Gets or sets the union branch selected when <see cref=\"Discriminator\"/> is one of: <c>{string.Join(", ", branch.Labels)}</c>.";

        writer.WriteXmlSummary(branchSummary);

        if (branch.Plan.Bound is int bound)
        {
            writer.WriteLine($"[Bound({bound})]");
        }

        writer.OpenBlock($"public {IdlNaming.TypeReference(branch.Plan.CSharpType, declaration.Namespace)} {branch.Plan.EscapedName}");
        writer.OpenBlock("get");
        writer.OpenBlock($"if ({UnionSelectionCondition(declaration, branch, negated: true)})");
        writer.WriteLine($"throw new global::System.InvalidOperationException(\"{branch.Field.Name} not selected\");");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine($"return {ManagedBackingFieldName(declaration, branch)};");
        writer.CloseBlock();
        writer.OpenBlock("set");
        writer.WriteLine($"{ManagedBackingFieldName(declaration, branch)} = value;");
        writer.BlankLine();
        writer.WriteLine($"Discriminator = {UnionBranchDiscriminator(declaration, branch)};");
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
        var validLabels = string.Join(" || ", branch.Labels.Select(label => $"discriminator == {ManagedDiscriminatorLabel(label, declaration)}"));

        writer.BlankLine();
        writer.WriteXmlSummary($"Sets the {branch.Field.Name} branch with an explicit discriminator value.");
        writer.WriteXmlParam("value", $"The value for the {branch.Field.Name} branch.");
        writer.WriteXmlParam("discriminator", "A discriminator value selecting this branch.");
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
        var explicitLabels = declaration.Branches.Where(branch => !branch.IsDefault)
            .SelectMany(branch => branch.Labels).Select(label => $"discriminator == {ManagedDiscriminatorLabel(label, declaration)}").ToArray();

        writer.WriteXmlSummary("Sets the default branch with an explicit discriminator value.");
        writer.WriteXmlParam("value", "The value for the default branch.");
        writer.WriteXmlParam("discriminator", "A discriminator value that does not select an explicit branch.");
        writer.OpenBlock($"public void {methodName}({IdlNaming.TypeReference(defaultBranch.Plan.CSharpType, declaration.Namespace)} value, {ManagedDiscriminatorType(declaration)} discriminator)");
        writer.OpenBlock($"if ({string.Join(" || ", explicitLabels)})");
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

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {ManagedDiscriminatorLabel(label, declaration)}:");
            }

            writer.Indent();
            var copiedValue = branch.Plan.BuildUnionCopyExpression(source);
            writer.WriteLine($"{destination}.{ManagedBackingFieldName(declaration, branch)}{assignment}{copiedValue};");
            writer.WriteLine("break;");
            writer.Unindent();
        }

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        if (defaultBranch is null && IsExhaustiveBooleanUnion(declaration, defaultBranch))
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

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            var labels = string.Join(" or ", branch.Labels.Select(label => ManagedDiscriminatorLabel(label, declaration)));
            writer.WriteLine($"{labels} => {IdlNaming.EscapeIdentifier(branch.Field.Name)},");
        }

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        if (!IsExhaustiveBooleanUnion(declaration, defaultBranch))
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

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            var labels = string.Join(" or ", branch.Labels.Select(label => ManagedDiscriminatorLabel(label, declaration)));
            writer.WriteLine($"{labels} => global::System.HashCode.Combine(Discriminator, {branch.Plan.HashValue()}),");
        }

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        string fallback;
        if (defaultBranch is not null)
        {
            fallback = $"global::System.HashCode.Combine(Discriminator, {IdlNaming.EscapeIdentifier(defaultBranch.Field.Name)})";
        }
        else
        {
            fallback = "global::System.HashCode.Combine(Discriminator)";
        }

        if (!IsExhaustiveBooleanUnion(declaration, defaultBranch))
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

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            var labels = string.Join(" or ", branch.Labels.Select(label => ManagedDiscriminatorLabel(label, declaration)));
            var thisPrefix = ManagedFieldPrefix(branch.Plan, "other");
            writer.WriteLine($"{labels} => {branch.Plan.EqualityExpression(thisPrefix: thisPrefix)},");
        }

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        var fallback = defaultBranch is null
            ? "true"
            : defaultBranch.Plan.EqualityExpression(thisPrefix: ManagedFieldPrefix(defaultBranch.Plan, "other"));
        if (!IsExhaustiveBooleanUnion(declaration, defaultBranch))
        {
            writer.WriteLine($"_ => {fallback},");
        }

        writer.CloseBlock(";");
    }

    /// <summary>Builds the condition that determines whether a union branch is selected.</summary>
    private static string UnionSelectionCondition(IdlEmissionUnion declaration, UnionBranchEmissionPlan branch, bool negated)
    {
        if (!branch.IsDefault)
        {
            var comparison = negated ? "!=" : "==";
            var join = negated ? " && " : " || ";
            return string.Join(join, branch.Labels.Select(label => $"Discriminator {comparison} {ManagedDiscriminatorLabel(label, declaration)}"));
        }

        var operators = declaration.Branches.Where(candidate => !candidate.IsDefault)
            .SelectMany(candidate => candidate.Labels)
            .Select(label => $"Discriminator {(negated ? "==" : "!=")} {ManagedDiscriminatorLabel(label, declaration)}");

        return string.Join(negated ? " || " : " && ", operators);
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

    /// <summary>Gets the discriminator assigned by a public union-branch property setter.</summary>
    private static string UnionBranchDiscriminator(IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        if (!branch.IsDefault)
        {
            return ManagedDiscriminatorLabel(branch.Labels[0], declaration);
        }

        return ManagedDiscriminatorValue(declaration, FindUnoccupiedDiscriminatorValue(declaration));
    }

    private static string ManagedDiscriminatorType(IdlEmissionUnion declaration) =>
        IdlNaming.TypeReference(declaration.DiscriminatorCSharpType, declaration.Namespace);

    private static string ManagedDefaultDiscriminator(IdlEmissionUnion declaration)
    {
        if (declaration.DiscriminatorIsEnum)
        {
            return ManagedDiscriminatorValue(declaration, declaration.DiscriminatorDefaultValue ?? 0);
        }

        if (!declaration.Branches.Any(branch => branch.IsDefault))
        {
            return declaration.DiscriminatorCSharpType switch
            {
                "bool" => "false",
                "char" => "'\\0'",
                _ => "0"
            };
        }

        return ManagedDiscriminatorValue(declaration, FindUnoccupiedDiscriminatorValue(declaration));
    }

    private static int FindUnoccupiedDiscriminatorValue(IdlEmissionUnion declaration)
    {
        var occupied = declaration.Branches
            .Where(branch => !branch.IsDefault)
            .SelectMany(branch => branch.LabelValues)
            .Distinct()
            .OrderBy(occupiedValue => occupiedValue)
            .ToArray();
        var (minimum, maximum) = DiscriminatorRange(declaration);

        var candidate = FindFirstUnoccupiedValue(occupied, 0, maximum)
            ?? FindFirstUnoccupiedValue(occupied, minimum, -1);

        if (candidate is int value)
        {
            return value;
        }

        throw new InvalidOperationException($"Union '{declaration.Name}' has no representable default discriminator.");
    }

    private static int? FindFirstUnoccupiedValue(IReadOnlyList<int> occupied, int minimum, int maximum)
    {
        if (minimum > maximum)
        {
            return null;
        }

        long candidate = minimum;
        foreach (var occupiedValue in occupied)
        {
            if (occupiedValue < minimum)
            {
                continue;
            }

            if (occupiedValue > maximum)
            {
                break;
            }

            if (occupiedValue > candidate)
            {
                return (int)candidate;
            }

            if (occupiedValue == candidate)
            {
                candidate++;
            }
        }

        return candidate <= maximum ? (int)candidate : null;
    }

    private static (int Minimum, int Maximum) DiscriminatorRange(IdlEmissionUnion declaration) =>
        declaration.DiscriminatorCSharpType switch
        {
            "bool" => (0, 1),
            "char" => (char.MinValue, char.MaxValue),
            "sbyte" => (sbyte.MinValue, sbyte.MaxValue),
            "byte" => (byte.MinValue, byte.MaxValue),
            "short" => (short.MinValue, short.MaxValue),
            "ushort" => (ushort.MinValue, ushort.MaxValue),
            _ => (int.MinValue, int.MaxValue)
        };

    private static string ManagedDiscriminatorValue(IdlEmissionUnion declaration, int value) =>
        declaration.DiscriminatorCSharpType switch
        {
            "bool" => value == 0 ? "false" : "true",
            "char" when value == 0 => "'\\0'",
            "char" => $"(char){value}",
            _ when declaration.DiscriminatorIsEnum => $"({ManagedDiscriminatorType(declaration)}){value}",
            _ => value.ToString()
        };

    private static bool IsExhaustiveBooleanUnion(IdlEmissionUnion declaration, UnionBranchEmissionPlan? defaultBranch) =>
        declaration.DiscriminatorCSharpType == "bool" && defaultBranch is null;

    private static string ManagedDiscriminatorLabel(string label, IdlEmissionUnion declaration) =>
        IdlNaming.TypeReference(label, declaration.Namespace);
}
