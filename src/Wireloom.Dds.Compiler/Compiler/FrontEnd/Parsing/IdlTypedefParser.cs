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
    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, IdlAnnotationApplicationSyntax? defaultApplication, ref int position)
    {
        if (defaultApplication is not null && (SequenceTypedefPattern.IsMatch(declarations.Substring(position)) || ArrayTypedefPattern.IsMatch(declarations.Substring(position))))
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
            var elementTypeOffset = baseOffset + position + sequenceTypedef.Groups[1].Index;
            var elementTypeWasEscaped = context.IsEscapedIdentifier(elementTypeOffset);
            var stringBound = context.TypeParser.ParseStringBound(input, elementTypeOffset, target, currentNamespace, "String sequence element bound must be a positive Int32.", elementTypeWasEscaped);
            var bound = sequenceTypedef.Groups[2].Success
                ? context.TypeParser.ResolveBound(input, baseOffset + position, sequenceTypedef.Groups[2].Value, currentNamespace)
                : null;
            var parsedSequence = new IdlTypedef(sequenceName, currentNamespace, "sequence", target, bound?.Value,
                stringBound: stringBound?.Value,
                effectiveSequenceBound: input.SequenceSize,
                effectiveStringBound: input.StringSize,
                elementTypeWasEscaped: elementTypeWasEscaped,
                isUnboundedSequence: bound is null && input.UnboundedSupport,
                isUnboundedStringElement: stringBound is null && input.UnboundedSupport);
            bound?.AddConsumer(parsedSequence.SetBound);
            stringBound?.AddConsumer(parsedSequence.SetStringBound);
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
            var arrayElementType = NormalizeIdlType(arrayTypedef.Groups[1].Value);
            var arrayElementTypeOffset = baseOffset + position + arrayTypedef.Groups[1].Index;
            var arrayElementTypeWasEscaped = context.IsEscapedIdentifier(arrayElementTypeOffset);
            var stringBound = context.TypeParser.ParseStringBound(input, arrayElementTypeOffset, arrayElementType, currentNamespace,
                "String array element bound must be a positive Int32.", arrayElementTypeWasEscaped);
            var parsedArray = new IdlTypedef(
                typedefName,
                currentNamespace,
                "array",
                arrayElementType,
                null,
                dimensions,
                stringBound: stringBound?.Value,
                effectiveSequenceBound: input.SequenceSize,
                effectiveStringBound: input.StringSize,
                elementTypeWasEscaped: arrayElementTypeWasEscaped,
                isUnboundedStringElement: stringBound is null && input.UnboundedSupport);
            stringBound?.AddConsumer(parsedArray.SetStringBound);
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
        var typedefTargetOffset = baseOffset + position + typedefDeclaration.Groups[1].Index;
        var targetWasEscaped = context.IsEscapedIdentifier(typedefTargetOffset);
        if (!targetWasEscaped && IdlBuiltinTypeSyntax.TryParseStringType(typedefTarget, out var isWideString, out _))
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
                effectiveStringBound: input.StringSize,
                isUnboundedString: bound is null && input.UnboundedSupport);
            bound?.AddConsumer(parsedStringTypedef.SetStringBound);
            context.AddTypedef(typeName, parsedStringTypedef, input, baseOffset + position);
            var typedefDeclarationModel = new IdlTypedefDeclaration(parsedStringTypedef, Path.GetFileName(input.Path));

            if (defaultApplication is not null)
            {
                typedefDeclarationModel.SetAnnotationApplications([defaultApplication]);
            }

            context.Declarations.Add(typedefDeclarationModel);

            position += typedefDeclaration.Length;

            return true;
        }

        var parsedTypedef = new IdlTypedef(
            name,
            currentNamespace,
            typedefTarget,
            null,
            null,
            effectiveSequenceBound: input.SequenceSize,
            effectiveStringBound: input.StringSize,
            targetWasEscaped: targetWasEscaped);
        context.AddTypedef(typeName, parsedTypedef, input, baseOffset + position);
        var typedefModel = new IdlTypedefDeclaration(parsedTypedef, Path.GetFileName(input.Path));

        if (defaultApplication is not null)
        {
            typedefModel.SetAnnotationApplications([defaultApplication]);
        }

        context.Declarations.Add(typedefModel);

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
