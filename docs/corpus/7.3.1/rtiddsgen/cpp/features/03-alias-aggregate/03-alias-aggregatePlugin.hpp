

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-alias-aggregate.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _03_alias_aggregatePlugin_937154352_h
#define _03_alias_aggregatePlugin_937154352_h

#include "03-alias-aggregate.hpp"

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

namespace CorpusAggregateAliases {

    #define PointPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define PointPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define PointPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define PointPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define PointPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Point*
    PointPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Point*
    PointPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Point*
    PointPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    PointPluginSupport_copy_data(
        Point *out,
        const Point *in);

    NDDSUSERDllExport extern void 
    PointPluginSupport_destroy_data_w_params(
        Point *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    PointPluginSupport_destroy_data_ex(
        Point *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    PointPluginSupport_destroy_data(
        Point *sample);

    NDDSUSERDllExport extern void 
    PointPluginSupport_print_data(
        const Point *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    PointPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    PointPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    PointPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    PointPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    PointPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Point *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    PointPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Point *out,
        const Point *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    PointPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Point *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    PointPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Point **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    PointPlugin_deserialize_from_cdr_buffer(
        Point *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    PointPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    PointPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    PointPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    PointPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    PointPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Point ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    PointPlugin_new(void);

    NDDSUSERDllExport extern void
    PointPlugin_delete(struct PRESTypePlugin *);

    #define PointAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define PointAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define PointAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define PointAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define PointAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PointAlias*
    PointAliasPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern PointAlias*
    PointAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern PointAlias*
    PointAliasPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    PointAliasPluginSupport_copy_data(
        PointAlias *out,
        const PointAlias *in);

    NDDSUSERDllExport extern void 
    PointAliasPluginSupport_destroy_data_w_params(
        PointAlias *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    PointAliasPluginSupport_destroy_data_ex(
        PointAlias *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    PointAliasPluginSupport_destroy_data(
        PointAlias *sample);

    NDDSUSERDllExport extern void 
    PointAliasPluginSupport_print_data(
        const PointAlias *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    PointAliasPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        PointAlias *out,
        const PointAlias *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    PointAliasPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    PointAliasPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    PointAliasPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    PointAliasPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define PointAlias2Plugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define PointAlias2Plugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define PointAlias2Plugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define PointAlias2Plugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define PointAlias2Plugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PointAlias2*
    PointAlias2PluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern PointAlias2*
    PointAlias2PluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern PointAlias2*
    PointAlias2PluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    PointAlias2PluginSupport_copy_data(
        PointAlias2 *out,
        const PointAlias2 *in);

    NDDSUSERDllExport extern void 
    PointAlias2PluginSupport_destroy_data_w_params(
        PointAlias2 *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    PointAlias2PluginSupport_destroy_data_ex(
        PointAlias2 *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    PointAlias2PluginSupport_destroy_data(
        PointAlias2 *sample);

    NDDSUSERDllExport extern void 
    PointAlias2PluginSupport_print_data(
        const PointAlias2 *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    PointAlias2Plugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        PointAlias2 *out,
        const PointAlias2 *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    PointAlias2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    PointAlias2Plugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    PointAlias2Plugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    PointAlias2Plugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define SamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define SamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define SamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define SamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define SamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    SamplePluginSupport_copy_data(
        Sample *out,
        const Sample *in);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_data_w_params(
        Sample *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_data_ex(
        Sample *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_data(
        Sample *sample);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_print_data(
        const Sample *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    SamplePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    SamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    SamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    SamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    SamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *out,
        const Sample *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Sample *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Sample **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_deserialize_from_cdr_buffer(
        Sample *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    SamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    SamplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    SamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    SamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Sample ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    SamplePlugin_new(void);

    NDDSUSERDllExport extern void
    SamplePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusAggregateAliases  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _03_alias_aggregatePlugin_937154352_h */

