#nullable enable annotations

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Constructs and independently verifies values for the C163–C172 wire cases.</summary>
internal static partial class ManagedFixtures
{
    /// <summary>Creates a case-specific typed sample and validates it against independently stated expectations.</summary>
    /// <typeparam name="T">Generated topic type for the selected case.</typeparam>
    /// <param name="options">Case and fixture selection.</param>
    /// <returns>The validated generated sample.</returns>
    internal static T CreateIdlGapFixture<T>(PeerOptions options)
    {
        object sample = options.CaseId switch
        {
#if WIRE_COMPATIBILITY_C163
            "14-extensibility-full" => CreateExtensibilityFull(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C164
            "14-autoid-scope" => CreateAutoIdScope(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C165
            "14-annotation-arguments" => CreateAnnotationArguments(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C166
            "14-member-declarators" => CreateMemberDeclarators(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C167
            "14-union-label-expressions" => CreateUnionLabelExpressions(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C168
            "14-union-scoped-enum" => CreateUnionScopedEnum(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C169
            "14-union-array-branch" => CreateUnionArrayBranch(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C170
            "14-string-expression-bound" => CreateStringExpressionBound(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C171
            "14-typed-defaults" => CreateTypedDefaults(options.Fixture),
#endif
#if WIRE_COMPATIBILITY_C172
            "14-enum-constant" => CreateEnumConstant(options.Fixture),
#endif
            _ => throw UnknownFixture(options)
        };

        VerifyIdlGapFixture((T)sample, options);
        return (T)sample;
    }

    /// <summary>Checks a generated sample using direct, case-specific member access.</summary>
    /// <typeparam name="T">Generated topic type for the selected case.</typeparam>
    /// <param name="sample">Sample to verify.</param>
    /// <param name="options">Case and fixture selection.</param>
    internal static void VerifyIdlGapFixture<T>(T sample, PeerOptions options)
    {
        switch (options.CaseId)
        {
#if WIRE_COMPATIBILITY_C163
            case "14-extensibility-full":
                VerifyExtensibilityFull((FullExtensibility)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C164
            case "14-autoid-scope":
                VerifyAutoIdScope((AutoIdScopeSample)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C165
            case "14-annotation-arguments":
                VerifyAnnotationArguments((AnnotationArguments)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C166
            case "14-member-declarators":
                VerifyMemberDeclarators((Declarators)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C167
            case "14-union-label-expressions":
                VerifyUnionLabelExpressions((LabelExpressions)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C168
            case "14-union-scoped-enum":
                VerifyUnionScopedEnum((ScopedEnum)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C169
            case "14-union-array-branch":
                VerifyUnionArrayBranch((ArrayBranch)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C170
            case "14-string-expression-bound":
                VerifyStringExpressionBound((StringBounded)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C171
            case "14-typed-defaults":
                VerifyTypedDefaults((TypedDefaults)(object)sample!, options);
                break;
#endif
#if WIRE_COMPATIBILITY_C172
            case "14-enum-constant":
                VerifyEnumConstant((EnumConstantSample)(object)sample!, options);
                break;
#endif
            default:
                throw new FixtureException($"No independent typed verifier is registered for '{options.CaseId}'.");
        }
    }

#if WIRE_COMPATIBILITY_C163
    private static FullExtensibility CreateExtensibilityFull(string fixture)
    {
        RequireFixture(fixture, "non-default");
        return new FullExtensibility { value = 16301 };
    }

    private static void VerifyExtensibilityFull(FullExtensibility sample, PeerOptions options) =>
        AssertValue(sample.value, 16301, "value", options);
#endif

#if WIRE_COMPATIBILITY_C164
    private static AutoIdScopeSample CreateAutoIdScope(string fixture)
    {
        RequireFixture(fixture, "scope-nested");
        return new AutoIdScopeSample
        {
            hashed = new AutoIds.Hashed { first = 16401, second = 16402 },
            reopened = new AutoIds.Reopened { first = 16411, second = 16412 },
            sequential = new Sequential { first = 16421, second = 16422 },
            usesInclude = new UsesInclude { included = new Included { first = 16431, second = 16432 } }
        };
    }

    private static void VerifyAutoIdScope(AutoIdScopeSample sample, PeerOptions options)
    {
        AssertValue(sample.hashed.first, 16401, "hashed.first", options);
        AssertValue(sample.hashed.second, 16402, "hashed.second", options);
        AssertValue(sample.reopened.first, 16411, "reopened.first", options);
        AssertValue(sample.reopened.second, 16412, "reopened.second", options);
        AssertValue(sample.sequential.first, 16421, "sequential.first", options);
        AssertValue(sample.sequential.second, 16422, "sequential.second", options);
        AssertValue(sample.usesInclude.included.first, 16431, "usesInclude.included.first", options);
        AssertValue(sample.usesInclude.included.second, 16432, "usesInclude.included.second", options);
    }
#endif

#if WIRE_COMPATIBILITY_C165
    private static AnnotationArguments CreateAnnotationArguments(string fixture) => fixture switch
    {
        "default" => new AnnotationArguments(),
        "non-default" => new AnnotationArguments { plain = 16501, selected = 16502 },
        _ => throw new FixtureException($"Unknown 14-annotation-arguments fixture '{fixture}'.")
    };

    private static void VerifyAnnotationArguments(AnnotationArguments sample, PeerOptions options)
    {
        var expectedPlain = options.Fixture switch
        {
            "default" => 0,
            "non-default" => 16501,
            _ => throw UnknownFixture(options)
        };
        var expectedSelected = options.Fixture switch
        {
            "default" => 42,
            "non-default" => 16502,
            _ => throw UnknownFixture(options)
        };
        AssertValue(sample.plain, expectedPlain, "plain", options);
        AssertValue(sample.selected, expectedSelected, "selected", options);
    }
#endif

#if WIRE_COMPATIBILITY_C166
    private static Declarators CreateMemberDeclarators(string fixture)
    {
        RequireFixture(fixture, "non-default");
        return new Declarators
        {
            first = 16601,
            second = 16602,
            numbers = [16611, 16612],
            matrix = new[,] { { 16621, 16622, 16623 }, { 16631, 16632, 16633 } }
        };
    }

    private static void VerifyMemberDeclarators(Declarators sample, PeerOptions options)
    {
        AssertValue(sample.first, 16601, "first", options);
        AssertValue(sample.second, 16602, "second", options);
        AssertSequence(sample.numbers, [16611, 16612], "numbers", options);
        AssertMatrix(sample.matrix, new[,] { { 16621, 16622, 16623 }, { 16631, 16632, 16633 } }, "matrix", options);
    }
#endif

#if WIRE_COMPATIBILITY_C167
    private static LabelExpressions CreateUnionLabelExpressions(string fixture) => fixture switch
    {
        "symbolic" => new LabelExpressions { symbolic = 16701 },
        "arithmetic" => new LabelExpressions { arithmetic = 16702 },
        "hexadecimal" => new LabelExpressions { hexadecimal = 16703 },
        _ => throw new FixtureException($"Unknown 14-union-label-expressions fixture '{fixture}'.")
    };

    private static void VerifyUnionLabelExpressions(LabelExpressions sample, PeerOptions options)
    {
        switch (options.Fixture)
        {
            case "symbolic":
                AssertValue(sample.Discriminator, 42, "Discriminator", options);
                AssertValue(sample.symbolic, 16701, "symbolic", options);
                break;
            case "arithmetic":
                AssertValue(sample.Discriminator, 4, "Discriminator", options);
                AssertValue(sample.arithmetic, 16702, "arithmetic", options);
                break;
            case "hexadecimal":
                AssertValue(sample.Discriminator, 16, "Discriminator", options);
                AssertValue(sample.hexadecimal, 16703, "hexadecimal", options);
                break;
            default:
                throw UnknownFixture(options);
        }
    }
#endif

#if WIRE_COMPATIBILITY_C168
    private static ScopedEnum CreateUnionScopedEnum(string fixture) => fixture switch
    {
        "enum-selected" => new ScopedEnum { selected = 16801 },
        "enum-first" => new ScopedEnum { first = 16802 },
        _ => throw new FixtureException($"Unknown 14-union-scoped-enum fixture '{fixture}'.")
    };

    private static void VerifyUnionScopedEnum(ScopedEnum sample, PeerOptions options)
    {
        switch (options.Fixture)
        {
            case "enum-selected":
                AssertValue(sample.Discriminator, Choice.SECOND, "Discriminator", options);
                AssertValue(sample.selected, 16801, "selected", options);
                break;
            case "enum-first":
                AssertValue(sample.Discriminator, Choice.FIRST, "Discriminator", options);
                AssertValue(sample.first, 16802, "first", options);
                break;
            default:
                throw UnknownFixture(options);
        }
    }
#endif

#if WIRE_COMPATIBILITY_C169
    private static ArrayBranch CreateUnionArrayBranch(string fixture) => fixture switch
    {
        "array-branch" => new ArrayBranch { values = new[,] { { 16901, 16902, 16903 }, { 16911, 16912, 16913 } } },
        "scalar-branch" => new ArrayBranch { scalar = 16921 },
        _ => throw new FixtureException($"Unknown 14-union-array-branch fixture '{fixture}'.")
    };

    private static void VerifyUnionArrayBranch(ArrayBranch sample, PeerOptions options)
    {
        switch (options.Fixture)
        {
            case "array-branch":
                AssertValue(sample.Discriminator, 0, "Discriminator", options);
                AssertMatrix(sample.values, new[,] { { 16901, 16902, 16903 }, { 16911, 16912, 16913 } }, "values", options);
                break;
            case "scalar-branch":
                AssertValue(sample.Discriminator, 1, "Discriminator", options);
                AssertValue(sample.scalar, 16921, "scalar", options);
                break;
            default:
                throw UnknownFixture(options);
        }
    }
#endif

#if WIRE_COMPATIBILITY_C170
    private static StringBounded CreateStringExpressionBound(string fixture)
    {
        RequireFixture(fixture, "string-boundary");
        return new StringBounded { text = "12345", wideText = "ABCDEF" };
    }

    private static void VerifyStringExpressionBound(StringBounded sample, PeerOptions options)
    {
        AssertValue(sample.text, "12345", "text", options);
        AssertValue(sample.wideText, "ABCDEF", "wideText", options);
    }
#endif

#if WIRE_COMPATIBILITY_C171
    private static TypedDefaults CreateTypedDefaults(string fixture) => fixture switch
    {
        "default" => new TypedDefaults(),
        "non-default" => new TypedDefaults { inherited = 17101, overridden = 17102, text = "explicit", ratio = 2.25d, single = -3.5f },
        _ => throw new FixtureException($"Unknown 14-typed-defaults fixture '{fixture}'.")
    };

    private static void VerifyTypedDefaults(TypedDefaults sample, PeerOptions options)
    {
        var expected = options.Fixture switch
        {
            "default" => new TypedDefaultsExpected(7, 9, "alias", 1.5d, 0f),
            "non-default" => new TypedDefaultsExpected(17101, 17102, "explicit", 2.25d, -3.5f),
            _ => throw UnknownFixture(options)
        };
        AssertValue(sample.inherited, expected.Inherited, "inherited", options);
        AssertValue(sample.overridden, expected.Overridden, "overridden", options);
        AssertValue(sample.text, expected.Text, "text", options);
        AssertValue(sample.ratio, expected.Ratio, "ratio", options);
        AssertValue(sample.single, expected.Single, "single", options);
    }

    private readonly record struct TypedDefaultsExpected(int Inherited, int Overridden, string Text, double Ratio, float Single);
#endif

#if WIRE_COMPATIBILITY_C172
    private static EnumConstantSample CreateEnumConstant(string fixture)
    {
        RequireFixture(fixture, "constant-and-label");
        return new EnumConstantSample
        {
            value = new EnumConstant { color = Color.GREEN },
            choice = new EnumUnion { selected = 17201 }
        };
    }

    private static void VerifyEnumConstant(EnumConstantSample sample, PeerOptions options)
    {
        AssertValue(sample.value.color, Color.GREEN, "value.color", options);
        AssertValue(sample.choice.Discriminator, Color.GREEN, "choice.Discriminator", options);
        AssertValue(sample.choice.selected, 17201, "choice.selected", options);
    }
#endif

    private static void RequireFixture(string actual, string expected)
    {
        if (!string.Equals(actual, expected, StringComparison.Ordinal))
        {
            throw new FixtureException($"Expected fixture '{expected}', received '{actual}'.");
        }
    }

    private static void AssertValue<TValue>(TValue actual, TValue expected, string member, PeerOptions options)
    {
        if (!EqualityComparer<TValue>.Default.Equals(actual, expected))
        {
            throw new FixtureException($"{options.CaseId} fixture {options.Fixture}: expected {member}={expected}, received {actual}.");
        }
    }

    private static void AssertSequence<TValue>(IReadOnlyList<TValue> actual, IReadOnlyList<TValue> expected, string member, PeerOptions options)
    {
        if (!actual.SequenceEqual(expected))
        {
            throw new FixtureException($"{options.CaseId} fixture {options.Fixture}: expected {member}=[{string.Join(", ", expected)}], received [{string.Join(", ", actual)}].");
        }
    }

    private static void AssertMatrix<TValue>(TValue[,] actual, TValue[,] expected, string member, PeerOptions options)
    {
        if (actual.GetLength(0) != expected.GetLength(0) || actual.GetLength(1) != expected.GetLength(1))
        {
            throw new FixtureException($"{options.CaseId} fixture {options.Fixture}: unexpected dimensions for {member}.");
        }

        for (var row = 0; row < expected.GetLength(0); row++)
        {
            for (var column = 0; column < expected.GetLength(1); column++)
            {
                AssertValue(actual[row, column], expected[row, column], $"{member}[{row},{column}]", options);
            }
        }
    }

    private static FixtureException UnknownFixture(PeerOptions options) =>
        new($"Unknown fixture '{options.Fixture}' for {options.CaseId}.");
}
