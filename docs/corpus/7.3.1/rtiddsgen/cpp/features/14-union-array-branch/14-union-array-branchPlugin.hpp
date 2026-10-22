

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-array-branch.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_union_array_branchPlugin_804243923_h
#define _14_union_array_branchPlugin_804243923_h

#include "14-union-array-branch.hpp"

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

#define ArrayBranchPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define ArrayBranchPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
#define ArrayBranchPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define ArrayBranchPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
#define ArrayBranchPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern ArrayBranch*
ArrayBranchPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern ArrayBranch*
ArrayBranchPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern ArrayBranch*
ArrayBranchPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool 
ArrayBranchPluginSupport_copy_data(
    ArrayBranch *out,
    const ArrayBranch *in);

NDDSUSERDllExport extern void 
ArrayBranchPluginSupport_destroy_data_w_params(
    ArrayBranch *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void 
ArrayBranchPluginSupport_destroy_data_ex(
    ArrayBranch *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void 
ArrayBranchPluginSupport_destroy_data(
    ArrayBranch *sample);

NDDSUSERDllExport extern void 
ArrayBranchPluginSupport_print_data(
    const ArrayBranch *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData 
ArrayBranchPlugin_on_participant_attached(
    void *registration_data, 
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration, 
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void 
ArrayBranchPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData 
ArrayBranchPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration, 
    void *container_plugin_context);

NDDSUSERDllExport extern void 
ArrayBranchPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void    
ArrayBranchPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    ArrayBranch *sample,
    void *handle);    

NDDSUSERDllExport extern RTIBool 
ArrayBranchPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    ArrayBranch *out,
    const ArrayBranch *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
ArrayBranchPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const ArrayBranch *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr()); 

NDDSUSERDllExport extern RTIBool 
ArrayBranchPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    ArrayBranch **sample, 
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample, 
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
ArrayBranchPlugin_deserialize_from_cdr_buffer(
    ArrayBranch *sample,
    const char * buffer,
    unsigned int length);    

NDDSUSERDllExport extern unsigned int 
ArrayBranchPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind 
ArrayBranchPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int 
ArrayBranchPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int 
ArrayBranchPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool 
ArrayBranchPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    ArrayBranch ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
ArrayBranchPlugin_new(void);

NDDSUSERDllExport extern void
ArrayBranchPlugin_delete(struct PRESTypePlugin *);

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_union_array_branchPlugin_804243923_h */

