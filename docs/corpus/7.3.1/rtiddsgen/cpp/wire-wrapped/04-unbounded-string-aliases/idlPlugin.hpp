

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idlPlugin_1662736032_h
#define idlPlugin_1662736032_h

#include "idl.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "features/04-unbounded-string-aliasesPlugin.hpp"

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

namespace CorpusUnboundedStrings {

    #define StringAliasesSamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define StringAliasesSamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define StringAliasesSamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define StringAliasesSamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define StringAliasesSamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern StringAliasesSample*
    StringAliasesSamplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern StringAliasesSample*
    StringAliasesSamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern StringAliasesSample*
    StringAliasesSamplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    StringAliasesSamplePluginSupport_copy_data(
        StringAliasesSample *out,
        const StringAliasesSample *in);

    NDDSUSERDllExport extern void 
    StringAliasesSamplePluginSupport_destroy_data_w_params(
        StringAliasesSample *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    StringAliasesSamplePluginSupport_destroy_data_ex(
        StringAliasesSample *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    StringAliasesSamplePluginSupport_destroy_data(
        StringAliasesSample *sample);

    NDDSUSERDllExport extern void 
    StringAliasesSamplePluginSupport_print_data(
        const StringAliasesSample *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    StringAliasesSamplePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    StringAliasesSamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    StringAliasesSamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    StringAliasesSamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    StringAliasesSamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        StringAliasesSample *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    StringAliasesSamplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        StringAliasesSample *out,
        const StringAliasesSample *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    StringAliasesSamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const StringAliasesSample *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    StringAliasesSamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        StringAliasesSample **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    StringAliasesSamplePlugin_deserialize_from_cdr_buffer(
        StringAliasesSample *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    StringAliasesSamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    StringAliasesSamplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    StringAliasesSamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    StringAliasesSamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    StringAliasesSamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        StringAliasesSample ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    StringAliasesSamplePlugin_new(void);

    NDDSUSERDllExport extern void
    StringAliasesSamplePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusUnboundedStrings  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* idlPlugin_1662736032_h */

