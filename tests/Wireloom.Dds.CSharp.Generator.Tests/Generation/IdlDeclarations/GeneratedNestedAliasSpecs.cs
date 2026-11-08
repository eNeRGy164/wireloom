using static Wireloom.Compiler.Tests.CompilerTestSupport;

namespace Wireloom.Generation.IdlDeclarations.Tests;

public sealed class GeneratedNestedAliasSpecs
{
    [Fact]
    public void CollectionAliasChainsProjectToTheCollectionDefiningTypeAtUseSites()
    {
        // Arrange
        var input = Input("collection-alias-members.idl",
            """
            module AliasMembers {
                typedef sequence<long, 3> Numbers;
                typedef Numbers Chain;
                typedef Chain LongerChain;
                typedef long Vector[2];
                typedef Vector VectorChain;
                typedef sequence<LongerChain, 2> Nested;
                typedef LongerChain Rows[2];
                struct Sample {
                    Numbers direct;
                    LongerChain chained;
                    VectorChain arrayAlias;
                    @optional LongerChain optionalChain;
                    Nested nested;
                    Rows rows;
                };
                union Choice switch(long) { case 0: LongerChain numbers; case 1: VectorChain vector; };
                module Inner { struct Holder { LongerChain values; }; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var sample = documents["AliasMembers.Sample.g.cs"].Source;
        sample.ShouldContain("public Numbers direct { get; set; } = new();");
        sample.ShouldContain("public Numbers chained { get; set; } = new();");
        sample.ShouldContain("public Vector arrayAlias { get; set; } = new();");
        sample.ShouldContain("public Numbers? optionalChain { get; set; }");
        sample.ShouldContain("chained = new Numbers(other.chained);");
        sample.ShouldContain("public Nested nested { get; set; } = new();");
        sample.ShouldContain("public Rows rows { get; set; } = new();");
        documents["AliasMembers.Nested.g.cs"].Source.ShouldContain("public ISequence<Numbers> Value { get; }");
        documents["AliasMembers.Rows.g.cs"].Source.ShouldContain("public Numbers[] Value { get; set; }");
        documents["AliasMembers.Choice.g.cs"].Source.ShouldContain("public Numbers numbers");
        documents["AliasMembers.Choice.g.cs"].Source.ShouldContain("public Vector vector");
        documents["AliasMembers.Inner.Holder.g.cs"].Source.ShouldContain("public global::AliasMembers.Numbers values");
        documents["AliasMembers.Chain.g.cs"].Source.ShouldContain("public Numbers Value { get; set; }");
        documents["AliasMembers.LongerChain.g.cs"].Source.ShouldContain("public Numbers Value { get; set; }");
        documents["AliasMembers.Implementation.SamplePlugin.g.cs"].Source.ShouldContain("LongerChainSupport.Instance.GetDynamicTypeInternal(isPublic)");

        var native = documents["AliasMembers.Implementation.NestedUnmanaged.g.cs"].Source;
        native.ShouldContain("FromNative<Numbers, NumbersUnmanaged>");
        native.ShouldNotContain("LongerChainUnmanaged");
    }

    [Fact]
    public void AggregateAliasCopiesCloneTheirMutableValues()
    {
        // Arrange
        var input = Input("aggregate-alias-copy.idl",
            "module AggregateAliasCopy { struct Point { long x; }; typedef Point PointAlias; typedef PointAlias PointAlias2; struct Sample { PointAlias2 point; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var alias = documents["AggregateAliasCopy.PointAlias2.g.cs"].Source;
        alias.ShouldContain("Value = new Point(other.Value);");

        var sample = documents["AggregateAliasCopy.Sample.g.cs"].Source;
        sample.ShouldContain("public Point point { get; set; } = new();");
        sample.ShouldContain("point = new Point(other.point);");
    }

    [Fact]
    public void AggregateAliasesInRawCollectionsUseUnderlyingAggregateValues()
    {
        // Arrange
        var input = Input("aggregate-alias-collections.idl",
            """
            module AggregateAliasCollections {
                struct Point { long x; };
                typedef Point PointAlias;
                typedef PointAlias PointAlias2;
                union Choice switch (long) {
                    case 0: long number;
                };
                typedef Choice ChoiceAlias;
                typedef ChoiceAlias ChoiceAlias2;
                struct Sample {
                    PointAlias points[2];
                    sequence<PointAlias2, 3> pointSequence;
                    ChoiceAlias choices[2];
                    sequence<ChoiceAlias2, 3> choiceSequence;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var sample = documents["AggregateAliasCollections.Sample.g.cs"].Source;
        sample.ShouldContain("public Point[] points { get; set; } = null!;");
        sample.ShouldContain("points = new Point[2];");
        sample.ShouldContain("points[dimension0] = new Point();");
        sample.ShouldContain("public ISequence<Point> pointSequence { get; }");
        sample.ShouldContain("pointSequence = new Sequence<Point>();");
        sample.ShouldContain("points[dimension0] = new Point(other.points[dimension0]);");
        sample.ShouldContain("element => new Point(element)");
        sample.ShouldContain("public Choice[] choices { get; set; } = null!;");
        sample.ShouldContain("choices[dimension0] = new Choice();");
        sample.ShouldContain("public ISequence<Choice> choiceSequence { get; }");
        sample.ShouldContain("choiceSequence = new Sequence<Choice>();");
        sample.ShouldContain("choices[dimension0] = new Choice(other.choices[dimension0]);");
        sample.ShouldContain("element => new Choice(element)");

        var native = documents["AggregateAliasCollections.Implementation.SampleUnmanaged.g.cs"].Source;
        native.ShouldContain("points.FromNative<Point, PointUnmanaged>(sample.points, keysOnly: false, dimension: 2);");
        native.ShouldContain("pointSequence.FromNative<Point, PointUnmanaged>(sample.pointSequence);");
        native.ShouldContain("points.ToNative<Point, PointUnmanaged>(sample.points, keysOnly: false, dimension: 2);");
        native.ShouldContain("pointSequence.ToNative<Point, PointUnmanaged>(sample.pointSequence);");
        native.ShouldContain("choices.FromNative<Choice, ChoiceUnmanaged>(sample.choices, keysOnly: false, dimension: 2);");
        native.ShouldContain("choiceSequence.FromNative<Choice, ChoiceUnmanaged>(sample.choiceSequence);");
        native.ShouldContain("choices.ToNative<Choice, ChoiceUnmanaged>(sample.choices, keysOnly: false, dimension: 2);");
        native.ShouldContain("choiceSequence.ToNative<Choice, ChoiceUnmanaged>(sample.choiceSequence);");
    }

    [Fact]
    public void SupportsGuardedNestedAppendableTypesWithBoundedStringAliases()
    {
        // Arrange
        var input = Input("nested-aliases.idl",
            """
            #ifndef NESTED_ALIASES_IDL
            #define NESTED_ALIASES_IDL

            @default_nested
            module Envelope {
                module Metadata {
                    typedef string<9> Correlation;
                    typedef string<17> Producer;

                    @appendable
                    struct Context {
                        Correlation correlation;
                        @optional Producer producer;
                    };

                    @appendable
                    struct Base {
                        Correlation state;
                    };

                    @appendable
                    struct Derived : Base {
                        Context traceContext;
                    };
                };
            };

            #endif
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var context = documents["Envelope.Metadata.Context.g.cs"].Source;
        context.ShouldContain("It is marked as <c>extensible</c>.");
        context.ShouldContain("Its maximum length is <c>9</c> UTF-8 bytes.");
        context.ShouldContain("[Bound(9)]\n    public string correlation");
        context.ShouldContain("Its maximum length is <c>17</c> UTF-8 bytes.");
        context.ShouldContain("[Optional]\n    [Bound(17)]\n    public string? producer");

        var correlation = documents["Envelope.Metadata.Correlation.g.cs"].Source;
        correlation.ShouldContain("Its IDL bound is <c>9</c> UTF-8 bytes.");
        correlation.ShouldContain("[Bound(9)]");
        correlation.ShouldContain("if (other is not null)\n        {\n            Value = other.Value;\n        }");

        var derived = documents["Envelope.Metadata.Derived.g.cs"].Source;
        derived.ShouldContain("public partial class Derived : Base, global::System.IEquatable<Derived>");
        derived.ShouldContain("public Context traceContext { get; set; } = new();");
        derived.ShouldNotContain("public Derived()\n    {\n        traceContext = new Context();\n    }");
        derived.ShouldContain("public Derived(string state, Context traceContext) : base(state)");

        var producerPlugin = documents["Envelope.Metadata.Implementation.ProducerPlugin.g.cs"].Source;
        producerPlugin.ShouldContain("dtf.CreateString(17)");
    }

    [Fact]
    public void OrdersDerivedDestroyLikeRtiAroundTheOptionalGuard()
    {
        // Arrange
        var input = Input("derived-destroy.idl",
            """
            module DerivedDestroy {
                struct Base { long baseValue; };
                struct Context { string correlation; };
                struct Derived : Base { string state; Context traceContext; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["DerivedDestroy.Implementation.DerivedUnmanaged.g.cs"].Source;
        native.ShouldContain("parent.Destroy(optionalsOnly);\n        traceContext.Destroy(optionalsOnly);\n\n        if (optionalsOnly)");
        native.ShouldContain("return;\n        }\n\n        state.Destroy();");
    }
}
