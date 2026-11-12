namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Checks the RTI 6.1.2 legacy FlatData plugin adapter.</summary>
public sealed class Rti612FlatDataSpecs
{
    private sealed class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("wireloom-cpp-rti612-flatdata-");

        internal string Path => directory.FullName;

        public void Dispose() => directory.Delete(recursive: true);
    }

    /// <summary>Emits FlatData sample and helper metadata with RTI 6.1.2 callbacks.</summary>
    [Fact]
    public void EmitsFlatDataAdapterForRti612()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "flat-data.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.Copy(
            Path.Combine(AppContext.BaseDirectory, "Corpus", "10-flat-data-binding.idl"),
            input);
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();
        var header = File.ReadAllText(Path.Combine(outputPath, "flat-data.hpp"));
        var plugin = File.ReadAllText(Path.Combine(outputPath, "flat-dataPlugin.cxx"));
        header.ShouldContain("SamplePlainHelper");
        header.ShouldContain("is_flat_data_type_only<::CorpusFlatDataBinding::Sample>");
        plugin.ShouldContain("RTI 6.1.2 FlatData plugin adapter");
        plugin.ShouldContain("#include <algorithm>");
        plugin.ShouldContain("#include <cstring>");
        plugin.ShouldContain("#include <ndds/xcdr/xcdr_stream.h>");
        plugin.ShouldContain("wireloom_initialize_dynamic_types()");
        plugin.ShouldContain("StructType plain_helper_type;");
        plugin.ShouldContain("helper_type_code->_annotations._allowedDataRepresentationMask");
        plugin.ShouldContain("WireloomRti612TypePlugin<::CorpusFlatDataBinding::SamplePlainHelper>::create");
        plugin.ShouldContain("type_programs<::CorpusFlatDataBinding::Sample>::get()");
        plugin.ShouldContain("ProgramsSingleton<");
        plugin.ShouldContain("PRES_TYPEPLUGIN_FLATDATA_TYPE");
        plugin.ShouldContain("RTIXCdrFlatSample_getEncapsulation");
        plugin.ShouldContain("std::uint32_t value_0");
        plugin.ShouldNotContain("GenericTypePluginFactory");
        plugin.ShouldNotContain("CTypePlugin");
        plugin.ShouldNotContain("forceUseWHPoolWithFlatData");
        plugin.ShouldNotContain("RTIXCdrXTypesComplianceMask_verifyGeneratedXTypesMask");
        plugin.ShouldNotContain("RTI FlatData registration requires the dedicated FlatData plugin callbacks");
    }
}
