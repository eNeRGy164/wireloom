

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _04_unbounded_string_aliasesPlugin_537514659_h
#define _04_unbounded_string_aliasesPlugin_537514659_h

#include "04-unbounded-string-aliases.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace CorpusUnboundedStrings {

    #define TextPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define TextPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define TextPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define TextPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define TextPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Text*
    TextPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Text*
    TextPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Text*
    TextPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    TextPluginSupport_copy_data(
        Text *out,
        const Text *in);

    NDDSUSERDllExport extern void
    TextPluginSupport_destroy_data_w_params(
        Text *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    TextPluginSupport_destroy_data_ex(
        Text *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    TextPluginSupport_destroy_data(
        Text *sample);

    NDDSUSERDllExport extern void
    TextPluginSupport_print_data(
        const Text *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    TextPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Text *out,
        const Text *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    TextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    TextPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    TextPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    TextPlugin_get_serialized_key_max_size_for_keyhash(
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

} /* namespace CorpusUnboundedStrings  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _04_unbounded_string_aliasesPlugin_537514659_h */
