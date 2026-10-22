

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-label-expressions.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_union_label_expressionsPlugin_1676448188_h
#define _14_union_label_expressionsPlugin_1676448188_h

#include "14-union-label-expressions.hpp"

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

#define LabelExpressionsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define LabelExpressionsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define LabelExpressionsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define LabelExpressionsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define LabelExpressionsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern LabelExpressions*
LabelExpressionsPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern LabelExpressions*
LabelExpressionsPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern LabelExpressions*
LabelExpressionsPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
LabelExpressionsPluginSupport_copy_data(
    LabelExpressions *out,
    const LabelExpressions *in);

NDDSUSERDllExport extern void
LabelExpressionsPluginSupport_destroy_data_w_params(
    LabelExpressions *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
LabelExpressionsPluginSupport_destroy_data_ex(
    LabelExpressions *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
LabelExpressionsPluginSupport_destroy_data(
    LabelExpressions *sample);

NDDSUSERDllExport extern void
LabelExpressionsPluginSupport_print_data(
    const LabelExpressions *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
LabelExpressionsPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
LabelExpressionsPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
LabelExpressionsPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
LabelExpressionsPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
LabelExpressionsPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    LabelExpressions *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
LabelExpressionsPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    LabelExpressions *out,
    const LabelExpressions *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
LabelExpressionsPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const LabelExpressions *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
LabelExpressionsPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    LabelExpressions **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
LabelExpressionsPlugin_deserialize_from_cdr_buffer(
    LabelExpressions *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
LabelExpressionsPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
LabelExpressionsPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
LabelExpressionsPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
LabelExpressionsPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
LabelExpressionsPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    LabelExpressions ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
LabelExpressionsPlugin_new(void);

NDDSUSERDllExport extern void
LabelExpressionsPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_union_label_expressionsPlugin_1676448188_h */
