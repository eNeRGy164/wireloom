

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idlPlugin_1662736227_h
#define idlPlugin_1662736227_h

#include "idl.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "features/05-unbounded-sequence-aliasesPlugin.hpp"

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace CorpusUnboundedSequences {

    #define SequenceAliasesSamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define SequenceAliasesSamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define SequenceAliasesSamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define SequenceAliasesSamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define SequenceAliasesSamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern SequenceAliasesSample*
    SequenceAliasesSamplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern SequenceAliasesSample*
    SequenceAliasesSamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern SequenceAliasesSample*
    SequenceAliasesSamplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    SequenceAliasesSamplePluginSupport_copy_data(
        SequenceAliasesSample *out,
        const SequenceAliasesSample *in);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePluginSupport_destroy_data_w_params(
        SequenceAliasesSample *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePluginSupport_destroy_data_ex(
        SequenceAliasesSample *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePluginSupport_destroy_data(
        SequenceAliasesSample *sample);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePluginSupport_print_data(
        const SequenceAliasesSample *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    SequenceAliasesSamplePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    SequenceAliasesSamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        SequenceAliasesSample *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    SequenceAliasesSamplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        SequenceAliasesSample *out,
        const SequenceAliasesSample *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    SequenceAliasesSamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const SequenceAliasesSample *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    SequenceAliasesSamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        SequenceAliasesSample **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    SequenceAliasesSamplePlugin_deserialize_from_cdr_buffer(
        SequenceAliasesSample *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    SequenceAliasesSamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    SequenceAliasesSamplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    SequenceAliasesSamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    SequenceAliasesSamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    SequenceAliasesSamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        SequenceAliasesSample ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    SequenceAliasesSamplePlugin_new(void);

    NDDSUSERDllExport extern void
    SequenceAliasesSamplePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusUnboundedSequences  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* idlPlugin_1662736227_h */
