namespace Wireloom.Dds.Generator.Tests;

public static class ShouldlyStringExtensions
{
    public static void ShouldContainInOrder(this string actual, params List<string> expected)
    {
        var previous = -1;

        foreach (var fragment in expected)
        {
            var current = actual.IndexOf(fragment, previous + 1, StringComparison.Ordinal);
            current.ShouldBeGreaterThan(previous, $"Expected '{fragment}' after the previous contract fragment.");

            previous = current;
        }
    }
}

