using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits the RTI FlatData callbacks, plugin factory, and native plugin context.</summary>
internal static class FlatDataPluginEmitter
{
    private static void EmitFlatDataCallbacks(
        SourceWriter source,
        StructPlan declaration,
        RtiApiProfile profile)
    {
        var prefix = "wireloom_" + declaration.Name + "_flatdata";
        var typeName = declaration.CppType;
        var offsetType = declaration.FlatDataOffsetType;
        source.WriteComment($"RTI {profile.RtiVersion} FlatData callback adapter for this validated fixed-size shape.");
        source.OpenBlock($"void* {prefix}_create_sample()");
        source.WriteLine($"return {typeName}::create_data();");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_destroy_sample(void* sample)");
        source.WriteLine($"{typeName}{"::delete_data(static_cast<" + typeName + "*>(sample));"}");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void* {prefix}_create_flat_data_sentinel()");
        source.WriteLine("return reinterpret_cast<void*>(PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS);");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_destroy_flat_data_sentinel(void*)");
        source.WriteComment("The sentinel represents a loaned FlatData buffer and is not an allocated sample.");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_copy_sample(");
        source.WriteLines("""
                PRESTypePluginEndpointData,
                void* destination,
                const void* source_sample)
            """);
        source.OpenBrace();
        source.WriteLine($"auto* destination_sample = static_cast<{typeName}*>(destination);");
        source.WriteLine($"const auto* source_value = static_cast<const {typeName}*>(source_sample);");
        source.WriteLines("""
            if (destination_sample == nullptr || source_value == nullptr)
                return RTI_FALSE;
            std::memcpy(destination_sample->get_buffer(), source_value->get_buffer(), source_value->buffer_size());
            return RTI_TRUE;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"PRESTypePluginParticipantData {prefix}_participant_attached(");
        source.WriteLines("""
                void*,
                const PRESTypePluginParticipantInfo* participant_info,
                RTIBool,
                void*,
                RTICdrTypeCode*)
            """);
        source.OpenBrace();
        source.WriteLines("""
            if (participant_info == nullptr)
                return nullptr;
            """);
        if (profile.RtiVersion != "6.1.2")
        {
            source.WriteLine(
                $"if (!RTIXCdrXTypesComplianceMask_verifyGeneratedXTypesMask({GetComplianceMask(profile)}))");
            source.WriteLine("    return nullptr;");
        }

        source.WriteLines("""
            auto* participant_data = static_cast<PRESTypePluginDefaultParticipantData*>(
                PRESTypePluginDefaultParticipantData_new(participant_info));
            if (participant_data == nullptr)
                return nullptr;
            RTIXCdrInterpreterProgramsGenProperty property =
                RTIXCdrInterpreterProgramsGenProperty_INITIALIZER;
            property.generateV1Encapsulation = RTI_XCDR_FALSE;
            property.generateV2Encapsulation = RTI_XCDR_TRUE;
            property.resolveAlias = RTI_XCDR_TRUE;
            property.inlineStruct = RTI_XCDR_TRUE;
            property.optimizeEnum = RTI_XCDR_TRUE;
            property.unboundedSize = RTIXCdrLong_MAX;
            auto* programs = DDS_TypeCodeFactory_assert_programs_in_global_list(
                DDS_TypeCodeFactory_get_instance(),
            """);
        source.WriteLine($"    const_cast<DDS_TypeCode*>(&::rti::topic::dynamic_type<{typeName}PlainHelper>::get().native()),");
        source.WriteLines("""
                &property,
                RTI_XCDR_PROGRAM_MASK_TYPEPLUGIN);
            if (programs == nullptr) {
                PRESTypePluginDefaultParticipantData_delete(participant_data);
                return nullptr;
            }
            participant_data->programs = programs;
            return participant_data;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_participant_detached(PRESTypePluginParticipantData participant)");
        source.WriteLines("""
            if (participant == nullptr)
                return;
            auto* participant_data = static_cast<PRESTypePluginDefaultParticipantData*>(participant);
            if (participant_data->programs != nullptr) {
                DDS_TypeCodeFactory_remove_programs_from_global_list(
                    DDS_TypeCodeFactory_get_instance(), participant_data->programs);
                participant_data->programs = nullptr;
            }
            PRESTypePluginDefaultParticipantData_delete(participant);
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"unsigned int {prefix}_serialized_sample_max_size(");
        source.WriteLine("    PRESTypePluginEndpointData, RTIBool, RTIEncapsulationId, unsigned int);");
        source.WriteLine($"unsigned int {prefix}_serialized_sample_size(");
        source.WriteLine("    PRESTypePluginEndpointData, RTIBool, RTIEncapsulationId, unsigned int, const void*);");
        source.BlankLine();

        source.WriteLine($"PRESTypePluginEndpointData {prefix}_endpoint_attached(");
        source.WriteLines("""
                PRESTypePluginParticipantData participant,
                const PRESTypePluginEndpointInfo* endpoint_info,
                RTIBool,
                void*)
            """);
        source.OpenBrace();
        source.WriteLines("""
            if (participant == nullptr || endpoint_info == nullptr)
                return nullptr;
            const bool use_flat_data_pool = endpoint_info->usesFstSampleDataPool != RTI_FALSE;
            auto endpoint = PRESTypePluginDefaultEndpointData_new(
                participant,
                endpoint_info,
            """);
        source.WriteLine($"    use_flat_data_pool ? {prefix}_create_flat_data_sentinel : ");
        source.WriteLine($"        {prefix}_create_sample,");
        source.WriteLine($"    use_flat_data_pool ? {prefix}_destroy_flat_data_sentinel : ");
        source.WriteLine($"        {prefix}_destroy_sample,");
        source.WriteLines("""
                nullptr,
                nullptr);
            if (endpoint == nullptr)
                return nullptr;
            """);
        source.WriteLine($"const auto maximum_size = {offsetType}::serialized_size(0)");
        source.WriteLines("""
                + RTI_XCDR_ENCAPSULATION_HEADER_SIZE;
            PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(endpoint, maximum_size);
            if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER
                && !PRESTypePluginDefaultEndpointData_createWriterPool(
                    endpoint,
                    endpoint_info,
            """);
        source.WriteLine($"        {prefix}_serialized_sample_max_size,");
        source.WriteLine("        endpoint,");
        source.WriteLine($"        {prefix}_serialized_sample_size,");
        source.WriteLines("""
                    endpoint)) {
                PRESTypePluginDefaultEndpointData_delete(endpoint);
                return nullptr;
            }
            return endpoint;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"void {prefix}_endpoint_detached(PRESTypePluginEndpointData endpoint)");
        source.WriteLine("PRESTypePluginDefaultEndpointData_delete(endpoint);");
        source.CloseBlock();
        source.BlankLine();

        EmitFlatDataSizeCallbacks(source, prefix, offsetType);
        EmitFlatDataStreamCallbacks(source, prefix, offsetType, typeName);
        EmitFlatDataSampleCallbacks(source, prefix, typeName, profile);
        EmitFlatDataPluginFactory(source, declaration, prefix, typeName, profile);
    }

    private static void EmitFlatDataSizeCallbacks(
        SourceWriter source,
        string prefix,
        string offsetType)
    {
        source.WriteLine($"unsigned int {prefix}_serialized_sample_max_size(");
        source.WriteLines("""
                PRESTypePluginEndpointData,
                RTIBool include_encapsulation,
                RTIEncapsulationId,
                unsigned int current_alignment)
            """);
        source.OpenBrace();
        source.WriteLine($"return {offsetType}::serialized_size(current_alignment)");
        source.WriteLine("    + (include_encapsulation ? RTI_XCDR_ENCAPSULATION_HEADER_SIZE : 0);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"unsigned int {prefix}_serialized_sample_min_size(");
        source.WriteLines("""
                PRESTypePluginEndpointData endpoint,
                RTIBool include_encapsulation,
                RTIEncapsulationId encapsulation,
                unsigned int current_alignment)
            """);
        source.OpenBrace();
        source.WriteLine($"return {prefix}_serialized_sample_max_size(");
        source.WriteLine("    endpoint, include_encapsulation, encapsulation, current_alignment);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"unsigned int {prefix}_serialized_sample_size(");
        source.WriteLines("""
                PRESTypePluginEndpointData,
                RTIBool include_encapsulation,
                RTIEncapsulationId,
                unsigned int current_alignment,
                const void*)
            """);
        source.OpenBrace();
        source.WriteLine($"return {offsetType}::serialized_size(current_alignment)");
        source.WriteLine("    + (include_encapsulation ? RTI_XCDR_ENCAPSULATION_HEADER_SIZE : 0);");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"unsigned int {prefix}_deserialized_sample_max_size(PRESTypePluginEndpointData)");
        source.WriteLine($"return RTI_XCDR_ENCAPSULATION_HEADER_SIZE + {offsetType}::serialized_size(0);");
        source.CloseBlock();
        source.BlankLine();

        source.OpenBlock($"PRESTypePluginKeyKind {prefix}_key_kind()");
        source.WriteLine("return PRES_TYPEPLUGIN_NO_KEY;");
        source.CloseBlock();
        source.BlankLine();
    }

    private static void EmitFlatDataStreamCallbacks(
        SourceWriter source,
        string prefix,
        string offsetType,
        string typeName)
    {
        source.WriteLine($"RTIBool {prefix}_serialize(");
        source.WriteLines("""
                PRESTypePluginEndpointData,
                const void* sample_data,
                RTICdrStream* cdr_stream,
                RTIBool serialize_encapsulation,
                RTIEncapsulationId encapsulation,
                RTIBool serialize_sample,
                void*)
            """);
        source.OpenBrace();
        source.WriteLine($"const auto* sample = static_cast<const {typeName}*>(sample_data);");
        source.WriteLines("""
            if (sample == nullptr || cdr_stream == nullptr
                || (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                    && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE))
                return RTI_FALSE;
            auto* stream = RTICdrStream_asXCdrStream(cdr_stream);
            char* alignment_position = nullptr;
            if (serialize_encapsulation) {
                if (!RTICdrStream_serializeAndSetCdrEncapsulation(cdr_stream, encapsulation))
                    return RTI_FALSE;
                alignment_position = RTICdrStream_resetAlignment(cdr_stream);
            }
            if (RTIXCdrFlatSample_getEncapsulation(reinterpret_cast<const char*>(sample->get_buffer())) != encapsulation)
                return RTI_FALSE;
            const bool serialized = !serialize_sample || RTIXCdrStream_skipNByte(
            """);
        source.WriteLine("    stream, 1, " + offsetType + "::serialized_size(0));");
        source.WriteLines("""
            if (serialize_encapsulation)
                RTICdrStream_restoreAlignment(cdr_stream, alignment_position);
            return serialized ? RTI_TRUE : RTI_FALSE;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_deserialize(");
        source.WriteLines("""
                PRESTypePluginEndpointData endpoint,
                void** sample_data,
                RTIBool* drop_sample,
                RTICdrStream* cdr_stream,
                RTIBool deserialize_encapsulation,
                RTIBool deserialize_sample,
                void*)
            """);
        source.OpenBrace();
        source.WriteLines("""
            if (sample_data == nullptr || cdr_stream == nullptr)
                return RTI_FALSE;
            auto* stream = RTICdrStream_asXCdrStream(cdr_stream);
            if (deserialize_encapsulation
                && !RTIXCdrStream_deserializeAndSetCdrEncapsulation(stream))
                return RTI_FALSE;
            const auto encapsulation = RTIXCdrStream_getEncapsulationId(stream);
            if (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE)
                return RTI_FALSE;
            """);
        source.WriteLine($"const auto sample_size = {offsetType}::serialized_size(0);");
        source.WriteLines("""
            auto* sample = reinterpret_cast<char*>(*sample_data);
            const auto sentinel = reinterpret_cast<void*>(PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS);
            if (sample == sentinel) {
                sample = static_cast<char*>(PRESTypePluginDefaultEndpointData_getFstSampleWithSize(
                    endpoint, static_cast<int>(sample_size + RTI_XCDR_ENCAPSULATION_HEADER_SIZE)));
                if (sample == nullptr)
                    return RTI_FALSE;
                if (!RTIXCdrFlatSample_initializeEncapsulation(sample, encapsulation))
                    return RTI_FALSE;
                *sample_data = sample;
            } else if (sample == nullptr) {
            """);
        source.WriteLine($"    sample = reinterpret_cast<char*>({typeName}::create_data());");
        source.WriteLines("""
                if (sample == nullptr)
                    return RTI_FALSE;
                *sample_data = sample;
                if (drop_sample != nullptr)
                    *drop_sample = RTI_TRUE;
            } else if (RTIXCdrFlatSample_getEncapsulation(sample) != encapsulation) {
                return RTI_FALSE;
            }
            if (deserialize_sample) {
                if (*sample_data == RTICdrStream_getBuffer(cdr_stream)) {
                    if (!RTIXCdrStream_skipNByte(stream, 1, sample_size))
                        return RTI_FALSE;
                } else if (!RTIXCdrStream_deserializeNByte(
                    stream, reinterpret_cast<unsigned char*>(sample + RTI_XCDR_ENCAPSULATION_HEADER_SIZE), 1, sample_size)) {
                    return RTI_FALSE;
                }
            }
            return RTI_TRUE;
            """);
        source.CloseBlock();
        source.BlankLine();
    }

    private static void EmitFlatDataSampleCallbacks(
        SourceWriter source,
        string prefix,
        string typeName,
        RtiApiProfile profile)
    {
        source.OpenBlock($"void* {prefix}_get_sample(PRESTypePluginEndpointData endpoint, void** handle)");
        source.WriteLine("return PRESTypePluginDefaultEndpointData_getSample(endpoint, handle);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void {prefix}_return_sample(");
        source.WriteLine("    PRESTypePluginEndpointData endpoint, void* sample, void* handle)");
        source.OpenBrace();
        source.WriteLines("""
            auto* pool = PRESTypePluginDefaultEndpointData_getFstSampleDataPool(endpoint);
            if (pool != nullptr && sample != reinterpret_cast<void*>(PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS)) {
                REDAFastBufferPool_returnBuffer(pool, sample);
                if (handle != nullptr)
                    *static_cast<void**>(handle) = reinterpret_cast<void*>(PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS);
            }
            if (handle != nullptr)
                PRESTypePluginDefaultEndpointData_returnSample(endpoint, sample, handle);
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void* {prefix}_writer_loan(PRESTypePluginEndpointData endpoint,");
        source.WriteLine("    const REDAFastBufferPoolGrowthProperty* allocation)");
        source.OpenBrace();
        source.WriteLine("return PRESTypePluginDefaultEndpointData_getWriterLoanedSample(endpoint, allocation);");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"void {prefix}_return_writer_loan(PRESTypePluginEndpointData endpoint, const void* sample)");
        source.WriteLine("PRESTypePluginDefaultEndpointData_returnWriterLoanedSample(endpoint, sample);");
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"void {prefix}_return_writer_loan_cookie(PRESTypePluginEndpointData endpoint,");
        source.WriteLine("    const REDABuffer* cookie)");
        source.OpenBrace();
        source.WriteLine("PRESTypePluginDefaultEndpointData_returnWriterLoanedSampleFromCookie(endpoint, cookie);");
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"RTIBool {prefix}_validate_writer_loan(PRESTypePluginEndpointData endpoint,");
        source.WriteLine("    const void* sample, RTIBool discard)");
        source.OpenBrace();
        source.WriteLine("return PRESTypePluginDefaultEndpointData_validateWriterLoanedSample(endpoint, sample, discard);");
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"void {prefix}_set_writer_serialized_state(PRESTypePluginEndpointData endpoint,");
        source.WriteLine("    const void* sample)");
        source.OpenBrace();
        source.WriteLine("PRESTypePluginDefaultEndpointData_setWriterLoanedSampleSerializedState(endpoint, sample);");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"RTIBool {prefix}_get_buffer_with_params(");
        source.WriteLines("""
                PRESTypePluginEndpointData endpoint,
                REDABuffer* buffer,
                const void* user_data,
                const NDDS_WriterHistory_AllocatorParams* allocator_params)
            """);
        source.OpenBrace();
        if (profile.RtiVersion == "6.1.2")
        {
            source.WriteLines("""
                if (allocator_params != nullptr && buffer != nullptr
                    && allocator_params->sampleKind == NDDS_WRITERHISTORY_DATA_SAMPLE) {
                """);
        }
        else
        {
            source.WriteLines("""
                if (allocator_params != nullptr && buffer != nullptr
                    && allocator_params->sampleKind == NDDS_WRITERHISTORY_DATA_SAMPLE
                    && !allocator_params->forceUseWHPoolWithFlatData) {
                """);
        }
        source.WriteLine("    const auto* sample = static_cast<const " + typeName + "*>(user_data);");
        source.WriteLines("""
                if (sample == nullptr)
                    return RTI_FALSE;
                buffer->pointer = reinterpret_cast<char*>(const_cast<unsigned char*>(sample->get_buffer()));
                buffer->length = static_cast<int>(sample->buffer_size());
                return RTI_TRUE;
            }
            return PRESTypePluginDefaultEndpointData_getBuffer(
                endpoint, buffer, allocator_params == nullptr ? RTI_XCDR_ENCAPSULATION_ID_CDR2_BE : allocator_params->encapsulationId, user_data);
            """);
        source.CloseBlock();
        source.BlankLine();

        var returnBufferReturnType = profile.RtiVersion == "7.7.0" ? "void" : "RTIBool";
        source.WriteLine($"{returnBufferReturnType} {prefix}_return_buffer_with_params(");
        source.WriteLines("""
                PRESTypePluginEndpointData endpoint,
                REDABuffer* buffer,
                const NDDS_WriterHistory_AllocatorParams* allocator_params)
            """);
        source.OpenBrace();
        if (profile.RtiVersion == "6.1.2")
        {
            source.WriteLines("""
                if (allocator_params == nullptr || buffer == nullptr
                    || allocator_params->sampleKind != NDDS_WRITERHISTORY_DATA_SAMPLE) {
                    PRESTypePluginDefaultEndpointData_returnBuffer(
                        endpoint, buffer, allocator_params == nullptr ? RTI_XCDR_ENCAPSULATION_ID_CDR2_BE : allocator_params->encapsulationId);
                }
                """);
        }
        else
        {
            source.WriteLines("""
                if (allocator_params == nullptr || buffer == nullptr
                    || allocator_params->sampleKind != NDDS_WRITERHISTORY_DATA_SAMPLE
                    || allocator_params->forceUseWHPoolWithFlatData) {
                    PRESTypePluginDefaultEndpointData_returnBuffer(
                        endpoint, buffer, allocator_params == nullptr ? RTI_XCDR_ENCAPSULATION_ID_CDR2_BE : allocator_params->encapsulationId);
                }
                """);
        }
        if (profile.RtiVersion != "7.7.0")
        {
            source.WriteLine("return RTI_TRUE;");
        }
        source.CloseBlock();
        source.BlankLine();
    }

    private static void EmitFlatDataPluginFactory(
        SourceWriter source,
        StructPlan declaration,
        string prefix,
        string typeName,
        RtiApiProfile profile)
    {
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
        source.WriteLine("plugin->createSampleFnc = PRESTypePluginDefaultEndpointData_createSample;");
        source.WriteLine("plugin->destroySampleFnc = PRESTypePluginDefaultEndpointData_deleteSample;");
        source.WriteLine($"plugin->serializeFnc = {prefix}_serialize;");
        source.WriteLine($"plugin->deserializeFnc = {prefix}_deserialize;");
        source.WriteLine($"plugin->getSampleFnc = {prefix}_get_sample;");
        source.WriteLine($"plugin->returnSampleFnc = {prefix}_return_sample;");
        source.WriteLine($"plugin->getSerializedSampleMaxSizeFnc = {prefix}_serialized_sample_max_size;");
        source.WriteLine($"plugin->getSerializedSampleMinSizeFnc = {prefix}_serialized_sample_min_size;");
        source.WriteLine($"plugin->getSerializedSampleSizeFnc = {prefix}_serialized_sample_size;");
        source.WriteLine($"plugin->getDeserializedSampleMaxSizeFnc = {prefix}_deserialized_sample_max_size;");
        source.WriteLine($"plugin->getKeyKindFnc = {prefix}_key_kind;");
        source.WriteLine("plugin->typeCode = const_cast<RTICdrTypeCode*>(");
        source.WriteLine($"    reinterpret_cast<const RTICdrTypeCode*>(&::rti::topic::dynamic_type<{typeName}>::get().native()));");
        source.WriteLine("plugin->languageKind = "
            + (profile.RtiVersion == "7.3.1" ? "PRES_TYPEPLUGIN_CPPSTL_LANG;" : "PRES_TYPEPLUGIN_FLATDATA_TYPE;"));
        source.WriteLine($"plugin->getBufferWithParams = {prefix}_get_buffer_with_params;");
        source.WriteLine($"plugin->returnBufferWithParams = {prefix}_return_buffer_with_params;");
        source.WriteLines("""
            plugin->getWriterLoanedSampleFnc = PRESTypePluginDefaultEndpointData_getWriterLoanedSample;
            plugin->returnWriterLoanedSampleFnc = PRESTypePluginDefaultEndpointData_returnWriterLoanedSample;
            plugin->returnWriterLoanedSampleFromCookieFnc = PRESTypePluginDefaultEndpointData_returnWriterLoanedSampleFromCookie;
            plugin->validateWriterLoanedSampleFnc = PRESTypePluginDefaultEndpointData_validateWriterLoanedSample;
            plugin->setWriterLoanedSampleSerializedStateFnc = PRESTypePluginDefaultEndpointData_setWriterLoanedSampleSerializedState;
            """);
        source.WriteLine($"plugin->endpointTypeName = \"{declaration.QualifiedName}\";");
        source.WriteLines("""
            plugin->isMetpType = RTI_FALSE;
            """);
        if (profile.RtiVersion == "7.7.0")
        {
            // RTI 7.7 adds this plugin metadata field; it is absent from the 7.3.1 ABI.
            source.WriteLine("plugin->isRecursiveType = RTI_FALSE;");
        }
        source.WriteLine("return plugin;");
        source.CloseBlock();
        source.BlankLine();
        source.OpenBlock($"void {prefix}_delete_plugin(PRESTypePlugin* plugin)");
        source.WriteLine("delete plugin;");
        source.CloseBlock();
    }

    /// <summary>Writes the native context and plugin creation for a FlatData sample.</summary>
    internal static void EmitFlatDataPluginContext(
        SourceWriter source,
        StructPlan declaration,
        RtiApiProfile profile)
    {
        if (profile.RtiVersion == "6.1.2")
        {
            EmitRti612FlatDataPluginContext(source, declaration, profile);
            return;
        }

        var contextName = declaration.ContextIdentifier;
        var helperType = declaration.FlatDataPlainHelperType;
        source.WriteLine($"struct {contextName}");
        source.WriteLines("""
            {
                ::rti::topic::cdr::GenericTypePluginFactory& factory;
                ::dds::core::xtypes::StructType* type;
                ::dds::core::xtypes::StructType* plain_helper_type;
                ::rti::topic::cdr::CTypePlugin* plain_helper_type_plugin;
            """);
        source.BlankLine();
        source.WriteLine($"    {contextName}()");
        source.WriteLines("""
                    : factory(::rti::topic::cdr::GenericTypePluginFactory::instance())
                    , type(nullptr)
                    , plain_helper_type(nullptr)
                    , plain_helper_type_plugin(nullptr)
                {
                }
            """);
        source.BlankLine();
        source.WriteLines("""
                void create_type()
                {
                    type = factory.create_struct(
            """);
        source.WriteLine($"            \"{declaration.QualifiedName}\",");
        source.WriteLine($"            {declaration.ExtensibilityExpression},");
        source.WriteLine($"            sizeof({helperType}),");
        source.WriteLine("            {");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
            source.WriteLine($"                offsetof({helperType}, {member.Name}){suffix}");
        }

        source.WriteLine("            });");
        NativeTypeEmitter.EmitPrivateTypeCodeAnnotations(source, declaration, "        ");
        source.WriteLine("        plain_helper_type = factory.create_struct(");
        source.WriteLine($"            \"{declaration.QualifiedName}PlainHelper\",");
        source.WriteLine("            ::dds::core::xtypes::ExtensibilityKind::FINAL,");
        source.WriteLine($"            sizeof({helperType}),");
        source.WriteLine("            {");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
            source.WriteLine($"                offsetof({helperType}, {member.Name}){suffix}");
        }

        source.WriteLines("""
                        });
                    // Generated TypeCode annotation: the helper is XCDR2-only.
                    plain_helper_type->native()._data._annotations._allowedDataRepresentationMask = RTI_XCDR_XCDR2;
                }
            """);
        source.BlankLine();
        source.WriteLines("""
                void create_plain_helper_type_plugin()
                {
                    plain_helper_type_plugin = &factory.create_type_plugin(*plain_helper_type);
                }
            """);
        source.BlankLine();
        source.WriteLines("""
                void add_members()
                {
                    // DynamicType member layout follows the plain helper, not the FlatData sample object.
            """);
        foreach (var member in declaration.Members)
        {
            DynamicTypeEmitter.EmitMemberRegistration(source, member, stagedInitialization: true,
                ownerQualifiedName: declaration.QualifiedName);
            DynamicTypeEmitter.EmitMemberRegistration(source, member, stagedInitialization: true,
                ownerQualifiedName: declaration.QualifiedName + "PlainHelper", typeMember: "plain_helper_type");
        }

        source.WriteLine("    }");
        source.WriteLine("};");
        source.BlankLine();
        source.WriteLine($"{contextName}& {contextName}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {contextName} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
        source.BlankLine();
        EmitFlatDataCallbacks(source, declaration, profile);
    }

    private static void EmitRti612FlatDataPluginContext(
        SourceWriter source,
        StructPlan declaration,
        RtiApiProfile profile)
    {
        var contextName = declaration.ContextIdentifier;
        var helperType = declaration.FlatDataPlainHelperType;
        source.WriteLine($"struct {contextName}");
        source.WriteLine("{");
        source.WriteLine("    ::dds::core::xtypes::StructType type;");
        source.WriteLine("    ::dds::core::xtypes::StructType plain_helper_type;");
        source.WriteLine("    ::RTIXCdrSampleAccessInfo flat_sample_access_info;");
        source.WriteLine("    ::RTIXCdrMemberAccessInfo flat_member_access_infos[" + declaration.Members.Count + "];");
        source.WriteLine("    ::RTIXCdrSampleAccessInfo helper_sample_access_info;");
        source.WriteLine("    ::RTIXCdrMemberAccessInfo helper_member_access_infos[" + declaration.Members.Count + "];");
        source.WriteLine("    ::RTIXCdrTypePlugin flat_type_plugin;");
        source.WriteLine("    ::RTIXCdrTypePlugin helper_type_plugin;");
        source.BlankLine();
        source.WriteLine($"    {contextName}()");
        source.WriteLine($"        : type(\"{declaration.QualifiedName}\", {{");
        EmitFlatDataMembers(source, declaration);
        source.WriteLine("          })");
        source.WriteLine($"        , plain_helper_type(\"{declaration.QualifiedName}PlainHelper\", {{");
        EmitFlatDataMembers(source, declaration);
        source.WriteLines("""
                  })
                , flat_sample_access_info(RTIXCdrSampleAccessInfo_INITIALIZER)
                , flat_member_access_infos{}
                , helper_sample_access_info(RTIXCdrSampleAccessInfo_INITIALIZER)
                , helper_member_access_infos{}
                , flat_type_plugin(RTIXCdrTypePlugin_INITIALIZER)
                , helper_type_plugin(RTIXCdrTypePlugin_INITIALIZER)
            {
            """);
        source.WriteLine($"        type.extensibility_kind({declaration.ExtensibilityExpression});");
        source.WriteLine("        plain_helper_type.extensibility_kind(::dds::core::xtypes::ExtensibilityKind::FINAL);");
        source.WriteLine("        flat_sample_access_info.languageBinding = RTI_XCDR_TYPE_BINDING_FLAT_DATA_CPP_11_STL;");
        source.WriteLine("        flat_sample_access_info.memberAccessInfos = flat_member_access_infos;");
        source.WriteLine("        helper_sample_access_info.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;");
        source.WriteLine("        helper_sample_access_info.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;");
        source.WriteLine($"        helper_sample_access_info.typeSize[0] = sizeof({helperType});");
        source.WriteLine("        helper_sample_access_info.getMemberValuePointerFcn =");
        source.WriteLine($"            ::rti::topic::interpreter::get_aggregation_value_pointer<{helperType}>;");
        source.WriteLine("        helper_sample_access_info.memberAccessInfos = helper_member_access_infos;");
        source.WriteLine($"        {helperType} helper_sample{{}};");
        source.WriteLine("        const auto* helper_sample_address = reinterpret_cast<const char*>(&helper_sample);");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            var byteOffset = index * 4;
            source.WriteLine(FormattableString.Invariant(
                $"        flat_member_access_infos[{index}].bindingMemberValueOffset[0] = {byteOffset};"));
            source.WriteLine(FormattableString.Invariant(
                $"        flat_member_access_infos[{index}].bindingMemberValueOffset[1] = {byteOffset};"));
            source.WriteLine(FormattableString.Invariant(
                $"        flat_member_access_infos[{index}].bindingMemberValueOffset[2] = {byteOffset};"));
            source.WriteLine(FormattableString.Invariant(
                $"        flat_member_access_infos[{index}].bindingMemberValueOffset[3] = {byteOffset};"));
            source.WriteLine($"        const auto* helper_member_{index} = reinterpret_cast<const char*>(&helper_sample.{member.Name});");
            source.WriteLine(FormattableString.Invariant(
                $"        helper_member_access_infos[{index}].bindingMemberValueOffset[0] ="));
            source.WriteLine($"            static_cast<::RTIXCdrUnsignedLong>(helper_member_{index} - helper_sample_address);");
        }

        source.WriteLine("        for (int offset = 0; offset < 4; ++offset) {");
        source.WriteLine("            flat_sample_access_info.typeSize[offset] =");
        source.WriteLine($"                static_cast<::RTIXCdrUnsignedLong>({declaration.FlatDataOffsetType}::serialized_size(offset));");
        source.WriteLine("        }");
        source.WriteLine("        auto* flat_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(");
        source.WriteLine("            const_cast<::DDS_TypeCode*>(&type.native()));");
        source.WriteLine("        auto* helper_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(");
        source.WriteLine("            const_cast<::DDS_TypeCode*>(&plain_helper_type.native()));");
        source.WriteLine("        flat_type_code->_kind |= DDS_TK_FLAT_DATA_LANGUAGE_BINDING;");
        source.WriteLine("        flat_type_code->_sampleAccessInfo = &flat_sample_access_info;");
        source.WriteLine("        flat_type_code->_typePlugin = &flat_type_plugin;");
        source.WriteLine("        helper_type_code->_sampleAccessInfo = &helper_sample_access_info;");
        source.WriteLine("        helper_type_code->_typePlugin = &helper_type_plugin;");
        if (declaration.IsXcdr2Only)
        {
            source.WriteLine("        flat_type_code->_annotations._allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        }
        source.WriteLine("        helper_type_code->_annotations._allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        NativeTypeEmitter.EmitMemberTypeCodeDefaults(
            source, declaration, "        ", "flat_type_code->_members");
        NativeTypeEmitter.EmitMemberTypeCodeDefaults(
            source, declaration, "        ", "helper_type_code->_members");
        source.WriteLines("""
            }
        };
        """);
        source.BlankLine();
        source.WriteLine($"{contextName}& {contextName}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {contextName} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
        source.BlankLine();
        EmitFlatDataCallbacks(source, declaration, profile);
    }

    private static void EmitFlatDataMembers(SourceWriter source, StructPlan declaration)
    {
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
            var memberExpression =
                $"::dds::core::xtypes::Member(\"{member.Name}\", "
                + DynamicTypeEmitter.FormatTypeExpression(member.Type, 12, rtiVersion: "6.1.2") + ")";
            memberExpression += FormattableString.Invariant($".id({member.MemberId})");
            if (member.IsKey)
            {
                memberExpression += ".key(true)";
            }

            if (member.IsOptional)
            {
                memberExpression += ".optional(true)";
            }

            source.WriteLine($"              {memberExpression}{suffix}");
        }
    }

    private static string GetComplianceMask(RtiApiProfile profile) => profile.RtiVersion switch
    {
        "7.3.1" => "0x0000068C",
        "7.7.0" => "0x0000018C",
        _ => throw new InvalidOperationException(
            $"No FlatData compliance mask is qualified for RTI {profile.RtiVersion}."),
    };
}
