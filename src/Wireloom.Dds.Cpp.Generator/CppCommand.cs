using System.Security.Cryptography;
using Wireloom.Dds.Idl.Compiler.FrontEnd.Preprocessing;

using Wireloom.Dds.Cpp.Emission;

namespace Wireloom.Dds.Cpp.Generator;

/// <summary>Runs standalone IDL generation for the supported Modern C++ profile.</summary>
internal static class CppCommand
{
    private const string Usage =
        "Usage: Wireloom.Dds.Cpp generate <idl-file> <output-directory> " +
        "[--define <symbol>] [--undefine <symbol>] [--include-directory <directory>] " +
        "[--sequence-size <size>] [--string-size <size>] [--strict] " +
        "[--unbounded-support|--finite-support] [--enable-escape-char] " +
        "[--file-sequence-size <path> <size>] [--file-string-size <path> <size>] " +
        "[--file-unbounded-support <path> <true|false>] " +
        "[--rti-version <version>]";

    /// <summary>Dispatches the command and reports diagnostics to the caller.</summary>
    /// <param name="arguments">Arguments passed to the CLI.</param>
    /// <param name="output">Writer for successful informational output.</param>
    /// <param name="error">Writer for usage errors and diagnostics.</param>
    /// <returns>Zero on success, one on generation failure, or two on invalid usage.</returns>
    internal static int Run(string[] arguments, TextWriter output, TextWriter error)
    {
        if (arguments is ["--help"] or ["-h"])
        {
            output.WriteLine(Usage);
            return 0;
        }

        if (arguments.Length < 3 || arguments[0] != "generate")
        {
            error.WriteLine(Usage);
            return 2;
        }

        try
        {
            var idlPath = Path.GetFullPath(arguments[1]);
            var outputPath = Path.GetFullPath(arguments[2]);
            var defines = new List<string>();
            var undefines = new List<string>();
            var includeDirectories = new List<string>();
            var unboundedSupport = false;
            var enableEscapeChar = false;
            var strict = false;
            var sequenceSize = 100;
            var stringSize = 255;
            var runtimeVersion = "7.7.0";
            var pathComparer = Path.DirectorySeparatorChar == '\\'
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal;
            var fileSequenceSizes = new Dictionary<string, int>(pathComparer);
            var fileStringSizes = new Dictionary<string, int>(pathComparer);
            var fileUnboundedSupport = new Dictionary<string, bool>(pathComparer);
            for (var index = 3; index < arguments.Length; index++)
            {
                if ((arguments[index] is "--file-sequence-size" or "--file-string-size" or "--file-unbounded-support")
                    && index + 2 < arguments.Length)
                {
                    var option = arguments[index++];
                    var path = Path.GetFullPath(arguments[index++]);
                    var value = arguments[index];
                    if (option == "--file-unbounded-support")
                    {
                        if (!bool.TryParse(value, out var enabled))
                        {
                            error.WriteLine("File unbounded support must be true or false.");
                            return 2;
                        }

                        fileUnboundedSupport[path] = enabled;
                    }
                    else if (!int.TryParse(value, out var size) || size <= 0)
                    {
                        error.WriteLine("File sizes must be positive 32-bit integers.");
                        return 2;
                    }
                    else if (option == "--file-sequence-size")
                    {
                        fileSequenceSizes[path] = size;
                    }
                    else
                    {
                        fileStringSizes[path] = size;
                    }

                    continue;
                }

                if (arguments[index] == "--unbounded-support")
                {
                    unboundedSupport = true;
                    continue;
                }

                if (arguments[index] == "--finite-support")
                {
                    unboundedSupport = false;
                    continue;
                }

                if (arguments[index] == "--enable-escape-char")
                {
                    enableEscapeChar = true;
                    continue;
                }

                if (arguments[index] == "--strict")
                {
                    strict = true;
                    continue;
                }

                if ((arguments[index] is "--define" or "--undefine" or "--include-directory" or "--sequence-size" or "--string-size" or "--rti-version")
                    && index + 1 < arguments.Length)
                {
                    var value = arguments[++index];
                    if (arguments[index - 1] == "--define")
                    {
                        defines.Add(value);
                    }
                    else if (arguments[index - 1] == "--include-directory")
                    {
                        includeDirectories.Add(Path.GetFullPath(value));
                    }
                    else if (arguments[index - 1] == "--undefine")
                    {
                        undefines.Add(value);
                    }
                    else if (arguments[index - 1] == "--sequence-size")
                    {
                        if (!int.TryParse(value, out sequenceSize) || sequenceSize <= 0)
                        {
                            error.WriteLine("Sequence size must be a positive 32-bit integer.");
                            return 2;
                        }
                    }
                    else if (arguments[index - 1] == "--string-size")
                    {
                        if (!int.TryParse(value, out stringSize) || stringSize <= 0)
                        {
                            error.WriteLine("String size must be a positive 32-bit integer.");
                            return 2;
                        }
                    }
                    else
                    {
                        runtimeVersion = value;
                    }

                    continue;
                }

                error.WriteLine(Usage);
                return 2;
            }

            if (!Version.TryParse(runtimeVersion, out var parsedRuntimeVersion)
                || parsedRuntimeVersion.Build < 0)
            {
                error.WriteLine($"Invalid RTI runtime version '{runtimeVersion}'. Use a version such as 6.1.2, 7.3.1, or 7.7.0.");
                error.WriteLine(Usage);
                return 2;
            }

            RtiApiProfile profile;
            try
            {
                profile = RtiApiProfile.GetForRuntimeVersion(parsedRuntimeVersion);
            }
            catch (ArgumentOutOfRangeException exception)
            {
                error.WriteLine(exception.Message);
                error.WriteLine(Usage);
                return 2;
            }

            var inputs = LoadInputClosure(
                idlPath,
                defines,
                undefines,
                includeDirectories,
                unboundedSupport,
                enableEscapeChar,
                strict,
                sequenceSize,
                stringSize,
                fileSequenceSizes,
                fileStringSizes,
                fileUnboundedSupport);
            return Generate(inputs, outputPath, error, profile);
        }
        catch (Exception exception) when (exception is IOException
                                           or UnauthorizedAccessException
                                           or ArgumentException
                                           or NotSupportedException)
        {
            error.WriteLine($"Wireloom C++: {exception.Message}");
            return 1;
        }
    }

    private static IReadOnlyList<IdlInput> LoadInputClosure(
        string idlPath,
        IReadOnlyList<string> defines,
        IReadOnlyList<string> undefines,
        IReadOnlyList<string> includeDirectories,
        bool unboundedSupport,
        bool enableEscapeChar,
        bool strict,
        int sequenceSize,
        int stringSize,
        IReadOnlyDictionary<string, int> fileSequenceSizes,
        IReadOnlyDictionary<string, int> fileStringSizes,
        IReadOnlyDictionary<string, bool> fileUnboundedSupport)
    {
        var rootSequenceSize = fileSequenceSizes.GetValueOrDefault(idlPath, sequenceSize);
        var rootStringSize = fileStringSizes.GetValueOrDefault(idlPath, stringSize);
        var rootUnboundedSupport = fileUnboundedSupport.GetValueOrDefault(idlPath, unboundedSupport);
        var root = new IdlInput(idlPath, File.ReadAllText(idlPath), defines: defines, undefines: undefines,
            includeDirectories: includeDirectories, unboundedSupport: rootUnboundedSupport, strict: strict,
            sequenceSize: rootSequenceSize, stringSize: rootStringSize, enableEscapeChar: enableEscapeChar);
        var comparer = Path.DirectorySeparatorChar == '\\'
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
        var inputs = new Dictionary<string, IdlInput>(comparer)
        {
            [idlPath] = root,
        };
        var preprocessor = new IdlPreprocessor(defines, undefines, CancellationToken.None);
        var active = new HashSet<string>(comparer);
        var onceIncluded = new HashSet<string>(comparer);

        try
        {
            Visit(root);
        }
        catch (IdlException)
        {
            // The shared analysis pass reports preprocessing failures with the normal diagnostics.
        }

        return inputs.Values.ToArray();

        void Visit(IdlInput including)
        {
            var includingPath = Path.GetFullPath(including.Path);
            if (onceIncluded.Contains(includingPath) || !active.Add(includingPath))
            {
                return;
            }

            try
            {
                preprocessor.ProcessWithMetadata(
                    including,
                    (includeName, isAngle, _) =>
                    {
                        var path = FindIncludePath(including.Path, includeName, isAngle, includeDirectories);
                        if (path is null)
                        {
                            return;
                        }

                        if (!inputs.TryGetValue(path, out var child))
                        {
                            var fileSequenceSize = fileSequenceSizes.GetValueOrDefault(path, sequenceSize);
                            var fileStringSize = fileStringSizes.GetValueOrDefault(path, stringSize);
                            var fileSupportsUnbounded = fileUnboundedSupport.GetValueOrDefault(path, unboundedSupport);
                            child = new IdlInput(path, File.ReadAllText(path), generate: false,
                                includeDirectories: includeDirectories, unboundedSupport: fileSupportsUnbounded,
                                sequenceSize: fileSequenceSize, stringSize: fileStringSize, enableEscapeChar: enableEscapeChar);
                            inputs.Add(path, child);
                        }

                        Visit(child);
                    },
                    pragmaOnceEncountered: () => onceIncluded.Add(includingPath),
                    includeProbe: (includeName, isAngle) =>
                        FindIncludePath(including.Path, includeName, isAngle, includeDirectories) is not null);
            }
            finally
            {
                active.Remove(includingPath);
            }
        }
    }

    private static string? FindIncludePath(
        string includingPath,
        string includeName,
        bool isAngle,
        IReadOnlyList<string> includeDirectories)
    {
        if (string.IsNullOrWhiteSpace(includeName)
            || includeName.IndexOf('\0') >= 0
            || Path.IsPathRooted(includeName))
        {
            return null;
        }

        var includingDirectory = Path.GetDirectoryName(includingPath);
        var searchDirectories = isAngle
            ? includeDirectories
            : new[] { includingDirectory! }.Concat(includeDirectories);

        foreach (var directory in searchDirectories)
        {
            var path = Path.GetFullPath(Path.Combine(directory, includeName));
            if (File.Exists(path))
            {
                return path;
            }
        }

        return null;
    }

    private static int Generate(
        IReadOnlyList<IdlInput> inputs, string outputPath, TextWriter error, RtiApiProfile profile)
    {
        var diagnostics = new List<IdlDiagnostic>();
        var files = Compiler.Compile(inputs, MD5.HashData, diagnostics, profile, CancellationToken.None);

        foreach (var diagnostic in diagnostics)
        {
            var offset = Math.Clamp(diagnostic.Offset, 0, diagnostic.SourceText.Length);
            var prefix = diagnostic.SourceText.AsSpan(0, offset);
            var line = 1 + prefix.Count('\n');
            var column = prefix.Length - prefix.LastIndexOf('\n');
            error.WriteLine($"{diagnostic.Path}({line},{column}): {diagnostic.Severity} {diagnostic.Id}: {diagnostic.Message}");
        }

        if (HasErrors(diagnostics))
        {
            return 1;
        }

        Directory.CreateDirectory(outputPath);
        foreach (var file in files)
        {
            var filePath = Path.Combine(outputPath, file.FileName);
            if (File.Exists(filePath)
                && string.Equals(File.ReadAllText(filePath), file.Source, StringComparison.Ordinal))
            {
                continue;
            }

            File.WriteAllText(filePath, file.Source);
        }

        return 0;
    }

    private static bool HasErrors(IEnumerable<IdlDiagnostic> diagnostics) =>
        diagnostics.Any(diagnostic => diagnostic.Severity == IdlDiagnosticSeverity.Error);
}
