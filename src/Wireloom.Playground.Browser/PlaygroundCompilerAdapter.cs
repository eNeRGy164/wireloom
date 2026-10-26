using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Wireloom;

/// <summary>Adapts immutable browser workspace snapshots to the shared compiler.</summary>
internal static class PlaygroundCompilerAdapter
{
    private const int ProtocolVersion = 1;
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

        var totalSourceBytes = 0;
        var files = new IdlInput[request.Files.Length];
        var seenPaths = new HashSet<string>(StringComparer.Ordinal);
        for (var index = 0; index < request.Files.Length; index++)
        {
            var file = request.Files[index];
            if (file is null
                || string.IsNullOrWhiteSpace(file.Path)
                || !file.Path.StartsWith("/workspace/", StringComparison.Ordinal)
                || file.Path.Contains('\\'))
            {
                throw new ArgumentException("File paths must be rooted under /workspace/.", nameof(requestJson));
            }

            var relativePath = file.Path["/workspace/".Length..];
            var pathSegments = relativePath.Split('/');
            if (pathSegments.Any(segment => segment is "" or "." or "..") || !seenPaths.Add(file.Path))
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
                throw new ArgumentException("Workspace input exceeds the 256 KiB stage 0 limit.", nameof(requestJson));
            }

            files[index] = new IdlInput(file.Path, file.Source, generate: file.IsGenerationRoot);
        }

        if (!request.Files.Any(file => file is not null && file.IsGenerationRoot))
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
/// <param name="Options">The reserved workspace compiler-options shape.</param>
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
internal sealed record WorkspaceFile(string Path, string Source, bool IsGenerationRoot);

/// <summary>Reserves a protocol location for future compiler controls.</summary>
internal sealed record WorkspaceOptions;

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
