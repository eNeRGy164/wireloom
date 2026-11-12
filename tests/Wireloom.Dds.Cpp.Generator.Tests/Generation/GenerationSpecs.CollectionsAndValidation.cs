namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Protects IDL-derived generation and fail-closed behavior at the command boundary.</summary>
public sealed partial class GenerationSpecs
{
    /// <summary>Preserves aggregate typedef chains in declarations and DynamicType metadata.</summary>
    [Fact]
    public void EmitsAggregateAliasChainInDeclarationOrder()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "03-alias-aggregate.idl"));

        // Act
        var result = workspace.Generate(source, "03-alias-aggregate");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-alias-aggregate.hpp"));
        var pointForward = header.IndexOf("struct Point;", StringComparison.Ordinal);
        var firstAlias = header.IndexOf("using PointAlias = Point;", StringComparison.Ordinal);
        var secondAlias = header.IndexOf("using PointAlias2 = PointAlias;", StringComparison.Ordinal);
        var pointDefinition = header.IndexOf("struct NDDSUSERDllExport Point", StringComparison.Ordinal);
        var sampleDefinition = header.IndexOf("struct NDDSUSERDllExport Sample", StringComparison.Ordinal);
        (pointForward < firstAlias && firstAlias < secondAlias && secondAlias < pointDefinition
            && pointDefinition < sampleDefinition).ShouldBeTrue();
        header.ShouldContain("PointAlias2 point = {};");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-alias-aggregatePlugin.cxx"));
        plugin.ShouldContain("#include <dds/core/xtypes/AliasType.hpp>");
        plugin.ShouldContain("#include <xcdr/xcdr_typeCode.h>");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusAggregateAliases::PointAlias2\",");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                    \"CorpusAggregateAliases::PointAlias\",");
        plugin.ShouldContain("::rti::topic::dynamic_type<::CorpusAggregateAliases::Point>::get()");
        plugin.ShouldContain("wireloom_alias_type_code___CorpusAggregateAliases__PointAlias()");
        plugin.ShouldContain("wireloom_alias_type_code___CorpusAggregateAliases__PointAlias2()");
        plugin.ShouldContain("wireloom_alias_type_code___CorpusAggregateAliases__PointAlias2());");
        plugin.ShouldContain("wireloom_alias_type_code___CorpusAggregateAliases__PointAlias());");
        plugin.ShouldContain("value._data._typeCode =\n            reinterpret_cast<::RTICdrTypeCode*>(\n                wireloom_alias_type_code___CorpusAggregateAliases__PointAlias());");
        plugin.ShouldContain("value._data._typeCode =\n            reinterpret_cast<::RTICdrTypeCode*>(\n                const_cast<::DDS_TypeCode*>(");
        plugin.ShouldContain("::rti::topic::dynamic_type<::CorpusAggregateAliases::Point>::get().native()" );
        plugin.ShouldContain("value._data._name = const_cast<char*>(\"CorpusAggregateAliases::PointAlias\");");
        plugin.ShouldContain("value._data._name = const_cast<char*>(\"CorpusAggregateAliases::PointAlias2\");");
        plugin.ShouldContain("value._data._annotations._allowedDataRepresentationMask = 5;");
        plugin.ShouldContain("::rti::topic::interpreter::get_aggregation_value_pointer<");
        plugin.ShouldContain("type->native()._data._members[0]._representation._typeCode =");
        header.ShouldContain("dynamic_type<::CorpusAggregateAliases::PointAlias2_AliasTag_t>");
    }

    /// <summary>Preserves scalar, enum, and bounded-enum-sequence typedef identity in metadata.</summary>
    [Fact]
    public void EmitsScalarEnumAndSequenceAliases()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "03-enums-aliases.idl"));

        // Act
        var result = workspace.Generate(source, "03-enums-aliases");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enums-aliases.hpp"));
        header.ShouldContain("using Scalar = ::std::int32_t;");
        header.ShouldContain("using ColorSequence = ::omg::types::bounded_sequence< Color, 3 >;");
        header.ShouldContain("using ScalarAlias = Scalar;");
        header.ShouldContain("using ColorAlias = Color;");
        header.ShouldContain("using ColorAlias2 = ColorAlias;");
        header.ShouldContain("Scalar value = 0;");
        header.ShouldContain("ScalarAlias aliasValue = 0;");
        header.ShouldContain("Color color = Color::RED;");
        header.ShouldContain("ColorAlias2 aliasColor = Color::RED;");
        header.ShouldContain("ColorSequence colors = {};");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enums-aliasesPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusEnums::Scalar\",");
        plugin.ShouldContain("::rti::topic::dynamic_type<::CorpusEnums::Color>::get()");
        plugin.ShouldContain("factory.add_member(*type, \"aliasColor\",\n            ::dds::core::xtypes::AliasType(");
        plugin.ShouldContain("\"CorpusEnums::ColorAlias2\",");
        plugin.ShouldContain("factory.create_sequence(\n                    ::rti::topic::dynamic_type<::CorpusEnums::Color>::get(),\n                    3)");
        plugin.ShouldContain("\"CorpusEnums::ColorSequence\",");
        plugin.ShouldContain("member_4_alias_type_code_0->_sampleAccessInfo =\n            &member_4_alias_access_info_1.sample;");
        plugin.ShouldContain("member_4_sequence_type_code_2->_sampleAccessInfo =\n            ::rti::topic::interpreter::detail::sequence_helper<\n                ::omg::types::bounded_sequence< ::CorpusEnums::Color, 3 >>::sample_access_info();");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.colors);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.colors, max_seq_length, max_str_length);");
        plugin.ShouldContain("dynamic_type<::CorpusEnums::ColorAlias2_AliasTag_t>::get()");
    }

    /// <summary>Preserves nested sequence aliases in C++ storage and recursively builds their metadata.</summary>
    [Fact]
    public void EmitsNestedSequenceAliasChain()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "03-alias-collections.idl"));

        // Act
        var result = workspace.Generate(source, "03-alias-collections");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-alias-collections.hpp"));
        header.ShouldContain("using LongSequence = ::omg::types::bounded_sequence< ::std::int32_t, 3 >;");
        header.ShouldContain("using NestedSequence = ::omg::types::bounded_sequence< LongSequence, 2 >;");
        header.ShouldContain("NestedSequence values = {};");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-alias-collectionsPlugin.cxx"));
        plugin.ShouldContain("factory.add_member(*type, \"values\",\n            ::dds::core::xtypes::AliasType(");
        plugin.ShouldContain("\"CorpusCollectionAliases::NestedSequence\",");
        plugin.ShouldContain("\"CorpusCollectionAliases::LongSequence\",");
        plugin.ShouldContain("member_0_sequence_type_code_2->_sampleAccessInfo =\n            ::rti::topic::interpreter::detail::sequence_helper<\n                ::omg::types::bounded_sequence< ::CorpusCollectionAliases::LongSequence, 2 >>::sample_access_info();");
        plugin.ShouldContain("auto* member_0_alias_type_code_3 = member_0_sequence_type_code_2->_typeCode;");
        plugin.ShouldContain("member_0_alias_type_code_3->_sampleAccessInfo =\n            &member_0_alias_access_info_4.sample;");
        plugin.ShouldContain("member_0_sequence_type_code_5->_sampleAccessInfo =\n            ::rti::topic::interpreter::detail::sequence_helper<\n                ::omg::types::bounded_sequence< ::std::int32_t, 3 >>::sample_access_info();");
        plugin.ShouldContain("plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;");
        plugin.ShouldContain("register_type_plugin(");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.values);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.values, max_seq_length, max_str_length);");
    }

    /// <summary>Emits multidimensional arrays, collection aliases, bounded defaults, and struct sequences.</summary>
    [Fact]
    public void EmitsCollectionsFromCorpusInput()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "05-collections.idl"));

        // Act
        var result = workspace.Generate(source, "05-collections");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-collections.hpp"));
        header.ShouldContain("using BoundedLongs = ::omg::types::bounded_sequence< ::std::int32_t, 4 >;");
        header.ShouldContain("using CoordinateGrid = ::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 >;");
        header.ShouldContain("::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 > values = {};");
        header.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 100 > unbounded = {};");
        header.ShouldContain("BoundedLongs bounded = {};");
        header.ShouldContain("::omg::types::bounded_sequence< Item, 2 > items = {};");
        header.ShouldContain("CoordinateGrid grid = {};");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-collections.cxx"));
        valueSource.ShouldContain("output << value.values[0][0];");
        valueSource.ShouldContain("output << value.values[1][2];");
        valueSource.ShouldContain("output << value.grid[0][0];");
        valueSource.ShouldContain("output << value.grid[1][2];");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-collectionsPlugin.cxx"));
        plugin.ShouldContain("factory.create_array(\n                ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                {2, 3}),");
        plugin.ShouldContain("factory.create_sequence(\n                ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                100),");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusCollections::BoundedLongs\",");
        plugin.ShouldContain("factory.create_sequence(\n                    ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                    4)");
        plugin.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusCollections::Item>::get(),\n                2),");
        plugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        plugin.ShouldContain("static ::RTIXCdrSampleAccessInfo* stl_sample_access_info()");
        plugin.ShouldContain("::rti::topic::interpreter::get_aggregation_value_pointer<\n                        ::CorpusCollections::Item>");
        plugin.ShouldContain("native_type_code->_sampleAccessInfo = stl_sample_access_info();");
        plugin.ShouldContain("::rti::topic::interpreter::detail::sequence_helper<\n                ::omg::types::bounded_sequence< ::CorpusCollections::Item, 2 >>::sample_access_info();");
        plugin.ShouldNotContain("alias_sequence_type_code");
        plugin.ShouldContain("wireloom_Sample_stl_sequence_create_plugin()");
        plugin.ShouldContain("plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.items);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.items, max_seq_length, max_str_length);");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.grid);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.grid, max_seq_length, max_str_length);");
    }

    /// <summary>Composes arrays, string and struct sequences, and nested collection aliases.</summary>
    [Fact]
    public void EmitsCollectionShapesFromCorpusInput()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "05-shapes.idl"));

        // Act
        var result = workspace.Generate(source, "05-shapes");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-shapes.hpp"));
        var itemForward = header.IndexOf("struct Item;", StringComparison.Ordinal);
        var itemsAlias = header.IndexOf(
            "using Items = ::omg::types::bounded_sequence< Item, 3 >;",
            StringComparison.Ordinal);
        var itemDefinition = header.IndexOf("struct NDDSUSERDllExport Item", StringComparison.Ordinal);
        (itemForward < itemsAlias && itemsAlias < itemDefinition).ShouldBeTrue();
        header.ShouldContain("using Row = ::dds::core::array< ::std::int32_t, 3 >;");
        header.ShouldContain("using Rows = ::omg::types::bounded_sequence< Row, 2 >;");
        header.ShouldContain("using Names = ::omg::types::bounded_sequence< ::std::string, 4 >;");
        header.ShouldContain("::omg::types::bounded_sequence< Item, 100 > unboundedItems = {};");
        header.ShouldContain("Items boundedItems = {};");
        header.ShouldContain("::omg::types::bounded_sequence< BoundedLongs, 2 > nestedSequences = {};");
        header.ShouldContain("::dds::core::array< Row, 2 > rows = {};");
        header.ShouldContain("::omg::types::bounded_sequence< Row, 2 > sequenceOfArrays = {};");
        header.ShouldContain("::omg::types::bounded_sequence< Rows, 2 > sequenceOfArrayAliases = {};");
        header.ShouldContain("Names names = {};");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-shapes.cxx"));
        valueSource.ShouldContain("output << value.rows[0][0];");
        valueSource.ShouldContain("output << value.rows[1][2];");
        valueSource.ShouldNotContain("output << value.rows[0];");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-shapesPlugin.cxx"));
        plugin.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusCollectionShapes::Item>::get(),\n                100),");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusCollectionShapes::Items\",");
        plugin.ShouldContain("factory.create_sequence(\n                    ::rti::topic::dynamic_type<::CorpusCollectionShapes::Item>::get(),\n                    3)");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                    \"CorpusCollectionShapes::BoundedLongs\",");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                    \"CorpusCollectionShapes::Row\",");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                    \"CorpusCollectionShapes::Rows\",");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusCollectionShapes::Names\",");
        plugin.ShouldContain("::dds::core::xtypes::StringType(32)");
        plugin.ShouldContain("RTI_XCDR_TK_ARRAY");
        plugin.ShouldContain("wireloom_Sample_stl_sequence_create_plugin");
        plugin.ShouldContain("get_aggregation_value_pointer<\n                        ::CorpusCollectionShapes::Item>");
        plugin.ShouldContain("member_1_alias_type_code_0->_sampleAccessInfo =\n            &member_1_alias_access_info_1.sample;");
        plugin.ShouldContain("sizeof(::CorpusCollectionShapes::Items)");
        plugin.ShouldContain("static ::DDS_TypeCode member_1_alias_sequence_type_code_2 = []()");
        plugin.ShouldContain("::rti::topic::interpreter::initialize_sequence_typecode<");
        plugin.ShouldContain("::CorpusCollectionShapes::Item, 3 >>(\n                    3);");
        plugin.ShouldContain("::rti::topic::dynamic_type<::CorpusCollectionShapes::Item>::get().native()");
        plugin.ShouldContain("member_1_alias_type_code_0->_typeCode =\n            reinterpret_cast<::RTICdrTypeCode*>(&member_1_alias_sequence_type_code_2);");
        plugin.ShouldContain("member_1_sequence_type_code_3->_sampleAccessInfo =\n            ::rti::topic::interpreter::detail::sequence_helper<");
        plugin.ShouldContain("member_2_alias_type_code_1->_sampleAccessInfo =\n            &member_2_alias_access_info_2.sample;");
        plugin.ShouldContain("sizeof(::CorpusCollectionShapes::BoundedLongs)");
        plugin.ShouldNotContain("member_2_alias_sequence_type_code_");
        plugin.ShouldContain("member_3_alias_type_code_1->_sampleAccessInfo =\n            &member_3_alias_access_info_2.sample;");
        plugin.ShouldContain("sizeof(::CorpusCollectionShapes::Row)");
        plugin.ShouldContain("member_5_alias_type_code_1->_sampleAccessInfo =\n            &member_5_alias_access_info_2.sample;");
        plugin.ShouldContain("member_5_alias_type_code_4->_sampleAccessInfo =\n            &member_5_alias_access_info_5.sample;");
        plugin.ShouldContain("sizeof(::CorpusCollectionShapes::Rows)");
        plugin.ShouldContain("sequence_helper<\n                ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Rows, 2 >>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::omg::types::bounded_sequence< ::std::string, 4 >>::sample_access_info();");
        plugin.ShouldContain("member_6_alias_type_code_0->_sampleAccessInfo =\n            &member_6_alias_access_info_1.sample;");
        plugin.ShouldContain("sizeof(::CorpusCollectionShapes::Names)");
        plugin.ShouldContain("member_6_string_type_code_3->_sampleAccessInfo =");
        plugin.ShouldContain("::std::string>::sample_access_info();");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.sequenceOfArrayAliases);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.names, max_seq_length, max_str_length);");

        // Arrange
        var aliasOnlySource = System.Text.RegularExpressions.Regex.Replace(
            source,
            @"(struct Sample\s*)\{.*?\};",
            "$1{ Items items; };",
            System.Text.RegularExpressions.RegexOptions.Singleline);

        // Act
        var aliasOnlyResult = workspace.Generate(aliasOnlySource, "05-shapes-alias-struct-sequence");

        // Assert
        aliasOnlyResult.ExitCode.ShouldBe(0, aliasOnlyResult.Diagnostics);
        var aliasOnlyPlugin = File.ReadAllText(Path.Combine(
            workspace.OutputPath, "05-shapes-alias-struct-sequencePlugin.cxx"));
        aliasOnlyPlugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
        aliasOnlyPlugin.ShouldContain("wireloom_Sample_stl_sequence_create_plugin");
        aliasOnlyPlugin.ShouldContain("get_aggregation_value_pointer<\n                        ::CorpusCollectionShapes::Sample>");
        aliasOnlyPlugin.ShouldContain("get_aggregation_value_pointer<\n                        ::CorpusCollectionShapes::Item>");

        // Arrange
        var aliasedElementSource = source.Replace(
            "typedef sequence<Item, 3> Items;",
            "typedef Item ItemAlias;\n    typedef sequence<ItemAlias, 3> AliasItems;",
            StringComparison.Ordinal);
        aliasedElementSource.ShouldNotBe(source);
        aliasedElementSource = System.Text.RegularExpressions.Regex.Replace(
            aliasedElementSource,
            @"(struct Sample\s*)\{.*?\};",
            "$1{ AliasItems items; };",
            System.Text.RegularExpressions.RegexOptions.Singleline);

        // Act
        var aliasedElementResult = workspace.Generate(
            aliasedElementSource, "05-shapes-aliased-struct-sequence-element");

        // Assert
        aliasedElementResult.ExitCode.ShouldBe(0, aliasedElementResult.Diagnostics);
        var aliasedElementPlugin = File.ReadAllText(Path.Combine(
            workspace.OutputPath, "05-shapes-aliased-struct-sequence-elementPlugin.cxx"));
        aliasedElementPlugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
        aliasedElementPlugin.ShouldContain("wireloom_Sample_stl_sequence_create_plugin");
        aliasedElementPlugin.ShouldContain("get_aggregation_value_pointer<\n                        ::CorpusCollectionShapes::Sample>");
        aliasedElementPlugin.ShouldContain("get_aggregation_value_pointer<\n                        ::CorpusCollectionShapes::Item>");
        aliasedElementPlugin.ShouldContain("initialize_sequence_typecode<");
        aliasedElementPlugin.ShouldContain("dynamic_type<::CorpusCollectionShapes::Item>::get().native()");
        aliasedElementPlugin.ShouldContain("member_0_alias_type_code_0->_typeCode =");
    }

    /// <summary>Preserves sequence bounds and array dimensions when sequences are array elements.</summary>
    [Fact]
    public void EmitsArraysOfBoundedAndFiniteFallbackSequences()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "05-array-of-sequences.idl"));

        // Act
        var result = workspace.Generate(source, "05-array-of-sequences");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-array-of-sequences.hpp"));
        header.ShouldContain("::dds::core::array< ::omg::types::bounded_sequence< ::std::int32_t, 100 >, 2 > values = {};");
        header.ShouldContain("::dds::core::array< ::omg::types::bounded_sequence< ::std::string, 3 >, 2 > names = {};");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-array-of-sequences.cxx"));
        valueSource.ShouldContain("output << value.values[0];");
        valueSource.ShouldContain("output << value.values[1];");
        valueSource.ShouldContain("output << value.names[0];");
        valueSource.ShouldContain("output << value.names[1];");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "05-array-of-sequencesPlugin.cxx"));
        plugin.ShouldContain("factory.create_array(\n                *factory.create_sequence(\n                    ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                    100),\n                2),");
        plugin.ShouldContain("factory.create_array(\n                *factory.create_sequence(\n                    ::dds::core::xtypes::StringType(16),\n                    3),\n                2),");
        plugin.ShouldContain("RTI_XCDR_TK_ARRAY");
        plugin.ShouldContain("member_0_sequence_type_code_1->_sampleAccessInfo =");
        plugin.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 100 >>::sample_access_info();");
        plugin.ShouldContain("member_1_string_type_code_2->_sampleAccessInfo =");
        plugin.ShouldContain("::omg::types::bounded_sequence< ::std::string, 3 >>::sample_access_info();");
        plugin.ShouldContain("::std::string>::sample_access_info();");
        plugin.ShouldContain("native_type_code->_sampleAccessInfo = stl_sample_access_info();");
        plugin.ShouldContain("wireloom_Sample_stl_sequence_create_plugin");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.values);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.values, max_seq_length, max_str_length);");
        plugin.ShouldContain("::rti::topic::reset_sample(sample.names);");
        plugin.ShouldContain("::rti::topic::allocate_sample(sample.names, max_seq_length, max_str_length);");
    }

    /// <summary>Loads the authored include closure and applies the corpus command-line define.</summary>
    [Fact]
    public void EmitsPreprocessedInputWithIncludedStructAndDefinedBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var corpusPath = Path.Combine(AppContext.BaseDirectory, "Corpus");
        var source = File.ReadAllText(Path.Combine(corpusPath, "11-preprocessing.idl"));

        // Act
        var result = workspace.Generate(source, "11-preprocessing", ["CORPUS_CAPTURE_DEFINE"], [corpusPath]);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "11-preprocessing.cxx",
                "11-preprocessing.hpp",
                "11-preprocessingPlugin.cxx",
                "11-preprocessingPlugin.hpp",
            }.Order());

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "11-preprocessing.hpp"));
        header.ShouldContain("constexpr ::std::int32_t BranchValue = 8;");
        header.ShouldContain("struct NDDSUSERDllExport IncludedType");
        header.ShouldContain("struct NDDSUSERDllExport Sample");
        header.ShouldContain("::IncludedType item = {};");
        header.ShouldContain("std::int32_t value = 0;");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "11-preprocessingPlugin.cxx"));
        plugin.ShouldContain("::rti::topic::dynamic_type<::IncludedType>::get()");
        plugin.ShouldContain("dynamic_type<::CorpusPreprocessing::Sample>::get()");
    }

    /// <summary>Resolves both quoted and angle-bracket include search paths.</summary>
    [Fact]
    public void EmitsIncludedTypeFromQuotedAndAngleBracketSearchPaths()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var corpusPath = Path.Combine(AppContext.BaseDirectory, "Corpus");
        var includePath = Path.Combine(corpusPath, "includes");
        var source = File.ReadAllText(Path.Combine(corpusPath, "11-include-search.idl"));

        // Act
        var result = workspace.Generate(source, "11-include-search",
            includeDirectories: [corpusPath, includePath]);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "11-include-search.cxx",
                "11-include-search.hpp",
                "11-include-searchPlugin.cxx",
                "11-include-searchPlugin.hpp",
            }.Order());

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "11-include-search.hpp"));
        header.ShouldContain("struct NDDSUSERDllExport IncludedType");
        header.ShouldContain("struct NDDSUSERDllExport Sample");
        header.ShouldContain("::IncludedType item = {};");
        header.ShouldContain("std::int32_t value = 0;");
        header.Split("struct NDDSUSERDllExport IncludedType").Length.ShouldBe(2);
    }

    /// <summary>Finds a quoted IDL include beside the root without an include-directory option.</summary>
    [Theory]
    [InlineData("#include \"sibling.idl\"")]
    [InlineData("#define HEADER \"sibling.idl\"\n#include HEADER")]
    public void ResolvesQuotedSiblingIncludeWithoutIncludeDirectories(string includeDirective)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = $$"""
            {{includeDirective}}
            module SiblingInclude {
                struct Sample { IncludedType item; };
            };
            """;
        var sibling = "struct IncludedType { long value; };";

        // Act
        var result = workspace.Generate(source, "sibling-root",
            siblingFiles: new Dictionary<string, string> { ["sibling.idl"] = sibling });

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "sibling-root.hpp"));
        header.ShouldContain("struct NDDSUSERDllExport IncludedType");
        header.ShouldContain("::IncludedType item = {};");
    }

    /// <summary>Expands advanced conditional branches, continuations, and variadic macros.</summary>
    [Theory]
    [InlineData(false, "9")]
    [InlineData(true, "8")]
    public void EmitsAdvancedPreprocessorBranch(bool useAdvancedDefine, string expectedValue)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "11-preprocessor-advanced.idl"));
        IReadOnlyList<string> defines = useAdvancedDefine ? ["CORPUS_ADVANCED_DEFINE"] : [];

        // Act
        var result = workspace.Generate(source, "11-preprocessor-advanced", defines);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "11-preprocessor-advanced.hpp"));
        header.ShouldContain($"constexpr ::std::int32_t Selected = {expectedValue};");
        header.ShouldContain("struct NDDSUSERDllExport Sample");
        header.ShouldContain("std::int32_t value = 0;");
        header.ShouldNotContain("CORPUS_");
    }

    /// <summary>Emits C004 constants, enum traits, nested values, and composed RTI member metadata.</summary>
    [Fact]
    public void EmitsNamesConstantsEnumsAndNestedStructs()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "02-names-constants.idl"));

        // Act
        var result = workspace.Generate(source, "02-names-constants");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-names-constants.hpp"));
        header.ShouldContain("constexpr ::std::int32_t Base = 4;");
        header.ShouldContain("constexpr ::std::int32_t Derived = 17;");
        header.ShouldContain("enum class State");
        header.ShouldContain("    Idle,");
        header.ShouldContain("    State state = State::Idle;");
        header.ShouldContain("    Nested::Point point = {};");
        header.ShouldContain("default_enumerator<::CorpusNames::State>");
        header.ShouldContain("dynamic_type<::CorpusNames::State>");
        header.ShouldContain("topic_type_support<::CorpusNames::Nested::Point>");
        header.ShouldNotContain("CorpusNames.Nested");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-names-constants.cxx"));
        valueSource.ShouldContain("case State::Idle:");
        valueSource.ShouldContain("output << \"State::Running\";");
        valueSource.ShouldContain("left.point == right.point");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-names-constantsPlugin.cxx"));
        pluginSource.ShouldContain("factory.create_enum(");
        pluginSource.ShouldContain("::dds::core::xtypes::EnumMember(\"Complete\", 2)");
        pluginSource.ShouldContain("::rti::topic::dynamic_type<::CorpusNames::State>::get()");
        pluginSource.ShouldContain("::rti::topic::dynamic_type<::CorpusNames::Nested::Point>::get()");
        pluginSource.ShouldContain("offsetof(::CorpusNames::KeywordRecord, point)");
    }

    /// <summary>Generates explicit and gapped enum ordinals without requiring a containing struct.</summary>
    [Fact]
    public void EmitsEnumOnlyInputWithExplicitValuesAndDefaultIdentity()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "03-enum-values-explicit.idl"));

        // Act
        var result = workspace.Generate(source, "03-enum-values-explicit");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "03-enum-values-explicit.cxx",
                "03-enum-values-explicit.hpp",
                "03-enum-values-explicitPlugin.cxx",
                "03-enum-values-explicitPlugin.hpp",
            }.Order());

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enum-values-explicit.hpp"));
        header.ShouldContain("enum class Explicit");
        header.ShouldContain("    NEGATIVE = -2,");
        header.ShouldContain("    ZERO = 0,");
        header.ShouldContain("    GAP = 7,");
        header.ShouldContain("    NEXT");
        header.ShouldNotContain("    NEXT = 8");
        header.ShouldContain("default_enumerator<::CorpusEnumValues::Explicit>");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enum-values-explicit.cxx"));
        valueSource.ShouldContain(
            "default_enumerator<::CorpusEnumValues::Explicit>::value =\n    ::CorpusEnumValues::Explicit::NEGATIVE;");
        valueSource.ShouldContain("case Explicit::NEGATIVE:");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enum-values-explicitPlugin.cxx"));
        pluginSource.ShouldContain("EnumMember(\"NEGATIVE\", -2)");
        pluginSource.ShouldContain("EnumMember(\"ZERO\", 0)");
        pluginSource.ShouldContain("EnumMember(\"GAP\", 7)");
        pluginSource.ShouldContain("EnumMember(\"NEXT\", 8)");
        pluginSource.ShouldContain("dynamic_type<::CorpusEnumValues::Explicit>::get()");
        pluginSource.ShouldNotContain("topic_type_support<");
    }

    /// <summary>Preserves a prefix @value ordinal and lets C++ assign the implicit successor.</summary>
    [Fact]
    public void EmitsPrefixEnumValueAndImplicitSuccessor()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "03-enum-values-prefix.idl"));

        // Act
        var result = workspace.Generate(source, "03-enum-values-prefix");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order()
            .ShouldBe(new[]
            {
                "03-enum-values-prefix.cxx",
                "03-enum-values-prefix.hpp",
                "03-enum-values-prefixPlugin.cxx",
                "03-enum-values-prefixPlugin.hpp",
            }.Order());

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enum-values-prefix.hpp"));
        header.ShouldContain("    FIRST = 7,");
        header.ShouldContain("    SECOND\n");
        header.ShouldNotContain("SECOND =");
        header.ShouldContain("default_enumerator<::CorpusEnumValuePrefix::Annotated>");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enum-values-prefix.cxx"));
        valueSource.ShouldContain("Annotated::FIRST;");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "03-enum-values-prefixPlugin.cxx"));
        pluginSource.ShouldContain("EnumMember(\"FIRST\", 7)");
        pluginSource.ShouldContain("EnumMember(\"SECOND\", 8)");
    }

    /// <summary>Generates the selected preprocessor corpus inputs after shared IDL preprocessing.</summary>
    [Theory]
    [InlineData("11-conditionals", "Enabled", "UndefBranch", "Disabled")]
    [InlineData("11-macros", "Sample", "value", "CORPUS_")]
    public void GeneratesPreprocessedCorpusDeclarations(string name, string firstType, string secondType, string absentText)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", name + ".idl"));

        // Act
        var result = workspace.Generate(source, name);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, name + ".hpp"));
        header.ShouldContain("struct NDDSUSERDllExport " + firstType);
        if (name == "11-conditionals")
        {
            header.ShouldContain("struct NDDSUSERDllExport " + secondType);
            header.ShouldNotContain(absentText);
        }
        else
        {
            header.ShouldContain("::std::int32_t value = 0;");
            header.ShouldNotContain("CORPUS_TYPE");
            header.ShouldNotContain("CORPUS_FIELD");
        }

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, name + ".cxx"));
        valueSource.ShouldContain(firstType + "::" + (name == "11-conditionals" ? "Enabled" : "Sample"));
    }

    /// <summary>Proves annotation-like comment text cannot change generated declarations or topic semantics.</summary>
    [Fact]
    public void TreatsCommentTriviaAsModelNeutral()
    {
        // Arrange
        using var commentsWorkspace = new GenerationWorkspace();
        using var plainWorkspace = new GenerationWorkspace();
        var comments = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "11-comments.idl"));
        const string withoutComments = "module CorpusComments { struct Sample { string<8> text; }; };";

        // Act
        var commentsResult = commentsWorkspace.Generate(comments, "11-comments");
        var plainResult = plainWorkspace.Generate(withoutComments, "11-comments");

        // Assert
        commentsResult.ExitCode.ShouldBe(0, commentsResult.Diagnostics);
        plainResult.ExitCode.ShouldBe(0, plainResult.Diagnostics);
        var files = new[]
        {
            "11-comments.cxx",
            "11-comments.hpp",
            "11-commentsPlugin.cxx",
            "11-commentsPlugin.hpp",
        };
        foreach (var file in files)
        {
            File.ReadAllText(Path.Combine(commentsWorkspace.OutputPath, file))
                .ShouldBe(File.ReadAllText(Path.Combine(plainWorkspace.OutputPath, file)));
        }

        var header = File.ReadAllText(Path.Combine(commentsWorkspace.OutputPath, "11-comments.hpp"));
        header.ShouldContain("::std::string text = {};");
        header.ShouldContain("REGISTER_TOPIC_TYPE(::CorpusComments::Sample)");
        header.ShouldNotContain("semantic-comment");
        header.ShouldNotContain("@topic");
    }

    /// <summary>Preserves stringification and token-pasting results from preprocessed IDL.</summary>
    [Fact]
    public void GeneratesMacroOperatorResults()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "11-macro-operators.idl"));

        // Act
        var result = workspace.Generate(source, "11-macro-operators");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "11-macro-operators.hpp"));
        header.ShouldContain("constexpr const char* Name = \"sample\";");
        header.ShouldContain("struct NDDSUSERDllExport Sample");
        header.ShouldContain("::std::int32_t field = 0;");
        header.ShouldNotContain("CORPUS_CAT");
        header.ShouldNotContain("CORPUS_STR");
        header.ShouldNotContain("CORPUS_");
    }

    /// <summary>Emits reopened scopes and bounded scalar sequences through the same compositional type plan.</summary>
    [Fact]
    public void EmitsReopenedScopesAndBoundedSequences()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "02-scopes.idl"));

        // Act
        var result = workspace.Generate(source, "02-scopes");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-scopes.hpp"));
        header.ShouldContain("constexpr ::std::int32_t Bound = 4;");
        header.ShouldContain("struct NDDSUSERDllExport Point");
        header.ShouldContain("Nested::Point relative = {};");
        header.ShouldContain("Nested::Point absolute = {};");
        header.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 4 > values = {};");
        header.ShouldContain("struct NDDSUSERDllExport Reopened");
        header.ShouldContain("KeywordRecord value = {};");
        header.ShouldNotContain("CorpusScopes.Nested");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-scopesPlugin.cxx"));
        pluginSource.ShouldContain("factory.create_sequence(");
        pluginSource.ShouldContain("::dds::core::xtypes::primitive_type<::std::int32_t>()");
        pluginSource.ShouldContain("                4),\n            3, false);");
        pluginSource.ShouldContain("::rti::topic::reset_sample(sample.values);");
        pluginSource.ShouldContain("::rti::topic::allocate_sample(sample.values, max_seq_length, max_str_length);");
        pluginSource.ShouldContain("offsetof(::CorpusScopes::KeywordRecord, relative)");
        pluginSource.ShouldContain("topic_type_support<::CorpusScopes::Reopened>");
    }

    /// <summary>Emits evaluated constants, bounded sequences, and one-dimensional primitive arrays.</summary>
    [Fact]
    public void EmitsConstantExpressionsAndPrimitiveCollections()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "02-constant-expressions.idl"));

        // Act
        var result = workspace.Generate(source, "02-constant-expressions");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-constant-expressions.hpp"));
        header.ShouldContain("constexpr ::std::int32_t Add = 5;");
        header.ShouldContain("constexpr ::std::int32_t Shifted = 21;");
        header.ShouldContain("constexpr ::std::int32_t Masked = 6;");
        header.ShouldContain("constexpr ::std::int32_t Bound = 5;");
        header.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 5 > values = {};");
        header.ShouldContain("::dds::core::array< ::std::int32_t, 5 > array = {};");
        header.ShouldContain("#include <dds/core/array.hpp>");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-constant-expressions.cxx"));
        valueSource.ShouldContain("output << value.array[4];");
        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "02-constant-expressionsPlugin.cxx"));
        pluginSource.ShouldContain("factory.create_sequence(");
        pluginSource.ShouldContain("factory.create_array(");
        pluginSource.ShouldContain("::rti::topic::reset_sample(sample.array);");
        pluginSource.ShouldContain("::rti::topic::allocate_sample(sample.array, max_seq_length, max_str_length);");
        header.ShouldNotContain("CorpusConstantExpressions.");
    }

    /// <summary>Stages recursive metadata and selects the RTI interpreter bridge for C019 aggregates.</summary>
    [Fact]
    public void EmitsRecursiveSequenceMetadataAndStlPluginBridge()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "06-aggregates.idl"));

        // Act
        var result = workspace.Generate(source, "06-aggregates");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-aggregates.cxx"));
        valueSource.ShouldNotContain("std::is_standard_layout<::CorpusAggregates::Composed>");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "06-aggregatesPlugin.cxx"));
        pluginSource.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusAggregates::Recursive>::get(&wireloom_recursive_resource_node),\n                100)");
        pluginSource.ShouldContain("new ::dds::core::xtypes::StructType(\"CorpusAggregates::Composed\")");
        pluginSource.ShouldContain("wireloom_Derived_stl_string_create_plugin");
        pluginSource.ShouldContain("wireloom_Composed_stl_string_create_plugin");
        pluginSource.ShouldContain("wireloom_recursive_resource_node.resource = &type->native();");
        pluginSource.ShouldContain("RTIXCdrTypeCode_isResourceVisited(");
        pluginSource.ShouldContain("dynamic_type<::CorpusAggregates::Recursive>::get(&wireloom_recursive_resource_node)");
        pluginSource.ShouldContain("wireloom_Recursive_stl_sequence_create_plugin");
        pluginSource.ShouldContain("RTI Connext DDS 7.7.0.0 does not support TypeObjectV2 for");
        pluginSource.ShouldContain("Strongly Connected Components (recursive types).");
        pluginSource.ShouldContain("get_cdr_serialization_programs<\n            ::CorpusAggregates::Recursive, true, true, true>()");
        pluginSource.ShouldNotContain("This inherited aggregate value graph is outside the RTI C-layout plugin bridge.");
        pluginSource.ShouldNotContain("CDR for inherited aggregate value graphs is outside the RTI C-layout plugin bridge.");
        var recursiveContext = pluginSource.Substring(
            pluginSource.IndexOf("struct wireloom_Recursive_type_context", StringComparison.Ordinal),
            pluginSource.IndexOf("wireloom_Recursive_type_context_instance()", StringComparison.Ordinal) -
            pluginSource.IndexOf("struct wireloom_Recursive_type_context", StringComparison.Ordinal));
        recursiveContext.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusAggregates::Recursive>::get(&wireloom_recursive_resource_node),\n                100)");
        recursiveContext.ShouldNotContain("member_1_recursive_sequence_type_code");
    }

    /// <summary>Preserves RTI's unbounded recursive sequence storage and DynamicType metadata when enabled.</summary>
    [Fact]
    public void EmitsUnboundedRecursiveSequenceWithResourceTraversal()
    {
        // Arrange
        const string source = "module CorpusAggregates { struct Recursive { long value; sequence<Recursive> children; }; };";
        var input = new IdlInput("06-aggregates.idl", source, unboundedSupport: true);

        // Act
        var files = GenerateBoundInput(input);

        // Assert
        var header = files.Single(file => file.FileName == "06-aggregates.hpp").Source;
        var plugin = files.Single(file => file.FileName == "06-aggregatesPlugin.cxx").Source;
        header.ShouldContain("::omg::types::sequence< Recursive > children");
        header.ShouldContain("get(RTIXCdrResourceNode* parentVisitedNode)");
        plugin.ShouldContain("get(::RTIXCdrResourceNode* parentVisitedNode)");
        plugin.ShouldContain("RTIXCdrTypeCode_isResourceVisited(");
        plugin.ShouldContain("factory.create_sequence(\n                ::rti::topic::dynamic_type<::CorpusAggregates::Recursive>::get(&wireloom_recursive_resource_node),\n                RTIXCdrLong_MAX)");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
    }

    /// <summary>Emits the primitive constants and signed-byte declarations in the next positive cases.</summary>
    [Theory]
    [InlineData("01-boundaries", "constexpr ::std::int16_t shortMin = -32768;", "::std::int16_t shortValue = 0;")]
    [InlineData("01-int8-boundaries", "constexpr ::std::int8_t int8Min = -128;", "::std::int8_t minimum = 0;")]
    public void EmitsIntegralBoundaryConstants(string name, string expectedConstant, string expectedMember)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", name + ".idl"));

        // Act
        var result = workspace.Generate(source, name);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, name + ".hpp"));
        header.ShouldContain(expectedConstant);
        header.ShouldContain(expectedMember);
    }

    /// <summary>Maps every fixed-width IDL integer to its matching C++ standard type.</summary>
    [Fact]
    public void EmitsFullWidthPrimitiveMatrix()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "01-full-widths.idl"));

        // Act
        var result = workspace.Generate(source, "01-full-widths");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "01-full-widths.hpp"));
        foreach (var declaration in new[]
                 {
                     "::std::int8_t i8", "::std::int16_t i16", "::std::int32_t i32", "::std::int64_t i64",
                     "::std::uint8_t u8", "::std::uint16_t u16", "::std::uint32_t u32", "::std::uint64_t u64"
                 })
        {
            header.ShouldContain(declaration);
        }
    }

    /// <summary>Emits a readable source layout with IDL names and separated RTI responsibilities.</summary>
    [Fact]
    public void EmitsReadableSourceLayout()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string idl = "module Example { struct Reading { long count; boolean enabled; char initial; wchar wide; float weight; double ratio; long double precise; }; };";

        // Act
        var result = workspace.Generate(idl, "reading");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "reading.hpp"));
        header.ShouldContain("// <auto-generated/>");
        header.ShouldContain("struct NDDSUSERDllExport Reading");
        header.ShouldContain("#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)");
        header.ShouldContain("#define NDDSUSERDllExport __declspec(dllexport)");
        header.ShouldContain("#define NDDSUSERDllExport __attribute__((visibility(\"default\")))");
        header.ShouldContain("#ifndef NDDS_STANDALONE_TYPE");
        header.ShouldContain("#endif // NDDS_STANDALONE_TYPE");
        header.ShouldContain("NDDSUSERDllExport bool operator==(");
        header.ShouldContain("NDDSUSERDllExport static std::string value()");
        header.ShouldContain("NDDSUSERDllExport static void register_type(");
        header.ShouldContain("NDDSUSERDllExport static const type& get();");
        header.ShouldContain("::std::int32_t count = 0;");
        header.ShouldContain("bool enabled = false;");
        header.ShouldContain("char initial = '\\0';");
        header.ShouldContain("wchar_t wide = L'\\0';");
        header.ShouldContain("float weight = 0.0F;");
        header.ShouldContain("double ratio = 0.0;");
        header.ShouldContain("::rti::core::LongDouble precise = ::rti::core::LongDouble{};");
        header.ShouldContain("::std::int32_t count,");
        header.ShouldContain("bool enabled");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "reading.cxx"));
        valueSource.ShouldContain("// Value construction");
        valueSource.ShouldContain("Reading::Reading(\n    ::std::int32_t count,");
        valueSource.ShouldContain("// Value comparison");
        valueSource.ShouldContain("// Stream representation");
        valueSource.ShouldNotContain("argument0");

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "readingPlugin.cxx"));
        pluginSource.ShouldContain("#ifndef NDDS_STANDALONE_TYPE");
        pluginSource.ShouldContain("#endif // NDDS_STANDALONE_TYPE");
        pluginSource.ShouldContain("namespace Example\n{\nnamespace\n{\n// Keep RTI plugin state private to this translation unit.\nstruct wireloom_Reading_type_context");
        pluginSource.ShouldContain("// Bind each IDL member to its RTI DynamicType metadata.");
        pluginSource.ShouldContain("offsetof(::Example::Reading, count)");
        pluginSource.ShouldContain("factory.add_member(*type, \"count\",");
        pluginSource.ShouldNotContain("wireloom_type_");
    }

    /// <summary>Derives C++ names and file names from arbitrary equivalent IDL inputs.</summary>
    [Theory]
    [InlineData("struct Counter { long count; };", "Counter")]
    [InlineData("module Device { struct Status { boolean enabled; }; };", "Status")]
    [InlineData("module Site { module Unit { struct Reading { double temperature; }; }; };", "Reading")]
    [InlineData("struct First { short value; }; struct Second { wchar character; };", "Second")]
    public void GeneratesRenamedAndReshapedScalarDeclarations(string source, string typeName)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source, "user-contract");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "user-contract.hpp"));
        header.ShouldContain("struct NDDSUSERDllExport " + typeName);
        header.ShouldNotContain("CorpusAnnotations");
        header.ShouldNotContain("CorpusPrimitives");
    }

    /// <summary>Plans optional self-recursive fixed struct arrays as staged metadata references.</summary>
    [Fact]
    public void GeneratesOptionalSelfRecursiveArray()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "module CorpusOptionalRecursiveArray { struct Node { long value; @optional Node children[2]; }; };";

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "contract.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "contractPlugin.cxx"));
        var implementation = File.ReadAllText(Path.Combine(workspace.OutputPath, "contract.cxx"));
        header.ShouldContain("::dds::core::optional<");
        header.ShouldContain("::dds::core::array<");
        header.ShouldContain("children = {};");
        implementation.ShouldContain("left.children == right.children");
        implementation.ShouldContain("swap(left.children, right.children)");
        plugin.ShouldContain("factory.create_array(");
        plugin.ShouldContain("*type,");
        plugin.ShouldContain(".optional(true)");
        var accessorStart = plugin.IndexOf(
            "const dynamic_type<::CorpusOptionalRecursiveArray::Node>::type&\ndynamic_type<::CorpusOptionalRecursiveArray::Node>::get()",
            StringComparison.Ordinal);
        var accessorEnd = plugin.IndexOf("\n}", accessorStart, StringComparison.Ordinal) + 2;
        var accessor = plugin[accessorStart..accessorEnd];
        accessor.ShouldContain("wireloom_initialize_dynamic_types();");
        accessor.ShouldNotContain("wireloom_initialize_type_plugins();");
    }

    /// <summary>Uses staged metadata initialization for union accessors in a recursive-array plan.</summary>
    [Fact]
    public void UnionDynamicTypeAccessorDoesNotCreatePluginsForOptionalRecursiveArrayPlan()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "module MetadataInitialization { struct Node { @optional Node children[2]; }; "
                              + "union Choice switch(long) { case 0: long number; }; };";

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "contractPlugin.cxx"));
        var accessorStart = plugin.IndexOf(
            "dynamic_type<::MetadataInitialization::Choice>::get()",
            StringComparison.Ordinal);
        accessorStart.ShouldBeGreaterThanOrEqualTo(0);
        var accessorEnd = plugin.IndexOf("\n}", accessorStart, StringComparison.Ordinal) + 2;
        var accessor = plugin[accessorStart..accessorEnd];
        accessor.ShouldContain("wireloom_initialize_dynamic_types();");
        accessor.ShouldNotContain("wireloom_initialize_type_plugins();");
        plugin.ShouldContain("void topic_type_support<::MetadataInitialization::Choice>::register_type(");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
    }

    /// <summary>Rejects unsupported constructs instead of quietly emitting a partial or different contract.</summary>
    [Theory]
    [InlineData("typedef sequence<long, 2> Values; struct Sample { Values values[3]; };")]
    [InlineData("")]
    [InlineData("struct Sample { @external long value; };")]
    [InlineData("struct Sample { @must_understand long value; };")]
    [InlineData("@appendable struct Sample { @must_understand @optional long value; };")]
    [InlineData("@appendable struct Sample { @must_understand @key long value; };")]
    [InlineData("@appendable struct Sample { @must_understand @hashid(\"value\") long value; };")]
    [InlineData("@appendable struct Sample { @must_understand string value; };")]
    [InlineData("@final struct Sample { @must_understand long value; };")]
    [InlineData("struct Sample { @range(min = 0, max = 10) string value; };")]
    [InlineData("struct Sample { @min(0) @range(min = 0, max = 10) long value; };")]
    [InlineData("struct Child { long value; }; struct Sample { @default(1) Child child; };")]
    [InlineData("@data_representation(XCDR) @appendable struct Sample { long value; };")]
    [InlineData("@data_representation(XCDR2) @data_representation(XCDR2) @appendable struct Sample { long value; };")]
    [InlineData("@allowed_data_representation(XCDR) @appendable struct Sample { long value; };")]
    [InlineData("@allowed_data_representation(XCDR2) @allowed_data_representation(XCDR2) @appendable struct Sample { long value; };")]
    [InlineData("@data_representation(XCDR2) @allowed_data_representation(XCDR2) @appendable struct Sample { long value; };")]
    [InlineData("@nested(1) struct Sample { long value; };")]
    [InlineData("@nested @nested struct Sample { long value; };")]
    [InlineData("@topic @nested struct Sample { long value; };")]
    [InlineData("@language_binding(APPENDABLE) struct Sample { long value; };")]
    [InlineData("@language_binding(PLAIN) @language_binding(PLAIN) struct Sample { long value; };")]
    [InlineData("@transfer_mode(OUTOFBAND) struct Sample { long value; };")]
    [InlineData("struct Sample { @unit(\"meters\") long value; };")]
    [InlineData("struct Sample { @range(min = -1, max = 1) @unit(\"meters\") double value; };")]
    [InlineData("struct Sample { @external string value; };")]
    [InlineData("typedef string<16> Text; struct Sample { @external Text value; };")]
    [InlineData("struct Sample { @external wstring<16> value; };")]
    [InlineData("struct Sample { @external @optional string<16> value; };")]
    [InlineData("@autoid(SEQUENTIAL) module Data { struct Sample { long value; }; };")]
    public void RejectsUnsupportedShapesWithNoOutput(string source)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("Error");
        result.Diagnostics.ShouldContain("contract.idl(");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Emits arrays containing aggregate and string values.</summary>
    [Fact]
    public void EmitsAggregateAndStringArrays()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "struct Child { long value; }; struct Sample { Child values[2]; string labels[2]; };";

        // Act
        var result = workspace.Generate(source, "array-elements");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "array-elements.hpp"));
        header.ShouldContain("values");
        header.ShouldContain("labels");
    }

    /// <summary>Uses unqualified generated types in their owning C++ namespace.</summary>
    [Fact]
    public void UsesNamespaceRelativeTypesInDeclarations()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "module Showcase { enum Mode { Ready }; typedef Mode ModeArray[2]; "
            + "struct Point { Mode mode; }; struct Sample { ModeArray modes; Point point; }; "
            + "union Payload switch(Mode) { case Ready: Point point; default: Mode mode; }; };";

        // Act
        var result = workspace.Generate(source, "relative-types");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "relative-types.hpp"));
        header.ShouldContain("using ModeArray = ::dds::core::array< Mode, 2 >;");
        header.ShouldContain("Mode mode = Mode::Ready;");
        header.ShouldContain("ModeArray modes = {");
        header.ShouldContain("Point point = {};");
        header.ShouldContain("/// @brief IDL struct Showcase::Point\n/// @details Extensibility:");
        header.ShouldContain("Point();\n\n    /// @brief Constructs a Point from its values.");
        header.ShouldContain("Point(const Point&) = default;\n\n    /// @brief Moves another value.");
        header.ShouldContain("NDDSUSERDllExport void swap(Point& left, Point& right) noexcept;");
        header.ShouldContain("NDDSUSERDllExport std::ostream& operator<<(std::ostream& output, const Point& value);");
        header.ShouldContain("Mode& _d();");
        header.ShouldContain("Point& point();");
        header.ShouldNotContain("// IDL type Showcase::Point");
    }

    /// <summary>Rejects declaration annotations that are unsupported on non-struct declarations.</summary>
    [Theory]
    [InlineData("@language_binding(FLAT_DATA) enum Color { RED }; struct Sample { Color color; };")]
    [InlineData("@language_binding(FLAT_DATA) union Choice switch(long) { case 0: long number; }; struct Sample { Choice choice; };")]
    [InlineData("@language_binding(FLAT_DATA) typedef long Number; struct Sample { Number value; };")]
    [InlineData("@language_binding(FLAT_DATA) const long Limit = 10; struct Sample { long value; };")]
    public void RejectsUnsupportedDeclarationAnnotations(string source)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("The C++ generator does not support annotation '@language_binding'");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Retains the C++ backend's supported extensibility annotations on enum and union declarations.</summary>
    [Theory]
    [InlineData("@final enum Color { RED }; struct Sample { Color color; };")]
    [InlineData("@appendable union Choice switch(long) { case 0: long number; }; struct Sample { Choice choice; };")]
    public void AcceptsExtensibilityAnnotationsOnEnumsAndUnions(string source)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
    }

    /// <summary>Uses bound defaults and documents inclusive scalar ranges for the scoped C++ case.</summary>
    [Fact]
    public void GeneratesBoundDefaultsAndInclusiveRangeDocumentation()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = """
            module Defaults {
                enum Color { GREEN, @default_literal RED, BLUE };
                @appendable struct Sample {
                    @min(0) @max(100) @default(50) long value;
                    @range(min = -32, max = 31) long ranged;
                    @default(BLUE) Color color;
                };
            };
            """;

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "contract.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "contractPlugin.cxx"));
        header.ShouldContain("enum class Color");
        header.ShouldContain("The default enumerator is RED.");
        header.ShouldContain("::std::int32_t value = 50;");
        header.ShouldContain("::std::int32_t ranged = 0;");
        header.ShouldContain("Color color = Color::BLUE;");
        header.ShouldContain("Inclusive value range: [0, 100].");
        header.ShouldContain("Inclusive value range: [-32, 31].");
        header.ShouldContain("This is documentation only; the public field does not enforce the");
        header.ShouldNotContain("std::int32_t value = 50;\n    if");
        plugin.ShouldContain("_annotations._defaultValue._d = RTI_XCDR_TK_ENUM;");
        plugin.ShouldContain("_annotations._defaultValue._u.enumerated_value = 2;");
        plugin.ShouldContain("_annotations._minValue._u.long_value = 0L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 100L;");
        plugin.ShouldContain("_annotations._minValue._u.long_value = -32L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 31L;");
    }

    /// <summary>Preserves the RTI default-only bindings, range/unit metadata, and bounded external string value shape.</summary>
    [Fact]
    public void GeneratesRtiAnnotationDefaultsRangeUnitAndExternalMember()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "10-annotations-rti.idl"));

        // Act
        var result = workspace.Generate(source, "10-annotations-rti");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-annotations-rti.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-annotations-rtiPlugin.cxx"));
        header.ShouldContain("dds::core::external<::std::string> externalText = {};");
        header.ShouldContain("struct topic_type_has_external_members<::CorpusRtiAnnotations::Sample>");
        header.ShouldContain("language_binding(PLAIN)");
        header.ShouldContain("transfer_mode(INBAND)");
        header.ShouldContain("@resolve_name(false)");
        header.ShouldContain("Unit metadata: meters.");
        plugin.ShouldContain("factory.add_member(*type, \"externalText\",");
        plugin.ShouldContain("3, false, false, true);");
        plugin.ShouldContain("CDR serialization for IDL @external members requires an RTI external-reference callback adapter.");
        plugin.ShouldContain("CDR deserialization for IDL @external members requires an RTI external-reference callback adapter.");
        plugin.ShouldContain("_annotations._minValue._u.long_value = -32L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 31L;");
        plugin.ShouldContain("_annotations._unitValue = (RTIXCdrChar *) \"meters\";");
        plugin.ShouldContain("#error \"nested/default/range TypeCode adapter requires RTI Connext DDS 7.7.0.\"");
    }

    /// <summary>Generates must-understand metadata for the scoped appendable required scalar case.</summary>
    [Fact]
    public void GeneratesMustUnderstandMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate("@appendable struct Sample { @must_understand long value; };");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "contract.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "contractPlugin.cxx"));
        header.ShouldContain("::std::int32_t value = 0;");
        header.ShouldContain("XTypes must-understand flag in generated DynamicType metadata");
        header.ShouldContain("api_cpp2/classdds_1_1core_1_1xtypes_1_1Member.html");
        plugin.ShouldContain("::dds::core::xtypes::Member(\"value\"");
        plugin.ShouldContain("primitive_type<::std::int32_t>()");
        plugin.ShouldContain(".id(0)");
        plugin.ShouldContain(".must_understand(true)");
        plugin.ShouldContain("::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE");
        plugin.ShouldNotContain(".optional(true)");
    }

    /// <summary>Preserves the XCDR2-only representation on the generated helper and RTI TypeCode.</summary>
    [Fact]
    public void GeneratesXcdr2OnlyDataRepresentation()
        => GeneratesXcdr2OnlyRepresentation("@data_representation(XCDR2)");

    /// <summary>Preserves the documented allowed_data_representation synonym.</summary>
    [Fact]
    public void GeneratesXcdr2OnlyAllowedDataRepresentation()
        => GeneratesXcdr2OnlyRepresentation("@allowed_data_representation(XCDR2)");

    /// <summary>Emits RTI FlatData's final-sample and offset API for a fixed-width long struct.</summary>
    [Fact]
    public void GeneratesFinalFlatDataSampleAndOffsetApi()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "10-flat-data-binding.idl"));

        // Act
        var result = workspace.Generate(source, "10-flat-data-binding");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-flat-data-binding.hpp"));
        var value = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-flat-data-binding.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "10-flat-data-bindingPlugin.cxx"));
        header.ShouldContain("#include <rti/topic/flat/FlatData.hpp>");
        header.IndexOf("#include <rti/core/Exception.hpp>", StringComparison.Ordinal)
            .ShouldBeLessThan(header.IndexOf("#include <rti/topic/flat/FlatData.hpp>", StringComparison.Ordinal));
        header.ShouldContain("class NDDSUSERDllExport SampleOffset");
        header.ShouldContain("using Sample = ::rti::flat::Sample< SampleOffset >;");
        header.ShouldContain("struct type_programs<::CorpusFlatDataBinding::Sample>");
        header.ShouldContain("dynamic_type<::CorpusFlatDataBinding::SamplePlainHelper>");
        header.ShouldContain("REGISTER_TOPIC_TYPE(::CorpusFlatDataBinding::SamplePlainHelper)");
        header.ShouldContain("topic_type_name<::CorpusFlatDataBinding::SamplePlainHelper>");
        header.ShouldContain("topic_type_support<::CorpusFlatDataBinding::SamplePlainHelper>");
        header.ShouldContain("TypePluginKind::STL");
        header.ShouldContain("struct NDDSUSERDllExport SamplePlainHelper");
        header.ShouldContain("is_flat_data_type_only<::CorpusFlatDataBinding::Sample>");
        header.ShouldContain("flat_type_traits<::CorpusFlatDataBinding::Sample>");
        header.ShouldContain("ExtensibilityKind::FINAL");
        value.ShouldContain("SampleOffset::id(::std::int32_t value)");
        value.ShouldContain("SampleOffset::value(::std::int32_t value)");
        value.ShouldContain("SamplePlainHelper::SamplePlainHelper()");
        plugin.ShouldContain("sizeof(::CorpusFlatDataBinding::SamplePlainHelper)");
        plugin.ShouldContain("create_plain_helper_type_plugin()");
        plugin.ShouldContain("plain_helper_type_plugin->register_type(");
        plugin.ShouldContain("plain_helper_type_plugin->serialize_to_cdr_buffer(");
        plugin.ShouldContain("plain_helper_type_plugin->deserialize_from_cdr_buffer(");
        var helperAllocation = plugin.Substring(plugin.IndexOf(
            "void topic_type_support<::CorpusFlatDataBinding::SamplePlainHelper>::allocate_sample(",
            StringComparison.Ordinal));
        helperAllocation.ShouldContain("    (void) sample;");
        helperAllocation.ShouldNotContain("sample = ::CorpusFlatDataBinding::SamplePlainHelper{};");
        plugin.ShouldContain("offsetof(::CorpusFlatDataBinding::SamplePlainHelper, id)");
        plugin.ShouldContain("_allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        plugin.ShouldContain("plain_helper_type->native()._data._annotations._allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        plugin.ShouldContain("_kind |= DDS_TK_FLAT_DATA_LANGUAGE_BINDING;");
        plugin.ShouldContain("ProgramsSingleton<");
        plugin.ShouldContain("FlatData CDR supports XCDR2 only.");
        plugin.ShouldContain("RTI_XCDR_ENCAPSULATION_ID_CDR2_LE");
        plugin.ShouldContain("rti::domain::register_type_plugin");
        plugin.ShouldContain("PRESTypePluginDefaultParticipantData_new");
        plugin.ShouldContain("RTIXCdrXTypesComplianceMask_verifyGeneratedXTypesMask(0x0000018C)");
        plugin.ShouldContain("DDS_TypeCodeFactory_assert_programs_in_global_list");
        plugin.ShouldContain("RTI_XCDR_PROGRAM_MASK_TYPEPLUGIN");
        plugin.ShouldContain("participant_data->programs = programs;");
        plugin.ShouldContain("DDS_TypeCodeFactory_remove_programs_from_global_list");
        plugin.ShouldContain("RTI_DDS_VERSION_REVISION != 0");
        plugin.ShouldContain("PRESTypePluginDefaultEndpointData_createWriterPool");
        plugin.ShouldContain("endpoint_info->usesFstSampleDataPool != RTI_FALSE");
        plugin.ShouldContain("_create_flat_data_sentinel");
        plugin.ShouldContain("PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS");
        plugin.ShouldContain("PRESTypePluginDefaultEndpointData_getWriterLoanedSample");
        plugin.ShouldContain("RTIXCdrStream_deserializeNByte");
        plugin.ShouldContain("PRES_TYPEPLUGIN_NO_KEY");
        plugin.ShouldContain("PRES_TYPEPLUGIN_FLATDATA_TYPE");
        plugin.ShouldNotContain("create_type_plugin(*type)");
        plugin.ShouldNotContain("RTI FlatData registration requires the dedicated FlatData plugin callbacks");
        plugin.ShouldNotContain("offsetof(::CorpusFlatDataBinding::Sample, id)");
    }

    /// <summary>Rejects unsupported FlatData extensibility and variable-size members.</summary>
    [Theory]
    [InlineData("@language_binding(FLAT_DATA) struct Sample { long value; };", "only on final structs")]
    [InlineData("@language_binding(FLAT_DATA) @final struct Sample { string<8> value; };", "direct long members")]
    public void RejectsUnsupportedFlatDataShapes(string source, string diagnostic)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain(diagnostic);
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    private static void GeneratesXcdr2OnlyRepresentation(string annotation)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = $"{annotation} @appendable struct Sample {{ @id(1) long value; }};";

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "contract.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "contractPlugin.cxx"));
        header.ShouldContain("The IDL type permits XCDR2 only.");
        header.ShouldContain("return dds::core::policy::DataRepresentation::xcdr2();");
        header.ShouldContain("DataRepresentation::auto_id());");
        header.ShouldContain("static dds::core::policy::DataRepresentationId data_representation()");
        plugin.ShouldContain("_allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        plugin.ShouldContain("XCDR2-only representation.");
        plugin.ShouldContain("#include <ndds/ndds_version.h>");
        plugin.ShouldContain("RTI_DDS_VERSION_MAJOR != 7");
        plugin.ShouldContain("RTI_DDS_VERSION_MINOR != 7");
        plugin.ShouldContain("RTI_DDS_VERSION_RELEASE != 0");
        plugin.ShouldContain("RTI_DDS_VERSION_REVISION != 0");
        plugin.ShouldContain("#error \"XCDR2-only representation TypeCode adapter requires RTI Connext DDS 7.7.0.\"");
        plugin.ShouldContain("representation != dds::core::policy::DataRepresentation::xcdr2())");
        plugin.ShouldContain("This IDL type permits XCDR2 only.");
        plugin.IndexOf("_allowedDataRepresentationMask", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("factory.create_type_plugin(*type)", StringComparison.Ordinal));
    }

    /// <summary>Preserves topic, nested, key, and direct long default semantics from the RTI profile.</summary>
    [Fact]
    public void GeneratesExtendedAnnotationTopicAndNestedSemantics()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = """
            module CorpusAnnotationVariants {
                @topic struct Sample {
                    @key long id;
                    @default(3) long value;
                };
                @nested struct NestedValue { long value; };
            };
            """;

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "contract.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "contractPlugin.cxx"));
        header.ShouldContain("::std::int32_t id = 0;");
        header.ShouldContain("::std::int32_t value = 3;");
        header.ShouldContain("REGISTER_TOPIC_TYPE(::CorpusAnnotationVariants::Sample)");
        header.ShouldNotContain("REGISTER_TOPIC_TYPE(::CorpusAnnotationVariants::NestedValue)");
        header.ShouldContain("REGISTER_TOPIC_TYPE(::CorpusAnnotationVariants::Sample)");
        header.ShouldNotContain("REGISTER_TOPIC_TYPE(::CorpusAnnotationVariants::NestedValue)");
        header.ShouldContain("struct topic_type_name<::CorpusAnnotationVariants::NestedValue>");
        var nestedTraitsStart = header.IndexOf(
            "struct topic_type_support<::CorpusAnnotationVariants::NestedValue>",
            StringComparison.Ordinal);
        nestedTraitsStart.ShouldBeGreaterThanOrEqualTo(0);
        var nestedTraitsEnd = header.IndexOf("};", nestedTraitsStart, StringComparison.Ordinal) + 2;
        var nestedTraits = header[nestedTraitsStart..nestedTraitsEnd];
        nestedTraits.ShouldContain("reset_sample");
        nestedTraits.ShouldContain("allocate_sample");
        nestedTraits.ShouldNotContain("register_type");
        nestedTraits.ShouldNotContain("to_cdr_buffer");
        nestedTraits.ShouldNotContain("from_cdr_buffer");
        plugin.ShouldContain("factory.add_member(*type, \"id\"");
        plugin.ShouldContain("primitive_type<::std::int32_t>(), 0, true);");
        plugin.ShouldContain("_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;");
        plugin.ShouldContain("_members[1]._annotations._defaultValue._u.long_value = 3;");
        plugin.ShouldContain("_annotations._isNested = RTI_XCDR_TRUE;");
        plugin.ShouldContain("#include <ndds/ndds_version.h>");
        plugin.ShouldContain("RTI_DDS_VERSION_MAJOR != 7");
        plugin.ShouldContain("RTI_DDS_VERSION_MINOR != 7");
        plugin.ShouldContain("RTI_DDS_VERSION_RELEASE != 0");
        plugin.ShouldContain("RTI_DDS_VERSION_REVISION != 0");
        plugin.ShouldContain("#error \"nested/default TypeCode adapter requires RTI Connext DDS 7.7.0.\"");
        ShouldContainExactlyOnce(plugin, "factory.create_type_plugin(*type)");
        plugin.ShouldNotContain("topic_type_support<::CorpusAnnotationVariants::NestedValue>::register_type");
        plugin.ShouldNotContain("topic_type_support<::CorpusAnnotationVariants::NestedValue>::to_cdr_buffer");
    }

    /// <summary>Rejects direct by-value self-reference before emitting an invalid C++ type.</summary>
    [Theory]
    [InlineData("struct Node { Node child; };")]
    [InlineData("struct Node { Node children[2]; };")]
    public void RejectsDirectRecursiveAggregate(string source)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("contract.idl(1,");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Rejects mutually recursive collection metadata until its plugin initialization is supported.</summary>
    [Fact]
    public void RejectsMutuallyRecursiveAggregateMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "struct First { sequence<Second, 4> children; }; " +
                              "struct Second { sequence<First, 4> children; };";

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("mutually recursive aggregate metadata");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Diagnoses valid IDL identifiers that would be invalid or ambiguous C++ declarations.</summary>
    [Theory]
    [InlineData("struct Sample { long class; };")]
    [InlineData("struct Sample { long xor; };")]
    [InlineData("struct Sample { long char16_t; };")]
    [InlineData("struct Sample { long char32_t; };")]
    [InlineData("module std { struct Sample { long value; }; };")]
    [InlineData("struct wireloom_float_equal { long value; };")]
    [InlineData("module namespace { struct Sample { long value; }; };")]
    [InlineData("struct Sample { long Sample; };")]
    public void RejectsUnmappableCppIdentifiers(string source)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("DDSGCPP001");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Propagates shared IDL parse errors without writing C++ files.</summary>
    [Fact]
    public void ReportsInvalidIdl()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate("struct Sample { long value;");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("contract.idl(");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Returns a clear error when the requested output path is already a file.</summary>
    [Fact]
    public void ReportsUnwritableOutputDirectory()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        File.WriteAllText(workspace.OutputPath, "occupied");

        // Act
        var result = workspace.Generate("struct Sample { long value; };");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("Wireloom C++:");
        File.ReadAllText(workspace.OutputPath).ShouldBe("occupied");
    }

    private sealed class GenerationWorkspace : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("wireloom-cpp-tests-");

        internal string OutputPath => Path.Combine(directory.FullName, "generated");

        private void WriteFile(string relativePath, string source)
        {
            var path = Path.Combine(directory.FullName, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, source);
        }

        internal (int ExitCode, string Diagnostics) Generate(
            string source,
            string stem = "contract",
            IReadOnlyList<string>? defines = null,
            IReadOnlyList<string>? includeDirectories = null,
            IReadOnlyDictionary<string, string>? siblingFiles = null)
        {
            var input = Path.Combine(directory.FullName, stem + ".idl");
            File.WriteAllText(input, source);
            foreach (var (path, contents) in siblingFiles ?? new Dictionary<string, string>())
            {
                WriteFile(path, contents);
            }

            using var output = new StringWriter();
            using var errors = new StringWriter();
            var arguments = new List<string> { "generate", input, OutputPath };
            foreach (var define in defines ?? [])
            {
                arguments.Add("--define");
                arguments.Add(define);
            }

            foreach (var includeDirectory in includeDirectories ?? [])
            {
                arguments.Add("--include-directory");
                arguments.Add(includeDirectory);
            }

            var exitCode = CppCommand.Run(arguments.ToArray(), output, errors);
            return (exitCode, errors.ToString());
        }

        public void Dispose() => directory.Delete(recursive: true);
    }
}
