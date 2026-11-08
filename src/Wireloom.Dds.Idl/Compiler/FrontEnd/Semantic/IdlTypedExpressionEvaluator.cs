using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Dispatches supported numeric constant expressions through the scalar type model.</summary>
internal static class IdlTypedExpressionEvaluator
{
    /// <summary>Evaluates an expression using the declared IDL scalar type as its semantic context.</summary>
    internal static IdlScalarValue Evaluate(string expression, string expectedType, IdlSymbolTable symbols, string? currentNamespace)
    {
        if (IsIntegralType(expectedType))
        {
            return new IdlScalarValue.Integral(
                IdlConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace, expectedType));
        }

        if (expectedType is "float" or "double")
        {
            return new IdlScalarValue.Floating(
                FloatingConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace));
        }

        throw new FormatException($"Constant expressions of type '{expectedType}' are not supported.");
    }

    private static bool IsIntegralType(string type) => type is
        "short" or "int16" or "unsigned short" or "uint16" or "long" or "int32" or "unsigned long" or "uint32"
        or "long long" or "int64" or "unsigned long long" or "uint64" or "int8" or "uint8" or "octet";
}
