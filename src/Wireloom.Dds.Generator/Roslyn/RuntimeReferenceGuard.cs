using Microsoft.CodeAnalysis;
using Wireloom.Compiler.Emission;

namespace Wireloom.Roslyn;

/// <summary>Checks whether the consuming compilation references a supported RTI runtime.</summary>
internal static class RuntimeReferenceGuard
{
    internal const string MinimumSupportedRtiVersion = "7.3.1";
    private static readonly Version minimumSupportedVersion = Version.Parse(MinimumSupportedRtiVersion);

    internal static bool TryGetProfile(Compilation compilation, out RtiApiProfile profile)
    {
        Version? runtimeVersion = null;
        foreach (var reference in compilation.References)
        {
            var assembly = compilation.GetAssemblyOrModuleSymbol(reference) as IAssemblySymbol;
            if (assembly?.Identity.Name == "Rti.ConnextDds")
            {
                runtimeVersion = assembly.Identity.Version;
                break;
            }
        }

        if (runtimeVersion is null || runtimeVersion < minimumSupportedVersion)
        {
            profile = null!;
            return false;
        }

        profile = RtiApiProfile.ForVersion(runtimeVersion);
        return true;
    }
}
