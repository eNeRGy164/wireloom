using Rti.Types;
using RuntimeCollectionAliasMembers;
using RuntimeCollectionAliasMembers.Implementation;

namespace Wireloom.Dds.CSharp.RuntimeContracts.Native;

/// <summary>Checks collection alias projection at managed and native use sites.</summary>
public sealed class CollectionAliasMemberSpecs
{
    /// <summary>Checks defaults, profile copy ownership, and native conversions of projected collection members.</summary>
#if WIRELOOM_RTI_6_1_2
    [Fact]
    public void RoundTripsProjectedCollectionMembers()
#else
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void RoundTripsProjectedCollectionMembers(bool optionalPresent)
#endif
    {
        // Arrange
        var source = new Sample();
        source.chained.Value.Add(17);
        source.arrayAlias.Value[0] = 23;
        source.nested.Value.Add(new Numbers(new Sequence<int> { 31 }));
        source.rows.Value[0].Value.Add(37);
#if !WIRELOOM_RTI_6_1_2
        if (optionalPresent)
        {
            source.optionalChain = new Numbers(new Sequence<int> { 41 });
        }
#endif

        // Act
        var copy = new Sample(source);
        var result = NativeSample.RoundTrip<Sample, SampleUnmanaged>(source);
        source.chained.Value[0] = 99;
        source.arrayAlias.Value[0] = 99;
        source.nested.Value[0].Value[0] = 99;
        source.rows.Value[0].Value[0] = 99;

        // Assert
        copy.chained.Value.ShouldBe([RtiRuntimeProfile.SharesCollectionAliases ? 99 : 17]);
        copy.arrayAlias.Value.ShouldBe([RtiRuntimeProfile.SharesCollectionAliases ? 99 : 23, 0]);
        copy.nested.Value[0].Value.ShouldBe([RtiRuntimeProfile.SharesCollectionAliases ? 99 : 31]);
        copy.rows.Value[0].Value.ShouldBe([RtiRuntimeProfile.SharesCollectionAliases ? 99 : 37]);
        result.chained.Value.ShouldBe([17]);
        result.arrayAlias.Value.ShouldBe([23, 0]);
        result.nested.Value[0].Value.ShouldBe([31]);
        result.rows.Value[0].Value.ShouldBe([37]);
        result.direct.Value.ShouldBeEmpty();
#if !WIRELOOM_RTI_6_1_2
        if (optionalPresent)
        {
            result.optionalChain.ShouldNotBeNull().Value.ShouldBe([41]);
            copy.optionalChain.ShouldNotBeNull().Value.ShouldBe([41]);
            copy.optionalChain.ShouldNotBeSameAs(source.optionalChain);
        }
        else
        {
            result.optionalChain.ShouldBeNull();
            copy.optionalChain.ShouldBeNull();
        }
#endif
    }

    /// <summary>Checks collection alias projection in union branches and nested module members.</summary>
    [Fact]
    public void RoundTripsUnionAndCrossModuleMembers()
    {
        // Arrange
        var sequence = new Choice { numbers = new Numbers(new Sequence<int> { 17 }) };
        var array = new Choice { vector = new Vector([23, 29]) };
        var holder = new RuntimeCollectionAliasMembers.Inner.Holder();
        holder.values.Value.Add(31);

        // Act
        var sequenceResult = NativeSample.RoundTrip<Choice, ChoiceUnmanaged>(sequence);
        var arrayResult = NativeSample.RoundTrip<Choice, ChoiceUnmanaged>(array);
        var holderResult = NativeSample.RoundTrip<RuntimeCollectionAliasMembers.Inner.Holder,
            RuntimeCollectionAliasMembers.Inner.Implementation.HolderUnmanaged>(holder);

        // Assert
        sequenceResult.numbers.Value.ShouldBe([17]);
        arrayResult.vector.Value.ShouldBe([23, 29]);
        holderResult.values.Value.ShouldBe([31]);
    }
}
