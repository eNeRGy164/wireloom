using System.Globalization;
using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Evaluates arithmetic expressions used by floating-point IDL defaults.</summary>
internal sealed class FloatingConstantExpressionEvaluator(string text, IdlSymbolTable symbols, string? currentNamespace, HashSet<string> activeConstants)
{
    private int position;

    /// <summary>Evaluates a floating-point arithmetic expression.</summary>
    internal static double Evaluate(string expression, IdlSymbolTable symbols, string? currentNamespace) =>
        Evaluate(expression, symbols, currentNamespace, new(StringComparer.Ordinal));

    private static double Evaluate(string expression, IdlSymbolTable symbols, string? currentNamespace, HashSet<string> activeConstants)
    {
        var evaluator = new FloatingConstantExpressionEvaluator(expression, symbols, currentNamespace, activeConstants);
        var value = evaluator.ParseAdditive();
        evaluator.SkipWhitespace();
        if (evaluator.position != expression.Length)
        {
            throw new FormatException("Unexpected token in floating-point expression.");
        }

        return value;
    }

    private double ParseAdditive()
    {
        var value = ParseMultiplicative();
        while (true)
        {
            if (TryConsume('+'))
            {
                value += ParseMultiplicative();
            }
            else if (TryConsume('-'))
            {
                value -= ParseMultiplicative();
            }
            else
            {
                return value;
            }
        }
    }

    private double ParseMultiplicative()
    {
        var value = ParseUnary();
        while (true)
        {
            if (TryConsume('*'))
            {
                value *= ParseUnary();
            }
            else if (TryConsume('/'))
            {
                var divisor = ParseUnary();
                if (divisor == 0)
                {
                    throw new FormatException("Division by zero in floating-point expression.");
                }

                value /= divisor;
            }
            else
            {
                return value;
            }
        }
    }

    private double ParseUnary()
    {
        if (TryConsume('+'))
        {
            return ParseUnary();
        }

        if (TryConsume('-'))
        {
            return -ParseUnary();
        }

        return ParsePrimary();
    }

    private double ParsePrimary()
    {
        if (TryConsume('('))
        {
            var parenthesizedValue = ParseAdditive();
            if (!TryConsume(')'))
            {
                throw new FormatException("Missing closing parenthesis in floating-point expression.");
            }

            return parenthesizedValue;
        }

        SkipWhitespace();
        var start = position;
        while (position < text.Length && (char.IsDigit(text[position]) || text[position] is '.' or 'e' or 'E'))
        {
            if ((text[position] is 'e' or 'E') && position + 1 < text.Length && (text[position + 1] is '+' or '-'))
            {
                position += 2;
                while (position < text.Length && char.IsDigit(text[position]))
                {
                    position++;
                }

                break;
            }

            position++;
        }

        if (start != position)
        {
            var literal = text[start..position];
            if (!literal.Contains('.') && !literal.Contains('e') && !literal.Contains('E'))
            {
                throw new FormatException("Integer literals cannot be used in floating-point constant expressions.");
            }

            if (double.TryParse(literal, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
            {
                return value;
            }

            throw new FormatException("Invalid floating-point literal.");
        }

        position = start;
        while (position < text.Length && (char.IsLetterOrDigit(text[position]) || text[position] is '_' or ':' or '.'))
        {
            position++;
        }

        if (start == position)
        {
            throw new FormatException("Expected a floating-point literal or constant name.");
        }

        var token = text[start..position];
        if (!symbols.TryResolveConstant(token, currentNamespace, out var constant)
            || !IsFloatingConstant(constant, symbols))
        {
            throw new FormatException($"Unknown floating-point constant: {token}");
        }

        if (constant.Value is IdlScalarValue.Floating floating)
        {
            return floating.Value;
        }

        var qualifiedName = constant.Namespace is null ? constant.Name : $"{constant.Namespace}.{constant.Name}";
        if (!activeConstants.Add(qualifiedName))
        {
            throw new FormatException($"Cyclic floating-point constant: {token}");
        }

        try
        {
            var resolvedValue = Evaluate(constant.Expression, symbols, constant.Namespace, activeConstants);
            return IsFloatConstant(constant, symbols) ? (float)resolvedValue : resolvedValue;
        }
        finally
        {
            activeConstants.Remove(qualifiedName);
        }
    }

    private static bool IsFloatingConstant(IdlConstantDeclaration declaration, IdlSymbolTable symbols)
    {
        var resolved = new IdlTypeResolver(symbols).Resolve(
            declaration.Type,
            declaration.Namespace,
            declaration.SourceInput,
            declaration.SourceOffset);
        while (resolved is IdlType.Alias alias)
        {
            resolved = alias.Target;
        }

        return resolved is IdlType.Primitive { Name: "float" or "double" };
    }

    private static bool IsFloatConstant(IdlConstantDeclaration declaration, IdlSymbolTable symbols)
    {
        var resolved = new IdlTypeResolver(symbols).Resolve(
            declaration.Type,
            declaration.Namespace,
            declaration.SourceInput,
            declaration.SourceOffset);
        while (resolved is IdlType.Alias alias)
        {
            resolved = alias.Target;
        }

        return resolved is IdlType.Primitive { Name: "float" };
    }

    private bool TryConsume(char expected)
    {
        SkipWhitespace();
        if (position >= text.Length || text[position] != expected)
        {
            return false;
        }

        position++;
        return true;
    }

    private void SkipWhitespace()
    {
        while (position < text.Length && char.IsWhiteSpace(text[position]))
        {
            position++;
        }
    }
}
