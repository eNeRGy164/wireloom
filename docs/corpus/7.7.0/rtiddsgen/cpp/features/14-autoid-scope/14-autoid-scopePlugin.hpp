

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_autoid_scopePlugin_2088995224_h
#define _14_autoid_scopePlugin_2088995224_h

#include "14-autoid-scope.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "includes/autoid-hash-includePlugin.hpp"

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace AutoIds {

    #define HashedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define HashedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define HashedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define HashedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define HashedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Hashed*
    HashedPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Hashed*
    HashedPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Hashed*
    HashedPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    HashedPluginSupport_copy_data(
        Hashed *out,
        const Hashed *in);

    NDDSUSERDllExport extern void
    HashedPluginSupport_destroy_data_w_params(
        Hashed *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    HashedPluginSupport_destroy_data_ex(
        Hashed *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    HashedPluginSupport_destroy_data(
        Hashed *sample);

    NDDSUSERDllExport extern void
    HashedPluginSupport_print_data(
        const Hashed *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    HashedPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    HashedPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    HashedPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    HashedPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    HashedPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Hashed *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    HashedPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Hashed *out,
        const Hashed *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    HashedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Hashed *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    HashedPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Hashed **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    HashedPlugin_deserialize_from_cdr_buffer(
        Hashed *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    HashedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    HashedPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    HashedPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    HashedPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    HashedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Hashed ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    HashedPlugin_new(void);

    NDDSUSERDllExport extern void
    HashedPlugin_delete(struct PRESTypePlugin *);

} /* namespace AutoIds  */
namespace AutoIds {

    #define ReopenedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ReopenedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ReopenedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ReopenedPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ReopenedPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Reopened*
    ReopenedPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Reopened*
    ReopenedPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Reopened*
    ReopenedPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ReopenedPluginSupport_copy_data(
        Reopened *out,
        const Reopened *in);

    NDDSUSERDllExport extern void
    ReopenedPluginSupport_destroy_data_w_params(
        Reopened *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ReopenedPluginSupport_destroy_data_ex(
        Reopened *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ReopenedPluginSupport_destroy_data(
        Reopened *sample);

    NDDSUSERDllExport extern void
    ReopenedPluginSupport_print_data(
        const Reopened *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ReopenedPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ReopenedPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ReopenedPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ReopenedPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ReopenedPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Reopened *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ReopenedPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Reopened *out,
        const Reopened *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ReopenedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Reopened *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ReopenedPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Reopened **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ReopenedPlugin_deserialize_from_cdr_buffer(
        Reopened *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ReopenedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ReopenedPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ReopenedPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ReopenedPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ReopenedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Reopened ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ReopenedPlugin_new(void);

    NDDSUSERDllExport extern void
    ReopenedPlugin_delete(struct PRESTypePlugin *);

} /* namespace AutoIds  */

#define SequentialPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define SequentialPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define SequentialPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define SequentialPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define SequentialPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern Sequential*
SequentialPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern Sequential*
SequentialPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Sequential*
SequentialPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
SequentialPluginSupport_copy_data(
    Sequential *out,
    const Sequential *in);

NDDSUSERDllExport extern void
SequentialPluginSupport_destroy_data_w_params(
    Sequential *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
SequentialPluginSupport_destroy_data_ex(
    Sequential *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
SequentialPluginSupport_destroy_data(
    Sequential *sample);

NDDSUSERDllExport extern void
SequentialPluginSupport_print_data(
    const Sequential *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
SequentialPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
SequentialPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
SequentialPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
SequentialPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
SequentialPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    Sequential *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
SequentialPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    Sequential *out,
    const Sequential *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
SequentialPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const Sequential *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
SequentialPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    Sequential **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
SequentialPlugin_deserialize_from_cdr_buffer(
    Sequential *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
SequentialPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
SequentialPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
SequentialPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
SequentialPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
SequentialPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    Sequential ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
SequentialPlugin_new(void);

NDDSUSERDllExport extern void
SequentialPlugin_delete(struct PRESTypePlugin *);

#define UsesIncludePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define UsesIncludePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define UsesIncludePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define UsesIncludePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define UsesIncludePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern UsesInclude*
UsesIncludePluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern UsesInclude*
UsesIncludePluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern UsesInclude*
UsesIncludePluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
UsesIncludePluginSupport_copy_data(
    UsesInclude *out,
    const UsesInclude *in);

NDDSUSERDllExport extern void
UsesIncludePluginSupport_destroy_data_w_params(
    UsesInclude *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
UsesIncludePluginSupport_destroy_data_ex(
    UsesInclude *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
UsesIncludePluginSupport_destroy_data(
    UsesInclude *sample);

NDDSUSERDllExport extern void
UsesIncludePluginSupport_print_data(
    const UsesInclude *sample,
    const char *desc,
    unsigned int indent);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
UsesIncludePlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
UsesIncludePlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
UsesIncludePlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
UsesIncludePlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
UsesIncludePlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    UsesInclude *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
UsesIncludePlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    UsesInclude *out,
    const UsesInclude *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
UsesIncludePlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const UsesInclude *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
UsesIncludePlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    UsesInclude **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
UsesIncludePlugin_deserialize_from_cdr_buffer(
    UsesInclude *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
UsesIncludePlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
UsesIncludePlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
UsesIncludePlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
UsesIncludePlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
UsesIncludePlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    UsesInclude ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
UsesIncludePlugin_new(void);

NDDSUSERDllExport extern void
UsesIncludePlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_autoid_scopePlugin_2088995224_h */
