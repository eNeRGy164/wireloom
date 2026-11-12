using System.Text.RegularExpressions;

namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Owns native oracle layout, IDL adaptation, and guarded RTI source corrections.</summary>
internal sealed class NativePeerSources(MatrixOptions options, MatrixWorkspace workspace, CorpusCaseCatalog catalog, WireOracleCatalog oracles)
{
    private readonly CppOracleCatalog cppOracles = new(
        Path.Combine(options.RepositoryRoot, "docs", "corpus", options.RtiVersion, "rtiddsgen", "cpp", "manifest.json"),
        options.RepositoryRoot);

    /// <summary>Resolves a case's exact native peer topic type from its hash-verified RTI C++ oracle.</summary>
    internal string DiscoverNativePeerScopeType(string caseId)
        => cppOracles.FindTopicType(caseId, catalog.Get(caseId), options.RtiVersion);

    /// <summary>Stages the selected oracle, returning false when the required capture is missing.</summary>
    internal bool Stage(CaseDescriptor descriptor, CaseWorkspace paths)
    {
        StageIdl(descriptor, paths);

        var hasExplicitPolicy = oracles.HasExplicitPolicy(descriptor.CaseId, "rtiCpp");
        var oracle = hasExplicitPolicy
            ? oracles.FindConfiguredSource(descriptor.CaseId, "rtiCpp")
            : cppOracles.FindSource(descriptor.CaseId, catalog.Get(descriptor.CaseId), options.RtiVersion);
        if (!string.IsNullOrEmpty(oracle))
        {
            StageCapturedOracle(descriptor.CaseId, workspace.OracleDirectory(oracle), paths.Native);

            if (descriptor.CaseId == "10-flat-data-binding" && options.RtiVersion != "7.7.0")
            {
                ApplyLegacyFlatDataExtensibilityCorrection(paths.Native);
            }

            var evidence = hasExplicitPolicy
                ? Path.GetRelativePath(options.RepositoryRoot, workspace.OracleManifest).Replace('\\', '/')
                : Path.GetRelativePath(options.RepositoryRoot, Path.Combine(options.RepositoryRoot, "docs", "corpus", options.RtiVersion, "rtiddsgen", "cpp", "manifest.json")).Replace('\\', '/');
            File.WriteAllText(Path.Combine(workspace.Logs, descriptor.CaseId + "-rti-cpp-oracle.log"),
                $"Using checked-in RTI C++ oracle ({oracle}; selection and output hashes are verified from {evidence}).{Environment.NewLine}");
        }
        var available = !string.IsNullOrEmpty(oracle) || descriptor.CaseId == "07-union-wchar-label";

        if (descriptor.CaseId == "06-aggregates")
        {
            File.Copy(Path.Combine(workspace.NativeTemplates, "recursive-sequence-peer.cpp"), Path.Combine(paths.Native, "recursive-sequence-peer.cpp"), overwrite: true);
        }

        if (string.IsNullOrEmpty(oracle) && available)
        {
            ApplyFallbackCorrections(descriptor.CaseId, paths.Native);
        }

        return available;
    }

    /// <summary>Writes authored peer sources around the selected native type.</summary>
    internal void WritePeerTemplates(CaseWorkspace paths, string cppType)
    {
        var main = File.ReadAllText(Path.Combine(workspace.NativeTemplates, "main.cpp.in"))
            .Replace("@IDL_BASE@", "idl", StringComparison.Ordinal)
            .Replace("@CPP_TYPE@", cppType, StringComparison.Ordinal);
        File.WriteAllText(Path.Combine(paths.Native, "main.cpp"), main);

        File.Copy(Path.Combine(workspace.NativeTemplates, "typed-fixture-peer.hpp.in"), Path.Combine(paths.Native, "typed-fixture-peer.hpp"), overwrite: true);
        File.Copy(Path.Combine(workspace.NativeTemplates, "peer-qos.hpp"), Path.Combine(paths.Native, "peer-qos.hpp"), overwrite: true);
        File.Copy(Path.Combine(workspace.NativeTemplates, "CMakeLists.txt"), Path.Combine(paths.Native, "CMakeLists.txt"), overwrite: true);
    }

    /// <summary>Writes a C++ peer entry point against generated Wireloom sources.</summary>
    internal void WriteWireloomCppTemplate(CaseWorkspace paths, string cppType, string idlBase)
    {
        var main = File.ReadAllText(Path.Combine(workspace.NativeTemplates, "main.cpp.in"))
            .Replace("@IDL_BASE@", idlBase, StringComparison.Ordinal)
            .Replace("@CPP_TYPE@", cppType, StringComparison.Ordinal);
        File.WriteAllText(paths.WireloomCppMain, main);
    }

    private void StageIdl(CaseDescriptor descriptor, CaseWorkspace paths)
    {
        var source = IdlSourceResolver.ResolveOracleIdl(options.RepositoryRoot, descriptor.CaseId, workspace.CorpusFile(descriptor.Idl));
        var target = Path.Combine(paths.Native, "idl.idl");

        if (descriptor.CaseId == "07-union-wchar-label")
        {
            File.Copy(source, target, overwrite: true);
            return;
        }

        if (descriptor.CaseId == "10-flat-data-binding" && options.RtiVersion != "7.7.0")
        {
            var idl = File.ReadAllText(source);
            idl = Regex.Replace(idl, @"(?m)^.*@language_binding\(FLAT_DATA\).*\r?\n", string.Empty);
            idl = idl.Replace("@topic @final struct Sample", "@topic struct Sample", StringComparison.Ordinal);
            File.WriteAllText(target, idl);
            return;
        }

        if (catalog.Get(descriptor.CaseId).RtiCppIdlAdaptations.Any(a => a.Contains($"{options.RtiVersion}:numeric-char-labels", StringComparison.Ordinal)))
        {
            File.WriteAllText(target,
                File.ReadAllText(source)
                    .Replace(@"case '\0':", "case 0:", StringComparison.Ordinal)
                    .Replace("case '~':", "case 126:", StringComparison.Ordinal));
            return;
        }

        File.Copy(source, target, overwrite: true);
    }

    private static void ApplyLegacyFlatDataExtensibilityCorrection(string nativeDirectory)
    {
        var typeCodePath = Path.Combine(nativeDirectory, "idl.cxx");
        var typeCode = File.ReadAllText(typeCodePath);
        const string finalTypeCode = "DDS_TK_STRUCT| DDS_TK_FINAL_EXTENSIBILITY";
        if (!typeCode.Contains(finalTypeCode, StringComparison.Ordinal))
        {
            throw new InvalidDataException("The retained legacy FlatData RTI C++ oracle has no final TypeCode marker to adapt.");
        }

        File.WriteAllText(typeCodePath, typeCode.Replace(finalTypeCode, "DDS_TK_STRUCT", StringComparison.Ordinal));

        var headerPath = Path.Combine(nativeDirectory, "idl.hpp");
        var header = File.ReadAllText(headerPath);
        const string finalTrait = "::dds::core::xtypes::ExtensibilityKind::FINAL";
        if (!header.Contains(finalTrait, StringComparison.Ordinal))
        {
            throw new InvalidDataException("The retained legacy FlatData RTI C++ oracle has no final extensibility trait to adapt.");
        }

        File.WriteAllText(headerPath, header.Replace(finalTrait, "::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE", StringComparison.Ordinal));
    }

    private void StageCapturedOracle(string caseId, string source, string destination)
    {
        foreach (var file in CppSources(source, SearchOption.AllDirectories))
        {
            var relative = Path.GetRelativePath(source, file);
            var staged = StagedOraclePath(caseId, source, relative);
            var target = Path.Combine(destination, staged);

            Directory.CreateDirectory(Path.GetDirectoryName(target)!);

            File.Copy(file, target, overwrite: true);
        }

        if (!source.Contains("wire-wrapped", StringComparison.Ordinal) && caseId != "05-array-of-sequences-idl")
        {
            foreach (var file in CppSources(destination, SearchOption.TopDirectoryOnly))
            {
                EditCpp(file, t => t
                        .Replace($"\"{caseId}Plugin.hpp\"", "\"idlPlugin.hpp\"", StringComparison.Ordinal)
                        .Replace($"\"{caseId}.hpp\"", "\"idl.hpp\"", StringComparison.Ordinal));
            }
        }
    }

    private string StagedOraclePath(string caseId, string source, string relative)
    {
        var isTopLevel = string.IsNullOrEmpty(Path.GetDirectoryName(relative));
        var name = Path.GetFileName(relative);

        if ((!source.Contains("wire-wrapped", StringComparison.Ordinal) && isTopLevel) || relative is "idl.cxx" or "idl.hpp" or "idlPlugin.cxx" or "idlPlugin.hpp")
        {
            return RenameOracle(name, caseId);
        }

        if (!isTopLevel)
        {
            return relative;
        }

        var stem = Path.GetFileNameWithoutExtension(name);
        if (stem.EndsWith("Plugin", StringComparison.Ordinal))
        {
            stem = stem[..^"Plugin".Length];
        }

        if (File.Exists(workspace.CorpusFile(Path.Combine("idl", "features", $"{stem}.idl"))))
        {
            return Path.Combine("features", name);
        }

        if (File.Exists(workspace.CorpusFile(Path.Combine("idl", "includes", $"{stem}.idl"))))
        {
            return Path.Combine("includes", name);
        }

        return name;
    }

    private void ApplyFallbackCorrections(string caseId, string nativeDirectory)
    {
        var cppPath = Path.Combine(nativeDirectory, "idl.cxx");

        if (caseId == "09-optional-recursive-array")
        {
            var source = File.ReadAllText(cppPath);
            if (!source.Contains("Node_g_tc_children_array._data._typeCode", StringComparison.Ordinal))
            {
                throw new InvalidDataException("RTI generated recursive array TypeCode initializer changed; update its guarded workaround.");
            }

            EditCpp(cppPath, t => Regex.Replace(t, @"(Node_g_tc_children_array\._data\._typeCode[^\r\n]*?)::get\(\)\.native\(\)", "$1::get(&node).native()"));

            if (options.RtiVersion == "7.3.1")
            {
                EditCpp(cppPath, t => t.Replace("dynamic_type< Node>", "dynamic_type< ::CorpusOptionalRecursiveArray::Node>", StringComparison.Ordinal));
            }
        }

        if (caseId == "06-aggregates" && options.RtiVersion == "7.3.1")
        {
            EditCpp(cppPath, t => t.Replace("dynamic_type< Recursive>", "dynamic_type< ::CorpusAggregates::Recursive>", StringComparison.Ordinal));
        }

        if (caseId == "10-resolve-name-wstring-aliases")
        {
            var source = File.ReadAllText(cppPath);

            if (Count(source, "dynamic_type< BoundedWideText>") != 1 || Count(source, "dynamic_type< BoundedWideTextAlias>") != 1)
            {
                throw new InvalidDataException($"RTI C++ generated TypeCode references changed for {caseId}; update the guarded alias-tag workaround.");
            }

            EditCpp(cppPath, t => t
                .Replace("dynamic_type< BoundedWideText>", "dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideText_AliasTag_t>", StringComparison.Ordinal)
                .Replace("dynamic_type< BoundedWideTextAlias>", "dynamic_type< ::CorpusResolveNameWstringAliases::BoundedWideTextAlias_AliasTag_t>", StringComparison.Ordinal));
        }

        if (caseId == "07-union-char-numeric-255")
        {
            RtiCharacterReferenceAdapter.Apply("cpp", caseId, nativeDirectory);
        }
    }

    private static IEnumerable<string> CppSources(string directory, SearchOption search)
        => Directory.EnumerateFiles(directory, "*", search).Where(f => Path.GetExtension(f) is ".cxx" or ".hpp");

    private static void EditCpp(string path, Func<string, string> edit) => File.WriteAllText(path, edit(File.ReadAllText(path)));

    private static int Count(string text, string value) => Regex.Count(text, Regex.Escape(value));

    private static string RenameOracle(string name, string caseId) => name switch
    {
        var value when value == $"{caseId}.cxx" => "idl.cxx",
        var value when value == $"{caseId}.hpp" => "idl.hpp",
        var value when value == $"{caseId}Plugin.cxx" => "idlPlugin.cxx",
        var value when value == $"{caseId}Plugin.hpp" => "idlPlugin.hpp",
        _ => name,
    };
}
