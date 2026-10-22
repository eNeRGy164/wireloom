using RuntimeUnions;
using RuntimeUnions.Implementation;

namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Checks every union branch conversion and optional-only cleanup.</summary>
public sealed class UnionNativeSpecs
{
    /// <summary>Provides all branch, key-filter, and allocation combinations.</summary>
    public static TheoryData<int, bool, bool, bool> Cases
    {
        get
        {
            var cases = new TheoryData<int, bool, bool, bool>();
            for (var branch = 0; branch <= 7; branch++)
            {
                foreach (var keysOnly in new[] { false, true })
                {
                    foreach (var allocatePointers in new[] { false, true })
                    {
                        foreach (var allocateMemory in new[] { false, true })
                        {
                            cases.Add(branch, keysOnly, allocatePointers, allocateMemory);
                        }
                    }
                }
            }

            return cases;
        }
    }

    /// <summary>Verifies explicit, multiple-label, aggregate, collection, and default branches.</summary>
    [Theory]
    [MemberData(nameof(Cases))]
    public void ConvertsSelectedBranch(int branch, bool keysOnly, bool allocatePointers, bool allocateMemory)
    {
        // Arrange
        var source = UnionSamples.Create(branch);

        // Act
        var result = NativeSample.RoundTrip<Choice, ChoiceUnmanaged>(source, allocatePointers, allocateMemory, keysOnly);

        // Assert
        result.Discriminator.ShouldBe(branch);
        switch (branch)
        {
            case 0:
                result.number.ShouldBe(17);
                break;
            case 1:
            case 2:
                result.text.ShouldBe("union");
                break;
            case 3:
                result.child.id.ShouldBe(23);
                result.child.text.ShouldBe("child");
                break;
            case 4:
                result.numbers.Value.ShouldBe([3, 4]);
                break;
            case 5:
                result.vector.Value.ShouldBe([5, 6]);
                break;
            case 6:
                result.values.ShouldBe([7, 8]);
                break;
            default:
                result.flag.ShouldBeTrue();
                break;
        }
    }

    /// <summary>Checks a native union with no active branch.</summary>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ConvertsNoActiveBranch(bool keysOnly)
    {
        // Arrange
        var source = new Sparse();

        // Act
        var result = NativeSample.RoundTrip<Sparse, SparseUnmanaged>(source, keysOnly: keysOnly);

        // Assert
        result.Discriminator.ShouldBe(Sparse.DefaultDiscriminator);
        if (RtiRuntimeProfile.Uses731Profile)
        {
            result.Discriminator.ShouldBe(1);
            result.Get().ShouldBe(0);
            result.number.ShouldBe(0);
        }
        else
        {
            result.Get().ShouldBeNull();
        }
    }
}
