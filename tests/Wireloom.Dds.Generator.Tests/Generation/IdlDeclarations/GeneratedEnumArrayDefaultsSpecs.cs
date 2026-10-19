using Wireloom.Compiler.Emission;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Generation.IdlDeclarations.Tests;

public sealed class GeneratedEnumArrayDefaultsSpecs
{
    [Fact]
    public void EmitsDeclaredEnumArrayDefaultsForBothRtiProfiles()
    {
        // Arrange
        var input = Input(
            "enum-array-defaults.idl",
            """
            module EnumArrayDefaults {
                enum Shade { BLACK, @default_literal WHITE };
                typedef Shade Shades[2][3];
                struct Sample {
                    Shade colors[2][3];
                    Shade moreColors[2];
                    Shades shades;
                };
            };
            """);

        // Act
        var oldProfile = IdlCompiler.CompileSourcesWithDiagnostics(
            [input],
            null,
            TestContext.Current.CancellationToken,
            RtiApiProfile.Rtiddsgen431);
        var currentProfile = IdlCompiler.CompileSourcesWithDiagnostics(
            [input],
            null,
            TestContext.Current.CancellationToken,
            RtiApiProfile.Rtiddsgen470);

        // Assert
        var oldManagedType = oldProfile["EnumArrayDefaults.Sample.g.cs"].Source;
        oldManagedType.ShouldContain("colors[dimension0, dimension1] = Shade.WHITE;");
        oldManagedType.ShouldContain("moreColors[dimension0] = Shade.WHITE;");

        var oldManagedAlias = oldProfile["EnumArrayDefaults.Shades.g.cs"].Source;
        oldManagedAlias.Split("Value[dimension0, dimension1] = Shade.WHITE;", StringSplitOptions.None)
            .Length.ShouldBe(3);
        oldManagedAlias.ShouldContain("if (other is null)");

        var oldNativeType = oldProfile["EnumArrayDefaults.Implementation.SampleUnmanaged.g.cs"].Source;
        oldNativeType.ShouldContain("var __wireloom_colors_defaultValues = new Shade[2, 3];");
        oldNativeType.ShouldContain("colors.ToNative(__wireloom_colors_defaultValues, dimension: 2 * 3);");

        var oldNativeAlias = oldProfile["EnumArrayDefaults.Implementation.ShadesUnmanaged.g.cs"].Source;
        oldNativeAlias.ShouldContain("var __wireloom_defaultValues = new Shade[2, 3];");
        oldNativeAlias.ShouldContain("Value.ToNative(__wireloom_defaultValues, dimension: 2 * 3);");

        var currentNativeType = currentProfile["EnumArrayDefaults.Implementation.SampleUnmanaged.g.cs"].Source;
        currentNativeType.ShouldContain(
            "colors.InitializeEnum(dimension: 2 * 3, allocateMemory: allocateMemory, defaultValue: 1);");
        currentNativeType.ShouldNotContain("__wireloom_colors_defaultValues");

        var currentNativeAlias = currentProfile["EnumArrayDefaults.Implementation.ShadesUnmanaged.g.cs"].Source;
        currentNativeAlias.ShouldContain(
            "Value.InitializeEnum(dimension: 2 * 3, allocateMemory: allocateMemory, defaultValue: 1);");
        currentNativeAlias.ShouldNotContain("__wireloom_defaultValues");
    }

    [Fact]
    public void EnumArrayInitializationIgnoresNonArrayAndOptionalMembers()
    {
        // Arrange
        var enumType = new EnumEmissionType("Example.Shade", 1, "White");
        var scalarPlan = CreateMemberPlan("shade", enumType, isOptional: false);
        var optionalArrayPlan = CreateMemberPlan(
            "shades",
            new ArrayEmissionType(enumType, [2], "Shade[]"),
            isOptional: true);
        var writer = new GeneratedSourceWriter(null, [], "enum-array-guards.idl");

        // Act
        scalarPlan.EmitEnumArrayInitialization(writer, "shade", hasFollowingStatements: false);
        optionalArrayPlan.EmitEnumArrayInitialization(writer, "shades", hasFollowingStatements: false);

        // Assert
        writer.ToString().ShouldNotContain("shade[");
        writer.ToString().ShouldNotContain("shades[");
    }

    [Fact]
    public void NativeEnumArrayInitializationUsesSuppliedNamespaceOverride()
    {
        // Arrange
        var enumType = new EnumEmissionType("Example.Shade", 1, "White");
        var plan = CreateMemberPlan(
            "shades",
            new ArrayEmissionType(enumType, [2], "Shade[]"),
            isOptional: false,
            profile: RtiApiProfile.Rtiddsgen431,
            currentNamespace: "Other");
        var writer = new GeneratedSourceWriter(null, [], "enum-array-namespace.idl");
        var fallbackPlan = CreateMemberPlan(
            "fallbackShades",
            new ArrayEmissionType(enumType, [2], "Shade[]"),
            isOptional: false,
            profile: RtiApiProfile.Rtiddsgen431,
            currentNamespace: "Other");
        var fallbackWriter = new GeneratedSourceWriter(null, [], "enum-array-namespace.idl");

        // Act
        plan.EmitNativeInitialization(writer, namespaceOverride: "Example", nativeFieldPrefix: "native.");
        fallbackPlan.EmitNativeInitialization(fallbackWriter, nativeFieldPrefix: "native.");

        // Assert
        writer.ToString().ShouldContain(
            "native.shades.Initialize<Shade>(dimension: 2, allocateMemory);");
        writer.ToString().ShouldContain("var __wireloom_shades_defaultValues = new Shade[2];");
        writer.ToString().ShouldContain(
            "native.shades.ToNative(__wireloom_shades_defaultValues, dimension: 2);");
        fallbackWriter.ToString().ShouldContain("var __wireloom_fallbackShades_defaultValues = new Example.Shade[2];");
    }

    private static MemberEmissionPlan CreateMemberPlan(
        string name,
        EmissionTypePlan type,
        bool isOptional,
        RtiApiProfile? profile = null,
        string? currentNamespace = "Example")
    {
        var field = new IdlEmissionField(
            name,
            type,
            new EmissionMetadata(
                isKey: false,
                memberId: null,
                isOptional: isOptional,
                valueMetadata: null,
                isExternal: false,
                isMustUnderstand: false,
                memberIdHashSource: null,
                usesAutoIdHash: false));

        return new MemberEmissionPlan(field, currentNamespace, profile: profile);
    }
}
