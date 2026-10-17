

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-integration.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_integrationPlugin_1009554025_h
#define _13_integrationPlugin_1009554025_h

#include "13-integration.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "../shared/13-statusPlugin.hpp"

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

#define CorpusIntegrationMessagePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define CorpusIntegrationMessagePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define CorpusIntegrationMessagePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define CorpusIntegrationMessagePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define CorpusIntegrationMessagePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern CorpusIntegrationMessage*
CorpusIntegrationMessagePluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern CorpusIntegrationMessage*
CorpusIntegrationMessagePluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern CorpusIntegrationMessage*
CorpusIntegrationMessagePluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationMessagePluginSupport_copy_data(
    CorpusIntegrationMessage *out,
    const CorpusIntegrationMessage *in);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePluginSupport_destroy_data_w_params(
    CorpusIntegrationMessage *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePluginSupport_destroy_data_ex(
    CorpusIntegrationMessage *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePluginSupport_destroy_data(
    CorpusIntegrationMessage *sample);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePluginSupport_print_data(
    const CorpusIntegrationMessage *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
CorpusIntegrationMessagePlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
CorpusIntegrationMessagePlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationMessage *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationMessagePlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationMessage *out,
    const CorpusIntegrationMessage *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
CorpusIntegrationMessagePlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const CorpusIntegrationMessage *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
CorpusIntegrationMessagePlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationMessage **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationMessagePlugin_deserialize_from_cdr_buffer(
    CorpusIntegrationMessage *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
CorpusIntegrationMessagePlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
CorpusIntegrationMessagePlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
CorpusIntegrationMessagePlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
CorpusIntegrationMessagePlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
CorpusIntegrationMessagePlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    CorpusIntegrationMessage ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
CorpusIntegrationMessagePlugin_new(void);

NDDSUSERDllExport extern void
CorpusIntegrationMessagePlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _13_integrationPlugin_1009554025_h */
