using System.Text;

namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Provides shared lexical rules and comment/literal scanning for preprocessing.</summary>
internal sealed class PreprocessorLexicalService(Func<int, int> originalOffset, CancellationToken cancellationToken)
{
    /// <summary>Joins continued source lines and returns their offset map.</summary>
    internal static string JoinContinuations(string source, out int[] offsetMap)
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

    /// <summary>Removes comments while preserving source offsets.</summary>
    internal string RemoveComments(IdlInput input, string source)
    {
        var output = new StringBuilder(source.Length);
        var index = 0;

        while (index < source.Length)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (StartsPrefixedLiteral(source, index) || IsLiteralStart(source, index))
            {
                var literalEnd = SkipLiteral(source, index, out var closed);
                if (!closed)
                {
                    throw new IdlException(input, originalOffset(index), "Unterminated string literal.");
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
                var closed = false;

                while (index < source.Length)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    if (source[index] == '*' && index + 1 < source.Length && source[index + 1] == '/')
                    {
                        output.Append("  ");
                        index += 2;
                        closed = true;
                        break;
                    }

                    var character = source[index++];
                    output.Append(character is '\r' or '\n' ? character : ' ');
                }

                if (!closed)
                {
                    throw new IdlException(input, originalOffset(commentOffset), "Unterminated comment.");
                }

                continue;
            }

            output.Append(source[index++]);
        }

        return output.ToString();
    }

    /// <summary>Determines whether text is an identifier.</summary>
    internal static bool IsIdentifier(string text) =>
        text.Length != 0 && IsIdentifierStart(text[0]) && (text.Length == 1 || IsIdentifierPart(text[1..]));

    /// <summary>Determines whether a literal begins at an index.</summary>
    internal static bool IsLiteralStart(string text, int index)
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

    /// <summary>Determines whether a prefixed literal begins at an index.</summary>
    internal static bool StartsPrefixedLiteral(string text, int index)
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

    /// <summary>Gets the quote index for a literal beginning at an index.</summary>
    internal static int LiteralQuoteIndex(string text, int index)
    {
        if (!StartsPrefixedLiteral(text, index))
        {
            return index;
        }

        return text[index] == 'u' && index + 1 < text.Length && text[index + 1] == '8'
            ? index + 2
            : index + 1;
    }

    /// <summary>Skips a literal and returns the first following index.</summary>
    internal static int SkipLiteral(string text, int index) => SkipLiteral(text, index, out _);

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

    /// <summary>Determines whether a character can start an identifier.</summary>
    internal static bool IsIdentifierStart(char character) => character == '_' || char.IsLetter(character);

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

    /// <summary>Determines whether a character can continue an identifier.</summary>
    internal static bool IsIdentifierPart(char character) => character == '_' || char.IsLetterOrDigit(character);

    private static bool IsLiteralQuote(char character) => character is '\'' or '"';

    private static bool LooksLikeAnApostrophe(string text, int index) =>
        index > 0
        && index + 1 < text.Length
        && char.IsLetterOrDigit(text[index - 1])
        && char.IsLetterOrDigit(text[index + 1]);

    private static bool IsEncodingPrefix(string text, int index) =>
        index > 0
        && (text[index - 1] is 'L' or 'u' or 'U'
            || (index > 1 && text[index - 2] == 'u' && text[index - 1] == '8'));
}
