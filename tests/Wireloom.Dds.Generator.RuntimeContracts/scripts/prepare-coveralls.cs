#:property NoWarn=CS7022
#:property RestorePackagesWithLockFile=false

using System.Xml.Linq;

if (args.Length < 2)
{
    throw new ArgumentException("Usage: prepare-coveralls.cs <repository-root> <report> [<report> ...]");
}

var repositoryRoot = Path.GetFullPath(args[0]);
var normalizedRoot = repositoryRoot.Replace('\\', '/').TrimEnd('/') + "/";
foreach (var reportPath in args.Skip(1))
{
    var report = XDocument.Load(reportPath);
    var classes = report.Descendants("class").ToArray();
    if (report.Root?.Name != "coverage" || classes.Length == 0)
    {
        throw new InvalidDataException($"Missing Cobertura source data: {reportPath}");
    }

    foreach (var type in classes)
    {
        var source = type.Attribute("filename") ?? throw new InvalidDataException($"Missing source filename: {reportPath}");
        var relativePath = source.Value.Replace('\\', '/');
        if (relativePath.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
        {
            relativePath = relativePath[normalizedRoot.Length..];
        }
        else
        {
            // A downloaded report can originate on another OS or runner workspace.
            const string fixtureMarker = "tests/Wireloom.Dds.Generator.RuntimeContracts/obj/";
            var markerIndex = relativePath.IndexOf("/" + fixtureMarker, StringComparison.Ordinal);
            if (markerIndex >= 0)
            {
                relativePath = relativePath[(markerIndex + 1)..];
            }
        }

        var localSource = Path.GetFullPath(Path.Combine(repositoryRoot, relativePath));
        if (!localSource.Replace('\\', '/').StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase) || !File.Exists(localSource))
        {
            throw new FileNotFoundException($"Coverage source is unavailable in the upload workspace: {source.Value}");
        }

        source.Value = Path.GetRelativePath(repositoryRoot, localSource).Replace('\\', '/');
    }

    report.Save(reportPath);
    Console.WriteLine($"Prepared {classes.Length} source entries: {reportPath}");
}
