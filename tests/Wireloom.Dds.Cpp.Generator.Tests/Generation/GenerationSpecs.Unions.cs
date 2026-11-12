namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Protects IDL-derived generation and fail-closed behavior at the command boundary.</summary>
public sealed partial class GenerationSpecs
{
    /// <summary>Emits the C++11 union value API and preserves the documented RTI default-label metadata.</summary>
    [Fact]
    public void EmitsPrimitiveDiscriminatedUnion()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-unions.idl"));

        // Act
        var result = workspace.Generate(source, "07-unions");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-unions.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-unions.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-unionsPlugin.cxx"));
        header.ShouldContain("class NDDSUSERDllExport Choice");
        header.ShouldContain("::std::int32_t& _d();");
        header.ShouldContain("const ::std::int32_t& _d() const;");
        header.ShouldContain("static ::std::int32_t default_discriminator();");
        header.ShouldContain("::std::int32_t& number();");
        header.ShouldContain("const ::std::int32_t& number() const;");
        header.ShouldContain("bool& flag();");
        header.ShouldContain("const bool& flag() const;");
        header.ShouldContain("void flag(bool value, ::std::int32_t discriminator);");
        header.ShouldContain("void flag(bool value);");
        header.ShouldContain("bool operator==(const Choice& other_) const;");
        header.ShouldContain("bool operator!=(const Choice& other_) const;");
        header.ShouldContain("void swap(Choice& other_) noexcept;");
        header.ShouldContain("#include <omg/types/string_view.hpp>");
        header.ShouldContain("void number(::std::int32_t value);");
        header.ShouldContain("void text(const ::omg::types::string_view& value);");
        header.ShouldContain("void text(::omg::types::string_view&& value);");
        header.ShouldContain("::std::string text_ = {};");
        header.ShouldContain("struct topic_type_name<::CorpusUnions::Choice>");
        header.ShouldContain("struct topic_type_support<::CorpusUnions::Choice>");
        header.ShouldContain("REGISTER_TOPIC_TYPE(::CorpusUnions::Choice)");
        header.ShouldContain("static void register_type(");
        header.ShouldContain("static std::vector<char>& to_cdr_buffer(");
        header.ShouldContain("static void reset_sample(::CorpusUnions::Choice& sample);");
        header.ShouldContain("static void allocate_sample(::CorpusUnions::Choice& sample,");
        values.ShouldContain("Choice::Choice() = default;");
        values.ShouldContain("throw ::dds::core::PreconditionNotMetError(\"text branch is inactive\");");
        values.ShouldContain("text_ = value;");
        values.ShouldContain("::std::int32_t Choice::default_discriminator()");
        values.ShouldContain("return 0;");
        values.ShouldContain("discriminator_ = discriminator;");
        values.ShouldContain("if (!((discriminator_ != 0 && discriminator_ != 1)))");
        values.ShouldContain("if (!((discriminator != 0 && discriminator != 1)))");
        values.ShouldContain("if (discriminator_ != other_.discriminator_) return false;");
        values.ShouldContain("return !(*this == other_);");
        values.ShouldContain("left.swap(right);");
        values.ShouldNotContain("number_ = 0;");
        values.ShouldContain("if ((value.discriminator_ != 0 && value.discriminator_ != 1))");
        values.ShouldContain("if (emitted_branch)\n            output << \", \";");
        values.ShouldContain("output << \"flag: \" << value.flag_;");
        values.ShouldContain("emitted_branch = true;");
        header.ShouldContain("using type = dds::core::xtypes::UnionType;");
        plugin.ShouldContain("\"number\", ::dds::core::xtypes::primitive_type<::std::int32_t>()");
        plugin.ShouldContain("\"text\", ::dds::core::xtypes::StringType(255)");
        plugin.ShouldContain("\"flag\", ::dds::core::xtypes::primitive_type<bool>()");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldContain("#include <dds/core/xtypes/UnionType.hpp>");
        plugin.ShouldContain("#include <ndds/dds_c/dds_c_typecode_impl.h>");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
        plugin.ShouldContain("// Stage 1: create all struct types without inheritance.");
        plugin.ShouldContain("// Create union shells in the same stage to support cross-type metadata references.");
        plugin.ShouldContain("wireloom_Choice_union_context_instance().create_type();");
        plugin.ShouldContain("wireloom_Choice_union_context_instance().add_members();");
        plugin.ShouldContain("wireloom_Choice_union_context_instance().create_type_plugin();");
        var shellStage = plugin.IndexOf("wireloom_Choice_union_context_instance().create_type();", StringComparison.Ordinal);
        var memberStage = plugin.IndexOf("wireloom_Choice_union_context_instance().add_members();", StringComparison.Ordinal);
        var pluginStage = plugin.IndexOf("wireloom_Choice_union_context_instance().create_type_plugin();", StringComparison.Ordinal);
        shellStage.ShouldBeLessThan(memberStage);
        memberStage.ShouldBeLessThan(pluginStage);
        plugin.ShouldContain("topic_type_support<::CorpusUnions::Choice>::register_type(");
        plugin.ShouldContain("topic_type_support<::CorpusUnions::Choice>::to_cdr_buffer(");
        plugin.ShouldContain("topic_type_support<::CorpusUnions::Choice>::from_cdr_buffer(");
        plugin.ShouldContain("topic_type_support<::CorpusUnions::Choice>::reset_sample(");
        plugin.ShouldContain("topic_type_support<::CorpusUnions::Choice>::allocate_sample(");
        plugin.ShouldNotContain("UnionMember(\"flag\", ::dds::core::xtypes::primitive_type<bool>(), 2)");
    }

    /// <summary>Emits enum-discriminated unions with aggregate branches and no default branch.</summary>
    [Fact]
    public void EmitsEnumDiscriminatedUnionWithAggregateBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-enum.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-enum");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-enum.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-enum.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-enumPlugin.cxx"));
        header.ShouldContain("void _d(Kind value);");
        header.ShouldContain("Kind discriminator_ = Kind::Number;");
        header.ShouldContain("const Payload& payload() const;");
        header.ShouldContain("Payload& payload();");
        header.ShouldContain("void payload(const Payload& value);");
        values.ShouldContain("discriminator_ == Kind::Number");
        values.ShouldContain("discriminator_ = Kind::PayloadValue;");
        values.ShouldContain("bool emitted_branch = false;");
        values.ShouldContain("if ((value.discriminator_ == Kind::PayloadValue))");
        values.ShouldContain("if (emitted_branch)\n            output << \", \";");
        values.ShouldContain("emitted_branch = true;");
        values.ShouldContain("return true;");
        plugin.ShouldContain("dynamic_type<::CorpusEnumUnion::Kind>::get()");
        plugin.ShouldContain("UnionMember(\n                \"payload\", *::CorpusEnumUnion::wireloom_Payload_type_context_instance().type,");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
    }

    /// <summary>Emits qualified aggregate and bounded-sequence union branches without a default label.</summary>
    [Fact]
    public void EmitsScopedAggregateAndBoundedSequenceUnionBranches()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-scoped.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-scoped");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-scoped.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-scoped.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-scopedPlugin.cxx"));
        header.ShouldContain("const Payload& qualifiedPayload() const;");
        header.ShouldContain("Payload& qualifiedPayload();");
        header.ShouldContain("void qualifiedPayload(const Payload& value);");
        header.ShouldContain("const ::omg::types::bounded_sequence< ::std::int32_t, 4 >& values() const;");
        header.ShouldContain("void values(const ::omg::types::bounded_sequence< ::std::int32_t, 4 >& value);");
        values.ShouldContain("if ((value.discriminator_ == 11))");
        values.ShouldContain("if (emitted_branch)\n            output << \", \";");
        plugin.ShouldContain("\"qualifiedPayload\", *::CorpusScopedUnion::wireloom_Payload_type_context_instance().type,");
        plugin.ShouldContain("                10));");
        plugin.ShouldContain("\"values\", *factory.create_sequence(");
        plugin.ShouldContain("4),\n                11));");
        plugin.ShouldContain("                11));");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldContain("wireloom_Payload_type_context_instance().create_type();");
        plugin.ShouldContain("wireloom_Payload_type_context_instance().add_members();");
        plugin.ShouldContain("wireloom_Payload_type_context_instance().create_type_plugin();");
        plugin.ShouldContain("dynamic_type<::CorpusScopedUnion::Choice>::get()\n{\n    wireloom_initialize_dynamic_types();");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
    }

    /// <summary>Preserves all explicit labels in branch access and DynamicType metadata.</summary>
    [Fact]
    public void EmitsMultipleLabelsForOneUnionBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-multilabel.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-multilabel");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-multilabel.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-multilabel.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-multilabelPlugin.cxx"));
        header.ShouldContain("const ::std::int32_t& _d() const;");
        header.ShouldContain("void number(::std::int32_t value);");
        header.ShouldContain("void number(::std::int32_t value, ::std::int32_t discriminator);");
        header.ShouldContain("/// @param discriminator One of: 1, 5.");
        header.ShouldContain("/// @throws dds::core::PreconditionNotMetError when the discriminator is not a branch label.");
        values.ShouldContain("discriminator_ == 1 || discriminator_ == 5");
        values.ShouldContain("discriminator_ = 1;");
        values.ShouldContain("void Choice::number(::std::int32_t value, ::std::int32_t discriminator)");
        values.ShouldContain("throw ::dds::core::PreconditionNotMetError(\n            \"invalid discriminator for number\");");
        values.ShouldContain("discriminator_ = discriminator;");
        values.ShouldContain("discriminator_ == 2");
        values.ShouldContain("bool emitted_branch = false;");
        values.ShouldContain("if ((value.discriminator_ == 2))");
        values.ShouldContain("if (emitted_branch)\n            output << \", \";");
        plugin.ShouldContain("\"number\", ::dds::core::xtypes::primitive_type<::std::int32_t>()");
        plugin.ShouldContain("UnionMember::LabelSeq{1, 5}");
        plugin.ShouldContain("\"text\", ::dds::core::xtypes::StringType(255)");
        plugin.ShouldContain("                2));");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
    }

    /// <summary>Uses evaluated constant-expression values for union branch predicates and metadata.</summary>
    [Fact]
    public void EmitsCanonicalValuesForUnionLabelExpressions()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-union-label-expressions.idl"));

        // Act
        var result = workspace.Generate(source, "14-union-label-expressions");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order(StringComparer.Ordinal)
            .ShouldBe(new[]
            {
                "14-union-label-expressions.cxx",
                "14-union-label-expressions.hpp",
                "14-union-label-expressionsPlugin.cxx",
                "14-union-label-expressionsPlugin.hpp"
            }.Order(StringComparer.Ordinal));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-label-expressions.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-label-expressionsPlugin.cxx"));
        values.ShouldContain("discriminator_ == 42");
        values.ShouldContain("discriminator_ = 42;");
        values.ShouldContain("discriminator_ == 4");
        values.ShouldContain("discriminator_ = 4;");
        values.ShouldContain("discriminator_ == 16");
        values.ShouldContain("discriminator_ = 16;");
        plugin.ShouldContain("\"symbolic\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                42));");
        plugin.ShouldContain("\"arithmetic\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                4));");
        plugin.ShouldContain("\"hexadecimal\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                16));");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
    }

    /// <summary>Emits enum constants after their type and retains symbolic union labels.</summary>
    [Fact]
    public void EmitsScopedEnumConstantAndUnionLabels()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-union-scoped-enum.idl"));

        // Act
        var result = workspace.Generate(source, "14-union-scoped-enum");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order(StringComparer.Ordinal)
            .ShouldBe(new[]
            {
                "14-union-scoped-enum.cxx",
                "14-union-scoped-enum.hpp",
                "14-union-scoped-enumPlugin.cxx",
                "14-union-scoped-enumPlugin.hpp"
            }.Order(StringComparer.Ordinal));
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-scoped-enum.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-scoped-enum.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-scoped-enumPlugin.cxx"));
        header.IndexOf("enum class Choice", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf("constexpr Choice Selected = Choice::SECOND;", StringComparison.Ordinal));
        values.ShouldContain("discriminator_ == Choice::SECOND");
        values.ShouldContain("discriminator_ = Choice::SECOND;");
        values.ShouldContain("discriminator_ == Choice::FIRST");
        values.ShouldContain("discriminator_ = Choice::FIRST;");
        plugin.ShouldContain("\"selected\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                1));");
        plugin.ShouldContain("\"first\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                0));");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
    }

    /// <summary>Emits a fixed-array union branch with its declared two-dimensional shape.</summary>
    [Fact]
    public void EmitsFixedArrayUnionBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-union-array-branch.idl"));

        // Act
        var result = workspace.Generate(source, "14-union-array-branch");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order(StringComparer.Ordinal)
            .ShouldBe(new[]
            {
                "14-union-array-branch.cxx",
                "14-union-array-branch.hpp",
                "14-union-array-branchPlugin.cxx",
                "14-union-array-branchPlugin.hpp"
            }.Order(StringComparer.Ordinal));
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-array-branch.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-array-branch.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-union-array-branchPlugin.cxx"));
        header.ShouldContain("::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 >& values();");
        header.ShouldContain("::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 > values_ = {};");
        values.ShouldContain("::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 >& ArrayBranch::values()");
        values.ShouldContain("discriminator_ == 0");
        values.ShouldContain("ArrayBranch::ArrayBranch() = default;");
        values.ShouldContain("discriminator_ == 1");
        values.ShouldContain("discriminator_ = 1;");
        plugin.ShouldContain("*factory.create_array(");
        plugin.ShouldContain("{2, 3}");
        plugin.ShouldContain("\"values\", *factory.create_array(");
        plugin.ShouldContain("\"scalar\", ::dds::core::xtypes::primitive_type<::std::int32_t>(),\n                1));");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldContain("dynamic_type<::ArrayBranch>::get()\n{\n    wireloom_initialize_dynamic_types();");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
    }

    /// <summary>Evaluates constant expressions used as narrow and wide string bounds.</summary>
    [Fact]
    public void EmitsEvaluatedStringExpressionBounds()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-string-expression-bound.idl"));

        // Act
        var result = workspace.Generate(source, "14-string-expression-bound");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order(StringComparer.Ordinal)
            .ShouldBe(new[]
            {
                "14-string-expression-bound.cxx",
                "14-string-expression-bound.hpp",
                "14-string-expression-boundPlugin.cxx",
                "14-string-expression-boundPlugin.hpp"
            }.Order(StringComparer.Ordinal));
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-string-expression-bound.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-string-expression-boundPlugin.cxx"));
        header.ShouldContain("::std::string text = {};");
        header.ShouldContain("::std::wstring wideText = {};");
        header.ShouldContain("/// @details String metadata bound is 5.");
        header.ShouldContain("/// @details String metadata bound is 6.");
        plugin.ShouldContain("factory.add_member(*type, \"text\",");
        plugin.ShouldContain("::dds::core::xtypes::StringType(5)");
        plugin.ShouldContain("factory.add_member(*type, \"wideText\",");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(6)");
        foreach (var file in Directory.GetFiles(workspace.OutputPath))
        {
            var content = File.ReadAllText(file);
            content.ShouldNotContain("/current/");
            content.Split('\n').Where(line => line.Contains("community.rti.com/", StringComparison.Ordinal))
                .ShouldAllBe(line => line.Contains("/7.7.0/doc/", StringComparison.Ordinal));
            var lines = File.ReadAllLines(file);
            content.ShouldNotContain("\r");
            lines.Where(line => !IsLongDoxygenSeeUrl(line)).Max(line => line.Length).ShouldBeLessThanOrEqualTo(120);
            lines.Where(IsLongDoxygenSeeUrl).ShouldAllBe(line => line.Length <= 320);
        }
    }

    /// <summary>Uses typed defaults from the nearest typedef and preserves declared aliases.</summary>
    [Fact]
    public void EmitsTypedTypedefAndMemberDefaultsThroughAliasChains()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-typed-defaults.idl"));

        // Act
        var result = workspace.Generate(source, "14-typed-defaults");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "14-typed-defaults.hpp"));
        header.ShouldContain("using Seven = ::std::int32_t;");
        header.ShouldContain("using TextAlias = ::std::string;");
        header.ShouldContain("using Ratio = double;");
        header.ShouldContain("Seven inherited = 7;");
        header.ShouldContain("Seven overridden = 9;");
        header.ShouldContain("TextAlias text = \"alias\";");
        header.ShouldContain("Ratio ratio = 1.5;");
        header.ShouldContain("float single = 1.25F;");

        var aliasChain = workspace.Generate(
            "@default(value=7) typedef long Base; @default(value=8) typedef Base Middle; typedef Middle Leaf; " +
            "struct AliasDefaults { Leaf inherited; @default(value=11) Leaf overridden; };",
            "alias-default-chain");
        aliasChain.ExitCode.ShouldBe(0, aliasChain.Diagnostics);
        var chainHeader = File.ReadAllText(Path.Combine(workspace.OutputPath, "alias-default-chain.hpp"));
        chainHeader.ShouldContain("using Middle =");
        chainHeader.ShouldContain("using Leaf =");
        chainHeader.ShouldContain("Leaf inherited = 8;");
        chainHeader.ShouldContain("Leaf overridden = 11;");
    }

    /// <summary>Emits wide-string typedef defaults.</summary>
    [Fact]
    public void EmitsWideStringTypedefDefault()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "@default(value=L\"wide\") typedef wstring<8> WideText; struct Sample { WideText text; };";

        // Act
        var result = workspace.Generate(source, "wide-typedef-default");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "wide-typedef-default.hpp"));
        header.ShouldContain("WideText text = L\"wide\";");
    }

    /// <summary>Rejects unsupported union discriminators and default-branch combinations.</summary>
    [Theory]
    [InlineData("""module CorpusUnions { union Choice switch(uint64) { case 0: long number; default: boolean flag; }; };""", "Unsupported union discriminator type: uint64")]
    [InlineData("""module CorpusUnions { union Inner switch(boolean) { case TRUE: long number; case FALSE: long other; }; union Choice switch(boolean) { case TRUE: Inner nested; case FALSE: string text; }; };""", "does not support union branch 'nested'")]
    public void RejectsUnsupportedUnionShapes(string idl, string expectedDiagnostic)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(idl, "unsupported-union");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain(expectedDiagnostic);
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Uses a signed 16-bit discriminator and an unused representable default-branch label.</summary>
    [Fact]
    public void EmitsShortDiscriminatedUnionWithDefaultBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-short.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-short");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-short.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-short.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-shortPlugin.cxx"));
        header.ShouldContain("const ::std::int16_t& _d() const;");
        header.ShouldContain("void negative(::std::int32_t value);");
        header.ShouldContain("::std::int16_t discriminator_ = 0;");
        values.ShouldContain("Choice::Choice() = default;");
        values.ShouldContain("discriminator_ != -1 && discriminator_ != 2");
        values.ShouldContain("    other_ = value;\n}");
        plugin.ShouldContain("primitive_type<::std::int16_t>()");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldNotContain("UnionMember(\"other\", ::dds::core::xtypes::primitive_type<bool>(), 0)");
    }

    /// <summary>Emits exact-width primitive discriminators, full-width labels, and separate default metadata.</summary>
    [Theory]
    [InlineData("07-union-octet", "::std::uint8_t", "::rti::core::xtypes::octet_tag_t", "0", "255", "1")]
    [InlineData("07-union-ushort", "::std::uint16_t", "::std::uint16_t", "0", "65535", "1")]
    [InlineData("07-union-ulong", "::std::uint32_t", "::std::uint32_t", "0", "4294967295", "1")]
    [InlineData("07-union-int8", "::std::int8_t", "::std::int8_t", "-128", "127", "0")]
    [InlineData("07-union-uint8", "::std::uint8_t", "::std::uint8_t", "0", "255", "1")]
    [InlineData("07-union-int16", "::std::int16_t", "::std::int16_t", "-32768", "32767", "0")]
    [InlineData("07-union-uint16", "::std::uint16_t", "::std::uint16_t", "0", "65535", "1")]
    [InlineData("07-union-int32", "::std::int32_t", "::std::int32_t", "-2147483648", "2147483647", "0")]
    [InlineData("07-union-uint32", "::std::uint32_t", "::std::uint32_t", "0", "4294967295", "1")]
    [InlineData("07-union-char-boundaries", "char", "char", "0", "126", "1")]
    public void EmitsExactWidthPrimitiveUnionDiscriminator(
        string name,
        string storageType,
        string metadataType,
        string minimumLabel,
        string maximumLabel,
        string fallbackValue)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", name + ".idl"));

        // Act
        var result = workspace.Generate(source, name);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        Directory.GetFiles(workspace.OutputPath).Select(Path.GetFileName).Order(StringComparer.Ordinal)
            .ShouldBe(new[] { name + ".cxx", name + ".hpp", name + "Plugin.cxx", name + "Plugin.hpp" }.Order(StringComparer.Ordinal));
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, name + ".hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, name + ".cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, name + "Plugin.cxx"));
        header.ShouldContain("const " + storageType + "& _d() const;");
        header.ShouldContain("void _d(" + storageType + " value);");
        header.ShouldContain(storageType + " discriminator_ = 0;");
        values.ShouldContain("discriminator_ == " + minimumLabel);
        values.ShouldContain("discriminator_ == " + maximumLabel);
        values.ShouldContain("fallback_ = value;\n    discriminator_ = discriminator;");
        plugin.ShouldContain("sample_value._d(" + fallbackValue + ");");
        plugin.ShouldContain("primitive_type<" + metadataType + ">()");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldContain(minimumLabel);
        plugin.ShouldContain(maximumLabel);
    }

    /// <summary>Emits ASCII character labels with a plain-char discriminator and portable default value.</summary>
    [Fact]
    public void EmitsAsciiCharacterDiscriminatedUnionWithDefaultBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-char.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-char");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-charPlugin.cxx"));
        header.ShouldContain("const char& _d() const;");
        header.ShouldContain("void _d(char value);");
        header.ShouldContain("char discriminator_ = 0;");
        header.ShouldContain("void letter(::std::int32_t value);");
        values.ShouldContain("discriminator_ == 97");
        values.ShouldContain("discriminator_ == 122");
        values.ShouldContain("discriminator_ != 97 && discriminator_ != 122");
        values.ShouldContain("    other_ = value;\n}");
        plugin.ShouldContain("primitive_type<char>()");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldNotContain("UnionMember(\"other\", ::dds::core::xtypes::primitive_type<bool>(), 0)");
    }

    /// <summary>Emits boolean discriminator labels and exhausts both values without default metadata.</summary>
    [Fact]
    public void EmitsBooleanDiscriminatedUnionWithExhaustiveLabels()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-boolean.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-boolean");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-boolean.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-boolean.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-booleanPlugin.cxx"));
        var normalizedPlugin = System.Text.RegularExpressions.Regex.Replace(plugin, @"\s+", " ");
        header.ShouldContain("const bool& _d() const;");
        header.ShouldContain("void _d(bool value);");
        header.ShouldContain("bool discriminator_ = false;");
        values.ShouldContain("discriminator_ == true");
        values.ShouldContain("discriminator_ == false");
        values.ShouldContain("    enabled_ = value;\n    discriminator_ = true;");
        values.ShouldContain("    disabled_ = value;\n    discriminator_ = false;");
        plugin.ShouldContain("primitive_type<bool>()");
        normalizedPlugin.ShouldContain("UnionMember( \"enabled\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), 1)");
        normalizedPlugin.ShouldContain("UnionMember( \"disabled\", ::dds::core::xtypes::StringType(255), 0)");
        plugin.ShouldNotContain("UnionMember::DEFAULT_LABEL");
    }

    /// <summary>Emits union aliases as using chains with one canonical staged metadata context.</summary>
    [Fact]
    public void EmitsCanonicalUnionAliasChainAndByValueHolderInStagedOrder()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-aliases.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-aliases");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-aliases.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-aliasesPlugin.cxx"));
        header.ShouldContain("using ChoiceAlias = Choice;");
        header.ShouldContain("using ChoiceAlias2 = ChoiceAlias;");
        header.ShouldContain("ChoiceAlias2 value = {};");
        header.ShouldContain("const ChoiceAlias2& value");
        header.IndexOf("class NDDSUSERDllExport Choice", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf("struct NDDSUSERDllExport Holder", StringComparison.Ordinal));
        plugin.ShouldContain("*::CorpusUnionAliases::wireloom_Choice_union_type()");
        plugin.ShouldContain("::DDS_TypeCode* wireloom_alias_type_code___CorpusUnionAliases__ChoiceAlias();");
        plugin.ShouldContain("::DDS_TypeCode* wireloom_alias_type_code___CorpusUnionAliases__ChoiceAlias2();");
        plugin.ShouldContain("wireloom_Choice_union_native_type_code()");
        plugin.ShouldContain("native_type_code->_sampleAccessInfo = alias_type_code->_sampleAccessInfo;");
        plugin.ShouldContain("native_type_code->_typePlugin = alias_type_code->_typePlugin;");
        plugin.ShouldContain("wireloom_Choice_union_context_instance().create_type();");
        plugin.ShouldContain("wireloom_Choice_union_context_instance().add_members();");
        plugin.ShouldContain("wireloom_Choice_union_context_instance().create_type_plugin();");
        plugin.ShouldContain("wireloom_Holder_type_context_instance().add_members();");
        plugin.ShouldContain("wireloom_Holder_stl_string_create_plugin");
        plugin.ShouldContain("static ::RTIXCdrSampleAccessInfo* stl_sample_access_info()");
        plugin.ShouldNotContain("wireloom_ChoiceAlias_union_context");
        plugin.ShouldNotContain("wireloom_ChoiceAlias2_union_context");
        plugin.IndexOf("wireloom_Choice_union_type();", StringComparison.Ordinal).ShouldBeLessThan(
            plugin.IndexOf("struct wireloom_Holder_type_context", StringComparison.Ordinal));
        plugin.IndexOf("wireloom_Choice_union_context_instance().add_members();", StringComparison.Ordinal)
            .ShouldBeLessThan(plugin.IndexOf("wireloom_Holder_type_context_instance().add_members();", StringComparison.Ordinal));
    }

    /// <summary>Orders mixed struct and union metadata by transitive complete-type dependencies.</summary>
    [Fact]
    public void EmitsMixedStructUnionMetadataInDependencyOrder()
    {
        // Arrange
        const string idl = """
            module MixedOrder {
                union U1 switch(boolean) { case TRUE: S item; case FALSE: long number; };
                typedef U2 U2Alias;
                struct S { U2Alias inner; };
                union U2 switch(boolean) { case TRUE: long number; case FALSE: string text; };
            };
            """;
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(idl, "mixed-union-order");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "mixed-union-order.hpp"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "mixed-union-orderPlugin.cxx"));
        header.IndexOf("class NDDSUSERDllExport U2", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf("struct NDDSUSERDllExport S", StringComparison.Ordinal));
        header.IndexOf("struct NDDSUSERDllExport S", StringComparison.Ordinal).ShouldBeLessThan(
            header.IndexOf("class NDDSUSERDllExport U1", StringComparison.Ordinal));
        var u2Metadata = plugin.IndexOf("wireloom_U2_union_context_instance().add_members();", StringComparison.Ordinal);
        var structMetadata = plugin.IndexOf("wireloom_S_type_context_instance().add_members();", StringComparison.Ordinal);
        var u1Metadata = plugin.IndexOf("wireloom_U1_union_context_instance().add_members();", StringComparison.Ordinal);
        u2Metadata.ShouldBeLessThan(structMetadata);
        structMetadata.ShouldBeLessThan(u1Metadata);
        plugin.IndexOf("wireloom_U1_union_context_instance().create_type();", StringComparison.Ordinal)
            .ShouldBeLessThan(u2Metadata);
        plugin.IndexOf("wireloom_U2_union_context_instance().create_type();", StringComparison.Ordinal)
            .ShouldBeLessThan(u2Metadata);
        plugin.IndexOf("wireloom_S_type_context_instance().create_type();", StringComparison.Ordinal)
            .ShouldBeLessThan(u2Metadata);
    }

    /// <summary>Rejects complete by-value cycles that cross a struct and union boundary.</summary>
    [Fact]
    public void RejectsByValueStructUnionCycle()
    {
        // Arrange
        const string idl = """
            module Cycle {
                struct Holder { Choice value; };
                union Choice switch(boolean) { case TRUE: Holder holder; case FALSE: long number; };
            };
            """;
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(idl, "union-cycle");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("Recursive value-type member 'holder' creates a type cycle");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Rejects wide-character union discriminators to match the RTI 7.7 parser limitation.</summary>
    [Fact]
    public void RejectsWideCharacterDiscriminatedUnionLikeRti()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-wchar-label.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-wchar-label");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("07-union-wchar-label.idl(2,25): Error DDSGCPP001:");
        result.Diagnostics.ShouldContain("does not support 'wchar' union discriminators");
        result.Diagnostics.ShouldContain("RTI Connext DDS 7.7.0 rtiddsgen's parser rejection");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Preserves numeric char label 127 and the declared default sentinel in generated output.</summary>
    [Fact]
    public void EmitsNumericCharacterDiscriminatedUnionWithDefaultBranch()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-char-numeric.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-char-numeric");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char-numeric.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char-numeric.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char-numericPlugin.cxx"));
        var normalizedPlugin = System.Text.RegularExpressions.Regex.Replace(plugin, @"\s+", " ");
        header.ShouldContain("const char& _d() const;");
        header.ShouldContain("void _d(char value);");
        header.ShouldContain("char discriminator_ = 0;");
        values.ShouldContain("discriminator_ == 127");
        values.ShouldContain("discriminator_ != 127");
        values.ShouldContain("    fallback_ = value;\n}");
        plugin.ShouldContain("primitive_type<char>()");
        normalizedPlugin.ShouldContain("UnionMember( \"value\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), 127)");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        normalizedPlugin.ShouldNotContain("UnionMember( \"fallback\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), 0)");
        plugin.ShouldContain("dynamic_type<::CorpusNumericCharUnion::Choice>::get()\n{\n    wireloom_initialize_type_plugins();");
    }

    /// <summary>Preserves numeric char label 255 across signedness and Modern XTypes metadata.</summary>
    [Fact]
    public void EmitsNumeric255CharacterUnionWithModernMetadataAndUnsignedSafeExpressions()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-char-numeric-255.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-char-numeric-255");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char-numeric-255.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char-numeric-255.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-char-numeric-255Plugin.cxx"));
        var normalizedPlugin = System.Text.RegularExpressions.Regex.Replace(plugin, @"\s+", " ");
        header.ShouldContain("const char& _d() const;");
        header.ShouldContain("void _d(char value);");
        values.ShouldContain("static_cast<char>(-1)");
        values.ShouldContain("discriminator_ == static_cast<char>(-1)");
        values.ShouldContain("discriminator_ != static_cast<char>(-1)");
        normalizedPlugin.ShouldContain("UnionMember( \"value\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), -1)");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        plugin.ShouldContain("dynamic_type<::CorpusNumericChar255Union::Choice>::get()\n{\n    wireloom_initialize_dynamic_types();");
    }

    /// <summary>Preserves signed short boundary labels in generated C++ and union metadata.</summary>
    [Fact]
    public void EmitsShortDiscriminatedUnionWithBoundaryLabelsAndUnusedDefault()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-short-boundaries.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-short-boundaries");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-short-boundaries.hpp"));
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-short-boundaries.cxx"));
        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-short-boundariesPlugin.cxx"));
        var normalizedPlugin = System.Text.RegularExpressions.Regex.Replace(plugin, @"\s+", " ");
        header.ShouldContain("const ::std::int16_t& _d() const;");
        header.ShouldContain("void minimum(::std::int32_t value);");
        header.ShouldContain("void maximum(::std::int32_t value);");
        header.ShouldContain("::std::int16_t discriminator_ = 0;");
        values.ShouldContain("discriminator_ = -32768;");
        values.ShouldContain("discriminator_ = 32767;");
        values.ShouldContain("discriminator_ != -32768 && discriminator_ != 32767");
        values.ShouldContain("    fallback_ = value;\n}");
        plugin.ShouldContain("primitive_type<::std::int16_t>()");
        normalizedPlugin.ShouldContain("UnionMember( \"minimum\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), -32768)");
        normalizedPlugin.ShouldContain("UnionMember( \"maximum\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), 32767)");
        plugin.ShouldContain("UnionMember::DEFAULT_LABEL");
        normalizedPlugin.ShouldNotContain("UnionMember( \"fallback\", ::dds::core::xtypes::primitive_type<::std::int32_t>(), 0)");
    }

    /// <summary>Qualifies union branch fields when generated parameter names could shadow them.</summary>
    [Fact]
    public void EmitsUnionValueOperationsWithQualifiedBranchStorage()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "07-union-short.idl"));

        // Act
        var result = workspace.Generate(source, "07-union-short");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "07-union-short.cxx"));
        values.ShouldContain("return this->other_ == other_.other_;");
        values.ShouldContain("swap(this->other_, other_.other_);");
    }

    /// <summary>Rejects a short default branch when every non-negative discriminator value is occupied.</summary>
    [Fact]
    public void RejectsShortUnionWithNoAvailableDefaultDiscriminator()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var labels = string.Join(" ", Enumerable.Range(0, short.MaxValue + 1).Select(value => $"case {value}:"));
        var idl = $$"""
            module CorpusUnions {
                union Choice switch(short) {
                    {{labels}} long number;
                    default: boolean flag;
                };
            };
            """;

        // Act
        var result = workspace.Generate(idl, "exhausted-short-union");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("cannot select an unused non-negative discriminator for default branch");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Supports global-scope names without emitting empty C++ namespaces.</summary>
    [Fact]
    public void EmitsUnionAtGlobalScope()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string idl = "union Choice switch(long) { case 0: long number; case 1: string text; default: boolean flag; };";

        // Act
        var result = workspace.Generate(idl, "global-union");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "global-union.hpp"));
        header.ShouldContain("class NDDSUSERDllExport Choice");
        header.ShouldNotContain("namespace \n{");
    }

    /// <summary>Wraps long floating-point comparisons without changing their tolerance helper.</summary>
    [Theory]
    [InlineData("struct Reading { float extraordinarilyLongMeasurementNameForReadableGeneratedSourceAndReview; };")]
    [InlineData("struct Reading { long prefix; float extraordinarilyLongMeasurementNameForReadableGeneratedSourceAndReview; };")]
    public void WrapsLongFloatingPointEquality(string idl)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();

        // Act
        var result = workspace.Generate(idl, "long-float-equality");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        var values = File.ReadAllText(Path.Combine(workspace.OutputPath, "long-float-equality.cxx"));
        values.ShouldContain("wireloom_float_equal(\n            left.");
        values.ShouldContain("extraordinarilyLongMeasurementNameForReadableGeneratedSourceAndReview");
    }

}
