using Wireloom.Compiler;
using Wireloom.Compiler.Emission;

namespace Wireloom;

/// <summary>Compiles in-memory IDL into deterministic C# documents.</summary>
[PublicAPI]
public static class IdlCompiler
{
    private static readonly Version MinimumRuntimeVersion = new(7, 3, 1);
    private static readonly Version Rti770RuntimeVersion = new(7, 7, 0);

    /// <summary>Maps a supported RTI runtime version to the nearest compiler API profile.</summary>
    /// <param name="runtimeVersion">The resolved RTI runtime version.</param>
    /// <returns>The compiler profile whose generated API matches the runtime.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="runtimeVersion"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The runtime version is below 7.3.1.</exception>
    public static IdlCompilerProfile GetProfileForRuntimeVersion(Version runtimeVersion)
    {
        ArgumentNullException.ThrowIfNull(runtimeVersion);

        if (runtimeVersion < MinimumRuntimeVersion)
        {
            throw new ArgumentOutOfRangeException(nameof(runtimeVersion), runtimeVersion,
                $"RTI runtime version {MinimumRuntimeVersion} or later is required.");
        }

        return runtimeVersion >= Rti770RuntimeVersion ? IdlCompilerProfile.Rti770 : IdlCompilerProfile.Rti731;
    }

    /// <summary>Compiles inputs using an explicit RTI C# API profile.</summary>
    /// <param name="inputs">All in-memory IDL inputs, including generation roots and supplied includes.</param>
    /// <param name="profile">The RTI API profile targeted by emitted C#.</param>
    /// <param name="computeMd5">A synchronous provider that returns the 16-byte MD5 digest of its input.</param>
    /// <param name="cancellationToken">A token that cancels compilation.</param>
    /// <returns>Generated documents and diagnostics from the compilation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="inputs"/> is null.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="computeMd5"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="profile"/> is not supported.</exception>
    /// <exception cref="InvalidOperationException">The provider fails or does not return a 16-byte digest.</exception>
    /// <exception cref="OperationCanceledException">Compilation was canceled.</exception>
    public static IdlCompilationResult Compile(
        IReadOnlyList<IdlInput> inputs,
        IdlCompilerProfile profile,
        Func<byte[], byte[]> computeMd5,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(computeMd5);

        if (!Enum.IsDefined(typeof(IdlCompilerProfile), profile))
        {
            throw new ArgumentOutOfRangeException(nameof(profile), profile, "The RTI API profile is not supported.");
        }

        // Capture the collection once so mutations to a caller-owned list cannot alter this compilation.
        var inputSnapshot = inputs.ToArray();
        var diagnostics = new List<IdlDiagnostic>();

        try
        {
            var generated = new IdlCompilation(inputSnapshot, ToEmissionProfile(profile), computeMd5, cancellationToken).Run(diagnostics);
            var documents = generated.ToDictionary(source => source.HintName, StringComparer.Ordinal);
            return new IdlCompilationResult(documents.Values, diagnostics);
        }
        catch (IdlException exception)
        {
            diagnostics.Add(new IdlDiagnostic("DDSG0001", IdlDiagnosticSeverity.Error, exception.Input, exception.Offset, exception.Message));
            return new IdlCompilationResult([], diagnostics);
        }
    }

    private static RtiApiProfile ToEmissionProfile(IdlCompilerProfile profile) =>
        profile == IdlCompilerProfile.Rti731 ? RtiApiProfile.Rtiddsgen431 : RtiApiProfile.Rtiddsgen470;
}
