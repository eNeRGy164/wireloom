using System.Globalization;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents one branch of an IDL union and its discriminator labels.</summary>
internal sealed class IdlUnionBranch(IdlMember field, IReadOnlyList<string>? rawLabels, bool isDefault = false)
{
    public IdlMember Field { get; } = field;
    public IReadOnlyList<string> Labels { get; private set; } = [];
    public IReadOnlyList<long> LabelValues { get; private set; } = [];
    public bool IsDefault { get; } = isDefault;

    private static readonly IReadOnlyDictionary<string, (BigInteger Minimum, BigInteger Maximum)> NumericLabelRanges =
        new Dictionary<string, (BigInteger Minimum, BigInteger Maximum)>(StringComparer.Ordinal)
        {
            ["boolean"] = (0, 1),
            ["char"] = (byte.MinValue, byte.MaxValue),
            ["wchar"] = (char.MinValue, char.MaxValue),
            ["int8"] = (sbyte.MinValue, sbyte.MaxValue),
            ["uint8"] = (byte.MinValue, byte.MaxValue),
            ["octet"] = (byte.MinValue, byte.MaxValue),
            ["short"] = (short.MinValue, short.MaxValue),
            ["int16"] = (short.MinValue, short.MaxValue),
            ["unsigned short"] = (ushort.MinValue, ushort.MaxValue),
            ["uint16"] = (ushort.MinValue, ushort.MaxValue),
            ["long"] = (int.MinValue, int.MaxValue),
            ["int32"] = (int.MinValue, int.MaxValue),
            ["unsigned long"] = (uint.MinValue, uint.MaxValue),
            ["uint32"] = (uint.MinValue, uint.MaxValue),
            ["long long"] = (int.MinValue, int.MaxValue),
            ["int64"] = (int.MinValue, int.MaxValue),
            ["unsigned long long"] = (int.MinValue, int.MaxValue),
            ["uint64"] = (int.MinValue, int.MaxValue)
        };

    private IReadOnlyList<string> RawLabels { get; } = rawLabels ?? [];

    /// <summary>Resolves raw discriminator labels after all declarations are known.</summary>
    internal void BindLabels(string? discriminatorIdlType, IdlEnum? discriminatorEnum, IdlSymbolTable symbols, string? currentNamespace)
    {
        var labels = new List<string>();
        var labelValues = new List<long>();

        foreach (var label in RawLabels)
        {
            if ((discriminatorIdlType is "char" or "wchar")
                && IdlCharacterLiteralParser.TryParse(label, discriminatorIdlType == "wchar", out var characterLabel))
            {
                ValidateLabelRange(discriminatorIdlType, label, characterLabel);
                labels.Add(label);
                labelValues.Add(characterLabel);
            }
            else if (discriminatorIdlType == "boolean" && label == "TRUE")
            {
                labels.Add(label);
                labelValues.Add(1);
            }
            else if (discriminatorIdlType == "boolean" && label == "FALSE")
            {
                labels.Add(label);
                labelValues.Add(0);
            }
            else if (discriminatorEnum is not null
                && TryResolveEnumMember(label, discriminatorEnum, symbols, currentNamespace, out var enumMember)
                && enumMember is not null)
            {
                labels.Add(enumMember.Name);
                labelValues.Add(enumMember.Value);
            }
            else if (symbols.TryResolveConstant(label.Trim(), currentNamespace, out var constant)
                && constant.Value is IdlScalarValue.Boolean boolean && discriminatorIdlType == "boolean")
            {
                labels.Add(boolean.Value ? "TRUE" : "FALSE");
                labelValues.Add(boolean.Value ? 1 : 0);
            }
            else if (symbols.TryResolveConstant(label.Trim(), currentNamespace, out constant)
                && constant.Value is IdlScalarValue.Character character
                && (discriminatorIdlType == "wchar" && character.IsWide || discriminatorIdlType == "char" && !character.IsWide))
            {
                labels.Add(((int)character.Value).ToString(CultureInfo.InvariantCulture));
                labelValues.Add(character.Value);
            }
            else if (discriminatorIdlType is not null && TryEvaluateLabel(label, symbols, currentNamespace, out var numericLabel))
            {
                ValidateNumericLabel(discriminatorIdlType, label, numericLabel);

                var labelValue = (long)numericLabel;
                string normalizedLabel;
                if (discriminatorIdlType == "boolean")
                {
                    normalizedLabel = labelValue == 0 ? "FALSE" : "TRUE";
                }
                else
                {
                    normalizedLabel = labelValue.ToString(CultureInfo.InvariantCulture);
                }

                labels.Add(normalizedLabel);
                labelValues.Add(labelValue);
            }
            else
            {
                throw new IdlException(Field.SourceInput!, Field.SourceOffset, $"Unknown union discriminator label: {label}");
            }
        }

        Labels = labels;
        LabelValues = labelValues;
    }

    private static bool TryEvaluateLabel(string label, IdlSymbolTable symbols, string? currentNamespace, out BigInteger value)
    {
        try
        {
            value = IdlConstantExpressionEvaluator.Evaluate(label, symbols, currentNamespace);
            return true;
        }
        catch (FormatException)
        {
            value = BigInteger.Zero;
            return false;
        }
    }

    private bool TryResolveEnumMember(
        string label,
        IdlEnum discriminatorEnum,
        IdlSymbolTable symbols,
        string? currentNamespace,
        out IdlEnumMember? enumMember)
    {
        enumMember = IdlEnumValueResolver.Resolve(label, discriminatorEnum, symbols, currentNamespace, Field.SourceInput!, Field.SourceOffset);
        return enumMember is not null;
    }

    private void ValidateNumericLabel(string discriminatorIdlType, string label, BigInteger value)
    {
        var normalizedType = IdlNaming.NormalizeIdlType(discriminatorIdlType);
        var maximumSupportedLabel = normalizedType is "unsigned long" or "uint32"
            ? uint.MaxValue
            : int.MaxValue;

        if (value < int.MinValue || value > maximumSupportedLabel)
        {
            throw new IdlException(
                Field.SourceInput!,
                Field.SourceOffset,
                $"Numeric union discriminator label '{label}' is outside the supported generated range.");
        }

        ValidateLabelRange(discriminatorIdlType, label, value);
    }

    private void ValidateLabelRange(string discriminatorIdlType, string label, BigInteger value)
    {
        var range = NumericLabelRanges[IdlNaming.NormalizeIdlType(discriminatorIdlType)];

        if (value < range.Minimum || value > range.Maximum)
        {
            throw new IdlException(
                Field.SourceInput!,
                Field.SourceOffset,
                $"Union discriminator label '{label}' is outside the range of {discriminatorIdlType}.");
        }
    }

}

/// <summary>Represents the target-independent semantic form of an IDL union.</summary>
internal sealed class IdlUnion(
    string name,
    string? @namespace,
    string discriminatorIdlType,
    IdlInput discriminatorInput,
    int discriminatorOffset,
    bool discriminatorIsEnum,
    int? discriminatorDefaultValue,
    IReadOnlyList<IdlUnionBranch> branches,
    IdlExtensibilityKind extensibility)
{
    private static readonly HashSet<string> SupportedDiscriminatorPrimitives =
    [
        "boolean",
        "char",
        "wchar",
        "int8",
        "uint8",
        "octet",
        "short",
        "int16",
        "unsigned short",
        "uint16",
        "long",
        "int32",
        "unsigned long",
        "uint32"
    ];

    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorIdlType { get; } = discriminatorIdlType;
    internal IdlInput DiscriminatorInput { get; } = discriminatorInput;
    internal int DiscriminatorOffset { get; } = discriminatorOffset;
    public bool DiscriminatorIsEnum { get; private set; } = discriminatorIsEnum;
    public int? DiscriminatorDefaultValue { get; private set; } = discriminatorDefaultValue;
    /// <summary>Gets the first declared enum value independently of an explicit default literal.</summary>
    internal int? DiscriminatorFirstValue { get; private set; }
    public string? DiscriminatorEnumQualifiedName { get; private set; }
    public string? DiscriminatorPrimitiveIdlType { get; private set; }
    public IReadOnlyList<IdlUnionBranch> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;

    /// <summary>Applies discriminator information resolved during binding.</summary>
    internal void BindDiscriminator(IdlType resolvedType, IdlEnum? discriminatorEnum)
    {
        var underlyingType = UnwrapAlias(resolvedType);

        if (underlyingType is IdlType.Enum enumType && discriminatorEnum is not null)
        {
            DiscriminatorIsEnum = true;
            DiscriminatorDefaultValue = enumType.DefaultValue;
            DiscriminatorFirstValue = discriminatorEnum.Members[0].Value;
            DiscriminatorEnumQualifiedName = enumType.QualifiedName;
            return;
        }

        if (underlyingType is IdlType.Primitive primitive)
        {
            if (!IsSupportedDiscriminatorPrimitive(primitive.Name))
            {
                throw new IdlException(DiscriminatorInput, DiscriminatorOffset, $"Unsupported union discriminator type: {DiscriminatorIdlType}");
            }

            DiscriminatorIsEnum = false;
            DiscriminatorDefaultValue = null;
            DiscriminatorPrimitiveIdlType = primitive.Name;
            return;
        }

        throw new IdlException(DiscriminatorInput, DiscriminatorOffset, $"Unsupported union discriminator type: {DiscriminatorIdlType}");
    }

    private static bool IsSupportedDiscriminatorPrimitive(string name) =>
        SupportedDiscriminatorPrimitives.Contains(name);

    private static IdlType UnwrapAlias(IdlType type)
    {
        if (type is IdlType.Alias alias)
        {
            return UnwrapAlias(alias.Target);
        }

        return type;
    }
}
