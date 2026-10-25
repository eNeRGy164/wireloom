using RuntimeManaged;
using Rti.Types;

namespace Wireloom.Dds.Generator.RuntimeContracts;

/// <summary>Provides independent, explicitly populated collection samples.</summary>
internal static class CollectionSamples
{
    /// <summary>Creates a populated sample with optional members present or absent.</summary>
    /// <param name="present">Whether optional members have values.</param>
    /// <returns>A new sample owning all of its containers.</returns>
    internal static Collections Create(bool present)
    {
        var sample = new Collections
        {
            child = new Child(11, "nested"),
            numbers = { 1, 2, 3 },
            children = { new Child(21, "child") },
            vector = [8, 9],
            matrix = new [,] { { 1, 2, 3 }, { 4, 5, 6 } },
            points = [new Child(41, "first"), new Child(42, "second")],
            texts = { "alpha", "beta" },
            wideTexts = { "世界", "Ω" }
        };
        if (present)
        {
            sample.optionalNumber = 7;
            sample.optionalText = "optional";
            sample.optionalChild = new Child(31, "present");
            sample.optionalNumbers = new Sequence<int> { 5, 6 };
            sample.optionalPoints = [new Child(51, "one"), new Child(52, "two")];
            sample.optionalState = State.Ready;
            sample.optionalWideText = "世界";
            sample.optionalWideTexts = new Sequence<string> { "世界", "Ω" };
            sample.optionalMatrix = new [,] { { 1, 2, 3 }, { 4, 5, 6 } };
        }

        return sample;
    }
}
