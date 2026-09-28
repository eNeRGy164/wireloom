namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed class PreprocessedIdl(
    string text,
    bool hasPragmaOnce,
    IReadOnlyList<SourceOriginSpan> sourceOrigins)
{
    public string Text { get; } = text;

    public bool HasPragmaOnce { get; } = hasPragmaOnce;

    public IReadOnlyList<SourceOriginSpan> SourceOrigins { get; } = sourceOrigins;
}

/// <summary>Maps a contiguous generated range to a source range or invocation.</summary>
internal readonly struct SourceOriginSpan(int outputStart, int outputLength, int sourceStart, int sourceLength)
{
    public int OutputStart { get; } = outputStart;
    public int OutputLength { get; } = outputLength;
    public int SourceStart { get; } = sourceStart;
    private int SourceLength { get; } = sourceLength;

    public int Map(int outputOffset) => SourceLength == 0
        ? SourceStart
        : SourceStart + Math.Min(outputOffset - OutputStart, SourceLength - 1);
}
