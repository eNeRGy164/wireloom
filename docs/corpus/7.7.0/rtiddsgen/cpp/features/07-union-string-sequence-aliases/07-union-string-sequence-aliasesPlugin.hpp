

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_string_sequence_aliasesPlugin_25436842_h
#define _07_union_string_sequence_aliasesPlugin_25436842_h

#include "07-union-string-sequence-aliases.hpp"

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

namespace RuntimeUnionStringSequences {

    #define NarrowTextsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NarrowTextsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define NarrowTextsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NarrowTextsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define NarrowTextsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NarrowTexts*
    NarrowTextsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NarrowTexts*
    NarrowTextsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NarrowTexts*
    NarrowTextsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    NarrowTextsPluginSupport_copy_data(
        NarrowTexts *out,
        const NarrowTexts *in);

    NDDSUSERDllExport extern void
    NarrowTextsPluginSupport_destroy_data_w_params(
        NarrowTexts *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    NarrowTextsPluginSupport_destroy_data_ex(
        NarrowTexts *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    NarrowTextsPluginSupport_destroy_data(
        NarrowTexts *sample);

    NDDSUSERDllExport extern void
    NarrowTextsPluginSupport_print_data(
        const NarrowTexts *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NarrowTextsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NarrowTexts *out,
        const NarrowTexts *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    NarrowTextsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    NarrowTextsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    NarrowTextsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    NarrowTextsPlugin_get_serialized_key_max_size_for_keyhash(
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

    #define BoundedNarrowTextsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedNarrowTextsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define BoundedNarrowTextsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedNarrowTextsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define BoundedNarrowTextsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedNarrowTexts*
    BoundedNarrowTextsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedNarrowTexts*
    BoundedNarrowTextsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedNarrowTexts*
    BoundedNarrowTextsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    BoundedNarrowTextsPluginSupport_copy_data(
        BoundedNarrowTexts *out,
        const BoundedNarrowTexts *in);

    NDDSUSERDllExport extern void
    BoundedNarrowTextsPluginSupport_destroy_data_w_params(
        BoundedNarrowTexts *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    BoundedNarrowTextsPluginSupport_destroy_data_ex(
        BoundedNarrowTexts *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    BoundedNarrowTextsPluginSupport_destroy_data(
        BoundedNarrowTexts *sample);

    NDDSUSERDllExport extern void
    BoundedNarrowTextsPluginSupport_print_data(
        const BoundedNarrowTexts *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    BoundedNarrowTextsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedNarrowTexts *out,
        const BoundedNarrowTexts *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    BoundedNarrowTextsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    BoundedNarrowTextsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    BoundedNarrowTextsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    BoundedNarrowTextsPlugin_get_serialized_key_max_size_for_keyhash(
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

    #define DirectStringSequencesPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define DirectStringSequencesPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define DirectStringSequencesPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define DirectStringSequencesPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define DirectStringSequencesPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern DirectStringSequences*
    DirectStringSequencesPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern DirectStringSequences*
    DirectStringSequencesPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern DirectStringSequences*
    DirectStringSequencesPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    DirectStringSequencesPluginSupport_copy_data(
        DirectStringSequences *out,
        const DirectStringSequences *in);

    NDDSUSERDllExport extern void
    DirectStringSequencesPluginSupport_destroy_data_w_params(
        DirectStringSequences *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    DirectStringSequencesPluginSupport_destroy_data_ex(
        DirectStringSequences *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    DirectStringSequencesPluginSupport_destroy_data(
        DirectStringSequences *sample);

    NDDSUSERDllExport extern void
    DirectStringSequencesPluginSupport_print_data(
        const DirectStringSequences *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    DirectStringSequencesPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    DirectStringSequencesPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    DirectStringSequencesPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    DirectStringSequencesPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    DirectStringSequencesPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        DirectStringSequences *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    DirectStringSequencesPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        DirectStringSequences *out,
        const DirectStringSequences *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    DirectStringSequencesPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const DirectStringSequences *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    DirectStringSequencesPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        DirectStringSequences **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    DirectStringSequencesPlugin_deserialize_from_cdr_buffer(
        DirectStringSequences *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    DirectStringSequencesPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    DirectStringSequencesPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    DirectStringSequencesPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    DirectStringSequencesPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    DirectStringSequencesPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        DirectStringSequences ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    DirectStringSequencesPlugin_new(void);

    NDDSUSERDllExport extern void
    DirectStringSequencesPlugin_delete(struct PRESTypePlugin *);

    #define StringSequenceAliasesPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define StringSequenceAliasesPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define StringSequenceAliasesPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define StringSequenceAliasesPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define StringSequenceAliasesPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern StringSequenceAliases*
    StringSequenceAliasesPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern StringSequenceAliases*
    StringSequenceAliasesPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern StringSequenceAliases*
    StringSequenceAliasesPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    StringSequenceAliasesPluginSupport_copy_data(
        StringSequenceAliases *out,
        const StringSequenceAliases *in);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPluginSupport_destroy_data_w_params(
        StringSequenceAliases *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPluginSupport_destroy_data_ex(
        StringSequenceAliases *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPluginSupport_destroy_data(
        StringSequenceAliases *sample);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPluginSupport_print_data(
        const StringSequenceAliases *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    StringSequenceAliasesPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    StringSequenceAliasesPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceAliases *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    StringSequenceAliasesPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceAliases *out,
        const StringSequenceAliases *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    StringSequenceAliasesPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const StringSequenceAliases *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    StringSequenceAliasesPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceAliases **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    StringSequenceAliasesPlugin_deserialize_from_cdr_buffer(
        StringSequenceAliases *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    StringSequenceAliasesPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    StringSequenceAliasesPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    StringSequenceAliasesPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    StringSequenceAliasesPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    StringSequenceAliasesPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        StringSequenceAliases ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    StringSequenceAliasesPlugin_new(void);

    NDDSUSERDllExport extern void
    StringSequenceAliasesPlugin_delete(struct PRESTypePlugin *);

} /* namespace RuntimeUnionStringSequences  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _07_union_string_sequence_aliasesPlugin_25436842_h */
