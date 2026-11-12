namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Verifies the native generator's command boundary and diagnostic behavior.</summary>
public sealed class CommandSpecs
{
    /// <summary>Describes direct native invocation without requiring an IDL input.</summary>
    [Theory]
    [InlineData("--help")]
    [InlineData("-h")]
    public void DescribesNativeInvocation(string argument)
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run([argument], output, errors);

        // Assert
        exitCode.ShouldBe(0);
        output.ToString().ShouldContain("Wireloom.Dds.Cpp generate");
        output.ToString().ShouldContain("--unbounded-support");
        output.ToString().ShouldContain("--undefine");
        output.ToString().ShouldContain("--sequence-size");
        output.ToString().ShouldContain("--rti-version");
        errors.ToString().ShouldBeEmpty();
    }

    /// <summary>Distinguishes invalid command syntax from generation errors.</summary>
    [Fact]
    public void RejectsInvalidArguments()
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate"], output, errors);

        // Assert
        exitCode.ShouldBe(2);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain("Usage:");
    }

    /// <summary>Rejects unknown options and options that require a value.</summary>
    [Theory]
    [InlineData("--unexpected")]
    [InlineData("--define")]
    [InlineData("--include-directory")]
    [InlineData("--rti-version")]
    public void RejectsUnsupportedOrIncompleteOptions(string option)
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", "input.idl", "output", option], output, errors);

        // Assert
        exitCode.ShouldBe(2);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain("Usage:");
    }

    /// <summary>Rejects malformed and unsupported RTI runtime versions as usage errors.</summary>
    [Theory]
    [InlineData("not-a-version")]
    [InlineData("6.1.1")]
    [InlineData("7.7")]
    public void RejectsUnsupportedRuntimeVersion(string version)
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", "missing.idl", "unused", "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(2);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain("RTI");
        errors.ToString().ShouldContain("Usage:");
    }

    /// <summary>Rejects malformed or nonpositive command-line size values.</summary>
    [Theory]
    [InlineData("--sequence-size", "not-a-number", "Sequence size must be a positive 32-bit integer.")]
    [InlineData("--sequence-size", "0", "Sequence size must be a positive 32-bit integer.")]
    [InlineData("--string-size", "not-a-number", "String size must be a positive 32-bit integer.")]
    [InlineData("--string-size", "0", "String size must be a positive 32-bit integer.")]
    public void RejectsInvalidRootSizes(string option, string value, string expectedError)
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", "input.idl", "output", option, value], output, errors);

        // Assert
        exitCode.ShouldBe(2);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain(expectedError);
    }

    /// <summary>Rejects malformed file-specific sizes and boolean support overrides.</summary>
    [Theory]
    [InlineData("--file-sequence-size", "not-a-number", "File sizes must be positive 32-bit integers.")]
    [InlineData("--file-sequence-size", "0", "File sizes must be positive 32-bit integers.")]
    [InlineData("--file-string-size", "not-a-number", "File sizes must be positive 32-bit integers.")]
    [InlineData("--file-string-size", "0", "File sizes must be positive 32-bit integers.")]
    [InlineData("--file-unbounded-support", "sometimes", "File unbounded support must be true or false.")]
    public void RejectsInvalidFileOverrides(string option, string value, string expectedError)
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", "input.idl", "output", option, "sample.idl", value], output, errors);

        // Assert
        exitCode.ShouldBe(2);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain(expectedError);
    }

    /// <summary>Emits the legacy RTI 6.1.2 plugin path for a direct scalar struct.</summary>
    [Fact]
    public void GeneratesScalarStructPluginForRti612()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-rti612-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "sample.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath, """
            struct Sample {
                long value;
            };
            """);
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", inputPath, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("RTIXCdrSampleAccessInfo");
        plugin.ShouldContain("WireloomRti612TypePlugin");
        plugin.ShouldNotContain("GenericTypePluginFactory");
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }

    /// <summary>Emits narrow string members through the retained RTI 6.1.2 plugin.</summary>
    [Fact]
    public void GeneratesStringStructPluginForRti612()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-rti612-unsupported-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "sample.idl");
        File.WriteAllText(inputPath, "struct Sample { string text; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var outputPath = Path.Combine(root, "output");
        var exitCode = CppCommand.Run(
            ["generate", inputPath, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"))
            .ShouldContain("::dds::core::xtypes::StringType(255)");
        errors.ToString().ShouldBeEmpty();
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }

    /// <summary>Leaves matching generated files untouched on repeated runs.</summary>
    [Fact]
    public void DoesNotRewriteUnchangedGeneratedFiles()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-idempotent-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "sample.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath, "struct Sample { long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();
        var firstExitCode = CppCommand.Run(["generate", inputPath, outputPath], output, errors);
        firstExitCode.ShouldBe(0, errors.ToString());
        var generatedFile = Path.Combine(outputPath, "sample.hpp");
        var sentinelTimestamp = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(generatedFile, sentinelTimestamp);

        // Act
        var secondExitCode = CppCommand.Run(["generate", inputPath, outputPath], output, errors);

        // Assert
        secondExitCode.ShouldBe(0, errors.ToString());
        File.GetLastWriteTimeUtc(generatedFile).ShouldBe(sentinelTimestamp);
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }

    /// <summary>Reports unreadable input through the command's failure channel.</summary>
    [Fact]
    public void ReportsMissingInput()
    {
        // Arrange
        using var output = new StringWriter();
        using var errors = new StringWriter();
        var missing = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "absent.idl");

        // Act
        var exitCode = CppCommand.Run(["generate", missing, "unused"], output, errors);

        // Assert
        exitCode.ShouldBe(1);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain("Wireloom C++:");
    }

    /// <summary>Propagates opt-in unbounded semantics through standalone generation.</summary>
    [Fact]
    public void GeneratesUnboundedSupportWhenRequested()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-unbounded-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "unbounded.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath, "struct Sample { sequence<long> values; string text; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", inputPath, outputPath, "--unbounded-support"],
            output,
            errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        File.ReadAllText(Path.Combine(outputPath, "unbounded.hpp"))
            .ShouldContain("::omg::types::sequence< ::std::int32_t > values = {};");
        File.ReadAllText(Path.Combine(outputPath, "unboundedPlugin.cxx"))
            .ShouldContain("RTIXCdrLong_MAX");

        var tempRoot = Path.GetFullPath(Path.GetTempPath());
        var fullRoot = Path.GetFullPath(root);
        fullRoot.StartsWith(tempRoot, StringComparison.OrdinalIgnoreCase).ShouldBeTrue();
        Directory.Delete(fullRoot, recursive: true);
    }

    /// <summary>Applies the CMake-exposed root settings through the native command.</summary>
    [Fact]
    public void AppliesRootSettingsFromStandaloneArguments()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-root-settings-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "sample.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath, "#if defined(REMOVED)\nstruct Wrong { long value; };\n#else\nstruct _Sample { sequence<long> values; string label; };\n#endif\n");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", inputPath, outputPath, "--define", "REMOVED", "--undefine", "REMOVED", "--strict", "--finite-support", "--enable-escape-char", "--sequence-size", "12", "--string-size", "42", "--file-sequence-size", inputPath, "24", "--file-string-size", inputPath, "30", "--file-unbounded-support", inputPath, "false"],
            output,
            errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        header.ShouldContain("Sample");
        header.ShouldNotContain("Wrong");
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }

    /// <summary>Loads cyclic and repeated pragma-once includes without duplicating generated types.</summary>
    [Fact]
    public void LoadsCyclicAndRepeatedIncludesOnce()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-include-closure-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "root.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath, "#include <first.idl>\n#include <first.idl>\nstruct Root { First first; Second second; };");
        File.WriteAllText(Path.Combine(root, "first.idl"), "#pragma once\n#include \"second.idl\"\nstruct First { long value; };");
        File.WriteAllText(Path.Combine(root, "second.idl"), "#include \"first.idl\"\nstruct Second { long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", inputPath, outputPath, "--include-directory", root], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        var header = File.ReadAllText(Path.Combine(outputPath, "root.hpp"));
        header.ShouldContain("struct Root");
        header.ShouldContain("struct First");
        header.ShouldContain("struct Second");
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }

    /// <summary>Reports unresolved includes without aborting standalone input loading.</summary>
    [Fact]
    public void ReportsUnresolvedInclude()
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-missing-include-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "root.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath, "#include \"missing.idl\"\nstruct Sample { long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", inputPath, outputPath], output, errors);

        // Assert
        exitCode.ShouldBe(1);
        output.ToString().ShouldBeEmpty();
        errors.ToString().ShouldContain("include");
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }

    /// <summary>Maps 8-bit union branch TypeCodes to the selected RTI profile.</summary>
    [Theory]
    [InlineData("7.7.0", "DDS_g_tc_int8")]
    public void EmitsProfileSpecificEightBitUnionTypeCodes(string version, string typeCode)
    {
        // Arrange
        var root = Path.Combine(Path.GetTempPath(), $"wireloom-union-typecodes-{Guid.NewGuid():N}");
        Directory.CreateDirectory(root);
        var inputPath = Path.Combine(root, "sample.idl");
        var outputPath = Path.Combine(root, "output");
        File.WriteAllText(inputPath,
            "union SmallNumbers switch(long) { case 0: int8 signed_value; case 1: uint8 unsigned_value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", inputPath, outputPath, "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain(typeCode);
        plugin.ShouldContain("DDS_g_tc_uint8");
        Directory.Delete(Path.GetFullPath(root), recursive: true);
    }
}
