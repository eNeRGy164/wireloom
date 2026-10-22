

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-extensibility-full.idl
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
#include "14-extensibility-fullPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-extensibility-full.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- FullExtensibility: 

FullExtensibility::FullExtensibility() :
    m_value_ (0)  {

}   

FullExtensibility::FullExtensibility (int32_t value_):
    m_value_(value_) {
}

void FullExtensibility::swap(FullExtensibility& other_)  noexcept 
{
    using std::swap;
    swap(m_value_, other_.m_value_);
}  

bool FullExtensibility::operator == (const FullExtensibility& other_) const {
    if (m_value_ != other_.m_value_) {
        return false;
    }
    return true;
}

bool FullExtensibility::operator != (const FullExtensibility& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const FullExtensibility& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "value: " << sample.value ();
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
        struct native_type_code< ::FullExtensibility > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member FullExtensibility_g_tc_members[1]=
                {

                    {
                        (char *)"value",/* Member name */
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

                static DDS_TypeCode FullExtensibility_g_tc =
                {{
                        DDS_TK_STRUCT| DDS_TK_MUTABLE_EXTENSIBILITY, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"FullExtensibility", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        FullExtensibility_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for FullExtensibility*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &FullExtensibility_g_tc;
                }

                FullExtensibility_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                FullExtensibility_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                FullExtensibility_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                FullExtensibility_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                FullExtensibility_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                FullExtensibility_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                FullExtensibility_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                FullExtensibility_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                FullExtensibility_g_tc._data._sampleAccessInfo = sample_access_info();
                FullExtensibility_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &FullExtensibility_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::FullExtensibility *sample;

                static RTIXCdrMemberAccessInfo FullExtensibility_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo FullExtensibility_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &FullExtensibility_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::FullExtensibility);
                if (sample == NULL) {
                    return NULL;
                }

                FullExtensibility_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                FullExtensibility_g_sampleAccessInfo.memberAccessInfos = 
                FullExtensibility_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::FullExtensibility);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        FullExtensibility_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        FullExtensibility_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                FullExtensibility_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                FullExtensibility_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::FullExtensibility >;

                FullExtensibility_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &FullExtensibility_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin FullExtensibility_g_typePlugin = 
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

                return &FullExtensibility_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::FullExtensibility >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::FullExtensibility >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::FullExtensibility >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::FullExtensibilityPlugin_new,
                ::FullExtensibilityPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::FullExtensibility >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::FullExtensibility& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = FullExtensibilityPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = FullExtensibilityPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::FullExtensibility >::from_cdr_buffer(::FullExtensibility& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = FullExtensibilityPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::FullExtensibility from cdr buffer");
        }

        void topic_type_support< ::FullExtensibility >::reset_sample(::FullExtensibility& sample) 
        {
            sample.value(0);
        }

        void topic_type_support< ::FullExtensibility >::allocate_sample(::FullExtensibility& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
