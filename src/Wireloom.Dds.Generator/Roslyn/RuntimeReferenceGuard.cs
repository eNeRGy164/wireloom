using Microsoft.CodeAnalysis;

namespace Wireloom.Roslyn;

/// <summary>Checks whether the consuming compilation references a supported RTI runtime.</summary>
internal static class RuntimeReferenceGuard
{
    internal const string MinimumSupportedRtiVersion = "7.3.1";
    private static readonly Version minimumSupportedVersion = Version.Parse(MinimumSupportedRtiVersion);

    internal static bool HasCompatibleRuntime(Compilation compilation)
    {
        foreach (var reference in compilation.References)
        {
            var assembly = compilation.GetAssemblyOrModuleSymbol(reference) as IAssemblySymbol;
            if (assembly?.Identity.Name == "Rti.ConnextDds" && assembly.Identity.Version >= minimumSupportedVersion)
            {
                return true;
            }
        }

        return false;
    }
}
