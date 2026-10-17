

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_aliasesPlugin_1295009180_h
#define _07_union_aliasesPlugin_1295009180_h

#include "07-union-aliases.hpp"

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

namespace CorpusUnionAliases {

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    KindPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    KindPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    KindPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* ----------------------------------------------------------------------------
    Support functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern void
    KindPluginSupport_print_data(
        const Kind *sample,
        const char *desc,
        unsigned int indent_level);

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

    #define ChoiceAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChoiceAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ChoiceAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChoiceAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ChoiceAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ChoiceAlias*
    ChoiceAliasPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ChoiceAlias*
    ChoiceAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ChoiceAlias*
    ChoiceAliasPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ChoiceAliasPluginSupport_copy_data(
        ChoiceAlias *out,
        const ChoiceAlias *in);

    NDDSUSERDllExport extern void 
    ChoiceAliasPluginSupport_destroy_data_w_params(
        ChoiceAlias *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ChoiceAliasPluginSupport_destroy_data_ex(
        ChoiceAlias *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ChoiceAliasPluginSupport_destroy_data(
        ChoiceAlias *sample);

    NDDSUSERDllExport extern void 
    ChoiceAliasPluginSupport_print_data(
        const ChoiceAlias *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    ChoiceAliasPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ChoiceAlias *out,
        const ChoiceAlias *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    ChoiceAliasPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ChoiceAliasPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ChoiceAliasPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ChoiceAliasPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ChoiceAlias2Plugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChoiceAlias2Plugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ChoiceAlias2Plugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChoiceAlias2Plugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ChoiceAlias2Plugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ChoiceAlias2*
    ChoiceAlias2PluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ChoiceAlias2*
    ChoiceAlias2PluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ChoiceAlias2*
    ChoiceAlias2PluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ChoiceAlias2PluginSupport_copy_data(
        ChoiceAlias2 *out,
        const ChoiceAlias2 *in);

    NDDSUSERDllExport extern void 
    ChoiceAlias2PluginSupport_destroy_data_w_params(
        ChoiceAlias2 *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ChoiceAlias2PluginSupport_destroy_data_ex(
        ChoiceAlias2 *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ChoiceAlias2PluginSupport_destroy_data(
        ChoiceAlias2 *sample);

    NDDSUSERDllExport extern void 
    ChoiceAlias2PluginSupport_print_data(
        const ChoiceAlias2 *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    ChoiceAlias2Plugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ChoiceAlias2 *out,
        const ChoiceAlias2 *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    ChoiceAlias2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ChoiceAlias2Plugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ChoiceAlias2Plugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ChoiceAlias2Plugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define HolderPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define HolderPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define HolderPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define HolderPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define HolderPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Holder*
    HolderPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Holder*
    HolderPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Holder*
    HolderPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    HolderPluginSupport_copy_data(
        Holder *out,
        const Holder *in);

    NDDSUSERDllExport extern void 
    HolderPluginSupport_destroy_data_w_params(
        Holder *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    HolderPluginSupport_destroy_data_ex(
        Holder *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    HolderPluginSupport_destroy_data(
        Holder *sample);

    NDDSUSERDllExport extern void 
    HolderPluginSupport_print_data(
        const Holder *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    HolderPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    HolderPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    HolderPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    HolderPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    HolderPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Holder *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    HolderPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Holder *out,
        const Holder *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    HolderPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Holder *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    HolderPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Holder **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    HolderPlugin_deserialize_from_cdr_buffer(
        Holder *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    HolderPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    HolderPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    HolderPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    HolderPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    HolderPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Holder ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    HolderPlugin_new(void);

    NDDSUSERDllExport extern void
    HolderPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusUnionAliases  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _07_union_aliasesPlugin_1295009180_h */

