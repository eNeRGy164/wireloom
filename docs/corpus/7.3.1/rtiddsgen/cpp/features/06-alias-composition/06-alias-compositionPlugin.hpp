

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-alias-composition.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _06_alias_compositionPlugin_39749492_h
#define _06_alias_compositionPlugin_39749492_h

#include "06-alias-composition.hpp"

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

namespace CorpusAggregateComposition {

    #define IdentifierPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define IdentifierPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define IdentifierPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define IdentifierPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define IdentifierPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Identifier*
    IdentifierPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Identifier*
    IdentifierPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Identifier*
    IdentifierPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    IdentifierPluginSupport_copy_data(
        Identifier *out,
        const Identifier *in);

    NDDSUSERDllExport extern void 
    IdentifierPluginSupport_destroy_data_w_params(
        Identifier *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    IdentifierPluginSupport_destroy_data_ex(
        Identifier *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    IdentifierPluginSupport_destroy_data(
        Identifier *sample);

    NDDSUSERDllExport extern void 
    IdentifierPluginSupport_print_data(
        const Identifier *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    IdentifierPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Identifier *out,
        const Identifier *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    IdentifierPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    IdentifierPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    IdentifierPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    IdentifierPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    StatePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    StatePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    StatePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* ----------------------------------------------------------------------------
    Support functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern void
    StatePluginSupport_print_data(
        const State *sample,
        const char *desc,
        unsigned int indent_level);

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

    #define NamedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NamedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define NamedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NamedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define NamedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Named*
    NamedPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Named*
    NamedPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Named*
    NamedPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    NamedPluginSupport_copy_data(
        Named *out,
        const Named *in);

    NDDSUSERDllExport extern void 
    NamedPluginSupport_destroy_data_w_params(
        Named *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    NamedPluginSupport_destroy_data_ex(
        Named *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    NamedPluginSupport_destroy_data(
        Named *sample);

    NDDSUSERDllExport extern void 
    NamedPluginSupport_print_data(
        const Named *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    NamedPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    NamedPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    NamedPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    NamedPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    NamedPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Named *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    NamedPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Named *out,
        const Named *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NamedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Named *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    NamedPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Named **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    NamedPlugin_deserialize_from_cdr_buffer(
        Named *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    NamedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    NamedPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    NamedPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    NamedPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    NamedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Named ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    NamedPlugin_new(void);

    NDDSUSERDllExport extern void
    NamedPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusAggregateComposition  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _06_alias_compositionPlugin_39749492_h */

