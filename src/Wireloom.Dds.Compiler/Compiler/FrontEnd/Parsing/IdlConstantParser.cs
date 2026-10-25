using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL constant declarations and validates their values.</summary>
internal sealed class IdlConstantParser
{
    private readonly IdlParseContext context;

    internal IdlConstantParser(IdlParseContext context)
        => this.context = context;

    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var constant = ConstantPattern.Match(declarations[position..]);
        if (!constant.Success)
        {
            return false;
        }

        var name = constant.Groups["name"].Value;
        var qualified = context.Qualify(name, currentNamespace);
        context.RecordIdentifier(input, baseOffset + position + constant.Groups["name"].Index, name, currentNamespace);
        context.EnsureNewName(input, baseOffset + position, qualified, isType: false);

        var type = NormalizeIdlType(constant.Groups["type"].Value);
        var typeWasEscaped = context.IsEscapedIdentifier(baseOffset + position + constant.Groups["type"].Index);
        var expressionGroup = constant.Groups["expression"];
        var expression = expressionGroup.Value.Trim();
        var expressionOffset = baseOffset + position + expressionGroup.Index
            + (expressionGroup.Value.Length - expressionGroup.Value.TrimStart().Length);
        var expressionWasEscaped = context.IsEscapedIdentifier(expressionOffset);
        var declarationOffset = context.MapOffset(baseOffset + position);
        context.DeferGeneratedDeclarationNameValidation(input, declarationOffset, name, ["Value"]);
        var declaration = new IdlConstantDeclaration(name, type, expression, currentNamespace, Path.GetFileName(input.Path), value: null, input, declarationOffset, typeWasEscaped, expressionWasEscaped);

        context.Symbols.AddConstant(qualified, declaration);
        context.Declarations.Add(declaration);

        position += constant.Length;

        return true;
    }

}
