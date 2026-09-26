using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Dds.Generator.Tests;

public sealed class IdlTypeResolutionSpecs
{
    [Fact]
    [Trait("Corpus", "C009")]
    public void ResolvesPrimitiveAliasChainsToUnderlyingValuesAndDistinctRuntimeMetadata()
    {
        // Arrange
        var input = Input("03-enums-aliases.idl",
            """
            module P03PrimitiveAlias {
                typedef long Scalar;
                typedef Scalar ScalarAlias;
                typedef ScalarAlias ScalarAlias2;
                struct Sample { ScalarAlias2 value; };
            };
            """);

        // Act
        var output = Compile(input);

        // Assert
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
        // Arrange
        var input = Input("03-enums-aliases.idl",
            """
            module P03EnumAlias {
                enum Color { RED, GREEN, BLUE };
                typedef Color ColorAlias;
                typedef ColorAlias ColorAlias2;
                struct Sample { ColorAlias2 value; };
            };
            """);

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public Color Value { get; set; }");
        output.ShouldContain("public Color value { get; set; }");
        output.ShouldContain("CreateAliasWithAccessInfo<ColorAlias2Unmanaged>");
        output.ShouldContain("ColorAlias2Support.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C012")]
    public void ResolvesAggregateAliasChainsToUnderlyingMembersAndDistinctRuntimeMetadata()
    {
        // Arrange
        var input = Input("03-alias-aggregate.idl",
            """
            module P03AggregateAlias {
                struct Point { long x; long y; };
                typedef Point PointAlias;
                typedef PointAlias PointAlias2;
                struct Sample { PointAlias2 point; };
            };
            """);

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("public Point Value { get; set; } = null!;");
        output.ShouldContain("public Point point { get; set; }");
        output.ShouldContain("CreateAliasWithAccessInfo<PointAlias2Unmanaged>");
        output.ShouldContain("PointAlias2Support.Instance.GetDynamicTypeInternal(isPublic)");
    }

    [Fact]
    [Trait("Corpus", "C013")]
    public void PreservesElementIdentityForNestedCollectionAliases()
    {
        // Arrange
        var input = Input("03-alias-collections.idl",
            """
            module P03NestedCollectionAlias {
                typedef sequence<long, 3> LongSequence;
                typedef sequence<LongSequence, 2> NestedSequence;
                struct Sample { NestedSequence values; };
            };
            """);

        // Act
        var output = Compile(input);

        // Assert
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
        // Arrange
        var prefixInput = Input("prefix.idl",
            "module P03 { enum Annotated { @value(7) FIRST, SECOND }; };");

        var explicitInput = Input("explicit.idl",
            "module P03 { enum Explicit { NEGATIVE = -2, ZERO = 0, GAP = 7, NEXT }; };");

        // Act
        var prefixDocuments = CompileSources(prefixInput);
        var explicitDocuments = CompileSources(explicitInput);

        // Assert
        var prefixEnum = prefixDocuments["P03.Annotated.g.cs"].Source;
        prefixEnum.ShouldContain("FIRST = 7");
        prefixEnum.ShouldContain("SECOND,");
        prefixEnum.ShouldNotContain("SECOND = 8");

        var prefixPlugin = prefixDocuments["P03.Implementation.AnnotatedPlugin.g.cs"].Source;
        prefixPlugin.ShouldContain("new EnumMember(\"FIRST\", 7)");
        prefixPlugin.ShouldContain("new AnnotationParameterValue { EnumValue = 7 },");

        var explicitEnum = explicitDocuments["P03.Explicit.g.cs"].Source;
        explicitEnum.ShouldContain("NEGATIVE = -2");
        explicitEnum.ShouldContain("ZERO = 0");
        explicitEnum.ShouldContain("GAP = 7");
        explicitEnum.ShouldContain("NEXT,");
        explicitEnum.ShouldNotContain("NEXT = 8");
    }

    [Fact]
    [Trait("Corpus", "C042")]
    public void PreservesDefaultLiteralForManagedAndTypeSupportDefaults()
    {
        // Arrange
        var input = Input("defaults.idl",
            """
            module Defaults {
                enum Color { GREEN, @default_literal RED, BLUE };
                struct Sample { Color color; };
            };
            """);

        // Act
        var output = Compile(input);

        // Assert
        output.ShouldContain("RED,");
        output.ShouldContain("EnumValue = 1");
        output.ShouldContain("public Color color { get; set; } = (Color)1;");
    }

    [Fact]
    public void RejectsMultipleDefaultLiterals()
    {
        // Arrange
        var input = Input("duplicate-default-literal.idl",
            "module Defaults { enum Color { @default_literal RED, @default_literal BLUE }; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("at most one @default_literal");
    }

    [Fact]
    public void ResolvesAbsoluteScopedStructBaseNames()
    {
        // Arrange
        var common = Input("common.idl",
            "module A { module B { module C { struct Header { long id; }; }; }; };",
            generate: false);

        var body = Input("body.idl",
            """
            #include "common.idl"

            module D {
                module E {
                    module F {
                        @topic
                        @appendable
                        struct Base : ::A::B::C::Header {
                            long value;
                        };
                    };
                };
            };
            """);

        // Act
        var documents = CompileSources(common, body);

        // Assert
        var baseType = documents["D.E.F.Base.g.cs"].Source;
        baseType.ShouldContain("public partial class Base : A.B.C.Header");
    }
}
