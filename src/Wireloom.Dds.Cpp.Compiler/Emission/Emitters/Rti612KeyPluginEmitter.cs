using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits RTI 6.1.2 PRESTypePlugin callbacks for keyed topic structs.</summary>
internal static class Rti612KeyPluginEmitter
{
    /// <summary>Writes key callback policy and per-topic-struct key-member projections.</summary>
    internal static void EmitCallbacks(SourceWriter source, GenerationPlan plan)
    {
        source.WriteLines("""
            template <typename T>
            struct WireloomRti612KeyCallbacks
            {
                static const bool has_keys = false;
                static PRESTypePluginKeyKind get_key_kind() { return PRES_TYPEPLUGIN_NO_KEY; }
                static void* create_key() { return nullptr; }
                static void destroy_key(void*) { }
                static RTIBool instance_to_key(PRESTypePluginEndpointData, void*, const void*) { return RTI_FALSE; }
                static RTIBool key_to_instance(PRESTypePluginEndpointData, void*, const void*) { return RTI_FALSE; }
                static unsigned int get_serialized_key_max_size(
                    PRESTypePluginEndpointData, RTIBool, RTIEncapsulationId, unsigned int) { return 0; }
                static unsigned int get_serialized_key_max_size_for_keyhash(
                    PRESTypePluginEndpointData, RTIEncapsulationId, unsigned int) { return 0; }
                static RTIBool deserialize_key(
                    PRESTypePluginEndpointData, void**, RTIBool*, RTICdrStream*, RTIBool, RTIBool, void*) { return RTI_FALSE; }
                static RTIBool serialized_sample_to_keyhash(
                    PRESTypePluginEndpointData, RTICdrStream*, DDS_KeyHash_t*, RTIBool, void*) { return RTI_FALSE; }
            };
            """);

        foreach (var declaration in plan.Structs.Where(structure => structure.IsTopic
                     && structure.Members.Any(member => member.IsKey)))
        {
            var typeName = declaration.CppType;
            source.BlankLine();
            source.WriteLine("template <>");
            source.WriteLine($"struct WireloomRti612KeyCallbacks<{typeName}>");
            source.WriteLine("{");
            source.WriteLine("    static const bool has_keys = true;");
            source.WriteLine("    static PRESTypePluginKeyKind get_key_kind() { return PRES_TYPEPLUGIN_USER_KEY; }");
            source.WriteLine($"    static void* create_key() {{ return new (std::nothrow) {typeName}{{}}; }}");
            source.WriteLine($"    static void destroy_key(void* key) {{ delete static_cast<{typeName}*>(key); }}");
            source.BlankLine();
            EmitCopyCallback(source, declaration, typeName, "instance_to_key");
            source.BlankLine();
            EmitCopyCallback(source, declaration, typeName, "key_to_instance");
            source.BlankLine();
            EmitKeyMaxSizeCallback(source);
            source.BlankLine();
            EmitKeyHashMaxSizeCallback(source);
            source.BlankLine();
            EmitDeserializeKeyCallback(source, typeName);
            source.BlankLine();
            EmitSerializedSampleToKeyHashCallback(source, typeName);
            source.WriteLine("};");
        }
    }

    private static void EmitCopyCallback(
        SourceWriter source,
        StructPlan declaration,
        string typeName,
        string callbackName)
    {
        source.WriteLine($"    static RTIBool {callbackName}(");
        source.WriteLine("        PRESTypePluginEndpointData, void* destination, const void* source)");
        source.WriteLine("    {");
        source.WriteLine("        if (destination == nullptr || source == nullptr) {");
        source.WriteLine("            return RTI_FALSE;");
        source.WriteLine("        }");
        source.WriteLine($"        auto* typed_destination = static_cast<{typeName}*>(destination);");
        source.WriteLine($"        const auto* typed_source = static_cast<const {typeName}*>(source);");
        source.WriteLine("        try {");
        foreach (var member in declaration.Members.Where(member => member.IsKey))
        {
            source.WriteLine($"            typed_destination->{member.Name} = typed_source->{member.Name};");
        }

        source.WriteLines("""
                    return RTI_TRUE;
                } catch (...) {
                    return RTI_FALSE;
                }
            }
            """);
    }

    private static void EmitKeyMaxSizeCallback(SourceWriter source)
    {
        source.WriteLines("""
                static unsigned int get_serialized_key_max_size(
                    PRESTypePluginEndpointData endpoint, RTIBool include_encapsulation,
                    RTIEncapsulationId encapsulation, unsigned int alignment)
                {
                    RTIBool overflow = RTI_FALSE;
                    const auto size = PRESTypePlugin_interpretedGetSerializedKeyMaxSize(
                        endpoint, &overflow, include_encapsulation, encapsulation, alignment);
                    return overflow ? RTI_CDR_MAX_SERIALIZED_SIZE : size;
                }
            """);
    }

    private static void EmitKeyHashMaxSizeCallback(SourceWriter source)
    {
        source.WriteLines("""
                static unsigned int get_serialized_key_max_size_for_keyhash(
                    PRESTypePluginEndpointData endpoint, RTIEncapsulationId encapsulation,
                    unsigned int alignment)
                {
                    RTIBool overflow = RTI_FALSE;
                    const auto size = PRESTypePlugin_interpretedGetSerializedKeyMaxSizeForKeyhash(
                        endpoint, &overflow, encapsulation, alignment);
                    return overflow ? RTI_CDR_MAX_SERIALIZED_SIZE : size;
                }
            """);
    }

    private static void EmitDeserializeKeyCallback(SourceWriter source, string typeName)
    {
        source.WriteLine("    static RTIBool deserialize_key(");
        source.WriteLine("        PRESTypePluginEndpointData endpoint, void** key, RTIBool* drop_key,");
        source.WriteLines("""
                RTICdrStream* stream, RTIBool deserialize_encapsulation,
                RTIBool deserialize_key_fields, void* endpoint_plugin_qos)
            {
                (void) drop_key;
                if (key == nullptr || stream == nullptr) {
                    return RTI_FALSE;
                }
                auto** typed_key = reinterpret_cast<KEY_TYPE**>(key);
                try {
                    stream->_xTypesState.unassignable = RTI_FALSE;
                    const auto result = PRESTypePlugin_interpretedDeserializeKey(
                        endpoint, *typed_key, stream, deserialize_encapsulation,
                        deserialize_key_fields, endpoint_plugin_qos);
                    return result && !stream->_xTypesState.unassignable ? RTI_TRUE : RTI_FALSE;
                } catch (...) {
                    return RTI_FALSE;
                }
            }
            """.Replace("KEY_TYPE", typeName, StringComparison.Ordinal));
    }

    private static void EmitSerializedSampleToKeyHashCallback(SourceWriter source, string typeName)
    {
        source.WriteLines("""
                static RTIBool serialized_sample_to_keyhash(
                    PRESTypePluginEndpointData endpoint, RTICdrStream* stream,
                    DDS_KeyHash_t* keyhash, RTIBool deserialize_encapsulation,
                    void* endpoint_plugin_qos)
                {
            """);
        source.WriteLine($"        auto* sample = static_cast<{typeName}*>(");
        source.WriteLines("""
                    PRESTypePluginDefaultEndpointData_getTempSample(endpoint));
                    if (sample == nullptr || stream == nullptr || keyhash == nullptr) {
                        return RTI_FALSE;
                    }
                    if (!PRESTypePlugin_interpretedSerializedSampleToKey(
                            endpoint, sample, stream, deserialize_encapsulation,
                            RTI_TRUE, endpoint_plugin_qos)) {
                        return RTI_FALSE;
                    }
                    return PRESTypePlugin_interpretedInstanceToKeyHash(
                        endpoint, keyhash, sample, RTICdrStream_getEncapsulationKind(stream));
                }
            """);
    }
}
