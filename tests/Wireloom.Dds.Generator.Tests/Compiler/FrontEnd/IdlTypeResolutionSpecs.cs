using System.Linq;
using Wireloom;
using Shouldly;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlTypeResolutionSpecs
{
    [Fact]
    [Trait("Corpus", "C009")]
    public void ResolvesPrimitiveAliasChainsToUnderlyingValuesAndDistinctRuntimeMetadata()
    {
        var output = CompilerTestSupport.Compile(
            CompilerTestSupport.Input(
                "C009-03-enums-aliases.idl",
                """
                module P03PrimitiveAlias {
                    typedef long Scalar;
                    typedef Scalar ScalarAlias;
                    typedef ScalarAlias ScalarAlias2;
                    struct Sample { ScalarAlias2 value; };
                };
                """
            )
        );

        output.ShouldContain("public int Value { get; set; }");
        output.ShouldNotContain("public Scalar Value { get; set; }");
        output.ShouldNotContain("public ScalarAlias Value { get; set; }");
        output.ShouldContain("public int value { get; set; }");
        output.ShouldContain("CreateAliasWithAccessInfo<ScalarAlias2Unmanaged>");
        output.ShouldContain("\"ScalarAlias2\"");
        output.ShouldContain("dtf.GetPrimitiveType<int>()");
        output.ShouldNotContain("@int");
        output.ShouldContain("ScalarAlias2Support.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C009")]
    public void ResolvesEnumAliasChainsToUnderlyingMembersAndDistinctRuntimeMetadata()
    {
        var output = CompilerTestSupport.Compile(
            CompilerTestSupport.Input(
                "C009-03-enums-aliases.idl",
                """
                module P03EnumAlias {
                    enum Color { RED, GREEN, BLUE };
                    typedef Color ColorAlias;
                    typedef ColorAlias ColorAlias2;
                    struct Sample { ColorAlias2 value; };
                };
                """
            )
        );

        output.ShouldContain("public Color Value { get; set; }");
        output.ShouldContain("public Color value { get; set; }");
        output.ShouldContain("CreateAliasWithAccessInfo<ColorAlias2Unmanaged>");
        output.ShouldContain("ColorAlias2Support.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C012")]
    public void ResolvesAggregateAliasChainsToUnderlyingMembersAndDistinctRuntimeMetadata()
    {
        var output = CompilerTestSupport.Compile(
            CompilerTestSupport.Input(
                "C012-03-alias-aggregate.idl",
                """
                module P03AggregateAlias {
                    struct Point { long x; long y; };
                    typedef Point PointAlias;
                    typedef PointAlias PointAlias2;
                    struct Sample { PointAlias2 point; };
                };
                """
            )
        );

        output.ShouldContain("public Point Value { get; set; } = null!;");
        output.ShouldContain("public Point point { get; set; }");
        output.ShouldContain("CreateAliasWithAccessInfo<PointAlias2Unmanaged>");
        output.ShouldContain("PointAlias2Support.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C013")]
    public void PreservesElementIdentityForNestedCollectionAliases()
    {
        var output = CompilerTestSupport.Compile(
            CompilerTestSupport.Input(
                "C013-03-alias-collections.idl",
                """
                module P03NestedCollectionAlias {
                    typedef sequence<long, 3> LongSequence;
                    typedef sequence<LongSequence, 2> NestedSequence;
                    struct Sample { NestedSequence values; };
                };
                """
            )
        );

        output.ShouldContain("public ISequence<LongSequence> Value { get; }");
        output.ShouldContain("CreateSequenceWithAccessInfo(dtf,");
        output.ShouldContain("LongSequenceSupport.Instance.GetDynamicTypeInternal(isPublic)");
        output.ShouldContain("NestedSequenceSupport.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C010")]
    [Trait("Corpus", "C011")]
    public void EmitsExplicitAndValuePrefixedEnumValues()
    {
        var prefixDocuments = IdlCompiler.CompileSources([
            CompilerTestSupport.Input("prefix.idl", "module P03 { enum Annotated { @value(7) FIRST, SECOND }; };")
        ], TestContext.Current.CancellationToken);
        var prefixEnum = prefixDocuments["P03.Annotated.g.cs"].Source;
        var prefixPlugin = prefixDocuments["P03.Implementation.AnnotatedPlugin.g.cs"].Source;
        prefixEnum.ShouldContain("FIRST = 7");
        prefixEnum.ShouldContain("SECOND,");
        prefixEnum.ShouldNotContain("SECOND = 8");
        prefixPlugin.ShouldContain("new EnumMember(\"FIRST\", 7)");
        prefixPlugin.ShouldContain("new AnnotationParameterValue { EnumValue = 7 },");

        var explicitDocuments = IdlCompiler.CompileSources([
            CompilerTestSupport.Input("explicit.idl", "module P03 { enum Explicit { NEGATIVE = -2, ZERO = 0, GAP = 7, NEXT }; };")
        ], TestContext.Current.CancellationToken);
        var explicitEnum = explicitDocuments["P03.Explicit.g.cs"].Source;
        explicitEnum.ShouldContain("NEGATIVE = -2");
        explicitEnum.ShouldContain("ZERO = 0");
        explicitEnum.ShouldContain("GAP = 7");
        explicitEnum.ShouldContain("NEXT,");
        explicitEnum.ShouldNotContain("NEXT = 8");
    }
}
