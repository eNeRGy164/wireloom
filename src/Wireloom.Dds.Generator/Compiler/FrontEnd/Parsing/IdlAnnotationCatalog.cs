using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>IDL syntax target categories used by the built-in annotation catalog.</summary>
[Flags]
internal enum IdlAnnotationTarget
{
    Declaration = 1,
    Typedef = 2,
    Member = 4,
    EnumLiteral = 8,
    UnionBranch = 16
}

/// <summary>Value shape expected for a built-in annotation parameter.</summary>
internal enum IdlAnnotationParameterKind
{
    Boolean,
    IntegerExpression,
    NumericExpression,
    ConstantExpression,
    StringLiteral,
    Identifier,
    Enumeration
}

/// <summary>Policy for repeated uses of one built-in annotation.</summary>
internal enum IdlAnnotationRepeatPolicy
{
    Allow,
    Reject
}

/// <summary>Parameter metadata for one built-in annotation definition.</summary>
internal sealed class IdlAnnotationParameter(
    string Name,
    IdlAnnotationParameterKind Kind,
    string? DefaultExpression = null,
    IReadOnlyList<string>? AllowedValues = null)
{
    internal string Name { get; } = Name;
    internal IdlAnnotationParameterKind Kind { get; } = Kind;
    internal string? DefaultExpression { get; } = DefaultExpression;
    internal IReadOnlyList<string>? AllowedValues { get; } = AllowedValues;
}

/// <summary>Declarative syntax and usage metadata for one built-in annotation.</summary>
internal sealed class IdlAnnotationDefinition(
    string Name,
    IdlAnnotationTarget Targets,
    IReadOnlyList<IdlAnnotationParameter> Parameters,
    bool AllowsPositionalArguments,
    IdlAnnotationRepeatPolicy RepeatPolicy,
    IReadOnlyList<string>? ConflictsWith = null,
    bool IsSupported = true)
{
    internal string Name { get; } = Name;
    internal IdlAnnotationTarget Targets { get; } = Targets;
    internal IReadOnlyList<IdlAnnotationParameter> Parameters { get; } = Parameters;
    internal bool AllowsPositionalArguments { get; } = AllowsPositionalArguments;
    internal IdlAnnotationRepeatPolicy RepeatPolicy { get; } = RepeatPolicy;
    internal IReadOnlyList<string>? ConflictsWith { get; } = ConflictsWith;
    internal bool IsSupported { get; } = IsSupported;
}

/// <summary>Defines the built-in annotation names whose applications Wireloom interprets.</summary>
internal static class IdlAnnotationCatalog
{
    private static readonly IReadOnlyDictionary<string, string> DuplicateMessages = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["autoid"] = "Duplicate @autoid annotation.",
        ["default"] = "Duplicate @default annotation.",
        ["default_literal"] = "Malformed or duplicate @default_literal annotation.",
        ["value"] = "Duplicate @value annotation.",
        ["key"] = "Duplicate @key annotation.",
        ["optional"] = "Duplicate @optional annotation.",
        ["id"] = "Duplicate or invalid @id annotation.",
        ["hashid"] = "Duplicate or conflicting @id/@hashid annotation.",
        ["min"] = "Duplicate @min or @range annotation.",
        ["max"] = "Duplicate @max or @range annotation.",
        ["range"] = "Duplicate @min, @max, or @range annotation.",
        ["external"] = "Duplicate @external annotation.",
        ["must_understand"] = "Duplicate @must_understand annotation.",
        ["unit"] = "Duplicate @unit annotation.",
        ["extensibility"] = "Duplicate extensibility annotation.",
        ["final"] = "Duplicate extensibility annotation.",
        ["appendable"] = "Duplicate extensibility annotation.",
        ["mutable"] = "Duplicate extensibility annotation."
    };

    private static readonly HashSet<string> RangeAnnotationNames = ["min", "max", "range"];

    private static readonly IdlAnnotationParameter BooleanValue = new("value", IdlAnnotationParameterKind.Boolean);
    private static readonly IdlAnnotationParameter IntegralValue = new("value", IdlAnnotationParameterKind.IntegerExpression);
    private static readonly IdlAnnotationParameter ExtensibilityValue = new(
        "value", IdlAnnotationParameterKind.Enumeration, AllowedValues: ["FINAL", "APPENDABLE", "MUTABLE"]);
    private static readonly IdlAnnotationParameter AutoIdValue = new(
        "value", IdlAnnotationParameterKind.Enumeration, "HASH", ["HASH", "SEQUENTIAL"]);

    private static readonly IReadOnlyDictionary<string, IdlAnnotationDefinition> Definitions = CreateDefinitions();

    /// <summary>Returns whether the exact annotation name belongs to the built-in catalog.</summary>
    internal static bool IsKnown(string name) => TryGetDefinition(name, out _);

    /// <summary>Returns whether the name is recognized but intentionally unsupported by Wireloom.</summary>
    internal static bool IsRecognizedUnsupported(string name) =>
        TryGetDefinition(name, out var definition) && !definition.IsSupported;

    /// <summary>Gets the built-in definition for a name, ignoring IDL name casing.</summary>
    private static bool TryGetDefinition(string name, out IdlAnnotationDefinition definition) =>
        Definitions.TryGetValue(name, out definition!);

    /// <summary>Rejects a known annotation application when its target is outside the catalogued set.</summary>
    internal static void ValidateTarget(IdlAnnotationApplicationSyntax application, IdlAnnotationTarget target)
    {
        if (TryGetDefinition(application.Name, out var definition)
            && (definition.Targets & target) == 0)
        {
            throw new FormatException($"Annotation '@{application.Name}' is not supported on this target.");
        }
    }

    /// <summary>Finds the first repeated or conflicting catalogued application for one target.</summary>
    internal static (IdlAnnotationApplicationSyntax Application, string Message)? FindCompositionViolation(
        IReadOnlyList<IdlAnnotationApplicationSyntax> applications,
        IdlAnnotationTarget target)
    {
        var seen = new Dictionary<string, IdlAnnotationApplicationSyntax>(StringComparer.OrdinalIgnoreCase);
        foreach (var application in applications)
        {
            if (!TryGetDefinition(application.Name, out var definition)
                || !definition.IsSupported
                || (definition.Targets & target) == 0)
            {
                continue;
            }

            if (seen.ContainsKey(definition.Name))
            {
                if (definition.RepeatPolicy == IdlAnnotationRepeatPolicy.Reject)
                {
                    return (application, DuplicateMessage(definition.Name));
                }
            }
            else
            {
                foreach (var conflict in definition.ConflictsWith ?? [])
                {
                    if (seen.ContainsKey(conflict))
                    {
                        return (application, ConflictMessage(definition.Name, conflict));
                    }
                }

                seen.Add(definition.Name, application);
            }
        }

        return null;
    }

    private static IReadOnlyDictionary<string, IdlAnnotationDefinition> CreateDefinitions()
    {
        var definitions = new[]
        {
            Definition("allowed_data_representation", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.Identifier)], positional: true),
            Definition("autoid", IdlAnnotationTarget.Declaration, [AutoIdValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("data_representation", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.Identifier)], positional: true),
            Definition("default", IdlAnnotationTarget.Typedef | IdlAnnotationTarget.Member, [new("value", IdlAnnotationParameterKind.ConstantExpression)], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("default_literal", IdlAnnotationTarget.EnumLiteral, [], repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("default_nested", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.Boolean, "true")], positional: true),
            Definition("extensibility", IdlAnnotationTarget.Declaration, [ExtensibilityValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["final", "appendable", "mutable"]),
            Definition("external", IdlAnnotationTarget.Member, [new("value", IdlAnnotationParameterKind.Boolean, "true")], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("hashid", IdlAnnotationTarget.Member, [new("value", IdlAnnotationParameterKind.StringLiteral, "")], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["id"]),
            Definition("id", IdlAnnotationTarget.Member, [IntegralValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["hashid"]),
            Definition("key", IdlAnnotationTarget.Member, [new("value", IdlAnnotationParameterKind.Boolean, "true")], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("language_binding", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.Identifier)], positional: true),
            Definition("max", IdlAnnotationTarget.Member, [IntegralValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["range"]),
            Definition("min", IdlAnnotationTarget.Member, [IntegralValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["range"]),
            Definition("must_understand", IdlAnnotationTarget.Member, [new("value", IdlAnnotationParameterKind.Boolean, "true")], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("nested", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.Boolean, "true")], positional: true),
            Definition("non_serialized", IdlAnnotationTarget.Member, [], repeat: IdlAnnotationRepeatPolicy.Reject, supported: false),
            Definition("optional", IdlAnnotationTarget.Member | IdlAnnotationTarget.UnionBranch, [new("value", IdlAnnotationParameterKind.Boolean, "true")], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("position", IdlAnnotationTarget.Member, [IntegralValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, supported: false),
            Definition("bit_bound", IdlAnnotationTarget.Declaration, [IntegralValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject, supported: false),
            Definition("range", IdlAnnotationTarget.Member, [new("min", IdlAnnotationParameterKind.NumericExpression), new("max", IdlAnnotationParameterKind.NumericExpression)], repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["min", "max"]),
            Definition("resolve_name", IdlAnnotationTarget.Member, [BooleanValue], positional: true),
            Definition("service", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.StringLiteral)], positional: true),
            Definition("topic", IdlAnnotationTarget.Declaration, []),
            Definition("transfer_mode", IdlAnnotationTarget.Declaration, [new("value", IdlAnnotationParameterKind.Identifier)], positional: true),
            Definition("unit", IdlAnnotationTarget.Member, [new("value", IdlAnnotationParameterKind.StringLiteral)], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("value", IdlAnnotationTarget.EnumLiteral, [IntegralValue], positional: true, repeat: IdlAnnotationRepeatPolicy.Reject),
            Definition("final", IdlAnnotationTarget.Declaration, [], repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["extensibility", "appendable", "mutable"]),
            Definition("appendable", IdlAnnotationTarget.Declaration, [], repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["extensibility", "final", "mutable"]),
            Definition("mutable", IdlAnnotationTarget.Declaration, [], repeat: IdlAnnotationRepeatPolicy.Reject, conflictsWith: ["extensibility", "final", "appendable"])
        };

        return definitions.ToDictionary(definition => definition.Name, StringComparer.OrdinalIgnoreCase);
    }

    private static IdlAnnotationDefinition Definition(
        string name,
        IdlAnnotationTarget targets,
        IReadOnlyList<IdlAnnotationParameter> parameters,
        bool positional = false,
        IdlAnnotationRepeatPolicy repeat = IdlAnnotationRepeatPolicy.Allow,
        IReadOnlyList<string>? conflictsWith = null,
        bool supported = true) =>
        new(name, targets, parameters, positional, repeat, conflictsWith, supported);

    /// <summary>Parses positional and named argument syntax after a known annotation is selected.</summary>
    internal static IReadOnlyList<IdlAnnotationArgumentSyntax> ParseArguments(IdlAnnotationApplicationSyntax application)
    {
        var opening = application.Text.IndexOf('(');
        if (opening < 0)
        {
            return [];
        }

        var closing = IdlSyntaxReader.FindClosingDelimiter(application.Text, opening, '(', ')');
        if (closing < 0)
        {
            throw new FormatException($"Malformed arguments in '@{application.Name}'.");
        }

        var argumentsText = application.Text[(opening + 1)..closing];
        var result = new List<IdlAnnotationArgumentSyntax>();
        foreach (var (start, length) in SplitArgumentSlices(argumentsText))
        {
            var part = argumentsText.Substring(start, length);
            var leadingWhitespace = part.Length - part.TrimStart().Length;
            part = part.Trim();
            if (part.Length == 0)
            {
                continue;
            }

            var equals = FindTopLevelEquals(part);
            var name = equals < 0 ? null : part[..equals].Trim();
            var expression = equals < 0 ? part : part[(equals + 1)..].Trim();
            if (name?.Length == 0)
            {
                throw new FormatException($"Annotation '@{application.Name}' has an empty argument name.");
            }

            var expressionLeadingWhitespace = equals < 0
                ? 0
                : part[(equals + 1)..].Length - part[(equals + 1)..].TrimStart().Length;
            var relativeOffset = opening + 1 + start + leadingWhitespace
                + (equals < 0 ? 0 : equals + 1 + expressionLeadingWhitespace);
            result.Add(new IdlAnnotationArgumentSyntax(name, expression, relativeOffset));
        }

        return result;
    }

    /// <summary>Rejects repeated named argument keys in a known annotation application.</summary>
    internal static void ValidateUniqueNamedArguments(IdlAnnotationApplicationSyntax application)
    {
        if (!IsKnown(application.Name))
        {
            return;
        }

        var opening = application.Text.IndexOf('(');
        if (opening < 0)
        {
            ValidateArgumentShape(application);
            return;
        }

        var closing = IdlSyntaxReader.FindClosingDelimiter(application.Text, opening, '(', ')');
        if (closing < 0)
        {
            throw new FormatException($"Malformed arguments in '@{application.Name}'.");
        }

        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var argument in application.ParseArguments())
        {
            if (argument.Name is null)
            {
                continue;
            }

            if (!names.Add(argument.Name))
            {
                throw new FormatException($"Duplicate annotation argument '{argument.Name}' in '@{application.Name}'.");
            }
        }

        ValidateArgumentShape(application);
    }

    private static void ValidateArgumentShape(IdlAnnotationApplicationSyntax application)
    {
        if (!TryGetDefinition(application.Name, out var definition) || !definition.IsSupported)
        {
            return;
        }

        var arguments = application.ParseArguments();
        var namedArguments = arguments.Where(argument => argument.Name is not null).ToArray();
        var positionalArguments = arguments.Where(argument => argument.Name is null).ToArray();
        if (namedArguments.Length > 0 && positionalArguments.Length > 0)
        {
            throw new FormatException($"Annotation '@{application.Name}' cannot mix named and positional arguments.");
        }

        if (positionalArguments.Length > 0 && !definition.AllowsPositionalArguments)
        {
            throw new FormatException($"Annotation '@{application.Name}' requires named arguments.");
        }

        if (positionalArguments.Length > definition.Parameters.Count)
        {
            throw new FormatException($"Annotation '@{application.Name}' accepts at most {definition.Parameters.Count} positional argument(s).");
        }

        foreach (var argument in namedArguments)
        {
            if (!definition.Parameters.Any(parameter => string.Equals(parameter.Name, argument.Name, StringComparison.Ordinal)))
            {
                throw new FormatException($"Unknown argument '{argument.Name}' for annotation '@{application.Name}'.");
            }
        }

        var suppliedNames = namedArguments.Select(argument => argument.Name!).ToHashSet(StringComparer.Ordinal);
        for (var index = 0; index < positionalArguments.Length; index++)
        {
            suppliedNames.Add(definition.Parameters[index].Name);
        }

        var missingParameter = definition.Parameters.FirstOrDefault(parameter =>
            !suppliedNames.Contains(parameter.Name) && parameter.DefaultExpression is null);
        if (missingParameter is not null)
        {
            throw new FormatException($"Annotation '@{application.Name}' requires argument '{missingParameter.Name}'.");
        }

        var positionalIndex = 0;
        foreach (var argument in arguments)
        {
            var parameter = argument.Name is null
                ? definition.Parameters[positionalIndex++]
                : definition.Parameters.First(candidate => candidate.Name == argument.Name);
            ValidateParameterKind(application, parameter, argument.Expression);
            if (parameter.AllowedValues is not null
                && !parameter.AllowedValues.Contains(argument.Expression, StringComparer.OrdinalIgnoreCase))
            {
                throw new FormatException(
                    $"Argument '{parameter.Name}' for annotation '@{application.Name}' must be one of: {string.Join(", ", parameter.AllowedValues)}.");
            }
        }
    }

    private static void ValidateParameterKind(
        IdlAnnotationApplicationSyntax application,
        IdlAnnotationParameter parameter,
        string expression)
    {
        var isValid = parameter.Kind switch
        {
            IdlAnnotationParameterKind.Boolean => IsBooleanLiteral(expression) || IdlScopedName.IsValid(expression),
            IdlAnnotationParameterKind.StringLiteral => IsStringLiteralSequence(expression),
            IdlAnnotationParameterKind.Identifier => IdlScopedName.IsValid(expression),
            _ => true
        };

        if (!isValid)
        {
            throw new FormatException(
                $"Argument '{parameter.Name}' for annotation '@{application.Name}' must be a {parameter.Kind.ToString().ToLowerInvariant()} value.");
        }
    }

    private static bool IsBooleanLiteral(string expression) =>
        string.Equals(expression, "true", StringComparison.OrdinalIgnoreCase)
        || string.Equals(expression, "false", StringComparison.OrdinalIgnoreCase);

    private static bool IsStringLiteralSequence(string expression)
    {
        var position = 0;
        var foundLiteral = false;
        while (position < expression.Length)
        {
            while (position < expression.Length && char.IsWhiteSpace(expression[position])) position++;
            if (position == expression.Length) break;
            if (expression[position] == 'L') position++;
            if (position == expression.Length || expression[position] != '"') return false;

            foundLiteral = true;
            position++;
            while (position < expression.Length && expression[position] != '"')
            {
                if (expression[position] == '\\') position++;
                if (position >= expression.Length) return false;
                position++;
            }

            if (position == expression.Length) return false;
            position++;
        }

        return foundLiteral;
    }

    private static string DuplicateMessage(string name) =>
        DuplicateMessages.TryGetValue(name, out var message)
            ? message
            : $"Duplicate @{name} annotation.";

    private static string ConflictMessage(string name, string conflict)
    {
        if (name == "id" && conflict == "hashid")
        {
            return "Duplicate or invalid @id annotation.";
        }

        if (name == "hashid" && conflict == "id")
        {
            return "Duplicate or conflicting @id/@hashid annotation.";
        }

        if (RangeAnnotationNames.Contains(name) || RangeAnnotationNames.Contains(conflict))
        {
            return "Duplicate @min, @max, or @range annotation.";
        }

        return "Duplicate or conflicting extensibility annotation.";
    }

    private static IEnumerable<(int Start, int Length)> SplitArgumentSlices(string arguments)
    {
        var start = 0;
        for (var index = 0; index < arguments.Length; index++)
        {
            if (arguments[index] is '(' or '[' or '{')
            {
                var closing = arguments[index] switch { '(' => ')', '[' => ']', _ => '}' };
                var match = IdlSyntaxReader.FindClosingDelimiter(arguments, index, arguments[index], closing);
                if (match >= 0)
                {
                    index = match;
                }
            }
            else if (arguments[index] == ',')
            {
                yield return (start, index - start);
                start = index + 1;
            }
            else if (arguments[index] is '\'' or '"')
            {
                var quote = arguments[index];
                while (++index < arguments.Length)
                {
                    if (arguments[index] == '\\')
                    {
                        index++;
                    }
                    else if (arguments[index] == quote)
                    {
                        break;
                    }
                }
            }
        }

        yield return (start, arguments.Length - start);
    }

    private static int FindTopLevelEquals(string argument)
    {
        var nesting = 0;
        var quote = '\0';
        for (var index = 0; index < argument.Length; index++)
        {
            var character = argument[index];
            if (quote != '\0')
            {
                if (character == '\\')
                {
                    index++;
                }
                else if (character == quote)
                {
                    quote = '\0';
                }

                continue;
            }

            if (character is '\'' or '"')
            {
                quote = character;
            }
            else if (character is '(' or '[' or '{')
            {
                nesting++;
            }
            else if (character is ')' or ']' or '}')
            {
                nesting--;
            }
            else if (character == '=' && nesting == 0)
            {
                return index;
            }
        }

        return -1;
    }
}
