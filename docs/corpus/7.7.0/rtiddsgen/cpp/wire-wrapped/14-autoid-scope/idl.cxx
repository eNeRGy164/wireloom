

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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
#include "idlPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "idl.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- AutoIdScopeSample:

AutoIdScopeSample::AutoIdScopeSample()  {

}

AutoIdScopeSample::AutoIdScopeSample (const ::AutoIds::Hashed& hashed_,const ::AutoIds::Reopened& reopened_,const ::Sequential& sequential_,const ::UsesInclude& usesInclude_) {
    hashed = hashed_;
    reopened = reopened_;
    sequential = sequential_;
    usesInclude = usesInclude_;
}

bool operator == (const AutoIdScopeSample& a, const AutoIdScopeSample& b) {

    if (a.hashed != b.hashed) {
        return false;
    }
    if (a.reopened != b.reopened) {
        return false;
    }
    if (a.sequential != b.sequential) {
        return false;
    }
    if (a.usesInclude != b.usesInclude) {
        return false;
    }

    return true;
}

bool operator != (const AutoIdScopeSample& a, const AutoIdScopeSample& b) {
    return !operator ==(a, b);
}

void swap(AutoIdScopeSample& a, AutoIdScopeSample& b) noexcept
{
    using std::swap;

    swap(a.hashed, b.hashed);
    swap(a.reopened, b.reopened);
    swap(a.sequential, b.sequential);
    swap(a.usesInclude, b.usesInclude);

}  
std::ostream& operator << (std::ostream& o,const AutoIdScopeSample& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "hashed: " << sample.hashed<<", ";
    o << "reopened: " << sample.reopened<<", ";
    o << "sequential: " << sample.sequential<<", ";
    o << "usesInclude: " << sample.usesInclude;
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
        struct native_type_code< ::AutoIdScopeSample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member AutoIdScopeSample_g_tc_members[4]=
                {

                    {
                        (char *)"hashed",/* Member name */
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
                        (char *)"reopened",/* Member name */
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
                    }, 
                    {
                        (char *)"sequential",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"usesInclude",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode AutoIdScopeSample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"AutoIdScopeSample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        AutoIdScopeSample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for AutoIdScopeSample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &AutoIdScopeSample_g_tc;
                }

                AutoIdScopeSample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                AutoIdScopeSample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::AutoIds::Hashed>::get().native();
                AutoIdScopeSample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::AutoIds::Reopened>::get().native();
                AutoIdScopeSample_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Sequential>::get().native();
                AutoIdScopeSample_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::UsesInclude>::get().native();

                /* Initialize the values for member annotations. */

                AutoIdScopeSample_g_tc._data._sampleAccessInfo = sample_access_info();
                AutoIdScopeSample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &AutoIdScopeSample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::AutoIdScopeSample *sample;

                static RTIXCdrMemberAccessInfo AutoIdScopeSample_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo AutoIdScopeSample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &AutoIdScopeSample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::AutoIdScopeSample);
                if (sample == NULL) {
                    return NULL;
                }

                AutoIdScopeSample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->hashed - (char *)sample);

                AutoIdScopeSample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->reopened - (char *)sample);

                AutoIdScopeSample_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->sequential - (char *)sample);

                AutoIdScopeSample_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->usesInclude - (char *)sample);

                AutoIdScopeSample_g_sampleAccessInfo.memberAccessInfos =
                AutoIdScopeSample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::AutoIdScopeSample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        AutoIdScopeSample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        AutoIdScopeSample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                AutoIdScopeSample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                AutoIdScopeSample_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::AutoIdScopeSample >;

                AutoIdScopeSample_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &AutoIdScopeSample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin AutoIdScopeSample_g_typePlugin =
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

                return &AutoIdScopeSample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::AutoIdScopeSample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::AutoIdScopeSample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::AutoIdScopeSample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::AutoIdScopeSamplePlugin_new,
                ::AutoIdScopeSamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::AutoIdScopeSample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::AutoIdScopeSample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = AutoIdScopeSamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = AutoIdScopeSamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::AutoIdScopeSample >::from_cdr_buffer(::AutoIdScopeSample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = AutoIdScopeSamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::AutoIdScopeSample from cdr buffer");
        }

        void topic_type_support< ::AutoIdScopeSample >::reset_sample(::AutoIdScopeSample& sample) 
        {
            ::rti::topic::reset_sample(sample.hashed);
            ::rti::topic::reset_sample(sample.reopened);
            ::rti::topic::reset_sample(sample.sequential);
            ::rti::topic::reset_sample(sample.usesInclude);
        }

        void topic_type_support< ::AutoIdScopeSample >::allocate_sample(::AutoIdScopeSample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.hashed,  -1, -1);
            ::rti::topic::allocate_sample(sample.reopened,  -1, -1);
            ::rti::topic::allocate_sample(sample.sequential,  -1, -1);
            ::rti::topic::allocate_sample(sample.usesInclude,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
