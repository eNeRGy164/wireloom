using System.Globalization;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents one branch of an IDL union and its discriminator labels.</summary>
internal sealed class IdlUnionBranch(IdlMember field, IReadOnlyList<string>? rawLabels, bool isDefault = false)
{
    public IdlMember Field { get; } = field;
    public IReadOnlyList<string> Labels { get; private set; } = [];
    public IReadOnlyList<int> LabelValues { get; private set; } = [];
    public bool IsDefault { get; } = isDefault;

    private static readonly IReadOnlyDictionary<string, (BigInteger Minimum, BigInteger Maximum)> NumericLabelRanges =
        new Dictionary<string, (BigInteger Minimum, BigInteger Maximum)>(StringComparer.Ordinal)
        {
            ["boolean"] = (0, 1),
            ["char"] = (char.MinValue, char.MaxValue),
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
            ["unsigned long"] = (int.MinValue, int.MaxValue),
            ["uint32"] = (int.MinValue, int.MaxValue),
            ["long long"] = (int.MinValue, int.MaxValue),
            ["int64"] = (int.MinValue, int.MaxValue),
            ["unsigned long long"] = (int.MinValue, int.MaxValue),
            ["uint64"] = (int.MinValue, int.MaxValue)
        };

    private IReadOnlyList<string> RawLabels { get; } = rawLabels ?? [];

    /// <summary>Resolves raw discriminator labels after all declarations are known.</summary>
    internal void BindLabels(string? discriminatorIdlType, IdlEnum? discriminatorEnum)
    {
        var labels = new List<string>();
        var labelValues = new List<int>();

        foreach (var label in RawLabels)
        {
            if ((discriminatorIdlType is "char" or "wchar")
                && TryParseCharacterLabel(label, discriminatorIdlType == "wchar", out var characterLabel))
            {
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
            else if (BigInteger.TryParse(label, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numericLabel))
            {
                if (discriminatorIdlType is null)
                {
                    throw new IdlException(
                        Field.SourceInput!,
                        Field.SourceOffset,
                        $"Unknown union discriminator label: {label}");
                }

                ValidateNumericLabel(discriminatorIdlType, label, numericLabel);

                var labelValue = (int)numericLabel;
                labels.Add(labelValue.ToString(CultureInfo.InvariantCulture));
                labelValues.Add(labelValue);
            }
            else
            {
                var enumMember = discriminatorEnum?.Members.SingleOrDefault(member => member.Name == label);
                if (enumMember is null)
                {
                    throw new IdlException(
                        Field.SourceInput!,
                        Field.SourceOffset,
                        $"Unknown union discriminator label: {label}");
                }

                labels.Add(label);
                labelValues.Add(enumMember.Value);
            }
        }

        Labels = labels;
        LabelValues = labelValues;
    }

    private void ValidateNumericLabel(string discriminatorIdlType, string label, BigInteger value)
    {
        if (value < int.MinValue || value > int.MaxValue)
        {
            throw new IdlException(
                Field.SourceInput!,
                Field.SourceOffset,
                $"Numeric union discriminator label '{label}' is outside the supported generated range.");
        }

        var normalizedType = IdlNaming.NormalizeIdlType(discriminatorIdlType);
        var range = NumericLabelRanges[normalizedType];

        if (value < range.Minimum || value > range.Maximum)
        {
            throw new IdlException(
                Field.SourceInput!,
                Field.SourceOffset,
                $"Numeric union discriminator label '{label}' is outside the range of {discriminatorIdlType}.");
        }
    }

    private static bool TryParseCharacterLabel(string label, bool allowWideLiteral, out int value)
    {
        value = 0;
        var isWideLiteral = label.StartsWith("L'", StringComparison.Ordinal);
        if (isWideLiteral && !allowWideLiteral)
        {
            return false;
        }

        var contentStart = isWideLiteral ? 2 : 1;
        if (label.Length < contentStart + 2 || label[contentStart - 1] != '\'' || label[^1] != '\'')
        {
            return false;
        }

        var content = label[contentStart..^1];
        var character = content switch
        {
            "\\n" => '\n',
            "\\r" => '\r',
            "\\t" => '\t',
            "\\\\" => '\\',
            "\\'" => '\'',
            _ when content.Length == 1 => content[0],
            _ => '\0'
        };

        if (content.Length != 1 && character == '\0')
        {
            return false;
        }

        value = character;

        return true;
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
        "uint32",
        "long long",
        "int64",
        "unsigned long long",
        "uint64"
    ];

    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorIdlType { get; } = discriminatorIdlType;
    internal IdlInput DiscriminatorInput { get; } = discriminatorInput;
    internal int DiscriminatorOffset { get; } = discriminatorOffset;
    public bool DiscriminatorIsEnum { get; private set; } = discriminatorIsEnum;
    public int? DiscriminatorDefaultValue { get; private set; } = discriminatorDefaultValue;
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
