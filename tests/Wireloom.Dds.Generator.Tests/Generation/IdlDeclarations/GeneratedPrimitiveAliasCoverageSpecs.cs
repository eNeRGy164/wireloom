namespace Wireloom.Generation.IdlDeclarations.Tests;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

public sealed class GeneratedPrimitiveAliasCoverageSpecs
{
    [Fact]
    public void PrimitiveAliasesEmitExpectedPluginAnnotationsAndNativeDefaults()
    {
        // Arrange
        var input = Input("primitive-alias-matrix.idl",
            """
            module PrimitiveAliases {
                typedef short ShortAlias;
                typedef long LongAlias;
                typedef long long LongLongAlias;
                typedef unsigned short UnsignedShortAlias;
                typedef unsigned long UnsignedLongAlias;
                typedef unsigned long long UnsignedLongLongAlias;
                typedef int8 Int8Alias;
                typedef uint8 Uint8Alias;
                typedef octet OctetAlias;
                typedef boolean BooleanAlias;
                typedef char CharAlias;
                typedef wchar WcharAlias;
                typedef float FloatAlias;
                typedef double DoubleAlias;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        AssertPrimitiveAlias(documents, "ShortAlias", "Int16", "Int16Value", "(short)0", "short.MinValue", "short.MaxValue", "0");
        AssertPrimitiveAlias(documents, "LongAlias", "Int32", "Int32Value", "0", "int.MinValue", "int.MaxValue", "(int)0");
        AssertPrimitiveAlias(documents, "LongLongAlias", "Int64", "Int64Value", "0L", "long.MinValue", "long.MaxValue", "0L");
        AssertPrimitiveAlias(documents, "UnsignedShortAlias", "Uint16", "Uint16Value", "(ushort)0", "ushort.MinValue", "ushort.MaxValue", "(ushort)0");
        AssertPrimitiveAlias(documents, "UnsignedLongAlias", "UInt32", "Uint32Value", "0U", "uint.MinValue", "uint.MaxValue", "(uint)0");
        AssertPrimitiveAlias(documents, "UnsignedLongLongAlias", "UInt64", "Uint64Value", "0UL", "ulong.MinValue", "ulong.MaxValue", "(ulong)0");
        AssertPrimitiveAlias(documents, "Int8Alias", "Int8", "Int8Value", "(sbyte)0", "sbyte.MinValue", "sbyte.MaxValue", "(sbyte)0");
        AssertPrimitiveAlias(documents, "Uint8Alias", "Uint8", "Uint8Value", "(byte)0", "byte.MinValue", "byte.MaxValue", "(byte)0");
        AssertPrimitiveAlias(documents, "OctetAlias", "Octet", "OctetValue", "(byte)0", "byte.MinValue", "byte.MaxValue", "(byte)0");
        AssertPrimitiveAlias(documents, "BooleanAlias", "Boolean", "BoolValue", "false", "null", "null", "false");
        AssertPrimitiveAlias(documents, "CharAlias", "Char8", "Char8Value", "'\\0'", "null", "null", "'\\0'");
        AssertPrimitiveAlias(documents, "WcharAlias", "Char16", "Char16Value", "'\\0'", "null", "null", "'\\0'");
        AssertPrimitiveAlias(documents, "FloatAlias", "Float32", "Float32Value", "0F", "float.MinValue", "float.MaxValue", "0.0F");
        AssertPrimitiveAlias(documents, "DoubleAlias", "Float64", "Float64Value", "0D", "double.MinValue", "double.MaxValue", "0.0D");

        var longLongNative = documents["PrimitiveAliases.Implementation.LongLongAliasUnmanaged.g.cs"].Source;
        longLongNative.ShouldContain("private long Value;");
        longLongNative.ShouldContain("Value = sample.Value;");
    }

    [Fact]
    public void PrimitiveSequenceAliasesUseDynamicPrimitiveTypesAndTypedNativeConversions()
    {
        // Arrange
        var input = Input("primitive-sequence-aliases.idl",
            """
            module PrimitiveSequenceAliases {
                typedef sequence<short, 2> Shorts;
                typedef sequence<long long, 3> LongLongs;
                typedef sequence<unsigned long, 4> UnsignedLongs;
                typedef sequence<float, 5> Floats;
                typedef sequence<double, 6> Doubles;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        AssertPrimitiveSequence(documents, "Shorts", "short", "2");
        AssertPrimitiveSequence(documents, "LongLongs", "long", "3");
        AssertPrimitiveSequence(documents, "UnsignedLongs", "uint", "4");
        AssertPrimitiveSequence(documents, "Floats", "float", "5");
        AssertPrimitiveSequence(documents, "Doubles", "double", "6");

    }

    [Fact]
    public void BoundedStringAndWstringAliasesPreservePluginBoundsAndNativeConversionShapes()
    {
        // Arrange
        var input = Input("bounded-primitive-aliases.idl",
            """
            module BoundedPrimitiveAliases {
                typedef string<8> Text;
                typedef wstring<4> WideText;
                typedef sequence<string<8>, 3> Texts;
                typedef sequence<wstring<4>, 2> WideTexts;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        AssertBoundedStringAlias(documents, "Text", "CreateString(8)", "NativeString", "size: 8", "8");
        AssertBoundedStringAlias(documents, "WideText", "CreateWideString(4)", "NativeWstring", "size: 4", "4");

        var textsPlugin = documents["BoundedPrimitiveAliases.Implementation.TextsPlugin.g.cs"].Source;
        textsPlugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.CreateString(8), 3)");

        var textsNative = documents["BoundedPrimitiveAliases.Implementation.TextsUnmanaged.g.cs"].Source;
        textsNative.ShouldContain("private NativeStringSeq Value;");
        textsNative.ShouldContain("Value.Initialize(max: 3, absoluteMax: 3, maxStrLen: 8, allocateMemory: allocateMemory);");
        textsNative.ShouldContain("Value.FromNative(sample.Value);");
        textsNative.ShouldContain("Value.ToNative(sample.Value, 8);");

        var wideTextsPlugin = documents["BoundedPrimitiveAliases.Implementation.WideTextsPlugin.g.cs"].Source;
        wideTextsPlugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(4), 2)");

        var wideTextsNative = documents["BoundedPrimitiveAliases.Implementation.WideTextsUnmanaged.g.cs"].Source;
        wideTextsNative.ShouldContain("private NativeWstringSeq Value;");
        wideTextsNative.ShouldContain("Value.Initialize(max: 2, absoluteMax: 2, maxStrLen: 4, allocateMemory: allocateMemory);");
        wideTextsNative.ShouldContain("Value.FromNative(sample.Value);");
        wideTextsNative.ShouldContain("Value.ToNative(sample.Value, 4);");
    }

    private static void AssertPrimitiveAlias(
        Dictionary<string, GeneratedIdlSource> documents,
        string alias,
        string typeKind,
        string valueProperty,
        string defaultValue,
        string minimum,
        string maximum,
        string nativeDefault)
    {
        var plugin = documents[$"PrimitiveAliases.Implementation.{alias}Plugin.g.cs"].Source;
        plugin.ShouldContain($"TypeKind.{typeKind}");
        plugin.ShouldContain($"{valueProperty} = {defaultValue}");
        plugin.ShouldContain(minimum == "null" ? "minValue: null" : $"{valueProperty} = {minimum}");
        plugin.ShouldContain(maximum == "null" ? "maxValue: null" : $"{valueProperty} = {maximum}");

        var native = documents[$"PrimitiveAliases.Implementation.{alias}Unmanaged.g.cs"].Source;
        native.ShouldContain($"Value = {nativeDefault};");
    }

    private static void AssertPrimitiveSequence(
        Dictionary<string, GeneratedIdlSource> documents,
        string alias,
        string csharpType,
        string bound)
    {
        var plugin = documents[$"PrimitiveSequenceAliases.Implementation.{alias}Plugin.g.cs"].Source;
        plugin.ShouldContain($"CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<{csharpType}>(), {bound})");

        var native = documents[$"PrimitiveSequenceAliases.Implementation.{alias}Unmanaged.g.cs"].Source;
        native.ShouldContain($"Value.Initialize<{csharpType}>(max: {bound}, absoluteMax: {bound}, allocateMemory: allocateMemory);");
        native.ShouldContain($"Value.FromNative((Sequence<{csharpType}>)sample.Value);");
        native.ShouldContain($"Value.ToNative((Sequence<{csharpType}>)sample.Value);");
    }

    private static void AssertBoundedStringAlias(
        Dictionary<string, GeneratedIdlSource> documents,
        string alias,
        string pluginType,
        string nativeType,
        string initializeSize,
        string bound)
    {
        var plugin = documents[$"BoundedPrimitiveAliases.Implementation.{alias}Plugin.g.cs"].Source;
        plugin.ShouldContain($"dtf.{pluginType}");

        var native = documents[$"BoundedPrimitiveAliases.Implementation.{alias}Unmanaged.g.cs"].Source;
        native.ShouldContain($"private {nativeType} Value;");
        native.ShouldContain($"Value.Initialize({initializeSize}, allocateMemory: allocateMemory);");
        native.ShouldContain("sample.Value = Value.FromNative();");
        native.ShouldContain($"Value.ToNative(sample.Value, {bound});");
    }
}
