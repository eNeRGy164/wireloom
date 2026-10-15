using RuntimeUnions;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts;

/// <summary>Creates independent union samples with explicit expected branch values.</summary>
internal static class UnionSamples
{
    /// <summary>Creates a sample selecting the requested branch or label.</summary>
    /// <param name="branch">The discriminator to select.</param>
    /// <returns>A populated union.</returns>
    internal static Choice Create(int branch)
    {
        var sample = new Choice();
        switch (branch)
        {
            case 0:
                sample.number = 17;
                break;
            case 1:
            case 2:
                sample.Settext("union", branch);
                break;
            case 3:
                sample.child = new Child(23, "child");
                break;
            case 4:
                sample.numbers = new Numbers(new Sequence<int> { 3, 4 });
                break;
            case 5:
                sample.vector = new Vector([5, 6]);
                break;
            default:
                sample.Setflag(true, branch);
                break;
        }

        return sample;
    }
}
