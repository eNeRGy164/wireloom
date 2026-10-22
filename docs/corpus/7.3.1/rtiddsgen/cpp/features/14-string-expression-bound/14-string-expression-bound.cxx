

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-string-expression-bound.idl
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
#include "14-string-expression-boundPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-string-expression-bound.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- StringBounded: 

StringBounded::StringBounded() :
    m_text_ ("")  {

        m_wideText_ = L"";
}   

StringBounded::StringBounded (const std::string& text_,const std::wstring& wideText_):
    m_text_(text_), 
    m_wideText_(wideText_) {
}

void StringBounded::swap(StringBounded& other_)  noexcept 
{
    using std::swap;
    swap(m_text_, other_.m_text_);
    swap(m_wideText_, other_.m_wideText_);
}  

bool StringBounded::operator == (const StringBounded& other_) const {
    if (m_text_ != other_.m_text_) {
        return false;
    }
    if (m_wideText_ != other_.m_wideText_) {
        return false;
    }
    return true;
}

bool StringBounded::operator != (const StringBounded& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const StringBounded& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "text: " << sample.text ()<<", ";
    o << "wideText: " << sample.wideText ();
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
        struct native_type_code< ::StringBounded > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode StringBounded_g_tc_text_string;
                static DDS_TypeCode StringBounded_g_tc_wideText_string;

                static DDS_TypeCode_Member StringBounded_g_tc_members[2]=
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"wideText",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode StringBounded_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"StringBounded", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        StringBounded_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for StringBounded*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &StringBounded_g_tc;
                }

                StringBounded_g_tc_text_string = initialize_string_typecode(((StringBound)));
                StringBounded_g_tc_wideText_string = initialize_wstring_typecode((2L*3L));

                StringBounded_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                StringBounded_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&StringBounded_g_tc_text_string;
                StringBounded_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&StringBounded_g_tc_wideText_string;

                /* Initialize the values for member annotations. */
                StringBounded_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringBounded_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringBounded_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringBounded_g_tc_members[1]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                StringBounded_g_tc._data._sampleAccessInfo = sample_access_info();
                StringBounded_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &StringBounded_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::StringBounded *sample;

                static RTIXCdrMemberAccessInfo StringBounded_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo StringBounded_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &StringBounded_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::StringBounded);
                if (sample == NULL) {
                    return NULL;
                }

                StringBounded_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->text() - (char *)sample);

                StringBounded_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wideText() - (char *)sample);

                StringBounded_g_sampleAccessInfo.memberAccessInfos = 
                StringBounded_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::StringBounded);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        StringBounded_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        StringBounded_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                StringBounded_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                StringBounded_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::StringBounded >;

                StringBounded_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &StringBounded_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin StringBounded_g_typePlugin = 
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

                return &StringBounded_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::StringBounded >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::StringBounded >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::StringBounded >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::StringBoundedPlugin_new,
                ::StringBoundedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::StringBounded >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::StringBounded& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = StringBoundedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = StringBoundedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::StringBounded >::from_cdr_buffer(::StringBounded& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = StringBoundedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::StringBounded from cdr buffer");
        }

        void topic_type_support< ::StringBounded >::reset_sample(::StringBounded& sample) 
        {
            sample.text("");
            sample.wideText(L"");
        }

        void topic_type_support< ::StringBounded >::allocate_sample(::StringBounded& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text(),  -1, (StringBound));
            ::rti::topic::allocate_sample(sample.wideText(),  -1, 2L*3L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
