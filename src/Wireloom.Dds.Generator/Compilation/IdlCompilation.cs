namespace Wireloom;

/// <summary>Coordinates input traversal, declaration parsing, and emission.</summary>
internal sealed class IdlCompilation(IReadOnlyList<IdlInput> inputs, CancellationToken cancellationToken)
{
    public IReadOnlyList<GeneratedIdlSource> Run()
    {
        var comparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        var files = new Dictionary<string, IdlInput>(comparer);

        foreach (var file in inputs)
        {
            files[Path.GetFullPath(file.Path)] = file;
        }

        var symbols = new IdlSymbolTable();
        var roots = inputs.Where(input => input.Generate).ToArray();
        var graph = new IdlInputGraph(files, roots.SelectMany(input => input.Defines), roots.SelectMany(input => input.Undefines), roots.SelectMany(input => input.IncludeDirectories), cancellationToken);
        var parser = new IdlDeclarationParser(symbols, cancellationToken);
        var compilation = new CompilationState(parser, symbols, inputs.Any(input => input.Generate && input.Strict));

        // Roots are sorted by path so generation is stable even when the build
        // system supplies AdditionalFiles in a different order.
        foreach (var input in roots.OrderBy(input => input.Path, StringComparer.Ordinal))
        {
            graph.Visit(input, parser.Parse);
        }

        compilation.EmitDeclarations();

        return compilation.Sources;
    }
}
