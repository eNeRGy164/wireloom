using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Dds.Generator.Tests;

namespace Wireloom.Generation.IdlDeclarations.Tests;

public sealed class GeneratedNestedAliasSpecs
{
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
        sample.ShouldContain("public Point point { get; set; } = new Point();");
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
                        Context context;
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
        derived.ShouldContain("public Context context { get; set; } = new Context();");
        derived.ShouldNotContain("public Derived()\n    {\n        context = new Context();\n    }");
        derived.ShouldContain("public Derived(string state, Context context) : base(state)");

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
