namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Describes the complete-type order and RTI initialization stages for a generated file.</summary>
internal sealed record TypeInitializationPlan(
    IReadOnlyList<TypeInitializationEntry> TypeInitializationOrder,
    IReadOnlyList<StructPlan> StructShellsBeforeMetadata,
    bool RequiresStagedInitialization,
    bool RequiresMetadataOnlyTypeInitialization);

/// <summary>Identifies one canonical struct or union in dependency-ordered generation.</summary>
internal sealed record TypeInitializationEntry(StructPlan? Struct, UnionPlan? Union);
