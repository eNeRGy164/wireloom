

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-boundaries.idl
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
#include "01-boundariesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "01-boundaries.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusPrimitiveBoundaries {

    // ---- Sample:

    Sample::Sample() :
        shortValue (0) ,
        longValue (0) ,
        longLongValue (0ll) ,
        int8Value (0) ,
        int64Value (0ll) ,
        floatValue (0.0f) ,
        doubleValue (0.0)  {

    }

    Sample::Sample (int16_t shortValue_,int32_t longValue_,int64_t longLongValue_,int8_t int8Value_,int64_t int64Value_,float floatValue_,double doubleValue_) {
        shortValue = shortValue_;
        longValue = longValue_;
        longLongValue = longLongValue_;
        int8Value = int8Value_;
        int64Value = int64Value_;
        floatValue = floatValue_;
        doubleValue = doubleValue_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.shortValue != b.shortValue) {
            return false;
        }
        if (a.longValue != b.longValue) {
            return false;
        }
        if (a.longLongValue != b.longLongValue) {
            return false;
        }
        if (a.int8Value != b.int8Value) {
            return false;
        }
        if (a.int64Value != b.int64Value) {
            return false;
        }
        if (std::fabs(a.floatValue - b.floatValue) > std::numeric_limits< float>::epsilon()
        && !(std::fabs(a.floatValue - b.floatValue) < (std::numeric_limits< float>::min)())) {
            return false;
        }
        if (std::fabs(a.doubleValue - b.doubleValue) > std::numeric_limits< double>::epsilon()
        && !(std::fabs(a.doubleValue - b.doubleValue) < (std::numeric_limits< double>::min)())) {
            return false;
        }

        return true;
    }

    bool operator != (const Sample& a, const Sample& b) {
        return !operator ==(a, b);
    }

    void swap(Sample& a, Sample& b) noexcept
    {
        using std::swap;

        swap(a.shortValue, b.shortValue);
        swap(a.longValue, b.longValue);
        swap(a.longLongValue, b.longLongValue);
        swap(a.int8Value, b.int8Value);
        swap(a.int64Value, b.int64Value);
        swap(a.floatValue, b.floatValue);
        swap(a.doubleValue, b.doubleValue);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "shortValue: " << sample.shortValue<<", ";
        o << "longValue: " << sample.longValue<<", ";
        o << "longLongValue: " << sample.longLongValue<<", ";
        o << "int8Value: " << (int)  sample.int8Value<<", ";
        o << "int64Value: " << sample.int64Value<<", ";
        o << "floatValue: " << std::setprecision(9) << sample.floatValue<<", ";
        o << "doubleValue: " << std::setprecision(15) << sample.doubleValue;
        o <<"]";
        return o;
    }

} // namespace CorpusPrimitiveBoundaries  

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
        struct native_type_code< ::CorpusPrimitiveBoundaries::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sample_g_tc_members[7]=
                {

                    {
                        (char *)"shortValue",/* Member name */
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
                        (char *)"longValue",/* Member name */
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
                        (char *)"longLongValue",/* Member name */
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
                        (char *)"int8Value",/* Member name */
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
                        (char *)"int64Value",/* Member name */
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
                    }, 
                    {
                        (char *)"floatValue",/* Member name */
                        {
                            5,/* Representation ID */
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
                        (char *)"doubleValue",/* Member name */
                        {
                            6,/* Representation ID */
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

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusPrimitiveBoundaries::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        7, /* Number of members */
                        Sample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Sample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Sample_g_tc;
                }

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_short;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_longlong;
                Sample_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_int8;
                Sample_g_tc_members[4]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_longlong;
                Sample_g_tc_members[5]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_float;
                Sample_g_tc_members[6]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_double;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[0]._annotations._defaultValue._u.short_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[0]._annotations._minValue._u.short_value = RTIXCdrShort_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[0]._annotations._maxValue._u.short_value = RTIXCdrShort_MAX;
                Sample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Sample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[2]._annotations._defaultValue._u.long_long_value = 0ll;
                Sample_g_tc_members[2]._annotations._minValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[2]._annotations._minValue._u.long_long_value = RTIXCdrLongLong_MIN;
                Sample_g_tc_members[2]._annotations._maxValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[2]._annotations._maxValue._u.long_long_value = RTIXCdrLongLong_MAX;
                Sample_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_INT8;
                Sample_g_tc_members[3]._annotations._defaultValue._u.int8_value = 0;
                Sample_g_tc_members[3]._annotations._minValue._d = RTI_XCDR_TK_INT8;
                Sample_g_tc_members[3]._annotations._minValue._u.int8_value = RTIXCdrInt8_MIN;
                Sample_g_tc_members[3]._annotations._maxValue._d = RTI_XCDR_TK_INT8;
                Sample_g_tc_members[3]._annotations._maxValue._u.int8_value = RTIXCdrInt8_MAX;
                Sample_g_tc_members[4]._annotations._defaultValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[4]._annotations._defaultValue._u.long_long_value = 0ll;
                Sample_g_tc_members[4]._annotations._minValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[4]._annotations._minValue._u.long_long_value = RTIXCdrLongLong_MIN;
                Sample_g_tc_members[4]._annotations._maxValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[4]._annotations._maxValue._u.long_long_value = RTIXCdrLongLong_MAX;
                Sample_g_tc_members[5]._annotations._defaultValue._d = RTI_XCDR_TK_FLOAT;
                Sample_g_tc_members[5]._annotations._defaultValue._u.float_value = 0.0f;
                Sample_g_tc_members[5]._annotations._minValue._d = RTI_XCDR_TK_FLOAT;
                Sample_g_tc_members[5]._annotations._minValue._u.float_value = RTIXCdrFloat_MIN;
                Sample_g_tc_members[5]._annotations._maxValue._d = RTI_XCDR_TK_FLOAT;
                Sample_g_tc_members[5]._annotations._maxValue._u.float_value = RTIXCdrFloat_MAX;
                Sample_g_tc_members[6]._annotations._defaultValue._d = RTI_XCDR_TK_DOUBLE;
                Sample_g_tc_members[6]._annotations._defaultValue._u.double_value = 0.0;
                Sample_g_tc_members[6]._annotations._minValue._d = RTI_XCDR_TK_DOUBLE;
                Sample_g_tc_members[6]._annotations._minValue._u.double_value = RTIXCdrDouble_MIN;
                Sample_g_tc_members[6]._annotations._maxValue._d = RTI_XCDR_TK_DOUBLE;
                Sample_g_tc_members[6]._annotations._maxValue._u.double_value = RTIXCdrDouble_MAX;

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusPrimitiveBoundaries::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[7] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusPrimitiveBoundaries::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->shortValue - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->longValue - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->longLongValue - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->int8Value - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->int64Value - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->floatValue - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->doubleValue - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusPrimitiveBoundaries::Sample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Sample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Sample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Sample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Sample_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusPrimitiveBoundaries::Sample >;

                Sample_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Sample_g_typePlugin =
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

                return &Sample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusPrimitiveBoundaries::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusPrimitiveBoundaries::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusPrimitiveBoundaries::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusPrimitiveBoundaries::SamplePlugin_new,
                ::CorpusPrimitiveBoundaries::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusPrimitiveBoundaries::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusPrimitiveBoundaries::Sample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusPrimitiveBoundaries::Sample >::from_cdr_buffer(::CorpusPrimitiveBoundaries::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusPrimitiveBoundaries::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusPrimitiveBoundaries::Sample >::reset_sample(::CorpusPrimitiveBoundaries::Sample& sample) 
        {
            sample.shortValue = 0;
            sample.longValue = 0;
            sample.longLongValue = 0ll;
            sample.int8Value = 0;
            sample.int64Value = 0ll;
            sample.floatValue = 0.0f;
            sample.doubleValue = 0.0;
        }

        void topic_type_support< ::CorpusPrimitiveBoundaries::Sample >::allocate_sample(::CorpusPrimitiveBoundaries::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
