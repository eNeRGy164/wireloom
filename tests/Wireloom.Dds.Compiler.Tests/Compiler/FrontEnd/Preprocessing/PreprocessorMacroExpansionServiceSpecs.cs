namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

[Trait("Preprocessor", "PP006")]
[Trait("Preprocessor", "PP007")]
[Trait("Preprocessor", "PP008")]
[Trait("Preprocessor", "PP010")]
[Trait("Preprocessor", "PP017")]
[Trait("Preprocessor", "PP020")]
[Trait("Preprocessor", "PP044")]
[Trait("Preprocessor", "PP046")]
[Trait("Preprocessor", "PP048")]
[Trait("Preprocessor", "PP050")]
[Trait("Preprocessor", "PP051")]
public sealed class PreprocessorMacroExpansionServiceSpecs
{
    [Fact]
    public void ExpandsObjectMacrosAndPreservesIdentifiersAndLiteralContents()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["OUTER"] = new PreprocessorMacro(null, "INNER", variadic: false),
            ["INNER"] = new PreprocessorMacro(null, "42", variadic: false),
            ["NAME"] = new PreprocessorMacro(null, "changed", variadic: false)
        };
        var service = CreateService(input, macros);

        // Act
        var nested = service.Expand("OUTER");
        var literalText = service.Expand("NAME \"NAME\" 'NAME' L\"NAME\" u8\"NAME\"");
        var escapedLiteral = service.Expand("\"NAME\\\"NAME\"");
        var unknown = service.Expand("UNKNOWN + ?");

        // Assert
        nested.ShouldBe("42");
        literalText.ShouldBe("changed \"NAME\" 'NAME' L\"NAME\" u8\"NAME\"");
        escapedLiteral.ShouldBe("\"NAME\\\"NAME\"");
        unknown.ShouldBe("UNKNOWN + ?");
    }

    [Fact]
    public void UsesSourceMappedOffsetsForLineBuiltinsAndFallsBackOutsideTheMap()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var mappedLines = new List<int>();
        var service = CreateService(input, logicalLine: (offset, _) =>
        {
            mappedLines.Add(offset);
            return offset + 1;
        });

        // Act
        var mapped = service.Expand("__LINE__", offset: 0, sourceOffsets: [100]);
        var unmapped = service.Expand("__LINE__", offset: 1, sourceOffsets: [100]);

        // Assert
        mapped.ShouldBe("101");
        unmapped.ShouldBe("2");
        mappedLines.ShouldBe([100, 1]);
    }

    [Theory]
    [InlineData("__has_include")]
    [InlineData("__FILE__")]
    [InlineData("__LINE__")]
    [InlineData("__COUNTER__")]
    [InlineData("UNKNOWN")]
    public void ReportsOutputLimitAtMappedBuiltinOrIdentifierOffset(string token)
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 1
        };
        var service = CreateService(
            input,
            expansionState: state,
            tryExpandHasInclude: (_, index) => (true, index, true));

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand(" " + token, sourceOffsets: [10, 91]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    [Fact]
    public void ExpandsFileAndCounterBuiltinsWhileKeepingRepeatedCountersDistinct()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var counter = 5;
        var service = CreateService(
            input,
            stringifyFile: () => "\"expansion.idl\"",
            nextCounter: () => counter++);

        // Act
        var result = service.Expand("__FILE__ __COUNTER__ __COUNTER__");

        // Assert
        result.ShouldBe("\"expansion.idl\" 5 6");
    }

    [Fact]
    public void DiagnosesWrongFunctionMacroArityAtTheMappedInvocationOffset()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["CALL"] = new PreprocessorMacro(["value"], "value", variadic: false)
        };
        var diagnostics = new List<IdlDiagnostic>();
        var service = CreateService(
            input,
            macros,
            diagnostics: diagnostics,
            expandFunctionMacro: (_, _, _, _, _, _) => "expanded");

        // Act
        var result = service.Expand("x CALL(a, b)", offset: 0, sourceOffsets: [10, 11, 73]);

        // Assert
        result.ShouldBe("x expanded");
        diagnostics.Single().Id.ShouldBe("DDSG0104");
        diagnostics.Single().Offset.ShouldBe(73);
        diagnostics.Single().Message.ShouldContain("wrong number of arguments");
    }

    [Fact]
    public void TreatsAnEmptyInvocationAsOneEmptyArgumentForSingleParameterMacros()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["IDENTITY"] = new PreprocessorMacro(["value"], "value", variadic: false)
        };
        var diagnostics = new List<IdlDiagnostic>();
        var service = CreateService(input, macros, diagnostics: diagnostics);

        // Act
        var result = service.Expand("IDENTITY()");

        // Assert
        result.ShouldBe("value");
        diagnostics.ShouldBeEmpty();
    }

    [Fact]
    public void AcceptsAnEmptyInvocationForMacrosWithNoParameters()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["NOTHING"] = new PreprocessorMacro([], "done", variadic: false)
        };
        var diagnostics = new List<IdlDiagnostic>();
        var service = CreateService(input, macros, diagnostics: diagnostics);

        // Act
        var result = service.Expand("NOTHING()");

        // Assert
        result.ShouldBe("done");
        diagnostics.ShouldBeEmpty();
    }

    [Fact]
    public void PassesMappedInvocationOffsetAndIncrementedDepthWhenRescanningAnObjectMacro()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["VALUE"] = new PreprocessorMacro(null, "1", variadic: false)
        };
        var expanding = new HashSet<string>(StringComparer.Ordinal);
        var observedDepth = -1;
        var observedOffset = -1;
        var service = CreateService(
            input,
            macros,
            rescanReplacementAndSuffix: (_, replacement, suffix, active, depth, tokenOffset, _, _, _) =>
            {
                active.Contains("VALUE").ShouldBeTrue();
                observedDepth = depth;
                observedOffset = tokenOffset;
                return replacement + suffix;
            });

        // Act
        var result = service.Expand("x VALUE", expanding, depth: 5, offset: 0, sourceOffsets: [10, 11, 73]);

        // Assert
        result.ShouldBe("x 1");
        observedDepth.ShouldBe(6);
        observedOffset.ShouldBe(73);
        expanding.ShouldBeEmpty();
    }

    [Fact]
    public void AllowsOutputExactlyAtTheLimitAndMapsTheFirstExcessCharacter()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 1
        };
        var service = CreateService(input, expansionState: state);

        // Act
        var exact = service.Expand(".", sourceOffsets: [25]);
        var exception = Should.Throw<IdlException>(() => service.Expand(".,", sourceOffsets: [25, 39]));

        // Assert
        exact.ShouldBe(".");
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(39);
    }

    [Fact]
    public void ReportsAnUnterminatedLiteralAtItsMappedStart()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var service = CreateService(input);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("\"unfinished", sourceOffsets: [81]));

        // Assert
        exception.Message.ShouldContain("Unterminated string literal");
        exception.Offset.ShouldBe(81);
    }

    [Fact]
    public void MapsAnUnterminatedLiteralAfterAnIdentifierToItsOriginalStart()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var service = CreateService(input);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("x \"unfinished", sourceOffsets: [10, 11, 81]));

        // Assert
        exception.Message.ShouldContain("Unterminated string literal");
        exception.Offset.ShouldBe(81);
    }

    [Fact]
    public void MapsAnOverflowingEscapedLiteralCharacterToItsOriginalOffset()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 3
        };
        var service = CreateService(input, expansionState: state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("\"A\\\"B\"", sourceOffsets: [10, 11, 12, 88, 89, 90]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(88);
    }

    [Fact]
    public void MapsAnOverflowingLiteralPrefixToItsOriginalOffset()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 2
        };
        var service = CreateService(input, expansionState: state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("x L\"a\"", sourceOffsets: [10, 11, 91, 92, 93, 94]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    [Fact]
    public void MapsAnOverflowingLiteralOpeningQuoteToItsOriginalOffset()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength
        };
        var service = CreateService(input, expansionState: state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("\"a\"", sourceOffsets: [91, 92, 93]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    [Fact]
    public void MapsAnOverflowingLiteralBodyCharacterToItsOriginalOffset()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 1
        };
        var service = CreateService(input, expansionState: state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("\"a\"", sourceOffsets: [10, 91, 92]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    [Fact]
    public void ReportsUnterminatedLiteralWhenItsFinalCharacterIsAnEscape()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var service = CreateService(input);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("\"unfinished\\", sourceOffsets: [81]));

        // Assert
        exception.Message.ShouldContain("Unterminated string literal");
        exception.Offset.ShouldBe(81);
    }

    [Fact]
    public void RejectsFunctionMacroOutputWhenNoOutputCapacityRemains()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["EMIT"] = new PreprocessorMacro([], "ignored", variadic: false)
        };
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength
        };
        var service = CreateService(input, macros, state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand("EMIT()", sourceOffsets: [91]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    [Fact]
    public void AllowsFunctionMacroOutputExactlyAtTheLimit()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["EMIT"] = new PreprocessorMacro([], "x", variadic: false)
        };
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 1
        };
        var service = CreateService(input, macros, state);

        // Act
        var result = service.Expand("EMIT()");

        // Assert
        result.ShouldBe("x");
    }

    [Fact]
    public void IncrementsDepthWhenRescanningAFunctionMacro()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["CALL"] = new PreprocessorMacro([], "1", variadic: false)
        };
        var expanding = new HashSet<string>(StringComparer.Ordinal);
        var observedDepth = -1;
        var service = CreateService(
            input,
            macros,
            rescanReplacementAndSuffix: (_, replacement, suffix, active, depth, _, _, _, _) =>
            {
                active.Contains("CALL").ShouldBeTrue();
                observedDepth = depth;
                return replacement + suffix;
            });

        // Act
        var result = service.Expand("CALL()", expanding, depth: 4, offset: 0, sourceOffsets: null);

        // Assert
        result.ShouldBe("1");
        observedDepth.ShouldBe(5);
        expanding.ShouldBeEmpty();
    }

    [Fact]
    public void MapsRecursiveMacroNamesWhenOutputCapacityIsFull()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["LOOP"] = new PreprocessorMacro(null, "LOOP", variadic: false)
        };
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 1
        };
        var expanding = new HashSet<string>(["LOOP"], StringComparer.Ordinal);
        var service = CreateService(input, macros, state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand(" LOOP", expanding, depth: 0, offset: 0, sourceOffsets: [10, 91]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    [Fact]
    public void MapsAnUninvokedFunctionMacroNameWhenOutputCapacityIsFull()
    {
        // Arrange
        var input = new IdlInput("expansion.idl", string.Empty);
        var macros = new PreprocessorMacroTable
        {
            ["FUNC"] = new PreprocessorMacro(["value"], "value", variadic: false)
        };
        var state = new PreprocessorExpansionState
        {
            BaseOutputLength = PreprocessorLimits.MaximumOutputLength - 1
        };
        var service = CreateService(input, macros, state);

        // Act
        var exception = Should.Throw<IdlException>(() =>
            service.Expand(" FUNC", sourceOffsets: [10, 91]));

        // Assert
        exception.Message.ShouldContain("Preprocessor output exceeds");
        exception.Offset.ShouldBe(91);
    }

    private static PreprocessorMacroExpansionService CreateService(
        IdlInput input,
        PreprocessorMacroTable? macros = null,
        PreprocessorExpansionState? expansionState = null,
        ICollection<IdlDiagnostic>? diagnostics = null,
        Func<string, PreprocessorMacro, List<string>, HashSet<string>, int, int, string>? expandFunctionMacro = null,
        Func<string, string, string, HashSet<string>, int, int, int, int, IReadOnlyList<int>?, string>? rescanReplacementAndSuffix = null,
        Func<string, int, (bool Handled, int Index, bool Value)>? tryExpandHasInclude = null,
        Func<string>? stringifyFile = null,
        Func<int>? nextCounter = null,
        Func<int, IReadOnlyList<int>?, int>? logicalLine = null) =>
        new(
            macros ?? new PreprocessorMacroTable(),
            new PreprocessorMacroTokenService(),
            expansionState ?? new PreprocessorExpansionState(),
            CancellationToken.None,
            () => input,
            () => diagnostics,
            tryExpandHasInclude ?? ((_, index) => (false, index, false)),
            (_, index) => (false, index),
            expandFunctionMacro ?? ((_, macro, _, _, _, _) => macro.Body),
            rescanReplacementAndSuffix ?? ((_, replacement, suffix, _, _, _, _, _, _) => replacement + suffix),
            stringifyFile ?? (() => "\"file.idl\""),
            logicalLine ?? ((offset, _) => offset + 1),
            nextCounter ?? (() => 0));
}
