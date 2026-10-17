

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-status.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_statusPlugin_1784849268_h
#define _13_statusPlugin_1784849268_h

#include "13-status.hpp"

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

#define CorpusIntegrationStatusPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define CorpusIntegrationStatusPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define CorpusIntegrationStatusPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define CorpusIntegrationStatusPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define CorpusIntegrationStatusPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern CorpusIntegrationStatus*
CorpusIntegrationStatusPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern CorpusIntegrationStatus*
CorpusIntegrationStatusPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern CorpusIntegrationStatus*
CorpusIntegrationStatusPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationStatusPluginSupport_copy_data(
    CorpusIntegrationStatus *out,
    const CorpusIntegrationStatus *in);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPluginSupport_destroy_data_w_params(
    CorpusIntegrationStatus *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPluginSupport_destroy_data_ex(
    CorpusIntegrationStatus *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPluginSupport_destroy_data(
    CorpusIntegrationStatus *sample);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPluginSupport_print_data(
    const CorpusIntegrationStatus *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
CorpusIntegrationStatusPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
CorpusIntegrationStatusPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationStatus *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationStatusPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationStatus *out,
    const CorpusIntegrationStatus *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
CorpusIntegrationStatusPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const CorpusIntegrationStatus *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
CorpusIntegrationStatusPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationStatus **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationStatusPlugin_deserialize_from_cdr_buffer(
    CorpusIntegrationStatus *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
CorpusIntegrationStatusPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
CorpusIntegrationStatusPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
CorpusIntegrationStatusPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
CorpusIntegrationStatusPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationStatusPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationStatus ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
CorpusIntegrationStatusPlugin_new(void);

NDDSUSERDllExport extern void
CorpusIntegrationStatusPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _13_statusPlugin_1784849268_h */
