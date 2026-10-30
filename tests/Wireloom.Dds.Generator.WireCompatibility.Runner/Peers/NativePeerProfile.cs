namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Native fixture selection and CMake switches for an exact case/profile pair.</summary>
/// <param name="CppType">Type substituted into the authored native peer template.</param>
/// <param name="Options">CMake definitions applied to this case.</param>
internal sealed record NativePeerProfile(string CppType, IReadOnlyList<string> Options)
{
    private static readonly IReadOnlyDictionary<string, string> FixtureFlags = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["09-optional-string-sequences"] = "WIRELOOM_TYPED_STRING_SEQUENCE_FIXTURE",
        ["04-strings"] = "WIRELOOM_TYPED_WIDE_STRINGS_FIXTURE",
        ["04-unbounded-string-aliases"] = "WIRELOOM_TYPED_UNBOUNDED_STRING_ALIASES",
        ["10-resolve-name-wstring-aliases"] = "WIRELOOM_TYPED_RESOLVE_NAME_WSTRING_ALIASES",
        ["10-default-literal-enum-array"] = "WIRELOOM_TYPED_DEFAULT_LITERAL_ENUM_ARRAY",
        ["07-union-char-numeric"] = "WIRELOOM_TYPED_NUMERIC_CHAR_UNION",
        ["07-union-char-numeric-255"] = "WIRELOOM_TYPED_NUMERIC_CHAR_UNION",
        ["04-string-alias-chains"] = "WIRELOOM_TYPED_STRING_ALIAS_CHAINS",
        ["05-unbounded-sequence-aliases"] = "WIRELOOM_TYPED_UNBOUNDED_SEQUENCE_ALIASES",
        ["05-array-of-sequences-idl"] = "WIRELOOM_TYPED_SEQUENCE_ARRAYS",
        ["07-union-string-sequence-aliases"] = "WIRELOOM_TYPED_UNION_STRING_SEQUENCE_ALIASES",
        ["07-union-string-sequence-bounded-direct"] = "WIRELOOM_TYPED_UNION_STRING_SEQUENCE_BOUNDED",
        ["14-extensibility-full"] = "WIRELOOM_TYPED_C163_EXTENSIBILITY",
        ["14-autoid-scope"] = "WIRELOOM_TYPED_C164_AUTOID_SCOPE",
        ["14-annotation-arguments"] = "WIRELOOM_TYPED_C165_ANNOTATION_ARGUMENTS",
        ["14-member-declarators"] = "WIRELOOM_TYPED_C166_MEMBER_DECLARATORS",
        ["14-union-label-expressions"] = "WIRELOOM_TYPED_C167_UNION_LABELS",
        ["14-union-scoped-enum"] = "WIRELOOM_TYPED_C168_SCOPED_ENUM",
        ["14-union-array-branch"] = "WIRELOOM_TYPED_C169_UNION_ARRAY",
        ["14-string-expression-bound"] = "WIRELOOM_TYPED_C170_STRING_BOUND",
        ["14-typed-defaults"] = "WIRELOOM_TYPED_C171_DEFAULTS",
        ["14-enum-constant"] = "WIRELOOM_TYPED_C172_ENUM_CONSTANT",
        ["14-unbounded-support"] = "WIRELOOM_TYPED_C173_UNBOUNDED_SUPPORT",
        ["14-unbounded-receive-lifecycle"] = "WIRELOOM_TYPED_C174_UNBOUNDED_RECEIVE_LIFECYCLE",
        ["04-boundaries"] = "WIRELOOM_TYPED_STRING_BOUNDARIES_FIXTURE",
        ["09-optional-aggregate-member"] = "WIRELOOM_TYPED_OPTIONAL_AGGREGATE_FIXTURE",
        ["09-optional-recursive-array"] = "WIRELOOM_TYPED_OPTIONAL_RECURSIVE_ARRAY_FIXTURE",
    };

    /// <summary>Resolves case-specific fixture flags without modifying staged sources.</summary>
    internal static NativePeerProfile ForCase(string caseId, string rtiVersion, string cppType)
    {
        var options = new List<string>();
        if (rtiVersion == "7.3.1")
        {
            options.Add("-DWIRELOOM_RTI_7_3_1=ON");
        }

        var ccache = Environment.GetEnvironmentVariable("PATH")?.Split(Path.PathSeparator).Select(directory => Path.Combine(directory, "ccache")).FirstOrDefault(File.Exists);
        if (ccache is not null)
        {
            options.Add($"-DCMAKE_CXX_COMPILER_LAUNCHER={ccache}");
        }

        if (caseId == "07-union-wchar-label")
        {
            cppType = "dds::core::xtypes::DynamicData";
            options.Add("-DWIRELOOM_DYNAMIC_WCHAR_UNION=ON");
        }

        if (FixtureFlags.TryGetValue(caseId, out var fixtureFlag))
        {
            options.Add($"-D{fixtureFlag}=ON");
        }

        if (caseId == "07-union-char-numeric-255")
        {
            options.Add("-DWIRELOOM_UNSIGNED_CHAR_NUMERIC_UNION=ON");
        }

        return new NativePeerProfile(cppType, options);
    }
}
