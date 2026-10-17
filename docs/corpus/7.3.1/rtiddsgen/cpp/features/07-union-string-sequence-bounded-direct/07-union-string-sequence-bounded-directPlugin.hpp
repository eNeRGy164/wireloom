

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-bounded-direct.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_string_sequence_bounded_directPlugin_586090969_h
#define _07_union_string_sequence_bounded_directPlugin_586090969_h

#include "07-union-string-sequence-bounded-direct.hpp"

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

namespace RuntimeUnionBoundedStringSequences {

    #define DirectBoundedStringSequencesPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define DirectBoundedStringSequencesPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define DirectBoundedStringSequencesPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define DirectBoundedStringSequencesPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define DirectBoundedStringSequencesPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern DirectBoundedStringSequences*
    DirectBoundedStringSequencesPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern DirectBoundedStringSequences*
    DirectBoundedStringSequencesPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern DirectBoundedStringSequences*
    DirectBoundedStringSequencesPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    DirectBoundedStringSequencesPluginSupport_copy_data(
        DirectBoundedStringSequences *out,
        const DirectBoundedStringSequences *in);

    NDDSUSERDllExport extern void 
    DirectBoundedStringSequencesPluginSupport_destroy_data_w_params(
        DirectBoundedStringSequences *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    DirectBoundedStringSequencesPluginSupport_destroy_data_ex(
        DirectBoundedStringSequences *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    DirectBoundedStringSequencesPluginSupport_destroy_data(
        DirectBoundedStringSequences *sample);

    NDDSUSERDllExport extern void 
    DirectBoundedStringSequencesPluginSupport_print_data(
        const DirectBoundedStringSequences *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    DirectBoundedStringSequencesPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    DirectBoundedStringSequencesPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    DirectBoundedStringSequencesPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    DirectBoundedStringSequencesPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    DirectBoundedStringSequencesPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        DirectBoundedStringSequences *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    DirectBoundedStringSequencesPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        DirectBoundedStringSequences *out,
        const DirectBoundedStringSequences *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    DirectBoundedStringSequencesPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const DirectBoundedStringSequences *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    DirectBoundedStringSequencesPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        DirectBoundedStringSequences **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    DirectBoundedStringSequencesPlugin_deserialize_from_cdr_buffer(
        DirectBoundedStringSequences *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    DirectBoundedStringSequencesPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    DirectBoundedStringSequencesPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    DirectBoundedStringSequencesPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    DirectBoundedStringSequencesPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    DirectBoundedStringSequencesPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        DirectBoundedStringSequences ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    DirectBoundedStringSequencesPlugin_new(void);

    NDDSUSERDllExport extern void
    DirectBoundedStringSequencesPlugin_delete(struct PRESTypePlugin *);

} /* namespace RuntimeUnionBoundedStringSequences  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _07_union_string_sequence_bounded_directPlugin_586090969_h */

