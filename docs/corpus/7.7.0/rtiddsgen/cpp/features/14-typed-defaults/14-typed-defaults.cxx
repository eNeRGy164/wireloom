

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-typed-defaults.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>
#include <cmath>
#include <limits>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "14-typed-defaultsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-typed-defaults.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- TypedDefaults:

TypedDefaults::TypedDefaults() :
    inherited (7L) ,
    overridden (9L) ,
    text ("alias") ,
    ratio (1.5) ,
    single (1.25f)  {

}

TypedDefaults::TypedDefaults (int32_t inherited_,int32_t overridden_,const ::omg::types::string_view& text_,double ratio_,float single_) {
    inherited = inherited_;
    overridden = overridden_;
    text = text_;
    ratio = ratio_;
    single = single_;
}

bool operator == (const TypedDefaults& a, const TypedDefaults& b) {

    if (a.inherited != b.inherited) {
        return false;
    }
    if (a.overridden != b.overridden) {
        return false;
    }
    if (a.text != b.text) {
        return false;
    }
    if (std::fabs(a.ratio - b.ratio) > std::numeric_limits< double>::epsilon()
    && !(std::fabs(a.ratio - b.ratio) < (std::numeric_limits< double>::min)())) {
        return false;
    }
    if (std::fabs(a.single - b.single) > std::numeric_limits< float>::epsilon()
    && !(std::fabs(a.single - b.single) < (std::numeric_limits< float>::min)())) {
        return false;
    }

    return true;
}

bool operator != (const TypedDefaults& a, const TypedDefaults& b) {
    return !operator ==(a, b);
}

void swap(TypedDefaults& a, TypedDefaults& b) noexcept
{
    using std::swap;

    swap(a.inherited, b.inherited);
    swap(a.overridden, b.overridden);
    swap(a.text, b.text);
    swap(a.ratio, b.ratio);
    swap(a.single, b.single);

}  
std::ostream& operator << (std::ostream& o,const TypedDefaults& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "inherited: " << sample.inherited<<", ";
    o << "overridden: " << sample.overridden<<", ";
    o << "text: " << sample.text<<", ";
    o << "ratio: " << std::setprecision(15) << sample.ratio<<", ";
    o << "single: " << std::setprecision(9) << sample.single;
    o <<"]";
    return o;
}

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
        struct native_type_code< ::Seven_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Seven_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"Seven", /* Name */
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
                    }}; /* Type code for  Seven */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Seven_g_tc;
                }

                Seven_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Seven_g_tc._data._typeCode =   (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Seven_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Seven_g_tc._data._annotations._defaultValue._u.long_value = 7L;
                Seven_g_tc._data._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Seven_g_tc._data._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Seven_g_tc._data._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Seven_g_tc._data._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Seven_g_tc._data._sampleAccessInfo = sample_access_info();
                Seven_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Seven_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Seven_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Seven_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Seven_g_sampleAccessInfo;
                }

                Seven_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Seven_g_sampleAccessInfo.memberAccessInfos =
                Seven_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::Seven);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Seven_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Seven_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Seven_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Seven_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::Seven >;

                Seven_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Seven_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Seven_g_typePlugin =
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

                return &Seven_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::Seven_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Seven_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::TextAlias_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode TextAlias_g_tc_string;

                static DDS_TypeCode TextAlias_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"TextAlias", /* Name */
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
                    }}; /* Type code for  TextAlias */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &TextAlias_g_tc;
                }

                TextAlias_g_tc_string = initialize_string_typecode((8L));

                TextAlias_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                TextAlias_g_tc._data._typeCode =   (RTICdrTypeCode *)&TextAlias_g_tc_string;

                /* Initialize the values for member annotations. */
                TextAlias_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                TextAlias_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "alias";

                TextAlias_g_tc._data._sampleAccessInfo = sample_access_info();
                TextAlias_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &TextAlias_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo TextAlias_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo TextAlias_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &TextAlias_g_sampleAccessInfo;
                }

                TextAlias_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                TextAlias_g_sampleAccessInfo.memberAccessInfos =
                TextAlias_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::TextAlias);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        TextAlias_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        TextAlias_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                TextAlias_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                TextAlias_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::TextAlias >;

                TextAlias_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &TextAlias_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin TextAlias_g_typePlugin =
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

                return &TextAlias_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::TextAlias_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::TextAlias_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::Ratio_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Ratio_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"Ratio", /* Name */
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
                    }}; /* Type code for  Ratio */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Ratio_g_tc;
                }

                Ratio_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Ratio_g_tc._data._typeCode =   (RTICdrTypeCode *)&DDS_g_tc_double;

                /* Initialize the values for member annotations. */
                Ratio_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_DOUBLE;
                Ratio_g_tc._data._annotations._defaultValue._u.double_value = 1.5;
                Ratio_g_tc._data._annotations._minValue._d = RTI_XCDR_TK_DOUBLE;
                Ratio_g_tc._data._annotations._minValue._u.double_value = RTIXCdrDouble_MIN;
                Ratio_g_tc._data._annotations._maxValue._d = RTI_XCDR_TK_DOUBLE;
                Ratio_g_tc._data._annotations._maxValue._u.double_value = RTIXCdrDouble_MAX;

                Ratio_g_tc._data._sampleAccessInfo = sample_access_info();
                Ratio_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Ratio_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Ratio_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Ratio_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Ratio_g_sampleAccessInfo;
                }

                Ratio_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Ratio_g_sampleAccessInfo.memberAccessInfos =
                Ratio_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::Ratio);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Ratio_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Ratio_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Ratio_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Ratio_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::Ratio >;

                Ratio_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Ratio_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Ratio_g_typePlugin =
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

                return &Ratio_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::Ratio_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Ratio_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::TypedDefaults > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member TypedDefaults_g_tc_members[5]=
                {

                    {
                        (char *)"inherited",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"overridden",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"text",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"ratio",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"single",/* Member name */
                        {
                            4,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode TypedDefaults_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"TypedDefaults", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        5, /* Number of members */
                        TypedDefaults_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for TypedDefaults*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &TypedDefaults_g_tc;
                }

                TypedDefaults_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                TypedDefaults_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Seven_AliasTag_t>::get().native();
                TypedDefaults_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Seven_AliasTag_t>::get().native();
                TypedDefaults_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::TextAlias_AliasTag_t>::get().native();
                TypedDefaults_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Ratio_AliasTag_t>::get().native();
                TypedDefaults_g_tc_members[4]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_float;

                /* Initialize the values for member annotations. */
                TypedDefaults_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                TypedDefaults_g_tc_members[0]._annotations._defaultValue._u.long_value = 7L;
                TypedDefaults_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                TypedDefaults_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                TypedDefaults_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                TypedDefaults_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                TypedDefaults_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                TypedDefaults_g_tc_members[1]._annotations._defaultValue._u.long_value = 9L;
                TypedDefaults_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                TypedDefaults_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                TypedDefaults_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                TypedDefaults_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                TypedDefaults_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                TypedDefaults_g_tc_members[2]._annotations._defaultValue._u.string_value = (DDS_Char *) "alias";
                TypedDefaults_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_DOUBLE;
                TypedDefaults_g_tc_members[3]._annotations._defaultValue._u.double_value = 1.5;
                TypedDefaults_g_tc_members[3]._annotations._minValue._d = RTI_XCDR_TK_DOUBLE;
                TypedDefaults_g_tc_members[3]._annotations._minValue._u.double_value = RTIXCdrDouble_MIN;
                TypedDefaults_g_tc_members[3]._annotations._maxValue._d = RTI_XCDR_TK_DOUBLE;
                TypedDefaults_g_tc_members[3]._annotations._maxValue._u.double_value = RTIXCdrDouble_MAX;
                TypedDefaults_g_tc_members[4]._annotations._defaultValue._d = RTI_XCDR_TK_FLOAT;
                TypedDefaults_g_tc_members[4]._annotations._defaultValue._u.float_value = 1.25f;
                TypedDefaults_g_tc_members[4]._annotations._minValue._d = RTI_XCDR_TK_FLOAT;
                TypedDefaults_g_tc_members[4]._annotations._minValue._u.float_value = RTIXCdrFloat_MIN;
                TypedDefaults_g_tc_members[4]._annotations._maxValue._d = RTI_XCDR_TK_FLOAT;
                TypedDefaults_g_tc_members[4]._annotations._maxValue._u.float_value = RTIXCdrFloat_MAX;

                TypedDefaults_g_tc._data._sampleAccessInfo = sample_access_info();
                TypedDefaults_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &TypedDefaults_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::TypedDefaults *sample;

                static RTIXCdrMemberAccessInfo TypedDefaults_g_memberAccessInfos[5] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo TypedDefaults_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &TypedDefaults_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::TypedDefaults);
                if (sample == NULL) {
                    return NULL;
                }

                TypedDefaults_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->inherited - (char *)sample);

                TypedDefaults_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->overridden - (char *)sample);

                TypedDefaults_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->text - (char *)sample);

                TypedDefaults_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->ratio - (char *)sample);

                TypedDefaults_g_memberAccessInfos[4].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->single - (char *)sample);

                TypedDefaults_g_sampleAccessInfo.memberAccessInfos =
                TypedDefaults_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::TypedDefaults);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        TypedDefaults_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        TypedDefaults_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                TypedDefaults_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                TypedDefaults_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::TypedDefaults >;

                TypedDefaults_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &TypedDefaults_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin TypedDefaults_g_typePlugin =
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

                return &TypedDefaults_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::TypedDefaults >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::TypedDefaults >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::TypedDefaults >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::TypedDefaultsPlugin_new,
                ::TypedDefaultsPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::TypedDefaults >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::TypedDefaults& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = TypedDefaultsPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = TypedDefaultsPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::TypedDefaults >::from_cdr_buffer(::TypedDefaults& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = TypedDefaultsPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::TypedDefaults from cdr buffer");
        }

        void topic_type_support< ::TypedDefaults >::reset_sample(::TypedDefaults& sample) 
        {
            sample.inherited = 7L;
            sample.overridden = 9L;
            sample.text = "alias";
            sample.ratio = 1.5;
            sample.single = 1.25f;
        }

        void topic_type_support< ::TypedDefaults >::allocate_sample(::TypedDefaults& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text,  -1, 8L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
