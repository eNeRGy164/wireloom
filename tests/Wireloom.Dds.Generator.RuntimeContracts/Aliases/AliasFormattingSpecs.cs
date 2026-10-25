using System.Globalization;
using Rti.Types;
using Rti.Dds.Topics;
using RuntimeAliases;

namespace Wireloom.Dds.Generator.RuntimeContracts.Aliases;

/// <summary>Checks managed alias formatting beyond RTI C#'s root DynamicData limitations.</summary>
public sealed class AliasFormattingSpecs
{
    /// <summary>Checks the hidden support formatter while retaining the RTI singleton and explicit-format overload.</summary>
    [Fact]
    public void FormatsThroughSupportWithoutReplacingItsRtiApi()
    {
        // Arrange
        var support = NumbersSupport.Instance;
        var sample = new Numbers(new Sequence<int> { 17, 18 });
        Func<Numbers, string> format = support.ToString;
        Func<Numbers, PrintFormatProperty, string> withOptions = support.ToString;

        // Act / Assert
        support.ToString(sample).ShouldBe("[17, 18]");
        format(sample).ShouldBe("[17, 18]");
        support.ToString(new Numbers((Numbers?)null)).ShouldBe("null");
        Should.Throw<ArgumentNullException>(() => support.ToString(null!));
        TextSupport.Instance.ToString(new Text("hello")).ShouldBe("hello");
        WideTextSupport.Instance.ToString(new WideText("世界")).ShouldBe("世界");
        support.ShouldBeSameAs(NumbersSupport.Instance);
        format.Target.ShouldBeSameAs(support);
        withOptions.Target.ShouldBeSameAs(support);
    }

    /// <summary>Characterizes RTI's nonvirtual base dispatch separately from the repaired generated API.</summary>
    [Fact]
    public void CharacterizesRtiBaseTypedDispatch()
    {
        // Arrange
        var sample = new Numbers(new Sequence<int> { 17 });
        TypeSupport<Numbers> baseSupport = NumbersSupport.Instance;

        // Act / Assert
        NumbersSupport.Instance.ToString(sample).ShouldBe("[17]");
        Should.Throw<Omg.Dds.Core.PreconditionNotMetException>(() => baseSupport.ToString(sample));
    }

    /// <summary>Checks scalar, enum, string, and chain values without initializing RTI root metadata.</summary>
    [Fact]
    public void FormatsUnderlyingValues()
    {
        // Arrange
        var scalar = new Scalar(17);
        var wide = new WideText("世界");

        // Act / Assert
        scalar.ToString().ShouldBe("17");
        new ScalarChain(17).ToString().ShouldBe("17");
        new EnumAlias(State.Ready).ToString().ShouldBe("Ready");
        new EnumChain(State.Ready).ToString().ShouldBe("Ready");
        new EnumAlias((State)42).ToString().ShouldBe("42");
        new Text("hello").ToString().ShouldBe("hello");
        wide.ToString().ShouldBe("世界");
        new Text().ToString().ShouldBeEmpty();
        new Text((string)null!).ToString().ShouldBe("null");
    }

    /// <summary>Checks collection ordering and transparent alias chains.</summary>
    [Fact]
    public void FormatsCollectionsAndChains()
    {
        // Arrange
        var numbers = new Numbers(new Sequence<int> { 17, 18 });
        var matrix = new Matrix(new [,] { { 17, 18, 19 }, { 20, 21, 22 } });

        // Act / Assert
        numbers.ToString().ShouldBe("[17, 18]");
        new NumbersChain(numbers).ToString().ShouldBe("[17, 18]");
        new Vector([17, 18]).ToString().ShouldBe("[17, 18]");
        matrix.ToString().ShouldBe("[[17, 18, 19], [20, 21, 22]]");
        new MatrixChain(matrix).ToString().ShouldBe(matrix.ToString());
        new Texts(new Sequence<string> { "hello", "世界" }).ToString().ShouldBe("[\"hello\", \"世界\"]");
        new WideTexts(new Sequence<string> { "世界" }).ToString().ShouldBe("[\"世界\"]");
    }

    /// <summary>Checks quoting through named string aliases nested in a collection.</summary>
    [Fact]
    public void QuotesNamedStringAliasesInCollections()
    {
        // Arrange
        var values = new RuntimeAliasFormatting.Texts(new Sequence<string>
        {
            "hello",
            null!,
            null!
        });
        var labels = new RuntimeAliasFormatting.Labels(new Sequence<string>
        {
            "world"
        });

        // Act
        var text = values.ToString();

        // Assert
        text.ShouldBe("[\"hello\", null, null]");
        labels.ToString().ShouldBe("[\"world\"]");
    }

    /// <summary>Checks exact string escaping, including null, empty, quotes, slashes, and all control escapes.</summary>
    [Fact]
    public void QuotesCollectionStringsAndPreservesUnicode()
    {
        // Arrange
        var values = new Texts(new Sequence<string> { "\"\\\n\r\t\b\f\0\u001f", "世界🙂", null!, "" });

        // Act
        var text = values.ToString();

        // Assert
        text.ShouldBe("[\"\\\"\\\\\\n\\r\\t\\b\\f\\u0000\\u001f\", \"世界🙂\", null, \"\"]");
    }

    /// <summary>Checks null references and empty collection dimensions without native allocation.</summary>
    [Fact]
    public void FormatsNullAndEmptyValues()
    {
        // Arrange
        var numbers = new Numbers((Numbers?)null);

        // Act / Assert
        numbers.ToString().ShouldBe("null");
        new Numbers().ToString().ShouldBe("[]");
        new Texts().ToString().ShouldBe("[]");
        new Vector([]).ToString().ShouldBe("[]");
        new Matrix(new int[0, 3]).ToString().ShouldBe("[]");
        new Matrix(new int[2, 0]).ToString().ShouldBe("[[], []]");
        new Matrix((int[,])null!).ToString().ShouldBe("null");
        new NumbersChain((Numbers)null!).ToString().ShouldBe("null");
        new Children(new Sequence<Child> { null! }).ToString().ShouldBe("[null]");
    }

    /// <summary>Checks that aggregate formatting continues to use RTI's original member formatter.</summary>
    [Fact]
    public void RetainsRtiAggregateFormattingInsideCollections()
    {
        // Arrange
        var child = new Child(17, "hello");
        var other = new Child(18, "other");
        var expectedChild = ChildSupport.Instance.ToString(child);
        var expectedOther = ChildSupport.Instance.ToString(other);

        // Act / Assert
        new Children(new Sequence<Child> { child }).ToString().ShouldBe($"[{expectedChild}]");
        new Points([child, other]).ToString().ShouldBe($"[{expectedChild}, {expectedOther}]");
        new Aggregate(child).ToString().ShouldBe(AggregateSupport.Instance.ToString(new Aggregate(child)));
        new AggregateChain(child).ToString().ShouldBe(expectedChild);
    }

    /// <summary>Checks invariant numeric formatting while preserving RTI long-double conversion.</summary>
    [Fact]
    public void FormatsNumbersIndependentlyOfCurrentCulture()
    {
        // Arrange
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

            // Act / Assert
            new RuntimeAliasFormatting.Number(1.25).ToString().ShouldBe("1.25");
            new RuntimeAliasFormatting.Extended((LongDouble)1.25m).ToString().ShouldBe("1.25");
            new RuntimeAliasFormatting.Numbers(new Sequence<double> { 1.25, -2.5 }).ToString().ShouldBe("[1.25, -2.5]");
            new RuntimeAliasFormatting.Flag(true).ToString().ShouldBe("True");
            new RuntimeAliasFormatting.Character('x').ToString().ShouldBe("x");
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    /// <summary>Checks three dimensions and nested sequence aliases.</summary>
    [Fact]
    public void PreservesNestedCollectionShapes()
    {
        // Arrange
        var rows = new RuntimeAliasFormatting.Rows(new Sequence<RuntimeAliasFormatting.Numbers>
        {
            new(new Sequence<double> { 1.25, 2.5 }),
            new()
        });

        // Act / Assert
        new RuntimeAliasFormatting.Cube(new [,,] { { { 1, 2 }, { 3, 4 } } }).ToString().ShouldBe("[[[1, 2], [3, 4]]]");
        rows.ToString().ShouldBe("[[1.25, 2.5], []]");
    }
}
