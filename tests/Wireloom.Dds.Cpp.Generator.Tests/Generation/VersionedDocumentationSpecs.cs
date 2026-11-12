namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Checks that generated documentation follows the selected RTI profile.</summary>
public sealed class VersionedDocumentationSpecs
{
    /// <summary>Emits signed-byte members through the RTI 6.1.2 profile adapter.</summary>
    [Fact]
    public void EmitsSignedByteMemberForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { int8 value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        File.ReadAllText(Path.Combine(outputPath, "sample.hpp")).ShouldContain("std::int8_t value");
    }

    /// <summary>Keeps direct scalar structs emit-able for every selected API profile.</summary>
    [Theory]
    [InlineData("6.1.2", "WireloomRti612TypePlugin")]
    [InlineData("7.3.1", "factory.create_struct(")]
    [InlineData("7.7.0", "factory.create_struct(")]
    public void EmitsDirectScalarStructForEachRtiProfile(string version, string expectedPluginPath)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain(expectedPluginPath);
        if (version == "6.1.2")
        {
            plugin.ShouldNotContain("#include <rti/topic/cdr/GenericTypePluginFactory.hpp>");
        }
        else
        {
            plugin.ShouldContain("#include <rti/topic/cdr/GenericTypePluginFactory.hpp>");
            plugin.ShouldNotContain("#include <dds/core/xtypes/AliasType.hpp>");
            plugin.ShouldNotContain("#include <dds/core/xtypes/MemberType.hpp>");
            plugin.ShouldNotContain("#include <dds/core/xtypes/UnionType.hpp>");
            plugin.ShouldNotContain("#include <dds/core/xtypes/CollectionTypes.hpp>");
            plugin.ShouldNotContain("#include <memory>");
            plugin.ShouldNotContain("#include <mutex>");
        }
    }

    /// <summary>Emits nested values without a private TypeCode annotation for RTI 7.3.1.</summary>
    [Fact]
    public void EmitsNestedStructWithoutPrivateAnnotationForRti731()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "@nested struct NestedValue { long value; }; struct Topic { NestedValue nested; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        var nestedSupportStart = header.IndexOf("struct topic_type_support<::NestedValue>", StringComparison.Ordinal);
        var nestedSupportEnd = header.IndexOf("};", nestedSupportStart, StringComparison.Ordinal) + 2;
        var nestedSupport = header[nestedSupportStart..nestedSupportEnd];
        nestedSupport.ShouldContain("allocate_sample(");
        nestedSupport.ShouldNotContain("register_type(");

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldNotContain("_annotations._isNested");
        plugin.ShouldContain("dynamic_type<::NestedValue>::get()");
    }

    /// <summary>Emits range metadata and docs for RTI 7.3.1 without 7.7-only unit fields.</summary>
    [Fact]
    public void EmitsRangeButOmitsUnitFieldForRti731()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { @range(min = -1, max = 1) @unit(\"meters\") long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("_annotations._minValue._u.long_value = -1L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 1L;");
        plugin.ShouldNotContain("_annotations._unitValue");

        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        header.ShouldContain("Unit metadata: meters.");
    }

    /// <summary>Emits TypeCode default and range metadata for the RTI 7.3.1 layout.</summary>
    [Fact]
    public void EmitsDefaultAndRangeTypeCodeAnnotationsForRti731()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "@appendable struct Sample { @min(0) @max(100) @default(50) long value; @range(min = -32, max = 31) long ranged; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("_annotations._defaultValue._d = RTI_XCDR_TK_LONG;");
        plugin.ShouldContain("_annotations._defaultValue._u.long_value = 50;");
        plugin.ShouldContain("_annotations._minValue._u.long_value = 0L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 100L;");
        plugin.ShouldContain("_annotations._minValue._u.long_value = -32L;");
        plugin.ShouldContain("_annotations._maxValue._u.long_value = 31L;");
        plugin.ShouldContain("#error \"nested/default/range TypeCode adapter requires RTI Connext DDS 7.3.1.\"");
        plugin.ShouldNotContain("_annotations._unitValue");
    }

    /// <summary>Emits the native TypeCode and PRESTypePlugin adapter for RTI 7.3.1 optional recursive arrays.</summary>
    [Fact]
    public void EmitsOptionalSelfRecursiveArrayAdapterForRti731()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Node { long value; @optional Node children[2]; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("RTICdrResourceNode wireloom_recursive_resource_node");
        plugin.ShouldContain("RTICdrTypeCode_isResourceVisited");
        plugin.ShouldContain("get_opt_array_sample_access_info<::Node>()");
        plugin.ShouldContain("DDS_TypeCodeFactory_assert_programs_in_global_list");
        plugin.ShouldContain("register_type_plugin(");
        plugin.ShouldNotContain("type_plugin = &factory.create_type_plugin(*type)");
        plugin.ShouldContain("get_cdr_serialization_programs<");
    }

    /// <summary>Emits aggregate alias TypeCode nodes with the selected 7.3.1 guard.</summary>
    [Fact]
    public void EmitsAggregateAliasTypeCodeForRti731()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input,
            "struct Point { long x; long y; }; typedef Point PointAlias; struct Sample { PointAlias point; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("Construct RTI raw alias nodes for aggregate typedef chains.");
        plugin.ShouldContain("RTI_DDS_VERSION_MINOR != 3");
        plugin.ShouldContain("RTI_DDS_VERSION_RELEASE != 1");
    }

    /// <summary>Emits XCDR2-only metadata against the fields available in RTI 7.3.1.</summary>
    [Fact]
    public void EmitsXcdr2OnlyTypeCodeAnnotationForRti731()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "@data_representation(XCDR2) struct Sample { long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "7.3.1"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("_data._annotations._allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        plugin.ShouldContain("RTI_DDS_VERSION_MINOR != 3");
        plugin.ShouldContain("RTI_DDS_VERSION_RELEASE != 1");
    }

    /// <summary>Emits RTI 6.1.2 sample access metadata for string sequences.</summary>
    [Theory]
    [InlineData("6.1.2")]
    public void EmitsStringSequenceSampleAccessForRti612(string version)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { sequence<string, 2> values; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain("::dds::core::xtypes::SequenceType(");
        plugin.ShouldContain("sequence_helper<::omg::types::bounded_sequence< ::std::string, 2 >>::sample_access_info()");
    }

    /// <summary>Uses the compliance mask and compile guard retained by the selected RTI profile.</summary>
    [Theory]
    [InlineData("7.3.1", "0x0000068C", "MINOR != 3", "RELEASE != 1")]
    [InlineData("7.7.0", "0x0000018C", "MINOR != 7", "RELEASE != 0")]
    public void EmitsProfileSpecificStlSampleAccessMetadata(string version, string complianceMask, string minorGuard, string releaseGuard)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { sequence<long, 2> values; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "samplePlugin.cxx"));
        plugin.ShouldContain($"verifyGeneratedXTypesMask({complianceMask})");
        plugin.ShouldContain(minorGuard);
        plugin.ShouldContain(releaseGuard);
        plugin.ShouldContain($"STL sample-access TypeCode metadata requires RTI Connext DDS {version}.");
    }

    /// <summary>Uses the selected profile for direct API references on scalar structs.</summary>
    [Theory]
    [InlineData("6.1.2")]
    [InlineData("7.3.1")]
    [InlineData("7.7.0")]
    public void UsesSelectedRtiVersionInDirectApiReferences(string version)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { long value; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());

        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        header.ShouldContain($"/connext-dds/{version}/doc/api/connext_dds/api_cpp2/classdds_1_1core_1_1xtypes_1_1StructType.html");

        var references = header.Split('\n').Where(line => line.Contains("@see [", StringComparison.Ordinal)).ToArray();
        references.ShouldNotBeEmpty();
        references.ShouldAllBe(line => line.Contains($"/connext-dds/{version}/doc/", StringComparison.Ordinal));
    }

    /// <summary>Uses the selected RTI profile for manual string-bound references where qualified.</summary>
    [Theory]
    [InlineData("7.3.1", "RTI_ConnextDDS_CoreLibraries_UsersManual.pdf")]
    [InlineData("7.7.0", "users_manual/Strings_and_Wide_Strings.htm#StringsAndWideStrings")]
    public void UsesSelectedRtiVersionInManualStringReferences(string version, string manualPath)
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "sample.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, "struct Sample { string<16> name; };");
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", version], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var header = File.ReadAllText(Path.Combine(outputPath, "sample.hpp"));
        header.ShouldContain($"RTI string bounds ({version})");
        header.ShouldContain($"/connext-dds/{version}/doc/manuals/connext_dds_professional/users_manual/{manualPath}");
    }

    private sealed class TemporaryDirectory : IDisposable
    {
        internal string Path { get; } = Directory.CreateTempSubdirectory("wireloom-cpp-docs-").FullName;

        public void Dispose() => Directory.Delete(Path, recursive: true);
    }
}
