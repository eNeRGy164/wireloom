

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-full-widths.idl
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
#include "01-full-widthsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "01-full-widths.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusFullWidths {

    // ---- Sample: 

    Sample::Sample() :
        m_i8_ (0) ,
        m_i16_ (0) ,
        m_i32_ (0) ,
        m_i64_ (0ll) ,
        m_u8_ (0) ,
        m_u16_ (0) ,
        m_u32_ (0u) ,
        m_u64_ (0ull)  {

    }   

    Sample::Sample (int8_t i8_,int16_t i16_,int32_t i32_,int64_t i64_,uint8_t u8_,uint16_t u16_,uint32_t u32_,uint64_t u64_):
        m_i8_(i8_), 
        m_i16_(i16_), 
        m_i32_(i32_), 
        m_i64_(i64_), 
        m_u8_(u8_), 
        m_u16_(u16_), 
        m_u32_(u32_), 
        m_u64_(u64_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_i8_, other_.m_i8_);
        swap(m_i16_, other_.m_i16_);
        swap(m_i32_, other_.m_i32_);
        swap(m_i64_, other_.m_i64_);
        swap(m_u8_, other_.m_u8_);
        swap(m_u16_, other_.m_u16_);
        swap(m_u32_, other_.m_u32_);
        swap(m_u64_, other_.m_u64_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_i8_ != other_.m_i8_) {
            return false;
        }
        if (m_i16_ != other_.m_i16_) {
            return false;
        }
        if (m_i32_ != other_.m_i32_) {
            return false;
        }
        if (m_i64_ != other_.m_i64_) {
            return false;
        }
        if (m_u8_ != other_.m_u8_) {
            return false;
        }
        if (m_u16_ != other_.m_u16_) {
            return false;
        }
        if (m_u32_ != other_.m_u32_) {
            return false;
        }
        if (m_u64_ != other_.m_u64_) {
            return false;
        }
        return true;
    }

    bool Sample::operator != (const Sample& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "i8: " << (int) sample.i8 ()<<", ";
        o << "i16: " << sample.i16 ()<<", ";
        o << "i32: " << sample.i32 ()<<", ";
        o << "i64: " << sample.i64 ()<<", ";
        o << "u8: " << (int) sample.u8 ()<<", ";
        o << "u16: " << sample.u16 ()<<", ";
        o << "u32: " << sample.u32 ()<<", ";
        o << "u64: " << sample.u64 ();
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
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

                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;
                Sample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_short;
                Sample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_longlong;
                Sample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;
                Sample_g_tc_members[5]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ushort;
                Sample_g_tc_members[6]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ulong;
                Sample_g_tc_members[7]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ulonglong;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[0]._annotations._defaultValue._u.octet_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[0]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[0]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;
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
                Sample_g_tc_members[4]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[4]._annotations._defaultValue._u.octet_value = 0;
                Sample_g_tc_members[4]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[4]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Sample_g_tc_members[4]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[4]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;
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
                (RTIXCdrUnsignedLong) ((char *)&sample->i8() - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->i16() - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->i32() - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->i64() - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->u8() - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->u16() - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->u32() - (char *)sample);

                Sample_g_memberAccessInfos[7].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->u64() - (char *)sample);

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
            sample.i8(0);
            sample.i16(0);
            sample.i32(0);
            sample.i64(0ll);
            sample.u8(0);
            sample.u16(0);
            sample.u32(0u);
            sample.u64(0ull);
        }

        void topic_type_support< ::CorpusFullWidths::Sample >::allocate_sample(::CorpusFullWidths::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
