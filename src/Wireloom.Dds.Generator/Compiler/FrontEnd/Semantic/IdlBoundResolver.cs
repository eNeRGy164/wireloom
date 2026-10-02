using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Resolves bounded IDL constructs through integral constant expressions.</summary>
internal static class IdlBoundResolver
{
    /// <summary>Resolves one positive Int32 bound expression.</summary>
    internal static int Resolve(
        IdlInput input,
        int offset,
        string expression,
        string? currentNamespace,
        IdlSymbolTable symbols,
        string diagnosticName)
    {
        BigInteger value;

        try
        {
            value = IdlConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace);
        }
        catch (FormatException)
        {
            throw new IdlException(input, offset, $"{diagnosticName} must resolve to a positive constant: {expression.Trim()}");
        }

        if (value <= 0 || value > int.MaxValue)
        {
            throw new IdlException(input, offset, $"{diagnosticName} must be a positive Int32: {expression.Trim()}");
        }

        return (int)value;
    }
}
