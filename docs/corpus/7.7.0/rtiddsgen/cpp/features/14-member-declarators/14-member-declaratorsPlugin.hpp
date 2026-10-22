

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-member-declarators.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_member_declaratorsPlugin_544658739_h
#define _14_member_declaratorsPlugin_544658739_h

#include "14-member-declarators.hpp"

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

/* The type used to store keys for instances of type struct
* AnotherSimple.
*
* By default, this type is struct Declarators
* itself. However, if for some reason this choice is not practical for your
* system (e.g. if sizeof(struct Declarators)
* is very large), you may redefine this typedef in terms of another type of
* your choosing. HOWEVER, if you define the KeyHolder type to be something
* other than struct AnotherSimple, the
* following restriction applies: the key of struct
* Declarators must consist of a
* single field of your redefined KeyHolder type and that field must be the
* first field in struct Declarators.
*/
typedef struct Declarators DeclaratorsKeyHolder;

#define DeclaratorsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

#define DeclaratorsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer
#define DeclaratorsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

#define DeclaratorsPlugin_get_key PRESTypePluginDefaultEndpointData_getKey
#define DeclaratorsPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

#define DeclaratorsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample
#define DeclaratorsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample

/* --------------------------------------------------------------------------------------
Support functions:
* -------------------------------------------------------------------------------------- */

NDDSUSERDllExport extern Declarators*
DeclaratorsPluginSupport_create_data_w_params(
    const struct DDS_TypeAllocationParams_t * alloc_params);

NDDSUSERDllExport extern Declarators*
DeclaratorsPluginSupport_create_data_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Declarators*
DeclaratorsPluginSupport_create_data(void);

NDDSUSERDllExport extern RTIBool
DeclaratorsPluginSupport_copy_data(
    Declarators *out,
    const Declarators *in);

NDDSUSERDllExport extern void
DeclaratorsPluginSupport_destroy_data_w_params(
    Declarators *sample,
    const struct DDS_TypeDeallocationParams_t * dealloc_params);

NDDSUSERDllExport extern void
DeclaratorsPluginSupport_destroy_data_ex(
    Declarators *sample,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
DeclaratorsPluginSupport_destroy_data(
    Declarators *sample);

NDDSUSERDllExport extern void
DeclaratorsPluginSupport_print_data(
    const Declarators *sample,
    const char *desc,
    unsigned int indent);

NDDSUSERDllExport extern Declarators*
DeclaratorsPluginSupport_create_key_ex(RTIBool allocate_pointers);

NDDSUSERDllExport extern Declarators*
DeclaratorsPluginSupport_create_key(void);

NDDSUSERDllExport extern void
DeclaratorsPluginSupport_destroy_key_ex(
    DeclaratorsKeyHolder *key,RTIBool deallocate_pointers);

NDDSUSERDllExport extern void
DeclaratorsPluginSupport_destroy_key(
    DeclaratorsKeyHolder *key);

/* ----------------------------------------------------------------------------
Callback functions:
* ---------------------------------------------------------------------------- */

NDDSUSERDllExport extern PRESTypePluginParticipantData
DeclaratorsPlugin_on_participant_attached(
    void *registration_data,
    const struct PRESTypePluginParticipantInfo *participant_info,
    RTIBool top_level_registration,
    void *container_plugin_context,
    RTICdrTypeCode *typeCode);

NDDSUSERDllExport extern void
DeclaratorsPlugin_on_participant_detached(
    PRESTypePluginParticipantData participant_data);

NDDSUSERDllExport extern PRESTypePluginEndpointData
DeclaratorsPlugin_on_endpoint_attached(
    PRESTypePluginParticipantData participant_data,
    const struct PRESTypePluginEndpointInfo *endpoint_info,
    RTIBool top_level_registration,
    void *container_plugin_context);

NDDSUSERDllExport extern void
DeclaratorsPlugin_on_endpoint_detached(
    PRESTypePluginEndpointData endpoint_data);

NDDSUSERDllExport extern void
DeclaratorsPlugin_return_sample(
    PRESTypePluginEndpointData endpoint_data,
    Declarators *sample,
    void *handle);

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_copy_sample(
    PRESTypePluginEndpointData endpoint_data,
    Declarators *out,
    const Declarators *in);

/* ----------------------------------------------------------------------------
(De)Serialize functions:
* ------------------------------------------------------------------------- */

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_serialize_to_cdr_buffer(
    char * buffer,
    unsigned int * length,
    const Declarators *sample,
    ::dds::core::policy::DataRepresentationId representation
    = ::dds::core::policy::DataRepresentation::xcdr());

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_deserialize(
    PRESTypePluginEndpointData endpoint_data,
    Declarators **sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_sample,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_deserialize_from_cdr_buffer(
    Declarators *sample,
    const char * buffer,
    unsigned int length);

NDDSUSERDllExport extern unsigned int
DeclaratorsPlugin_get_serialized_sample_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

/* --------------------------------------------------------------------------------------
Key Management functions:
* -------------------------------------------------------------------------------------- */
NDDSUSERDllExport extern PRESTypePluginKeyKind
DeclaratorsPlugin_get_key_kind(void);

NDDSUSERDllExport extern unsigned int
DeclaratorsPlugin_get_serialized_key_max_size(
    PRESTypePluginEndpointData endpoint_data,
    RTIBool include_encapsulation,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern unsigned int
DeclaratorsPlugin_get_serialized_key_max_size_for_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    RTIEncapsulationId encapsulation_id,
    unsigned int current_alignment);

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_deserialize_key(
    PRESTypePluginEndpointData endpoint_data,
    Declarators ** sample,
    RTIBool * drop_sample,
    struct RTICdrStream *cdrStream,
    RTIBool deserialize_encapsulation,
    RTIBool deserialize_key,
    void *endpoint_plugin_qos);

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_instance_to_key(
    PRESTypePluginEndpointData endpoint_data,
    DeclaratorsKeyHolder *key,
    const Declarators *instance);

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_key_to_instance(
    PRESTypePluginEndpointData endpoint_data,
    Declarators *instance,
    const DeclaratorsKeyHolder *key);

NDDSUSERDllExport extern RTIBool
DeclaratorsPlugin_serialized_sample_to_keyhash(
    PRESTypePluginEndpointData endpoint_data,
    struct RTICdrStream *cdrStream,
    DDS_KeyHash_t *keyhash,
    RTIBool deserialize_encapsulation,
    void *endpoint_plugin_qos);

/* Plugin Functions */
NDDSUSERDllExport extern struct PRESTypePlugin*
DeclaratorsPlugin_new(void);

NDDSUSERDllExport extern void
DeclaratorsPlugin_delete(struct PRESTypePlugin *);

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _14_member_declaratorsPlugin_544658739_h */
