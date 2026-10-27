using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;

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
    [InlineData("protocolVersion", 1)]
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
        var request = new CompileRequest(3, 1, 1, 1, "7.7.0", [], new WorkspaceOptions());
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
        var request = new CompileRequest(3, 1, 1, 1, "7.7.0", [], new WorkspaceOptions());
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

    [Theory]
    [InlineData("7.3.1")]
    [InlineData("7.7.0")]
    public void AppliesWorkspaceAndDeclaringFileSettingsWithNativeParity(string profile)
    {
        // Arrange
        const string root = """
            #include <types.idl>
            #if defined(FEATURE) && !defined(DISABLED)
            struct _Sample { Shared::_Value value; };
            #endif
            """;
        const string shared = "module Shared { struct _Value { string name; sequence<long> values; }; };";
        var options = new WorkspaceOptions
        {
            SequenceArrayMapping = "Idl",
            SequenceSize = 12,
            StringSize = 42,
            IncludeDirectories = ["/workspace/z", "/workspace/a"],
            EnableEscapeChar = true,
        };
        var request = new CompileRequest(3, 1, 1, 1, profile,
            [new WorkspaceFile("/workspace/main.idl", root, true, new FileOptions { Defines = ["FEATURE=1", "DISABLED=1"], Undefines = ["DISABLED"], Strict = true }),
             new WorkspaceFile("/workspace/a/types.idl", shared, false),
             new WorkspaceFile("/workspace/z/types.idl", "invalid IDL", false)], options);
        var expected = IdlCompiler.Compile(
            [new IdlInput("/workspace/main.idl", root, IdlSequenceArrayMapping.Idl, generate: true, strict: true,
                defines: ["FEATURE=1", "DISABLED=1"], undefines: ["DISABLED"], includeDirectories: ["/workspace/a", "/workspace/z"], sequenceSize: 12, stringSize: 42, enableEscapeChar: true),
             new IdlInput("/workspace/a/types.idl", shared, IdlSequenceArrayMapping.Idl, generate: false, sequenceSize: 12, stringSize: 42, enableEscapeChar: true),
             new IdlInput("/workspace/z/types.idl", "invalid IDL", generate: false, enableEscapeChar: true)],
            profile == "7.3.1" ? IdlCompilerProfile.Rti731 : IdlCompilerProfile.Rti770, MD5.HashData, TestContext.Current.CancellationToken);

        // Act
        using var response = JsonDocument.Parse(PlaygroundCompilerAdapter.Compile(
            JsonSerializer.Serialize(request, PlaygroundJsonContext.Default.CompileRequest), MD5.HashData));

        // Assert
        expected.Success.ShouldBeTrue();
        response.RootElement.GetProperty("success").GetBoolean().ShouldBeTrue();
        response.RootElement.GetProperty("documents").EnumerateArray()
            .Select(document => document.GetProperty("source").GetString()).ToArray()
            .ShouldBe(expected.Documents.Select(document => document.Source).ToArray());
    }

    [Theory]
    [InlineData("sequenceArrayMapping", "unknown")]
    [InlineData("sequenceSize", 0)]
    [InlineData("stringSize", -1)]
    public void RejectsInvalidFileSettings(string name, object value)
    {
        // Arrange
        var request = RequestObject("");
        var file = File("/workspace/main.idl", "", true);
        file["options"] = new Dictionary<string, object> { ["defines"] = Array.Empty<string>(), ["undefines"] = Array.Empty<string>(), ["strict"] = false, ["sequenceArrayMapping"] = "RtiCSharp", ["sequenceSize"] = 100, ["stringSize"] = 255, [name] = value };
        request["files"] = new[] { file };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Theory]
    [InlineData("defines")]
    [InlineData("undefines")]
    public void RejectsNullFileSettingLists(string name)
    {
        // Arrange
        var request = RequestObject("");
        var file = File("/workspace/main.idl", "", true);
        file["options"] = new Dictionary<string, object?>
        {
            ["defines"] = Array.Empty<string>(),
            ["undefines"] = Array.Empty<string>(),
            ["strict"] = false,
            [name] = null,
        };
        request["files"] = new[] { file };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Theory]
    [InlineData("includeDirectories")]
    public void RejectsNullAndOversizedSettingLists(string name)
    {
        // Arrange
        var request = RequestObject("");
        request["options"] = new Dictionary<string, object?> { ["includeDirectories"] = Array.Empty<string>(), [name] = null };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
        request["options"] = new Dictionary<string, object?> { ["includeDirectories"] = Array.Empty<string>(), [name] = Enumerable.Repeat("SYMBOL", 129).ToArray() };
        Should.Throw<ArgumentException>(() => Compile(request));
        request["options"] = new Dictionary<string, object?> { ["includeDirectories"] = Array.Empty<string>(), [name] = new[] { new string('a', 1025) } };
        Should.Throw<ArgumentException>(() => Compile(request));
        request["options"] = new Dictionary<string, object?> { ["includeDirectories"] = Array.Empty<string>(), [name] = new[] { "bad\nentry" } };
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void RejectsFileFolderCollisionsAndUnboundedPaths()
    {
        // Arrange
        var request = RequestObject("");
        request["files"] = new[] { File("/workspace/shared", "", true), File("/workspace/shared/types.idl", "", false) };

        // Act and Assert
        Should.Throw<ArgumentException>(() => Compile(request));
        foreach (var path in new[] { "/workspace/bad\0.idl", "/workspace/ bad.idl", "/workspace/" + new string('a', 1024), "/workspace/" + string.Join("/", Enumerable.Repeat("dir", 33)) })
        {
            request["files"] = new[] { File(path, "", true) };
            Should.Throw<ArgumentException>(() => Compile(request));
        }

        request["files"] = new[] { File("/workspace/main.idl", "", true) };
        request["options"] = new { includeDirectories = new[] { "../host" } };
        Should.Throw<ArgumentException>(() => Compile(request));
    }

    [Fact]
    public void TypeScriptMirrorMatchesTheCanonicalAdapterDtos()
    {
        // Arrange
        var source = System.IO.File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Playground", "protocol.ts"));
        using var acceptedResponse = JsonDocument.Parse(Compile(RequestObject("struct Sample { long value; };")));
        var aliases = new Dictionary<string, string>
        {
            ["CompileRequest.ProtocolVersion"] = acceptedResponse.RootElement.GetProperty("protocolVersion").GetRawText(),
            ["CompileRequest.Profile"] = "\"7.3.1\" | \"7.7.0\"",
            ["FileOptions.SequenceArrayMapping"] = "\"RtiCSharp\" | \"Idl\"",
            ["FileOptions.SequenceSize"] = "number",
            ["FileOptions.StringSize"] = "number",
            ["WorkspaceOptions.SequenceArrayMapping"] = "\"RtiCSharp\" | \"Idl\"",
            ["CompileResponse.Kind"] = "\"compiler\" | \"limit\"",
        };

        // Act and Assert
        foreach (var type in new[] { typeof(CompileRequest), typeof(WorkspaceFile), typeof(WorkspaceOptions), typeof(FileOptions), typeof(CompileResponse), typeof(DiagnosticDto), typeof(DocumentDto) })
        {
            var name = type.Name.Replace("Dto", "");
            var body = Regex.Match(source, $@"export interface {name} \{{([^}}]*)\}}", RegexOptions.Singleline).Groups[1].Value;
            var fields = Regex.Matches(body, @"^\s*(\w+)(\?)?:\s*([^;]+);", RegexOptions.Multiline)
                .ToDictionary(match => match.Groups[1].Value, match => (Optional: match.Groups[2].Success, Type: match.Groups[3].Value));
            fields.Count.ShouldBe(type.GetProperties().Length, $"{name} property count");
            foreach (var property in type.GetProperties())
            {
                var key = JsonNamingPolicy.CamelCase.ConvertName(property.Name);
                var expected = aliases.GetValueOrDefault($"{type.Name}.{property.Name}", TypeScriptType(property.PropertyType));
                if (type == typeof(DiagnosticDto) && property.Name == "FilePath" || type == typeof(CompileResponse) && property.Name == "Error")
                {
                    expected += " | null";
                }

                fields.ShouldContainKey(key);
                var optional = type == typeof(WorkspaceFile) && property.Name == "Options" ||
                               type == typeof(FileOptions) && property.Name is "SequenceArrayMapping" or "SequenceSize" or "StringSize";
                fields[key].Optional.ShouldBe(optional, $"{name}.{key} optionality");
                Regex.Replace(fields[key].Type, @"\s+", "").ShouldBe(Regex.Replace(expected, @"\s+", ""), $"{name}.{key}");
            }
        }
    }

    private static string TypeScriptType(Type type)
    {
        if (type.IsArray)
        {
            return TypeScriptType(type.GetElementType()!) + "[]";
        }

        if (type == typeof(int) || type == typeof(long))
        {
            return "number";
        }

        if (type == typeof(bool))
        {
            return "boolean";
        }

        if (type == typeof(string))
        {
            return "string";
        }

        return type.Name.Replace("Dto", "");
    }

    private static string Compile(Dictionary<string, object?> request)
    {
        return PlaygroundCompilerAdapter.Compile(JsonSerializer.Serialize(request, WebJson), MD5.HashData);
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
            ["protocolVersion"] = 3,
            ["workerGeneration"] = 2,
            ["requestId"] = 4,
            ["workspaceRevision"] = 8,
            ["profile"] = "7.7.0",
            ["files"] = new[] { File("/workspace/main.idl", source, true) },
            ["options"] = new WorkspaceOptions(),
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
