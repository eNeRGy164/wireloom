

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from common.idl
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
#include "commonPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "common.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- IncludedType:

IncludedType::IncludedType() :
    m_value_ (0)  {

}

IncludedType::IncludedType (int32_t value_):
    m_value_(value_) {
}

void IncludedType::swap(IncludedType& other_)  noexcept
{
    using std::swap;
    swap(m_value_, other_.m_value_);
}

bool IncludedType::operator == (const IncludedType& other_) const {
    if (m_value_ != other_.m_value_) {
        return false;
    }
    return true;
}

bool IncludedType::operator != (const IncludedType& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const IncludedType& sample)
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
        struct native_type_code< ::IncludedType > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member IncludedType_g_tc_members[1]=
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

                static DDS_TypeCode IncludedType_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"IncludedType", /* Name */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        IncludedType_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for IncludedType*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &IncludedType_g_tc;
                }

                IncludedType_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                IncludedType_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                IncludedType_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                IncludedType_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                IncludedType_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                IncludedType_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                IncludedType_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                IncludedType_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                IncludedType_g_tc._data._sampleAccessInfo = sample_access_info();
                IncludedType_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &IncludedType_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::IncludedType *sample;

                static RTIXCdrMemberAccessInfo IncludedType_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo IncludedType_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &IncludedType_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::IncludedType);
                if (sample == NULL) {
                    return NULL;
                }

                IncludedType_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                IncludedType_g_sampleAccessInfo.memberAccessInfos =
                IncludedType_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::IncludedType);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        IncludedType_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        IncludedType_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                IncludedType_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                IncludedType_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::IncludedType >;

                IncludedType_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &IncludedType_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin IncludedType_g_typePlugin =
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

                return &IncludedType_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::IncludedType >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::IncludedType >::get())));
        }
    }
}

namespace dds {
    namespace topic {
        void topic_type_support< ::IncludedType >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name)
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::IncludedTypePlugin_new,
                ::IncludedTypePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::IncludedType >::to_cdr_buffer(
            std::vector<char>& buffer,
            const ::IncludedType& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = IncludedTypePlugin_serialize_to_cdr_buffer(
                NULL,
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = IncludedTypePlugin_serialize_to_cdr_buffer(
                &buffer[0],
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::IncludedType >::from_cdr_buffer(::IncludedType& sample,
        const std::vector<char>& buffer)
        {

            RTIBool ok  = IncludedTypePlugin_deserialize_from_cdr_buffer(
                &sample,
                &buffer[0],
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::IncludedType from cdr buffer");
        }

        void topic_type_support< ::IncludedType >::reset_sample(::IncludedType& sample)
        {
            sample.value(0);
        }

        void topic_type_support< ::IncludedType >::allocate_sample(::IncludedType& sample, int, int)
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}

#endif // NDDS_STANDALONE_TYPE
