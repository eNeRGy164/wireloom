using System.Globalization;
using System.Text;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Formats string and character literals without a compiler-platform dependency.</summary>
internal static class CSharpLiteralFormatter
{
    internal static string FormatString(string value) => Format(value, isCharacter: false);

    internal static string FormatCharacter(char value) => Format(value.ToString(), isCharacter: true);

    private static string Format(string value, bool isCharacter)
    {
        var quote = isCharacter ? '\'' : '"';
        var result = new StringBuilder(value.Length + 2);
        result.Append(quote);

        for (var index = 0; index < value.Length; index++)
        {
            var character = value[index];
            switch (character)
            {
                case '\0': result.Append("\\0"); break;
                case '\a': result.Append("\\a"); break;
                case '\b': result.Append("\\b"); break;
                case '\f': result.Append("\\f"); break;
                case '\n': result.Append("\\n"); break;
                case '\r': result.Append("\\r"); break;
                case '\t': result.Append("\\t"); break;
                case '\v': result.Append("\\v"); break;
                case '\\': result.Append("\\\\"); break;
                case '"' when !isCharacter: result.Append("\\\""); break;
                case '\'' when isCharacter: result.Append("\\'"); break;
                default:
                    if (char.IsHighSurrogate(character) && index + 1 < value.Length && char.IsLowSurrogate(value[index + 1]))
                    {
                        result.Append(character).Append(value[++index]);
                    }
                    else if (ShouldEscape(character))
                    {
                        result.Append("\\u").Append(((int)character).ToString("x4", CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        result.Append(character);
                    }
                    break;
            }
        }

        result.Append(quote);
        return result.ToString();
    }

    private static bool ShouldEscape(char character)
    {
        var category = char.GetUnicodeCategory(character);
        return char.IsControl(character)
            || char.IsSurrogate(character)
            || category is UnicodeCategory.LineSeparator or UnicodeCategory.ParagraphSeparator
                or UnicodeCategory.OtherNotAssigned;
    }
}
