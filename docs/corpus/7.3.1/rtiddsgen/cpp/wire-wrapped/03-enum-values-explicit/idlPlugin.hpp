

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idlPlugin_1662735992_h
#define idlPlugin_1662735992_h

#include "idl.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "features/03-enum-values-explicitPlugin.hpp"

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

namespace CorpusEnumValues {

    #define EnumTopicPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define EnumTopicPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define EnumTopicPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define EnumTopicPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define EnumTopicPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern EnumTopic*
    EnumTopicPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern EnumTopic*
    EnumTopicPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern EnumTopic*
    EnumTopicPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    EnumTopicPluginSupport_copy_data(
        EnumTopic *out,
        const EnumTopic *in);

    NDDSUSERDllExport extern void 
    EnumTopicPluginSupport_destroy_data_w_params(
        EnumTopic *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    EnumTopicPluginSupport_destroy_data_ex(
        EnumTopic *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    EnumTopicPluginSupport_destroy_data(
        EnumTopic *sample);

    NDDSUSERDllExport extern void 
    EnumTopicPluginSupport_print_data(
        const EnumTopic *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    EnumTopicPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    EnumTopicPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    EnumTopicPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    EnumTopicPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    EnumTopicPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        EnumTopic *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    EnumTopicPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        EnumTopic *out,
        const EnumTopic *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    EnumTopicPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const EnumTopic *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    EnumTopicPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        EnumTopic **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    EnumTopicPlugin_deserialize_from_cdr_buffer(
        EnumTopic *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    EnumTopicPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    EnumTopicPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    EnumTopicPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    EnumTopicPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    EnumTopicPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        EnumTopic ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    EnumTopicPlugin_new(void);

    NDDSUSERDllExport extern void
    EnumTopicPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusEnumValues  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* idlPlugin_1662735992_h */

