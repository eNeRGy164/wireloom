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

    /// <summary>Parses a typedef declaration when one begins at the current position.</summary>
    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var sequenceTypedef = SequenceTypedefPattern.Match(declarations.Substring(position));
        if (sequenceTypedef.Success)
        {
            var sequenceName = sequenceTypedef.Groups[3].Value;
            var qualified = context.Qualify(sequenceName, currentNamespace);
            var declarationOffset = context.MapOffset(baseOffset + position);
            context.EnsureNewName(input, baseOffset + position, qualified);
            context.EnsureGeneratedCompanionNames(input, baseOffset + position, sequenceName, currentNamespace, includeUnmanaged: true);
            context.DeferGeneratedDeclarationNameValidation(input, declarationOffset, sequenceName, GeneratedTypedefMemberNames);
            var target = NormalizeIdlType(sequenceTypedef.Groups[1].Value);
            _ = context.TypeParser.ParseStringBound(input, baseOffset + position, target, currentNamespace, "String sequence element bound must be a positive Int32.");
            var bound = sequenceTypedef.Groups[2].Success
                ? context.TypeParser.ResolveBound(input, baseOffset + position, sequenceTypedef.Groups[2].Value, currentNamespace)
                : null;
            var parsedSequence = new IdlTypedef(sequenceName, currentNamespace, "sequence", target, bound?.Value);
            bound?.AddConsumer(parsedSequence.SetBound);
            context.AddTypedef(qualified, parsedSequence, input, baseOffset + position);
            context.Declarations.Add(new IdlTypedefDeclaration(parsedSequence, Path.GetFileName(input.Path)));

            position += sequenceTypedef.Length;

            return true;
        }

        var arrayTypedef = ArrayTypedefPattern.Match(declarations.Substring(position));
        if (arrayTypedef.Success)
        {
            var typedefName = arrayTypedef.Groups[2].Value;
            var qualified = context.Qualify(typedefName, currentNamespace);
            var declarationOffset = context.MapOffset(baseOffset + position);
            context.EnsureNewName(input, baseOffset + position, qualified);
            context.EnsureGeneratedCompanionNames(input, baseOffset + position, typedefName, currentNamespace, includeUnmanaged: true);
            var dimensions = context.TypeParser.ParseDimensions(input, baseOffset + position, arrayTypedef.Groups[3].Value, currentNamespace);
            context.DeferGeneratedDeclarationNameValidation(input, declarationOffset, typedefName, GeneratedTypedefMemberNames);
            var parsedArray = new IdlTypedef(
                typedefName,
                currentNamespace,
                "array",
                NormalizeIdlType(arrayTypedef.Groups[1].Value),
                null,
                dimensions);
            context.AddTypedef(qualified, parsedArray, input, baseOffset + position);
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
        var typedefOffset = context.MapOffset(baseOffset + position);
        context.EnsureNewName(input, baseOffset + position, typeName);
        context.EnsureGeneratedCompanionNames(input, baseOffset + position, name, currentNamespace, includeUnmanaged: true);
        context.DeferGeneratedDeclarationNameValidation(input, typedefOffset, name, GeneratedTypedefMemberNames);
        var typedefTarget = NormalizeIdlType(typedefDeclaration.Groups[1].Value);
        if (IdlBuiltinTypeSyntax.TryParseStringType(typedefTarget, out var isWideString, out _))
        {
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
                stringBound: bound?.Value,
                isWideString: isWideString);
            bound?.AddConsumer(parsedStringTypedef.SetStringBound);
            context.AddTypedef(typeName, parsedStringTypedef, input, baseOffset + position);
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
        context.AddTypedef(typeName, parsedTypedef, input, baseOffset + position);
        context.Declarations.Add(new IdlTypedefDeclaration(parsedTypedef, Path.GetFileName(input.Path)));

        position += typedefDeclaration.Length;

        return true;
    }

    private static readonly string[] GeneratedTypedefMemberNames =
    [
        "Value",
        "Equals",
        "GetHashCode",
        "ToString"
    ];
}
