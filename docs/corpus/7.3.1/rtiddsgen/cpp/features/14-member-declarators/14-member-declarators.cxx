

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-member-declarators.idl
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
#include "14-member-declaratorsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-member-declarators.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- Declarators: 

Declarators::Declarators() :
    m_first_ (0) ,
    m_second_ (0)  {

}   

Declarators::Declarators (int32_t first_,int32_t second_,const ::dds::core::array< int32_t, 2L>& numbers_,const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& matrix_):
    m_first_(first_), 
    m_second_(second_), 
    m_numbers_(numbers_), 
    m_matrix_(matrix_) {
}

void Declarators::swap(Declarators& other_)  noexcept 
{
    using std::swap;
    swap(m_first_, other_.m_first_);
    swap(m_second_, other_.m_second_);
    swap(m_numbers_, other_.m_numbers_);
    swap(m_matrix_, other_.m_matrix_);
}  

bool Declarators::operator == (const Declarators& other_) const {
    if (m_first_ != other_.m_first_) {
        return false;
    }
    if (m_second_ != other_.m_second_) {
        return false;
    }
    if (m_numbers_ != other_.m_numbers_) {
        return false;
    }
    if (m_matrix_ != other_.m_matrix_) {
        return false;
    }
    return true;
}

bool Declarators::operator != (const Declarators& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const Declarators& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "first: " << sample.first ()<<", ";
    o << "second: " << sample.second ()<<", ";
    o << "numbers: " << sample.numbers ()<<", ";
    o << "matrix: " << sample.matrix ();
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
        struct native_type_code< ::Declarators > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};
                static DDS_UnsignedLong Declarators_g_tc_matrix_dimensions [2] = {2L,3L} ;

                static DDS_TypeCode Declarators_g_tc_numbers_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);
                static DDS_TypeCode Declarators_g_tc_matrix_array =DDS_INITIALIZE_ARRAY_TYPECODE(2,2L,Declarators_g_tc_matrix_dimensions,NULL);

                static DDS_TypeCode_Member Declarators_g_tc_members[4]=
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"second",/* Member name */
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"numbers",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"matrix",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Declarators_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"Declarators", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        Declarators_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Declarators*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Declarators_g_tc;
                }

                Declarators_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Declarators_g_tc_numbers_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                Declarators_g_tc_matrix_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                Declarators_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Declarators_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Declarators_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)& Declarators_g_tc_numbers_array;
                Declarators_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)& Declarators_g_tc_matrix_array;

                /* Initialize the values for member annotations. */
                Declarators_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Declarators_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Declarators_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Declarators_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Declarators_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Declarators_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Declarators_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Declarators_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Declarators_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Declarators_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Declarators_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Declarators_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Declarators_g_tc._data._sampleAccessInfo = sample_access_info();
                Declarators_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Declarators_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::Declarators *sample;

                static RTIXCdrMemberAccessInfo Declarators_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Declarators_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Declarators_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::Declarators);
                if (sample == NULL) {
                    return NULL;
                }

                Declarators_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->first() - (char *)sample);

                Declarators_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->second() - (char *)sample);

                Declarators_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->numbers() - (char *)sample);

                Declarators_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->matrix() - (char *)sample);

                Declarators_g_sampleAccessInfo.memberAccessInfos = 
                Declarators_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::Declarators);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Declarators_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Declarators_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Declarators_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Declarators_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::Declarators >;

                Declarators_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Declarators_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Declarators_g_typePlugin = 
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

                return &Declarators_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::Declarators >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Declarators >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::Declarators >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::DeclaratorsPlugin_new,
                ::DeclaratorsPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::Declarators >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::Declarators& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = DeclaratorsPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = DeclaratorsPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::Declarators >::from_cdr_buffer(::Declarators& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DeclaratorsPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::Declarators from cdr buffer");
        }

        void topic_type_support< ::Declarators >::reset_sample(::Declarators& sample) 
        {
            sample.first(0);
            sample.second(0);
            ::rti::topic::reset_sample(sample.numbers());
            ::rti::topic::reset_sample(sample.matrix());
        }

        void topic_type_support< ::Declarators >::allocate_sample(::Declarators& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.numbers(),  -1, -1);
            ::rti::topic::allocate_sample(sample.matrix(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
