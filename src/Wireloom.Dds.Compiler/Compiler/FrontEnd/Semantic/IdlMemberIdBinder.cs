namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Resolves aggregate member IDs after constants and inheritance have been bound.</summary>
internal static class IdlMemberIdBinder
{
    /// <summary>Assigns effective IDs while preserving explicit and hash annotation metadata.</summary>
    internal static void Bind(IEnumerable<IdlClassDeclaration> declarations)
    {
        foreach (var declaration in declarations)
        {
            long nextSequentialId = 0;
            foreach (var field in declaration.InheritedFields.Concat(declaration.Fields))
            {
                if (field.Metadata.MemberId is null && nextSequentialId > int.MaxValue)
                {
                    throw new IdlException(field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                        "Sequential member ID exceeds Int32.");
                }

                var memberId = field.Metadata.MemberId ?? (int)nextSequentialId;
                field.Metadata.SetEffectiveMemberId(memberId);
                if (field.Metadata.MemberIdHashSource is null)
                {
                    nextSequentialId = (long)memberId + 1;
                }
            }
        }
    }
}
