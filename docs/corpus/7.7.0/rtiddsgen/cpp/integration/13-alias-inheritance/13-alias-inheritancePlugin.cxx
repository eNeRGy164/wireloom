

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
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

#include "13-alias-inheritancePlugin.hpp"

namespace CorpusIntegrationTrace {

    /* ----------------------------------------------------------------------------
    *  Type Producer
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    ProducerPluginSupport_create_dataI(void)
    {
        try {
            Producer *sample = new Producer();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Producer *
    ProducerPluginSupport_create_data(void)
    {
        return (Producer *) ProducerPluginSupport_create_dataI();
    }

    void
    ProducerPluginSupport_destroy_data(
        Producer *sample)
    {
        delete sample;
    }

    void
    ProducerPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusIntegrationTrace::ProducerPluginSupport_destroy_data((Producer *) sample);
    }

    RTIBool
    ProducerPluginSupport_copy_data(
        Producer *dst,
        const Producer *src)
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

    RTIBool
    ProducerPlugin_copy_sample(
        PRESTypePluginEndpointData,
        Producer *dst,
        const Producer *src)
    {
        return ::CorpusIntegrationTrace::ProducerPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    ProducerPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int
    ProducerPlugin_get_serialized_sample_max_size(
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
    ProducerPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    ProducerPlugin_get_serialized_key_max_size(
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
    ProducerPlugin_get_serialized_key_max_size_for_keyhash(
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

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */

    /* ----------------------------------------------------------------------------
    *  Type Context
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    ContextPluginSupport_create_dataI(void)
    {
        try {
            Context *sample = new Context();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Context *
    ContextPluginSupport_create_data(void)
    {
        return (Context *) ContextPluginSupport_create_dataI();
    }

    void
    ContextPluginSupport_destroy_data(
        Context *sample)
    {
        delete sample;
    }

    void
    ContextPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusIntegrationTrace::ContextPluginSupport_destroy_data((Context *) sample);
    }

    RTIBool
    ContextPluginSupport_copy_data(
        Context *dst,
        const Context *src)
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

    RTIBool
    ContextPlugin_copy_sample(
        PRESTypePluginEndpointData,
        Context *dst,
        const Context *src)
    {
        return ::CorpusIntegrationTrace::ContextPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    ContextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    ContextPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Context *sample,
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
                (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Context >::get().native()
                ;
            } catch (...) {
                return RTI_FALSE;
            }
            pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
            Context,
            true, true, true>();

            encapsulationId = DDS_TypeCode_get_native_encapsulation(
                (DDS_TypeCode *) plugin.typeCode,
                representation);

            if (encapsulationId == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                return RTI_FALSE;
            }

            epd._maxSizeSerializedSample =
            ContextPlugin_get_serialized_sample_max_size(
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
    ContextPlugin_deserialize_from_cdr_buffer(
        Context *sample,
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
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Context >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        Context,
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
    ContextPlugin_get_serialized_sample_max_size(
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
    ContextPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    RTIBool ContextPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Context **sample,
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
    ContextPlugin_get_serialized_key_max_size(
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
    ContextPlugin_get_serialized_key_max_size_for_keyhash(
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

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */

    /* ----------------------------------------------------------------------------
    *  Type Base
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    BasePluginSupport_create_dataI(void)
    {
        try {
            Base *sample = new Base();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Base *
    BasePluginSupport_create_data(void)
    {
        return (Base *) BasePluginSupport_create_dataI();
    }

    void
    BasePluginSupport_destroy_data(
        Base *sample)
    {
        delete sample;
    }

    void
    BasePluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusIntegrationTrace::BasePluginSupport_destroy_data((Base *) sample);
    }

    RTIBool
    BasePluginSupport_copy_data(
        Base *dst,
        const Base *src)
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

    RTIBool
    BasePlugin_copy_sample(
        PRESTypePluginEndpointData,
        Base *dst,
        const Base *src)
    {
        return ::CorpusIntegrationTrace::BasePluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    BasePlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    BasePlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Base *sample,
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
                (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Base >::get().native()
                ;
            } catch (...) {
                return RTI_FALSE;
            }
            pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
            Base,
            true, true, true>();

            encapsulationId = DDS_TypeCode_get_native_encapsulation(
                (DDS_TypeCode *) plugin.typeCode,
                representation);

            if (encapsulationId == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                return RTI_FALSE;
            }

            epd._maxSizeSerializedSample =
            BasePlugin_get_serialized_sample_max_size(
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
    BasePlugin_deserialize_from_cdr_buffer(
        Base *sample,
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
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Base >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        Base,
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
    BasePlugin_get_serialized_sample_max_size(
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
    BasePlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    RTIBool BasePlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Base **sample,
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
    BasePlugin_get_serialized_key_max_size(
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
    BasePlugin_get_serialized_key_max_size_for_keyhash(
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

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */

    /* ----------------------------------------------------------------------------
    *  Type Derived
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    DerivedPluginSupport_create_dataI(void)
    {
        try {
            Derived *sample = new Derived();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Derived *
    DerivedPluginSupport_create_data(void)
    {
        return (Derived *) DerivedPluginSupport_create_dataI();
    }

    void
    DerivedPluginSupport_destroy_data(
        Derived *sample)
    {
        delete sample;
    }

    void
    DerivedPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusIntegrationTrace::DerivedPluginSupport_destroy_data((Derived *) sample);
    }

    RTIBool
    DerivedPluginSupport_copy_data(
        Derived *dst,
        const Derived *src)
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

    RTIBool
    DerivedPlugin_copy_sample(
        PRESTypePluginEndpointData,
        Derived *dst,
        const Derived *src)
    {
        return ::CorpusIntegrationTrace::DerivedPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    DerivedPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    RTIBool
    DerivedPlugin_serialize_to_cdr_buffer(
        char * buffer,
        unsigned int * length,
        const Derived *sample,
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
                (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Derived >::get().native()
                ;
            } catch (...) {
                return RTI_FALSE;
            }
            pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
            Derived,
            true, true, true>();

            encapsulationId = DDS_TypeCode_get_native_encapsulation(
                (DDS_TypeCode *) plugin.typeCode,
                representation);

            if (encapsulationId == RTI_CDR_ENCAPSULATION_ID_INVALID) {
                return RTI_FALSE;
            }

            epd._maxSizeSerializedSample =
            DerivedPlugin_get_serialized_sample_max_size(
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
    DerivedPlugin_deserialize_from_cdr_buffer(
        Derived *sample,
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
            (RTIXCdrTypeCode *)&::rti::topic::dynamic_type< Derived >::get().native()
            ;
        } catch (...) {
            return RTI_FALSE;
        }
        pd.programs = ::rti::topic::interpreter::get_cdr_serialization_programs<
        Derived,
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
    DerivedPlugin_get_serialized_sample_max_size(
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
    DerivedPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    RTIBool DerivedPlugin_deserialize_key(
        PRESTypePluginEndpointData endpoint_data,
        Derived **sample,
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
    DerivedPlugin_get_serialized_key_max_size(
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
    DerivedPlugin_get_serialized_key_max_size_for_keyhash(
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

    /* ------------------------------------------------------------------------
    * Plug-in Installation Methods
    * ------------------------------------------------------------------------ */
} /* namespace CorpusIntegrationTrace  */

#undef RTI_CDR_CURRENT_SUBMODULE
