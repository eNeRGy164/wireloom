namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratedNestedAliasSpecs
{
    [Fact]
    public void SupportsGuardedNestedAppendableTypesWithBoundedStringAliases()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input(
                "nested-aliases.idl",
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
                """
            )
        ], TestContext.Current.CancellationToken);

        var context = documents["Envelope.Metadata.Context.g.cs"].Source;
        context.ShouldContain("It is marked as <c>extensible</c>.");
        context.ShouldContain("Its maximum length is <c>9</c>.");
        context.ShouldContain("[Bound(9)]\n    public string correlation");
        context.ShouldContain("Its maximum length is <c>17</c>.");
        context.ShouldContain("[Optional]\n    [Bound(17)]\n    public string? producer");

        var correlation = documents["Envelope.Metadata.Correlation.g.cs"].Source;
        correlation.ShouldContain("Its maximum length is <c>9</c>.");
        correlation.ShouldContain("[Bound(9)]");
        correlation.ShouldContain("if (other is not null)\n        {\n            Value = other.Value;\n        }");

        var derived = documents["Envelope.Metadata.Derived.g.cs"].Source;
        derived.ShouldContain("public partial class Derived : Base, IEquatable<Derived>");
        derived.ShouldContain("public Context context { get; set; } = new Context();");
        derived.ShouldNotContain("public Derived()\n    {\n        context = new Context();\n    }");
        derived.ShouldContain("public Derived(string state, Context context) : base(state)");

        var producerPlugin = documents["Envelope.Metadata.Implementation.ProducerPlugin.g.cs"].Source;
        producerPlugin.ShouldContain("dtf.CreateString(17)");

    }

    [Fact]
    public void OrdersDerivedDestroyLikeRtiAroundTheOptionalGuard()
    {
        var documents = IdlCompiler.CompileSources([
            CompilerTestSupport.Input("derived-destroy.idl", """
                module DerivedDestroy {
                    struct Base { long baseValue; };
                    struct Context { string correlation; };
                    struct Derived : Base { string state; Context traceContext; };
                };
                """)
        ], TestContext.Current.CancellationToken);

        var native = documents["DerivedDestroy.Implementation.DerivedUnmanaged.g.cs"].Source;
        native.ShouldContain("parent.Destroy(optionalsOnly);\n        traceContext.Destroy(optionalsOnly);\n\n        if (optionalsOnly)");
        native.ShouldContain("return;\n        }\n\n        state.Destroy();");
    }
}
