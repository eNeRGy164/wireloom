using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits the typed RTI operations for ordinary struct values.</summary>
internal static class TopicSupportEmitter
{
    /// <summary>Writes the typed topic operations for a planned struct or FlatData sample.</summary>
    internal static void EmitTopicSupport(SourceWriter source, StructPlan declaration, GenerationPlan plan)
    {
        if (declaration.IsFlatData)
        {
            FlatDataTopicSupportEmitter.Emit(source, declaration, plan.Profile);
            return;
        }

        new StructTopicOperations(source, declaration, plan).Emit();
    }

    private static void EmitInterpretedCdrSerialization(SourceWriter source, StructPlan declaration)
    {
        var typeName = declaration.CppType;
        source.WriteLines("""
                struct RTICdrStream stream;
                struct PRESTypePluginDefaultEndpointData endpoint{};
                struct PRESTypePluginDefaultParticipantData participant{};
                struct PRESTypePlugin plugin = PRES_TYPEPLUGIN_DEFAULT;
                struct RTIXCdrTypePluginProgramContext program_context =
                    RTIXCdrTypePluginProgramContext_INTIALIZER;
                RTICdrStream_init(&stream);
                plugin.typeCode = reinterpret_cast<RTICdrTypeCode*>(const_cast<DDS_TypeCode*>(
                    reinterpret_cast<const DDS_TypeCode*>(&
            """);
        source.WriteLine($"            ::rti::topic::dynamic_type<{typeName}>::get().native())));");
        source.WriteLine("    participant.programs =");
        source.WriteLine("        ::rti::topic::interpreter::get_cdr_serialization_programs<");
        source.WriteLine($"            {typeName}, true, true, true>();");
        source.WriteLines("""
                endpoint.programContext = program_context;
                endpoint._participantData = &participant;
                endpoint.typePlugin = &plugin;
                endpoint.programContext.endpointPluginData = &endpoint;
                const auto encapsulation = DDS_TypeCode_get_native_encapsulation(
                    reinterpret_cast<DDS_TypeCode*>(plugin.typeCode),
                    static_cast<DDS_DataRepresentationId_t>(representation));
                if (encapsulation == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                    throw dds::core::InvalidArgumentError("The requested CDR representation is unsupported.");
                }
                const auto size = PRESTypePlugin_interpretedGetSerializedSampleSize(
                    &endpoint, RTI_TRUE, encapsulation, 0, &sample);
                if (size == 0) {
                    throw dds::core::InvalidArgumentError("RTI could not size the sequence-bearing sample.");
                }
                buffer.resize(size);
                RTICdrStream_set(&stream, buffer.data(), size);
                if (!PRESTypePlugin_interpretedSerialize(
                        &endpoint, &sample, &stream, RTI_TRUE, encapsulation, RTI_TRUE, nullptr)) {
                    throw dds::core::InvalidArgumentError("RTI failed to serialize the sequence-bearing sample.");
                }
                buffer.resize(RTICdrStream_getCurrentPositionOffset(&stream));
                return buffer;
            """);
    }

    private static void EmitInterpretedCdrDeserialization(SourceWriter source, StructPlan declaration, bool isRti612)
    {
        var typeName = declaration.CppType;
        source.WriteLines("""
                if (buffer.empty()) {
                    throw dds::core::InvalidArgumentError("CDR buffer is empty.");
                }
                struct RTICdrStream stream;
                struct PRESTypePluginDefaultEndpointData endpoint{};
                struct PRESTypePluginDefaultParticipantData participant{};
                struct PRESTypePlugin plugin = PRES_TYPEPLUGIN_DEFAULT;
                struct RTIXCdrTypePluginProgramContext program_context =
                    RTIXCdrTypePluginProgramContext_INTIALIZER;
                RTICdrStream_init(&stream);
                RTICdrStream_set(&stream, const_cast<char*>(buffer.data()), static_cast<unsigned int>(buffer.size()));
                plugin.typeCode = reinterpret_cast<RTICdrTypeCode*>(const_cast<DDS_TypeCode*>(
                    reinterpret_cast<const DDS_TypeCode*>(&
            """);
        source.WriteLine($"            ::rti::topic::dynamic_type<{typeName}>::get().native())));");
        source.WriteLine("    participant.programs =");
        source.WriteLine("        ::rti::topic::interpreter::get_cdr_serialization_programs<");
        source.WriteLine($"            {typeName}, true, true, true>();");
        source.WriteLines("""
                endpoint.programContext = program_context;
                endpoint._participantData = &participant;
                endpoint.typePlugin = &plugin;
                endpoint.programContext.endpointPluginData = &endpoint;
            """);
        if (!isRti612)
        {
            source.WriteLines("""
                RTIXCdrSampleAssignabilityProperty_setFromGlobalComplianceMask(
                    &endpoint._assignabilityProperty);
                """);
        }
        source.WriteLine("    topic_type_support<" + typeName + ">::reset_sample(sample);");
        source.WriteLines("""
                if (!PRESTypePlugin_interpretedDeserialize(
                        &endpoint, &sample, &stream, RTI_TRUE, RTI_TRUE, nullptr)) {
                    throw dds::core::InvalidArgumentError("RTI failed to deserialize the sequence-bearing sample.");
                }
            """);
    }

    private sealed class StructTopicOperations(SourceWriter source, StructPlan declaration, GenerationPlan plan)
    {
        private readonly string contextName = PluginSymbols.ContextFunctionName(declaration);
        private readonly string typeName = declaration.CppType;
        private readonly bool stagedInitialization = plan.Initialization.RequiresStagedInitialization;
        private readonly bool hasExternalMembers = declaration.Members.Any(member => member.IsExternal);
        private readonly bool hasSequenceInGraph = RtiSampleAccess.HasSequenceInValueGraph(plan, declaration);
        private readonly bool hasStringInGraph = RtiSampleAccess.HasStringInValueGraph(plan, declaration);
        private readonly bool hasDirectStringBridge = RtiSampleAccess.UsesDirectStringPluginBridge(plan, declaration);

        /// <summary>Writes topic-only operations followed by the sample lifecycle methods.</summary>
        internal void Emit()
        {
            if (declaration.IsTopic)
            {
                EmitRegistration();
                EmitCdrSerialization();
                EmitCdrDeserialization();
            }

            EmitSampleLifecycle();
        }

        private void EmitRegistration()
        {
            source.WriteLine($"void topic_type_support<{typeName}>::register_type(");
            source.WriteLines("""
                    dds::domain::DomainParticipant& participant,
                    const std::string& name)
                {
                """);
            if (stagedInitialization)
            {
                source.WriteLine("    wireloom_initialize_type_plugins();");
            }
            else if (hasSequenceInGraph || hasStringInGraph)
            {
                source.WriteLine($"    (void) ::rti::topic::dynamic_type<{typeName}>::get();");
            }

            if (declaration.HasSelfRecursiveSequence)
            {
                source.WriteLines("""
                        (void) participant;
                        (void) name;
                        throw dds::core::InvalidArgumentError(
                            "RTI Connext DDS 7.7.0.0 does not support TypeObjectV2 for "
                            "Strongly Connected Components (recursive types).");
                    """);
            }
            else if (plan.Profile.RtiVersion == "6.1.2")
            {
                Rti612PluginEmitter.WriteRegistration(source, declaration);
            }
            else if (plan.Profile.RtiVersion == "7.3.1"
                     && RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration))
            {
                PluginSymbols.WriteInterpreterRegistration(source, declaration.Namespace,
                    PluginSymbols.StlSequencePluginPrefix(declaration));
            }
            else if (hasSequenceInGraph && RtiSampleAccess.SupportsSequenceInValueGraph(plan, declaration))
            {
                PluginSymbols.WriteInterpreterRegistration(source, declaration.Namespace,
                    PluginSymbols.StlSequencePluginPrefix(declaration));
            }
            else if (hasSequenceInGraph)
            {
                source.WriteLine("    throw dds::core::InvalidArgumentError(");
                source.WriteLine("        \"Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.\");");
            }
            else if (hasDirectStringBridge)
            {
                PluginSymbols.WriteInterpreterRegistration(source, declaration.Namespace,
                    PluginSymbols.StlStringPluginPrefix(declaration));
            }
            else if (RtiSampleAccess.IsReferencedByStlInterpreterGraph(plan, declaration))
            {
                PluginSymbols.WriteInterpreterRegistration(source, declaration.Namespace,
                    PluginSymbols.StlSequencePluginPrefix(declaration));
            }
            else if (RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration))
            {
                source.WriteLine("    throw dds::core::InvalidArgumentError(");
                source.WriteLine("        \"This inherited aggregate value graph is outside the RTI C-layout plugin bridge.\");");
            }
            else if (hasStringInGraph)
            {
                source.WriteLine("    throw dds::core::InvalidArgumentError(");
                source.WriteLine("        \"Wireloom C++ does not have a qualified interpreter bridge for this string-bearing aggregate shape.\");");
            }
            else
            {
                PluginSymbols.WriteContextMemberCall(source, contextName, "type_plugin->register_type(", 4);
                source.WriteLine("        participant,");
                source.WriteLine("        name.c_str());");
            }
            source.WriteLine("}");
            source.BlankLine();
        }

        private void EmitCdrSerialization()
        {
            source.WriteComment("Adapt the standard-layout value to RTI's generic CDR wrapper interface.");
            PluginSymbols.WriteCdrBufferSignature(source, typeName);
            source.WriteLine("    std::vector<char>& buffer,");
            source.WriteLine($"    const {typeName}& sample,");
            source.WriteLine("    dds::core::policy::DataRepresentationId representation)");
            source.WriteLine("{");
            if (hasExternalMembers)
            {
                source.WriteLines("""
                        (void) buffer;
                        (void) sample;
                        (void) representation;
                        throw dds::core::InvalidArgumentError(
                            "CDR serialization for IDL @external members requires an RTI external-reference callback adapter.");
                    """);
            }
            else if (declaration.IsXcdr2Only)
            {
                source.WriteLines("""
                        if (representation != dds::core::policy::DataRepresentation::auto_id() &&
                            representation != dds::core::policy::DataRepresentation::xcdr2()) {
                            throw dds::core::InvalidArgumentError("This IDL type permits XCDR2 only.");
                        }
                    """);
            }

            if (!hasExternalMembers && stagedInitialization)
            {
                source.WriteLine("    wireloom_initialize_type_plugins();");
            }

            if (!hasExternalMembers)
            {
                if (UsesInterpretedCdr())
                {
                    EmitInterpretedCdrSerialization(source, declaration);
                }
                else if (hasSequenceInGraph)
                {
                    source.WriteLine("    throw dds::core::InvalidArgumentError(");
                    source.WriteLine(
                        "        \"Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.\");");
                }
                else if (RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration))
                {
                    source.WriteLine("    throw dds::core::InvalidArgumentError(");
                    source.WriteLine(
                        "        \"CDR for inherited aggregate value graphs is outside the RTI C-layout plugin bridge.\");");
                }
                else if (hasStringInGraph)
                {
                    source.WriteLine("    throw dds::core::InvalidArgumentError(");
                    source.WriteLine(
                        "        \"Wireloom C++ does not have a qualified CDR bridge for this string-bearing aggregate shape.\");");
                }
                else
                {
                    PluginSymbols.WriteContextMemberCall(source, contextName, "type_plugin->serialize_to_cdr_buffer(", 4);
                    source.WriteLines("""
                                buffer,
                                reinterpret_cast<const ::rti::topic::cdr::CSampleWrapper&>(sample),
                                static_cast<DDS_DataRepresentationId_t>(representation));
                            return buffer;
                        """);
                }
            }
            source.WriteLine("}");
            source.BlankLine();
        }

        private void EmitCdrDeserialization()
        {
            source.WriteLine($"void topic_type_support<{typeName}>::from_cdr_buffer(");
            source.WriteLine($"    {typeName}& sample,");
            source.WriteLine("    const std::vector<char>& buffer)");
            source.WriteLine("{");
            if (hasExternalMembers)
            {
                source.WriteLines("""
                        (void) sample;
                        (void) buffer;
                        throw dds::core::InvalidArgumentError(
                            "CDR deserialization for IDL @external members requires an RTI external-reference callback adapter.");
                    """);
            }

            if (!hasExternalMembers)
            {
                if (stagedInitialization)
                {
                    source.WriteLine("    wireloom_initialize_type_plugins();");
                }

                if (UsesInterpretedCdr())
                {
                    EmitInterpretedCdrDeserialization(source, declaration, plan.Profile.RtiVersion == "6.1.2");
                }
                else if (hasSequenceInGraph)
                {
                    source.WriteLine("    throw dds::core::InvalidArgumentError(");
                    source.WriteLine(
                        "        \"Wireloom C++ does not have a qualified interpreter bridge for this sequence-bearing aggregate shape.\");");
                }
                else if (RtiSampleAccess.HasInheritedAggregateStorage(plan, declaration))
                {
                    source.WriteLine("    throw dds::core::InvalidArgumentError(");
                    source.WriteLine(
                        "        \"CDR for inherited aggregate value graphs is outside the RTI C-layout plugin bridge.\");");
                }
                else if (hasStringInGraph)
                {
                    source.WriteLine("    throw dds::core::InvalidArgumentError(");
                    source.WriteLine(
                        "        \"Wireloom C++ does not have a qualified CDR bridge for this string-bearing aggregate shape.\");");
                }
                else
                {
                    PluginSymbols.WriteContextMemberCall(source, contextName, "type_plugin->deserialize_from_cdr_buffer(", 4);
                    source.WriteLine("        reinterpret_cast<::rti::topic::cdr::CSampleWrapper&>(sample),");
                    source.WriteLine("        buffer);");
                }
            }
            source.WriteLine("}");
            source.BlankLine();
        }

        private bool UsesInterpretedCdr() =>
            plan.Profile.RtiVersion == "6.1.2"
            || plan.Profile.RtiVersion == "7.3.1"
                && RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration)
            || hasSequenceInGraph && RtiSampleAccess.SupportsSequenceInValueGraph(plan, declaration)
            || hasDirectStringBridge
            || RtiSampleAccess.IsReferencedByStlInterpreterGraph(plan, declaration);

        private void EmitSampleLifecycle()
        {
            source.WriteComment("Reset the inline scalar values to their IDL defaults.");
            source.WriteLine($"void topic_type_support<{typeName}>::reset_sample(");
            source.WriteLine($"    {typeName}& sample)");
            source.OpenBrace();
            if (stagedInitialization && declaration.IsTopic)
            {
                source.WriteLine("wireloom_initialize_type_plugins();");
            }

            source.WriteLine($"sample = {typeName}{{}};");
            foreach (var member in declaration.Members
                         .Where(member => member.Type.ContainsCollection && !member.IsOptional))
            {
                source.WriteLine($"::rti::topic::reset_sample(sample.{member.Name});");
            }

            source.CloseBlock();
            source.BlankLine();

            source.WriteComment("This value type owns no separately allocated sample storage.");
            source.WriteLine($"void topic_type_support<{typeName}>::allocate_sample(");
            source.WriteLine($"    {typeName}& sample,");
            source.WriteLine("    int max_seq_length,");
            source.WriteLine("    int max_str_length)");
            source.OpenBrace();
            if (stagedInitialization && declaration.IsTopic)
            {
                source.WriteLine("wireloom_initialize_type_plugins();");
            }

            source.WriteLine($"sample = {typeName}{{}};");
            foreach (var member in declaration.Members
                         .Where(member => member.Type.ContainsCollection && !member.IsOptional))
            {
                source.WriteLine($"::rti::topic::allocate_sample(sample.{member.Name}, max_seq_length, max_str_length);");
            }

            source.CloseBlock();
            source.BlankLine();
        }
    }
}
