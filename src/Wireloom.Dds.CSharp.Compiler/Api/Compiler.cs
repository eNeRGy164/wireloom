using Wireloom.Dds.CSharp.Compiler.Emission;

namespace Wireloom.Dds.CSharp.Compiler;

/// <summary>Compiles in-memory IDL into deterministic C# documents.</summary>
[PublicAPI]
public static class IdlCompiler
{
    private static readonly Version MinimumRuntimeVersion = new(6, 1, 2);
    private static readonly Version Rti731RuntimeVersion = new(7, 3, 1);
    private static readonly Version Rti770RuntimeVersion = new(7, 7, 0);

    /// <summary>Compiles a shared host request using a C# API profile.</summary>
    /// <param name="request">The immutable IDL inputs and host services.</param>
    /// <param name="profile">The RTI API profile targeted by emitted C#.</param>
    /// <returns>Generated C# documents and diagnostics.</returns>
    public static IdlCompilationResult<GeneratedIdlSource> Compile(IdlCompilationRequest request, IdlCompilerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!Enum.IsDefined(typeof(IdlCompilerProfile), profile))
        {
            throw new ArgumentOutOfRangeException(nameof(profile), profile, "The RTI API profile is not supported.");
        }

        var diagnostics = new List<IdlDiagnostic>();
        try
        {
            var emissionProfile = ToEmissionProfile(profile);
            var generated = new IdlCompilation(request.Inputs, emissionProfile, request.ComputeMd5, request.CancellationToken).Run(diagnostics);
            var documents = generated.ToDictionary(source => source.HintName, StringComparer.Ordinal);

            return new IdlCompilationResult<GeneratedIdlSource>(documents.Values, diagnostics);
        }
        catch (IdlException exception)
        {
            diagnostics.Add(new IdlDiagnostic("DDSG0001", IdlDiagnosticSeverity.Error, exception.Input, exception.Offset, exception.Message));
            return new IdlCompilationResult<GeneratedIdlSource>([], diagnostics);
        }
    }

    /// <summary>Maps a supported RTI runtime version to the nearest compiler API profile.</summary>
    /// <param name="runtimeVersion">The resolved RTI runtime version.</param>
    /// <returns>The compiler profile whose generated API matches the runtime.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="runtimeVersion"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The runtime version is below 6.1.2.</exception>
    public static IdlCompilerProfile GetProfileForRuntimeVersion(Version runtimeVersion)
    {
        ArgumentNullException.ThrowIfNull(runtimeVersion);

        if (runtimeVersion < MinimumRuntimeVersion)
        {
            throw new ArgumentOutOfRangeException(nameof(runtimeVersion), runtimeVersion, $"RTI runtime version {MinimumRuntimeVersion} or later is required.");
        }

        if (runtimeVersion >= Rti770RuntimeVersion)
        {
            return IdlCompilerProfile.Rti770;
        }

        return runtimeVersion >= Rti731RuntimeVersion ? IdlCompilerProfile.Rti731 : IdlCompilerProfile.Rti612;
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
    public static IdlCompilationResult<GeneratedIdlSource> Compile(
        IReadOnlyList<IdlInput> inputs,
        IdlCompilerProfile profile,
        Func<byte[], byte[]> computeMd5,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(inputs);
        ArgumentNullException.ThrowIfNull(computeMd5);

        return Compile(new IdlCompilationRequest(inputs, computeMd5, cancellationToken), profile);
    }

    private static RtiApiProfile ToEmissionProfile(IdlCompilerProfile profile) =>
        profile switch
        {
            IdlCompilerProfile.Rti612 => RtiApiProfile.Rtiddsgen312,
            IdlCompilerProfile.Rti731 => RtiApiProfile.Rtiddsgen431,
            _ => RtiApiProfile.Rtiddsgen470,
        };
}
