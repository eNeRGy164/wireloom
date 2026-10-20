using RuntimeSequenceArrays;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Exercises the opt-in IDL mapping through typed serialization and managed ownership.</summary>
public sealed class SequenceArraySpecs
{
    /// <summary>Preserves independent sequence lengths, element kinds, dimensions, and optional presence.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SerializesSequenceArrays(bool present)
    {
        // Arrange
        var sample = Create();
        if (present)
        {
            sample.optionalValues = [new Sample_optionalValuesSequence(), new Sample_optionalValuesSequence()];
            sample.optionalValues[1].Value.Add(77);
        }

        using var serializer = SampleSupport.Instance.CreateSerializer();

        // Act
        var result = serializer.Deserialize(serializer.Serialize(sample));

        // Assert
        Check(result);
        if (present)
        {
            result.optionalValues.ShouldNotBeNull();
            result.optionalValues.Length.ShouldBe(2);
            result.optionalValues[0].Value.ShouldBeEmpty();
            result.optionalValues[1].Value.ShouldBe([77]);
        }
        else
        {
            result.optionalValues.ShouldBeNull();
        }

        var description = SampleSupport.Instance.DynamicType.ToString();
        description.ShouldContain("sequence<long,4> values[2]");
        description.ShouldNotContain("Sample_valuesSequence values");
    }

    /// <summary>Creates independent wrapper, sequence, and aggregate storage when copying arrays.</summary>
    [Fact]
    public void CopiesWithoutSharingInnerSequences()
    {
        // Arrange
        var original = Create();

        // Act
        var copy = new Sample(original);
        copy.values[0].Value[0] = 99;
        copy.items[0].Value[0].value = 100;
        copy.matrix[1, 2].Value.Add(88);

        // Assert
        Check(original);
        ReferenceEquals(original.values, copy.values).ShouldBeFalse();
        ReferenceEquals(original.values[0], copy.values[0]).ShouldBeFalse();
        original.matrix[1, 2].Value.ShouldBe([123]);
    }

    /// <summary>Uses the declaring file's mapping for inherited members.</summary>
    [Fact]
    public void SerializesInheritedSequenceArrays()
    {
        // Arrange
        var sample = new Derived { extra = 9 };
        sample.values[1].Value.Add(42);
        using var serializer = DerivedSupport.Instance.CreateSerializer();

        // Act
        var result = serializer.Deserialize(serializer.Serialize(sample));

        // Assert
        result.values.Length.ShouldBe(2);
        result.values[0].Value.ShouldBeEmpty();
        result.values[1].Value.ShouldBe([42]);
        result.extra.ShouldBe(9);
    }

    private static Sample Create()
    {
        var sample = new Sample();
        sample.values[0].Value.Add(17);
        sample.values[1].Value.Add(23);
        sample.values[1].Value.Add(42);
        sample.names[0].Value.Add("alpha");
        sample.names[1].Value.Add("beta");
        sample.wideNames[1].Value.Add("Ω");
        sample.items[0].Value.Add(new Item { value = 55 });
        sample.states[1].Value.Add(State.Done);
        sample.scalars[0].Value.Add(66);
        sample.matrix[1, 2].Value.Add(123);
        return sample;
    }

    private static void Check(Sample sample)
    {
        sample.values.Length.ShouldBe(2);
        sample.values[0].Value.ShouldBe([17]);
        sample.values[1].Value.ShouldBe([23, 42]);
        sample.names[0].Value.ShouldBe(["alpha"]);
        sample.names[1].Value.ShouldBe(["beta"]);
        sample.wideNames[1].Value.ShouldBe(["Ω"]);
        sample.items[0].Value.Single().value.ShouldBe(55);
        sample.items[1].Value.ShouldBeEmpty();
        sample.states[1].Value.ShouldBe([State.Done]);
        sample.scalars[0].Value.ShouldBe([66]);
        sample.matrix.GetLength(0).ShouldBe(2);
        sample.matrix.GetLength(1).ShouldBe(3);
        sample.matrix[1, 2].Value.ShouldBe([123]);
    }
}
