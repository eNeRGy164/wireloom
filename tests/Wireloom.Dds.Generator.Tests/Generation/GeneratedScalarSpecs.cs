using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedScalarSpecs
{
    [Fact]
    [Trait("Corpus", "C001")]
    public void PrimitiveScalarEmitsAllManagedNativePluginAndSupportArtifacts()
    {
        // Arrange
        var input = Input(
                "primitive.idl",
                "module Primitive { struct Sample { long value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Primitive.Sample.g.cs"].Source;
        managed.ShouldContain("public partial class Sample");
        managed.ShouldContain("public int value { get; set; }");
        managed.ShouldContain("public Sample()");
        managed.ShouldContain("public Sample(int value)");
        managed.ShouldContain("public Sample(Sample? other)");
        managed.ShouldContain("public bool Equals(Sample? other)");
        managed.ShouldContain("public override int GetHashCode()");
        managed.ShouldContainInOrder(
            "public Sample()",
            "public Sample(int value)",
            "public Sample(Sample? other)");

        var unmanaged = documents["Primitive.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("public struct SampleUnmanaged");
        unmanaged.ShouldContain("public void Destroy(bool optionalsOnly)");
        unmanaged.ShouldContain("public void FromNative(Sample sample, bool keysOnly = false)");
        unmanaged.ShouldContain("public void Initialize(bool allocatePointers = true, bool allocateMemory = true)");
        unmanaged.ShouldContain("public void ToNative(Sample sample, bool keysOnly = false)");

        var samplePlugin = documents["Primitive.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("""new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)""");

        var sampleSupport = documents["Primitive.SampleSupport.g.cs"].Source;
        sampleSupport.ShouldContain("public class SampleSupport : TypeSupport<Sample>");
    }

    [Fact]
    [Trait("Corpus", "C014")]
    [Trait("Corpus", "C015")]
    public void BoundedStringsEmitBoundedManagedNativeAndPluginArtifacts()
    {
        // Arrange
        var input = Input(
                "strings.idl",
                "module Strings { struct Sample { string<8> text; wstring<4> wide; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Strings.Sample.g.cs"].Source;
        managed.ShouldContain("[Bound(8)]");
        managed.ShouldContain("public string text");
        managed.ShouldContain("[Bound(4)]");
        managed.ShouldContain("public string wide");

        var unmanaged = documents["Strings.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("private NativeString text");
        unmanaged.ShouldContain("private NativeWstring wide");
        unmanaged.ShouldContain("text.Destroy();");
        unmanaged.ShouldContain("wide.Destroy();");

        var plugin = documents["Strings.Implementation.SamplePlugin.g.cs"].Source;
        plugin.ShouldContain("dtf.CreateString(8)");
        plugin.ShouldContain("dtf.CreateWideString(4)");

        var sampleSupport = documents["Strings.SampleSupport.g.cs"].Source;
        sampleSupport.ShouldContain("TypeSupport<Sample>");
    }

    [Fact]
    [Trait("Corpus", "C038")]
    public void OptionalPrimitivesEmitOptionalStorageAndMetadata()
    {
        // Arrange
        var input = Input(
                "optional-primitive.idl",
                "module OptionalPrimitive { @appendable struct Sample { @optional long value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var sample = documents["OptionalPrimitive.Sample.g.cs"].Source;
        sample.ShouldContain("[Optional]");
        sample.ShouldContain("public int? value");

        var unmanaged = documents["OptionalPrimitive.Implementation.SampleUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("NativeUnmanagedOptional value");
        unmanaged.ShouldContain("value.Destroy(optionalsOnly);");
        unmanaged.ShouldContain("value.FromNative<int>()");

        var samplePlugin = documents["OptionalPrimitive.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("isOptional: true");
    }

    [Fact]
    [Trait("Corpus", "C009")]
    public void ScalarAliasesDelegateThroughAliasArtifacts()
    {
        // Arrange
        var input = Input(
                "scalar-alias.idl",
                "module ScalarAlias { typedef long Scalar; struct Sample { Scalar value; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var scalar = documents["ScalarAlias.Scalar.g.cs"].Source;
        scalar.ShouldContain("public int Value");

        var unmanaged = documents["ScalarAlias.Implementation.ScalarUnmanaged.g.cs"].Source;
        unmanaged.ShouldContain("public void Destroy(bool optionalsOnly)");
        unmanaged.ShouldContain("Value");

        var scalarPlugin = documents["ScalarAlias.Implementation.ScalarPlugin.g.cs"].Source;
        scalarPlugin.ShouldContain("CreateAliasWithAccessInfo<ScalarUnmanaged>");

        var samplePlugin = documents["ScalarAlias.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("ScalarSupport.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C009")]
    public void EnumsEmitManagedNativePluginAndSupportSurfaces()
    {
        // Arrange
        var input = Input(
                "enum.idl",
                "module Enum { enum Color { Red, Blue }; struct Sample { Color color; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var color = documents["Enum.Color.g.cs"].Source;
        color.ShouldContain("public enum Color");
        color.ShouldContain("Red");
        color.ShouldContain("Blue");

        var sample = documents["Enum.Sample.g.cs"].Source;
        sample.ShouldContain("public Color color");
        sample.ShouldContain("color { get; set; } = (Color)0");

        var sampleUnmanaged = documents["Enum.Implementation.SampleUnmanaged.g.cs"].Source;
        sampleUnmanaged.ShouldContain("public struct SampleUnmanaged");

        var colorPlugin = documents["Enum.Implementation.ColorPlugin.g.cs"].Source;
        colorPlugin.ShouldContain("new EnumMember(\"Red\"");
        colorPlugin.ShouldContain("EnumValue = 0");

        var samplePlugin = documents["Enum.Implementation.SamplePlugin.g.cs"].Source;
        samplePlugin.ShouldContain("ColorSupport.Instance.GetDynamicTypeInternal(isPublic)");

        var colorSupport = documents["Enum.ColorSupport.g.cs"].Source;
        colorSupport.ShouldContain("TypeSupport<Color>");

        var sampleSupport = documents["Enum.SampleSupport.g.cs"].Source;
        sampleSupport.ShouldContain("TypeSupport<Sample>");
    }
}

