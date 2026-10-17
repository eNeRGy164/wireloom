

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-boundaries.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_boundariesPlugin_919417863_h
#define _08_key_boundariesPlugin_919417863_h

#include "08-key-boundaries.hpp"

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

namespace CorpusKeyBoundaries {

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct ArrayKey
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct ArrayKey)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * ArrayKey must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct ArrayKey.
    */
    typedef struct ArrayKey ArrayKeyKeyHolder;

    #define ArrayKeyPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ArrayKeyPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define ArrayKeyPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ArrayKeyPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define ArrayKeyPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define ArrayKeyPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define ArrayKeyPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern ArrayKey*
    ArrayKeyPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern ArrayKey*
    ArrayKeyPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ArrayKey*
    ArrayKeyPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPluginSupport_copy_data(
        ArrayKey *out,
        const ArrayKey *in);

    NDDSUSERDllExport extern void
    ArrayKeyPluginSupport_destroy_data_w_params(
        ArrayKey *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    ArrayKeyPluginSupport_destroy_data_ex(
        ArrayKey *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ArrayKeyPluginSupport_destroy_data(
        ArrayKey *sample);

    NDDSUSERDllExport extern void
    ArrayKeyPluginSupport_print_data(
        const ArrayKey *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern ArrayKey*
    ArrayKeyPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern ArrayKey*
    ArrayKeyPluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    ArrayKeyPluginSupport_destroy_key_ex(
        ArrayKeyKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    ArrayKeyPluginSupport_destroy_key(
        ArrayKeyKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    ArrayKeyPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    ArrayKeyPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    ArrayKeyPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    ArrayKeyPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    ArrayKeyPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        ArrayKey *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        ArrayKey *out,
        const ArrayKey *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const ArrayKey *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        ArrayKey **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_deserialize_from_cdr_buffer(
        ArrayKey *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    ArrayKeyPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    ArrayKeyPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    ArrayKeyPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    ArrayKeyPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        ArrayKey ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        ArrayKeyKeyHolder *key,
        const ArrayKey *instance);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        ArrayKey *instance,
        const ArrayKeyKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    ArrayKeyPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    ArrayKeyPlugin_new(void);

    NDDSUSERDllExport extern void
    ArrayKeyPlugin_delete(struct PRESTypePlugin *);

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct BoundedStringKey
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct BoundedStringKey)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * BoundedStringKey must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct BoundedStringKey.
    */
    typedef struct BoundedStringKey BoundedStringKeyKeyHolder;

    #define BoundedStringKeyPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedStringKeyPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define BoundedStringKeyPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedStringKeyPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define BoundedStringKeyPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define BoundedStringKeyPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define BoundedStringKeyPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedStringKey*
    BoundedStringKeyPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedStringKey*
    BoundedStringKeyPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedStringKey*
    BoundedStringKeyPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPluginSupport_copy_data(
        BoundedStringKey *out,
        const BoundedStringKey *in);

    NDDSUSERDllExport extern void
    BoundedStringKeyPluginSupport_destroy_data_w_params(
        BoundedStringKey *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    BoundedStringKeyPluginSupport_destroy_data_ex(
        BoundedStringKey *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    BoundedStringKeyPluginSupport_destroy_data(
        BoundedStringKey *sample);

    NDDSUSERDllExport extern void
    BoundedStringKeyPluginSupport_print_data(
        const BoundedStringKey *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern BoundedStringKey*
    BoundedStringKeyPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedStringKey*
    BoundedStringKeyPluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    BoundedStringKeyPluginSupport_destroy_key_ex(
        BoundedStringKeyKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    BoundedStringKeyPluginSupport_destroy_key(
        BoundedStringKeyKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    BoundedStringKeyPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    BoundedStringKeyPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    BoundedStringKeyPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    BoundedStringKeyPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    BoundedStringKeyPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedStringKey *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedStringKey *out,
        const BoundedStringKey *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const BoundedStringKey *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        BoundedStringKey **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_deserialize_from_cdr_buffer(
        BoundedStringKey *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    BoundedStringKeyPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    BoundedStringKeyPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    BoundedStringKeyPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    BoundedStringKeyPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        BoundedStringKey ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        BoundedStringKeyKeyHolder *key,
        const BoundedStringKey *instance);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        BoundedStringKey *instance,
        const BoundedStringKeyKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    BoundedStringKeyPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    BoundedStringKeyPlugin_new(void);

    NDDSUSERDllExport extern void
    BoundedStringKeyPlugin_delete(struct PRESTypePlugin *);

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct UnboundedStringKey
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct UnboundedStringKey)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * UnboundedStringKey must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct UnboundedStringKey.
    */
    typedef struct UnboundedStringKey UnboundedStringKeyKeyHolder;

    #define UnboundedStringKeyPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define UnboundedStringKeyPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define UnboundedStringKeyPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define UnboundedStringKeyPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define UnboundedStringKeyPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define UnboundedStringKeyPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
    #define UnboundedStringKeyPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern UnboundedStringKey*
    UnboundedStringKeyPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern UnboundedStringKey*
    UnboundedStringKeyPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern UnboundedStringKey*
    UnboundedStringKeyPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPluginSupport_copy_data(
        UnboundedStringKey *out,
        const UnboundedStringKey *in);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPluginSupport_destroy_data_w_params(
        UnboundedStringKey *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPluginSupport_destroy_data_ex(
        UnboundedStringKey *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPluginSupport_destroy_data(
        UnboundedStringKey *sample);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPluginSupport_print_data(
        const UnboundedStringKey *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern UnboundedStringKey*
    UnboundedStringKeyPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern UnboundedStringKey*
    UnboundedStringKeyPluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPluginSupport_destroy_key_ex(
        UnboundedStringKeyKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPluginSupport_destroy_key(
        UnboundedStringKeyKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    UnboundedStringKeyPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    UnboundedStringKeyPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        UnboundedStringKey *sample,
        void *handle);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        UnboundedStringKey *out,
        const UnboundedStringKey *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const UnboundedStringKey *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr());

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        UnboundedStringKey **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_deserialize_from_cdr_buffer(
        UnboundedStringKey *sample,
        const char * buffer,
        unsigned int length);

    NDDSUSERDllExport extern unsigned int
    UnboundedStringKeyPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind
    UnboundedStringKeyPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int
    UnboundedStringKeyPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int
    UnboundedStringKeyPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        UnboundedStringKey ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        UnboundedStringKeyKeyHolder *key,
        const UnboundedStringKey *instance);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        UnboundedStringKey *instance,
        const UnboundedStringKeyKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    UnboundedStringKeyPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    UnboundedStringKeyPlugin_new(void);

    NDDSUSERDllExport extern void
    UnboundedStringKeyPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusKeyBoundaries  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _08_key_boundariesPlugin_919417863_h */
