

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-identifier-control.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_identifier_controlPlugin_1802072989_h
#define _12_identifier_controlPlugin_1802072989_h

#include "12-identifier-control.hpp"

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

namespace CorpusIdentifierControl {

    #define _StructPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define _StructPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define _StructPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define _StructPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define _StructPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern _Struct*
    _StructPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern _Struct*
    _StructPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern _Struct*
    _StructPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    _StructPluginSupport_copy_data(
        _Struct *out,
        const _Struct *in);

    NDDSUSERDllExport extern void
    _StructPluginSupport_destroy_data_w_params(
        _Struct *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    _StructPluginSupport_destroy_data_ex(
        _Struct *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    _StructPluginSupport_destroy_data(
        _Struct *sample);

    NDDSUSERDllExport extern void
    _StructPluginSupport_print_data(
        const _Struct *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    _StructPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    _StructPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    _StructPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    _StructPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    _StructPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        _Struct *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    _StructPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        _Struct *out,
        const _Struct *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    _StructPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const _Struct *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    _StructPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        _Struct **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    _StructPlugin_deserialize_from_cdr_buffer(
        _Struct *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    _StructPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    _StructPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    _StructPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    _StructPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    _StructPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        _Struct ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    _StructPlugin_new(void);

    NDDSUSERDllExport extern void
    _StructPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusIdentifierControl  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _12_identifier_controlPlugin_1802072989_h */
