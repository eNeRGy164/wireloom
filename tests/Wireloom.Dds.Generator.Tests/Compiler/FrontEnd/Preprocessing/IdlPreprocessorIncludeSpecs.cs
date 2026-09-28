using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlPreprocessorIncludeSpecs : IdlPreprocessorTestBase
{
    [Fact]
    [Trait("Preprocessor", "PP031")]
    public void ReportsThatIncludeFilenamesNeedQuotesOrAngleBrackets()
    {
        // Arrange
        var input = Input("unquoted-include.idl",
            """
            #include CommonHeader.idl
            struct Sample { long value; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("expected a quoted or angle-bracket filename");
        exception.Offset.ShouldBe(0);
        exception.Input.Path.ShouldEndWith("unquoted-include.idl");
    }

    [Fact]
    [Trait("Preprocessor", "PP032")]
    public void ReportsAngleBracketIncludeFilenames()
    {
        // Arrange
        var includeName = string.Empty;
        var includeIsAngle = false;
        var includeOffset = -1;
        var includePreprocessor = new IdlPreprocessor([], []);
        var includeInput = Input("include.idl", """#include <Common.idl>""");

        // Act
        includePreprocessor.Process(includeInput, (name, isAngle, offset) =>
        {
            includeName = name;
            includeIsAngle = isAngle;
            includeOffset = offset;
        });

        // Assert
        includeName.ShouldBe("Common.idl");
        includeIsAngle.ShouldBeTrue();
        includeOffset.ShouldBe(0);
    }

    [Fact]
    [Trait("Preprocessor", "PP043")]
    public void AcceptsImportAndUsingAsIdlIncludeForms()
    {
        // Arrange
        var names = new List<string>();
        var input = Input("imports.idl",
            """
            #import "Imported.idl"
            #using <Used.idl>
            """);
        var preprocessor = new IdlPreprocessor([], []);

        // Act
        preprocessor.Process(input, (name, _, _) => names.Add(name));

        // Assert
        names.ShouldBe(["Imported.idl", "Used.idl"]);
    }

    [Theory]
    [InlineData("#import Common.idl", "#import")]
    [InlineData("#using Common.idl", "#using")]
    [Trait("Preprocessor", "PP043")]
    public void RejectsMalformedImportAndUsingOperands(string directive, string name)
    {
        // Arrange
        var input = Input("malformed-import.idl", directive);

        // Act
        var exception = Should.Throw<IdlException>(() => new IdlPreprocessor([], []).Process(input, (_, _, _) => { }));

        // Assert
        exception.Message.ShouldContain($"Malformed {name} directive");
    }
}
