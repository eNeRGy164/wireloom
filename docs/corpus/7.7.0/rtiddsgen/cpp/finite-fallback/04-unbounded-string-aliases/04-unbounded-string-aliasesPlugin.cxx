

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
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

#include "04-unbounded-string-aliasesPlugin.hpp"

namespace CorpusUnboundedStrings {

    /* ----------------------------------------------------------------------------
    *  Type Text
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    TextPluginSupport_create_dataI(void)
    {
        try {
            Text *sample = new Text();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    Text *
    TextPluginSupport_create_data(void)
    {
        return (Text *) TextPluginSupport_create_dataI();
    }

    void
    TextPluginSupport_destroy_data(
        Text *sample)
    {
        delete sample;
    }

    void
    TextPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusUnboundedStrings::TextPluginSupport_destroy_data((Text *) sample);
    }

    RTIBool
    TextPluginSupport_copy_data(
        Text *dst,
        const Text *src)
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
    TextPlugin_copy_sample(
        PRESTypePluginEndpointData,
        Text *dst,
        const Text *src)
    {
        return ::CorpusUnboundedStrings::TextPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    TextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int
    TextPlugin_get_serialized_sample_max_size(
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
    TextPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    TextPlugin_get_serialized_key_max_size(
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
    TextPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type WideText
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    void *
    WideTextPluginSupport_create_dataI(void)
    {
        try {
            WideText *sample = new WideText();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    WideText *
    WideTextPluginSupport_create_data(void)
    {
        return (WideText *) WideTextPluginSupport_create_dataI();
    }

    void
    WideTextPluginSupport_destroy_data(
        WideText *sample)
    {
        delete sample;
    }

    void
    WideTextPluginSupport_destroy_dataI(
        void *sample)
    {
        ::CorpusUnboundedStrings::WideTextPluginSupport_destroy_data((WideText *) sample);
    }

    RTIBool
    WideTextPluginSupport_copy_data(
        WideText *dst,
        const WideText *src)
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
    WideTextPlugin_copy_sample(
        PRESTypePluginEndpointData,
        WideText *dst,
        const WideText *src)
    {
        return ::CorpusUnboundedStrings::WideTextPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int
    WideTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int
    WideTextPlugin_get_serialized_sample_max_size(
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
    WideTextPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    WideTextPlugin_get_serialized_key_max_size(
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
    WideTextPlugin_get_serialized_key_max_size_for_keyhash(
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
} /* namespace CorpusUnboundedStrings  */

#undef RTI_CDR_CURRENT_SUBMODULE
