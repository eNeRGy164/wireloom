

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_optional_aggregate_memberPlugin_916542322_h
#define _09_optional_aggregate_memberPlugin_916542322_h

#include "09-optional-aggregate-member.hpp"

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

namespace CorpusOptionalAggregate {

    #define PayloadPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define PayloadPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define PayloadPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define PayloadPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define PayloadPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Payload*
    PayloadPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Payload*
    PayloadPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Payload*
    PayloadPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    PayloadPluginSupport_copy_data(
        Payload *out,
        const Payload *in);

    NDDSUSERDllExport extern void
    PayloadPluginSupport_destroy_data_w_params(
        Payload *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    PayloadPluginSupport_destroy_data_ex(
        Payload *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    PayloadPluginSupport_destroy_data(
        Payload *sample);

    NDDSUSERDllExport extern void
    PayloadPluginSupport_print_data(
        const Payload *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    PayloadPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    PayloadPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    PayloadPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    PayloadPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    PayloadPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Payload *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    PayloadPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Payload *out,
        const Payload *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    PayloadPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Payload *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    PayloadPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Payload **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    PayloadPlugin_deserialize_from_cdr_buffer(
        Payload *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    PayloadPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    PayloadPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    PayloadPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    PayloadPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    PayloadPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Payload ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    PayloadPlugin_new(void);

    NDDSUSERDllExport extern void
    PayloadPlugin_delete(struct PRESTypePlugin *);

    #define PayloadAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define PayloadAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define PayloadAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define PayloadAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define PayloadAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PayloadAlias*
    PayloadAliasPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern PayloadAlias*
    PayloadAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern PayloadAlias*
    PayloadAliasPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    PayloadAliasPluginSupport_copy_data(
        PayloadAlias *out,
        const PayloadAlias *in);

    NDDSUSERDllExport extern void
    PayloadAliasPluginSupport_destroy_data_w_params(
        PayloadAlias *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    PayloadAliasPluginSupport_destroy_data_ex(
        PayloadAlias *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    PayloadAliasPluginSupport_destroy_data(
        PayloadAlias *sample);

    NDDSUSERDllExport extern void
    PayloadAliasPluginSupport_print_data(
        const PayloadAlias *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    PayloadAliasPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        PayloadAlias *out,
        const PayloadAlias *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    PayloadAliasPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    PayloadAliasPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    PayloadAliasPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    PayloadAliasPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ChoicePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChoicePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ChoicePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChoicePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ChoicePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Choice*
    ChoicePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Choice*
    ChoicePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Choice*
    ChoicePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ChoicePluginSupport_copy_data(
        Choice *out,
        const Choice *in);

    NDDSUSERDllExport extern void
    ChoicePluginSupport_destroy_data_w_params(
        Choice *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ChoicePluginSupport_destroy_data_ex(
        Choice *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ChoicePluginSupport_destroy_data(
        Choice *sample);

    NDDSUSERDllExport extern void
    ChoicePluginSupport_print_data(
        const Choice *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ChoicePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ChoicePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ChoicePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ChoicePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ChoicePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Choice *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Choice *out,
        const Choice *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Choice *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Choice **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_deserialize_from_cdr_buffer(
        Choice *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ChoicePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ChoicePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ChoicePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ChoicePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ChoicePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Choice ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ChoicePlugin_new(void);

    NDDSUSERDllExport extern void
    ChoicePlugin_delete(struct PRESTypePlugin *);

    #define ChoiceAliasPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ChoiceAliasPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ChoiceAliasPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ChoiceAliasPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ChoiceAliasPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ChoiceAlias*
    ChoiceAliasPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ChoiceAlias*
    ChoiceAliasPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ChoiceAlias*
    ChoiceAliasPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ChoiceAliasPluginSupport_copy_data(
        ChoiceAlias *out,
        const ChoiceAlias *in);

    NDDSUSERDllExport extern void
    ChoiceAliasPluginSupport_destroy_data_w_params(
        ChoiceAlias *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ChoiceAliasPluginSupport_destroy_data_ex(
        ChoiceAlias *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ChoiceAliasPluginSupport_destroy_data(
        ChoiceAlias *sample);

    NDDSUSERDllExport extern void
    ChoiceAliasPluginSupport_print_data(
        const ChoiceAlias *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ChoiceAliasPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ChoiceAlias *out,
        const ChoiceAlias *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ChoiceAliasPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ChoiceAliasPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ChoiceAliasPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ChoiceAliasPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

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

} /* namespace CorpusOptionalAggregate  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _09_optional_aggregate_memberPlugin_916542322_h */
