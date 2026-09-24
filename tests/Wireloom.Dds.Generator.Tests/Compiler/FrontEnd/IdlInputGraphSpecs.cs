using System;
using Wireloom;
using Shouldly;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlInputGraphSpecs
{
    [Fact]
    public void EmitsSharedIncludesOnceAndInDeterministicOrder()
    {
        var common = CompilerTestSupport.Input("common.idl", "struct Shared { int32 count; };", generate: false);
        var first = CompilerTestSupport.Input("first.idl", "#include \"common.idl\"\nstruct First { string<256> msg; };");
        var second = CompilerTestSupport.Input("second.idl", "#include \"common.idl\"\nstruct Second { boolean active; };");

        var output = CompilerTestSupport.Compile(first, second, common);

        output.Split("public partial class Shared", StringSplitOptions.None).Length.ShouldBe(2);
        output.ShouldContain("Bound(256)");
        output.IndexOf("/// Gets or sets the <c>msg</c> member.", StringComparison.Ordinal)
            .ShouldBeLessThan(output.IndexOf("[Bound(256)]", StringComparison.Ordinal));
        output.ShouldBe(CompilerTestSupport.Compile(common, second, first));
    }

    [Fact]
    public void RecompilesWhenAnIncludedFileChanges()
    {
        var first = CompilerTestSupport.Input("first.idl", "#include \"common.idl\"\nstruct First { string<256> msg; };");
        var second = CompilerTestSupport.Input("second.idl", "#include \"common.idl\"\nstruct Second { boolean active; };");

        var output = CompilerTestSupport.Compile(
            first,
            second,
            CompilerTestSupport.Input("common.idl", "struct Shared { uint32 changed; };", generate: false));

        output.ShouldContain("uint changed");
        output.ShouldNotContain("int count");
    }

    [Fact]
    [Trait("Corpus", "C060")]
    public void ReportsMissingIncludes()
    {
        var missing = CompilerTestSupport.Input("missing.idl", "#include \"common.idl\"");

        var exception = Should.Throw<IdlException>(() => CompilerTestSupport.Compile(missing));

        exception.Message.ShouldContain("Could not resolve included IDL");
    }

    [Fact]
    [Trait("Corpus", "C077")]
    [Trait("Corpus", "C078")]
    [Trait("Corpus", "C079")]
    public void ReportsCyclicIncludes()
    {
        var first = CompilerTestSupport.Input("a.idl", "#include \"b.idl\"");
        var second = CompilerTestSupport.Input("b.idl", "#include \"a.idl\"", generate: false);

        var exception = Should.Throw<IdlException>(() => CompilerTestSupport.Compile(first, second));

        exception.Message.ShouldContain("Cyclic include");
    }
}
