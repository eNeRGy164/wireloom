namespace Wireloom.Dds.Generator.RuntimeContracts.Native;

/// <summary>Supplies both key-filter values and all native allocation flag combinations.</summary>
public static class NativeCases
{
    /// <summary>Gets the complete native flag matrix.</summary>
    public static TheoryData<bool, bool, bool> All
    {
        get
        {
            var cases = new TheoryData<bool, bool, bool>();
            foreach (var keysOnly in new[] { false, true })
            {
                foreach (var allocatePointers in new[] { false, true })
                {
                    foreach (var allocateMemory in new[] { false, true })
                    {
                        cases.Add(keysOnly, allocatePointers, allocateMemory);
                    }
                }
            }

            return cases;
        }
    }
}
