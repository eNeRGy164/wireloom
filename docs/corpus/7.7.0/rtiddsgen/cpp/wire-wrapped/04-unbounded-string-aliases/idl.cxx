

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

namespace CorpusUnboundedStrings {

    // ---- StringAliasesSample:

    StringAliasesSample::StringAliasesSample() :
        narrow ("")  {

            wide = L"";
    }

    StringAliasesSample::StringAliasesSample (const ::omg::types::string_view& narrow_,const ::omg::types::wstring_view& wide_) {
        narrow = narrow_;
        wide = wide_;
    }

    bool operator == (const StringAliasesSample& a, const StringAliasesSample& b) {

        if (a.narrow != b.narrow) {
            return false;
        }
        if (a.wide != b.wide) {
            return false;
        }

        return true;
    }

    bool operator != (const StringAliasesSample& a, const StringAliasesSample& b) {
        return !operator ==(a, b);
    }

    void swap(StringAliasesSample& a, StringAliasesSample& b) noexcept
    {
        using std::swap;

        swap(a.narrow, b.narrow);
        swap(a.wide, b.wide);

    }  
    std::ostream& operator << (std::ostream& o,const StringAliasesSample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "narrow: " << sample.narrow<<", ";
        o << "wide: ";
        rti::util::detail::to_ostream<std::wstring>(o, sample.wide);
        o <<"]";
        return o;
    }

} // namespace CorpusUnboundedStrings  

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
        struct native_type_code< ::CorpusUnboundedStrings::StringAliasesSample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member StringAliasesSample_g_tc_members[2]=
                {

                    {
                        (char *)"narrow",/* Member name */
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
                        (char *)"wide",/* Member name */
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

                static DDS_TypeCode StringAliasesSample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnboundedStrings::StringAliasesSample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        StringAliasesSample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for StringAliasesSample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &StringAliasesSample_g_tc;
                }

                StringAliasesSample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                StringAliasesSample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedStrings::Text_AliasTag_t>::get().native();
                StringAliasesSample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedStrings::WideText_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                StringAliasesSample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringAliasesSample_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringAliasesSample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringAliasesSample_g_tc_members[1]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                StringAliasesSample_g_tc._data._sampleAccessInfo = sample_access_info();
                StringAliasesSample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &StringAliasesSample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnboundedStrings::StringAliasesSample *sample;

                static RTIXCdrMemberAccessInfo StringAliasesSample_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo StringAliasesSample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &StringAliasesSample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusUnboundedStrings::StringAliasesSample);
                if (sample == NULL) {
                    return NULL;
                }

                StringAliasesSample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->narrow - (char *)sample);

                StringAliasesSample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wide - (char *)sample);

                StringAliasesSample_g_sampleAccessInfo.memberAccessInfos =
                StringAliasesSample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedStrings::StringAliasesSample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        StringAliasesSample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        StringAliasesSample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                StringAliasesSample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                StringAliasesSample_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedStrings::StringAliasesSample >;

                StringAliasesSample_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &StringAliasesSample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin StringAliasesSample_g_typePlugin =
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

                return &StringAliasesSample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnboundedStrings::StringAliasesSample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedStrings::StringAliasesSample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnboundedStrings::StringAliasesSample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnboundedStrings::StringAliasesSamplePlugin_new,
                ::CorpusUnboundedStrings::StringAliasesSamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnboundedStrings::StringAliasesSample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnboundedStrings::StringAliasesSample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = StringAliasesSamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = StringAliasesSamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusUnboundedStrings::StringAliasesSample >::from_cdr_buffer(::CorpusUnboundedStrings::StringAliasesSample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = StringAliasesSamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnboundedStrings::StringAliasesSample from cdr buffer");
        }

        void topic_type_support< ::CorpusUnboundedStrings::StringAliasesSample >::reset_sample(::CorpusUnboundedStrings::StringAliasesSample& sample) 
        {
            sample.narrow = "";
            sample.wide = L"";
        }

        void topic_type_support< ::CorpusUnboundedStrings::StringAliasesSample >::allocate_sample(::CorpusUnboundedStrings::StringAliasesSample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.narrow,  -1, -1);
            ::rti::topic::allocate_sample(sample.wide,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
