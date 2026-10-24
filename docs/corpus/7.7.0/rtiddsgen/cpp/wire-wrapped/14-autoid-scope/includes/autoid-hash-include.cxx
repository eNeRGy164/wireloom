

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from autoid-hash-include.idl
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
#include "autoid-hash-includePlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "autoid-hash-include.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- Included:

Included::Included() :
    first (0) ,
    second (0)  {

}

Included::Included (int32_t first_,int32_t second_) {
    first = first_;
    second = second_;
}

bool operator == (const Included& a, const Included& b) {

    if (a.first != b.first) {
        return false;
    }
    if (a.second != b.second) {
        return false;
    }

    return true;
}

bool operator != (const Included& a, const Included& b) {
    return !operator ==(a, b);
}

void swap(Included& a, Included& b) noexcept
{
    using std::swap;

    swap(a.first, b.first);
    swap(a.second, b.second);

}  
std::ostream& operator << (std::ostream& o,const Included& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "first: " << sample.first<<", ";
    o << "second: " << sample.second;
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
        struct native_type_code< ::Included > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Included_g_tc_members[2]=
                {

                    {
                        (char *)"first",/* Member name */
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
                        (char *)"second",/* Member name */
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
                    }
                };

                static DDS_TypeCode Included_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"Included", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Included_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Included*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Included_g_tc;
                }

                Included_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Included_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Included_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Included_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Included_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Included_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Included_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Included_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Included_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Included_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Included_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Included_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Included_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Included_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Included_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Included_g_tc._data._sampleAccessInfo = sample_access_info();
                Included_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Included_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::Included *sample;

                static RTIXCdrMemberAccessInfo Included_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Included_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Included_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::Included);
                if (sample == NULL) {
                    return NULL;
                }

                Included_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->first - (char *)sample);

                Included_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->second - (char *)sample);

                Included_g_sampleAccessInfo.memberAccessInfos =
                Included_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::Included);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Included_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Included_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Included_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Included_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::Included >;

                Included_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Included_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Included_g_typePlugin =
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

                return &Included_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::Included >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Included >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::Included >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::IncludedPlugin_new,
                ::IncludedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::Included >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::Included& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = IncludedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = IncludedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::Included >::from_cdr_buffer(::Included& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = IncludedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::Included from cdr buffer");
        }

        void topic_type_support< ::Included >::reset_sample(::Included& sample) 
        {
            sample.first = 0;
            sample.second = 0;
        }

        void topic_type_support< ::Included >::allocate_sample(::Included& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
