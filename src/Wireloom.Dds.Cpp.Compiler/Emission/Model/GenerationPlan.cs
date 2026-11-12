namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Contains validated C++ declarations and initialization decisions for the emitters.</summary>
internal sealed class GenerationPlan
{
    /// <summary>Creates a complete emission plan from validated declarations.</summary>
    internal GenerationPlan(
        string fileStem,
        IReadOnlyList<ConstantPlan> constants,
        IReadOnlyList<EnumPlan> enums,
        IReadOnlyList<AliasPlan> aliases,
        IReadOnlyList<StructPlan> structs,
        IReadOnlyList<UnionPlan> unions,
        TypeInitializationPlan initialization,
        RtiApiProfile profile)
    {
        FileStem = fileStem;
        Constants = constants;
        Enums = enums;
        Aliases = aliases;
        Structs = structs;
        Unions = unions;
        Initialization = initialization;
        Profile = profile;
    }

    /// <summary>Gets the input-derived file stem used by the output documents.</summary>
    internal string FileStem { get; }

    /// <summary>Gets the supported scalar constants in source order.</summary>
    internal IReadOnlyList<ConstantPlan> Constants { get; }

    /// <summary>Gets the supported IDL enums in source order.</summary>
    internal IReadOnlyList<EnumPlan> Enums { get; }

    /// <summary>Gets supported typedef declarations in source order.</summary>
    internal IReadOnlyList<AliasPlan> Aliases { get; }

    /// <summary>Gets the supported struct declarations in source order.</summary>
    internal IReadOnlyList<StructPlan> Structs { get; }

    /// <summary>Gets supported IDL union declarations in source order.</summary>
    internal IReadOnlyList<UnionPlan> Unions { get; }

    /// <summary>Gets the order and stages for generated type initialization.</summary>
    internal TypeInitializationPlan Initialization { get; }

    /// <summary>Gets the RTI C++ API profile selected for generated source.</summary>
    internal RtiApiProfile Profile { get; }
}
