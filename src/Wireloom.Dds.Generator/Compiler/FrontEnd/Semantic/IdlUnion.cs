using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.FrontEnd.Semantic;

/// <summary>Represents one branch of an IDL union and its discriminator labels.</summary>
internal sealed class IdlUnionBranch(IdlMember field, IReadOnlyList<string>? rawLabels, bool isDefault = false)
{
    public IdlMember Field { get; } = field;
    public IReadOnlyList<string> Labels { get; private set; } = [];
    public IReadOnlyList<int> LabelValues { get; private set; } = [];
    public bool IsDefault { get; } = isDefault;

    private IReadOnlyList<string> RawLabels { get; } = rawLabels ?? [];

    /// <summary>Resolves raw discriminator labels after all declarations are known.</summary>
    internal void BindLabels(string discriminatorIdlType, string? currentNamespace, IdlEnum? discriminatorEnum)
    {
        var labels = new List<string>();
        var labelValues = new List<int>();
        var discriminatorQualified = IdlNaming.ResolveTypeName(discriminatorIdlType, currentNamespace);

        foreach (var label in RawLabels)
        {
            if (discriminatorIdlType == "char" && TryParseCharacterLabel(label, out var characterLabel))
            {
                labels.Add(label);
                labelValues.Add(characterLabel);
            }
            else if (discriminatorIdlType == "boolean" && label == "TRUE")
            {
                labels.Add("true");
                labelValues.Add(1);
            }
            else if (discriminatorIdlType == "boolean" && label == "FALSE")
            {
                labels.Add("false");
                labelValues.Add(0);
            }
            else if (int.TryParse(label, out var numericLabel))
            {
                labels.Add(numericLabel.ToString());
                labelValues.Add(numericLabel);
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

                labels.Add($"{IdlNaming.EscapeQualifiedIdentifier(discriminatorQualified)}.{IdlNaming.EscapeIdentifier(label)}");
                labelValues.Add(enumMember.Value);
            }
        }

        Labels = labels;
        LabelValues = labelValues;
    }

    private static bool TryParseCharacterLabel(string label, out int value)
    {
        value = 0;
        if (label.Length < 3 || label[0] != '\'' || label[^1] != '\'')
        {
            return false;
        }

        var content = label[1..^1];
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
internal sealed class IdlUnion(string name, string? @namespace, string discriminatorIdlType, bool discriminatorIsEnum, int? discriminatorDefaultValue, IReadOnlyList<IdlUnionBranch> branches, IdlExtensibilityKind extensibility)
{
    public string Name { get; } = name;
    public string? Namespace { get; } = @namespace;
    public string DiscriminatorIdlType { get; } = discriminatorIdlType;
    public bool DiscriminatorIsEnum { get; private set; } = discriminatorIsEnum;
    public int? DiscriminatorDefaultValue { get; private set; } = discriminatorDefaultValue;
    public IReadOnlyList<IdlUnionBranch> Branches { get; } = branches;
    public IdlExtensibilityKind Extensibility { get; } = extensibility;

    /// <summary>Applies discriminator information resolved during binding.</summary>
    internal void BindDiscriminator(bool isEnum, int? defaultValue)
    {
        DiscriminatorIsEnum = isEnum;
        DiscriminatorDefaultValue = defaultValue;
    }
}
