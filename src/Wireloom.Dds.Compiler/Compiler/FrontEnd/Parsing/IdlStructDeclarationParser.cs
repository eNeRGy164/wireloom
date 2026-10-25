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

    internal int Parse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, int position, bool inheritedHashIds = false, IdlAnnotationApplicationSyntax? localAutoId = null)
    {
        var prefix = IdlTypeAnnotationPrefix.Read(declarations, position, input, context, baseOffset, inheritedHashIds, localAutoId is not null);
        var isTopic = prefix.IsTopic;
        var declarationStart = prefix.DeclarationStart;
        var useHashIds = prefix.UseHashIds;

        var remainingDeclarations = declarations[declarationStart..];
        var forwardDeclaration = StructForwardPattern.Match(remainingDeclarations);
        if (forwardDeclaration.Success)
        {
            if (isTopic || prefix.Extensibility is not null || prefix.UnknownApplications.Count > 0 || localAutoId is not null)
            {
                throw new IdlException(input, context.MapOffset(baseOffset + declarationStart), "Annotations are not supported on struct forward declarations.");
            }

            var forwardName = forwardDeclaration.Groups["name"].Value;
            var qualifiedForwardName = context.Qualify(forwardName, currentNamespace);

            context.RecordIdentifier(input, baseOffset + declarationStart + forwardDeclaration.Groups["name"].Index, forwardName, currentNamespace);
            context.EnsureForwardStruct(input, baseOffset + declarationStart, qualifiedForwardName);

            return declarationStart + forwardDeclaration.Length - position;
        }

        var declaration = StructPattern.Match(remainingDeclarations);
        if (!declaration.Success)
        {
            var remaining = declarations[declarationStart..].TrimStart();
            var tokenEnd = remaining.IndexOfAny([' ', '\t', '\r', '\n', '{', ';']);
            var token = tokenEnd < 0 ? remaining : remaining[..tokenEnd];
            throw new IdlException(input, context.MapOffset(baseOffset + declarationStart), $"Unsupported IDL syntax near '{token}'. This generator does not support that declaration or annotation.");
        }

        context.ReportUnknownTypePrefixApplications(input, prefix.UnknownApplications);

        var name = declaration.Groups["name"].Value;
        var fullyQualifiedName = context.Qualify(name, currentNamespace);
        context.RecordIdentifier(input, baseOffset + declarationStart + declaration.Groups["name"].Index, name, currentNamespace);

        if (declaration.Groups["kind"].Value.Equals("struct", StringComparison.OrdinalIgnoreCase))
        {
            context.EnsureStructDefinitionName(input, baseOffset + declarationStart, fullyQualifiedName);
        }
        else
        {
            context.EnsureNewName(input, baseOffset + declarationStart, fullyQualifiedName);
        }

        context.EnsureGeneratedCompanionNames(input, baseOffset + declarationStart, name, currentNamespace, includeUnmanaged: true);

        var openingBrace = declarationStart + declaration.Length - 1;
        var closingBrace = IdlSyntaxReader.FindClosingDelimiter(declarations, openingBrace, '{', '}');
        if (closingBrace < 0)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + declarationStart), "Unterminated struct declaration.");
        }

        var end = closingBrace + 1;
        while (end < declarations.Length && char.IsWhiteSpace(declarations[end]))
        {
            end++;
        }

        if (end >= declarations.Length || declarations[end] != ';')
        {
            throw new IdlException(input, context.MapOffset(baseOffset + closingBrace), "Struct declaration must end with a semicolon.");
        }

        var body = declarations.Substring(openingBrace + 1, closingBrace - openingBrace - 1);
        var extensibility = prefix.Extensibility ?? IdlExtensibilityKind.Extensible;
        var fields = ParseMembers(input, body, baseOffset + openingBrace + 1, currentNamespace, fullyQualifiedName, useHashIds);
        var incompleteValueMember = fields.FirstOrDefault(field =>
            field.Metadata is { IsOptional: false, IsExternal: false }
            && context.IsIncompleteForwardValueType(field.Type));
        if (incompleteValueMember is not null)
        {
            throw new IdlException(
                incompleteValueMember.SourceInput ?? input,
                incompleteValueMember.SourceOffset,
                $"Forward-declared struct member '{incompleteValueMember.Name}' must use a sequence, @optional, or @external indirection before the type is defined.");
        }

        var validationOffset = context.MapOffset(baseOffset + declarationStart);
        var baseType = declaration.Groups["base"].Success
            ? NormalizeIdlType(declaration.Groups["base"].Value)
            : null;
        var baseTypeOffset = declaration.Groups["base"].Success
            ? context.MapOffset(baseOffset + declarationStart + declaration.Groups["base"].Index)
            : validationOffset;
        context.DeferGeneratedNameCollisionValidation(input, validationOffset, name, fields, baseType);

        var parsedDeclaration = new IdlClassDeclaration(
            name,
            currentNamespace,
            fields,
            extensibility,
            input,
            baseType,
            baseTypeOffset,
            isTopic);

        if (localAutoId is not null)
        {
            parsedDeclaration.SetAnnotationApplications([localAutoId]);
        }

        context.DeferMemberIdValidation(input, validationOffset, parsedDeclaration);
        context.Declarations.Add(parsedDeclaration);

        context.AddClass(fullyQualifiedName, parsedDeclaration);

        return end + 1 - position;
    }

    private List<IdlMember> ParseMembers(IdlInput input, string body, int sourceOffset, string? currentNamespace, string memberScope, bool useHashIds = false)
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

            var (Fields, Length) = context.TypeParser.ParseMember(input, body, offset, sourceOffset, currentNamespace, members, useHashIds, memberScope);
            fields.AddRange(Fields);
            offset += Length;
        }

        return fields;
    }
}
