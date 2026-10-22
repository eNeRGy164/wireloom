

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-extensibility-full.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_extensibility_fullPlugin_220549410_h
#define _14_extensibility_fullPlugin_220549410_h

#include "14-extensibility-full.hpp"

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

#define FullExtensibilityPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define FullExtensibilityPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define FullExtensibilityPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define FullExtensibilityPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define FullExtensibilityPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern FullExtensibility*
FullExtensibilityPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern FullExtensibility*
FullExtensibilityPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern FullExtensibility*
FullExtensibilityPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
FullExtensibilityPluginSupport_copy_data(
    FullExtensibility *out,
    const FullExtensibility *in);

NDDSUSERDllExport extern void
FullExtensibilityPluginSupport_destroy_data_w_params(
    FullExtensibility *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
FullExtensibilityPluginSupport_destroy_data_ex(
    FullExtensibility *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
FullExtensibilityPluginSupport_destroy_data(
    FullExtensibility *sample);

NDDSUSERDllExport extern void
FullExtensibilityPluginSupport_print_data(
    const FullExtensibility *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
FullExtensibilityPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
FullExtensibilityPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
FullExtensibilityPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
FullExtensibilityPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
FullExtensibilityPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    FullExtensibility *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
FullExtensibilityPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    FullExtensibility *out,
    const FullExtensibility *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
FullExtensibilityPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const FullExtensibility *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
FullExtensibilityPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    FullExtensibility **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
FullExtensibilityPlugin_deserialize_from_cdr_buffer(
    FullExtensibility *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
FullExtensibilityPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
FullExtensibilityPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
FullExtensibilityPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
FullExtensibilityPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
FullExtensibilityPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    FullExtensibility ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
FullExtensibilityPlugin_new(void);

NDDSUSERDllExport extern void
FullExtensibilityPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_extensibility_fullPlugin_220549410_h */
