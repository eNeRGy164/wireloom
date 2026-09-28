using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public sealed class IdlInputGraphStateSpecs
{
    [Fact]
    public void EmitsSharedIncludesOnceAndInDeterministicOrder()
    {
        // Arrange
        var common = Input("common.idl", """struct Shared { int32 count; };""", generate: false);
        var first = Input("first.idl",
            """
            #include "common.idl"
            struct First { string<256> msg; };
            """);
        var second = Input("second.idl",
            """
            #include "common.idl"
            struct Second { boolean active; };
            """);

        // Act
        var output = Compile(first, second, common);
        var reorderedOutput = Compile(common, second, first);

        // Assert
        var sharedClassOccurrences = output.Split("public partial class Shared", StringSplitOptions.None).Length;
        sharedClassOccurrences.ShouldBe(2);
        output.ShouldContain("Bound(256)");
        output.IndexOf("/// Gets or sets the <c>msg</c> member.", StringComparison.Ordinal)
            .ShouldBeLessThan(output.IndexOf("[Bound(256)]", StringComparison.Ordinal));
        output.ShouldBe(reorderedOutput);
    }

    [Fact]
    [Trait("Preprocessor", "PP031")]
    public void RecompilesWhenAnIncludedFileChanges()
    {
        // Arrange
        var first = Input("first.idl",
            """
            #include "common.idl"
            struct First { string<256> msg; };
            """);
        var second = Input("second.idl",
            """
            #include "common.idl"
            struct Second { boolean active; };
            """);
        var common = Input("common.idl", """struct Shared { uint32 changed; };""", generate: false);

        // Act
        var output = Compile(first, second, common);

        // Assert
        output.ShouldContain("uint changed");
        output.ShouldNotContain("int count");
    }

    [Fact]
    [Trait("Preprocessor", "PP034")]
    public void DeduplicatesRepeatedIncludesWhileRetainingTheirMacroState()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #include "common.idl"
            #include "common.idl"
            #ifdef FROM_COMMON
            struct Root { Shared value; };
            #endif
            """);
        var common = Input("common.idl",
            """
            #ifndef COMMON_IDL
            #define COMMON_IDL
            #define FROM_COMMON
            struct Shared { long value; };
            #endif
            """,
            generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class Root");
        output.Split("public partial class Shared", StringSplitOptions.None).Length.ShouldBe(2);
    }

    [Fact]
    [Trait("Preprocessor", "PP035")]
    public void SkipsLaterPragmaOnceIncludesBeforeReprocessingMacroState()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #include "common.idl"
            #undef FROM_ONCE
            #include "common.idl"
            #ifdef FROM_ONCE
            struct Unexpected { long value; };
            #endif
            """);
        var common = Input("common.idl",
            """
            #pragma once
            #define FROM_ONCE
            struct Shared { long value; };
            """,
            generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class Shared");
        output.ShouldNotContain("Unexpected");
    }

    [Fact]
    [Trait("Preprocessor", "PP034")]
    public void ReprocessesNonPragmaOnceIncludesForMacroSideEffects()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #include "common.idl"
            #undef FROM_COMMON
            #include "common.idl"
            #ifdef FROM_COMMON
            struct Reprocessed { long value; };
            #endif
            """);
        var common = Input("common.idl",
            """
            #define FROM_COMMON
            struct Shared { long value; };
            """,
            generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class Shared");
        output.ShouldContain("public partial class Reprocessed");
    }

    [Fact]
    [Trait("Preprocessor", "PP035")]
    public void ResetsPragmaOnceStateBetweenRoots()
    {
        // Arrange
        var first = Input("first.idl",
            """
            #include "common.idl"
            #ifdef FROM_SHARED
            struct First { long value; };
            #endif
            """);
        var second = Input("second.idl",
            """
            #include "common.idl"
            #ifdef FROM_SHARED
            struct Second { long value; };
            #endif
            """);
        var common = Input("common.idl",
            """
            #pragma once
            #define FROM_SHARED
            """,
            generate: false);

        // Act
        var output = Compile(first, second, common);

        // Assert
        output.ShouldContain("public partial class First");
        output.ShouldContain("public partial class Second");
    }

    [Fact]
    [Trait("Preprocessor", "PP034")]
    public void ReprocessesDeclarationsWhenMacroStateChangesForAnIncludedPath()
    {
        // Arrange
        var root = Input("root.idl",
            """
            #include "common.idl"
            #define ENABLE_COMMON
            #include "common.idl"
            """);
        var common = Input("common.idl",
            """
            #if defined(ENABLE_COMMON)
            struct EnabledCommon { long value; };
            #endif
            """,
            generate: false);

        // Act
        var output = Compile(root, common);

        // Assert
        output.ShouldContain("public partial class EnabledCommon");
    }

    [Fact]
    [Trait("Preprocessor", "PP034")]
    public void DoesNotParseTheSameIncludeOutputTwiceWhenMacroStateReturns()
    {
        // Arrange
        var first = Input("first.idl",
            """
            #define ENABLE_FIRST
            #include "common.idl"
            """, generate: true);
        var second = Input("second.idl",
            """
            #define ENABLE_SECOND
            #include "common.idl"
            """, generate: true);
        var third = Input("third.idl",
            """
            #define ENABLE_FIRST
            #include "common.idl"
            """, generate: true);
        var common = Input("common.idl",
            """
            #if defined(ENABLE_FIRST)
            struct FirstCommon { long value; };
            #elif defined(ENABLE_SECOND)
            struct SecondCommon { long value; };
            #endif
            """, generate: false);

        // Act
        var output = Compile(first, second, third, common);

        // Assert
        output.Split("public partial class FirstCommon", StringSplitOptions.None).Length.ShouldBe(2);
        output.Split("public partial class SecondCommon", StringSplitOptions.None).Length.ShouldBe(2);
    }

    [Fact]
    [Trait("Preprocessor", "PP023")]
    [Trait("Preprocessor", "PP032")]
    public void AppliesBatchPreprocessorConfigurationToEveryRoot()
    {
        // Arrange
        var first = new IdlInput(
            "first.idl",
            "struct First { long value; };",
            defines: ["BATCH_FEATURE"],
            includeDirectories: ["configured"]);
        var second = Input("second.idl",
            """
            #include <shared.idl>
            #if defined(BATCH_FEATURE)
            struct EnabledSecond { long value; };
            #endif
            """);
        var shared = Input(Path.Combine("configured", "shared.idl"), """struct Shared { long value; };""", generate: false);

        // Act
        var output = Compile(first, second, shared);

        // Assert
        output.ShouldContain("public partial class Shared");
        output.ShouldContain("public partial class EnabledSecond");
    }
}
