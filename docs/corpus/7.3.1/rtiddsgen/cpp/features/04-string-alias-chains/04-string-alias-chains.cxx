

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "04-string-alias-chainsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "04-string-alias-chains.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusStringAliasChains {

} // namespace CorpusStringAliasChains  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        template<>
        struct native_type_code< ::CorpusStringAliasChains::NarrowText_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode NarrowText_g_tc_string;

                static DDS_TypeCode NarrowText_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::NarrowText", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  NarrowText */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &NarrowText_g_tc;
                }

                NarrowText_g_tc_string = initialize_string_typecode((255L));

                NarrowText_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NarrowText_g_tc._data._typeCode =  (RTICdrTypeCode *)&NarrowText_g_tc_string;

                /* Initialize the values for member annotations. */
                NarrowText_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                NarrowText_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                NarrowText_g_tc._data._sampleAccessInfo = sample_access_info();
                NarrowText_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &NarrowText_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo NarrowText_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NarrowText_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &NarrowText_g_sampleAccessInfo;
                }

                NarrowText_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                NarrowText_g_sampleAccessInfo.memberAccessInfos = 
                NarrowText_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::NarrowText);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NarrowText_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NarrowText_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NarrowText_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NarrowText_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::NarrowText >;

                NarrowText_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &NarrowText_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NarrowText_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &NarrowText_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::NarrowText_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::NarrowText_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::NarrowTextChain_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode NarrowTextChain_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::NarrowTextChain", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  NarrowTextChain */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &NarrowTextChain_g_tc;
                }

                NarrowTextChain_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NarrowTextChain_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::NarrowText_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                NarrowTextChain_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                NarrowTextChain_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                NarrowTextChain_g_tc._data._sampleAccessInfo = sample_access_info();
                NarrowTextChain_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &NarrowTextChain_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo NarrowTextChain_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NarrowTextChain_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &NarrowTextChain_g_sampleAccessInfo;
                }

                NarrowTextChain_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                NarrowTextChain_g_sampleAccessInfo.memberAccessInfos = 
                NarrowTextChain_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::NarrowTextChain);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NarrowTextChain_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NarrowTextChain_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NarrowTextChain_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NarrowTextChain_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::NarrowTextChain >;

                NarrowTextChain_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &NarrowTextChain_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NarrowTextChain_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &NarrowTextChain_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::NarrowTextChain_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::NarrowTextChain_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::BoundedNarrowText_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedNarrowText_g_tc_string;

                static DDS_TypeCode BoundedNarrowText_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::BoundedNarrowText", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  BoundedNarrowText */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedNarrowText_g_tc;
                }

                BoundedNarrowText_g_tc_string = initialize_string_typecode((16L));

                BoundedNarrowText_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedNarrowText_g_tc._data._typeCode =  (RTICdrTypeCode *)&BoundedNarrowText_g_tc_string;

                /* Initialize the values for member annotations. */
                BoundedNarrowText_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                BoundedNarrowText_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                BoundedNarrowText_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedNarrowText_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedNarrowText_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedNarrowText_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedNarrowText_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedNarrowText_g_sampleAccessInfo;
                }

                BoundedNarrowText_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedNarrowText_g_sampleAccessInfo.memberAccessInfos = 
                BoundedNarrowText_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::BoundedNarrowText);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedNarrowText_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedNarrowText_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedNarrowText_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedNarrowText_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::BoundedNarrowText >;

                BoundedNarrowText_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedNarrowText_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedNarrowText_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &BoundedNarrowText_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::BoundedNarrowText_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::BoundedNarrowText_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::BoundedNarrowTextChain_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedNarrowTextChain_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::BoundedNarrowTextChain", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  BoundedNarrowTextChain */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedNarrowTextChain_g_tc;
                }

                BoundedNarrowTextChain_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedNarrowTextChain_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedNarrowText_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                BoundedNarrowTextChain_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                BoundedNarrowTextChain_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                BoundedNarrowTextChain_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedNarrowTextChain_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedNarrowTextChain_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedNarrowTextChain_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedNarrowTextChain_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedNarrowTextChain_g_sampleAccessInfo;
                }

                BoundedNarrowTextChain_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedNarrowTextChain_g_sampleAccessInfo.memberAccessInfos = 
                BoundedNarrowTextChain_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::BoundedNarrowTextChain);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedNarrowTextChain_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedNarrowTextChain_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedNarrowTextChain_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedNarrowTextChain_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::BoundedNarrowTextChain >;

                BoundedNarrowTextChain_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedNarrowTextChain_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedNarrowTextChain_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &BoundedNarrowTextChain_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::BoundedNarrowTextChain_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::BoundedNarrowTextChain_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::BoundedNarrowTextChain2_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedNarrowTextChain2_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::BoundedNarrowTextChain2", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  BoundedNarrowTextChain2 */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedNarrowTextChain2_g_tc;
                }

                BoundedNarrowTextChain2_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedNarrowTextChain2_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedNarrowTextChain_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                BoundedNarrowTextChain2_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                BoundedNarrowTextChain2_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                BoundedNarrowTextChain2_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedNarrowTextChain2_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedNarrowTextChain2_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedNarrowTextChain2_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedNarrowTextChain2_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedNarrowTextChain2_g_sampleAccessInfo;
                }

                BoundedNarrowTextChain2_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedNarrowTextChain2_g_sampleAccessInfo.memberAccessInfos = 
                BoundedNarrowTextChain2_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::BoundedNarrowTextChain2);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedNarrowTextChain2_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedNarrowTextChain2_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedNarrowTextChain2_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedNarrowTextChain2_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::BoundedNarrowTextChain2 >;

                BoundedNarrowTextChain2_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedNarrowTextChain2_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedNarrowTextChain2_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &BoundedNarrowTextChain2_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::BoundedNarrowTextChain2_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::BoundedNarrowTextChain2_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::WideText_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode WideText_g_tc_string;

                static DDS_TypeCode WideText_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::WideText", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  WideText */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &WideText_g_tc;
                }

                WideText_g_tc_string = initialize_wstring_typecode((255L));

                WideText_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                WideText_g_tc._data._typeCode =  (RTICdrTypeCode *)&WideText_g_tc_string;

                /* Initialize the values for member annotations. */
                WideText_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                WideText_g_tc._data._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                WideText_g_tc._data._sampleAccessInfo = sample_access_info();
                WideText_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &WideText_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo WideText_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo WideText_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &WideText_g_sampleAccessInfo;
                }

                WideText_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                WideText_g_sampleAccessInfo.memberAccessInfos = 
                WideText_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::WideText);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        WideText_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        WideText_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                WideText_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                WideText_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::WideText >;

                WideText_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &WideText_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin WideText_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &WideText_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::WideText_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::WideText_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::WideTextChain_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode WideTextChain_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::WideTextChain", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  WideTextChain */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &WideTextChain_g_tc;
                }

                WideTextChain_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                WideTextChain_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::WideText_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                WideTextChain_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                WideTextChain_g_tc._data._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                WideTextChain_g_tc._data._sampleAccessInfo = sample_access_info();
                WideTextChain_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &WideTextChain_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo WideTextChain_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo WideTextChain_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &WideTextChain_g_sampleAccessInfo;
                }

                WideTextChain_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                WideTextChain_g_sampleAccessInfo.memberAccessInfos = 
                WideTextChain_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::WideTextChain);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        WideTextChain_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        WideTextChain_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                WideTextChain_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                WideTextChain_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::WideTextChain >;

                WideTextChain_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &WideTextChain_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin WideTextChain_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &WideTextChain_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::WideTextChain_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::WideTextChain_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::BoundedWideText_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedWideText_g_tc_string;

                static DDS_TypeCode BoundedWideText_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::BoundedWideText", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  BoundedWideText */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedWideText_g_tc;
                }

                BoundedWideText_g_tc_string = initialize_wstring_typecode((8L));

                BoundedWideText_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedWideText_g_tc._data._typeCode =  (RTICdrTypeCode *)&BoundedWideText_g_tc_string;

                /* Initialize the values for member annotations. */
                BoundedWideText_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                BoundedWideText_g_tc._data._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                BoundedWideText_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedWideText_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedWideText_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedWideText_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedWideText_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedWideText_g_sampleAccessInfo;
                }

                BoundedWideText_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedWideText_g_sampleAccessInfo.memberAccessInfos = 
                BoundedWideText_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::BoundedWideText);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedWideText_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedWideText_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedWideText_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedWideText_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::BoundedWideText >;

                BoundedWideText_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedWideText_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedWideText_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &BoundedWideText_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::BoundedWideText_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::BoundedWideText_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::BoundedWideTextChain_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedWideTextChain_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::BoundedWideTextChain", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  BoundedWideTextChain */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedWideTextChain_g_tc;
                }

                BoundedWideTextChain_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedWideTextChain_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedWideText_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                BoundedWideTextChain_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                BoundedWideTextChain_g_tc._data._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                BoundedWideTextChain_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedWideTextChain_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedWideTextChain_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedWideTextChain_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedWideTextChain_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedWideTextChain_g_sampleAccessInfo;
                }

                BoundedWideTextChain_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedWideTextChain_g_sampleAccessInfo.memberAccessInfos = 
                BoundedWideTextChain_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::BoundedWideTextChain);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedWideTextChain_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedWideTextChain_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedWideTextChain_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedWideTextChain_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::BoundedWideTextChain >;

                BoundedWideTextChain_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedWideTextChain_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedWideTextChain_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &BoundedWideTextChain_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::BoundedWideTextChain_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::BoundedWideTextChain_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusStringAliasChains::BoundedWideTextChain2_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedWideTextChain2_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusStringAliasChains::BoundedWideTextChain2", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  BoundedWideTextChain2 */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedWideTextChain2_g_tc;
                }

                BoundedWideTextChain2_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedWideTextChain2_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedWideTextChain_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                BoundedWideTextChain2_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                BoundedWideTextChain2_g_tc._data._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                BoundedWideTextChain2_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedWideTextChain2_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedWideTextChain2_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedWideTextChain2_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedWideTextChain2_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedWideTextChain2_g_sampleAccessInfo;
                }

                BoundedWideTextChain2_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedWideTextChain2_g_sampleAccessInfo.memberAccessInfos = 
                BoundedWideTextChain2_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::BoundedWideTextChain2);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedWideTextChain2_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedWideTextChain2_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedWideTextChain2_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedWideTextChain2_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::BoundedWideTextChain2 >;

                BoundedWideTextChain2_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedWideTextChain2_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedWideTextChain2_g_typePlugin = 
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &BoundedWideTextChain2_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusStringAliasChains::BoundedWideTextChain2_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::BoundedWideTextChain2_AliasTag_t >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
    }
}  

#endif // NDDS_STANDALONE_TYPE
