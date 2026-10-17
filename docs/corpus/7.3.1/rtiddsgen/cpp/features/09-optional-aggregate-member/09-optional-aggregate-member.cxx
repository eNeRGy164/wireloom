

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
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
#include "09-optional-aggregate-memberPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-optional-aggregate-member.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusOptionalAggregate {

    // ---- Payload: 

    Payload::Payload() :
        m_value_ (0)  {

    }   

    Payload::Payload (int32_t value_):
        m_value_(value_) {
    }

    void Payload::swap(Payload& other_)  noexcept 
    {
        using std::swap;
        swap(m_value_, other_.m_value_);
    }  

    bool Payload::operator == (const Payload& other_) const {
        if (m_value_ != other_.m_value_) {
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
        o << "value: " << sample.value ();
        o <<"]";
        return o;
    }

    // ---- Choice: 

    Choice::Union_::Union_() :
        m_number_ (0)  {
    }

    Choice::Union_::Union_(const ::CorpusOptionalAggregate::Payload& payload_,int32_t number_):
        m_payload_(payload_), 
        m_number_(number_) {
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
                swap(m_u_.m_payload_, other_.m_u_.m_payload_);
            } break;
            case 1:
            {  
                swap(m_u_.m_number_, other_.m_u_.m_number_);
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
                case 0:
                {  
                    swap(m_u_.m_payload_, other_.m_u_.m_payload_);
                } break;
                case 1:
                {  
                    swap(m_u_.m_number_, other_.m_u_.m_number_);
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
            case 0:
            {  
                if (m_u_.m_payload_ != other_.m_u_.m_payload_) {
                    return false;
                }
            } break ;
            case 1:
            {  
                if (m_u_.m_number_ != other_.m_u_.m_number_) {
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
            case 0:
            {  
                o << "payload: " << sample.payload ()<<", ";
            } break ;
            case 1:
            {  
                o << "number: " << sample.number ();
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

    int32_t Choice::default_discriminator() {
        return 0;
    }

    // ---- Holder: 

    Holder::Holder()  {

    }   

    Holder::Holder (const ::dds::core::optional< ::CorpusOptionalAggregate::Payload >& payload_,const ::dds::core::optional< ::CorpusOptionalAggregate::Payload >& payloadAlias_,const ::dds::core::optional< ::CorpusOptionalAggregate::Choice >& choice_,const ::dds::core::optional< ::CorpusOptionalAggregate::Choice >& choiceAlias_):
        m_payload_(payload_), 
        m_payloadAlias_(payloadAlias_), 
        m_choice_(choice_), 
        m_choiceAlias_(choiceAlias_) {
    }

    void Holder::swap(Holder& other_)  noexcept 
    {
        using std::swap;
        swap(m_payload_, other_.m_payload_);
        swap(m_payloadAlias_, other_.m_payloadAlias_);
        swap(m_choice_, other_.m_choice_);
        swap(m_choiceAlias_, other_.m_choiceAlias_);
    }  

    bool Holder::operator == (const Holder& other_) const {
        if (m_payload_ != other_.m_payload_) {
            return false;
        }
        if (m_payloadAlias_ != other_.m_payloadAlias_) {
            return false;
        }
        if (m_choice_ != other_.m_choice_) {
            return false;
        }
        if (m_choiceAlias_ != other_.m_choiceAlias_) {
            return false;
        }
        return true;
    }

    bool Holder::operator != (const Holder& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Holder& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "payload: " << sample.payload ()<<", ";
        o << "payloadAlias: " << sample.payloadAlias ()<<", ";
        o << "choice: " << sample.choice ()<<", ";
        o << "choiceAlias: " << sample.choiceAlias ();
        o <<"]";
        return o;
    }

} // namespace CorpusOptionalAggregate  

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
        struct native_type_code< ::CorpusOptionalAggregate::Payload > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Payload_g_tc_members[1]=
                {

                    {
                        (char *)"value",/* Member name */
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
                    }
                };

                static DDS_TypeCode Payload_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusOptionalAggregate::Payload", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
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

                Payload_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Payload_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Payload_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Payload_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Payload_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Payload_g_tc._data._sampleAccessInfo = sample_access_info();
                Payload_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Payload_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusOptionalAggregate::Payload *sample;

                static RTIXCdrMemberAccessInfo Payload_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Payload_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Payload_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusOptionalAggregate::Payload);
                if (sample == NULL) {
                    return NULL;
                }

                Payload_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                Payload_g_sampleAccessInfo.memberAccessInfos = 
                Payload_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalAggregate::Payload);

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
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalAggregate::Payload >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusOptionalAggregate::Payload >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalAggregate::Payload >::get())));
        }

        template<>
        struct native_type_code< ::CorpusOptionalAggregate::PayloadAlias_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode PayloadAlias_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusOptionalAggregate::PayloadAlias", /* Name */
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
                    }}; /* Type code for  PayloadAlias */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &PayloadAlias_g_tc;
                }

                PayloadAlias_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                PayloadAlias_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::Payload>::get().native();

                /* Initialize the values for member annotations. */

                PayloadAlias_g_tc._data._sampleAccessInfo = sample_access_info();
                PayloadAlias_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &PayloadAlias_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo PayloadAlias_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo PayloadAlias_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &PayloadAlias_g_sampleAccessInfo;
                }

                PayloadAlias_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                PayloadAlias_g_sampleAccessInfo.memberAccessInfos = 
                PayloadAlias_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalAggregate::PayloadAlias);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        PayloadAlias_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        PayloadAlias_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                PayloadAlias_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                PayloadAlias_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalAggregate::PayloadAlias >;

                PayloadAlias_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &PayloadAlias_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin PayloadAlias_g_typePlugin = 
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

                return &PayloadAlias_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusOptionalAggregate::PayloadAlias_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalAggregate::PayloadAlias_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusOptionalAggregate::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Choice_g_tc_members[2]=
                {

                    {
                        (char *)"payload",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"number",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(1),
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
                        (char *)"CorpusOptionalAggregate::Choice", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
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

                Choice_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::Payload>::get().native();
                Choice_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Choice_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Choice_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Choice_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Choice_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                Choice_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusOptionalAggregate::Choice *sample;

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusOptionalAggregate::Choice);
                if (sample == NULL) {
                    return NULL;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 0;
                Choice_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->payload() - (char *)sample);

                sample->_d() = 1;
                Choice_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->number() - (char *)sample);

                Choice_g_sampleAccessInfo.memberAccessInfos = 
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalAggregate::Choice);

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
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalAggregate::Choice >;

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

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusOptionalAggregate::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalAggregate::Choice >::get())));
        }

        template<>
        struct native_type_code< ::CorpusOptionalAggregate::ChoiceAlias_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ChoiceAlias_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusOptionalAggregate::ChoiceAlias", /* Name */
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
                    }}; /* Type code for  ChoiceAlias */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ChoiceAlias_g_tc;
                }

                ChoiceAlias_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ChoiceAlias_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::Choice>::get().native();

                /* Initialize the values for member annotations. */

                ChoiceAlias_g_tc._data._sampleAccessInfo = sample_access_info();
                ChoiceAlias_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &ChoiceAlias_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ChoiceAlias_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ChoiceAlias_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ChoiceAlias_g_sampleAccessInfo;
                }

                ChoiceAlias_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ChoiceAlias_g_sampleAccessInfo.memberAccessInfos = 
                ChoiceAlias_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalAggregate::ChoiceAlias);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ChoiceAlias_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ChoiceAlias_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ChoiceAlias_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ChoiceAlias_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalAggregate::ChoiceAlias >;

                ChoiceAlias_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ChoiceAlias_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ChoiceAlias_g_typePlugin = 
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

                return &ChoiceAlias_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusOptionalAggregate::ChoiceAlias_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalAggregate::ChoiceAlias_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusOptionalAggregate::Holder > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Holder_g_tc_members[4]=
                {

                    {
                        (char *)"payload",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"payloadAlias",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"choice",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"choiceAlias",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Holder_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusOptionalAggregate::Holder", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        Holder_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Holder*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Holder_g_tc;
                }

                Holder_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Holder_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::Payload>::get().native();
                Holder_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::PayloadAlias_AliasTag_t>::get().native();
                Holder_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::Choice>::get().native();
                Holder_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusOptionalAggregate::ChoiceAlias_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                Holder_g_tc._data._sampleAccessInfo = sample_access_info();
                Holder_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Holder_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusOptionalAggregate::Holder *sample;

                static RTIXCdrMemberAccessInfo Holder_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Holder_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Holder_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusOptionalAggregate::Holder);
                if (sample == NULL) {
                    return NULL;
                }

                Holder_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->payload() - (char *)sample);

                Holder_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->payloadAlias() - (char *)sample);

                Holder_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->choice() - (char *)sample);

                Holder_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->choiceAlias() - (char *)sample);

                Holder_g_sampleAccessInfo.memberAccessInfos = 
                Holder_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusOptionalAggregate::Holder);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Holder_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Holder_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Holder_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Holder_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusOptionalAggregate::Holder >;

                Holder_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Holder_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Holder_g_typePlugin = 
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

                return &Holder_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusOptionalAggregate::Holder >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusOptionalAggregate::Holder >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusOptionalAggregate::Payload >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusOptionalAggregate::PayloadPlugin_new,
                ::CorpusOptionalAggregate::PayloadPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusOptionalAggregate::Payload >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusOptionalAggregate::Payload& sample,
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

        void topic_type_support< ::CorpusOptionalAggregate::Payload >::from_cdr_buffer(::CorpusOptionalAggregate::Payload& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = PayloadPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusOptionalAggregate::Payload from cdr buffer");
        }

        void topic_type_support< ::CorpusOptionalAggregate::Payload >::reset_sample(::CorpusOptionalAggregate::Payload& sample) 
        {
            sample.value(0);
        }

        void topic_type_support< ::CorpusOptionalAggregate::Payload >::allocate_sample(::CorpusOptionalAggregate::Payload& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusOptionalAggregate::Choice >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusOptionalAggregate::ChoicePlugin_new,
                ::CorpusOptionalAggregate::ChoicePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusOptionalAggregate::Choice >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusOptionalAggregate::Choice& sample,
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

        void topic_type_support< ::CorpusOptionalAggregate::Choice >::from_cdr_buffer(::CorpusOptionalAggregate::Choice& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChoicePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusOptionalAggregate::Choice from cdr buffer");
        }

        void topic_type_support< ::CorpusOptionalAggregate::Choice >::reset_sample(::CorpusOptionalAggregate::Choice& sample) 
        {
            sample._d() = 0;
            ::rti::topic::reset_sample(sample.payload());
            sample._d() = 1;
            sample.number(0);

            sample._d() = ::CorpusOptionalAggregate::Choice::default_discriminator();
        }

        void topic_type_support< ::CorpusOptionalAggregate::Choice >::allocate_sample(::CorpusOptionalAggregate::Choice& sample, int, int) 
        {
            sample._d() = 0;
            ::rti::topic::allocate_sample(sample.payload(),  -1, -1);

            sample._d() = ::CorpusOptionalAggregate::Choice::default_discriminator();
        }
        void topic_type_support< ::CorpusOptionalAggregate::Holder >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusOptionalAggregate::HolderPlugin_new,
                ::CorpusOptionalAggregate::HolderPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusOptionalAggregate::Holder >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusOptionalAggregate::Holder& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = HolderPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = HolderPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusOptionalAggregate::Holder >::from_cdr_buffer(::CorpusOptionalAggregate::Holder& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = HolderPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusOptionalAggregate::Holder from cdr buffer");
        }

        void topic_type_support< ::CorpusOptionalAggregate::Holder >::reset_sample(::CorpusOptionalAggregate::Holder& sample) 
        {
            ::rti::topic::reset_sample(sample.payload());
            ::rti::topic::reset_sample(sample.payloadAlias());
            ::rti::topic::reset_sample(sample.choice());
            ::rti::topic::reset_sample(sample.choiceAlias());
        }

        void topic_type_support< ::CorpusOptionalAggregate::Holder >::allocate_sample(::CorpusOptionalAggregate::Holder& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
