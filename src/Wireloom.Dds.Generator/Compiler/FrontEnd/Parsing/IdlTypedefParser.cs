using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL typedef declarations.</summary>
internal sealed class IdlTypedefParser
{
    private readonly IdlParseContext context;

    internal IdlTypedefParser(IdlParseContext context) =>
        this.context = context;

    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var sequenceTypedef = SequenceTypedefPattern.Match(declarations.Substring(position));
        if (sequenceTypedef.Success)
        {
            var sequenceName = sequenceTypedef.Groups[3].Value;
            var qualified = context.Qualify(sequenceName, currentNamespace);
            context.EnsureNewName(input, baseOffset + position, qualified);
            var target = NormalizeIdlType(sequenceTypedef.Groups[1].Value);
            var bound = sequenceTypedef.Groups[2].Success
                ? context.TypeParser.ResolveBound(input, baseOffset + position, sequenceTypedef.Groups[2].Value, currentNamespace)
                : (int?)null;
            var parsedSequence = new IdlTypedef(sequenceName, currentNamespace, "sequence", target, bound);
            context.Symbols.AddTypedef(qualified, parsedSequence);
            context.Declarations.Add(new IdlTypedefDeclaration(parsedSequence, Path.GetFileName(input.Path)));

            position += sequenceTypedef.Length;

            return true;
        }

        var arrayTypedef = ArrayTypedefPattern.Match(declarations.Substring(position));
        if (arrayTypedef.Success)
        {
            var typedefName = arrayTypedef.Groups[2].Value;
            var qualified = context.Qualify(typedefName, currentNamespace);
            context.EnsureNewName(input, baseOffset + position, qualified);
            var dimensions = context.TypeParser.ParseDimensions(input, baseOffset + position, arrayTypedef.Groups[3].Value, currentNamespace);
            var parsedArray = new IdlTypedef(
                typedefName,
                currentNamespace,
                "array",
                NormalizeIdlType(arrayTypedef.Groups[1].Value),
                null,
                dimensions);
            context.Symbols.AddTypedef(qualified, parsedArray);
            context.Declarations.Add(new IdlTypedefDeclaration(parsedArray, Path.GetFileName(input.Path)));

            position += arrayTypedef.Length;

            return true;
        }

        var typedefDeclaration = TypedefPattern.Match(declarations.Substring(position));
        if (!typedefDeclaration.Success)
        {
            return false;
        }

        var name = typedefDeclaration.Groups[2].Value;
        var typeName = context.Qualify(name, currentNamespace);
        context.EnsureNewName(input, baseOffset + position, typeName);
        var typedefTarget = NormalizeIdlType(typedefDeclaration.Groups[1].Value);
        if (typedefTarget.StartsWith("string", StringComparison.Ordinal) ||
            typedefTarget.StartsWith("wstring", StringComparison.Ordinal))
        {
            var isWideString = typedefTarget.StartsWith("wstring", StringComparison.Ordinal);
            var bound = context.TypeParser.ParseStringBound(
                input,
                baseOffset + position,
                typedefTarget,
                currentNamespace,
                "String typedef bound must be a positive Int32.");
            var parsedStringTypedef = new IdlTypedef(
                name,
                currentNamespace,
                isWideString ? "wstring" : "string",
                null,
                null,
                stringBound: bound,
                isWideString: isWideString);
            context.Symbols.AddTypedef(typeName, parsedStringTypedef);
            context.Declarations.Add(new IdlTypedefDeclaration(parsedStringTypedef, Path.GetFileName(input.Path)));

            position += typedefDeclaration.Length;

            return true;
        }

        var parsedTypedef = new IdlTypedef(
            name,
            currentNamespace,
            typedefTarget,
            null,
            null);
        context.Symbols.AddTypedef(typeName, parsedTypedef);
        context.Declarations.Add(new IdlTypedefDeclaration(parsedTypedef, Path.GetFileName(input.Path)));

        position += typedefDeclaration.Length;

        return true;
    }

    internal void Validate(IdlInput input, int offset, string name) =>
        context.Validator.ValidateTypedef(input, offset, name);
}
