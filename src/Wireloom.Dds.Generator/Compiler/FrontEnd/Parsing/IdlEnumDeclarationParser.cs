using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL enum declarations.</summary>
internal sealed class IdlEnumDeclarationParser
{
    private readonly IdlParseContext context;

    internal IdlEnumDeclarationParser(IdlParseContext context) =>
        this.context = context;

    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var enumDeclaration = EnumPattern.Match(declarations.Substring(position));
        if (!enumDeclaration.Success)
        {
            return false;
        }

        var enumName = enumDeclaration.Groups["name"].Value;
        var qualified = context.Qualify(enumName, currentNamespace);
        context.EnsureNewName(input, baseOffset + position, qualified);
        var enumMembers = new List<IdlEnumMember>();
        var hasDefaultLiteral = false;
        var nextValue = 0L;
        var enumBody = enumDeclaration.Groups["body"];
        var rawMembers = enumBody.Value.Split(',');
        var memberOffset = 0;

        foreach (var rawMember in rawMembers)
        {
            var trimmedMember = rawMember.Trim();
            var leadingWhitespace = rawMember.Length - rawMember.TrimStart().Length;
            var sourceOffset = baseOffset + position + enumBody.Index + memberOffset + leadingWhitespace;
            memberOffset += rawMember.Length + 1;

            var member = EnumMemberPattern.Match(trimmedMember);
            if (!member.Success)
            {
                throw new IdlException(input, sourceOffset, "Malformed enum member; expected an identifier, an optional prefix @value(<signed decimal>), or an explicit signed decimal value.");
            }

            if (member.Groups["value"].Success && member.Groups["explicit"].Success)
            {
                throw new IdlException(input, sourceOffset, "Combined @value and explicit enum values are unsupported.");
            }

            var value = ParseEnumValue(input, sourceOffset, member, nextValue);
            var isDefaultLiteral = member.Groups["defaultLiteral"].Success;
            if (isDefaultLiteral && hasDefaultLiteral)
            {
                throw new IdlException(input, sourceOffset, "An enum may contain at most one @default_literal.");
            }

            hasDefaultLiteral |= isDefaultLiteral;
            var hasExplicitValue = member.Groups["value"].Success || member.Groups["explicit"].Success;
            enumMembers.Add(new IdlEnumMember(member.Groups["name"].Value, value, hasExplicitValue, isDefaultLiteral));

            nextValue = value + 1;
        }

        var parsedEnum = new IdlEnum(enumName, currentNamespace, enumMembers, IdlParseContext.ParseExtensibility(enumDeclaration.Groups["extensibility"].Value));
        context.Symbols.AddEnum(qualified, parsedEnum);
        context.Declarations.Add(new IdlEnumDeclaration(parsedEnum, Path.GetFileName(input.Path)));

        position += enumDeclaration.Length;

        return true;
    }

    private static int ParseEnumValue(IdlInput input, int sourceOffset, Match member, long nextValue)
    {
        if (member.Groups["explicit"].Success || member.Groups["value"].Success)
        {
            var valueText = member.Groups["explicit"].Success ? member.Groups["explicit"].Value : member.Groups["value"].Value;
            if (!int.TryParse(valueText, out var value))
            {
                throw new IdlException(input, sourceOffset, "Enum value must be a signed Int32 decimal.");
            }

            return value;
        }

        if (nextValue is < int.MinValue or > int.MaxValue)
        {
            throw new IdlException(input, sourceOffset, "Implicit enum value exceeds the Int32 range.");
        }

        return (int)nextValue;
    }
}
