using System.Globalization;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Declares FlatData offsets, the plain helper value, and their RTI traits.</summary>
internal static class FlatDataHeaderEmitter
{
    /// <summary>Writes the offset classes and plain helper value for a FlatData sample.</summary>
    internal static void EmitFlatDataStruct(SourceWriter source, StructPlan declaration)
    {
        var constOffsetName = declaration.Name + "ConstOffset";
        var offsetName = declaration.Name + "Offset";
        var plainName = declaration.FlatDataPlainHelperName;
        var serializedSize = declaration.Members.Count * 4;

        source.WriteDoxygenBrief("Read-only offsets into the final FlatData sample.");
        RtiDocumentation.WriteApiSee(source,
            $"RTI Connext DDS {source.RtiVersion} FlatData API",
            "group__RTIFlatDataModule.html");
        source.WriteLine($"class NDDSUSERDllExport {constOffsetName}");
        source.WriteLine($"    : public ::rti::flat::FinalOffset< {constOffsetName} >");
        source.WriteLines("""
            {
            public:
                enum { required_alignment = 4 };
                static ::rti::flat::offset_t serialized_size(::rti::flat::offset_t initial_position)
                {
            """);
        source.WriteLine(
            "        static const ::rti::flat::offset_t sizes[] = { "
            + $"{string.Join(", ", Enumerable.Repeat(serializedSize.ToString(CultureInfo.InvariantCulture), 4))} }};");
        source.WriteLines("""
                    return sizes[initial_position % 4];
                }
            """);
        source.WriteDoxygenBrief("Creates a null offset.", indentationLevel: 1);
        source.WriteLine($"{constOffsetName}() = default;");
        source.WriteDoxygenBrief("Creates an internal offset at a sample position.", indentationLevel: 1);
        source.WriteLine($"    {constOffsetName}(");
        source.WriteLine("        const ::rti::flat::SampleBase *sample,");
        source.WriteLine("        ::rti::flat::offset_t offset)");
        source.WriteLine(
            $"        : ::rti::flat::FinalOffset< {constOffsetName} >(const_cast< ::rti::flat::SampleBase * >(sample), offset)");
        source.WriteLine("    {");
        source.WriteLine("    }");
        foreach (var member in declaration.Members)
        {
            source.WriteDoxygenBrief($"Reads IDL member {member.Name}.", indentationLevel: 1);
            source.WriteLine($"    {member.StorageType} {member.Name}() const;");
        }

        source.WriteLine("};");
        source.BlankLine();
        source.WriteLine($"class NDDSUSERDllExport {offsetName}");
        source.WriteLine($"    : public ::rti::flat::FinalOffset< {offsetName} >");
        source.WriteLine("{");
        source.WriteLine("public:");
        source.WriteLine($"    using ConstOffset = {constOffsetName};");
        source.WriteLines("""
                enum { required_alignment = 4 };
                static ::rti::flat::offset_t serialized_size(::rti::flat::offset_t initial_position)
                {
            """);
        source.WriteLine(
            "        static const ::rti::flat::offset_t sizes[] = { "
            + $"{string.Join(", ", Enumerable.Repeat(serializedSize.ToString(CultureInfo.InvariantCulture), 4))} }};");
        source.WriteLine("        return sizes[initial_position % 4];");
        source.WriteLine("    }");
        source.WriteLine($"    {offsetName}() = default;");
        source.WriteLine($"    {offsetName}(");
        source.WriteLine("        ::rti::flat::SampleBase *sample,");
        source.WriteLine("        ::rti::flat::offset_t offset)");
        source.WriteLine($"        : ::rti::flat::FinalOffset< {offsetName} >(sample, offset)");
        source.WriteLine("    {");
        source.WriteLine("    }");
        foreach (var member in declaration.Members)
        {
            source.WriteDoxygenBrief($"Reads IDL member {member.Name}.", indentationLevel: 1);
            source.WriteLine($"    {member.StorageType} {member.Name}() const;");
        }

        source.WriteLine("    // Modifiers.");
        foreach (var member in declaration.Members)
        {
            source.WriteDoxygenBrief($"Writes IDL member {member.Name}.", indentationLevel: 1);
            source.WriteLine($"    bool {member.Name}({member.StorageType} value);");
        }

        source.WriteLine("};");
        source.WriteLine($"using {declaration.Name} = ::rti::flat::Sample< {offsetName} >;");
        source.BlankLine();
        source.WriteDoxygenBrief($"Plain value helper for {declaration.QualifiedName}.");
        source.WriteLine($"struct NDDSUSERDllExport {plainName}");
        source.WriteLine("{");
        foreach (var member in declaration.Members)
        {
            source.WriteDoxygenBrief($"IDL member {member.Name}.", indentationLevel: 1);
            source.WriteLine($"    {member.StorageType} {member.Name} = {member.DefaultValue};");
        }

        source.WriteLine($"    {plainName}();");
        source.WriteLine($"    {plainName}(");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            source.WriteLine(
                $"        {member.ConstructorParameterType} {member.Name}{(index == declaration.Members.Count - 1 ? ")" : ",")}");
        }

        source.WriteLine(";");
        source.WriteLine("};");
        source.WriteLine("NDDSUSERDllExport bool operator==(const ");
        source.WriteLine($"{plainName}& left, const {plainName}& right);");
        source.WriteLine("NDDSUSERDllExport bool operator!=(const ");
        source.WriteLine($"{plainName}& left, const {plainName}& right);");
        source.WriteLine($"NDDSUSERDllExport void swap({plainName}& left, {plainName}& right) noexcept;");
        source.WriteLine($"NDDSUSERDllExport std::ostream& operator<<(std::ostream& output, const {plainName}& value);");
    }

    /// <summary>Writes topic support declarations for a FlatData sample and its plain helper.</summary>
    internal static void EmitFlatDataTopicTraits(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        TypeTraitsEmitter.EmitTopicTraits(source, declaration);
        EmitFlatDataPlainHelperTopicTraits(source, declaration);
    }

    private static void EmitFlatDataPlainHelperTopicTraits(SourceWriter source, StructPlan declaration)
    {
        var helperType = declaration.FlatDataPlainHelperType;
        source.BlankLine();
        source.WriteDoxygenBrief("Provides the DDS topic type name for the plain FlatData value helper.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "group__DDSTopicTraitsModule.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct topic_type_name<{helperType}>");
        source.WriteDoxygenBrief("Returns the helper type name used for DDS topics.");
        source.WriteDoxygenReturn("The fully qualified helper type name.");
        source.WriteLine("NDDSUSERDllExport static std::string value()");
        source.WriteLine("{");
        source.WriteLine($"    return \"{declaration.QualifiedName}PlainHelper\";");
        source.WriteLine("}");
        source.CloseBlock(";");

        source.BlankLine();
        source.WriteDoxygenBrief("Provides topic registration and CDR support for the plain helper.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "group__DDSTopicTraitsModule.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct topic_type_support<{helperType}>");
        source.WriteDoxygenBrief("Registers the plain helper with a participant.");
        source.WriteDoxygen("@param participant Participant to register with.");
        source.WriteDoxygen("@param name Topic type name.");
        source.WriteLines("""
            NDDSUSERDllExport static void register_type(
                dds::domain::DomainParticipant& participant,
                const std::string& name);
            """);
        source.BlankLine();
        source.WriteDoxygenBrief("Serializes a plain helper to a CDR buffer through the generic C-layout plugin.");
        source.WriteDoxygen("@param buffer Destination buffer.");
        source.WriteDoxygen("@param sample Helper value to serialize.");
        source.WriteDoxygenReturn("The destination buffer.");
        source.WriteLine("NDDSUSERDllExport static std::vector<char>& to_cdr_buffer(");
        source.WriteLine("    std::vector<char>& buffer,");
        source.WriteLine($"    const {helperType}& sample,");
        source.WriteLine("    dds::core::policy::DataRepresentationId representation =");
        source.WriteLine("        dds::core::policy::DataRepresentation::auto_id());");
        source.BlankLine();
        source.WriteDoxygenBrief("Deserializes a CDR buffer into a plain helper.");
        source.WriteDoxygen("@param sample Helper value to populate.");
        source.WriteDoxygen("@param buffer Encoded sample bytes.");
        source.WriteLine($"NDDSUSERDllExport static void from_cdr_buffer({helperType}& sample, const std::vector<char>& buffer);");
        source.BlankLine();
        source.WriteDoxygenBrief("Resets a helper value to its IDL defaults.");
        source.WriteLine($"NDDSUSERDllExport static void reset_sample({helperType}& sample);");
        source.BlankLine();
        source.WriteDoxygenBrief("Initializes the helper value for the requested capacity.");
        source.WriteDoxygen("@param sample Helper value to initialize.");
        source.WriteDoxygen("@param initial_sample_capacity Initial sample capacity.");
        source.WriteDoxygen("@param maximum_samples Maximum sample count.");
        source.WriteLine("NDDSUSERDllExport static void allocate_sample(");
        source.WriteLine($"    {helperType}& sample,");
        source.WriteLine("    int initial_sample_capacity,");
        source.WriteLine("    int maximum_samples);");
        source.BlankLine();
        source.WriteLine("static const rti::topic::TypePluginKind::type type_plugin_kind =");
        source.WriteLine("    rti::topic::TypePluginKind::STL;");
        source.CloseBlock(";");
    }

    /// <summary>Writes the RTI trait that marks a value as FlatData-only.</summary>
    internal static void EmitFlatDataOnlyTrait(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        source.OpenNamespaces("rti");
        source.OpenNamespaces("flat");
        source.OpenNamespaces("topic");
        source.WriteLine("template<>");
        source.WriteLine($"struct is_flat_data_type_only<{declaration.CppType}> : public ::dds::core::true_type {{}}; ");
        source.CloseNamespaces("topic");
        source.CloseNamespaces("flat");
        source.CloseNamespaces("rti");
    }

    /// <summary>Writes FlatData sample and offset traits.</summary>
    internal static void EmitFlatDataTraits(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        source.OpenNamespaces("rti");
        source.OpenNamespaces("flat");
        source.WriteLine("template<>");
        source.OpenBlock($"struct flat_type_traits<{declaration.CppType}>");
        source.WriteLine($"using plain_type = {declaration.FlatDataPlainHelperType};");
        source.WriteLine($"using offset = {declaration.FlatDataOffsetType};");
        source.CloseBlock(";");
        source.WriteLine("template<>");
        source.OpenBlock($"struct flat_type_traits<{declaration.FlatDataOffsetType}>");
        source.WriteLine($"using flat_type = {declaration.CppType};");
        source.WriteLine($"using plain_type = {declaration.FlatDataPlainHelperType};");
        source.CloseBlock(";");
        source.WriteLine("template<>");
        source.WriteLine($"struct flat_type_traits<{declaration.FlatDataConstOffsetType}>");
        source.WriteLine("    : flat_type_traits<");
        source.WriteLine($"{declaration.FlatDataOffsetType}> {{}}; ");
        source.CloseNamespaces("flat");
        source.CloseNamespaces("rti");
    }
}
