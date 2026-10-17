

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-strings.idl
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
#include "04-stringsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "04-strings.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusStrings {

    // ---- Sample:

    Sample::Sample() :
        unbounded ("") ,
        bounded ("")  {

            wideUnbounded = L"";
            wideBounded = L"";
    }

    Sample::Sample (const ::omg::types::string_view& unbounded_,const ::omg::types::string_view& bounded_,const ::omg::types::wstring_view& wideUnbounded_,const ::omg::types::wstring_view& wideBounded_) {
        unbounded = unbounded_;
        bounded = bounded_;
        wideUnbounded = wideUnbounded_;
        wideBounded = wideBounded_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.unbounded != b.unbounded) {
            return false;
        }
        if (a.bounded != b.bounded) {
            return false;
        }
        if (a.wideUnbounded != b.wideUnbounded) {
            return false;
        }
        if (a.wideBounded != b.wideBounded) {
            return false;
        }

        return true;
    }

    bool operator != (const Sample& a, const Sample& b) {
        return !operator ==(a, b);
    }

    void swap(Sample& a, Sample& b) noexcept
    {
        using std::swap;

        swap(a.unbounded, b.unbounded);
        swap(a.bounded, b.bounded);
        swap(a.wideUnbounded, b.wideUnbounded);
        swap(a.wideBounded, b.wideBounded);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "unbounded: " << sample.unbounded<<", ";
        o << "bounded: " << sample.bounded<<", ";
        o << "wideUnbounded: ";
        rti::util::detail::to_ostream<std::wstring>(o, sample.wideUnbounded);
        o << "wideBounded: ";
        rti::util::detail::to_ostream<std::wstring>(o, sample.wideBounded);
        o <<"]";
        return o;
    }

} // namespace CorpusStrings  

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
        struct native_type_code< ::CorpusStrings::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Sample_g_tc_unbounded_string;
                static DDS_TypeCode Sample_g_tc_bounded_string;
                static DDS_TypeCode Sample_g_tc_wideUnbounded_string;
                static DDS_TypeCode Sample_g_tc_wideBounded_string;

                static DDS_TypeCode_Member Sample_g_tc_members[4]=
                {

                    {
                        (char *)"unbounded",/* Member name */
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
                        (char *)"bounded",/* Member name */
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
                        (char *)"wideUnbounded",/* Member name */
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
                        (char *)"wideBounded",/* Member name */
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

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusStrings::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        Sample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Sample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Sample_g_tc;
                }

                Sample_g_tc_unbounded_string = initialize_string_typecode((255L));
                Sample_g_tc_bounded_string = initialize_string_typecode((8L));
                Sample_g_tc_wideUnbounded_string = initialize_wstring_typecode((255L));
                Sample_g_tc_wideBounded_string = initialize_wstring_typecode((8L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_unbounded_string;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_bounded_string;
                Sample_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_wideUnbounded_string;
                Sample_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_wideBounded_string;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Sample_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                Sample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Sample_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                Sample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                Sample_g_tc_members[2]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";Sample_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                Sample_g_tc_members[3]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusStrings::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusStrings::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->unbounded - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->bounded - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wideUnbounded - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wideBounded - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStrings::Sample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Sample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Sample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Sample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Sample_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusStrings::Sample >;

                Sample_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Sample_g_typePlugin =
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

                return &Sample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusStrings::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStrings::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusStrings::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusStrings::SamplePlugin_new,
                ::CorpusStrings::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusStrings::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusStrings::Sample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusStrings::Sample >::from_cdr_buffer(::CorpusStrings::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusStrings::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusStrings::Sample >::reset_sample(::CorpusStrings::Sample& sample) 
        {
            sample.unbounded = "";
            sample.bounded = "";
            sample.wideUnbounded = L"";
            sample.wideBounded = L"";
        }

        void topic_type_support< ::CorpusStrings::Sample >::allocate_sample(::CorpusStrings::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.unbounded,  -1, 255L);
            ::rti::topic::allocate_sample(sample.bounded,  -1, 8L);
            ::rti::topic::allocate_sample(sample.wideUnbounded,  -1, 255L);
            ::rti::topic::allocate_sample(sample.wideBounded,  -1, 8L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
