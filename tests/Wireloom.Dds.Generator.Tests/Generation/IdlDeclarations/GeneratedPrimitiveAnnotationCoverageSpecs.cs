using Wireloom.Dds.Generator.Tests;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Generation.IdlDeclarations.Tests;

public sealed class GeneratedPrimitiveAnnotationCoverageSpecs
{
    [Fact]
    public void PrimitiveAnnotationsEmitKindsDefaultsRangesAndUnits()
    {
        // Arrange
        var input = Input("primitive-annotations.idl",
            """
            module PrimitiveAnnotations {
                struct Values {
                    @min(-32768) @max(32767) @unit("shorts") short signedShort;
                    @min(0) @max(4294967295) @unit("items") unsigned long unsignedLong;
                    @min(-128) @max(127) @unit("bytes") int8 signedByte;
                    @min(0) @max(255) @unit("bytes") uint8 unsignedByte;
                    @min(0) @max(255) @unit("octets") octet octetValue;
                    @unit("flags") boolean enabled;
                    @unit("characters") char character;
                    @unit("characters") wchar wideCharacter;
                    @unit("meters") float distance;
                    @unit("meters") double preciseDistance;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var plugin = documents["PrimitiveAnnotations.Implementation.ValuesPlugin.g.cs"].Source;
        plugin.ShouldContainInOrder(
            "TypeKind.Int16,",
            "defaultValue: new AnnotationParameterValue { Int16Value = 0 },",
            "minValue: new AnnotationParameterValue { Int16Value = -32768 },",
            "maxValue: new AnnotationParameterValue { Int16Value = 32767 },",
            "unit: \"shorts\"");
        plugin.ShouldContainInOrder(
            "TypeKind.UInt32,",
            "defaultValue: new AnnotationParameterValue { Uint32Value = 0U },",
            "minValue: new AnnotationParameterValue { Uint32Value = 0U },",
            "maxValue: new AnnotationParameterValue { Uint32Value = 4294967295U },",
            "unit: \"items\"");
        plugin.ShouldContainInOrder(
            "TypeKind.Int8,",
            "defaultValue: new AnnotationParameterValue { Int8Value = 0 },",
            "minValue: new AnnotationParameterValue { Int8Value = -128 },",
            "maxValue: new AnnotationParameterValue { Int8Value = 127 },",
            "unit: \"bytes\"");
        plugin.ShouldContainInOrder(
            "TypeKind.Uint8,",
            "defaultValue: new AnnotationParameterValue { Uint8Value = 0 },",
            "minValue: new AnnotationParameterValue { Uint8Value = 0 },",
            "maxValue: new AnnotationParameterValue { Uint8Value = 255 },");
        plugin.ShouldContainInOrder(
            "TypeKind.Octet,",
            "defaultValue: new AnnotationParameterValue { OctetValue = 0 },",
            "minValue: new AnnotationParameterValue { OctetValue = 0 },",
            "maxValue: new AnnotationParameterValue { OctetValue = 255 },",
            "unit: \"octets\"");
    }

    [Fact]
    public void PrimitiveAnnotationsUseExpectedFloatingPointAndCharacterLiterals()
    {
        // Arrange
        var input = Input("primitive-literals.idl",
            """
            module PrimitiveLiterals {
                struct Values {
                    float ratio;
                    double preciseRatio;
                    boolean enabled;
                    char character;
                    wchar wideCharacter;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var plugin = documents["PrimitiveLiterals.Implementation.ValuesPlugin.g.cs"].Source;
        plugin.ShouldContainInOrder(
            "TypeKind.Float32,",
            "defaultValue: new AnnotationParameterValue { Float32Value = 0.0F },",
            "minValue: new AnnotationParameterValue { Float32Value = float.MinValue },",
            "maxValue: new AnnotationParameterValue { Float32Value = float.MaxValue },");
        plugin.ShouldContainInOrder(
            "TypeKind.Float64,",
            "defaultValue: new AnnotationParameterValue { Float64Value = 0.0D },",
            "minValue: new AnnotationParameterValue { Float64Value = double.MinValue },",
            "maxValue: new AnnotationParameterValue { Float64Value = double.MaxValue },");
        plugin.ShouldContain("TypeKind.Boolean,");
        plugin.ShouldContain("defaultValue: new AnnotationParameterValue { BoolValue = false },");
        plugin.ShouldContain("TypeKind.Char8,");
        plugin.ShouldContain("defaultValue: new AnnotationParameterValue { Char8Value = '\\0' },");
        plugin.ShouldContain("TypeKind.Char16,");
        plugin.ShouldContain("defaultValue: new AnnotationParameterValue { Char16Value = '\\0' },");
    }

    [Fact]
    public void PrimitiveDefaultsFormatSignedUnsignedAndSentinelValues()
    {
        // Arrange
        var input = Input("primitive-defaults.idl",
            """
            module PrimitiveDefaults {
                struct Values {
                    @default(-32768) short signedShort;
                    @default(65535) unsigned short unsignedShort;
                    @default(-2147483648) long signedLong;
                    @default(4294967295) unsigned long unsignedLong;
                    @default(-9223372036854775808) long long minimumLong;
                    @default(18446744073709551615) unsigned long long maximumUnsignedLong;
                    @default(-128) int8 signedByte;
                    @default(255) uint8 unsignedByte;
                    @default(255) octet octetValue;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["PrimitiveDefaults.Values.g.cs"].Source;
        managed.ShouldContain("signedShort = -32768;");
        managed.ShouldContain("unsignedShort = 65535;");
        managed.ShouldContain("signedLong = -2147483648;");
        managed.ShouldContain("unsignedLong = 4294967295U;");
        managed.ShouldContain("minimumLong = long.MinValue;");
        managed.ShouldContain("maximumUnsignedLong = ulong.MaxValue;");
        managed.ShouldContain("signedByte = -128;");
        managed.ShouldContain("unsignedByte = 255;");
        managed.ShouldContain("octetValue = 255;");

        var plugin = documents["PrimitiveDefaults.Implementation.ValuesPlugin.g.cs"].Source;
        plugin.ShouldContain("Int64Value = long.MinValue");
        plugin.ShouldContain("Uint64Value = ulong.MaxValue");
    }
}
