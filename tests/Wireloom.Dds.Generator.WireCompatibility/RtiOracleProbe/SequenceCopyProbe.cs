using System.Text.Json;
using Rti.Types;
using RuntimeSequenceCopy;

// Discovery: distinguish copy-constructor ownership from value-constructor reference retention.
var observations = new List<object>();
Observe("bounded-struct-typedef", () => new Children(ChildrenValues()), value => new Children(value),
    value => value.Value, value => value.Value[0]);
Observe("unbounded-struct-typedef", () => new UnboundedChildren(ChildrenValues()), value => new UnboundedChildren(value),
    value => value.Value, value => value.Value[0]);
Observe("sequence-alias-chain", () => new ChildrenChain(new Children(ChildrenValues())), value => new ChildrenChain(value),
    value => value.Value.Value, value => value.Value.Value[0]);
Observe("aggregate-alias-elements", () => new AliasChildren(ChildrenValues()), value => new AliasChildren(value),
    value => value.Value, value => value.Value[0]);
Observe("union-elements", () => new Choices(new Sequence<Choice> { new Choice { child = ChildValue() } }), value => new Choices(value),
    value => value.Value, value => value.Value[0].child);
Observe("aggregate-array-control", () => new Points(new[] { ChildValue(), ChildValue() }), value => new Points(value),
    value => value.Value, value => value.Value[0]);
Observe("struct-direct-sequence", DirectHolder, value => new Holder(value),
    value => value.direct, value => value.direct[0]);
Observe("struct-typedef-sequence", () => new Holder { wrapped = new Children(ChildrenValues()) }, value => new Holder(value),
    value => value.wrapped.Value, value => value.wrapped.Value[0]);
Observe("struct-optional-direct-sequence", () => new Holder { optionalChildren = ChildrenValues() }, value => new Holder(value),
    value => value.optionalChildren, value => value.optionalChildren[0]);
Observe("union-direct-sequence", () => new Container { direct = ChildrenValues() }, value => new Container(value),
    value => value.direct, value => value.direct[0]);
Observe("union-typedef-sequence", () => new Container { wrapped = new Children(ChildrenValues()) }, value => new Container(value),
    value => value.wrapped.Value, value => value.wrapped.Value[0]);

// Primitive elements need independent container storage; string elements are immutable.
var numbers = new Numbers(new Sequence<int> { 17 });
var numbersCopy = new Numbers(numbers);
numbersCopy.Value[0] = 99;
observations.Add(new { shape = "primitive-sequence-control", storageShared = ReferenceEquals(numbers.Value, numbersCopy.Value), sourceValueAfter = numbers.Value[0] });

var scalars = new Scalars(new Sequence<int> { 17 });
var scalarsCopy = new Scalars(scalars);
scalarsCopy.Value[0] = 99;
observations.Add(new { shape = "scalar-alias-elements", sourceValueAfter = scalars.Value[0], elementApi = "int" });

var supplied = ChildrenValues();
var fromValue = new Children(supplied);
observations.Add(new { shape = "value-constructor-control", storageShared = ReferenceEquals(supplied, fromValue.Value), elementShared = ReferenceEquals(supplied[0], fromValue.Value[0]) });
observations.Add(new { shape = "null-source-control", valueIsNull = new Children((Children)null).Value == null });

if (args.Length != 1)
{
    throw new ArgumentException("Provide the JSON report path.");
}

File.WriteAllText(args[0], JsonSerializer.Serialize(observations, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine($"Recorded {observations.Count} sequence-copy observations on .NET {Environment.Version} in {args[0]}.");

void Observe<T>(string shape, Func<T> create, Func<T, T> copy, Func<T, object> storage, Func<T, Child> child) where T : IEquatable<T>
{
    // Arrange
    var source = create();

    // Act
    var destination = copy(source);
    var equalBefore = destination.Equals(source);
    var equalHashesBefore = destination.GetHashCode() == source.GetHashCode();
    var storageShared = ReferenceEquals(storage(source), storage(destination));
    var elementShared = ReferenceEquals(child(source), child(destination));
    var nestedStorageShared = ReferenceEquals(child(source).values, child(destination).values);
    child(destination).id = 99;
    child(destination).values[0] = 77;

    // Assert/report: record independent expected source sentinels 17 and 7 without endorsing shallow copies.
    observations.Add(new
    {
        shape, equalBefore, equalHashesBefore, storageShared, elementShared, nestedStorageShared,
        sourceIdAfter = child(source).id, sourceNestedAfter = child(source).values[0],
        copyIdAfter = child(destination).id, copyNestedAfter = child(destination).values[0]
    });
}

static Child ChildValue()
{
    var child = new Child { id = 17 };
    child.values.Add(7);
    return child;
}

static Sequence<Child> ChildrenValues() => new() { ChildValue() };

static Holder DirectHolder()
{
    var holder = new Holder();
    holder.direct.Add(ChildValue());
    return holder;
}
