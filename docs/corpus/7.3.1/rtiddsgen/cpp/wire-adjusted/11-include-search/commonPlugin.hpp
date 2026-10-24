

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from common.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef commonPlugin_1183366319_h
#define commonPlugin_1183366319_h

#include "common.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#define IncludedTypePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define IncludedTypePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define IncludedTypePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define IncludedTypePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define IncludedTypePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern IncludedType*
IncludedTypePluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern IncludedType*
IncludedTypePluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern IncludedType*
IncludedTypePluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
IncludedTypePluginSupport_copy_data(
    IncludedType *out,
    const IncludedType *in);

NDDSUSERDllExport extern void
IncludedTypePluginSupport_destroy_data_w_params(
    IncludedType *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
IncludedTypePluginSupport_destroy_data_ex(
    IncludedType *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
IncludedTypePluginSupport_destroy_data(
    IncludedType *sample);

NDDSUSERDllExport extern void
IncludedTypePluginSupport_print_data(
    const IncludedType *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
IncludedTypePlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
IncludedTypePlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
IncludedTypePlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
IncludedTypePlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
IncludedTypePlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    IncludedType *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
IncludedTypePlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    IncludedType *out,
    const IncludedType *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
IncludedTypePlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const IncludedType *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
IncludedTypePlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    IncludedType **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
IncludedTypePlugin_deserialize_from_cdr_buffer(
    IncludedType *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
IncludedTypePlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
IncludedTypePlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
IncludedTypePlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
IncludedTypePlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
IncludedTypePlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    IncludedType ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
IncludedTypePlugin_new(void);

NDDSUSERDllExport extern void
IncludedTypePlugin_delete(struct PRESTypePlugin *);

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* commonPlugin_1183366319_h */
