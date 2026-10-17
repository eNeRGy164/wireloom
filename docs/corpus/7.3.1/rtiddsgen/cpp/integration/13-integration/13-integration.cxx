

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-integration.idl
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
#include "13-integrationPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "13-integration.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- CorpusIntegrationMessage: 

CorpusIntegrationMessage::CorpusIntegrationMessage() :
    m_text_ ("")  {

}   

CorpusIntegrationMessage::CorpusIntegrationMessage (const std::string& text_):
    m_text_(text_) {
}

void CorpusIntegrationMessage::swap(CorpusIntegrationMessage& other_)  noexcept 
{
    using std::swap;
    swap(m_text_, other_.m_text_);
}  

bool CorpusIntegrationMessage::operator == (const CorpusIntegrationMessage& other_) const {
    if (m_text_ != other_.m_text_) {
        return false;
    }
    return true;
}

bool CorpusIntegrationMessage::operator != (const CorpusIntegrationMessage& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const CorpusIntegrationMessage& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "text: " << sample.text ();
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
        struct native_type_code< ::CorpusIntegrationMessage > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode CorpusIntegrationMessage_g_tc_text_string;

                static DDS_TypeCode_Member CorpusIntegrationMessage_g_tc_members[1]=
                {

                    {
                        (char *)"text",/* Member name */
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
                    }
                };

                static DDS_TypeCode CorpusIntegrationMessage_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationMessage", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        CorpusIntegrationMessage_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for CorpusIntegrationMessage*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &CorpusIntegrationMessage_g_tc;
                }

                CorpusIntegrationMessage_g_tc_text_string = initialize_string_typecode((255L));

                CorpusIntegrationMessage_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                CorpusIntegrationMessage_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&CorpusIntegrationMessage_g_tc_text_string;

                /* Initialize the values for member annotations. */
                CorpusIntegrationMessage_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                CorpusIntegrationMessage_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                CorpusIntegrationMessage_g_tc._data._sampleAccessInfo = sample_access_info();
                CorpusIntegrationMessage_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &CorpusIntegrationMessage_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegrationMessage *sample;

                static RTIXCdrMemberAccessInfo CorpusIntegrationMessage_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo CorpusIntegrationMessage_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &CorpusIntegrationMessage_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusIntegrationMessage);
                if (sample == NULL) {
                    return NULL;
                }

                CorpusIntegrationMessage_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->text() - (char *)sample);

                CorpusIntegrationMessage_g_sampleAccessInfo.memberAccessInfos = 
                CorpusIntegrationMessage_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationMessage);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        CorpusIntegrationMessage_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        CorpusIntegrationMessage_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                CorpusIntegrationMessage_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                CorpusIntegrationMessage_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationMessage >;

                CorpusIntegrationMessage_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &CorpusIntegrationMessage_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin CorpusIntegrationMessage_g_typePlugin = 
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

                return &CorpusIntegrationMessage_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegrationMessage >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationMessage >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusIntegrationMessage >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusIntegrationMessagePlugin_new,
                ::CorpusIntegrationMessagePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusIntegrationMessage >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusIntegrationMessage& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = CorpusIntegrationMessagePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = CorpusIntegrationMessagePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusIntegrationMessage >::from_cdr_buffer(::CorpusIntegrationMessage& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = CorpusIntegrationMessagePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusIntegrationMessage from cdr buffer");
        }

        void topic_type_support< ::CorpusIntegrationMessage >::reset_sample(::CorpusIntegrationMessage& sample) 
        {
            sample.text("");
        }

        void topic_type_support< ::CorpusIntegrationMessage >::allocate_sample(::CorpusIntegrationMessage& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text(),  -1, 255L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
