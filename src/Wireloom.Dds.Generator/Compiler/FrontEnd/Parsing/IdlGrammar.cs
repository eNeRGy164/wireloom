using System.Text.RegularExpressions;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Regular expressions for the intentionally supported IDL subset.</summary>
internal static class IdlGrammar
{
    internal static readonly Regex ModulePattern = new(
        @"^module\s+([A-Za-z_]\w*)\s*\{",
        RegexOptions.Compiled);

    internal static readonly Regex DefaultNestedAnnotationPattern = new(
        @"^@default_nested\b\s*",
        RegexOptions.Compiled);

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
        @"^@autoid(?:\s*\(\s*(?<value>HASH|SEQUENTIAL)\s*\))?\s*",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    internal static readonly Regex UnknownAnnotationPattern = new(
        @"^@(?<name>[A-Za-z_]\w*)\b(?:\s*\([^()]*\))?\s*",
        RegexOptions.Compiled);

    internal static readonly Regex TopicAnnotationPattern = new(
        @"^@topic\b\s*",
        RegexOptions.Compiled);

    internal static readonly Regex StructPattern = new(
        @"^(?:(?<nested>@nested\s+))?(?:(?<extensibility>@(?:final|appendable|mutable)\s+))?(?:(?<nestedAfter>@nested\s+))?(?:struct|valuetype)\s+(?<name>[A-Za-z_]\w*)(?:\s*:\s*(?<base>(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*))?\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex InterfacePattern = new(
        @"^interface\s+(?<name>[A-Za-z_]\w*)\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex UnionPattern = new(
        @"^(?:(?<nested>@nested\s+))?(?:(?<extensibility>@appendable\s+))?(?:(?<nestedAfter>@nested\s+))?union\s+(?<name>[A-Za-z_]\w*)\s+switch\s*\(\s*(?<discriminator>boolean|char|short|long|unsigned\s+short|unsigned\s+long|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s*\)\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex UnionBranchPattern = new(
        @"^(?:(?<labels>(?:case\s+(?:-?[0-9]+|L'(?:\\.|[^'])'|'(?:\\.|[^'])'|[A-Za-z_]\w*)\s*:\s*)+)|(?<default>default\s*:\s*))(?<type>(?:sequence\s*<\s*[^>]+\s*>|(?:string|wstring)(?:\s*<\s*[0-9]+\s*>)?|unsigned\s+long\s+long|unsigned\s+short|unsigned\s+long|long\s+long|short|long|boolean|char|wchar|float|double|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*))\s+(?<name>[A-Za-z_]\w*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex EnumPattern = new(
        @"^(?:(?<extensibility>@appendable\s+))?enum\s+(?<name>[A-Za-z_]\w*)\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex TypedefPattern = new(
        @"^typedef\s+([^;]+?)\s+([A-Za-z_]\w*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex SequenceTypedefPattern = new(
        @"^typedef\s+sequence\s*<\s*((?:string|wstring)\s*<\s*[^>]+\s*>|[^,>]+)\s*(?:,\s*([^>]+)\s*)?>\s*([A-Za-z_]\w*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex ArrayTypedefPattern = new(
        @"^typedef\s+([^;]+?)\s+([A-Za-z_]\w*)\s*((?:\[[^\]]+\])+?)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex EnumMemberPattern = new(
        @"^(?<defaultLiteral>@default_literal\s+)?(?:@value\s*\(\s*(?<value>-?[0-9]+)\s*\)\s*)?(?<name>[A-Za-z_]\w*)\s*(?:=\s*(?<explicit>-?[0-9]+))?\s*$",
        RegexOptions.Compiled);

    internal static readonly Regex ConstantPattern = new(
        @"^const\s+(?<type>string|wstring|long\s+double|unsigned\s+long\s+long|unsigned\s+long|unsigned\s+short|long\s+long|short|long|int8|int16|int32|int64|uint8|uint16|uint32|uint64|octet|boolean|char|wchar|float|double)\s+(?<name>[A-Za-z_]\w*)\s*=\s*(?<expression>[^;]+);",
        RegexOptions.Compiled);

    internal static readonly Regex MemberPattern = new(
        @"^(?:public\s+)?(sequence\s*<\s*(?:(?:string|wstring)\s*<\s*[^>]+\s*>|[^,>]+)\s*(?:,\s*[^>]+)?\s*>|(?:string|wstring)(?:\s*<\s*((?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*|[0-9]+)\s*>)?|long\s+double|unsigned\s+long\s+long|unsigned\s+short|unsigned\s+long|long\s+long|int8|int16|int32|int64|uint8|uint16|short|long|octet|boolean|char|wchar|float|double|(?:::)?[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s+([A-Za-z_]\w*)\s*((?:\[[^\]]+\])*)\s*;",
        RegexOptions.Compiled);

    internal static readonly Regex KeyAnnotationPattern = new(
        @"^@key\b\s+",
        RegexOptions.Compiled);

    internal static readonly Regex IdAnnotationPattern = new(
        @"^@id\s*\(\s*(-?[0-9]+)\s*\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex OptionalAnnotationPattern = new(
        @"^@optional\b\s*",
        RegexOptions.Compiled);

    internal static readonly Regex MinimumAnnotationPattern = new(
        @"^@min\s*\(\s*(?<value>[^)]+)\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex MaximumAnnotationPattern = new(
        @"^@max\s*\(\s*(?<value>[^)]+)\)\s*",
        RegexOptions.Compiled);

    internal static readonly Regex DefaultAnnotationPattern = new(
        @"^@default\s*\(\s*(?<value>[^)]+)\)\s*",
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
        @"^@external\b\s*",
        RegexOptions.Compiled);

    internal static readonly Regex MustUnderstandAnnotationPattern = new(
        @"^@must_understand\b\s*",
        RegexOptions.Compiled);

    internal static readonly Regex HashIdAnnotationPattern = new(
        "^@hashid(?:\\s*\\(\\s*\\\"(?<value>[^\\\"]*)\\\"\\s*\\))?\\s*",
        RegexOptions.Compiled);

    internal static readonly Regex WhitespacePattern = new("\\s+", RegexOptions.Compiled);
}
