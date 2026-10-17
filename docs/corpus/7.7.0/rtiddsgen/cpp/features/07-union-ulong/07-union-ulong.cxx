

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-ulong.idl
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
#include "07-union-ulongPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-ulong.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusUnionUnsignedLong {

    // ---- Choice:

    Choice::Union_::Union_() :
        minimum (0) ,
        maximum (0) ,
        fallback (0)  {
    }

    Choice::Union_::Union_(int32_t minimum_,int32_t maximum_,int32_t fallback_):
        minimum(minimum_), 
        maximum(maximum_), 
        fallback(fallback_) {
    }
    Choice::Choice() :m_d_(default_discriminator())
    {
    }

    void Choice::swap(Choice& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.minimum, other_.m_u_.minimum);
            } break;
            case 4294967295:
            {  
                swap(m_u_.maximum, other_.m_u_.maximum);
            } break;
            default:
            {  
                swap(m_u_.fallback, other_.m_u_.fallback);
            } ;

        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 0:
                {  
                    swap(m_u_.minimum, other_.m_u_.minimum);
                } break;
                case 4294967295:
                {  
                    swap(m_u_.maximum, other_.m_u_.maximum);
                } break;
                default:
                {  
                    swap(m_u_.fallback, other_.m_u_.fallback);
                } ;

            }
        }
    }  

    bool Choice::operator == (const Choice& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.minimum != other_.m_u_.minimum) {
                    return false;
                }
            } break ;
            case 4294967295:
            {  
                if (m_u_.maximum != other_.m_u_.maximum) {
                    return false;
                }
            } break ;
            default:
            {  
                if (m_u_.fallback != other_.m_u_.fallback) {
                    return false;
                }
            } ;
        }
        return true;
    }

    bool Choice::operator != (const Choice& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Choice& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "minimum: " << sample.minimum()<<", ";
            } break ;
            case 4294967295:
            {  
                o << "maximum: " << sample.maximum()<<", ";
            } break ;
            default:
            {  
                o << "fallback: " << sample.fallback();
            } ;
        }
        o <<"]";
        return o;
    }

    uint32_t Choice::default_discriminator() {
        return 0;
    }

} // namespace CorpusUnionUnsignedLong  

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
        struct native_type_code< ::CorpusUnionUnsignedLong::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Choice_g_tc_members[3]=
                {

                    {
                        (char *)"minimum",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(0),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"maximum",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(4294967295),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"fallback",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        RTI_CDR_TYPE_CODE_UNION_DEFAULT_LABEL, /* First label */
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Choice_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        2, /*Ignored*/
                        (char *)"CorpusUnionUnsignedLong::Choice", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        Choice_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Choice*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Choice_g_tc;
                }

                Choice_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Choice_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Choice_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Choice_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Choice_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Choice_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Choice_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Choice_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Choice_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Choice_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Choice_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[2]._annotations._defaultValue._u.long_value = 0;
                Choice_g_tc_members[2]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[2]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Choice_g_tc_members[2]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[2]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                Choice_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_ulong;

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnionUnsignedLong::Choice *sample;

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusUnionUnsignedLong::Choice);
                if (sample == NULL) {
                    return NULL;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(0);
                Choice_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->minimum() - (char *)sample);

                sample->_d(4294967295);
                Choice_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->maximum() - (char *)sample);

                sample->_d(1);
                Choice_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->fallback() - (char *)sample);

                Choice_g_sampleAccessInfo.memberAccessInfos =
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnionUnsignedLong::Choice);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Choice_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Choice_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Choice_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Choice_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusUnionUnsignedLong::Choice >;

                Choice_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Choice_g_typePlugin =
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

                return &Choice_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusUnionUnsignedLong::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnionUnsignedLong::Choice >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnionUnsignedLong::Choice >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnionUnsignedLong::ChoicePlugin_new,
                ::CorpusUnionUnsignedLong::ChoicePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnionUnsignedLong::Choice >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnionUnsignedLong::Choice& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ChoicePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ChoicePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusUnionUnsignedLong::Choice >::from_cdr_buffer(::CorpusUnionUnsignedLong::Choice& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChoicePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnionUnsignedLong::Choice from cdr buffer");
        }

        void topic_type_support< ::CorpusUnionUnsignedLong::Choice >::reset_sample(::CorpusUnionUnsignedLong::Choice& sample) 
        {
            sample._d(0);
            sample.minimum(0);
            sample._d(4294967295);
            sample.maximum(0);
            sample.fallback(0, 1);

            sample._d(::CorpusUnionUnsignedLong::Choice::default_discriminator());
        }

        void topic_type_support< ::CorpusUnionUnsignedLong::Choice >::allocate_sample(::CorpusUnionUnsignedLong::Choice& sample, int, int) 
        {

            sample._d(::CorpusUnionUnsignedLong::Choice::default_discriminator());
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
