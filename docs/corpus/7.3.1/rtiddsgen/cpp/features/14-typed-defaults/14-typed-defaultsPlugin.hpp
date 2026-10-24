

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-typed-defaults.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_typed_defaultsPlugin_188549010_h
#define _14_typed_defaultsPlugin_188549010_h

#include "14-typed-defaults.hpp"

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

#define SevenPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define SevenPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
#define SevenPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define SevenPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
#define SevenPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern Seven*
SevenPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern Seven*
SevenPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Seven*
SevenPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool 
SevenPluginSupport_copy_data(
    Seven *out,
    const Seven *in);

NDDSUSERDllExport extern void 
SevenPluginSupport_destroy_data_w_params(
    Seven *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void 
SevenPluginSupport_destroy_data_ex(
    Seven *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void 
SevenPluginSupport_destroy_data(
    Seven *sample);

NDDSUSERDllExport extern void 
SevenPluginSupport_print_data(
    const Seven *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool 
SevenPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    Seven *out,
    const Seven *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern unsigned int 
SevenPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind 
SevenPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int 
SevenPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int 
SevenPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

#define TextAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define TextAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
#define TextAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define TextAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
#define TextAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern TextAlias*
TextAliasPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern TextAlias*
TextAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern TextAlias*
TextAliasPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool 
TextAliasPluginSupport_copy_data(
    TextAlias *out,
    const TextAlias *in);

NDDSUSERDllExport extern void 
TextAliasPluginSupport_destroy_data_w_params(
    TextAlias *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void 
TextAliasPluginSupport_destroy_data_ex(
    TextAlias *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void 
TextAliasPluginSupport_destroy_data(
    TextAlias *sample);

NDDSUSERDllExport extern void 
TextAliasPluginSupport_print_data(
    const TextAlias *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool 
TextAliasPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    TextAlias *out,
    const TextAlias *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern unsigned int 
TextAliasPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind 
TextAliasPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int 
TextAliasPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int 
TextAliasPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

#define RatioPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define RatioPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
#define RatioPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define RatioPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
#define RatioPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern Ratio*
RatioPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern Ratio*
RatioPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Ratio*
RatioPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool 
RatioPluginSupport_copy_data(
    Ratio *out,
    const Ratio *in);

NDDSUSERDllExport extern void 
RatioPluginSupport_destroy_data_w_params(
    Ratio *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void 
RatioPluginSupport_destroy_data_ex(
    Ratio *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void 
RatioPluginSupport_destroy_data(
    Ratio *sample);

NDDSUSERDllExport extern void 
RatioPluginSupport_print_data(
    const Ratio *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool 
RatioPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    Ratio *out,
    const Ratio *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern unsigned int 
RatioPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind 
RatioPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int 
RatioPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int 
RatioPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

#define TypedDefaultsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define TypedDefaultsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
#define TypedDefaultsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define TypedDefaultsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
#define TypedDefaultsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern TypedDefaults*
TypedDefaultsPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern TypedDefaults*
TypedDefaultsPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern TypedDefaults*
TypedDefaultsPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool 
TypedDefaultsPluginSupport_copy_data(
    TypedDefaults *out,
    const TypedDefaults *in);

NDDSUSERDllExport extern void 
TypedDefaultsPluginSupport_destroy_data_w_params(
    TypedDefaults *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void 
TypedDefaultsPluginSupport_destroy_data_ex(
    TypedDefaults *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void 
TypedDefaultsPluginSupport_destroy_data(
    TypedDefaults *sample);

NDDSUSERDllExport extern void 
TypedDefaultsPluginSupport_print_data(
    const TypedDefaults *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData 
TypedDefaultsPlugin_on_participant_attached(
    void *registration_data, 
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration, 
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void 
TypedDefaultsPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData 
TypedDefaultsPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration, 
    void *container_plugin_context);

NDDSUSERDllExport extern void 
TypedDefaultsPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void    
TypedDefaultsPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    TypedDefaults *sample,
    void *handle);    

NDDSUSERDllExport extern RTIBool 
TypedDefaultsPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    TypedDefaults *out,
    const TypedDefaults *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
TypedDefaultsPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const TypedDefaults *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr()); 

NDDSUSERDllExport extern RTIBool 
TypedDefaultsPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    TypedDefaults **sample, 
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample, 
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
TypedDefaultsPlugin_deserialize_from_cdr_buffer(
    TypedDefaults *sample,
    const char * buffer,
    unsigned int length);    

NDDSUSERDllExport extern unsigned int 
TypedDefaultsPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind 
TypedDefaultsPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int 
TypedDefaultsPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int 
TypedDefaultsPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool 
TypedDefaultsPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    TypedDefaults ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
TypedDefaultsPlugin_new(void);

NDDSUSERDllExport extern void
TypedDefaultsPlugin_delete(struct PRESTypePlugin *);

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_typed_defaultsPlugin_188549010_h */

