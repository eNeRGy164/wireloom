

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-nested.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_nestedPlugin_1591529600_h
#define _08_key_nestedPlugin_1591529600_h

#include "08-key-nested.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace CorpusNestedKeys {

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Inner
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Inner)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Inner must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Inner.
    */
    typedef struct Inner InnerKeyHolder;

    #define InnerPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define InnerPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define InnerPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define InnerPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define InnerPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define InnerPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define InnerPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Inner*
    InnerPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Inner*
    InnerPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Inner*
    InnerPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    InnerPluginSupport_copy_data(
        Inner *out,
        const Inner *in);

    NDDSUSERDllExport extern void
    InnerPluginSupport_destroy_data_w_params(
        Inner *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    InnerPluginSupport_destroy_data_ex(
        Inner *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    InnerPluginSupport_destroy_data(
        Inner *sample);

    NDDSUSERDllExport extern void
    InnerPluginSupport_print_data(
        const Inner *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern Inner*
    InnerPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Inner*
    InnerPluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    InnerPluginSupport_destroy_key_ex(
        InnerKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    InnerPluginSupport_destroy_key(
        InnerKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    InnerPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    InnerPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    InnerPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    InnerPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    InnerPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Inner *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Inner *out,
        const Inner *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Inner *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Inner **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_deserialize_from_cdr_buffer(
        Inner *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    InnerPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    InnerPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    InnerPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    InnerPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Inner ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        InnerKeyHolder *key,
        const Inner *instance);

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Inner *instance,
        const InnerKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    InnerPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    InnerPlugin_new(void);

    NDDSUSERDllExport extern void
    InnerPlugin_delete(struct PRESTypePlugin *);

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Outer
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Outer)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Outer must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Outer.
    */
    typedef struct Outer OuterKeyHolder;

    #define OuterPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define OuterPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define OuterPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define OuterPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define OuterPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define OuterPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define OuterPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Outer*
    OuterPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Outer*
    OuterPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Outer*
    OuterPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    OuterPluginSupport_copy_data(
        Outer *out,
        const Outer *in);

    NDDSUSERDllExport extern void
    OuterPluginSupport_destroy_data_w_params(
        Outer *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    OuterPluginSupport_destroy_data_ex(
        Outer *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    OuterPluginSupport_destroy_data(
        Outer *sample);

    NDDSUSERDllExport extern void
    OuterPluginSupport_print_data(
        const Outer *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern Outer*
    OuterPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Outer*
    OuterPluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    OuterPluginSupport_destroy_key_ex(
        OuterKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    OuterPluginSupport_destroy_key(
        OuterKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    OuterPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    OuterPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    OuterPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    OuterPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    OuterPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Outer *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Outer *out,
        const Outer *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Outer *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Outer **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_deserialize_from_cdr_buffer(
        Outer *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    OuterPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    OuterPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    OuterPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    OuterPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Outer ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        OuterKeyHolder *key,
        const Outer *instance);

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Outer *instance,
        const OuterKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    OuterPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    OuterPlugin_new(void);

    NDDSUSERDllExport extern void
    OuterPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusNestedKeys  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _08_key_nestedPlugin_1591529600_h */
