namespace Wireloom.Dds.Cpp.Emission.Model;

/// <summary>Represents a validated IDL scalar member projection.</summary>
internal sealed record MemberPlan(
    string Name,
    MemberTypePlan Type,
    int MemberId,
    bool IsKey,
    bool IsOptional = false,
    bool IsHashId = false,
    bool IsMustUnderstand = false,
    BigInteger? Minimum = null,
    BigInteger? Maximum = null,
    string? TypeCodeDefaultValue = null,
    string? TypeCodeUnit = null,
    bool IsExternal = false,
    bool HasResolveNameFalse = false)
{
    /// <summary>Gets the public C++ storage type, including optional presence state.</summary>
    internal string StorageType
    {
        get
        {
            var valueType = IsExternal ? $"::dds::core::external<{Type.StorageType}>" : Type.StorageType;
            return IsOptional ? $"::dds::core::optional<{valueType}>" : valueType;
        }
    }

    /// <summary>Gets the member's default initializer expression.</summary>
    internal string DefaultValue => IsOptional ? "{}" : Type.DefaultValue;

    /// <summary>Gets the value-constructor parameter type.</summary>
    internal string ConstructorParameterType => IsExternal
        ? StorageType
        : IsOptional
            ? $"const {StorageType}&"
            : Type.ConstructorParameterType;
}
