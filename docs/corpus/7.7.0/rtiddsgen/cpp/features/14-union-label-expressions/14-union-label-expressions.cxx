

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-label-expressions.idl
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
#include "14-union-label-expressionsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-union-label-expressions.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- LabelExpressions:

LabelExpressions::Union_::Union_() :
    symbolic (0) ,
    arithmetic (0) ,
    hexadecimal (0)  {
}

LabelExpressions::Union_::Union_(int32_t symbolic_,int32_t arithmetic_,int32_t hexadecimal_):
    symbolic(symbolic_), 
    arithmetic(arithmetic_), 
    hexadecimal(hexadecimal_) {
}
LabelExpressions::LabelExpressions() :m_d_(default_discriminator())
{
}

void LabelExpressions::swap(LabelExpressions& other_)  noexcept 
{
    using std::swap;
    swap (m_d_,other_.m_d_);
    switch (::rti::topic::cdr::integer_case(_d())) {
        case Selected:
        {  
            swap(m_u_.symbolic, other_.m_u_.symbolic);
        } break;
        case (3+1):
            {  
                swap(m_u_.arithmetic, other_.m_u_.arithmetic);
        } break;
        case 0x10:
        {  
            swap(m_u_.hexadecimal, other_.m_u_.hexadecimal);
        } break;

        default:
        {
            /*
            * Prevents compiler warnings when discriminator is an enum.
            */
        }
    }
    if (_d() != other_._d()){
        switch(::rti::topic::cdr::integer_case(other_._d())){
            case Selected:
            {  
                swap(m_u_.symbolic, other_.m_u_.symbolic);
            } break;
            case (3+1):
                {  
                    swap(m_u_.arithmetic, other_.m_u_.arithmetic);
            } break;
            case 0x10:
            {  
                swap(m_u_.hexadecimal, other_.m_u_.hexadecimal);
            } break;

            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
    }
}  

bool LabelExpressions::operator == (const LabelExpressions& other_) const {
    if (_d() != other_._d()){
        return false;
    }
    switch(::rti::topic::cdr::integer_case(_d())){
        case Selected:
        {  
            if (m_u_.symbolic != other_.m_u_.symbolic) {
                return false;
            }
        } break ;
        case (3+1):
            {  
                if (m_u_.arithmetic != other_.m_u_.arithmetic) {
                    return false;
            }
        } break ;
        case 0x10:
        {  
            if (m_u_.hexadecimal != other_.m_u_.hexadecimal) {
                return false;
            }
        } break ;
        default:
        {
            /*
            * Prevents compiler warnings when discriminator is an enum.
            */
        }
    }
    return true;
}

bool LabelExpressions::operator != (const LabelExpressions& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const LabelExpressions& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "_d: " << sample._d() <<", ";
    switch(::rti::topic::cdr::integer_case(sample._d())){
        case Selected:
        {  
            o << "symbolic: " << sample.symbolic()<<", ";
        } break ;
        case (3+1):
            {  
                o << "arithmetic: " << sample.arithmetic()<<", ";
        } break ;
        case 0x10:
        {  
            o << "hexadecimal: " << sample.hexadecimal();
        } break ;
        default:
        {
            /*
            * Prevents compiler warnings when discriminator is an enum.
            */
        }
    }
    o <<"]";
    return o;
}

int32_t LabelExpressions::default_discriminator() {
    return 0;
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
        struct native_type_code< ::LabelExpressions > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member LabelExpressions_g_tc_members[3]=
                {

                    {
                        (char *)"symbolic",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(Selected),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"arithmetic",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((3+1)),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"hexadecimal",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(0x10),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode LabelExpressions_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"LabelExpressions", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        LabelExpressions_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for LabelExpressions*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &LabelExpressions_g_tc;
                }

                LabelExpressions_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                LabelExpressions_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                LabelExpressions_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                LabelExpressions_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                LabelExpressions_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                LabelExpressions_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                LabelExpressions_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                LabelExpressions_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                LabelExpressions_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                LabelExpressions_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                LabelExpressions_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[2]._annotations._defaultValue._u.long_value = 0;
                LabelExpressions_g_tc_members[2]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[2]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                LabelExpressions_g_tc_members[2]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                LabelExpressions_g_tc_members[2]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                LabelExpressions_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                LabelExpressions_g_tc._data._sampleAccessInfo = sample_access_info();
                LabelExpressions_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &LabelExpressions_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::LabelExpressions *sample;

                static RTIXCdrMemberAccessInfo LabelExpressions_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo LabelExpressions_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &LabelExpressions_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::LabelExpressions);
                if (sample == NULL) {
                    return NULL;
                }

                LabelExpressions_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(Selected);
                LabelExpressions_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->symbolic() - (char *)sample);

                sample->_d((3+1));
                LabelExpressions_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->arithmetic() - (char *)sample);

                sample->_d(0x10);
                LabelExpressions_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->hexadecimal() - (char *)sample);

                LabelExpressions_g_sampleAccessInfo.memberAccessInfos =
                LabelExpressions_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::LabelExpressions);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        LabelExpressions_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        LabelExpressions_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                LabelExpressions_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                LabelExpressions_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::LabelExpressions >;

                LabelExpressions_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &LabelExpressions_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin LabelExpressions_g_typePlugin =
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

                return &LabelExpressions_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::LabelExpressions >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::LabelExpressions >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::LabelExpressions >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::LabelExpressionsPlugin_new,
                ::LabelExpressionsPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::LabelExpressions >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::LabelExpressions& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = LabelExpressionsPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = LabelExpressionsPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::LabelExpressions >::from_cdr_buffer(::LabelExpressions& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = LabelExpressionsPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::LabelExpressions from cdr buffer");
        }

        void topic_type_support< ::LabelExpressions >::reset_sample(::LabelExpressions& sample) 
        {
            sample._d(Selected);
            sample.symbolic(0);
            sample._d((3+1));
            sample.arithmetic(0);
            sample._d(0x10);
            sample.hexadecimal(0);

            sample._d(::LabelExpressions::default_discriminator());
        }

        void topic_type_support< ::LabelExpressions >::allocate_sample(::LabelExpressions& sample, int, int) 
        {

            sample._d(::LabelExpressions::default_discriminator());
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
