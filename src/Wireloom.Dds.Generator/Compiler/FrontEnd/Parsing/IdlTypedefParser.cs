using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

internal sealed partial class IdlDeclarationParser
{
    private bool TryParseTypedef(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var sequenceTypedef = SequenceTypedefPattern.Match(declarations.Substring(position));
        if (sequenceTypedef.Success)
        {
            var sequenceName = sequenceTypedef.Groups[3].Value;
            var qualified = Qualify(sequenceName, currentNamespace);
            EnsureNewName(input, baseOffset + position, qualified);
            var target = NormalizeIdlType(sequenceTypedef.Groups[1].Value);
            var bound = sequenceTypedef.Groups[2].Success
                ? ResolveBound(input, baseOffset + position, sequenceTypedef.Groups[2].Value, currentNamespace)
                : (int?)null;
            var parsedSequence = new IdlTypedef(sequenceName, currentNamespace, "sequence", target, bound);
            symbols.AddTypedef(qualified, parsedSequence);
            declarationQueue.Add(new IdlTypedefDeclaration(parsedSequence, Path.GetFileName(input.Path)));

            position += sequenceTypedef.Length;

            return true;
        }

        var arrayTypedef = ArrayTypedefPattern.Match(declarations.Substring(position));
        if (arrayTypedef.Success)
        {
            var typedefName = arrayTypedef.Groups[2].Value;
            var qualified = Qualify(typedefName, currentNamespace);
            EnsureNewName(input, baseOffset + position, qualified);
            var dimensions = ParseDimensions(input, baseOffset + position, arrayTypedef.Groups[3].Value, currentNamespace);
            var parsedArray = new IdlTypedef(
                typedefName,
                currentNamespace,
                "array",
                NormalizeIdlType(arrayTypedef.Groups[1].Value),
                null,
                dimensions);
            symbols.AddTypedef(qualified, parsedArray);
            declarationQueue.Add(new IdlTypedefDeclaration(parsedArray, Path.GetFileName(input.Path)));

            position += arrayTypedef.Length;

            return true;
        }

        var typedefDeclaration = TypedefPattern.Match(declarations.Substring(position));
        if (!typedefDeclaration.Success)
        {
            return false;
        }

        var name = typedefDeclaration.Groups[2].Value;
        var typeName = Qualify(name, currentNamespace);
        EnsureNewName(input, baseOffset + position, typeName);
        var typedefTarget = NormalizeIdlType(typedefDeclaration.Groups[1].Value);
        if (typedefTarget.StartsWith("string", StringComparison.Ordinal) ||
            typedefTarget.StartsWith("wstring", StringComparison.Ordinal))
        {
            var isWideString = typedefTarget.StartsWith("wstring", StringComparison.Ordinal);
            var bound = ParseStringBound(
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
            symbols.AddTypedef(typeName, parsedStringTypedef);
            declarationQueue.Add(new IdlTypedefDeclaration(parsedStringTypedef, Path.GetFileName(input.Path)));

            position += typedefDeclaration.Length;

            return true;
        }

        var parsedTypedef = new IdlTypedef(
            name,
            currentNamespace,
            typedefTarget,
            null,
            null);
        symbols.AddTypedef(typeName, parsedTypedef);
        declarationQueue.Add(new IdlTypedefDeclaration(parsedTypedef, Path.GetFileName(input.Path)));

        position += typedefDeclaration.Length;

        return true;
    }

    private void ValidateTypedef(IdlInput input, int offset, string name) =>
        validator.ValidateTypedef(input, offset, name);
}
