using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Implements generated union construction, branch access, and value operations.</summary>
internal static class UnionEmitter
{
    /// <summary>Writes the implementation of the generated union value API.</summary>
    internal static void EmitUnion(SourceWriter source, UnionPlan declaration, bool isRti612 = false)
    {
        var type = declaration.Name;
        string Relative(string spelling) => CppTypeNameFormatter.MakeRelative(spelling, declaration.NamespaceSegments);
        source.WriteLine($"{type}::{type}() = default;");
        source.BlankLine();
        var discriminatorType = Relative(declaration.DiscriminatorType);
        source.WriteLine($"{discriminatorType}& {type}::_d()");
        source.OpenBrace();
        source.WriteLine("return discriminator_;");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"const {discriminatorType}& {type}::_d() const");
        source.WriteLine("return discriminator_;");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"void {type}::_d({discriminatorType} value)");
        source.WriteLine("discriminator_ = value;");
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"{discriminatorType} {type}::default_discriminator()");
        source.OpenBrace();
        source.WriteLine($"return {Relative(declaration.DiscriminatorDefaultValue)};");
        source.CloseBlock();

        foreach (var branch in declaration.Branches)
        {
            source.BlankLine();
            EmitUnionBranchAccessors(source, declaration, branch, isRti612);
        }

        source.BlankLine();
        source.OpenBlock($"bool {type}::operator==(const {type}& other_) const");
        source.WriteLine("if (discriminator_ != other_.discriminator_) return false;");
        foreach (var branch in declaration.Branches)
        {
            var leftStorage = branch.Name == "other" ? "this->other_" : $"{branch.Name}_";
            source.WriteLine($"if ({BranchCondition(branch, declaration, "discriminator_")})");
            source.WriteLine($"    return {leftStorage} == other_.{branch.Name}_;");
        }

        source.WriteLine("return true;");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"bool {type}::operator!=(const {type}& other_) const");
        source.WriteLine("return !(*this == other_);");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"void {type}::swap({type}& other_) noexcept");
        source.WriteLine("using ::std::swap;");
        source.WriteLine("swap(discriminator_, other_.discriminator_);");
        foreach (var branch in declaration.Branches)
        {
            var leftStorage = branch.Name == "other" ? "this->other_" : $"{branch.Name}_";
            source.WriteLine($"if ({BranchCondition(branch, declaration, "discriminator_")})");
            source.WriteLine("{");
            source.WriteLine($"    swap({leftStorage}, other_.{branch.Name}_);");
            source.WriteLine("}");
        }

        source.WriteLine("if (discriminator_ != other_.discriminator_)");
        source.WriteLine("{");
        foreach (var branch in declaration.Branches)
        {
            var leftStorage = branch.Name == "other" ? "this->other_" : $"{branch.Name}_";
            source.WriteLine($"    if ({BranchCondition(branch, declaration, "other_.discriminator_")})");
            source.WriteLine("    {");
            source.WriteLine($"        swap({leftStorage}, other_.{branch.Name}_);");
            source.WriteLine("    }");
        }

        source.WriteLine("}");

        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"void swap({type}& left, {type}& right)");
        source.WriteLine("left.swap(right);");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"::std::ostream& operator<<(::std::ostream& output, const {type}& value)");
        source.WriteLine($"output << \"{type}{{\";");
        source.WriteLine("bool emitted_branch = false;");
        foreach (var branch in declaration.Branches)
        {
            source.WriteLine($"if ({BranchCondition(branch, declaration, "value.discriminator_")})");
            source.WriteLines("""
                {
                    if (emitted_branch)
                        output << ", ";
                """);
            source.WriteLine($"    output << \"{branch.Name}: \" << value.{branch.Name}_;");
            source.WriteLine("    emitted_branch = true;");
            source.WriteLine("}");
        }

        source.WriteLine("return output << '}';");
        source.CloseBlock();
    }

    private static void EmitUnionBranchAccessors(
        SourceWriter source,
        UnionPlan declaration,
        UnionBranchPlan branch,
        bool isRti612)
    {
        var typeName = CppTypeNameFormatter.MakeRelative(
            branch.Type.StorageType, declaration.NamespaceSegments);
        var condition = BranchCondition(branch, declaration, "discriminator_");
        source.WriteLine($"{typeName}& {declaration.Name}::{branch.Name}()");
        source.OpenBrace();
        source.WriteLine($"if (!({condition}))");
        source.WriteLine($"    throw ::dds::core::PreconditionNotMetError(\"{branch.Name} branch is inactive\");");
        source.WriteLine($"return {branch.Name}_;");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"const {typeName}& {declaration.Name}::{branch.Name}() const");
        source.WriteLine($"if (!({condition}))");
        source.WriteLine($"    throw ::dds::core::PreconditionNotMetError(\"{branch.Name} branch is inactive\");");
        source.WriteLine($"return {branch.Name}_;");
        source.CloseBlock();

        if (branch.IsDefault)
        {
            EmitUnionBranchSetter(source, declaration, branch, isRti612, includeDiscriminator: false);
            if (branch.Type is not (MemberTypePlan.Primitive or MemberTypePlan.Enumeration))
            {
                EmitUnionBranchSetter(source, declaration, branch, isRti612, useRvalue: true, includeDiscriminator: false);
            }

            EmitUnionBranchSetter(source, declaration, branch, isRti612, includeDiscriminator: true);
            if (branch.Type is not (MemberTypePlan.Primitive or MemberTypePlan.Enumeration))
            {
                EmitUnionBranchSetter(source, declaration, branch, isRti612, useRvalue: true, includeDiscriminator: true);
            }

            return;
        }

        EmitUnionBranchSetter(source, declaration, branch, isRti612);
        if (branch.LabelExpressions.Count > 1)
        {
            EmitUnionBranchSetter(source, declaration, branch, isRti612, includeDiscriminator: true);
        }

        if (branch.Type is not (MemberTypePlan.Primitive or MemberTypePlan.Enumeration))
        {
            EmitUnionBranchSetter(source, declaration, branch, isRti612, useRvalue: true);
            if (branch.LabelExpressions.Count > 1)
            {
                EmitUnionBranchSetter(source, declaration, branch, isRti612, useRvalue: true, includeDiscriminator: true);
            }
        }
    }

    private static void EmitUnionBranchSetter(
        SourceWriter source,
        UnionPlan declaration,
        UnionBranchPlan branch,
        bool isRti612,
        bool useRvalue = false,
        bool includeDiscriminator = false)
    {
        var parameterType = CppTypeNameFormatter.MakeRelative(
            branch.SetterParameterType(useRvalue, isRti612), declaration.NamespaceSegments);
        var discriminatorType = CppTypeNameFormatter.MakeRelative(
            declaration.DiscriminatorType, declaration.NamespaceSegments);
        var discriminatorParameter = includeDiscriminator ? $", {discriminatorType} discriminator" : string.Empty;
        source.OpenBlock($"void {declaration.Name}::{branch.Name}({parameterType} value{discriminatorParameter})");
        if (branch.IsDefault && includeDiscriminator)
        {
            var validDiscriminator = BranchCondition(branch, declaration, "discriminator");
            source.WriteLine($"if (!({validDiscriminator}))");
            source.WriteLine("    throw ::dds::core::PreconditionNotMetError(");
            source.WriteLine($"        \"invalid discriminator for {branch.Name}\");");
        }
        else if (!branch.IsDefault && includeDiscriminator && branch.LabelExpressions.Count > 1)
        {
            var validDiscriminator = BranchCondition(branch, declaration, "discriminator");
            source.WriteLine($"if (!({validDiscriminator}))");
            source.WriteLine("    throw ::dds::core::PreconditionNotMetError(");
            source.WriteLine($"        \"invalid discriminator for {branch.Name}\");");
        }
        else if (branch.IsDefault)
        {
            var isActive = BranchCondition(branch, declaration, "discriminator_");
            source.WriteLine($"if (!({isActive}))");
            source.WriteLine($"    throw ::dds::core::PreconditionNotMetError(\"{branch.Name} branch is inactive\");");
        }

        var assignedValue = useRvalue ? "::std::move(value)" : "value";
        source.WriteLine($"{branch.Name}_ = {assignedValue};");
        if (includeDiscriminator)
        {
            source.WriteLine("discriminator_ = discriminator;");
        }
        else if (!branch.IsDefault)
        {
            source.WriteLine($"discriminator_ = {CppTypeNameFormatter.MakeRelative(branch.LabelExpressions.First(), declaration.NamespaceSegments)};");
        }

        source.CloseBlock();
        source.BlankLine();
    }

    private static string BranchCondition(UnionBranchPlan branch, UnionPlan declaration, string discriminator)
    {
        if (branch.IsDefault)
        {
            var labels = declaration.Branches.Where(candidate => !candidate.IsDefault)
                .SelectMany(candidate => candidate.LabelExpressions).Distinct(StringComparer.Ordinal).ToArray();
            return labels.Length == 0
                ? "true"
                : "(" + string.Join(" && ", labels.Select(label =>
                    $"{discriminator} != {CppTypeNameFormatter.MakeRelative(label, declaration.NamespaceSegments)}")) + ")";
        }

        return "(" + string.Join(" || ", branch.LabelExpressions.Select(label =>
            $"{discriminator} == {CppTypeNameFormatter.MakeRelative(label, declaration.NamespaceSegments)}")) + ")";
    }
}
