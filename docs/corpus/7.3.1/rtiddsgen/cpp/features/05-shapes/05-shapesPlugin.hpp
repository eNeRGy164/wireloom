

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-shapes.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _05_shapesPlugin_1457046950_h
#define _05_shapesPlugin_1457046950_h

#include "05-shapes.hpp"

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

namespace CorpusCollectionShapes {

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

    #define RowPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define RowPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define RowPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define RowPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define RowPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Row*
    RowPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Row*
    RowPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Row*
    RowPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    RowPluginSupport_copy_data(
        Row *out,
        const Row *in);

    NDDSUSERDllExport extern void 
    RowPluginSupport_destroy_data_w_params(
        Row *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    RowPluginSupport_destroy_data_ex(
        Row *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    RowPluginSupport_destroy_data(
        Row *sample);

    NDDSUSERDllExport extern void 
    RowPluginSupport_print_data(
        const Row *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    RowPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Row *out,
        const Row *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    RowPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    RowPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    RowPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    RowPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define BoundedLongsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define BoundedLongsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define BoundedLongsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define BoundedLongsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define BoundedLongsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern BoundedLongs*
    BoundedLongsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern BoundedLongs*
    BoundedLongsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern BoundedLongs*
    BoundedLongsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    BoundedLongsPluginSupport_copy_data(
        BoundedLongs *out,
        const BoundedLongs *in);

    NDDSUSERDllExport extern void 
    BoundedLongsPluginSupport_destroy_data_w_params(
        BoundedLongs *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    BoundedLongsPluginSupport_destroy_data_ex(
        BoundedLongs *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    BoundedLongsPluginSupport_destroy_data(
        BoundedLongs *sample);

    NDDSUSERDllExport extern void 
    BoundedLongsPluginSupport_print_data(
        const BoundedLongs *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    BoundedLongsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        BoundedLongs *out,
        const BoundedLongs *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    BoundedLongsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    BoundedLongsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    BoundedLongsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    BoundedLongsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define RowsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define RowsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define RowsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define RowsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define RowsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Rows*
    RowsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Rows*
    RowsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Rows*
    RowsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    RowsPluginSupport_copy_data(
        Rows *out,
        const Rows *in);

    NDDSUSERDllExport extern void 
    RowsPluginSupport_destroy_data_w_params(
        Rows *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    RowsPluginSupport_destroy_data_ex(
        Rows *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    RowsPluginSupport_destroy_data(
        Rows *sample);

    NDDSUSERDllExport extern void 
    RowsPluginSupport_print_data(
        const Rows *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    RowsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Rows *out,
        const Rows *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    RowsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    RowsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    RowsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    RowsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define ItemsPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define ItemsPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define ItemsPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define ItemsPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define ItemsPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Items*
    ItemsPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Items*
    ItemsPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Items*
    ItemsPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    ItemsPluginSupport_copy_data(
        Items *out,
        const Items *in);

    NDDSUSERDllExport extern void 
    ItemsPluginSupport_destroy_data_w_params(
        Items *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    ItemsPluginSupport_destroy_data_ex(
        Items *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    ItemsPluginSupport_destroy_data(
        Items *sample);

    NDDSUSERDllExport extern void 
    ItemsPluginSupport_print_data(
        const Items *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    ItemsPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Items *out,
        const Items *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    ItemsPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    ItemsPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    ItemsPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    ItemsPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define NamesPlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define NamesPlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define NamesPlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

    #define NamesPlugin_create_sample PRESTypePluginDefaultEndpointData_createSample 
    #define NamesPlugin_destroy_sample PRESTypePluginDefaultEndpointData_deleteSample 

    /* --------------------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------------------- */

    NDDSUSERDllExport extern Names*
    NamesPluginSupport_create_data_w_params(
        const struct DDS_TypeAllocationParams_t * alloc_params);

    NDDSUSERDllExport extern Names*
    NamesPluginSupport_create_data_ex(RTIBool allocate_pointers);

    NDDSUSERDllExport extern Names*
    NamesPluginSupport_create_data(void);

    NDDSUSERDllExport extern RTIBool 
    NamesPluginSupport_copy_data(
        Names *out,
        const Names *in);

    NDDSUSERDllExport extern void 
    NamesPluginSupport_destroy_data_w_params(
        Names *sample,
        const struct DDS_TypeDeallocationParams_t * dealloc_params);

    NDDSUSERDllExport extern void 
    NamesPluginSupport_destroy_data_ex(
        Names *sample,RTIBool deallocate_pointers);

    NDDSUSERDllExport extern void 
    NamesPluginSupport_destroy_data(
        Names *sample);

    NDDSUSERDllExport extern void 
    NamesPluginSupport_print_data(
        const Names *sample,
        const char *desc,
        unsigned int indent);

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    NDDSUSERDllExport extern RTIBool 
    NamesPlugin_copy_sample(
        PRESTypePluginEndpointData endpoint_data,
        Names *out,
        const Names *in);

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */

    NDDSUSERDllExport extern unsigned int 
    NamesPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */
    NDDSUSERDllExport extern PRESTypePluginKeyKind 
    NamesPlugin_get_key_kind(void);

    NDDSUSERDllExport extern unsigned int 
    NamesPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    NDDSUSERDllExport extern unsigned int 
    NamesPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    #define SamplePlugin_get_sample PRESTypePluginDefaultEndpointData_getSample

    #define SamplePlugin_get_buffer PRESTypePluginDefaultEndpointData_getBuffer 
    #define SamplePlugin_return_buffer PRESTypePluginDefaultEndpointData_returnBuffer

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

    /* Plugin Functions */
    NDDSUSERDllExport extern struct PRESTypePlugin*
    SamplePlugin_new(void);

    NDDSUSERDllExport extern void
    SamplePlugin_delete(struct PRESTypePlugin *);

} /* namespace CorpusCollectionShapes  */

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif /* _05_shapesPlugin_1457046950_h */

