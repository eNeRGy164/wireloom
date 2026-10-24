

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-multiple.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _02_multiplePlugin_223670400_h
#define _02_multiplePlugin_223670400_h

#include "02-multiple.hpp"

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

#define FirstPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define FirstPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define FirstPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define FirstPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define FirstPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern First*
FirstPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern First*
FirstPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern First*
FirstPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
FirstPluginSupport_copy_data(
    First *out,
    const First *in);

NDDSUSERDllExport extern void
FirstPluginSupport_destroy_data_w_params(
    First *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
FirstPluginSupport_destroy_data_ex(
    First *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
FirstPluginSupport_destroy_data(
    First *sample);

NDDSUSERDllExport extern void
FirstPluginSupport_print_data(
    const First *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
FirstPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
FirstPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
FirstPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
FirstPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
FirstPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    First *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
FirstPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    First *out,
    const First *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
FirstPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const First *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
FirstPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    First **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
FirstPlugin_deserialize_from_cdr_buffer(
    First *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
FirstPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
FirstPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
FirstPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
FirstPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
FirstPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    First ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
FirstPlugin_new(void);

NDDSUSERDllExport extern void
FirstPlugin_delete(struct PRESTypePlugin *);

#define SecondPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define SecondPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define SecondPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define SecondPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define SecondPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern Second*
SecondPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern Second*
SecondPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Second*
SecondPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
SecondPluginSupport_copy_data(
    Second *out,
    const Second *in);

NDDSUSERDllExport extern void
SecondPluginSupport_destroy_data_w_params(
    Second *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
SecondPluginSupport_destroy_data_ex(
    Second *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
SecondPluginSupport_destroy_data(
    Second *sample);

NDDSUSERDllExport extern void
SecondPluginSupport_print_data(
    const Second *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
SecondPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
SecondPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
SecondPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
SecondPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
SecondPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    Second *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
SecondPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    Second *out,
    const Second *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
SecondPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const Second *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
SecondPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    Second **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
SecondPlugin_deserialize_from_cdr_buffer(
    Second *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
SecondPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
SecondPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
SecondPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
SecondPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
SecondPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    Second ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
SecondPlugin_new(void);

NDDSUSERDllExport extern void
SecondPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _02_multiplePlugin_223670400_h */
