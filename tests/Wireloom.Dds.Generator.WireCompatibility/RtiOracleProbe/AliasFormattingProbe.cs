using System.Text.Json;
using RuntimeAliases;
using Rti.Types;

// Compile this typed probe against either RTI or Wireloom output for the same fixture.
// Collect every outcome so one failing shape cannot hide the remaining observations.
var results = new List<object>();

// Arrange
var child = new Child(17, "hello");
var cases = new (string Shape, Func<string> Format)[]
{
    ("scalar", () => new Scalar(17).ToString()),
    ("scalar-chain", () => new ScalarChain(17).ToString()),
    ("enum", () => new EnumAlias(State.Ready).ToString()),
    ("enum-chain", () => new EnumChain(State.Ready).ToString()),
    ("text", () => new Text("hello").ToString()),
    ("wide-text", () => new WideText("世界").ToString()),
    ("numbers", () => new Numbers(new Sequence<int> { 17 }).ToString()),
    ("numbers-chain", () => new NumbersChain(new Numbers(new Sequence<int> { 17 })).ToString()),
    ("children", () => new Children(new Sequence<Child> { child }).ToString()),
    ("texts", () => new Texts(new Sequence<string> { "hello" }).ToString()),
    ("wide-texts", () => new WideTexts(new Sequence<string> { "世界" }).ToString()),
    ("vector", () => new Vector([17, 18]).ToString()),
    ("matrix", () => new Matrix(new int[,] { { 17, 18, 19 }, { 20, 21, 22 } }).ToString()),
    ("matrix-chain", () => new MatrixChain(new Matrix(new int[,] { { 17, 18, 19 }, { 20, 21, 22 } })).ToString()),
    ("points", () => new Points([child, new Child(18, "other")]).ToString()),
    ("child-control", () => child.ToString()),
    ("aggregate-control", () => new Aggregate(child).ToString()),
    ("aggregate-chain-control", () => new AggregateChain(child).ToString())
};

foreach (var (shape, format) in cases)
{
    // Act
    try
    {
        var text = format();
        results.Add(new { shape, outcome = "returned", text });
    }
    catch (Exception exception)
    {
        var causes = new List<object>();
        for (var cause = exception; cause != null; cause = cause.InnerException)
        {
            causes.Add(new { type = cause.GetType().FullName, cause.Message, cause.StackTrace });
        }

        results.Add(new { shape, outcome = "threw", causes });
    }
}

// Assert/report: this is a discovery probe; all outcomes are evidence, not passing contracts.
if (results.Count != 18)
{
    throw new InvalidOperationException("The probe must report all 15 F2 shapes and three controls.");
}

var report = JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
if (args.Length == 1)
{
    File.WriteAllText(args[0], report);
    Console.WriteLine($"Recorded {results.Count} alias formatting observations on .NET {Environment.Version} in {args[0]}.");
}
else
{
    Console.WriteLine(report);
}
