using Microsoft.CodeAnalysis.CSharp;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits documented C# representations of IDL constants.</summary>
internal static class ConstantEmitter
{
    /// <summary>Emits the generated document for an IDL constant.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(CompilationContext compilation, IdlConstantDeclaration declaration, string sourceIdlFileName)
    {
        var name = declaration.Name;
        var currentNamespace = declaration.Namespace;

        var effectiveIdlType = ResolveEffectiveConstantType(compilation, declaration);
        string type;
        if (declaration.Value is IdlScalarValue.Enumeration { TypeQualifiedName: var enumTypeQualifiedName })
        {
            type = IdlNaming.ResolvedTypeReference(enumTypeQualifiedName, currentNamespace);
        }
        else
        {
            type = MapConstantType(effectiveIdlType);
        }

        var expression = declaration.Value switch
        {
            IdlScalarValue.Enumeration enumeration => $"{IdlNaming.ResolvedTypeReference(enumeration.TypeQualifiedName, currentNamespace)}.{IdlNaming.EscapeIdentifier(enumeration.MemberName)}",
            IdlScalarValue.Text text when !IsSimpleConstantReference(declaration.Expression) => SymbolDisplay.FormatLiteral(text.Value, quote: true),
            IdlScalarValue.Floating floating => CSharpScalarValueFormatter.FormatFloating(type, floating.Value),
            IdlScalarValue.Integral integer => CSharpScalarValueFormatter.Format(type, integer.Value),
            IdlScalarValue.Boolean boolean => boolean.Value ? "true" : "false",
            IdlScalarValue.Character character when !IsSimpleConstantReference(declaration.Expression) => CSharpScalarValueFormatter.Format("char", character.Value),
            _ => FormatConstantReference(compilation, declaration)
        };

        var writer = EmissionSupport.CreateSource(currentNamespace, [], sourceIdlFileName);

        var valueDescription = declaration.Type == "long" ? "integer" : declaration.Type;

        writer.WriteXmlSummary($"Provides the <c>{name}</c> IDL constant.");
        writer.OpenBlock($"public static class {IdlNaming.EscapeIdentifier(name)}");
        writer.WriteXmlSummary($"Gets the {valueDescription} value of the <c>{name}</c> IDL constant.");
        writer.WriteLine($"public const {type} Value = {expression};");
        writer.CloseBlock();

        var generatedName = IdlNaming.CreateGeneratedName(currentNamespace, name);
        return [new GeneratedIdlSource(generatedName.HintName, writer.ToString())];
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

    private static string ResolveEffectiveConstantType(CompilationContext compilation, IdlConstantDeclaration declaration)
    {
        var resolvedType = compilation.ResolveType(declaration.Type, declaration.Namespace);
        while (resolvedType is IdlType.Alias alias)
        {
            resolvedType = alias.Target;
        }

        return resolvedType switch
        {
            IdlType.Primitive primitive => primitive.Name,
            IdlType.StringType { IsWide: true } => "wstring",
            IdlType.StringType => "string",
            _ => declaration.Type
        };
    }

    private static bool IsSimpleConstantReference(string expression) =>
        IdlScopedName.IsValid(expression.Trim());

    private static string FormatConstantReference(CompilationContext compilation, IdlConstantDeclaration declaration)
    {
        var reference = declaration.Expression.Trim();
        if (!compilation.TryResolveConstant(reference, declaration.Namespace, out var qualifiedName))
        {
            throw new InvalidOperationException($"Constant '{declaration.Name}' has no bound value or reference.");
        }

        if (reference.StartsWith("::", StringComparison.Ordinal))
        {
            return $"global::{IdlNaming.EscapeQualifiedIdentifier(qualifiedName)}.Value";
        }

        return $"{IdlNaming.ResolvedTypeReference(qualifiedName, declaration.Namespace)}.Value";
    }
}
