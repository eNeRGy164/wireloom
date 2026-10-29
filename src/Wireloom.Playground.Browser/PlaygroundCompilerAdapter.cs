using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wireloom;

/// <summary>Adapts immutable browser workspace snapshots to the shared compiler.</summary>
internal static class PlaygroundCompilerAdapter
{
    /// <summary>The worker protocol shipped atomically with this adapter.</summary>
    private const int ProtocolVersion = 4;
    private const int MaxSourceBytes = 256 * 1024;
    private const int MaxDocuments = 512;
    private const int MaxGeneratedBytes = 16 * 1024 * 1024;

    /// <summary>Validates a workspace request, compiles its files, and serializes the result.</summary>
    /// <param name="requestJson">The JSON-encoded immutable workspace request.</param>
    /// <param name="computeMd5">The synchronous per-compilation MD5 provider.</param>
    /// <returns>The JSON-encoded compiler response.</returns>
    /// <exception cref="ArgumentException">The protocol identity, profile, file paths, or input limits are invalid.</exception>
    /// <exception cref="InvalidOperationException">The host MD5 provider fails or returns an invalid digest.</exception>
    public static string Compile(string requestJson, Func<byte[], byte[]> computeMd5)
    {
        var request = JsonSerializer.Deserialize(requestJson, PlaygroundJsonContext.Default.CompileRequest)
            ?? throw new ArgumentException("Request is empty.", nameof(requestJson));
        if (request.ProtocolVersion != ProtocolVersion
            || request.RequestId <= 0
            || request.WorkerGeneration <= 0
            || request.WorkspaceRevision < 0)
        {
            throw new ArgumentException("Request identity or protocol version is invalid.", nameof(requestJson));
        }

        if (request.Files is null || request.Files.Length == 0 || request.Files.Length > 512)
        {
            throw new ArgumentException("Workspace must contain between 1 and 512 files.", nameof(requestJson));
        }

        if (request.Options is null)
        {
            throw new ArgumentException("Workspace options cannot be null.", nameof(requestJson));
        }

        ValidateOptions(request.Options);
        var includeDirectories = request.Options.IncludeDirectories
            .Distinct(StringComparer.Ordinal)
            .Order(StringComparer.Ordinal)
            .ToArray();

        var totalSourceBytes = 0;
        var files = new IdlInput[request.Files.Length];
        var seenPaths = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < request.Files.Length; index++)
        {
            var file = request.Files[index];
            if (file is null || !ValidPath(file.Path))
            {
                throw new ArgumentException("File paths must be rooted under /workspace/.", nameof(requestJson));
            }

            if (!seenPaths.Add(file.Path))
            {
                throw new ArgumentException("Workspace paths must be normalized and unique.", nameof(requestJson));
            }

            if (file.Source is null)
            {
                throw new ArgumentException("File source text cannot be null.", nameof(requestJson));
            }

            totalSourceBytes += Encoding.UTF8.GetByteCount(file.Source);
            if (totalSourceBytes > MaxSourceBytes)
            {
                throw new ArgumentException("Workspace input exceeds the 256 KiB UTF-8 limit.", nameof(requestJson));
            }

            var options = file.Options ?? new FileOptions();
            var mapping = options.SequenceArrayMapping ?? request.Options.SequenceArrayMapping;
            var sequenceSize = options.SequenceSize ?? request.Options.SequenceSize;
            var stringSize = options.StringSize ?? request.Options.StringSize;
            var defines = options.Defines;
            var undefines = options.Undefines;
            if (sequenceSize <= 0 || stringSize <= 0
                || mapping is not ("RtiCSharp" or "Idl")
                || defines is null || undefines is null
                || defines.Length > 128 || undefines.Length > 128
                || defines.Any(InvalidSetting) || undefines.Any(InvalidSetting))
            {
                throw new ArgumentException("File settings are invalid.", nameof(requestJson));
            }

            files[index] = new IdlInput(
                file.Path,
                file.Source,
                mapping switch
                {
                    "Idl" => IdlSequenceArrayMapping.Idl,
                    _ => IdlSequenceArrayMapping.RtiCSharp,
                },
                generate: file.IsGenerationRoot,
                strict: file.IsGenerationRoot && options.Strict,
                defines: file.IsGenerationRoot ? defines : [],
                undefines: file.IsGenerationRoot ? undefines : [],
                includeDirectories: includeDirectories,
                sequenceSize: sequenceSize,
                stringSize: stringSize,
                enableEscapeChar: request.Options.EnableEscapeChar,
                unboundedSupport: options.UnboundedSupport ?? request.Options.UnboundedSupport);
        }

        if (seenPaths.Any(path => seenPaths.Any(other => other.StartsWith(path + "/", StringComparison.Ordinal))))
        {
            throw new ArgumentException("A file cannot also be a virtual folder.", nameof(requestJson));
        }

        if (!files.Any(file => file.Generate))
        {
            throw new ArgumentException("Workspace must include a generation root.", nameof(requestJson));
        }

        var profile = request.Profile switch
        {
            "7.3.1" => IdlCompilerProfile.Rti731,
            "7.7.0" => IdlCompilerProfile.Rti770,
            _ => throw new ArgumentException("Profile must be 7.3.1 or 7.7.0.", nameof(requestJson)),
        };
        var result = IdlCompiler.Compile(files, profile, computeMd5);
        var documents = result.Documents.Select(document => new DocumentDto(document.HintName, document.Source)).ToArray();
        var diagnostics = result.Diagnostics
            .Select(diagnostic => new DiagnosticDto(
                diagnostic.Id,
                diagnostic.Severity.ToString(),
                diagnostic.Message,
                diagnostic.Path,
                diagnostic.Offset,
                0))
            .ToArray();

        return SerializeResult(request, result.Success, documents, diagnostics);
    }

    private static bool ValidPath(string? path, bool allowRoot = false)
    {
        if (allowRoot && path == "/workspace")
        {
            return true;
        }

        if (path is null || !path.StartsWith("/workspace/", StringComparison.Ordinal)
            || path.Length > 1024 || path.Any(character => character == '\\' || char.IsControl(character)))
        {
            return false;
        }

        var segments = path["/workspace/".Length..].Split('/');
        return segments.Length <= 32 && segments.All(segment => segment is not ("" or "." or "..") && segment.Trim() == segment);
    }

    private static void ValidateOptions(WorkspaceOptions options)
    {
        foreach (var list in new[] { options.IncludeDirectories })
        {
            if (list is null || list.Length > 128
                || list.Any(entry => string.IsNullOrEmpty(entry) || entry.Length > 1024 || entry.Any(char.IsControl)))
            {
                throw new ArgumentException("Each settings list supports at most 128 non-empty entries of 1,024 characters.");
            }
        }

        if (options.IncludeDirectories.Any(path => !ValidPath(path, allowRoot: true)))
        {
            throw new ArgumentException("Include folders must be normalized virtual workspace paths.");
        }

        if (options.SequenceArrayMapping is not ("RtiCSharp" or "Idl")
            || options.SequenceSize <= 0 || options.StringSize <= 0)
        {
            throw new ArgumentException("Workspace sequence settings require a known mapping and positive 32-bit sizes.");
        }
    }

    private static bool InvalidSetting(string? entry) =>
        string.IsNullOrEmpty(entry) || entry.Length > 1024 || entry.Any(char.IsControl);

    /// <summary>Applies result bounds and serializes compiler documents and diagnostics.</summary>
    /// <param name="request">The immutable request that produced the result.</param>
    /// <param name="success">Whether compilation completed without errors.</param>
    /// <param name="documents">Compiler-provided generated documents.</param>
    /// <param name="diagnostics">Compiler-provided diagnostics.</param>
    /// <returns>A complete compiler result or a limit failure without partial output.</returns>
    // ReSharper disable once MemberCanBePrivate.Global
    // Source-linked tests call this seam directly to verify both output caps.
    internal static string SerializeResult(
        CompileRequest request,
        bool success,
        DocumentDto[] documents,
        DiagnosticDto[] diagnostics)
    {
        var generatedBytes = documents.Sum(document => (long)Encoding.UTF8.GetByteCount(document.Source));
        if (documents.Length > MaxDocuments || generatedBytes > MaxGeneratedBytes)
        {
            return JsonSerializer.Serialize(CompileResponse.Limit(request, "Generated output exceeds the browser result limit."), PlaygroundJsonContext.Default.CompileResponse);
        }

        var response = CompileResponse.Successful(request, success, documents, diagnostics);
        return JsonSerializer.Serialize(response, PlaygroundJsonContext.Default.CompileResponse);
    }
}

/// <summary>Identifies a versioned, immutable workspace compilation snapshot.</summary>
/// <param name="ProtocolVersion">The protocol version expected by the adapter.</param>
/// <param name="WorkerGeneration">The worker instance generation.</param>
/// <param name="RequestId">The monotonically increasing request identity.</param>
/// <param name="WorkspaceRevision">The immutable workspace revision.</param>
/// <param name="Profile">The selected RTI API profile.</param>
/// <param name="Files">The virtual workspace file collection.</param>
/// <param name="Options">The project defaults and batch-wide compiler settings.</param>
internal sealed record CompileRequest(
    int ProtocolVersion,
    int WorkerGeneration,
    long RequestId,
    long WorkspaceRevision,
    string Profile,
    WorkspaceFile?[] Files,
    WorkspaceOptions Options);

/// <summary>Describes one virtual workspace file and whether it is a generation root.</summary>
/// <param name="Path">The normalized virtual path under `/workspace/`.</param>
/// <param name="Source">The complete IDL source text.</param>
/// <param name="IsGenerationRoot">Whether this file starts generation.</param>
/// <param name="Options">The file's root settings and optional declaring-file overrides.</param>
internal sealed record WorkspaceFile(string Path, string Source, bool IsGenerationRoot, FileOptions? Options = null);

/// <summary>Configures project sequence defaults and batch-wide compiler settings.</summary>
internal sealed record WorkspaceOptions
{
    /// <summary>Gets the project default direct sequence-array mapping, RtiCSharp or Idl.</summary>
    public string SequenceArrayMapping { get; init; } = "RtiCSharp";

    /// <summary>Gets the project default unbounded sequence size.</summary>
    public int SequenceSize { get; init; } = 100;

    /// <summary>Gets the project default unbounded string size.</summary>
    public int StringSize { get; init; } = 255;

    /// <summary>Gets normalized virtual include folders, searched in ordinal alphabetical order.</summary>
    public string[] IncludeDirectories { get; init; } = [];

    /// <summary>Gets whether leading underscores escape identifiers across the batch.</summary>
    public bool EnableEscapeChar { get; init; }

    /// <summary>Gets whether project files default to RTI-style unbounded sequence and string semantics.</summary>
    public bool UnboundedSupport { get; init; }
}

/// <summary>Configures DdsIdl root metadata and optional declaring-file overrides.</summary>
internal sealed record FileOptions
{
    /// <summary>Gets external preprocessor definitions applied for this root.</summary>
    public string[]? Defines { get; init; } = [];

    /// <summary>Gets symbols removed after applying definitions for this root.</summary>
    public string[]? Undefines { get; init; } = [];

    /// <summary>Gets whether strict validation is enabled for this root.</summary>
    public bool Strict { get; init; }

    /// <summary>Gets an optional legacy direct sequence-array mapping override.</summary>
    public string? SequenceArrayMapping { get; init; }

    /// <summary>Gets an optional legacy unbounded sequence size override.</summary>
    public int? SequenceSize { get; init; }

    /// <summary>Gets an optional legacy unbounded string size override.</summary>
    public int? StringSize { get; init; }

    /// <summary>Gets an optional declaring-file override for unbounded sequence and string semantics.</summary>
    public bool? UnboundedSupport { get; init; }
}

/// <summary>Describes one generated C# document.</summary>
/// <param name="Name">The compiler-provided document name.</param>
/// <param name="Source">The complete compiler-provided C# source text.</param>
internal sealed record DocumentDto(string Name, string Source);

/// <summary>Describes one compiler diagnostic without copying its source text.</summary>
/// <param name="Id">The compiler-owned diagnostic identifier.</param>
/// <param name="Severity">The compiler-owned diagnostic severity.</param>
/// <param name="Message">The diagnostic message.</param>
/// <param name="FilePath">The associated virtual source path, if any.</param>
/// <param name="StartOffset">The UTF-16 offset within the source snapshot.</param>
/// <param name="Length">The span length; zero because the current compiler API exposes only the start offset.</param>
internal sealed record DiagnosticDto(
    string Id,
    string Severity,
    string Message,
    string? FilePath,
    int StartOffset,
    int Length);

/// <summary>Returns the identity and outcome of a workspace compilation.</summary>
/// <param name="ProtocolVersion">The protocol version used by the request.</param>
/// <param name="WorkerGeneration">The worker generation used by the request.</param>
/// <param name="RequestId">The request identity.</param>
/// <param name="WorkspaceRevision">The immutable workspace revision.</param>
/// <param name="Profile">The selected RTI API profile.</param>
/// <param name="Kind">The result kind, `compiler` or `limit`.</param>
/// <param name="Success">Whether the compiler result succeeded.</param>
/// <param name="Documents">The complete generated document set.</param>
/// <param name="Diagnostics">The compiler diagnostics without copied source text.</param>
/// <param name="Error">The host or limit error message, if any.</param>
internal sealed record CompileResponse(
    int ProtocolVersion,
    int WorkerGeneration,
    long RequestId,
    long WorkspaceRevision,
    string Profile,
    string Kind,
    bool Success,
    DocumentDto[] Documents,
    DiagnosticDto[] Diagnostics,
    string? Error)
{
    /// <summary>Creates a response from a compiler result.</summary>
    /// <param name="request">The request that produced the result.</param>
    /// <param name="success">Whether compilation succeeded.</param>
    /// <param name="documents">Every generated document.</param>
    /// <param name="diagnostics">Every compiler diagnostic.</param>
    /// <returns>A compiler response that retains the request identity.</returns>
    public static CompileResponse Successful(
        CompileRequest request,
        bool success,
        DocumentDto[] documents,
        DiagnosticDto[] diagnostics)
    {
        return new CompileResponse(
            request.ProtocolVersion,
            request.WorkerGeneration,
            request.RequestId,
            request.WorkspaceRevision,
            request.Profile,
            "compiler",
            success,
            documents,
            diagnostics,
            null);
    }

    /// <summary>Creates a response for a bounded-result failure.</summary>
    /// <param name="request">The request that exceeded a host limit.</param>
    /// <param name="message">The limit failure message.</param>
    /// <returns>A limit response that retains the request identity.</returns>
    public static CompileResponse Limit(CompileRequest request, string message)
    {
        return new CompileResponse(
            request.ProtocolVersion,
            request.WorkerGeneration,
            request.RequestId,
            request.WorkspaceRevision,
            request.Profile,
            "limit",
            false,
            [],
            [],
            message);
    }
}

/// <summary>Provides source-generated JSON metadata for the worker protocol.</summary>
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(CompileRequest))]
[JsonSerializable(typeof(CompileResponse))]
internal sealed partial class PlaygroundJsonContext : JsonSerializerContext;
