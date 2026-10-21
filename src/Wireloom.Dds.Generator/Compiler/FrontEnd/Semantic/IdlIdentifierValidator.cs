namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Detects IDL naming violations without choosing their severity.</summary>
internal static class IdlIdentifierValidator
{
    // Canonical spellings from OMG IDL 4.2, Table 7-6. These are IDL tokens,
    // not the target language's reserved identifiers.
    private static readonly Dictionary<string, string> keywords = CreateKeywords();

    /// <summary>Finds naming violations in deterministic declaration order.</summary>
    /// <param name="identifiers">Parsed declaration occurrences in source order.</param>
    /// <param name="cancellationToken">Cancellation for validation traversal.</param>
    /// <returns>Mode-independent findings at the offending identifier.</returns>
    internal static IEnumerable<IdlSpecificationViolation> FindViolations(IReadOnlyList<IdlIdentifierOccurrence> identifiers, CancellationToken cancellationToken)
    {
        var scopes = new Dictionary<(IdlIdentifierScopeKind Kind, string Name), Dictionary<string, IdlIdentifierOccurrence>>();

        foreach (var identifier in identifiers)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var scopeKey = (identifier.ScopeKind, identifier.Scope);
            if (!scopes.TryGetValue(scopeKey, out var names))
            {
                names = new Dictionary<string, IdlIdentifierOccurrence>(StringComparer.OrdinalIgnoreCase);
                scopes.Add(scopeKey, names);
            }

            names.TryGetValue(identifier.Name, out var previous);
            names.TryAdd(identifier.Name, identifier);
            if (keywords.TryGetValue(identifier.Name, out var keyword))
            {
                var rule = IdlSpecificationRule.KeywordCaseCollision;
                if (string.Equals(identifier.Name, keyword, StringComparison.Ordinal))
                {
                    rule = IdlSpecificationRule.ReservedKeyword;
                }
                else if (identifier.ScopeKind == IdlIdentifierScopeKind.NonServiceInterface)
                {
                    continue;
                }

                yield return new IdlSpecificationViolation(rule, identifier.Input, identifier.Offset, identifier.Name, keyword);
            }
            else if (identifier.ScopeKind != IdlIdentifierScopeKind.NonServiceInterface
                && previous is not null && !string.Equals(previous.Name, identifier.Name, StringComparison.Ordinal))
            {
                yield return new IdlSpecificationViolation(
                    IdlSpecificationRule.IdentifierCaseCollision, identifier.Input, identifier.Offset,
                    identifier.Name, previous.Name, identifier.Scope);
            }
        }
    }

    /// <summary>Builds a case-insensitive index retaining canonical keyword spelling.</summary>
    private static Dictionary<string, string> CreateKeywords()
    {
        const string names =
            "abstract any alias attribute bitfield bitmask bitset boolean case char component connector" +
            " const consumes context custom default double exception emits enum eventtype factory FALSE" +
            " finder fixed float getraises home import in inout interface local long manages" +
            " map mirrorport module multiple native Object octet oneway out primarykey private port" +
            " porttype provides public publishes raises readonly setraises sequence short string struct supports" +
            " switch TRUE truncatable typedef typeid typename typeprefix unsigned union uses ValueBase valuetype" +
            " void wchar wstring int8 uint8 int16 int32 int64 uint16 uint32 uint64";
        return names.Split(' ').ToDictionary(name => name, name => name, StringComparer.OrdinalIgnoreCase);
    }
}
