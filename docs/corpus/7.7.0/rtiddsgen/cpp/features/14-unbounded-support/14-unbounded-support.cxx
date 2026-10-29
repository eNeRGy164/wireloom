

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-support.idl
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
#include "14-unbounded-supportPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-unbounded-support.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusUnboundedSupport {

    // ---- Sample:

    Sample::Sample() :
        text ("") ,
        boundedText ("")  {

            wideText = L"";
    }

    Sample::Sample (const ::omg::types::sequence< int32_t >& values_,const ::omg::types::string_view& text_,const ::omg::types::wstring_view& wideText_,const ::omg::types::sequence< std::string >& labels_,const ::CorpusUnboundedSupport::LongValues& aliasValues_,const ::omg::types::bounded_sequence< int32_t, 4L >& boundedValues_,const ::omg::types::string_view& boundedText_) {
        values = values_;
        text = text_;
        wideText = wideText_;
        labels = labels_;
        aliasValues = aliasValues_;
        boundedValues = boundedValues_;
        boundedText = boundedText_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.values != b.values) {
            return false;
        }
        if (a.text != b.text) {
            return false;
        }
        if (a.wideText != b.wideText) {
            return false;
        }
        if (a.labels != b.labels) {
            return false;
        }
        if (a.aliasValues != b.aliasValues) {
            return false;
        }
        if (a.boundedValues != b.boundedValues) {
            return false;
        }
        if (a.boundedText != b.boundedText) {
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

        swap(a.values, b.values);
        swap(a.text, b.text);
        swap(a.wideText, b.wideText);
        swap(a.labels, b.labels);
        swap(a.aliasValues, b.aliasValues);
        swap(a.boundedValues, b.boundedValues);
        swap(a.boundedText, b.boundedText);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "values: " << sample.values<<", ";
        o << "text: " << sample.text<<", ";
        o << "wideText: ";
        rti::util::detail::to_ostream<std::wstring>(o, sample.wideText);
        o << "labels: " << sample.labels<<", ";
        o << "aliasValues: " << sample.aliasValues<<", ";
        o << "boundedValues: " << sample.boundedValues<<", ";
        o << "boundedText: " << sample.boundedText;
        o <<"]";
        return o;
    }

} // namespace CorpusUnboundedSupport  

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
        struct native_type_code< ::CorpusUnboundedSupport::LongValues_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode LongValues_g_tc_sequence;

                static DDS_TypeCode LongValues_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSupport::LongValues", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  LongValues */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &LongValues_g_tc;
                }

                LongValues_g_tc_sequence = initialize_sequence_typecode< ::omg::types::sequence< int32_t > >(RTIXCdrLong_MAX);

                LongValues_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                LongValues_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                LongValues_g_tc._data._typeCode =   (RTICdrTypeCode *)& LongValues_g_tc_sequence;

                /* Initialize the values for member annotations. */

                LongValues_g_tc._data._sampleAccessInfo = sample_access_info();
                LongValues_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &LongValues_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo LongValues_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo LongValues_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &LongValues_g_sampleAccessInfo;
                }

                LongValues_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                LongValues_g_sampleAccessInfo.memberAccessInfos =
                LongValues_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSupport::LongValues);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        LongValues_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        LongValues_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                LongValues_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                LongValues_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSupport::LongValues >;

                LongValues_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &LongValues_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin LongValues_g_typePlugin =
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

                return &LongValues_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSupport::LongValues_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSupport::LongValues_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSupport::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Sample_g_tc_values_sequence;
                static DDS_TypeCode Sample_g_tc_text_string;
                static DDS_TypeCode Sample_g_tc_wideText_string;
                static DDS_TypeCode Sample_g_tc_labels_string;
                static DDS_TypeCode Sample_g_tc_labels_sequence;
                static DDS_TypeCode Sample_g_tc_boundedValues_sequence;
                static DDS_TypeCode Sample_g_tc_boundedText_string;

                static DDS_TypeCode_Member Sample_g_tc_members[7]=
                {

                    {
                        (char *)"values",/* Member name */
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
                        (char *)"text",/* Member name */
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
                        (char *)"wideText",/* Member name */
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
                        (char *)"labels",/* Member name */
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
                    }, 
                    {
                        (char *)"aliasValues",/* Member name */
                        {
                            4,/* Representation ID */
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
                        (char *)"boundedValues",/* Member name */
                        {
                            5,/* Representation ID */
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
                        (char *)"boundedText",/* Member name */
                        {
                            6,/* Representation ID */
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
                        (char *)"CorpusUnboundedSupport::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        7, /* Number of members */
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

                Sample_g_tc_values_sequence = initialize_sequence_typecode< ::omg::types::sequence< int32_t > >(RTIXCdrLong_MAX);
                Sample_g_tc_text_string = initialize_string_typecode(RTIXCdrLong_MAX);
                Sample_g_tc_wideText_string = initialize_wstring_typecode(RTIXCdrLong_MAX);
                Sample_g_tc_labels_string = initialize_string_typecode(RTIXCdrLong_MAX);
                Sample_g_tc_labels_sequence = initialize_sequence_typecode< ::omg::types::sequence< std::string > >(RTIXCdrLong_MAX);
                Sample_g_tc_boundedValues_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< int32_t, 4L > >((4L));
                Sample_g_tc_boundedText_string = initialize_string_typecode((8L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_values_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_labels_sequence._data._typeCode = (RTICdrTypeCode *)&Sample_g_tc_labels_string;
                Sample_g_tc_boundedValues_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_values_sequence;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_text_string;
                Sample_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_wideText_string;
                Sample_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_labels_sequence;
                Sample_g_tc_members[4]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSupport::LongValues_AliasTag_t>::get().native();
                Sample_g_tc_members[5]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_boundedValues_sequence;
                Sample_g_tc_members[6]._representation._typeCode =  (RTICdrTypeCode *)&Sample_g_tc_boundedText_string;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Sample_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                Sample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                Sample_g_tc_members[2]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";Sample_g_tc_members[6]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Sample_g_tc_members[6]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnboundedSupport::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[7] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusUnboundedSupport::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->values - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->text - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wideText - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->labels - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->aliasValues - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedValues - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedText - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSupport::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSupport::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnboundedSupport::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSupport::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnboundedSupport::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnboundedSupport::SamplePlugin_new,
                ::CorpusUnboundedSupport::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnboundedSupport::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnboundedSupport::Sample& sample,
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

        void topic_type_support< ::CorpusUnboundedSupport::Sample >::from_cdr_buffer(::CorpusUnboundedSupport::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnboundedSupport::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusUnboundedSupport::Sample >::reset_sample(::CorpusUnboundedSupport::Sample& sample) 
        {
            ::rti::topic::reset_sample(sample.values);
            sample.text = "";
            sample.wideText = L"";
            ::rti::topic::reset_sample(sample.labels);
            ::rti::topic::reset_sample(sample.aliasValues);
            ::rti::topic::reset_sample(sample.boundedValues);
            sample.boundedText = "";
        }

        void topic_type_support< ::CorpusUnboundedSupport::Sample >::allocate_sample(::CorpusUnboundedSupport::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.values,  -1, -1);
            ::rti::topic::allocate_sample(sample.text,  -1, -1);
            ::rti::topic::allocate_sample(sample.wideText,  -1, -1);
            ::rti::topic::allocate_sample(sample.labels,  -1, -1);
            ::rti::topic::allocate_sample(sample.aliasValues,  -1, -1);
            ::rti::topic::allocate_sample(sample.boundedValues,  4L, -1);
            ::rti::topic::allocate_sample(sample.boundedText,  -1, 8L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
