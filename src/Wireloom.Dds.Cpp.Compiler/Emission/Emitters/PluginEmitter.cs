using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Coordinates the RTI plugin implementation file in translation-unit order.</summary>
internal static class PluginEmitter
{
    /// <summary>Renders RTI topic support definitions for every planned struct.</summary>
    internal static string Emit(GenerationPlan plan)
    {
        var source = new SourceWriter(plan.Profile.RtiVersion);
        if (plan.Profile.RtiVersion == "6.1.2")
        {
            Rti612PluginEmitter.WritePreamble(source, plan);
            Rti612PluginEmitter.WriteTypeContexts(source, plan);
            Rti612KeyPluginEmitter.EmitCallbacks(source, plan);
            Rti612PluginEmitter.WritePluginBridge(source, plan);
            Rti612PluginEmitter.EmitDynamicTypes(source, plan);
            EmitFlatDataPrograms(source, plan);
            EmitTopicOperations(source, plan);
            source.WriteLine("#endif // NDDS_STANDALONE_TYPE");
            return source.ToString();
        }

        var metadataOnlyTypeInitialization = plan.Initialization.RequiresMetadataOnlyTypeInitialization
            || RtiSampleAccess.HasOptionalSelfRecursiveArray(plan)
            || RtiSampleAccess.HasCollectionUnionBranch(plan);
        WritePreamble(source, plan);
        EmitPluginContexts(source, plan);

        if (plan.Initialization.RequiresStagedInitialization)
        {
            source.BlankLine();
            StructPluginEmitter.EmitStructTypeInitialization(source, plan, metadataOnlyTypeInitialization);
        }

        EmitDynamicTypes(source, plan, metadataOnlyTypeInitialization);
        EmitFlatDataPrograms(source, plan);
        EmitTopicOperations(source, plan);
        source.WriteLine("#endif // NDDS_STANDALONE_TYPE");
        return source.ToString();
    }

    private static void WritePreamble(SourceWriter source, GenerationPlan plan)
    {
        var hasRangeUnitMetadata = plan.Structs.SelectMany(declaration => declaration.Members)
            .Any(member => member.TypeCodeUnit is not null
                || member.Minimum is not null && member.Maximum is not null);
        var hasStlInterpreterGraphs = plan.Structs.Any(declaration =>
            RtiSampleAccess.UsesStlInterpreterPluginBridge(plan, declaration)
            || declaration.Members.Any(member =>
                member.Type is MemberTypePlan.Alias && RtiSampleAccess.IsAggregateAliasTarget(member.Type)));
        EmitBanner(source, plan);
        source.WriteInclude($"{plan.FileStem}Plugin.hpp", quoted: true);
        source.BlankLine();
        source.WriteLine("#ifndef NDDS_STANDALONE_TYPE");
        source.BlankLine();
        source.WriteComment("Standard library");
        source.WriteInclude("cstddef");
        if (RtiSampleAccess.HasAliasedSequenceMember(plan))
        {
            source.WriteInclude("stdexcept");
        }
        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("algorithm");
            source.WriteInclude("cstring");
            source.WriteInclude("new");
            source.WriteInclude("ndds/pres/pres_typePlugin.h");
            source.WriteInclude("ndds/pres/pres_typePlugin_impl.h");
            source.WriteInclude("ndds/dds_c/dds_c_typecode_impl.h");
            source.WriteInclude("ndds/xcdr/xcdr_interpreter.h");
            source.WriteInclude("xcdr/xcdr_interpreter.h");
            source.WriteInclude("ndds/ndds_version.h");
            EmitExactRuntimeVersionGuard(source, plan.Profile, "FlatData PRESTypePlugin lifecycle adapter");
            source.WriteInclude("ndds/cdr/cdr_stream.h");
            source.WriteInclude("ndds/xcdr/xcdr_flat_data.h");
            source.WriteInclude("ndds/xcdr/xcdr_stream.h");
            source.WriteInclude("ndds/reda/reda_fastBuffer.h");
        }
        if (plan.Initialization.RequiresStagedInitialization)
        {
            source.WriteInclude("memory");
            source.WriteInclude("mutex");
        }

        source.WriteInclude("vector");
        source.BlankLine();
        source.WriteComment("RTI Connext DDS Modern C++ API");
        source.WriteInclude("dds/core/xtypes/PrimitiveTypes.hpp");
        source.WriteInclude("dds/core/xtypes/EnumType.hpp");
        if (plan.Aliases.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/AliasType.hpp");
        }
        if (plan.Structs.SelectMany(declaration => declaration.Members).Any(member => member.IsOptional))
        {
            source.WriteInclude("dds/core/xtypes/MemberType.hpp");
        }
        if (plan.Unions.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/UnionType.hpp");
        }
        if (plan.Structs.SelectMany(declaration => declaration.Members)
            .Any(member => member.Type.ContainsString))
        {
            source.WriteInclude("dds/core/xtypes/CollectionTypes.hpp");
        }
        if (plan.Structs.SelectMany(declaration => declaration.Members).Any(member => member.IsExternal)
            || plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("dds/core/Exception.hpp");
        }

        source.WriteInclude("rti/topic/cdr/GenericTypePluginFactory.hpp");
        if (plan.Unions.Count > 0)
        {
            source.WriteInclude("ndds/ndds_version.h");
            source.WriteInclude("ndds/pres/pres_typePlugin.h");
            source.WriteInclude("ndds/pres/pres_typePlugin_impl.h");
            source.WriteInclude("ndds/cdr/cdr_stream.h");
            source.WriteInclude("ndds/dds_c/dds_c_typecode_impl.h");
            source.WriteInclude("rti/topic/cdr/InterpreterHelpers.hpp");
            source.WriteInclude("rti/domain/PluginSupport.hpp");
            source.WriteInclude("xcdr/xcdr_typeCode.h");
        }
        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("xcdr/xcdr_stream.h");
        }
        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("rti/topic/cdr/InterpreterHelpers.hpp");
            source.WriteInclude("rti/domain/PluginSupport.hpp");
        }
        if (hasStlInterpreterGraphs || plan.Unions.Count > 0)
        {
            source.WriteInclude("new");
            source.WriteInclude("ndds/ndds_version.h");
            source.WriteInclude("ndds/pres/pres_typePlugin.h");
            source.WriteInclude("ndds/pres/pres_typePlugin_impl.h");
            source.WriteInclude("ndds/cdr/cdr_stream.h");
            source.WriteInclude("ndds/dds_c/dds_c_typecode_impl.h");
            source.WriteInclude("rti/topic/cdr/InterpreterHelpers.hpp");
            source.WriteInclude("rti/domain/PluginSupport.hpp");
            source.WriteInclude("xcdr/xcdr_typeCode.h");
            EmitExactRuntimeVersionGuard(source, plan.Profile,
                plan.Unions.Count > 0 ? "union TypeCode adapter" : "STL sample-access TypeCode metadata");
        }

        var aggregateAliases = plan.Aliases
            .Where(alias => alias.ResolvedType is MemberTypePlan.Struct or MemberTypePlan.Union)
            .ToArray();
        var unionsWithStagedReferences = plan.Unions
            .Where(union => UnionPluginEmitter.RequiresStagedUnionAccessor(plan, union))
            .ToArray();
        if (plan.Initialization.RequiresStagedInitialization && unionsWithStagedReferences.Length > 0)
        {
            UnionPluginEmitter.EmitUnionContextAccessorDeclarations(source, unionsWithStagedReferences);
        }
        if (aggregateAliases.Length > 0)
        {
            source.BlankLine();
            source.WriteInclude("ndds/ndds_version.h");
            source.WriteInclude("xcdr/xcdr_typeCode.h");
            EmitExactRuntimeVersionGuard(source, plan.Profile, "aggregate alias TypeCode adapter");
            source.BlankLine();
            NativeTypeEmitter.EmitAggregateAliasTypeCodeHelpers(source, aggregateAliases);
        }
        if (plan.Structs.Any(declaration => declaration.IsXcdr2Only))
        {
            // Mirror rtiddsgen's RTIXCdrTypeCode annotation for the selected profile.
            source.WriteInclude("dds/core/Exception.hpp");
            source.WriteInclude("ndds/ndds_version.h");
            source.WriteInclude("xcdr/xcdr_typeCode.h");
            EmitExactRuntimeVersionGuard(source, plan.Profile, "XCDR2-only representation TypeCode adapter");
        }
        else if (plan.Structs.Any(declaration => declaration.IsNested
                     || declaration.Members.Any(member => member.TypeCodeDefaultValue is not null
                         || member.TypeCodeUnit is not null
                         || member.Minimum is not null && member.Maximum is not null)))
        {
            source.WriteInclude("ndds/ndds_version.h");
            source.WriteInclude("xcdr/xcdr_typeCode.h");
            EmitExactRuntimeVersionGuard(source, plan.Profile,
                hasRangeUnitMetadata ? "nested/default/range TypeCode adapter" : "nested/default TypeCode adapter");
        }
    }

    private static void EmitPluginContexts(SourceWriter source, GenerationPlan plan)
    {
        var namespaces = new NamespaceBlockWriter(source);
        IReadOnlyList<string> contextNamespace = Array.Empty<string>();
        var anonymousNamespaceOpen = false;

        void SwitchContextNamespace(IReadOnlyList<string> requestedNamespace)
        {
            if (anonymousNamespaceOpen && !contextNamespace.SequenceEqual(requestedNamespace))
            {
                source.WriteLine("} // namespace");
                anonymousNamespaceOpen = false;
            }

            namespaces.SwitchTo(requestedNamespace);
            if (!anonymousNamespaceOpen)
            {
                source.WriteLine("namespace");
                source.WriteLine("{");
                contextNamespace = requestedNamespace.ToArray();
                anonymousNamespaceOpen = true;
            }
        }

        foreach (var declaration in plan.Enums)
        {
            source.BlankLine();
            SwitchContextNamespace(declaration.NamespaceSegments);
            source.WriteComment("Keep RTI enum metadata private to this translation unit.");
            DynamicTypeEmitter.EmitEnumPluginContext(source, declaration);
        }

        foreach (var declaration in plan.Structs)
        {
            source.BlankLine();
            SwitchContextNamespace(declaration.NamespaceSegments);
            source.WriteComment("Keep RTI plugin state private to this translation unit.");
            StructPluginEmitter.EmitPluginContext(source, declaration, plan);
            if (RtiSampleAccess.UsesStlInterpreterPluginBridge(plan, declaration))
            {
                var prefix = RtiSampleAccess.UsesDirectStringPluginBridge(plan, declaration)
                    ? PluginSymbols.StlStringPluginPrefix(declaration)
                    : PluginSymbols.StlSequencePluginPrefix(declaration);
                StlPluginEmitter.EmitStlInterpreterPluginBridge(
                    source,
                    declaration.CppType,
                    declaration.QualifiedName,
                    prefix,
                    plan.Profile,
                    declaration);
            }
        }

        foreach (var declaration in plan.Unions)
        {
            source.BlankLine();
            SwitchContextNamespace(declaration.NamespaceSegments);
            source.WriteComment("Keep RTI plugin state private to this translation unit.");
            UnionPluginEmitter.EmitUnionPluginContext(source, declaration, plan);
            if (UnionPluginEmitter.RequiresStagedUnionAccessor(plan, declaration))
            {
                UnionPluginEmitter.EmitUnionContextAccessors(source, declaration);
            }
            if (UnionPluginEmitter.RequiresStlInterpreterPluginBridge(declaration))
            {
                var prefix = PluginSymbols.StlUnionPluginPrefix(declaration);
                var qualifiedType = string.IsNullOrEmpty(declaration.Namespace)
                    ? $"::{declaration.Name}"
                    : $"::{declaration.Namespace.Replace(".", "::", StringComparison.Ordinal)}::{declaration.Name}";
                StlPluginEmitter.EmitStlInterpreterPluginBridge(
                    source,
                    qualifiedType,
                    declaration.QualifiedName,
                    prefix,
                    plan.Profile);
            }
        }

        if (anonymousNamespaceOpen)
        {
            source.WriteLine("} // namespace");
        }

        namespaces.Close();
    }

    private static void EmitDynamicTypes(SourceWriter source, GenerationPlan plan, bool metadataOnlyTypeInitialization)
    {
        source.BlankLine();
        source.WriteComment("RTI DynamicType accessors");
        source.OpenNamespaces("rti");
        source.OpenNamespaces("topic");
        foreach (var declaration in plan.Enums)
        {
            DynamicTypeEmitter.EmitDynamicTypeAccessor(source, declaration);
        }

        foreach (var declaration in plan.Structs)
        {
            DynamicTypeEmitter.EmitDynamicTypeAccessor(source, declaration, plan.Initialization.RequiresStagedInitialization,
                metadataOnlyTypeInitialization);
        }

        foreach (var declaration in plan.Unions)
        {
            DynamicTypeEmitter.EmitDynamicTypeAccessor(source, declaration, metadataOnlyTypeInitialization);
        }

        foreach (var declaration in plan.Aliases)
        {
            DynamicTypeEmitter.EmitDynamicTypeAccessor(source, declaration);
        }

        source.CloseNamespaces("topic");
        source.CloseNamespaces("rti");
    }

    private static void EmitFlatDataPrograms(SourceWriter source, GenerationPlan plan)
    {
        foreach (var declaration in plan.Structs.Where(declaration => declaration.IsFlatData))
        {
            source.BlankLine();
            source.WriteComment("RTI FlatData sample initialization programs.");
            source.WriteLine("namespace rti { namespace xcdr {");
            source.OpenBlock($"RTIXCdrInterpreterPrograms* type_programs<{declaration.CppType}>::get() ");
            source.WriteLine("return ::rti::xcdr::ProgramsSingleton<");
            source.WriteLine($"    {declaration.CppType},");
            source.WriteLine("    RTI_XCDR_INITIALIZE_SAMPLE_PROGRAM,");
            source.WriteLine("    true, true, true>::get_instance();");
            source.CloseBlock();
            source.WriteLine("} } // namespace rti::xcdr");
        }
    }

    private static void EmitTopicOperations(SourceWriter source, GenerationPlan plan)
    {
        source.BlankLine();
        source.WriteComment("DDS topic registration and CDR operations");
        source.OpenNamespaces("dds");
        source.OpenNamespaces("topic");
        foreach (var declaration in plan.Structs)
        {
            TopicSupportEmitter.EmitTopicSupport(source, declaration, plan);
        }

        foreach (var declaration in plan.Unions)
        {
            UnionTopicSupportEmitter.Emit(source, declaration, plan.Profile.RtiVersion == "6.1.2");
        }

        source.CloseNamespaces("topic");
        source.CloseNamespaces("dds");
    }

    internal static void EmitBanner(SourceWriter source, GenerationPlan plan)
    {
        source.WriteComment("<auto-generated/>");
        source.WriteComment($"Generated by Wireloom from '{plan.FileStem}.idl'.");
        source.WriteComment("Do not edit this file directly. Update the IDL source and rebuild.");
        source.WriteComment("Requires the RTI Connext DDS Modern C++ runtime.");
        source.BlankLine();
    }

    internal static void EmitExactRuntimeVersionGuard(
        SourceWriter source,
        RtiApiProfile profile,
        string feature)
    {
        var version = Version.Parse(profile.RtiVersion);
        source.WriteLine($"#if RTI_DDS_VERSION_MAJOR != {version.Major} || RTI_DDS_VERSION_MINOR != {version.Minor} || \\");
        source.WriteLine($"    RTI_DDS_VERSION_RELEASE != {version.Build} || RTI_DDS_VERSION_REVISION != 0");
        source.WriteLine($"#error \"{feature} requires RTI Connext DDS {profile.RtiVersion}.\"");
        source.WriteLine("#endif");
    }
}
