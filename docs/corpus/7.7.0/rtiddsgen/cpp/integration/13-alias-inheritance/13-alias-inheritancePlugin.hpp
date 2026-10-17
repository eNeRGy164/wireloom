

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_alias_inheritancePlugin_745606614_h
#define _13_alias_inheritancePlugin_745606614_h

#include "13-alias-inheritance.hpp"

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

namespace CorpusIntegrationTrace {

    #define ProducerPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ProducerPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ProducerPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ProducerPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ProducerPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Producer*
    ProducerPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Producer*
    ProducerPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Producer*
    ProducerPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ProducerPluginSupport_copy_data(
        Producer *out,
        const Producer *in);

    NDDSUSERDllExport extern void
    ProducerPluginSupport_destroy_data_w_params(
        Producer *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ProducerPluginSupport_destroy_data_ex(
        Producer *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ProducerPluginSupport_destroy_data(
        Producer *sample);

    NDDSUSERDllExport extern void
    ProducerPluginSupport_print_data(
        const Producer *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ProducerPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Producer *out,
        const Producer *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int
    ProducerPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ProducerPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ProducerPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ProducerPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ContextPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ContextPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ContextPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ContextPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ContextPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Context*
    ContextPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Context*
    ContextPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Context*
    ContextPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ContextPluginSupport_copy_data(
        Context *out,
        const Context *in);

    NDDSUSERDllExport extern void
    ContextPluginSupport_destroy_data_w_params(
        Context *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ContextPluginSupport_destroy_data_ex(
        Context *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ContextPluginSupport_destroy_data(
        Context *sample);

    NDDSUSERDllExport extern void
    ContextPluginSupport_print_data(
        const Context *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ContextPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Context *out,
        const Context *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ContextPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Context *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ContextPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Context **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ContextPlugin_deserialize_from_cdr_buffer(
        Context *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ContextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ContextPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ContextPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ContextPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ContextPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Context ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    #define BasePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BasePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define BasePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BasePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define BasePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    BasePluginSupport_copy_data(
        Base *out,
        const Base *in);

    NDDSUSERDllExport extern void
    BasePluginSupport_destroy_data_w_params(
        Base *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    BasePluginSupport_destroy_data_ex(
        Base *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    BasePluginSupport_destroy_data(
        Base *sample);

    NDDSUSERDllExport extern void
    BasePluginSupport_print_data(
        const Base *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    BasePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Base *out,
        const Base *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    BasePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Base *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    BasePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Base **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    BasePlugin_deserialize_from_cdr_buffer(
        Base *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    BasePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    BasePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    BasePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    BasePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    BasePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Base ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

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

} /* namespace CorpusIntegrationTrace  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _13_alias_inheritancePlugin_745606614_h */
