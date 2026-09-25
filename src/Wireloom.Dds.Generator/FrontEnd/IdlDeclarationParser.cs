using System.Numerics;
using System.Text.RegularExpressions;

namespace Wireloom;

using static IdlCompiler;
using static IdlGrammar;

internal sealed class IdlDeclarationParser
{
    private readonly IdlTypeResolver typeResolver;
    private readonly IdlSemanticValidator validator;
    private readonly IdlSymbolTable symbols;
    private readonly List<IdlDeclaration> declarationQueue = [];
    private readonly Dictionary<string, IdlClassDeclaration> classes = new(StringComparer.Ordinal);
    private readonly CancellationToken cancellationToken;

    internal IdlDeclarationParser(IdlSymbolTable symbols, CancellationToken cancellationToken)
    {
        this.symbols = symbols;
        typeResolver = new IdlTypeResolver(symbols);
        validator = new IdlSemanticValidator(symbols);
        this.cancellationToken = cancellationToken;
    }

    internal IReadOnlyList<IdlDeclaration> Declarations => declarationQueue;

    internal bool TryGetClass(string name, out IdlClassDeclaration declaration) =>
        classes.TryGetValue(name, out declaration!);

    private void AddClass(string qualifiedName, IdlClassDeclaration declaration) =>
        classes.Add(qualifiedName, declaration);

    /// <summary>Parses declarations in one module and emits their documents.</summary>
    public void Parse(string declarations, IdlInput input, int baseOffset, string? currentNamespace)
    {
        var position = 0;

        while (position < declarations.Length)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (char.IsWhiteSpace(declarations[position]))
            {
                position++;
                continue;
            }

            var defaultNested = DefaultNestedAnnotationPattern.Match(declarations.Substring(position));
            if (defaultNested.Success)
            {
                position += defaultNested.Length;
                continue;
            }

            if (TryParseModule(declarations, input, baseOffset, currentNamespace, ref position) ||
                TryParseConstant(declarations, input, baseOffset, currentNamespace, ref position) ||
                TryParseEnum(declarations, input, baseOffset, currentNamespace, ref position) ||
                TryParseTypedef(declarations, input, baseOffset, currentNamespace, ref position))
            {
                continue;
            }

            if (TryParseUnion(declarations, input, baseOffset, currentNamespace, ref position))
            {
                continue;
            }

            position += ParseStruct(declarations, input, baseOffset, currentNamespace, position);
        }

        foreach (var typedefName in symbols.TypedefNames.ToArray())
        {
            ValidateTypedef(input, baseOffset, typedefName);
        }
    }

    private int ParseStruct(string declarations, IdlInput input, int baseOffset, string? currentNamespace, int position)
    {
        var declaration = StructPattern.Match(declarations.Substring(position));
        if (!declaration.Success)
        {
            var remaining = declarations[position..].TrimStart();
            var tokenEnd = remaining.IndexOfAny([' ', '\t', '\r', '\n', '{', ';']);
            var token = tokenEnd < 0 ? remaining : remaining[..tokenEnd];
            throw new IdlException(input, baseOffset + position, $"Unsupported IDL syntax near '{token}'. This generator does not support that declaration or annotation.");
        }

        var name = declaration.Groups["name"].Value;
        var fullyQualifiedName = Qualify(name, currentNamespace);
        EnsureNewName(input, baseOffset + position, fullyQualifiedName);
        var body = declaration.Groups["body"];
        var extensibility = declaration.Groups["extensibility"].Value.Trim() switch
        {
            "@final" => IdlExtensibilityKind.Final,
            "@mutable" => IdlExtensibilityKind.Mutable,
            _ => IdlExtensibilityKind.Extensible
        };
        var fields = ParseStructMembers(input, body.Value, baseOffset + position + body.Index, currentNamespace);
        IdlSemanticValidator.ValidateMemberIds(input, baseOffset + position, fields);

        var parsedDeclaration = new IdlClassDeclaration(
            name,
            currentNamespace,
            fields,
            extensibility,
            declaration.Groups["nested"].Success || declaration.Groups["nestedAfter"].Success,
            input,
            declaration.Groups["base"].Success
                ? EscapeQualifiedIdentifier(ResolveTypeName(declaration.Groups["base"].Value, currentNamespace))
                : null);
        declarationQueue.Add(parsedDeclaration);

        AddClass(fullyQualifiedName, parsedDeclaration);

        return declaration.Length;
    }

    private List<IdlMember> ParseStructMembers(IdlInput input, string body, int sourceOffset, string? currentNamespace)
    {
        var offset = 0;
        var members = new HashSet<string>(StringComparer.Ordinal);
        var fields = new List<IdlMember>();

        while (offset < body.Length)
        {
            if (char.IsWhiteSpace(body[offset]))
            {
                offset++;
                continue;
            }

            var (Field, Length) = ParseMember(input, body, offset, sourceOffset, currentNamespace, members);
            fields.Add(Field);
            offset += Length;
        }

        return fields;
    }

    private bool TryParseModule(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var module = ModulePattern.Match(declarations.Substring(position));
        if (!module.Success)
        {
            return false;
        }

        var openBrace = position + module.Length - 1;
        var closeBrace = FindClosingBrace(declarations, openBrace);
        if (closeBrace < 0)
        {
            throw new IdlException(input, baseOffset + position, "Unterminated module declaration.");
        }

        var moduleName = Qualify(module.Groups[1].Value, currentNamespace);
        Parse(
            declarations.Substring(openBrace + 1, closeBrace - openBrace - 1),
            input,
            baseOffset + openBrace + 1,
            moduleName);

        position = closeBrace + 1;
        while (position < declarations.Length && char.IsWhiteSpace(declarations[position]))
        {
            position++;
        }

        if (position >= declarations.Length || declarations[position] != ';')
        {
            throw new IdlException(input, baseOffset + closeBrace, "Module declaration must end with a semicolon.");
        }

        position++;

        return true;
    }

    private bool TryParseConstant(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var constant = ConstantPattern.Match(declarations.Substring(position));
        if (!constant.Success)
        {
            return false;
        }

        var name = constant.Groups["name"].Value;
        var qualified = Qualify(name, currentNamespace);
        EnsureNewName(input, baseOffset + position, qualified);

        var type = NormalizeIdlType(constant.Groups["type"].Value);
        var expression = constant.Groups["expression"].Value.Trim();
        var integerValue = TryEvaluateIntegerConstant(
            input,
            baseOffset + position,
            type,
            expression,
            currentNamespace);
        var declaration = new IdlConstantDeclaration(
            name,
            type,
            expression,
            currentNamespace,
            Path.GetFileName(input.Path),
            integerValue);

        symbols.AddConstant(qualified, declaration);
        declarationQueue.Add(declaration);

        position += constant.Length;

        return true;
    }

    private bool TryParseEnum(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var enumDeclaration = EnumPattern.Match(declarations.Substring(position));
        if (!enumDeclaration.Success)
        {
            return false;
        }

        var enumName = enumDeclaration.Groups["name"].Value;
        var qualified = Qualify(enumName, currentNamespace);
        EnsureNewName(input, baseOffset + position, qualified);
        var enumMembers = new List<IdlEnumMember>();
        var nextValue = 0L;
        var enumBody = enumDeclaration.Groups["body"];
        var rawMembers = enumBody.Value.Split(',');
        var memberOffset = 0;

        foreach (var rawMember in rawMembers)
        {
            var trimmedMember = rawMember.Trim();
            var leadingWhitespace = rawMember.Length - rawMember.TrimStart().Length;
            var sourceOffset = baseOffset + position + enumBody.Index + memberOffset + leadingWhitespace;
            memberOffset += rawMember.Length + 1;

            var member = EnumMemberPattern.Match(trimmedMember);
            if (!member.Success)
            {
                throw new IdlException(input, sourceOffset, "Malformed enum member; expected an identifier, an optional prefix @value(<signed decimal>), or an explicit signed decimal value.");
            }

            if (member.Groups["value"].Success && member.Groups["explicit"].Success)
            {
                throw new IdlException(input, sourceOffset, "Combined @value and explicit enum values are unsupported.");
            }

            var value = ParseEnumValue(input, sourceOffset, member, nextValue);
            var isDefaultLiteral = member.Groups["defaultLiteral"].Success;
            if (isDefaultLiteral && hasDefaultLiteral)
            {
                throw new IdlException(input, sourceOffset, "An enum may contain at most one @default_literal.");
            }

            hasDefaultLiteral |= isDefaultLiteral;
            var hasExplicitValue = member.Groups["value"].Success || member.Groups["explicit"].Success;
            enumMembers.Add(new IdlEnumMember(member.Groups["name"].Value, value, hasExplicitValue, isDefaultLiteral));

            nextValue = value + 1;
        }

        var parsedEnum = new IdlEnum(enumName, currentNamespace, enumMembers, ParseExtensibility(enumDeclaration.Groups["extensibility"].Value));
        symbols.AddEnum(qualified, parsedEnum);
        declarationQueue.Add(new IdlEnumDeclaration(parsedEnum, Path.GetFileName(input.Path)));

        position += enumDeclaration.Length;

        return true;
    }

    private static int ParseEnumValue(IdlInput input, int sourceOffset, Match member, long nextValue)
    {
        if (member.Groups["explicit"].Success || member.Groups["value"].Success)
        {
            var valueText = member.Groups["explicit"].Success ? member.Groups["explicit"].Value : member.Groups["value"].Value;
            if (!int.TryParse(valueText, out var value))
            {
                throw new IdlException(input, sourceOffset, "Enum value must be a signed Int32 decimal.");
            }

            return value;
        }

        if (nextValue is < int.MinValue or > int.MaxValue)
        {
            throw new IdlException(input, sourceOffset, "Implicit enum value exceeds the Int32 range.");
        }

        return (int)nextValue;
    }

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

    private bool TryParseUnion(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var unionDeclaration = UnionPattern.Match(declarations.Substring(position));
        if (!unionDeclaration.Success)
        {
            return false;
        }

        var unionName = unionDeclaration.Groups["name"].Value;
        var qualified = Qualify(unionName, currentNamespace);
        EnsureNewName(input, baseOffset + position, qualified);
        var discriminatorIdlType = NormalizeIdlType(unionDeclaration.Groups["discriminator"].Value);
        var discriminatorQualified = ResolveTypeName(discriminatorIdlType, currentNamespace);
        var discriminatorIsEnum = symbols.TryGetEnum(discriminatorQualified, out var discriminatorEnum);
        var unionBody = unionDeclaration.Groups["body"];
        var branches = new List<IdlUnionBranch>();
        var branchNames = new HashSet<string>(StringComparer.Ordinal);
        var unionOffset = 0;

        while (unionOffset < unionBody.Length)
        {
            if (char.IsWhiteSpace(unionBody.Value[unionOffset]))
            {
                unionOffset++;
                continue;
            }

            var (Branch, Length) = ParseUnionBranch(
                input,
                unionBody.Value,
                unionOffset,
                baseOffset + position + unionBody.Index,
                currentNamespace,
                discriminatorQualified,
                discriminatorIsEnum,
                discriminatorEnum,
                branchNames);
            branches.Add(Branch);

            unionOffset += Length;
        }

        if (branches.Count == 0 || branches.Count(branch => branch.IsDefault) > 1)
        {
            throw new IdlException(input, baseOffset + position, "The union must contain at least one branch and at most one default branch.");
        }

        declarationQueue.Add(new IdlUnionDeclaration(
            new IdlUnion(
                unionName,
                currentNamespace,
                discriminatorIdlType,
                discriminatorIsEnum,
                branches,
                ParseExtensibility(unionDeclaration.Groups["extensibility"].Value),
                unionDeclaration.Groups["nested"].Success || unionDeclaration.Groups["nestedAfter"].Success),
            Path.GetFileName(input.Path)));

        position += unionDeclaration.Length;

        return true;
    }

    private static IdlExtensibilityKind ParseExtensibility(string annotation) => annotation.Trim() switch
    {
        "@final" => IdlExtensibilityKind.Final,
        "@mutable" => IdlExtensibilityKind.Mutable,
        _ => IdlExtensibilityKind.Extensible
    };

    private (IdlUnionBranch Branch, int Length) ParseUnionBranch(
        IdlInput input,
        string body,
        int offset,
        int sourceOffset,
        string? currentNamespace,
        string discriminatorQualified,
        bool discriminatorIsEnum,
        IdlEnum? discriminatorEnum,
        HashSet<string> branchNames)
    {
        var branch = UnionBranchPattern.Match(body.Substring(offset));
        if (!branch.Success)
        {
            throw new IdlException(input, sourceOffset + offset, "Unsupported union branch declaration.");
        }

        var branchName = branch.Groups["name"].Value;
        if (!branchNames.Add(branchName))
        {
            throw new IdlException(input, sourceOffset + offset, $"Duplicate union branch: {branchName}");
        }

        var branchType = NormalizeIdlType(branch.Groups["type"].Value);
        var field = ParseUnionBranchField(input, branchName, branchType, sourceOffset + offset, currentNamespace);
        var labels = new List<string>();
        var labelValues = new List<int>();

        if (branch.Groups[1].Success)
        {
            foreach (Match match in Regex.Matches(branch.Groups[1].Value, @"case\s+(-?[0-9]+|[A-Za-z_]\w*)"))
            {
                var label = match.Groups[1].Value;
                if (int.TryParse(label, out var numericLabel))
                {
                    labels.Add(numericLabel.ToString());
                    labelValues.Add(numericLabel);
                }
                else if (discriminatorIsEnum && discriminatorEnum!.Members.Any(member => member.Name == label))
                {
                    labels.Add($"{EscapeQualifiedIdentifier(discriminatorQualified)}.{EscapeIdentifier(label)}");
                    labelValues.Add(discriminatorEnum.Members.Single(member => member.Name == label).Value);
                }
                else
                {
                    throw new IdlException(input, sourceOffset + offset, $"Unknown union discriminator label: {label}");
                }
            }
        }

        return (new IdlUnionBranch(field, labels, labelValues, branch.Groups[2].Success), branch.Length);
    }

    private IdlMember ParseUnionBranchField(IdlInput input, string branchName, string branchType, int sourceOffset, string? currentNamespace)
    {
        if (branchType.StartsWith("sequence", StringComparison.Ordinal))
        {
            var (ElementType, Bound) = ParseSequenceType(input, sourceOffset, branchType, currentNamespace);
            var element = ResolveFieldType(ElementType, currentNamespace);
            if (element is null)
            {
                throw new IdlException(input, sourceOffset, $"Unknown union collection element type: {ElementType}");
            }

            return CreateCollectionMember(branchName, IdlCollectionKind.Sequence, Bound ?? 100, element);
        }

        if (branchType.StartsWith("string", StringComparison.Ordinal) ||
            branchType.StartsWith("wstring", StringComparison.Ordinal))
        {
            var bound = ParseStringBound(input, sourceOffset, branchType, currentNamespace, "Union string bound must be a positive Int32.");
            return new IdlMember(
                branchName,
                new IdlType.StringType(branchType.StartsWith("wstring", StringComparison.Ordinal), bound));
        }

        if (IsPrimitive(branchType))
        {
            return new IdlMember(branchName, new IdlType.Primitive(NormalizeIdlType(branchType)));
        }

        var resolved = ResolveFieldType(branchType, currentNamespace);
        if (resolved is null)
        {
            throw new IdlException(input, sourceOffset, $"Unknown union branch type: {branchType}");
        }

        return new IdlMember(branchName, resolved);
    }

    private (IdlMember Field, int Length) ParseMember(
        IdlInput input,
        string body,
        int offset,
        int sourceOffset,
        string? currentNamespace,
        HashSet<string> members)
    {
        var annotationsLength = 0;
        var isKey = false;
        var isOptional = false;
        int? memberId = null;

        while (true)
        {
            var keyAnnotation = KeyAnnotationPattern.Match(body.Substring(offset));
            if (keyAnnotation.Success)
            {
                if (isKey)
                {
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @key annotation.");
                }

                isKey = true;
                offset += keyAnnotation.Length;
                annotationsLength += keyAnnotation.Length;

                continue;
            }

            var optionalAnnotation = OptionalAnnotationPattern.Match(body.Substring(offset));
            if (optionalAnnotation.Success)
            {
                if (isOptional)
                {
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @optional annotation.");
                }

                isOptional = true;
                offset += optionalAnnotation.Length;
                annotationsLength += optionalAnnotation.Length;

                continue;
            }

            var idAnnotation = IdAnnotationPattern.Match(body.Substring(offset));
            if (idAnnotation.Success)
            {
                if (memberId is not null || !int.TryParse(idAnnotation.Groups[1].Value, out var parsedId))
                {
                    throw new IdlException(input, sourceOffset + offset, "Duplicate or invalid @id annotation.");
                }

                memberId = parsedId;
                offset += idAnnotation.Length;
                annotationsLength += idAnnotation.Length;

                continue;
            }

            break;
        }

        var member = MemberPattern.Match(body.Substring(offset));
        if (!member.Success)
        {
            throw new IdlException(input, sourceOffset + offset, "Unsupported member declaration.");
        }

        var field = member.Groups[3].Value;
        if (!members.Add(field))
        {
            throw new IdlException(input, sourceOffset + offset, $"Duplicate member: {field}");
        }

        var kind = member.Groups[1].Value.Trim();
        var memberSourceOffset = sourceOffset + offset;
        var dimensions = member.Groups[4].Success && member.Groups[4].Value.Length > 0
            ? ParseDimensions(input, memberSourceOffset, member.Groups[4].Value, currentNamespace)
            : [];
        IdlMember parsedField;

        if (kind.StartsWith("sequence", StringComparison.Ordinal))
        {
            var (ElementType, Bound) = ParseSequenceType(input, memberSourceOffset, kind, currentNamespace);

            var element = ResolveFieldType(ElementType, currentNamespace);
            if (element is null)
            {
                throw new IdlException(input, memberSourceOffset, $"Unknown collection element type: {ElementType}");
            }

            parsedField = CreateCollectionMember(field, IdlCollectionKind.Sequence, Bound, element, isKey: isKey, isOptional: isOptional, memberId: memberId);
        }
        else if (dimensions.Count > 0)
        {
            var element = ResolveFieldType(kind, currentNamespace);
            if (element is null)
            {
                throw new IdlException(input, memberSourceOffset, $"Unknown collection element type: {kind}");
            }

            parsedField = CreateCollectionMember(field, IdlCollectionKind.Array, null, element, dimensions, isKey, isOptional, memberId);
        }
        else if (kind.StartsWith("string", StringComparison.Ordinal) || kind.StartsWith("wstring", StringComparison.Ordinal))
        {
            var bound = 255;

            if (member.Groups[2].Success)
            {
                bound = ResolveBound(input, memberSourceOffset, member.Groups[2].Value, currentNamespace, "String bound");
            }

            parsedField = new IdlMember(
                field,
                new IdlType.StringType(
                    kind.StartsWith("wstring", StringComparison.Ordinal),
                    bound),
                new IdlMemberMetadata(isKey, isOptional, memberId));
        }
        else if (IsPrimitive(kind))
        {
            parsedField = new IdlMember(field, new IdlType.Primitive(NormalizeIdlType(kind)), new IdlMemberMetadata(isKey, isOptional, memberId));
        }
        else
        {
            var resolved = ResolveFieldType(kind, currentNamespace);
            if (resolved is null)
            {
                throw new IdlException(input, memberSourceOffset, $"Unknown struct type: {kind}");
            }

            if (isOptional && !IsOptionalScalar(resolved))
            {
                throw new IdlException(input, memberSourceOffset, "Optional aggregate members are not supported yet.");
            }

            parsedField = new IdlMember(field, resolved, new IdlMemberMetadata(isKey, isOptional, memberId));
        }

        return (parsedField, annotationsLength + member.Length);
    }

    private void EnsureNewName(IdlInput input, int offset, string name) =>
        validator.EnsureNewName(input, offset, name);

    private string Qualify(string name, string? currentNamespace) =>
        currentNamespace is null ? name : $"{currentNamespace}.{name}";

    private IdlType? ResolveFieldType(string idlType, string? currentNamespace) =>
        typeResolver.Resolve(idlType, currentNamespace);

    private void ValidateTypedef(IdlInput input, int offset, string name) =>
        validator.ValidateTypedef(input, offset, name);

    private int ResolveBound(
        IdlInput input,
        int offset,
        string text,
        string? currentNamespace,
        string diagnosticName = "Collection bound") =>
        validator.ResolveBound(input, offset, text, currentNamespace, diagnosticName);

    private IReadOnlyList<int> ParseDimensions(IdlInput input, int offset, string text, string? currentNamespace)
    {
        var result = new List<int>();

        foreach (Match match in Regex.Matches(text, @"\[([^\]]+)\]"))
        {
            result.Add(ResolveBound(input, offset, match.Groups[1].Value, currentNamespace, "Array dimensions"));
        }

        if (result.Count == 0)
        {
            throw new IdlException(input, offset, "Array declaration must specify at least one dimension.");
        }

        return result;
    }

    private int ParseStringBound(IdlInput input, int offset, string type, string? currentNamespace, string errorMessage)
    {
        var bound = 255;
        var open = type.IndexOf('<');
        if (open >= 0)
        {
            bound = ResolveBound(input, offset, type.Substring(open + 1, type.Length - open - 2), currentNamespace, errorMessage.TrimEnd('.'));
        }

        return bound;
    }

    private (string ElementType, int? Bound) ParseSequenceType(IdlInput input, int offset, string type, string? currentNamespace)
    {
        var inner = type.Substring(type.IndexOf('<') + 1, type.LastIndexOf('>') - type.IndexOf('<') - 1);
        var parts = inner.Split(',');
        if (parts.Length > 2 || parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
        {
            throw new IdlException(input, offset, "Malformed sequence declaration.");
        }

        return (NormalizeIdlType(parts[0].Trim()), parts.Length == 2 ? ResolveBound(input, offset, parts[1], currentNamespace) : null);
    }

    private BigInteger? TryEvaluateIntegerConstant(
        IdlInput input,
        int offset,
        string type,
        string expression,
        string? currentNamespace)
    {
        if (type is "string" or "wstring" or "float" or "double" or "long double" or "boolean" or "char" or "wchar")
        {
            return null;
        }

        try
        {
            var value = IdlConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace);
            ValidateConstantRange(input, offset, type, value);
            return value;
        }
        catch (FormatException exception)
        {
            throw new IdlException(input, offset, $"Invalid {type} constant expression: {exception.Message}");
        }
    }

    private static void ValidateConstantRange(IdlInput input, int offset, string type, BigInteger value)
    {
        var (minimum, maximum) = type switch
        {
            "int8" => (BigInteger.Parse("-128"), BigInteger.Parse("127")),
            "uint8" or "octet" => (BigInteger.Zero, BigInteger.Parse("255")),
            "short" or "int16" => (BigInteger.Parse("-32768"), BigInteger.Parse("32767")),
            "unsigned short" or "uint16" => (BigInteger.Zero, BigInteger.Parse("65535")),
            "long" or "int32" => (BigInteger.Parse(int.MinValue.ToString()), BigInteger.Parse(int.MaxValue.ToString())),
            "unsigned long" or "uint32" => (BigInteger.Zero, BigInteger.Parse(uint.MaxValue.ToString())),
            "long long" or "int64" => (BigInteger.Parse(long.MinValue.ToString()), BigInteger.Parse(long.MaxValue.ToString())),
            "unsigned long long" or "uint64" => (BigInteger.Zero, BigInteger.Parse(ulong.MaxValue.ToString())),
            _ => throw new InvalidOperationException($"Unsupported integral constant type: {type}")
        };

        if (value < minimum || value > maximum)
        {
            throw new IdlException(input, offset, $"{type} constant value is outside its representable range.");
        }
    }

    private static IdlMember CreateCollectionMember(
        string name,
        IdlCollectionKind kind,
        int? bound,
        IdlType element,
        IReadOnlyList<int>? dimensions = null,
        bool isKey = false,
        bool isOptional = false,
        int? memberId = null)
    {
        IdlType collection = kind == IdlCollectionKind.Sequence
            ? new IdlType.Sequence(element, bound)
            : new IdlType.Array(element, dimensions ?? []);

        return new IdlMember(name, collection, new IdlMemberMetadata(isKey, isOptional, memberId));
    }

    private static bool IsOptionalScalar(IdlType type) => type switch
    {
        IdlType.Primitive or IdlType.StringType or IdlType.Enum => true,
        IdlType.Alias alias => IsOptionalScalar(alias.Target),
        _ => false
    };

    /// <summary>Finds the closing brace matching an opening brace.</summary>
    private static int FindClosingBrace(string text, int openingBrace)
    {
        var depth = 0;
        for (var index = openingBrace; index < text.Length; index++)
        {
            if (text[index] == '{')
            {
                depth++;
            }

            if (text[index] == '}' && --depth == 0)
            {
                return index;
            }
        }

        return -1;
    }
}
