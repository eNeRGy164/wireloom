using System.Security.Cryptography;

namespace Wireloom.Dds.Cpp.Compiler.Tests;

/// <summary>Verifies the public C++ compiler host contract.</summary>
public sealed class CppCompilerApiSpecs
{
    /// <summary>Compiles a request into C++ documents through the public facade.</summary>
    [Theory]
    [InlineData(CppCompilerProfile.Rti612)]
    [InlineData(CppCompilerProfile.Rti731)]
    [InlineData(CppCompilerProfile.Rti770)]
    public void CompilesSharedRequestToCppDocuments(CppCompilerProfile profile)
    {
        // Arrange
        var request = new IdlCompilationRequest(
            [new IdlInput("sample.idl", "struct Sample { long value; };")],
            MD5.HashData);

        // Act
        var result = CppCompiler.Compile(request, profile);

        // Assert
        result.Success.ShouldBeTrue();
        result.Documents.Select(document => document.FileName).ShouldBe([
            "sample.hpp", "sample.cxx", "samplePlugin.hpp", "samplePlugin.cxx"]);
        result.Diagnostics.ShouldBeEmpty();
    }

    /// <summary>Maps runtime versions through the C++ facade profile contract.</summary>
    [Theory]
    [InlineData("6.1.2", CppCompilerProfile.Rti612)]
    [InlineData("7.3.1", CppCompilerProfile.Rti731)]
    [InlineData("7.7.0", CppCompilerProfile.Rti770)]
    public void MapsRuntimeVersionToProfile(string runtimeVersion, CppCompilerProfile expected)
    {
        // Arrange
        var version = Version.Parse(runtimeVersion);

        // Act
        var actual = CppCompiler.GetProfileForRuntimeVersion(version);

        // Assert
        actual.ShouldBe(expected);
    }

    /// <summary>Rejects a null runtime version before selecting a compiler profile.</summary>
    [Fact]
    public void RejectsNullRuntimeVersion()
    {
        // Arrange
        Version? version = null;

        // Act
        Action getProfile = () => CppCompiler.GetProfileForRuntimeVersion(version!);

        // Assert
        getProfile.ShouldThrow<ArgumentNullException>();
    }

    /// <summary>Rejects runtime versions below the minimum supported C++ profile.</summary>
    [Fact]
    public void RejectsRuntimeVersionBelowMinimum()
    {
        // Arrange
        var version = new Version(6, 1, 1);

        // Act
        Action getProfile = () => CppCompiler.GetProfileForRuntimeVersion(version);

        // Assert
        getProfile.ShouldThrow<ArgumentOutOfRangeException>();
    }

    /// <summary>Rejects null compilation requests at the public C++ compiler boundary.</summary>
    [Fact]
    public void RejectsNullCompilationRequest()
    {
        // Arrange
        IdlCompilationRequest? request = null;

        // Act
        Action compile = () => CppCompiler.Compile(request!, CppCompilerProfile.Rti770);

        // Assert
        compile.ShouldThrow<ArgumentNullException>();
    }

    /// <summary>Rejects undefined C++ API profiles before converting to an emission profile.</summary>
    [Fact]
    public void RejectsUndefinedProfile()
    {
        // Arrange
        var request = new IdlCompilationRequest(
            [new IdlInput("sample.idl", "struct Sample { long value; };")],
            MD5.HashData);

        // Act
        Action compile = () => CppCompiler.Compile(request, (CppCompilerProfile)int.MaxValue);

        // Assert
        compile.ShouldThrow<ArgumentOutOfRangeException>();
    }
}
