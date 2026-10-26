using System.Security.Cryptography;
using System.Text.Json;

namespace Wireloom.Compiler.Tests.Playground;

public sealed class PlaygroundCompilerAdapterSpecs
{
    private static readonly JsonSerializerOptions WebJson = new(JsonSerializerDefaults.Web);

    [Theory]
    [InlineData("7.3.1")]
    [InlineData("7.7.0")]
    public void CompilesSelectedProfileAndPreservesTheRequestIdentity(string profile)
    {
        // Arrange
        var hashCalls = 0;
        var request = CreateRequest("@autoid(HASH) struct Sample { long value; };", profile);

        // Act
        var json = PlaygroundCompilerAdapter.Compile(request, bytes =>
        {
            hashCalls++;
            return MD5.HashData(bytes);
        });

        // Assert
        using var response = JsonDocument.Parse(json);
        response.RootElement.GetProperty("kind").GetString().ShouldBe("compiler");
        response.RootElement.GetProperty("success").GetBoolean().ShouldBeTrue();
        response.RootElement.GetProperty("requestId").GetInt64().ShouldBe(4);
        response.RootElement.GetProperty("workerGeneration").GetInt32().ShouldBe(2);
        response.RootElement.GetProperty("workspaceRevision").GetInt64().ShouldBe(8);
        response.RootElement.GetProperty("profile").GetString().ShouldBe(profile);
        response.RootElement.GetProperty("documents").GetArrayLength().ShouldBeGreaterThan(0);
        hashCalls.ShouldBeGreaterThan(0);
    }

    [Fact]
    public void ConvertsCompilerDiagnosticsAndKeepsFailedOutputEmpty()
    {
        // Arrange
        const string source = "#include \"missing.idl\"\n";

        // Act
        var json = PlaygroundCompilerAdapter.Compile(CreateRequest(source, "7.7.0"), MD5.HashData);

        // Assert
        using var response = JsonDocument.Parse(json);
        response.RootElement.GetProperty("kind").GetString().ShouldBe("compiler");
        response.RootElement.GetProperty("success").GetBoolean().ShouldBeFalse();
        response.RootElement.GetProperty("documents").GetArrayLength().ShouldBe(0);

        var diagnostic = response.RootElement.GetProperty("diagnostics").EnumerateArray().Single();
        diagnostic.GetProperty("id").GetString().ShouldBe("DDSG0001");
        diagnostic.GetProperty("severity").GetString().ShouldBe("Error");
        diagnostic.GetProperty("filePath").GetString().ShouldBe("/workspace/main.idl");
        diagnostic.GetProperty("startOffset").GetInt32().ShouldBe(0);
    }

    [Theory]
    [InlineData("protocolVersion", 2)]
    [InlineData("workerGeneration", 0)]
    [InlineData("requestId", 0)]
    [InlineData("workspaceRevision", -1)]
    public void RejectsInvalidRequestIdentity(string property, long value)
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request[property] = value;

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsAnEmptyWorkspace()
    {
        // Arrange
        var empty = RequestObject("struct Sample { long value; };");
        empty["files"] = Array.Empty<object>();

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(empty));
    }

    [Fact]
    public void RejectsAWorkspaceWithMoreThan512Files()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["files"] = Enumerable.Range(0, 513)
            .Select(index => File($"/workspace/{index}.idl", "", index == 0))
            .ToArray();

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsNullWorkspaceOptions()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["options"] = null;

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("relative.idl")]
    [InlineData("C:\\workspace\\main.idl")]
    [InlineData("/workspace/main.idl/")]
    [InlineData("/workspace//main.idl")]
    [InlineData("/workspace/./main.idl")]
    [InlineData("/workspace/../main.idl")]
    public void RejectsInvalidAndUnnormalizedPaths(string? path)
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["files"] = new[] { File(path, "struct Sample { long value; };", true) };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsDuplicateWorkspacePaths()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["files"] = new[] { File("/workspace/main.idl", "", true), File("/workspace/main.idl", "", false) };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsNullWorkspaceFileEntries()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["files"] = new object?[] { null };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsNullWorkspaceFileSource()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["files"] = new[] { File("/workspace/main.idl", null, true) };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void AcceptsTheUtf8InputLimit()
    {
        // Arrange
        var request = RequestObject(new string('é', 128 * 1024));

        // Act
        var result = JsonDocument.Parse(Compile(request));

        // Assert
        result.RootElement.GetProperty("kind").GetString().ShouldBe("compiler");
    }

    [Fact]
    public void RejectsUtf8InputOneByteOverTheLimit()
    {
        // Arrange
        var request = RequestObject(new string('é', 128 * 1024) + "a");

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsAWorkspaceWithoutAGenerationRoot()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        ((Dictionary<string, object?>[])request["files"]!)[0]["isGenerationRoot"] = false;

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsAnUnrecognizedProfile()
    {
        // Arrange
        var request = RequestObject("struct Sample { long value; };");
        request["profile"] = "7.8.0";

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void ReturnsALimitResponseWhenDocumentCountExceeds512()
    {
        // Arrange
        var request = new CompileRequest(1, 1, 1, 1, "7.7.0", [], new WorkspaceOptions());
        var tooMany = Enumerable.Range(0, 513).Select(index => new DocumentDto($"{index}.cs", "x")).ToArray();
        var diagnostics = Array.Empty<DiagnosticDto>();

        // Act
        var countResult = JsonDocument.Parse(PlaygroundCompilerAdapter.SerializeResult(request, true, tooMany, diagnostics));

        // Assert
        countResult.RootElement.GetProperty("kind").GetString().ShouldBe("limit");
        countResult.RootElement.GetProperty("documents").GetArrayLength().ShouldBe(0);
    }

    [Fact]
    public void ReturnsALimitResponseWhenGeneratedOutputExceeds16MiB()
    {
        // Arrange
        var request = new CompileRequest(1, 1, 1, 1, "7.7.0", [], new WorkspaceOptions());
        var tooLarge = new[] { new DocumentDto("large.cs", new string('x', 16 * 1024 * 1024 + 1)) };
        var diagnostics = Array.Empty<DiagnosticDto>();

        // Act
        var bytesResult = JsonDocument.Parse(PlaygroundCompilerAdapter.SerializeResult(request, true, tooLarge, diagnostics));

        // Assert
        bytesResult.RootElement.GetProperty("kind").GetString().ShouldBe("limit");
        bytesResult.RootElement.GetProperty("documents").GetArrayLength().ShouldBe(0);
    }

    [Fact]
    public void PropagatesHostProviderFailureInsteadOfReturningPartialOutput()
    {
        // Arrange
        var request = CreateRequest("@autoid(HASH) struct Sample { long value; };", "7.7.0");

        // Act and Assert
        Should.Throw<InvalidOperationException>(() => PlaygroundCompilerAdapter.Compile(request, _ => throw new InvalidOperationException("provider offline")));
    }

    private static string Compile(Dictionary<string, object?> request)
    {
        return PlaygroundCompilerAdapter.Compile(JsonSerializer.Serialize(request), MD5.HashData);
    }

    private static string CreateRequest(string source, string profile)
    {
        var request = RequestObject(source);
        request["profile"] = profile;
        return JsonSerializer.Serialize(request, WebJson);
    }

    private static Dictionary<string, object?> RequestObject(string? source)
    {
        return new Dictionary<string, object?>
        {
            ["protocolVersion"] = 1,
            ["workerGeneration"] = 2,
            ["requestId"] = 4,
            ["workspaceRevision"] = 8,
            ["profile"] = "7.7.0",
            ["files"] = new[] { File("/workspace/main.idl", source, true) },
            ["options"] = new { },
        };
    }

    private static Dictionary<string, object?> File(string? path, string? source, bool root)
    {
        return new Dictionary<string, object?>
        {
            ["path"] = path,
            ["source"] = source,
            ["isGenerationRoot"] = root,
        };
    }
}
