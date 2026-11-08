using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Captures an annotation application without interpreting its arguments.</summary>
internal sealed class IdlAnnotationApplicationSyntax(string name, string text, int offset, int length)
{
    internal string Name { get; } = name;

    internal string Text { get; } = text;

    internal int Offset { get; } = offset;

    internal int Length { get; } = length;

    /// <summary>Parses argument nodes on demand after the catalog identifies this application as built in.</summary>
    internal IReadOnlyList<IdlAnnotationArgumentSyntax> ParseArguments() => IdlAnnotationCatalog.ParseArguments(this);
}

/// <summary>Retains one positional or named annotation argument and its span within the application.</summary>
internal sealed class IdlAnnotationArgumentSyntax(string? name, string expression, int relativeOffset)
{
    internal string? Name { get; } = name;

    internal string Expression { get; } = expression;

    internal int RelativeOffset { get; } = relativeOffset;

}

/// <summary>Reads delimited IDL syntax without treating quoted literal contents as syntax.</summary>
internal static class IdlSyntaxReader
{
    /// <summary>Reads consecutive annotation applications, retaining their source order and offsets.</summary>
    internal static IReadOnlyList<IdlAnnotationApplicationSyntax> ReadAnnotationApplications(string text, int offset = 0)
    {
        var applications = new List<IdlAnnotationApplicationSyntax>();
        var position = 0;
        while (position < text.Length)
        {
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }

            var start = position;

            if (!TryReadAnnotationApplication(text[position..], out var name, out var length))
            {
                break;
            }

            position += length;
            applications.Add(new IdlAnnotationApplicationSyntax(name, text[start..position], offset + start, position - start));
        }

        return applications;
    }

    /// <summary>Reads a complete annotation name and optional balanced argument list without interpreting its meaning.</summary>
    internal static bool TryReadAnnotationApplication(string text, out string name, out int length)
    {
        name = string.Empty;
        length = 0;
        if (text.Length == 0 || text[0] != '@')
        {
            return false;
        }

        var position = 1;
        while (position < text.Length && (char.IsLetterOrDigit(text[position]) || text[position] is '_' or ':'))
        {
            position++;
        }

        name = text[1..position];
        if (!IdlScopedName.IsValid(name))
        {
            return false;
        }

        while (position < text.Length && char.IsWhiteSpace(text[position]))
        {
            position++;
        }

        if (position < text.Length && text[position] == '(')
        {
            var closing = FindClosingDelimiter(text, position, '(', ')');
            if (closing < 0)
            {
                return false;
            }

            position = closing + 1;
            while (position < text.Length && char.IsWhiteSpace(text[position]))
            {
                position++;
            }
        }

        length = position;
        return true;
    }

    /// <summary>Finds a matching delimiter while preserving quoted literals and nested delimiters.</summary>
    internal static int FindClosingDelimiter(string text, int openingOffset, char opening, char closing)
    {
        var depth = 0;
        for (var index = openingOffset; index < text.Length; index++)
        {
            if (IsLiteralStart(text, index))
            {
                var quote = text[index++];
                while (index < text.Length)
                {
                    var character = text[index++];
                    if (character == '\\' && index < text.Length)
                    {
                        index++;
                    }
                    else if (character == quote)
                    {
                        break;
                    }
                }

                index--;
            }
            else if (text[index] == opening)
            {
                depth++;
            }
            else if (text[index] == closing && --depth == 0)
            {
                return index;
            }
        }

        return -1;
    }

    /// <summary>Splits text at separators outside nested delimiters and quoted literals.</summary>
    internal static IReadOnlyList<(int Start, int Length)> SplitTopLevel(string text, char separator)
    {
        var slices = new List<(int Start, int Length)>();
        var expectedClosings = new Stack<char>();
        var start = 0;
        for (var index = 0; index < text.Length; index++)
        {
            if (IsLiteralStart(text, index))
            {
                var quote = text[index++];
                while (index < text.Length)
                {
                    if (text[index] == '\\')
                    {
                        index++;
                    }
                    else if (text[index] == quote)
                    {
                        break;
                    }

                    index++;
                }

                continue;
            }

            if (text[index] is '(' or '[' or '{')
            {
                expectedClosings.Push(text[index] switch { '(' => ')', '[' => ']', _ => '}' });
            }
            else if (expectedClosings.Count > 0 && text[index] == expectedClosings.Peek())
            {
                expectedClosings.Pop();
            }
            else if (text[index] == separator && expectedClosings.Count == 0)
            {
                slices.Add((start, index - start));
                start = index + 1;
            }
        }

        slices.Add((start, text.Length - start));
        return slices;
    }

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

        var hasPrefix = index > 0 && (text[index - 1] is 'L' or 'u' or 'U'
            || index > 1 && text[index - 1] == '8' && text[index - 2] == 'u');
        var isIdentifierApostrophe = index > 0 && index + 1 < text.Length
            && char.IsLetterOrDigit(text[index - 1]) && char.IsLetterOrDigit(text[index + 1]);
        return hasPrefix || !isIdentifierApostrophe;
    }
}
