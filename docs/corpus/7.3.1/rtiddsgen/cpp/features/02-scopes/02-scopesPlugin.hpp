

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-scopes.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _02_scopesPlugin_1527551736_h
#define _02_scopesPlugin_1527551736_h

#include "02-scopes.hpp"

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

namespace CorpusScopes {
    namespace Nested {

        #define PointPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

        #define PointPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
        #define PointPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

        #define PointPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
        #define PointPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

        /* --------------------------------------------------------------------------------------
        Support functions:
        * -------------------------------------------------------------------------------------- */

        NDDSUSERDllExport extern Point*
        PointPluginSupport_create_data_w_params(
            const struct DDS_TypeAllocationParams_t * alloc_params);

        NDDSUSERDllExport extern Point*
        PointPluginSupport_create_data_ex(RTIBool allocate_pointers);

        NDDSUSERDllExport extern Point*
        PointPluginSupport_create_data(void);

        NDDSUSERDllExport extern RTIBool 
        PointPluginSupport_copy_data(
            Point *out,
            const Point *in);

        NDDSUSERDllExport extern void 
        PointPluginSupport_destroy_data_w_params(
            Point *sample,
            const struct DDS_TypeDeallocationParams_t * dealloc_params);

        NDDSUSERDllExport extern void 
        PointPluginSupport_destroy_data_ex(
            Point *sample,RTIBool deallocate_pointers);

        NDDSUSERDllExport extern void 
        PointPluginSupport_destroy_data(
            Point *sample);

        NDDSUSERDllExport extern void 
        PointPluginSupport_print_data(
            const Point *sample,
            const char *desc,
            unsigned int indent);

        /* ----------------------------------------------------------------------------
        Callback functions:
        * ---------------------------------------------------------------------------- */

        NDDSUSERDllExport extern PRESTypePluginParticipantData 
        PointPlugin_on_participant_attached(
            void *registration_data, 
            const struct PRESTypePluginParticipantInfo *participant_info,
            RTIBool top_level_registration, 
            void *container_plugin_context,
            RTICdrTypeCode *typeCode);

        NDDSUSERDllExport extern void 
        PointPlugin_on_participant_detached(
            PRESTypePluginParticipantData participant_data);

        NDDSUSERDllExport extern PRESTypePluginEndpointData 
        PointPlugin_on_endpoint_attached(
            PRESTypePluginParticipantData participant_data,
            const struct PRESTypePluginEndpointInfo *endpoint_info,
            RTIBool top_level_registration, 
            void *container_plugin_context);

        NDDSUSERDllExport extern void 
        PointPlugin_on_endpoint_detached(
            PRESTypePluginEndpointData endpoint_data);

        NDDSUSERDllExport extern void    
        PointPlugin_return_sample(
            PRESTypePluginEndpointData endpoint_data,
            Point *sample,
            void *handle);    

        NDDSUSERDllExport extern RTIBool 
        PointPlugin_copy_sample(
            PRESTypePluginEndpointData endpoint_data,
            Point *out,
            const Point *in);

        /* ----------------------------------------------------------------------------
        (De)Serialize functions:
        * ------------------------------------------------------------------------- */

        NDDSUSERDllExport extern RTIBool
        PointPlugin_serialize_to_cdr_buffer(
            char * buffer,
            unsigned int * length,
            const Point *sample,
            ::dds::core::policy::DataRepresentationId representation
            = ::dds::core::policy::DataRepresentation::xcdr()); 

        NDDSUSERDllExport extern RTIBool 
        PointPlugin_deserialize(
            PRESTypePluginEndpointData endpoint_data,
            Point **sample, 
            RTIBool * drop_sample,
            struct RTICdrStream *cdrStream,
            RTIBool deserialize_encapsulation,
            RTIBool deserialize_sample, 
            void *endpoint_plugin_qos);

        NDDSUSERDllExport extern RTIBool
        PointPlugin_deserialize_from_cdr_buffer(
            Point *sample,
            const char * buffer,
            unsigned int length);    

        NDDSUSERDllExport extern unsigned int 
        PointPlugin_get_serialized_sample_max_size(
            PRESTypePluginEndpointData endpoint_data,
            RTIBool include_encapsulation,
            RTIEncapsulationId encapsulation_id,
            unsigned int current_alignment);

        /* --------------------------------------------------------------------------------------
        Key Management functions:
        * -------------------------------------------------------------------------------------- */
        NDDSUSERDllExport extern PRESTypePluginKeyKind 
        PointPlugin_get_key_kind(void);

        NDDSUSERDllExport extern unsigned int 
        PointPlugin_get_serialized_key_max_size(
            PRESTypePluginEndpointData endpoint_data,
            RTIBool include_encapsulation,
            RTIEncapsulationId encapsulation_id,
            unsigned int current_alignment);

        NDDSUSERDllExport extern unsigned int 
        PointPlugin_get_serialized_key_max_size_for_keyhash(
            PRESTypePluginEndpointData endpoint_data,
            RTIEncapsulationId encapsulation_id,
            unsigned int current_alignment);

        NDDSUSERDllExport extern RTIBool 
        PointPlugin_deserialize_key(
            PRESTypePluginEndpointData endpoint_data,
            Point ** sample,
            RTIBool * drop_sample,
            struct RTICdrStream *cdrStream,
            RTIBool deserialize_encapsulation,
            RTIBool deserialize_key,
            void *endpoint_plugin_qos);

        /* Plugin Functions */
        NDDSUSERDllExport extern struct PRESTypePlugin*
        PointPlugin_new(void);

        NDDSUSERDllExport extern void
        PointPlugin_delete(struct PRESTypePlugin *);

    } /* namespace Nested  */

    #define KeywordRecordPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define KeywordRecordPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define KeywordRecordPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define KeywordRecordPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define KeywordRecordPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern KeywordRecord*
    KeywordRecordPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern KeywordRecord*
    KeywordRecordPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern KeywordRecord*
    KeywordRecordPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    KeywordRecordPluginSupport_copy_data(
        KeywordRecord *out,
        const KeywordRecord *in);

    NDDSUSERDllExport extern void 
    KeywordRecordPluginSupport_destroy_data_w_params(
        KeywordRecord *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    KeywordRecordPluginSupport_destroy_data_ex(
        KeywordRecord *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    KeywordRecordPluginSupport_destroy_data(
        KeywordRecord *sample);

    NDDSUSERDllExport extern void 
    KeywordRecordPluginSupport_print_data(
        const KeywordRecord *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern PRESTypePluginParticipantData 
    KeywordRecordPlugin_on_participant_attached(
        void *registration_data, 
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration, 
        void *container_plugin_context,
        RTICdrTypeCode *typeCode);

    NDDSUSERDllExport extern void 
    KeywordRecordPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data);

    NDDSUSERDllExport extern PRESTypePluginEndpointData 
    KeywordRecordPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration, 
        void *container_plugin_context);

    NDDSUSERDllExport extern void 
    KeywordRecordPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data);

    NDDSUSERDllExport extern void    
    KeywordRecordPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        KeywordRecord *sample,
        void *handle);    

    NDDSUSERDllExport extern RTIBool 
    KeywordRecordPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        KeywordRecord *out,
        const KeywordRecord *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool
    KeywordRecordPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const KeywordRecord *sample,
        ::dds::core::policy::DataRepresentationId representation
        = ::dds::core::policy::DataRepresentation::xcdr()); 

    NDDSUSERDllExport extern RTIBool 
    KeywordRecordPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        KeywordRecord **sample, 
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample, 
        void *endpoint_plugin_qos);

    NDDSUSERDllExport extern RTIBool
    KeywordRecordPlugin_deserialize_from_cdr_buffer(
        KeywordRecord *sample,
        const char * buffer,
        unsigned int length);    

    NDDSUSERDllExport extern unsigned int 
    KeywordRecordPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    KeywordRecordPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    KeywordRecordPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    KeywordRecordPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern RTIBool 
    KeywordRecordPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        KeywordRecord ** sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos);

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    KeywordRecordPlugin_new(void);

    NDDSUSERDllExport extern void
    KeywordRecordPlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusScopes  */
namespace CorpusScopes {

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

} /* namespace CorpusScopes  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _02_scopesPlugin_1527551736_h */

