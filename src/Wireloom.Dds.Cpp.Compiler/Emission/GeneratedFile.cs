namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Represents a named Modern C++ source or header emitted by the generator.</summary>
/// <param name="FileName">The file name within the consumer's output directory.</param>
/// <param name="Source">The emitted C++ text.</param>
internal sealed record GeneratedCppFile(string FileName, string Source);
