

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-string-sequences.idl
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
#include "09-optional-string-sequencesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-optional-string-sequences.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusOptionalStringSequences {

    // ---- Sample:

    Sample::Sample()  {

    }

    Sample::Sample (const ::omg::types::optional< ::omg::types::bounded_sequence< std::string, 4L > >& narrowValues_,const ::omg::types::optional< ::omg::types::bounded_sequence< std::wstring, 4L > >& wideValues_) {
        narrowValues = narrowValues_;
        wideValues = wideValues_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.narrowValues != b.narrowValues) {
            return false;
        }
        if (a.wideValues != b.wideValues) {
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

        swap(a.narrowValues, b.narrowValues);
        swap(a.wideValues, b.wideValues);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "narrowValues: " << sample.narrowValues<<", ";
        o << "wideValues: " << sample.wideValues;
        o <<"]";
        return o;
    }

} // namespace CorpusOptionalStringSequences  

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
        struct native_type_code< ::CorpusOptionalStringSequences::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Sample_g_tc_narrowValues_string;
                static DDS_TypeCode Sample_g_tc_narrowValues_sequence;
                static DDS_TypeCode Sample_g_tc_wideValues_string;
                static DDS_TypeCode Sample_g_tc_wideValues_sequence;

                static DDS_TypeCode_Member Sample_g_tc_members[2]=
                {

                    {
                        (char *)"narrowValues",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"wideValues",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusOptionalStringSequences::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
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

                Sample_g_tc_narrowValues_string = initialize_string_typecode((16L));
                Sample_g_tc_narrowValues_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::string, 4L > >((4L));
                Sample_g_tc_wideValues_string = initialize_wstring_typecode((16L));
                Sample_g_tc_wideValues_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::wstring, 4L > >((4L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_narrowValues_sequence._data._typeCode = (RTICdrTypeCode *)&Sample_g_tc_narrowValues_string;
                Sample_g_tc_wideValues_sequence._data._typeCode = (RTICdrTypeCode *)&Sample_g_tc_wideValues_string;
                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_narrowValues_sequence;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_wideValues_sequence;

                /* Initialize the values for member annotations. */

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusOptionalStringSequences::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusOptionalStringSequences::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->narrowValues - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wideValues - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalStringSequences::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalStringSequences::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusOptionalStringSequences::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalStringSequences::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusOptionalStringSequences::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusOptionalStringSequences::SamplePlugin_new,
                ::CorpusOptionalStringSequences::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusOptionalStringSequences::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusOptionalStringSequences::Sample& sample,
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

        void topic_type_support< ::CorpusOptionalStringSequences::Sample >::from_cdr_buffer(::CorpusOptionalStringSequences::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusOptionalStringSequences::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusOptionalStringSequences::Sample >::reset_sample(::CorpusOptionalStringSequences::Sample& sample) 
        {
            ::rti::topic::reset_sample(sample.narrowValues);
            ::rti::topic::reset_sample(sample.wideValues);
        }

        void topic_type_support< ::CorpusOptionalStringSequences::Sample >::allocate_sample(::CorpusOptionalStringSequences::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
