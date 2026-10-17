

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-zero-bound.idl
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
#include "12-zero-boundPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "12-zero-bound.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- ZeroBound:

ZeroBound::ZeroBound() :
    value ("")  {

}

ZeroBound::ZeroBound (const ::omg::types::string_view& value_) {
    value = value_;
}

bool operator == (const ZeroBound& a, const ZeroBound& b) {

    if (a.value != b.value) {
        return false;
    }

    return true;
}

bool operator != (const ZeroBound& a, const ZeroBound& b) {
    return !operator ==(a, b);
}

void swap(ZeroBound& a, ZeroBound& b) noexcept
{
    using std::swap;

    swap(a.value, b.value);

}  
std::ostream& operator << (std::ostream& o,const ZeroBound& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "value: " << sample.value;
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
        struct native_type_code< ::ZeroBound > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ZeroBound_g_tc_value_string;

                static DDS_TypeCode_Member ZeroBound_g_tc_members[1]=
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode ZeroBound_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"ZeroBound", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        ZeroBound_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ZeroBound*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ZeroBound_g_tc;
                }

                ZeroBound_g_tc_value_string = initialize_string_typecode((0L));

                ZeroBound_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ZeroBound_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&ZeroBound_g_tc_value_string;

                /* Initialize the values for member annotations. */
                ZeroBound_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                ZeroBound_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                ZeroBound_g_tc._data._sampleAccessInfo = sample_access_info();
                ZeroBound_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &ZeroBound_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::ZeroBound *sample;

                static RTIXCdrMemberAccessInfo ZeroBound_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ZeroBound_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ZeroBound_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::ZeroBound);
                if (sample == NULL) {
                    return NULL;
                }

                ZeroBound_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                ZeroBound_g_sampleAccessInfo.memberAccessInfos =
                ZeroBound_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::ZeroBound);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ZeroBound_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ZeroBound_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ZeroBound_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ZeroBound_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::ZeroBound >;

                ZeroBound_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ZeroBound_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ZeroBound_g_typePlugin =
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

                return &ZeroBound_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::ZeroBound >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::ZeroBound >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::ZeroBound >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::ZeroBoundPlugin_new,
                ::ZeroBoundPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::ZeroBound >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::ZeroBound& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ZeroBoundPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ZeroBoundPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::ZeroBound >::from_cdr_buffer(::ZeroBound& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ZeroBoundPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::ZeroBound from cdr buffer");
        }

        void topic_type_support< ::ZeroBound >::reset_sample(::ZeroBound& sample) 
        {
            sample.value = "";
        }

        void topic_type_support< ::ZeroBound >::allocate_sample(::ZeroBound& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.value,  -1, 0L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
