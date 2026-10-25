#:project ../../../../src/Wireloom.Dds.Generator/Wireloom.Dds.Generator.csproj
#:project ../../../../src/Wireloom.Dds.Compiler/Wireloom.Dds.Compiler.csproj
#:package Microsoft.CodeAnalysis.CSharp
#:property NoWarn=CS7022
#:property RestorePackagesWithLockFile=false

using Wireloom;

if (args.Length < 2)
{
    throw new ArgumentException("Usage: export-probe-idl.cs <fresh-output-directory> <idl> [<idl> ...]");
}

var output = Path.GetFullPath(args[0]);
if (Directory.Exists(output) && Directory.EnumerateFileSystemEntries(output).Any())
{
    throw new ArgumentException("Use an empty output directory so stale generated sources cannot contaminate the probe.");
}

var inputs = args.Skip(1).Select(path => new IdlInput(Path.GetFullPath(path), File.ReadAllText(path))).ToList();
var sources = IdlCompiler.CompileSources(inputs, CancellationToken.None);
Directory.CreateDirectory(output);

foreach (var source in sources.Values)
{
    File.WriteAllText(Path.Combine(output, source.HintName), source.Source);
}

Console.WriteLine($"Exported {sources.Count} Wireloom source files to {output}; compile them with the typed probe project.");
