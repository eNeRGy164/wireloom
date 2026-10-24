

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idlPlugin_1662736047_h
#define idlPlugin_1662736047_h

#include "idl.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "features/14-autoid-scopePlugin.hpp"

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

#define AutoIdScopeSamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define AutoIdScopeSamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define AutoIdScopeSamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define AutoIdScopeSamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define AutoIdScopeSamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern AutoIdScopeSample*
AutoIdScopeSamplePluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern AutoIdScopeSample*
AutoIdScopeSamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern AutoIdScopeSample*
AutoIdScopeSamplePluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
AutoIdScopeSamplePluginSupport_copy_data(
    AutoIdScopeSample *out,
    const AutoIdScopeSample *in);

NDDSUSERDllExport extern void
AutoIdScopeSamplePluginSupport_destroy_data_w_params(
    AutoIdScopeSample *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
AutoIdScopeSamplePluginSupport_destroy_data_ex(
    AutoIdScopeSample *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
AutoIdScopeSamplePluginSupport_destroy_data(
    AutoIdScopeSample *sample);

NDDSUSERDllExport extern void
AutoIdScopeSamplePluginSupport_print_data(
    const AutoIdScopeSample *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
AutoIdScopeSamplePlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
AutoIdScopeSamplePlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
AutoIdScopeSamplePlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
AutoIdScopeSamplePlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
AutoIdScopeSamplePlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    AutoIdScopeSample *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
AutoIdScopeSamplePlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    AutoIdScopeSample *out,
    const AutoIdScopeSample *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
AutoIdScopeSamplePlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const AutoIdScopeSample *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
AutoIdScopeSamplePlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    AutoIdScopeSample **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
AutoIdScopeSamplePlugin_deserialize_from_cdr_buffer(
    AutoIdScopeSample *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
AutoIdScopeSamplePlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
AutoIdScopeSamplePlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
AutoIdScopeSamplePlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
AutoIdScopeSamplePlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
AutoIdScopeSamplePlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    AutoIdScopeSample ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
AutoIdScopeSamplePlugin_new(void);

NDDSUSERDllExport extern void
AutoIdScopeSamplePlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* idlPlugin_1662736047_h */
