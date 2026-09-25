using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Wireloom;

/// <summary>
/// Processes the deterministic preprocessing subset used by IDL inputs.
/// </summary>
internal sealed class IdlPreprocessor
{
    private static readonly Regex CommentPattern = new(
        @"/\*[\s\S]*?\*/|//[^\r\n]*",
        RegexOptions.Compiled);

    private static readonly Regex DirectivePattern = new(
        @"^\s*#\s*(?<name>[A-Za-z_]\w*)\b(?<rest>.*)$",
        RegexOptions.Compiled);

    private static readonly Regex DefinePattern = new(
        @"^(?<name>[A-Za-z_]\w*)(?<function>\((?<parameters>[^)]*)\))?(?:[ \t]+(?<body>.*))?$",
        RegexOptions.Compiled);

    private static readonly Regex IncludePattern = new(
        @"^(?:""(?<quoted>[^""]+)""|<(?<angle>[^>]+)>)\s*$",
        RegexOptions.Compiled);

    private static readonly Regex DefinedPattern = new(
        @"\bdefined\s*(?:\(\s*(?<name>[A-Za-z_]\w*)\s*\)|(?<name2>[A-Za-z_]\w*))",
        RegexOptions.Compiled);

    private static readonly Regex StringifyPattern = new(
        @"(?<!#)#(?!#)\s*(?<name>[A-Za-z_]\w*)",
        RegexOptions.Compiled);

    private static readonly Regex TokenPastePattern = new(
        @"(?<left>[A-Za-z_]\w*)\s*##\s*(?<right>[A-Za-z_]\w*)",
        RegexOptions.Compiled);

    private readonly Dictionary<string, Macro> macros = new(StringComparer.Ordinal);
    private IdlInput? currentInput;

    public IdlPreprocessor(IEnumerable<string> defines, IEnumerable<string> undefines)
    {
        foreach (var define in defines)
        {
            var separator = define.IndexOf('=');
            var name = (separator < 0 ? define : define[..separator]).Trim();
            var value = separator < 0 ? "1" : define[(separator + 1)..];

            if (IsIdentifier(name))
            {
                macros[name] = new Macro(null, value, false);
            }
        }

        foreach (var undefine in undefines)
        {
            var name = undefine.Trim();
            if (IsIdentifier(name))
            {
                macros.Remove(name);
            }
        }
    }

    public string Process(IdlInput input, Action<string, bool, int> include)
    {
        currentInput = input;

        var source = CommentPattern.Replace(input.Text, match => new string(' ', match.Length));
        source = JoinContinuations(source);

        var output = new StringBuilder(source.Length);
        var conditionals = new Stack<ConditionalFrame>();
        var active = true;
        var offset = 0;

        while (offset < source.Length)
        {
            var lineEnd = source.IndexOf('\n', offset);
            if (lineEnd < 0)
            {
                lineEnd = source.Length;
            }

            var contentEnd = lineEnd;
            if (contentEnd > offset && source[contentEnd - 1] == '\r')
            {
                contentEnd--;
            }

            var line = source.Substring(offset, contentEnd - offset);
            var directive = DirectivePattern.Match(line);
            if (directive.Success)
            {
                ProcessDirective(directive, input, offset, include, conditionals, ref active, output, line.Length);
            }
            else if (active)
            {
                output.Append(Expand(line));
            }
            else
            {
                output.Append(new string(' ', line.Length));
            }

            if (lineEnd < source.Length)
            {
                output.Append('\n');
                offset = lineEnd + 1;
            }
            else
            {
                offset = source.Length;
            }
        }

        if (conditionals.Count != 0)
        {
            throw new IdlException(input, input.Text.Length, "Unterminated preprocessor conditional.");
        }

        return output.ToString();
    }

    private void ProcessDirective(
        Match directive,
        IdlInput input,
        int offset,
        Action<string, bool, int> include,
        Stack<ConditionalFrame> conditionals,
        ref bool active,
        StringBuilder output,
        int lineLength)
    {
        var name = directive.Groups["name"].Value;
        var rest = directive.Groups["rest"].Value.Trim();

        switch (name)
        {
            case "if":
                PushConditional(EvaluateCondition(rest), conditionals, ref active);
                AppendBlankLine(output, lineLength);
                return;

            case "ifdef":
                PushConditional(macros.ContainsKey(rest), conditionals, ref active);
                AppendBlankLine(output, lineLength);
                return;

            case "ifndef":
                PushConditional(!macros.ContainsKey(rest), conditionals, ref active);
                AppendBlankLine(output, lineLength);
                return;

            case "elif":
                if (conditionals.Count == 0)
                {
                    throw new IdlException(input, offset, "Unexpected #elif.");
                }

                var elifFrame = conditionals.Pop();
                var elifActive = elifFrame.ParentActive && !elifFrame.BranchTaken && EvaluateCondition(rest);
                conditionals.Push(new ConditionalFrame(elifFrame.ParentActive, elifFrame.BranchTaken || elifActive));
                active = elifActive;

                AppendBlankLine(output, lineLength);

                return;

            case "else":
                if (conditionals.Count == 0)
                {
                    throw new IdlException(input, offset, "Unexpected #else.");
                }

                var elseFrame = conditionals.Pop();
                var elseActive = elseFrame.ParentActive && !elseFrame.BranchTaken;
                conditionals.Push(new ConditionalFrame(elseFrame.ParentActive, true));
                active = elseActive;

                AppendBlankLine(output, lineLength);

                return;

            case "endif":
                if (conditionals.Count == 0)
                {
                    throw new IdlException(input, offset, "Unexpected #endif.");
                }

                var endifFrame = conditionals.Pop();
                active = endifFrame.ParentActive;

                AppendBlankLine(output, lineLength);

                return;

            case "define":
                if (active)
                {
                    Define(input, offset, rest);
                }

                AppendBlankLine(output, lineLength);

                return;

            case "undef":
                if (active)
                {
                    if (!IsIdentifier(rest))
                    {
                        throw new IdlException(input, offset, "Malformed #undef directive.");
                    }

                    macros.Remove(rest);
                }

                AppendBlankLine(output, lineLength);

                return;

            case "include":
                if (active)
                {
                    var match = IncludePattern.Match(rest);
                    if (!match.Success)
                    {
                        throw new IdlException(input, offset, "Malformed #include directive; expected a quoted or angle-bracket filename, for example #include \"Common.idl\".");
                    }

                    include(
                        match.Groups["quoted"].Success ? match.Groups["quoted"].Value : match.Groups["angle"].Value,
                        match.Groups["angle"].Success,
                        offset);
                }

                AppendBlankLine(output, lineLength);

                return;

            case "error":
                if (active)
                {
                    throw new IdlException(input, offset, $"Preprocessor error: {rest}");
                }

                AppendBlankLine(output, lineLength);

                return;

            default:
                if (active)
                {
                    throw new IdlException(input, offset, $"Unsupported preprocessor directive: #{name}.");
                }

                AppendBlankLine(output, lineLength);

                return;
        }
    }

    private static void PushConditional(bool condition, Stack<ConditionalFrame> conditionals, ref bool active)
    {
        var parentActive = active;
        var branchActive = parentActive && condition;
        conditionals.Push(new ConditionalFrame(parentActive, branchActive));
        active = branchActive;
    }

    private void Define(IdlInput input, int offset, string text)
    {
        var match = DefinePattern.Match(text);
        if (!match.Success)
        {
            throw new IdlException(input, offset, "Malformed #define directive.");
        }

        var name = match.Groups["name"].Value;
        var variadic = false;
        var parameters = match.Groups["function"].Success
            ? ParseParameters(input, offset, match.Groups["parameters"].Value, out variadic)
            : null;
        var body = match.Groups["body"].Success ? match.Groups["body"].Value : string.Empty;
        macros[name] = new Macro(parameters, body, parameters is not null && variadic);
    }

    private static List<string> ParseParameters(IdlInput input, int offset, string text, out bool variadic)
    {
        variadic = false;

        if (string.IsNullOrWhiteSpace(text))
        {
            return [];
        }

        var parameters = text.Split(',');
        if (parameters.Length > 0 && parameters[^1].Trim() == "...")
        {
            variadic = true;
            parameters = [.. parameters.Take(parameters.Length - 1)];
        }

        var result = new List<string>();
        foreach (var parameter in parameters)
        {
            var name = parameter.Trim();
            if (!IsIdentifier(name))
            {
                throw new IdlException(input, offset, "Malformed #define parameter list.");
            }

            result.Add(name);
        }

        if (variadic)
        {
            result.Add("__VA_ARGS__");
        }

        return result;
    }

    private string Expand(string text) => Expand(text, new HashSet<string>(StringComparer.Ordinal), 0);

    private string Expand(string text, HashSet<string> expanding, int depth)
    {
        if (depth > 64 || text.Length == 0)
        {
            return text;
        }

        var output = new StringBuilder(text.Length);
        var index = 0;

        while (index < text.Length)
        {
            if (text[index] is '\'' or '"')
            {
                var quote = text[index++];
                var closed = false;
                output.Append(quote);

                while (index < text.Length)
                {
                    var character = text[index++];
                    output.Append(character);

                    if (character == '\\' && index < text.Length)
                    {
                        output.Append(text[index++]);
                    }
                    else if (character == quote)
                    {
                        closed = true;
                        break;
                    }
                }

                if (!closed)
                {
                    throw new IdlException(currentInput!, 0, "Unterminated string literal.");
                }

                continue;
            }

            if (!IsIdentifierStart(text[index]))
            {
                output.Append(text[index++]);
                continue;
            }

            var tokenStart = index++;
            while (index < text.Length && IsIdentifierPart(text[index]))
            {
                index++;
            }

            var name = text[tokenStart..index];
            if (!macros.TryGetValue(name, out var macro))
            {
                output.Append(name);
                continue;
            }

            if (expanding.Contains(name))
            {
                throw new IdlException(currentInput!, 0, $"Recursive macro expansion detected: {name}");
            }

            if (macro.Parameters is not null)
            {
                var open = index;

                while (open < text.Length && char.IsWhiteSpace(text[open]))
                {
                    open++;
                }

                if (open >= text.Length
                    || text[open] != '('
                    || !TryReadArguments(text, open, out var arguments, out var end)
                    || (!macro.Variadic && arguments.Count != macro.Parameters.Count)
                    || (macro.Variadic && arguments.Count < macro.Parameters.Count - 1))
                {
                    output.Append(name);
                    continue;
                }

                expanding.Add(name);
                try
                {
                    output.Append(ExpandFunctionMacro(macro, arguments, expanding, depth));
                    index = end;
                }
                finally
                {
                    expanding.Remove(name);
                }

                continue;
            }

            expanding.Add(name);

            try
            {
                output.Append(Expand(macro.Body, expanding, depth + 1));
            }
            finally
            {
                expanding.Remove(name);
            }
        }

        return output.ToString();
    }

    private string ExpandFunctionMacro(Macro macro, List<string> arguments, HashSet<string> expanding, int depth)
    {
        var substitutions = new Dictionary<string, (string Raw, string Expanded)>(StringComparer.Ordinal);
        var fixedCount = macro.Variadic ? macro.Parameters!.Count - 1 : macro.Parameters!.Count;

        for (var index = 0; index < fixedCount; index++)
        {
            var raw = arguments[index].Trim();
            substitutions[macro.Parameters[index]] = (raw, Expand(raw, expanding, depth + 1));
        }

        if (macro.Variadic)
        {
            var raw = string.Join(", ", arguments.Skip(fixedCount).Select(argument => argument.Trim()));
            substitutions["__VA_ARGS__"] = (raw, Expand(raw, expanding, depth + 1));
        }

        var replacement = StringifyPattern.Replace(macro.Body, match =>
            substitutions.TryGetValue(match.Groups["name"].Value, out var substitution)
                ? Stringify(substitution.Raw)
                : match.Value);

        while (TokenPastePattern.IsMatch(replacement))
        {
            replacement = TokenPastePattern.Replace(replacement, match =>
            {
                var left = substitutions.TryGetValue(match.Groups["left"].Value, out var leftValue)
                    ? leftValue.Raw
                    : match.Groups["left"].Value;
                var right = substitutions.TryGetValue(match.Groups["right"].Value, out var rightValue)
                    ? rightValue.Raw
                    : match.Groups["right"].Value;
                return left + right;
            });
        }

        foreach (var substitution in substitutions)
        {
            replacement = Regex.Replace(replacement, $"\\b{Regex.Escape(substitution.Key)}\\b", _ => substitution.Value.Expanded);
        }

        return Expand(replacement, expanding, depth + 1);
    }

    private static string Stringify(string text) =>
        "\"" + text.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";

    private static bool TryReadArguments(string text, int open, out List<string> arguments, out int end)
    {
        arguments = [];
        end = open;
        var depth = 0;
        var start = open + 1;

        for (var index = open; index < text.Length; index++)
        {
            switch (text[index])
            {
                case '(':
                    depth++;
                    break;

                case ')':
                    depth--;
                    if (depth == 0)
                    {
                        var argument = text.Substring(start, index - start).Trim();
                        if (argument.Length != 0 || arguments.Count != 0)
                        {
                            arguments.Add(argument);
                        }

                        end = index + 1;
                        return true;
                    }

                    break;

                case ',' when depth == 1:
                    arguments.Add(text.Substring(start, index - start).Trim());
                    start = index + 1;
                    break;
            }
        }

        return false;
    }

    private bool EvaluateCondition(string text)
    {
        text = DefinedPattern.Replace(text, match =>
            macros.ContainsKey(match.Groups["name"].Success ? match.Groups["name"].Value : match.Groups["name2"].Value)
                ? "1"
                : "0");

        return EvaluateTruth(Expand(text).Trim());
    }

    private static bool EvaluateTruth(string text)
    {
        text = TrimOuterParentheses(text);

        var or = FindOperator(text, "||");
        if (or >= 0)
        {
            return EvaluateTruth(text[..or]) || EvaluateTruth(text[(or + 2)..]);
        }

        var and = FindOperator(text, "&&");
        if (and >= 0)
        {
            return EvaluateTruth(text[..and]) && EvaluateTruth(text[(and + 2)..]);
        }

        if (text.StartsWith("!", StringComparison.Ordinal))
        {
            return !EvaluateTruth(text[1..]);
        }

        foreach (var comparison in new[] { "==", "!=", ">=", "<=", ">", "<" })
        {
            var index = FindOperator(text, comparison);
            if (index < 0)
            {
                continue;
            }

            var left = text[..index].Trim();
            var right = text[(index + comparison.Length)..].Trim();
            var equal = string.Equals(left, right, StringComparison.Ordinal)
                || (TryParseInteger(left, out var leftValue) && TryParseInteger(right, out var rightValue) && leftValue == rightValue);

            return comparison switch
            {
                "==" => equal,
                "!=" => !equal,
                ">=" => TryParseInteger(left, out leftValue) && TryParseInteger(right, out rightValue) && leftValue >= rightValue,
                "<=" => TryParseInteger(left, out leftValue) && TryParseInteger(right, out rightValue) && leftValue <= rightValue,
                ">" => TryParseInteger(left, out leftValue) && TryParseInteger(right, out rightValue) && leftValue > rightValue,
                "<" => TryParseInteger(left, out leftValue) && TryParseInteger(right, out rightValue) && leftValue < rightValue,
                _ => false
            };
        }

        return TryParseInteger(text, out var value) ? value != 0 : text.Length != 0 && text != "0";
    }

    private static int FindOperator(string text, string operatorText)
    {
        var depth = 0;

        for (var index = 0; index <= text.Length - operatorText.Length; index++)
        {
            if (text[index] == '(')
            {
                depth++;
            }
            else if (text[index] == ')')
            {
                depth--;
            }
            else if (depth == 0 && text.AsSpan(index, operatorText.Length).SequenceEqual(operatorText))
            {
                return index;
            }
        }

        return -1;
    }

    private static string TrimOuterParentheses(string text)
    {
        while (text.Length >= 2 && text[0] == '(' && text[^1] == ')' && IsBalanced(text[1..^1]))
        {
            text = text[1..^1].Trim();
        }

        return text;
    }

    private static bool IsBalanced(string text)
    {
        var depth = 0;
        foreach (var character in text)
        {
            if (character == '(')
            {
                depth++;
            }
            else if (character == ')' && --depth < 0)
            {
                return false;
            }
        }

        return depth == 0;
    }

    private static bool TryParseInteger(string text, out long value)
    {
        text = text.Trim();

        if (text.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            return long.TryParse(text[2..], NumberStyles.HexNumber, null, out value);
        }

        return long.TryParse(text, out value);
    }

    private static string JoinContinuations(string source)
    {
        var output = new StringBuilder(source.Length);

        for (var index = 0; index < source.Length; index++)
        {
            if (source[index] == '\\' && index + 1 < source.Length)
            {
                if (source[index + 1] == '\r')
                {
                    output.Append(' ');
                    index++;

                    if (index + 1 < source.Length && source[index + 1] == '\n')
                    {
                        index++;
                    }

                    continue;
                }

                if (source[index + 1] == '\n')
                {
                    output.Append(' ');
                    index++;
                    continue;
                }
            }

            output.Append(source[index]);
        }

        return output.ToString();
    }

    private static void AppendBlankLine(StringBuilder output, int lineLength) => output.Append(' ', lineLength);

    private static bool IsIdentifier(string text) =>
        text.Length != 0 && IsIdentifierStart(text[0]) && (text.Length == 1 || IsIdentifierPart(text[1..]));

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

    private sealed class Macro(List<string>? parameters, string body, bool variadic)
    {
        public List<string>? Parameters { get; } = parameters;
        public string Body { get; } = body;
        public bool Variadic { get; } = variadic;
    }

    private sealed class ConditionalFrame(bool parentActive, bool branchTaken)
    {
        public bool ParentActive { get; } = parentActive;
        public bool BranchTaken { get; } = branchTaken;
    }
}
