

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-evolution-optional.idl
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
#include "09-evolution-optionalPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-evolution-optional.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusOptionalEvolution {

    // ---- Message: 

    Message::Message() :
        m_requiredValue_ (0)  {

    }   

    Message::Message (int32_t requiredValue_,const ::dds::core::optional< int32_t >& optionalValue_,const ::dds::core::optional< std::string >& optionalText_):
        m_requiredValue_(requiredValue_), 
        m_optionalValue_(optionalValue_), 
        m_optionalText_(optionalText_) {
    }

    void Message::swap(Message& other_)  noexcept 
    {
        using std::swap;
        swap(m_requiredValue_, other_.m_requiredValue_);
        swap(m_optionalValue_, other_.m_optionalValue_);
        swap(m_optionalText_, other_.m_optionalText_);
    }  

    bool Message::operator == (const Message& other_) const {
        if (m_requiredValue_ != other_.m_requiredValue_) {
            return false;
        }
        if (m_optionalValue_ != other_.m_optionalValue_) {
            return false;
        }
        if (m_optionalText_ != other_.m_optionalText_) {
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
        o << "requiredValue: " << sample.requiredValue ()<<", ";
        o << "optionalValue: " << sample.optionalValue ()<<", ";
        o << "optionalText: " << sample.optionalText ();
        o <<"]";
        return o;
    }

} // namespace CorpusOptionalEvolution  

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
        struct native_type_code< ::CorpusOptionalEvolution::Message > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Message_g_tc_optionalText_string;

                static DDS_TypeCode_Member Message_g_tc_members[3]=
                {

                    {
                        (char *)"requiredValue",/* Member name */
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
                        (char *)"optionalValue",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"optionalText",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
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
                        (char *)"CorpusOptionalEvolution::Message", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
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

                Message_g_tc_optionalText_string = initialize_string_typecode((16L));

                Message_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Message_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Message_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Message_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&Message_g_tc_optionalText_string;

                /* Initialize the values for member annotations. */
                Message_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Message_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Message_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Message_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
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

                ::CorpusOptionalEvolution::Message *sample;

                static RTIXCdrMemberAccessInfo Message_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Message_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Message_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusOptionalEvolution::Message);
                if (sample == NULL) {
                    return NULL;
                }

                Message_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->requiredValue() - (char *)sample);

                Message_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->optionalValue() - (char *)sample);

                Message_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->optionalText() - (char *)sample);

                Message_g_sampleAccessInfo.memberAccessInfos = 
                Message_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalEvolution::Message);

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
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalEvolution::Message >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusOptionalEvolution::Message >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalEvolution::Message >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusOptionalEvolution::Message >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusOptionalEvolution::MessagePlugin_new,
                ::CorpusOptionalEvolution::MessagePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusOptionalEvolution::Message >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusOptionalEvolution::Message& sample,
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

        void topic_type_support< ::CorpusOptionalEvolution::Message >::from_cdr_buffer(::CorpusOptionalEvolution::Message& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = MessagePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusOptionalEvolution::Message from cdr buffer");
        }

        void topic_type_support< ::CorpusOptionalEvolution::Message >::reset_sample(::CorpusOptionalEvolution::Message& sample) 
        {
            sample.requiredValue(0);
            ::rti::topic::reset_sample(sample.optionalValue());
            ::rti::topic::reset_sample(sample.optionalText());
        }

        void topic_type_support< ::CorpusOptionalEvolution::Message >::allocate_sample(::CorpusOptionalEvolution::Message& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
