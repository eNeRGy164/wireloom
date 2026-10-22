using Wireloom.Compiler.Emission;

using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Generation.Tests;

public sealed class UnionSpecs
{
    [Fact]
    public void DocumentsStringBoundsInEncodingUnitsForUnionBranches()
    {
        // Arrange
        var input = Input("union-string-bounds.idl",
            "module Example { union Choice switch(long) { case 1: string<12> narrow; default: wstring<7> wide; }; };");

        // Act
        var managed = CompileSources(input)["Example.Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("The bound on this narrow IDL string is measured in UTF-8 bytes.");
        managed.ShouldContain("The bound on this wide IDL string is measured in UTF-16 code units.");
        managed.ShouldContain("<seealso href=\"https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Strings_and_Wide_Strings.htm\">RTI Connext 7.7.0 string and wide-string bounds</seealso>");
        managed.ShouldContain("[Bound(12)]");
        managed.ShouldContain("[Bound(7)]");
    }

    [Fact]
    public void EmitsUnionManagedNativePluginAndTypeSupportDocuments()
    {
        // Arrange
        var input = Input("union.idl",
            "module Example { union Choice switch(long) { case 1: case 5: long number; case 2: string text; default: boolean flag; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents.Keys.ShouldBe([
            "Example.Choice.g.cs",
            "Example.Implementation.ChoiceUnmanaged.g.cs",
            "Example.Implementation.ChoicePlugin.g.cs",
            "Example.ChoiceSupport.g.cs"]);

        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("public int number");
        managed.ShouldContain("public string text");
        managed.ShouldContain("public bool flag");
        managed.ShouldContain("Setflag");
        managed.ShouldContain("case 1:");
        managed.ShouldContain("case 5:");
        managed.ShouldContain("throw new global::System.InvalidOperationException");

        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("FromNative");
        native.ShouldContain("ToNative");
        native.ShouldContain("Initialize");
        native.ShouldContain("Destroy");

        var plugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("BuildUnion()");
        plugin.ShouldContain("UnionMember.DefaultLabel");

        var support = documents["Example.ChoiceSupport.g.cs"].Source;
        support.ShouldContain("TypeSupport<Choice>");
    }

    [Fact]
    public void EmitsDirectBoundedStringSequenceUnionBranchesWithSpecializedNativeStorage()
    {
        // Arrange
        var input = Input("union-string-sequences.idl",
            """
            module Example {
                const long NarrowBound = 8;
                const long WideBound = 4;
                union Choice switch(long) {
                    case 0: sequence<string<NarrowBound>, 3> narrow;
                    case 1: sequence<wstring<WideBound>, 3> wide;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("public ISequence<string> narrow");
        managed.ShouldContain("public ISequence<string> wide");

        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("private NativeStringSeq narrow;");
        native.ShouldContain("private NativeWstringSeq wide;");
        native.ShouldContain("narrow.Initialize(max: 3, absoluteMax: 3, maxStrLen: 8, allocateMemory);");
        native.ShouldContain("wide.Initialize(max: 3, absoluteMax: 3, maxStrLen: 4, allocateMemory);");
        native.ShouldContain("narrow.ToNative(sample.narrow, 8);");
        native.ShouldContain("wide.ToNative(sample.wide, 4);");
        native.ShouldContain("narrow.FromNative(sample.narrow);");
        native.ShouldContain("wide.FromNative(sample.wide);");
        native.ShouldContain("narrow.Destroy();");
        native.ShouldContain("wide.Destroy();");

        var plugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.CreateString(8), 3)");
        plugin.ShouldContain("CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(4), 3)");
    }

    [Fact]
    public void MultiLabelUnionAccessorsAndFromNativePreserveTheNativeDiscriminator()
    {
        // Arrange
        var input = Input("multi-label-union.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 1: case 5: long number;
                    default: boolean flag;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("Discriminator != 1 && Discriminator != 5");
        managed.ShouldContain("public void Setnumber(int value, int discriminator)");

        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("sample.Setnumber(number, _discriminator);");
        native.ShouldNotContain("sample.Setnumber(sample.number = number, _discriminator);");
    }

    [Fact]
    public void ResolvesSymbolicHexadecimalAndExpressionUnionLabels()
    {
        // Arrange
        var input = Input("union-expression-labels.idl",
            """
            module Example {
                const long Base = 1;
                const long High = 0xFF;
                union Choice switch(long) {
                    case Base + 1: long next;
                    case High: long high;
                    default: long other;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("case 2:");
        managed.ShouldContain("case 255:");
    }

    [Fact]
    public void ProjectsBooleanConstantUnionLabelsAsTheirBooleanValues()
    {
        // Arrange
        var input = Input("union-boolean-constant-labels.idl",
            "const boolean Enabled = TRUE; const boolean Selected = Enabled; const boolean Disabled = FALSE; " +
            "union EnabledChoice switch(boolean) { case Enabled: long yes; default: long no; }; " +
            "union ChainedChoice switch(boolean) { case Selected: long yes; default: long no; }; " +
            "union DisabledChoice switch(boolean) { case Disabled: long no; default: long yes; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["EnabledChoice.g.cs"].Source.ShouldContain("case true:");
        documents["ChainedChoice.g.cs"].Source.ShouldContain("case true:");
        documents["DisabledChoice.g.cs"].Source.ShouldContain("case false:");
    }

    [Fact]
    public void ResolvesScopedEnumUnionLabels()
    {
        // Arrange
        var input = Input("union-scoped-enum-labels.idl",
            """
            module Example {
                enum Kind { FIRST, SECOND };
                union Choice switch(Kind) {
                    case Kind::FIRST: long first;
                    case SECOND: long second;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Example.Choice.g.cs"].Source.ShouldContain("Kind.FIRST");
        documents["Example.Choice.g.cs"].Source.ShouldContain("Kind.SECOND");
    }

    [Fact]
    public void RejectsUnionEnumLabelsFromAnotherEnumType()
    {
        // Arrange
        var input = Input("union-foreign-enum-label.idl",
            """
            enum First { ONE };
            enum Second { TWO };
            union Choice switch(First) { case Second::TWO: long value; };
            """);

        // Act
        var exception = Should.Throw<IdlException>(() => Compile(input));

        // Assert
        exception.Message.ShouldContain("Unknown union discriminator label");
    }

    [Fact]
    public void PlansDirectFixedArrayUnionBranchesWithNativeLifecycleSupport()
    {
        // Arrange
        var input = Input("union-array-branch.idl",
            "union Sample switch(long) { case 0: long numbers[2]; default: long other; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Sample.g.cs"].Source.ShouldContain("public int[] numbers");
        documents["Sample.g.cs"].Source.ShouldContain("other.numbers.Clone()");
        var native = documents["Implementation.SampleUnmanaged.g.cs"].Source;
        native.ShouldContain("numbers.Initialize<int>(dimension: 2, allocateMemory);");
        native.ShouldContain("numbers.Destroy");
        native.ShouldContain("FromNative");
        native.ShouldContain("ToNative");
        documents["Implementation.SamplePlugin.g.cs"].Source.ShouldContain("new[] { 0 }");
    }

    [Fact]
    public void CopiesExplicitAggregateArrayUnionBranchesElementByElement()
    {
        // Arrange
        var input = Input("union-explicit-aggregate-array.idl",
            "struct Item { long value; }; union Choice switch(long) { case 1: Item items[2]; default: long number; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Choice.g.cs"].Source;
        managed.ShouldContain("other.items.Clone()");
        managed.ShouldContain("new Item(other.items[dimension0])");
    }

    [Fact]
    public void InitializesDefaultEnumArrayUnionBranchesToDeclaredDefaultLiteral()
    {
        // Arrange
        var input = Input("union-default-enum-array.idl",
            "enum Shade { BLACK, @default_literal WHITE }; union Choice switch(long) { default: Shade values[2]; };");

        // Act
        var managed = CompileSources(input)["Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("new Shade[2]");
        managed.ShouldContain("values[dimension0] = Shade.WHITE;");
    }

    [Fact]
    public void MapsDirectSequenceUnionArrayBranchesWithIdlConfiguration()
    {
        // Arrange
        const string source = "union Choice switch(long) { case 1: long number; default: sequence<long, 4> values[2]; };";
        var input = new IdlInput("union-sequence-array-branch.idl", source, IdlSequenceArrayMapping.Idl);

        // Act
        var documents = IdlCompiler.CompileSources([input], TestContext.Current.CancellationToken);

        // Assert
        var managed = documents["Choice.g.cs"].Source;
        managed.ShouldContain("public Choice_valuesSequence[] values");
        managed.ShouldContain("_values = new Choice_valuesSequence[2];");
        managed.ShouldContain("_values[dimension0] = new Choice_valuesSequence();");
        managed.ShouldContain("new Choice_valuesSequence(other.values[dimension0])");

        var native = documents["Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("values.Initialize");
        native.ShouldContain("dimension: 2");

        var plugin = documents["Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("CreateArrayWithAccessInfo<global::Implementation.Choice_valuesSequenceUnmanaged>");
        plugin.ShouldContain("new uint[] { 2 }");
        documents.ContainsKey("Choice_valuesSequence.g.cs").ShouldBeTrue();
    }

    [Fact]
    public void DirectSequenceUnionArrayWithLegacyMappingReportsTheExistingWarning()
    {
        // Arrange
        const string source = "union Choice switch(long) { case 1: long number; default: sequence<long, 4> values[2]; };";
        var input = new IdlInput("union-sequence-array-branch.idl", source);
        var diagnostics = new List<IdlDiagnostic>();

        // Act
        var documents = IdlCompiler.CompileSourcesWithDiagnostics(
            [input], diagnostics, TestContext.Current.CancellationToken, RtiApiProfile.Rtiddsgen470);

        // Assert
        diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0105"
            && diagnostic.Message.Contains("arrays of sequences", StringComparison.Ordinal));
        documents["Choice.g.cs"].Source.ShouldContain("public ISequence<int> values");
    }

    [Fact]
    public void QualifiesShadowedUnionNativeFieldsAcrossConversionPaths()
    {
        // Arrange
        var input = Input("union-shadowing.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 1: case 2: long sample;
                    case 3: long keysOnly;
                    default: boolean flag;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("sample.Setsample(this.sample, _discriminator);");
        native.ShouldContain("sample.keysOnly = this.keysOnly;");
        native.ShouldContain("this.sample = sample.sample;");
        native.ShouldContain("this.keysOnly = sample.keysOnly;");
        native.ShouldNotContain("sample.Setsample(sample, _discriminator);");
        native.ShouldNotContain("sample.keysOnly = keysOnly;");
    }

    [Fact]
    public void QualifiesReferenceEqualsWhenAUnionBranchUsesThatName()
    {
        // Arrange
        var input = Input(
            "union-reference-equals.idl",
            """
            union Choice switch(long) {
                case 1: long ReferenceEquals;
                default: long value;
            };
            """);

        // Act
        var managed = CompileSources(input)["Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("global::System.Object.ReferenceEquals(this, other)");
    }

    [Fact]
    public void UnionDestroyReleasesAllResourceBearingBranchesAndHolderDelegatesDirectly()
    {
        // Arrange
        var input = Input("union-destroy.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 1: string text;
                    case 2: Payload payload;
                };
                typedef Choice ChoiceAlias;
                struct Holder { ChoiceAlias value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var union = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        union.ShouldContain("text.Destroy();");
        union.ShouldContain("payload.Destroy(optionalsOnly);");

        var choiceAlias = documents["Example.Implementation.ChoiceAliasUnmanaged.g.cs"].Source;
        choiceAlias.ShouldContain("Value.Destroy(optionalsOnly);");
        choiceAlias.ShouldNotContain("if (optionalsOnly)");

        var holder = documents["Example.Implementation.HolderUnmanaged.g.cs"].Source;
        holder.ShouldContain("value.Destroy(optionalsOnly);");
        holder.ShouldNotContain("if (optionalsOnly)");
    }

    [Fact]
    public void AggregateMembersUseTheUnderlyingUnionForChainedTypedefs()
    {
        // Arrange
        var input = Input("union-alias-chain.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 0: long number;
                    case 1: string text;
                };
                typedef Choice ChoiceAlias;
                typedef ChoiceAlias ChoiceAlias2;
                struct Holder { ChoiceAlias2 value; };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var holder = documents["Example.Holder.g.cs"].Source;
        holder.ShouldContain("public Choice value { get; set; }");
        holder.ShouldContain("value = new Choice(other.value);");

        var choiceAlias2 = documents["Example.ChoiceAlias2.g.cs"].Source;
        choiceAlias2.ShouldContain("public ChoiceAlias2(Choice value)");
        choiceAlias2.ShouldContain("Value = value;");
        choiceAlias2.ShouldNotContain("this.Value = Value;");
        choiceAlias2.ShouldContain("Value = new Choice(other.Value);");

        var holderNative = documents["Example.Implementation.HolderUnmanaged.g.cs"].Source;
        holderNative.ShouldContain("private ChoiceUnmanaged value;");
        holderNative.ShouldContain("value.FromNative(sample.value, keysOnly: false);");
        holderNative.ShouldContain("value.ToNative(sample.value, keysOnly: false);");
    }

    [Fact]
    public void EmitsEnumDiscriminatorAndUnionWithoutDefault()
    {
        // Arrange
        var input = Input("enum-union.idl",
            """
            module Example {
                enum Kind { Number, Text };
                union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("Discriminator { get; private set; }");
        managed.ShouldContain("public Kind Discriminator { get; private set; }");
        managed.ShouldContain("public const Kind DefaultDiscriminator = 0;");
        managed.ShouldContain("Discriminator != Kind.Number");
        managed.ShouldContain("Discriminator = Kind.Number;");
        managed.ShouldNotContain("global::Example.Kind");
        managed.ShouldNotContain("Settext");
        managed.ShouldContain("number");
        managed.ShouldContain("text");

        var plugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        plugin.ShouldContain("WithDiscriminator(KindSupport.Instance.GetDynamicTypeInternal(isPublic))");
    }

    [Fact]
    public void QualifiesEnumDiscriminatorsAgainstShadowingNamespaces()
    {
        // Arrange
        var input = Input(
            "enum-union-shadowing.idl",
            """
            module Shared {
                enum Kind { First };
            };
            module Example {
                module Shared {
                    enum Kind { Shadow };
                };
                union Choice switch(::Shared::Kind) {
                    case First: long value;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);
        var managed = documents["Example.Choice.g.cs"].Source;
        var unmanaged = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        var plugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;

        // Assert
        managed.ShouldContain("public global::Shared.Kind Discriminator");
        managed.ShouldContain("global::Shared.Kind.First");
        unmanaged.ShouldContain("private global::Shared.Kind _discriminator");
        plugin.ShouldContain("WithDiscriminator(global::Shared.KindSupport.Instance.GetDynamicTypeInternal(isPublic))");
    }

    [Fact]
    public void ResolvesForwardEnumUnionDiscriminatorDuringBinding()
    {
        // Arrange
        var input = Input("forward-enum-union.idl",
            """
            module Example {
                union Choice switch(Kind) {
                    case First: long value;
                };
                enum Kind { First };
            };
            """);

        // Act
        var managed = CompileSources(input)["Example.Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("public Kind Discriminator { get; private set; }");
        managed.ShouldContain("Discriminator != Kind.First");
    }

    [Fact]
    public void ResolvesPrimitiveTypedefUnionDiscriminatorsDuringBinding()
    {
        // Arrange
        var input = Input("primitive-typedef-union.idl",
            """
            module Example {
                typedef long ChoiceKind;
                union Choice switch(ChoiceKind) {
                    case 1: long value;
                    default: string other;
                };
            };
            """);

        // Act
        var managed = CompileSources(input)["Example.Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("public int Discriminator { get; private set; }");
        managed.ShouldContain("Discriminator != 1");
    }

    [Fact]
    public void InitializesEnumDiscriminatorToTheFirstLiteralValue()
    {
        // Arrange
        var input = Input("enum-default-value.idl",
            """
            module Example {
                enum Kind { First = 10, Second = 20 };
                union Choice switch(Kind) {
                    case Second: long value;
                    default: string other;
                };
            };
            """);

        // Act
        var managed = CompileSources(input)["Example.Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("public const Kind DefaultDiscriminator = (Kind)10;");
        managed.ShouldContain("Discriminator = 0;");
    }

    [Fact]
    public void UnionGeneratedBehaviorSelectsBranchesAndCopiesValues()
    {
        // Arrange
        var input = Input("behavior.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 1: long number;
                    default: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;

        managed.ShouldContain("Discriminator = 1;");
        managed.ShouldContain("Discriminator = discriminator;");
        managed.ShouldContain("return _number;");
        managed.ShouldContain("return _text;");
        managed.ShouldContain("public Choice(Choice? other)");
        managed.ShouldContain("if (Discriminator != 1)\n            {");
        managed.ShouldContain("Discriminator = other.Discriminator;\n\n");
        managed.ShouldContain("switch (Discriminator)");
        managed.ShouldContain("this._text = other.text;");
        managed.ShouldContain("1 => number,");
        managed.ShouldContain("1 => number.Equals(other.number),");
        managed.ShouldNotContain("throw new global::System.InvalidOperationException(\"number not selected\");\n\n            return _number;");
    }

    [Fact]
    public void UnionCopyConstructorOmitsEmptyDefaultCaseWhenNoDefaultBranchExists()
    {
        // Arrange
        var input = Input("copy-constructor-without-default-branch.idl",
            "module Example { union Choice switch(long) { case 0: long number; case 1: long other; }; };");

        // Act
        var managed = CompileSources(input)["Example.Choice.g.cs"].Source;
        var constructorStart = managed.IndexOf("public Choice(Choice? other)", StringComparison.Ordinal);
        constructorStart.ShouldBeGreaterThanOrEqualTo(0);
        var constructorEnd = managed.IndexOf("public object? Get()", constructorStart, StringComparison.Ordinal);
        var constructor = managed[constructorStart..constructorEnd];

        // Assert
        constructor.ShouldContain("case 0:");
        constructor.ShouldContain("this._number = other.number;");
        constructor.ShouldContain("case 1:");
        constructor.ShouldContain("this._other = other.other;");
        constructor.ShouldNotContain("default:");
    }

    [Fact]
    public void UnionGetReturnNullabilityMatchesItsSelectableBranches()
    {
        // Arrange
        var input = Input("union-get-nullability.idl",
            "module Example { union DefaultChoice switch(long) { case 1: long number; default: string text; }; union OpenChoice switch(long) { case 1: long number; }; };");

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Example.DefaultChoice.g.cs"].Source.ShouldContain("public object Get()");
        documents["Example.OpenChoice.g.cs"].Source.ShouldContain("public object? Get()");
    }

    [Fact]
    public void UnionConstructorDoesNotInitializeInactiveAggregateBranches()
    {
        // Arrange
        var input = Input("struct-union.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 10: Payload payload;
                    case 11: sequence<long, 4> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("private Payload _payload = null!;");
        managed.ShouldContain("private ISequence<int> _values = null!;");
        var constructorStart = managed.IndexOf("public Choice()", StringComparison.Ordinal);
        constructorStart.ShouldBeGreaterThanOrEqualTo(0);
        var constructor = managed[constructorStart..managed.IndexOf("public Choice(Choice? other)", constructorStart, StringComparison.Ordinal)];

        constructor.ShouldContain("Discriminator = DefaultDiscriminator;");
        constructor.ShouldNotContain("new global::Example.Payload()");
        constructor.ShouldNotContain("new Sequence<long>");
    }

    [Fact]
    public void UnionConstructorInitializesTheAggregateBranchSelectedByItsDefaultDiscriminator()
    {
        // Arrange
        var input = Input("union-default-aggregate-branch.idl",
            "module Example { struct Payload { long value; }; union Choice switch(long) { case 0: @optional Payload payload; case 1: long number; }; };");

        // Act
        var managed = CompileSources(input)["Example.Choice.g.cs"].Source;

        // Assert
        var constructorStart = managed.IndexOf("public Choice()", StringComparison.Ordinal);
        constructorStart.ShouldBeGreaterThanOrEqualTo(0);
        var constructor = managed[constructorStart..managed.IndexOf("public Choice(Choice? other)", constructorStart, StringComparison.Ordinal)];
        constructor.ShouldContain("Discriminator = DefaultDiscriminator;");
        constructor.ShouldContain("_payload = new Payload();");
        var copyStart = managed.IndexOf("public Choice(Choice? other)", StringComparison.Ordinal);
        var copyBranchStart = managed.IndexOf("Discriminator = other.Discriminator;", copyStart, StringComparison.Ordinal);
        var nullCopy = managed[copyStart..copyBranchStart];
        nullCopy.ShouldContain("if (other is null)");
        nullCopy.ShouldContain("Discriminator = DefaultDiscriminator;");
        nullCopy.ShouldContain("_payload = new Payload();");
        managed[copyBranchStart..].ShouldNotContain("_payload = new Payload();");
    }

    [Fact]
    public void UnionDefaultAggregateSequenceInitializesNativeStorage()
    {
        // Arrange
        var input = Input("union-default-sequence.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 1: long number;
                    default: sequence<Payload, 2> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        var constructorStart = managed.IndexOf("public Choice()", StringComparison.Ordinal);
        constructorStart.ShouldBeGreaterThanOrEqualTo(0);
        var constructor = managed[constructorStart..managed.IndexOf("public Choice(Choice? other)", constructorStart, StringComparison.Ordinal)];
        constructor.ShouldContain("_values = new Sequence<Payload>();");

        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("values.Initialize<Payload, PayloadUnmanaged>(max: 2, absoluteMax: 2, allocateMemory);");
    }

    [Fact]
    public void UnionDefaultEqualityQualifiesABranchNamedOther()
    {
        // Arrange
        var input = Input("default-other-equality.idl",
            """
            module Example {
                union Choice switch(long) {
                    case 1: long value;
                    default: string other;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var managed = documents["Example.Choice.g.cs"].Source;
        managed.ShouldContain("_ => this.other.Equals(other.other),");
    }

    [Fact]
    public void UnionFromNativeInitializesActiveAggregateBranchesWhenTheDiscriminatorChanges()
    {
        // Arrange
        var input = Input("union-from-native.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 10: Payload payload;
                    case 11: sequence<long, 4> values;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;

        native.ShouldContain("if (sample.Discriminator != _discriminator)");
        native.ShouldContain("sample.payload = new ");
        native.ShouldContain("payload.FromNative(sample.payload, keysOnly: false);");
        native.ShouldContain("sample.values = new Sequence<int>();");
        native.ShouldContain("values.FromNative((Sequence<int>)sample.values);");
    }

    [Fact]
    public void UnionConversionsHandleStringAndAggregateMultiLabelBranches()
    {
        // Arrange
        var input = Input("union-multi-label-conversions.idl",
            """
            module Example {
                struct Payload { long value; };
                union Choice switch(long) {
                    case 1: case 2: string text;
                    case 3: case 4: Payload payload;
                    default: long value;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var native = documents["Example.Implementation.ChoiceUnmanaged.g.cs"].Source;
        native.ShouldContain("sample.Settext(text.FromNative(), _discriminator);");
        native.ShouldContain("payload.FromNative(sample.payload, keysOnly: false);");
        native.ShouldContain("sample.Setpayload(sample.payload, _discriminator);");
        native.ShouldContain("sample.Setvalue(value, _discriminator);");
    }

    [Fact]
    public void UnionDefaultDiscriminatorsHandleEnumOccupiedIntegerAndBooleanLabels()
    {
        // Arrange
        var input = Input("union-default-discriminators.idl",
            """
            module Example {
                enum Kind { Number, Text };
                union EnumChoice switch(Kind) {
                    case Number: long number;
                    default: string text;
                };
                union ExhaustiveEnumChoice switch(Kind) {
                    case Number:
                    case Text: long value;
                    default: string other;
                };
                union IntegerChoice switch(long) {
                    case 0: long number;
                    default: string text;
                };
                union BooleanChoice switch(boolean) {
                    case FALSE: long number;
                    default: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var enumChoice = documents["Example.EnumChoice.g.cs"].Source;
        enumChoice.ShouldContain("public const Kind DefaultDiscriminator = 0;");
        enumChoice.ShouldContain("Discriminator = (Kind)1;");

        var exhaustiveEnumChoice = documents["Example.ExhaustiveEnumChoice.g.cs"].Source;
        exhaustiveEnumChoice.ShouldContain("public const Kind DefaultDiscriminator = 0;");
        exhaustiveEnumChoice.ShouldContain("Discriminator = (Kind)2;");

        var integerChoice = documents["Example.IntegerChoice.g.cs"].Source;
        integerChoice.ShouldContain("public const int DefaultDiscriminator = 0;");
        integerChoice.ShouldContain("Discriminator = 1;");

        var booleanChoice = documents["Example.BooleanChoice.g.cs"].Source;
        booleanChoice.ShouldContain("public const bool DefaultDiscriminator = false;");
        booleanChoice.ShouldContain("Discriminator = true;");
    }

    [Fact]
    public void UnionBackingFieldsAvoidNamesAlreadyUsedByBranches()
    {
        // Arrange
        var input = Input("union-backing-field-collision.idl",
            "union Choice switch(long) { case 1: long value; case 2: long _value; };");

        // Act
        var managed = CompileSources(input)["Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("private int __value;");
        managed.ShouldContain("private int ___value;");
    }

    [Fact]
    public void UsesTheLowestNegativeDiscriminatorWhenAllNonNegativeValuesAreOccupied()
    {
        // Arrange
        var cases = string.Join(" ", Enumerable.Range(0, sbyte.MaxValue + 1).Select(value => $"case {value}:"));
        var input = Input("signed-byte-default-discriminator.idl",
            $$"""
            union Choice switch(int8) {
                {{cases}}
                long value;
                default: string other;
            };
            """);

        // Act
        var managed = CompileSources(input)["Choice.g.cs"].Source;

        // Assert
        managed.ShouldContain("Discriminator = -128;");
    }

    [Fact]
    public void AppendableEnumAndUnionUseExtensibleTypeSupport()
    {
        // Arrange
        var input = Input("appendable.idl",
            """
            module Example {
                @appendable enum Kind { Number, Text };
                @appendable union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var kindPlugin = documents["Example.Implementation.KindPlugin.g.cs"].Source;
        kindPlugin.ShouldContain("WithExtensibility(ExtensibilityKind.Extensible)");

        var choicePlugin = documents["Example.Implementation.ChoicePlugin.g.cs"].Source;
        choicePlugin.ShouldContain("WithExtensibility(ExtensibilityKind.Extensible)");
    }

    [Theory]
    [InlineData("@final", "Final")]
    [InlineData("@mutable", "Mutable")]
    public void UnionShorthandExtensibilityAnnotationsMatchFullAnnotations(string annotation, string expectedKind)
    {
        // Arrange
        var input = Input("union-extensibility.idl",
            $$"""
            union Choice switch(long) {
                case 0: long value;
            };
            {{annotation}} union AnnotatedChoice switch(long) {
                case 0: long value;
            };
            @extensibility({{expectedKind.ToUpperInvariant()}}) union FullyAnnotatedChoice switch(long) {
                case 0: long value;
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        documents["Implementation.AnnotatedChoicePlugin.g.cs"].Source.ShouldContain($"WithExtensibility(ExtensibilityKind.{expectedKind})");
        documents["Implementation.FullyAnnotatedChoicePlugin.g.cs"].Source.ShouldContain($"WithExtensibility(ExtensibilityKind.{expectedKind})");
    }

    [Fact]
    public void NestedAppendableUnionIsAccepted()
    {
        // Arrange
        var input = Input("nested-union.idl",
            """
            module Example {
                enum Kind { Number, Text };
                @nested @appendable union Choice switch(Kind) {
                    case Number: long number;
                    case Text: string text;
                };
            };
            """);

        // Act
        var documents = CompileSources(input);

        // Assert
        var choice = documents["Example.Choice.g.cs"].Source;
        choice.ShouldContain("public partial class Choice");
    }
}
