using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlInputGraphSpecs
{
    [Fact]
    public void EmitsSharedIncludesOnceAndInDeterministicOrder()
    {
        // Arrange
        var common = Input("common.idl", "struct Shared { int32 count; };", generate: false);
        var first = Input("first.idl", "#include \"common.idl\"\nstruct First { string<256> msg; };");
        var second = Input("second.idl", "#include \"common.idl\"\nstruct Second { boolean active; };");

        // Act
        var output = Compile(first, second, common);
        var reorderedOutput = Compile(common, second, first);

        // Assert
        var sharedClassOccurrences = output.Split("public partial class Shared", StringSplitOptions.None).Length;
        sharedClassOccurrences.ShouldBe(2);
        output.ShouldContain("Bound(256)");
        output.IndexOf("/// Gets or sets the <c>msg</c> member.", StringComparison.Ordinal)
            .ShouldBeLessThan(output.IndexOf("[Bound(256)]", StringComparison.Ordinal));
        output.ShouldBe(reorderedOutput);
    }

    [Fact]
    public void RecompilesWhenAnIncludedFileChanges()
    {
        // Arrange
        var first = Input("first.idl", "#include \"common.idl\"\nstruct First { string<256> msg; };");
        var second = Input("second.idl", "#include \"common.idl\"\nstruct Second { boolean active; };");
        var common = Input("common.idl", "struct Shared { uint32 changed; };", generate: false);

        // Act
        var output = Compile(first, second, common);

        // Assert
        output.ShouldContain("uint changed");
        output.ShouldNotContain("int count");
    }

    [Fact]
    [Trait("Corpus", "C060")]
    public void ReportsMissingIncludes()
    {
        // Arrange
        var missing = Input("missing.idl", "#include \"common.idl\"");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(missing));

        // Assert
        exception.Message.ShouldContain("Could not resolve included IDL");
    }

    [Fact]
    [Trait("Corpus", "C077")]
    [Trait("Corpus", "C078")]
    [Trait("Corpus", "C079")]
    public void ReportsCyclicIncludes()
    {
        // Arrange
        var first = Input("a.idl", "#include \"b.idl\"");
        var second = Input("b.idl", "#include \"a.idl\"", generate: false);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(first, second));

        // Assert
        exception.Message.ShouldContain("Cyclic include");
    }
}
