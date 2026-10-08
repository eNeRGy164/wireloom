namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>
/// The target-independent semantic type system produced by the IDL front end.
/// It must not contain generated-language or RTI-specific names.
/// </summary>
internal abstract class IdlType
{
    /// <summary>Represents an IDL primitive type.</summary>
    public sealed class Primitive(string name)
        : IdlType
    {
        public string Name { get; } = name;
    }

    /// <summary>Represents a bounded IDL string or wide-string type.</summary>
    public sealed class StringType(bool isWide, int bound, bool isBounded = true)
        : IdlType
    {
        public bool IsWide { get; } = isWide;
        public bool IsBounded { get; } = isBounded;
        public int Bound { get; private set; } = bound;

        /// <summary>Applies a bound resolved after parsing.</summary>
        internal void SetBound(int value) => Bound = value;
    }

    /// <summary>Represents a reference to an IDL enum type.</summary>
    public sealed class Enum(string qualifiedName, int defaultValue, string defaultMemberName)
        : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
        public int DefaultValue { get; } = defaultValue;
        public string DefaultMemberName { get; } = defaultMemberName;
    }

    /// <summary>Represents a reference to an IDL struct type.</summary>
    public sealed class Struct(string qualifiedName)
        : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
    }

    /// <summary>Represents a reference to an IDL union type.</summary>
    public sealed class Union(string qualifiedName)
        : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
    }

    /// <summary>Represents an IDL alias and its resolved target type.</summary>
    public sealed class Alias(string qualifiedName, IdlType target)
        : IdlType
    {
        public string QualifiedName { get; } = qualifiedName;
        public IdlType Target { get; } = target;
    }

    /// <summary>Retains a parsed named type until the complete symbol set is available.</summary>
    public sealed class Reference(IdlTypeReference reference, string errorPrefix)
        : IdlType
    {
        public IdlTypeReference TypeReference { get; } = reference;
        public string ErrorPrefix { get; } = errorPrefix;
    }

    /// <summary>Represents an IDL sequence type, optionally bounded and multidimensional.</summary>
    public sealed class Sequence(IdlType element, int? bound, IReadOnlyList<int>? dimensions = null)
        : IdlType
    {
        public IdlType Element { get; } = element;
        public int? Bound { get; private set; } = bound;
        public IReadOnlyList<int> Dimensions { get; } = dimensions ?? [];

        /// <summary>Applies a bound resolved after parsing.</summary>
        internal void SetBound(int value) => Bound = value;
    }

    /// <summary>Represents a fixed-size IDL array type.</summary>
    public sealed class Array(IdlType element, IReadOnlyList<int> dimensions)
        : IdlType
    {
        public IdlType Element { get; } = element;
        public IReadOnlyList<int> Dimensions { get; } = dimensions;

    }
}
