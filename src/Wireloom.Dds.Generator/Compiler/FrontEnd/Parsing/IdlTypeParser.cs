using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL member types, annotations, bounds, and dimensions.</summary>
internal sealed class IdlTypeParser
{
    private readonly IdlParseContext context;

    internal IdlTypeParser(IdlParseContext context) =>
        this.context = context;

    internal (IdlMember Field, int Length) ParseMember(IdlInput input, string body, int offset, int sourceOffset, string? currentNamespace, HashSet<string> members, bool useHashIds = false)
    {
        var (annotations, annotationsLength) = ParseMemberAnnotations(input, body, offset, sourceOffset);
        offset += annotationsLength;

        var member = MemberPattern.Match(body[offset..]);
        if (!member.Success)
        {
            throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Unsupported member declaration.");
        }

        var field = member.Groups[3].Value;
        if (!members.Add(field))
        {
            throw new IdlException(input, context.MapOffset(sourceOffset + offset), $"Duplicate member: {field}");
        }

        var kind = member.Groups[1].Value.Trim();
        var memberSourceOffset = sourceOffset + offset;
        var dimensions = member.Groups[4].Success && member.Groups[4].Value.Length > 0
            ? ParseDimensions(input, memberSourceOffset, member.Groups[4].Value, currentNamespace)
            : [];
        IdlType parsedType;

        if (kind.StartsWith("sequence", StringComparison.Ordinal))
        {
            var (ElementType, Bound) = ParseSequenceType(input, memberSourceOffset, kind, currentNamespace);

            var element = context.ResolveFieldType(ElementType, currentNamespace, input, memberSourceOffset);
            if (element is null)
            {
                throw new IdlException(input, context.MapOffset(memberSourceOffset), $"Unknown collection element type: {ElementType}");
            }

            parsedType = new IdlType.Sequence(element, Bound, dimensions);
        }
        else if (dimensions.Count > 0)
        {
            var element = context.ResolveFieldType(kind, currentNamespace, input, memberSourceOffset);
            if (element is null)
            {
                throw new IdlException(input, context.MapOffset(memberSourceOffset), $"Unknown collection element type: {kind}");
            }

            parsedType = new IdlType.Array(element, dimensions);
        }
        else if (kind.StartsWith("string", StringComparison.Ordinal) || kind.StartsWith("wstring", StringComparison.Ordinal))
        {
            var bound = 255;

            if (member.Groups[2].Success)
            {
                bound = ResolveBound(input, memberSourceOffset, member.Groups[2].Value, currentNamespace, "String bound");
            }

            parsedType = new IdlType.StringType(kind.StartsWith("wstring", StringComparison.Ordinal), bound);
        }
        else if (IsPrimitive(kind))
        {
            parsedType = new IdlType.Primitive(NormalizeIdlType(kind));
        }
        else
        {
            var resolved = context.ResolveFieldType(kind, currentNamespace, input, memberSourceOffset);
            if (resolved is null)
            {
                throw new IdlException(input, context.MapOffset(memberSourceOffset), $"Unknown struct type: {kind}");
            }

            if (annotations.IsOptional && !IsOptionalScalar(resolved))
            {
                throw new IdlException(input, context.MapOffset(memberSourceOffset), "Optional aggregate members are not supported yet.");
            }

            parsedType = resolved;
        }

        if (parsedType is IdlType.Sequence { Dimensions.Count: > 0 })
        {
            context.Diagnostics?.Add(new IdlDiagnostic("DDSG0105", input, context.MapOffset(memberSourceOffset), $"The C# binding does not support arrays of sequences without using a typedef; generated code for member '{field}' may not match IDL semantics."));
        }

        var valueMetadata = ResolveMemberValueMetadata(input, memberSourceOffset, currentNamespace, parsedType, annotations);
        var memberId = annotations.MemberId;
        var memberIdHashSource = annotations.HashIdExpression is not null
            ? string.IsNullOrEmpty(annotations.HashIdExpression) ? field : annotations.HashIdExpression
            : useHashIds ? field : null;
        if (memberId is null && memberIdHashSource is not null)
        {
            memberId = ComputeHashMemberId(memberIdHashSource);
        }

        var usesAutoIdHash = useHashIds && annotations.MemberId is null && annotations.HashIdExpression is null;
        var metadata = new IdlMemberMetadata(
            annotations.IsKey,
            annotations.IsOptional,
            memberId,
            valueMetadata,
            annotations.IsExternal,
            annotations.IsMustUnderstand,
            memberIdHashSource,
            usesAutoIdHash);

        return (new IdlMember(field, parsedType, metadata), annotationsLength + member.Length);
    }

    private (MemberAnnotationState Annotations, int Length) ParseMemberAnnotations(IdlInput input, string body, int offset, int sourceOffset)
    {
        var annotations = new MemberAnnotationState();
        var length = 0;

        while (true)
        {
            var remaining = body[offset..];
            var keyAnnotation = KeyAnnotationPattern.Match(remaining);
            if (keyAnnotation.Success)
            {
                if (annotations.IsKey)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @key annotation.");
                }

                annotations.IsKey = true;
                offset += keyAnnotation.Length;
                length += keyAnnotation.Length;

                continue;
            }

            var optionalAnnotation = OptionalAnnotationPattern.Match(remaining);
            if (optionalAnnotation.Success)
            {
                if (annotations.IsOptional)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @optional annotation.");
                }

                annotations.IsOptional = true;
                offset += optionalAnnotation.Length;
                length += optionalAnnotation.Length;

                continue;
            }

            var idAnnotation = IdAnnotationPattern.Match(remaining);
            if (idAnnotation.Success)
            {
                if (annotations.MemberId is not null || annotations.HashIdExpression is not null || !int.TryParse(idAnnotation.Groups[1].Value, out var parsedId))
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate or invalid @id annotation.");
                }

                annotations.MemberId = parsedId;
                offset += idAnnotation.Length;
                length += idAnnotation.Length;

                continue;
            }

            var hashIdAnnotation = HashIdAnnotationPattern.Match(remaining);
            if (hashIdAnnotation.Success)
            {
                if (annotations.MemberId is not null || annotations.HashIdExpression is not null)
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

            var defaultAnnotation = DefaultAnnotationPattern.Match(remaining);
            if (defaultAnnotation.Success)
            {
                if (annotations.DefaultExpression is not null)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @default annotation.");
                }

                annotations.DefaultExpression = defaultAnnotation.Groups["value"].Value.Trim();
                offset += defaultAnnotation.Length;
                length += defaultAnnotation.Length;

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
                if (annotations.IsExternal)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @external annotation.");
                }

                annotations.IsExternal = true;
                offset += externalAnnotation.Length;
                length += externalAnnotation.Length;

                continue;
            }

            var mustUnderstandAnnotation = MustUnderstandAnnotationPattern.Match(remaining);
            if (mustUnderstandAnnotation.Success)
            {
                if (annotations.IsMustUnderstand)
                {
                    throw new IdlException(input, context.MapOffset(sourceOffset + offset), "Duplicate @must_understand annotation.");
                }

                annotations.IsMustUnderstand = true;
                offset += mustUnderstandAnnotation.Length;
                length += mustUnderstandAnnotation.Length;

                continue;
            }

            return (annotations, length);
        }
    }

    private IdlMemberValueMetadata? ResolveMemberValueMetadata(IdlInput input, int offset, string? currentNamespace, IdlType type, MemberAnnotationState annotations)
    {
        if (annotations.MinimumExpression is null &&
            annotations.MaximumExpression is null &&
            annotations.DefaultExpression is null &&
            annotations.UnitExpression is null)
        {
            return null;
        }

        var valueType = UnwrapAliases(type);
        if (valueType is not IdlType.Primitive and not IdlType.Enum)
        {
            throw new IdlException(input, context.MapOffset(offset), "@min, @max, @range, and @default are only supported on primitive and enum members.");
        }

        BigInteger? minimum = null;
        BigInteger? maximum = null;
        BigInteger? defaultValue = null;

        if (annotations.MinimumExpression is not null || annotations.MaximumExpression is not null)
        {
            if (valueType is not IdlType.Primitive primitive)
            {
                throw new IdlException(input, context.MapOffset(offset), "@min, @max, and @range require a primitive member.");
            }

            minimum = annotations.MinimumExpression is null
                ? null
                : EvaluateMemberInteger(input, offset, currentNamespace, annotations.MinimumExpression, "minimum");
            maximum = annotations.MaximumExpression is null
                ? null
                : EvaluateMemberInteger(input, offset, currentNamespace, annotations.MaximumExpression, "maximum");

            if (minimum is { } minimumValue)
            {
                IdlConstantParser.ValidateConstantRange(input, context.MapOffset(offset), primitive.Name, minimumValue);
            }

            if (maximum is { } maximumValue)
            {
                IdlConstantParser.ValidateConstantRange(input, context.MapOffset(offset), primitive.Name, maximumValue);
            }

            if (minimum is { } lower && maximum is { } upper && lower > upper)
            {
                throw new IdlException(input, context.MapOffset(offset), "Member minimum value cannot be greater than its maximum value.");
            }
        }

        if (annotations.DefaultExpression is not null)
        {
            if (valueType is IdlType.Primitive primitive)
            {
                defaultValue = EvaluateMemberInteger(input, offset, currentNamespace, annotations.DefaultExpression, "default");
                IdlConstantParser.ValidateConstantRange(input, context.MapOffset(offset), primitive.Name, defaultValue.Value);
            }
            else if (valueType is IdlType.Enum @enum)
            {
                if (!context.Symbols.TryGetEnum(@enum.QualifiedName, out var enumDeclaration))
                {
                    throw new IdlException(input, context.MapOffset(offset), $"Unknown enum type: {@enum.QualifiedName}");
                }

                var defaultName = annotations.DefaultExpression.Replace("::", ".").Split('.').Last();
                var enumMember = enumDeclaration.Members.SingleOrDefault(member => member.Name == defaultName);
                if (enumMember is null)
                {
                    throw new IdlException(input, context.MapOffset(offset), $"Unknown default enum value: {annotations.DefaultExpression}");
                }

                defaultValue = enumMember.Value;
            }
        }

        if (defaultValue is { } value && ((minimum is { } defaultLower && value < defaultLower) || (maximum is { } defaultUpper && value > defaultUpper)))
        {
            throw new IdlException(input, context.MapOffset(offset), "Member default value is outside its declared range.");
        }

        return new IdlMemberValueMetadata(defaultValue, minimum, maximum, annotations.DefaultExpression, annotations.UnitExpression);
    }

    private BigInteger EvaluateMemberInteger(IdlInput input, int offset, string? currentNamespace, string expression, string valueName)
    {
        try
        {
            return IdlConstantExpressionEvaluator.Evaluate(expression, context.Symbols, currentNamespace);
        }
        catch (FormatException exception)
        {
            throw new IdlException(input, context.MapOffset(offset), $"Invalid {valueName} expression: {exception.Message}");
        }
    }

    private static IdlType UnwrapAliases(IdlType type)
    {
        while (type is IdlType.Alias alias)
        {
            type = alias.Target;
        }

        return type;
    }

    private sealed class MemberAnnotationState
    {
        public bool IsKey { get; set; }
        public bool IsOptional { get; set; }
        public int? MemberId { get; set; }
        public string? HashIdExpression { get; set; }
        public string? MinimumExpression { get; set; }
        public string? MaximumExpression { get; set; }
        public string? DefaultExpression { get; set; }
        public string? UnitExpression { get; set; }
        public bool IsExternal { get; set; }
        public bool IsMustUnderstand { get; set; }
    }

    private static int ComputeHashMemberId(string value)
    {
        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
        var littleEndian = (uint)(hash[0] |
            (hash[1] << 8) |
            (hash[2] << 16) |
            (hash[3] << 24));
        return (int)(littleEndian & 0x0FFFFFFF);
    }

    internal int ResolveBound(IdlInput input, int offset, string text, string? currentNamespace, string diagnosticName = "Collection bound") =>
        context.Validator.ResolveBound(input, context.MapOffset(offset), text, currentNamespace, diagnosticName);

    internal IReadOnlyList<int> ParseDimensions(IdlInput input, int offset, string text, string? currentNamespace)
    {
        var result = new List<int>();

        foreach (Match match in Regex.Matches(text, @"\[([^\]]+)\]"))
        {
            result.Add(ResolveBound(input, offset, match.Groups[1].Value, currentNamespace, "Array dimensions"));
        }

        if (result.Count == 0)
        {
            throw new IdlException(input, context.MapOffset(offset), "Array declaration must specify at least one dimension.");
        }

        return result;
    }

    internal int ParseStringBound(IdlInput input, int offset, string type, string? currentNamespace, string errorMessage)
    {
        var bound = 255;

        var open = type.IndexOf('<');
        if (open >= 0)
        {
            bound = ResolveBound(input, offset, type.Substring(open + 1, type.Length - open - 2), currentNamespace, errorMessage.TrimEnd('.'));
        }

        return bound;
    }

    internal (string ElementType, int? Bound) ParseSequenceType(IdlInput input, int offset, string type, string? currentNamespace)
    {
        var inner = type.Substring(type.IndexOf('<') + 1, type.LastIndexOf('>') - type.IndexOf('<') - 1);
        var parts = inner.Split(',');
        if (parts.Length > 2 || parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
        {
            throw new IdlException(input, context.MapOffset(offset), "Malformed sequence declaration.");
        }

        return (NormalizeIdlType(parts[0].Trim()), parts.Length == 2 ? ResolveBound(input, offset, parts[1], currentNamespace) : null);
    }

    private static bool IsOptionalScalar(IdlType type) => type switch
    {
        IdlType.Primitive or IdlType.StringType or IdlType.Enum => true,
        IdlType.Alias alias => IsOptionalScalar(alias.Target),
        _ => false
    };

}
