using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlInputGraphSearchSpecs
{
    [Fact]
    [Trait("Preprocessor", "PP031")]
    public void UsesIncludingDirectoryBeforeConfiguredPathsForQuotedIncludes()
    {
        // Arrange
        var root = new IdlInput(
            Path.Combine("root", "root.idl"),
            """
            #include "common.idl"
            """,
            includeDirectories: ["configured"]);
        var local = Input(Path.Combine("root", "common.idl"), """struct LocalChoice { long value; };""", generate: false);
        var configured = Input(Path.Combine("configured", "common.idl"), """struct ConfiguredChoice { long value; };""", generate: false);

        // Act
        var output = Compile(root, local, configured);

        // Assert
        output.ShouldContain("LocalChoice");
        output.ShouldNotContain("ConfiguredChoice");
    }

    [Fact]
    [Trait("Preprocessor", "PP032")]
    public void UsesOnlyConfiguredPathsForAngleIncludes()
    {
        // Arrange
        var root = new IdlInput(Path.Combine("root", "root.idl"), """#include <common.idl>""", includeDirectories: ["configured"]);
        var local = Input(Path.Combine("root", "common.idl"), """struct LocalChoice { long value; };""", generate: false);
        var configured = Input(Path.Combine("configured", "common.idl"), """struct ConfiguredChoice { long value; };""", generate: false);

        // Act
        var output = Compile(root, local, configured);

        // Assert
        output.ShouldContain("ConfiguredChoice");
        output.ShouldNotContain("LocalChoice");
    }

    [Fact]
    [Trait("Preprocessor", "PP048")]
    public void EvaluatesHasIncludeAgainstTheInputGraph()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #if __has_include("common.idl")
            struct Found { long value; };
            #else
            struct Missing { long value; };
            #endif
            """);
        var common = Input("common.idl", """struct Common { long value; };""", generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class Found");
        output.ShouldNotContain("public partial class Missing");
    }

    [Fact]
    [Trait("Preprocessor", "PP048")]
    public void EvaluatesHasIncludeAsFalseForMissingInput()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #if __has_include("missing.idl")
            struct Unexpected { long value; };
            #else
            struct Expected { long value; };
            #endif
            """);

        // Act
        var output = Compile(root);

        // Assert
        output.ShouldContain("public partial class Expected");
        output.ShouldNotContain("public partial class Unexpected");
    }

    [Fact]
    [Trait("Preprocessor", "PP048")]
    public void AcceptsWhitespaceBeforeHasIncludeArguments()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #if __has_include   ("common.idl")
            struct Found { long value; };
            #else
            struct Missing { long value; };
            #endif
            """);
        var common = Input("common.idl", "struct Common { long value; };", generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class Found");
        output.ShouldNotContain("public partial class Missing");
    }

    [Fact]
    [Trait("Preprocessor", "PP048")]
    public void ExpandsMacrosBeforeEvaluatingHasInclude()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #define HEADER "common.idl"
            #if __has_include(HEADER)
            struct Found { long value; };
            #else
            struct Missing { long value; };
            #endif
            """);
        var common = Input("common.idl", "struct Common { long value; };", generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class Found");
        output.ShouldNotContain("public partial class Missing");
    }

    [Fact]
    [Trait("Preprocessor", "PP048")]
    public void LeavesMalformedHasIncludeAsAnUndefinedCondition()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #if __has_include
            struct Unexpected { long value; };
            #else
            struct Expected { long value; };
            #endif
            """);

        // Act
        var output = Compile(root);

        // Assert
        output.ShouldContain("public partial class Expected");
        output.ShouldNotContain("public partial class Unexpected");
    }

    [Fact]
    [Trait("Preprocessor", "PP006")]
    [Trait("Preprocessor", "PP022")]
    public void IsolatesMacroStateBetweenRoots()
    {
        // Arrange
        var first = Input("a.idl",
            """
            #define PRIVATE
            struct First { long value; };
            """);
        var second = Input("b.idl",
            """
            #ifdef PRIVATE
            struct Leaked { long value; };
            #else
            struct Isolated { long value; };
            #endif
            """);

        // Act
        var output = Compile(first, second);

        // Assert
        output.ShouldContain("Isolated");
        output.ShouldNotContain("Leaked");
    }

    [Fact]
    [Trait("Corpus", "C060")]
    [Trait("Preprocessor", "PP031")]
    public void ReportsMissingIncludes()
    {
        // Arrange
        var missing = Input("missing.idl",
            """
            #include "common.idl"
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(missing));

        // Assert
        exception.Message.ShouldContain("Could not resolve included IDL");
    }

    [Theory]
    [InlineData("")]
    [InlineData("C:\\rooted.idl")]
    [InlineData("bad\0name.idl")]
    [Trait("Preprocessor", "PP031")]
    public void RejectsUnsafeIncludeNamesBeforeSearching(string includeName)
    {
        // Arrange
        var resolver = new IdlIncludeResolver(new Dictionary<string, IdlInput>(), []);
        var including = Input("root.idl", "");

        // Act
        var resolved = resolver.TryResolve(including, includeName, angle: false, out _);

        // Assert
        resolved.ShouldBeFalse();
    }

    [Fact]
    [Trait("Preprocessor", "PP031")]
    public void RejectsIncludeNamesThatExceedThePlatformPathLimit()
    {
        // Arrange
        var resolver = new IdlIncludeResolver(new Dictionary<string, IdlInput>(), []);
        var including = Input("root.idl", "");
        var includeName = new string('a', 40_000);

        // Act
        var resolved = resolver.TryResolve(including, includeName, angle: false, out _);

        // Assert
        resolved.ShouldBeFalse();
    }
}
