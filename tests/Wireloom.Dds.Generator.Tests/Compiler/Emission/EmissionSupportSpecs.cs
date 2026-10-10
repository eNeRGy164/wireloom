using Wireloom.Compiler.Emission;
using Wireloom.Compiler.Emission.Model;

namespace Wireloom.Compiler.Emission.Tests;

public sealed class EmissionSupportSpecs
{
    [Fact]
    public void UsesSimpleNameForUnmanagedTypeInTheCurrentImplementationNamespace()
    {
        // Arrange
        const string typeName = "Example.Payload";
        const string currentNamespace = "Example.Implementation";

        // Act
        var unmanagedType = EmissionSupport.GetUnmanagedType(typeName, currentNamespace);

        // Assert
        unmanagedType.ShouldBe("PayloadUnmanaged");
    }

    [Fact]
    public void KeepsExternalUnmanagedTypesFullyQualified()
    {
        // Arrange
        const string typeName = "Other.Payload";
        const string currentNamespace = "Example.Implementation";

        // Act
        var unmanagedType = EmissionSupport.GetUnmanagedType(typeName, currentNamespace);

        // Assert
        unmanagedType.ShouldBe("global::Other.Implementation.PayloadUnmanaged");
    }

    [Fact]
    public void RequiresOnlyTheManagedNamespacesUsedByTheEmissionPlan()
    {
        // Arrange
        var primitive = new PrimitiveEmissionType("short", "short");
        var sequence = new SequenceEmissionType(primitive, 4, "ISequence<short>");
        var sequenceAlias = new AliasEmissionType("Example.SequenceAlias", sequence, "SequenceAlias");
        var longDoubleSequence = new SequenceEmissionType(new PrimitiveEmissionType("long double", "LongDouble"), 4, "ISequence<LongDouble>");
        var longDoubleSequenceAlias = new AliasEmissionType("Example.LongDoubleSequenceAlias", longDoubleSequence, "LongDoubleSequenceAlias");

        // Act
        var primitiveUsings = EmissionSupport.GetManagedTypeUsings([primitive], usesOmgTypes: false);
        var sequenceUsings = EmissionSupport.GetManagedTypeUsings([sequence], usesOmgTypes: false);
        var sequenceAliasUsings = EmissionSupport.GetManagedTypeUsings([sequenceAlias], usesOmgTypes: false);
        var longDoubleSequenceAliasUsings = EmissionSupport.GetManagedTypeUsings([longDoubleSequenceAlias], usesOmgTypes: false);

        // Assert
        primitiveUsings.ShouldBeEmpty();
        sequenceUsings.ShouldBe(["Omg.Types", "Rti.Types"]);
        sequenceAliasUsings.ShouldBeEmpty();
        longDoubleSequenceAliasUsings.ShouldBeEmpty();
    }

    [Fact]
    public void RequiresNativeStorageNamespacesOnlyForNativeRuntimeTypes()
    {
        // Arrange
        var primitive = new PrimitiveEmissionType("short", "short");
        var stringType = new StringEmissionType(isWide: false, bound: 32, isBounded: true);
        var sequence = new SequenceEmissionType(primitive, 4, "Sequence<short>");
        var sequenceAlias = new AliasEmissionType("Example.SequenceAlias", sequence, "SequenceAlias");
        var sequenceOfLongDouble = new SequenceEmissionType(new PrimitiveEmissionType("long double", "LongDouble"), 4, "Sequence<LongDouble>");
        var longDoubleSequenceAlias = new AliasEmissionType("Example.LongDoubleSequenceAlias", sequenceOfLongDouble, "LongDoubleSequenceAlias");

        // Act
        var primitiveUsings = EmissionSupport.GetUnmanagedTypeUsings([primitive]);
        var stringUsings = EmissionSupport.GetUnmanagedTypeUsings([stringType]);
        var sequenceUsings = EmissionSupport.GetUnmanagedTypeUsings([sequence]);
        var sequenceAliasUsings = EmissionSupport.GetUnmanagedTypeUsings([sequenceAlias]);
        var sequenceAliasEmitterUsings = EmissionSupport.GetUnmanagedTypeUsings([primitive], usesSequence: true);
        var longDoubleUsings = EmissionSupport.GetUnmanagedTypeUsings([sequenceOfLongDouble]);
        var longDoubleSequenceAliasUsings = EmissionSupport.GetUnmanagedTypeUsings([longDoubleSequenceAlias]);

        // Assert
        primitiveUsings.ShouldBe(["Rti.Dds.NativeInterface.TypePlugin"]);
        stringUsings.ShouldBe(["Rti.Dds.NativeInterface.TypePlugin"]);
        sequenceUsings.ShouldBe(["Rti.Dds.NativeInterface.TypePlugin"]);
        sequenceAliasUsings.ShouldBe(["Rti.Dds.NativeInterface.TypePlugin"]);
        sequenceAliasEmitterUsings.ShouldBe(["Rti.Types", "Rti.Dds.NativeInterface.TypePlugin"]);
        longDoubleUsings.ShouldBe(["Rti.Types", "Rti.Dds.NativeInterface.TypePlugin"]);
        longDoubleSequenceAliasUsings.ShouldBe(["Rti.Dds.NativeInterface.TypePlugin"]);
    }

    [Fact]
    public void RequiresPluginNamespacesOnlyForEmittedMetadataAndPrimitiveTypes()
    {
        // Arrange
        var shortType = new PrimitiveEmissionType("short", "short");
        var longDoubleType = new PrimitiveEmissionType("long double", "LongDouble");

        // Act
        var plainUsings = EmissionSupport.GetPluginUsings([shortType], usesExtensibility: false, usesAnnotations: false);
        var metadataUsings = EmissionSupport.GetPluginUsings([longDoubleType], usesExtensibility: true, usesAnnotations: true);

        // Assert
        plainUsings.ShouldBe(["Rti.Dds.Core", "Rti.Dds.NativeInterface.TypePlugin", "Rti.Types.Dynamic"]);
        metadataUsings.ShouldBe(["Rti.Dds.Core", "Rti.Dds.NativeInterface.TypePlugin", "Rti.Types.Dynamic", "Omg.Types", "Omg.Types.Dynamic", "Rti.Types"]);
    }
}
