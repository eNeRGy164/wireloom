namespace Wireloom;

using static EmissionTypeProjector;
using static IdlCompiler;

internal sealed class UnionEmitter
{
    /// <summary>Emits the managed, native, plugin, and type-support documents for an IDL union.</summary>
    public static void Emit(CompilationState compilation, IdlUnion declaration, string sourceIdlFileName)
    {
        EmitUnionCore(compilation, ToEmissionUnion(declaration), sourceIdlFileName);
    }

    private static void EmitUnionCore(CompilationState compilation, IdlEmissionUnion declaration, string sourceIdlFileName)
    {
        var typeName = EscapeIdentifier(declaration.Name);
        var implementationNamespace = declaration.Namespace is null ? "Implementation" : $"{declaration.Namespace}.Implementation";
        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);

        var writer = CreateSource(declaration.Namespace, DataTypeUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Represents the <c>{declaration.Name}</c> DDS union declared in <c>{sourceIdlFileName}</c>. Exactly one branch is selected by <see cref=\"Discriminator\"/>.");
        writer.OpenBlock($"public partial class {typeName} : IEquatable<{typeName}>");

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

            writer.WriteLine($"private {TypeReference(branch.Plan.CSharpType, declaration.Namespace)} _{branch.Plan.EscapedName}{initializer}");
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Gets the discriminator that selects the active DDS union branch.");
        writer.WriteLine($"public {declaration.DiscriminatorCSharpType} Discriminator {{ get; private set; }}");
        writer.BlankLine();

        writer.WriteXmlSummary("Gets the discriminator value used to initialize this union.");
        writer.WriteLine($"public const {declaration.DiscriminatorCSharpType} DefaultDiscriminator = 0;");

        foreach (var branch in declaration.Branches)
        {
            EmitUnionBranchProperty(writer, declaration, branch);
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Initializes a new union with its RTI default discriminator.");
        writer.OpenBlock($"public {typeName}()");
        writer.WriteLine("Discriminator = DefaultDiscriminator;");

        foreach (var branch in declaration.Branches)
        {
            if (branch.Plan.IsSequence)
            {
                writer.WriteLine(branch.Plan.ManagedDefaultInitializationStatement!);
            }
            else if (branch.Plan.IsAggregate)
            {
                writer.WriteLine(branch.Plan.ManagedDefaultInitializationStatement!);
            }
        }

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
        }

        writer.BlankLine();

        writer.WriteXmlSummary("Gets the currently active union-branch value.");
        writer.WriteXmlReturns("The value of the branch selected by <see cref=\"Discriminator\"/>.");
        writer.OpenBlock("public object Get()");
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
        writer.OpenBlock("if (ReferenceEquals(this, other))");
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

        compilation.AddSource(new GeneratedIdlSource(CreateHintName(declaration.Namespace, declaration.Name), writer.ToString()));

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

        writer.OpenBlock($"public {TypeReference(branch.Plan.CSharpType, declaration.Namespace)} {branch.Plan.EscapedName}");
        writer.OpenBlock("get");
        writer.OpenBlock($"if ({UnionSelectionCondition(declaration, branch, negated: true)})");
        writer.WriteLine($"throw new InvalidOperationException(\"{branch.Field.Name} not selected\");");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine($"return _{branch.Plan.EscapedName};");
        writer.CloseBlock();
        writer.BlankLine();
        writer.OpenBlock("set");
        writer.WriteLine($"_{branch.Plan.EscapedName} = value;");
        writer.WriteLine($"Discriminator = {UnionBranchDiscriminator(declaration, branch)};");
        writer.CloseBlock();
        writer.CloseBlock();
    }

    /// <summary>Emits the explicit-discriminator setter required for a default branch.</summary>
    private static void EmitDefaultBranchSetter(GeneratedSourceWriter writer, IdlEmissionUnion declaration, UnionBranchEmissionPlan defaultBranch)
    {
        var methodName = "Set" + EscapeIdentifier(defaultBranch.Field.Name);

        writer.WriteXmlSummary("Sets the default branch with an explicit discriminator value.");
        writer.WriteXmlParam("value", "The value for the default branch.");
        writer.WriteXmlParam("discriminator", "A discriminator value that does not select an explicit branch.");
        writer.OpenBlock($"public void {methodName}({TypeReference(defaultBranch.Plan.CSharpType, declaration.Namespace)} value, {declaration.DiscriminatorCSharpType} discriminator)");
        var explicitLabels = declaration.Branches.Where(branch => !branch.IsDefault)
            .SelectMany(branch => branch.Labels).Select(label => $"discriminator == {label}").ToArray();
        writer.OpenBlock($"if ({string.Join(" || ", explicitLabels)})");
        writer.WriteLine($"throw new ArgumentException(\"Invalid discriminator value for {defaultBranch.Field.Name}\", nameof(discriminator));");
        writer.CloseBlock();
        writer.WriteLine($"_{defaultBranch.Plan.EscapedName} = value;");
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
                writer.WriteLine($"case {label}:");
            }

            writer.Indent();
            var copiedValue = branch.Plan.BuildUnionCopyExpression(source);
            writer.WriteLine($"{destination}._{branch.Plan.EscapedName}{assignment}{copiedValue};");
            writer.WriteLine("break;");
            writer.Unindent();
        }

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        writer.WriteLine("default:");
        writer.Indent();

        if (defaultBranch is not null)
        {
            var copiedValue = defaultBranch.Plan.BuildUnionCopyExpression(source);
            writer.WriteLine($"{destination}._{defaultBranch.Plan.EscapedName}{assignment}{copiedValue};");
        }

        writer.WriteLine("break;");
        writer.Unindent();
        writer.CloseBlock();
    }

    /// <summary>Emits a switch that returns the active public union branch.</summary>
    private static void EmitUnionReturnSwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration)
    {
        writer.OpenBlock("switch (Discriminator)");

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {label}:");
            }

            writer.Indent();
            writer.WriteLine($"return {EscapeIdentifier(branch.Field.Name)};");
            writer.Unindent();
        }

        writer.WriteLine("default:");
        writer.Indent();

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        if (defaultBranch is not null)
        {
            writer.WriteLine($"return {EscapeIdentifier(defaultBranch.Field.Name)};");
        }
        else
        {
            writer.WriteLine("throw new InvalidOperationException(\"No union branch is selected\");");
        }

        writer.Unindent();
        writer.CloseBlock();
    }

    /// <summary>Emits a switch that hashes the discriminator and active union branch.</summary>
    private static void EmitUnionHashSwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration)
    {
        writer.OpenBlock("switch (Discriminator)");

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {label}:");
            }

            writer.Indent();
            writer.WriteLine($"return HashCode.Combine(Discriminator, {EscapeIdentifier(branch.Field.Name)});");
            writer.Unindent();
        }

        writer.WriteLine("default:");
        writer.Indent();

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        if (defaultBranch is not null)
        {
            writer.WriteLine($"return HashCode.Combine(Discriminator, {EscapeIdentifier(defaultBranch.Field.Name)});");
        }
        else
        {
            writer.WriteLine("return Discriminator.GetHashCode();");
        }

        writer.Unindent();
        writer.CloseBlock();
    }

    /// <summary>Emits a switch that compares the active union branch values.</summary>
    private static void EmitUnionEqualitySwitch(GeneratedSourceWriter writer, IdlEmissionUnion declaration)
    {
        writer.OpenBlock("switch (Discriminator)");

        foreach (var branch in declaration.Branches.Where(branch => !branch.IsDefault))
        {
            foreach (var label in branch.Labels)
            {
                writer.WriteLine($"case {label}:");
            }

            writer.Indent();
            writer.WriteLine($"return {EscapeIdentifier(branch.Field.Name)}.Equals(other.{EscapeIdentifier(branch.Field.Name)});");
            writer.Unindent();
        }

        writer.WriteLine("default:");
        writer.Indent();

        var defaultBranch = declaration.Branches.SingleOrDefault(branch => branch.IsDefault);
        if (defaultBranch is not null)
        {
            var defaultField = EscapeIdentifier(defaultBranch.Field.Name);
            writer.WriteLine($"return {defaultField}.Equals(other.{defaultField});");
        }
        else
        {
            writer.WriteLine("return true;");
        }

        writer.Unindent();
        writer.CloseBlock();
    }

    /// <summary>Builds the condition that determines whether a union branch is selected.</summary>
    private static string UnionSelectionCondition(IdlEmissionUnion declaration, UnionBranchEmissionPlan branch, bool negated)
    {
        if (!branch.IsDefault)
        {
            return $"Discriminator {(negated ? "!=" : "==")} {branch.Labels[0]}";
        }

        var operators = declaration.Branches.Where(candidate => !candidate.IsDefault)
            .SelectMany(candidate => candidate.Labels)
            .Select(label => $"Discriminator {(negated ? "==" : "!=")} {label}");

        return string.Join(negated ? " || " : " && ", operators);
    }

    /// <summary>Gets the discriminator assigned by a public union-branch property setter.</summary>
    private static string UnionBranchDiscriminator(IdlEmissionUnion declaration, UnionBranchEmissionPlan branch)
    {
        if (!branch.IsDefault)
        {
            return branch.Labels[0];
        }

        if (declaration.DiscriminatorCSharpType.Contains("."))
        {
            return $"default({declaration.DiscriminatorCSharpType})";
        }

        var labels = new HashSet<int>(declaration.Branches.Where(candidate => !candidate.IsDefault)
            .SelectMany(candidate => candidate.LabelValues));
        var candidate = 0;
        while (labels.Contains(candidate))
        {
            candidate++;
        }

        return candidate.ToString();
    }

}
