using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Generation.IdlDeclarations.Tests;

/// <summary>Checks imports and shared effective bounds for unbounded typedefs.</summary>
public sealed class UnboundedTypedefSpecs
{
    /// <summary>Checks that an effective string bound always has its attribute namespace.</summary>
    [Theory]
    [InlineData("string")]
    [InlineData("wstring")]
    [InlineData("string<255>")]
    [InlineData("wstring<255>")]
    public void ImportsBoundAttributesForStringAliases(string target)
    {
        // Arrange
        var input = Input("unbounded-string-alias.idl", $$"""
            module Example { typedef {{target}} Text; };
            """);

        // Act
        var sources = CompileSources(input);

        // Assert
        var text = sources["Example.Text.g.cs"].Source;
        text.ShouldContain("using Omg.Types;");
        text.ShouldContain("[Bound(255)]");
    }

    /// <summary>Checks every sequence allocation branch and the corresponding metadata bound.</summary>
    [Theory]
    [InlineData("long", "", "Value.Initialize<int>(max: 100, absoluteMax: 100, allocateMemory);")]
    [InlineData("State", "enum State { Idle, Ready };", "Value.Initialize<State>(max: 100, absoluteMax: 100, allocateMemory);")]
    [InlineData("Child", "struct Child { long value; };", "Value.Initialize<Child, ChildUnmanaged>(max: 100, absoluteMax: 100, allocateMemory);")]
    [InlineData("Scalar", "typedef long Scalar;", "Value.Initialize<int>(max: 100, absoluteMax: 100, allocateMemory);")]
    [InlineData("ScalarChain", "typedef long Scalar; typedef Scalar ScalarChain;", "Value.Initialize<int>(max: 100, absoluteMax: 100, allocateMemory);")]
    [InlineData("string", "", "Value.Initialize(max: 100, absoluteMax: 100, maxStrLen: 255, allocateMemory);")]
    [InlineData("wstring", "", "Value.Initialize(max: 100, absoluteMax: 100, maxStrLen: 255, allocateMemory);")]
    [InlineData("string<8>", "", "Value.Initialize(max: 100, absoluteMax: 100, maxStrLen: 8, allocateMemory);")]
    [InlineData("wstring<4>", "", "Value.Initialize(max: 100, absoluteMax: 100, maxStrLen: 4, allocateMemory);")]
    public void UsesTheEffectiveSequenceBoundAcrossDocuments(string element, string declarations, string initialization)
    {
        // Arrange
        var input = Input("unbounded-sequence-alias.idl", $$"""
            module Example {
                {{declarations}}
                typedef sequence<{{element}}> Values;
                typedef Values ValuesChain;
            };
            """);

        // Act
        var sources = CompileSources(input);

        // Assert
        var managed = sources["Example.Values.g.cs"].Source;
        managed.ShouldContain("[Bound(100)]");
        managed.ShouldContain("Its maximum number of elements is <c>100</c>.");
        managed.ShouldContain("For an unbounded IDL sequence");
        sources["Example.Implementation.ValuesUnmanaged.g.cs"].Source.ShouldContain(initialization);
        sources["Example.Implementation.ValuesPlugin.g.cs"].Source.ShouldContain(", 100));");
        sources["Example.ValuesChain.g.cs"].Source.ShouldContain("public Values Value");
    }

    /// <summary>Checks that explicit sequence bounds are not replaced by the fallback.</summary>
    [Theory]
    [InlineData(1)]
    [InlineData(4)]
    [InlineData(100)]
    [InlineData(255)]
    public void PreservesExplicitSequenceBounds(int bound)
    {
        // Arrange
        var input = Input("bounded-sequence-alias.idl", $$"""
            module Example { typedef sequence<long, {{bound}}> Values; };
            """);

        // Act
        var sources = CompileSources(input);

        // Assert
        sources["Example.Values.g.cs"].Source.ShouldContain($"[Bound({bound})]");
        sources["Example.Values.g.cs"].Source.ShouldNotContain("For an unbounded IDL sequence");
        sources["Example.Implementation.ValuesUnmanaged.g.cs"].Source.ShouldContain($"max: {bound}, absoluteMax: {bound}");
        sources["Example.Implementation.ValuesPlugin.g.cs"].Source.ShouldContain($", {bound}));");
    }
}
