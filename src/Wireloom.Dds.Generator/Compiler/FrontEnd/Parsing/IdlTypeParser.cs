using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;
using BigInt = System.Numerics.BigInteger;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL member types, annotations, bounds, and dimensions.</summary>
internal sealed class IdlTypeParser
{
    private readonly IdlParseContext context;

    internal IdlTypeParser(IdlParseContext context) =>
        this.context = context;

    /// <summary>Parses one member statement and returns its declarators and consumed source length.</summary>
    internal (IReadOnlyList<IdlMember> Fields, int Length) ParseMember(IdlInput input, string body, int offset, int sourceOffset, string? currentNamespace, HashSet<string> members, bool useHashIds = false, string? memberScope = null)
    {
        var (annotations, annotationsLength) = ParseMemberAnnotations(input, body, offset, sourceOffset);
        offset += annotationsLength;

        var member = MemberPattern.Match(body[offset..]);
        if (!member.Success)
        {
            throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Unsupported member declaration.");
        }

        var kind = member.Groups[1].Value.Trim();
        var memberSourceOffset = sourceOffset + offset;
        var typeOffset = memberSourceOffset + member.Groups[1].Index;
        var declarators = new List<(string Name, string Dimensions, int RelativeOffset)>
        {
            (member.Groups[3].Value, member.Groups[4].Value, member.Groups[3].Index)
        };
        var remainingDeclarators = member.Groups["rest"];
        foreach (Match declarator in Regex.Matches(remainingDeclarators.Value, @"(?<name>[A-Za-z_]\w*)\s*(?<dimensions>(?:\[[^\]]+\])*)"))
        {
            declarators.Add((
                declarator.Groups["name"].Value,
                declarator.Groups["dimensions"].Value,
                remainingDeclarators.Index + declarator.Groups["name"].Index));
        }

        var fields = new List<IdlMember>(declarators.Count);
        for (var declaratorIndex = 0; declaratorIndex < declarators.Count; declaratorIndex++)
        {
            var (Name, Dimensions, RelativeOffset) = declarators[declaratorIndex];
            var fieldAnnotations = declaratorIndex == declarators.Count - 1 ? annotations : new IdlMemberAnnotationSyntax();
            var field = Name;
            var fieldOffset = memberSourceOffset + RelativeOffset;
            context.RecordIdentifier(input, sourceOffset + offset + RelativeOffset, field, memberScope, IdlIdentifierScopeKind.Aggregate);
            if (!members.Add(field))
            {
                var duplicateOffset = RelativeOffset == member.Groups[3].Index ? typeOffset : fieldOffset;
                throw new IdlException(input, context.MapOffset(duplicateOffset), $"Duplicate member: {field}");
            }

            var dimensions = Dimensions.Length > 0
                ? ParseDimensions(input, fieldOffset + field.Length, Dimensions, currentNamespace)
                : [];
            IdlType parsedType;

            if (IdlBuiltinTypeSyntax.IsSequenceType(kind))
            {
                var (ElementType, Bound) = ParseSequenceType(input, typeOffset, kind, currentNamespace);
                var element = ParseCollectionElementType(input, typeOffset, ElementType, currentNamespace, "Unknown collection element type");
                var sequence = new IdlType.Sequence(element, Bound?.Value, dimensions, input.SequenceSize);
                Bound?.AddConsumer(sequence.SetBound);
                parsedType = sequence;
            }
            else if (dimensions.Count > 0)
            {
                var element = ParseCollectionElementType(input, typeOffset, kind, currentNamespace, "Unknown collection element type");
                parsedType = new IdlType.Array(element, dimensions);
            }
            else if (IdlBuiltinTypeSyntax.TryParseStringType(kind, out var isWideString, out var stringBoundExpression))
            {
                var stringType = new IdlType.StringType(isWideString, input.StringSize, isBounded: stringBoundExpression is not null);
                if (member.Groups[2].Success)
                {
                    var deferredBound = ResolveBound(input, typeOffset, member.Groups[2].Value, currentNamespace, "String bound");
                    deferredBound.AddConsumer(stringType.SetBound);
                }

                parsedType = stringType;
            }
            else if (IsPrimitive(kind))
            {
                parsedType = new IdlType.Primitive(NormalizeIdlType(kind));
            }
            else
            {
                parsedType = context.ReferenceType(kind, currentNamespace, input, typeOffset, "Unknown struct type");
            }

            if (parsedType is IdlType.Sequence { Dimensions.Count: > 0 } && input.SequenceArrayMapping == IdlSequenceArrayMapping.RtiCSharp)
            {
                context.Diagnostics?.Add(new IdlDiagnostic("DDSG0105", input, context.MapOffset(fieldOffset), $"The C# binding does not support arrays of sequences without using a typedef; generated code for member '{field}' may not match IDL semantics."));
            }

            int? memberId = null;
            var memberIdHashSource = fieldAnnotations.HashIdExpression is not null
                ? string.IsNullOrEmpty(fieldAnnotations.HashIdExpression) ? field : fieldAnnotations.HashIdExpression
                : useHashIds && fieldAnnotations.MemberIdExpression is null ? field : null;
            if (memberId is null && memberIdHashSource is not null)
            {
                memberId = IdlMemberIdHash.Compute(memberIdHashSource);
            }

            var usesAutoIdHash = useHashIds && fieldAnnotations.MemberIdExpression is null
                && fieldAnnotations.HashIdExpression is null;
            var metadata = new IdlMemberMetadata(
                fieldAnnotations.IsKey,
                fieldAnnotations.IsOptional,
                memberId,
                valueMetadata: null,
                fieldAnnotations.IsExternal,
                fieldAnnotations.IsMustUnderstand,
                memberIdHashSource,
                usesAutoIdHash);

            var memberLocation = declaratorIndex == 0 ? typeOffset : fieldOffset;
            var result = new IdlMember(field, parsedType, metadata, input, context.MapOffset(memberLocation));
            fields.Add(result);
            context.ValueBinder.DeferMemberValues(result, input, context.MapOffset(typeOffset), currentNamespace, fieldAnnotations);

        }

        return (fields, annotationsLength + member.Length);
    }

    private (IdlMemberAnnotationSyntax Annotations, int Length) ParseMemberAnnotations(IdlInput input, string body, int offset, int sourceOffset)
    {
        var annotations = new IdlMemberAnnotationSyntax();
        var length = 0;
        var applications = IdlSyntaxReader.ReadAnnotationApplications(
            body[offset..],
            context.MapOffset(sourceOffset + offset));
        var compositionViolation = IdlAnnotationCatalog.FindCompositionViolation(applications, IdlAnnotationTarget.Member);
        if (compositionViolation is { } violation)
        {
            throw new IdlException(input, violation.Application.Offset, violation.Message);
        }

        while (true)
        {
            var remaining = body[offset..];
            if (!IdlSyntaxReader.TryReadAnnotationApplication(remaining, out var capturedName, out var capturedLength))
            {
                return (annotations, length);
            }
            var captured = new IdlAnnotationApplicationSyntax(
                capturedName,
                remaining[..capturedLength],
                context.MapOffset(sourceOffset + offset),
                capturedLength);
            if (IdlAnnotationCatalog.IsRecognizedUnsupported(capturedName))
            {
                context.Diagnostics?.Add(new IdlDiagnostic("DDSG0102", input, captured.Offset, $"Annotation '@{capturedName}' is recognized but unsupported and will be ignored."));
                throw new IdlException(input, captured.Offset, $"Annotation '@{capturedName}' is not supported in this context.");
            }
            if (!IdlAnnotationCatalog.IsKnown(capturedName))
            {
                context.Diagnostics?.Add(new IdlDiagnostic("DDSG0101", input, captured.Offset, $"Annotation '@{capturedName}' is not recognized and will be ignored."));
                offset += capturedLength;
                length += capturedLength;
                continue;
            }

            IReadOnlyList<IdlAnnotationArgumentSyntax> arguments;
            try
            {
                IdlAnnotationCatalog.ValidateTarget(captured, IdlAnnotationTarget.Member);
                IdlAnnotationCatalog.ValidateUniqueNamedArguments(captured);
                arguments = captured.ParseArguments();
            }
            catch (FormatException exception)
            {
                throw new IdlException(input, captured.Offset, exception.Message);
            }

            var value = arguments.Count == 0 ? null : arguments[0].Expression;
            switch (capturedName.ToLowerInvariant())
            {
                case "range":
                    var range = arguments.ToDictionary(argument => argument.Name!, argument => argument.Expression, StringComparer.Ordinal);
                    annotations.MinimumExpression = range["min"];
                    annotations.MaximumExpression = range["max"];
                    break;
                case "key":
                    if (annotations.HasKey) throw new IdlException(input, captured.Offset, "Duplicate @key annotation.");
                    annotations.HasKey = true;
                    annotations.IsKey = ParseBooleanAnnotationArgument(value, true, out var keyExpression);
                    annotations.KeyExpression = keyExpression;
                    break;
                case "optional":
                    if (annotations.HasOptional) throw new IdlException(input, captured.Offset, "Duplicate @optional annotation.");
                    annotations.HasOptional = true;
                    annotations.IsOptional = ParseBooleanAnnotationArgument(value, true, out var optionalExpression);
                    annotations.OptionalExpression = optionalExpression;
                    break;
                case "id":
                    annotations.MemberIdExpression = value;
                    break;
                case "hashid":
                    annotations.HashIdExpression = DecodeStringLiteralArgument(value, input, captured.Offset);
                    break;
                case "min":
                    if (annotations.MinimumExpression is not null) throw new IdlException(input, captured.Offset, "Duplicate @min or @range annotation.");
                    annotations.MinimumExpression = value!.Trim();
                    break;
                case "max":
                    if (annotations.MaximumExpression is not null) throw new IdlException(input, captured.Offset, "Duplicate @max or @range annotation.");
                    annotations.MaximumExpression = value!.Trim();
                    break;
                case "default":
                    if (annotations.DefaultExpression is not null) throw new IdlException(input, captured.Offset, "Duplicate @default annotation.");
                    annotations.DefaultExpression = value;
                    break;
                case "unit":
                    if (annotations.UnitExpression is not null) throw new IdlException(input, captured.Offset, "Duplicate @unit annotation.");
                    annotations.UnitExpression = value is null
                        ? null
                        : DecodeStringLiteralArgument(value, input, captured.Offset);
                    break;
                case "resolve_name":
                    break;
                case "external":
                    if (annotations.HasExternal) throw new IdlException(input, captured.Offset, "Duplicate @external annotation.");
                    annotations.HasExternal = true;
                    annotations.IsExternal = ParseBooleanAnnotationArgument(value, true, out var externalExpression);
                    annotations.ExternalExpression = externalExpression;
                    break;
                case "must_understand":
                    if (annotations.HasMustUnderstand) throw new IdlException(input, captured.Offset, "Duplicate @must_understand annotation.");
                    annotations.HasMustUnderstand = true;
                    annotations.IsMustUnderstand = ParseBooleanAnnotationArgument(value, true, out var mustUnderstandExpression);
                    annotations.MustUnderstandExpression = mustUnderstandExpression;
                    break;
                default:
                    throw new IdlException(input, captured.Offset, $"Annotation '@{capturedName}' is not supported on members.");
            }

            offset += capturedLength;
            length += capturedLength;
        }
    }

    private static string DecodeStringLiteralArgument(string? value, IdlInput input, int offset)
    {
        if (value is null)
        {
            return string.Empty;
        }

        var isWide = value.TrimStart().StartsWith("L\"", StringComparison.Ordinal);
        return IdlStringLiteralDecoder.Decode(value, isWide, input, offset);
    }

    private static bool ParseBooleanAnnotationArgument(string? value, bool defaultValue, out string? unresolvedExpression)
    {
        unresolvedExpression = null;
        if (value is null)
        {
            return defaultValue;
        }

        if (string.Equals(value, "true", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (string.Equals(value, "false", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        unresolvedExpression = value;
        return false;
    }

    /// <summary>Resolves and validates a collection bound.</summary>
    internal IdlDeferredBound ResolveBound(IdlInput input, int offset, string text, string? currentNamespace, string diagnosticName = "Collection bound") =>
        context.DeferBound(input, offset, text, currentNamespace, diagnosticName);

    /// <summary>Parses array dimensions from an IDL declaration.</summary>
    internal IReadOnlyList<int> ParseDimensions(IdlInput input, int offset, string text, string? currentNamespace)
    {
        var result = new List<int>();
        IdlDeferredBound? lastDimensionBound = null;

        foreach (Match match in Regex.Matches(text, @"\[([^\]]+)\]"))
        {
            var bound = ResolveBound(input, offset, match.Groups[1].Value, currentNamespace, "Array dimensions");
            var index = result.Count;
            result.Add(bound.Value);
            bound.AddConsumer(value => result[index] = value);
            lastDimensionBound = bound;
        }

        if (result.Count == 0)
        {
            throw new IdlException(input, context.MapOffset(offset), "Array declaration must specify at least one dimension.");
        }

        lastDimensionBound!.AddConsumer(_ => ValidateArrayElementCount(input, offset, result));

        return result;
    }

    private void ValidateArrayElementCount(IdlInput input, int offset, IReadOnlyList<int> dimensions)
    {
        var elementCount = dimensions.Aggregate(BigInt.One, (count, dimension) => count * dimension);
        if (elementCount > int.MaxValue)
        {
            throw new IdlException(input, context.MapOffset(offset), "The total number of array elements cannot exceed Int32.MaxValue.");
        }
    }

    /// <summary>Parses and validates a bounded string type.</summary>
    internal IdlDeferredBound? ParseStringBound(IdlInput input, int offset, string type, string? currentNamespace, string errorMessage)
    {
        if (IdlBuiltinTypeSyntax.TryParseStringType(type, out _, out var boundExpression)
            && boundExpression is not null)
        {
            return ResolveBound(input, offset, boundExpression, currentNamespace, errorMessage.TrimEnd('.'));
        }

        return null;
    }

    /// <summary>Parses a collection element while retaining bounds on inline string types.</summary>
    internal IdlType ParseCollectionElementType(
        IdlInput input,
        int offset,
        string elementType,
        string? currentNamespace,
        string errorPrefix)
    {
        if (!IdlBuiltinTypeSyntax.TryParseStringType(elementType, out var isWideString, out var boundExpression))
        {
            return context.ReferenceType(elementType, currentNamespace, input, offset, errorPrefix);
        }

        var stringType = new IdlType.StringType(isWideString, input.StringSize, isBounded: boundExpression is not null);
        var bound = ParseStringBound(input, offset, elementType, currentNamespace, "String bound must be a positive Int32.");
        bound?.AddConsumer(stringType.SetBound);
        return stringType;
    }

    internal (string ElementType, IdlDeferredBound? Bound) ParseSequenceType(IdlInput input, int offset, string type, string? currentNamespace)
    {
        var inner = type.Substring(type.IndexOf('<') + 1, type.LastIndexOf('>') - type.IndexOf('<') - 1);
        var parts = inner.Split(',');
        if (parts.Length > 2 || parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
        {
            throw new IdlException(input, context.MapOffset(offset), "Malformed sequence declaration.");
        }

        return (NormalizeIdlType(parts[0].Trim()), parts.Length == 2 ? ResolveBound(input, offset, parts[1], currentNamespace) : null);
    }

}
