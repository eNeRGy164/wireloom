using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Declares the public DDS topic support and RTI DynamicType traits.</summary>
internal static class TypeTraitsEmitter
{
    /// <summary>Writes the DDS topic name and operations for the planned value.</summary>
    internal static void EmitTopicTraits(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief("Provides the DDS topic type name for this generated value type.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "group__DDSTopicTraitsModule.html");
        source.WriteDoxygenCode(
        [
            $"auto type_name = dds::topic::topic_type_name<{declaration.CppType}>::value();",
        ]);
        source.WriteLine("template<>");
        source.OpenBlock($"struct topic_type_name<{declaration.CppType}>");
        source.WriteDoxygenBrief("Returns the IDL type name used for DDS topics.");
        source.WriteDoxygenReturn("The fully qualified IDL type name.");
        source.WriteLine("NDDSUSERDllExport static std::string value()");
        source.WriteLine("{");
        source.WriteLine($"    return \"{declaration.QualifiedName}\";");
        source.WriteLine("}");
        source.CloseBlock(";");

        if (declaration.IsNested)
        {
            EmitNestedTopicSupport(source, declaration);
            return;
        }

        source.BlankLine();
        source.WriteDoxygenBrief("Provides RTI topic registration and sample support operations.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "group__DDSTopicTraitsModule.html");
        source.WriteLine("template<>");
        source.WriteLine($"struct topic_type_support<{declaration.CppType}>");
        source.WriteLine("{");
        source.WriteDoxygenBrief("Registers this type with a participant. ", indentationLevel: 1);
        source.WriteDoxygenParam("participant", "Participant to register with.", indentationLevel: 1);
        source.WriteDoxygenParam("name", "Topic type name.", indentationLevel: 1);
        source.WriteLines("""
                NDDSUSERDllExport static void register_type(
                    dds::domain::DomainParticipant& participant,
                    const std::string& name);
            """);
        source.BlankLine();
        if (declaration.IsXcdr2Only)
        {
            source.WriteDoxygenBrief("Returns the sole representation declared by this IDL type.", indentationLevel: 1);
            source.WriteDoxygenReturn("XCDR2.", indentationLevel: 1);
            source.WriteLines("""
                    static dds::core::policy::DataRepresentationId data_representation()
                    {
                        return dds::core::policy::DataRepresentation::xcdr2();
                    }
                """);
            source.BlankLine();
        }
        source.WriteDoxygenBrief("Serializes a sample to a CDR buffer.", indentationLevel: 1);
        source.WriteDoxygenParam("buffer", "Destination buffer.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Sample to serialize.", indentationLevel: 1);
        source.WriteDoxygenReturn("The destination buffer.", indentationLevel: 1);
        if (declaration.IsFlatData)
        {
            source.WriteDoxygenWrapped("@details",
                "Copies the sample's existing fixed-size XCDR2 buffer. This operation does not register or invoke a topic plugin.",
                indentationLevel: 1);
            source.WriteDoxygenThrows("dds::core::InvalidArgumentError", "when XCDR1 is requested or the sample buffer is not XCDR2.", indentationLevel: 1);
        }
        if (declaration.Members.Any(member => member.IsExternal))
        {
            source.WriteDoxygenThrows("dds::core::InvalidArgumentError", "when external-reference callbacks are unavailable.", indentationLevel: 1);
        }
        source.WriteLine("    NDDSUSERDllExport static std::vector<char>& to_cdr_buffer(");
        source.WriteLine("        std::vector<char>& buffer,");
        source.WriteLine($"        const {declaration.CppType}& sample,");
        source.WriteLine("        dds::core::policy::DataRepresentationId representation =");
        source.WriteLine("            dds::core::policy::DataRepresentation::auto_id());");
        source.BlankLine();
        source.WriteDoxygenBrief("Deserializes a sample from a CDR buffer.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Sample to populate.", indentationLevel: 1);
        source.WriteDoxygenParam("buffer", "Encoded sample bytes.", indentationLevel: 1);
        if (declaration.IsFlatData)
        {
            source.WriteDoxygenWrapped("@details",
                "Copies an exactly sized XCDR2 buffer into an already allocated FlatData sample.",
                indentationLevel: 1);
            source.WriteDoxygenThrows("dds::core::InvalidArgumentError", "when the buffer size or XCDR2 encapsulation is invalid.", indentationLevel: 1);
        }
        if (declaration.Members.Any(member => member.IsExternal))
        {
            source.WriteDoxygenThrows("dds::core::InvalidArgumentError", "when external-reference callbacks are unavailable.", indentationLevel: 1);
        }
        source.WriteLine("    NDDSUSERDllExport static void from_cdr_buffer(");
        source.WriteLine($"        {declaration.CppType}& sample,");
        source.WriteLine("        const std::vector<char>& buffer);");
        source.WriteDoxygenBrief("Resets a sample to its IDL defaults.", indentationLevel: 1);
        EmitSampleReferenceSignature(source, "reset_sample", declaration.CppType, "    ");
        source.WriteDoxygenBrief("Prepares a sample for the requested capacity.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Sample to prepare.", indentationLevel: 1);
        source.WriteDoxygenParam("initial_sample_capacity", "Initial capacity.", indentationLevel: 1);
        source.WriteDoxygenParam("maximum_samples", "Maximum sample count.", indentationLevel: 1);
        source.WriteLines("""
                NDDSUSERDllExport static void allocate_sample(
            """);
        source.WriteLine($"        {declaration.CppType}& sample,");
        source.WriteLine("        int initial_sample_capacity,");
        source.WriteLine("        int maximum_samples);");
        source.BlankLine();
        source.WriteLines("""
                static const rti::topic::TypePluginKind::type type_plugin_kind =
                    rti::topic::TypePluginKind::STL;
            };
            """);
    }

    private static void EmitNestedTopicSupport(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief("Provides sample lifecycle helpers for this non-topic nested value.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "group__DDSTopicTraitsModule.html");
        source.WriteLine("template<>");
        source.WriteLine($"struct topic_type_support<{declaration.CppType}>");
        source.WriteLine("{");
        source.WriteDoxygenBrief("Resets a value to its IDL defaults.", indentationLevel: 1);
        EmitSampleReferenceSignature(source, "reset_sample", declaration.CppType, "    ");
        source.WriteDoxygenBrief("Prepares a value for the requested capacity.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Value to prepare.", indentationLevel: 1);
        source.WriteDoxygenParam("initial_sample_capacity", "Initial sample capacity.", indentationLevel: 1);
        source.WriteDoxygenParam("maximum_samples", "Maximum sample count.", indentationLevel: 1);
        source.WriteLines("""
                NDDSUSERDllExport static void allocate_sample(
            """);
        source.WriteLine($"        {declaration.CppType}& sample,");
        source.WriteLine("        int initial_sample_capacity,");
        source.WriteLine("        int maximum_samples);");
        source.BlankLine();
        source.WriteLines("""
                static const rti::topic::TypePluginKind::type type_plugin_kind =
                    rti::topic::TypePluginKind::STL;
            };
            """);
    }

    /// <summary>Writes the DDS topic name and operations for the planned value.</summary>
    internal static void EmitTopicTraits(SourceWriter source, UnionPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief("Provides the DDS topic type name for this generated union.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "structrti_1_1topic_1_1dynamic__type.html");
        var typeNameExample = $"auto type_name = dds::topic::topic_type_name<{declaration.CppType}>::value();";
        source.WriteDoxygenCode(typeNameExample.Length <= 120
            ? [typeNameExample]
            : ["auto type_name =", $"    dds::topic::topic_type_name<{declaration.CppType}>::value();"]);
        source.WriteLine("template<>");
        source.OpenBlock($"struct topic_type_name<{declaration.CppType}>");
        source.WriteDoxygenBrief("Returns the IDL type name used for DDS topics.");
        source.WriteDoxygenReturn("The fully qualified IDL type name.");
        source.WriteLine("NDDSUSERDllExport static std::string value()");
        source.WriteLine("{");
        source.WriteLine($"    return \"{declaration.QualifiedName}\";");
        source.WriteLine("}");
        source.CloseBlock(";");

        source.BlankLine();
        source.WriteDoxygenBrief("Provides RTI topic registration and sample support operations.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ topic traits",
            "structrti_1_1topic_1_1dynamic__type.html");
        source.WriteLine("template<>");
        source.WriteLine($"struct topic_type_support<{declaration.CppType}>");
        source.WriteLines("""
            {
            """);
        source.WriteDoxygenBrief("Registers this type with a participant.", indentationLevel: 1);
        source.WriteDoxygenParam("participant", "Participant to register with.", indentationLevel: 1);
        source.WriteDoxygenParam("name", "Topic type name.", indentationLevel: 1);
        source.WriteLines("""
                NDDSUSERDllExport static void register_type(
                    dds::domain::DomainParticipant& participant,
                    const std::string& name);
            """);
        source.BlankLine();
        source.WriteDoxygenBrief("Serializes a sample to a CDR buffer.", indentationLevel: 1);
        source.WriteDoxygenParam("buffer", "Destination buffer.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Sample to serialize.", indentationLevel: 1);
        source.WriteDoxygenReturn("The destination buffer.", indentationLevel: 1);
        source.WriteLines("""
                NDDSUSERDllExport static std::vector<char>& to_cdr_buffer(
                    std::vector<char>& buffer,
            """);
        source.WriteLine($"        const {declaration.CppType}& sample,");
        source.WriteLine("        dds::core::policy::DataRepresentationId representation =");
        source.WriteLine("            dds::core::policy::DataRepresentation::auto_id());");
        source.BlankLine();
        source.WriteDoxygenBrief("Deserializes a sample from a CDR buffer.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Sample to populate.", indentationLevel: 1);
        source.WriteDoxygenParam("buffer", "Encoded sample bytes.", indentationLevel: 1);
        source.WriteLines("""
                NDDSUSERDllExport static void from_cdr_buffer(
            """);
        source.WriteLine($"        {declaration.CppType}& sample,");
        source.WriteLine("        const std::vector<char>& buffer);");
        source.WriteDoxygenBrief("Resets a sample to its IDL defaults.", indentationLevel: 1);
        EmitSampleReferenceSignature(source, "reset_sample", declaration.CppType, "    ");
        source.WriteDoxygenBrief("Prepares a sample for the requested capacity.", indentationLevel: 1);
        source.WriteDoxygenParam("sample", "Sample to prepare.", indentationLevel: 1);
        source.WriteDoxygenParam("initial_sample_capacity", "Initial capacity.", indentationLevel: 1);
        source.WriteDoxygenParam("maximum_samples", "Maximum sample count.", indentationLevel: 1);
        EmitSampleReferenceSignature(source, "allocate_sample", declaration.CppType, "    ", includeComma: true);
        source.WriteLine("        int initial_sample_capacity,");
        source.WriteLine("        int maximum_samples);");
        source.BlankLine();
        source.WriteLines("""
                static const rti::topic::TypePluginKind::type type_plugin_kind =
                    rti::topic::TypePluginKind::STL;
            };
            """);
    }

    /// <summary>Declares the RTI DynamicType accessor and related traits.</summary>
    internal static void EmitDynamicTypeTraits(SourceWriter source, StructPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI DynamicType descriptor for {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI DynamicType and DynamicData",
            "group__DDSTopicTraitsModule.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct dynamic_type<{declaration.CppType}>");
        source.WriteLine("using type = dds::core::xtypes::StructType;");
        source.WriteDoxygenBrief("Returns the runtime type descriptor.");
        source.WriteDoxygenReturn("The RTI DynamicType descriptor.");
        source.WriteLine("NDDSUSERDllExport static const type& get();");
        if (declaration.HasSelfRecursiveSequence
            || RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration))
        {
            source.WriteDoxygenBrief("Resolves this type while building a recursive RTI TypeCode graph.");
            source.WriteDoxygen("@param parentVisitedNode Parent resources visited during TypeCode construction.");
            source.WriteDoxygenReturn("The existing type shell when recursion reaches this declaration.");
            var resourceNodeType = source.RtiVersion == "7.7.0"
                ? "RTIXCdrResourceNode"
                : "RTICdrResourceNode";
            source.WriteLine($"NDDSUSERDllExport static const type& get({resourceNodeType}* parentVisitedNode);");
        }

        source.CloseBlock(";");

        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI extensibility kind for {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI extensibility trait",
            "structrti_1_1topic_1_1extensibility.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct extensibility<{declaration.CppType}>");
        source.WriteLine("static const dds::core::xtypes::ExtensibilityKind::type kind =");
        source.WriteLine($"    {declaration.ExtensibilityExpression};");
        source.CloseBlock(";");

        if (declaration.IsFlatData)
        {
            source.BlankLine();
            source.WriteDoxygenBrief($"Provides the RTI DynamicType descriptor for the plain helper of {declaration.QualifiedName}.");
            RtiDocumentation.WriteApiSee(source,
                "RTI DynamicType and DynamicData",
                "group__DDSTopicTraitsModule.html");
            source.WriteLine("template<>");
            source.OpenBlock($"struct dynamic_type<{declaration.FlatDataPlainHelperType}>");
            source.WriteLine("using type = dds::core::xtypes::StructType;");
            source.WriteDoxygenBrief("Returns the helper's runtime type descriptor.");
            source.WriteDoxygenReturn("The RTI DynamicType descriptor.");
            source.WriteLine("NDDSUSERDllExport static const type& get();");
            source.CloseBlock(";");
            source.WriteLine("template<>");
            source.OpenBlock($"struct extensibility<{declaration.FlatDataPlainHelperType}>");
            source.WriteLine("static const dds::core::xtypes::ExtensibilityKind::type kind =");
            source.WriteLine("    dds::core::xtypes::ExtensibilityKind::FINAL;");
            source.CloseBlock(";");
        }

        if (declaration.IsTopic && declaration.Members.Any(member => member.IsExternal))
        {
            source.BlankLine();
            source.WriteDoxygenBrief($"Indicates that {declaration.QualifiedName} contains IDL external members.");
            RtiDocumentation.WriteApiSee(source,
                $"RTI {source.RtiVersion} topic_type_has_external_members trait",
                "group__DDSTopicTraitsModule.html");
            source.WriteLine("template<>");
            source.WriteLine($"struct topic_type_has_external_members<{declaration.CppType}> : public ::dds::core::true_type {{}};");
        }
    }

    /// <summary>Declares the RTI DynamicType accessor and related traits.</summary>
    internal static void EmitDynamicTypeTraits(SourceWriter source, EnumPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI DynamicType descriptor for {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI DynamicType and DynamicData",
            "group__DDSTopicTraitsModule.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct dynamic_type<{declaration.CppType}>");
        source.WriteLine("using type = dds::core::xtypes::EnumType;");
        source.WriteDoxygenBrief("Returns the runtime type descriptor.");
        source.WriteDoxygenReturn("The RTI DynamicType descriptor.");
        source.WriteLine("NDDSUSERDllExport static const type& get();");
        source.CloseBlock(";");

        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI extensibility kind for {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI extensibility trait",
            "structrti_1_1topic_1_1extensibility.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct extensibility<{declaration.CppType}>");
        source.WriteLine("static const dds::core::xtypes::ExtensibilityKind::type kind =");
        source.WriteLine($"    {declaration.ExtensibilityExpression};");
        source.CloseBlock(";");
    }

    /// <summary>Declares the RTI DynamicType accessor and related traits.</summary>
    internal static void EmitDynamicTypeTraits(SourceWriter source, UnionPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI DynamicType descriptor for {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI DynamicType and DynamicData",
            "structrti_1_1topic_1_1dynamic__type.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct dynamic_type<{declaration.CppType}>");
        source.WriteLine("using type = dds::core::xtypes::UnionType;");
        source.WriteDoxygenBrief("Returns the runtime type descriptor.");
        source.WriteDoxygenReturn("The RTI DynamicType descriptor.");
        source.WriteLine("NDDSUSERDllExport static const type& get();");
        source.CloseBlock(";");

        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI extensibility kind for {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI extensibility trait",
            "structrti_1_1topic_1_1extensibility.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct extensibility<{declaration.CppType}>");
        source.WriteLine("static const dds::core::xtypes::ExtensibilityKind::type kind =");
        source.WriteLine($"    {declaration.ExtensibilityExpression};");
        source.CloseBlock(";");
    }

    /// <summary>Declares the RTI DynamicType accessor and related traits.</summary>
    internal static void EmitDynamicTypeTraits(SourceWriter source, AliasPlan declaration)
    {
        source.BlankLine();
        source.WriteDoxygenBrief($"Provides the RTI DynamicType descriptor for typedef {declaration.QualifiedName}.");
        RtiDocumentation.WriteApiSee(source,
            "RTI Modern C++ AliasType",
            "classdds_1_1core_1_1xtypes_1_1AliasType.html");
        source.WriteLine("template<>");
        source.OpenBlock($"struct dynamic_type<{declaration.CppType}_AliasTag_t>");
        source.WriteLine("using type = dds::core::xtypes::AliasType;");
        source.WriteDoxygenBrief("Returns the runtime alias descriptor.");
        source.WriteDoxygenReturn("The RTI AliasType descriptor.");
        source.WriteLine("NDDSUSERDllExport static const type& get();");
        source.CloseBlock(";");
    }

    private static void EmitSampleReferenceSignature(
        SourceWriter source,
        string methodName,
        string typeName,
        string indentation,
        bool includeComma = false)
    {
        var suffix = includeComma ? "," : ");";
        var declaration = $"{indentation}NDDSUSERDllExport static void {methodName}({typeName}& sample{suffix}";
        if (declaration.Length <= 120)
        {
            source.WriteLine(declaration);
            return;
        }

        source.WriteLine($"{indentation}NDDSUSERDllExport static void {methodName}(");
        source.WriteLine($"{indentation}    {typeName}& sample{suffix}");
    }

    /// <summary>Declares the RTI default-enumerator traits for planned enums.</summary>
    internal static void EmitDefaultEnumeratorTraits(SourceWriter source, GenerationPlan plan)
    {
        if (plan.Enums.Count == 0)
        {
            return;
        }

        source.BlankLine();
        source.WriteComment("RTI default enumerators used for value initialization and reset.");
        source.OpenNamespaces("rti");
        source.OpenNamespaces("topic");
        foreach (var declaration in plan.Enums)
        {
            source.WriteLine("template<>");
            source.OpenBlock($"struct default_enumerator<{declaration.CppType}>");
            source.WriteLine($"static const {declaration.CppType} value;");
            source.CloseBlock(";");
        }

        source.CloseNamespaces("topic");
        source.CloseNamespaces("rti");
    }
}
