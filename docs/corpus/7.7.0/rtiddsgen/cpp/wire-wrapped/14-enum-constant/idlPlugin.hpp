

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idlPlugin_1662735979_h
#define idlPlugin_1662735979_h

#include "idl.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "features/14-enum-constantPlugin.hpp"

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

#define EnumConstantSamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define EnumConstantSamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define EnumConstantSamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define EnumConstantSamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define EnumConstantSamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern EnumConstantSample*
EnumConstantSamplePluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern EnumConstantSample*
EnumConstantSamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern EnumConstantSample*
EnumConstantSamplePluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
EnumConstantSamplePluginSupport_copy_data(
    EnumConstantSample *out,
    const EnumConstantSample *in);

NDDSUSERDllExport extern void
EnumConstantSamplePluginSupport_destroy_data_w_params(
    EnumConstantSample *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
EnumConstantSamplePluginSupport_destroy_data_ex(
    EnumConstantSample *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
EnumConstantSamplePluginSupport_destroy_data(
    EnumConstantSample *sample);

NDDSUSERDllExport extern void
EnumConstantSamplePluginSupport_print_data(
    const EnumConstantSample *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
EnumConstantSamplePlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
EnumConstantSamplePlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
EnumConstantSamplePlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
EnumConstantSamplePlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
EnumConstantSamplePlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstantSample *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
EnumConstantSamplePlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstantSample *out,
    const EnumConstantSample *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
EnumConstantSamplePlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const EnumConstantSample *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
EnumConstantSamplePlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstantSample **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
EnumConstantSamplePlugin_deserialize_from_cdr_buffer(
    EnumConstantSample *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
EnumConstantSamplePlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
EnumConstantSamplePlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
EnumConstantSamplePlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
EnumConstantSamplePlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
EnumConstantSamplePlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstantSample ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
EnumConstantSamplePlugin_new(void);

NDDSUSERDllExport extern void
EnumConstantSamplePlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* idlPlugin_1662735979_h */
