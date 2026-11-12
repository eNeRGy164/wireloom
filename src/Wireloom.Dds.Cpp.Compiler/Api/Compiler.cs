using Wireloom.Dds.Cpp.Emission;

namespace Wireloom.Dds.Cpp;

/// <summary>Compiles in-memory IDL into deterministic Modern C++ documents.</summary>
[PublicAPI]
public static class CppCompiler
{
    private static readonly Version MinimumRuntimeVersion = new(6, 1, 2);
    private static readonly Version Rti731RuntimeVersion = new(7, 3, 1);
    private static readonly Version Rti770RuntimeVersion = new(7, 7, 0);

    /// <summary>Maps a supported RTI runtime version to the matching C++ compiler profile.</summary>
    /// <param name="runtimeVersion">The resolved RTI runtime version.</param>
    /// <returns>The compiler profile whose generated API matches the runtime.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="runtimeVersion"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The version is below 6.1.2.</exception>
    public static CppCompilerProfile GetProfileForRuntimeVersion(Version runtimeVersion)
    {
        ArgumentNullException.ThrowIfNull(runtimeVersion);

        if (runtimeVersion < MinimumRuntimeVersion)
        {
            throw new ArgumentOutOfRangeException(nameof(runtimeVersion), runtimeVersion,
                $"RTI runtime version {MinimumRuntimeVersion} or later is required.");
        }

        if (runtimeVersion >= Rti770RuntimeVersion)
        {
            return CppCompilerProfile.Rti770;
        }

        return runtimeVersion >= Rti731RuntimeVersion ? CppCompilerProfile.Rti731 : CppCompilerProfile.Rti612;
    }

    /// <summary>Compiles a shared host request using a C++ API profile.</summary>
    /// <param name="request">The immutable IDL inputs and host services.</param>
    /// <param name="profile">The RTI C++ API profile targeted by emitted files.</param>
    /// <returns>Generated C++ files and diagnostics.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="request"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="profile"/> is unsupported.</exception>
    /// <exception cref="InvalidOperationException">The digest provider fails or returns an invalid digest.</exception>
    /// <exception cref="OperationCanceledException">Compilation was canceled.</exception>
    public static IdlCompilationResult<GeneratedCppSource> Compile(IdlCompilationRequest request, CppCompilerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!Enum.IsDefined(profile))
        {
            throw new ArgumentOutOfRangeException(nameof(profile), profile, "The RTI C++ API profile is not supported.");
        }

        var diagnostics = new List<IdlDiagnostic>();
        var emissionProfile = ToEmissionProfile(profile);
        var files = Compiler.Compile(request.Inputs, request.ComputeMd5, diagnostics, emissionProfile, request.CancellationToken);
        var documents = files.Select(file => new GeneratedCppSource(file.FileName, file.Source));

        return new IdlCompilationResult<GeneratedCppSource>(documents, diagnostics);
    }

    private static RtiApiProfile ToEmissionProfile(CppCompilerProfile profile) =>
        profile switch
        {
            CppCompilerProfile.Rti612 => RtiApiProfile.Rti612,
            CppCompilerProfile.Rti731 => RtiApiProfile.Rti731,
            _ => RtiApiProfile.Rti770,
        };
}
