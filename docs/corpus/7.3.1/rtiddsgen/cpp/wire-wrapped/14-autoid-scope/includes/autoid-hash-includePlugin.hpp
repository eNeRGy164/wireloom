

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from autoid-hash-include.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef autoid_hash_includePlugin_2081427871_h
#define autoid_hash_includePlugin_2081427871_h

#include "autoid-hash-include.hpp"

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

#define IncludedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define IncludedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
#define IncludedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define IncludedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
#define IncludedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern Included*
IncludedPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern Included*
IncludedPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Included*
IncludedPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool 
IncludedPluginSupport_copy_data(
    Included *out,
    const Included *in);

NDDSUSERDllExport extern void 
IncludedPluginSupport_destroy_data_w_params(
    Included *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void 
IncludedPluginSupport_destroy_data_ex(
    Included *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void 
IncludedPluginSupport_destroy_data(
    Included *sample);

NDDSUSERDllExport extern void 
IncludedPluginSupport_print_data(
    const Included *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData 
IncludedPlugin_on_participant_attached(
    void *registration_data, 
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration, 
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void 
IncludedPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData 
IncludedPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration, 
    void *container_plugin_context);

NDDSUSERDllExport extern void 
IncludedPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void    
IncludedPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    Included *sample,
    void *handle);    

NDDSUSERDllExport extern RTIBool 
IncludedPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    Included *out,
    const Included *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
IncludedPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const Included *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr()); 

NDDSUSERDllExport extern RTIBool 
IncludedPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    Included **sample, 
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample, 
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
IncludedPlugin_deserialize_from_cdr_buffer(
    Included *sample,
    const char * buffer,
    unsigned int length);    

NDDSUSERDllExport extern unsigned int 
IncludedPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind 
IncludedPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int 
IncludedPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int 
IncludedPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool 
IncludedPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    Included ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
IncludedPlugin_new(void);

NDDSUSERDllExport extern void
IncludedPlugin_delete(struct PRESTypePlugin *);

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* autoid_hash_includePlugin_2081427871_h */

