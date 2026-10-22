

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-annotation-arguments.idl
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
#include "14-annotation-argumentsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-annotation-arguments.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- AnnotationArguments: 

AnnotationArguments::AnnotationArguments() :
    m_plain_ (0) ,
    m_selected_ ((Selected))  {

}   

AnnotationArguments::AnnotationArguments (int32_t plain_,int32_t selected_):
    m_plain_(plain_), 
    m_selected_(selected_) {
}

void AnnotationArguments::swap(AnnotationArguments& other_)  noexcept 
{
    using std::swap;
    swap(m_plain_, other_.m_plain_);
    swap(m_selected_, other_.m_selected_);
}  

bool AnnotationArguments::operator == (const AnnotationArguments& other_) const {
    if (m_plain_ != other_.m_plain_) {
        return false;
    }
    if (m_selected_ != other_.m_selected_) {
        return false;
    }
    return true;
}

bool AnnotationArguments::operator != (const AnnotationArguments& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const AnnotationArguments& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "plain: " << sample.plain ()<<", ";
    o << "selected: " << sample.selected ();
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
        struct native_type_code< ::AnnotationArguments > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member AnnotationArguments_g_tc_members[2]=
                {

                    {
                        (char *)"plain",/* Member name */
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
                        (char *)"selected",/* Member name */
                        {
                            16,/* Representation ID */
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

                static DDS_TypeCode AnnotationArguments_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"AnnotationArguments", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        AnnotationArguments_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for AnnotationArguments*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &AnnotationArguments_g_tc;
                }

                AnnotationArguments_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                AnnotationArguments_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                AnnotationArguments_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                AnnotationArguments_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                AnnotationArguments_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                AnnotationArguments_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                AnnotationArguments_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                AnnotationArguments_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                AnnotationArguments_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                AnnotationArguments_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                AnnotationArguments_g_tc_members[1]._annotations._defaultValue._u.long_value = (Selected);
                AnnotationArguments_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                AnnotationArguments_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                AnnotationArguments_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                AnnotationArguments_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                AnnotationArguments_g_tc._data._sampleAccessInfo = sample_access_info();
                AnnotationArguments_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &AnnotationArguments_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::AnnotationArguments *sample;

                static RTIXCdrMemberAccessInfo AnnotationArguments_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo AnnotationArguments_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &AnnotationArguments_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::AnnotationArguments);
                if (sample == NULL) {
                    return NULL;
                }

                AnnotationArguments_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->plain() - (char *)sample);

                AnnotationArguments_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->selected() - (char *)sample);

                AnnotationArguments_g_sampleAccessInfo.memberAccessInfos = 
                AnnotationArguments_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::AnnotationArguments);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        AnnotationArguments_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        AnnotationArguments_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                AnnotationArguments_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                AnnotationArguments_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::AnnotationArguments >;

                AnnotationArguments_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &AnnotationArguments_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin AnnotationArguments_g_typePlugin = 
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

                return &AnnotationArguments_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::AnnotationArguments >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::AnnotationArguments >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::AnnotationArguments >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::AnnotationArgumentsPlugin_new,
                ::AnnotationArgumentsPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::AnnotationArguments >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::AnnotationArguments& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = AnnotationArgumentsPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = AnnotationArgumentsPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::AnnotationArguments >::from_cdr_buffer(::AnnotationArguments& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = AnnotationArgumentsPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::AnnotationArguments from cdr buffer");
        }

        void topic_type_support< ::AnnotationArguments >::reset_sample(::AnnotationArguments& sample) 
        {
            sample.plain(0);
            sample.selected((Selected));
        }

        void topic_type_support< ::AnnotationArguments >::allocate_sample(::AnnotationArguments& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
