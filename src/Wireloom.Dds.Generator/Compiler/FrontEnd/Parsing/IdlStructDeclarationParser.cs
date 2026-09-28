using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;
using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL struct and class declarations.</summary>
internal sealed class IdlStructDeclarationParser
{
    private readonly IdlParseContext context;

    internal IdlStructDeclarationParser(IdlParseContext context) =>
        this.context = context;

    internal int Parse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, int position)
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
            throw new IdlException(input, context.MapOffset(baseOffset + declarationStart), $"Unsupported IDL syntax near '{token}'. This generator does not support that declaration or annotation.");
        }

        var name = declaration.Groups["name"].Value;
        var fullyQualifiedName = context.Qualify(name, currentNamespace);
        context.EnsureNewName(input, baseOffset + declarationStart, fullyQualifiedName);
        var body = declaration.Groups["body"];
        var extensibility = declaration.Groups["extensibility"].Value.Trim() switch
        {
            "@final" => IdlExtensibilityKind.Final,
            "@mutable" => IdlExtensibilityKind.Mutable,
            _ => IdlExtensibilityKind.Extensible
        };
        var useHashIds = autoId.Success && string.Equals(autoId.Groups["value"].Value, "HASH", StringComparison.OrdinalIgnoreCase);
        var fields = ParseMembers(input, body.Value, baseOffset + declarationStart + body.Index, currentNamespace, useHashIds);
        IdlSemanticValidator.ValidateMemberIds(input, context.MapOffset(baseOffset + declarationStart), fields);

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
        context.Declarations.Add(parsedDeclaration);

        context.AddClass(fullyQualifiedName, parsedDeclaration);

        return declarationStart - position + declaration.Length;
    }

    private List<IdlMember> ParseMembers(IdlInput input, string body, int sourceOffset, string? currentNamespace, bool useHashIds = false)
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

            var (Field, Length) = context.TypeParser.ParseMember(input, body, offset, sourceOffset, currentNamespace, members, useHashIds);
            fields.Add(Field);
            offset += Length;
        }

        return fields;
    }
}
