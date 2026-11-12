extern alias CppCompilerAssembly;

using BackendCompiler = CppCompilerAssembly::Wireloom.Dds.Cpp.Compiler;
using System.Security.Cryptography;
using Wireloom.Dds.Cpp.Emission;

namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Protects IDL-derived generation and fail-closed behavior at the command boundary.</summary>
public sealed partial class GenerationSpecs
{
    /// <summary>Preserves bounded and default-bound narrow strings in generated value and DynamicType code.</summary>
    [Theory]
    [InlineData("02-formatting", "Value", "text", 16)]
    [InlineData("02-multiple", "Second", "name", 255)]
    public void EmitsNarrowStringMembersWithResolvedBounds(
        string caseName,
        string typeName,
        string memberName,
        int bound)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", caseName + ".idl"));

        // Act
        var result = workspace.Generate(source, caseName);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, caseName + ".hpp"));
        header.ShouldContain("struct NDDSUSERDllExport " + typeName);
        header.ShouldContain("::std::string " + memberName + " = {};");
        if (caseName == "02-multiple")
        {
            header.ShouldContain("struct NDDSUSERDllExport First");
        }

        var pluginSource = File.ReadAllText(Path.Combine(workspace.OutputPath, caseName + "Plugin.cxx"));
        pluginSource.ShouldContain("::dds::core::xtypes::StringType(" + bound + ")");
        pluginSource.ShouldContain("#include <dds/core/xtypes/CollectionTypes.hpp>");
    }

    /// <summary>Emits bounded and source-unbounded narrow and wide strings with resolved metadata.</summary>
    [Fact]
    public void EmitsBoundedAndUnboundedWideStringMembers()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "04-strings.idl"));

        // Act
        var result = workspace.Generate(source, "04-strings");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "04-strings.hpp"));
        header.ShouldContain("::std::string unbounded = {};");
        header.ShouldContain("::std::string bounded = {};");
        header.ShouldContain("::std::wstring wideUnbounded = {};");
        header.ShouldContain("::std::wstring wideBounded = {};");
        header.ShouldContain("RTI's effective string maximum is 255 bytes");
        header.ShouldContain("excluding the");
        header.ShouldContain("trailing null.");
        header.ShouldContain("does not enforce this limit when assigned.");
        header.ShouldContain("RTI's effective wide-string maximum is 8 UTF-16 code units");
        header.ShouldContain("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Strings_and_Wide_Strings.htm#StringsAndWideStrings");
        header.ShouldNotContain("/current/");
        header.ShouldContain("sample.unbounded.assign(255, 'a');");
        header.ShouldContain("sample.wideBounded.assign(8, L'a');");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "04-stringsPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::StringType(255)");
        plugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(255)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(8)");
        plugin.ShouldContain("#include <dds/core/xtypes/CollectionTypes.hpp>");
        plugin.ShouldContain("wireloom_Sample_stl_string_create_plugin");
        plugin.ShouldContain("sequence_helper<");
        plugin.ShouldContain("::std::string>::sample_access_info();");
        plugin.ShouldContain("::std::wstring>::sample_access_info();");

        var valueSource = File.ReadAllText(Path.Combine(workspace.OutputPath, "04-strings.cxx"));
        valueSource.ShouldContain("wireloom_write_wide_string(output, value.wideUnbounded);");
        valueSource.ShouldContain("wireloom_write_wide_string(output, value.wideBounded);");
        valueSource.ShouldContain("wireloom_write_wide_escape(output, 'u', code_unit, 4);");
        valueSource.ShouldContain("wireloom_write_wide_escape(output, 'U', code_unit, 8);");
        valueSource.ShouldNotContain("output << value.wideUnbounded");
    }

    /// <summary>Retains narrow and wide string alias names and effective metadata bounds.</summary>
    [Theory]
    [InlineData("04-unbounded-string-aliases", "Text", "WideText")]
    [InlineData("04-string-alias-chains", "NarrowTextChain", "WideTextChain")]
    public void EmitsStringTypedefAliases(string caseName, string narrowAlias, string wideAlias)
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", caseName + ".idl"));
        source += caseName == "04-unbounded-string-aliases"
            ? """

              module CorpusUnboundedStrings {
                  struct StringAliasesSample {
                      Text narrow;
                      WideText wide;
                  };
              };
              """
            : """

              module CorpusStringAliasChains {
                  struct StringAliasChainsSample {
                      NarrowText narrow;
                      NarrowTextChain narrowChain;
                      BoundedNarrowText boundedNarrow;
                      BoundedNarrowTextChain boundedNarrowChain;
                      BoundedNarrowTextChain2 boundedNarrowChain2;
                      WideText wide;
                      WideTextChain wideChain;
                      BoundedWideText boundedWide;
                      BoundedWideTextChain boundedWideChain;
                      BoundedWideTextChain2 boundedWideChain2;
                  };
              };
              """;

        // Act
        var result = workspace.Generate(source, caseName);

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, caseName + ".hpp"));
        if (caseName == "04-unbounded-string-aliases")
        {
            header.ShouldContain($"using {narrowAlias} = ::std::string;");
            header.ShouldContain($"using {wideAlias} = ::std::wstring;");
        }
        else
        {
            header.ShouldContain("using NarrowText = ::std::string;");
            header.ShouldContain($"using {narrowAlias} = NarrowText;");
            header.ShouldContain("using BoundedNarrowText = ::std::string;");
            header.ShouldContain("using BoundedNarrowTextChain = BoundedNarrowText;");
            header.ShouldContain($"using {wideAlias} = WideText;");
            header.ShouldContain("using WideText = ::std::wstring;");
            header.ShouldContain("using BoundedWideText = ::std::wstring;");
            header.ShouldContain("using BoundedWideTextChain = BoundedWideText;");
            header.ShouldContain("using BoundedNarrowTextChain2 = BoundedNarrowTextChain;");
            header.ShouldContain("using BoundedWideTextChain2 = BoundedWideTextChain;");
        }

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, caseName + "Plugin.cxx"));
        plugin.ShouldContain("::rti::domain::register_type_plugin(");
        plugin.ShouldContain("sequence_helper<\n                ::std::string>::sample_access_info();");
        plugin.ShouldContain("sequence_helper<\n                ::std::wstring>::sample_access_info();");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this string-bearing aggregate shape.");
    }

    /// <summary>Keeps nested string typedef aggregates outside the qualified interpreter bridge.</summary>
    [Fact]
    public void KeepsNestedStringTypedefAggregateFailClosed()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = """
            module NestedStringAlias {
                typedef string Text;
                struct Child {
                    Text value;
                };
                struct Sample {
                    Child child;
                };
            };
            """;

        // Act
        var result = workspace.Generate(source, "nested-string-alias");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var plugin = File.ReadAllText(Path.Combine(
            workspace.OutputPath,
            "nested-string-aliasPlugin.cxx"));
        plugin.ShouldContain("Wireloom C++ does not have a qualified interpreter bridge for this string-bearing aggregate shape.");
    }

    /// <summary>Preserves wstring typedef chains and accepts only the supported resolve-name setting.</summary>
    [Fact]
    public void EmitsResolveNameWideStringAliasChain()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(
            AppContext.BaseDirectory,
            "Corpus",
            "10-resolve-name-wstring-aliases.idl"));

        // Act
        var result = workspace.Generate(source, "10-resolve-name-wstring-aliases");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(
            workspace.OutputPath,
            "10-resolve-name-wstring-aliases.hpp"));
        header.ShouldContain("using BoundedWideText = ::std::wstring;");
        header.ShouldContain("/// @brief IDL typedef CorpusResolveNameWstringAliases.BoundedWideText.");
        header.ShouldContain("effective wide string metadata bound is 8.");
        header.ShouldContain("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1xtypes_1_1AliasType.html");
        header.ShouldContain("using BoundedWideTextAlias = BoundedWideText;");
        header.ShouldContain("BoundedWideText firstLevel = {};");
        header.ShouldContain("BoundedWideTextAlias secondLevel = {};");
        header.ShouldContain("dynamic_type<::CorpusResolveNameWstringAliases::BoundedWideText_AliasTag_t>");
        header.ShouldContain("dynamic_type<::CorpusResolveNameWstringAliases::BoundedWideTextAlias_AliasTag_t>");

        var plugin = File.ReadAllText(Path.Combine(
            workspace.OutputPath,
            "10-resolve-name-wstring-aliasesPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::WStringType(8)");
        plugin.ShouldContain("factory.add_member(*type, \"firstLevel\",\n            ::dds::core::xtypes::AliasType(");
        plugin.ShouldContain("\"CorpusResolveNameWstringAliases::BoundedWideTextAlias\",");
        plugin.ShouldContain("::dds::core::xtypes::AliasType(\n                    \"CorpusResolveNameWstringAliases::BoundedWideText\",");
        plugin.ShouldContain("::rti::domain::register_type_plugin(");
        plugin.ShouldContain("sequence_helper<\n                ::std::wstring>::sample_access_info();");
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this string-bearing aggregate shape.");
    }

    /// <summary>Rejects unsupported resolve-name values before emitting files.</summary>
    [Fact]
    public void RejectsResolveNameTrue()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(
                AppContext.BaseDirectory,
                "Corpus",
                "10-resolve-name-wstring-aliases.idl"))
            .Replace("@resolve_name(false)", "@resolve_name(true)", StringComparison.Ordinal);

        // Act
        var result = workspace.Generate(source, "10-resolve-name-wstring-aliases");

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("supports only @resolve_name(false)");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Rejects repeated resolve-name annotations, even when each value is the RTI default.</summary>
    [Fact]
    public void RejectsDuplicateResolveNameAnnotations()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        const string source = "struct Sample { @resolve_name(false) @resolve_name(false) string<16> text; };";

        // Act
        var result = workspace.Generate(source);

        // Assert
        result.ExitCode.ShouldBe(1);
        result.Diagnostics.ShouldContain("does not support duplicate @resolve_name annotations");
        Directory.Exists(workspace.OutputPath).ShouldBeFalse();
    }

    /// <summary>Uses zero metadata bounds for omitted string alias bounds only in unbounded mode.</summary>
    [Fact]
    public void EmitsUnboundedStringAliasMetadataWhenEnabled()
    {
        // Arrange
        const string source = """
            module StringAliasModes {
                typedef string Narrow;
                typedef wstring Wide;
                typedef string<16> BoundedNarrow;
                typedef wstring<8> BoundedWide;
                typedef Narrow NarrowChain;
                typedef Wide WideChain;
                struct Sample {
                    NarrowChain narrow;
                    WideChain wide;
                    BoundedNarrow boundedNarrow;
                    BoundedWide boundedWide;
                };
            };
            """;
        var finiteInput = new IdlInput("contract.idl", source);
        var unboundedInput = new IdlInput("contract.idl", source, unboundedSupport: true);

        // Act
        var finiteFiles = GenerateBoundInput(finiteInput);
        var unboundedFiles = GenerateBoundInput(unboundedInput);

        // Assert
        finiteFiles.Single(file => file.FileName == "contractPlugin.cxx").Source
            .ShouldContain("::dds::core::xtypes::StringType(255)");
        finiteFiles.Single(file => file.FileName == "contractPlugin.cxx").Source
            .ShouldContain("::dds::core::xtypes::WStringType(255)");
        unboundedFiles.Single(file => file.FileName == "contractPlugin.cxx").Source
            .ShouldContain("::dds::core::xtypes::StringType(RTIXCdrLong_MAX)");
        unboundedFiles.Single(file => file.FileName == "contractPlugin.cxx").Source
            .ShouldContain("::dds::core::xtypes::WStringType(RTIXCdrLong_MAX)");
        var unboundedPlugin = unboundedFiles.Single(file => file.FileName == "contractPlugin.cxx").Source;
        unboundedPlugin.ShouldContain("::dds::core::xtypes::StringType(16)");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::WStringType(8)");
    }

    [Theory]
    [InlineData("enum Color { RED }; struct Sample { sequence<Color> colors; };")]
    [InlineData("struct Sample { sequence<long> values[2]; };")]
    public void RejectsUnsupportedUnboundedSequenceShapes(string source)
    {
        // Arrange
        var input = new IdlInput("unsupported-unbounded-struct-sequence.idl", source, unboundedSupport: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var bound = IdlAnalyzer.Analyze([input], MD5.HashData, CancellationToken.None, diagnostics);
        diagnostics.ShouldBeEmpty();
        var files = BackendCompiler.Generate(bound.Declarations, input, diagnostics);

        // Assert
        diagnostics.ShouldContain(diagnostic =>
            diagnostic.Severity == IdlDiagnosticSeverity.Error);
        files.ShouldBeEmpty();
    }

    /// <summary>Retains finite fallbacks by default and emits RTI unbounded forms only when enabled.</summary>
    [Fact]
    public void EmitsUnboundedSupportSequenceAndStringShapesOnlyWhenEnabled()
    {
        // Arrange
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-unbounded-support.idl"));
        var finiteInput = new IdlInput("14-unbounded-support.idl", source);
        var unboundedInput = new IdlInput("14-unbounded-support.idl", source, unboundedSupport: true);

        // Act
        var finiteFiles = GenerateBoundInput(finiteInput);
        var unboundedFiles = GenerateBoundInput(unboundedInput);

        // Assert
        var finiteHeader = finiteFiles.Single(file => file.FileName == "14-unbounded-support.hpp").Source;
        var finitePlugin = finiteFiles.Single(file => file.FileName == "14-unbounded-supportPlugin.cxx").Source;
        finiteHeader.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 100 > values = {};");
        finiteHeader.ShouldContain("using LongValues = ::omg::types::bounded_sequence< ::std::int32_t, 100 >;");
        finiteHeader.ShouldContain("::omg::types::bounded_sequence< ::std::string, 100 > labels = {};");
        finiteHeader.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 4 > boundedValues = {};");
        finitePlugin.ShouldContain("::dds::core::xtypes::StringType(255)");
        finitePlugin.ShouldContain("::dds::core::xtypes::WStringType(255)");
        finitePlugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        finitePlugin.ShouldContain(".create_sequence(");
        finitePlugin.ShouldContain("\n                100)");

        var unboundedHeader = unboundedFiles.Single(file => file.FileName == "14-unbounded-support.hpp").Source;
        var unboundedPlugin = unboundedFiles.Single(file => file.FileName == "14-unbounded-supportPlugin.cxx").Source;
        unboundedHeader.ShouldContain("::omg::types::sequence< ::std::int32_t > values = {};");
        unboundedHeader.ShouldContain("using LongValues = ::omg::types::sequence< ::std::int32_t >;");
        unboundedHeader.ShouldContain("::omg::types::sequence< ::std::string > labels = {};");
        unboundedHeader.ShouldContain("String metadata bound is RTIXCdrLong_MAX (unbounded).");
        unboundedHeader.ShouldContain("Effective sequence metadata bound is RTIXCdrLong_MAX (unbounded).");
        unboundedHeader.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 4 > boundedValues = {};");
        unboundedHeader.ShouldContain("::std::string boundedText = {};");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::StringType(RTIXCdrLong_MAX)");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::WStringType(RTIXCdrLong_MAX)");
        unboundedPlugin.ShouldContain("RTIXCdrLong_MAX");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        unboundedPlugin.ShouldContain("*factory.create_sequence(");
    }

    /// <summary>Uses RTI's interpreted CDR path for direct sequences of unbounded narrow strings.</summary>
    [Fact]
    public void SupportsUnboundedNarrowStringSequenceBridge()
    {
        // Arrange
        const string source = """
            module UnboundedNarrowStringSequence {
                struct Sample { sequence<string> values; };
            };
            """;
        var input = new IdlInput("unbounded-narrow-string-sequence.idl", source, unboundedSupport: true);

        // Act
        var files = GenerateBoundInput(input);

        // Assert
        var plugin = files.Single(file => file.FileName == "unbounded-narrow-string-sequencePlugin.cxx").Source;
        plugin.ShouldNotContain("Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.");
        plugin.ShouldContain("get_cdr_serialization_programs<");
    }

    /// <summary>Emits C174's nested optional narrow string only in the RTI unbounded-support profile.</summary>
    [Fact]
    public void EmitsUnboundedReceiveLifecycleOptionalAggregateMetadataWhenEnabled()
    {
        // Arrange
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "14-unbounded-receive-lifecycle.idl"));
        var finiteInput = new IdlInput("14-unbounded-receive-lifecycle.idl", source);
        var unboundedInput = new IdlInput("14-unbounded-receive-lifecycle.idl", source, unboundedSupport: true);

        // Act
        var finiteDiagnostics = new List<IdlDiagnostic>();
        var finiteBound = IdlAnalyzer.Analyze([finiteInput], MD5.HashData, CancellationToken.None, finiteDiagnostics);
        var finiteFiles = BackendCompiler.Generate(finiteBound.Declarations, finiteInput, finiteDiagnostics);
        var unboundedFiles = GenerateBoundInput(unboundedInput);

        // Assert
        finiteDiagnostics.ShouldContain(diagnostic => diagnostic.Severity == IdlDiagnosticSeverity.Error);
        finiteFiles.ShouldBeEmpty();

        var header = unboundedFiles.Single(file => file.FileName == "14-unbounded-receive-lifecycle.hpp").Source;
        var plugin = unboundedFiles.Single(file => file.FileName == "14-unbounded-receive-lifecyclePlugin.cxx").Source;
        header.ShouldContain("::omg::types::sequence< ::std::int32_t > values = {};");
        header.ShouldContain("::std::string text = {};");
        header.ShouldContain("::std::wstring wideText = {};");
        header.ShouldContain("::omg::types::sequence< ::std::string > labels = {};");
        header.ShouldContain("using LongValues = ::omg::types::sequence< ::std::int32_t >;");
        header.ShouldContain("::omg::types::bounded_sequence< ::std::int32_t, 4 > boundedValues = {};");
        header.ShouldContain("::dds::core::optional<::std::string> note = {};");
        header.ShouldContain("::dds::core::optional<OptionalState> optionalState = {};");
        plugin.ShouldContain("::dds::core::xtypes::StringType(RTIXCdrLong_MAX)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(RTIXCdrLong_MAX)");
        plugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        plugin.ShouldContain("*factory.create_sequence(");
        plugin.ShouldContain("void wireloom_initialize_dynamic_types()");
        plugin.ShouldContain("void wireloom_initialize_type_plugins()");
        plugin.ShouldContain("wireloom_initialize_dynamic_types();");
        var accessorStart = plugin.IndexOf(
            "const dynamic_type<::CorpusUnboundedReceiveLifecycle::Sample>::type&\ndynamic_type<::CorpusUnboundedReceiveLifecycle::Sample>::get()",
            StringComparison.Ordinal);
        accessorStart.ShouldBeGreaterThanOrEqualTo(0);
        var accessorEnd = plugin.IndexOf("\n}", accessorStart, StringComparison.Ordinal) + 2;
        var accessor = plugin[accessorStart..accessorEnd];
        accessor.ShouldContain("wireloom_initialize_dynamic_types();");
        accessor.ShouldNotContain("wireloom_initialize_type_plugins();");
        plugin.ShouldContain("void topic_type_support<::CorpusUnboundedReceiveLifecycle::Sample>::register_type(");
        plugin.ShouldContain("wireloom_initialize_type_plugins();");
        var optionalStateStart = plugin.IndexOf(
            "struct wireloom_OptionalState_type_context",
            StringComparison.Ordinal);
        var sampleStart = plugin.IndexOf(
            "struct wireloom_Sample_type_context",
            optionalStateStart,
            StringComparison.Ordinal);
        plugin[optionalStateStart..sampleStart]
            .ShouldContain("native_type_code->_sampleAccessInfo = stl_sample_access_info();");
        plugin.ShouldContain("member_8_type_code->_sampleAccessInfo =");
    }

    /// <summary>Rejects optional wide or default-mode unbounded narrow strings at the optional-shape boundary.</summary>
    [Theory]
    [InlineData("string", false)]
    [InlineData("wstring", true)]
    public void RejectsOutOfProfileOptionalUnboundedStrings(string stringType, bool unboundedSupport)
    {
        // Arrange
        var input = new IdlInput(
            "optional-unbounded-string.idl",
            $"module CorpusOptionalUnboundedString {{ struct Sample {{ @optional {stringType} note; }}; }};",
            unboundedSupport: unboundedSupport);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var bound = IdlAnalyzer.Analyze([input], MD5.HashData, CancellationToken.None, diagnostics);
        diagnostics.ShouldBeEmpty();
        var files = BackendCompiler.Generate(bound.Declarations, input, diagnostics);

        // Assert
        diagnostics.ShouldContain(diagnostic =>
            diagnostic.Severity == IdlDiagnosticSeverity.Error
            && diagnostic.Message.Contains("optional member 'note'", StringComparison.Ordinal));
        files.ShouldBeEmpty();
    }

    /// <summary>Accepts an optional unbounded narrow string as a type shape only in unbounded-support mode.</summary>
    [Fact]
    public void EmitsOptionalUnboundedNarrowStringWhenEnabled()
    {
        // Arrange
        const string source = "module CorpusOptionalUnboundedString { struct Sample { @optional string note; }; };";
        var input = new IdlInput("optional-unbounded-string.idl", source, unboundedSupport: true);

        // Act
        var files = GenerateBoundInput(input);

        // Assert
        files.Single(file => file.FileName == "optional-unbounded-string.hpp").Source
            .ShouldContain("::dds::core::optional<::std::string> note = {};");
        var plugin = files.Single(file => file.FileName == "optional-unbounded-stringPlugin.cxx").Source;
        plugin.ShouldContain("::dds::core::xtypes::StringType(RTIXCdrLong_MAX)");
        plugin.ShouldNotContain("void wireloom_initialize_dynamic_types()");
    }

    /// <summary>Retains unbounded sequence typedef aliases and element alias metadata only when enabled.</summary>
    [Fact]
    public void EmitsUnboundedSequenceTypedefAliasesOnlyWhenEnabled()
    {
        // Arrange
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "05-unbounded-sequence-aliases.idl"));
        source += """

            module CorpusUnboundedSequences {
                struct SequenceAliasesSample {
                    NumbersChain numbers;
                    States states;
                    Children children;
                    Scalars scalars;
                    Texts texts;
                    WideTexts wideTexts;
                    BoundedTexts boundedTexts;
                    BoundedWideTexts boundedWideTexts;
                };
            };
            """;
        var finiteInput = new IdlInput("05-unbounded-sequence-aliases.idl", source);
        var unboundedInput = new IdlInput("05-unbounded-sequence-aliases.idl", source, unboundedSupport: true);

        // Act
        var finiteFiles = GenerateBoundInput(finiteInput);
        var unboundedFiles = GenerateBoundInput(unboundedInput);

        // Assert
        var finiteHeader = finiteFiles.Single(file => file.FileName == "05-unbounded-sequence-aliases.hpp").Source;
        var finitePlugin = finiteFiles.Single(file => file.FileName == "05-unbounded-sequence-aliasesPlugin.cxx").Source;
        finiteHeader.ShouldContain("using Numbers = ::omg::types::bounded_sequence< ::std::int32_t, 100 >;");
        finiteHeader.ShouldContain("using NumbersChain = Numbers;");
        finitePlugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusUnboundedSequences::Scalar\"");
        finitePlugin.ShouldContain("::dds::core::xtypes::StringType(255)");
        finitePlugin.ShouldContain("::dds::core::xtypes::WStringType(255)");

        var unboundedHeader = unboundedFiles.Single(file => file.FileName == "05-unbounded-sequence-aliases.hpp").Source;
        var unboundedPlugin = unboundedFiles.Single(file => file.FileName == "05-unbounded-sequence-aliasesPlugin.cxx").Source;
        unboundedHeader.ShouldContain("using Numbers = ::omg::types::sequence< ::std::int32_t >;");
        unboundedHeader.ShouldContain("using States = ::omg::types::sequence< State >;");
        unboundedHeader.ShouldContain("using Children = ::omg::types::sequence< Child >;");
        unboundedHeader.ShouldContain("using Scalars = ::omg::types::sequence< Scalar >;");
        unboundedHeader.ShouldContain("using Texts = ::omg::types::sequence< ::std::string >;");
        unboundedHeader.ShouldContain("using WideTexts = ::omg::types::sequence< ::std::wstring >;");
        unboundedHeader.ShouldContain("using BoundedTexts = ::omg::types::sequence< ::std::string >;");
        unboundedHeader.ShouldContain("using BoundedWideTexts = ::omg::types::sequence< ::std::wstring >;");
        unboundedHeader.ShouldContain("using NumbersChain = Numbers;");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::StringType(RTIXCdrLong_MAX)");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::WStringType(RTIXCdrLong_MAX)");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::WStringType(4)");
        unboundedPlugin.ShouldContain("RTIXCdrLong_MAX");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::AliasType(\n                \"CorpusUnboundedSequences::Scalar\"");
        unboundedPlugin.ShouldContain("::dds::core::xtypes::AliasType(\n            \"CorpusUnboundedSequences::Numbers\"");
        unboundedPlugin.ShouldContain("::rti::topic::interpreter::initialize_sequence_typecode<");
    }

    /// <summary>Rejects neighboring unbounded sequence typedef element shapes outside the qualified family.</summary>
    [Theory]
    [InlineData("typedef sequence<long> Inner; typedef sequence<Inner> Nested;", "Nested")]
    [InlineData("typedef long Pair[2]; typedef sequence<Pair> Pairs;", "Pairs")]
    [InlineData("union Choice switch(long) { case 0: long value; }; typedef sequence<Choice> Choices;", "Choices")]
    public void RejectsUnsupportedUnboundedSequenceTypedefElements(string declarations, string expectedAlias)
    {
        // Arrange
        var source = "module CorpusUnsupported { " + declarations + " };";
        var input = new IdlInput("unsupported-unbounded-sequence-alias.idl", source, unboundedSupport: true);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var bound = IdlAnalyzer.Analyze([input], MD5.HashData, CancellationToken.None, diagnostics);
        diagnostics.ShouldBeEmpty();
        var files = BackendCompiler.Generate(bound.Declarations, input, diagnostics);

        // Assert
        diagnostics.ShouldContain(diagnostic =>
            diagnostic.Severity == IdlDiagnosticSeverity.Error
            && diagnostic.Message.Contains(expectedAlias, StringComparison.Ordinal));
        files.ShouldBeEmpty();
    }

    private static IReadOnlyList<GeneratedCppFile> GenerateBoundInput(IdlInput input)
    {
        var diagnostics = new List<IdlDiagnostic>();
        var bound = IdlAnalyzer.Analyze([input], MD5.HashData, CancellationToken.None, diagnostics);
        diagnostics.ShouldBeEmpty();

        var files = BackendCompiler.Generate(bound.Declarations, input, diagnostics);
        diagnostics.ShouldBeEmpty();
        return files;
    }

    /// <summary>Preserves explicit bounds for narrow and wide string boundary members.</summary>
    [Fact]
    public void EmitsBoundedNarrowAndWideStringBoundaries()
    {
        // Arrange
        using var workspace = new GenerationWorkspace();
        var source = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Corpus", "04-boundaries.idl"));

        // Act
        var result = workspace.Generate(source, "04-boundaries");

        // Assert
        result.ExitCode.ShouldBe(0, result.Diagnostics);
        result.Diagnostics.ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(workspace.OutputPath, "04-boundaries.hpp"));
        header.ShouldContain("::std::string narrow = {};");
        header.ShouldContain("::std::wstring wide = {};");

        var plugin = File.ReadAllText(Path.Combine(workspace.OutputPath, "04-boundariesPlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::StringType(8)");
        plugin.ShouldContain("::dds::core::xtypes::WStringType(8)");
        plugin.ShouldContain("#include <dds/core/xtypes/CollectionTypes.hpp>");
    }

}
