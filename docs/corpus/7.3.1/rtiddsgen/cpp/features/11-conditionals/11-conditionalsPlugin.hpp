

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-conditionals.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _11_conditionalsPlugin_493459758_h
#define _11_conditionalsPlugin_493459758_h

#include "11-conditionals.hpp"

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

namespace CorpusConditionals {

    #define EnabledPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define EnabledPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define EnabledPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define EnabledPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define EnabledPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Enabled*
    EnabledPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Enabled*
    EnabledPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Enabled*
    EnabledPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    EnabledPluginSupport_copy_data(
        Enabled *out,
        const Enabled *in);

    NDDSUSERDllExport extern void 
    EnabledPluginSupport_destroy_data_w_params(
        Enabled *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    EnabledPluginSupport_destroy_data_ex(
        Enabled *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    EnabledPluginSupport_destroy_data(
        Enabled *sample);

    NDDSUSERDllExport extern void 
    EnabledPluginSupport_print_data(
        const Enabled *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    EnabledPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    EnabledPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    EnabledPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    EnabledPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    EnabledPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Enabled *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    EnabledPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Enabled *out,
        const Enabled *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    EnabledPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Enabled *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    EnabledPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Enabled **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    EnabledPlugin_deserialize_from_cdr_buffer(
        Enabled *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    EnabledPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    EnabledPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    EnabledPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    EnabledPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    EnabledPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Enabled ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    EnabledPlugin_new(void);

    NDDSUSERDllExport extern void
    EnabledPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusConditionals  */
namespace CorpusConditionals {

    #define UndefBranchPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define UndefBranchPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define UndefBranchPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define UndefBranchPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define UndefBranchPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern UndefBranch*
    UndefBranchPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern UndefBranch*
    UndefBranchPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern UndefBranch*
    UndefBranchPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    UndefBranchPluginSupport_copy_data(
        UndefBranch *out,
        const UndefBranch *in);

    NDDSUSERDllExport extern void 
    UndefBranchPluginSupport_destroy_data_w_params(
        UndefBranch *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    UndefBranchPluginSupport_destroy_data_ex(
        UndefBranch *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    UndefBranchPluginSupport_destroy_data(
        UndefBranch *sample);

    NDDSUSERDllExport extern void 
    UndefBranchPluginSupport_print_data(
        const UndefBranch *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    UndefBranchPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    UndefBranchPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    UndefBranchPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    UndefBranchPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    UndefBranchPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        UndefBranch *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    UndefBranchPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        UndefBranch *out,
        const UndefBranch *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    UndefBranchPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const UndefBranch *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    UndefBranchPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        UndefBranch **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    UndefBranchPlugin_deserialize_from_cdr_buffer(
        UndefBranch *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    UndefBranchPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    UndefBranchPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    UndefBranchPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    UndefBranchPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    UndefBranchPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        UndefBranch ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    UndefBranchPlugin_new(void);

    NDDSUSERDllExport extern void
    UndefBranchPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusConditionals  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _11_conditionalsPlugin_493459758_h */

