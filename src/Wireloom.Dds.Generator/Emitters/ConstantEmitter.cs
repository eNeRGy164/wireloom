namespace Wireloom;

using System.Numerics;
using static IdlCompiler;

/// <summary>Emits documented C# representations of IDL constants.</summary>
internal static class ConstantEmitter
{
    public static void Emit(CompilationState compilation, IdlConstantDeclaration declaration, string sourceIdlFileName)
    {
        var name = declaration.Name;
        var currentNamespace = declaration.Namespace;
        var type = MapConstantType(declaration.Type);
        var expression = ShouldEmitEvaluatedLiteral(declaration)
            ? FormatIntegerLiteral(declaration.Type, declaration.IntegerValue!.Value)
            : ReplaceConstantReferences(declaration.Expression, currentNamespace, compilation);

        var writer = CreateSource(currentNamespace, ["System"], sourceIdlFileName);

        var valueDescription = declaration.Type == "long" ? "integer" : declaration.Type;

        writer.WriteXmlSummary($"Provides the <c>{name}</c> IDL constant.");
        writer.OpenBlock($"public static class {EscapeIdentifier(name)}");
        writer.WriteXmlSummary($"Gets the {valueDescription} value of the <c>{name}</c> IDL constant.");
        writer.WriteLine($"public const {type} Value = {expression};");
        writer.CloseBlock();

        compilation.AddSource(new(CreateHintName(currentNamespace, name), writer.ToString()));
    }

    private static string MapConstantType(string type) => type switch
    {
        "string" or "wstring" => "string",
        "boolean" => "bool",
        "char" or "wchar" => "char",
        "float" => "float",
        "double" => "double",
        _ => MapPrimitive(type)
    };

    private static string FormatIntegerLiteral(string type, BigInteger value) => type switch
    {
        "long long" or "int64" when value == long.MinValue => "long.MinValue",
        "long long" or "int64" => $"{value}L",
        "unsigned long long" or "uint64" when value == ulong.MaxValue => "ulong.MaxValue",
        "unsigned long long" or "uint64" => $"{value}UL",
        "unsigned long" or "uint32" => $"{value}U",
        _ => value.ToString()
    };

    private static bool ShouldEmitEvaluatedLiteral(IdlConstantDeclaration declaration)
    {
        return (declaration.IntegerValue is not null && declaration.Type is "long long" or "int64" or "unsigned long" or "uint32" or "unsigned long long" or "uint64")
            || declaration.IntegerValue is { } value && value == long.MinValue;
    }

    private static string ReplaceConstantReferences(string expression, string? currentNamespace, CompilationState compilation)
    {
        foreach (var constant in compilation.Constants)
        {
            var qualifiedName = constant.Namespace is null ? constant.Name : $"{constant.Namespace}.{constant.Name}";
            var replacement = $"{TypeReference(EscapeQualifiedIdentifier(qualifiedName), currentNamespace)}.Value";

            expression = expression.Replace(
                constant.Namespace is null ? constant.Name : $"{constant.Namespace.Replace(".", "::")}::{constant.Name}",
                replacement);
            expression = expression.Replace(qualifiedName, replacement);

            if (constant.Namespace == currentNamespace)
            {
                expression = expression.Replace(constant.Name, replacement);
            }
        }

        return expression;
    }
}
