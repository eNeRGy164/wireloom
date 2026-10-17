

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-unknown-annotation.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_unknown_annotationPlugin_1836872915_h
#define _12_unknown_annotationPlugin_1836872915_h

#include "12-unknown-annotation.hpp"

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

namespace CorpusNegativeIgnored {

    #define UnknownPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define UnknownPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define UnknownPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define UnknownPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define UnknownPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Unknown*
    UnknownPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Unknown*
    UnknownPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Unknown*
    UnknownPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    UnknownPluginSupport_copy_data(
        Unknown *out,
        const Unknown *in);

    NDDSUSERDllExport extern void 
    UnknownPluginSupport_destroy_data_w_params(
        Unknown *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    UnknownPluginSupport_destroy_data_ex(
        Unknown *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    UnknownPluginSupport_destroy_data(
        Unknown *sample);

    NDDSUSERDllExport extern void 
    UnknownPluginSupport_print_data(
        const Unknown *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    UnknownPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    UnknownPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    UnknownPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    UnknownPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    UnknownPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Unknown *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    UnknownPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Unknown *out,
        const Unknown *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    UnknownPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Unknown *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    UnknownPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Unknown **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    UnknownPlugin_deserialize_from_cdr_buffer(
        Unknown *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    UnknownPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    UnknownPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    UnknownPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    UnknownPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    UnknownPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Unknown ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    UnknownPlugin_new(void);

    NDDSUSERDllExport extern void
    UnknownPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusNegativeIgnored  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _12_unknown_annotationPlugin_1836872915_h */

