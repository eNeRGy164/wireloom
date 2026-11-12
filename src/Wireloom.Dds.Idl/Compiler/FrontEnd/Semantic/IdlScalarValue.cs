namespace Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

/// <summary>A bound scalar value whose IDL category remains explicit after evaluation.</summary>
public abstract class IdlScalarValue
{
    /// <summary>Initializes the shared base of a typed scalar value.</summary>
    private protected IdlScalarValue()
    {
    }

    /// <summary>Represents an integer value eligible for integer expressions and bounds.</summary>
    public sealed class Integral(BigInteger value) : IdlScalarValue
    {
        /// <summary>Gets the arbitrary-precision integer value.</summary>
        public BigInteger Value { get; } = value;
    }

    /// <summary>Represents a boolean value independently of its native numeric representation.</summary>
    public sealed class Boolean(bool value) : IdlScalarValue
    {
        /// <summary>Gets the scalar boolean value.</summary>
        public bool Value { get; } = value;
    }

    /// <summary>Represents a character with its narrow or wide IDL identity.</summary>
    public sealed class Character(char value, bool isWide) : IdlScalarValue
    {
        /// <summary>Gets the scalar character value.</summary>
        public char Value { get; } = value;

        /// <summary>Gets whether the character has IDL <c>wchar</c> type.</summary>
        public bool IsWide { get; } = isWide;
    }

    /// <summary>Represents a floating-point value, preserving signed zero.</summary>
    public sealed class Floating(double value) : IdlScalarValue
    {
        /// <summary>Gets the evaluated floating-point value, including signed zero.</summary>
        public double Value { get; } = value;
    }

    /// <summary>Represents decoded string content with its narrow or wide IDL identity.</summary>
    public sealed class Text(string value, bool isWide) : IdlScalarValue
    {
        /// <summary>Gets the decoded and concatenated string content.</summary>
        public string Value { get; } = value;

        /// <summary>Gets whether the text has IDL <c>wstring</c> type.</summary>
        public bool IsWide { get; } = isWide;
    }

    /// <summary>Represents an enumerator while retaining its declaring enum identity.</summary>
    public sealed class Enumeration(string typeQualifiedName, string memberName, int value) : IdlScalarValue
    {
        /// <summary>Gets the fully qualified name of the declaring enum.</summary>
        public string TypeQualifiedName { get; } = typeQualifiedName;

        /// <summary>Gets the selected enumerator's name.</summary>
        public string MemberName { get; } = memberName;

        /// <summary>Gets the selected enumerator's numeric value.</summary>
        public int Value { get; } = value;
    }
}
