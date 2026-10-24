namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Captures member annotation arguments without resolving their values.</summary>
internal sealed class IdlMemberAnnotationSyntax
{
    /// <summary>Gets or sets whether a key annotation was declared.</summary>
    public bool HasKey { get; set; }

    /// <summary>Gets or sets the declared key flag.</summary>
    public bool IsKey { get; set; }

    /// <summary>Gets or sets the unresolved boolean key argument.</summary>
    public string? KeyExpression { get; set; }

    /// <summary>Gets or sets whether an optional annotation was declared.</summary>
    public bool HasOptional { get; set; }

    /// <summary>Gets or sets the declared optional flag.</summary>
    public bool IsOptional { get; set; }

    /// <summary>Gets or sets the unresolved boolean optional argument.</summary>
    public string? OptionalExpression { get; set; }

    /// <summary>Gets or sets the unresolved member-ID expression.</summary>
    public string? MemberIdExpression { get; set; }

    /// <summary>Gets or sets the declared hash-ID source.</summary>
    public string? HashIdExpression { get; set; }

    /// <summary>Gets or sets the unresolved minimum expression.</summary>
    public string? MinimumExpression { get; set; }

    /// <summary>Gets or sets the unresolved maximum expression.</summary>
    public string? MaximumExpression { get; set; }

    /// <summary>Gets or sets the unresolved default expression.</summary>
    public string? DefaultExpression { get; set; }

    /// <summary>Gets or sets the declared unit text.</summary>
    public string? UnitExpression { get; set; }

    /// <summary>Gets or sets whether an external annotation was declared.</summary>
    public bool HasExternal { get; set; }

    /// <summary>Gets or sets the declared external flag.</summary>
    public bool IsExternal { get; set; }

    /// <summary>Gets or sets the unresolved boolean external argument.</summary>
    public string? ExternalExpression { get; set; }

    /// <summary>Gets or sets whether a must-understand annotation was declared.</summary>
    public bool HasMustUnderstand { get; set; }

    /// <summary>Gets or sets the declared must-understand flag.</summary>
    public bool IsMustUnderstand { get; set; }

    /// <summary>Gets or sets the unresolved boolean must-understand argument.</summary>
    public string? MustUnderstandExpression { get; set; }
}
