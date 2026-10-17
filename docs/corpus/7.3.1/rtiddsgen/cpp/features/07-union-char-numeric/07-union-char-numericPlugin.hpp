

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char-numeric.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_char_numericPlugin_1779107459_h
#define _07_union_char_numericPlugin_1779107459_h

#include "07-union-char-numeric.hpp"

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

namespace CorpusNumericCharUnion {

    #define ChoicePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChoicePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ChoicePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChoicePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ChoicePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Choice*
    ChoicePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Choice*
    ChoicePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Choice*
    ChoicePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ChoicePluginSupport_copy_data(
        Choice *out,
        const Choice *in);

    NDDSUSERDllExport extern void 
    ChoicePluginSupport_destroy_data_w_params(
        Choice *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ChoicePluginSupport_destroy_data_ex(
        Choice *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ChoicePluginSupport_destroy_data(
        Choice *sample);

    NDDSUSERDllExport extern void 
    ChoicePluginSupport_print_data(
        const Choice *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    ChoicePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    ChoicePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    ChoicePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    ChoicePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    ChoicePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Choice *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    ChoicePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Choice *out,
        const Choice *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Choice *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    ChoicePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Choice **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_deserialize_from_cdr_buffer(
        Choice *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    ChoicePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ChoicePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ChoicePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ChoicePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    ChoicePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Choice ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ChoicePlugin_new(void);

    NDDSUSERDllExport extern void
    ChoicePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusNumericCharUnion  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _07_union_char_numericPlugin_1779107459_h */

