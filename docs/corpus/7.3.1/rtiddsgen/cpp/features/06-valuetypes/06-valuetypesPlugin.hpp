

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-valuetypes.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _06_valuetypesPlugin_932148422_h
#define _06_valuetypesPlugin_932148422_h

#include "06-valuetypes.hpp"

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

namespace CorpusValueTypes {

    #define BaseValuePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BaseValuePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BaseValuePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BaseValuePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BaseValuePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BaseValue*
    BaseValuePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BaseValue*
    BaseValuePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BaseValue*
    BaseValuePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BaseValuePluginSupport_copy_data(
        BaseValue *out,
        const BaseValue *in);

    NDDSUSERDllExport extern void 
    BaseValuePluginSupport_destroy_data_w_params(
        BaseValue *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BaseValuePluginSupport_destroy_data_ex(
        BaseValue *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BaseValuePluginSupport_destroy_data(
        BaseValue *sample);

    NDDSUSERDllExport extern void 
    BaseValuePluginSupport_print_data(
        const BaseValue *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    BaseValuePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    BaseValuePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    BaseValuePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    BaseValuePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    BaseValuePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        BaseValue *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    BaseValuePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BaseValue *out,
        const BaseValue *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    BaseValuePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const BaseValue *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    BaseValuePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        BaseValue **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    BaseValuePlugin_deserialize_from_cdr_buffer(
        BaseValue *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    BaseValuePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BaseValuePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BaseValuePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BaseValuePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    BaseValuePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        BaseValue ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    BaseValuePlugin_new(void);

    NDDSUSERDllExport extern void
    BaseValuePlugin_delete(struct PRESTypePlugin *);

    #define DerivedValuePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define DerivedValuePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define DerivedValuePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define DerivedValuePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define DerivedValuePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern DerivedValue*
    DerivedValuePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern DerivedValue*
    DerivedValuePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern DerivedValue*
    DerivedValuePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    DerivedValuePluginSupport_copy_data(
        DerivedValue *out,
        const DerivedValue *in);

    NDDSUSERDllExport extern void 
    DerivedValuePluginSupport_destroy_data_w_params(
        DerivedValue *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    DerivedValuePluginSupport_destroy_data_ex(
        DerivedValue *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    DerivedValuePluginSupport_destroy_data(
        DerivedValue *sample);

    NDDSUSERDllExport extern void 
    DerivedValuePluginSupport_print_data(
        const DerivedValue *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    DerivedValuePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    DerivedValuePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    DerivedValuePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    DerivedValuePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    DerivedValuePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        DerivedValue *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    DerivedValuePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        DerivedValue *out,
        const DerivedValue *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    DerivedValuePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const DerivedValue *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    DerivedValuePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        DerivedValue **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    DerivedValuePlugin_deserialize_from_cdr_buffer(
        DerivedValue *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    DerivedValuePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    DerivedValuePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    DerivedValuePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    DerivedValuePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    DerivedValuePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        DerivedValue ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    DerivedValuePlugin_new(void);

    NDDSUSERDllExport extern void
    DerivedValuePlugin_delete(struct PRESTypePlugin *);

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

} /* namespace CorpusValueTypes  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _06_valuetypesPlugin_932148422_h */

