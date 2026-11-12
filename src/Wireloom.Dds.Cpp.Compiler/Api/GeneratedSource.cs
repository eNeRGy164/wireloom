namespace Wireloom.Dds.Cpp;

/// <summary>Represents one generated Modern C++ source or header file.</summary>
/// <param name="FileName">The file name within the consumer output directory.</param>
/// <param name="Source">The complete generated C++ text.</param>
[PublicAPI]
public sealed record GeneratedCppSource(string FileName, string Source);
