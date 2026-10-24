

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-unbounded-sequence-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _05_unbounded_sequence_aliasesPlugin_114743980_h
#define _05_unbounded_sequence_aliasesPlugin_114743980_h

#include "05-unbounded-sequence-aliases.hpp"

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

namespace CorpusUnboundedSequences {

    #define NumbersPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NumbersPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define NumbersPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NumbersPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define NumbersPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Numbers*
    NumbersPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Numbers*
    NumbersPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Numbers*
    NumbersPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    NumbersPluginSupport_copy_data(
        Numbers *out,
        const Numbers *in);

    NDDSUSERDllExport extern void 
    NumbersPluginSupport_destroy_data_w_params(
        Numbers *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    NumbersPluginSupport_destroy_data_ex(
        Numbers *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    NumbersPluginSupport_destroy_data(
        Numbers *sample);

    NDDSUSERDllExport extern void 
    NumbersPluginSupport_print_data(
        const Numbers *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    NumbersPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Numbers *out,
        const Numbers *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    NumbersPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    NumbersPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    NumbersPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    NumbersPlugin_get_serialized_key_max_size_for_keyhash(
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

    #define ChildPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChildPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ChildPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChildPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ChildPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Child*
    ChildPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Child*
    ChildPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Child*
    ChildPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ChildPluginSupport_copy_data(
        Child *out,
        const Child *in);

    NDDSUSERDllExport extern void 
    ChildPluginSupport_destroy_data_w_params(
        Child *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ChildPluginSupport_destroy_data_ex(
        Child *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ChildPluginSupport_destroy_data(
        Child *sample);

    NDDSUSERDllExport extern void 
    ChildPluginSupport_print_data(
        const Child *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    ChildPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    ChildPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    ChildPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    ChildPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    ChildPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Child *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    ChildPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Child *out,
        const Child *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ChildPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Child *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    ChildPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Child **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ChildPlugin_deserialize_from_cdr_buffer(
        Child *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    ChildPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ChildPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ChildPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ChildPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    ChildPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Child ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ChildPlugin_new(void);

    NDDSUSERDllExport extern void
    ChildPlugin_delete(struct PRESTypePlugin *);

    #define ScalarPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ScalarPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ScalarPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ScalarPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ScalarPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Scalar*
    ScalarPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Scalar*
    ScalarPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Scalar*
    ScalarPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ScalarPluginSupport_copy_data(
        Scalar *out,
        const Scalar *in);

    NDDSUSERDllExport extern void 
    ScalarPluginSupport_destroy_data_w_params(
        Scalar *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ScalarPluginSupport_destroy_data_ex(
        Scalar *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ScalarPluginSupport_destroy_data(
        Scalar *sample);

    NDDSUSERDllExport extern void 
    ScalarPluginSupport_print_data(
        const Scalar *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    ScalarPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Scalar *out,
        const Scalar *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    ScalarPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ScalarPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ScalarPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ScalarPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define StatesPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define StatesPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define StatesPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define StatesPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define StatesPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern States*
    StatesPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern States*
    StatesPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern States*
    StatesPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    StatesPluginSupport_copy_data(
        States *out,
        const States *in);

    NDDSUSERDllExport extern void 
    StatesPluginSupport_destroy_data_w_params(
        States *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    StatesPluginSupport_destroy_data_ex(
        States *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    StatesPluginSupport_destroy_data(
        States *sample);

    NDDSUSERDllExport extern void 
    StatesPluginSupport_print_data(
        const States *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    StatesPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        States *out,
        const States *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    StatesPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    StatesPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    StatesPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    StatesPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ChildrenPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChildrenPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ChildrenPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChildrenPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ChildrenPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Children*
    ChildrenPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Children*
    ChildrenPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Children*
    ChildrenPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ChildrenPluginSupport_copy_data(
        Children *out,
        const Children *in);

    NDDSUSERDllExport extern void 
    ChildrenPluginSupport_destroy_data_w_params(
        Children *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ChildrenPluginSupport_destroy_data_ex(
        Children *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ChildrenPluginSupport_destroy_data(
        Children *sample);

    NDDSUSERDllExport extern void 
    ChildrenPluginSupport_print_data(
        const Children *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    ChildrenPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Children *out,
        const Children *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    ChildrenPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ChildrenPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ChildrenPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ChildrenPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ScalarsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ScalarsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ScalarsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ScalarsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ScalarsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Scalars*
    ScalarsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Scalars*
    ScalarsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Scalars*
    ScalarsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ScalarsPluginSupport_copy_data(
        Scalars *out,
        const Scalars *in);

    NDDSUSERDllExport extern void 
    ScalarsPluginSupport_destroy_data_w_params(
        Scalars *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ScalarsPluginSupport_destroy_data_ex(
        Scalars *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ScalarsPluginSupport_destroy_data(
        Scalars *sample);

    NDDSUSERDllExport extern void 
    ScalarsPluginSupport_print_data(
        const Scalars *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    ScalarsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Scalars *out,
        const Scalars *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    ScalarsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ScalarsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ScalarsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ScalarsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define TextsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define TextsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define TextsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define TextsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define TextsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Texts*
    TextsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Texts*
    TextsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Texts*
    TextsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    TextsPluginSupport_copy_data(
        Texts *out,
        const Texts *in);

    NDDSUSERDllExport extern void 
    TextsPluginSupport_destroy_data_w_params(
        Texts *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    TextsPluginSupport_destroy_data_ex(
        Texts *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    TextsPluginSupport_destroy_data(
        Texts *sample);

    NDDSUSERDllExport extern void 
    TextsPluginSupport_print_data(
        const Texts *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    TextsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Texts *out,
        const Texts *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    TextsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    TextsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    TextsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    TextsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define WideTextsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define WideTextsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define WideTextsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define WideTextsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define WideTextsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern WideTexts*
    WideTextsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern WideTexts*
    WideTextsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern WideTexts*
    WideTextsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    WideTextsPluginSupport_copy_data(
        WideTexts *out,
        const WideTexts *in);

    NDDSUSERDllExport extern void 
    WideTextsPluginSupport_destroy_data_w_params(
        WideTexts *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    WideTextsPluginSupport_destroy_data_ex(
        WideTexts *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    WideTextsPluginSupport_destroy_data(
        WideTexts *sample);

    NDDSUSERDllExport extern void 
    WideTextsPluginSupport_print_data(
        const WideTexts *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    WideTextsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        WideTexts *out,
        const WideTexts *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    WideTextsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    WideTextsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    WideTextsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    WideTextsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedTextsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedTextsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedTextsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedTextsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedTextsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedTexts*
    BoundedTextsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedTexts*
    BoundedTextsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedTexts*
    BoundedTextsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedTextsPluginSupport_copy_data(
        BoundedTexts *out,
        const BoundedTexts *in);

    NDDSUSERDllExport extern void 
    BoundedTextsPluginSupport_destroy_data_w_params(
        BoundedTexts *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedTextsPluginSupport_destroy_data_ex(
        BoundedTexts *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedTextsPluginSupport_destroy_data(
        BoundedTexts *sample);

    NDDSUSERDllExport extern void 
    BoundedTextsPluginSupport_print_data(
        const BoundedTexts *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedTextsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedTexts *out,
        const BoundedTexts *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedTextsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedTextsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedTextsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedTextsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedWideTextsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedWideTextsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedWideTextsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedWideTextsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedWideTextsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedWideTexts*
    BoundedWideTextsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedWideTexts*
    BoundedWideTextsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedWideTexts*
    BoundedWideTextsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextsPluginSupport_copy_data(
        BoundedWideTexts *out,
        const BoundedWideTexts *in);

    NDDSUSERDllExport extern void 
    BoundedWideTextsPluginSupport_destroy_data_w_params(
        BoundedWideTexts *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedWideTextsPluginSupport_destroy_data_ex(
        BoundedWideTexts *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedWideTextsPluginSupport_destroy_data(
        BoundedWideTexts *sample);

    NDDSUSERDllExport extern void 
    BoundedWideTextsPluginSupport_print_data(
        const BoundedWideTexts *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedWideTexts *out,
        const BoundedWideTexts *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedWideTextsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define NumbersChainPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NumbersChainPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define NumbersChainPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NumbersChainPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define NumbersChainPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NumbersChain*
    NumbersChainPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NumbersChain*
    NumbersChainPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NumbersChain*
    NumbersChainPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    NumbersChainPluginSupport_copy_data(
        NumbersChain *out,
        const NumbersChain *in);

    NDDSUSERDllExport extern void 
    NumbersChainPluginSupport_destroy_data_w_params(
        NumbersChain *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    NumbersChainPluginSupport_destroy_data_ex(
        NumbersChain *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    NumbersChainPluginSupport_destroy_data(
        NumbersChain *sample);

    NDDSUSERDllExport extern void 
    NumbersChainPluginSupport_print_data(
        const NumbersChain *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    NumbersChainPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NumbersChain *out,
        const NumbersChain *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    NumbersChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    NumbersChainPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    NumbersChainPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    NumbersChainPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

} /* namespace CorpusUnboundedSequences  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _05_unbounded_sequence_aliasesPlugin_114743980_h */

