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
    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, string? defaultExpression, ref int position)
    {
        if (defaultExpression is not null && (SequenceTypedefPattern.IsMatch(declarations.Substring(position)) || ArrayTypedefPattern.IsMatch(declarations.Substring(position))))
        {
            throw new IdlException(input, context.MapOffset(baseOffset + position), "@default is only supported on scalar and string typedefs.");
        }

        var sequenceTypedef = SequenceTypedefPattern.Match(declarations.Substring(position));
        if (sequenceTypedef.Success)
        {
            var sequenceName = sequenceTypedef.Groups[3].Value;
            context.RecordIdentifier(input, baseOffset + position + sequenceTypedef.Groups[3].Index, sequenceName, currentNamespace);
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
            context.RecordIdentifier(input, baseOffset + position + arrayTypedef.Groups[2].Index, typedefName, currentNamespace);
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
        context.RecordIdentifier(input, baseOffset + position + typedefDeclaration.Groups[2].Index, name, currentNamespace);
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
                isWideString: isWideString,
                defaultExpression: defaultExpression);
            bound?.AddConsumer(parsedStringTypedef.SetStringBound);
            context.AddTypedef(typeName, parsedStringTypedef, input, baseOffset + position);
            DeferTypedefDefault(parsedStringTypedef, input, baseOffset + position, currentNamespace, typedefTarget, defaultExpression);
            context.Declarations.Add(new IdlTypedefDeclaration(parsedStringTypedef, Path.GetFileName(input.Path)));

            position += typedefDeclaration.Length;

            return true;
        }

        var parsedTypedef = new IdlTypedef(
            name,
            currentNamespace,
            typedefTarget,
            null,
            null,
            defaultExpression: defaultExpression);
        context.AddTypedef(typeName, parsedTypedef, input, baseOffset + position);
        DeferTypedefDefault(parsedTypedef, input, baseOffset + position, currentNamespace, typedefTarget, defaultExpression);
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

    private void DeferTypedefDefault(IdlTypedef typedef, IdlInput input, int offset, string? currentNamespace, string target, string? defaultExpression)
    {
        if (defaultExpression is null)
        {
            return;
        }

        context.ValueBinder.DeferTypedefDefault(typedef, input, context.MapOffset(offset), currentNamespace, target, defaultExpression);
    }
}
