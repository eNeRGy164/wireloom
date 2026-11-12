using System.Text;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Owns deterministic layout primitives for generated C++ source.</summary>
internal sealed class SourceWriter(string rtiVersion = "7.7.0")
{
    private const string IndentText = "    ";
    private readonly StringBuilder output = new();
    private int indentation;

    /// <summary>Gets the RTI documentation version for this generated source.</summary>
    internal string RtiVersion { get; } = rtiVersion;

    /// <summary>Writes a line at the current indentation, terminated by LF.</summary>
    /// <param name="text">The text to write, or null for an empty line.</param>
    public void WriteLine(string? text = null)
    {
        if (!string.IsNullOrEmpty(text))
        {
            output.Append(string.Concat(Enumerable.Repeat(IndentText, indentation))).Append(text);
        }

        output.Append('\n');
    }

    public void WriteLines(string text)
    {
        foreach (var line in text.Split('\n'))
        {
            WriteLine(line.TrimEnd('\r'));
        }
    }

    /// <summary>Writes a C++ header include directive.</summary>
    /// <param name="header">The header path without delimiters.</param>
    /// <param name="quoted">Whether to use quotes instead of angle brackets.</param>
    public void WriteInclude(string header, bool quoted = false) =>
        WriteLine(quoted ? $"#include \"{header}\"" : $"#include <{header}>");

    public void BlankLine() => WriteLine();
    public void WriteComment(string text) => WriteLine($"// {text}");

    /// <summary>Writes a Doxygen line with optional indentation relative to the current block.</summary>
    public void WriteDoxygen(string text, int indentationLevel = 0)
    {
        var prefix = new string(' ', indentationLevel * IndentText.Length);
        WriteLine($"{prefix}/// {text}");
    }

    /// <summary>Writes a Doxygen summary with optional additional indentation.</summary>
    public void WriteDoxygenBrief(string text, int indentationLevel = 0) =>
        WriteDoxygen($"@brief {text}", indentationLevel);

    /// <summary>Documents a parameter with optional additional indentation.</summary>
    public void WriteDoxygenParam(string name, string description, int indentationLevel = 0) =>
        WriteDoxygen($"@param {name} {description}", indentationLevel);

    /// <summary>Documents the return value with optional additional indentation.</summary>
    public void WriteDoxygenReturn(string description, int indentationLevel = 0) =>
        WriteDoxygen($"@return {description}", indentationLevel);

    /// <summary>Documents an exception with optional additional indentation.</summary>
    public void WriteDoxygenThrows(string exceptionType, string description, int indentationLevel = 0) =>
        WriteDoxygen($"@throws {exceptionType} {description}", indentationLevel);

    public void WriteDoxygenDetails(string text) => WriteDoxygen($"@details {text}");
    public void WriteDoxygenSummary(string text) => WriteDoxygenBrief(text);

    public void WriteDoxygenWrapped(string command, string text, int indentationLevel = 0)
    {
        if (indentationLevel < 0 || indentationLevel > 116 / IndentText.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(indentationLevel));
        }

        var prefix = new string(' ', indentationLevel * IndentText.Length) + "/// " + command + " ";
        var continuation = new string(' ', indentationLevel * IndentText.Length) + "/// ";
        if (continuation.Length >= 116)
        {
            throw new ArgumentOutOfRangeException(nameof(indentationLevel));
        }

        if (prefix.Length >= 116)
        {
            throw new ArgumentException("The Doxygen command leaves no room for wrapped text.", nameof(command));
        }

        var line = prefix;
        foreach (var word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
        {
            var remaining = word;
            var baseLength = line == prefix ? prefix.Length : continuation.Length;
            if (line.Length > baseLength && line.Length + 1 + remaining.Length > 116)
            {
                output.Append(line).Append('\n');
                line = continuation;
            }

            while (line.Length + (line.Length == (line == prefix ? prefix.Length : continuation.Length) ? 0 : 1)
                + remaining.Length > 116)
            {
                var separatorLength = line.Length == (line == prefix ? prefix.Length : continuation.Length) ? 0 : 1;
                var available = 116 - line.Length - separatorLength;
                if (separatorLength > 0) line += " ";
                var count = Math.Min(available, remaining.Length);
                line += remaining[..count];
                remaining = remaining[count..];
                output.Append(line).Append('\n');
                line = continuation;
            }

            if (line.Length > (line == prefix ? prefix.Length : continuation.Length)) line += " ";
            line += remaining;
        }

        output.Append(line).Append('\n');
    }

    public void WriteDoxygenSee(string description, string url, int indentationLevel = 0)
    {
        var prefix = new string(' ', indentationLevel * IndentText.Length);
        output.Append(prefix)
            .Append("/// @see [")
            .Append(description)
            .Append("](")
            .Append(url)
            .Append(")\n");
    }

    public void WriteDoxygenCode(IEnumerable<string> lines, int indentationLevel = 0)
    {
        var prefix = new string(' ', indentationLevel * IndentText.Length);
        WriteLine($"{prefix}/// @code");
        foreach (var line in lines)
        {
            WriteLine($"{prefix}/// {line}");
        }

        WriteLine($"{prefix}/// @endcode");
    }
    /// <summary>Opens namespace scopes without indenting their contents.</summary>
    /// <param name="segments">Namespace names in outermost-to-innermost order.</param>
    public void OpenNamespaces(params IReadOnlyList<string> segments)
    {
        foreach (var segment in segments)
        {
            WriteLine($"namespace {segment}");
            WriteLine("{");
        }
    }

    /// <summary>Closes namespace scopes from innermost to outermost.</summary>
    /// <param name="segments">Namespace names in outermost-to-innermost order.</param>
    public void CloseNamespaces(params IReadOnlyList<string> segments)
    {
        for (var index = segments.Count - 1; index >= 0; index--)
        {
            WriteLine($"}} // namespace {segments[index]}");
        }
    }

    public void OpenBlock(string declaration)
    {
        WriteLine(declaration);
        OpenBrace();
    }

    public void OpenBrace()
    {
        WriteLine("{");
        indentation++;
    }

    public void CloseBlock(string suffix = "")
    {
        indentation--;
        WriteLine("}" + suffix);
    }
    public override string ToString() => output.ToString();
}
