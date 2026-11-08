namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>A bound scalar value whose IDL category remains explicit after evaluation.</summary>
internal abstract class IdlScalarValue
{
    /// <summary>Initializes the shared base of a typed scalar value.</summary>
    protected IdlScalarValue()
    {
    }

    /// <summary>Represents an integer value eligible for integer expressions and bounds.</summary>
    internal sealed class Integral(BigInteger value) : IdlScalarValue
    {
        /// <summary>Gets the arbitrary-precision integer validated against its declared type.</summary>
        internal BigInteger Value { get; } = value;
    }

    /// <summary>Represents a boolean value independently of its native numeric representation.</summary>
    internal sealed class Boolean(bool value) : IdlScalarValue
    {
        /// <summary>Gets the boolean value.</summary>
        internal bool Value { get; } = value;
    }

    /// <summary>Represents a character with its narrow or wide IDL identity.</summary>
    internal sealed class Character(char value, bool isWide) : IdlScalarValue
    {
        /// <summary>Gets the character value.</summary>
        internal char Value { get; } = value;

        /// <summary>Gets whether the value has IDL wchar type.</summary>
        internal bool IsWide { get; } = isWide;
    }

    /// <summary>Represents a floating-point value, preserving signed zero.</summary>
    internal sealed class Floating(double value) : IdlScalarValue
    {
        /// <summary>Gets the evaluated floating-point value.</summary>
        internal double Value { get; } = value;
    }

    /// <summary>Represents decoded string content with its narrow or wide IDL identity.</summary>
    internal sealed class Text(string value, bool isWide) : IdlScalarValue
    {
        /// <summary>Gets the decoded and concatenated content.</summary>
        internal string Value { get; } = value;

        /// <summary>Gets whether the value has IDL wstring type.</summary>
        internal bool IsWide { get; } = isWide;
    }

    /// <summary>Represents an enumerator while retaining its declaring enum identity.</summary>
    internal sealed class Enumeration(string typeQualifiedName, string memberName, int value) : IdlScalarValue
    {
        /// <summary>Gets the canonical declaring enum name.</summary>
        internal string TypeQualifiedName { get; } = typeQualifiedName;

        /// <summary>Gets the selected enumerator name.</summary>
        internal string MemberName { get; } = memberName;

        /// <summary>Gets the enumerator's numeric representation.</summary>
        internal int Value { get; } = value;
    }
}
