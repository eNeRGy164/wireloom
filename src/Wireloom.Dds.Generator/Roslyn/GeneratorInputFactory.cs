using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Wireloom.Roslyn;

/// <summary>Creates compiler inputs from Roslyn additional files and metadata.</summary>
internal static class GeneratorInputFactory
{
    internal static IdlInput Create((AdditionalText File, AnalyzerConfigOptionsProvider Options) inputAndOptions, CancellationToken cancellationToken)
    {
        var input = inputAndOptions.File;
        var options = inputAndOptions.Options.GetOptions(input);
        options.TryGetValue("build_metadata.AdditionalFiles.Generate", out var generate);
        options.TryGetValue("build_metadata.AdditionalFiles.Strict", out var strict);
        options.TryGetValue("build_metadata.AdditionalFiles.Defines", out var defines);
        options.TryGetValue("build_metadata.AdditionalFiles.Undefines", out var undefines);
        options.TryGetValue("build_metadata.AdditionalFiles.IncludeDirectories", out var includeDirectories);

        return new(
            input.Path,
            input.GetText(cancellationToken)?.ToString() ?? string.Empty,
            string.Equals(generate, "true", StringComparison.OrdinalIgnoreCase),
            string.Equals(strict, "true", StringComparison.OrdinalIgnoreCase),
            ParseSymbols(defines),
            ParseSymbols(undefines),
            ParseSymbols(includeDirectories));
    }

    private static List<string> ParseSymbols(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? []
            : [.. value!.Split([';'], StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s.Length > 0)];
}
