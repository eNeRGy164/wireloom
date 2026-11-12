namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Protects IDL-derived generation and fail-closed behavior at the command boundary.</summary>
public sealed partial class GenerationSpecs
{

    /// <summary>Uses the declared @default_literal for every element of an enum array.</summary>
    [Fact]
    public void EmitsDefaultLiteralForEnumArrayElements()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory,
            "Corpus",
            "10-default-literal-enum-array.idl"));

        // Act
        var result = workspace.Generate(source, "10-default-literal-enum-array");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-default-literal-enum-array.hpp"));
        var arrayDefaultStart = header.IndexOf("ColorArray colorArray = ", StringComparison.Ordinal);
        arrayDefaultStart.ShouldBeGreaterThanOrEqualTo(0);
        var arrayDefaultEnd = header.IndexOf(';', arrayDefaultStart);
        var arrayDefault = header[arrayDefaultStart..arrayDefaultEnd];
        arrayDefault.Split("Color::RED")
            .Length.ShouldBe(6);
        arrayDefault.ShouldNotContain("Color::GREEN");
        header.ShouldContain("default_enumerator<::CorpusDefaultLiteralEnumArray::Color>");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-default-literal-enum-array.cxx"));
        valueSource.ShouldContain(
            "default_enumerator<::CorpusDefaultLiteralEnumArray::Color>::value =\n" +
            "    ::CorpusDefaultLiteralEnumArray::Color::RED;");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-default-literal-enum-arrayPlugin.cxx"));
        pluginSource.ShouldContain("factory.create_array(");
        pluginSource.ShouldContain("::rti::topic::dynamic_type<::CorpusDefaultLiteralEnumArray::Color>::get()");
        pluginSource.ShouldContain("::dds::core::xtypes::EnumMember(\"RED\", 1)");
    }

    /// <summary>Preserves signed 32-bit minimum, maximum, and default union discriminator cases.</summary>
    [Fact]
    public void EmitsLongUnionDiscriminatorBoundaries()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-long.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "07-union-long");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-long.hpp"));
        header.ShouldContain("const ::std::int32_t& _d() const;");
        header.ShouldContain("void _d(::std::int32_t value);");
        header.ShouldContain("const ::std::int32_t& minimum() const;");
        header.ShouldContain("const ::std::int32_t& maximum() const;");
        header.ShouldContain("const ::std::int32_t& fallback() const;");
        header.ShouldContain("::std::int32_t minimum_ = 0;");
        header.ShouldContain("::std::int32_t maximum_ = 0;");
        header.ShouldContain("::std::int32_t fallback_ = 0;");
        header.ShouldContain("bool operator==(const Choice& other_) const;");

        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-long.cxx"));
        values.ShouldContain("discriminator_ = -2147483648;");
        values.ShouldContain("discriminator_ = 2147483647;");
        values.ShouldContain("Choice::default_discriminator()");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-longPlugin.cxx"));
        plugin.ShouldContain("\"minimum\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                -2147483648));");
        plugin.ShouldContain("\"maximum\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                2147483647));");
        plugin.ShouldContain("\"fallback\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                ::dds::core::xtypes::UnionMember::DEFAULT_LABEL));");
    }

    /// <summary>Preserves bounded narrow and wide string sequence union branches and metadata bounds.</summary>
    [Fact]
    public void EmitsDirectBoundedStringSequenceUnionBranches()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-string-sequence-bounded-direct.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "07-union-string-sequence-bounded-direct");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-string-sequence-bounded-direct.hpp"));
        header.ShouldContain("const ::omg::types::bounded_sequence< ::std::string, 3 >& narrow() const;");
        header.ShouldContain("::omg::types::bounded_sequence< ::std::string, 3 >& narrow();");
        header.ShouldContain("void narrow(const ::omg::types::bounded_sequence< ::std::string, 3 >& value);");
        header.ShouldContain("const ::omg::types::bounded_sequence< ::std::wstring, 3 >& wide() const;");
        header.ShouldContain("::omg::types::bounded_sequence< ::std::wstring, 3 >& wide();");
        header.ShouldContain("void wide(const ::omg::types::bounded_sequence< ::std::wstring, 3 >& value);");
        header.ShouldContain("bool operator==(const DirectBoundedStringSequences& other_) const;");
        header.ShouldContain("/// auto type_name =\n///     dds::topic::topic_type_name<::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences>::value();");
        header.ShouldContain("NDDSUSERDllExport static void reset_sample(\n        ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample);");
        header.ShouldContain("NDDSUSERDllExport static void allocate_sample(\n        ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample,");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-string-sequence-bounded-directPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::StringType(8),\n                    3),\n                0));");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(4),\n                    3),\n                1));");
        plugin.ShouldContain("dynamic_type<::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences>::get()\n{\n    wireloom_initialize_dynamic_types();");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
        plugin.ShouldContain("rti::domain::register_type_plugin(");
        plugin.ShouldContain("wireloom_DirectBoundedStringSequences_stl_union_create_plugin");
        plugin.ShouldContain("topic_type_support<::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences>::to_cdr_buffer(");
        plugin.ShouldContain("participant_data.programs =\n        ::rti::topic::interpreter::get_cdr_serialization_programs<\n            ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences, true, true, true>();");
        plugin.ShouldContain("PRESTypePlugin_interpretedSerialize(");
        plugin.ShouldContain("sequence_helper<");
        plugin.ShouldContain("sequence_helper<\n                ::omg::types::bounded_sequence< ::std::string, 3 >>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::omg::types::bounded_sequence< ::std::wstring, 3 >>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::std::string>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::std::wstring>::sample_access_info();");
    }

    /// <summary>Preserves direct and typedef bounded string-sequence union branches.</summary>
    [Fact]
    public void EmitsStringSequenceAliasUnionBranchesAndMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-string-sequence-aliases.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "07-union-string-sequence-aliases");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-string-sequence-aliases.hpp"));
        header.ShouldContain("using NarrowTexts = ::omg::types::bounded_sequence< ::std::string, 3 >;");
        header.ShouldContain("using WideTexts = ::omg::types::bounded_sequence< ::std::wstring, 3 >;");
        header.ShouldContain("using BoundedNarrowTexts = ::omg::types::bounded_sequence< ::std::string, 3 >;");
        header.ShouldContain("using BoundedWideTexts = ::omg::types::bounded_sequence< ::std::wstring, 3 >;");
        header.ShouldContain("void narrow(const NarrowTexts& value);");
        header.ShouldContain("void wide(const WideTexts& value);");
        header.ShouldContain("void boundedNarrow(const BoundedNarrowTexts& value);");
        header.ShouldContain("void boundedWide(const BoundedWideTexts& value);");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-string-sequence-aliasesPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::StringType(255)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(255)");
        plugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(4)");
        plugin.ShouldContain("factory.create_sequence(");
        plugin.ShouldContain("sequence_helper<");
        plugin.ShouldContain("sequence_helper<\n                ::omg::types::bounded_sequence< ::std::string, 3 >>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::omg::types::bounded_sequence< ::std::wstring, 3 >>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::std::string>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::std::wstring>::sample_access_info();");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(");
        plugin.ShouldContain("\"RuntimeUnionStringSequences::NarrowTexts\"");
        plugin.ShouldContain("\"RuntimeUnionStringSequences::WideTexts\"");
        plugin.ShouldContain("\"RuntimeUnionStringSequences::BoundedNarrowTexts\"");
        plugin.ShouldContain("\"RuntimeUnionStringSequences::BoundedWideTexts\"");
        plugin.ShouldContain("::dds::core::xtypes::UnionMember(\n                \"boundedWide\"");
        plugin.ShouldContain("dynamic_type<::RuntimeUnionStringSequences::DirectStringSequences>::get()\n{\n    wireloom_initialize_dynamic_types();");
        plugin.ShouldContain("dynamic_type<::RuntimeUnionStringSequences::StringSequenceAliases>::get()\n{\n    wireloom_initialize_dynamic_types();");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
        plugin.ShouldContain("rti::domain::register_type_plugin(");
        plugin.ShouldContain("wireloom_DirectStringSequences_stl_union_create_plugin");
    }

    /// <summary>Emits an enum-valued constant after its enum and uses it in struct and union output.</summary>
    [Fact]
    public void EmitsEnumConstantDeclarationAndUnionLabels()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-enum-constant.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "14-enum-constant");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-enum-constant.hpp"));
        header.IndexOf("enum class Color", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf("constexpr Color Selected = Color::GREEN;", StringComparison.Ordinal));
        header.ShouldContain("constexpr Color Selected = Color::GREEN;");
        header.ShouldContain("Color color = Color::RED;");
        header.ShouldContain("Color discriminator_ = Color::RED;");
        header.ShouldContain("/// @details Discriminator labels: Color::GREEN.");
        header.ShouldContain("/// @details Discriminator labels: Color::RED.");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-enum-constantPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::UnionMember(\n                \"selected\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                1));");
        plugin.ShouldContain("::dds::core::xtypes::UnionMember(\n                \"red\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                0));");
    }

    /// <summary>Generates topic support for each of multiple root struct declarations once.</summary>
    [Fact]
    public void GeneratesMultipleRootStructsAndTopicSupport()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "02-multiple.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "02-multiple");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[] { "02-multiple.cxx", "02-multiple.hpp", "02-multiplePlugin.cxx", "02-multiplePlugin.hpp" }.Order());

        var sourceFile = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-multiple.cxx"));
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-multiple.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-multiplePlugin.cxx"));
        var pluginHeader = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-multiplePlugin.hpp"));

        ShouldContainExactlyOnce(pluginHeader, "#include \"02-multiple.hpp\"");

        foreach (var typeName in new[] { "First", "Second" })
        {
            ShouldContainExactlyOnce(sourceFile, $"std::is_standard_layout<::{typeName}>::value");
            ShouldContainExactlyOnce(header, $"struct NDDSUSERDllExport {typeName}\n");
            ShouldContainExactlyOnce(header, $"struct topic_type_name<::{typeName}>");
            ShouldContainExactlyOnce(header, $"struct topic_type_support<::{typeName}>");
            ShouldContainExactlyOnce(header, $"struct dynamic_type<::{typeName}>");
            ShouldContainExactlyOnce(plugin, $"struct wireloom_{typeName}_type_context");

            if (typeName == "Second")
            {
                plugin.ShouldContain($"dynamic_type<::{typeName}>::get()");
                plugin.ShouldContain($"wireloom_{typeName}_stl_string_create_plugin");
            }
            else
            {
                ShouldContainExactlyOnce(plugin, $"dynamic_type<::{typeName}>::get()");
            }

            ShouldContainExactlyOnce(plugin, $"topic_type_support<::{typeName}>::register_type(");
        }
    }

    /// <summary>Documents generated public value and union APIs with Doxygen comments.</summary>
    [Fact]
    public void EmitsDoxygenDocumentationForPublicCppApi()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-unions.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "07-unions");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-unions.hpp"));
        header.ShouldContain("/// @brief IDL union CorpusUnions::Choice");
        header.ShouldContain("/// @brief Accesses the number branch when active.");
        header.ShouldContain("/// @throws dds::core::PreconditionNotMetError when the branch is inactive.");
        header.ShouldContain("/// @brief Provides RTI topic registration and sample support operations.");
        header.ShouldContain("/// @param value New discriminator value.");
        header.ShouldContain("/// @return The discriminator value.");
        header.ShouldContain("/// @details Discriminator labels:");

        var dynamicTypeSeeLine = header.Split('\n').First(line =>
            line.Contains("@see [RTI DynamicType and DynamicData](https://community.rti.com/", StringComparison.Ordinal) &&
            line.Contains("structrti_1_1topic_1_1dynamic__type.html", StringComparison.Ordinal));
        dynamicTypeSeeLine.ShouldContain(
            "https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/structrti_1_1topic_1_1dynamic__type.html");
        dynamicTypeSeeLine.ShouldNotContain('\\');
        dynamicTypeSeeLine.ShouldEndWith(")");

        header.Split('\n').Where(IsLongDoxygenSeeUrl).ShouldAllBe(line => line.Length <= 320 && !line.Contains('\\'));
        header.ShouldContain("/// @code");
        header.ShouldContain("/// auto type_name = dds::topic::topic_type_name<::CorpusUnions::Choice>::value();");
    }

    private static bool IsLongDoxygenSeeUrl(string line) =>
        line.TrimStart().StartsWith("/// @see [", StringComparison.Ordinal)
        && line.Contains("https://community.rti.com/static/documentation/connext-dds/", StringComparison.Ordinal);

    private static void ShouldContainExactlyOnce(string source, string value) =>
        (source.Split(value).Length - 1).ShouldBe(1, value);

    /// <summary>Documents IDL bounds, array dimensions, and key membership from the resolved plan.</summary>
    [Fact]
    public void EmitsDoxygenMetadataForMembers()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "05-collections.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "05-collections");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-collections.hpp"));
        header.ShouldContain("Fixed array dimensions are 2 x 3.");
        header.ShouldContain("Effective sequence metadata bound is 4.");
        header.ShouldContain("String metadata bound is 16.");

        var keySource = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "08-keys.idl"));
        var keyResult = workspace.Generate(keySource, "08-keys");
        keyResult.ExitCode.ShouldBe(0, keyResult.Diagnostics);
        File.ReadAllText(Path.Combine(workspace.OutputPath, "08-keys.hpp"))
            .ShouldContain("This member is an IDL key.");
    }

    /// <summary>Emits aggregate inheritance and recursive type metadata in staged initialization order.</summary>
    [Fact]
    public void EmitsAggregateInheritanceAndRecursiveMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "06-aggregates.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "06-aggregates");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-aggregates.hpp"));
        header.ShouldContain("struct NDDSUSERDllExport Derived : public Base");
        header.ShouldContain("bounded_sequence< Recursive, 100 > children");
        header.ShouldContain("Base base = {};");
        header.ShouldContain("Derived derived = {};");

        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-aggregates.cxx"));
        values.ShouldContain("    : Base(base)");
        values.ShouldContain("return static_cast<const Base&>(left) == static_cast<const Base&>(right)");
        values.ShouldContain("&& left.derived == right.derived;");
        values.ShouldContain("swap(static_cast<Base&>(left), static_cast<Base&>(right));");
        values.ShouldContain("base: \" << static_cast<const Base&>(value)");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-aggregatesPlugin.cxx"));
        plugin.ShouldContain("new ::dds::core::xtypes::StructType(");
        plugin.ShouldContain("\"CorpusAggregates::Derived\",");
        plugin.ShouldContain("*::CorpusAggregates::wireloom_Base_type_context_instance().type));");
        plugin.ShouldContain("type->add_member(\n            ::dds::core::xtypes::Member(\"derived\"");
        plugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        plugin.ShouldContain("            1, false);");
        plugin.ShouldContain("new ::dds::core::xtypes::StructType(\"CorpusAggregates::Composed\")");
        plugin.ShouldContain("*factory.create_sequence(");
        plugin.ShouldContain("                100),");
        plugin.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusAggregates::Recursive>::get(&wireloom_recursive_resource_node),\n                100)");
        plugin.ShouldNotContain("This inherited aggregate value graph is outside the RTI C-layout plugin bridge.");
        plugin.ShouldNotContain("CDR for inherited aggregate value graphs is outside the RTI C-layout plugin bridge.");
        var recursiveContext = plugin[
            plugin.IndexOf("struct wireloom_Recursive_type_context", StringComparison.Ordinal)..plugin.IndexOf("wireloom_Recursive_type_context_instance()", StringComparison.Ordinal)];
        recursiveContext.ShouldNotContain("dynamic_type<::CorpusAggregates::Recursive>::get()");
        recursiveContext.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusAggregates::Recursive>::get(&wireloom_recursive_resource_node),\n                100)");

        var shellStage = plugin.IndexOf("// Stage 1: create all struct types without inheritance.", StringComparison.Ordinal);
        var memberStage = plugin.IndexOf("// Stage 2: register metadata in dependency order", StringComparison.Ordinal);
        shellStage.ShouldBeLessThan(memberStage);

        var pluginStage = plugin.IndexOf("// Stage 3: create plugins only after every DynamicType is complete.", StringComparison.Ordinal);
        memberStage.ShouldBeLessThan(pluginStage);
        plugin.IndexOf("wireloom_Recursive_type_context_instance().add_members()", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("wireloom_Composed_type_context_instance().add_members()", StringComparison.Ordinal));
    }

    /// <summary>Uses the shared value-class path for valuetype inheritance and composition.</summary>
    [Fact]
    public void EmitsValueTypeInheritanceAndHolderMember()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "06-valuetypes.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "06-valuetypes");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-valuetypes.hpp"));
        header.ShouldContain("struct NDDSUSERDllExport DerivedValue : public BaseValue");
        header.ShouldContain("::std::string name = {};");
        header.ShouldContain("struct NDDSUSERDllExport Holder");
        header.ShouldContain("DerivedValue value = {};");

        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-valuetypes.cxx"));
        values.ShouldContain("return static_cast<const BaseValue&>(left) == static_cast<const BaseValue&>(right)");
        values.ShouldContain("&& left.name == right.name;");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-valuetypesPlugin.cxx"));
        plugin.ShouldContain("new ::dds::core::xtypes::StructType(");
        plugin.ShouldContain("\"CorpusValueTypes::DerivedValue\",");
        plugin.ShouldContain("*::CorpusValueTypes::wireloom_BaseValue_type_context_instance().type));");
        plugin.ShouldContain("type->add_member(\n            ::dds::core::xtypes::Member(\"name\"");
        plugin.ShouldContain("This inherited aggregate value graph is outside the RTI C-layout plugin bridge.");
        plugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        plugin.ShouldContain("*::CorpusValueTypes::wireloom_DerivedValue_type_context_instance().type)");
        plugin.ShouldContain("This inherited aggregate value graph is outside the RTI C-layout plugin bridge.");
        plugin.ShouldNotContain("wireloom_DerivedValue_stl_string_create_plugin");
        plugin.ShouldNotContain("wireloom_BaseValue_stl_string_create_plugin");
    }

    /// <summary>Composes scalar aliases, enum metadata, constants, sequences, and aggregate members.</summary>
    [Fact]
    public void EmitsAliasCompositionAndConstantBound()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "06-alias-composition.idl"));

        // Act
        var result = workspace.Generate(source, "06-alias-composition");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-alias-composition.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-alias-compositionPlugin.cxx"));
        header.ShouldContain("constexpr ::std::int32_t Max = 4;");
        header.ShouldContain("using Identifier = ::std::int32_t;");
        header.ShouldContain("enum class State");
        header.ShouldContain("Identifier id = 0;");
        header.ShouldContain("::omg::types::bounded_sequence< float, 4 > values = {};");
        header.ShouldContain("Sample sample = {};");
        plugin.ShouldContain("::dds::core::xtypes::primitive_type<float>()");
        plugin.ShouldContain("                4),");
        plugin.ShouldContain("dynamic_type<::CorpusAggregateComposition::State>::get()");
        plugin.ShouldContain("dynamic_type<::CorpusAggregateComposition::Sample>::get()");
    }

    /// <summary>Emits direct, multiple, and nested key flags on their declared DynamicType members.</summary>
    [Fact]
    public void EmitsDeclaredKeyFlagsForSimpleMultipleAndNestedKeys()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "08-keys.idl"));

        // Act
        var result = workspace.Generate(source, "08-keys");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-keysPlugin.cxx"));
        GetMemberRegistration(plugin, "Simple", "id").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "Simple", "name").ShouldEndWith(", false);");
        GetMemberRegistration(plugin, "Identity", "tenant").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "Identity", "name").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "Sample", "identity").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "Sample", "payload").ShouldEndWith(", false);");
        plugin.ShouldContain("plugin->getKeyKindFnc = []() { return PRES_TYPEPLUGIN_USER_KEY; };");
        plugin.ShouldContain("plugin->serializeKeyFnc = PRESTypePlugin_interpretedSerializeKey;");
        plugin.ShouldContain("plugin->deserializeKeySampleFnc = PRESTypePlugin_interpretedDeserializeKey;");
        plugin.ShouldContain("plugin->instanceToKeyHashFnc = PRESTypePlugin_interpretedInstanceToKeyHash;");
        plugin.ShouldContain("destination->id = source->id;");
        plugin.ShouldContain("destination->tenant = source->tenant;");
        plugin.ShouldContain("destination->name = source->name;");
        plugin.ShouldContain("destination->identity = source->identity;");
        plugin.ShouldContain("PRESTypePluginDefaultEndpointData_createMD5StreamWithInfo(");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this string-bearing aggregate shape.");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified CDR bridge for this string-bearing aggregate shape.");
    }

    /// <summary>Maps unannotated IDL extensibility to RTI's default extensible metadata.</summary>
    [Fact]
    public void EmitsDefaultExtensibilityTraitAndDynamicTypeMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-default.idl"));

        // Act
        var result = workspace.Generate(source, "09-default");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-default.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-defaultPlugin.cxx"));
        header.ShouldContain("struct extensibility<::CorpusDefaultExtensibility::Message>");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::EXTENSIBLE");
        header.ShouldContain("/// @brief IDL struct CorpusDefaultExtensibility::Message");
        header.ShouldContain("/// @details Extensibility: Extensible.");
        header.ShouldContain("/// @details String metadata bound is 16.");
        plugin.ShouldContain(
            "\"CorpusDefaultExtensibility::Message\",\n              ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE,");
        GetMemberRegistration(plugin, "Message", "id").ShouldContain("0, false);");
        GetMemberRegistration(plugin, "Message", "text").ShouldContain("1, false);");
        plugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        plugin.IndexOf("\"id\"", StringComparison.Ordinal).ShouldBeLessThan(
            plugin.IndexOf("\"text\"", StringComparison.Ordinal));
    }

    /// <summary>Preserves explicit non-contiguous member IDs in appendable metadata.</summary>
    [Fact]
    public void EmitsExplicitMemberIdGapsInDeclarationOrder()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-id-gaps.idl"));

        // Act
        var result = workspace.Generate(source, "09-id-gaps");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-id-gaps.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-id-gaps.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-id-gapsPlugin.cxx"));
        header.ShouldContain("struct extensibility<::CorpusIdGaps::Message>");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::EXTENSIBLE");
        header.ShouldContain("std::int32_t first = 0;");
        header.ShouldContain("std::int32_t seventh = 0;");
        header.ShouldContain("/// @details Extensibility: Appendable.");
        header.ShouldContain("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/structrti_1_1topic_1_1extensibility.html");
        values.ShouldContain("Message::Message(\n    ::std::int32_t first,\n    ::std::int32_t seventh");
        plugin.ShouldContain(
            "\"CorpusIdGaps::Message\",\n              ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE,");
        (plugin.Split("factory.add_member(*type, ").Length - 1).ShouldBe(2);
        GetMemberRegistration(plugin, "Message", "first").ShouldContain(
            "::dds::core::xtypes::primitive_type<::std::int32_t>()");
        GetMemberRegistration(plugin, "Message", "first").ShouldEndWith(", 1, false);");
        GetMemberRegistration(plugin, "Message", "seventh").ShouldContain(
            "::dds::core::xtypes::primitive_type<::std::int32_t>()");
        GetMemberRegistration(plugin, "Message", "seventh").ShouldEndWith(", 7, false);");
        plugin.IndexOf("\"first\"", StringComparison.Ordinal).ShouldBeLessThan(
            plugin.IndexOf("\"seventh\"", StringComparison.Ordinal));
    }

    /// <summary>Uses bound MD5 member IDs for both parameterized and bare hash annotations.</summary>
    [Fact]
    public void EmitsBoundHashMemberIdsInDeclarationOrder()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-autoid-hash.idl"));

        // Act
        var result = workspace.Generate(source, "09-autoid-hash");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-autoid-hash.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-autoid-hashPlugin.cxx"));
        header.ShouldContain("struct extensibility<::CorpusAutoId::Sample>");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::EXTENSIBLE");
        header.ShouldContain("::std::int32_t id = 0;");
        header.ShouldContain("::std::string text = {};");
        header.ShouldContain("The shared IDL binder resolved this hashed member ID as 75475440.");
        header.ShouldContain("206680604.");
        header.ShouldContain(
            "https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1xtypes_1_1Member.html");
        plugin.ShouldContain(
            "\"CorpusAutoId::Sample\",\n              ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE,");
        GetMemberRegistration(plugin, "Sample", "id").ShouldEndWith(", 75475440, false);");
        GetMemberRegistration(plugin, "Sample", "text").ShouldContain(
            "::dds::core::xtypes::StringType(16)");
        GetMemberRegistration(plugin, "Sample", "text").ShouldContain("206680604, false);");
        plugin.IndexOf("\"id\"", StringComparison.Ordinal).ShouldBeLessThan(
            plugin.IndexOf("\"text\"", StringComparison.Ordinal));
    }

    /// <summary>Preserves block-scoped auto-ID behavior across reopened modules and includes.</summary>
    [Fact]
    public void EmitsModuleAutoIdOnlyWithinItsAnnotatedDeclarationBlock()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var corpusPath = Path.Combine(AppContext.BaseDirectory, "Corpus");
        var source = File.ReadAllText(Path.Combine(corpusPath, "14-autoid-scope.idl"));

        // Act
        var result = workspace.Generate(source, "14-autoid-scope",
            includeDirectories: [corpusPath]);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-autoid-scopePlugin.cxx"));
        GetMemberRegistration(plugin, "Hashed", "first").ShouldEndWith(", 64291979, false);");
        GetMemberRegistration(plugin, "Hashed", "second").ShouldEndWith(", 182907049, false);");
        GetMemberRegistration(plugin, "Reopened", "first").ShouldEndWith(", 0, false);");
        GetMemberRegistration(plugin, "Reopened", "second").ShouldEndWith(", 1, false);");
        GetMemberRegistration(plugin, "Included", "first").ShouldEndWith(", 0, false);");
        GetMemberRegistration(plugin, "Included", "second").ShouldEndWith(", 1, false);");
        GetMemberRegistration(plugin, "Sequential", "first").ShouldEndWith(", 0, false);");
        GetMemberRegistration(plugin, "Sequential", "second").ShouldEndWith(", 1, false);");
    }

    /// <summary>Maps all extensibility annotations and optional scalar presence to generated C++.</summary>
    [Fact]
    public void EmitsExtensibilityKindsAndOptionalScalarPresenceMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-extensibility.idl"));

        // Act
        var result = workspace.Generate(source, "09-extensibility");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-extensibility.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-extensibilityPlugin.cxx"));
        header.ShouldContain("struct extensibility<::CorpusExtensibility::Appendable>");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::EXTENSIBLE");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::FINAL");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::MUTABLE");
        header.ShouldContain("::dds::core::optional<::std::int32_t> optionalValue = {};");
        header.ShouldContain("Optional scalar storage preserves set and unset state.");
        header.ShouldContain("assign a value to set it");
        header.ShouldContain("reset() to unset it");
        header.ShouldContain("value() to access it when set");
        header.ShouldContain("Mutable sample{};");
        header.ShouldContain("sample.optionalValue = 7;");
        header.ShouldContain("sample.optionalValue.has_value()");
        header.ShouldContain("sample.optionalValue.value()");
        header.ShouldContain("sample.optionalValue.reset();");
        header.ShouldContain("::dds::core::optional<::std::int32_t> optionalValue");
        header.ShouldContain("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1optional.html");
        plugin.ShouldContain("factory.add_member(*type, \"id\"");
        plugin.ShouldContain("::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE");
        plugin.ShouldContain("::dds::core::xtypes::ExtensibilityKind::FINAL");
        plugin.ShouldContain("::dds::core::xtypes::ExtensibilityKind::MUTABLE");
        plugin.ShouldContain("::dds::core::xtypes::Member(\"optionalValue\"");
        plugin.ShouldContain(".id(2)\n                .optional(true));");
        plugin.IndexOf("factory.add_member(*type, \"id\"", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("::dds::core::xtypes::Member(\"optionalValue\"", StringComparison.Ordinal));
        GetMemberRegistration(plugin, "Mutable", "id").ShouldContain("1, false);");
    }

    /// <summary>Maps the full-form mutable annotation to the C++ trait and plugin metadata.</summary>
    [Fact]
    public void EmitsFullFormMutableExtensibilityAndMemberRegistration()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-extensibility-full.idl"));

        // Act
        var result = workspace.Generate(source, "14-extensibility-full");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "14-extensibility-full.cxx",
                "14-extensibility-full.hpp",
                "14-extensibility-fullPlugin.cxx",
                "14-extensibility-fullPlugin.hpp"
            }.Order());

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-extensibility-full.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-extensibility-fullPlugin.cxx"));
        header.ShouldContain("struct extensibility<::FullExtensibility>");
        header.ShouldContain("dds::core::xtypes::ExtensibilityKind::MUTABLE");
        plugin.ShouldContain("\"FullExtensibility\",");
        plugin.ShouldContain("::dds::core::xtypes::ExtensibilityKind::MUTABLE");
        plugin.ShouldContain("factory.create_struct(\n              \"FullExtensibility\",\n              ::dds::core::xtypes::ExtensibilityKind::MUTABLE,");
        GetMemberRegistration(plugin, "FullExtensibility", "value")
            .ShouldContain("::dds::core::xtypes::primitive_type<::std::int32_t>(), 0, false);");
    }

    /// <summary>Preserves explicit boolean, ID, and constant-valued default annotation arguments.</summary>
    [Fact]
    public void EmitsAnnotationArgumentsAsValueAndMemberMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-annotation-arguments.idl"));

        // Act
        var result = workspace.Generate(source, "14-annotation-arguments");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "14-annotation-arguments.cxx",
                "14-annotation-arguments.hpp",
                "14-annotation-argumentsPlugin.cxx",
                "14-annotation-argumentsPlugin.hpp"
            }.Order());

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-annotation-arguments.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-annotation-argumentsPlugin.cxx"));
        header.ShouldContain("constexpr ::std::int32_t Selected = 42;");
        header.ShouldContain("std::int32_t selected = 42;");
        header.ShouldContain("std::int32_t plain = 0;");

        var plain = GetMemberRegistration(plugin, "AnnotationArguments", "plain");
        var selected = GetMemberRegistration(plugin, "AnnotationArguments", "selected");
        plain.ShouldContain("0, false);");
        plain.ShouldNotContain("key(true)");
        plain.ShouldNotContain("optional(true)");
        selected.ShouldContain("16, false);");
        plugin.ShouldNotContain(".optional(true)");
        plugin.ShouldNotContain(".key(true)");
    }

    /// <summary>Preserves member declarator order, IDs, key selection, dimensions, and documentation.</summary>
    [Fact]
    public void EmitsMemberDeclaratorsWithIndividualDimensionsAndMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-member-declarators.idl"));

        // Act
        var result = workspace.Generate(source, "14-member-declarators");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "14-member-declarators.cxx",
                "14-member-declarators.hpp",
                "14-member-declaratorsPlugin.cxx",
                "14-member-declaratorsPlugin.hpp"
            }.Order());

        foreach (var path in Directory.GetFiles(workspace.OutputPath))
        {
            var generated = File.ReadAllText(path);
            generated.ShouldNotContain("/current/");
            generated.ShouldNotContain("\r");
            var lines = generated.Split('\n');
            lines.Where(line => !IsLongDoxygenSeeUrl(line)).Max(line => line.Length).ShouldBeLessThanOrEqualTo(120);
            lines.Where(IsLongDoxygenSeeUrl).ShouldAllBe(line => line.Length <= 320);
        }

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-member-declarators.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-member-declaratorsPlugin.cxx"));
        var first = GetMemberRegistration(plugin, "Declarators", "first");
        var second = GetMemberRegistration(plugin, "Declarators", "second");
        var numbers = GetMemberRegistration(plugin, "Declarators", "numbers");
        var matrix = GetMemberRegistration(plugin, "Declarators", "matrix");

        header.ShouldContain("::std::int32_t first = 0;");
        header.ShouldContain("::std::int32_t second = 0;");
        header.ShouldContain("::dds::core::array< ::std::int32_t, 2 > numbers = {};");
        header.ShouldContain("::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 > matrix = {};");
        header.IndexOf(" first = 0;", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf(" second = 0;", StringComparison.Ordinal));
        header.IndexOf(" second = 0;", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf(" numbers = {};", StringComparison.Ordinal));
        header.IndexOf(" numbers = {};", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf(" matrix = {};", StringComparison.Ordinal));
        header.ShouldContain("/// @details This member is an IDL key.\n    ::std::int32_t second = 0;");
        header.ShouldNotContain("/// @details This member is an IDL key.\n    ::std::int32_t first = 0;");
        header.ShouldContain("/// @details Fixed array dimensions are 2.");
        header.ShouldContain("/// @details Fixed array dimensions are 2 x 3.");

        first.ShouldContain("::dds::core::xtypes::primitive_type<::std::int32_t>()");
        first.ShouldEndWith(", 0, false);");
        first.ShouldNotContain("key(true)");
        second.ShouldContain("::dds::core::xtypes::primitive_type<::std::int32_t>()");
        second.ShouldEndWith(", 4, true);");
        numbers.ShouldContain("::dds::core::xtypes::primitive_type<::std::int32_t>()");
        numbers.ShouldContain("factory.create_array(");
        numbers.ShouldContain("2),");
        numbers.ShouldContain("5, false);");
        matrix.ShouldContain("::dds::core::xtypes::primitive_type<::std::int32_t>()");
        matrix.ShouldContain("factory.create_array(");
        matrix.ShouldContain("{2, 3}),");
        matrix.ShouldContain("6, false);");

        plugin.IndexOf("factory.add_member(*type, \"first\"", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("factory.add_member(*type, \"second\"", StringComparison.Ordinal));
        plugin.IndexOf("factory.add_member(*type, \"second\"", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("factory.add_member(*type, \"numbers\"", StringComparison.Ordinal));
        plugin.IndexOf("factory.add_member(*type, \"numbers\"", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("factory.add_member(*type, \"matrix\"", StringComparison.Ordinal));
    }

}
