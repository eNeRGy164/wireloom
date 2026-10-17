

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-name-collisions.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_name_collisionsPlugin_687327134_h
#define _12_name_collisionsPlugin_687327134_h

#include "12-name-collisions.hpp"

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

namespace CorpusNameCollisions {

    #define ItemPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ItemPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ItemPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ItemPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ItemPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Item*
    ItemPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Item*
    ItemPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Item*
    ItemPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ItemPluginSupport_copy_data(
        Item *out,
        const Item *in);

    NDDSUSERDllExport extern void
    ItemPluginSupport_destroy_data_w_params(
        Item *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ItemPluginSupport_destroy_data_ex(
        Item *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ItemPluginSupport_destroy_data(
        Item *sample);

    NDDSUSERDllExport extern void
    ItemPluginSupport_print_data(
        const Item *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ItemPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ItemPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ItemPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ItemPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ItemPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Item *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ItemPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Item *out,
        const Item *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ItemPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Item *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ItemPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Item **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ItemPlugin_deserialize_from_cdr_buffer(
        Item *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ItemPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ItemPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ItemPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ItemPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ItemPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Item ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ItemPlugin_new(void);

    NDDSUSERDllExport extern void
    ItemPlugin_delete(struct PRESTypePlugin *);

    #define LifecycleParametersPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define LifecycleParametersPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define LifecycleParametersPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define LifecycleParametersPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define LifecycleParametersPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern LifecycleParameters*
    LifecycleParametersPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern LifecycleParameters*
    LifecycleParametersPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern LifecycleParameters*
    LifecycleParametersPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    LifecycleParametersPluginSupport_copy_data(
        LifecycleParameters *out,
        const LifecycleParameters *in);

    NDDSUSERDllExport extern void
    LifecycleParametersPluginSupport_destroy_data_w_params(
        LifecycleParameters *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    LifecycleParametersPluginSupport_destroy_data_ex(
        LifecycleParameters *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    LifecycleParametersPluginSupport_destroy_data(
        LifecycleParameters *sample);

    NDDSUSERDllExport extern void
    LifecycleParametersPluginSupport_print_data(
        const LifecycleParameters *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    LifecycleParametersPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    LifecycleParametersPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    LifecycleParametersPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    LifecycleParametersPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    LifecycleParametersPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        LifecycleParameters *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    LifecycleParametersPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        LifecycleParameters *out,
        const LifecycleParameters *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    LifecycleParametersPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const LifecycleParameters *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    LifecycleParametersPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        LifecycleParameters **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    LifecycleParametersPlugin_deserialize_from_cdr_buffer(
        LifecycleParameters *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    LifecycleParametersPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    LifecycleParametersPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    LifecycleParametersPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    LifecycleParametersPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    LifecycleParametersPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        LifecycleParameters ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    LifecycleParametersPlugin_new(void);

    NDDSUSERDllExport extern void
    LifecycleParametersPlugin_delete(struct PRESTypePlugin *);

    #define ManagedMethodParametersPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ManagedMethodParametersPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ManagedMethodParametersPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ManagedMethodParametersPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ManagedMethodParametersPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ManagedMethodParameters*
    ManagedMethodParametersPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ManagedMethodParameters*
    ManagedMethodParametersPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ManagedMethodParameters*
    ManagedMethodParametersPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ManagedMethodParametersPluginSupport_copy_data(
        ManagedMethodParameters *out,
        const ManagedMethodParameters *in);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPluginSupport_destroy_data_w_params(
        ManagedMethodParameters *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPluginSupport_destroy_data_ex(
        ManagedMethodParameters *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPluginSupport_destroy_data(
        ManagedMethodParameters *sample);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPluginSupport_print_data(
        const ManagedMethodParameters *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ManagedMethodParametersPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ManagedMethodParametersPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        ManagedMethodParameters *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ManagedMethodParametersPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ManagedMethodParameters *out,
        const ManagedMethodParameters *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ManagedMethodParametersPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const ManagedMethodParameters *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ManagedMethodParametersPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        ManagedMethodParameters **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ManagedMethodParametersPlugin_deserialize_from_cdr_buffer(
        ManagedMethodParameters *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ManagedMethodParametersPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ManagedMethodParametersPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ManagedMethodParametersPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ManagedMethodParametersPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ManagedMethodParametersPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        ManagedMethodParameters ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ManagedMethodParametersPlugin_new(void);

    NDDSUSERDllExport extern void
    ManagedMethodParametersPlugin_delete(struct PRESTypePlugin *);

    #define ArrayInitializationLocalsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ArrayInitializationLocalsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ArrayInitializationLocalsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ArrayInitializationLocalsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ArrayInitializationLocalsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ArrayInitializationLocals*
    ArrayInitializationLocalsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ArrayInitializationLocals*
    ArrayInitializationLocalsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ArrayInitializationLocals*
    ArrayInitializationLocalsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ArrayInitializationLocalsPluginSupport_copy_data(
        ArrayInitializationLocals *out,
        const ArrayInitializationLocals *in);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPluginSupport_destroy_data_w_params(
        ArrayInitializationLocals *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPluginSupport_destroy_data_ex(
        ArrayInitializationLocals *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPluginSupport_destroy_data(
        ArrayInitializationLocals *sample);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPluginSupport_print_data(
        const ArrayInitializationLocals *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ArrayInitializationLocalsPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ArrayInitializationLocalsPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        ArrayInitializationLocals *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ArrayInitializationLocalsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ArrayInitializationLocals *out,
        const ArrayInitializationLocals *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ArrayInitializationLocalsPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const ArrayInitializationLocals *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ArrayInitializationLocalsPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        ArrayInitializationLocals **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ArrayInitializationLocalsPlugin_deserialize_from_cdr_buffer(
        ArrayInitializationLocals *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ArrayInitializationLocalsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ArrayInitializationLocalsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ArrayInitializationLocalsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ArrayInitializationLocalsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ArrayInitializationLocalsPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        ArrayInitializationLocals ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ArrayInitializationLocalsPlugin_new(void);

    NDDSUSERDllExport extern void
    ArrayInitializationLocalsPlugin_delete(struct PRESTypePlugin *);

    #define NativeInheritanceStoragePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NativeInheritanceStoragePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define NativeInheritanceStoragePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NativeInheritanceStoragePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define NativeInheritanceStoragePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NativeInheritanceStorage*
    NativeInheritanceStoragePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NativeInheritanceStorage*
    NativeInheritanceStoragePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NativeInheritanceStorage*
    NativeInheritanceStoragePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    NativeInheritanceStoragePluginSupport_copy_data(
        NativeInheritanceStorage *out,
        const NativeInheritanceStorage *in);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePluginSupport_destroy_data_w_params(
        NativeInheritanceStorage *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePluginSupport_destroy_data_ex(
        NativeInheritanceStorage *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePluginSupport_destroy_data(
        NativeInheritanceStorage *sample);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePluginSupport_print_data(
        const NativeInheritanceStorage *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    NativeInheritanceStoragePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    NativeInheritanceStoragePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        NativeInheritanceStorage *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    NativeInheritanceStoragePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NativeInheritanceStorage *out,
        const NativeInheritanceStorage *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NativeInheritanceStoragePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const NativeInheritanceStorage *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    NativeInheritanceStoragePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        NativeInheritanceStorage **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    NativeInheritanceStoragePlugin_deserialize_from_cdr_buffer(
        NativeInheritanceStorage *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    NativeInheritanceStoragePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    NativeInheritanceStoragePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    NativeInheritanceStoragePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    NativeInheritanceStoragePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    NativeInheritanceStoragePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        NativeInheritanceStorage ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    NativeInheritanceStoragePlugin_new(void);

    NDDSUSERDllExport extern void
    NativeInheritanceStoragePlugin_delete(struct PRESTypePlugin *);

    #define DerivedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define DerivedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define DerivedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define DerivedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define DerivedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    DerivedPluginSupport_copy_data(
        Derived *out,
        const Derived *in);

    NDDSUSERDllExport extern void
    DerivedPluginSupport_destroy_data_w_params(
        Derived *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    DerivedPluginSupport_destroy_data_ex(
        Derived *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    DerivedPluginSupport_destroy_data(
        Derived *sample);

    NDDSUSERDllExport extern void
    DerivedPluginSupport_print_data(
        const Derived *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    DerivedPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    DerivedPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    DerivedPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    DerivedPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    DerivedPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Derived *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Derived *out,
        const Derived *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Derived *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Derived **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_deserialize_from_cdr_buffer(
        Derived *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    DerivedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    DerivedPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    DerivedPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    DerivedPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Derived ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    DerivedPlugin_new(void);

    NDDSUSERDllExport extern void
    DerivedPlugin_delete(struct PRESTypePlugin *);

    #define NativeLifecycleParametersPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NativeLifecycleParametersPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define NativeLifecycleParametersPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NativeLifecycleParametersPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define NativeLifecycleParametersPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NativeLifecycleParameters*
    NativeLifecycleParametersPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NativeLifecycleParameters*
    NativeLifecycleParametersPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NativeLifecycleParameters*
    NativeLifecycleParametersPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    NativeLifecycleParametersPluginSupport_copy_data(
        NativeLifecycleParameters *out,
        const NativeLifecycleParameters *in);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPluginSupport_destroy_data_w_params(
        NativeLifecycleParameters *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPluginSupport_destroy_data_ex(
        NativeLifecycleParameters *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPluginSupport_destroy_data(
        NativeLifecycleParameters *sample);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPluginSupport_print_data(
        const NativeLifecycleParameters *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    NativeLifecycleParametersPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    NativeLifecycleParametersPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        NativeLifecycleParameters *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    NativeLifecycleParametersPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NativeLifecycleParameters *out,
        const NativeLifecycleParameters *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NativeLifecycleParametersPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const NativeLifecycleParameters *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    NativeLifecycleParametersPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        NativeLifecycleParameters **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    NativeLifecycleParametersPlugin_deserialize_from_cdr_buffer(
        NativeLifecycleParameters *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    NativeLifecycleParametersPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    NativeLifecycleParametersPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    NativeLifecycleParametersPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    NativeLifecycleParametersPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    NativeLifecycleParametersPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        NativeLifecycleParameters ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    NativeLifecycleParametersPlugin_new(void);

    NDDSUSERDllExport extern void
    NativeLifecycleParametersPlugin_delete(struct PRESTypePlugin *);

    #define NativeDiscriminatorStoragePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NativeDiscriminatorStoragePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define NativeDiscriminatorStoragePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NativeDiscriminatorStoragePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define NativeDiscriminatorStoragePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern NativeDiscriminatorStorage*
    NativeDiscriminatorStoragePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern NativeDiscriminatorStorage*
    NativeDiscriminatorStoragePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern NativeDiscriminatorStorage*
    NativeDiscriminatorStoragePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    NativeDiscriminatorStoragePluginSupport_copy_data(
        NativeDiscriminatorStorage *out,
        const NativeDiscriminatorStorage *in);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePluginSupport_destroy_data_w_params(
        NativeDiscriminatorStorage *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePluginSupport_destroy_data_ex(
        NativeDiscriminatorStorage *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePluginSupport_destroy_data(
        NativeDiscriminatorStorage *sample);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePluginSupport_print_data(
        const NativeDiscriminatorStorage *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    NativeDiscriminatorStoragePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    NativeDiscriminatorStoragePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        NativeDiscriminatorStorage *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    NativeDiscriminatorStoragePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        NativeDiscriminatorStorage *out,
        const NativeDiscriminatorStorage *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    NativeDiscriminatorStoragePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const NativeDiscriminatorStorage *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    NativeDiscriminatorStoragePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        NativeDiscriminatorStorage **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    NativeDiscriminatorStoragePlugin_deserialize_from_cdr_buffer(
        NativeDiscriminatorStorage *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    NativeDiscriminatorStoragePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    NativeDiscriminatorStoragePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    NativeDiscriminatorStoragePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    NativeDiscriminatorStoragePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    NativeDiscriminatorStoragePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        NativeDiscriminatorStorage ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    NativeDiscriminatorStoragePlugin_new(void);

    NDDSUSERDllExport extern void
    NativeDiscriminatorStoragePlugin_delete(struct PRESTypePlugin *);

    #define ManagedDiscriminatorStoragePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ManagedDiscriminatorStoragePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ManagedDiscriminatorStoragePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ManagedDiscriminatorStoragePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ManagedDiscriminatorStoragePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ManagedDiscriminatorStorage*
    ManagedDiscriminatorStoragePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ManagedDiscriminatorStorage*
    ManagedDiscriminatorStoragePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ManagedDiscriminatorStorage*
    ManagedDiscriminatorStoragePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ManagedDiscriminatorStoragePluginSupport_copy_data(
        ManagedDiscriminatorStorage *out,
        const ManagedDiscriminatorStorage *in);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePluginSupport_destroy_data_w_params(
        ManagedDiscriminatorStorage *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePluginSupport_destroy_data_ex(
        ManagedDiscriminatorStorage *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePluginSupport_destroy_data(
        ManagedDiscriminatorStorage *sample);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePluginSupport_print_data(
        const ManagedDiscriminatorStorage *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ManagedDiscriminatorStoragePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ManagedDiscriminatorStoragePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        ManagedDiscriminatorStorage *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ManagedDiscriminatorStoragePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ManagedDiscriminatorStorage *out,
        const ManagedDiscriminatorStorage *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ManagedDiscriminatorStoragePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const ManagedDiscriminatorStorage *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ManagedDiscriminatorStoragePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        ManagedDiscriminatorStorage **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ManagedDiscriminatorStoragePlugin_deserialize_from_cdr_buffer(
        ManagedDiscriminatorStorage *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ManagedDiscriminatorStoragePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ManagedDiscriminatorStoragePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ManagedDiscriminatorStoragePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ManagedDiscriminatorStoragePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ManagedDiscriminatorStoragePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        ManagedDiscriminatorStorage ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ManagedDiscriminatorStoragePlugin_new(void);

    NDDSUSERDllExport extern void
    ManagedDiscriminatorStoragePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusNameCollisions  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _12_name_collisionsPlugin_687327134_h */
