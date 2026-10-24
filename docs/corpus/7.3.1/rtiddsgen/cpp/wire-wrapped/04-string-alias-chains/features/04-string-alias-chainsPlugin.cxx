

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

#include "04-string-alias-chainsPlugin.hpp"

namespace CorpusStringAliasChains {

    /* ----------------------------------------------------------------------------
    *  Type NarrowText
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    NarrowText *
    NarrowTextPluginSupport_create_data(void)
    {
        try {
            NarrowText *sample = new NarrowText();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    NarrowTextPluginSupport_destroy_data(
        NarrowText *sample) 
    {
        delete sample;
    }

    RTIBool 
    NarrowTextPluginSupport_copy_data(
        NarrowText *dst,
        const NarrowText *src)
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
    NarrowTextPlugin_copy_sample(
        PRESTypePluginEndpointData,
        NarrowText *dst,
        const NarrowText *src)
    {
        return ::CorpusStringAliasChains::NarrowTextPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    NarrowTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    NarrowTextPlugin_get_serialized_sample_max_size(
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
    NarrowTextPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    NarrowTextPlugin_get_serialized_key_max_size(
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
    NarrowTextPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type NarrowTextChain
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    NarrowTextChain *
    NarrowTextChainPluginSupport_create_data(void)
    {
        try {
            NarrowTextChain *sample = new NarrowTextChain();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    NarrowTextChainPluginSupport_destroy_data(
        NarrowTextChain *sample) 
    {
        delete sample;
    }

    RTIBool 
    NarrowTextChainPluginSupport_copy_data(
        NarrowTextChain *dst,
        const NarrowTextChain *src)
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
    NarrowTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData,
        NarrowTextChain *dst,
        const NarrowTextChain *src)
    {
        return ::CorpusStringAliasChains::NarrowTextChainPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    NarrowTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    NarrowTextChainPlugin_get_serialized_sample_max_size(
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
    NarrowTextChainPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    NarrowTextChainPlugin_get_serialized_key_max_size(
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
    NarrowTextChainPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type BoundedNarrowText
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    BoundedNarrowText *
    BoundedNarrowTextPluginSupport_create_data(void)
    {
        try {
            BoundedNarrowText *sample = new BoundedNarrowText();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    BoundedNarrowTextPluginSupport_destroy_data(
        BoundedNarrowText *sample) 
    {
        delete sample;
    }

    RTIBool 
    BoundedNarrowTextPluginSupport_copy_data(
        BoundedNarrowText *dst,
        const BoundedNarrowText *src)
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
    BoundedNarrowTextPlugin_copy_sample(
        PRESTypePluginEndpointData,
        BoundedNarrowText *dst,
        const BoundedNarrowText *src)
    {
        return ::CorpusStringAliasChains::BoundedNarrowTextPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    BoundedNarrowTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    BoundedNarrowTextPlugin_get_serialized_sample_max_size(
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
    BoundedNarrowTextPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    BoundedNarrowTextPlugin_get_serialized_key_max_size(
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
    BoundedNarrowTextPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type BoundedNarrowTextChain
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    BoundedNarrowTextChain *
    BoundedNarrowTextChainPluginSupport_create_data(void)
    {
        try {
            BoundedNarrowTextChain *sample = new BoundedNarrowTextChain();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    BoundedNarrowTextChainPluginSupport_destroy_data(
        BoundedNarrowTextChain *sample) 
    {
        delete sample;
    }

    RTIBool 
    BoundedNarrowTextChainPluginSupport_copy_data(
        BoundedNarrowTextChain *dst,
        const BoundedNarrowTextChain *src)
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
    BoundedNarrowTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData,
        BoundedNarrowTextChain *dst,
        const BoundedNarrowTextChain *src)
    {
        return ::CorpusStringAliasChains::BoundedNarrowTextChainPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    BoundedNarrowTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    BoundedNarrowTextChainPlugin_get_serialized_sample_max_size(
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
    BoundedNarrowTextChainPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    BoundedNarrowTextChainPlugin_get_serialized_key_max_size(
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
    BoundedNarrowTextChainPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type BoundedNarrowTextChain2
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    BoundedNarrowTextChain2 *
    BoundedNarrowTextChain2PluginSupport_create_data(void)
    {
        try {
            BoundedNarrowTextChain2 *sample = new BoundedNarrowTextChain2();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    BoundedNarrowTextChain2PluginSupport_destroy_data(
        BoundedNarrowTextChain2 *sample) 
    {
        delete sample;
    }

    RTIBool 
    BoundedNarrowTextChain2PluginSupport_copy_data(
        BoundedNarrowTextChain2 *dst,
        const BoundedNarrowTextChain2 *src)
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
    BoundedNarrowTextChain2Plugin_copy_sample(
        PRESTypePluginEndpointData,
        BoundedNarrowTextChain2 *dst,
        const BoundedNarrowTextChain2 *src)
    {
        return ::CorpusStringAliasChains::BoundedNarrowTextChain2PluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    BoundedNarrowTextChain2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    BoundedNarrowTextChain2Plugin_get_serialized_sample_max_size(
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
    BoundedNarrowTextChain2Plugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    BoundedNarrowTextChain2Plugin_get_serialized_key_max_size(
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
    BoundedNarrowTextChain2Plugin_get_serialized_key_max_size_for_keyhash(
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

    WideText *
    WideTextPluginSupport_create_data(void)
    {
        try {
            WideText *sample = new WideText();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    WideTextPluginSupport_destroy_data(
        WideText *sample) 
    {
        delete sample;
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
        return ::CorpusStringAliasChains::WideTextPluginSupport_copy_data(dst,src);
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

    /* ----------------------------------------------------------------------------
    *  Type WideTextChain
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    WideTextChain *
    WideTextChainPluginSupport_create_data(void)
    {
        try {
            WideTextChain *sample = new WideTextChain();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    WideTextChainPluginSupport_destroy_data(
        WideTextChain *sample) 
    {
        delete sample;
    }

    RTIBool 
    WideTextChainPluginSupport_copy_data(
        WideTextChain *dst,
        const WideTextChain *src)
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
    WideTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData,
        WideTextChain *dst,
        const WideTextChain *src)
    {
        return ::CorpusStringAliasChains::WideTextChainPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    WideTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    WideTextChainPlugin_get_serialized_sample_max_size(
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
    WideTextChainPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    WideTextChainPlugin_get_serialized_key_max_size(
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
    WideTextChainPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type BoundedWideText
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    BoundedWideText *
    BoundedWideTextPluginSupport_create_data(void)
    {
        try {
            BoundedWideText *sample = new BoundedWideText();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    BoundedWideTextPluginSupport_destroy_data(
        BoundedWideText *sample) 
    {
        delete sample;
    }

    RTIBool 
    BoundedWideTextPluginSupport_copy_data(
        BoundedWideText *dst,
        const BoundedWideText *src)
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
    BoundedWideTextPlugin_copy_sample(
        PRESTypePluginEndpointData,
        BoundedWideText *dst,
        const BoundedWideText *src)
    {
        return ::CorpusStringAliasChains::BoundedWideTextPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    BoundedWideTextPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    BoundedWideTextPlugin_get_serialized_sample_max_size(
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
    BoundedWideTextPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    BoundedWideTextPlugin_get_serialized_key_max_size(
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
    BoundedWideTextPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type BoundedWideTextChain
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    BoundedWideTextChain *
    BoundedWideTextChainPluginSupport_create_data(void)
    {
        try {
            BoundedWideTextChain *sample = new BoundedWideTextChain();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    BoundedWideTextChainPluginSupport_destroy_data(
        BoundedWideTextChain *sample) 
    {
        delete sample;
    }

    RTIBool 
    BoundedWideTextChainPluginSupport_copy_data(
        BoundedWideTextChain *dst,
        const BoundedWideTextChain *src)
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
    BoundedWideTextChainPlugin_copy_sample(
        PRESTypePluginEndpointData,
        BoundedWideTextChain *dst,
        const BoundedWideTextChain *src)
    {
        return ::CorpusStringAliasChains::BoundedWideTextChainPluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    BoundedWideTextChainPlugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    BoundedWideTextChainPlugin_get_serialized_sample_max_size(
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
    BoundedWideTextChainPlugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    BoundedWideTextChainPlugin_get_serialized_key_max_size(
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
    BoundedWideTextChainPlugin_get_serialized_key_max_size_for_keyhash(
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
    *  Type BoundedWideTextChain2
    * -------------------------------------------------------------------------- */

    /* -----------------------------------------------------------------------------
    Support functions:
    * -------------------------------------------------------------------------- */

    BoundedWideTextChain2 *
    BoundedWideTextChain2PluginSupport_create_data(void)
    {
        try {
            BoundedWideTextChain2 *sample = new BoundedWideTextChain2();
            ::rti::topic::allocate_sample(*sample);
            return sample;
        } catch (...) {
            return NULL;
        }
    }

    void 
    BoundedWideTextChain2PluginSupport_destroy_data(
        BoundedWideTextChain2 *sample) 
    {
        delete sample;
    }

    RTIBool 
    BoundedWideTextChain2PluginSupport_copy_data(
        BoundedWideTextChain2 *dst,
        const BoundedWideTextChain2 *src)
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
    BoundedWideTextChain2Plugin_copy_sample(
        PRESTypePluginEndpointData,
        BoundedWideTextChain2 *dst,
        const BoundedWideTextChain2 *src)
    {
        return ::CorpusStringAliasChains::BoundedWideTextChain2PluginSupport_copy_data(dst,src);
    }

    /* ----------------------------------------------------------------------------
    (De)Serialize functions:
    * ------------------------------------------------------------------------- */
    unsigned int 
    BoundedWideTextChain2Plugin_get_serialized_sample_max_size(
        PRESTypePluginEndpointData endpoint_data,
        RTIBool include_encapsulation,
        RTIEncapsulationId encapsulation_id,
        unsigned int current_alignment);

    unsigned int 
    BoundedWideTextChain2Plugin_get_serialized_sample_max_size(
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
    BoundedWideTextChain2Plugin_get_key_kind(void)
    {
        return PRES_TYPEPLUGIN_NO_KEY;
    }

    unsigned int
    BoundedWideTextChain2Plugin_get_serialized_key_max_size(
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
    BoundedWideTextChain2Plugin_get_serialized_key_max_size_for_keyhash(
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
} /* namespace CorpusStringAliasChains  */
#undef RTI_CDR_CURRENT_SUBMODULE 
