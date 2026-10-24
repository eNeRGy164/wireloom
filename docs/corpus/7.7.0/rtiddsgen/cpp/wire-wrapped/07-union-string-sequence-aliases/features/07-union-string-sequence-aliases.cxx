

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-aliases.idl
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
#include "07-union-string-sequence-aliasesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-string-sequence-aliases.hpp"

#include <rti/util/ostream_operators.hpp>

namespace RuntimeUnionStringSequences {

    // ---- DirectStringSequences:

    DirectStringSequences::Union_::Union_()  {
    }

    DirectStringSequences::Union_::Union_(const ::omg::types::bounded_sequence< std::string, 3L >& narrow_,const ::omg::types::bounded_sequence< std::wstring, 3L >& wide_):
        narrow(narrow_), 
        wide(wide_) {
    }
    DirectStringSequences::DirectStringSequences() :m_d_(default_discriminator())
    {
    }

    void DirectStringSequences::swap(DirectStringSequences& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.narrow, other_.m_u_.narrow);
            } break;
            case 1:
            {  
                swap(m_u_.wide, other_.m_u_.wide);
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
                    swap(m_u_.narrow, other_.m_u_.narrow);
                } break;
                case 1:
                {  
                    swap(m_u_.wide, other_.m_u_.wide);
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

    bool DirectStringSequences::operator == (const DirectStringSequences& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.narrow != other_.m_u_.narrow) {
                    return false;
                }
            } break ;
            case 1:
            {  
                if (m_u_.wide != other_.m_u_.wide) {
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

    bool DirectStringSequences::operator != (const DirectStringSequences& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const DirectStringSequences& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "narrow: " << sample.narrow()<<", ";
            } break ;
            case 1:
            {  
                o << "wide: " << sample.wide();
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

    int32_t DirectStringSequences::default_discriminator() {
        return 0;
    }

    // ---- StringSequenceAliases:

    StringSequenceAliases::Union_::Union_()  {
    }

    StringSequenceAliases::Union_::Union_(const ::RuntimeUnionStringSequences::NarrowTexts& narrow_,const ::RuntimeUnionStringSequences::WideTexts& wide_,const ::RuntimeUnionStringSequences::BoundedNarrowTexts& boundedNarrow_,const ::RuntimeUnionStringSequences::BoundedWideTexts& boundedWide_):
        narrow(narrow_), 
        wide(wide_), 
        boundedNarrow(boundedNarrow_), 
        boundedWide(boundedWide_) {
    }
    StringSequenceAliases::StringSequenceAliases() :m_d_(default_discriminator())
    {
    }

    void StringSequenceAliases::swap(StringSequenceAliases& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.narrow, other_.m_u_.narrow);
            } break;
            case 1:
            {  
                swap(m_u_.wide, other_.m_u_.wide);
            } break;
            case 2:
            {  
                swap(m_u_.boundedNarrow, other_.m_u_.boundedNarrow);
            } break;
            case 3:
            {  
                swap(m_u_.boundedWide, other_.m_u_.boundedWide);
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
                    swap(m_u_.narrow, other_.m_u_.narrow);
                } break;
                case 1:
                {  
                    swap(m_u_.wide, other_.m_u_.wide);
                } break;
                case 2:
                {  
                    swap(m_u_.boundedNarrow, other_.m_u_.boundedNarrow);
                } break;
                case 3:
                {  
                    swap(m_u_.boundedWide, other_.m_u_.boundedWide);
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

    bool StringSequenceAliases::operator == (const StringSequenceAliases& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.narrow != other_.m_u_.narrow) {
                    return false;
                }
            } break ;
            case 1:
            {  
                if (m_u_.wide != other_.m_u_.wide) {
                    return false;
                }
            } break ;
            case 2:
            {  
                if (m_u_.boundedNarrow != other_.m_u_.boundedNarrow) {
                    return false;
                }
            } break ;
            case 3:
            {  
                if (m_u_.boundedWide != other_.m_u_.boundedWide) {
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

    bool StringSequenceAliases::operator != (const StringSequenceAliases& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const StringSequenceAliases& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "narrow: " << sample.narrow()<<", ";
            } break ;
            case 1:
            {  
                o << "wide: " << sample.wide()<<", ";
            } break ;
            case 2:
            {  
                o << "boundedNarrow: " << sample.boundedNarrow()<<", ";
            } break ;
            case 3:
            {  
                o << "boundedWide: " << sample.boundedWide();
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

    int32_t StringSequenceAliases::default_discriminator() {
        return 0;
    }

} // namespace RuntimeUnionStringSequences  

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
        struct native_type_code< ::RuntimeUnionStringSequences::NarrowTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode NarrowTexts_g_tc_string;
                static DDS_TypeCode NarrowTexts_g_tc_sequence;

                static DDS_TypeCode NarrowTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"RuntimeUnionStringSequences::NarrowTexts", /* Name */
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
                    }}; /* Type code for  NarrowTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &NarrowTexts_g_tc;
                }

                NarrowTexts_g_tc_string = initialize_string_typecode((255L));
                NarrowTexts_g_tc_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::string, 3L > >((3L));

                NarrowTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NarrowTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&NarrowTexts_g_tc_string;
                NarrowTexts_g_tc._data._typeCode =   (RTICdrTypeCode *)& NarrowTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                NarrowTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                NarrowTexts_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &NarrowTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo NarrowTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NarrowTexts_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &NarrowTexts_g_sampleAccessInfo;
                }

                NarrowTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                NarrowTexts_g_sampleAccessInfo.memberAccessInfos =
                NarrowTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionStringSequences::NarrowTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NarrowTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NarrowTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NarrowTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NarrowTexts_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionStringSequences::NarrowTexts >;

                NarrowTexts_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &NarrowTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NarrowTexts_g_typePlugin =
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

                return &NarrowTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::RuntimeUnionStringSequences::NarrowTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionStringSequences::NarrowTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::RuntimeUnionStringSequences::WideTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode WideTexts_g_tc_string;
                static DDS_TypeCode WideTexts_g_tc_sequence;

                static DDS_TypeCode WideTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"RuntimeUnionStringSequences::WideTexts", /* Name */
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
                    }}; /* Type code for  WideTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &WideTexts_g_tc;
                }

                WideTexts_g_tc_string = initialize_wstring_typecode((255L));
                WideTexts_g_tc_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::wstring, 3L > >((3L));

                WideTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                WideTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&WideTexts_g_tc_string;
                WideTexts_g_tc._data._typeCode =   (RTICdrTypeCode *)& WideTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                WideTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                WideTexts_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &WideTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo WideTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo WideTexts_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &WideTexts_g_sampleAccessInfo;
                }

                WideTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                WideTexts_g_sampleAccessInfo.memberAccessInfos =
                WideTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionStringSequences::WideTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        WideTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        WideTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                WideTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                WideTexts_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionStringSequences::WideTexts >;

                WideTexts_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &WideTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin WideTexts_g_typePlugin =
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

                return &WideTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::RuntimeUnionStringSequences::WideTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionStringSequences::WideTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::RuntimeUnionStringSequences::BoundedNarrowTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedNarrowTexts_g_tc_string;
                static DDS_TypeCode BoundedNarrowTexts_g_tc_sequence;

                static DDS_TypeCode BoundedNarrowTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"RuntimeUnionStringSequences::BoundedNarrowTexts", /* Name */
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
                    }}; /* Type code for  BoundedNarrowTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedNarrowTexts_g_tc;
                }

                BoundedNarrowTexts_g_tc_string = initialize_string_typecode((8L));
                BoundedNarrowTexts_g_tc_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::string, 3L > >((3L));

                BoundedNarrowTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedNarrowTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&BoundedNarrowTexts_g_tc_string;
                BoundedNarrowTexts_g_tc._data._typeCode =   (RTICdrTypeCode *)& BoundedNarrowTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                BoundedNarrowTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedNarrowTexts_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &BoundedNarrowTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedNarrowTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedNarrowTexts_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedNarrowTexts_g_sampleAccessInfo;
                }

                BoundedNarrowTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedNarrowTexts_g_sampleAccessInfo.memberAccessInfos =
                BoundedNarrowTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionStringSequences::BoundedNarrowTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedNarrowTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedNarrowTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedNarrowTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedNarrowTexts_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionStringSequences::BoundedNarrowTexts >;

                BoundedNarrowTexts_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedNarrowTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedNarrowTexts_g_typePlugin =
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

                return &BoundedNarrowTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::RuntimeUnionStringSequences::BoundedNarrowTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionStringSequences::BoundedNarrowTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::RuntimeUnionStringSequences::BoundedWideTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedWideTexts_g_tc_string;
                static DDS_TypeCode BoundedWideTexts_g_tc_sequence;

                static DDS_TypeCode BoundedWideTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"RuntimeUnionStringSequences::BoundedWideTexts", /* Name */
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
                    }}; /* Type code for  BoundedWideTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedWideTexts_g_tc;
                }

                BoundedWideTexts_g_tc_string = initialize_wstring_typecode((4L));
                BoundedWideTexts_g_tc_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::wstring, 3L > >((3L));

                BoundedWideTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedWideTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&BoundedWideTexts_g_tc_string;
                BoundedWideTexts_g_tc._data._typeCode =   (RTICdrTypeCode *)& BoundedWideTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                BoundedWideTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedWideTexts_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &BoundedWideTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedWideTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedWideTexts_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedWideTexts_g_sampleAccessInfo;
                }

                BoundedWideTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedWideTexts_g_sampleAccessInfo.memberAccessInfos =
                BoundedWideTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionStringSequences::BoundedWideTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedWideTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedWideTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedWideTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedWideTexts_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionStringSequences::BoundedWideTexts >;

                BoundedWideTexts_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedWideTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedWideTexts_g_typePlugin =
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

                return &BoundedWideTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::RuntimeUnionStringSequences::BoundedWideTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionStringSequences::BoundedWideTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::RuntimeUnionStringSequences::DirectStringSequences > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode DirectStringSequences_g_tc_narrow_string;
                static DDS_TypeCode DirectStringSequences_g_tc_narrow_sequence;
                static DDS_TypeCode DirectStringSequences_g_tc_wide_string;
                static DDS_TypeCode DirectStringSequences_g_tc_wide_sequence;

                static DDS_TypeCode_Member DirectStringSequences_g_tc_members[2]=
                {

                    {
                        (char *)"narrow",/* Member name */
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
                        (char *)"wide",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode DirectStringSequences_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"RuntimeUnionStringSequences::DirectStringSequences", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        DirectStringSequences_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for DirectStringSequences*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &DirectStringSequences_g_tc;
                }

                DirectStringSequences_g_tc_narrow_string = initialize_string_typecode((255L));
                DirectStringSequences_g_tc_narrow_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::string, 3L > >((3L));
                DirectStringSequences_g_tc_wide_string = initialize_wstring_typecode((255L));
                DirectStringSequences_g_tc_wide_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< std::wstring, 3L > >((3L));

                DirectStringSequences_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                DirectStringSequences_g_tc_narrow_sequence._data._typeCode = (RTICdrTypeCode *)&DirectStringSequences_g_tc_narrow_string;
                DirectStringSequences_g_tc_wide_sequence._data._typeCode = (RTICdrTypeCode *)&DirectStringSequences_g_tc_wide_string;
                DirectStringSequences_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)& DirectStringSequences_g_tc_narrow_sequence;
                DirectStringSequences_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)& DirectStringSequences_g_tc_wide_sequence;

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                DirectStringSequences_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                DirectStringSequences_g_tc._data._sampleAccessInfo = sample_access_info();
                DirectStringSequences_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &DirectStringSequences_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::RuntimeUnionStringSequences::DirectStringSequences *sample;

                static RTIXCdrMemberAccessInfo DirectStringSequences_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo DirectStringSequences_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &DirectStringSequences_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::RuntimeUnionStringSequences::DirectStringSequences);
                if (sample == NULL) {
                    return NULL;
                }

                DirectStringSequences_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(0);
                DirectStringSequences_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->narrow() - (char *)sample);

                sample->_d(1);
                DirectStringSequences_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wide() - (char *)sample);

                DirectStringSequences_g_sampleAccessInfo.memberAccessInfos =
                DirectStringSequences_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionStringSequences::DirectStringSequences);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        DirectStringSequences_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        DirectStringSequences_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                DirectStringSequences_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                DirectStringSequences_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionStringSequences::DirectStringSequences >;

                DirectStringSequences_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &DirectStringSequences_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin DirectStringSequences_g_typePlugin =
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

                return &DirectStringSequences_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::RuntimeUnionStringSequences::DirectStringSequences >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionStringSequences::DirectStringSequences >::get())));
        }

        template<>
        struct native_type_code< ::RuntimeUnionStringSequences::StringSequenceAliases > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member StringSequenceAliases_g_tc_members[4]=
                {

                    {
                        (char *)"narrow",/* Member name */
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
                        (char *)"wide",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"boundedNarrow",/* Member name */
                        {
                            3,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(2),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"boundedWide",/* Member name */
                        {
                            4,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(3),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode StringSequenceAliases_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"RuntimeUnionStringSequences::StringSequenceAliases", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        StringSequenceAliases_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for StringSequenceAliases*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &StringSequenceAliases_g_tc;
                }

                StringSequenceAliases_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                StringSequenceAliases_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::RuntimeUnionStringSequences::NarrowTexts_AliasTag_t>::get().native();
                StringSequenceAliases_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::RuntimeUnionStringSequences::WideTexts_AliasTag_t>::get().native();
                StringSequenceAliases_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::RuntimeUnionStringSequences::BoundedNarrowTexts_AliasTag_t>::get().native();
                StringSequenceAliases_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::RuntimeUnionStringSequences::BoundedWideTexts_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                StringSequenceAliases_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                StringSequenceAliases_g_tc._data._sampleAccessInfo = sample_access_info();
                StringSequenceAliases_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &StringSequenceAliases_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::RuntimeUnionStringSequences::StringSequenceAliases *sample;

                static RTIXCdrMemberAccessInfo StringSequenceAliases_g_memberAccessInfos[5] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo StringSequenceAliases_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &StringSequenceAliases_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::RuntimeUnionStringSequences::StringSequenceAliases);
                if (sample == NULL) {
                    return NULL;
                }

                StringSequenceAliases_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(0);
                StringSequenceAliases_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->narrow() - (char *)sample);

                sample->_d(1);
                StringSequenceAliases_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->wide() - (char *)sample);

                sample->_d(2);
                StringSequenceAliases_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedNarrow() - (char *)sample);

                sample->_d(3);
                StringSequenceAliases_g_memberAccessInfos[4].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedWide() - (char *)sample);

                StringSequenceAliases_g_sampleAccessInfo.memberAccessInfos =
                StringSequenceAliases_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionStringSequences::StringSequenceAliases);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        StringSequenceAliases_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        StringSequenceAliases_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                StringSequenceAliases_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                StringSequenceAliases_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionStringSequences::StringSequenceAliases >;

                StringSequenceAliases_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &StringSequenceAliases_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin StringSequenceAliases_g_typePlugin =
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

                return &StringSequenceAliases_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::RuntimeUnionStringSequences::StringSequenceAliases >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionStringSequences::StringSequenceAliases >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::RuntimeUnionStringSequences::DirectStringSequences >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::RuntimeUnionStringSequences::DirectStringSequencesPlugin_new,
                ::RuntimeUnionStringSequences::DirectStringSequencesPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::RuntimeUnionStringSequences::DirectStringSequences >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::RuntimeUnionStringSequences::DirectStringSequences& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = DirectStringSequencesPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = DirectStringSequencesPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::RuntimeUnionStringSequences::DirectStringSequences >::from_cdr_buffer(::RuntimeUnionStringSequences::DirectStringSequences& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DirectStringSequencesPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::RuntimeUnionStringSequences::DirectStringSequences from cdr buffer");
        }

        void topic_type_support< ::RuntimeUnionStringSequences::DirectStringSequences >::reset_sample(::RuntimeUnionStringSequences::DirectStringSequences& sample) 
        {
            sample._d(0);
            ::rti::topic::reset_sample(sample.narrow());
            sample._d(1);
            ::rti::topic::reset_sample(sample.wide());

            sample._d(::RuntimeUnionStringSequences::DirectStringSequences::default_discriminator());
        }

        void topic_type_support< ::RuntimeUnionStringSequences::DirectStringSequences >::allocate_sample(::RuntimeUnionStringSequences::DirectStringSequences& sample, int, int) 
        {
            sample._d(0);
            ::rti::topic::allocate_sample(sample.narrow(),  3L, 255L);
            sample._d(1);
            ::rti::topic::allocate_sample(sample.wide(),  3L, 255L);

            sample._d(::RuntimeUnionStringSequences::DirectStringSequences::default_discriminator());
        }
        void topic_type_support< ::RuntimeUnionStringSequences::StringSequenceAliases >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::RuntimeUnionStringSequences::StringSequenceAliasesPlugin_new,
                ::RuntimeUnionStringSequences::StringSequenceAliasesPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::RuntimeUnionStringSequences::StringSequenceAliases >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::RuntimeUnionStringSequences::StringSequenceAliases& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = StringSequenceAliasesPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = StringSequenceAliasesPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::RuntimeUnionStringSequences::StringSequenceAliases >::from_cdr_buffer(::RuntimeUnionStringSequences::StringSequenceAliases& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = StringSequenceAliasesPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::RuntimeUnionStringSequences::StringSequenceAliases from cdr buffer");
        }

        void topic_type_support< ::RuntimeUnionStringSequences::StringSequenceAliases >::reset_sample(::RuntimeUnionStringSequences::StringSequenceAliases& sample) 
        {
            sample._d(0);
            ::rti::topic::reset_sample(sample.narrow());
            sample._d(1);
            ::rti::topic::reset_sample(sample.wide());
            sample._d(2);
            ::rti::topic::reset_sample(sample.boundedNarrow());
            sample._d(3);
            ::rti::topic::reset_sample(sample.boundedWide());

            sample._d(::RuntimeUnionStringSequences::StringSequenceAliases::default_discriminator());
        }

        void topic_type_support< ::RuntimeUnionStringSequences::StringSequenceAliases >::allocate_sample(::RuntimeUnionStringSequences::StringSequenceAliases& sample, int, int) 
        {
            sample._d(0);
            ::rti::topic::allocate_sample(sample.narrow(),  -1, -1);
            sample._d(1);
            ::rti::topic::allocate_sample(sample.wide(),  -1, -1);
            sample._d(2);
            ::rti::topic::allocate_sample(sample.boundedNarrow(),  -1, -1);
            sample._d(3);
            ::rti::topic::allocate_sample(sample.boundedWide(),  -1, -1);

            sample._d(::RuntimeUnionStringSequences::StringSequenceAliases::default_discriminator());
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
