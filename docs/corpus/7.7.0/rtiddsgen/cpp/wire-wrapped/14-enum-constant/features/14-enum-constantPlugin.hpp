

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_enum_constantPlugin_1907946297_h
#define _14_enum_constantPlugin_1907946297_h

#include "14-enum-constant.hpp"

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
ColorPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern unsigned int
ColorPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
ColorPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* ----------------------------------------------------------------------------
Support functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern void
ColorPluginSupport_print_data(
    const Color *sample,
    const char *desc,
    unsigned int indent_level);

#define EnumConstantPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define EnumConstantPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define EnumConstantPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define EnumConstantPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define EnumConstantPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern EnumConstant*
EnumConstantPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern EnumConstant*
EnumConstantPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern EnumConstant*
EnumConstantPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
EnumConstantPluginSupport_copy_data(
    EnumConstant *out,
    const EnumConstant *in);

NDDSUSERDllExport extern void
EnumConstantPluginSupport_destroy_data_w_params(
    EnumConstant *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
EnumConstantPluginSupport_destroy_data_ex(
    EnumConstant *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
EnumConstantPluginSupport_destroy_data(
    EnumConstant *sample);

NDDSUSERDllExport extern void
EnumConstantPluginSupport_print_data(
    const EnumConstant *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
EnumConstantPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
EnumConstantPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
EnumConstantPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
EnumConstantPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
EnumConstantPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstant *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
EnumConstantPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstant *out,
    const EnumConstant *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
EnumConstantPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const EnumConstant *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
EnumConstantPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstant **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
EnumConstantPlugin_deserialize_from_cdr_buffer(
    EnumConstant *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
EnumConstantPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
EnumConstantPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
EnumConstantPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
EnumConstantPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
EnumConstantPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    EnumConstant ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
EnumConstantPlugin_new(void);

NDDSUSERDllExport extern void
EnumConstantPlugin_delete(struct PRESTypePlugin *);

#define EnumUnionPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define EnumUnionPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define EnumUnionPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define EnumUnionPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define EnumUnionPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern EnumUnion*
EnumUnionPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern EnumUnion*
EnumUnionPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern EnumUnion*
EnumUnionPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
EnumUnionPluginSupport_copy_data(
    EnumUnion *out,
    const EnumUnion *in);

NDDSUSERDllExport extern void
EnumUnionPluginSupport_destroy_data_w_params(
    EnumUnion *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
EnumUnionPluginSupport_destroy_data_ex(
    EnumUnion *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
EnumUnionPluginSupport_destroy_data(
    EnumUnion *sample);

NDDSUSERDllExport extern void
EnumUnionPluginSupport_print_data(
    const EnumUnion *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
EnumUnionPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
EnumUnionPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
EnumUnionPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
EnumUnionPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
EnumUnionPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    EnumUnion *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
EnumUnionPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    EnumUnion *out,
    const EnumUnion *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
EnumUnionPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const EnumUnion *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
EnumUnionPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    EnumUnion **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
EnumUnionPlugin_deserialize_from_cdr_buffer(
    EnumUnion *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
EnumUnionPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
EnumUnionPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
EnumUnionPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
EnumUnionPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
EnumUnionPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    EnumUnion ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
EnumUnionPlugin_new(void);

NDDSUSERDllExport extern void
EnumUnionPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_enum_constantPlugin_1907946297_h */
