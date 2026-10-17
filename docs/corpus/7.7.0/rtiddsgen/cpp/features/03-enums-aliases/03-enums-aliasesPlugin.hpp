

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enums-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _03_enums_aliasesPlugin_1935353038_h
#define _03_enums_aliasesPlugin_1935353038_h

#include "03-enums-aliases.hpp"

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

namespace CorpusEnums {

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

    #define ColorSequencePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ColorSequencePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ColorSequencePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ColorSequencePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ColorSequencePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ColorSequence*
    ColorSequencePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ColorSequence*
    ColorSequencePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ColorSequence*
    ColorSequencePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ColorSequencePluginSupport_copy_data(
        ColorSequence *out,
        const ColorSequence *in);

    NDDSUSERDllExport extern void
    ColorSequencePluginSupport_destroy_data_w_params(
        ColorSequence *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ColorSequencePluginSupport_destroy_data_ex(
        ColorSequence *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ColorSequencePluginSupport_destroy_data(
        ColorSequence *sample);

    NDDSUSERDllExport extern void
    ColorSequencePluginSupport_print_data(
        const ColorSequence *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ColorSequencePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ColorSequence *out,
        const ColorSequence *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ColorSequencePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ColorSequencePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ColorSequencePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ColorSequencePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ScalarAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ScalarAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ScalarAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ScalarAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ScalarAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ScalarAlias*
    ScalarAliasPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ScalarAlias*
    ScalarAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ScalarAlias*
    ScalarAliasPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ScalarAliasPluginSupport_copy_data(
        ScalarAlias *out,
        const ScalarAlias *in);

    NDDSUSERDllExport extern void
    ScalarAliasPluginSupport_destroy_data_w_params(
        ScalarAlias *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ScalarAliasPluginSupport_destroy_data_ex(
        ScalarAlias *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ScalarAliasPluginSupport_destroy_data(
        ScalarAlias *sample);

    NDDSUSERDllExport extern void
    ScalarAliasPluginSupport_print_data(
        const ScalarAlias *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ScalarAliasPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ScalarAlias *out,
        const ScalarAlias *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ScalarAliasPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ScalarAliasPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ScalarAliasPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ScalarAliasPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ColorAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ColorAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ColorAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ColorAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ColorAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ColorAlias*
    ColorAliasPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ColorAlias*
    ColorAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ColorAlias*
    ColorAliasPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ColorAliasPluginSupport_copy_data(
        ColorAlias *out,
        const ColorAlias *in);

    NDDSUSERDllExport extern void
    ColorAliasPluginSupport_destroy_data_w_params(
        ColorAlias *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ColorAliasPluginSupport_destroy_data_ex(
        ColorAlias *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ColorAliasPluginSupport_destroy_data(
        ColorAlias *sample);

    NDDSUSERDllExport extern void
    ColorAliasPluginSupport_print_data(
        const ColorAlias *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ColorAliasPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ColorAlias *out,
        const ColorAlias *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ColorAliasPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ColorAliasPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ColorAliasPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ColorAliasPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ColorAlias2Plugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ColorAlias2Plugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ColorAlias2Plugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ColorAlias2Plugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ColorAlias2Plugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ColorAlias2*
    ColorAlias2PluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ColorAlias2*
    ColorAlias2PluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ColorAlias2*
    ColorAlias2PluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ColorAlias2PluginSupport_copy_data(
        ColorAlias2 *out,
        const ColorAlias2 *in);

    NDDSUSERDllExport extern void
    ColorAlias2PluginSupport_destroy_data_w_params(
        ColorAlias2 *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ColorAlias2PluginSupport_destroy_data_ex(
        ColorAlias2 *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ColorAlias2PluginSupport_destroy_data(
        ColorAlias2 *sample);

    NDDSUSERDllExport extern void
    ColorAlias2PluginSupport_print_data(
        const ColorAlias2 *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ColorAlias2Plugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ColorAlias2 *out,
        const ColorAlias2 *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ColorAlias2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ColorAlias2Plugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ColorAlias2Plugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ColorAlias2Plugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define RecordPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define RecordPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define RecordPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define RecordPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define RecordPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Record*
    RecordPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Record*
    RecordPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Record*
    RecordPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    RecordPluginSupport_copy_data(
        Record *out,
        const Record *in);

    NDDSUSERDllExport extern void
    RecordPluginSupport_destroy_data_w_params(
        Record *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    RecordPluginSupport_destroy_data_ex(
        Record *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    RecordPluginSupport_destroy_data(
        Record *sample);

    NDDSUSERDllExport extern void
    RecordPluginSupport_print_data(
        const Record *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    RecordPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    RecordPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    RecordPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    RecordPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    RecordPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Record *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    RecordPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Record *out,
        const Record *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    RecordPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Record *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    RecordPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Record **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    RecordPlugin_deserialize_from_cdr_buffer(
        Record *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    RecordPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    RecordPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    RecordPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    RecordPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    RecordPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Record ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    RecordPlugin_new(void);

    NDDSUSERDllExport extern void
    RecordPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusEnums  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _03_enums_aliasesPlugin_1935353038_h */
