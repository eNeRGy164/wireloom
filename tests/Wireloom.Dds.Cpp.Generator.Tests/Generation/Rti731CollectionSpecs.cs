namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Checks collection metadata supported by the RTI 7.3.1 C++ profile.</summary>
public sealed class Rti731CollectionSpecs
{
    private sealed class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("wireloom-cpp-rti731-tests-");

        internal string Path => directory.FullName;

        public void Dispose() => directory.Delete(recursive: true);
    }

    /// <summary>Emits optional primitive collections for arbitrary member names, IDs, and bounds.</summary>
    [Theory]
    [InlineData("sequence<short, 4> readings", "readings")]
    [InlineData("sequence<long, 5> readings", "readings")]
    [InlineData("long coordinates[3]", "coordinates")]
    public void EmitsOptionalPrimitiveCollectionsForRti731(string memberDeclaration, string memberName)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "measurement.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, $"struct Measurement {{ @id(37) @optional {memberDeclaration}; }};");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();
        var plugin = File.ReadAllText(Path.Combine(outputPath, "measurementPlugin.cxx"));
        plugin.ShouldContain($"Member(\"{memberName}\"");
        plugin.ShouldContain(".id(37)\n                .optional(true));");
    }
}
