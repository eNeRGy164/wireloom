namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>
/// The target-independent semantic type system produced by the IDL front end.
/// It must not contain generated-language or RTI-specific names.
/// </summary>
public abstract class IdlType
{
    private protected IdlType()
    {
    }

    /// <summary>Represents an IDL primitive type.</summary>
    public sealed class Primitive(string name)
        : IdlType
    {
        /// <summary>Gets the IDL primitive type name.</summary>
        public string Name { get; } = name;
    }

    /// <summary>Represents a bounded IDL string or wide-string type.</summary>
    public sealed class StringType(bool isWide, int bound, bool isBounded, bool isUnbounded = false)
        : IdlType
    {
        /// <summary>Gets whether this is a wide string.</summary>
        public bool IsWide { get; } = isWide;
        /// <summary>Gets whether the source declares an explicit bound.</summary>
        public bool IsBounded { get; } = isBounded;
        /// <summary>Gets whether the source explicitly requests unbounded semantics.</summary>
        public bool IsUnbounded { get; } = isUnbounded;
        /// <summary>Gets the resolved string bound or its effective fallback.</summary>
        public int Bound { get; private set; } = bound;

        /// <summary>Applies a bound resolved after parsing.</summary>
        internal void SetBound(int value) => Bound = value;
    }

    /// <summary>Represents a reference to an IDL enum type.</summary>
    public sealed class Enum(string qualifiedName, int defaultValue, string defaultMemberName)
        : IdlType
    {
        /// <summary>Gets the fully qualified IDL enum name.</summary>
        public string QualifiedName { get; } = qualifiedName;
        /// <summary>Gets the default enumerator's numeric value.</summary>
        public int DefaultValue { get; } = defaultValue;
        /// <summary>Gets the default enumerator's name.</summary>
        public string DefaultMemberName { get; } = defaultMemberName;
    }

    /// <summary>Represents a reference to an IDL struct type.</summary>
    public sealed class Struct(string qualifiedName)
        : IdlType
    {
        /// <summary>Gets the fully qualified IDL struct name.</summary>
        public string QualifiedName { get; } = qualifiedName;
    }

    /// <summary>Represents a reference to an IDL union type.</summary>
    public sealed class Union(string qualifiedName)
        : IdlType
    {
        /// <summary>Gets the fully qualified IDL union name.</summary>
        public string QualifiedName { get; } = qualifiedName;
    }

    /// <summary>Represents an IDL alias and its resolved target type.</summary>
    public sealed class Alias(string qualifiedName, IdlType target)
        : IdlType
    {
        /// <summary>Gets the fully qualified IDL alias name.</summary>
        public string QualifiedName { get; } = qualifiedName;
        /// <summary>Gets the resolved type represented by the alias.</summary>
        public IdlType Target { get; } = target;
    }

    /// <summary>Retains a parsed named type until the complete symbol set is available.</summary>
    internal sealed class Reference(IdlTypeReference reference, string errorPrefix)
        : IdlType
    {
        /// <summary>Gets the unresolved parsed type reference.</summary>
        public IdlTypeReference TypeReference { get; } = reference;
        /// <summary>Gets the diagnostic prefix used if the reference cannot be resolved.</summary>
        public string ErrorPrefix { get; } = errorPrefix;
    }

    /// <summary>Represents an IDL sequence type, optionally bounded and multidimensional.</summary>
    public sealed class Sequence(IdlType element, int? bound, IReadOnlyList<int>? dimensions = null, int effectiveBound = 100, bool isUnbounded = false)
        : IdlType
    {
        /// <summary>Gets the type of each sequence element.</summary>
        public IdlType Element { get; } = element;
        /// <summary>Gets the declared sequence bound, or null when no bound was specified.</summary>
        public int? Bound { get; private set; } = bound;
        /// <summary>Gets the array dimensions applied to each sequence element.</summary>
        public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];
        /// <summary>Gets the declared bound or the configured effective fallback.</summary>
        public int EffectiveBound { get; private set; } = bound ?? effectiveBound;
        /// <summary>Gets whether the source explicitly requests unbounded semantics.</summary>
        public bool IsUnbounded { get; } = isUnbounded;

        /// <summary>Applies a bound resolved after parsing.</summary>
        internal void SetBound(int value)
        {
            Bound = value;
            EffectiveBound = value;
        }
    }

    /// <summary>Represents a fixed-size IDL array type.</summary>
    public sealed class Array(IdlType element, IReadOnlyList<int> dimensions)
        : IdlType
    {
        /// <summary>Gets the type stored in each array element.</summary>
        public IdlType Element { get; } = element;
        /// <summary>Gets the array dimensions in source order.</summary>
        public IReadOnlyList<int> Dimensions { get; } = dimensions;

    }
}
