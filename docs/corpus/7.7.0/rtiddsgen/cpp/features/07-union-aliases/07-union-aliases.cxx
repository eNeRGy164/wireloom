

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-aliases.idl
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
#include "07-union-aliasesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-aliases.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusUnionAliases {
    std::ostream& operator << (std::ostream& o,const Kind& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case Kind::Zero:
            o << "Kind::Zero" << " ";
            break;
            case Kind::Ten:
            o << "Kind::Ten" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }

    // ---- Choice:

    Choice::Union_::Union_() :
        zero (0) ,
        ten ("")  {
    }

    Choice::Union_::Union_(int32_t zero_,const ::omg::types::string_view& ten_):
        zero(zero_), 
        ten(ten_) {
    }
    Choice::Choice() :m_d_(default_discriminator())
    {
    }

    void Choice::swap(Choice& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case (CorpusUnionAliases::Kind::Zero):
                {  
                    swap(m_u_.zero, other_.m_u_.zero);
            } break;
            case (CorpusUnionAliases::Kind::Ten):
                {  
                    swap(m_u_.ten, other_.m_u_.ten);
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
                case (CorpusUnionAliases::Kind::Zero):
                    {  
                        swap(m_u_.zero, other_.m_u_.zero);
                } break;
                case (CorpusUnionAliases::Kind::Ten):
                    {  
                        swap(m_u_.ten, other_.m_u_.ten);
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
            case (CorpusUnionAliases::Kind::Zero):
                {  
                    if (m_u_.zero != other_.m_u_.zero) {
                        return false;
                }
            } break ;
            case (CorpusUnionAliases::Kind::Ten):
                {  
                    if (m_u_.ten != other_.m_u_.ten) {
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
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case (CorpusUnionAliases::Kind::Zero):
                {  
                    o << "zero: " << sample.zero()<<", ";
            } break ;
            case (CorpusUnionAliases::Kind::Ten):
                {  
                    o << "ten: " << sample.ten();
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

    ::CorpusUnionAliases::Kind Choice::default_discriminator() {
        return ::CorpusUnionAliases::Kind(static_cast< ::CorpusUnionAliases::Kind >(0));
    }

    // ---- Holder:

    Holder::Holder()  {

    }

    Holder::Holder (const ::CorpusUnionAliases::Choice& value_) {
        value = value_;
    }

    bool operator == (const Holder& a, const Holder& b) {

        if (a.value != b.value) {
            return false;
        }

        return true;
    }

    bool operator != (const Holder& a, const Holder& b) {
        return !operator ==(a, b);
    }

    void swap(Holder& a, Holder& b) noexcept
    {
        using std::swap;

        swap(a.value, b.value);

    }  
    std::ostream& operator << (std::ostream& o,const Holder& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value;
        o <<"]";
        return o;
    }

} // namespace CorpusUnionAliases  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusUnionAliases::Kind default_enumerator< ::CorpusUnionAliases::Kind>::value = ::CorpusUnionAliases::Kind::Zero;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusUnionAliases::Kind default_enumerator< ::CorpusUnionAliases::Kind>::value = ::CorpusUnionAliases::Kind::Zero;

        template<>
        struct native_type_code< ::CorpusUnionAliases::Kind > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Kind_g_tc_members[2]=
                {

                    {
                        (char *)"Zero",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusUnionAliases::Kind::Zero),
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"Ten",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusUnionAliases::Kind::Ten),
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Kind_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnionAliases::Kind", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
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
                    size_t candidateTypeSize = sizeof(::CorpusUnionAliases::Kind);

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
                interpreter::get_aggregation_value_pointer< ::CorpusUnionAliases::Kind >;

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

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusUnionAliases::Kind >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnionAliases::Kind >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnionAliases::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Choice_g_tc_ten_string;

                static DDS_TypeCode_Member Choice_g_tc_members[2]=
                {

                    {
                        (char *)"zero",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((CorpusUnionAliases::Kind::Zero)),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"ten",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((CorpusUnionAliases::Kind::Ten)),
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
                        -1, /*Ignored*/
                        (char *)"CorpusUnionAliases::Choice", /* Name */
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

                Choice_g_tc_ten_string = initialize_string_typecode((255L));

                Choice_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Choice_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Choice_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Choice_g_tc_ten_string;

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
                Choice_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnionAliases::Kind>::get().native();

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnionAliases::Choice *sample;

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusUnionAliases::Choice);
                if (sample == NULL) {
                    return NULL;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d((CorpusUnionAliases::Kind::Zero));
                Choice_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->zero() - (char *)sample);

                sample->_d((CorpusUnionAliases::Kind::Ten));
                Choice_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->ten() - (char *)sample);

                Choice_g_sampleAccessInfo.memberAccessInfos =
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnionAliases::Choice);

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
                interpreter::get_aggregation_value_pointer< ::CorpusUnionAliases::Choice >;

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

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusUnionAliases::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnionAliases::Choice >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnionAliases::ChoiceAlias_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ChoiceAlias_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnionAliases::ChoiceAlias", /* Name */
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

                ChoiceAlias_g_tc._data._typeCode =   (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnionAliases::Choice>::get().native();

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
                    size_t candidateTypeSize = sizeof(::CorpusUnionAliases::ChoiceAlias);

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
                interpreter::get_aggregation_value_pointer< ::CorpusUnionAliases::ChoiceAlias >;

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

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnionAliases::ChoiceAlias_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnionAliases::ChoiceAlias_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnionAliases::ChoiceAlias2_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ChoiceAlias2_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnionAliases::ChoiceAlias2", /* Name */
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
                    }}; /* Type code for  ChoiceAlias2 */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ChoiceAlias2_g_tc;
                }

                ChoiceAlias2_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ChoiceAlias2_g_tc._data._typeCode =   (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnionAliases::ChoiceAlias_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                ChoiceAlias2_g_tc._data._sampleAccessInfo = sample_access_info();
                ChoiceAlias2_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &ChoiceAlias2_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ChoiceAlias2_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ChoiceAlias2_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ChoiceAlias2_g_sampleAccessInfo;
                }

                ChoiceAlias2_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ChoiceAlias2_g_sampleAccessInfo.memberAccessInfos =
                ChoiceAlias2_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnionAliases::ChoiceAlias2);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ChoiceAlias2_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ChoiceAlias2_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ChoiceAlias2_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ChoiceAlias2_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusUnionAliases::ChoiceAlias2 >;

                ChoiceAlias2_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ChoiceAlias2_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ChoiceAlias2_g_typePlugin =
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

                return &ChoiceAlias2_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnionAliases::ChoiceAlias2_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnionAliases::ChoiceAlias2_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnionAliases::Holder > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Holder_g_tc_members[1]=
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Holder_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnionAliases::Holder", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
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

                Holder_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnionAliases::ChoiceAlias2_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                Holder_g_tc._data._sampleAccessInfo = sample_access_info();
                Holder_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Holder_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnionAliases::Holder *sample;

                static RTIXCdrMemberAccessInfo Holder_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Holder_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Holder_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusUnionAliases::Holder);
                if (sample == NULL) {
                    return NULL;
                }

                Holder_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                Holder_g_sampleAccessInfo.memberAccessInfos =
                Holder_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnionAliases::Holder);

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
                interpreter::get_aggregation_value_pointer< ::CorpusUnionAliases::Holder >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnionAliases::Holder >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnionAliases::Holder >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnionAliases::Choice >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnionAliases::ChoicePlugin_new,
                ::CorpusUnionAliases::ChoicePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnionAliases::Choice >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnionAliases::Choice& sample,
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

        void topic_type_support< ::CorpusUnionAliases::Choice >::from_cdr_buffer(::CorpusUnionAliases::Choice& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChoicePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnionAliases::Choice from cdr buffer");
        }

        void topic_type_support< ::CorpusUnionAliases::Choice >::reset_sample(::CorpusUnionAliases::Choice& sample) 
        {
            sample._d((CorpusUnionAliases::Kind::Zero));
            sample.zero(0);
            sample._d((CorpusUnionAliases::Kind::Ten));
            sample.ten("");

            sample._d(::CorpusUnionAliases::Choice::default_discriminator());
        }

        void topic_type_support< ::CorpusUnionAliases::Choice >::allocate_sample(::CorpusUnionAliases::Choice& sample, int, int) 
        {
            sample._d((CorpusUnionAliases::Kind::Ten));
            ::rti::topic::allocate_sample(sample.ten(),  -1, 255L);

            sample._d(::CorpusUnionAliases::Choice::default_discriminator());
        }
        void topic_type_support< ::CorpusUnionAliases::Holder >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnionAliases::HolderPlugin_new,
                ::CorpusUnionAliases::HolderPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnionAliases::Holder >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnionAliases::Holder& sample,
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

        void topic_type_support< ::CorpusUnionAliases::Holder >::from_cdr_buffer(::CorpusUnionAliases::Holder& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = HolderPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnionAliases::Holder from cdr buffer");
        }

        void topic_type_support< ::CorpusUnionAliases::Holder >::reset_sample(::CorpusUnionAliases::Holder& sample) 
        {
            ::rti::topic::reset_sample(sample.value);
        }

        void topic_type_support< ::CorpusUnionAliases::Holder >::allocate_sample(::CorpusUnionAliases::Holder& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.value,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
