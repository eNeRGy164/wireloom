using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

[assembly: SupportedOSPlatform("browser")]

namespace Wireloom;

/// <summary>Composes the browser compiler adapter with worker-local JavaScript interop.</summary>
public static partial class PlaygroundCompiler
{
    /// <summary>Provides the WebAssembly executable entry point.</summary>
    public static void Main()
    {
    }

    /// <summary>Compiles a versioned workspace snapshot using the worker's MD5 provider.</summary>
    /// <param name="requestJson">The immutable JSON workspace request.</param>
    /// <returns>The JSON compiler response, including request identity and diagnostics.</returns>
    /// <exception cref="ArgumentException">The workspace request is invalid.</exception>
    /// <exception cref="InvalidOperationException">The host MD5 provider fails.</exception>
    [PublicAPI]
    [JSExport]
    public static string Compile(string requestJson) => PlaygroundCompilerAdapter.Compile(requestJson, ComputeMd5);

    [JSImport("computeMd5", "md5-provider")]
    private static partial byte[] ComputeMd5(byte[] input);
}
