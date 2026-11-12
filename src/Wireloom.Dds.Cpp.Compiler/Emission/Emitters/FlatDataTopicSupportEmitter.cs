using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits FlatData topic operations and plain-helper CDR wrappers.</summary>
internal static class FlatDataTopicSupportEmitter
{
    /// <summary>Writes the typed operations for the FlatData sample and its plain helper value.</summary>
    internal static void Emit(SourceWriter source, StructPlan declaration, RtiApiProfile profile)
    {
        if (profile.RtiVersion == "6.1.2")
        {
            EmitRti612FlatDataTopicSupport(source, declaration);
            return;
        }

        EmitUnsupportedFlatDataTopicSupport(source, declaration);
        EmitFlatDataPlainHelperTopicSupport(source, declaration);
    }

    private static void EmitRti612FlatDataTopicSupport(SourceWriter source, StructPlan declaration)
    {
        var typeName = declaration.CppType;
        var helperType = declaration.FlatDataPlainHelperType;
        var pluginFactory = "wireloom_" + declaration.Name + "_flatdata";
        if (!string.IsNullOrEmpty(declaration.Namespace))
        {
            pluginFactory = "::" + declaration.Namespace.Replace(".", "::", StringComparison.Ordinal) + "::" + pluginFactory;
        }
        else
        {
            pluginFactory = "::" + pluginFactory;
        }

        source.WriteLine($"void topic_type_support<{typeName} >::register_type(");
        source.WriteLine("    dds::domain::DomainParticipant& participant,");
        source.WriteLine("    const std::string& name)");
        source.OpenBrace();
        source.WriteLine($"::rti::domain::register_type_plugin(participant, name, &{pluginFactory}_create_plugin, &{pluginFactory}_delete_plugin);");
        source.CloseBlock();
        source.BlankLine();

        PluginSymbols.WriteCdrBufferSignature(source, typeName);
        source.WriteLine("    std::vector<char>& buffer,");
        source.WriteLine($"    const {typeName}& sample,");
        source.WriteLine("    dds::core::policy::DataRepresentationId representation)");
        source.OpenBrace();
        source.WriteLines("""
            if (representation != dds::core::policy::DataRepresentation::auto_id()
                && representation != dds::core::policy::DataRepresentation::xcdr2())
                throw dds::core::InvalidArgumentError("FlatData CDR supports XCDR2 only.");
            const auto* bytes = sample.get_buffer();
            const auto encapsulation = (static_cast<unsigned int>(bytes[0]) << 8)
                | static_cast<unsigned int>(bytes[1]);
            if (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE)
                throw dds::core::InvalidArgumentError("FlatData sample does not contain an XCDR2 CDR buffer.");
            buffer.assign(reinterpret_cast<const char*>(bytes),
                reinterpret_cast<const char*>(bytes) + sample.buffer_size());
            return buffer;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName} >::from_cdr_buffer(");
        source.WriteLine($"    {typeName}& sample,");
        source.WriteLine("    const std::vector<char>& buffer)");
        source.OpenBrace();
        source.WriteLine($"if (buffer.size() != RTI_XCDR_ENCAPSULATION_SIZE + {declaration.FlatDataOffsetType}::serialized_size(0))");
        source.WriteLine("    throw dds::core::InvalidArgumentError(\"FlatData XCDR2 buffer has an unexpected size.\");");
        source.WriteLines("""
            const auto encapsulation = (static_cast<unsigned int>(static_cast<unsigned char>(buffer[0])) << 8)
                | static_cast<unsigned int>(static_cast<unsigned char>(buffer[1]));
            if (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE)
                throw dds::core::InvalidArgumentError("FlatData buffer is not XCDR2 CDR.");
            std::copy(buffer.begin(), buffer.end(), reinterpret_cast<char*>(sample.get_buffer()));
            """);
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"void topic_type_support<{typeName} >::reset_sample({typeName}&)");
        source.OpenBrace();
        source.WriteLines("""
            throw dds::core::InvalidArgumentError(
                "FlatData sample lifecycle is managed by the RTI FlatData API.");
            """);
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"void topic_type_support<{typeName} >::allocate_sample({typeName}&, int, int)");
        source.OpenBrace();
        source.WriteLines("""
            throw dds::core::InvalidArgumentError(
                "FlatData sample lifecycle is managed by the RTI FlatData API.");
            """);
        source.CloseBlock();

        source.BlankLine();
        source.WriteLine($"void topic_type_support<{helperType}>::register_type(");
        source.WriteLine("    dds::domain::DomainParticipant& participant,");
        source.WriteLine("    const std::string& name)");
        source.OpenBrace();
        source.WriteLine("::rti::domain::register_type_plugin(");
        source.WriteLine("    participant,");
        source.WriteLine("    name,");
        source.WriteLine($"    &::WireloomRti612TypePlugin<{helperType}>::create,");
        source.WriteLine($"    &::WireloomRti612TypePlugin<{helperType}>::destroy);");
        source.CloseBlock();
        EmitRti612PlainHelperCdr(source, declaration);
        source.BlankLine();
        source.WriteLine($"void topic_type_support<{helperType}>::reset_sample({helperType}& sample)");
        source.OpenBrace();
        source.WriteLine($"sample = {helperType}{{}};");
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"void topic_type_support<{helperType}>::allocate_sample({helperType}& sample, int, int)");
        source.OpenBrace();
        source.WriteLine($"sample = {helperType}{{}};");
        source.CloseBlock();
    }

    private static void EmitRti612PlainHelperCdr(SourceWriter source, StructPlan declaration)
    {
        var helperType = declaration.FlatDataPlainHelperType;
        var size = 4 + declaration.Members.Count * 4;
        source.BlankLine();
        PluginSymbols.WriteCdrBufferSignature(source, helperType);
        source.WriteLine("    std::vector<char>& buffer,");
        source.WriteLine($"    const {helperType}& sample,");
        source.WriteLine("    dds::core::policy::DataRepresentationId representation)");
        source.OpenBrace();
        source.WriteLines("""
            if (representation != dds::core::policy::DataRepresentation::auto_id()
                && representation != dds::core::policy::DataRepresentation::xcdr2())
                throw dds::core::InvalidArgumentError("FlatData helper CDR supports XCDR2 only.");
            """);
        source.WriteLine($"buffer.resize({size});");
        source.WriteLine("buffer[0] = static_cast<char>(RTI_XCDR_ENCAPSULATION_ID_CDR2_BE >> 8);");
        source.WriteLine("buffer[1] = static_cast<char>(RTI_XCDR_ENCAPSULATION_ID_CDR2_BE & 0xff);");
        source.WriteLines("""
            buffer[2] = 0;
            buffer[3] = 0;
            """);
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            source.WriteLine($"const auto value_{index} = static_cast<std::uint32_t>(sample.{declaration.Members[index].Name});");
            source.WriteLine($"buffer[{4 + index * 4}] = static_cast<char>((value_{index} >> 24) & 0xff);");
            source.WriteLine($"buffer[{5 + index * 4}] = static_cast<char>((value_{index} >> 16) & 0xff);");
            source.WriteLine($"buffer[{6 + index * 4}] = static_cast<char>((value_{index} >> 8) & 0xff);");
            source.WriteLine($"buffer[{7 + index * 4}] = static_cast<char>(value_{index} & 0xff);");
        }

        source.WriteLine("return buffer;");
        source.CloseBlock();
        source.BlankLine();
        source.WriteLine($"void topic_type_support<{helperType}>::from_cdr_buffer(");
        source.WriteLine($"    {helperType}& sample,");
        source.WriteLine("    const std::vector<char>& buffer)");
        source.OpenBrace();
        source.WriteLine($"if (buffer.size() != {size})");
        source.WriteLine("    throw dds::core::InvalidArgumentError(\"FlatData helper CDR has an unexpected size.\");");
        source.WriteLines("""
            const auto encapsulation = (static_cast<unsigned int>(static_cast<unsigned char>(buffer[0])) << 8)
                | static_cast<unsigned int>(static_cast<unsigned char>(buffer[1]));
            if (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE)
                throw dds::core::InvalidArgumentError("FlatData helper buffer is not XCDR2 CDR.");
            const bool little_endian = encapsulation == RTI_XCDR_ENCAPSULATION_ID_CDR2_LE;
            """);
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var offset = 4 + index * 4;
            source.WriteLine($"std::uint32_t value_{index};");
            source.WriteLine($"if (little_endian) value_{index} = static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset}])) | (static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset + 1}])) << 8) | (static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset + 2}])) << 16) | (static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset + 3}])) << 24);");
            source.WriteLine($"else value_{index} = (static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset}])) << 24) | (static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset + 1}])) << 16) | (static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset + 2}])) << 8) | static_cast<std::uint32_t>(static_cast<unsigned char>(buffer[{offset + 3}]));");
            source.WriteLine($"std::memcpy(&sample.{declaration.Members[index].Name}, &value_{index}, sizeof(value_{index}));");
        }

        source.CloseBlock();
    }

    private static void EmitUnsupportedFlatDataTopicSupport(
        SourceWriter source,
        StructPlan declaration)
    {
        var typeName = declaration.CppType;
        var offsetType = declaration.FlatDataOffsetType;
        var prefix = "wireloom_" + declaration.Name + "_flatdata";
        source.WriteLine($"void topic_type_support<{typeName} >::register_type(");
        source.WriteLine("    dds::domain::DomainParticipant& participant,");
        source.WriteLine("    const std::string& name)");
        source.OpenBrace();
        var pluginFactory = string.IsNullOrEmpty(declaration.Namespace)
            ? "::" + prefix
            : "::" + declaration.Namespace.Replace(".", "::", StringComparison.Ordinal) + "::" + prefix;
        source.WriteLine($"::rti::domain::register_type_plugin(participant, name, &{pluginFactory}_create_plugin, &{pluginFactory}"
            + "_delete_plugin);");
        source.CloseBlock();
        source.BlankLine();

        PluginSymbols.WriteCdrBufferSignature(source, typeName);
        source.WriteLine("    std::vector<char>& buffer,");
        source.WriteLine($"    const {typeName}& sample,");
        source.WriteLine("    dds::core::policy::DataRepresentationId representation)");
        source.OpenBrace();
        source.WriteLines("""
            if (representation != dds::core::policy::DataRepresentation::auto_id()
                && representation != dds::core::policy::DataRepresentation::xcdr2())
                throw dds::core::InvalidArgumentError("FlatData CDR supports XCDR2 only.");
            """);
        source.WriteLine($"const auto expected_size = RTI_XCDR_ENCAPSULATION_SIZE + {offsetType}::serialized_size(0);");
        source.WriteLines("""
            const auto* bytes = sample.get_buffer();
            const auto encapsulation = (static_cast<unsigned int>(bytes[0]) << 8)
                | static_cast<unsigned int>(bytes[1]);
            if (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE)
                throw dds::core::InvalidArgumentError("FlatData sample does not contain an XCDR2 CDR buffer.");
            buffer.assign(reinterpret_cast<const char*>(bytes),
                reinterpret_cast<const char*>(bytes) + expected_size);
            return buffer;
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName} >::from_cdr_buffer(");
        source.WriteLine($"    {typeName}& sample,");
        source.WriteLine("    const std::vector<char>& buffer)");
        source.OpenBrace();
        source.WriteLine($"const auto expected_size = RTI_XCDR_ENCAPSULATION_SIZE + {offsetType}::serialized_size(0);");
        source.WriteLines("""
            if (buffer.size() != expected_size)
                throw dds::core::InvalidArgumentError("FlatData XCDR2 buffer has an unexpected size.");
            const auto encapsulation = (static_cast<unsigned int>(static_cast<unsigned char>(buffer[0])) << 8)
                | static_cast<unsigned int>(static_cast<unsigned char>(buffer[1]));
            if (encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_BE
                && encapsulation != RTI_XCDR_ENCAPSULATION_ID_CDR2_LE)
                throw dds::core::InvalidArgumentError("FlatData buffer is not XCDR2 CDR.");
            std::copy(buffer.begin(), buffer.end(), reinterpret_cast<char*>(sample.get_buffer()));
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName} >::reset_sample(");
        source.WriteLine($"    {typeName}& sample)");
        source.OpenBrace();
        source.WriteLines("""
            (void) sample;
            throw dds::core::InvalidArgumentError(
                "FlatData sample lifecycle is managed by the RTI FlatData API.");
            """);
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{typeName} >::allocate_sample(");
        source.WriteLine($"    {typeName}& sample,");
        source.WriteLine("    int initial_sample_capacity,");
        source.WriteLine("    int maximum_samples)");
        source.OpenBrace();
        source.WriteLines("""
            (void) sample;
            (void) initial_sample_capacity;
            (void) maximum_samples;
            throw dds::core::InvalidArgumentError(
                "FlatData sample lifecycle is managed by the RTI FlatData API.");
            """);
        source.CloseBlock();
    }

    private static void EmitFlatDataPlainHelperTopicSupport(
        SourceWriter source,
        StructPlan declaration)
    {
        var contextName = PluginSymbols.ContextFunctionName(declaration);
        var helperType = declaration.FlatDataPlainHelperType;
        source.BlankLine();
        source.WriteLine($"void topic_type_support<{helperType}>::register_type(");
        source.WriteLines("""
                dds::domain::DomainParticipant& participant,
                const std::string& name)
            {
                wireloom_initialize_type_plugins();
            """);
        PluginSymbols.WriteContextMemberCall(source, contextName, "plain_helper_type_plugin->register_type(", 4);
        source.WriteLines("""
                    participant,
                    name.c_str());
            }
            """);
        source.BlankLine();

        source.WriteComment("Adapt the plain helper to RTI's generic CDR wrapper interface.");
        PluginSymbols.WriteCdrBufferSignature(source, helperType);
        source.WriteLine("    std::vector<char>& buffer,");
        source.WriteLine($"    const {helperType}& sample,");
        source.WriteLines("""
                dds::core::policy::DataRepresentationId representation)
            {
                wireloom_initialize_type_plugins();
            """);
        PluginSymbols.WriteContextMemberCall(source, contextName, "plain_helper_type_plugin->serialize_to_cdr_buffer(", 4);
        source.WriteLines("""
                    buffer,
                    reinterpret_cast<const ::rti::topic::cdr::CSampleWrapper&>(sample),
                    static_cast<DDS_DataRepresentationId_t>(representation));
                return buffer;
            }
            """);
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{helperType}>::from_cdr_buffer(");
        source.WriteLine($"    {helperType}& sample,");
        source.WriteLines("""
                const std::vector<char>& buffer)
            {
                wireloom_initialize_type_plugins();
            """);
        PluginSymbols.WriteContextMemberCall(source, contextName, "plain_helper_type_plugin->deserialize_from_cdr_buffer(", 4);
        source.WriteLines("""
                    reinterpret_cast<::rti::topic::cdr::CSampleWrapper&>(sample),
                    buffer);
            }
            """);
        source.BlankLine();

        source.WriteComment("The plain helper owns only inline scalar values.");
        source.WriteLine($"void topic_type_support<{helperType}>::reset_sample(");
        source.WriteLine($"    {helperType}& sample)");
        source.OpenBrace();
        source.WriteLine($"sample = {helperType}{{}};");
        source.CloseBlock();
        source.BlankLine();

        source.WriteLine($"void topic_type_support<{helperType}>::allocate_sample(");
        source.WriteLine($"    {helperType}& sample,");
        source.WriteLine("    int initial_sample_capacity,");
        source.WriteLine("    int maximum_samples)");
        source.OpenBrace();
        source.WriteLines("""
            (void) sample;
            (void) initial_sample_capacity;
            (void) maximum_samples;
            """);
        source.CloseBlock();
    }
}
