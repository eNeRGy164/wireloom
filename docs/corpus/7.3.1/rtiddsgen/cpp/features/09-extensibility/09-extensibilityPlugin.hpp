

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-extensibility.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_extensibilityPlugin_747423966_h
#define _09_extensibilityPlugin_747423966_h

#include "09-extensibility.hpp"

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

namespace CorpusExtensibility {

    #define AppendablePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define AppendablePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define AppendablePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define AppendablePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define AppendablePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Appendable*
    AppendablePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Appendable*
    AppendablePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Appendable*
    AppendablePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    AppendablePluginSupport_copy_data(
        Appendable *out,
        const Appendable *in);

    NDDSUSERDllExport extern void 
    AppendablePluginSupport_destroy_data_w_params(
        Appendable *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    AppendablePluginSupport_destroy_data_ex(
        Appendable *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    AppendablePluginSupport_destroy_data(
        Appendable *sample);

    NDDSUSERDllExport extern void 
    AppendablePluginSupport_print_data(
        const Appendable *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    AppendablePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    AppendablePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    AppendablePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    AppendablePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    AppendablePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Appendable *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    AppendablePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Appendable *out,
        const Appendable *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    AppendablePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Appendable *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    AppendablePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Appendable **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    AppendablePlugin_deserialize_from_cdr_buffer(
        Appendable *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    AppendablePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    AppendablePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    AppendablePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    AppendablePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    AppendablePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Appendable ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    AppendablePlugin_new(void);

    NDDSUSERDllExport extern void
    AppendablePlugin_delete(struct PRESTypePlugin *);

    #define FinalPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define FinalPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define FinalPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define FinalPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define FinalPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Final*
    FinalPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Final*
    FinalPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Final*
    FinalPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    FinalPluginSupport_copy_data(
        Final *out,
        const Final *in);

    NDDSUSERDllExport extern void 
    FinalPluginSupport_destroy_data_w_params(
        Final *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    FinalPluginSupport_destroy_data_ex(
        Final *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    FinalPluginSupport_destroy_data(
        Final *sample);

    NDDSUSERDllExport extern void 
    FinalPluginSupport_print_data(
        const Final *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    FinalPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    FinalPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    FinalPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    FinalPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    FinalPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Final *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    FinalPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Final *out,
        const Final *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    FinalPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Final *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    FinalPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Final **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    FinalPlugin_deserialize_from_cdr_buffer(
        Final *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    FinalPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    FinalPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    FinalPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    FinalPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    FinalPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Final ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    FinalPlugin_new(void);

    NDDSUSERDllExport extern void
    FinalPlugin_delete(struct PRESTypePlugin *);

    #define MutablePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define MutablePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define MutablePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define MutablePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define MutablePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Mutable*
    MutablePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Mutable*
    MutablePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Mutable*
    MutablePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    MutablePluginSupport_copy_data(
        Mutable *out,
        const Mutable *in);

    NDDSUSERDllExport extern void 
    MutablePluginSupport_destroy_data_w_params(
        Mutable *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    MutablePluginSupport_destroy_data_ex(
        Mutable *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    MutablePluginSupport_destroy_data(
        Mutable *sample);

    NDDSUSERDllExport extern void 
    MutablePluginSupport_print_data(
        const Mutable *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    MutablePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    MutablePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    MutablePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    MutablePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    MutablePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Mutable *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    MutablePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Mutable *out,
        const Mutable *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    MutablePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Mutable *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    MutablePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Mutable **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    MutablePlugin_deserialize_from_cdr_buffer(
        Mutable *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    MutablePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    MutablePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    MutablePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    MutablePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    MutablePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Mutable ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    MutablePlugin_new(void);

    NDDSUSERDllExport extern void
    MutablePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusExtensibility  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _09_extensibilityPlugin_747423966_h */

