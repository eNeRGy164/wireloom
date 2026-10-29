using System.Globalization;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Wireloom.Roslyn;

/// <summary>Creates compiler inputs from Roslyn additional files and metadata.</summary>
internal static class GeneratorInputFactory
{
    internal static GeneratorInput Create((AdditionalText File, AnalyzerConfigOptionsProvider Options) inputAndOptions, CancellationToken cancellationToken)
    {
        var input = inputAndOptions.File;
        var options = inputAndOptions.Options.GetOptions(input);
        options.TryGetValue("build_metadata.AdditionalFiles.Generate", out var generate);
        options.TryGetValue("build_metadata.AdditionalFiles.Strict", out var strict);
        options.TryGetValue("build_metadata.AdditionalFiles.Defines", out var defines);
        options.TryGetValue("build_metadata.AdditionalFiles.Undefines", out var undefines);
        options.TryGetValue("build_metadata.AdditionalFiles.IncludeDirectories", out var includeDirectories);
        options.TryGetValue("build_metadata.AdditionalFiles.SequenceArrayMapping", out var sequenceArrayMapping);
        options.TryGetValue("build_metadata.AdditionalFiles.SequenceSize", out var sequenceSize);
        options.TryGetValue("build_metadata.AdditionalFiles.StringSize", out var stringSize);
        options.TryGetValue("build_metadata.AdditionalFiles.UnboundedSupport", out var unboundedSupportSetting);

        if (string.IsNullOrWhiteSpace(sequenceArrayMapping))
        {
            inputAndOptions.Options.GlobalOptions.TryGetValue("build_property.DdsIdlSequenceArrayMapping", out sequenceArrayMapping);
        }

        inputAndOptions.Options.GlobalOptions.TryGetValue("build_property.DdsIdlSequenceSize", out var projectSequenceSize);
        inputAndOptions.Options.GlobalOptions.TryGetValue("build_property.DdsIdlStringSize", out var projectStringSize);
        inputAndOptions.Options.GlobalOptions.TryGetValue("build_property.DdsIdlEnableEscapeChar", out var enableEscapeCharSetting);
        inputAndOptions.Options.GlobalOptions.TryGetValue("build_property.DdsIdlUnboundedSupport", out var projectUnboundedSupportSetting);
        var (enableEscapeChar, invalidEnableEscapeCharValue) = ParseBooleanSetting(enableEscapeCharSetting);
        var (unboundedSupport, invalidUnboundedSupportValue) = ParseBooleanSetting(
            string.IsNullOrWhiteSpace(unboundedSupportSetting) ? projectUnboundedSupportSetting : unboundedSupportSetting);
        var isGenerationRoot = string.Equals(generate, "true", StringComparison.OrdinalIgnoreCase);
        string? invalidBooleanSettingName = null;
        if (invalidEnableEscapeCharValue is not null && isGenerationRoot)
        {
            invalidBooleanSettingName = "DdsIdlEnableEscapeChar";
        }
        else if (invalidUnboundedSupportValue is not null)
        {
            invalidBooleanSettingName = string.IsNullOrWhiteSpace(unboundedSupportSetting)
                ? "DdsIdlUnboundedSupport"
                : "UnboundedSupport metadata";
        }

        return new GeneratorInput(
            new IdlInput(
                input.Path,
                input.GetText(cancellationToken)?.ToString() ?? string.Empty,
                ParseSequenceArrayMapping(sequenceArrayMapping),
                generate: isGenerationRoot,
                strict: string.Equals(strict, "true", StringComparison.OrdinalIgnoreCase),
                defines: ParseSymbols(defines),
                undefines: ParseSymbols(undefines),
                includeDirectories: ParseIncludeDirectories(includeDirectories),
                sequenceSize: ParsePositiveSize(sequenceSize, projectSequenceSize, 100),
                stringSize: ParsePositiveSize(stringSize, projectStringSize, 255),
                enableEscapeChar: enableEscapeChar,
                unboundedSupport: unboundedSupport),
            invalidBooleanSettingName);
    }

    private static IdlSequenceArrayMapping ParseSequenceArrayMapping(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || string.Equals(value!.Trim(), "RtiCSharp", StringComparison.OrdinalIgnoreCase))
        {
            return IdlSequenceArrayMapping.RtiCSharp;
        }

        if (string.Equals(value.Trim(), "Idl", StringComparison.OrdinalIgnoreCase))
        {
            return IdlSequenceArrayMapping.Idl;
        }

        // Keep invalid configuration inside the compiler's source-located diagnostic boundary.
        return (IdlSequenceArrayMapping)(-1);
    }

    private static List<string> ParseSymbols(string? value) => ParseValues(value, [';']);

    private static List<string> ParseIncludeDirectories(string? value) => ParseValues(value, [';', '|']);

    private static int ParsePositiveSize(string? fileValue, string? projectValue, int defaultValue)
    {
        var value = string.IsNullOrWhiteSpace(fileValue) ? projectValue : fileValue;
        if (string.IsNullOrWhiteSpace(value))
        {
            return defaultValue;
        }

        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) && parsed > 0
            ? parsed
            : 0;
    }

    private static (bool Value, string? InvalidValue) ParseBooleanSetting(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return (false, null);
        }

        if (bool.TryParse(value!.Trim(), out var parsed))
        {
            return (parsed, null);
        }

        return (false, value);
    }

    private static List<string> ParseValues(string? value, char[] separators)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return [.. value!.Split(separators, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).Where(s => s.Length > 0)];
    }
}

/// <summary>Pairs a resolved compiler input with an invalid Boolean setting name, if any.</summary>
internal sealed class GeneratorInput(IdlInput input, string? invalidBooleanSettingName)
{
    /// <summary>Gets the compiler input resolved from project and file settings.</summary>
    internal IdlInput Input { get; } = input;

    /// <summary>Gets the setting name used in a source-located configuration diagnostic.</summary>
    internal string? InvalidBooleanSettingName { get; } = invalidBooleanSettingName;
}
