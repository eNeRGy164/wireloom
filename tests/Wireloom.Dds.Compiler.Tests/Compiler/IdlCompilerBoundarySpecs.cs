using Microsoft.CodeAnalysis.CSharp;
using Wireloom.Compiler.Naming;
using Wireloom.Compiler.Emission.Planning;

namespace Wireloom.Compiler.Tests;

public sealed class IdlCompilerBoundarySpecs
{
    [Fact]
    public void AllowsEmptySuccessfulCompilations()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = Array.Empty<IdlInput>();

        // Act
        var result = IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeTrue();
        result.Documents.ShouldBeEmpty();
        result.Diagnostics.ShouldBeEmpty();
    }

    [Fact]
    public void ReturnsFatalErrorsTogetherWithAnEmptyDocumentList()
    {
        // Arrange
        const string source = "#include \"missing.idl\"\n";
        IReadOnlyList<IdlInput> inputs = [new("root.idl", source)];

        // Act
        var result = IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeFalse();
        result.Documents.ShouldBeEmpty();
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe("DDSG0001");
        diagnostic.Severity.ShouldBe(IdlDiagnosticSeverity.Error);
        diagnostic.Path.ShouldBe("root.idl");
        diagnostic.SourceText.ShouldBe(source);
        diagnostic.Offset.ShouldBe(0);
        diagnostic.Message.ShouldContain("missing.idl");
    }

    [Fact]
    public void ReturnsUnsupportedProfileErrorsWithoutThrowing()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "struct Sample { @must_understand long value; };")];

        // Act
        var result = IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti731, CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeFalse();
        result.Documents.ShouldBeEmpty();
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe("DDSG0004");
        diagnostic.Severity.ShouldBe(IdlDiagnosticSeverity.Error);
    }

    [Theory]
    [InlineData("7.3.1", IdlCompilerProfile.Rti731)]
    [InlineData("7.6.9", IdlCompilerProfile.Rti731)]
    [InlineData("7.7.0", IdlCompilerProfile.Rti770)]
    [InlineData("8.0.0", IdlCompilerProfile.Rti770)]
    public void MapsRuntimeVersionsToTheNearestSupportedProfile(string version, IdlCompilerProfile expected)
    {
        // Arrange
        var runtimeVersion = Version.Parse(version);

        // Act
        var result = IdlCompiler.GetProfileForRuntimeVersion(runtimeVersion);

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void RejectsRuntimeVersionsBelowTheMinimumProfile()
    {
        // Arrange
        var runtimeVersion = Version.Parse("7.3.0");

        // Act
        var exception = Should.Throw<ArgumentOutOfRangeException>(() => IdlCompiler.GetProfileForRuntimeVersion(runtimeVersion));

        // Assert
        exception.ParamName.ShouldBe("runtimeVersion");
    }

    [Fact]
    public void RejectsNullRuntimeVersions()
    {
        // Arrange

        // Act
        var exception = Should.Throw<ArgumentNullException>(() => IdlCompiler.GetProfileForRuntimeVersion(null!));

        // Assert
        exception.ParamName.ShouldBe("runtimeVersion");
    }

    [Fact]
    public void RejectsNullInputCollections()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = null!;

        // Act
        var exception = Should.Throw<ArgumentNullException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5));

        // Assert
        exception.ParamName.ShouldBe("inputs");
    }

    [Fact]
    public void RejectsUnknownProfiles()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [];

        // Act
        var exception = Should.Throw<ArgumentOutOfRangeException>(() =>
            IdlCompiler.Compile(inputs, (IdlCompilerProfile)int.MaxValue, CompilerTestSupport.ComputeMd5));

        // Assert
        exception.ParamName.ShouldBe("profile");
    }

    [Fact]
    public void RequiresMd5ProviderEvenWhenThereAreNoHashIds()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = Array.Empty<IdlInput>();

        // Act
        var exception = Should.Throw<ArgumentNullException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, null!));

        // Assert
        exception.ParamName.ShouldBe("computeMd5");
    }

    [Fact]
    public void UsesTheInjectedMd5ProviderForMemberIds()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "struct Sample { @hashid(\"é\") long value; };")];
        byte[]? received = null;

        // Act
        var result = IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, bytes =>
        {
            received = bytes;
            var digest = new byte[16];
            digest[0] = 0x34;
            digest[1] = 0x12;
            return digest;
        }, TestContext.Current.CancellationToken);

        // Assert
        received.ShouldBe("é"u8.ToArray());
        result.Success.ShouldBeTrue();
        result.Documents.ShouldContain(document => document.Source.Contains("id: 4660", StringComparison.Ordinal));
    }

    [Fact]
    public void RejectsAnInvalidMd5DigestWithoutReturningAnIdlDiagnostic()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "@autoid(HASH) struct Sample { long value; };")];

        // Act
        var exception = Should.Throw<InvalidOperationException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, static _ => new byte[15]));

        // Assert
        exception.Message.ShouldContain("exactly 16");
    }

    [Fact]
    public void RejectsANullMd5Digest()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "@autoid(HASH) struct Sample { long value; };")];

        // Act
        var exception = Should.Throw<InvalidOperationException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, static _ => null!));

        // Assert
        exception.Message.ShouldContain("exactly 16");
    }

    [Fact]
    public void WrapsProviderExceptionsSoTheyAreNotReportedAsIdlDiagnostics()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "@autoid(HASH) struct Sample { long value; };")];
        var providerException = new IdlException(new IdlInput("provider.idl", string.Empty), 0, "provider failure");

        // Act
        var exception = Should.Throw<InvalidOperationException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, _ => throw providerException));

        // Assert
        exception.InnerException.ShouldBeSameAs(providerException);
    }

    [Fact]
    public void PropagatesCancellationFromTheMd5Provider()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "@autoid(HASH) struct Sample { long value; };")];
        var cancellationToken = TestContext.Current.CancellationToken;

        // Act
        var exception = Should.Throw<OperationCanceledException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770,
                _ => throw new OperationCanceledException(cancellationToken), cancellationToken));

        // Assert
        exception.CancellationToken.ShouldBe(cancellationToken);
    }

    [Fact]
    public void CopiesPreprocessorCollectionsAtInputConstruction()
    {
        // Arrange
        var defines = new List<string> { "FIRST" };
        var undefines = new List<string> { "OLD" };
        var includeDirectories = new List<string> { "includes" };

        // Act
        var input = new IdlInput("root.idl", string.Empty, defines: defines, undefines: undefines,
            includeDirectories: includeDirectories);
        defines.Add("LATER");
        undefines.Clear();
        includeDirectories.Clear();

        // Assert
        input.Defines.ShouldBe(["FIRST"]);
        input.Undefines.ShouldBe(["OLD"]);
        input.IncludeDirectories.ShouldBe(["includes"]);
        Should.Throw<NotSupportedException>(() => ((IList<string>)input.Defines).Add("MUTATION"));
    }

    [Fact]
    public void PropagatesCancellationInsteadOfReturningADiagnostic()
    {
        // Arrange
        IReadOnlyList<IdlInput> inputs = [new("sample.idl", "struct Sample { long value; };")];
        using var source = new CancellationTokenSource();
        source.Cancel();

        // Act
        var exception = Should.Throw<OperationCanceledException>(() =>
            IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti770, CompilerTestSupport.ComputeMd5, source.Token));

        // Assert
        exception.CancellationToken.ShouldBe(source.Token);
    }

    [Fact]
    public void CoreKeywordAndLiteralFormattingMatchesRoslynAcrossUtf16()
    {
        // Arrange
        var keywordSpellings = Enum.GetValues<SyntaxKind>()
            .Where(kind => kind.ToString().EndsWith("Keyword", StringComparison.Ordinal))
            .Select(SyntaxFacts.GetText)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        // Act and Assert
        foreach (var keyword in keywordSpellings)
        {
            CSharpIdentifier.IsReservedKeyword(keyword)
                .ShouldBe(SyntaxFacts.IsReservedKeyword(SyntaxFacts.GetKeywordKind(keyword)), keyword);
        }

        var allUtf16CodeUnits = new string(Enumerable.Range(char.MinValue, char.MaxValue + 1)
            .Select(codeUnit => (char)codeUnit).ToArray());
        CSharpLiteralFormatter.FormatString(allUtf16CodeUnits)
            .ShouldBe(SymbolDisplay.FormatLiteral(allUtf16CodeUnits, quote: true));

        var characterLiteralSamples = new[]
        {
            '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v', '\\', '"', '\'',
            '\u001f', '\u007f', '\u0378', '\u2028', '\u2029', '\ud800', '\udfff'
        };
        foreach (var character in characterLiteralSamples)
        {
            CSharpLiteralFormatter.FormatCharacter(character)
                .ShouldBe(SymbolDisplay.FormatLiteral(character, quote: true), $"U+{(int)character:X4} character");
            CSharpLiteralFormatter.FormatString(character.ToString())
                .ShouldBe(SymbolDisplay.FormatLiteral(character.ToString(), quote: true), $"U+{(int)character:X4} string");
        }

        var supplementaryCharacter = char.ConvertFromUtf32(0x1F600);
        CSharpLiteralFormatter.FormatString(supplementaryCharacter)
            .ShouldBe(SymbolDisplay.FormatLiteral(supplementaryCharacter, quote: true));
    }
}
