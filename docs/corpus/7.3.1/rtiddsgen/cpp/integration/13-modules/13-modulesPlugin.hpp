

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-modules.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_modulesPlugin_745897924_h
#define _13_modulesPlugin_745897924_h

#include "13-modules.hpp"

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

namespace CorpusIntegration {
    namespace Contracts {

        #define NamespacedMessagePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

        #define NamespacedMessagePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
        #define NamespacedMessagePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

        #define NamespacedMessagePlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
        #define NamespacedMessagePlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

        /* --------------------------------------------------------------------------------------
        Support functions:
        * -------------------------------------------------------------------------------------- */

        NDDSUSERDllExport extern NamespacedMessage*
        NamespacedMessagePluginSupport_create_data_w_params(
            const struct DDS_TypeAllocationParams_t * alloc_params);

        NDDSUSERDllExport extern NamespacedMessage*
        NamespacedMessagePluginSupport_create_data_ex(RTIBool allocate_pointers);

        NDDSUSERDllExport extern NamespacedMessage*
        NamespacedMessagePluginSupport_create_data(void);

        NDDSUSERDllExport extern RTIBool 
        NamespacedMessagePluginSupport_copy_data(
            NamespacedMessage *out,
            const NamespacedMessage *in);

        NDDSUSERDllExport extern void 
        NamespacedMessagePluginSupport_destroy_data_w_params(
            NamespacedMessage *sample,
            const struct DDS_TypeDeallocationParams_t * dealloc_params);

        NDDSUSERDllExport extern void 
        NamespacedMessagePluginSupport_destroy_data_ex(
            NamespacedMessage *sample,RTIBool deallocate_pointers);

        NDDSUSERDllExport extern void 
        NamespacedMessagePluginSupport_destroy_data(
            NamespacedMessage *sample);

        NDDSUSERDllExport extern void 
        NamespacedMessagePluginSupport_print_data(
            const NamespacedMessage *sample,
            const char *desc,
            unsigned int indent);

        /* ----------------------------------------------------------------------------
        Callback functions:
        * ---------------------------------------------------------------------------- */

        NDDSUSERDllExport extern PRESTypePluginParticipantData 
        NamespacedMessagePlugin_on_participant_attached(
            void *registration_data, 
            const struct PRESTypePluginParticipantInfo *participant_info,
            RTIBool top_level_registration, 
            void *container_plugin_context,
            RTICdrTypeCode *typeCode);

        NDDSUSERDllExport extern void 
        NamespacedMessagePlugin_on_participant_detached(
            PRESTypePluginParticipantData participant_data);

        NDDSUSERDllExport extern PRESTypePluginEndpointData 
        NamespacedMessagePlugin_on_endpoint_attached(
            PRESTypePluginParticipantData participant_data,
            const struct PRESTypePluginEndpointInfo *endpoint_info,
            RTIBool top_level_registration, 
            void *container_plugin_context);

        NDDSUSERDllExport extern void 
        NamespacedMessagePlugin_on_endpoint_detached(
            PRESTypePluginEndpointData endpoint_data);

        NDDSUSERDllExport extern void    
        NamespacedMessagePlugin_return_sample(
            PRESTypePluginEndpointData endpoint_data,
            NamespacedMessage *sample,
            void *handle);    

        NDDSUSERDllExport extern RTIBool 
        NamespacedMessagePlugin_copy_sample(
            PRESTypePluginEndpointData endpoint_data,
            NamespacedMessage *out,
            const NamespacedMessage *in);

        /* ----------------------------------------------------------------------------
        (De)Serialize functions:
        * ------------------------------------------------------------------------- */

        NDDSUSERDllExport extern RTIBool
        NamespacedMessagePlugin_serialize_to_cdr_buffer(
            char * buffer,
            unsigned int * length,
            const NamespacedMessage *sample,
            ::dds::core::policy::DataRepresentationId representation
            = ::dds::core::policy::DataRepresentation::xcdr()); 

        NDDSUSERDllExport extern RTIBool 
        NamespacedMessagePlugin_deserialize(
            PRESTypePluginEndpointData endpoint_data,
            NamespacedMessage **sample, 
            RTIBool * drop_sample,
            struct RTICdrStream *cdrStream,
            RTIBool deserialize_encapsulation,
            RTIBool deserialize_sample, 
            void *endpoint_plugin_qos);

        NDDSUSERDllExport extern RTIBool
        NamespacedMessagePlugin_deserialize_from_cdr_buffer(
            NamespacedMessage *sample,
            const char * buffer,
            unsigned int length);    

        NDDSUSERDllExport extern unsigned int 
        NamespacedMessagePlugin_get_serialized_sample_max_size(
            PRESTypePluginEndpointData endpoint_data,
            RTIBool include_encapsulation,
            RTIEncapsulationId encapsulation_id,
            unsigned int current_alignment);

        /* --------------------------------------------------------------------------------------
        Key Management functions:
        * -------------------------------------------------------------------------------------- */
        NDDSUSERDllExport extern PRESTypePluginKeyKind 
        NamespacedMessagePlugin_get_key_kind(void);

        NDDSUSERDllExport extern unsigned int 
        NamespacedMessagePlugin_get_serialized_key_max_size(
            PRESTypePluginEndpointData endpoint_data,
            RTIBool include_encapsulation,
            RTIEncapsulationId encapsulation_id,
            unsigned int current_alignment);

        NDDSUSERDllExport extern unsigned int 
        NamespacedMessagePlugin_get_serialized_key_max_size_for_keyhash(
            PRESTypePluginEndpointData endpoint_data,
            RTIEncapsulationId encapsulation_id,
            unsigned int current_alignment);

        NDDSUSERDllExport extern RTIBool 
        NamespacedMessagePlugin_deserialize_key(
            PRESTypePluginEndpointData endpoint_data,
            NamespacedMessage ** sample,
            RTIBool * drop_sample,
            struct RTICdrStream *cdrStream,
            RTIBool deserialize_encapsulation,
            RTIBool deserialize_key,
            void *endpoint_plugin_qos);

        /* Plugin Functions */
        NDDSUSERDllExport extern struct PRESTypePlugin*
        NamespacedMessagePlugin_new(void);

        NDDSUSERDllExport extern void
        NamespacedMessagePlugin_delete(struct PRESTypePlugin *);

    } /* namespace Contracts  */
} /* namespace CorpusIntegration  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _13_modulesPlugin_745897924_h */

