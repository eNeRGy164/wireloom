using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits STL sample-access metadata and the interpreter-backed callback bridge.</summary>
internal static class StlPluginEmitter
{
    /// <summary>Writes the metadata RTI uses to access STL sample storage.</summary>
    internal static void EmitStlSampleAccessInfo(SourceWriter source, StructPlan declaration)
    {
        var typeName = declaration.CppType;
        source.WriteLines("""
                static ::RTIXCdrSampleAccessInfo* stl_sample_access_info()
                {
                    struct AccessInfo
                    {
                        ::RTIXCdrSampleAccessInfo sample;
            """);
        source.WriteLine(FormattableString.Invariant($"            ::RTIXCdrMemberAccessInfo members[{declaration.Members.Count}];"));
        source.BlankLine();
        source.WriteLines("""
                        AccessInfo()
                            : sample(RTIXCdrSampleAccessInfo_INITIALIZER)
                            , members{}
                        {
            """);
        source.WriteLine($"                {typeName} sample_value{{}};");
        source.WriteLine("                const auto* sample_address = reinterpret_cast<const char*>(&sample_value);");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            source.WriteLine(
                FormattableString.Invariant($"                const auto* member_{index}")
                + $" = reinterpret_cast<const char*>(&sample_value.{member.Name});");
            source.WriteLine(FormattableString.Invariant($"                members[{index}].bindingMemberValueOffset[0] ="));
            source.WriteLine(
                FormattableString.Invariant($"                    static_cast<::RTIXCdrUnsignedLong>(member_{index}")
                + " - sample_address);");
        }

        source.WriteLine("                sample.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;");
        const string sampleSizePrefix = "                sample.typeSize[0] = static_cast<::RTIXCdrUnsignedLong>(sizeof(";
        if (sampleSizePrefix.Length + typeName.Length + "));".Length <= 120)
        {
            source.WriteLine($"{sampleSizePrefix}{typeName}));");
        }
        else
        {
            source.WriteLine("                sample.typeSize[0] =");
            source.WriteLine("                    static_cast<::RTIXCdrUnsignedLong>(");
            source.WriteLine($"                        sizeof({typeName}));");
        }
        source.WriteLines("""
                            sample.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;
                            sample.getMemberValuePointerFcn =
                                ::rti::topic::interpreter::get_aggregation_value_pointer<
            """);
        source.WriteLine($"                        {typeName}>;");
        source.WriteLines("""
                            sample.memberAccessInfos = members;
                        }
                    };
            """);
        source.BlankLine();
        source.WriteLines("""
                    static AccessInfo access_info;
                    return &access_info.sample;
                }
            """);
    }

    /// <summary>Connects native TypeCode members to their STL sample-access metadata.</summary>
    internal static void EmitStlSampleAccessAssignments(
        SourceWriter source,
        GenerationPlan plan,
        StructPlan declaration,
        string indentation)
    {
        if (!RtiSampleAccess.HasInterpreterSampleAccess(plan, declaration)
            && !RtiSampleAccess.IsSequenceAggregateElement(plan, declaration))
        {
            return;
        }

        source.WriteLine("auto* native_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(");
        source.WriteLine("    const_cast<::DDS_TypeCode*>(&type->native()));");
        source.WriteLine($"{indentation}native_type_code->_sampleAccessInfo = stl_sample_access_info();");
        if (RtiSampleAccess.NeedsAggregateTypePluginMetadata(plan, declaration))
        {
            source.WriteLine($"{indentation}static ::RTIXCdrTypePlugin type_plugin = RTIXCdrTypePlugin_INITIALIZER;");
            source.WriteLine($"{indentation}native_type_code->_typePlugin = &type_plugin;");
        }
    }

    /// <summary>Writes sample-access metadata for string members.</summary>
    internal static void EmitStlStringMemberAccessInfo(
        SourceWriter source,
        GenerationPlan plan,
        StructPlan declaration,
        MemberPlan member,
        int memberIndex)
    {
        if ((!RtiSampleAccess.IsSequenceAggregateElement(plan, declaration)
                && !RtiSampleAccess.HasInterpreterSampleAccess(plan, declaration))
            || member.Type is not MemberTypePlan.String stringType)
        {
            return;
        }

        source.WriteLine(
            FormattableString.Invariant($"        type->native()._data._members[{memberIndex}")
            + "]._representation._typeCode->_sampleAccessInfo =");
        source.WriteLine("            ::rti::topic::interpreter::detail::sequence_helper<");
        source.WriteLine($"                {stringType.StorageType}>::sample_access_info();");
    }

    private static void EmitStlKeyPluginCallbacks(
        SourceWriter source,
        string typeName,
        string prefix,
        IReadOnlyList<MemberPlan> keyMembers)
    {
        source.OpenBlock($"void* {prefix}_create_key()");
        source.WriteLine($"return new (std::nothrow) {typeName}{{}};");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_delete_key(void* key)");
        source.WriteLine($"delete static_cast<{typeName}*>(key);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_instance_to_key(");
        source.WriteLine("    PRESTypePluginEndpointData, void* key, const void* instance)");
        source.OpenBrace();
        source.WriteLine($"auto* destination = static_cast<{typeName}*>(key);");
        source.WriteLine($"const auto* source = static_cast<const {typeName}*>(instance);");
        source.WriteLines("""
            if (destination == nullptr || source == nullptr)
                return RTI_FALSE;
            try {
            """);
        foreach (var member in keyMembers)
        {
            source.WriteLine($"    destination->{member.Name} = source->{member.Name};");
        }
        source.WriteLines("""
                return RTI_TRUE;
            } catch (...) {
                return RTI_FALSE;
            }
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_key_to_instance(");
        source.WriteLine("    PRESTypePluginEndpointData, void* instance, const void* key)");
        source.OpenBrace();
        source.WriteLine($"auto* destination = static_cast<{typeName}*>(instance);");
        source.WriteLine($"const auto* source = static_cast<const {typeName}*>(key);");
        source.WriteLines("""
            if (destination == nullptr || source == nullptr)
                return RTI_FALSE;
            try {
            """);
        foreach (var member in keyMembers)
        {
            source.WriteLine($"    destination->{member.Name} = source->{member.Name};");
        }
        source.WriteLines("""
                return RTI_TRUE;
            } catch (...) {
                return RTI_FALSE;
            }
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"unsigned int {prefix}_serialized_key_max_size(");
        source.WriteLine("    PRESTypePluginEndpointData endpoint, RTIBool include_encapsulation,");
        source.WriteLine("    RTIEncapsulationId encapsulation, unsigned int alignment)");
        source.OpenBrace();
        source.WriteLines("""
            RTIBool overflow = RTI_FALSE;
            const auto size = PRESTypePlugin_interpretedGetSerializedKeyMaxSize(
                endpoint, &overflow, include_encapsulation, encapsulation, alignment);
            return overflow ? RTI_CDR_MAX_SERIALIZED_SIZE : size;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_deserialize_key(");
        source.WriteLines("""
                PRESTypePluginEndpointData endpoint, void** key, RTIBool* drop_key,
                RTICdrStream* stream, RTIBool deserialize_encapsulation,
                RTIBool deserialize_key, void* endpoint_plugin_qos)
            """);
        source.OpenBrace();
        source.WriteLines("""
            (void) drop_key;
            if (key == nullptr || *key == nullptr || stream == nullptr)
                return RTI_FALSE;
            try {
                stream->_xTypesState.unassignable = RTI_FALSE;
                const auto result = PRESTypePlugin_interpretedDeserializeKey(
                    endpoint, *key, stream, deserialize_encapsulation,
                    deserialize_key, endpoint_plugin_qos);
                return result && !stream->_xTypesState.unassignable ? RTI_TRUE : RTI_FALSE;
            } catch (...) {
                return RTI_FALSE;
            }
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_serialized_sample_to_keyhash(");
        source.WriteLines("""
                PRESTypePluginEndpointData endpoint, RTICdrStream* stream,
                DDS_KeyHash_t* keyhash, RTIBool deserialize_encapsulation,
                void* endpoint_plugin_qos)
            """);
        source.OpenBrace();
        source.WriteLine($"auto* sample = static_cast<{typeName}*>(");
        source.WriteLines("""
                PRESTypePluginDefaultEndpointData_getTempSample(endpoint));
            if (sample == nullptr || stream == nullptr || keyhash == nullptr)
                return RTI_FALSE;
            if (!PRESTypePlugin_interpretedSerializedSampleToKey(
                    endpoint, sample, stream, deserialize_encapsulation,
                    RTI_TRUE, endpoint_plugin_qos)) {
                return RTI_FALSE;
            }
            return PRESTypePlugin_interpretedInstanceToKeyHash(
                endpoint, keyhash, sample, RTICdrStream_getEncapsulationKind(stream));
            """);
        source.CloseBlock();
        source.BlankLine();
    }

    /// <summary>Writes the interpreter-backed RTI plugin bridge for a value type.</summary>
    internal static void EmitStlInterpreterPluginBridge(
        SourceWriter source,
        string typeName,
        string qualifiedName,
        string prefix,
        RtiApiProfile profile,
        StructPlan? declaration = null)
    {
        var keyMembers = declaration?.Members.Where(member => member.IsKey).ToArray()
            ?? Array.Empty<MemberPlan>();
        var hasKeys = keyMembers.Length > 0;

        source.OpenBlock($"void* {prefix}_create_sample()");
        source.WriteLine($"return new (std::nothrow) {typeName}{{}};");
        source.CloseBlock();
        source.BlankLine();

        if (hasKeys)
        {
            EmitStlKeyPluginCallbacks(source, typeName, prefix, keyMembers);
        }

        source.OpenBlock($"void {prefix}_delete_sample(void* sample)");
        source.WriteLine($"delete static_cast<{typeName}*>(sample);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_copy_sample(");
        source.WriteLine("    PRESTypePluginEndpointData, void* destination, const void* source_sample)");
        source.OpenBrace();
        source.WriteLine("auto* destination_value =");
        source.WriteLine($"    static_cast<{typeName}*>(destination);");
        source.WriteLine("const auto* source_value =");
        source.WriteLine($"    static_cast<const {typeName}*>(source_sample);");
        source.WriteLines("""
            if (destination_value == nullptr || source_value == nullptr)
                return RTI_FALSE;
            try {
                *destination_value = *source_value;
                return RTI_TRUE;
            } catch (...) {
                return RTI_FALSE;
            }
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void {prefix}_return_sample(");
        source.WriteLine("    PRESTypePluginEndpointData endpoint, void* sample, void* handle)");
        source.OpenBrace();
        source.WriteLine("if (sample != nullptr)");
        source.WriteLine($"    *static_cast<{typeName}*>(sample) = {{}};");
        source.WriteLine("PRESTypePluginDefaultEndpointData_returnSample(endpoint, sample, handle);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"unsigned int {prefix}_serialized_sample_max_size(");
        source.WriteLine("    PRESTypePluginEndpointData endpoint, RTIBool include_encapsulation,");
        source.WriteLine("    RTIEncapsulationId encapsulation, unsigned int alignment)");
        source.OpenBrace();
        source.WriteLines("""
            RTIBool overflow = RTI_FALSE;
            const auto size = PRESTypePlugin_interpretedGetSerializedSampleMaxSize(
                endpoint, &overflow, include_encapsulation, encapsulation, alignment);
            return overflow ? RTI_CDR_MAX_SERIALIZED_SIZE : size;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"PRESTypePluginParticipantData {prefix}_participant_attached(");
        source.WriteLine("    void*, const PRESTypePluginParticipantInfo* participant_info,");
        source.WriteLine("    RTIBool, void*, RTICdrTypeCode*)");
        source.OpenBrace();
        var complianceMask = profile.RtiVersion switch
        {
            "7.3.1" => "0x0000068C",
            "7.7.0" => "0x0000018C",
            _ => throw new InvalidOperationException(
                $"No generated XTypes compliance mask is qualified for RTI {profile.RtiVersion}."),
        };
        source.WriteLine(
            "if (participant_info == nullptr || !RTIXCdrXTypesComplianceMask_verifyGeneratedXTypesMask("
            + $"{complianceMask}))");
        source.WriteLines("""
                return nullptr;
            auto* data = static_cast<PRESTypePluginDefaultParticipantData*>(
                PRESTypePluginDefaultParticipantData_new(participant_info));
            if (data == nullptr)
                return nullptr;
            RTIXCdrInterpreterProgramsGenProperty property =
                RTIXCdrInterpreterProgramsGenProperty_INITIALIZER;
            property.generateV1Encapsulation = RTI_XCDR_TRUE;
            property.generateV2Encapsulation = RTI_XCDR_TRUE;
            property.resolveAlias = RTI_XCDR_TRUE;
            property.inlineStruct = RTI_XCDR_TRUE;
            property.optimizeEnum = RTI_XCDR_TRUE;
            property.unboundedSize = RTIXCdrLong_MAX;
            property.externalReferenceSize =
                static_cast<RTIXCdrUnsignedShort>(sizeof(dds::core::external<char>));
            property.getExternalRefPointerFcn = ::rti::topic::interpreter::get_external_value_pointer;
            auto* native_type_code = const_cast<DDS_TypeCode*>(
                reinterpret_cast<const DDS_TypeCode*>(&
            """);
        source.WriteLine($"        ::rti::topic::dynamic_type<{typeName}>::get()");
        source.WriteLine("            .native()));");
        source.WriteLines("""
            data->programs = DDS_TypeCodeFactory_assert_programs_in_global_list(
                DDS_TypeCodeFactory_get_instance(), native_type_code,
                &property, RTI_XCDR_PROGRAM_MASK_TYPEPLUGIN);
            if (data->programs == nullptr) {
                PRESTypePluginDefaultParticipantData_delete(data);
                return nullptr;
            }
            return data;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_participant_detached(PRESTypePluginParticipantData participant)");
        source.WriteLines("""
            if (participant != nullptr) {
                auto* data = static_cast<PRESTypePluginDefaultParticipantData*>(participant);
                if (data->programs != nullptr) {
                    DDS_TypeCodeFactory_remove_programs_from_global_list(
                        DDS_TypeCodeFactory_get_instance(), data->programs);
                    data->programs = nullptr;
                }
            }
            PRESTypePluginDefaultParticipantData_delete(participant);
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"PRESTypePluginEndpointData {prefix}_endpoint_attached(");
        source.WriteLine("    PRESTypePluginParticipantData participant,");
        source.WriteLine("    const PRESTypePluginEndpointInfo* endpoint_info, RTIBool, void*)");
        source.OpenBrace();
        source.WriteLines("""
            if (participant == nullptr || endpoint_info == nullptr)
                return nullptr;
            auto endpoint = PRESTypePluginDefaultEndpointData_new(
            """);
        source.WriteLine($"    participant, endpoint_info, {prefix}_create_sample,");
        source.WriteLine($"    {prefix}_delete_sample,");
        if (hasKeys)
        {
            source.WriteLine($"    {prefix}_create_key,");
            source.WriteLine($"    {prefix}_delete_key);");
        }
        else
        {
            source.WriteLine("    nullptr, nullptr);");
        }
        source.WriteLine("if (endpoint == nullptr)");
        source.WriteLine("    return nullptr;");
        if (hasKeys)
        {
            source.WriteLine($"const auto key_max_size = {prefix}_serialized_key_max_size(");
            source.WriteLines("""
                    endpoint, RTI_FALSE, RTI_CDR_ENCAPSULATION_ID_CDR_BE, 0);
                RTIBool key_max_size_v2_overflow = RTI_FALSE;
                const auto key_max_size_v2 = PRESTypePlugin_interpretedGetSerializedKeyMaxSizeForKeyhash(
                    endpoint, &key_max_size_v2_overflow, RTI_CDR_ENCAPSULATION_ID_CDR2_BE, 0);
                if (key_max_size_v2_overflow)
                    return nullptr;
                if (!PRESTypePluginDefaultEndpointData_createMD5StreamWithInfo(
                        endpoint, endpoint_info, key_max_size, key_max_size_v2)) {
                    PRESTypePluginDefaultEndpointData_delete(endpoint);
                    return nullptr;
                }
                """);
        }
        source.WriteLine("if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER) {");
        source.WriteLine("    const auto maximum_size = " + prefix + "_serialized_sample_max_size(");
        source.WriteLines("""
                    endpoint, RTI_FALSE, RTI_CDR_ENCAPSULATION_ID_CDR_BE, 0);
                PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(endpoint, maximum_size);
                if (!PRESTypePluginDefaultEndpointData_createWriterPool(
            """);
        source.WriteLine("            endpoint, endpoint_info,");
        source.WriteLine("            " + prefix + "_serialized_sample_max_size, endpoint,");
        source.WriteLines("""
                        PRESTypePlugin_interpretedGetSerializedSampleSize, endpoint)) {
                    PRESTypePluginDefaultEndpointData_delete(endpoint);
                    return nullptr;
                }
            }
            return endpoint;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_endpoint_detached(PRESTypePluginEndpointData endpoint)");
        source.WriteLine("PRESTypePluginDefaultEndpointData_delete(endpoint);");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"PRESTypePlugin* {prefix}_create_plugin()");
        source.WriteLines("""
            auto* plugin = new (std::nothrow) PRESTypePlugin;
            if (plugin == nullptr)
                return nullptr;
            *plugin = PRES_TYPEPLUGIN_DEFAULT;
            plugin->version = PRES_TYPE_PLUGIN_VERSION_2_0;
            """);
        source.WriteLine($"plugin->onParticipantAttached = {prefix}_participant_attached;");
        source.WriteLine($"plugin->onParticipantDetached = {prefix}_participant_detached;");
        source.WriteLine($"plugin->onEndpointAttached = {prefix}_endpoint_attached;");
        source.WriteLine($"plugin->onEndpointDetached = {prefix}_endpoint_detached;");
        source.WriteLine($"plugin->copySampleFnc = {prefix}_copy_sample;");
        source.WriteLines("""
            plugin->createSampleFnc = PRESTypePluginDefaultEndpointData_createSample;
            plugin->destroySampleFnc = PRESTypePluginDefaultEndpointData_deleteSample;
            plugin->serializeFnc = PRESTypePlugin_interpretedSerialize;
            plugin->deserializeFnc = PRESTypePlugin_interpretedDeserializeWithAlloc;
            plugin->getSampleFnc = PRESTypePluginDefaultEndpointData_getSample;
            """);
        source.WriteLine($"plugin->returnSampleFnc = {prefix}_return_sample;");
        source.WriteLine($"plugin->getSerializedSampleMaxSizeFnc = {prefix}_serialized_sample_max_size;");
        source.WriteLine("plugin->getSerializedSampleMinSizeFnc = PRESTypePlugin_interpretedGetSerializedSampleMinSize;");
        source.WriteLine("plugin->getSerializedSampleSizeFnc = PRESTypePlugin_interpretedGetSerializedSampleSize;");
        if (hasKeys)
        {
            source.WriteLines("""
                plugin->getKeyKindFnc = []() { return PRES_TYPEPLUGIN_USER_KEY; };
                plugin->getKeyFnc = PRESTypePluginDefaultEndpointData_getKey;
                plugin->returnKeyFnc = PRESTypePluginDefaultEndpointData_returnKey;
                """);
            source.WriteLine($"plugin->getSerializedKeyMaxSizeFnc = {prefix}_serialized_key_max_size;");
            source.WriteLine("plugin->serializeKeyFnc = PRESTypePlugin_interpretedSerializeKey;");
            source.WriteLine($"plugin->deserializeKeyFnc = {prefix}_deserialize_key;");
            source.WriteLine("plugin->deserializeKeySampleFnc = PRESTypePlugin_interpretedDeserializeKey;");
            source.WriteLine("plugin->instanceToKeyHashFnc = PRESTypePlugin_interpretedInstanceToKeyHash;");
            source.WriteLine($"plugin->serializedSampleToKeyHashFnc = {prefix}_serialized_sample_to_keyhash;");
            source.WriteLine($"plugin->instanceToKeyFnc = {prefix}_instance_to_key;");
            source.WriteLine($"plugin->keyToInstanceFnc = {prefix}_key_to_instance;");
        }
        else
        {
            source.WriteLine("plugin->getKeyKindFnc = []() { return PRES_TYPEPLUGIN_NO_KEY; };");
        }
        source.WriteLine("auto* native_type_code = const_cast<DDS_TypeCode*>(");
        source.WriteLine("    reinterpret_cast<const DDS_TypeCode*>(&");
        source.WriteLine($"        ::rti::topic::dynamic_type<{typeName}>::get()");
        source.WriteLine("            .native()));");
        source.WriteLines("""
            plugin->typeCode = reinterpret_cast<RTICdrTypeCode*>(native_type_code);
            plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;
            plugin->getBuffer = PRESTypePluginDefaultEndpointData_getBuffer;
            plugin->returnBuffer = PRESTypePluginDefaultEndpointData_returnBuffer;
            """);
        source.WriteLine($"plugin->endpointTypeName = \"{qualifiedName}\";");
        source.WriteLine("return plugin;");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"void {prefix}_delete_plugin(PRESTypePlugin* plugin)");
        source.WriteLine("delete plugin;");
        source.CloseBlock();
        source.BlankLine();
    }

    /// <summary>Writes sequence member callbacks and native sample-access metadata.</summary>
    internal static void EmitSequenceMemberAccessInfo(
        SourceWriter source,
        MemberPlan member,
        int memberIndex,
        GenerationPlan plan,
        StructPlan declaration,
        bool hasSupportedSequenceGraph)
    {
        if (member.IsOptional
            && RtiSampleAccess.IsSelfRecursiveArrayPlan(member.Type, declaration.QualifiedName))
        {
            var recursiveArrayTypeCode = $"member_{memberIndex}_recursive_array_type_code";
            source.WriteLine($"        static ::DDS_TypeCode {recursiveArrayTypeCode} =");
            source.WriteLine($"            DDS_INITIALIZE_ARRAY_TYPECODE(1, {((MemberTypePlan.Array)member.Type).Dimensions[0]}L, NULL, NULL);");
            source.WriteLine(
                FormattableString.Invariant($"        type->native()._data._members[{memberIndex}]")
                + "._representation._typeCode =");
            source.WriteLine("            reinterpret_cast<::RTICdrTypeCode*>(");
            source.WriteLine($"                &{recursiveArrayTypeCode});");
            source.WriteLine($"        {recursiveArrayTypeCode}._data._typeCode =");
            source.WriteLine("            reinterpret_cast<::RTICdrTypeCode*>(");
            source.WriteLine("                const_cast<::DDS_TypeCode*>(&::rti::topic::dynamic_type<");
            source.WriteLine($"                    ::{declaration.QualifiedName}>::get(&wireloom_recursive_resource_node).native()));");
            source.WriteLine($"        {recursiveArrayTypeCode}._data._sampleAccessInfo =");
            source.WriteLine(
                "            ::rti::topic::interpreter::get_opt_array_sample_access_info<"
                + $"{declaration.CppType}>();");
            return;
        }

        if ((hasSupportedSequenceGraph
                || RtiSampleAccess.UsesDirectStringPluginBridge(plan, declaration))
            && member.Type is MemberTypePlan.Struct structure)
        {
            var nested = plan.Structs.Single(candidate => candidate.QualifiedName == structure.QualifiedName);
            var memberTypeCodeName = $"member_{memberIndex}_type_code";
            source.WriteLine($"        auto* {memberTypeCodeName} =");
            source.WriteLine(
                FormattableString.Invariant($"            type->native()._data._members[{memberIndex}")
                + "]._representation._typeCode;");
            NativeTypeEmitter.EmitAggregateTypeCodeAccessInfo(
                source,
                nested,
                memberTypeCodeName,
                $"member_{memberIndex}",
                plan,
                8,
                includeTypePluginMetadata: !hasSupportedSequenceGraph);
        }

        if (member.Type is MemberTypePlan.Alias aggregateAlias
            && RtiSampleAccess.IsAggregateAliasTarget(aggregateAlias))
        {
            source.WriteLine(
                FormattableString.Invariant($"        type->native()._data._members[{memberIndex}]._representation._typeCode ="));
            source.WriteLine("            reinterpret_cast<::RTICdrTypeCode*>(");
            source.WriteLine($"                {PluginSymbols.AggregateAliasTypeCodeFunctionName(aggregateAlias.AliasStorageType)}());");
            return;
        }

        var hasStringAlias = RtiSampleAccess.UsesDirectStringPluginBridge(plan, declaration)
            && member.Type is MemberTypePlan.Alias
            && RtiSampleAccess.UnwrapAlias(member.Type) is MemberTypePlan.String;

        if (hasSupportedSequenceGraph
            && member is
            {
                IsOptional: true,
                Type: MemberTypePlan.Array { Dimensions.Count: 1 } optionalArray,
            }
            && IsPrimitiveOrEnum(optionalArray.ElementType))
        {
            source.WriteLine(
                FormattableString.Invariant($"        type->native()._data._members[{memberIndex}")
                + "]._representation._typeCode->_sampleAccessInfo =");
            source.WriteLine(
                "            ::rti::topic::interpreter::get_opt_array_sample_access_info<"
                + $"{optionalArray.ElementType.StorageType}>();");
        }

        if (!hasSupportedSequenceGraph && !RtiSampleAccess.IsAggregateAliasTarget(member.Type) && !hasStringAlias)
        {
            return;
        }

        var typeCodeExpression = $"type->native()._data._members[{memberIndex}]._representation._typeCode";
        var nodeIndex = 0;
        NativeTypeEmitter.EmitNestedTypeCodeAccessInfo(
            source,
            member.Type,
            memberIndex,
            typeCodeExpression,
            ref nodeIndex,
            plan,
            stringSequenceElement: hasStringAlias);
    }

    private static bool IsPrimitiveOrEnum(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Primitive or MemberTypePlan.Enumeration => true,
        MemberTypePlan.Alias alias => IsPrimitiveOrEnum(alias.Target),
        _ => false,
    };
}
