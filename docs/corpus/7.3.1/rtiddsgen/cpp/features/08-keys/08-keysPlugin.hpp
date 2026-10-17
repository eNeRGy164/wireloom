

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-keys.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_keysPlugin_1982703403_h
#define _08_keysPlugin_1982703403_h

#include "08-keys.hpp"

struct RTICdrStream;

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

namespace CorpusKeys {

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Simple
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Simple)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Simple must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Simple.
    */
    typedef class Simple SimpleKeyHolder;

    #define SimplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define SimplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define SimplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define SimplePlugin_get_key PRESTypePluginDefaultEndpointData_getKey 
    #define SimplePlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define SimplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define SimplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Simple*
    SimplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Simple*
    SimplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Simple*
    SimplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    SimplePluginSupport_copy_data(
        Simple *out,
        const Simple *in);

    NDDSUSERDllExport extern void 
    SimplePluginSupport_destroy_data_w_params(
        Simple *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    SimplePluginSupport_destroy_data_ex(
        Simple *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    SimplePluginSupport_destroy_data(
        Simple *sample);

    NDDSUSERDllExport extern void 
    SimplePluginSupport_print_data(
        const Simple *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern Simple*
    SimplePluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Simple*
    SimplePluginSupport_create_key(void);

    NDDSUSERDllExport extern void 
    SimplePluginSupport_destroy_key_ex(
        SimpleKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    SimplePluginSupport_destroy_key(
        SimpleKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    SimplePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    SimplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    SimplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    SimplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    SimplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Simple *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    SimplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Simple *out,
        const Simple *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    SimplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Simple *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    SimplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Simple **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    SimplePlugin_deserialize_from_cdr_buffer(
        Simple *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    SimplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    SimplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    SimplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    SimplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    SimplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Simple ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool 
    SimplePlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        SimpleKeyHolder *key, 
        const Simple *instance);

    NDDSUSERDllExport extern RTIBool 
    SimplePlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Simple *instance, 
        const SimpleKeyHolder *key);

    NDDSUSERDllExport extern RTIBool 
    SimplePlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream, 
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos); 

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    SimplePlugin_new(void);

    NDDSUSERDllExport extern void
    SimplePlugin_delete(struct PRESTypePlugin *);

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Identity
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Identity)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Identity must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Identity.
    */
    typedef class Identity IdentityKeyHolder;

    #define IdentityPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define IdentityPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define IdentityPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define IdentityPlugin_get_key PRESTypePluginDefaultEndpointData_getKey 
    #define IdentityPlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define IdentityPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define IdentityPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Identity*
    IdentityPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Identity*
    IdentityPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Identity*
    IdentityPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    IdentityPluginSupport_copy_data(
        Identity *out,
        const Identity *in);

    NDDSUSERDllExport extern void 
    IdentityPluginSupport_destroy_data_w_params(
        Identity *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    IdentityPluginSupport_destroy_data_ex(
        Identity *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    IdentityPluginSupport_destroy_data(
        Identity *sample);

    NDDSUSERDllExport extern void 
    IdentityPluginSupport_print_data(
        const Identity *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern Identity*
    IdentityPluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Identity*
    IdentityPluginSupport_create_key(void);

    NDDSUSERDllExport extern void 
    IdentityPluginSupport_destroy_key_ex(
        IdentityKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    IdentityPluginSupport_destroy_key(
        IdentityKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    IdentityPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    IdentityPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    IdentityPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    IdentityPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    IdentityPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Identity *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    IdentityPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Identity *out,
        const Identity *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    IdentityPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Identity *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    IdentityPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Identity **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    IdentityPlugin_deserialize_from_cdr_buffer(
        Identity *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    IdentityPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    IdentityPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    IdentityPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    IdentityPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    IdentityPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Identity ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool 
    IdentityPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        IdentityKeyHolder *key, 
        const Identity *instance);

    NDDSUSERDllExport extern RTIBool 
    IdentityPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Identity *instance, 
        const IdentityKeyHolder *key);

    NDDSUSERDllExport extern RTIBool 
    IdentityPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream, 
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos); 

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    IdentityPlugin_new(void);

    NDDSUSERDllExport extern void
    IdentityPlugin_delete(struct PRESTypePlugin *);

    /* The type used to store keys for instances of type struct
    * AnotherSimple.
    *
    * By default, this type is struct Sample
    * itself. However, if for some reason this choice is not practical for your
    * system (e.g. if sizeof(struct Sample)
    * is very large), you may redefine this typedef in terms of another type of
    * your choosing. HOWEVER, if you define the KeyHolder type to be something
    * other than struct AnotherSimple, the
    * following restriction applies: the key of struct
    * Sample must consist of a
    * single field of your redefined KeyHolder type and that field must be the
    * first field in struct Sample.
    */
    typedef class Sample SampleKeyHolder;

    #define SamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define SamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define SamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define SamplePlugin_get_key PRESTypePluginDefaultEndpointData_getKey 
    #define SamplePlugin_return_key PRESTypePluginDefaultEndpointData_returnKey

    #define SamplePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define SamplePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    SamplePluginSupport_copy_data(
        Sample *out,
        const Sample *in);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_data_w_params(
        Sample *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_data_ex(
        Sample *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_data(
        Sample *sample);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_print_data(
        const Sample *sample,
        const char *desc,
        unsigned int indent);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_key_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Sample*
    SamplePluginSupport_create_key(void);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_key_ex(
        SampleKeyHolder *key,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    SamplePluginSupport_destroy_key(
        SampleKeyHolder *key);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    SamplePlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    SamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    SamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    SamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    SamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *out,
        const Sample *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Sample *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Sample **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    SamplePlugin_deserialize_from_cdr_buffer(
        Sample *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    SamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    SamplePlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    SamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    SamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Sample ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        SampleKeyHolder *key, 
        const Sample *instance);

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Sample *instance, 
        const SampleKeyHolder *key);

    NDDSUSERDllExport extern RTIBool 
    SamplePlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream, 
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos); 

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    SamplePlugin_new(void);

    NDDSUSERDllExport extern void
    SamplePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusKeys  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _08_keysPlugin_1982703403_h */

