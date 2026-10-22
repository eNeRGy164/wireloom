using System.Globalization;
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
                var sequence = new IdlType.Sequence(element, Bound?.Value, dimensions);
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
                var stringType = new IdlType.StringType(isWideString, 255, isBounded: stringBoundExpression is not null);
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

            var memberId = fieldAnnotations.MemberId;
            var memberIdHashSource = fieldAnnotations.HashIdExpression is not null
                ? string.IsNullOrEmpty(fieldAnnotations.HashIdExpression) ? field : fieldAnnotations.HashIdExpression
                : useHashIds && fieldAnnotations.MemberIdExpression is null ? field : null;
            if (memberId is null && memberIdHashSource is not null)
            {
                memberId = IdlMemberIdHash.Compute(memberIdHashSource);
            }

            var usesAutoIdHash = useHashIds && fieldAnnotations.MemberId is null
                && fieldAnnotations.MemberIdExpression is null && fieldAnnotations.HashIdExpression is null;
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

        while (true)
        {
            var remaining = body[offset..];
            var keyAnnotation = KeyAnnotationPattern.Match(remaining);
            if (keyAnnotation.Success)
            {
                if (annotations.HasKey)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @key annotation.");
                }

                annotations.HasKey = true;
                annotations.IsKey = !keyAnnotation.Groups["value"].Success
                    || !string.Equals(keyAnnotation.Groups["value"].Value, "false", StringComparison.OrdinalIgnoreCase);
                offset += keyAnnotation.Length;
                length += keyAnnotation.Length;

                continue;
            }

            var optionalAnnotation = OptionalAnnotationPattern.Match(remaining);
            if (optionalAnnotation.Success)
            {
                if (annotations.HasOptional)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @optional annotation.");
                }

                annotations.HasOptional = true;
                annotations.IsOptional = !optionalAnnotation.Groups["value"].Success
                    || !string.Equals(optionalAnnotation.Groups["value"].Value, "false", StringComparison.OrdinalIgnoreCase);
                offset += optionalAnnotation.Length;
                length += optionalAnnotation.Length;

                continue;
            }

            if (IdlSyntaxReader.TryReadAnnotationValue(remaining, "id", out var memberIdExpression, out var idAnnotationLength))
            {
                if (annotations.MemberId is not null || annotations.MemberIdExpression is not null || annotations.HashIdExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate or invalid @id annotation.");
                }

                if (TryParseIdArgument(memberIdExpression, out var parsedId))
                {
                    annotations.MemberId = parsedId;
                }
                else
                {
                    annotations.MemberIdExpression = memberIdExpression;
                }

                offset += idAnnotationLength;
                length += idAnnotationLength;

                continue;
            }

            var hashIdAnnotation = HashIdAnnotationPattern.Match(remaining);
            if (hashIdAnnotation.Success)
            {
                if (annotations.MemberId is not null || annotations.MemberIdExpression is not null || annotations.HashIdExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate or conflicting @id/@hashid annotation.");
                }

                annotations.HashIdExpression = hashIdAnnotation.Groups["value"].Success
                    ? hashIdAnnotation.Groups["value"].Value
                    : string.Empty;
                offset += hashIdAnnotation.Length;
                length += hashIdAnnotation.Length;

                continue;
            }

            var minimumAnnotation = MinimumAnnotationPattern.Match(remaining);
            if (minimumAnnotation.Success)
            {
                if (annotations.MinimumExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @min or @range annotation.");
                }

                annotations.MinimumExpression = minimumAnnotation.Groups["value"].Value.Trim();
                offset += minimumAnnotation.Length;
                length += minimumAnnotation.Length;

                continue;
            }

            var maximumAnnotation = MaximumAnnotationPattern.Match(remaining);
            if (maximumAnnotation.Success)
            {
                if (annotations.MaximumExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @max or @range annotation.");
                }

                annotations.MaximumExpression = maximumAnnotation.Groups["value"].Value.Trim();
                offset += maximumAnnotation.Length;
                length += maximumAnnotation.Length;

                continue;
            }

            var rangeAnnotation = RangeAnnotationPattern.Match(remaining);
            if (rangeAnnotation.Success)
            {
                if (annotations.MinimumExpression is not null || annotations.MaximumExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @min, @max, or @range annotation.");
                }

                annotations.MinimumExpression = rangeAnnotation.Groups["min"].Value.Trim();
                annotations.MaximumExpression = rangeAnnotation.Groups["max"].Value.Trim();
                offset += rangeAnnotation.Length;
                length += rangeAnnotation.Length;

                continue;
            }

            if (IdlSyntaxReader.TryReadAnnotationValue(remaining, "default", out var defaultExpression, out var defaultAnnotationLength))
            {
                if (annotations.DefaultExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @default annotation.");
                }

                annotations.DefaultExpression = defaultExpression;
                offset += defaultAnnotationLength;
                length += defaultAnnotationLength;

                continue;
            }

            var unitAnnotation = UnitAnnotationPattern.Match(remaining);
            if (unitAnnotation.Success)
            {
                if (annotations.UnitExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @unit annotation.");
                }

                annotations.UnitExpression = unitAnnotation.Groups["value"].Value;
                offset += unitAnnotation.Length;
                length += unitAnnotation.Length;

                continue;
            }

            var resolveNameAnnotation = ResolveNameAnnotationPattern.Match(remaining);
            if (resolveNameAnnotation.Success)
            {
                offset += resolveNameAnnotation.Length;
                length += resolveNameAnnotation.Length;

                continue;
            }

            var externalAnnotation = ExternalAnnotationPattern.Match(remaining);
            if (externalAnnotation.Success)
            {
                if (annotations.HasExternal)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @external annotation.");
                }

                annotations.HasExternal = true;
                annotations.IsExternal = !externalAnnotation.Groups["value"].Success
                    || !string.Equals(externalAnnotation.Groups["value"].Value, "false", StringComparison.OrdinalIgnoreCase);
                offset += externalAnnotation.Length;
                length += externalAnnotation.Length;

                continue;
            }

            var mustUnderstandAnnotation = MustUnderstandAnnotationPattern.Match(remaining);
            if (mustUnderstandAnnotation.Success)
            {
                if (annotations.HasMustUnderstand)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @must_understand annotation.");
                }

                annotations.HasMustUnderstand = true;
                annotations.IsMustUnderstand = !mustUnderstandAnnotation.Groups["value"].Success
                    || !string.Equals(mustUnderstandAnnotation.Groups["value"].Value, "false", StringComparison.OrdinalIgnoreCase);
                offset += mustUnderstandAnnotation.Length;
                length += mustUnderstandAnnotation.Length;

                continue;
            }

            return (annotations, length);
        }
    }

    private static bool TryParseIdArgument(string value, out int parsedId)
    {
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            && BigInt.TryParse("0" + value[2..], NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var hexadecimal)
            && hexadecimal >= int.MinValue
            && hexadecimal <= int.MaxValue)
        {
            parsedId = (int)hexadecimal;
            return true;
        }

        return int.TryParse(value, out parsedId);
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

        var stringType = new IdlType.StringType(isWideString, 255, isBounded: boundExpression is not null);
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
