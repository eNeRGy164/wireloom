using System.Text.RegularExpressions;

namespace Wireloom;

/// <summary>Regular expressions for the intentionally supported IDL subset.</summary>
internal static class IdlGrammar
{
    internal static readonly Regex ModulePattern = new(
        @"^module\s+([A-Za-z_]\w*)\s*\{",
        RegexOptions.Compiled);
    internal static readonly Regex DefaultNestedAnnotationPattern = new(
        @"^@default_nested\b\s*",
        RegexOptions.Compiled);
    internal static readonly Regex StructPattern = new(
        @"^(?:(?<extensibility>@(?:final|appendable|mutable)\s+))?struct\s+(?<name>[A-Za-z_]\w*)(?:\s*:\s*(?<base>[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*))?\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);
    internal static readonly Regex UnionPattern = new(
        @"^(?:(?<extensibility>@appendable\s+))?union\s+(?<name>[A-Za-z_]\w*)\s+switch\s*\(\s*(?<discriminator>boolean|char|short|long|unsigned\s+short|unsigned\s+long|[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s*\)\s*\{(?<body>[^{}]*)\}\s*;",
        RegexOptions.Compiled);
    internal static readonly Regex UnionBranchPattern = new(
        @"^(?:(?<labels>(?:case\s+(?:-?[0-9]+|[A-Za-z_]\w*)\s*:\s*)+)|(?<default>default\s*:\s*))(?<type>(?:sequence\s*<\s*[^>]+\s*>|(?:string|wstring)(?:\s*<\s*[0-9]+\s*>)?|unsigned\s+long\s+long|unsigned\s+short|unsigned\s+long|long\s+long|short|long|boolean|char|wchar|float|double|[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*))\s+(?<name>[A-Za-z_]\w*)\s*;",
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
        @"^(?:@value\s*\(\s*(-?[0-9]+)\s*\)\s*)?([A-Za-z_]\w*)\s*(?:=\s*(-?[0-9]+))?\s*$",
        RegexOptions.Compiled);
    internal static readonly Regex ConstantPattern = new(
        @"^const\s+(?<type>string|wstring|long\s+double|unsigned\s+long\s+long|unsigned\s+long|unsigned\s+short|long\s+long|short|long|int8|int16|int32|int64|uint8|uint16|uint32|uint64|octet|boolean|char|wchar|float|double)\s+(?<name>[A-Za-z_]\w*)\s*=\s*(?<expression>[^;]+);",
        RegexOptions.Compiled);
    internal static readonly Regex MemberPattern = new(
        @"^(sequence\s*<\s*(?:(?:string|wstring)\s*<\s*[^>]+\s*>|[^,>]+)\s*(?:,\s*[^>]+)?\s*>|(?:string|wstring)(?:\s*<\s*([A-Za-z_]\w*(?:::[A-Za-z_]\w*)*|[0-9]+)\s*>)?|long\s+double|unsigned\s+long\s+long|unsigned\s+short|unsigned\s+long|long\s+long|int8|int16|int32|int64|uint8|uint16|short|long|octet|boolean|char|wchar|float|double|[A-Za-z_]\w*(?:::[A-Za-z_]\w*)*)\s+([A-Za-z_]\w*)\s*((?:\[[^\]]+\])*)\s*;",
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
    internal static readonly Regex WhitespacePattern = new("\\s+", RegexOptions.Compiled);
}
