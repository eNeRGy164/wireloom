using System.Text.Json;
using RuntimeUnions;
using RuntimeUnionNullCopy;

// Discovery only: record all states and failures without turning an RTI quirk into a desired contract.
var observations = new List<object>();
Observe("choice", Choice.DefaultDiscriminator, () => new Choice(), value => new Choice(value),
    value => value.Discriminator, value => value.Get(),
    ("number", value => value.number), ("flag", value => value.flag));
Observe("sparse", Sparse.DefaultDiscriminator, () => new Sparse(), value => new Sparse(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number));
Observe("zero-scalar", ZeroScalar.DefaultDiscriminator, () => new ZeroScalar(), value => new ZeroScalar(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number));
Observe("zero-string", ZeroString.DefaultDiscriminator, () => new ZeroString(), value => new ZeroString(value),
    value => value.Discriminator, value => value.Get(), ("text", value => value.text));
Observe("zero-aggregate", ZeroAggregate.DefaultDiscriminator, () => new ZeroAggregate(), value => new ZeroAggregate(value),
    value => value.Discriminator, value => value.Get(), ("child", value => value.child));
Observe("zero-sequence", ZeroSequence.DefaultDiscriminator, () => new ZeroSequence(), value => new ZeroSequence(value),
    value => value.Discriminator, value => value.Get(), ("numbers", value => value.numbers));
Observe("enum-no-default", EnumNoDefault.DefaultDiscriminator, () => new EnumNoDefault(), value => new EnumNoDefault(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number));
Observe("enum-with-default", EnumWithDefault.DefaultDiscriminator, () => new EnumWithDefault(), value => new EnumWithDefault(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number), ("flag", value => value.flag));
Observe("negative-labels", NegativeLabels.DefaultDiscriminator, () => new NegativeLabels(), value => new NegativeLabels(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number), ("flag", value => value.flag));
Observe("negative-and-zero", NegativeAndZero.DefaultDiscriminator, () => new NegativeAndZero(), value => new NegativeAndZero(value),
    value => value.Discriminator, value => value.Get(), ("negative", value => value.negative), ("zero", value => value.zero), ("flag", value => value.flag));
Observe("negative-enum", NegativeEnum.DefaultDiscriminator, () => new NegativeEnum(), value => new NegativeEnum(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number), ("other", value => value.other));
Observe("mixed-enum", MixedEnum.DefaultDiscriminator, () => new MixedEnum(), value => new MixedEnum(value),
    value => value.Discriminator, value => value.Get(), ("number", value => value.number), ("zero", value => value.zero), ("flag", value => value.flag));

// Non-null populated source control, including an explicit label other than the branch's first label.
var source = new Choice();
source.Settext("copied", 2);
var sourceCopy = new Choice(source);
observations.Add(new { shape = "populated-copy-control", discriminator = sourceCopy.Discriminator, sourceCopy.text, equal = sourceCopy.Equals(source) });

var negativeSource = new NegativeLabels();
negativeSource.Setnumber(23, -1);
var negativeCopy = new NegativeLabels(negativeSource);
observations.Add(new { shape = "negative-copy-control", discriminator = negativeCopy.Discriminator, negativeCopy.number, equal = negativeCopy.Equals(negativeSource) });

if (args.Length != 1)
{
    throw new ArgumentException("Provide the JSON report path.");
}

File.WriteAllText(args[0], JsonSerializer.Serialize(observations, new JsonSerializerOptions { WriteIndented = true }));
Console.WriteLine($"Recorded twelve union shapes and two populated-copy controls on .NET {Environment.Version} in {args[0]}.");

void Observe<T>(string shape, object declaredDefault, Func<T> construct, Func<T, T> copy,
    Func<T, object> discriminator, Func<T, object> get, params (string Name, Func<T, object> Read)[] branches)
    where T : class, IEquatable<T>
{
    // Arrange
    var normal = construct();

    // Act
    var nullCopy = copy(null);
    var secondNullCopy = copy(null);
    var normalCopy = copy(normal);

    // Assert/report: preserve observations rather than asserting equivalence to the default constructor.
    observations.Add(new
    {
        shape,
        declaredDefault,
        normal = Snapshot(normal),
        nullCopy = Snapshot(nullCopy),
        normalCopy = Snapshot(normalCopy),
        normalCopyEqualsNormal = Capture(() => normalCopy.Equals(normal)),
        nullCopyEqualsNormal = Capture(() => nullCopy.Equals(normal)),
        normalEqualsNullCopy = Capture(() => normal.Equals(nullCopy)),
        nullCopiesEqual = Capture(() => nullCopy.Equals(secondNullCopy)),
        nullCopiesHaveEqualHashes = Capture(() => nullCopy.GetHashCode() == secondNullCopy.GetHashCode())
    });

    object Snapshot(T value) => new
    {
        discriminator = discriminator(value),
        get = Capture(() => get(value)),
        branches = branches.Select(branch => new { branch.Name, result = Capture(() => branch.Read(value)) }).ToArray()
    };
}

static object Capture(Func<object> read)
{
    try
    {
        return new { outcome = "returned", value = read() };
    }
    catch (Exception exception)
    {
        return new { outcome = "threw", type = exception.GetType().FullName, exception.Message };
    }
}
