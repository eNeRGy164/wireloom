

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-annotations-extended.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _10_annotations_extendedPlugin_192864544_h
#define _10_annotations_extendedPlugin_192864544_h

#include "10-annotations-extended.hpp"

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

namespace CorpusAnnotationVariants {

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Sample
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Sample)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Sample must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Sample.
    */
    typedef struct Sample SampleKeyHolder;

    #define SamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define SamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define SamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define SamplePlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define SamplePlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define SamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define SamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    SamplePluginSupport_copy_data(
        Sample *out,
        const Sample *in);

    NDDSUSERDllExport extern void
    SamplePluginSupport_destroy_data_w_params(
        Sample *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    SamplePluginSupport_destroy_data_ex(
        Sample *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    SamplePluginSupport_destroy_data(
        Sample *sample);

    NDDSUSERDllExport extern void
    SamplePluginSupport_print_data(
        const Sample *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    SamplePluginSupport_destroy_key_ex(
        SampleKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    SamplePluginSupport_destroy_key(
        SampleKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    SamplePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    SamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    SamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    SamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    SamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *out,
        const Sample *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Sample *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Sample **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_deserialize_from_cdr_buffer(
        Sample *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    SamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    SamplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    SamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    SamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Sample ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        SampleKeyHolder *key,
        const Sample *instance);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Sample *instance,
        const SampleKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    SamplePlugin_new(void);

    NDDSUSERDllExport extern void
    SamplePlugin_delete(struct PRESTypePlugin *);

    #define NestedValuePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NestedValuePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define NestedValuePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NestedValuePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define NestedValuePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NestedValue*
    NestedValuePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NestedValue*
    NestedValuePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NestedValue*
    NestedValuePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    NestedValuePluginSupport_copy_data(
        NestedValue *out,
        const NestedValue *in);

    NDDSUSERDllExport extern void
    NestedValuePluginSupport_destroy_data_w_params(
        NestedValue *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    NestedValuePluginSupport_destroy_data_ex(
        NestedValue *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    NestedValuePluginSupport_destroy_data(
        NestedValue *sample);

    NDDSUSERDllExport extern void
    NestedValuePluginSupport_print_data(
        const NestedValue *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NestedValuePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NestedValue *out,
        const NestedValue *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NestedValuePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const NestedValue *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    NestedValuePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        NestedValue **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    NestedValuePlugin_deserialize_from_cdr_buffer(
        NestedValue *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    NestedValuePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    NestedValuePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    NestedValuePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    NestedValuePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    NestedValuePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        NestedValue ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

} /* namespace CorpusAnnotationVariants  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _10_annotations_extendedPlugin_192864544_h */
