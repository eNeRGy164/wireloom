

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-full-widths.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "01-full-widthsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "01-full-widths.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusFullWidths {

    // ---- Sample:

    Sample::Sample() :
        i8 (0) ,
        i16 (0) ,
        i32 (0) ,
        i64 (0ll) ,
        u8 (0) ,
        u16 (0) ,
        u32 (0u) ,
        u64 (0ull)  {

    }

    Sample::Sample (int8_t i8_,int16_t i16_,int32_t i32_,int64_t i64_,uint8_t u8_,uint16_t u16_,uint32_t u32_,uint64_t u64_) {
        i8 = i8_;
        i16 = i16_;
        i32 = i32_;
        i64 = i64_;
        u8 = u8_;
        u16 = u16_;
        u32 = u32_;
        u64 = u64_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.i8 != b.i8) {
            return false;
        }
        if (a.i16 != b.i16) {
            return false;
        }
        if (a.i32 != b.i32) {
            return false;
        }
        if (a.i64 != b.i64) {
            return false;
        }
        if (a.u8 != b.u8) {
            return false;
        }
        if (a.u16 != b.u16) {
            return false;
        }
        if (a.u32 != b.u32) {
            return false;
        }
        if (a.u64 != b.u64) {
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

        swap(a.i8, b.i8);
        swap(a.i16, b.i16);
        swap(a.i32, b.i32);
        swap(a.i64, b.i64);
        swap(a.u8, b.u8);
        swap(a.u16, b.u16);
        swap(a.u32, b.u32);
        swap(a.u64, b.u64);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "i8: " << (int)  sample.i8<<", ";
        o << "i16: " << sample.i16<<", ";
        o << "i32: " << sample.i32<<", ";
        o << "i64: " << sample.i64<<", ";
        o << "u8: " << (int)  sample.u8<<", ";
        o << "u16: " << sample.u16<<", ";
        o << "u32: " << sample.u32<<", ";
        o << "u64: " << sample.u64;
        o <<"]";
        return o;
    }

} // namespace CorpusFullWidths  

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
        struct native_type_code< ::CorpusFullWidths::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sample_g_tc_members[8]=
                {

                    {
                        (char *)"i8",/* Member name */
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
                        (char *)"i16",/* Member name */
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
                        (char *)"i32",/* Member name */
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
                        (char *)"i64",/* Member name */
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
                        (char *)"u8",/* Member name */
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
                        (char *)"u16",/* Member name */
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
                        (char *)"u32",/* Member name */
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
                    }, 
                    {
                        (char *)"u64",/* Member name */
                        {
                            7,/* Representation ID */
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
                        (char *)"CorpusFullWidths::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        8, /* Number of members */
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

                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_int8;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_short;
                Sample_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_longlong;
                Sample_g_tc_members[4]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_uint8;
                Sample_g_tc_members[5]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_ushort;
                Sample_g_tc_members[6]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_ulong;
                Sample_g_tc_members[7]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_ulonglong;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_INT8;
                Sample_g_tc_members[0]._annotations._defaultValue._u.int8_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_INT8;
                Sample_g_tc_members[0]._annotations._minValue._u.int8_value = RTIXCdrInt8_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_INT8;
                Sample_g_tc_members[0]._annotations._maxValue._u.int8_value = RTIXCdrInt8_MAX;
                Sample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[1]._annotations._defaultValue._u.short_value = 0;
                Sample_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[1]._annotations._minValue._u.short_value = RTIXCdrShort_MIN;
                Sample_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[1]._annotations._maxValue._u.short_value = RTIXCdrShort_MAX;
                Sample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[2]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[2]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[2]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[2]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[2]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Sample_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[3]._annotations._defaultValue._u.long_long_value = 0ll;
                Sample_g_tc_members[3]._annotations._minValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[3]._annotations._minValue._u.long_long_value = RTIXCdrLongLong_MIN;
                Sample_g_tc_members[3]._annotations._maxValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[3]._annotations._maxValue._u.long_long_value = RTIXCdrLongLong_MAX;
                Sample_g_tc_members[4]._annotations._defaultValue._d = RTI_XCDR_TK_UINT8;
                Sample_g_tc_members[4]._annotations._defaultValue._u.uint8_value = 0;
                Sample_g_tc_members[4]._annotations._minValue._d = RTI_XCDR_TK_UINT8;
                Sample_g_tc_members[4]._annotations._minValue._u.uint8_value = RTIXCdrUInt8_MIN;
                Sample_g_tc_members[4]._annotations._maxValue._d = RTI_XCDR_TK_UINT8;
                Sample_g_tc_members[4]._annotations._maxValue._u.uint8_value = RTIXCdrUInt8_MAX;
                Sample_g_tc_members[5]._annotations._defaultValue._d = RTI_XCDR_TK_USHORT;
                Sample_g_tc_members[5]._annotations._defaultValue._u.ushort_value = 0;
                Sample_g_tc_members[5]._annotations._minValue._d = RTI_XCDR_TK_USHORT;
                Sample_g_tc_members[5]._annotations._minValue._u.ushort_value = RTIXCdrUnsignedShort_MIN;
                Sample_g_tc_members[5]._annotations._maxValue._d = RTI_XCDR_TK_USHORT;
                Sample_g_tc_members[5]._annotations._maxValue._u.ushort_value = RTIXCdrUnsignedShort_MAX;
                Sample_g_tc_members[6]._annotations._defaultValue._d = RTI_XCDR_TK_ULONG;
                Sample_g_tc_members[6]._annotations._defaultValue._u.ulong_value = 0u;
                Sample_g_tc_members[6]._annotations._minValue._d = RTI_XCDR_TK_ULONG;
                Sample_g_tc_members[6]._annotations._minValue._u.ulong_value = RTIXCdrUnsignedLong_MIN;
                Sample_g_tc_members[6]._annotations._maxValue._d = RTI_XCDR_TK_ULONG;
                Sample_g_tc_members[6]._annotations._maxValue._u.ulong_value = RTIXCdrUnsignedLong_MAX;
                Sample_g_tc_members[7]._annotations._defaultValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[7]._annotations._defaultValue._u.ulong_long_value = 0ull;
                Sample_g_tc_members[7]._annotations._minValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[7]._annotations._minValue._u.ulong_long_value = RTIXCdrUnsignedLongLong_MIN;
                Sample_g_tc_members[7]._annotations._maxValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[7]._annotations._maxValue._u.ulong_long_value = RTIXCdrUnsignedLongLong_MAX;

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusFullWidths::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[8] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusFullWidths::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->i8 - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->i16 - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->i32 - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->i64 - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->u8 - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->u16 - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->u32 - (char *)sample);

                Sample_g_memberAccessInfos[7].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->u64 - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusFullWidths::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusFullWidths::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusFullWidths::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusFullWidths::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusFullWidths::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusFullWidths::SamplePlugin_new,
                ::CorpusFullWidths::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusFullWidths::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusFullWidths::Sample& sample,
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

        void topic_type_support< ::CorpusFullWidths::Sample >::from_cdr_buffer(::CorpusFullWidths::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusFullWidths::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusFullWidths::Sample >::reset_sample(::CorpusFullWidths::Sample& sample) 
        {
            sample.i8 = 0;
            sample.i16 = 0;
            sample.i32 = 0;
            sample.i64 = 0ll;
            sample.u8 = 0;
            sample.u16 = 0;
            sample.u32 = 0u;
            sample.u64 = 0ull;
        }

        void topic_type_support< ::CorpusFullWidths::Sample >::allocate_sample(::CorpusFullWidths::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
