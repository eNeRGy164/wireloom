

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-id-gaps.idl
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
#include "09-id-gapsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-id-gaps.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusIdGaps {

    // ---- Message: 

    Message::Message() :
        m_first_ (0) ,
        m_seventh_ (0)  {

    }   

    Message::Message (int32_t first_,int32_t seventh_):
        m_first_(first_), 
        m_seventh_(seventh_) {
    }

    void Message::swap(Message& other_)  noexcept 
    {
        using std::swap;
        swap(m_first_, other_.m_first_);
        swap(m_seventh_, other_.m_seventh_);
    }  

    bool Message::operator == (const Message& other_) const {
        if (m_first_ != other_.m_first_) {
            return false;
        }
        if (m_seventh_ != other_.m_seventh_) {
            return false;
        }
        return true;
    }

    bool Message::operator != (const Message& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Message& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "first: " << sample.first ()<<", ";
        o << "seventh: " << sample.seventh ();
        o <<"]";
        return o;
    }

} // namespace CorpusIdGaps  

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
        struct native_type_code< ::CorpusIdGaps::Message > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Message_g_tc_members[2]=
                {

                    {
                        (char *)"first",/* Member name */
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
                        (char *)"seventh",/* Member name */
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

                static DDS_TypeCode Message_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIdGaps::Message", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Message_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Message*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Message_g_tc;
                }

                Message_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Message_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Message_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Message_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Message_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Message_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Message_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Message_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Message_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Message_g_tc._data._sampleAccessInfo = sample_access_info();
                Message_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Message_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIdGaps::Message *sample;

                static RTIXCdrMemberAccessInfo Message_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Message_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Message_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusIdGaps::Message);
                if (sample == NULL) {
                    return NULL;
                }

                Message_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->first() - (char *)sample);

                Message_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->seventh() - (char *)sample);

                Message_g_sampleAccessInfo.memberAccessInfos = 
                Message_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIdGaps::Message);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Message_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Message_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Message_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Message_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusIdGaps::Message >;

                Message_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Message_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Message_g_typePlugin = 
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

                return &Message_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIdGaps::Message >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIdGaps::Message >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusIdGaps::Message >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusIdGaps::MessagePlugin_new,
                ::CorpusIdGaps::MessagePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusIdGaps::Message >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusIdGaps::Message& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = MessagePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = MessagePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusIdGaps::Message >::from_cdr_buffer(::CorpusIdGaps::Message& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = MessagePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusIdGaps::Message from cdr buffer");
        }

        void topic_type_support< ::CorpusIdGaps::Message >::reset_sample(::CorpusIdGaps::Message& sample) 
        {
            sample.first(0);
            sample.seventh(0);
        }

        void topic_type_support< ::CorpusIdGaps::Message >::allocate_sample(::CorpusIdGaps::Message& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
