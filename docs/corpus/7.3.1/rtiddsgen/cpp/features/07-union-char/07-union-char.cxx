

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char.idl
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
#include "07-union-charPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-char.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusCharUnion {

    // ---- Choice: 

    Choice::Union_::Union_() :
        m_letter_ (0) ,
        m_text_ ("") ,
        m_other_ (0)  {
    }

    Choice::Union_::Union_(int32_t letter_,const std::string& text_,bool other_):
        m_letter_(letter_), 
        m_text_(text_), 
        m_other_(other_) {
    }

    Choice::Choice() :m_d_(default_discriminator())
    {
    }

    void Choice::swap(Choice& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 'a':
            {  
                swap(m_u_.m_letter_, other_.m_u_.m_letter_);
            } break;
            case 'z':
            {  
                swap(m_u_.m_text_, other_.m_u_.m_text_);
            } break;
            default:
            {  
                swap(m_u_.m_other_, other_.m_u_.m_other_);
            } ;

        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 'a':
                {  
                    swap(m_u_.m_letter_, other_.m_u_.m_letter_);
                } break;
                case 'z':
                {  
                    swap(m_u_.m_text_, other_.m_u_.m_text_);
                } break;
                default:
                {  
                    swap(m_u_.m_other_, other_.m_u_.m_other_);
                } ;

            }
        }
    }  

    bool Choice::operator == (const Choice& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 'a':
            {  
                if (m_u_.m_letter_ != other_.m_u_.m_letter_) {
                    return false;
                }
            } break ;
            case 'z':
            {  
                if (m_u_.m_text_ != other_.m_u_.m_text_) {
                    return false;
                }
            } break ;
            default:
            {  
                if (m_u_.m_other_ != other_.m_u_.m_other_) {
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
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 'a':
            {  
                o << "letter: " << sample.letter ()<<", ";
            } break ;
            case 'z':
            {  
                o << "text: " << sample.text ()<<", ";
            } break ;
            default:
            {  
                o << "other: " << sample.other ();
            } ;
        }
        o <<"]";
        return o;
    }

    char Choice::default_discriminator() {
        return 0;
    }

} // namespace CorpusCharUnion  

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
        struct native_type_code< ::CorpusCharUnion::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Choice_g_tc_text_string;

                static DDS_TypeCode_Member Choice_g_tc_members[3]=
                {

                    {
                        (char *)"letter",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>('a'),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"text",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>('z'),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"other",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Choice_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        2, /*Ignored*/
                        (char *)"CorpusCharUnion::Choice", /* Name */
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

                Choice_g_tc_text_string = initialize_string_typecode((255L));

                Choice_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Choice_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Choice_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&Choice_g_tc_text_string;
                Choice_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_bool_typecode();

                /* Initialize the values for member annotations. */
                Choice_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Choice_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Choice_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Choice_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Choice_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                Choice_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_BOOLEAN;
                Choice_g_tc_members[2]._annotations._defaultValue._u.boolean_value = 0;

                /* Discriminator type code */
                Choice_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_char;

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusCharUnion::Choice *sample;

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusCharUnion::Choice);
                if (sample == NULL) {
                    return NULL;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 'a';
                Choice_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->letter() - (char *)sample);

                sample->_d() = 'z';
                Choice_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->text() - (char *)sample);

                sample->_d() = 0;
                Choice_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->other() - (char *)sample);

                Choice_g_sampleAccessInfo.memberAccessInfos = 
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCharUnion::Choice);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCharUnion::Choice >;

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

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusCharUnion::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCharUnion::Choice >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusCharUnion::Choice >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusCharUnion::ChoicePlugin_new,
                ::CorpusCharUnion::ChoicePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusCharUnion::Choice >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusCharUnion::Choice& sample,
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

        void topic_type_support< ::CorpusCharUnion::Choice >::from_cdr_buffer(::CorpusCharUnion::Choice& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChoicePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusCharUnion::Choice from cdr buffer");
        }

        void topic_type_support< ::CorpusCharUnion::Choice >::reset_sample(::CorpusCharUnion::Choice& sample) 
        {
            sample._d() = 'a';
            sample.letter(0);
            sample._d() = 'z';
            sample.text("");
            sample._d() = ::CorpusCharUnion::Choice::default_discriminator();

            sample.other(0);

            sample._d() = ::CorpusCharUnion::Choice::default_discriminator();
        }

        void topic_type_support< ::CorpusCharUnion::Choice >::allocate_sample(::CorpusCharUnion::Choice& sample, int, int) 
        {
            sample._d() = 'z';
            ::rti::topic::allocate_sample(sample.text(),  -1, 255L);

            sample._d() = ::CorpusCharUnion::Choice::default_discriminator();
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
