namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Applies the batch validation mode to specification findings.</summary>
internal static class IdlValidationPolicy
{
    /// <summary>Reports a nonfatal finding or rejects it before emission.</summary>
    /// <param name="violation">The mode-independent semantic finding.</param>
    /// <param name="strict">Whether strict validation applies to this batch.</param>
    /// <param name="diagnostics">The optional destination for nonfatal notices.</param>
    internal static void Apply(IdlSpecificationViolation violation, bool strict, ICollection<IdlDiagnostic>? diagnostics)
    {
        if (violation.Rule == IdlSpecificationRule.DerivedKey && !strict)
        {
            return;
        }

        var message = violation.Rule switch
        {
            IdlSpecificationRule.ReservedKeyword => $"IDL keyword '{violation.Name}' cannot be used as an identifier.",
            IdlSpecificationRule.KeywordCaseCollision => $"IDL identifier '{violation.Name}' conflicts with keyword '{violation.ConflictingName}' ignoring case.",
            IdlSpecificationRule.IdentifierCaseCollision => $"IDL identifier '{violation.Name}' conflicts with '{violation.ConflictingName}' in scope '{violation.Scope}' ignoring case.",
            _ => "struct/valuetype derived from a struct/valuetype can not contain @key fields. This check is only enforced when using strict validation."
        };

        if (strict || violation.Rule == IdlSpecificationRule.ReservedKeyword)
        {
            throw new IdlException(violation.Input, violation.Offset, message);
        }

        diagnostics?.Add(new IdlDiagnostic("DDSG0108", violation.Input, violation.Offset, $"{message} Strict validation rejects this declaration."));
    }
}
