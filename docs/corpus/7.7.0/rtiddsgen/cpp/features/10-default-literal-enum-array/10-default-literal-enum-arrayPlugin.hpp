

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-default-literal-enum-array.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _10_default_literal_enum_arrayPlugin_1866069445_h
#define _10_default_literal_enum_arrayPlugin_1866069445_h

#include "10-default-literal-enum-array.hpp"

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

namespace CorpusDefaultLiteralEnumArray {

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ColorPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ColorPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ColorPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* ----------------------------------------------------------------------------
    Support functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern void
    ColorPluginSupport_print_data(
        const Color *sample,
        const char *desc,
        unsigned int indent_level);

    #define ColorArrayPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ColorArrayPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ColorArrayPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ColorArrayPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ColorArrayPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ColorArray*
    ColorArrayPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ColorArray*
    ColorArrayPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ColorArray*
    ColorArrayPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ColorArrayPluginSupport_copy_data(
        ColorArray *out,
        const ColorArray *in);

    NDDSUSERDllExport extern void
    ColorArrayPluginSupport_destroy_data_w_params(
        ColorArray *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ColorArrayPluginSupport_destroy_data_ex(
        ColorArray *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ColorArrayPluginSupport_destroy_data(
        ColorArray *sample);

    NDDSUSERDllExport extern void
    ColorArrayPluginSupport_print_data(
        const ColorArray *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ColorArrayPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ColorArray *out,
        const ColorArray *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ColorArrayPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ColorArrayPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ColorArrayPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ColorArrayPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ShapePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ShapePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ShapePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ShapePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ShapePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Shape*
    ShapePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Shape*
    ShapePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Shape*
    ShapePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ShapePluginSupport_copy_data(
        Shape *out,
        const Shape *in);

    NDDSUSERDllExport extern void
    ShapePluginSupport_destroy_data_w_params(
        Shape *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ShapePluginSupport_destroy_data_ex(
        Shape *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ShapePluginSupport_destroy_data(
        Shape *sample);

    NDDSUSERDllExport extern void
    ShapePluginSupport_print_data(
        const Shape *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ShapePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ShapePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ShapePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ShapePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ShapePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Shape *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ShapePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Shape *out,
        const Shape *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ShapePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Shape *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ShapePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Shape **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ShapePlugin_deserialize_from_cdr_buffer(
        Shape *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ShapePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ShapePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ShapePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ShapePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ShapePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Shape ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ShapePlugin_new(void);

    NDDSUSERDllExport extern void
    ShapePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusDefaultLiteralEnumArray  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _10_default_literal_enum_arrayPlugin_1866069445_h */
