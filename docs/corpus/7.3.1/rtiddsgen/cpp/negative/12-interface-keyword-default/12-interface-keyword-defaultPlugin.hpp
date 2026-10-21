

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-keyword-default.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_interface_keyword_defaultPlugin_241455650_h
#define _12_interface_keyword_defaultPlugin_241455650_h

#include "12-interface-keyword-default.hpp"

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

namespace CorpusInterfaceKeyword {

    #define Service_Context_InPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define Service_Context_InPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define Service_Context_InPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define Service_Context_InPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define Service_Context_InPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Service_Context_In*
    Service_Context_InPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Service_Context_In*
    Service_Context_InPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Service_Context_In*
    Service_Context_InPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    Service_Context_InPluginSupport_copy_data(
        Service_Context_In *out,
        const Service_Context_In *in);

    NDDSUSERDllExport extern void 
    Service_Context_InPluginSupport_destroy_data_w_params(
        Service_Context_In *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    Service_Context_InPluginSupport_destroy_data_ex(
        Service_Context_In *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    Service_Context_InPluginSupport_destroy_data(
        Service_Context_In *sample);

    NDDSUSERDllExport extern void 
    Service_Context_InPluginSupport_print_data(
        const Service_Context_In *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    Service_Context_InPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_In *out,
        const Service_Context_In *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    Service_Context_InPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Service_Context_In *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    Service_Context_InPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_In **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    Service_Context_InPlugin_deserialize_from_cdr_buffer(
        Service_Context_In *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    Service_Context_InPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    Service_Context_InPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    Service_Context_InPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    Service_Context_InPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    Service_Context_InPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_In ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    #define Service_Context_OutPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define Service_Context_OutPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define Service_Context_OutPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define Service_Context_OutPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define Service_Context_OutPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Service_Context_Out*
    Service_Context_OutPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Service_Context_Out*
    Service_Context_OutPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Service_Context_Out*
    Service_Context_OutPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    Service_Context_OutPluginSupport_copy_data(
        Service_Context_Out *out,
        const Service_Context_Out *in);

    NDDSUSERDllExport extern void 
    Service_Context_OutPluginSupport_destroy_data_w_params(
        Service_Context_Out *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    Service_Context_OutPluginSupport_destroy_data_ex(
        Service_Context_Out *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    Service_Context_OutPluginSupport_destroy_data(
        Service_Context_Out *sample);

    NDDSUSERDllExport extern void 
    Service_Context_OutPluginSupport_print_data(
        const Service_Context_Out *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    Service_Context_OutPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_Out *out,
        const Service_Context_Out *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    Service_Context_OutPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Service_Context_Out *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    Service_Context_OutPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_Out **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    Service_Context_OutPlugin_deserialize_from_cdr_buffer(
        Service_Context_Out *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    Service_Context_OutPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    Service_Context_OutPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    Service_Context_OutPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    Service_Context_OutPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    Service_Context_OutPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_Out ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    #define Service_Context_ResultPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define Service_Context_ResultPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define Service_Context_ResultPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define Service_Context_ResultPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define Service_Context_ResultPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Service_Context_Result*
    Service_Context_ResultPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Service_Context_Result*
    Service_Context_ResultPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Service_Context_Result*
    Service_Context_ResultPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    Service_Context_ResultPluginSupport_copy_data(
        Service_Context_Result *out,
        const Service_Context_Result *in);

    NDDSUSERDllExport extern void 
    Service_Context_ResultPluginSupport_destroy_data_w_params(
        Service_Context_Result *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    Service_Context_ResultPluginSupport_destroy_data_ex(
        Service_Context_Result *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    Service_Context_ResultPluginSupport_destroy_data(
        Service_Context_Result *sample);

    NDDSUSERDllExport extern void 
    Service_Context_ResultPluginSupport_print_data(
        const Service_Context_Result *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    Service_Context_ResultPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_Result *out,
        const Service_Context_Result *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    Service_Context_ResultPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Service_Context_Result *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    Service_Context_ResultPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_Result **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    Service_Context_ResultPlugin_deserialize_from_cdr_buffer(
        Service_Context_Result *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    Service_Context_ResultPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    Service_Context_ResultPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    Service_Context_ResultPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    Service_Context_ResultPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    Service_Context_ResultPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Service_Context_Result ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    #define Service_CallPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define Service_CallPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define Service_CallPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define Service_CallPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define Service_CallPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Service_Call*
    Service_CallPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Service_Call*
    Service_CallPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Service_Call*
    Service_CallPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    Service_CallPluginSupport_copy_data(
        Service_Call *out,
        const Service_Call *in);

    NDDSUSERDllExport extern void 
    Service_CallPluginSupport_destroy_data_w_params(
        Service_Call *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    Service_CallPluginSupport_destroy_data_ex(
        Service_Call *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    Service_CallPluginSupport_destroy_data(
        Service_Call *sample);

    NDDSUSERDllExport extern void 
    Service_CallPluginSupport_print_data(
        const Service_Call *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    Service_CallPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    Service_CallPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    Service_CallPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    Service_CallPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    Service_CallPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Call *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    Service_CallPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Call *out,
        const Service_Call *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    Service_CallPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Service_Call *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    Service_CallPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Service_Call **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    Service_CallPlugin_deserialize_from_cdr_buffer(
        Service_Call *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    Service_CallPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    Service_CallPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    Service_CallPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    Service_CallPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    Service_CallPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Service_Call ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    Service_CallPlugin_new(void);

    NDDSUSERDllExport extern void
    Service_CallPlugin_delete(struct PRESTypePlugin *);

    #define Service_ReturnPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define Service_ReturnPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define Service_ReturnPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define Service_ReturnPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define Service_ReturnPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Service_Return*
    Service_ReturnPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Service_Return*
    Service_ReturnPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Service_Return*
    Service_ReturnPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    Service_ReturnPluginSupport_copy_data(
        Service_Return *out,
        const Service_Return *in);

    NDDSUSERDllExport extern void 
    Service_ReturnPluginSupport_destroy_data_w_params(
        Service_Return *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    Service_ReturnPluginSupport_destroy_data_ex(
        Service_Return *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    Service_ReturnPluginSupport_destroy_data(
        Service_Return *sample);

    NDDSUSERDllExport extern void 
    Service_ReturnPluginSupport_print_data(
        const Service_Return *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    Service_ReturnPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    Service_ReturnPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    Service_ReturnPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    Service_ReturnPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    Service_ReturnPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Return *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    Service_ReturnPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Service_Return *out,
        const Service_Return *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    Service_ReturnPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Service_Return *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    Service_ReturnPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Service_Return **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    Service_ReturnPlugin_deserialize_from_cdr_buffer(
        Service_Return *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    Service_ReturnPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    Service_ReturnPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    Service_ReturnPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    Service_ReturnPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    Service_ReturnPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Service_Return ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    Service_ReturnPlugin_new(void);

    NDDSUSERDllExport extern void
    Service_ReturnPlugin_delete(struct PRESTypePlugin *);

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

} /* namespace CorpusInterfaceKeyword  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _12_interface_keyword_defaultPlugin_241455650_h */

