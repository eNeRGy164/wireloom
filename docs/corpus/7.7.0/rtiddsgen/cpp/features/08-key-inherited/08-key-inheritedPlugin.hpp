

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-inherited.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_inheritedPlugin_663625449_h
#define _08_key_inheritedPlugin_663625449_h

#include "08-key-inherited.hpp"

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

namespace CorpusInheritedKeys {

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Base
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Base)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Base must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Base.
    */
    typedef struct Base BaseKeyHolder;

    #define BasePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BasePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define BasePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BasePlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define BasePlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

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

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Base*
    BasePluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    BasePluginSupport_destroy_key_ex(
        BaseKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    BasePluginSupport_destroy_key(
        BaseKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData
    BasePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void
    BasePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData
    BasePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *container_plugin_context);

    NDDSUSERDllExport extern void
    BasePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void
    BasePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Base *sample,
        void *handle);

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

    NDDSUSERDllExport extern RTIBool
    BasePlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        BaseKeyHolder *key,
        const Base *instance);

    NDDSUSERDllExport extern RTIBool
    BasePlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Base *instance,
        const BaseKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    BasePlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    BasePlugin_new(void);

    NDDSUSERDllExport extern void
    BasePlugin_delete(struct PRESTypePlugin *);

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Derived
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Derived)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Derived must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Derived.
    */
    typedef struct Derived DerivedKeyHolder;

    #define DerivedPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define DerivedPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
    #define DerivedPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define DerivedPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
    #define DerivedPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

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

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Derived*
    DerivedPluginSupport_create_key(void);

    NDDSUSERDllExport extern void
    DerivedPluginSupport_destroy_key_ex(
        DerivedKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void
    DerivedPluginSupport_destroy_key(
        DerivedKeyHolder *key);

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

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        DerivedKeyHolder *key,
        const Derived *instance);

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Derived *instance,
        const DerivedKeyHolder *key);

    NDDSUSERDllExport extern RTIBool
    DerivedPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    DerivedPlugin_new(void);

    NDDSUSERDllExport extern void
    DerivedPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusInheritedKeys  */

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _08_key_inheritedPlugin_663625449_h */
