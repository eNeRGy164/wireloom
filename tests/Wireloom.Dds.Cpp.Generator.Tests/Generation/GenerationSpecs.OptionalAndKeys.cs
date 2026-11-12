namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Protects IDL-derived generation and fail-closed behavior at the command boundary.</summary>
public sealed partial class GenerationSpecs
{
    /// <summary>Preserves optional bounded narrow-string presence, member identity, and Doxygen examples.</summary>
    [Fact]
    public void EmitsOptionalBoundedStringPresenceAndMetadata()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-evolution-optional.idl"));

        // Act
        var result = workspace.Generate(source, "09-evolution-optional");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-evolution-optional.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-evolution-optionalPlugin.cxx"));
        header.ShouldContain("::dds::core::optional<::std::int32_t> optionalValue = {};");
        header.ShouldContain("::dds::core::optional<::std::string> optionalText = {};");
        header.ShouldContain("Optional string storage preserves set and unset state.");
        header.ShouldContain("sample.optionalValue = 7;");
        header.ShouldContain("sample.optionalText = \"text\";");
        header.ShouldContain("sample.optionalText.has_value()");
        header.ShouldContain("sample.optionalText.value()");
        header.ShouldContain("sample.optionalText.reset();");
        header.ShouldContain("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1optional.html");
        header.ShouldNotContain("/current/");
        var required = GetMemberRegistration(plugin, "Message", "requiredValue");
        var optionalValue = plugin.Substring(plugin.IndexOf("::dds::core::xtypes::Member(\"optionalValue\"", StringComparison.Ordinal),
            plugin.IndexOf("::dds::core::xtypes::Member(\"optionalText\"", StringComparison.Ordinal) -
            plugin.IndexOf("::dds::core::xtypes::Member(\"optionalValue\"", StringComparison.Ordinal));
        var optionalText = plugin.Substring(plugin.IndexOf("::dds::core::xtypes::Member(\"optionalText\"", StringComparison.Ordinal));
        required.ShouldContain("1, false);");
        optionalValue.ShouldContain(".id(2)\n                .optional(true));");
        optionalText.ShouldContain("::dds::core::xtypes::StringType(16)");
        optionalText.ShouldContain(".id(3)\n                .optional(true));");
        plugin.IndexOf("\"requiredValue\"", StringComparison.Ordinal).ShouldBeLessThan(
            plugin.IndexOf("\"optionalValue\"", StringComparison.Ordinal));
        plugin.IndexOf("\"optionalValue\"", StringComparison.Ordinal).ShouldBeLessThan(
            plugin.IndexOf("\"optionalText\"", StringComparison.Ordinal));
    }

    /// <summary>Keeps optional collection support limited to the reviewed sequence and array shapes.</summary>
    [Fact]
    public void RejectsOptionalShapesOutsideTheSupportedSubset()
    {
        // Arrange
        var unsupportedInputs = new[]
        {
            "struct Sample { @optional sequence<short, 4> value; };",
            "struct Sample { @optional sequence<long, 5> value; };",
            "struct Sample { @optional sequence<long> value; };",
            "struct Sample { @optional long values[3]; };",
            "struct Sample { @optional long values[2][2]; };",
            "enum Mode { IDLE, ACTIVE }; struct Sample { @optional Mode mode; };",
            "struct Node { @optional Node value; };",
            "struct Sample { @optional wstring<16> value; };",
            "struct Sample { @optional string value; };",
            "struct Sample { @optional string<16> values[2]; };",
            "struct Node { @optional sequence<Node, 4> children; };",
            "struct First { @optional Second children[2]; }; struct Second { @optional First children[2]; };",
        };

        foreach (var source in unsupportedInputs)
        {
            using var workspace = new GenerationWorkspace();

            // Act
            var result = workspace.Generate(source);

            // Assert
            result.ExitCode.ShouldBe(1);
            result.Diagnostics.ShouldContain("Error DDSG");
            Directory.Exists(workspace.OutputPath).ShouldBeFalse();
        }
    }

    /// <summary>Preserves optional completed aggregate values and the RTI union branch contract.</summary>
    [Fact]
    public void EmitsOptionalAggregateMembersAndUnionBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-optional-aggregate-member.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "09-optional-aggregate-member");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-aggregate-member.hpp"));
        header.ShouldContain("::dds::core::optional<Payload> payload = {};");
        header.ShouldContain("::dds::core::optional<PayloadAlias> payloadAlias = {};");
        header.ShouldContain("::dds::core::optional<Choice> choice = {};");
        header.ShouldContain("::dds::core::optional<ChoiceAlias> choiceAlias = {};");
        header.ShouldContain("using PayloadAlias = Payload;");
        header.ShouldContain("using ChoiceAlias = Choice;");
        header.ShouldContain("sample.payload = Payload{};");
        header.ShouldContain("sample.payloadAlias.has_value()");
        header.ShouldContain("sample.payloadAlias.value()");
        header.ShouldContain("sample.choice.reset();");
        header.ShouldContain("const Payload& payload() const;");
        header.ShouldContain("Payload& payload();");
        header.ShouldContain("void payload(const Payload& value);");

        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-aggregate-member.cxx"));
        values.ShouldContain("return left.payload == right.payload");
        values.ShouldContain("swap(left.choiceAlias, right.choiceAlias);");
        values.ShouldContain("if (value.payload.has_value())");
        values.ShouldContain("payload: <unset>");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-aggregate-memberPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::Member(\n                \"payload\",");
        plugin.ShouldContain(".optional(true));");
        var unionBranchStart = plugin.IndexOf("::dds::core::xtypes::UnionMember(", StringComparison.Ordinal);
        var nextUnionBranch = plugin.IndexOf(
            "::dds::core::xtypes::UnionMember(", unionBranchStart + 1, StringComparison.Ordinal);
        var payloadUnionBranch = plugin[unionBranchStart..nextUnionBranch];
        payloadUnionBranch.ShouldContain("\"payload\"");
        payloadUnionBranch.ShouldContain("                0));");
        payloadUnionBranch.ShouldNotContain(".optional(true)");
        values.ShouldNotContain("value.payload_.has_value()");
        values.ShouldContain("payload: \" << value.payload_;");
        header.ShouldNotContain("/current/");
    }

    /// <summary>Preserves optional collection storage, absent versus empty state, IDs, and dimensions.</summary>
    [Fact]
    public void EmitsOptionalBoundedSequenceAndFixedArrayPresence()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-optional-collections.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "09-optional-collections");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-collections.hpp"));
        header.ShouldContain("::dds::core::optional<::omg::types::bounded_sequence< ::std::int32_t, 4 >> values = {};");
        header.ShouldContain("::dds::core::optional<::dds::core::array< ::std::int32_t, 2 >> items = {};");
        header.ShouldContain("Effective sequence metadata bound is 4.");
        header.ShouldContain("Fixed array dimensions are 2.");
        header.ShouldContain("if (!sample.values.has_value()) { /* absent */ }");
        header.ShouldContain("sample.values = ::omg::types::bounded_sequence< ::std::int32_t, 4 >{}; // present and empty");
        header.ShouldContain("sample.items.has_value()");
        header.ShouldContain("sample.items.reset();");
        header.ShouldContain("Sample(\n");
        header.ShouldContain("const ::dds::core::optional<::omg::types::bounded_sequence< ::std::int32_t, 4 >>& values,");
        header.ShouldContain("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1optional.html");
        header.ShouldNotContain("/current/");

        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-collections.cxx"));
        values.ShouldContain("return left.values == right.values");
        values.ShouldContain("&& left.items == right.items;");
        values.ShouldContain("swap(left.values, right.values);");
        values.ShouldContain("swap(left.items, right.items);");
        values.ShouldContain("if (value.values.has_value())");
        values.ShouldContain("if (value.items.has_value())");
        values.ShouldContain("items: <unset>");
        values.ShouldContain("values: <unset>");
        values.ShouldContain("value.items.value()[0]");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-collectionsPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::Member(\"values\"");
        plugin.ShouldContain("*factory.create_sequence(");
        plugin.ShouldContain("                4))");
        plugin.ShouldContain(".id(1)\n                .optional(true));");
        plugin.ShouldContain("::dds::core::xtypes::Member(\"items\"");
        plugin.ShouldContain("*factory.create_array(");
        plugin.ShouldContain("                2))");
        plugin.ShouldContain(".id(2)\n                .optional(true));");
        plugin.ShouldContain("::rti::topic::interpreter::get_opt_array_sample_access_info<::std::int32_t>();");
        plugin.ShouldContain("::rti::topic::interpreter::detail::sequence_helper<");
        var resetIndex = plugin.IndexOf(">::reset_sample(", StringComparison.Ordinal);
        resetIndex.ShouldBeGreaterThanOrEqualTo(0);
        var resetAndAllocate = plugin.Substring(resetIndex);
        resetAndAllocate.ShouldContain("sample = ::CorpusOptionalCollections::Sample{};");
        resetAndAllocate.ShouldNotContain("reset_sample(sample.values)");
        resetAndAllocate.ShouldNotContain("allocate_sample(sample.values");
        resetAndAllocate.ShouldNotContain("reset_sample(sample.items)");
        resetAndAllocate.ShouldNotContain("allocate_sample(sample.items");
    }

    /// <summary>Preserves optional bounded narrow and wide string sequence shapes and metadata.</summary>
    [Fact]
    public void EmitsOptionalBoundedNarrowAndWideStringSequences()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "09-optional-string-sequences.idl"));

        // Act
        var result = workspace.Generate(source, "09-optional-string-sequences");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-string-sequences.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-string-sequences.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "09-optional-string-sequencesPlugin.cxx"));
        header.ShouldContain("::dds::core::optional<::omg::types::bounded_sequence< ::std::string, 4 >> narrowValues = {};");
        header.ShouldContain("::dds::core::optional<::omg::types::bounded_sequence< ::std::wstring, 4 >> wideValues = {};");
        header.ShouldContain("Effective sequence metadata bound is 4.");
        header.ShouldContain("String metadata bound is 16.");
        header.ShouldContain("narrowValues.has_value()");
        header.ShouldContain("wideValues.reset();");
        values.ShouldContain("left.narrowValues == right.narrowValues");
        values.ShouldContain("left.wideValues == right.wideValues");
        values.ShouldContain("swap(left.narrowValues, right.narrowValues);");
        values.ShouldContain("swap(left.wideValues, right.wideValues);");
        values.ShouldContain("narrowValues: <unset>");
        values.ShouldContain("wideValues: <unset>");
        plugin.ShouldContain("::dds::core::xtypes::Member(\"narrowValues\"");
        plugin.ShouldContain("::dds::core::xtypes::Member(\"wideValues\"");
        plugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(16)");
        plugin.ShouldContain("*factory.create_sequence(");
        plugin.ShouldContain("                4))");
        plugin.ShouldContain(".id(1)\n                .optional(true));");
        plugin.ShouldContain(".id(2)\n                .optional(true));");
        plugin.ShouldContain("sequence_helper<");
        plugin.ShouldContain("::omg::types::bounded_sequence< ::std::string, 4 >>::sample_access_info();");
        plugin.ShouldContain("::omg::types::bounded_sequence< ::std::wstring, 4 >>::sample_access_info();");
        plugin.ShouldContain("member_0_sequence_type_code_0->_sampleAccessInfo =");
        plugin.ShouldContain("detail::sequence_helper<\n                ::std::string>::sample_access_info();");
        plugin.ShouldContain("detail::sequence_helper<\n                ::std::wstring>::sample_access_info();");
        plugin.ShouldContain("plugin->serializeFnc = PRESTypePlugin_interpretedSerialize;");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
    }

    /// <summary>Rejects optional sequences whose elements are not bounded strings.</summary>
    [Fact]
    public void RejectsOptionalSequenceWithPrimitiveElements()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = """
            module UnsupportedOptionalSequence {
                @appendable struct Sample {
                    @optional sequence<long, 4> values;
                };
            };
            """;

        // Act
        var result = workspace.Generate(source, "unsupported-optional-sequence");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("does not support optional member 'values' with type 'Sequence'");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Applies the optional bounded-string sequence rule by type shape.</summary>
    [Fact]
    public void AcceptsOptionalBoundedStringSequenceOutsideCorpusNamesAndBounds()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = """
            module OtherOptionalStringSequence {
                struct Message {
                    @optional sequence<string<7>, 2> texts;
                };
            };
            """;

        // Act
        var result = workspace.Generate(source, "other-optional-string-sequence");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "other-optional-string-sequence.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "other-optional-string-sequencePlugin.cxx"));
        header.ShouldContain("::dds::core::optional<::omg::types::bounded_sequence< ::std::string, 2 >> texts = {};");
        plugin.ShouldContain("::dds::core::xtypes::StringType(7)");
        plugin.ShouldContain(".id(0)\n                .optional(true));");
    }

    /// <summary>Keeps unbounded string-sequence bridge support scoped to narrow strings.</summary>
    [Theory]
    [InlineData("@optional sequence<string<16>, 3> values;")]
    [InlineData("sequence<string<16>, 4> values;")]
    [InlineData("sequence<string> values;")]
    [InlineData("sequence<wstring> values;")]
    [InlineData("@optional sequence<wstring<15>, 4> values;")]
    public void RejectsUnqualifiedStringSequenceBridgeShapes(string member)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = $$"""
            module UnqualifiedStringSequence {
                struct Sample { {{member}} };
            };
            """;

        // Act
        var result = workspace.Generate(source, "unqualified-string-sequence");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "unqualified-string-sequencePlugin.cxx"));
        plugin.ShouldContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
    }

    /// <summary>Preserves declared key flags and nested aggregate metadata without flattening key paths.</summary>
    [Fact]
    public void EmitsNestedAggregateKeyFlagsAndMemberIdentity()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "08-key-nested.idl"));

        // Act
        var result = workspace.Generate(source, "08-key-nested");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-nested.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-nestedPlugin.cxx"));
        header.ShouldContain("/// @brief IDL struct CorpusNestedKeys::Inner");
        header.ShouldContain("/// @details This member is an IDL key.");
        header.ShouldContain("/// @brief IDL struct CorpusNestedKeys::Outer");

        var innerCode = GetMemberRegistration(plugin, "Inner", "code");
        var innerRevision = GetMemberRegistration(plugin, "Inner", "revision");
        var outerIdentity = GetMemberRegistration(plugin, "Outer", "identity");
        var outerPayload = GetMemberRegistration(plugin, "Outer", "payload");
        innerCode.ShouldContain("0, true);");
        innerRevision.ShouldContain("1, false);");
        outerIdentity.ShouldContain("dynamic_type<::CorpusNestedKeys::Inner>::get()");
        outerIdentity.ShouldContain("0, true);");
        outerPayload.ShouldContain("1, false);");

        var innerCodePosition = plugin.IndexOf("factory.add_member(*type, \"code\"", StringComparison.Ordinal);
        var innerRevisionPosition = plugin.IndexOf("factory.add_member(*type, \"revision\"", StringComparison.Ordinal);
        var outerIdentityPosition = plugin.IndexOf("factory.add_member(*type, \"identity\"", StringComparison.Ordinal);
        var outerPayloadPosition = plugin.IndexOf("factory.add_member(*type, \"payload\"", StringComparison.Ordinal);
        innerCodePosition.ShouldBeLessThan(innerRevisionPosition);
        innerRevisionPosition.ShouldBeLessThan(outerIdentityPosition);
        outerIdentityPosition.ShouldBeLessThan(outerPayloadPosition);
    }

    /// <summary>Preserves a union-valued key as one canonical union type and keyed struct member.</summary>
    [Fact]
    public void EmitsUnionTypeMetadataForUnionValuedKey()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "08-key-union.idl"));

        // Act
        var result = workspace.Generate(source, "08-key-union");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-union.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-unionPlugin.cxx"));
        header.ShouldContain("class NDDSUSERDllExport Identity");
        header.ShouldContain("using type = dds::core::xtypes::UnionType;");
        header.ShouldContain("struct topic_type_name<::CorpusUnionKeys::Identity>");
        header.ShouldContain("struct topic_type_support<::CorpusUnionKeys::Identity>");
        plugin.ShouldContain("struct wireloom_Identity_union_context");
        plugin.ShouldContain("type = factory.create_union(\"CorpusUnionKeys::Identity\",");
        plugin.ShouldContain("\"CorpusUnionKeys::Identity\"");
        plugin.ShouldContain("UnionMember(\n                \"number\"");
        plugin.ShouldContain("UnionMember(\n                \"text\"");
        plugin.ShouldContain("wireloom_Identity_union_context_instance().create_type();");

        var identity = GetMemberRegistration(plugin, "Sample", "identity");
        var payload = GetMemberRegistration(plugin, "Sample", "payload");
        identity.ShouldContain("wireloom_Identity_union_type()");
        identity.ShouldEndWith(", true);");
        payload.ShouldEndWith(", false);");
        plugin.ShouldNotContain("Identity_alias");
        plugin.ShouldNotContain("wireloom_Sample_identity_");
        plugin.ShouldNotContain("identity.number");
        plugin.ShouldNotContain("identity.text");
        var unionContextCount = plugin.Split(
            "struct wireloom_Identity_union_context").Length;
        unionContextCount.ShouldBe(2);
    }

    /// <summary>Emits inherited base metadata and declared keys with stable local member identities.</summary>
    [Fact]
    public void EmitsInheritedBaseMetadataAndLocalKeyFlags()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "08-key-inherited.idl"));

        // Act
        var (ExitCode, Diagnostics) = workspace.Generate(source, "08-key-inherited");

        // Assert
        ExitCode.ShouldBe(0, Diagnostics);
        Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-inherited.hpp"));
        header.ShouldContain("struct NDDSUSERDllExport Derived : public Base");
        var derivedTypeStart = header.IndexOf("/// @brief IDL struct CorpusInheritedKeys::Derived", StringComparison.Ordinal);
        derivedTypeStart.ShouldBeGreaterThanOrEqualTo(0);

        var baseDocumentation = header[..derivedTypeStart];
        baseDocumentation.ShouldContain("/// @brief IDL struct CorpusInheritedKeys::Base");
        baseDocumentation.ShouldContain("/// @brief IDL member tenant.\n    /// @details This member is an IDL key.\n    ::std::int32_t tenant = 0;");
        baseDocumentation.ShouldContain("/// @brief IDL member baseValue.\n    ::std::int32_t baseValue = 0;");

        var derivedDocumentation = header[derivedTypeStart..];
        derivedDocumentation.ShouldContain("/// @brief IDL struct CorpusInheritedKeys::Derived");
        derivedDocumentation.ShouldContain("/// @brief IDL member localId.\n    /// @details This member is an IDL key.\n    ::std::int32_t localId = 0;");
        derivedDocumentation.ShouldContain("/// @brief IDL member payload.\n    ::std::int32_t payload = 0;");
        derivedDocumentation.ShouldContain("/// @brief Constructs a Derived from its values.\n    /// @param base Initial values for the base subobject.\n    /// @param localId Initial localId value.");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-inheritedPlugin.cxx"));
        var baseTenant = GetMemberRegistration(plugin, "Base", "tenant");
        baseTenant.ShouldEndWith("0, true);");

        var baseValue = GetMemberRegistration(plugin, "Base", "baseValue");
        baseValue.ShouldEndWith("1, false);");

        var derivedLocalId = GetMemberRegistration(plugin, "Derived", "localId");
        derivedLocalId.ShouldContain(".id(2)");
        derivedLocalId.ShouldContain(".key(true)");

        var derivedPayload = GetMemberRegistration(plugin, "Derived", "payload");
        derivedPayload.ShouldContain(".id(3)");
        plugin.ShouldContain("*::CorpusInheritedKeys::wireloom_Base_type_context_instance().type");
    }

    /// <summary>Documents array and string key boundaries while preserving declared member flags.</summary>
    [Fact]
    public void EmitsKeyFlagsAndDoxygenMetadataForArrayAndStringBoundaries()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "08-key-boundaries.idl"));

        // Act
        var result = workspace.Generate(source, "08-key-boundaries");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-boundaries.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "08-key-boundariesPlugin.cxx"));
        header.ShouldContain("Fixed array dimensions are 2.");
        header.ShouldContain("String metadata bound is 12.");
        header.ShouldContain("String metadata bound is 255.");
        header.ShouldContain("/// @details Fixed array dimensions are 2. This member is an IDL key.\n    ::dds::core::array< ::std::int32_t, 2 > coordinates = {};");
        header.ShouldContain("String metadata bound is 12.");
        header.ShouldContain("String metadata bound is 255.");
        header.ShouldContain("This member is an IDL key.");
        header.ShouldContain("::std::string name = {};");

        GetMemberRegistration(plugin, "ArrayKey", "coordinates").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "ArrayKey", "payload").ShouldEndWith(", false);");
        GetMemberRegistration(plugin, "BoundedStringKey", "name").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "BoundedStringKey", "payload").ShouldEndWith(", false);");
        GetMemberRegistration(plugin, "UnboundedStringKey", "name").ShouldEndWith(", true);");
        GetMemberRegistration(plugin, "UnboundedStringKey", "payload").ShouldEndWith(", false);");

        plugin.ShouldContain("::dds::core::xtypes::StringType(12)");
        plugin.ShouldContain("::dds::core::xtypes::StringType(255)");
    }

    private static string GetMemberRegistration(string plugin, string typeName, string memberName)
    {
        var contextStart = plugin.IndexOf($"struct wireloom_{typeName}_type_context", StringComparison.Ordinal);
        contextStart.ShouldBeGreaterThanOrEqualTo(0);
        var contextEnd = plugin.IndexOf("\n};", contextStart, StringComparison.Ordinal);
        contextEnd.ShouldBeGreaterThan(contextStart);
        var typeRegistration = plugin.IndexOf(
            $"factory.add_member(*type, \"{memberName}\"", contextStart, StringComparison.Ordinal);
        if (typeRegistration < 0)
        {
            typeRegistration = plugin.IndexOf(
                $"::dds::core::xtypes::Member(\"{memberName}\"", contextStart, StringComparison.Ordinal);
        }
        typeRegistration.ShouldBeGreaterThanOrEqualTo(0);
        var nextFactoryRegistration = plugin.IndexOf("factory.add_member(*type, ", typeRegistration + 1, StringComparison.Ordinal);
        var nextMemberRegistration = plugin.IndexOf("::dds::core::xtypes::Member(\"", typeRegistration + 1, StringComparison.Ordinal);
        var nextRegistration = nextFactoryRegistration < 0
            ? nextMemberRegistration
            : nextMemberRegistration < 0
                ? nextFactoryRegistration
                : Math.Min(nextFactoryRegistration, nextMemberRegistration);
        var registrationEnd = plugin.IndexOf(");", typeRegistration, StringComparison.Ordinal);
        registrationEnd.ShouldBeLessThan(contextEnd);
        (nextRegistration < 0 || registrationEnd < nextRegistration).ShouldBeTrue();
        return plugin.Substring(typeRegistration, registrationEnd - typeRegistration + 2);
    }

}
