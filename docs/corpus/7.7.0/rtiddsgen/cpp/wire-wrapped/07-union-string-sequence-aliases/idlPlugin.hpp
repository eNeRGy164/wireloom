

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idlPlugin_1662736082_h
#define idlPlugin_1662736082_h

#include "idl.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "features/07-union-string-sequence-aliasesPlugin.hpp"

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace RuntimeUnionStringSequences {

    #define StringSequenceUnionsSamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define StringSequenceUnionsSamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define StringSequenceUnionsSamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define StringSequenceUnionsSamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define StringSequenceUnionsSamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern StringSequenceUnionsSample*
    StringSequenceUnionsSamplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern StringSequenceUnionsSample*
    StringSequenceUnionsSamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern StringSequenceUnionsSample*
    StringSequenceUnionsSamplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    StringSequenceUnionsSamplePluginSupport_copy_data(
        StringSequenceUnionsSample *out,
        const StringSequenceUnionsSample *in);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePluginSupport_destroy_data_w_params(
        StringSequenceUnionsSample *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePluginSupport_destroy_data_ex(
        StringSequenceUnionsSample *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePluginSupport_destroy_data(
        StringSequenceUnionsSample *sample);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePluginSupport_print_data(
        const StringSequenceUnionsSample *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    StringSequenceUnionsSamplePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    StringSequenceUnionsSamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceUnionsSample *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    StringSequenceUnionsSamplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceUnionsSample *out,
        const StringSequenceUnionsSample *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    StringSequenceUnionsSamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const StringSequenceUnionsSample *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    StringSequenceUnionsSamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceUnionsSample **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    StringSequenceUnionsSamplePlugin_deserialize_from_cdr_buffer(
        StringSequenceUnionsSample *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    StringSequenceUnionsSamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    StringSequenceUnionsSamplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    StringSequenceUnionsSamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    StringSequenceUnionsSamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    StringSequenceUnionsSamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceUnionsSample ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    StringSequenceUnionsSamplePlugin_new(void);

    NDDSUSERDllExport extern void
    StringSequenceUnionsSamplePlugin_delete(struct PRESTypePlugin *);

} /* namespace RuntimeUnionStringSequences  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* idlPlugin_1662736082_h */
