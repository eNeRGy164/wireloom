using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

internal sealed partial class IdlDeclarationParser
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
        classes.TryGetValue(name, out declaration);

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
                    diagnostics?.Add(new IdlDiagnostic("DDSG0102", input, baseOffset + position, $"Annotation '{name}' is recognized but unsupported and will be ignored."));
                    throw new IdlException(
                        input,
                        baseOffset + position,
                        $"Annotation '@{name}' is not supported in this context.");
                }

                diagnostics?.Add(new IdlDiagnostic("DDSG0101", input, baseOffset + position, $"Annotation '@{name}' is not recognized and will be ignored."));
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
