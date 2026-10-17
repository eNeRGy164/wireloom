

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-enum.idl
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
#include "07-union-enumPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-enum.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusEnumUnion {

    std::ostream& operator << (std::ostream& o,const Kind& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case Kind::Number:
            o << "Kind::Number" << " ";
            break;
            case Kind::Text:
            o << "Kind::Text" << " ";
            break;
            case Kind::PayloadValue:
            o << "Kind::PayloadValue" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }

    // ---- Payload: 

    Payload::Payload() :
        m_code_ (0) ,
        m_label_ ("")  {

    }   

    Payload::Payload (int32_t code_,const std::string& label_):
        m_code_(code_), 
        m_label_(label_) {
    }

    void Payload::swap(Payload& other_)  noexcept 
    {
        using std::swap;
        swap(m_code_, other_.m_code_);
        swap(m_label_, other_.m_label_);
    }  

    bool Payload::operator == (const Payload& other_) const {
        if (m_code_ != other_.m_code_) {
            return false;
        }
        if (m_label_ != other_.m_label_) {
            return false;
        }
        return true;
    }

    bool Payload::operator != (const Payload& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Payload& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "code: " << sample.code ()<<", ";
        o << "label: " << sample.label ();
        o <<"]";
        return o;
    }

    // ---- Choice: 

    Choice::Union_::Union_() :
        m_number_ (0) ,
        m_text_ ("")  {
    }

    Choice::Union_::Union_(int32_t number_,const std::string& text_,const ::CorpusEnumUnion::Payload& payload_):
        m_number_(number_), 
        m_text_(text_), 
        m_payload_(payload_) {
    }

    Choice::Choice() :m_d_(default_discriminator())
    {
    }

    void Choice::swap(Choice& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case (CorpusEnumUnion::Kind::Number):
                {  
                    swap(m_u_.m_number_, other_.m_u_.m_number_);
            } break;
            case (CorpusEnumUnion::Kind::Text):
                {  
                    swap(m_u_.m_text_, other_.m_u_.m_text_);
            } break;
            case (CorpusEnumUnion::Kind::PayloadValue):
                {  
                    swap(m_u_.m_payload_, other_.m_u_.m_payload_);
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
                case (CorpusEnumUnion::Kind::Number):
                    {  
                        swap(m_u_.m_number_, other_.m_u_.m_number_);
                } break;
                case (CorpusEnumUnion::Kind::Text):
                    {  
                        swap(m_u_.m_text_, other_.m_u_.m_text_);
                } break;
                case (CorpusEnumUnion::Kind::PayloadValue):
                    {  
                        swap(m_u_.m_payload_, other_.m_u_.m_payload_);
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

    bool Choice::operator == (const Choice& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case (CorpusEnumUnion::Kind::Number):
                {  
                    if (m_u_.m_number_ != other_.m_u_.m_number_) {
                        return false;
                }
            } break ;
            case (CorpusEnumUnion::Kind::Text):
                {  
                    if (m_u_.m_text_ != other_.m_u_.m_text_) {
                        return false;
                }
            } break ;
            case (CorpusEnumUnion::Kind::PayloadValue):
                {  
                    if (m_u_.m_payload_ != other_.m_u_.m_payload_) {
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

    bool Choice::operator != (const Choice& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Choice& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case (CorpusEnumUnion::Kind::Number):
                {  
                    o << "number: " << sample.number ()<<", ";
            } break ;
            case (CorpusEnumUnion::Kind::Text):
                {  
                    o << "text: " << sample.text ()<<", ";
            } break ;
            case (CorpusEnumUnion::Kind::PayloadValue):
                {  
                    o << "payload: " << sample.payload ();
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

    ::CorpusEnumUnion::Kind Choice::default_discriminator() {
        return ::CorpusEnumUnion::Kind(static_cast< ::CorpusEnumUnion::Kind >(0));
    }

} // namespace CorpusEnumUnion  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusEnumUnion::Kind default_enumerator< ::CorpusEnumUnion::Kind>::value = ::CorpusEnumUnion::Kind::Number;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusEnumUnion::Kind default_enumerator< ::CorpusEnumUnion::Kind>::value = ::CorpusEnumUnion::Kind::Number;
        template<>
        struct native_type_code< ::CorpusEnumUnion::Kind > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Kind_g_tc_members[3]=
                {

                    {
                        (char *)"Number",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumUnion::Kind::Number), 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */ 

                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"Text",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumUnion::Kind::Text), 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */ 

                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"PayloadValue",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumUnion::Kind::PayloadValue), 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */ 

                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Kind_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusEnumUnion::Kind", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        Kind_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Kind*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Kind_g_tc;
                }

                Kind_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                /* Initialize the values for annotations. */
                Kind_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Kind_g_tc._data._annotations._defaultValue._u.long_value = 0;

                Kind_g_tc._data._sampleAccessInfo = sample_access_info();
                Kind_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Kind_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Kind_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Kind_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Kind_g_sampleAccessInfo;
                }

                Kind_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Kind_g_sampleAccessInfo.memberAccessInfos = 
                Kind_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnumUnion::Kind);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Kind_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Kind_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Kind_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Kind_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnumUnion::Kind >;

                Kind_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Kind_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Kind_g_typePlugin = 
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

                return &Kind_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusEnumUnion::Kind >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnumUnion::Kind >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnumUnion::Payload > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Payload_g_tc_label_string;

                static DDS_TypeCode_Member Payload_g_tc_members[2]=
                {

                    {
                        (char *)"code",/* Member name */
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
                        (char *)"label",/* Member name */
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

                static DDS_TypeCode Payload_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusEnumUnion::Payload", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Payload_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Payload*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Payload_g_tc;
                }

                Payload_g_tc_label_string = initialize_string_typecode((16L));

                Payload_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Payload_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Payload_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&Payload_g_tc_label_string;

                /* Initialize the values for member annotations. */
                Payload_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Payload_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Payload_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Payload_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Payload_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Payload_g_tc._data._sampleAccessInfo = sample_access_info();
                Payload_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Payload_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusEnumUnion::Payload *sample;

                static RTIXCdrMemberAccessInfo Payload_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Payload_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Payload_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusEnumUnion::Payload);
                if (sample == NULL) {
                    return NULL;
                }

                Payload_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->code() - (char *)sample);

                Payload_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->label() - (char *)sample);

                Payload_g_sampleAccessInfo.memberAccessInfos = 
                Payload_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnumUnion::Payload);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Payload_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Payload_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Payload_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Payload_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnumUnion::Payload >;

                Payload_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Payload_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Payload_g_typePlugin = 
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

                return &Payload_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusEnumUnion::Payload >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnumUnion::Payload >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnumUnion::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Choice_g_tc_text_string;

                static DDS_TypeCode_Member Choice_g_tc_members[3]=
                {

                    {
                        (char *)"number",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((CorpusEnumUnion::Kind::Number)),
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
                        static_cast<int>((CorpusEnumUnion::Kind::Text)),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"payload",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((CorpusEnumUnion::Kind::PayloadValue)),
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
                        -1, /*Ignored*/
                        (char *)"CorpusEnumUnion::Choice", /* Name */
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
                Choice_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnumUnion::Payload>::get().native();

                /* Initialize the values for member annotations. */
                Choice_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Choice_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Choice_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Choice_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Choice_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                /* Discriminator type code */
                Choice_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnumUnion::Kind>::get().native();

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusEnumUnion::Choice *sample;

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusEnumUnion::Choice);
                if (sample == NULL) {
                    return NULL;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = (CorpusEnumUnion::Kind::Number);
                Choice_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->number() - (char *)sample);

                sample->_d() = (CorpusEnumUnion::Kind::Text);
                Choice_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->text() - (char *)sample);

                sample->_d() = (CorpusEnumUnion::Kind::PayloadValue);
                Choice_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->payload() - (char *)sample);

                Choice_g_sampleAccessInfo.memberAccessInfos = 
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnumUnion::Choice);

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
                interpreter::get_aggregation_value_pointer< ::CorpusEnumUnion::Choice >;

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

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusEnumUnion::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnumUnion::Choice >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusEnumUnion::Payload >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusEnumUnion::PayloadPlugin_new,
                ::CorpusEnumUnion::PayloadPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusEnumUnion::Payload >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusEnumUnion::Payload& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = PayloadPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = PayloadPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusEnumUnion::Payload >::from_cdr_buffer(::CorpusEnumUnion::Payload& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = PayloadPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusEnumUnion::Payload from cdr buffer");
        }

        void topic_type_support< ::CorpusEnumUnion::Payload >::reset_sample(::CorpusEnumUnion::Payload& sample) 
        {
            sample.code(0);
            sample.label("");
        }

        void topic_type_support< ::CorpusEnumUnion::Payload >::allocate_sample(::CorpusEnumUnion::Payload& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.label(),  -1, 16L);
        }
        void topic_type_support< ::CorpusEnumUnion::Choice >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusEnumUnion::ChoicePlugin_new,
                ::CorpusEnumUnion::ChoicePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusEnumUnion::Choice >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusEnumUnion::Choice& sample,
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

        void topic_type_support< ::CorpusEnumUnion::Choice >::from_cdr_buffer(::CorpusEnumUnion::Choice& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChoicePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusEnumUnion::Choice from cdr buffer");
        }

        void topic_type_support< ::CorpusEnumUnion::Choice >::reset_sample(::CorpusEnumUnion::Choice& sample) 
        {
            sample._d() = (CorpusEnumUnion::Kind::Number);
            sample.number(0);
            sample._d() = (CorpusEnumUnion::Kind::Text);
            sample.text("");
            sample._d() = (CorpusEnumUnion::Kind::PayloadValue);
            ::rti::topic::reset_sample(sample.payload());

            sample._d() = ::CorpusEnumUnion::Choice::default_discriminator();
        }

        void topic_type_support< ::CorpusEnumUnion::Choice >::allocate_sample(::CorpusEnumUnion::Choice& sample, int, int) 
        {
            sample._d() = (CorpusEnumUnion::Kind::Text);
            ::rti::topic::allocate_sample(sample.text(),  -1, 255L);
            sample._d() = (CorpusEnumUnion::Kind::PayloadValue);
            ::rti::topic::allocate_sample(sample.payload(),  -1, -1);

            sample._d() = ::CorpusEnumUnion::Choice::default_discriminator();
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
