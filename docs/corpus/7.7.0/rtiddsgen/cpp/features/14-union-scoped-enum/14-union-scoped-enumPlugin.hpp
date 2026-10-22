

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-scoped-enum.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_union_scoped_enumPlugin_1634330211_h
#define _14_union_scoped_enumPlugin_1634330211_h

#include "14-union-scoped-enum.hpp"

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

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern unsigned int
ChoicePlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern unsigned int
ChoicePlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
ChoicePlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* ----------------------------------------------------------------------------
Support functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern void
ChoicePluginSupport_print_data(
    const Choice *sample,
    const char *desc,
    unsigned int indent_level);

#define ScopedEnumPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define ScopedEnumPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define ScopedEnumPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define ScopedEnumPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define ScopedEnumPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern ScopedEnum*
ScopedEnumPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern ScopedEnum*
ScopedEnumPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern ScopedEnum*
ScopedEnumPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
ScopedEnumPluginSupport_copy_data(
    ScopedEnum *out,
    const ScopedEnum *in);

NDDSUSERDllExport extern void
ScopedEnumPluginSupport_destroy_data_w_params(
    ScopedEnum *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
ScopedEnumPluginSupport_destroy_data_ex(
    ScopedEnum *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
ScopedEnumPluginSupport_destroy_data(
    ScopedEnum *sample);

NDDSUSERDllExport extern void
ScopedEnumPluginSupport_print_data(
    const ScopedEnum *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
ScopedEnumPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
ScopedEnumPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
ScopedEnumPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
ScopedEnumPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
ScopedEnumPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    ScopedEnum *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
ScopedEnumPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    ScopedEnum *out,
    const ScopedEnum *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
ScopedEnumPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const ScopedEnum *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
ScopedEnumPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    ScopedEnum **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
ScopedEnumPlugin_deserialize_from_cdr_buffer(
    ScopedEnum *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
ScopedEnumPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
ScopedEnumPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
ScopedEnumPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
ScopedEnumPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
ScopedEnumPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    ScopedEnum ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
ScopedEnumPlugin_new(void);

NDDSUSERDllExport extern void
ScopedEnumPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_union_scoped_enumPlugin_1634330211_h */
