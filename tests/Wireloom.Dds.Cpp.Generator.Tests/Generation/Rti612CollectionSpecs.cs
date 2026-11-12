namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Checks the retained RTI 6.1.2 collection adapter boundary.</summary>
public sealed class Rti612CollectionSpecs
{
    private sealed class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("wireloom-cpp-rti612-tests-");

        internal string Path => directory.FullName;

        public void Dispose() => directory.Delete(recursive: true);
    }

    /// <summary>Emits primitive and string collection metadata for RTI 6.1.2.</summary>
    [Fact]
    public void EmitsQualifiedCollectionsForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "enum Color { RED, GREEN, BLUE }; typedef long LongAlias; typedef Color ColorAlias; typedef sequence<long, 4> LongSequence; typedef string<16> Label; struct Sample { long fixed_values[3]; sequence<long, 4> values; string<16> label; string default_bound_label; sequence<string<16>, 2> labels; Color color; sequence<Color, 2> colors; LongAlias alias_value; ColorAlias alias_color; LongSequence alias_values; Label alias_label; @optional long optional_value; @optional string<16> optional_label; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        header.ShouldContain("#include <rti/core/BoundedSequence.hpp>");
        header.ShouldContain("using bounded_sequence = ::rti::core::bounded_sequence<T, N>;");
        plugin.ShouldContain("::dds::core::xtypes::ArrayType(");
        plugin.ShouldContain("::dds::core::xtypes::SequenceType(");
        plugin.ShouldContain("::dds::core::xtypes::EnumType type;");
        plugin.ShouldContain("::rti::topic::dynamic_type<::Color>::get()");
        plugin.ShouldContain("::LongAlias_AliasTag_t>::type type(");
        plugin.ShouldContain("dynamic_type<::LongAlias_AliasTag_t>::get()");
        plugin.ShouldContain("dynamic_type<::ColorAlias_AliasTag_t>::get()");
        plugin.ShouldContain("dynamic_type<::LongSequence_AliasTag_t>::get()");
        plugin.ShouldContain("dynamic_type<::Label_AliasTag_t>::get()");
        plugin.ShouldContain(".optional(true)");
        plugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        plugin.ShouldContain("::dds::core::xtypes::StringType(255)");
        plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::string, 2 >>::sample_access_info()");
        plugin.ShouldContain("_representation._typeCode->_sampleAccessInfo =");
        plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::int32_t, 4 >>::sample_access_info()");
        plugin.ShouldContain("_representation._typeCode->_typeCode->_sampleAccessInfo =");
        plugin.ShouldContain("WireloomRti612TypePlugin<::Sample>::create");
    }

    /// <summary>Emits RTI 6.1.2 TypeCode and sample access for wide strings and collections.</summary>
    [Fact]
    public void EmitsWideStringsForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "measurement.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Measurement { wstring<24> title; sequence<wstring<12>, 2> translations; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();
        var plugin = File.ReadAllText(Path.Combine(outputPath, "measurementPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::WStringType(24)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(12)");
        plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::wstring, 2 >>::sample_access_info()");
        plugin.ShouldContain("sequence_helper<::std::wstring>::sample_access_info()");
    }

    /// <summary>Emits multidimensional arrays as one ranked RTI 6.1.2 array type.</summary>
    [Fact]
    public void EmitsMultidimensionalArrayTypeCodeForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "arrays.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "typedef long Grid[2][3]; struct Sample { long values[2][3]; Grid grid; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "arraysPlugin.cxx"));
        plugin.ShouldContain("ArrayType(::dds::core::xtypes::primitive_type<::std::int32_t>(), { 2, 3 })");
        plugin.ShouldNotContain("ArrayType(::dds::core::xtypes::ArrayType(");
    }

    /// <summary>Emits the RTI 6.1.2 DynamicType pointer flag for an external member.</summary>
    [Fact]
    public void EmitsExternalMemberMetadataForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "external.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { @external string<16> text; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(outputPath, "external.hpp"));
        header.ShouldContain("::dds::core::external<::std::string> text = {};");

        var plugin = File.ReadAllText(Path.Combine(outputPath, "externalPlugin.cxx"));
        plugin.ShouldContain("Member(\"text\", ::dds::core::xtypes::StringType(16)).id(0).pointer(true)");
    }

    /// <summary>Initializes sample access for aggregate TypeCodes nested inside another aggregate.</summary>
    [Fact]
    public void EmitsNestedAggregateSampleAccessForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "nested.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Inner { long value; }; struct Middle { Inner inner; }; struct Root { Middle middle; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "nestedPlugin.cxx"));
        plugin.ShouldContain("native_type_code->_members[0]._representation._typeCode->_members[0]._representation._typeCode->_sampleAccessInfo =");
        plugin.ShouldContain("native_type_code->_members[0]._representation._typeCode->_members[0]._representation._typeCode->_typePlugin =");
    }

    /// <summary>Preserves explicitly named typedef metadata for resolve-name annotations.</summary>
    [Fact]
    public void EmitsResolveNameFalseAliasMetadataForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "aliases.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "const long WSTRING_BOUND = 8; typedef wstring<WSTRING_BOUND> BoundedWideText; typedef BoundedWideText BoundedWideTextAlias; struct Sample { @resolve_name(false) BoundedWideText firstLevel; @resolve_name(false) BoundedWideTextAlias secondLevel; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "aliasesPlugin.cxx"));
        plugin.ShouldContain("dynamic_type<::BoundedWideText_AliasTag_t>::get()");
        plugin.ShouldContain("dynamic_type<::BoundedWideTextAlias_AliasTag_t>::get()");
        plugin.ShouldContain("Member(\"firstLevel\"");
        plugin.ShouldContain("Member(\"secondLevel\"");
    }

    /// <summary>Emits the 6.1.2-compatible shape of RTI key, range, unit, resolve-name, and external annotations.</summary>
    [Fact]
    public void EmitsRtiAnnotationsForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "annotations.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "@language_binding(PLAIN) @transfer_mode(INBAND) @topic struct Sample { @key long id; @range(min = -32, max = 31) @unit(\"meters\") long value; @resolve_name(false) string<16> text; @external string<16> externalText; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "annotationsPlugin.cxx"));
        plugin.ShouldContain(".id(0).key(true)");
        plugin.ShouldContain("_annotations._minValue._u.long_value = -32L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 31L;");
        plugin.ShouldContain("Member(\"text\"");
        plugin.ShouldContain("Member(\"externalText\"");
        plugin.ShouldContain("pointer(true)");
    }

    /// <summary>Emits nested aggregate sample access metadata for RTI 6.1.2.</summary>
    [Fact]
    public void EmitsNestedAggregateForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "nested.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "@nested struct NestedValue { long value; }; struct Topic { NestedValue nested; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "nestedPlugin.cxx"));
        plugin.ShouldContain("dynamic_type<::NestedValue>::get()");
        plugin.ShouldContain("_representation._typeCode->_sampleAccessInfo =");
    }

    /// <summary>Emits keyed topic callbacks for the RTI 6.1.2 plugin.</summary>
    [Fact]
    public void EmitsKeyCallbacksForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { @key long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("WireloomRti612KeyCallbacks<::Sample>");
        plugin.ShouldContain("PRES_TYPEPLUGIN_USER_KEY");
        plugin.ShouldContain("plugin->instanceToKeyFnc = KeyCallbacks::instance_to_key;");
        plugin.ShouldContain("typed_destination->value = typed_source->value;");
        plugin.ShouldContain("Member(\"value\", ::dds::core::xtypes::primitive_type<::std::int32_t>()).id(0).key(true)");
    }

    /// <summary>Emits optional sequence and array metadata for RTI 6.1.2.</summary>
    [Fact]
    public void EmitsOptionalCollectionsForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Measurement { @id(17) @optional sequence<long, 4> readings; @id(23) @optional long coordinates[2]; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("Member(\"readings\"");
        plugin.ShouldContain("Member(\"coordinates\"");
        plugin.ShouldContain(".id(17).optional(true)");
        plugin.ShouldContain(".id(23).optional(true)");
        plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::int32_t, 4 >>::sample_access_info()");
    }

    /// <summary>Emits optional primitive collections across bounds, lengths, and sequence mapping modes.</summary>
    [Theory]
    [InlineData("sequence<short, 4> readings")]
    [InlineData("sequence<long, 5> readings")]
    [InlineData("sequence<long> readings")]
    [InlineData("long readings[3]")]
    public void EmitsOptionalPrimitiveCollectionsForRti612(string memberDeclaration)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, $"struct Measurement {{ @id(37) @optional {memberDeclaration}; }};");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", input, outputPath, "--rti-version", "6.1.2", "--unbounded-support"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("Member(\"readings\"");
        plugin.ShouldContain(".id(37).optional(true)");
    }

    /// <summary>Emits union metadata and branch sample access for RTI 6.1.2.</summary>
    [Fact]
    public void EmitsUnionTypeCodeForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "union Choice switch(long) { case 0: long number; default: string<16> text; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        header.ShouldContain("#include <string>");
        header.ShouldNotContain("omg/types/string_view.hpp");
        header.ShouldContain("void text(const ::std::string& value");
        header.ShouldContain("void text(::std::string&& value");

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::UnionType type;");
        plugin.ShouldContain("::dds::core::xtypes::UnionMember(\"number\"");
        plugin.ShouldContain("::dds::core::xtypes::UnionMember(\"text\"");
        plugin.ShouldContain("member_access_infos[0].bindingMemberValueOffset[0]");
        plugin.ShouldContain("sample.text()");
        plugin.ShouldContain("native_type_code->_sampleAccessInfo = &sample_access_info;");
        plugin.ShouldContain("WireloomRti612TypePlugin<::Choice>::create");
    }

    /// <summary>Propagates union sample access through typedef chains on RTI 6.1.2.</summary>
    [Fact]
    public void EmitsUnionAliasSampleAccessForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "enum Kind { ZERO = 0, TEN = 10 }; union Choice switch(Kind) { case ZERO: long zero; case TEN: string ten; }; typedef Choice ChoiceAlias; typedef ChoiceAlias ChoiceAlias2; struct Holder { ChoiceAlias2 value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("dynamic_type<::ChoiceAlias_AliasTag_t>::get()");
        plugin.ShouldContain("dynamic_type<::ChoiceAlias2_AliasTag_t>::get()");
        plugin.ShouldContain("alias_type_code->_typeCode->_sampleAccessInfo = target_type_code->_sampleAccessInfo;");
        plugin.ShouldContain("_representation._typeCode->_typeCode->_typeCode->_sampleAccessInfo =");
    }

    /// <summary>Uses RTI's signed internal TypeCode label for high IDL char discriminators.</summary>
    [Fact]
    public void EmitsSignedHighCharUnionLabelForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "union Choice switch(char) { case 255: long value; default: long fallback; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        var normalizedPlugin = System.Text.RegularExpressions.Regex.Replace(plugin, @"\s+", " ");
        normalizedPlugin.ShouldContain("UnionMember(\"value\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), -1)");
    }

    /// <summary>Emits collection branches in the RTI 6.1.2 union adapter.</summary>
    [Theory]
    [InlineData("07-union-scoped")]
    [InlineData("07-union-string-sequence-aliases")]
    [InlineData("07-union-string-sequence-bounded-direct")]
    [InlineData("14-union-array-branch")]
    public void EmitsUnionCollectionBranchesForRti612(string caseName)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", caseName + ".idl"));
        if (caseName == "07-union-string-sequence-aliases")
        {
            source += """

                module RuntimeUnionStringSequences {
                    struct WireHolder {
                        DirectStringSequences direct;
                        StringSequenceAliases aliases;
                    };
                };
                """;
        }

        File.WriteAllText(input, source);
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::UnionType type;");

        switch (caseName)
        {
            case "07-union-scoped":
                plugin.ShouldContain("UnionMember(\"qualifiedPayload\"");
                plugin.ShouldContain("SequenceType(");
                break;
            case "07-union-string-sequence-aliases":
                plugin.ShouldContain("UnionMember(\"narrow\"");
                plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::string, 3 >>::sample_access_info()");
                plugin.ShouldContain("sequence_helper<::std::string>::sample_access_info()");
                plugin.ShouldContain("sequence_helper<::std::wstring>::sample_access_info()");
                plugin.ShouldContain("native_type_code->_members[0]._representation._typeCode->_members[0]._representation._typeCode->_sampleAccessInfo =");
                plugin.ShouldContain("native_type_code->_members[0]._representation._typeCode->_members[0]._representation._typeCode->_typeCode->_sampleAccessInfo =");
                plugin.ShouldContain("native_type_code->_members[0]._representation._typeCode->_members[1]._representation._typeCode->_sampleAccessInfo =");
                plugin.ShouldContain("native_type_code->_members[1]._representation._typeCode->_members[0]._representation._typeCode->_typeCode->_sampleAccessInfo =");
                break;
            case "07-union-string-sequence-bounded-direct":
                plugin.ShouldContain("UnionMember(\"narrow\"");
                plugin.ShouldContain("StringType(8)");
                plugin.ShouldContain("WStringType(4)");
                plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::string, 3 >>::sample_access_info()");
                plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::wstring, 3 >>::sample_access_info()");
                plugin.ShouldContain("sequence_helper<::std::string>::sample_access_info()");
                plugin.ShouldContain("sequence_helper<::std::wstring>::sample_access_info()");
                break;
            case "14-union-array-branch":
                plugin.ShouldContain("ArrayType(");
                break;
        }
    }
}
