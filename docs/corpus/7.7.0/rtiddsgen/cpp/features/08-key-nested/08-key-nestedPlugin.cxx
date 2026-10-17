

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-nested.idl
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

#include "08-key-nestedPlugin.hpp"

namespace CorpusNestedKeys {

    /* ----------------------------------------------------------------------------
    *  Type Inner
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    InnerPluginSupport_create_dataI(void)
    {
        try {
            Inner *sample = new Inner();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Inner *
    InnerPluginSupport_create_data(void)
    {
        return (Inner *) InnerPluginSupport_create_dataI();
    }

    void
    InnerPluginSupport_destroy_data(
        Inner *sample)
    {
        delete sample;
    }

    void
    InnerPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusNestedKeys::InnerPluginSupport_destroy_data((Inner *) sample);
    }

    RTIBool
    InnerPluginSupport_copy_data(
        Inner *dst,
        const Inner *src)
    {
        try {
            *dst = *src;
        } catch (...) {
            return RTI_FALSE;
        }

        return RTI_TRUE;
    }

    void *
    InnerPluginSupport_create_keyI(void)
    {
        return InnerPluginSupport_create_data();
    }

    Inner *
    InnerPluginSupport_create_key(void)
    {
        return (Inner *) InnerPluginSupport_create_keyI();
    }

    void
    InnerPluginSupport_destroy_key(
        InnerKeyHolder *key)
    {
        InnerPluginSupport_destroy_data(key);
    }

    void
    InnerPluginSupport_destroy_keyI(
        void *key)
    {
        ::CorpusNestedKeys::InnerPluginSupport_destroy_key((InnerKeyHolder *) key);
    }

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    PRESTypePluginParticipantData
    InnerPlugin_on_participant_attached(
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

            programProperty.generateV1Encapsulation = RTI_XCDR_TRUE;
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
                (DDS_TypeCode *) (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Inner >::get().native()
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
    InnerPlugin_on_participant_detached(
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
    InnerPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *containerPluginContext)
    {
        try {
            PRESTypePluginEndpointData epd = NULL;
            unsigned int serializedSampleMaxSize = 0;

            unsigned int serializedKeyMaxSize = 0;
            unsigned int serializedKeyMaxSizeV2 = 0;

            RTIOsapiUtility_unusedParameter(top_level_registration);
            RTIOsapiUtility_unusedParameter(containerPluginContext);

            if (participant_data == NULL) {
                return NULL;
            }

            epd = PRESTypePluginDefaultEndpointData_new(
                participant_data,
                endpoint_info,
                InnerPluginSupport_create_dataI,
                InnerPluginSupport_destroy_dataI,
                ::CorpusNestedKeys::InnerPluginSupport_create_keyI ,                ::CorpusNestedKeys::InnerPluginSupport_destroy_keyI);

            if (epd == NULL) {
                return NULL;
            }

            serializedKeyMaxSize =  ::CorpusNestedKeys::InnerPlugin_get_serialized_key_max_size(
                epd,RTI_FALSE,RTI_CDR_ENCAPSULATION_ID_CDR_BE,0);
            serializedKeyMaxSizeV2 = InnerPlugin_get_serialized_key_max_size_for_keyhash(
                epd,
                RTI_CDR_ENCAPSULATION_ID_CDR2_BE,
                0);

            if(!PRESTypePluginDefaultEndpointData_createMD5StreamWithInfo(
                epd,
                endpoint_info,
                serializedKeyMaxSize,
                serializedKeyMaxSizeV2))
            {
                PRESTypePluginDefaultEndpointData_delete(epd);
                return NULL;
            }

            if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER) {
                serializedSampleMaxSize = ::CorpusNestedKeys::InnerPlugin_get_serialized_sample_max_size(
                    epd,RTI_FALSE,RTI_CDR_ENCAPSULATION_ID_CDR_BE,0);
                PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(epd, serializedSampleMaxSize);

                if (PRESTypePluginDefaultEndpointData_createWriterPool(
                    epd,
                    endpoint_info,
                    (PRESTypePluginGetSerializedSampleMaxSizeFunction)
                    ::CorpusNestedKeys::InnerPlugin_get_serialized_sample_max_size, epd,
                    (PRESTypePluginGetSerializedSampleSizeFunction)
                    PRESTypePlugin_interpretedGetSerializedSampleSize,
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
    InnerPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data)
    {
        PRESTypePluginDefaultEndpointData_delete(endpoint_data);
    }

    void
    InnerPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Inner *sample,
        void *handle)
    {
        try {
            ::rti::topic::reset_sample(*sample);
        } catch(const std::exception& ex) {
            RTICdrLog_logWithFunctionName(
                RTI_LOG_BIT_EXCEPTION,
                "InnerPlugin_return_sample",
                &RTI_LOG_ANY_FAILURE_ss,
                "exception: ",
                ex.what());
        }

        PRESTypePluginDefaultEndpointData_returnSample(
            endpoint_data, sample, handle);
    }

    RTIBool
    InnerPlugin_copy_sample(
        PRESTypePluginEndpointData,
        Inner *dst,
        const Inner *src)
    {
        return ::CorpusNestedKeys::InnerPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    InnerPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    InnerPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Inner *sample,
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
                (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Inner >::get().native()
                ;
            } catch (...) {
                return RTI_FALSE;
            }
            pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
            Inner,
            true, true, true>();

            encapsulationId = DDS_TypeCode_get_native_encapsulation(
                (DDS_TypeCode *) plugin.typeCode,
                representation);

            if (encapsulationId == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                return RTI_FALSE;
            }

            epd._maxSizeSerializedSample =
            InnerPlugin_get_serialized_sample_max_size(
                (PRESTypePluginEndpointData)&epd,
                RTI_TRUE,
                encapsulationId,
                0);

            if (buffer == NULL) {
                *length =
                PRESTypePlugin_interpretedGetSerializedSampleSize(
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

            result = PRESTypePlugin_interpretedSerialize(
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
    InnerPlugin_deserialize_from_cdr_buffer(
        Inner *sample,
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
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Inner >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        Inner,
        true, true, true>();

        RTIXCdrSampleAssignabilityProperty_setFromGlobalComplianceMask(
            &epd._assignabilityProperty);

        ::rti::topic::reset_sample(*sample);

        return PRESTypePlugin_interpretedDeserialize(
            (PRESTypePluginEndpointData)&epd,
            sample,
            &cdrStream,
            RTI_TRUE,
            RTI_TRUE,
            NULL);
    }

    unsigned int
    InnerPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        try {
            unsigned int size;
            RTIBool overflow = RTI_FALSE;

            size = PRESTypePlugin_interpretedGetSerializedSampleMaxSize(
                endpoint_data,&overflow,include_encapsulation,encapsulation_id,current_alignment);

            if (overflow) {
                size = RTI_CDR_MAX_SERIALIZED_SIZE;
            }

            return size;
        } catch (...) {
            return 0;
        }
    }

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    PRESTypePluginKeyKind
    InnerPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_USER_KEY;
    }

    RTIBool InnerPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Inner **sample,
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
            result= PRESTypePlugin_interpretedDeserializeKey(
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
    InnerPlugin_get_serialized_key_max_size(
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
    InnerPlugin_get_serialized_key_max_size_for_keyhash(
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
    InnerPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        InnerKeyHolder *dst, 
        const Inner *src)
    {
        try {
            RTIOsapiUtility_unusedParameter(endpoint_data);   

            dst->code = src->code;
            return RTI_TRUE;
        } catch (...) {
            return RTI_FALSE;
        }    
    }

    RTIBool 
    InnerPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Inner *dst, const
        InnerKeyHolder *src)
    {
        try {
            RTIOsapiUtility_unusedParameter(endpoint_data);   
            dst->code = src->code;
            return RTI_TRUE;
        } catch (...) {
            return RTI_FALSE;
        }    
    }

    RTIBool 
    InnerPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream, 
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos)
    {
        Inner * sample = NULL;
        sample = (Inner *)
        PRESTypePluginDefaultEndpointData_getTempSample(endpoint_data);
        if (sample == NULL) {
            return RTI_FALSE;
        }
        if (!PRESTypePlugin_interpretedSerializedSampleToKey(
            endpoint_data,
            sample,
            cdrStream, 
            deserialize_encapsulation, 
            RTI_TRUE,
            endpoint_plugin_qos)) {
            return RTI_FALSE;
        }
        if (!PRESTypePlugin_interpretedInstanceToKeyHash(
            endpoint_data,
            keyhash,
            sample,
            RTICdrStream_getEncapsulationKind(cdrStream))) {
            return RTI_FALSE;
        }
        return RTI_TRUE;   
    }

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */
    struct PRESTypePlugin *InnerPlugin_new(void)
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
        ::CorpusNestedKeys::InnerPlugin_on_participant_attached;
        plugin->onParticipantDetached =
        (PRESTypePluginOnParticipantDetachedCallback)
        ::CorpusNestedKeys::InnerPlugin_on_participant_detached;
        plugin->onEndpointAttached =
        (PRESTypePluginOnEndpointAttachedCallback)
        ::CorpusNestedKeys::InnerPlugin_on_endpoint_attached;
        plugin->onEndpointDetached =
        (PRESTypePluginOnEndpointDetachedCallback)
        ::CorpusNestedKeys::InnerPlugin_on_endpoint_detached;

        plugin->copySampleFnc =
        (PRESTypePluginCopySampleFunction)
        ::CorpusNestedKeys::InnerPlugin_copy_sample;
        plugin->createSampleFnc =
        (PRESTypePluginCreateSampleFunction)
        InnerPlugin_create_sample;
        plugin->destroySampleFnc =
        (PRESTypePluginDestroySampleFunction)
        InnerPlugin_destroy_sample;

        plugin->serializeFnc =
        (PRESTypePluginSerializeFunction) PRESTypePlugin_interpretedSerialize;
        plugin->deserializeFnc =
        (PRESTypePluginDeserializeFunction) PRESTypePlugin_interpretedDeserializeWithAlloc;
        plugin->getSerializedSampleMaxSizeFnc =
        (PRESTypePluginGetSerializedSampleMaxSizeFunction)
        ::CorpusNestedKeys::InnerPlugin_get_serialized_sample_max_size;
        plugin->getSerializedSampleMinSizeFnc =
        (PRESTypePluginGetSerializedSampleMinSizeFunction)
        PRESTypePlugin_interpretedGetSerializedSampleMinSize;
        plugin->getDeserializedSampleMaxSizeFnc = NULL;
        plugin->getSampleFnc =
        (PRESTypePluginGetSampleFunction)
        InnerPlugin_get_sample;
        plugin->returnSampleFnc =
        (PRESTypePluginReturnSampleFunction)
        InnerPlugin_return_sample;
        plugin->getKeyKindFnc =
        (PRESTypePluginGetKeyKindFunction)
        ::CorpusNestedKeys::InnerPlugin_get_key_kind;

        plugin->getSerializedKeyMaxSizeFnc =
        (PRESTypePluginGetSerializedKeyMaxSizeFunction)
        ::CorpusNestedKeys::InnerPlugin_get_serialized_key_max_size;
        plugin->serializeKeyFnc =
        (PRESTypePluginSerializeKeyFunction)
        PRESTypePlugin_interpretedSerializeKey;
        plugin->deserializeKeyFnc =
        (PRESTypePluginDeserializeKeyFunction)
        ::CorpusNestedKeys::InnerPlugin_deserialize_key;
        plugin->deserializeKeySampleFnc =
        (PRESTypePluginDeserializeKeySampleFunction)
        PRESTypePlugin_interpretedDeserializeKey;

        plugin-> instanceToKeyHashFnc =
        (PRESTypePluginInstanceToKeyHashFunction)
        PRESTypePlugin_interpretedInstanceToKeyHash;
        plugin->serializedSampleToKeyHashFnc =
        (PRESTypePluginSerializedSampleToKeyHashFunction)
        ::CorpusNestedKeys::InnerPlugin_serialized_sample_to_keyhash;

        plugin->getKeyFnc =
        (PRESTypePluginGetKeyFunction)
        InnerPlugin_get_key;
        plugin->returnKeyFnc =
        (PRESTypePluginReturnKeyFunction)
        InnerPlugin_return_key;

        plugin->instanceToKeyFnc =
        (PRESTypePluginInstanceToKeyFunction)
        ::CorpusNestedKeys::InnerPlugin_instance_to_key;
        plugin->keyToInstanceFnc =
        (PRESTypePluginKeyToInstanceFunction)
        ::CorpusNestedKeys::InnerPlugin_key_to_instance;
        plugin->serializedKeyToKeyHashFnc = NULL; /* Not supported yet */
        #ifdef NDDS_STANDALONE_TYPE
        plugin->typeCode = NULL;
        #else
        try {
            plugin->typeCode = (struct RTICdrTypeCode *)
            &::rti::topic::dynamic_type< ::CorpusNestedKeys::Inner >::get().native();
        } catch (...) {
            ::CorpusNestedKeys::InnerPlugin_delete(plugin);
            return NULL;
        }
        #endif
        plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;

        /* Serialized buffer */
        plugin->getBuffer =
        (PRESTypePluginGetBufferFunction)
        InnerPlugin_get_buffer;
        plugin->returnBuffer =
        (PRESTypePluginReturnBufferFunction)
        InnerPlugin_return_buffer;
        plugin->getBufferWithParams = NULL;
        plugin->returnBufferWithParams = NULL;
        plugin->getSerializedSampleSizeFnc =
        (PRESTypePluginGetSerializedSampleSizeFunction)
        PRESTypePlugin_interpretedGetSerializedSampleSize;

        plugin->getWriterLoanedSampleFnc = NULL;
        plugin->returnWriterLoanedSampleFnc = NULL;
        plugin->returnWriterLoanedSampleFromCookieFnc = NULL;
        plugin->validateWriterLoanedSampleFnc = NULL;
        plugin->setWriterLoanedSampleSerializedStateFnc = NULL;

        static const char * TYPE_NAME = "CorpusNestedKeys::Inner";
        plugin->endpointTypeName = TYPE_NAME;
        plugin->isMetpType = RTI_FALSE;
        plugin->isRecursiveType = RTI_FALSE;
        return plugin;
    }

    void
    InnerPlugin_delete(struct PRESTypePlugin *plugin)
    {
        RTIOsapiHeap_freeStructure(plugin);
    }

    /* ----------------------------------------------------------------------------
    *  Type Outer
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    OuterPluginSupport_create_dataI(void)
    {
        try {
            Outer *sample = new Outer();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Outer *
    OuterPluginSupport_create_data(void)
    {
        return (Outer *) OuterPluginSupport_create_dataI();
    }

    void
    OuterPluginSupport_destroy_data(
        Outer *sample)
    {
        delete sample;
    }

    void
    OuterPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusNestedKeys::OuterPluginSupport_destroy_data((Outer *) sample);
    }

    RTIBool
    OuterPluginSupport_copy_data(
        Outer *dst,
        const Outer *src)
    {
        try {
            *dst = *src;
        } catch (...) {
            return RTI_FALSE;
        }

        return RTI_TRUE;
    }

    void *
    OuterPluginSupport_create_keyI(void)
    {
        return OuterPluginSupport_create_data();
    }

    Outer *
    OuterPluginSupport_create_key(void)
    {
        return (Outer *) OuterPluginSupport_create_keyI();
    }

    void
    OuterPluginSupport_destroy_key(
        OuterKeyHolder *key)
    {
        OuterPluginSupport_destroy_data(key);
    }

    void
    OuterPluginSupport_destroy_keyI(
        void *key)
    {
        ::CorpusNestedKeys::OuterPluginSupport_destroy_key((OuterKeyHolder *) key);
    }

    /* ----------------------------------------------------------------------------
    Callback functions:
    * ---------------------------------------------------------------------------- */

    PRESTypePluginParticipantData
    OuterPlugin_on_participant_attached(
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

            programProperty.generateV1Encapsulation = RTI_XCDR_TRUE;
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
                (DDS_TypeCode *) (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Outer >::get().native()
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
    OuterPlugin_on_participant_detached(
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
    OuterPlugin_on_endpoint_attached(
        PRESTypePluginParticipantData participant_data,
        const struct PRESTypePluginEndpointInfo *endpoint_info,
        RTIBool top_level_registration,
        void *containerPluginContext)
    {
        try {
            PRESTypePluginEndpointData epd = NULL;
            unsigned int serializedSampleMaxSize = 0;

            unsigned int serializedKeyMaxSize = 0;
            unsigned int serializedKeyMaxSizeV2 = 0;

            RTIOsapiUtility_unusedParameter(top_level_registration);
            RTIOsapiUtility_unusedParameter(containerPluginContext);

            if (participant_data == NULL) {
                return NULL;
            }

            epd = PRESTypePluginDefaultEndpointData_new(
                participant_data,
                endpoint_info,
                OuterPluginSupport_create_dataI,
                OuterPluginSupport_destroy_dataI,
                ::CorpusNestedKeys::OuterPluginSupport_create_keyI ,                ::CorpusNestedKeys::OuterPluginSupport_destroy_keyI);

            if (epd == NULL) {
                return NULL;
            }

            serializedKeyMaxSize =  ::CorpusNestedKeys::OuterPlugin_get_serialized_key_max_size(
                epd,RTI_FALSE,RTI_CDR_ENCAPSULATION_ID_CDR_BE,0);
            serializedKeyMaxSizeV2 = OuterPlugin_get_serialized_key_max_size_for_keyhash(
                epd,
                RTI_CDR_ENCAPSULATION_ID_CDR2_BE,
                0);

            if(!PRESTypePluginDefaultEndpointData_createMD5StreamWithInfo(
                epd,
                endpoint_info,
                serializedKeyMaxSize,
                serializedKeyMaxSizeV2))
            {
                PRESTypePluginDefaultEndpointData_delete(epd);
                return NULL;
            }

            if (endpoint_info->endpointKind == PRES_TYPEPLUGIN_ENDPOINT_WRITER) {
                serializedSampleMaxSize = ::CorpusNestedKeys::OuterPlugin_get_serialized_sample_max_size(
                    epd,RTI_FALSE,RTI_CDR_ENCAPSULATION_ID_CDR_BE,0);
                PRESTypePluginDefaultEndpointData_setMaxSizeSerializedSample(epd, serializedSampleMaxSize);

                if (PRESTypePluginDefaultEndpointData_createWriterPool(
                    epd,
                    endpoint_info,
                    (PRESTypePluginGetSerializedSampleMaxSizeFunction)
                    ::CorpusNestedKeys::OuterPlugin_get_serialized_sample_max_size, epd,
                    (PRESTypePluginGetSerializedSampleSizeFunction)
                    PRESTypePlugin_interpretedGetSerializedSampleSize,
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
    OuterPlugin_on_endpoint_detached(
        PRESTypePluginEndpointData endpoint_data)
    {
        PRESTypePluginDefaultEndpointData_delete(endpoint_data);
    }

    void
    OuterPlugin_return_sample(
        PRESTypePluginEndpointData endpoint_data,
        Outer *sample,
        void *handle)
    {
        try {
            ::rti::topic::reset_sample(*sample);
        } catch(const std::exception& ex) {
            RTICdrLog_logWithFunctionName(
                RTI_LOG_BIT_EXCEPTION,
                "OuterPlugin_return_sample",
                &RTI_LOG_ANY_FAILURE_ss,
                "exception: ",
                ex.what());
        }

        PRESTypePluginDefaultEndpointData_returnSample(
            endpoint_data, sample, handle);
    }

    RTIBool
    OuterPlugin_copy_sample(
        PRESTypePluginEndpointData,
        Outer *dst,
        const Outer *src)
    {
        return ::CorpusNestedKeys::OuterPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    OuterPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    OuterPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Outer *sample,
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
                (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Outer >::get().native()
                ;
            } catch (...) {
                return RTI_FALSE;
            }
            pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
            Outer,
            true, true, true>();

            encapsulationId = DDS_TypeCode_get_native_encapsulation(
                (DDS_TypeCode *) plugin.typeCode,
                representation);

            if (encapsulationId == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                return RTI_FALSE;
            }

            epd._maxSizeSerializedSample =
            OuterPlugin_get_serialized_sample_max_size(
                (PRESTypePluginEndpointData)&epd,
                RTI_TRUE,
                encapsulationId,
                0);

            if (buffer == NULL) {
                *length =
                PRESTypePlugin_interpretedGetSerializedSampleSize(
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

            result = PRESTypePlugin_interpretedSerialize(
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
    OuterPlugin_deserialize_from_cdr_buffer(
        Outer *sample,
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
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Outer >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        Outer,
        true, true, true>();

        RTIXCdrSampleAssignabilityProperty_setFromGlobalComplianceMask(
            &epd._assignabilityProperty);

        ::rti::topic::reset_sample(*sample);

        return PRESTypePlugin_interpretedDeserialize(
            (PRESTypePluginEndpointData)&epd,
            sample,
            &cdrStream,
            RTI_TRUE,
            RTI_TRUE,
            NULL);
    }

    unsigned int
    OuterPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment)
    {
        try {
            unsigned int size;
            RTIBool overflow = RTI_FALSE;

            size = PRESTypePlugin_interpretedGetSerializedSampleMaxSize(
                endpoint_data,&overflow,include_encapsulation,encapsulation_id,current_alignment);

            if (overflow) {
                size = RTI_CDR_MAX_SERIALIZED_SIZE;
            }

            return size;
        } catch (...) {
            return 0;
        }
    }

    /* --------------------------------------------------------------------------------------
    Key Management functions:
    * -------------------------------------------------------------------------------------- */

    PRESTypePluginKeyKind
    OuterPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_USER_KEY;
    }

    RTIBool OuterPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Outer **sample,
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
            result= PRESTypePlugin_interpretedDeserializeKey(
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
    OuterPlugin_get_serialized_key_max_size(
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
    OuterPlugin_get_serialized_key_max_size_for_keyhash(
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
    OuterPlugin_instance_to_key(
        PRESTypePluginEndpointData endpoint_data,
        OuterKeyHolder *dst, 
        const Outer *src)
    {
        try {
            RTIOsapiUtility_unusedParameter(endpoint_data);   

            dst->identity = src->identity;
            return RTI_TRUE;
        } catch (...) {
            return RTI_FALSE;
        }    
    }

    RTIBool 
    OuterPlugin_key_to_instance(
        PRESTypePluginEndpointData endpoint_data,
        Outer *dst, const
        OuterKeyHolder *src)
    {
        try {
            RTIOsapiUtility_unusedParameter(endpoint_data);   
            dst->identity = src->identity;
            return RTI_TRUE;
        } catch (...) {
            return RTI_FALSE;
        }    
    }

    RTIBool 
    OuterPlugin_serialized_sample_to_keyhash(
        PRESTypePluginEndpointData endpoint_data,
        struct RTICdrStream *cdrStream, 
        DDS_KeyHash_t *keyhash,
        RTIBool deserialize_encapsulation,
        void *endpoint_plugin_qos)
    {
        Outer * sample = NULL;
        sample = (Outer *)
        PRESTypePluginDefaultEndpointData_getTempSample(endpoint_data);
        if (sample == NULL) {
            return RTI_FALSE;
        }
        if (!PRESTypePlugin_interpretedSerializedSampleToKey(
            endpoint_data,
            sample,
            cdrStream, 
            deserialize_encapsulation, 
            RTI_TRUE,
            endpoint_plugin_qos)) {
            return RTI_FALSE;
        }
        if (!PRESTypePlugin_interpretedInstanceToKeyHash(
            endpoint_data,
            keyhash,
            sample,
            RTICdrStream_getEncapsulationKind(cdrStream))) {
            return RTI_FALSE;
        }
        return RTI_TRUE;   
    }

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */
    struct PRESTypePlugin *OuterPlugin_new(void)
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
        ::CorpusNestedKeys::OuterPlugin_on_participant_attached;
        plugin->onParticipantDetached =
        (PRESTypePluginOnParticipantDetachedCallback)
        ::CorpusNestedKeys::OuterPlugin_on_participant_detached;
        plugin->onEndpointAttached =
        (PRESTypePluginOnEndpointAttachedCallback)
        ::CorpusNestedKeys::OuterPlugin_on_endpoint_attached;
        plugin->onEndpointDetached =
        (PRESTypePluginOnEndpointDetachedCallback)
        ::CorpusNestedKeys::OuterPlugin_on_endpoint_detached;

        plugin->copySampleFnc =
        (PRESTypePluginCopySampleFunction)
        ::CorpusNestedKeys::OuterPlugin_copy_sample;
        plugin->createSampleFnc =
        (PRESTypePluginCreateSampleFunction)
        OuterPlugin_create_sample;
        plugin->destroySampleFnc =
        (PRESTypePluginDestroySampleFunction)
        OuterPlugin_destroy_sample;

        plugin->serializeFnc =
        (PRESTypePluginSerializeFunction) PRESTypePlugin_interpretedSerialize;
        plugin->deserializeFnc =
        (PRESTypePluginDeserializeFunction) PRESTypePlugin_interpretedDeserializeWithAlloc;
        plugin->getSerializedSampleMaxSizeFnc =
        (PRESTypePluginGetSerializedSampleMaxSizeFunction)
        ::CorpusNestedKeys::OuterPlugin_get_serialized_sample_max_size;
        plugin->getSerializedSampleMinSizeFnc =
        (PRESTypePluginGetSerializedSampleMinSizeFunction)
        PRESTypePlugin_interpretedGetSerializedSampleMinSize;
        plugin->getDeserializedSampleMaxSizeFnc = NULL;
        plugin->getSampleFnc =
        (PRESTypePluginGetSampleFunction)
        OuterPlugin_get_sample;
        plugin->returnSampleFnc =
        (PRESTypePluginReturnSampleFunction)
        OuterPlugin_return_sample;
        plugin->getKeyKindFnc =
        (PRESTypePluginGetKeyKindFunction)
        ::CorpusNestedKeys::OuterPlugin_get_key_kind;

        plugin->getSerializedKeyMaxSizeFnc =
        (PRESTypePluginGetSerializedKeyMaxSizeFunction)
        ::CorpusNestedKeys::OuterPlugin_get_serialized_key_max_size;
        plugin->serializeKeyFnc =
        (PRESTypePluginSerializeKeyFunction)
        PRESTypePlugin_interpretedSerializeKey;
        plugin->deserializeKeyFnc =
        (PRESTypePluginDeserializeKeyFunction)
        ::CorpusNestedKeys::OuterPlugin_deserialize_key;
        plugin->deserializeKeySampleFnc =
        (PRESTypePluginDeserializeKeySampleFunction)
        PRESTypePlugin_interpretedDeserializeKey;

        plugin-> instanceToKeyHashFnc =
        (PRESTypePluginInstanceToKeyHashFunction)
        PRESTypePlugin_interpretedInstanceToKeyHash;
        plugin->serializedSampleToKeyHashFnc =
        (PRESTypePluginSerializedSampleToKeyHashFunction)
        ::CorpusNestedKeys::OuterPlugin_serialized_sample_to_keyhash;

        plugin->getKeyFnc =
        (PRESTypePluginGetKeyFunction)
        OuterPlugin_get_key;
        plugin->returnKeyFnc =
        (PRESTypePluginReturnKeyFunction)
        OuterPlugin_return_key;

        plugin->instanceToKeyFnc =
        (PRESTypePluginInstanceToKeyFunction)
        ::CorpusNestedKeys::OuterPlugin_instance_to_key;
        plugin->keyToInstanceFnc =
        (PRESTypePluginKeyToInstanceFunction)
        ::CorpusNestedKeys::OuterPlugin_key_to_instance;
        plugin->serializedKeyToKeyHashFnc = NULL; /* Not supported yet */
        #ifdef NDDS_STANDALONE_TYPE
        plugin->typeCode = NULL;
        #else
        try {
            plugin->typeCode = (struct RTICdrTypeCode *)
            &::rti::topic::dynamic_type< ::CorpusNestedKeys::Outer >::get().native();
        } catch (...) {
            ::CorpusNestedKeys::OuterPlugin_delete(plugin);
            return NULL;
        }
        #endif
        plugin->languageKind = PRES_TYPEPLUGIN_CPPSTL_LANG;

        /* Serialized buffer */
        plugin->getBuffer =
        (PRESTypePluginGetBufferFunction)
        OuterPlugin_get_buffer;
        plugin->returnBuffer =
        (PRESTypePluginReturnBufferFunction)
        OuterPlugin_return_buffer;
        plugin->getBufferWithParams = NULL;
        plugin->returnBufferWithParams = NULL;
        plugin->getSerializedSampleSizeFnc =
        (PRESTypePluginGetSerializedSampleSizeFunction)
        PRESTypePlugin_interpretedGetSerializedSampleSize;

        plugin->getWriterLoanedSampleFnc = NULL;
        plugin->returnWriterLoanedSampleFnc = NULL;
        plugin->returnWriterLoanedSampleFromCookieFnc = NULL;
        plugin->validateWriterLoanedSampleFnc = NULL;
        plugin->setWriterLoanedSampleSerializedStateFnc = NULL;

        static const char * TYPE_NAME = "CorpusNestedKeys::Outer";
        plugin->endpointTypeName = TYPE_NAME;
        plugin->isMetpType = RTI_FALSE;
        plugin->isRecursiveType = RTI_FALSE;
        return plugin;
    }

    void
    OuterPlugin_delete(struct PRESTypePlugin *plugin)
    {
        RTIOsapiHeap_freeStructure(plugin);
    }
} /* namespace CorpusNestedKeys  */

#undef RTI_CDR_CURRENT_SUBMODULE
