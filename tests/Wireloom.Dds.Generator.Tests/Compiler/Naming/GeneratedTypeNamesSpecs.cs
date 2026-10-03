using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Naming.Tests;

public sealed class GeneratedTypeNamesSpecs
{
    [Fact]
    public void NamesShareEscapedTypesNamespacesAndHintIdentities()
    {
        // Arrange
        var names = IdlNaming.CreateGeneratedTypeNames("Example.Module", "class");

        // Assert
        names.ManagedTypeName.ShouldBe("@class");
        names.SupportTypeName.ShouldBe("classSupport");
        names.PluginTypeName.ShouldBe("classPlugin");
        names.UnmanagedTypeName.ShouldBe("classUnmanaged");
        names.ImplementationNamespace.ShouldBe("Example.Module.Implementation");
        names.Managed.HintName.ShouldBe("Example.Module.@class.g.cs");
        names.Support.HintName.ShouldBe("Example.Module.classSupport.g.cs");
        names.Plugin.HintName.ShouldBe("Example.Module.Implementation.classPlugin.g.cs");
        names.Unmanaged.HintName.ShouldBe("Example.Module.Implementation.classUnmanaged.g.cs");
        names.SupportIdentity.ShouldBe("Example.Module.classSupport");
        names.PluginIdentity.ShouldBe("Example.Module.Implementation.classPlugin");
        names.UnmanagedIdentity.ShouldBe("Example.Module.Implementation.classUnmanaged");
    }

    [Fact]
    public void ValidatorUsesTheSameCompanionIdentitiesForCollisionDetection()
    {
        // Arrange
        var validator = new IdlSemanticValidator(new IdlSymbolTable());
        var input = new IdlInput("names.idl", string.Empty);

        // Act
        validator.EnsureGeneratedCompanionNames(input, 0, "Sample", "Example", includeUnmanaged: true);

        // Assert
        var exception = Should.Throw<IdlException>(() =>
            validator.EnsureGeneratedCompanionNames(input, 0, "Sample", "Example", includeUnmanaged: true));
        exception.Message.ShouldContain("collides with a generated companion type");
    }

    [Fact]
    public void EscapesKeywordNamespacesInGeneratedIdentities()
    {
        // Arrange
        var names = IdlNaming.CreateGeneratedTypeNames("class", "Sample");

        // Assert
        names.ImplementationNamespace.ShouldBe("@class.Implementation");
        names.SupportIdentity.ShouldBe("@class.SampleSupport");
        names.PluginIdentity.ShouldBe("@class.Implementation.SamplePlugin");
        names.UnmanagedIdentity.ShouldBe("@class.Implementation.SampleUnmanaged");
        names.RuntimeTypeName.ShouldBe("@class.Sample");
    }
}
