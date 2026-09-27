using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

internal sealed partial class IdlDeclarationParser
{
    private bool TryParseUnion(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var unionDeclaration = UnionPattern.Match(declarations.Substring(position));
        if (!unionDeclaration.Success)
        {
            return false;
        }

        var unionName = unionDeclaration.Groups["name"].Value;
        var qualified = Qualify(unionName, currentNamespace);
        EnsureNewName(input, baseOffset + position, qualified);
        var discriminatorIdlType = NormalizeIdlType(unionDeclaration.Groups["discriminator"].Value);
        var discriminatorQualified = ResolveTypeName(discriminatorIdlType, currentNamespace);
        var discriminatorIsEnum = symbols.TryGetEnum(discriminatorQualified, out var discriminatorEnum);
        var unionBody = unionDeclaration.Groups["body"];
        var branches = new List<IdlUnionBranch>();
        var branchNames = new HashSet<string>(StringComparer.Ordinal);
        var unionOffset = 0;

        while (unionOffset < unionBody.Length)
        {
            if (char.IsWhiteSpace(unionBody.Value[unionOffset]))
            {
                unionOffset++;
                continue;
            }

            var (Branch, Length) = ParseUnionBranch(
                input,
                unionBody.Value,
                unionOffset,
                baseOffset + position + unionBody.Index,
                currentNamespace,
                discriminatorIdlType,
                discriminatorQualified,
                discriminatorIsEnum,
                discriminatorEnum,
                branchNames);
            branches.Add(Branch);

            unionOffset += Length;
        }

        if (branches.Count == 0 || branches.Count(branch => branch.IsDefault) > 1)
        {
            throw new IdlException(input, baseOffset + position, "The union must contain at least one branch and at most one default branch.");
        }

        var parsedUnion = new IdlUnion(
            unionName,
            currentNamespace,
            discriminatorIdlType,
            discriminatorIsEnum,
            branches,
            ParseExtensibility(unionDeclaration.Groups["extensibility"].Value));
        symbols.AddUnion(qualified, parsedUnion);
        declarationQueue.Add(new IdlUnionDeclaration(parsedUnion, Path.GetFileName(input.Path)));

        position += unionDeclaration.Length;

        return true;
    }

    private static IdlExtensibilityKind ParseExtensibility(string annotation) => annotation.Trim() switch
    {
        "@final" => IdlExtensibilityKind.Final,
        "@mutable" => IdlExtensibilityKind.Mutable,
        _ => IdlExtensibilityKind.Extensible
    };

    private (IdlUnionBranch Branch, int Length) ParseUnionBranch(
        IdlInput input,
        string body,
        int offset,
        int sourceOffset,
        string? currentNamespace,
        string discriminatorIdlType,
        string discriminatorQualified,
        bool discriminatorIsEnum,
        IdlEnum? discriminatorEnum,
        HashSet<string> branchNames)
    {
        var branch = UnionBranchPattern.Match(body.Substring(offset));
        if (!branch.Success)
        {
            throw new IdlException(input, sourceOffset + offset, "Unsupported union branch declaration.");
        }

        var branchName = branch.Groups["name"].Value;
        if (!branchNames.Add(branchName))
        {
            throw new IdlException(input, sourceOffset + offset, $"Duplicate union branch: {branchName}");
        }

        var branchType = NormalizeIdlType(branch.Groups["type"].Value);
        var field = ParseUnionBranchField(input, branchName, branchType, sourceOffset + offset, currentNamespace);
        var labels = new List<string>();
        var labelValues = new List<int>();

        if (branch.Groups[1].Success)
        {
            foreach (Match match in Regex.Matches(branch.Groups[1].Value, @"case\s+(-?[0-9]+|'(?:\\.|[^'])'|[A-Za-z_]\w*)"))
            {
                var label = match.Groups[1].Value;
                if (discriminatorIdlType == "char" && TryParseCharacterLabel(label, out var characterLabel))
                {
                    labels.Add(label);
                    labelValues.Add(characterLabel);
                }
                else if (discriminatorIdlType == "boolean" && label is "TRUE" or "FALSE")
                {
                    var booleanLabel = label == "TRUE";
                    labels.Add(booleanLabel ? "true" : "false");
                    labelValues.Add(booleanLabel ? 1 : 0);
                }
                else if (int.TryParse(label, out var numericLabel))
                {
                    labels.Add(numericLabel.ToString());
                    labelValues.Add(numericLabel);
                }
                else if (discriminatorIsEnum && discriminatorEnum!.Members.Any(member => member.Name == label))
                {
                    labels.Add($"{EscapeQualifiedIdentifier(discriminatorQualified)}.{EscapeIdentifier(label)}");
                    labelValues.Add(discriminatorEnum.Members.Single(member => member.Name == label).Value);
                }
                else
                {
                    throw new IdlException(input, sourceOffset + offset, $"Unknown union discriminator label: {label}");
                }
            }
        }

        return (new IdlUnionBranch(field, labels, labelValues, branch.Groups[2].Success), branch.Length);
    }

    private static bool TryParseCharacterLabel(string label, out int value)
    {
        value = 0;
        if (label.Length < 3 || label[0] != '\'' || label[^1] != '\'')
        {
            return false;
        }

        var content = label[1..^1];
        var character = content switch
        {
            "\\n" => '\n',
            "\\r" => '\r',
            "\\t" => '\t',
            "\\\\" => '\\',
            "\\'" => '\'',
            _ when content.Length == 1 => content[0],
            _ => '\0'
        };

        if (content.Length != 1 && character == '\0')
        {
            return false;
        }

        value = character;

        return true;
    }

    private IdlMember ParseUnionBranchField(IdlInput input, string branchName, string branchType, int sourceOffset, string? currentNamespace)
    {
        if (branchType.StartsWith("sequence", StringComparison.Ordinal))
        {
            var (ElementType, Bound) = ParseSequenceType(input, sourceOffset, branchType, currentNamespace);

            var element = ResolveFieldType(ElementType, currentNamespace, input, sourceOffset);
            if (element is null)
            {
                throw new IdlException(input, sourceOffset, $"Unknown union collection element type: {ElementType}");
            }

            return new IdlMember(branchName, new IdlType.Sequence(element, Bound ?? 100));
        }

        if (branchType.StartsWith("string", StringComparison.Ordinal) ||
            branchType.StartsWith("wstring", StringComparison.Ordinal))
        {
            var bound = ParseStringBound(input, sourceOffset, branchType, currentNamespace, "Union string bound must be a positive Int32.");

            return new IdlMember(branchName, new IdlType.StringType(branchType.StartsWith("wstring", StringComparison.Ordinal), bound));
        }

        if (IsPrimitive(branchType))
        {
            return new IdlMember(branchName, new IdlType.Primitive(NormalizeIdlType(branchType)));
        }

        var resolved = ResolveFieldType(branchType, currentNamespace, input, sourceOffset);
        if (resolved is null)
        {
            throw new IdlException(input, sourceOffset, $"Unknown union branch type: {branchType}");
        }

        return new IdlMember(branchName, resolved);
    }

}
