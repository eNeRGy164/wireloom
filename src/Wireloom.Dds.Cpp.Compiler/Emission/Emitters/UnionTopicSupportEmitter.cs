using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits union registration, sample lifecycle, and CDR operations.</summary>
internal static class UnionTopicSupportEmitter
{
    /// <summary>Writes the typed topic operations for a planned union.</summary>
    internal static void Emit(SourceWriter source, UnionPlan declaration, bool isRti612 = false)
    {
        var contextName = PluginSymbols.ContextFunctionName(declaration);
        var typeName = declaration.CppType;
        source.WriteLine($"void topic_type_support<{typeName}>::register_type(");
        source.WriteLines("""
                dds::domain::DomainParticipant& participant,
                const std::string& name)
            {
            """);
        if (isRti612)
        {
            source.WriteLine($"        (void) ::rti::topic::dynamic_type<{typeName}>::get();");
            Rti612PluginEmitter.WriteRegistration(source, declaration);
        }
        else
        {
            source.WriteLine("        wireloom_initialize_type_plugins();");
        }

        if (!isRti612 && UnionPluginEmitter.RequiresStlInterpreterPluginBridge(declaration))
        {
            PluginSymbols.WriteInterpreterRegistration(source, declaration.Namespace,
                PluginSymbols.StlUnionPluginPrefix(declaration));
        }
        else if (!isRti612)
        {
            PluginSymbols.WriteContextMemberCall(source, contextName, "type_plugin->register_type(", 4);
            source.WriteLine("        participant,");
            source.WriteLine("        name.c_str());");
        }
        source.WriteLine("}");
        source.BlankLine();

        PluginSymbols.WriteCdrBufferSignature(source, typeName);
        source.WriteLine("    std::vector<char>& buffer,");
        source.WriteLine($"    const {typeName}& sample,");
        source.WriteLine("    dds::core::policy::DataRepresentationId representation)");
        EmitUnionCdrSerialization(source, declaration, isRti612);
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName}>::from_cdr_buffer(");
        source.WriteLine($"    {typeName}& sample,");
        source.WriteLine("    const std::vector<char>& buffer)");
        EmitUnionCdrDeserialization(source, declaration, isRti612);
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName}>::reset_sample(");
        source.WriteLine($"    {typeName}& sample)");
        source.OpenBrace();
        WriteTypeInitialization(source, typeName, isRti612);
        source.WriteLine($"sample = {typeName}{{}};");
        var resetCollectionBranches = declaration.Branches.Where(branch => branch.Type.ContainsCollection).ToArray();
        if (resetCollectionBranches.Length > 0)
        {
            source.WriteLine("const auto original_discriminator = sample._d();");
            foreach (var branch in resetCollectionBranches)
            {
                source.WriteLine($"sample.{branch.Name}({{}});");
                source.WriteLine($"::rti::topic::reset_sample(sample.{branch.Name}());");
            }

            source.WriteLine("sample._d(original_discriminator);");
        }

        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName}>::allocate_sample(");
        source.WriteLine($"    {typeName}& sample,");
        source.WriteLine("    int initial_sample_capacity,");
        source.WriteLine("    int maximum_samples)");
        source.OpenBrace();
        WriteTypeInitialization(source, typeName, isRti612);
        var allocationCollectionBranches = declaration.Branches.Where(branch => branch.Type.ContainsCollection).ToArray();
        if (allocationCollectionBranches.Length > 0)
        {
            source.WriteLine($"sample = {typeName}{{}};");
            source.WriteLine("const auto original_discriminator = sample._d();");
            foreach (var branch in allocationCollectionBranches)
            {
                source.WriteLine($"sample.{branch.Name}({{}});");
                source.WriteLine($"::rti::topic::allocate_sample(sample.{branch.Name}(), initial_sample_capacity, maximum_samples);");
            }

            source.WriteLine("sample._d(original_discriminator);");
        }
        else
        {
            source.WriteLine("(void) initial_sample_capacity;");
            source.WriteLine("(void) maximum_samples;");
            source.WriteLine($"sample = {typeName}{{}};");
        }

        source.CloseBlock();
        source.BlankLine();
    }

    private static void EmitUnionCdrSerialization(SourceWriter source, UnionPlan declaration, bool isRti612)
    {
        var typeName = declaration.CppType;
        source.OpenBrace();
        source.WriteLines("""
            RTIEncapsulationId encapsulation_id = RTI_CDR_ENCAPSULATION_ID_INVALID;
            RTICdrStream cdr_stream;
            PRESTypePluginDefaultEndpointData endpoint_data;
            PRESTypePluginDefaultParticipantData participant_data;
            RTIXCdrTypePluginProgramContext program_context = RTIXCdrTypePluginProgramContext_INTIALIZER;
            PRESTypePlugin plugin = PRES_TYPEPLUGIN_DEFAULT;
            RTIBool serialized;
            unsigned int length = 0;
            """);
        WriteTypeInitialization(source, typeName, isRti612);
        source.WriteLines("""
            RTIOsapiMemory_zero(&endpoint_data, sizeof(endpoint_data));
            endpoint_data.programContext = program_context;
            endpoint_data._participantData = &participant_data;
            endpoint_data.typePlugin = &plugin;
            endpoint_data.programContext.endpointPluginData = &endpoint_data;
            const auto& dynamic_type =
            """);
        source.WriteLine($"    ::rti::topic::dynamic_type<{typeName}>::get();");
        source.WriteLines("""
            plugin.typeCode = reinterpret_cast<RTICdrTypeCode*>(
                const_cast<DDS_TypeCode*>(&dynamic_type.native()));
            participant_data.programs =
                ::rti::topic::interpreter::get_cdr_serialization_programs<
            """);
        source.WriteLine($"        {typeName}, true, true, true>();");
        source.WriteLines("""
            encapsulation_id = DDS_TypeCode_get_native_encapsulation(
                reinterpret_cast<DDS_TypeCode*>(plugin.typeCode),
                static_cast<DDS_DataRepresentationId_t>(representation));
            if (encapsulation_id == RTI_CDR_ENCAPSULATION_ID_INVALID)
                throw dds::core::InvalidArgumentError("Unsupported CDR representation for this union");
            endpoint_data._maxSizeSerializedSample = RTIXCdrLong_MAX;
            length = PRESTypePlugin_interpretedGetSerializedSampleSize(
                reinterpret_cast<PRESTypePluginEndpointData>(&endpoint_data),
                RTI_TRUE,
                encapsulation_id,
                0,
                &sample);
            if (length == 0)
                throw dds::core::Error("RTI could not calculate the union CDR sample size");
            buffer.resize(length);
            RTICdrStream_init(&cdr_stream);
            RTICdrStream_set(&cdr_stream, buffer.data(), length);
            serialized = PRESTypePlugin_interpretedSerialize(
                reinterpret_cast<PRESTypePluginEndpointData>(&endpoint_data),
                &sample,
                &cdr_stream,
                RTI_TRUE,
                encapsulation_id,
                RTI_TRUE,
                NULL);
            if (!serialized)
                throw dds::core::Error("RTI could not serialize the union CDR sample");
            buffer.resize(static_cast<std::size_t>(RTICdrStream_getCurrentPositionOffset(&cdr_stream)));
            return buffer;
            """);
        source.CloseBlock();
    }

    private static void EmitUnionCdrDeserialization(SourceWriter source, UnionPlan declaration, bool isRti612)
    {
        var typeName = declaration.CppType;
        source.OpenBrace();
        source.WriteLines("""
            RTICdrStream cdr_stream;
            PRESTypePluginDefaultParticipantData participant_data;
            RTIXCdrTypePluginProgramContext program_context = RTIXCdrTypePluginProgramContext_INTIALIZER;
            PRESTypePlugin plugin = PRES_TYPEPLUGIN_DEFAULT;
            PRESTypePluginDefaultEndpointData endpoint_data;
            if (buffer.empty())
                throw dds::core::InvalidArgumentError("Cannot deserialize an empty union CDR buffer");
            """);
        WriteTypeInitialization(source, typeName, isRti612);
        source.WriteLines("""
            RTICdrStream_init(&cdr_stream);
            RTICdrStream_set(&cdr_stream, const_cast<char*>(buffer.data()), static_cast<unsigned int>(buffer.size()));
            endpoint_data.programContext = program_context;
            endpoint_data._participantData = &participant_data;
            endpoint_data.typePlugin = &plugin;
            endpoint_data.programContext.endpointPluginData = &endpoint_data;
            const auto& dynamic_type =
            """);
        source.WriteLine($"    ::rti::topic::dynamic_type<{typeName}>::get();");
        source.WriteLines("""
            plugin.typeCode = reinterpret_cast<RTICdrTypeCode*>(
                const_cast<DDS_TypeCode*>(&dynamic_type.native()));
            participant_data.programs =
                ::rti::topic::interpreter::get_cdr_serialization_programs<
            """);
        source.WriteLine($"        {typeName}, true, true, true>();");
        if (!isRti612)
        {
            source.WriteLine("RTIXCdrSampleAssignabilityProperty_setFromGlobalComplianceMask(");
            source.WriteLine("    &endpoint_data._assignabilityProperty);");
        }

        source.WriteLine($"topic_type_support<{typeName}>::reset_sample(sample);");
        source.WriteLines("""
            const RTIBool deserialized = PRESTypePlugin_interpretedDeserialize(
                reinterpret_cast<PRESTypePluginEndpointData>(&endpoint_data),
                &sample,
                &cdr_stream,
                RTI_TRUE,
                RTI_TRUE,
                NULL);
            if (!deserialized)
                throw dds::core::Error("RTI could not deserialize the union CDR sample");
            """);
        source.CloseBlock();
    }

    private static void WriteTypeInitialization(SourceWriter source, string typeName, bool isRti612)
    {
        source.WriteLine(isRti612
            ? $"(void) ::rti::topic::dynamic_type<{typeName}>::get();"
            : "wireloom_initialize_type_plugins();");
    }
}
