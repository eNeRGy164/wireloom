using RuntimeUnionNullCopy;
using RuntimeUnions;

namespace Wireloom.Dds.Generator.RuntimeContracts.Unions;

/// <summary>Checks XTypes defaults and null-source initialization independently of RTI's null-copy quirks.</summary>
public sealed class UnionConstructionSpecs
{
    /// <summary>Uses the selected profile when zero labels precede a default branch.</summary>
    [Fact]
    public void IntegerDefaultsFollowTheSelectedProfile()
    {
        // Arrange/Act/Assert
        if (RtiRuntimeProfile.Uses731Profile)
        {
            AssertDefault(() => new Choice(), () => new Choice(null), value => value.Discriminator, value => value.Get(), 6, false);
            AssertDefault(() => new NegativeAndZero(), () => new NegativeAndZero(null), value => value.Discriminator, value => value.Get(), 1, false);
        }
        else
        {
            AssertDefault(() => new Choice(), () => new Choice(null), value => value.Discriminator, value => value.Get(), 0, 0);
            AssertDefault(() => new NegativeAndZero(), () => new NegativeAndZero(null), value => value.Discriminator, value => value.Get(), 0, 0);
        }
        AssertDefault(() => new NegativeLabels(), () => new NegativeLabels(null), value => value.Discriminator, value => value.Get(), 0, false);
        var sparseDiscriminator = RtiRuntimeProfile.Uses731Profile ? 1 : 0;
        AssertDefault(() => new Sparse(), () => new Sparse(null), value => value.Discriminator, value => value.Get(), sparseDiscriminator,
            RtiRuntimeProfile.Uses731Profile ? 0 : null);
    }

    /// <summary>Preserves primitive and string field initialization for a false discriminator.</summary>
    [Fact]
    public void BooleanDefaultsInitializeTheSelectedValue()
    {
        // Arrange/Act/Assert
        AssertDefault(() => new ZeroScalar(), () => new ZeroScalar(null), value => value.Discriminator, value => value.Get(), false, 0);
        if (RtiRuntimeProfile.Uses731Profile)
        {
            AssertDefault(() => new BooleanDefault(), () => new BooleanDefault(null), value => value.Discriminator, value => value.Get(), true, new RuntimeUnionNullCopy.Child());
        }
        else
        {
            AssertDefault(() => new BooleanDefault(), () => new BooleanDefault(null), value => value.Discriminator, value => value.Get(), false, 0);
        }
        AssertDefault(() => new ZeroString(), () => new ZeroString(null), value => value.Discriminator, value => value.Get(), false, "");
    }

    /// <summary>Uses the first enum literal even when zero is unnamed or selects another branch.</summary>
    [Fact]
    public void EnumDefaultsPreservePositiveAndNegativeFirstLiterals()
    {
        // Arrange/Act/Assert
        AssertDefault(() => new EnumNoDefault(), () => new EnumNoDefault(null), value => value.Discriminator, value => value.Get(),
            Mode.First, 0);
        AssertDefault(() => new EnumWithDefault(), () => new EnumWithDefault(null), value => value.Discriminator, value => value.Get(),
            Mode.First, 0);
        AssertDefault(() => new NegativeEnum(), () => new NegativeEnum(null), value => value.Discriminator, value => value.Get(),
            NegativeMode.NegativeFirst, 0);
        AssertDefault(() => new MixedEnum(), () => new MixedEnum(null), value => value.Discriminator, value => value.Get(),
            MixedMode.MixedFirst, 0);
    }

    /// <summary>Executes default-only and aliased enum constructors with a non-first default literal.</summary>
    [Fact]
    public void EnumFallbackConstructorsPreserveTheProfileDefault()
    {
        // Arrange
        var expected = RtiRuntimeProfile.Uses731Profile ? AnnotatedMode.FirstDeclared : AnnotatedMode.Preferred;

        // Act
        var defaultOnly = new EnumDefaultOnly();
        var laterCase = new EnumLaterCase(null);

        // Assert
        defaultOnly.Discriminator.ShouldBe(expected);
        defaultOnly.fallback.ShouldBe(0);
        laterCase.Discriminator.ShouldBe(expected);
        laterCase.fallback.ShouldBe(0);
    }

    /// <summary>Creates independently owned aggregate, sequence, and array values for null sources.</summary>
    [Fact]
    public void NullCopiesInitializeIndependentReferenceBranches()
    {
        // Arrange
        var child = new ZeroAggregate(null);
        var childCopy = new ZeroAggregate(null);
        var sequence = new ZeroSequence(null);
        var sequenceCopy = new ZeroSequence(null);
        var array = new ZeroArray(null);
        var arrayCopy = new ZeroArray(null);
        var fallback = new DefaultAggregate(null);
        var enumChild = new EnumAggregate(null);

        // Act
        child.child.id = 17;
        sequence.numbers.Value.Add(23);
        array.vector.Value[0] = 31;

        // Assert
        child.Discriminator.ShouldBeFalse();
        childCopy.child.id.ShouldBe(0);
        childCopy.child.ShouldNotBeSameAs(child.child);
        sequenceCopy.numbers.Value.ShouldBeEmpty();
        sequenceCopy.numbers.ShouldNotBeSameAs(sequence.numbers);
        arrayCopy.vector.Value.ShouldBe([0, 0]);
        arrayCopy.vector.ShouldNotBeSameAs(array.vector);
        fallback.Discriminator.ShouldBe(0);
        fallback.child.id.ShouldBe(0);
        enumChild.Discriminator.ShouldBe(Mode.First);
        enumChild.child.id.ShouldBe(0);
        AssertEqualDefaults(new ZeroAggregate(), childCopy);
        AssertEqualDefaults(new ZeroSequence(), sequenceCopy);
        AssertEqualDefaults(new ZeroArray(), arrayCopy);
        AssertEqualDefaults(new DefaultAggregate(), fallback);
        AssertEqualDefaults(new EnumAggregate(), enumChild);
    }

    /// <summary>Keeps negative multi-label and named-zero transitions and copies usable.</summary>
    [Fact]
    public void CopiesNonNullSourcesAndTransitionsAcrossNegativeAndZeroLabels()
    {
        // Arrange
        var negative = new NegativeLabels();
        var mixed = new MixedEnum(null);

        // Act
        negative.Setnumber(23, -1);
        var copy = new NegativeLabels(negative);
        mixed.zero = 31;
        var zeroCopy = new MixedEnum(mixed);

        // Assert
        copy.Discriminator.ShouldBe(-1);
        copy.number.ShouldBe(23);
        AssertEqualDefaults(negative, copy);
        mixed.Discriminator.ShouldBe(MixedMode.MixedZero);
        zeroCopy.zero.ShouldBe(31);
        AssertEqualDefaults(mixed, zeroCopy);
        Should.Throw<InvalidOperationException>(() => mixed.number);
        mixed.number = 41;
        mixed.Discriminator.ShouldBe(MixedMode.MixedFirst);
        mixed.number.ShouldBe(41);
        zeroCopy.zero.ShouldBe(31);
    }

    private static void AssertDefault<T>(Func<T> construct, Func<T> nullCopy, Func<T, object> discriminator,
        Func<T, object?> get, object expectedDiscriminator, object? expectedValue) where T : IEquatable<T>
    {
        // Arrange
        var expected = construct();

        // Act
        var actual = nullCopy();

        // Assert
        discriminator(expected).ShouldBe(expectedDiscriminator);
        discriminator(actual).ShouldBe(expectedDiscriminator);
        get(expected).ShouldBe(expectedValue);
        get(actual).ShouldBe(expectedValue);
        AssertEqualDefaults(expected, actual);
    }

    private static void AssertEqualDefaults<T>(T expected, T actual) where T : IEquatable<T>
    {
        expected.Equals(actual).ShouldBeTrue();
        actual.Equals(expected).ShouldBeTrue();
        actual.Equals((object?)expected).ShouldBeTrue();
        actual.GetHashCode().ShouldBe(expected.GetHashCode());
        actual.ToString().ShouldBe(expected.ToString());
    }
}
