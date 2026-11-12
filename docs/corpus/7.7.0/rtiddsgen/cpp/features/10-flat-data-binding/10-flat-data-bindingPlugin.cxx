

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-flat-data-binding.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <string.h>

#ifndef ndds_c_h
#include "ndds/ndds_c.h"
#endif

#ifndef osapi_type_h
#include "osapi/osapi_type.h"
#endif
#ifndef osapi_heap_h
#include "osapi/osapi_heap.h"
#endif

#ifndef osapi_utility_h
#include "osapi/osapi_utility.h"
#endif

#ifndef cdr_log_h
#include "cdr/cdr_log.h"
#endif

#ifndef cdr_type_h
#include "cdr/cdr_type.h"
#endif

#ifndef cdr_type_object_h
#include "cdr/cdr_typeObject.h"
#endif

#ifndef cdr_encapsulation_h
#include "cdr/cdr_encapsulation.h"
#endif

#ifndef cdr_stream_h
#include "cdr/cdr_stream.h"
#endif

#ifndef cdr_log_h
#include "cdr/cdr_log.h"
#endif

#ifndef pres_typePlugin_h
#include "pres/pres_typePlugin.h"
#endif

#include "dds_c/dds_c_typecode_impl.h"

#include "rti/topic/cdr/Serialization.hpp"
#define RTI_CDR_CURRENT_SUBMODULE RTI_CDR_SUBMODULE_MASK_STREAM

#include "10-flat-data-bindingPlugin.hpp"

namespace CorpusFlatDataBinding {

    /* ----------------------------------------------------------------------------
    *  Type Sample
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    SamplePluginSupport_create_dataI(void)
    {
        return Sample::create_data();
    }

    Sample *
    SamplePluginSupport_create_data(void)
    {
        return (Sample *) SamplePluginSupport_create_dataI();
    }

    void
    SamplePluginSupport_destroy_data(
        Sample *sample)
    {
        Sample::delete_data(sample);
    }

    void
    SamplePluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusFlatDataBinding::SamplePluginSupport_destroy_data((Sample *) sample);
    }

    RTIBool
    SamplePluginSupport_copy_data(
        Sample *dst,
        const Sample *src)
    {
        RTIOsapiUtility_unusedParameter(dst);
        RTIOsapiUtility_unusedParameter(src);

        return RTI_FALSE;
    }

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    RTI_UINT32
    SamplePlugin_get_deserialized_sample_max_size(
        PRESTypePluginEndpointData epd)
    {
        RTIOsapiUtility_unusedParameter(epd);
        return RTI_XCDR_ENCAPSULATION_HEADER_SIZE
        + RTIOsapiAlignment_alignUInt32Up(
            SampleOffset::serialized_size(0),
            MIG_RTPS_SUBMESSAGE_ALIGNMENT);
    }

    RTIBool SamplePlugin_get_buffer_w_params(
        PRESTypePluginEndpointData endpointData,
        struct REDABuffer *buffer,
        const void *user_data,
        const struct NDDS_WriterHistory_AllocatorParams *allocatorParams)
    {
        if ((allocatorParams->sampleKind == NDDS_WRITERHISTORY_DATA_SAMPLE)
        && !allocatorParams->forceUseWHPoolWithFlatData) {
            buffer->pointer = (char *) user_data; // FLAT_DATA: the sample is the buffer
            buffer->length = static_cast<int>(RTIOsapiAlignment_alignUInt32Up(
                SampleOffset::serialized_size(0),
                MIG_RTPS_SUBMESSAGE_ALIGNMENT)) 
                + RTI_XCDR_ENCAPSULATION_HEADER_SIZE;
            return RTI_TRUE;
        } else {
            return PRESTypePluginDefaultEndpointData_getBuffer(
                endpointData, buffer, allocatorParams->encapsulationId, user_data);
        }
    }

    void SamplePlugin_return_buffer_w_params(
        PRESTypePluginEndpointData endpointData,
        struct REDABuffer *buffer,
        const struct NDDS_WriterHistory_AllocatorParams *allocatorParams)
    {
        if ((allocatorParams->sampleKind != NDDS_WRITERHISTORY_DATA_SAMPLE)
        || allocatorParams->forceUseWHPoolWithFlatData) {
            return PRESTypePluginDefaultEndpointData_returnBuffer(
                endpointData, buffer, allocatorParams->encapsulationId);
        }

        // FLAT_DATA : nothing to do for NDDS_WRITERHISTORY_DATA_SAMPLE
    }

    void *
    SamplePlugin_create_flat_data_sentinel(void)
    {
        return (Sample*)PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS;
    }

    void
    SamplePlugin_destroy_flat_data_sentinel(void* sample)
    {
        RTIOsapiUtility_unusedParameter(sample);
    }

    void*
    SamplePlugin_get_sample_flat_data(
        PRESTypePluginEndpointData endpointData,
        void **handle)
    {
        return PRESTypePluginDefaultEndpointData_getSample(endpointData,handle);
    }

    void
    SamplePlugin_return_sample_flat_data(
        PRESTypePluginEndpointData endpointData,
        void *sample, 
        void *handle)
    {
        struct REDAFastBufferPool *pool = PRESTypePluginDefaultEndpointData_getFstSampleDataPool(endpointData);
        if (pool && (sample != reinterpret_cast<void *>(PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS))) {
            REDAFastBufferPool_returnBuffer(pool, sample);
            if (handle != NULL) {
                *static_cast<void**>(handle) =
                reinterpret_cast<void *>(PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS);
            }
        }
        if (handle != NULL) {
            PRESTypePluginDefaultEndpointData_returnSample(endpointData,sample,handle);
        }
    }

    PRESTypePluginParticipantData
    SamplePlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *type_code)
    {
        try {
            struct RTIXCdrInterpreterPrograms *programs = NULL;
            struct PRESTypePluginDefaultParticipantData *pd = NULL;
            struct RTIXCdrInterpreterProgramsGenProperty programProperty =
            RTIXCdrInterpreterProgramsGenProperty_INITIALIZER;
            RTIOsapiUtility_unusedParameter(registration_data);
            RTIOsapiUtility_unusedParameter(participant_info);
            RTIOsapiUtility_unusedParameter(top_level_registration);
            RTIOsapiUtility_unusedParameter(container_plugin_context);
            RTIOsapiUtility_unusedParameter(type_code);

            if (!RTIXCdrXTypesComplianceMask_verifyGeneratedXTypesMask(0x0000018C)) {
                return NULL;
            }

            pd = (struct PRESTypePluginDefaultParticipantData *)
            PRESTypePluginDefaultParticipantData_new(participant_info);

            programProperty.generateV1Encapsulation = RTI_XCDR_FALSE;
            programProperty.generateV2Encapsulation = RTI_XCDR_TRUE;
            programProperty.resolveAlias = RTI_XCDR_TRUE;
            programProperty.inlineStruct = RTI_XCDR_TRUE;
            programProperty.optimizeEnum = RTI_XCDR_TRUE;
            programProperty.unboundedSize = RTIXCdrLong_MAX;

            programProperty.externalReferenceSize =
            (RTIXCdrUnsignedShort) sizeof(::dds::core::external<char>);
            programProperty.getExternalRefPointerFcn =
            ::rti::topic::interpreter::get_external_value_pointer;

            programs = DDS_TypeCodeFactory_assert_programs_in_global_list(
                DDS_TypeCodeFactory_get_instance(),
                (DDS_TypeCode *)&::rti::topic::dynamic_type< SamplePlainHelper>::get().native(),
                &programProperty,
                RTI_XCDR_PROGRAM_MASK_TYPEPLUGIN);

            if (programs == NULL) {
                PRESTypePluginDefaultParticipantData_delete(
                    (PRESTypePluginParticipantData)pd);
                return NULL;
            }

            pd->programs = programs;
            return (PRESTypePluginParticipantData)pd;
        } catch (...) {
            return NULL;
        }
    }

    void
    SamplePlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data)
    {
        if (participant_data != NULL) {
            struct PRESTypePluginDefaultParticipantData *pd =
            (struct PRESTypePluginDefaultParticipantData *)participant_data;

            if (pd->programs != NULL) {
                DDS_TypeCodeFactory_remove_programs_from_global_list(
                    DDS_TypeCodeFactory_get_instance(),
                    pd->programs);
                pd->programs = NULL;
            }
            PRESTypePluginDefaultParticipantData_delete(participant_data);
        }
    }

    PRESTypePluginEndpointData
    SamplePlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *containerPluginContext)
    {
        try {
            PRESTypePluginEndpointData epd = NULL;
            unsigned int serializedSampleMaxSize = 0;

            RTIOsapiUtility_unusedParameter(top_level_registration);
            RTIOsapiUtility_unusedParameter(containerPluginContext);

            if (participant_data == NULL) {
                return NULL;
            }

            if (endpoint_info->usesFstSampleDataPool) {
                epd = PRESTypePluginDefaultEndpointData_new(
                    participant_data,
                    endpoint_info,
                    SamplePlugin_create_flat_data_sentinel,
                    SamplePlugin_destroy_flat_data_sentinel,
                    NULL , NULL );
            } else {
                epd = PRESTypePluginDefaultEndpointData_new(
                    participant_data,
                    endpoint_info,
                    SamplePluginSupport_create_dataI,
                    SamplePluginSupport_destroy_dataI,
                    NULL , NULL );
            }

            if (epd == NULL) {
                return NULL;
            }

            serializedSampleMaxSize = ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_max_size(
                epd,RTI_TRUE,RTI_XCDR_ENCAPSULATION_ID_PL_CDR2_BE,0);

            PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(epd, serializedSampleMaxSize);

            if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER) {

                if (PRESTypePluginDefaultEndpointData_createWriterPool(
                    epd,
                    endpoint_info,
                    (PRESTypePluginGetSerializedSampleMaxSizeFunction)
                    ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_max_size, epd,
                    (PRESTypePluginGetSerializedSampleSizeFunction)
                    ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_size,
                    epd) == RTI_FALSE) {
                    PRESTypePluginDefaultEndpointData_delete(epd);
                    return NULL;
                }
            }

            return epd;
        } catch (...) {
            return NULL;
        }
    }

    void
    SamplePlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data)
    {
        PRESTypePluginDefaultEndpointData_delete(endpoint_data);
    }

    void
    SamplePlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Sample *sample,
        void *handle)
    {
        try {
            ::rti::topic::reset_sample(*sample);
        } catch(const std::exception& ex) {
            RTICdrLog_logWithFunctionName(
                RTI_LOG_BIT_EXCEPTION,
                "SamplePlugin_return_sample",
                &RTI_LOG_ANY_FAILURE_ss,
                "exception: ",
                ex.what());
        }

        PRESTypePluginDefaultEndpointData_returnSample(
            endpoint_data, sample, handle);
    }

    RTIBool
    SamplePlugin_copy_sample(
        PRESTypePluginEndpointData,
        Sample *dst,
        const Sample *src)
    {
        return ::CorpusFlatDataBinding::SamplePluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    SamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    SamplePlugin_serialize(
        PRESTypePluginEndpointData endpoint_data,
        const void *s,
        struct RTICdrStream *cdrStream,
        RTIBool serialize_encapsulation,
        RTIEncapsulationId encapsulation_id,
        RTIBool serialize_sample,
        void *endpoint_plugin_qos)
    {
        const Sample *sample =
        (const Sample *) s;
        RTIXCdrStream *xcdrStream = RTICdrStream_asXCdrStream(cdrStream);
        RTIXCdrUnsignedLong sampleSize = 0;

        char * rti_position = NULL;

        if(serialize_encapsulation) {
            if (!RTICdrStream_serializeAndSetCdrEncapsulation(cdrStream , encapsulation_id)) {
                return RTI_FALSE;
            }
            rti_position = RTICdrStream_resetAlignment(cdrStream);
        }

        RTIOsapiUtility_unusedParameter(endpoint_data);
        RTIOsapiUtility_unusedParameter(endpoint_plugin_qos);

        if (RTIXCdrFlatSample_getEncapsulation(
            reinterpret_cast<const char *>(sample)) != encapsulation_id) {
            return RTI_FALSE;
        }

        if (serialize_sample) {
            sampleSize = SampleOffset::serialized_size(0);
            if (!RTIXCdrStream_skipNByte(
                xcdrStream,
                1,
                sampleSize)) {
                return RTI_FALSE;
            }

        }
        if (serialize_encapsulation) {
            RTICdrStream_restoreAlignment(cdrStream, rti_position);
        }
        return RTI_TRUE;

    }

    RTIBool
    SamplePlugin_deserialize_sample(
        PRESTypePluginEndpointData endpoint_data,
        void *s,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos)
    {
        Sample *sample = (Sample *)s;
        RTIOsapiUtility_unusedParameter(endpoint_data);
        RTIOsapiUtility_unusedParameter(sample);
        RTIOsapiUtility_unusedParameter(cdrStream);
        RTIOsapiUtility_unusedParameter(deserialize_encapsulation);
        RTIOsapiUtility_unusedParameter(deserialize_sample);
        RTIOsapiUtility_unusedParameter(endpoint_plugin_qos);

        return RTI_FALSE;

    }

    RTIBool
    SamplePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Sample *sample,
        ::dds::core::policy::DataRepresentationId representation)
    {
        if (representation == ::dds::core::policy::DataRepresentation::xcdr()) {
            // Flat Data only supports XCDR2
            RTICdrLog_logWithFunctionName(
                RTI_LOG_BIT_EXCEPTION,
                "SamplePlugin_serialize_to_cdr_buffer",
                &RTI_CDR_LOG_SERIALIZE_FAILURE_s,
                "representation id parameter must be XCDR2");
            return RTI_FALSE;
        }
        if (length == NULL) {
            return RTI_FALSE;
        }

        const unsigned int serialized_size = static_cast<unsigned int>(
            sample->buffer_size());

        if (buffer == NULL) {
            *length = serialized_size;
            return RTI_TRUE;
        }

        if (*length < serialized_size) {
            return RTI_FALSE;
        }

        RTIOsapiMemory_copy(buffer, sample->get_buffer(), serialized_size);

        return RTI_TRUE;
    }

    RTIBool
    SamplePlugin_deserialize_from_cdr_buffer(
        Sample *sample,
        const char * buffer,
        unsigned int length)
    {
        struct RTICdrStream cdrStream;
        struct PRESTypePluginDefaultParticipantData pd;
        struct RTIXCdrTypePluginProgramContext defaultProgramContext =
        RTIXCdrTypePluginProgramContext_INTIALIZER;
        struct PRESTypePlugin plugin;
        struct PRESTypePluginDefaultEndpointData epd;

        RTICdrStream_init(&cdrStream);
        /*
        * The buffer is constant because this is a deserialization function
        * (the buffer is an input parameter, not an output parameter).
        * However, the buffer member in the stream is a (char *) so coverity
        * complains in case something else modifies the buffer's contents later.
        *
        * We don't currently have a stream type with a constant buffer.
        * Therefore, we suppress the warning after making sure that this function
        * doesn't modify the contents of the stream's buffer.
        */
        /* coverity[cert_exp40_c_violation : FALSE] */
        RTICdrStream_set(&cdrStream, (char *) buffer, length);

        epd.programContext = defaultProgramContext;
        epd._participantData = &pd;
        epd.typePlugin = &plugin;
        epd.programContext.endpointPluginData = &epd;
        try {
            plugin.typeCode = (struct RTICdrTypeCode *)
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Sample >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        Sample,
        true, true, true>();

        RTIXCdrSampleAssignabilityProperty_setFromGlobalComplianceMask(
            &epd._assignabilityProperty);

        ::rti::topic::reset_sample(*sample);

        return ::CorpusFlatDataBinding::SamplePlugin_deserialize_sample(
            (PRESTypePluginEndpointData)&epd,
            sample,
            &cdrStream,
            RTI_TRUE,
            RTI_TRUE,
            NULL);
    }

    RTIBool
    SamplePlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        Sample **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos)
    {
        try {
            RTIBool result = RTI_FALSE;
            RTIOsapiUtility_unusedParameter(drop_sample);

            cdrStream->_xTypesState.unassignable = RTI_FALSE;

            RTIXCdrStream *xcdrStream = RTICdrStream_asXCdrStream(cdrStream);
            RTIXCdrUnsignedLong sampleSize = 0;

            RTIOsapiUtility_unusedParameter(endpoint_plugin_qos);

            if (deserialize_encapsulation) {
                if (!RTIXCdrStream_deserializeAndSetCdrEncapsulation(xcdrStream)) {
                    return RTI_FALSE;
                }
            }

            if (deserialize_sample) {
                sampleSize = SampleOffset::serialized_size(0);
                if (sampleSize > RTI_XCDR_MAX_SERIALIZED_SIZE_NO_ENCAPSULATION) {
                    return RTI_FALSE;
                }

                if (*reinterpret_cast<void **>(sample) == reinterpret_cast<void *>(
                    PRES_TYPE_PLUGIN_INVALID_SAMPLE_ADDRESS)) {
                    *sample = reinterpret_cast< Sample * >(PRESTypePluginDefaultEndpointData_getFstSampleWithSize(
                        endpoint_data, 
                        (int) sampleSize + RTI_XCDR_ENCAPSULATION_HEADER_SIZE));
                    RTIXCdrFlatSample_initializeEncapsulation(
                        *reinterpret_cast<char **>(sample),
                        RTIXCdrStream_getEncapsulationId(xcdrStream));            
                } else if (RTIXCdrFlatSample_getEncapsulation(
                    *reinterpret_cast<char **>(sample)) !=
                    RTIXCdrStream_getEncapsulationId(xcdrStream)) {
                    return RTI_FALSE;
                }

                if (*reinterpret_cast<char **>(sample) != RTIXCdrStream_getBuffer(xcdrStream)) {
                    if (!RTIXCdrStream_deserializeNByte(
                        xcdrStream,
                        *reinterpret_cast<char **>(sample) 
                        + RTI_XCDR_ENCAPSULATION_HEADER_SIZE,
                        1,
                        sampleSize)) {
                        return RTI_FALSE;
                    }
                } else {
                    if (!RTIXCdrStream_skipNByte(
                        xcdrStream,
                        1,
                        sampleSize)) {
                        return RTI_FALSE;
                    }    
                }
            }
            result = RTI_TRUE;
            if (result) {
                if (cdrStream->_xTypesState.unassignable) {
                    result = RTI_FALSE;
                }
            }
            if (!result && cdrStream->_xTypesState.unassignable ) {

                RTICdrLog_exception(
                    &RTI_CDR_LOG_UNASSIGNABLE_SAMPLE_OF_TYPE_s,
                    "Sample");

            }
            return result;

        } catch (...) {
            return RTI_FALSE;
        }
    }

    RTIBool SamplePlugin_skip(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        RTIBool skip_encapsulation,
        RTIBool skip_sample,
        void *endpoint_plugin_qos)
    {
        return PRESTypePlugin_interpretedSkip(
            endpoint_data,
            cdrStream,
            skip_encapsulation,
            skip_sample,
            endpoint_plugin_qos);
    }

    unsigned int
    SamplePlugin_get_serialized_sample_max_size_ex(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool *overflow,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        RTIOsapiUtility_unusedParameter(overflow);
        RTIOsapiUtility_unusedParameter(endpoint_data);
        RTIOsapiUtility_unusedParameter(encapsulation_id);
        RTIOsapiUtility_unusedParameter(current_alignment);

        if (include_encapsulation) {
            return static_cast<unsigned int>(
                SampleOffset::serialized_size(current_alignment) +
                RTI_XCDR_ENCAPSULATION_HEADER_SIZE);
        } else {
            return static_cast<unsigned int>(
                SampleOffset::serialized_size(current_alignment));
        }

    }

    unsigned int
    SamplePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        try {
            unsigned int size;
            RTIBool overflow = RTI_FALSE;

            size = ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_max_size_ex(
                endpoint_data,&overflow,include_encapsulation,encapsulation_id,current_alignment);

            if (overflow) {
                size = RTI_CDR_MAX_SERIALIZED_SIZE;
            }

            return size;
        } catch (...) {
            return 0;
        }
    }

    unsigned int
    SamplePlugin_get_serialized_sample_min_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedSampleMinSize(
            endpoint_data,
            include_encapsulation,
            encapsulation_id,
            current_alignment);

    }

    /* Returns the size of the sample in its serialized form (in bytes).
    * It can also be an estimation in excess of the real buffer needed
    * during a call to the serialize() function.
    * The value reported does not have to include the space for the
    * encapsulation flags.
    */
    unsigned int
    SamplePlugin_get_serialized_sample_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment,
        const void *s)
    {
        const Sample *sample =
        (const Sample *) s;
        RTIOsapiUtility_unusedParameter(sample);
        RTIOsapiUtility_unusedParameter(endpoint_data);
        RTIOsapiUtility_unusedParameter(encapsulation_id);
        RTIOsapiUtility_unusedParameter(current_alignment);

        if (include_encapsulation) {
            return static_cast<unsigned int>(
                SampleOffset::serialized_size(current_alignment) +
                RTI_XCDR_ENCAPSULATION_HEADER_SIZE);
        } else {
            return static_cast<unsigned int>(
                SampleOffset::serialized_size(current_alignment));
        }

    }

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    PRESTypePluginKeyKind
    SamplePlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    RTIBool
    SamplePlugin_serialize_key_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        const Sample *sample,
        struct RTICdrStream *cdrStream,
        RTIEncapsulationId encapsulation_id,
        void *endpoint_plugin_qos)
    {
        RTIBool result = RTI_FALSE;
        RTIXCdrUnsignedLong sampleSize = 0;
        struct RTICdrStream tempStream;

        SamplePlainHelper *plainSample =
        reinterpret_cast< ::CorpusFlatDataBinding::SamplePlainHelper * >(
            PRESTypePluginDefaultEndpointData_getPlainHelperSample(endpoint_data));
        if (plainSample == NULL) {
            return RTI_FALSE;
        }

        sampleSize = static_cast<unsigned int>(
            SampleOffset::serialized_size(0) +
            RTI_XCDR_ENCAPSULATION_HEADER_SIZE);
        RTICdrStream_init(&tempStream);
        RTICdrStream_set(&tempStream, (char *)sample, sampleSize);
        result =  PRESTypePlugin_interpretedSerializedSampleToKey(
            endpoint_data,
            plainSample,
            &tempStream,
            RTI_TRUE,
            RTI_TRUE,
            endpoint_plugin_qos);
        if (result) {
            result = PRESTypePlugin_interpretedSerializeKeyForKeyhash(
                endpoint_data,
                plainSample,
                cdrStream,
                encapsulation_id,
                endpoint_plugin_qos);
        }

        return result;
    }

    RTIBool
    SamplePlugin_serialize_key(
        PRESTypePluginEndpointData endpoint_data,
        const void *s,
        struct RTICdrStream *cdrStream,
        RTIBool serialize_encapsulation,
        RTIEncapsulationId encapsulation_id,
        RTIBool serialize_key,
        void *endpoint_plugin_qos)
    {
        const Sample *sample =
        (const Sample *) s;
        RTIBool result = RTI_FALSE;
        RTIXCdrUnsignedLong sampleSize = 0;
        struct RTICdrStream tempStream;
        SamplePlainHelper *plainSample =
        reinterpret_cast< ::CorpusFlatDataBinding::SamplePlainHelper *>(
            PRESTypePluginDefaultEndpointData_getPlainHelperSample(endpoint_data));
        if (plainSample == NULL) {
            return RTI_FALSE;
        }

        sampleSize = static_cast<unsigned int>(
            SampleOffset::serialized_size(0) +
            RTI_XCDR_ENCAPSULATION_HEADER_SIZE);
        RTICdrStream_init(&tempStream);
        RTICdrStream_set(&tempStream, (char *)sample, sampleSize);
        if (PRESTypePlugin_interpretedSerializedSampleToKey(
            endpoint_data,
            plainSample,
            &tempStream,
            RTI_TRUE,
            RTI_TRUE,
            endpoint_plugin_qos)) {
            result = PRESTypePlugin_interpretedSerializeKey(
                endpoint_data,
                plainSample,
                cdrStream,
                serialize_encapsulation,
                encapsulation_id,
                serialize_key,
                endpoint_plugin_qos);
        }

        return result;

    }

    RTIBool SamplePlugin_deserialize_key_sample(
        PRESTypePluginEndpointData endpoint_data,
        void *s,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos)
    {
        Sample *sample = (Sample *) s;
        RTIBool result = RTI_FALSE;
        SamplePlainHelper *plainSample = 
        reinterpret_cast< ::CorpusFlatDataBinding::SamplePlainHelper * >(
            PRESTypePluginDefaultEndpointData_getPlainHelperSample(endpoint_data));
        if (plainSample == NULL) {
            return RTI_FALSE;
        }
        if (!PRESTypePlugin_interpretedDeserializeKey(
            endpoint_data,
            plainSample,
            cdrStream,
            deserialize_encapsulation,
            deserialize_key,
            endpoint_plugin_qos)) {
            return RTI_FALSE;
        }
        unsigned int length = PRESTypePluginDefaultEndpointData_getMaxSizeSerializedSample(
            endpoint_data);
        result = SamplePlainHelperPlugin_serialize_to_cdr_buffer(
            (char *)sample,
            &length,
            plainSample,
            DDS_XCDR2_DATA_REPRESENTATION);

        return result;

    }

    RTIBool SamplePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Sample **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos)
    {
        try {
            RTIBool result;
            void* return_loaned_buffer =
            (*reinterpret_cast<char **>(sample) == RTICdrStream_getBuffer(cdrStream)) ?
            *reinterpret_cast<void **>(sample) : NULL;
            RTIOsapiUtility_unusedParameter(drop_sample);
            cdrStream->_xTypesState.unassignable = RTI_FALSE;
            if (deserialize_key) {
                *sample = reinterpret_cast< Sample *>(PRESTypePluginDefaultEndpointData_getFstSampleWithSize(
                    endpoint_data, static_cast<int>(
                        RTI_XCDR_ENCAPSULATION_HEADER_SIZE +
                        SampleOffset::serialized_size(0))));
            }
            if (deserialize_encapsulation && deserialize_key) {
                RTIOsapiMemory_copy(
                    (void *) *sample,
                    RTICdrStream_getBuffer(cdrStream),
                    RTI_XCDR_ENCAPSULATION_HEADER_SIZE);
            } else if (deserialize_key) {
                RTIXCdrFlatSample_initializeEncapsulation(
                    *reinterpret_cast<char **>(sample),
                    RTICdrStream_getEncapsulationKind(cdrStream));
            }
            /* This is code generated without the interpreter that is deprecated and
            should not be used */
            /* coverity[var_deref_model : FALSE] */
            result= ::CorpusFlatDataBinding::SamplePlugin_deserialize_key_sample(
                endpoint_data, (sample != NULL)?*sample:NULL, cdrStream,
                deserialize_encapsulation, deserialize_key, endpoint_plugin_qos);
            if (result) {
                if (cdrStream->_xTypesState.unassignable) {
                    result = RTI_FALSE;
                }
            }
            if (return_loaned_buffer != NULL) {
                REDAFastBufferPool_returnBuffer(
                    PRESTypePluginDefaultEndpointData_getFstSampleDataPool(
                        endpoint_data),
                        RTICdrStream_getBuffer(cdrStream));
            }
            return result;
        } catch (...) {
            return RTI_FALSE;
        }
    }

    unsigned int
    SamplePlugin_get_serialized_key_max_size_ex(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool * overflow,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedKeyMaxSize(
            endpoint_data,
            overflow,
            include_encapsulation,
            encapsulation_id,
            current_alignment);

    }

    unsigned int
    SamplePlugin_get_serialized_key_max_size_for_keyhash_ex(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool * overflow,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedKeyMaxSizeForKeyhash(
            endpoint_data,
            overflow,
            encapsulation_id,
            current_alignment);
    }

    unsigned int
    SamplePlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        try {
            unsigned int size;
            RTIBool overflow = RTI_FALSE;

            size = PRESTypePlugin_interpretedGetSerializedKeyMaxSize(
                endpoint_data,&overflow,include_encapsulation,encapsulation_id,current_alignment);
            if (overflow) {
                size = RTI_CDR_MAX_SERIALIZED_SIZE;
            }

            return size;
        } catch (...) {
            return RTI_FALSE;
        }
    }

    unsigned int
    SamplePlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        unsigned int size;
        RTIBool overflow = RTI_FALSE;

        size = PRESTypePlugin_interpretedGetSerializedKeyMaxSizeForKeyhash(
            endpoint_data,
            &overflow,
            encapsulation_id,
            current_alignment);
        if (overflow) {
            size = RTI_CDR_MAX_SERIALIZED_SIZE;
        }

        return size;
    }

    RTIBool
    SamplePlugin_serialized_sample_to_key(
        PRESTypePluginEndpointData endpoint_data,
        void *s,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos)
    {
        return PRESTypePlugin_interpretedSerializedSampleToKey(
            endpoint_data,
            s,
            cdrStream,
            deserialize_encapsulation,
            deserialize_key,
            endpoint_plugin_qos);

    }

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */
    struct PRESTypePlugin *SamplePlugin_new(void)
    {
        struct PRESTypePlugin *plugin = NULL;
        const struct PRESTypePluginVersion PLUGIN_VERSION =
        PRES_TYPE_PLUGIN_VERSION_2_0;

        RTIOsapiHeap_allocateStructure(
            &plugin, struct PRESTypePlugin);
        if (plugin == NULL) {
            return NULL;
        }

        plugin->version = PLUGIN_VERSION;

        /* set up parent's function pointers */
        plugin->onParticipantAttached =
        (PRESTypePluginOnParticipantAttachedCallback)
        ::CorpusFlatDataBinding::SamplePlugin_on_participant_attached;
        plugin->onParticipantDetached =
        (PRESTypePluginOnParticipantDetachedCallback)
        ::CorpusFlatDataBinding::SamplePlugin_on_participant_detached;
        plugin->onEndpointAttached =
        (PRESTypePluginOnEndpointAttachedCallback)
        ::CorpusFlatDataBinding::SamplePlugin_on_endpoint_attached;
        plugin->onEndpointDetached =
        (PRESTypePluginOnEndpointDetachedCallback)
        ::CorpusFlatDataBinding::SamplePlugin_on_endpoint_detached;

        plugin->copySampleFnc =
        (PRESTypePluginCopySampleFunction)
        ::CorpusFlatDataBinding::SamplePlugin_copy_sample;
        plugin->createSampleFnc =
        (PRESTypePluginCreateSampleFunction)
        SamplePlugin_create_sample;
        plugin->destroySampleFnc =
        (PRESTypePluginDestroySampleFunction)
        SamplePlugin_destroy_sample;

        plugin->serializeFnc =
        (PRESTypePluginSerializeFunction) ::CorpusFlatDataBinding::SamplePlugin_serialize;
        plugin->deserializeFnc =
        (PRESTypePluginDeserializeFunction) ::CorpusFlatDataBinding::SamplePlugin_deserialize;
        plugin->getSerializedSampleMaxSizeFnc =
        (PRESTypePluginGetSerializedSampleMaxSizeFunction)
        ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_max_size;
        plugin->getSerializedSampleMinSizeFnc =
        (PRESTypePluginGetSerializedSampleMinSizeFunction)
        ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_min_size;
        plugin->getDeserializedSampleMaxSizeFnc =
        (PRESTypePluginGetDeserializedSampleMaxSizeFunction)
        SamplePlugin_get_deserialized_sample_max_size;
        plugin->getSampleFnc =
        (PRESTypePluginGetSampleFunction)
        SamplePlugin_get_sample_flat_data;
        plugin->returnSampleFnc =
        (PRESTypePluginReturnSampleFunction)
        SamplePlugin_return_sample_flat_data;
        plugin->getKeyKindFnc =
        (PRESTypePluginGetKeyKindFunction)
        ::CorpusFlatDataBinding::SamplePlugin_get_key_kind;

        /* These functions are only used for keyed types. As this is not a keyed
        type they are all set to NULL
        */
        plugin->serializeKeyFnc = NULL ;
        plugin->deserializeKeyFnc = NULL;
        plugin->getKeyFnc = NULL;
        plugin->returnKeyFnc = NULL;
        plugin->instanceToKeyFnc = NULL;
        plugin->keyToInstanceFnc = NULL;
        plugin->getSerializedKeyMaxSizeFnc = NULL;
        plugin->instanceToKeyHashFnc = NULL;
        plugin->serializedSampleToKeyHashFnc = NULL;
        plugin->serializedKeyToKeyHashFnc = NULL;
        #ifdef NDDS_STANDALONE_TYPE
        plugin->typeCode = NULL;
        #else
        try {
            plugin->typeCode = (struct RTICdrTypeCode *)
            &::rti::topic::dynamic_type< ::CorpusFlatDataBinding::Sample >::get().native();
        } catch (...) {
            ::CorpusFlatDataBinding::SamplePlugin_delete(plugin);
            return NULL;
        }
        #endif
        plugin->languageKind = PRES_TYPEPLUGIN_FLATDATA_TYPE;

        /* Serialized buffer */
        plugin->getBufferWithParams =
        (NDDS_WriterHistory_Allocator_getBufferWithParams_fn)
        SamplePlugin_get_buffer_w_params;
        plugin->returnBufferWithParams =
        (NDDS_WriterHistory_Allocator_returnBufferWithParams_fn)
        SamplePlugin_return_buffer_w_params;
        plugin->getBuffer = NULL;
        plugin->returnBuffer = NULL;
        plugin->getSerializedSampleSizeFnc =
        (PRESTypePluginGetSerializedSampleSizeFunction)
        ::CorpusFlatDataBinding::SamplePlugin_get_serialized_sample_size;

        /* writer loaned sample pool */
        plugin->getWriterLoanedSampleFnc =
        (PRESTypePluginGetWriterLoanedSampleFunction)
        SamplePlugin_get_writer_loaned_sample;
        plugin->returnWriterLoanedSampleFnc =
        (PRESTypePluginReturnWriterLoanedSampleFunction)
        SamplePlugin_return_writer_loaned_sample;
        plugin->returnWriterLoanedSampleFromCookieFnc =
        (PRESTypePluginReturnWriterLoanedSampleFromCookieFunction)
        SamplePlugin_return_writer_loaned_sample_from_cookie;
        plugin->validateWriterLoanedSampleFnc =
        (PRESTypePluginValidateWriterLoanedSampleFunction)
        SamplePlugin_validate_writer_loaned_sample;
        plugin->setWriterLoanedSampleSerializedStateFnc =
        (PRESTypePluginSetWriterLoanedSampleSerializedStateFunction)
        SamplePlugin_set_writer_loaned_sample_serialized_state;

        static const char * TYPE_NAME = "CorpusFlatDataBinding::Sample";
        plugin->endpointTypeName = TYPE_NAME;
        plugin->isMetpType = RTI_FALSE;
        plugin->isRecursiveType = RTI_FALSE;
        return plugin;
    }

    void
    SamplePlugin_delete(struct PRESTypePlugin *plugin)
    {
        RTIOsapiHeap_freeStructure(plugin);
    }

    /* ----------------------------------------------------------------------------
    *  Type SamplePlainHelper
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    SamplePlainHelperPluginSupport_create_dataI(void)
    {
        try {
            SamplePlainHelper *sample = new SamplePlainHelper();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    SamplePlainHelper *
    SamplePlainHelperPluginSupport_create_data(void)
    {
        return (SamplePlainHelper *) SamplePlainHelperPluginSupport_create_dataI();
    }

    void
    SamplePlainHelperPluginSupport_destroy_data(
        SamplePlainHelper *sample)
    {
        delete sample;
    }

    void
    SamplePlainHelperPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusFlatDataBinding::SamplePlainHelperPluginSupport_destroy_data((SamplePlainHelper *) sample);
    }

    RTIBool
    SamplePlainHelperPluginSupport_copy_data(
        SamplePlainHelper *dst,
        const SamplePlainHelper *src)
    {
        try {
            *dst = *src;
        } catch (...) {
            return RTI_FALSE;
        }

        return RTI_TRUE;
    }

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    PRESTypePluginParticipantData
    SamplePlainHelperPlugin_on_participant_attached(
        void *registration_data,
        const struct PRESTypePluginParticipantInfo *participant_info,
        RTIBool top_level_registration,
        void *container_plugin_context,
        RTICdrTypeCode *type_code)
    {
        try {
            struct RTIXCdrInterpreterPrograms *programs = NULL;
            struct PRESTypePluginDefaultParticipantData *pd = NULL;
            struct RTIXCdrInterpreterProgramsGenProperty programProperty =
            RTIXCdrInterpreterProgramsGenProperty_INITIALIZER;
            RTIOsapiUtility_unusedParameter(registration_data);
            RTIOsapiUtility_unusedParameter(participant_info);
            RTIOsapiUtility_unusedParameter(top_level_registration);
            RTIOsapiUtility_unusedParameter(container_plugin_context);
            RTIOsapiUtility_unusedParameter(type_code);

            if (!RTIXCdrXTypesComplianceMask_verifyGeneratedXTypesMask(0x0000018C)) {
                return NULL;
            }

            pd = (struct PRESTypePluginDefaultParticipantData *)
            PRESTypePluginDefaultParticipantData_new(participant_info);

            programProperty.generateV1Encapsulation = RTI_XCDR_FALSE;
            programProperty.generateV2Encapsulation = RTI_XCDR_TRUE;
            programProperty.resolveAlias = RTI_XCDR_TRUE;
            programProperty.inlineStruct = RTI_XCDR_TRUE;
            programProperty.optimizeEnum = RTI_XCDR_TRUE;
            programProperty.unboundedSize = RTIXCdrLong_MAX;

            programProperty.externalReferenceSize =
            (RTIXCdrUnsignedShort) sizeof(::dds::core::external<char>);
            programProperty.getExternalRefPointerFcn =
            ::rti::topic::interpreter::get_external_value_pointer;

            programs = DDS_TypeCodeFactory_assert_programs_in_global_list(
                DDS_TypeCodeFactory_get_instance(),
                (DDS_TypeCode *) (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< SamplePlainHelper >::get().native()
                ,
                &programProperty,
                RTI_XCDR_PROGRAM_MASK_TYPEPLUGIN);

            if (programs == NULL) {
                PRESTypePluginDefaultParticipantData_delete(
                    (PRESTypePluginParticipantData)pd);
                return NULL;
            }

            pd->programs = programs;
            return (PRESTypePluginParticipantData)pd;
        } catch (...) {
            return NULL;
        }
    }

    void
    SamplePlainHelperPlugin_on_participant_detached(
        PRESTypePluginParticipantData participant_data)
    {
        if (participant_data != NULL) {
            struct PRESTypePluginDefaultParticipantData *pd =
            (struct PRESTypePluginDefaultParticipantData *)participant_data;

            if (pd->programs != NULL) {
                DDS_TypeCodeFactory_remove_programs_from_global_list(
                    DDS_TypeCodeFactory_get_instance(),
                    pd->programs);
                pd->programs = NULL;
            }
            PRESTypePluginDefaultParticipantData_delete(participant_data);
        }
    }

    PRESTypePluginEndpointData
    SamplePlainHelperPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *containerPluginContext)
    {
        try {
            PRESTypePluginEndpointData epd = NULL;
            unsigned int serializedSampleMaxSize = 0;

            RTIOsapiUtility_unusedParameter(top_level_registration);
            RTIOsapiUtility_unusedParameter(containerPluginContext);

            if (participant_data == NULL) {
                return NULL;
            }

            epd = PRESTypePluginDefaultEndpointData_new(
                participant_data,
                endpoint_info,
                SamplePlainHelperPluginSupport_create_dataI,
                SamplePlainHelperPluginSupport_destroy_dataI,
                NULL , NULL );

            if (epd == NULL) {
                return NULL;
            }

            if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER) {
                PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(epd, serializedSampleMaxSize);

                if (PRESTypePluginDefaultEndpointData_createWriterPool(
                    epd,
                    endpoint_info,
                    (PRESTypePluginGetSerializedSampleMaxSizeFunction)
                    ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_max_size, epd,
                    (PRESTypePluginGetSerializedSampleSizeFunction)
                    ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_size,
                    epd) == RTI_FALSE) {
                    PRESTypePluginDefaultEndpointData_delete(epd);
                    return NULL;
                }
            }

            return epd;
        } catch (...) {
            return NULL;
        }
    }

    void
    SamplePlainHelperPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data)
    {
        PRESTypePluginDefaultEndpointData_delete(endpoint_data);
    }

    void
    SamplePlainHelperPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        SamplePlainHelper *sample,
        void *handle)
    {
        try {
            ::rti::topic::reset_sample(*sample);
        } catch(const std::exception& ex) {
            RTICdrLog_logWithFunctionName(
                RTI_LOG_BIT_EXCEPTION,
                "SamplePlainHelperPlugin_return_sample",
                &RTI_LOG_ANY_FAILURE_ss,
                "exception: ",
                ex.what());
        }

        PRESTypePluginDefaultEndpointData_returnSample(
            endpoint_data, sample, handle);
    }

    RTIBool
    SamplePlainHelperPlugin_copy_sample(
        PRESTypePluginEndpointData,
        SamplePlainHelper *dst,
        const SamplePlainHelper *src)
    {
        return ::CorpusFlatDataBinding::SamplePlainHelperPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    SamplePlainHelperPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    SamplePlainHelperPlugin_serialize(
        PRESTypePluginEndpointData endpoint_data,
        const void *s,
        struct RTICdrStream *cdrStream,
        RTIBool serialize_encapsulation,
        RTIEncapsulationId encapsulation_id,
        RTIBool serialize_sample,
        void *endpoint_plugin_qos)
    {
        const SamplePlainHelper *sample =
        (const SamplePlainHelper *) s;
        RTIBool retval = RTI_TRUE;
        retval = PRESTypePlugin_interpretedSerialize(
            endpoint_data,
            sample,
            cdrStream,
            serialize_encapsulation,
            encapsulation_id,
            serialize_sample,
            endpoint_plugin_qos);
        return retval;

    }

    RTIBool
    SamplePlainHelperPlugin_deserialize_sample(
        PRESTypePluginEndpointData endpoint_data,
        void *s,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos)
    {
        SamplePlainHelper *sample = (SamplePlainHelper *)s;
        return PRESTypePlugin_interpretedDeserialize(
            endpoint_data,
            sample,
            cdrStream,
            deserialize_encapsulation,
            deserialize_sample,
            endpoint_plugin_qos);

    }

    RTIBool
    SamplePlainHelperPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const SamplePlainHelper *sample,
        ::dds::core::policy::DataRepresentationId representation)
    {
        using namespace ::dds::core::policy;

        try{
            RTIEncapsulationId encapsulationId = RTI_CDR_ENCAPSULATION_ID_INVALID;
            struct RTICdrStream cdrStream;
            struct PRESTypePluginDefaultEndpointData epd;
            RTIBool result;
            struct PRESTypePluginDefaultParticipantData pd;
            struct RTIXCdrTypePluginProgramContext defaultProgramContext =
            RTIXCdrTypePluginProgramContext_INTIALIZER;
            struct PRESTypePlugin plugin = PRES_TYPEPLUGIN_DEFAULT;

            if (length == NULL) {
                return RTI_FALSE;
            }

            RTIOsapiMemory_zero(&epd, sizeof(struct PRESTypePluginDefaultEndpointData));
            epd.programContext = defaultProgramContext;
            epd._participantData = &pd;
            epd.typePlugin = &plugin;
            epd.programContext.endpointPluginData = &epd;
            try {
                plugin.typeCode = (struct RTICdrTypeCode *)
                (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< SamplePlainHelper >::get().native()
                ;
            } catch (...) {
                return RTI_FALSE;
            }
            pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
            SamplePlainHelper,
            true, true, true>();

            encapsulationId = DDS_TypeCode_get_native_encapsulation(
                (DDS_TypeCode *) plugin.typeCode,
                representation);

            if (encapsulationId == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                return RTI_FALSE;
            }

            epd._maxSizeSerializedSample =
            SamplePlainHelperPlugin_get_serialized_sample_max_size(
                (PRESTypePluginEndpointData)&epd,
                RTI_TRUE,
                encapsulationId,
                0);

            if (buffer == NULL) {
                *length =
                ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_size(
                    (PRESTypePluginEndpointData)&epd,
                    RTI_TRUE,
                    encapsulationId,
                    0,
                    sample);

                if (*length == 0) {
                    return RTI_FALSE;
                }

                return RTI_TRUE;
            }

            RTICdrStream_init(&cdrStream);
            RTICdrStream_set(&cdrStream, buffer, *length);

            result = ::CorpusFlatDataBinding::SamplePlainHelperPlugin_serialize(
                (PRESTypePluginEndpointData)&epd,
                sample,
                &cdrStream,
                RTI_TRUE,
                encapsulationId,
                RTI_TRUE,
                NULL);

            *length = (unsigned int) RTICdrStream_getCurrentPositionOffset(&cdrStream);
            return result;
        } catch (...) {
            return RTI_FALSE;
        }
    }

    RTIBool
    SamplePlainHelperPlugin_deserialize_from_cdr_buffer(
        SamplePlainHelper *sample,
        const char * buffer,
        unsigned int length)
    {
        struct RTICdrStream cdrStream;
        struct PRESTypePluginDefaultParticipantData pd;
        struct RTIXCdrTypePluginProgramContext defaultProgramContext =
        RTIXCdrTypePluginProgramContext_INTIALIZER;
        struct PRESTypePlugin plugin;
        struct PRESTypePluginDefaultEndpointData epd;

        RTICdrStream_init(&cdrStream);
        /*
        * The buffer is constant because this is a deserialization function
        * (the buffer is an input parameter, not an output parameter).
        * However, the buffer member in the stream is a (char *) so coverity
        * complains in case something else modifies the buffer's contents later.
        *
        * We don't currently have a stream type with a constant buffer.
        * Therefore, we suppress the warning after making sure that this function
        * doesn't modify the contents of the stream's buffer.
        */
        /* coverity[cert_exp40_c_violation : FALSE] */
        RTICdrStream_set(&cdrStream, (char *) buffer, length);

        epd.programContext = defaultProgramContext;
        epd._participantData = &pd;
        epd.typePlugin = &plugin;
        epd.programContext.endpointPluginData = &epd;
        try {
            plugin.typeCode = (struct RTICdrTypeCode *)
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< SamplePlainHelper >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        SamplePlainHelper,
        true, true, true>();

        RTIXCdrSampleAssignabilityProperty_setFromGlobalComplianceMask(
            &epd._assignabilityProperty);

        ::rti::topic::reset_sample(*sample);

        return ::CorpusFlatDataBinding::SamplePlainHelperPlugin_deserialize_sample(
            (PRESTypePluginEndpointData)&epd,
            sample,
            &cdrStream,
            RTI_TRUE,
            RTI_TRUE,
            NULL);
    }

    RTIBool
    SamplePlainHelperPlugin_deserialize(
        PRESTypePluginEndpointData endpoint_data,
        SamplePlainHelper **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_sample,
        void *endpoint_plugin_qos)
    {
        try {
            RTIBool result = RTI_FALSE;
            RTIOsapiUtility_unusedParameter(drop_sample);

            cdrStream->_xTypesState.unassignable = RTI_FALSE;

            /* This is code generated without the interpreter that is deprecated and
            should not be used */
            /* coverity[var_deref_model : FALSE] */
            result= ::CorpusFlatDataBinding::SamplePlainHelperPlugin_deserialize_sample(
                endpoint_data, (sample != NULL)?*sample:NULL,
                cdrStream, deserialize_encapsulation, deserialize_sample,
                endpoint_plugin_qos);
            if (result) {
                if (cdrStream->_xTypesState.unassignable) {
                    result = RTI_FALSE;
                }
            }
            if (!result && cdrStream->_xTypesState.unassignable ) {

                RTICdrLog_exception(
                    &RTI_CDR_LOG_UNASSIGNABLE_SAMPLE_OF_TYPE_s,
                    "SamplePlainHelper");

            }
            return result;

        } catch (...) {
            return RTI_FALSE;
        }
    }

    RTIBool SamplePlainHelperPlugin_skip(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream,
        RTIBool skip_encapsulation,
        RTIBool skip_sample,
        void *endpoint_plugin_qos)
    {
        return PRESTypePlugin_interpretedSkip(
            endpoint_data,
            cdrStream,
            skip_encapsulation,
            skip_sample,
            endpoint_plugin_qos);
    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_sample_max_size_ex(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool *overflow,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedSampleMaxSize(
            endpoint_data,
            overflow,
            include_encapsulation,
            encapsulation_id,
            current_alignment);

    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        try {
            unsigned int size;
            RTIBool overflow = RTI_FALSE;

            size = ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_max_size_ex(
                endpoint_data,&overflow,include_encapsulation,encapsulation_id,current_alignment);

            if (overflow) {
                size = RTI_CDR_MAX_SERIALIZED_SIZE;
            }

            return size;
        } catch (...) {
            return 0;
        }
    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_sample_min_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedSampleMinSize(
            endpoint_data,
            include_encapsulation,
            encapsulation_id,
            current_alignment);

    }

    /* Returns the size of the sample in its serialized form (in bytes).
    * It can also be an estimation in excess of the real buffer needed
    * during a call to the serialize() function.
    * The value reported does not have to include the space for the
    * encapsulation flags.
    */
    unsigned int
    SamplePlainHelperPlugin_get_serialized_sample_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment,
        const void *s)
    {
        const SamplePlainHelper *sample =
        (const SamplePlainHelper *) s;
        return PRESTypePlugin_interpretedGetSerializedSampleSize(
            endpoint_data,
            include_encapsulation,
            encapsulation_id,
            current_alignment,
            sample);

    }

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    PRESTypePluginKeyKind
    SamplePlainHelperPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    RTIBool
    SamplePlainHelperPlugin_serialize_key_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        const SamplePlainHelper *sample,
        struct RTICdrStream *cdrStream,
        RTIEncapsulationId encapsulation_id,
        void *endpoint_plugin_qos)
    {
        return PRESTypePlugin_interpretedSerializeKeyForKeyhash(
            endpoint_data,
            sample,
            cdrStream,
            encapsulation_id,
            endpoint_plugin_qos);
    }

    RTIBool
    SamplePlainHelperPlugin_serialize_key(
        PRESTypePluginEndpointData endpoint_data,
        const void *s,
        struct RTICdrStream *cdrStream,
        RTIBool serialize_encapsulation,
        RTIEncapsulationId encapsulation_id,
        RTIBool serialize_key,
        void *endpoint_plugin_qos)
    {
        const SamplePlainHelper *sample =
        (const SamplePlainHelper *) s;
        return PRESTypePlugin_interpretedSerializeKey(
            endpoint_data,
            sample,
            cdrStream,
            serialize_encapsulation,
            encapsulation_id,
            serialize_key,
            endpoint_plugin_qos);

    }

    RTIBool SamplePlainHelperPlugin_deserialize_key_sample(
        PRESTypePluginEndpointData endpoint_data,
        void *s,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos)
    {
        SamplePlainHelper *sample = (SamplePlainHelper *) s;
        return PRESTypePlugin_interpretedDeserializeKey(
            endpoint_data,
            sample,
            cdrStream,
            deserialize_encapsulation,
            deserialize_key,
            endpoint_plugin_qos);

    }

    RTIBool SamplePlainHelperPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        SamplePlainHelper **sample,
        RTIBool * drop_sample,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos)
    {
        try {
            RTIBool result;
            RTIOsapiUtility_unusedParameter(drop_sample);
            cdrStream->_xTypesState.unassignable = RTI_FALSE;
            /* This is code generated without the interpreter that is deprecated and
            should not be used */
            /* coverity[var_deref_model : FALSE] */
            result= ::CorpusFlatDataBinding::SamplePlainHelperPlugin_deserialize_key_sample(
                endpoint_data, (sample != NULL)?*sample:NULL, cdrStream,
                deserialize_encapsulation, deserialize_key, endpoint_plugin_qos);
            if (result) {
                if (cdrStream->_xTypesState.unassignable) {
                    result = RTI_FALSE;
                }
            }
            return result;
        } catch (...) {
            return RTI_FALSE;
        }
    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_key_max_size_ex(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool * overflow,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedKeyMaxSize(
            endpoint_data,
            overflow,
            include_encapsulation,
            encapsulation_id,
            current_alignment);

    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_key_max_size_for_keyhash_ex(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool * overflow,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        return PRESTypePlugin_interpretedGetSerializedKeyMaxSizeForKeyhash(
            endpoint_data,
            overflow,
            encapsulation_id,
            current_alignment);
    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_key_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        try {
            unsigned int size;
            RTIBool overflow = RTI_FALSE;

            size = ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_key_max_size_ex(
                endpoint_data,&overflow,include_encapsulation,encapsulation_id,current_alignment);
            if (overflow) {
                size = RTI_CDR_MAX_SERIALIZED_SIZE;
            }

            return size;
        } catch (...) {
            return RTI_FALSE;
        }
    }

    unsigned int
    SamplePlainHelperPlugin_get_serialized_key_max_size_for_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        unsigned int size;
        RTIBool overflow = RTI_FALSE;

        size = ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_key_max_size_for_keyhash_ex(
            endpoint_data,
            &overflow,
            encapsulation_id,
            current_alignment);
        if (overflow) {
            size = RTI_CDR_MAX_SERIALIZED_SIZE;
        }

        return size;
    }

    RTIBool
    SamplePlainHelperPlugin_serialized_sample_to_key(
        PRESTypePluginEndpointData endpoint_data,
        void *s,
        struct RTICdrStream *cdrStream,
        RTIBool deserialize_encapsulation,
        RTIBool deserialize_key,
        void *endpoint_plugin_qos)
    {
        return PRESTypePlugin_interpretedSerializedSampleToKey(
            endpoint_data,
            s,
            cdrStream,
            deserialize_encapsulation,
            deserialize_key,
            endpoint_plugin_qos);

    }

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */
    struct PRESTypePlugin *SamplePlainHelperPlugin_new(void)
    {
        struct PRESTypePlugin *plugin = NULL;
        const struct PRESTypePluginVersion PLUGIN_VERSION =
        PRES_TYPE_PLUGIN_VERSION_2_0;

        RTIOsapiHeap_allocateStructure(
            &plugin, struct PRESTypePlugin);
        if (plugin == NULL) {
            return NULL;
        }

        plugin->version = PLUGIN_VERSION;

        /* set up parent's function pointers */
        plugin->onParticipantAttached =
        (PRESTypePluginOnParticipantAttachedCallback)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_on_participant_attached;
        plugin->onParticipantDetached =
        (PRESTypePluginOnParticipantDetachedCallback)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_on_participant_detached;
        plugin->onEndpointAttached =
        (PRESTypePluginOnEndpointAttachedCallback)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_on_endpoint_attached;
        plugin->onEndpointDetached =
        (PRESTypePluginOnEndpointDetachedCallback)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_on_endpoint_detached;

        plugin->copySampleFnc =
        (PRESTypePluginCopySampleFunction)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_copy_sample;
        plugin->createSampleFnc =
        (PRESTypePluginCreateSampleFunction)
        SamplePlainHelperPlugin_create_sample;
        plugin->destroySampleFnc =
        (PRESTypePluginDestroySampleFunction)
        SamplePlainHelperPlugin_destroy_sample;

        plugin->serializeFnc =
        (PRESTypePluginSerializeFunction) ::CorpusFlatDataBinding::SamplePlainHelperPlugin_serialize;
        plugin->deserializeFnc =
        (PRESTypePluginDeserializeFunction) ::CorpusFlatDataBinding::SamplePlainHelperPlugin_deserialize;
        plugin->getSerializedSampleMaxSizeFnc =
        (PRESTypePluginGetSerializedSampleMaxSizeFunction)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_max_size;
        plugin->getSerializedSampleMinSizeFnc =
        (PRESTypePluginGetSerializedSampleMinSizeFunction)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_min_size;
        plugin->getDeserializedSampleMaxSizeFnc = NULL;
        plugin->getSampleFnc =
        (PRESTypePluginGetSampleFunction)
        SamplePlainHelperPlugin_get_sample;
        plugin->returnSampleFnc =
        (PRESTypePluginReturnSampleFunction)
        SamplePlainHelperPlugin_return_sample;
        plugin->getKeyKindFnc =
        (PRESTypePluginGetKeyKindFunction)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_key_kind;

        /* These functions are only used for keyed types. As this is not a keyed
        type they are all set to NULL
        */
        plugin->serializeKeyFnc = NULL ;
        plugin->deserializeKeyFnc = NULL;
        plugin->getKeyFnc = NULL;
        plugin->returnKeyFnc = NULL;
        plugin->instanceToKeyFnc = NULL;
        plugin->keyToInstanceFnc = NULL;
        plugin->getSerializedKeyMaxSizeFnc = NULL;
        plugin->instanceToKeyHashFnc = NULL;
        plugin->serializedSampleToKeyHashFnc = NULL;
        plugin->serializedKeyToKeyHashFnc = NULL;
        #ifdef NDDS_STANDALONE_TYPE
        plugin->typeCode = NULL;
        #else
        try {
            plugin->typeCode = (struct RTICdrTypeCode *)
            &::rti::topic::dynamic_type< ::CorpusFlatDataBinding::SamplePlainHelper >::get().native();
        } catch (...) {
            ::CorpusFlatDataBinding::SamplePlainHelperPlugin_delete(plugin);
            return NULL;
        }
        #endif
        plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;

        /* Serialized buffer */
        plugin->getBuffer =
        (PRESTypePluginGetBufferFunction)
        SamplePlainHelperPlugin_get_buffer;
        plugin->returnBuffer =
        (PRESTypePluginReturnBufferFunction)
        SamplePlainHelperPlugin_return_buffer;
        plugin->getBufferWithParams = NULL;
        plugin->returnBufferWithParams = NULL;
        plugin->getSerializedSampleSizeFnc =
        (PRESTypePluginGetSerializedSampleSizeFunction)
        ::CorpusFlatDataBinding::SamplePlainHelperPlugin_get_serialized_sample_size;

        plugin->getWriterLoanedSampleFnc = NULL;
        plugin->returnWriterLoanedSampleFnc = NULL;
        plugin->returnWriterLoanedSampleFromCookieFnc = NULL;
        plugin->validateWriterLoanedSampleFnc = NULL;
        plugin->setWriterLoanedSampleSerializedStateFnc = NULL;

        static const char * TYPE_NAME = "CorpusFlatDataBinding::SamplePlainHelper";
        plugin->endpointTypeName = TYPE_NAME;
        plugin->isMetpType = RTI_FALSE;
        plugin->isRecursiveType = RTI_FALSE;
        return plugin;
    }

    void
    SamplePlainHelperPlugin_delete(struct PRESTypePlugin *plugin)
    {
        RTIOsapiHeap_freeStructure(plugin);
    }
} /* namespace CorpusFlatDataBinding  */

#undef RTI_CDR_CURRENT_SUBMODULE
