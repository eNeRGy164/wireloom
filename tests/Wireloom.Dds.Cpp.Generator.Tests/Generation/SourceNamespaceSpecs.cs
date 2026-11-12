namespace Wireloom.Dds.Cpp.Generator.Tests;

/// <summary>Checks namespace layout in generated C++ implementation files.</summary>
public sealed class SourceNamespaceSpecs
{
    private sealed class TemporaryDirectory : IDisposable
    {
        private readonly DirectoryInfo directory = Directory.CreateTempSubdirectory("wireloom-cpp-namespaces-");

        internal string Path => directory.FullName;

        public void Dispose() => directory.Delete(recursive: true);
    }

    /// <summary>Groups adjacent same-namespace definitions while keeping plugin contexts translation-unit private.</summary>
    [Fact]
    public void ConsolidatesConsecutiveSourceNamespacesAndKeepsPluginContextsPrivate()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "same-namespace.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, """
            module Showcase {
                enum Mode { Idle, Ready };
                enum State { Off, On };
                struct First { Mode mode; };
                struct Second { long value; };
                union Payload switch(Mode) { case Idle: long number; default: string text; };
                union Alternative switch(State) { case Off: long count; default: boolean enabled; };
            };
            """);
        using var output = new StringWriter();
        using var errors = new StringWriter();

        // Act
        var exitCode = CppCommand.Run(["generate", input, outputPath], output, errors);

        // Assert
        exitCode.ShouldBe(0, errors.ToString());
        errors.ToString().ShouldBeEmpty();
        var valueSource = File.ReadAllText(Path.Combine(outputPath, "same-namespace.cxx"));
        var pluginSource = File.ReadAllText(Path.Combine(outputPath, "same-namespacePlugin.cxx"));
        valueSource.Split("namespace Showcase\n{").Length.ShouldBe(2);
        pluginSource.Split("namespace Showcase\n{").Length.ShouldBe(2);

        var floatHelper = valueSource.IndexOf("namespace\n{\n// Preserve the epsilon comparison", StringComparison.Ordinal);
        var firstShowcaseNamespace = valueSource.IndexOf("namespace Showcase\n{", StringComparison.Ordinal);
        (floatHelper >= 0 && floatHelper < firstShowcaseNamespace).ShouldBeTrue();

        var showcaseStart = pluginSource.IndexOf("namespace Showcase\n{", StringComparison.Ordinal);
        var showcaseEnd = pluginSource.IndexOf("} // namespace Showcase", showcaseStart, StringComparison.Ordinal);
        var showcaseContexts = pluginSource[showcaseStart..showcaseEnd];
        showcaseContexts.Split("namespace\n{").Length.ShouldBe(2);
        showcaseContexts.Split("} // namespace\n").Length.ShouldBe(2);
        var firstEnum = showcaseContexts.IndexOf("Keep RTI enum metadata private", StringComparison.Ordinal);
        var firstStruct = showcaseContexts.IndexOf("Keep RTI plugin state private", StringComparison.Ordinal);
        var firstUnion = showcaseContexts.LastIndexOf("Keep RTI plugin state private", StringComparison.Ordinal);
        (firstEnum >= 0 && firstStruct > firstEnum && firstUnion > firstStruct).ShouldBeTrue();

        var valueEnum = valueSource.IndexOf("Mode::Idle", StringComparison.Ordinal);
        var valueStruct = valueSource.IndexOf("First::First", StringComparison.Ordinal);
        var valueUnion = valueSource.IndexOf("Payload::Payload", StringComparison.Ordinal);
        (valueEnum >= 0 && valueStruct > valueEnum && valueUnion > valueStruct).ShouldBeTrue();
    }

    /// <summary>Groups RTI 6.1.2 plugin type contexts in their shared IDL namespace.</summary>
    [Fact]
    public void ConsolidatesRti612PluginContextNamespaces()
    {
        // Arrange
        using var directory = new TemporaryDirectory();
        var input = Path.Combine(directory.Path, "same-namespace.idl");
        var outputPath = Path.Combine(directory.Path, "generated");
        File.WriteAllText(input, """
            module Showcase {
                enum Mode { Idle, Ready };
                enum State { Off, On };
                struct First { Mode mode; };
                struct Second { long value; };
                union Payload switch(Mode) { case Idle: long number; default: string text; };
                union Alternative switch(State) { case Off: long count; default: boolean enabled; };
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
        var valueSource = File.ReadAllText(Path.Combine(outputPath, "same-namespace.cxx"));
        var pluginSource = File.ReadAllText(Path.Combine(outputPath, "same-namespacePlugin.cxx"));
        valueSource.Split("namespace Showcase\n{").Length.ShouldBe(2);
        pluginSource.Split("namespace Showcase\n{").Length.ShouldBe(2);
        pluginSource.IndexOf("wireloom_Mode_enum_type_context", StringComparison.Ordinal)
            .ShouldBeLessThan(pluginSource.IndexOf("wireloom_Payload_union_context", StringComparison.Ordinal));
        pluginSource.IndexOf("wireloom_Payload_union_context", StringComparison.Ordinal)
            .ShouldBeLessThan(pluginSource.IndexOf("wireloom_First_type_context", StringComparison.Ordinal));
    }
}
