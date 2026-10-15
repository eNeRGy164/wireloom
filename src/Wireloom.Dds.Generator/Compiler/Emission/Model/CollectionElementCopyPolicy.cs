namespace Wireloom.Compiler.Emission.Model;

/// <summary>Determines whether a generated collection element requires an independent managed copy.</summary>
internal static class CollectionElementCopyPolicy
{
    /// <summary>Returns whether copying the element type requires invoking its generated copy constructor.</summary>
    public static bool RequiresManagedCopy(EmissionTypePlan elementType) =>
        elementType.Shape.IsAggregate
        || elementType is AliasEmissionType alias
            && !string.Equals(alias.CSharpType, alias.Target.CSharpType, StringComparison.Ordinal);
}
