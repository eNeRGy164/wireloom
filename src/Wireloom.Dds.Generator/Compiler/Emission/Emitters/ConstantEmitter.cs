using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits documented C# representations of IDL constants.</summary>
internal static class ConstantEmitter
{
    public static void Emit(CompilationContext compilation, IdlConstantDeclaration declaration, string sourceIdlFileName)
    {
        var name = declaration.Name;
        var currentNamespace = declaration.Namespace;
        var type = MapConstantType(declaration.Type);
        var expression = ShouldEmitEvaluatedLiteral(declaration)
            ? FormatIntegerLiteral(declaration.Type, declaration.IntegerValue!.Value)
            : FormatExpression(ReplaceConstantReferences(declaration.Expression, currentNamespace, compilation), declaration.Type);

        var writer = EmissionSupport.CreateSource(currentNamespace, [], sourceIdlFileName);

        var valueDescription = declaration.Type == "long" ? "integer" : declaration.Type;

        writer.WriteXmlSummary($"Provides the <c>{name}</c> IDL constant.");
        writer.OpenBlock($"public static class {IdlNaming.EscapeIdentifier(name)}");
        writer.WriteXmlSummary($"Gets the {valueDescription} value of the <c>{name}</c> IDL constant.");
        writer.WriteLine($"public const {type} Value = {expression};");
        writer.CloseBlock();

        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(currentNamespace, name), writer.ToString()));
    }

    private static string MapConstantType(string type) => type switch
    {
        "string" or "wstring" => "string",
        "boolean" => "bool",
        "char" or "wchar" => "char",
        "float" => "float",
        "double" => "double",
        _ => IdlNaming.MapPrimitive(type)
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

    private static string ReplaceConstantReferences(string expression, string? currentNamespace, CompilationContext compilation)
    {
        const string tokenPattern = "(?<![A-Za-z0-9_])(?<name>(?:::)?[A-Za-z_][A-Za-z0-9_]*(?:::[A-Za-z_][A-Za-z0-9_]*)*)(?![A-Za-z0-9_])";
        var result = new System.Text.StringBuilder(expression.Length);
        var codeStart = 0;

        for (var index = 0; index < expression.Length; index++)
        {
            if (expression[index] is not ('\'' or '"'))
            {
                continue;
            }

            result.Append(ReplaceConstantReferencesInCode(expression[codeStart..index], currentNamespace, compilation, tokenPattern));

            var literalEnd = FindLiteralEnd(expression, index);
            result.Append(expression[index..literalEnd]);
            index = literalEnd - 1;
            codeStart = literalEnd;
        }

        result.Append(ReplaceConstantReferencesInCode(expression[codeStart..], currentNamespace, compilation, tokenPattern));

        return result.ToString();
    }

    private static string FormatExpression(string expression, string type)
    {
        var syntax = SyntaxFactory.ParseExpression(expression);
        if (syntax.ContainsDiagnostics)
        {
            return expression;
        }

        var simplified = new ConstantLiteralRewriter(type)
            .Visit(new RedundantParenthesesRewriter().Visit(syntax));
        return simplified.NormalizeWhitespace().ToFullString();
    }

    private sealed class ConstantLiteralRewriter(string type) : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitIdentifierName(IdentifierNameSyntax node)
        {
            if (type == "boolean" && node.Identifier.ValueText is "TRUE" or "FALSE")
            {
                var value = node.Identifier.ValueText == "TRUE" ? "true" : "false";
                return node.WithIdentifier(SyntaxFactory.Identifier(node.Identifier.LeadingTrivia, value, node.Identifier.TrailingTrivia));
            }

            return base.VisitIdentifierName(node)!;
        }

        public override SyntaxNode VisitLiteralExpression(LiteralExpressionSyntax node)
        {
            if ((type is "float" or "double") && node.IsKind(SyntaxKind.NumericLiteralExpression))
            {
                var text = node.Token.Text;
                if ((text.IndexOf('.') >= 0 || text.IndexOf('e') >= 0 || text.IndexOf('E') >= 0)
                    && !text.EndsWith("f", StringComparison.Ordinal)
                    && !text.EndsWith("F", StringComparison.Ordinal)
                    && !text.EndsWith("d", StringComparison.Ordinal)
                    && !text.EndsWith("D", StringComparison.Ordinal))
                {
                    var suffix = type == "float" ? "F" : "D";
                    var token = SyntaxFactory.ParseToken(text + suffix)
                        .WithLeadingTrivia(node.Token.LeadingTrivia)
                        .WithTrailingTrivia(node.Token.TrailingTrivia);
                    return node.WithToken(token);
                }
            }

            return base.VisitLiteralExpression(node)!;
        }
    }

    private sealed class RedundantParenthesesRewriter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitParenthesizedExpression(ParenthesizedExpressionSyntax node)
        {
            var visited = (ParenthesizedExpressionSyntax)base.VisitParenthesizedExpression(node)!;
            if (CanRemoveParentheses(node, visited.Expression))
            {
                return visited.Expression.WithTriviaFrom(visited);
            }

            return visited;
        }

        private static bool CanRemoveParentheses(ParenthesizedExpressionSyntax original, ExpressionSyntax expression)
        {
            if (original.Parent is null or ParenthesizedExpressionSyntax)
            {
                return true;
            }

            if (expression is not BinaryExpressionSyntax child)
            {
                return true;
            }

            if (original.Parent is not BinaryExpressionSyntax parent)
            {
                return false;
            }

            var childPrecedence = GetPrecedence(child.Kind());
            var parentPrecedence = GetPrecedence(parent.Kind());
            if (childPrecedence > parentPrecedence)
            {
                return true;
            }

            return childPrecedence == parentPrecedence && parent.Left == original;
        }

        private static int GetPrecedence(SyntaxKind kind) => kind switch
        {
            SyntaxKind.LogicalOrExpression => 1,
            SyntaxKind.LogicalAndExpression => 2,
            SyntaxKind.BitwiseOrExpression => 3,
            SyntaxKind.ExclusiveOrExpression => 4,
            SyntaxKind.BitwiseAndExpression => 5,
            SyntaxKind.EqualsExpression or SyntaxKind.NotEqualsExpression => 6,
            SyntaxKind.LessThanExpression or SyntaxKind.LessThanOrEqualExpression
                or SyntaxKind.GreaterThanExpression or SyntaxKind.GreaterThanOrEqualExpression => 7,
            SyntaxKind.LeftShiftExpression or SyntaxKind.RightShiftExpression => 8,
            SyntaxKind.AddExpression or SyntaxKind.SubtractExpression => 9,
            SyntaxKind.MultiplyExpression or SyntaxKind.DivideExpression or SyntaxKind.ModuloExpression => 10,
            _ => 0
        };
    }

    private static string ReplaceConstantReferencesInCode(
        string expression,
        string? currentNamespace,
        CompilationContext compilation,
        string pattern) =>
        Regex.Replace(expression, pattern, match =>
        {
            var reference = match.Groups["name"].Value;
            if (!compilation.TryResolveConstant(reference, currentNamespace, out var qualifiedName))
            {
                return reference;
            }

            var isAbsolute = reference.StartsWith("::", StringComparison.Ordinal);
            return isAbsolute
                ? $"global::{IdlNaming.EscapeQualifiedIdentifier(qualifiedName)}.Value"
                : $"{IdlNaming.TypeReference(IdlNaming.EscapeQualifiedIdentifier(qualifiedName), currentNamespace)}.Value";
        });

    private static int FindLiteralEnd(string expression, int literalStart)
    {
        var quote = expression[literalStart];
        for (var index = literalStart + 1; index < expression.Length; index++)
        {
            if (expression[index] == '\\')
            {
                index++;
                continue;
            }

            if (expression[index] == quote)
            {
                return index + 1;
            }
        }

        return expression.Length;
    }
}
