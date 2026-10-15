#:property NoWarn=CS7022
#:property RestorePackagesWithLockFile=false

using System.Globalization;
using System.Xml.Linq;

if (args.Length != 1)
{
    Console.Error.WriteLine("Usage: dotnet run --file scripts/verify-coverage.cs -- <coverage.cobertura.xml>");
    return 1;
}

var report = XDocument.Load(args[0]);
var root = report.Root ?? throw new InvalidDataException("Missing Cobertura root.");
var classes = root.Descendants("class").ToArray();
if (classes.Length == 0 || classes.Any(type => !((string?)type.Attribute("filename") ?? "").EndsWith(".g.cs", StringComparison.Ordinal)))
{
    throw new InvalidDataException("The report must contain generated fixture sources only.");
}

foreach (var counter in new[] { "branches-valid", "branches-covered", "lines-valid", "lines-covered" })
{
    if (!long.TryParse((string?)root.Attribute(counter), CultureInfo.InvariantCulture, out var value) || value <= 0)
    {
        throw new InvalidDataException($"Missing or empty coverage counter: {counter}");
    }
}

foreach (var family in new[] { "RuntimeManaged.", "RuntimeUnions.", "RuntimeAliases.", "RuntimeDiscriminators.", "RuntimeSequenceCopy." })
{
    if (!classes.Any(type => ((string?)type.Attribute("name") ?? "").StartsWith(family, StringComparison.Ordinal)))
    {
        throw new InvalidDataException($"Missing generated family: {family}");
    }
}

Console.WriteLine($"Generated classes: {classes.Length}");
Console.WriteLine($"Branches: {root.Attribute("branches-covered")?.Value}/{root.Attribute("branches-valid")?.Value} ({root.Attribute("branch-rate")?.Value})");
Console.WriteLine($"Lines: {root.Attribute("lines-covered")?.Value}/{root.Attribute("lines-valid")?.Value} ({root.Attribute("line-rate")?.Value})");
return 0;
