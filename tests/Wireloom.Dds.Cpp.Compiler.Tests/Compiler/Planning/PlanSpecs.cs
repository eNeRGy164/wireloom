extern alias CppCompilerAssembly;

using InternalCompiler = CppCompilerAssembly::Wireloom.Dds.Cpp.Compiler;
using System.Numerics;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;
using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Compiler.Tests;

/// <summary>Checks typed scalar projection and defensive handling at the shared-model boundary.</summary>
public sealed class PlanSpecs
{
    /// <summary>Preserves the exact finite integer value in portable C++ literals.</summary>
    [Theory]
    [InlineData("short", "17", "17")]
    [InlineData("long long", "42", "42LL")]
    [InlineData("long long", "-9223372036854775808", "(-9223372036854775807LL - 1LL)")]
    [InlineData("unsigned long long", "18446744073709551615", "18446744073709551615ULL")]
    public void FormatsIntegerDefaults(string type, string value, string expected)
    {
        // Arrange
        PrimitiveType.TryResolve(new IdlType.Primitive(type), out var primitive).ShouldBeTrue();

        // Act
        var literal = LiteralFormatter.FormatDefault(new IdlScalarValue.Integral(BigInteger.Parse(value)), primitive);

        // Assert
        literal.ShouldBe(expected);
    }

    /// <summary>Emits typed floating literals including exponent notation and signed zero.</summary>
    [Theory]
    [InlineData("float", 12.0, "12.0F")]
    [InlineData("float", 1.25, "1.25F")]
    [InlineData("double", 1.25, "1.25")]
    [InlineData("double", 1e100, "1E+100")]
    public void FormatsFloatingDefaults(string type, double value, string expected)
    {
        // Arrange
        PrimitiveType.TryResolve(new IdlType.Primitive(type), out var primitive).ShouldBeTrue();

        // Act
        var literal = LiteralFormatter.FormatDefault(new IdlScalarValue.Floating(value), primitive);

        // Assert
        literal.ShouldBe(expected);
    }

    /// <summary>Preserves decoded character identity and escapes C++ literal delimiters.</summary>
    [Theory]
    [InlineData('a', false, "'a'")]
    [InlineData('a', true, "L'a'")]
    [InlineData('\\', false, "'\\\\'")]
    [InlineData('\'', false, "'\\\''")]
    [InlineData('\n', false, "'\\n'")]
    [InlineData('\r', false, "'\\r'")]
    [InlineData('\t', false, "'\\t'")]
    [InlineData('\0', false, "'\\0'")]
    [InlineData('\u0001', true, "L'\\x0001'")]
    public void FormatsCharacterDefaults(char value, bool wide, string expected)
    {
        // Arrange
        PrimitiveType.TryResolve(new IdlType.Primitive(wide ? "wchar" : "char"), out var primitive).ShouldBeTrue();

        // Act
        var literal = LiteralFormatter.FormatDefault(new IdlScalarValue.Character(value, wide), primitive);

        // Assert
        literal.ShouldBe(expected);
    }

    /// <summary>Maps both boolean values to C++ boolean expressions.</summary>
    [Theory]
    [InlineData(false, "false")]
    [InlineData(true, "true")]
    public void FormatsBooleanDefaults(bool value, string expected)
    {
        // Arrange
        PrimitiveType.TryResolve(new IdlType.Primitive("boolean"), out var primitive).ShouldBeTrue();

        // Act
        var literal = LiteralFormatter.FormatDefault(new IdlScalarValue.Boolean(value), primitive);

        // Assert
        literal.ShouldBe(expected);
    }

    /// <summary>Rejects a bound value category without a supported scalar representation.</summary>
    [Fact]
    public void RejectsUnsupportedDefaultCategory()
    {
        // Arrange
        PrimitiveType.TryResolve(new IdlType.Primitive("long"), out var primitive).ShouldBeTrue();

        // Act
        var format = () => LiteralFormatter.FormatDefault(new IdlScalarValue.Text("unsupported", isWide: false), primitive);

        // Assert
        format.ShouldThrow<InvalidOperationException>();
    }

    /// <summary>Rejects external member flags while accepting the scoped must-understand metadata flag.</summary>
    [Theory]
    [InlineData(false, true, false)]
    public void RejectsUnsupportedBoundFlags(bool optional, bool external, bool mustUnderstand)
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { long value; };");
        var member = new IdlMember("value", new IdlType.Primitive("long"),
            new IdlMemberMetadata(isOptional: optional, isExternal: external, isMustUnderstand: mustUnderstand));
        var declaration = CreateDeclaration(input, [member]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        files.ShouldBeEmpty();
        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSGCPP001");
    }

    /// <summary>Preserves the bound must-understand flag on the supported required scalar member.</summary>
    [Fact]
    public void PreservesMustUnderstandMemberFlag()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "@appendable struct Sample { @must_understand long value; };");
        var member = new IdlMember("value", new IdlType.Primitive("long"),
            new IdlMemberMetadata(isMustUnderstand: true));
        var declaration = CreateDeclaration(input, [member], IdlExtensibilityKind.Appendable);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        diagnostics.ShouldBeEmpty();
        files.ShouldContain(file => file.FileName.EndsWith("Plugin.cxx", StringComparison.Ordinal)
            && file.Source.Contains(".id(0)", StringComparison.Ordinal)
            && file.Source.Contains(".must_understand(true)", StringComparison.Ordinal));
    }

    /// <summary>Rejects unknown primitive categories rather than emitting an invalid C++ type.</summary>
    [Fact]
    public void RejectsUnknownBoundPrimitive()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { long value; };");
        var declaration = CreateDeclaration(input, [new IdlMember("value", new IdlType.Primitive("unknown"))]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        files.ShouldBeEmpty();
        diagnostics.ShouldContain(diagnostic => diagnostic.Message.Contains("unknown", StringComparison.Ordinal));
    }

    /// <summary>Rejects unsupported bound defaults before rendering any output.</summary>
    [Fact]
    public void RejectsUnformattableBoundDefault()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { long value; };");
        var metadata = new IdlMemberMetadata(valueMetadata: new IdlMemberValueMetadata(new IdlScalarValue.Text("bad", false)));
        var declaration = CreateDeclaration(input, [new IdlMember("value", new IdlType.Primitive("long"), metadata)]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        files.ShouldBeEmpty();
        diagnostics.ShouldContain(diagnostic => diagnostic.Message.Contains("default", StringComparison.Ordinal));
    }

    /// <summary>Escapes a bounded narrow string member default as a C++ string literal.</summary>
    [Fact]
    public void FormatsStringMemberDefaultAndEscapesControlCharacters()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { string<32> value; };");
        var stringType = new IdlType.StringType(isWide: false, bound: 32, isBounded: true);
        var value = new IdlScalarValue.Text("quote \" slash \\\n\r\t\0\u0001", isWide: false);
        var metadata = new IdlMemberMetadata(valueMetadata: new IdlMemberValueMetadata(value));
        var declaration = CreateDeclaration(input, [new IdlMember("value", stringType, metadata)]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        diagnostics.ShouldBeEmpty();
        files.Single(file => file.FileName == "contract.hpp").Source.ShouldContain(
            """::std::string value = "quote \" slash \\\n\r\t\000\001";""");
    }

    /// <summary>Escapes supplementary Unicode scalars in wide string defaults.</summary>
    [Fact]
    public void FormatsSupplementaryUnicodeWideStringDefault()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { wstring<32> value; };");
        var stringType = new IdlType.StringType(isWide: true, bound: 32, isBounded: true);
        var value = new IdlScalarValue.Text("A😀", isWide: true);
        var metadata = new IdlMemberMetadata(valueMetadata: new IdlMemberValueMetadata(value));
        var declaration = CreateDeclaration(input, [new IdlMember("value", stringType, metadata)]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        diagnostics.ShouldBeEmpty();
        files.Single(file => file.FileName == "contract.hpp").Source.ShouldContain(
            "::std::wstring value = L\"A\\U0001F600\";");
    }

    /// <summary>Uses member ordering for IDs when the supplied model has not assigned them.</summary>
    [Fact]
    public void ProjectsUnassignedMemberIdentity()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { long first; boolean second; };");
        var declaration = CreateDeclaration(input,
            [new IdlMember("first", new IdlType.Primitive("long")), new IdlMember("second", new IdlType.Primitive("boolean"))]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var plan = GenerationPlanner.Create([declaration], input, diagnostics);

        // Assert
        plan.ShouldNotBeNull();
        plan.Structs.ShouldHaveSingleItem().Members.Select(member => member.MemberId).ShouldBe([0, 1]);
        diagnostics.ShouldBeEmpty();
    }

    /// <summary>Represents collection elements and fixed dimensions recursively without widening parser support.</summary>
    [Fact]
    public void ComposesSequenceAndArrayMemberPlansRecursively()
    {
        // Arrange
        PrimitiveType.TryResolve(new IdlType.Primitive("long"), out var mapping).ShouldBeTrue();
        var primitive = new MemberTypePlan.Primitive(mapping, mapping.DefaultValue);

        // Act
        var nestedArray = new MemberTypePlan.Array(primitive, [2, 3]);
        var sequenceOfArrays = new MemberTypePlan.Sequence(nestedArray, 4);

        // Assert
        nestedArray.StorageType.ShouldBe(
            "::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 >");
        nestedArray.ContainsArray.ShouldBeTrue();
        nestedArray.ContainsSequence.ShouldBeFalse();
        sequenceOfArrays.StorageType.ShouldBe(
            "::omg::types::bounded_sequence< ::dds::core::array< ::dds::core::array< ::std::int32_t, 3 >, 2 >, 4 >");
        sequenceOfArrays.ContainsSequence.ShouldBeTrue();
        sequenceOfArrays.ContainsArray.ShouldBeTrue();
        sequenceOfArrays.ContainsCollection.ShouldBeTrue();
    }

    /// <summary>Maps true unbounded-support strings to the zero-bound DynamicType representation.</summary>
    [Fact]
    public void EmitsZeroBoundMetadataForUnboundedStringSupport()
    {
        // Arrange
        var input = new IdlInput("contract.idl", "struct Sample { string value; };", unboundedSupport: true);
        var stringType = new IdlType.StringType(
            isWide: false,
            bound: input.StringSize,
            isBounded: false,
            isUnbounded: input.UnboundedSupport);
        var declaration = CreateDeclaration(input, [new IdlMember("value", stringType)]);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var files = InternalCompiler.Generate([declaration], input, diagnostics);

        // Assert
        diagnostics.ShouldBeEmpty();
        files.Single(file => file.FileName == "contractPlugin.cxx").Source
            .ShouldContain("::dds::core::xtypes::StringType(RTIXCdrLong_MAX)");
    }

    private static IdlClassDeclaration CreateDeclaration(
        IdlInput input,
        IReadOnlyList<IdlMember> members,
        IdlExtensibilityKind extensibility = IdlExtensibilityKind.Extensible) =>
        new("Sample", null, members, extensibility, input,
            baseType: null, baseTypeOffset: 0, isTopic: true);
}
