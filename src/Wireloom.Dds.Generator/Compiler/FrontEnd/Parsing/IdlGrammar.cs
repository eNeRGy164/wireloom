using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Regular expressions for the intentionally supported IDL subset.</summary>
internal static class IdlGrammar
{
    internal static readonly Regex ModulePattern = new(
        @"^module\s+([A-Za-z_]\w*)\s*\{",
        RegexOptions.Compiled);

    internal static readonly Regex DefaultNestedAnnotationPattern = new(
        @"^@default_nested(?:\s*\(\s*(?:true|false)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex LanguageBindingAnnotationPattern = new(
        @"^@language_binding\s*\(\s*[A-Za-z_]\w*\s*\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex TransferModeAnnotationPattern = new(
        @"^@transfer_mode\s*\(\s*[A-Za-z_]\w*\s*\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex DataRepresentationAnnotationPattern = new(
        @"^@data_representation\s*\(\s*[A-Za-z_]\w*\s*\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex AllowedDataRepresentationAnnotationPattern = new(
        @"^@allowed_data_representation\s*\(\s*[A-Za-z_]\w*\s*\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex AutoIdAnnotationPattern = new(
        @"^@autoid(?:\s*\(\s*(?:value\s*=\s*)?(?<value>HASH|SEQUENTIAL)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex ServiceAnnotationPattern = new(
        @"^@service\s*\(\s*""(?<value>[^""]+)""\s*\)\s*",
        RegexOptions.Compiled);

    /// <summary>Matches supported type annotations while retaining their argument values.</summary>
    internal static readonly Regex TypePrefixAnnotationPattern = new(
        @"^@(?:(?<topic>topic)\b|(?<autoid>autoid)(?:\s*\(\s*(?:value\s*=\s*)?(?<autoidValue>HASH|SEQUENTIAL)\s*\))?|(?<extensibility>final|appendable|mutable)\b|(?<fullExtensibility>extensibility)\s*\(\s*(?<fullExtensibilityValue>FINAL|APPENDABLE|MUTABLE)\s*\)|(?<nested>nested)(?:\s*\(\s*(?:true|false)\s*\))?)\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex StructPattern = new(
        @"^(?:struct|valuetype)\s+(?<name>[A-Za-z_]\w*)(?:\s*:\s*(?<base>(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*))?\s*\{",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex InterfacePattern = new(
        @"^interface\s+(?<name>[A-Za-z_]\w*)\s*\{",
        RegexOptions.Compiled);

    internal static readonly Regex UnionPattern = new(
        @"^union\s+(?<name>[A-Za-z_]\w*)\s+switch\s*\(\s*(?<discriminator>boolean|char|short|long|unsigned\s+short|unsigned\s+long|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s*\)\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex UnionBranchPattern = new(
        @"^(?:(?<labels>(?:case\s+(?:L'(?:\\.|[^'])*'|'(?:\\.|[^'])*'|(?:(?:[^;:'\x22]|::)+?))(?<!:)\s*:(?!:)\s*)+)|(?<default>default\s*:\s*))(?:(?<optional>@optional\s+))?(?<type>(?:sequence\s*<\s*(?:(?:string|wstring)\s*<\s*(?:[^<>]|\([^()]*\))+\s*>|[^,>]+)\s*(?:,\s*[^>]+)?\s*>|(?:string|wstring)(?:\s*<\s*(?:[^<>]|\([^()]*\))+\s*>)?|unsigned\s+long\s+long|unsigned\s+short|unsigned\s+long|long\s+long|short|long|boolean|char|wchar|float|double|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*))\s+(?<name>[A-Za-z_]\w*)\s*(?<dimensions>(?:\[[^\]]+\])*)\s*;",
        RegexOptions.Compiled);

    /// <summary>Matches a complete union case expression without splitting scoped names or quoted labels.</summary>
    internal static readonly Regex UnionCaseLabelPattern = new(
        @"case\s+(?<label>L'(?:\\.|[^'])*'|'(?:\\.|[^'])*'|(?:(?:[^;:'\x22]|::)+?))(?<!:)\s*:(?!:)",
        RegexOptions.Compiled);

    internal static readonly Regex EnumPattern = new(
        @"^enum\s+(?<name>[A-Za-z_]\w*)\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex TypedefPattern = new(
        @"^typedef\s+([^;]+?)\s+([A-Za-z_]\w*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex SequenceTypedefPattern = new(
        @"^typedef\s+sequence\s*<\s*((?:string|wstring)\s*<\s*(?:[^<>]|\([^()]*\))+\s*>|[^,>]+)\s*(?:,\s*([^>]+)\s*)?>\s*([A-Za-z_]\w*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex ArrayTypedefPattern = new(
        @"^typedef\s+([^;]+?)\s+([A-Za-z_]\w*)\s*((?:\[[^\]]+\])+?)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex EnumMemberPattern = new(
        @"^(?<defaultLiteral>@default_literal\s+)?(?:@value\s*\(\s*(?<value>-?[0-9]+)\s*\)\s*)?(?<name>[A-Za-z_]\w*)\s*(?:=\s*(?<explicit>-?[0-9]+))?\s*$",
        RegexOptions.Compiled);

    internal static readonly Regex ConstantPattern = new(
        @"^const\s+(?<type>string|wstring|long\s+double|unsigned\s+long\s+long|unsigned\s+long|unsigned\s+short|long\s+long|short|long|int8|int16|int32|int64|uint8|uint16|uint32|uint64|octet|boolean|char|wchar|float|double|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s+(?<name>[A-Za-z_]\w*)\s*=\s*(?<expression>[^;]+);",
        RegexOptions.Compiled);

    internal static readonly Regex MemberPattern = new(
        @"^(?:public\s+)?(sequence\s*<\s*(?:(?:string|wstring)\s*<\s*(?:[^<>]|\([^()]*\))+\s*>|[^,>]+)\s*(?:,\s*[^>]+)?\s*>|(?:string|wstring)(?:\s*<\s*((?:[^<>]|\([^()]*\))+)\s*>)?|long\s+double|unsigned\s+long\s+long|unsigned\s+short|unsigned\s+long|long\s+long|int8|int16|int32|int64|uint8|uint16|short|long|octet|boolean|char|wchar|float|double|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s+([A-Za-z_]\w*)\s*((?:\[[^\]]+\])*)(?<rest>(?:\s*,\s*[A-Za-z_]\w*\s*(?:\[[^\]]+\])*)*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex KeyAnnotationPattern = new(
        @"^@key(?:\s*\(\s*(?:value\s*=\s*)?(?<value>true|false)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex OptionalAnnotationPattern = new(
        @"^@optional(?:\s*\(\s*(?:value\s*=\s*)?(?<value>true|false)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex MinimumAnnotationPattern = new(
        @"^@min\s*\(\s*(?<value>[^)]+)\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex MaximumAnnotationPattern = new(
        @"^@max\s*\(\s*(?<value>[^)]+)\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex RangeAnnotationPattern = new(
        @"^@range\s*\(\s*min\s*=\s*(?<min>[^,]+)\s*,\s*max\s*=\s*(?<max>[^)]+)\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex UnitAnnotationPattern = new(
        "^@unit\\s*\\(\\s*\\\"(?<value>[^\\\"]*)\\\"\\s*\\)\\s*",
        RegexOptions.Compiled);

    internal static readonly Regex ResolveNameAnnotationPattern = new(
        @"^@resolve_name\s*\(\s*(?:true|false)\s*\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex ExternalAnnotationPattern = new(
        @"^@external(?:\s*\(\s*(?:value\s*=\s*)?(?<value>true|false)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex MustUnderstandAnnotationPattern = new(
        @"^@must_understand(?:\s*\(\s*(?:value\s*=\s*)?(?<value>true|false)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex HashIdAnnotationPattern = new(
        "^@hashid(?:\\s*\\(\\s*\\\"(?<value>[^\\\"]*)\\\"\\s*\\))?\\s*",
        RegexOptions.Compiled);

    internal static readonly Regex WhitespacePattern = new("\\s+", RegexOptions.Compiled);
}
