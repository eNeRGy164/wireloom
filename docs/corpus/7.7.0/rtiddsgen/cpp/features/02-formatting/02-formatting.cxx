

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-formatting.idl
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
#include "02-formattingPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "02-formatting.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusFormatting {

    // ---- Value:

    Value::Value() :
        text ("")  {

    }

    Value::Value (const ::omg::types::string_view& text_) {
        text = text_;
    }

    bool operator == (const Value& a, const Value& b) {

        if (a.text != b.text) {
            return false;
        }

        return true;
    }

    bool operator != (const Value& a, const Value& b) {
        return !operator ==(a, b);
    }

    void swap(Value& a, Value& b) noexcept
    {
        using std::swap;

        swap(a.text, b.text);

    }  
    std::ostream& operator << (std::ostream& o,const Value& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "text: " << sample.text;
        o <<"]";
        return o;
    }

} // namespace CorpusFormatting  

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
        struct native_type_code< ::CorpusFormatting::Value > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Value_g_tc_text_string;

                static DDS_TypeCode_Member Value_g_tc_members[1]=
                {

                    {
                        (char *)"text",/* Member name */
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

                static DDS_TypeCode Value_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusFormatting::Value", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Value_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Value*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Value_g_tc;
                }

                Value_g_tc_text_string = initialize_string_typecode((16L));

                Value_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Value_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&Value_g_tc_text_string;

                /* Initialize the values for member annotations. */
                Value_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Value_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Value_g_tc._data._sampleAccessInfo = sample_access_info();
                Value_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Value_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusFormatting::Value *sample;

                static RTIXCdrMemberAccessInfo Value_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Value_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Value_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusFormatting::Value);
                if (sample == NULL) {
                    return NULL;
                }

                Value_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->text - (char *)sample);

                Value_g_sampleAccessInfo.memberAccessInfos =
                Value_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusFormatting::Value);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Value_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Value_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Value_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Value_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusFormatting::Value >;

                Value_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Value_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Value_g_typePlugin =
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

                return &Value_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusFormatting::Value >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusFormatting::Value >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusFormatting::Value >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusFormatting::ValuePlugin_new,
                ::CorpusFormatting::ValuePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusFormatting::Value >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusFormatting::Value& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ValuePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ValuePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusFormatting::Value >::from_cdr_buffer(::CorpusFormatting::Value& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ValuePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusFormatting::Value from cdr buffer");
        }

        void topic_type_support< ::CorpusFormatting::Value >::reset_sample(::CorpusFormatting::Value& sample) 
        {
            sample.text = "";
        }

        void topic_type_support< ::CorpusFormatting::Value >::allocate_sample(::CorpusFormatting::Value& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text,  -1, 16L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
