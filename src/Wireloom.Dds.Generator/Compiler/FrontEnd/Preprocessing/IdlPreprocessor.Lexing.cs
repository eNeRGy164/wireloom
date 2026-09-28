using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private static string JoinContinuations(string source, out int[] offsetMap)
    {
        var output = new StringBuilder(source.Length);
        var offsets = new List<int>(source.Length);

        for (var index = 0; index < source.Length; index++)
        {
            if (source[index] == '\\' && index + 1 < source.Length)
            {
                if (source[index + 1] == '\r')
                {
                    index++;

                    if (index + 1 < source.Length && source[index + 1] == '\n')
                    {
                        index++;
                    }

                    continue;
                }

                if (source[index + 1] == '\n')
                {
                    index++;
                    continue;
                }
            }

            output.Append(source[index]);
            offsets.Add(index);
        }

        offsets.Add(source.Length);
        offsetMap = [.. offsets];

        return output.ToString();
    }

    private string RemoveComments(IdlInput input, string source)
    {
        var output = new StringBuilder(source.Length);
        var index = 0;

        while (index < source.Length)
        {
            if (StartsPrefixedLiteral(source, index) || IsLiteralStart(source, index))
            {
                var literalEnd = SkipLiteral(source, index, out var closed);
                if (!closed)
                {
                    throw new IdlException(input, OriginalOffset(index), "Unterminated string literal.");
                }

                output.Append(source, index, literalEnd - index);
                index = literalEnd;

                continue;
            }

            if (source[index] == '/' && index + 1 < source.Length && source[index + 1] == '/')
            {
                output.Append("  ");
                index += 2;

                while (index < source.Length && source[index] is not '\r' and not '\n')
                {
                    output.Append(' ');
                    index++;
                }

                continue;
            }

            if (source[index] == '/' && index + 1 < source.Length && source[index + 1] == '*')
            {
                var commentOffset = index;
                output.Append("  ");
                index += 2;

                while (index < source.Length)
                {
                    if (source[index] == '*' && index + 1 < source.Length && source[index + 1] == '/')
                    {
                        output.Append("  ");
                        index += 2;
                        break;
                    }

                    var character = source[index++];
                    output.Append(character is '\r' or '\n' ? character : ' ');
                }

                if (index >= source.Length
                    && (source.Length < 2 || source[^2] != '*' || source[^1] != '/'))
                {
                    throw new IdlException(input, OriginalOffset(commentOffset), "Unterminated comment.");
                }

                continue;
            }

            output.Append(source[index++]);
        }

        return output.ToString();
    }

    private static void AppendBlankLine(StringBuilder output, int lineLength) => output.Append(' ', lineLength);

    private static bool IsIdentifier(string text) =>
        text.Length != 0 && IsIdentifierStart(text[0]) && (text.Length == 1 || IsIdentifierPart(text[1..]));

    private static bool IsLiteralStart(string text, int index)
    {
        if (text[index] == '"')
        {
            return true;
        }

        if (text[index] != '\'')
        {
            return false;
        }

        return IsEncodingPrefix(text, index) || !LooksLikeAnApostrophe(text, index);
    }

    private static bool StartsPrefixedLiteral(string text, int index)
    {
        if (index + 1 < text.Length
            && text[index] is 'L' or 'u' or 'U'
            && IsLiteralQuote(text[index + 1]))
        {
            return true;
        }

        return index + 2 < text.Length
            && text[index] == 'u'
            && text[index + 1] == '8'
            && IsLiteralQuote(text[index + 2]);
    }

    private static int LiteralQuoteIndex(string text, int index)
    {
        if (!StartsPrefixedLiteral(text, index))
        {
            return index;
        }

        return text[index] == 'u' && index + 1 < text.Length && text[index + 1] == '8'
            ? index + 2
            : index + 1;
    }

    private static bool IsLiteralQuote(char character) => character is '\'' or '"';

    private static bool LooksLikeAnApostrophe(string text, int index) =>
        index > 0
        && index + 1 < text.Length
        && char.IsLetterOrDigit(text[index - 1])
        && char.IsLetterOrDigit(text[index + 1]);

    private static int SkipLiteral(string text, int index)
    {
        return SkipLiteral(text, index, out _);
    }

    private static int SkipLiteral(string text, int index, out bool closed)
    {
        var quoteIndex = LiteralQuoteIndex(text, index);
        var quote = text[quoteIndex];
        index = quoteIndex + 1;
        closed = false;

        while (index < text.Length)
        {
            if (text[index] == '\\' && index + 1 < text.Length)
            {
                index += 2;
                continue;
            }

            if (text[index++] == quote)
            {
                closed = true;
                break;
            }
        }

        return index;
    }

    private static bool IsEncodingPrefix(string text, int index) =>
        index > 0
        && (text[index - 1] is 'L' or 'u' or 'U'
            || (index > 1 && text[index - 2] == 'u' && text[index - 1] == '8'));

    private static bool IsIdentifierStart(char character) => character == '_' || char.IsLetter(character);

    private static bool IsIdentifierPart(string text)
    {
        foreach (var character in text)
        {
            if (!IsIdentifierPart(character))
            {
                return false;
            }
        }

        return true;
    }

    private static bool IsIdentifierPart(char character) =>
        character == '_' || char.IsLetterOrDigit(character);

}
