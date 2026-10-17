

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-default.idl
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
#include "09-defaultPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-default.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusDefaultExtensibility {

    // ---- Message: 

    Message::Message() :
        m_id_ (0) ,
        m_text_ ("")  {

    }   

    Message::Message (int32_t id_,const std::string& text_):
        m_id_(id_), 
        m_text_(text_) {
    }

    void Message::swap(Message& other_)  noexcept 
    {
        using std::swap;
        swap(m_id_, other_.m_id_);
        swap(m_text_, other_.m_text_);
    }  

    bool Message::operator == (const Message& other_) const {
        if (m_id_ != other_.m_id_) {
            return false;
        }
        if (m_text_ != other_.m_text_) {
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
        o << "id: " << sample.id ()<<", ";
        o << "text: " << sample.text ();
        o <<"]";
        return o;
    }

} // namespace CorpusDefaultExtensibility  

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
        struct native_type_code< ::CorpusDefaultExtensibility::Message > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Message_g_tc_text_string;

                static DDS_TypeCode_Member Message_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                        (char *)"text",/* Member name */
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
                    }
                };

                static DDS_TypeCode Message_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusDefaultExtensibility::Message", /* Name */
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

                Message_g_tc_text_string = initialize_string_typecode((16L));

                Message_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Message_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Message_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&Message_g_tc_text_string;

                /* Initialize the values for member annotations. */
                Message_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Message_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Message_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Message_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Message_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Message_g_tc._data._sampleAccessInfo = sample_access_info();
                Message_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Message_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusDefaultExtensibility::Message *sample;

                static RTIXCdrMemberAccessInfo Message_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Message_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Message_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusDefaultExtensibility::Message);
                if (sample == NULL) {
                    return NULL;
                }

                Message_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->id() - (char *)sample);

                Message_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->text() - (char *)sample);

                Message_g_sampleAccessInfo.memberAccessInfos = 
                Message_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusDefaultExtensibility::Message);

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
                interpreter::get_aggregation_value_pointer< ::CorpusDefaultExtensibility::Message >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusDefaultExtensibility::Message >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusDefaultExtensibility::Message >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusDefaultExtensibility::Message >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusDefaultExtensibility::MessagePlugin_new,
                ::CorpusDefaultExtensibility::MessagePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusDefaultExtensibility::Message >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusDefaultExtensibility::Message& sample,
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

        void topic_type_support< ::CorpusDefaultExtensibility::Message >::from_cdr_buffer(::CorpusDefaultExtensibility::Message& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = MessagePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusDefaultExtensibility::Message from cdr buffer");
        }

        void topic_type_support< ::CorpusDefaultExtensibility::Message >::reset_sample(::CorpusDefaultExtensibility::Message& sample) 
        {
            sample.id(0);
            sample.text("");
        }

        void topic_type_support< ::CorpusDefaultExtensibility::Message >::allocate_sample(::CorpusDefaultExtensibility::Message& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text(),  -1, 16L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
