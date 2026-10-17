

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-aggregates.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _06_aggregatesPlugin_2110920171_h
#define _06_aggregatesPlugin_2110920171_h

#include "06-aggregates.hpp"

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

namespace CorpusAggregates {

    #define BasePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BasePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BasePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BasePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BasePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BasePluginSupport_copy_data(
        Base *out,
        const Base *in);

    NDDSUSERDllExport extern void 
    BasePluginSupport_destroy_data_w_params(
        Base *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BasePluginSupport_destroy_data_ex(
        Base *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BasePluginSupport_destroy_data(
        Base *sample);

    NDDSUSERDllExport extern void 
    BasePluginSupport_print_data(
        const Base *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    BasePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    BasePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    BasePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    BasePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    BasePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Base *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    BasePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Base *out,
        const Base *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    BasePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Base *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    BasePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Base **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    BasePlugin_deserialize_from_cdr_buffer(
        Base *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    BasePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BasePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BasePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BasePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    BasePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Base ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    BasePlugin_new(void);

    NDDSUSERDllExport extern void
    BasePlugin_delete(struct PRESTypePlugin *);

    #define DerivedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define DerivedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define DerivedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define DerivedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define DerivedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    DerivedPluginSupport_copy_data(
        Derived *out,
        const Derived *in);

    NDDSUSERDllExport extern void 
    DerivedPluginSupport_destroy_data_w_params(
        Derived *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    DerivedPluginSupport_destroy_data_ex(
        Derived *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    DerivedPluginSupport_destroy_data(
        Derived *sample);

    NDDSUSERDllExport extern void 
    DerivedPluginSupport_print_data(
        const Derived *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    DerivedPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    DerivedPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    DerivedPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    DerivedPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    DerivedPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Derived *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    DerivedPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Derived *out,
        const Derived *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Derived *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    DerivedPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Derived **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_deserialize_from_cdr_buffer(
        Derived *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    DerivedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    DerivedPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    DerivedPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    DerivedPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    DerivedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Derived ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    DerivedPlugin_new(void);

    NDDSUSERDllExport extern void
    DerivedPlugin_delete(struct PRESTypePlugin *);

    #define RecursivePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define RecursivePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define RecursivePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define RecursivePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define RecursivePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Recursive*
    RecursivePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Recursive*
    RecursivePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Recursive*
    RecursivePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    RecursivePluginSupport_copy_data(
        Recursive *out,
        const Recursive *in);

    NDDSUSERDllExport extern void 
    RecursivePluginSupport_destroy_data_w_params(
        Recursive *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    RecursivePluginSupport_destroy_data_ex(
        Recursive *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    RecursivePluginSupport_destroy_data(
        Recursive *sample);

    NDDSUSERDllExport extern void 
    RecursivePluginSupport_print_data(
        const Recursive *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    RecursivePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    RecursivePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    RecursivePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    RecursivePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    RecursivePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Recursive *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    RecursivePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Recursive *out,
        const Recursive *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    RecursivePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Recursive *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    RecursivePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Recursive **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    RecursivePlugin_deserialize_from_cdr_buffer(
        Recursive *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    RecursivePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    RecursivePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    RecursivePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    RecursivePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    RecursivePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Recursive ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    RecursivePlugin_new(void);

    NDDSUSERDllExport extern void
    RecursivePlugin_delete(struct PRESTypePlugin *);

    #define ComposedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ComposedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ComposedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ComposedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ComposedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Composed*
    ComposedPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Composed*
    ComposedPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Composed*
    ComposedPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ComposedPluginSupport_copy_data(
        Composed *out,
        const Composed *in);

    NDDSUSERDllExport extern void 
    ComposedPluginSupport_destroy_data_w_params(
        Composed *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ComposedPluginSupport_destroy_data_ex(
        Composed *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ComposedPluginSupport_destroy_data(
        Composed *sample);

    NDDSUSERDllExport extern void 
    ComposedPluginSupport_print_data(
        const Composed *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    ComposedPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    ComposedPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    ComposedPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    ComposedPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    ComposedPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Composed *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    ComposedPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Composed *out,
        const Composed *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ComposedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Composed *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    ComposedPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Composed **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ComposedPlugin_deserialize_from_cdr_buffer(
        Composed *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    ComposedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ComposedPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ComposedPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ComposedPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    ComposedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Composed ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ComposedPlugin_new(void);

    NDDSUSERDllExport extern void
    ComposedPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusAggregates  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _06_aggregatesPlugin_2110920171_h */

