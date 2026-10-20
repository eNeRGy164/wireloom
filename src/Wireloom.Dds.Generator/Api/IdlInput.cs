namespace Wireloom;

/// <summary>
/// Represents one IDL file available to the compiler.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="IdlInput"/> class.
/// </remarks>
/// <param name="path">The absolute or project-relative path of the input.</param>
/// <param name="text">The complete IDL source text.</param>
/// <param name="generate">
/// Whether this file is a generation root. Files that are not roots can be
/// resolved through an include directive.
/// </param>
/// <param name="strict">
/// Whether RTI-compatible strict semantic validation is enabled for the
/// generation batch.
/// </param>
/// <param name="defines">Initial preprocessor symbols supplied by the build.</param>
/// <param name="undefines">Initial preprocessor symbols removed by the build.</param>
/// <param name="includeDirectories">Directories searched for non-relative includes.</param>
public sealed class IdlInput(
    string path,
    string text,
    bool generate = true,
    bool strict = false,
    List<string>? defines = null,
    List<string>? undefines = null,
    List<string>? includeDirectories = null)
{
    /// <summary>Initializes an IDL input with an explicit direct sequence-array mapping.</summary>
    /// <param name="path">The absolute or project-relative input path.</param>
    /// <param name="text">The complete IDL source text.</param>
    /// <param name="sequenceArrayMapping">The mapping for declarations physically in this file.</param>
    /// <param name="generate">Whether this file is a generation root.</param>
    /// <param name="strict">Whether strict validation is enabled for the generation batch.</param>
    /// <param name="defines">Initial preprocessor symbols.</param>
    /// <param name="undefines">Preprocessor symbols to remove.</param>
    /// <param name="includeDirectories">Directories searched for includes.</param>
    public IdlInput(string path, string text, IdlSequenceArrayMapping sequenceArrayMapping,
        bool generate = true, bool strict = false, List<string>? defines = null,
        List<string>? undefines = null, List<string>? includeDirectories = null)
        : this(path, text, generate, strict, defines, undefines, includeDirectories)
    {
        SequenceArrayMapping = sequenceArrayMapping;
    }

    /// <summary>Gets the direct sequence-array mapping for declarations physically in this file.</summary>
    public IdlSequenceArrayMapping SequenceArrayMapping { get; } = IdlSequenceArrayMapping.RtiCSharp;

    /// <summary>
    /// Gets the absolute or project-relative path of the input.
    /// </summary>
    public string Path { get; } = path;

    /// <summary>
    /// Gets the complete IDL source text.
    /// </summary>
    public string Text { get; } = text;

    /// <summary>
    /// Gets a value indicating whether this file is a generation root.
    /// </summary>
    public bool Generate { get; } = generate;

    /// <summary>
    /// Gets a value indicating whether strict RTI-compatible validation is
    /// enabled for this generation root.
    /// </summary>
    public bool Strict { get; } = strict;

    /// <summary>
    /// Gets preprocessor symbols supplied by the build for this input.
    /// </summary>
    public List<string> Defines { get; } = defines ?? [];

    /// <summary>
    /// Gets preprocessor symbols removed from the initial symbol set.
    /// </summary>
    public List<string> Undefines { get; } = undefines ?? [];

    /// <summary>
    /// Gets directories searched after the including file's directory when
    /// resolving an IDL include.
    /// </summary>
    public List<string> IncludeDirectories { get; } = includeDirectories ?? [];
}
