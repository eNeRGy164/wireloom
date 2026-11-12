namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Checks key callback generation for the RTI 6.1.2 C++ plugin adapter.</summary>
public sealed class Rti612KeyPluginSpecs
{
    private sealed class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("wireloom-cpp-rti612-keys-");

        internal string Path => directory.FullName;

        public void Dispose() => directory.Delete(recursive: true);
    }

    /// <summary>Projects scalar, composite, string, and array keys while excluding non-key members.</summary>
    [Fact]
    public void EmitsKeyCallbacksForSupportedKeyShapes()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "keys.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input,
            """
            module Keys {
                struct Direct { @key long id; long payload; };
                struct Composite { @key long tenant; @key string<12> name; long payload; };
                struct Coordinates { @key long coordinates[2]; long payload; };
                struct Unkeyed { long value; };
            };
            """);
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(
            ["generate", input, outputPath, "--rti-version", "6.1.2"], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();

        var plugin = File.ReadAllText(Path.Combine(outputPath, "keysPlugin.cxx"));
        plugin.ShouldContain("WireloomRti612KeyCallbacks<::Keys::Direct>");
        plugin.ShouldContain("WireloomRti612KeyCallbacks<::Keys::Composite>");
        plugin.ShouldContain("WireloomRti612KeyCallbacks<::Keys::Coordinates>");
        plugin.ShouldContain("struct WireloomRti612KeyCallbacks");
        plugin.ShouldContain("static const bool has_keys = true;");
        plugin.ShouldContain("return PRES_TYPEPLUGIN_USER_KEY;");
        plugin.ShouldContain("return PRES_TYPEPLUGIN_NO_KEY;");
        plugin.ShouldContain("typed_destination->id = typed_source->id;");
        plugin.ShouldContain("typed_destination->tenant = typed_source->tenant;");
        plugin.ShouldContain("typed_destination->name = typed_source->name;");
        plugin.ShouldContain("typed_destination->coordinates = typed_source->coordinates;");
        plugin.ShouldNotContain("typed_destination->payload = typed_source->payload;");
        plugin.ShouldContain("WireloomRti612KeyCallbacks<T>::create_key");
        plugin.ShouldContain("WireloomRti612KeyCallbacks<T>::destroy_key");
        plugin.ShouldContain("plugin->getKeyKindFnc = KeyCallbacks::get_key_kind;");
        plugin.ShouldContain("plugin->instanceToKeyFnc = KeyCallbacks::instance_to_key;");
        plugin.ShouldContain("plugin->keyToInstanceFnc = KeyCallbacks::key_to_instance;");
        plugin.ShouldContain("PRESTypePluginDefaultEndpointData_createMD5StreamWithInfo(");
    }
}
