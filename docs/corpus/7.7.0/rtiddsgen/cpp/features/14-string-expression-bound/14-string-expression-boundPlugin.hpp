

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-string-expression-bound.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_string_expression_boundPlugin_994811211_h
#define _14_string_expression_boundPlugin_994811211_h

#include "14-string-expression-bound.hpp"

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

#define StringBoundedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define StringBoundedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define StringBoundedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define StringBoundedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define StringBoundedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern StringBounded*
StringBoundedPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern StringBounded*
StringBoundedPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern StringBounded*
StringBoundedPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
StringBoundedPluginSupport_copy_data(
    StringBounded *out,
    const StringBounded *in);

NDDSUSERDllExport extern void
StringBoundedPluginSupport_destroy_data_w_params(
    StringBounded *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
StringBoundedPluginSupport_destroy_data_ex(
    StringBounded *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
StringBoundedPluginSupport_destroy_data(
    StringBounded *sample);

NDDSUSERDllExport extern void
StringBoundedPluginSupport_print_data(
    const StringBounded *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
StringBoundedPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
StringBoundedPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
StringBoundedPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
StringBoundedPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
StringBoundedPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    StringBounded *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
StringBoundedPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    StringBounded *out,
    const StringBounded *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
StringBoundedPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const StringBounded *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
StringBoundedPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    StringBounded **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
StringBoundedPlugin_deserialize_from_cdr_buffer(
    StringBounded *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
StringBoundedPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
StringBoundedPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
StringBoundedPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
StringBoundedPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
StringBoundedPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    StringBounded ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
StringBoundedPlugin_new(void);

NDDSUSERDllExport extern void
StringBoundedPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_string_expression_boundPlugin_994811211_h */
