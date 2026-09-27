using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Wireloom.Compiler.Semantics;

using static Wireloom.IdlCompiler;
using static Wireloom.IdlGrammar;

namespace Wireloom;

internal sealed class IdlDeclarationParser
{
    private readonly IdlTypeResolver typeResolver;
    private readonly IdlSemanticValidator validator;
    private readonly IdlSymbolTable symbols;
    private readonly List<IdlDeclaration> declarationQueue = [];
    private readonly Dictionary<string, IdlClassDeclaration> classes = new(StringComparer.Ordinal);
    private readonly CancellationToken cancellationToken;
    private readonly ICollection<IdlDiagnostic>? diagnostics;

    internal IdlDeclarationParser(IdlSymbolTable symbols, CancellationToken cancellationToken, ICollection<IdlDiagnostic>? diagnostics = null)
    {
        this.symbols = symbols;
        typeResolver = new IdlTypeResolver(symbols);
        validator = new IdlSemanticValidator(symbols);
        this.cancellationToken = cancellationToken;
        this.diagnostics = diagnostics;
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

            var languageBindingAnnotation = LanguageBindingAnnotationPattern.Match(declarations[position..]);
            if (languageBindingAnnotation.Success)
            {
                position += languageBindingAnnotation.Length;
                continue;
            }

            var transferModeAnnotation = TransferModeAnnotationPattern.Match(declarations[position..]);
            if (transferModeAnnotation.Success)
            {
                position += transferModeAnnotation.Length;
                continue;
            }

            var dataRepresentationAnnotation = DataRepresentationAnnotationPattern.Match(declarations[position..]);
            if (dataRepresentationAnnotation.Success)
            {
                position += dataRepresentationAnnotation.Length;
                continue;
            }

            var allowedDataRepresentationAnnotation = AllowedDataRepresentationAnnotationPattern.Match(declarations[position..]);
            if (allowedDataRepresentationAnnotation.Success)
            {
                position += allowedDataRepresentationAnnotation.Length;
                continue;
            }

            var defaultNested = DefaultNestedAnnotationPattern.Match(declarations.Substring(position));
            if (defaultNested.Success)
            {
                position += defaultNested.Length;
                continue;
            }

            var unknownAnnotation = UnknownAnnotationPattern.Match(declarations[position..]);
            if (unknownAnnotation.Success && !KnownDeclarationAnnotationNames.Contains(unknownAnnotation.Groups["name"].Value))
            {
                var name = unknownAnnotation.Groups["name"].Value;
                if (UnsupportedAnnotationNames.Contains(name))
                {
                    diagnostics?.Add(new IdlDiagnostic(
                        "DDSG0102",
                        input,
                        baseOffset + position,
                        $"Annotation '{name}' is recognized but unsupported and will be ignored."));
                    throw new IdlException(
                        input,
                        baseOffset + position,
                        $"Annotation '@{name}' is not supported in this context.");
                }

                diagnostics?.Add(new IdlDiagnostic(
                    "DDSG0101",
                    input,
                    baseOffset + position,
                    $"Annotation '@{name}' is not recognized and will be ignored."));
                position += unknownAnnotation.Length;
                continue;
            }

            if (TryParseInterface(declarations, input, baseOffset, ref position) ||
                TryParseModule(declarations, input, baseOffset, currentNamespace, ref position) ||
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

    private static readonly HashSet<string> UnsupportedAnnotationNames =
        ["position", "bit_bound", "service"];

    private static readonly HashSet<string> KnownDeclarationAnnotationNames =
    [
        "topic", "autoid", "nested", "final", "appendable", "mutable",
        "language_binding", "transfer_mode", "data_representation",
        "allowed_data_representation", "default_nested"
    ];

    private int ParseStruct(string declarations, IdlInput input, int baseOffset, string? currentNamespace, int position)
    {
        var isTopic = TopicAnnotationPattern.IsMatch(declarations[position..]);
        var topicLength = isTopic ? TopicAnnotationPattern.Match(declarations[position..]).Length : 0;
        var autoId = AutoIdAnnotationPattern.Match(declarations[(position + topicLength)..]);
        var declarationStart = isTopic
            ? position + TopicAnnotationPattern.Match(declarations[position..]).Length
            : position;
        if (autoId.Success)
        {
            declarationStart += autoId.Length;
        }

        var declaration = StructPattern.Match(declarations.Substring(declarationStart));
        if (!declaration.Success)
        {
            var remaining = declarations[declarationStart..].TrimStart();
            var tokenEnd = remaining.IndexOfAny([' ', '\t', '\r', '\n', '{', ';']);
            var token = tokenEnd < 0 ? remaining : remaining[..tokenEnd];
            throw new IdlException(input, baseOffset + declarationStart, $"Unsupported IDL syntax near '{token}'. This generator does not support that declaration or annotation.");
        }

        var name = declaration.Groups["name"].Value;
        var fullyQualifiedName = Qualify(name, currentNamespace);
        EnsureNewName(input, baseOffset + declarationStart, fullyQualifiedName);
        var body = declaration.Groups["body"];
        var extensibility = declaration.Groups["extensibility"].Value.Trim() switch
        {
            "@final" => IdlExtensibilityKind.Final,
            "@mutable" => IdlExtensibilityKind.Mutable,
            _ => IdlExtensibilityKind.Extensible
        };
        var useHashIds = autoId.Success && string.Equals(autoId.Groups["value"].Value, "HASH", StringComparison.OrdinalIgnoreCase);
        var fields = ParseStructMembers(input, body.Value, baseOffset + declarationStart + body.Index, currentNamespace, useHashIds);
        IdlSemanticValidator.ValidateMemberIds(input, baseOffset + declarationStart, fields);

        var parsedDeclaration = new IdlClassDeclaration(
            name,
            currentNamespace,
            fields,
            extensibility,
            input,
            declaration.Groups["base"].Success
                ? EscapeQualifiedIdentifier(ResolveTypeName(declaration.Groups["base"].Value, currentNamespace))
                : null,
            isTopic);
        declarationQueue.Add(parsedDeclaration);

        AddClass(fullyQualifiedName, parsedDeclaration);

        return declarationStart - position + declaration.Length;
    }

    private bool TryParseInterface(string declarations, IdlInput input, int baseOffset, ref int position)
    {
        var @interface = InterfacePattern.Match(declarations[position..]);
        if (!@interface.Success)
        {
            return false;
        }

        diagnostics?.Add(new IdlDiagnostic("DDSG0103", input, baseOffset + position, $"The interface '{@interface.Groups["name"].Value}' is ignored because it is not a DDS service."));

        position += @interface.Length;

        return true;
    }

    private List<IdlMember> ParseStructMembers(IdlInput input, string body, int sourceOffset, string? currentNamespace, bool useHashIds = false)
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

            var (Field, Length) = ParseMember(input, body, offset, sourceOffset, currentNamespace, members, useHashIds);
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
        var hasDefaultLiteral = false;
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
                discriminatorIdlType,
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

        var parsedUnion = new IdlUnion(
            unionName,
            currentNamespace,
            discriminatorIdlType,
            discriminatorIsEnum,
            branches,
            ParseExtensibility(unionDeclaration.Groups["extensibility"].Value));
        symbols.AddUnion(qualified, parsedUnion);
        declarationQueue.Add(new IdlUnionDeclaration(parsedUnion, Path.GetFileName(input.Path)));

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
        string discriminatorIdlType,
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
            foreach (Match match in Regex.Matches(branch.Groups[1].Value, @"case\s+(-?[0-9]+|'(?:\\.|[^'])'|[A-Za-z_]\w*)"))
            {
                var label = match.Groups[1].Value;
                if (discriminatorIdlType == "char" && TryParseCharacterLabel(label, out var characterLabel))
                {
                    labels.Add(label);
                    labelValues.Add(characterLabel);
                }
                else if (discriminatorIdlType == "boolean" && label is "TRUE" or "FALSE")
                {
                    var booleanLabel = label == "TRUE";
                    labels.Add(booleanLabel ? "true" : "false");
                    labelValues.Add(booleanLabel ? 1 : 0);
                }
                else if (int.TryParse(label, out var numericLabel))
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

    private static bool TryParseCharacterLabel(string label, out int value)
    {
        value = 0;
        if (label.Length < 3 || label[0] != '\'' || label[^1] != '\'')
        {
            return false;
        }

        var content = label[1..^1];
        var character = content switch
        {
            "\\n" => '\n',
            "\\r" => '\r',
            "\\t" => '\t',
            "\\\\" => '\\',
            "\\'" => '\'',
            _ when content.Length == 1 => content[0],
            _ => '\0'
        };

        if (content.Length != 1 && character == '\0')
        {
            return false;
        }

        value = character;

        return true;
    }

    private IdlMember ParseUnionBranchField(IdlInput input, string branchName, string branchType, int sourceOffset, string? currentNamespace)
    {
        if (branchType.StartsWith("sequence", StringComparison.Ordinal))
        {
            var (ElementType, Bound) = ParseSequenceType(input, sourceOffset, branchType, currentNamespace);
            var element = ResolveFieldType(ElementType, currentNamespace, input, sourceOffset);
            if (element is null)
            {
                throw new IdlException(input, sourceOffset, $"Unknown union collection element type: {ElementType}");
            }

            return new IdlMember(branchName, new IdlType.Sequence(element, Bound ?? 100));
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

        var resolved = ResolveFieldType(branchType, currentNamespace, input, sourceOffset);
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
        HashSet<string> members,
        bool useHashIds = false)
    {
        var (annotations, annotationsLength) = ParseMemberAnnotations(input, body, offset, sourceOffset);
        offset += annotationsLength;

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
        IdlType parsedType;

        if (kind.StartsWith("sequence", StringComparison.Ordinal))
        {
            var (ElementType, Bound) = ParseSequenceType(input, memberSourceOffset, kind, currentNamespace);

            var element = ResolveFieldType(ElementType, currentNamespace, input, memberSourceOffset);
            if (element is null)
            {
                throw new IdlException(input, memberSourceOffset, $"Unknown collection element type: {ElementType}");
            }

            parsedType = new IdlType.Sequence(element, Bound, dimensions);
        }
        else if (dimensions.Count > 0)
        {
            var element = ResolveFieldType(kind, currentNamespace, input, memberSourceOffset);
            if (element is null)
            {
                throw new IdlException(input, memberSourceOffset, $"Unknown collection element type: {kind}");
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
            var resolved = ResolveFieldType(kind, currentNamespace, input, memberSourceOffset);
            if (resolved is null)
            {
                throw new IdlException(input, memberSourceOffset, $"Unknown struct type: {kind}");
            }

            if (annotations.IsOptional && !IsOptionalScalar(resolved))
            {
                throw new IdlException(input, memberSourceOffset, "Optional aggregate members are not supported yet.");
            }

            parsedType = resolved;
        }

        if (parsedType is IdlType.Sequence { Dimensions.Count: > 0 })
        {
            diagnostics?.Add(new IdlDiagnostic(
                "DDSG0105",
                input,
                memberSourceOffset,
                $"The C# binding does not support arrays of sequences without using a typedef; generated code for member '{field}' may not match IDL semantics."));
        }

        var valueMetadata = ResolveMemberValueMetadata(input, memberSourceOffset, currentNamespace, parsedType, annotations);
        var memberId = annotations.MemberId;
        var memberIdHashSource = annotations.HashIdExpression is not null
            ? string.IsNullOrEmpty(annotations.HashIdExpression) ? field : annotations.HashIdExpression
            : useHashIds ? field : null;
        var usesAutoIdHash = useHashIds && annotations.MemberId is null && annotations.HashIdExpression is null;
        if (memberId is null && memberIdHashSource is not null)
        {
            memberId = ComputeHashMemberId(memberIdHashSource);
        }

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

    private (MemberAnnotationState Annotations, int Length) ParseMemberAnnotations(
        IdlInput input,
        string body,
        int offset,
        int sourceOffset)
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @key annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @optional annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate or invalid @id annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate or conflicting @id/@hashid annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @min or @range annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @max or @range annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @min, @max, or @range annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @default annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @unit annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @external annotation.");
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
                    throw new IdlException(input, sourceOffset + offset, "Duplicate @must_understand annotation.");
                }

                annotations.IsMustUnderstand = true;
                offset += mustUnderstandAnnotation.Length;
                length += mustUnderstandAnnotation.Length;

                continue;
            }

            return (annotations, length);
        }
    }

    private IdlMemberValueMetadata? ResolveMemberValueMetadata(
        IdlInput input,
        int offset,
        string? currentNamespace,
        IdlType type,
        MemberAnnotationState annotations)
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
            throw new IdlException(input, offset, "@min, @max, @range, and @default are only supported on primitive and enum members.");
        }

        BigInteger? minimum = null;
        BigInteger? maximum = null;
        BigInteger? defaultValue = null;

        if (annotations.MinimumExpression is not null || annotations.MaximumExpression is not null)
        {
            if (valueType is not IdlType.Primitive primitive)
            {
                throw new IdlException(input, offset, "@min, @max, and @range require a primitive member.");
            }

            minimum = annotations.MinimumExpression is null
                ? null
                : EvaluateMemberInteger(input, offset, currentNamespace, annotations.MinimumExpression, "minimum");
            maximum = annotations.MaximumExpression is null
                ? null
                : EvaluateMemberInteger(input, offset, currentNamespace, annotations.MaximumExpression, "maximum");

            if (minimum is { } minimumValue)
            {
                ValidateConstantRange(input, offset, primitive.Name, minimumValue);
            }

            if (maximum is { } maximumValue)
            {
                ValidateConstantRange(input, offset, primitive.Name, maximumValue);
            }

            if (minimum is { } lower && maximum is { } upper && lower > upper)
            {
                throw new IdlException(input, offset, "Member minimum value cannot be greater than its maximum value.");
            }
        }

        if (annotations.DefaultExpression is not null)
        {
            if (valueType is IdlType.Primitive primitive)
            {
                defaultValue = EvaluateMemberInteger(input, offset, currentNamespace, annotations.DefaultExpression, "default");
                ValidateConstantRange(input, offset, primitive.Name, defaultValue.Value);
            }
            else if (valueType is IdlType.Enum @enum)
            {
                if (!symbols.TryGetEnum(@enum.QualifiedName, out var enumDeclaration))
                {
                    throw new IdlException(input, offset, $"Unknown enum type: {@enum.QualifiedName}");
                }

                var defaultName = annotations.DefaultExpression.Replace("::", ".").Split('.').Last();
                var enumMember = enumDeclaration.Members.SingleOrDefault(member => member.Name == defaultName);
                if (enumMember is null)
                {
                    throw new IdlException(input, offset, $"Unknown default enum value: {annotations.DefaultExpression}");
                }

                defaultValue = enumMember.Value;
            }
        }

        if (defaultValue is { } value && ((minimum is { } defaultLower && value < defaultLower) || (maximum is { } defaultUpper && value > defaultUpper)))
        {
            throw new IdlException(input, offset, "Member default value is outside its declared range.");
        }

        return new IdlMemberValueMetadata(defaultValue, minimum, maximum, annotations.DefaultExpression, annotations.UnitExpression);
    }

    private BigInteger EvaluateMemberInteger(IdlInput input, int offset, string? currentNamespace, string expression, string valueName)
    {
        try
        {
            return IdlConstantExpressionEvaluator.Evaluate(expression, symbols, currentNamespace);
        }
        catch (FormatException exception)
        {
            throw new IdlException(input, offset, $"Invalid {valueName} expression: {exception.Message}");
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

    private void EnsureNewName(IdlInput input, int offset, string name) =>
        validator.EnsureNewName(input, offset, name);

    private string Qualify(string name, string? currentNamespace) =>
        currentNamespace is null ? name : $"{currentNamespace}.{name}";

    private IdlType? ResolveFieldType(string idlType, string? currentNamespace, IdlInput input, int offset) =>
        typeResolver.Resolve(idlType, currentNamespace, input, offset);

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
