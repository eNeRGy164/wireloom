

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-modules.idl
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
#include "13-modulesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "13-modules.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusIntegration {

    namespace Contracts {

        // ---- NamespacedMessage: 

        NamespacedMessage::NamespacedMessage() :
            m_enabled_ (0)  {

        }   

        NamespacedMessage::NamespacedMessage (bool enabled_):
            m_enabled_(enabled_) {
        }

        void NamespacedMessage::swap(NamespacedMessage& other_)  noexcept 
        {
            using std::swap;
            swap(m_enabled_, other_.m_enabled_);
        }  

        bool NamespacedMessage::operator == (const NamespacedMessage& other_) const {
            if (m_enabled_ != other_.m_enabled_) {
                return false;
            }
            return true;
        }

        bool NamespacedMessage::operator != (const NamespacedMessage& other_) const {
            return !this->operator ==(other_);
        }

        std::ostream& operator << (std::ostream& o,const NamespacedMessage& sample)
        {
            ::rti::util::StreamFlagSaver flag_saver (o);
            o <<"[";
            o << "enabled: " << sample.enabled ();
            o <<"]";
            return o;
        }

    } // namespace Contracts  

} // namespace CorpusIntegration  

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
        struct native_type_code< ::CorpusIntegration::Contracts::NamespacedMessage > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member NamespacedMessage_g_tc_members[1]=
                {

                    {
                        (char *)"enabled",/* Member name */
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

                static DDS_TypeCode NamespacedMessage_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegration::Contracts::NamespacedMessage", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        NamespacedMessage_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for NamespacedMessage*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &NamespacedMessage_g_tc;
                }

                NamespacedMessage_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NamespacedMessage_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_bool_typecode();

                /* Initialize the values for member annotations. */
                NamespacedMessage_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_BOOLEAN;
                NamespacedMessage_g_tc_members[0]._annotations._defaultValue._u.boolean_value = 0;

                NamespacedMessage_g_tc._data._sampleAccessInfo = sample_access_info();
                NamespacedMessage_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &NamespacedMessage_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegration::Contracts::NamespacedMessage *sample;

                static RTIXCdrMemberAccessInfo NamespacedMessage_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NamespacedMessage_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &NamespacedMessage_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusIntegration::Contracts::NamespacedMessage);
                if (sample == NULL) {
                    return NULL;
                }

                NamespacedMessage_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->enabled() - (char *)sample);

                NamespacedMessage_g_sampleAccessInfo.memberAccessInfos = 
                NamespacedMessage_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegration::Contracts::NamespacedMessage);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NamespacedMessage_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NamespacedMessage_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NamespacedMessage_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NamespacedMessage_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusIntegration::Contracts::NamespacedMessage >;

                NamespacedMessage_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &NamespacedMessage_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NamespacedMessage_g_typePlugin = 
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

                return &NamespacedMessage_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegration::Contracts::NamespacedMessage >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegration::Contracts::NamespacedMessage >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusIntegration::Contracts::NamespacedMessage >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusIntegration::Contracts::NamespacedMessagePlugin_new,
                ::CorpusIntegration::Contracts::NamespacedMessagePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusIntegration::Contracts::NamespacedMessage >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusIntegration::Contracts::NamespacedMessage& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = NamespacedMessagePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = NamespacedMessagePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusIntegration::Contracts::NamespacedMessage >::from_cdr_buffer(::CorpusIntegration::Contracts::NamespacedMessage& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = NamespacedMessagePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusIntegration::Contracts::NamespacedMessage from cdr buffer");
        }

        void topic_type_support< ::CorpusIntegration::Contracts::NamespacedMessage >::reset_sample(::CorpusIntegration::Contracts::NamespacedMessage& sample) 
        {
            sample.enabled(0);
        }

        void topic_type_support< ::CorpusIntegration::Contracts::NamespacedMessage >::allocate_sample(::CorpusIntegration::Contracts::NamespacedMessage& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
