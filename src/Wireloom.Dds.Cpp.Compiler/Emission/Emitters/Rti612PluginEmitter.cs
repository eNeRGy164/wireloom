using System.Globalization;
using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits the retained RTI 6.1.2 C plugin adapter for supported structs.</summary>
internal static class Rti612PluginEmitter
{
    /// <summary>Writes value-backed DynamicType accessors for the legacy contexts.</summary>
    internal static void EmitDynamicTypes(SourceWriter source, GenerationPlan plan)
    {
        source.BlankLine();
        source.WriteComment("RTI DynamicType accessors");
        source.OpenNamespaces("rti");
        source.OpenNamespaces("topic");
        foreach (var declaration in plan.Enums)
        {
            source.BlankLine();
            source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
            source.OpenBlock($"dynamic_type<{declaration.CppType}>::get()");
            var context = declaration.NamespaceSegments.Count == 0
                ? $"::{declaration.ContextIdentifier}_instance()"
                : $"::{string.Join("::", declaration.NamespaceSegments)}::{declaration.ContextIdentifier}_instance()";
            source.WriteLine($"return {context}.type;");
            source.CloseBlock();
        }

        foreach (var declaration in plan.Aliases)
        {
            EmitAliasDynamicTypeAccessor(source, declaration);
        }

        foreach (var declaration in plan.Unions)
        {
            source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
            source.OpenBlock($"dynamic_type<{declaration.CppType}>::get()");
            var context = declaration.NamespaceSegments.Count == 0
                ? $"::{declaration.ContextIdentifier}_instance()"
                : $"::{string.Join("::", declaration.NamespaceSegments)}::{declaration.ContextIdentifier}_instance()";
            source.WriteLine($"return {context}.type;");
            source.CloseBlock();
            source.BlankLine();
        }

        foreach (var declaration in plan.Structs)
        {
            source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
            source.OpenBlock($"dynamic_type<{declaration.CppType}>::get()");
            var context = declaration.NamespaceSegments.Count == 0
                ? $"::{declaration.ContextIdentifier}_instance()"
                : $"::{string.Join("::", declaration.NamespaceSegments)}::{declaration.ContextIdentifier}_instance()";
            if (declaration.IsFlatData)
            {
                source.WriteLine("::wireloom_initialize_dynamic_types();");
                source.WriteLine($"return {context}.type;");
            }
            else
            {
                source.WriteLine($"return {context}.type;");
            }
            source.CloseBlock();
            source.BlankLine();

            if (declaration.IsFlatData)
            {
                source.WriteLine($"const dynamic_type<{declaration.FlatDataPlainHelperType}>::type&");
                source.OpenBlock($"dynamic_type<{declaration.FlatDataPlainHelperType}>::get()");
                source.WriteLine("::wireloom_initialize_dynamic_types();");
                source.WriteLine($"return {context}.plain_helper_type;");
                source.CloseBlock();
                source.BlankLine();
            }
        }

        source.CloseNamespaces("topic");
        source.CloseNamespaces("rti");
    }

    /// <summary>Writes includes required by the RTI 6.1.2 Modern C++ plugin API.</summary>
    internal static void WritePreamble(SourceWriter source, GenerationPlan plan)
    {
        PluginEmitter.EmitBanner(source, plan);
        source.WriteInclude($"{plan.FileStem}Plugin.hpp", quoted: true);
        source.BlankLine();
        source.WriteLine("#ifndef NDDS_STANDALONE_TYPE");
        source.BlankLine();
        source.WriteInclude("cstddef");
        source.WriteInclude("cstdint");
        source.WriteInclude("memory");

        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteInclude("algorithm");
            source.WriteInclude("cstring");
            source.WriteInclude("mutex");
            source.WriteInclude("new");
            source.WriteInclude("ndds/ndds_version.h");
            source.WriteInclude("ndds/xcdr/xcdr_flat_data.h");
            source.WriteInclude("ndds/xcdr/xcdr_stream.h");
            source.WriteInclude("ndds/reda/reda_fastBuffer.h");
            source.WriteInclude("rti/topic/flat/FlatData.hpp");
            PluginEmitter.EmitExactRuntimeVersionGuard(source, plan.Profile,
                "RTI 6.1.2 FlatData plugin adapter");
        }

        if (plan.Structs.Any(declaration => declaration.Members.Any(member => member.IsKey)))
        {
            source.WriteInclude("new");
        }

        source.WriteInclude("string");
        source.WriteInclude("ndds/pres/pres_typePlugin.h");
        source.WriteInclude("ndds/pres/pres_typePlugin_impl.h");
        source.WriteInclude("ndds/dds_c/dds_c_typecode_impl.h");
        source.WriteInclude("ndds/xcdr/xcdr_typeCode.h");
        source.WriteInclude("ndds/xcdr/xcdr_interpreter.h");
        source.WriteInclude("ndds/cdr/cdr_stream.h");
        source.WriteInclude("osapi/osapi_heap.h");
        source.WriteInclude("rti/domain/PluginSupport.hpp");
        source.WriteInclude("dds/core/xtypes/StructType.hpp");
        source.WriteInclude("dds/core/xtypes/MemberType.hpp");
        source.WriteInclude("dds/core/xtypes/PrimitiveTypes.hpp");

        if (plan.Enums.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/EnumType.hpp");
        }

        if (plan.Unions.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/UnionType.hpp");
        }

        if (plan.Aliases.Count > 0)
        {
            source.WriteInclude("dds/core/xtypes/AliasType.hpp");
        }

        if (plan.Structs.SelectMany(declaration => declaration.Members).Any(member =>
                member.Type.ContainsCollection || member.Type.ContainsString))
        {
            source.WriteInclude("dds/core/xtypes/CollectionTypes.hpp");
        }

        source.WriteInclude("rti/topic/cdr/InterpreterHelpers.hpp");
        source.WriteInclude("rti/topic/cdr/Serialization.hpp");
    }

    /// <summary>Writes DynamicType contexts with 6.1.2 sample access metadata.</summary>
    internal static void WriteTypeContexts(SourceWriter source, GenerationPlan plan)
    {
        var namespaces = new NamespaceBlockWriter(source);

        foreach (var declaration in plan.Enums)
        {
            source.BlankLine();
            namespaces.SwitchTo(declaration.NamespaceSegments);
            source.WriteLine($"struct {declaration.ContextIdentifier}");
            source.WriteLine("{");
            source.WriteLine("    ::dds::core::xtypes::EnumType type;");
            source.BlankLine();
            source.WriteLine($"    {declaration.ContextIdentifier}()");
            source.WriteLine($"        : type(\"{declaration.QualifiedName}\", {{");

            for (var index = 0; index < declaration.Members.Count; index++)
            {
                var member = declaration.Members[index];
                var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
                source.WriteLine(FormattableString.Invariant(
                    $"              ::dds::core::xtypes::EnumMember(\"{member.Name}\", {member.Value}){suffix}"));
            }

            source.WriteLine("          })");
            source.WriteLine("    {");
            source.WriteLine($"        type.extensibility_kind({declaration.ExtensibilityExpression});");
            source.WriteLine("    }");
            source.WriteLine("};");
            source.BlankLine();
            source.WriteLine($"{declaration.ContextIdentifier}& {declaration.ContextIdentifier}_instance()");
            source.OpenBrace();
            source.WriteLine($"static {declaration.ContextIdentifier} value;");
            source.WriteLine("return value;");
            source.CloseBlock();
        }

        foreach (var declaration in plan.Unions)
        {
            WriteUnionTypeContext(source, declaration, namespaces, plan);
        }

        foreach (var declaration in plan.Structs)
        {
            source.BlankLine();
            namespaces.SwitchTo(declaration.NamespaceSegments);

            if (declaration.IsFlatData)
            {
                source.WriteLine("namespace");
                source.WriteLine("{");
                FlatDataPluginEmitter.EmitFlatDataPluginContext(source, declaration, plan.Profile);
                source.WriteLine("} // namespace");
                continue;
            }

            source.WriteLine($"struct {declaration.ContextIdentifier}");
            source.WriteLine("{");
            source.WriteLine("    ::dds::core::xtypes::StructType type;");
            source.WriteLine("    ::RTIXCdrSampleAccessInfo sample_access_info;");
            source.WriteLine($"    ::RTIXCdrMemberAccessInfo member_access_infos[{declaration.Members.Count}];");
            source.WriteLine("    ::RTIXCdrTypePlugin type_plugin;");
            source.BlankLine();
            source.WriteLine($"    {declaration.ContextIdentifier}()");
            var typeConstructor = declaration.CppBaseType is { } baseType
                ? $"        : type(\"{declaration.QualifiedName}\", ::rti::topic::dynamic_type<{baseType}>::get(), {{"
                : $"        : type(\"{declaration.QualifiedName}\", {{";
            source.WriteLine(typeConstructor);

            for (var index = 0; index < declaration.Members.Count; index++)
            {
                var member = declaration.Members[index];
                var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
                var memberExpression =
                    $"::dds::core::xtypes::Member(\"{member.Name}\", {MetadataTypeExpression(member.Type)})";
                memberExpression += FormattableString.Invariant($".id({member.MemberId})");
                if (member.IsKey)
                {
                    memberExpression += ".key(true)";
                }

                if (member.IsOptional)
                {
                    memberExpression += ".optional(true)";
                }

                if (member.IsExternal)
                {
                    memberExpression += ".pointer(true)";
                }

                source.WriteLine($"              {memberExpression}{suffix}");
            }

            source.WriteLine("          })");
            source.WriteLine("        , sample_access_info(RTIXCdrSampleAccessInfo_INITIALIZER)");
            source.WriteLine("        , member_access_infos{}");
            source.WriteLine("        , type_plugin(RTIXCdrTypePlugin_INITIALIZER)");
            source.WriteLine("    {");
            source.WriteLine($"        type.extensibility_kind({declaration.ExtensibilityExpression});");
            source.WriteLine("        sample_access_info.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;");
            source.WriteLine("        sample_access_info.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;");
            source.WriteLine($"        sample_access_info.typeSize[0] = sizeof({declaration.CppType});");
            source.WriteLine("        sample_access_info.getMemberValuePointerFcn =");
            source.WriteLine($"            ::rti::topic::interpreter::get_aggregation_value_pointer<{declaration.CppType}>;");
            source.WriteLine("        sample_access_info.memberAccessInfos = member_access_infos;");
            source.WriteLine($"        {declaration.CppType} sample_value{{}};");
            source.WriteLine("        const auto* sample_address = reinterpret_cast<const char*>(&sample_value);");
            for (var index = 0; index < declaration.Members.Count; index++)
            {
                var member = declaration.Members[index];
                source.WriteLine($"        const auto* member_{index} = reinterpret_cast<const char*>(&sample_value.{member.Name});");
                source.WriteLine(FormattableString.Invariant(
                    $"        member_access_infos[{index}].bindingMemberValueOffset[0] ="));
                source.WriteLine($"            static_cast<::RTIXCdrUnsignedLong>(member_{index} - sample_address);");
            }

            source.WriteLine("        auto* native_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(");
            source.WriteLine("            const_cast<::DDS_TypeCode*>(&type.native()));");
            source.WriteLine("        native_type_code->_sampleAccessInfo = &sample_access_info;");
            source.WriteLine("        native_type_code->_typePlugin = &type_plugin;");
            NativeTypeEmitter.EmitMemberTypeCodeDefaults(
                source,
                declaration,
                "        ",
                "native_type_code->_members");

            for (var index = 0; index < declaration.Members.Count; index++)
            {
                WriteSampleAccessAssignments(
                    source,
                    declaration.Members[index].Type,
                    $"native_type_code->_members[{index}]._representation._typeCode",
                    plan);
            }

            source.WriteLine("    }");
            source.WriteLine("};");
            source.BlankLine();
            source.WriteLine($"{declaration.ContextIdentifier}& {declaration.ContextIdentifier}_instance()");
            source.OpenBrace();
            source.WriteLine($"static {declaration.ContextIdentifier} value;");
            source.WriteLine("return value;");
            source.CloseBlock();
        }

        namespaces.Close();

        EmitFlatDataTypeInitialization(source, plan);
    }

    /// <summary>Writes a type-generic PRESTypePlugin bridge for the supported struct shapes.</summary>
    internal static void WritePluginBridge(SourceWriter source, GenerationPlan plan)
    {
        source.BlankLine();
        source.WriteLine("namespace");
        source.WriteLine("{");
        source.WriteLines("""
            template <typename T>
            struct WireloomRti612TypePlugin
            {
                static PRESTypePluginParticipantData on_participant_attached(
                    void*,
                    const PRESTypePluginParticipantInfo* participant_info,
                    RTIBool,
                    void*,
                    RTICdrTypeCode*)
                {
                    auto* participant_data = static_cast<PRESTypePluginDefaultParticipantData*>(
                        PRESTypePluginDefaultParticipantData_new(participant_info));
                    if (participant_data == nullptr) {
                        return nullptr;
                    }

                    RTIXCdrInterpreterProgramsGenProperty properties =
                        RTIXCdrInterpreterProgramsGenProperty_INITIALIZER;
                    properties.generateV1Encapsulation = RTI_XCDR_TRUE;
                    properties.generateV2Encapsulation = RTI_XCDR_TRUE;
                    properties.resolveAlias = RTI_XCDR_TRUE;
                    properties.inlineStruct = RTI_XCDR_TRUE;
                    properties.optimizeEnum = RTI_XCDR_TRUE;

                    auto* type_code = const_cast<DDS_TypeCode*>(
                        &::rti::topic::dynamic_type<T>::get().native());
                    participant_data->programs = DDS_TypeCodeFactory_assert_programs_in_global_list(
                        DDS_TypeCodeFactory_get_instance(),
                        type_code,
                        &properties,
                        RTI_XCDR_PROGRAM_MASK_TYPEPLUGIN);
                    if (participant_data->programs == nullptr) {
                        PRESTypePluginDefaultParticipantData_delete(participant_data);
                        return nullptr;
                    }

                    return participant_data;
                }

                static void on_participant_detached(PRESTypePluginParticipantData participant_data)
                {
                    if (participant_data == nullptr) {
                        return;
                    }

                    auto* data = static_cast<PRESTypePluginDefaultParticipantData*>(participant_data);
                    if (data->programs != nullptr) {
                        DDS_TypeCodeFactory_remove_programs_from_global_list(
                            DDS_TypeCodeFactory_get_instance(), data->programs);
                        data->programs = nullptr;
                    }
                    PRESTypePluginDefaultParticipantData_delete(participant_data);
                }

                static void* create_sample()
                {
                    auto* sample = new T();
                    ::dds::topic::topic_type_support<T>::allocate_sample(*sample, 0, 0);
                    return sample;
                }

                static void destroy_sample(void* sample)
                {
                    delete static_cast<T*>(sample);
                }

                static void* create_plugin_sample(void*)
                {
                    return create_sample();
                }

                static void destroy_plugin_sample(void*, void* sample)
                {
                    destroy_sample(sample);
                }

                static PRESTypePluginEndpointData on_endpoint_attached(
                    PRESTypePluginParticipantData participant_data,
                    const PRESTypePluginEndpointInfo* endpoint_info,
                    RTIBool,
                    void*)
                {
                    if (participant_data == nullptr) {
                        return nullptr;
                    }

                    auto endpoint_data = PRESTypePluginDefaultEndpointData_new(
                        participant_data,
                        endpoint_info,
                        create_sample,
                        destroy_sample,
                        WireloomRti612KeyCallbacks<T>::create_key,
                        WireloomRti612KeyCallbacks<T>::destroy_key);
                    if (endpoint_data == nullptr) {
                        return nullptr;
                    }

                    if (WireloomRti612KeyCallbacks<T>::has_keys) {
                        const auto key_size = WireloomRti612KeyCallbacks<T>::get_serialized_key_max_size(
                            endpoint_data, RTI_FALSE, RTI_CDR_ENCAPSULATION_ID_CDR_BE, 0);
                        const auto key_size_v2 = WireloomRti612KeyCallbacks<T>::get_serialized_key_max_size_for_keyhash(
                            endpoint_data, RTI_CDR_ENCAPSULATION_ID_CDR2_BE, 0);
                        if (!PRESTypePluginDefaultEndpointData_createMD5StreamWithInfo(
                                endpoint_data, endpoint_info, key_size, key_size_v2)) {
                            PRESTypePluginDefaultEndpointData_delete(endpoint_data);
                            return nullptr;
                        }
                    }

                    if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER) {
                        auto maximum_size = get_serialized_sample_max_size(
                            endpoint_data, RTI_FALSE, RTI_CDR_ENCAPSULATION_ID_CDR_BE, 0);
                        PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(
                            endpoint_data, maximum_size);
                        if (!PRESTypePluginDefaultEndpointData_createWriterPool(
                                endpoint_data,
                                endpoint_info,
                                get_serialized_sample_max_size,
                                endpoint_data,
                                PRESTypePlugin_interpretedGetSerializedSampleSize,
                                endpoint_data)) {
                            PRESTypePluginDefaultEndpointData_delete(endpoint_data);
                            return nullptr;
                        }
                    }

                    return endpoint_data;
                }

                static void on_endpoint_detached(PRESTypePluginEndpointData endpoint_data)
                {
                    PRESTypePluginDefaultEndpointData_delete(endpoint_data);
                }

                static RTIBool deserialize_sample(
                    PRESTypePluginEndpointData endpoint_data,
                    void** sample,
                    RTIBool* drop_sample,
                    RTICdrStream* stream,
                    RTIBool deserialize_encapsulation,
                    RTIBool deserialize_sample,
                    void* endpoint_plugin_qos)
                {
                    if (sample != nullptr && *sample != nullptr && deserialize_sample) {
                        ::dds::topic::topic_type_support<T>::reset_sample(*static_cast<T*>(*sample));
                    }

                    return PRESTypePlugin_interpretedDeserializeWithAlloc(
                        endpoint_data,
                        sample,
                        drop_sample,
                        stream,
                        deserialize_encapsulation,
                        deserialize_sample,
                        endpoint_plugin_qos);
                }

                static RTIBool copy_sample(PRESTypePluginEndpointData, void* destination, const void* source)
                {
                    try {
                        *static_cast<T*>(destination) = *static_cast<const T*>(source);
                        return RTI_TRUE;
                    } catch (...) {
                        return RTI_FALSE;
                    }
                }

                static unsigned int get_serialized_sample_max_size(
                    PRESTypePluginEndpointData endpoint_data,
                    RTIBool include_encapsulation,
                    RTIEncapsulationId encapsulation_id,
                    unsigned int current_alignment)
                {
                    RTIBool overflow = RTI_FALSE;
                    auto size = PRESTypePlugin_interpretedGetSerializedSampleMaxSize(
                        endpoint_data,
                        &overflow,
                        include_encapsulation,
                        encapsulation_id,
                        current_alignment);
                    return overflow ? RTI_CDR_MAX_SERIALIZED_SIZE : size;
                }

                static PRESTypePlugin* create()
                {
                    PRESTypePlugin* plugin = nullptr;
                    RTIOsapiHeap_allocateStructure(&plugin, PRESTypePlugin);
                    if (plugin == nullptr) {
                        return nullptr;
                    }

                    const PRESTypePluginVersion version = PRES_TYPE_PLUGIN_VERSION_2_0;
                    const PRESTypePlugin default_plugin = PRES_TYPEPLUGIN_DEFAULT;
                    *plugin = default_plugin;
                    plugin->version = version;
                    plugin->onParticipantAttached = on_participant_attached;
                    plugin->onParticipantDetached = on_participant_detached;
                    plugin->onEndpointAttached = on_endpoint_attached;
                    plugin->onEndpointDetached = on_endpoint_detached;
                    plugin->copySampleFnc = copy_sample;
                    plugin->createSampleFnc = create_plugin_sample;
                    plugin->destroySampleFnc = destroy_plugin_sample;
                    plugin->serializeFnc = PRESTypePlugin_interpretedSerialize;
                    plugin->deserializeFnc = deserialize_sample;
                    plugin->getSerializedSampleMaxSizeFnc = get_serialized_sample_max_size;
                    plugin->getSerializedSampleMinSizeFnc = PRESTypePlugin_interpretedGetSerializedSampleMinSize;
                    plugin->getSerializedSampleSizeFnc = PRESTypePlugin_interpretedGetSerializedSampleSize;
                    using KeyCallbacks = WireloomRti612KeyCallbacks<T>;
                    plugin->getKeyKindFnc = KeyCallbacks::get_key_kind;
                    plugin->getSerializedKeyMaxSizeFnc = KeyCallbacks::get_serialized_key_max_size;
                    plugin->serializeKeyFnc = PRESTypePlugin_interpretedSerializeKey;
                    plugin->deserializeKeyFnc = KeyCallbacks::deserialize_key;
                    plugin->deserializeKeySampleFnc = PRESTypePlugin_interpretedDeserializeKey;
                    plugin->instanceToKeyHashFnc = PRESTypePlugin_interpretedInstanceToKeyHash;
                    plugin->serializedSampleToKeyHashFnc = KeyCallbacks::serialized_sample_to_keyhash;
                    plugin->getKeyFnc = PRESTypePluginDefaultEndpointData_getKey;
                    plugin->returnKeyFnc = PRESTypePluginDefaultEndpointData_returnKey;
                    plugin->instanceToKeyFnc = KeyCallbacks::instance_to_key;
                    plugin->keyToInstanceFnc = KeyCallbacks::key_to_instance;
                    plugin->serializedKeyToKeyHashFnc = nullptr;
                    plugin->typeCode = reinterpret_cast<RTICdrTypeCode*>(
                        const_cast<DDS_TypeCode*>(&::rti::topic::dynamic_type<T>::get().native()));
                    plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;
                    plugin->getSampleFnc = PRESTypePluginDefaultEndpointData_getSample;
                    plugin->returnSampleFnc = PRESTypePluginDefaultEndpointData_returnSample;
                    plugin->getBuffer = PRESTypePluginDefaultEndpointData_getBuffer;
                    plugin->returnBuffer = PRESTypePluginDefaultEndpointData_returnBuffer;
                    plugin->endpointTypeName = type_name().c_str();
                    plugin->isMetpType = RTI_FALSE;
                    return plugin;
                }

                static void destroy(PRESTypePlugin* plugin)
                {
                    RTIOsapiHeap_freeStructure(plugin);
                }

                static PRESTypePluginKeyKind get_key_kind()
                {
                    return WireloomRti612KeyCallbacks<T>::get_key_kind();
                }

                static const std::string& type_name()
                {
                    static const std::string value = ::dds::topic::topic_type_name<T>::value();
                    return value;
                }
            };
            """);
        source.BlankLine();
        source.WriteLine("void wireloom_initialize_type_plugins()");
        source.WriteLine("{");
        if (plan.Structs.Any(declaration => declaration.IsFlatData))
        {
            source.WriteLine("    ::wireloom_initialize_dynamic_types();");
        }

        foreach (var declaration in plan.Enums)
        {
            source.WriteLine($"    (void)::rti::topic::dynamic_type<{declaration.CppType}>::get();");
        }

        foreach (var declaration in plan.Aliases)
        {
            source.WriteLine($"    (void)::rti::topic::dynamic_type<{declaration.CppType}_AliasTag_t>::get();");
        }

        foreach (var declaration in plan.Initialization.TypeInitializationOrder)
        {
            var cppType = declaration.Struct?.CppType ?? declaration.Union!.CppType;
            source.WriteLine($"    (void)::rti::topic::dynamic_type<{cppType}>::get();");
        }

        source.WriteLine("}");
        source.WriteLine("} // namespace");
    }

    private static void EmitFlatDataTypeInitialization(SourceWriter source, GenerationPlan plan)
    {
        var declarations = plan.Structs.Where(declaration => declaration.IsFlatData).ToArray();
        if (declarations.Length == 0)
        {
            return;
        }

        source.BlankLine();
        source.WriteLine("namespace");
        source.WriteLine("{");
        source.WriteLine("::std::once_flag wireloom_dynamic_types_once;");
        source.WriteLine("void wireloom_initialize_dynamic_types()");
        source.WriteLine("{");
        source.WriteLine("    ::std::call_once(wireloom_dynamic_types_once, []");
        source.WriteLine("    {");

        foreach (var declaration in declarations)
        {
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "type;", 8);
            PluginSymbols.WriteContextMemberCall(source, PluginSymbols.ContextFunctionName(declaration), "plain_helper_type;", 8);
        }

        source.WriteLine("    });");
        source.WriteLine("}");
        source.WriteLine("} // namespace");
    }

    /// <summary>Writes profile-specific registration for one topic struct.</summary>
    internal static void WriteRegistration(SourceWriter source, StructPlan declaration)
    {
        source.WriteLine("    ::rti::domain::register_type_plugin(");
        source.WriteLine("        participant,");
        source.WriteLine("        name,");
        source.WriteLine($"        &WireloomRti612TypePlugin<{declaration.CppType}>::create,");
        source.WriteLine($"        &WireloomRti612TypePlugin<{declaration.CppType}>::destroy);");
    }

    /// <summary>Writes RTI 6.1.2 registration for a generated union type.</summary>
    internal static void WriteRegistration(SourceWriter source, UnionPlan declaration)
    {
        source.WriteLine("    ::rti::domain::register_type_plugin(");
        source.WriteLine("        participant,");
        source.WriteLine("        name,");
        source.WriteLine($"        &WireloomRti612TypePlugin<{declaration.CppType}>::create,");
        source.WriteLine($"        &WireloomRti612TypePlugin<{declaration.CppType}>::destroy);");
    }

    private static void WriteUnionTypeContext(
        SourceWriter source,
        UnionPlan declaration,
        NamespaceBlockWriter namespaces,
        GenerationPlan plan)
    {
        source.BlankLine();
        namespaces.SwitchTo(declaration.NamespaceSegments);
        source.WriteLine($"struct {declaration.ContextIdentifier}");
        source.WriteLine("{");
        source.WriteLine("    ::dds::core::xtypes::UnionType type;");
        source.WriteLine("    ::RTIXCdrSampleAccessInfo sample_access_info;");
        source.WriteLine($"    ::RTIXCdrMemberAccessInfo member_access_infos[{declaration.Branches.Count + 1}];");
        source.WriteLine("    ::RTIXCdrTypePlugin type_plugin;");
        source.BlankLine();
        source.WriteLine($"    {declaration.ContextIdentifier}()");
        source.WriteLine($"        : type(\"{declaration.QualifiedName}\", {UnionDiscriminatorType(declaration)}, {{");

        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            var branch = declaration.Branches[index];
            var suffix = index == declaration.Branches.Count - 1 ? string.Empty : ",";

            string labels;
            if (branch.IsDefault)
            {
                labels = "::dds::core::xtypes::UnionMember::DEFAULT_LABEL";
            }
            else if (branch.MetadataLabels.Count == 1)
            {
                labels = branch.MetadataLabels[0].ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                labels = $"::dds::core::xtypes::UnionMember::LabelSeq{{{string.Join(", ", branch.MetadataLabels)}}}";
            }

            source.WriteLine($"              ::dds::core::xtypes::UnionMember(\"{branch.Name}\", {MetadataTypeExpression(branch.Type)}, {labels}){suffix}");
        }

        source.WriteLine("          })");
        source.WriteLine("        , sample_access_info(RTIXCdrSampleAccessInfo_INITIALIZER)");
        source.WriteLine("        , member_access_infos{}");
        source.WriteLine("        , type_plugin(RTIXCdrTypePlugin_INITIALIZER)");
        source.WriteLine("    {");
        source.WriteLine($"        type.extensibility_kind({declaration.ExtensibilityExpression});");
        source.WriteLine("        sample_access_info.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;");
        source.WriteLine("        sample_access_info.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;");
        source.WriteLine($"        sample_access_info.typeSize[0] = sizeof({declaration.CppType});");
        source.WriteLine($"        sample_access_info.getMemberValuePointerFcn = ::rti::topic::interpreter::get_aggregation_value_pointer<{declaration.CppType}>;");
        source.WriteLine("        sample_access_info.memberAccessInfos = member_access_infos;");
        source.WriteLine($"        {declaration.CppType} sample{{}};");
        source.WriteLine("        member_access_infos[0].bindingMemberValueOffset[0] =");
        source.WriteLine("            static_cast<::RTIXCdrUnsignedLong>(reinterpret_cast<const char*>(&sample._d()) - reinterpret_cast<const char*>(&sample));");
        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            var branch = declaration.Branches[index];
            var sampleDiscriminator = branch.IsDefault
                ? branch.DefaultBranchAccessDiscriminator
                : branch.LabelExpressions[0];
            source.WriteLine($"        sample._d({sampleDiscriminator});");
            source.WriteLine($"        member_access_infos[{index + 1}].bindingMemberValueOffset[0] =");
            source.WriteLine("            static_cast<::RTIXCdrUnsignedLong>(");
            source.WriteLine($"                reinterpret_cast<const char*>(&sample.{branch.Name}()) - reinterpret_cast<const char*>(&sample));");
        }

        source.WriteLine("        auto* native_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(const_cast<::DDS_TypeCode*>(&type.native()));");
        source.WriteLine("        native_type_code->_sampleAccessInfo = &sample_access_info;");
        source.WriteLine("        native_type_code->_typePlugin = &type_plugin;");
        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            WriteSampleAccessAssignments(
                source,
                declaration.Branches[index].Type,
                $"native_type_code->_members[{index}]._representation._typeCode",
                plan);
        }

        source.WriteLine("    }");
        source.WriteLine("};");
        source.BlankLine();
        source.WriteLine($"{declaration.ContextIdentifier}& {declaration.ContextIdentifier}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {declaration.ContextIdentifier} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
    }

    private static string UnionDiscriminatorType(UnionPlan declaration) =>
        declaration.DiscriminatorEnumQualifiedName is { } enumName
            ? $"::rti::topic::dynamic_type<::{enumName}>::get()"
            : $"::dds::core::xtypes::primitive_type<{Rti612DiscriminatorMetadataType(declaration)}>()";

    private static string Rti612DiscriminatorMetadataType(UnionPlan declaration) =>
        declaration.DiscriminatorType is "::std::int8_t" or "std::int8_t" or "int8_t"
            || declaration.DiscriminatorMetadataTypeExpression.Contains("octet_tag_t", StringComparison.Ordinal)
                ? "::std::uint8_t"
                : declaration.DiscriminatorMetadataTypeExpression;

    private static void EmitAliasDynamicTypeAccessor(SourceWriter source, AliasPlan declaration)
    {
        source.WriteLine($"const dynamic_type<{declaration.CppType}_AliasTag_t>::type&");
        source.OpenBlock($"dynamic_type<{declaration.CppType}_AliasTag_t>::get()");
        source.WriteLine("static const dynamic_type<");
        source.WriteLine($"    {declaration.CppType}_AliasTag_t>::type type(");
        source.WriteLine($"    \"{declaration.QualifiedName.Replace(".", "::", StringComparison.Ordinal)}\",");
        source.WriteLine($"    {MetadataTypeExpression(declaration.MetadataTargetType)});");
        if (FindUnion(declaration.MetadataTargetType) is { } union)
        {
            source.WriteLines("""
                    auto* alias_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(
                        const_cast<::DDS_TypeCode*>(&type.native()));
                    auto* target_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(
                        const_cast<::DDS_TypeCode*>(&::rti::topic::dynamic_type<UNION_TYPE>::get().native()));
                    alias_type_code->_typeCode->_sampleAccessInfo = target_type_code->_sampleAccessInfo;
                    alias_type_code->_typeCode->_typePlugin = target_type_code->_typePlugin;
                """.Replace("UNION_TYPE", union.StorageType, StringComparison.Ordinal));
        }
        source.WriteLine("return type;");
        source.CloseBlock();
        source.BlankLine();
    }

    private static MemberTypePlan.Union? FindUnion(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Union union => union,
        MemberTypePlan.Alias alias => FindUnion(alias.Target),
        _ => null,
    };

    private static string MetadataTypeExpression(MemberTypePlan type)
    {
        switch (type)
        {
            case MemberTypePlan.Primitive primitive:
                return $"::dds::core::xtypes::primitive_type<{PrimitiveMetadataType(primitive)}>()";
            case MemberTypePlan.Enumeration:
                return $"::rti::topic::dynamic_type<{type.StorageType}>::get()";
            case MemberTypePlan.Struct or MemberTypePlan.Union:
                return $"::rti::topic::dynamic_type<{type.StorageType}>::get()";
            case MemberTypePlan.Alias alias:
                return $"::rti::topic::dynamic_type<{alias.StorageType}_AliasTag_t>::get()";
            case MemberTypePlan.String { IsWide: false } text:
                {
                    var bound = text.MetadataBound == 0
                        ? "::dds::core::xtypes::StringType::UNBOUNDED"
                        : text.MetadataBound.ToString(CultureInfo.InvariantCulture);
                    return $"::dds::core::xtypes::StringType({bound})";
                }
            case MemberTypePlan.String { IsWide: true } text:
                {
                    var bound = text.MetadataBound == 0
                        ? "::dds::core::xtypes::WStringType::UNBOUNDED"
                        : text.MetadataBound.ToString(CultureInfo.InvariantCulture);
                    return $"::dds::core::xtypes::WStringType({bound})";
                }
            case MemberTypePlan.Sequence sequence:
                {
                    var bound = sequence.IsUnbounded
                        ? "::dds::core::xtypes::SequenceType::UNBOUNDED"
                        : sequence.Bound.ToString(CultureInfo.InvariantCulture);
                    return $"::dds::core::xtypes::SequenceType({MetadataTypeExpression(sequence.ElementType)}, {bound})";
                }
            case MemberTypePlan.Array array:
                {
                    var elementExpression = MetadataTypeExpression(array.ElementType);
                    var dimensions = string.Join(", ", array.Dimensions);
                    return $"::dds::core::xtypes::ArrayType({elementExpression}, {{ {dimensions} }})";
                }
            default:
                throw new InvalidOperationException($"Unsupported RTI 6.1.2 member metadata type '{type.GetType().Name}'.");
        }
    }

    private static void WriteSampleAccessAssignments(
        SourceWriter source,
        MemberTypePlan type,
        string typeCodeExpression,
        GenerationPlan? plan,
        HashSet<string>? activeStructs = null)
    {
        activeStructs ??= new HashSet<string>(StringComparer.Ordinal);

        switch (type)
        {
            case MemberTypePlan.Alias alias:
                WriteSampleAccessAssignments(source, alias.Target, $"{typeCodeExpression}->_typeCode", plan, activeStructs);
                break;
            case MemberTypePlan.Sequence sequence:
                WriteSequenceSampleAccess(sequence.StorageType);
                WriteSampleAccessAssignments(source, sequence.ElementType, $"{typeCodeExpression}->_typeCode", plan, activeStructs);
                break;
            case MemberTypePlan.Array array:
                WriteSampleAccessAssignments(source, array.ElementType, $"{typeCodeExpression}->_typeCode", plan, activeStructs);
                break;
            case MemberTypePlan.Union union:
                source.WriteLine($"        {typeCodeExpression}->_sampleAccessInfo = reinterpret_cast<::RTIXCdrTypeCode*>(const_cast<::DDS_TypeCode*>(&::rti::topic::dynamic_type<{union.StorageType}>::get().native()))->_sampleAccessInfo;");
                source.WriteLine($"        {typeCodeExpression}->_typePlugin = reinterpret_cast<::RTIXCdrTypeCode*>(const_cast<::DDS_TypeCode*>(&::rti::topic::dynamic_type<{union.StorageType}>::get().native()))->_typePlugin;");
                if (plan?.Unions.SingleOrDefault(declaration => declaration.QualifiedName == union.QualifiedName) is { } unionDeclaration)
                {
                    for (var index = 0; index < unionDeclaration.Branches.Count; index++)
                    {
                        WriteSampleAccessAssignments(
                            source,
                            unionDeclaration.Branches[index].Type,
                            $"{typeCodeExpression}->_members[{index}]._representation._typeCode",
                            plan,
                            activeStructs);
                    }
                }

                break;
            case MemberTypePlan.Struct structure:
                source.WriteLine($"        {typeCodeExpression}->_sampleAccessInfo = reinterpret_cast<::RTIXCdrTypeCode*>(const_cast<::DDS_TypeCode*>(&::rti::topic::dynamic_type<{structure.StorageType}>::get().native()))->_sampleAccessInfo;");
                source.WriteLine($"        {typeCodeExpression}->_typePlugin = reinterpret_cast<::RTIXCdrTypeCode*>(const_cast<::DDS_TypeCode*>(&::rti::topic::dynamic_type<{structure.StorageType}>::get().native()))->_typePlugin;");
                if (plan?.Structs.SingleOrDefault(declaration => declaration.QualifiedName == structure.QualifiedName) is { } structDeclaration
                    && !activeStructs.Contains(structure.QualifiedName))
                {
                    var nestedActiveStructs = new HashSet<string>(activeStructs, StringComparer.Ordinal)
                    {
                        structure.QualifiedName,
                    };
                    for (var index = 0; index < structDeclaration.Members.Count; index++)
                    {
                        WriteSampleAccessAssignments(
                            source,
                            structDeclaration.Members[index].Type,
                            $"{typeCodeExpression}->_members[{index}]._representation._typeCode",
                            plan,
                            nestedActiveStructs);
                    }
                }

                break;
            case MemberTypePlan.String text:
                WriteSequenceSampleAccess(text.StorageType);
                break;
        }

        void WriteSequenceSampleAccess(string storageType)
        {
            source.WriteLine($"        {typeCodeExpression}->_sampleAccessInfo =");
            source.WriteLine($"            ::rti::topic::interpreter::detail::sequence_helper<{storageType}>::sample_access_info();");
        }
    }

    private static string PrimitiveMetadataType(MemberTypePlan.Primitive primitive)
    {
        if (primitive.StorageType is "::std::int8_t" or "std::int8_t" or "int8_t")
        {
            // The 6.1.2 DynamicType API represents the IDL octet kind through
            // uint8_t; the signed C++ field still carries the same one-byte wire representation.
            return "::std::uint8_t";
        }

        if (primitive.Mapping.IsOctet)
        {
            // RTI 6.1.2 maps uint8_t directly to the IDL octet TypeCode. The
            // octet_tag_t wrapper is used by the newer profile adapter only.
            return primitive.StorageType;
        }

        return primitive.StorageType switch
        {
            "::std::int64_t" => "::rti::core::int64",
            "::std::uint64_t" => "::rti::core::uint64",
            _ => primitive.StorageType
        };
    }
}
