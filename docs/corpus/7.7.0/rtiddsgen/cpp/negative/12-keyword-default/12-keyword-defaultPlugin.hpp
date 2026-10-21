

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-keyword-default.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_keyword_defaultPlugin_1643856437_h
#define _12_keyword_defaultPlugin_1643856437_h

#include "12-keyword-default.hpp"

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

namespace CorpusStrictKeyword {

    #define StructPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define StructPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define StructPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define StructPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define StructPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Struct*
    StructPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Struct*
    StructPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Struct*
    StructPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    StructPluginSupport_copy_data(
        Struct *out,
        const Struct *in);

    NDDSUSERDllExport extern void
    StructPluginSupport_destroy_data_w_params(
        Struct *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    StructPluginSupport_destroy_data_ex(
        Struct *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    StructPluginSupport_destroy_data(
        Struct *sample);

    NDDSUSERDllExport extern void
    StructPluginSupport_print_data(
        const Struct *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    StructPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    StructPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    StructPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    StructPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    StructPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Struct *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    StructPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Struct *out,
        const Struct *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    StructPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Struct *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    StructPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Struct **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    StructPlugin_deserialize_from_cdr_buffer(
        Struct *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    StructPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    StructPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    StructPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    StructPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    StructPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Struct ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    StructPlugin_new(void);

    NDDSUSERDllExport extern void
    StructPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusStrictKeyword  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _12_keyword_defaultPlugin_1643856437_h */
