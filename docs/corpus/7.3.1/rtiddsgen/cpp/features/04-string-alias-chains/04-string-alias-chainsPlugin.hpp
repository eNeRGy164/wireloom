

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _04_string_alias_chainsPlugin_1820324921_h
#define _04_string_alias_chainsPlugin_1820324921_h

#include "04-string-alias-chains.hpp"

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

namespace CorpusStringAliasChains {

    #define NarrowTextPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NarrowTextPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define NarrowTextPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NarrowTextPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define NarrowTextPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NarrowText*
    NarrowTextPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NarrowText*
    NarrowTextPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NarrowText*
    NarrowTextPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    NarrowTextPluginSupport_copy_data(
        NarrowText *out,
        const NarrowText *in);

    NDDSUSERDllExport extern void 
    NarrowTextPluginSupport_destroy_data_w_params(
        NarrowText *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    NarrowTextPluginSupport_destroy_data_ex(
        NarrowText *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    NarrowTextPluginSupport_destroy_data(
        NarrowText *sample);

    NDDSUSERDllExport extern void 
    NarrowTextPluginSupport_print_data(
        const NarrowText *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    NarrowTextPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NarrowText *out,
        const NarrowText *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    NarrowTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    NarrowTextPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    NarrowTextPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    NarrowTextPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define NarrowTextChainPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NarrowTextChainPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define NarrowTextChainPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NarrowTextChainPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define NarrowTextChainPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NarrowTextChain*
    NarrowTextChainPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NarrowTextChain*
    NarrowTextChainPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NarrowTextChain*
    NarrowTextChainPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    NarrowTextChainPluginSupport_copy_data(
        NarrowTextChain *out,
        const NarrowTextChain *in);

    NDDSUSERDllExport extern void 
    NarrowTextChainPluginSupport_destroy_data_w_params(
        NarrowTextChain *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    NarrowTextChainPluginSupport_destroy_data_ex(
        NarrowTextChain *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    NarrowTextChainPluginSupport_destroy_data(
        NarrowTextChain *sample);

    NDDSUSERDllExport extern void 
    NarrowTextChainPluginSupport_print_data(
        const NarrowTextChain *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    NarrowTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NarrowTextChain *out,
        const NarrowTextChain *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    NarrowTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    NarrowTextChainPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    NarrowTextChainPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    NarrowTextChainPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedNarrowTextPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedNarrowTextPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedNarrowTextPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedNarrowTextPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedNarrowTextPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedNarrowText*
    BoundedNarrowTextPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedNarrowText*
    BoundedNarrowTextPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedNarrowText*
    BoundedNarrowTextPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedNarrowTextPluginSupport_copy_data(
        BoundedNarrowText *out,
        const BoundedNarrowText *in);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextPluginSupport_destroy_data_w_params(
        BoundedNarrowText *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextPluginSupport_destroy_data_ex(
        BoundedNarrowText *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextPluginSupport_destroy_data(
        BoundedNarrowText *sample);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextPluginSupport_print_data(
        const BoundedNarrowText *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedNarrowTextPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedNarrowText *out,
        const BoundedNarrowText *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedNarrowTextPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedNarrowTextChainPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedNarrowTextChainPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedNarrowTextChainPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedNarrowTextChainPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedNarrowTextChainPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedNarrowTextChain*
    BoundedNarrowTextChainPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedNarrowTextChain*
    BoundedNarrowTextChainPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedNarrowTextChain*
    BoundedNarrowTextChainPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedNarrowTextChainPluginSupport_copy_data(
        BoundedNarrowTextChain *out,
        const BoundedNarrowTextChain *in);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChainPluginSupport_destroy_data_w_params(
        BoundedNarrowTextChain *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChainPluginSupport_destroy_data_ex(
        BoundedNarrowTextChain *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChainPluginSupport_destroy_data(
        BoundedNarrowTextChain *sample);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChainPluginSupport_print_data(
        const BoundedNarrowTextChain *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedNarrowTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedNarrowTextChain *out,
        const BoundedNarrowTextChain *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedNarrowTextChainPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextChainPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextChainPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedNarrowTextChain2Plugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedNarrowTextChain2Plugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedNarrowTextChain2Plugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedNarrowTextChain2Plugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedNarrowTextChain2Plugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedNarrowTextChain2*
    BoundedNarrowTextChain2PluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedNarrowTextChain2*
    BoundedNarrowTextChain2PluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedNarrowTextChain2*
    BoundedNarrowTextChain2PluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedNarrowTextChain2PluginSupport_copy_data(
        BoundedNarrowTextChain2 *out,
        const BoundedNarrowTextChain2 *in);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChain2PluginSupport_destroy_data_w_params(
        BoundedNarrowTextChain2 *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChain2PluginSupport_destroy_data_ex(
        BoundedNarrowTextChain2 *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChain2PluginSupport_destroy_data(
        BoundedNarrowTextChain2 *sample);

    NDDSUSERDllExport extern void 
    BoundedNarrowTextChain2PluginSupport_print_data(
        const BoundedNarrowTextChain2 *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedNarrowTextChain2Plugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedNarrowTextChain2 *out,
        const BoundedNarrowTextChain2 *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextChain2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedNarrowTextChain2Plugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextChain2Plugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedNarrowTextChain2Plugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define WideTextPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define WideTextPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define WideTextPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define WideTextPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define WideTextPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern WideText*
    WideTextPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern WideText*
    WideTextPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern WideText*
    WideTextPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    WideTextPluginSupport_copy_data(
        WideText *out,
        const WideText *in);

    NDDSUSERDllExport extern void 
    WideTextPluginSupport_destroy_data_w_params(
        WideText *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    WideTextPluginSupport_destroy_data_ex(
        WideText *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    WideTextPluginSupport_destroy_data(
        WideText *sample);

    NDDSUSERDllExport extern void 
    WideTextPluginSupport_print_data(
        const WideText *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    WideTextPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        WideText *out,
        const WideText *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    WideTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    WideTextPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    WideTextPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    WideTextPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define WideTextChainPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define WideTextChainPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define WideTextChainPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define WideTextChainPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define WideTextChainPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern WideTextChain*
    WideTextChainPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern WideTextChain*
    WideTextChainPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern WideTextChain*
    WideTextChainPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    WideTextChainPluginSupport_copy_data(
        WideTextChain *out,
        const WideTextChain *in);

    NDDSUSERDllExport extern void 
    WideTextChainPluginSupport_destroy_data_w_params(
        WideTextChain *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    WideTextChainPluginSupport_destroy_data_ex(
        WideTextChain *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    WideTextChainPluginSupport_destroy_data(
        WideTextChain *sample);

    NDDSUSERDllExport extern void 
    WideTextChainPluginSupport_print_data(
        const WideTextChain *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    WideTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        WideTextChain *out,
        const WideTextChain *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    WideTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    WideTextChainPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    WideTextChainPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    WideTextChainPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedWideTextPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedWideTextPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedWideTextPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedWideTextPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedWideTextPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedWideText*
    BoundedWideTextPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedWideText*
    BoundedWideTextPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedWideText*
    BoundedWideTextPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextPluginSupport_copy_data(
        BoundedWideText *out,
        const BoundedWideText *in);

    NDDSUSERDllExport extern void 
    BoundedWideTextPluginSupport_destroy_data_w_params(
        BoundedWideText *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedWideTextPluginSupport_destroy_data_ex(
        BoundedWideText *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedWideTextPluginSupport_destroy_data(
        BoundedWideText *sample);

    NDDSUSERDllExport extern void 
    BoundedWideTextPluginSupport_print_data(
        const BoundedWideText *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedWideText *out,
        const BoundedWideText *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedWideTextPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedWideTextChainPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedWideTextChainPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedWideTextChainPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedWideTextChainPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedWideTextChainPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedWideTextChain*
    BoundedWideTextChainPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedWideTextChain*
    BoundedWideTextChainPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedWideTextChain*
    BoundedWideTextChainPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextChainPluginSupport_copy_data(
        BoundedWideTextChain *out,
        const BoundedWideTextChain *in);

    NDDSUSERDllExport extern void 
    BoundedWideTextChainPluginSupport_destroy_data_w_params(
        BoundedWideTextChain *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedWideTextChainPluginSupport_destroy_data_ex(
        BoundedWideTextChain *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedWideTextChainPluginSupport_destroy_data(
        BoundedWideTextChain *sample);

    NDDSUSERDllExport extern void 
    BoundedWideTextChainPluginSupport_print_data(
        const BoundedWideTextChain *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedWideTextChain *out,
        const BoundedWideTextChain *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedWideTextChainPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextChainPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextChainPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedWideTextChain2Plugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedWideTextChain2Plugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedWideTextChain2Plugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedWideTextChain2Plugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedWideTextChain2Plugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedWideTextChain2*
    BoundedWideTextChain2PluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedWideTextChain2*
    BoundedWideTextChain2PluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedWideTextChain2*
    BoundedWideTextChain2PluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextChain2PluginSupport_copy_data(
        BoundedWideTextChain2 *out,
        const BoundedWideTextChain2 *in);

    NDDSUSERDllExport extern void 
    BoundedWideTextChain2PluginSupport_destroy_data_w_params(
        BoundedWideTextChain2 *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedWideTextChain2PluginSupport_destroy_data_ex(
        BoundedWideTextChain2 *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedWideTextChain2PluginSupport_destroy_data(
        BoundedWideTextChain2 *sample);

    NDDSUSERDllExport extern void 
    BoundedWideTextChain2PluginSupport_print_data(
        const BoundedWideTextChain2 *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedWideTextChain2Plugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedWideTextChain2 *out,
        const BoundedWideTextChain2 *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextChain2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedWideTextChain2Plugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextChain2Plugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedWideTextChain2Plugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

} /* namespace CorpusStringAliasChains  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _04_string_alias_chainsPlugin_1820324921_h */

