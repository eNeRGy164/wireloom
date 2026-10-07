using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;
using Wireloom;
using Wireloom.Compiler.FrontEnd.Parsing;
using Wireloom.Compiler.FrontEnd.Preprocessing;
using Wireloom.Compiler.FrontEnd.Symbols;

namespace Wireloom.Compiler.FrontEnd.Semantic.Tests;

public sealed class IdlTypeResolutionSpecs
{
    [Fact]
    public void DefersSemanticValidationUntilTheValidationPhase()
    {
        // Arrange
        var input = Input("deferred-validation.idl", "struct Broken { long Destroy; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);

        // Act
        parser.Parse(input.Text, input, 0, currentNamespace: null);

        // Assert
        Should.NotThrow(parser.Bind);
        Should.Throw<IdlException>(() => parser.Validate(strict: false)).Message.ShouldContain("collides with a generated member");
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void CapturesDeferredValidationOriginsBeforeParsingAnotherInput()
    {
        // Arrange
        var first = Input("first.idl", "struct Duplicate { long value; };");
        var second = Input("second.idl", "struct Duplicate { long value; };");
        var third = Input("third.idl", "struct Other { long value; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        IReadOnlyList<SourceOriginSpan> firstOrigins = [new SourceOriginSpan(0, first.Text.Length, 100, first.Text.Length)];
        IReadOnlyList<SourceOriginSpan> secondOrigins = [new SourceOriginSpan(0, second.Text.Length, 200, second.Text.Length)];
        IReadOnlyList<SourceOriginSpan> thirdOrigins = [new SourceOriginSpan(0, third.Text.Length, 300, third.Text.Length)];

        parser.Parse(first.Text, first, 0, currentNamespace: null, firstOrigins);
        parser.Parse(second.Text, second, 0, currentNamespace: null, secondOrigins);
        parser.Parse(third.Text, third, 0, currentNamespace: null, thirdOrigins);
        parser.Bind();

        // Act
        var exception = Should.Throw<IdlException>(() => parser.Validate(strict: false));

        // Assert
        exception.Input.ShouldBe(first);
        exception.Offset.ShouldBe(100);
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void CapturesDeferredMemberMetadataOriginsBeforeParsingAnotherInput()
    {
        // Arrange
        var first = Input(
            "first.idl",
            "struct Holder { @max(1) @default(2) Alias value; }; typedef long Alias;");
        var second = Input("second.idl", "struct Other { long value; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        IReadOnlyList<SourceOriginSpan> firstOrigins = [new SourceOriginSpan(0, first.Text.Length, 100, first.Text.Length)];
        IReadOnlyList<SourceOriginSpan> secondOrigins = [new SourceOriginSpan(0, second.Text.Length, 200, second.Text.Length)];

        parser.Parse(first.Text, first, 0, currentNamespace: null, firstOrigins);
        parser.Parse(second.Text, second, 0, currentNamespace: null, secondOrigins);

        // Act
        var exception = Should.Throw<IdlException>(parser.Bind);

        // Assert
        exception.Input.ShouldBe(first);
        exception.Offset.ShouldBe(100 + first.Text.IndexOf("Alias", StringComparison.Ordinal));
    }

    [Fact]
    [Trait("Preprocessor", "PP050")]
    public void MapsCapturedOriginsAtAndOutsideTheirOutputRange()
    {
        // Arrange
        IReadOnlyList<SourceOriginSpan> origins = [new SourceOriginSpan(2, 3, 100, 3)];

        // Act
        var before = IdlParseContext.MapOffset(1, origins);
        var inside = IdlParseContext.MapOffset(3, origins);
        var after = IdlParseContext.MapOffset(5, origins);
        var negative = IdlParseContext.MapOffset(-1, origins);
        var empty = IdlParseContext.MapOffset(0, []);

        // Assert
        before.ShouldBe(100);
        inside.ShouldBe(101);
        after.ShouldBe(100);
        negative.ShouldBe(-1);
        empty.ShouldBe(0);
    }

    [Fact]
    public void ResolvesForwardConstantBoundsAfterTheCompleteSymbolGraphIsParsed()
    {
        // Arrange
        var input = Input(
            "forward-bound.idl",
            "typedef string<Bound> Name; const long Bound = 9; struct Sample { Name value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("Its maximum length is <c>9</c>.");
    }

    [Fact]
    public void BindsForwardMemberReferencesAfterTheCompleteDeclarationGraphIsParsed()
    {
        // Arrange
        var input = Input(
            "forward-member-reference.idl",
            "struct Holder { Payload payload; }; struct Payload { long value; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Holder.g.cs"].Source.ShouldContain("Payload payload");

        // The reference is resolved only after Payload has been added to the symbol graph.
        documents["Holder.g.cs"].Source.ShouldContain("public Payload Payload");
    }

    [Fact]
    public void DoesNotBindAConstantAsAForwardMemberType()
    {
        // Arrange
        var input = Input(
            "constant-member-type.idl",
            "struct Holder { Later value; }; const long Later = 1;");

        // Act
        var exception = Should.Throw<IdlException>(() => CompileSources(input));

        // Assert
        exception.Message.ShouldContain("Unknown struct type: Later");
    }

    [Fact]
    public void BindsRawTypeReferencesThroughTheSemanticBinder()
    {
        // Arrange
        var binder = new IdlTypeBinder(new IdlSymbolTable());
        var reference = new IdlTypeReference("long", "Example", Input("binder.idl", string.Empty), 0);

        // Act
        var bound = binder.Bind(new IdlType.Reference(reference, "Unknown type"));

        // Assert
        bound.ShouldBeOfType<IdlType.Primitive>().Name.ShouldBe("long");
    }

    [Fact]
    public void ResolvesUserDefinedTypeNamesWithBuiltinPrefixes()
    {
        // Arrange
        var input = Input(
            "builtin-prefixes.idl",
            """
            struct stringRecord { long value; };
            struct wstringRecord { long value; };
            struct sequenceRecord { long value; };
            typedef stringRecord StringAlias;
            typedef wstringRecord WideStringAlias;
            struct Holder { StringAlias text; WideStringAlias wideText; sequenceRecord values; };
            union Choice switch(long) { case 1: stringRecord text; default: sequenceRecord values; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var holder = documents["Holder.g.cs"].Source;
        holder.ShouldContain("public stringRecord Text");
        holder.ShouldContain("public wstringRecord WideText");
        holder.ShouldContain("public sequenceRecord Values");

        var choice = documents["Choice.g.cs"].Source;
        choice.ShouldContain("public stringRecord text");
        choice.ShouldContain("public sequenceRecord values");
    }

    [Fact]
    public void ResolvesConstantAndDefaultBoundsOnStringsNestedInCollectionAliases()
    {
        // Arrange
        var input = Input(
            "nested-string-bound.idl",
            """
            const long Bound = 9;
            typedef sequence<string<Bound>, 2> Names;
            typedef sequence<string, 2> PlainNames;
            struct Holder { sequence<string, 2> values; };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Implementation.NamesUnmanaged.g.cs"].Source.ShouldContain("maxStrLen: 9");
        documents["Implementation.PlainNamesUnmanaged.g.cs"].Source.ShouldContain("maxStrLen: 255");
    }

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
        output.ShouldContain("PointSupport.Instance.GetDynamicTypeInternal(isPublic)");
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
    public void ResolvesTypeAliasesFromEnclosingModuleScopes()
    {
        // Arrange
        var input = Input(
            "enclosing-module-alias.idl",
            """
            module Outer {
                struct Value { long number; };
                module Inner {
                    typedef Value Alias;
                    struct Holder { Alias value; };
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Outer.Inner.Holder.g.cs"].Source.ShouldContain("public global::Outer.Value Value");
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
        output.ShouldContain("public Color color { get; set; } = Color.RED;");
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
    [Trait("Corpus", "C042")]
    public void RejectsMemberDefaultsOutsideTheirDeclaredRange()
    {
        // Arrange
        var input = Input(
            "invalid-default-range.idl",
            "module Defaults { struct Sample { @min(0) @max(10) @default(11) long value; }; };");

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("outside its declared range");
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
        baseType.ShouldContain("public partial class Base : global::A.B.C.Header");
    }

    [Fact]
    public void QualifiesAbsoluteStructBaseNamesAgainstShadowingNamespaces()
    {
        // Arrange
        var input = Input(
            "base-shadowing.idl",
            """
            module Shared {
                struct Base { long rootValue; };
            };
            module Example {
                module Shared {
                    struct Base { long shadowValue; };
                };
                struct Derived : ::Shared::Base { long localValue; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var derived = documents["Example.Derived.g.cs"].Source;
        derived.ShouldContain("public partial class Derived : global::Shared.Base");
        derived.ShouldContain("public Derived(int rootValue, int localValue) : base(rootValue)");
    }

    [Fact]
    public void BindsAggregateInheritanceBeforeEmission()
    {
        // Arrange
        var input = Input(
            "inheritance-binding.idl",
            "struct Derived : Missing { long value; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        parser.Parse(input.Text, input, 0, currentNamespace: null);

        // Act
        var exception = Should.Throw<IdlException>(parser.Bind);

        // Assert
        exception.Message.ShouldContain("Unknown struct base type: Missing");
        exception.Input.ShouldBe(input);
        exception.Offset.ShouldBe(input.Text.IndexOf("Missing", StringComparison.Ordinal));
    }

    [Fact]
    public void RejectsNonAggregateBaseTypesBeforeEmission()
    {
        // Arrange
        var input = Input(
            "non-aggregate-base.idl",
            "enum Base { Value }; struct Derived : Base { long value; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        parser.Parse(input.Text, input, 0, currentNamespace: null);

        // Act
        var exception = Should.Throw<IdlException>(parser.Bind);

        // Assert
        exception.Message.ShouldContain("Unknown struct base type: Base");
        exception.Input.ShouldBe(input);
        exception.Offset.ShouldBe(input.Text.LastIndexOf("Base", StringComparison.Ordinal));
    }

    [Fact]
    public void RejectsCyclicAggregateInheritanceBeforeEmission()
    {
        // Arrange
        var input = Input(
            "cyclic-inheritance.idl",
            "struct First : Second { long first; }; struct Second : First { long second; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        parser.Parse(input.Text, input, 0, currentNamespace: null);

        // Act
        var exception = Should.Throw<IdlException>(parser.Bind);

        // Assert
        exception.Message.ShouldContain("Cyclic struct inheritance detected: First");
        exception.Input.ShouldBe(input);
        exception.Offset.ShouldBe(input.Text.IndexOf("Second", StringComparison.Ordinal));
    }

    [Fact]
    public void ValidatesDerivedKeysBeforeEmission()
    {
        // Arrange
        var input = Input(
            "strict-inheritance.idl",
            "struct Base { @key long tenant; }; struct Derived : Base { @key long localId; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        parser.Parse(input.Text, input, 0, currentNamespace: null);
        parser.Bind();

        // Act
        var exception = Should.Throw<IdlException>(() => parser.Validate(strict: true));

        // Assert
        exception.Message.ShouldContain("derived from a struct/valuetype can not contain @key fields");
        exception.Input.ShouldBe(input);
        exception.Offset.ShouldBe(input.Text.IndexOf("long localId", StringComparison.Ordinal));
    }

    [Fact]
    public void StrictValidationAllowsDerivedAggregatesWithoutDirectKeys()
    {
        // Arrange
        var input = Input(
            "strict-inheritance-without-key.idl",
            "struct Base { @key long tenant; }; struct Derived : Base { long localId; };");
        var parser = new IdlDeclarationParser(new IdlSymbolTable(), CancellationToken.None);
        parser.Parse(input.Text, input, 0, currentNamespace: null);
        parser.Bind();

        // Act
        var exception = Record.Exception(() => parser.Validate(strict: true));

        // Assert
        exception.ShouldBeNull();
    }
}
