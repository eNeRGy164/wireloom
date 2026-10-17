

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enums-aliases.idl
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
#include "03-enums-aliasesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "03-enums-aliases.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusEnums {

    std::ostream& operator << (std::ostream& o,const Color& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case Color::RED:
            o << "Color::RED" << " ";
            break;
            case Color::GREEN:
            o << "Color::GREEN" << " ";
            break;
            case Color::BLUE:
            o << "Color::BLUE" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }

    // ---- Record: 

    Record::Record() :
        m_value_ (0) ,
        m_aliasValue_ (0) ,
        m_color_(CorpusEnums::Color::RED) ,
        m_aliasColor_(CorpusEnums::Color::RED)  {

    }   

    Record::Record (int32_t value_,int32_t aliasValue_,const ::CorpusEnums::Color& color_,const ::CorpusEnums::Color& aliasColor_,const ::CorpusEnums::ColorSequence& colors_):
        m_value_(value_), 
        m_aliasValue_(aliasValue_), 
        m_color_(color_), 
        m_aliasColor_(aliasColor_), 
        m_colors_(colors_) {
    }

    void Record::swap(Record& other_)  noexcept 
    {
        using std::swap;
        swap(m_value_, other_.m_value_);
        swap(m_aliasValue_, other_.m_aliasValue_);
        swap(m_color_, other_.m_color_);
        swap(m_aliasColor_, other_.m_aliasColor_);
        swap(m_colors_, other_.m_colors_);
    }  

    bool Record::operator == (const Record& other_) const {
        if (m_value_ != other_.m_value_) {
            return false;
        }
        if (m_aliasValue_ != other_.m_aliasValue_) {
            return false;
        }
        if (m_color_ != other_.m_color_) {
            return false;
        }
        if (m_aliasColor_ != other_.m_aliasColor_) {
            return false;
        }
        if (m_colors_ != other_.m_colors_) {
            return false;
        }
        return true;
    }

    bool Record::operator != (const Record& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Record& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value ()<<", ";
        o << "aliasValue: " << sample.aliasValue ()<<", ";
        o << "color: " << sample.color ()<<", ";
        o << "aliasColor: " << sample.aliasColor ()<<", ";
        o << "colors: " << sample.colors ();
        o <<"]";
        return o;
    }

} // namespace CorpusEnums  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusEnums::Color default_enumerator< ::CorpusEnums::Color>::value = ::CorpusEnums::Color::RED;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusEnums::Color default_enumerator< ::CorpusEnums::Color>::value = ::CorpusEnums::Color::RED;
        template<>
        struct native_type_code< ::CorpusEnums::Color > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Color_g_tc_members[3]=
                {

                    {
                        (char *)"RED",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnums::Color::RED), 
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
                        (char *)"GREEN",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnums::Color::GREEN), 
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
                        (char *)"BLUE",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnums::Color::BLUE), 
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

                static DDS_TypeCode Color_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusEnums::Color", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        Color_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Color*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Color_g_tc;
                }

                Color_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                /* Initialize the values for annotations. */
                Color_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Color_g_tc._data._annotations._defaultValue._u.long_value = 0;

                Color_g_tc._data._sampleAccessInfo = sample_access_info();
                Color_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Color_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Color_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Color_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Color_g_sampleAccessInfo;
                }

                Color_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Color_g_sampleAccessInfo.memberAccessInfos = 
                Color_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::Color);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Color_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Color_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Color_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Color_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::Color >;

                Color_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Color_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Color_g_typePlugin = 
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

                return &Color_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusEnums::Color >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::Color >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnums::Scalar_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Scalar_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusEnums::Scalar", /* Name */
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
                    }}; /* Type code for  Scalar */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Scalar_g_tc;
                }

                Scalar_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Scalar_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Scalar_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Scalar_g_tc._data._annotations._defaultValue._u.long_value = 0;
                Scalar_g_tc._data._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Scalar_g_tc._data._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Scalar_g_tc._data._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Scalar_g_tc._data._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Scalar_g_tc._data._sampleAccessInfo = sample_access_info();
                Scalar_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Scalar_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Scalar_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Scalar_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Scalar_g_sampleAccessInfo;
                }

                Scalar_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Scalar_g_sampleAccessInfo.memberAccessInfos = 
                Scalar_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::Scalar);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Scalar_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Scalar_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Scalar_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Scalar_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::Scalar >;

                Scalar_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Scalar_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Scalar_g_typePlugin = 
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

                return &Scalar_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusEnums::Scalar_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::Scalar_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnums::ColorSequence_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ColorSequence_g_tc_sequence;

                static DDS_TypeCode ColorSequence_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusEnums::ColorSequence", /* Name */
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
                    }}; /* Type code for  ColorSequence */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ColorSequence_g_tc;
                }

                ColorSequence_g_tc_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusEnums::Color, 3L > >((3L));

                ColorSequence_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ColorSequence_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::Color>::get().native();
                ColorSequence_g_tc._data._typeCode =  (RTICdrTypeCode *)& ColorSequence_g_tc_sequence;

                /* Initialize the values for member annotations. */

                ColorSequence_g_tc._data._sampleAccessInfo = sample_access_info();
                ColorSequence_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &ColorSequence_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ColorSequence_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ColorSequence_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ColorSequence_g_sampleAccessInfo;
                }

                ColorSequence_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ColorSequence_g_sampleAccessInfo.memberAccessInfos = 
                ColorSequence_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::ColorSequence);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ColorSequence_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ColorSequence_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ColorSequence_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ColorSequence_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::ColorSequence >;

                ColorSequence_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ColorSequence_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ColorSequence_g_typePlugin = 
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

                return &ColorSequence_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusEnums::ColorSequence_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::ColorSequence_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnums::ScalarAlias_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ScalarAlias_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusEnums::ScalarAlias", /* Name */
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
                    }}; /* Type code for  ScalarAlias */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ScalarAlias_g_tc;
                }

                ScalarAlias_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ScalarAlias_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::Scalar_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                ScalarAlias_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ScalarAlias_g_tc._data._annotations._defaultValue._u.long_value = 0;
                ScalarAlias_g_tc._data._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ScalarAlias_g_tc._data._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ScalarAlias_g_tc._data._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ScalarAlias_g_tc._data._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ScalarAlias_g_tc._data._sampleAccessInfo = sample_access_info();
                ScalarAlias_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &ScalarAlias_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ScalarAlias_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ScalarAlias_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ScalarAlias_g_sampleAccessInfo;
                }

                ScalarAlias_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ScalarAlias_g_sampleAccessInfo.memberAccessInfos = 
                ScalarAlias_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::ScalarAlias);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ScalarAlias_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ScalarAlias_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ScalarAlias_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ScalarAlias_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::ScalarAlias >;

                ScalarAlias_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ScalarAlias_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ScalarAlias_g_typePlugin = 
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

                return &ScalarAlias_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusEnums::ScalarAlias_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::ScalarAlias_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnums::ColorAlias_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ColorAlias_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusEnums::ColorAlias", /* Name */
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
                    }}; /* Type code for  ColorAlias */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ColorAlias_g_tc;
                }

                ColorAlias_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ColorAlias_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::Color>::get().native();

                /* Initialize the values for member annotations. */
                ColorAlias_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                ColorAlias_g_tc._data._annotations._defaultValue._u.enumerated_value = 0;

                ColorAlias_g_tc._data._sampleAccessInfo = sample_access_info();
                ColorAlias_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &ColorAlias_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ColorAlias_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ColorAlias_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ColorAlias_g_sampleAccessInfo;
                }

                ColorAlias_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ColorAlias_g_sampleAccessInfo.memberAccessInfos = 
                ColorAlias_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::ColorAlias);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ColorAlias_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ColorAlias_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ColorAlias_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ColorAlias_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::ColorAlias >;

                ColorAlias_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ColorAlias_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ColorAlias_g_typePlugin = 
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

                return &ColorAlias_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusEnums::ColorAlias_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::ColorAlias_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnums::ColorAlias2_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ColorAlias2_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusEnums::ColorAlias2", /* Name */
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
                    }}; /* Type code for  ColorAlias2 */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ColorAlias2_g_tc;
                }

                ColorAlias2_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ColorAlias2_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::ColorAlias_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                ColorAlias2_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                ColorAlias2_g_tc._data._annotations._defaultValue._u.enumerated_value = 0;

                ColorAlias2_g_tc._data._sampleAccessInfo = sample_access_info();
                ColorAlias2_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &ColorAlias2_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ColorAlias2_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ColorAlias2_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ColorAlias2_g_sampleAccessInfo;
                }

                ColorAlias2_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ColorAlias2_g_sampleAccessInfo.memberAccessInfos = 
                ColorAlias2_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::ColorAlias2);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ColorAlias2_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ColorAlias2_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ColorAlias2_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ColorAlias2_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::ColorAlias2 >;

                ColorAlias2_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ColorAlias2_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ColorAlias2_g_typePlugin = 
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

                return &ColorAlias2_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusEnums::ColorAlias2_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::ColorAlias2_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusEnums::Record > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Record_g_tc_members[5]=
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
                    }, 
                    {
                        (char *)"aliasValue",/* Member name */
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
                    }, 
                    {
                        (char *)"color",/* Member name */
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
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"aliasColor",/* Member name */
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
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"colors",/* Member name */
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
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Record_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusEnums::Record", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        5, /* Number of members */
                        Record_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Record*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Record_g_tc;
                }

                Record_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Record_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::Scalar_AliasTag_t>::get().native();
                Record_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::ScalarAlias_AliasTag_t>::get().native();
                Record_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::Color>::get().native();
                Record_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::ColorAlias2_AliasTag_t>::get().native();
                Record_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusEnums::ColorSequence_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                Record_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Record_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Record_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Record_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Record_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Record_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Record_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Record_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Record_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Record_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Record_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Record_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Record_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Record_g_tc_members[2]._annotations._defaultValue._u.enumerated_value = 0;
                Record_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Record_g_tc_members[3]._annotations._defaultValue._u.enumerated_value = 0;

                Record_g_tc._data._sampleAccessInfo = sample_access_info();
                Record_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Record_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusEnums::Record *sample;

                static RTIXCdrMemberAccessInfo Record_g_memberAccessInfos[5] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Record_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Record_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusEnums::Record);
                if (sample == NULL) {
                    return NULL;
                }

                Record_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                Record_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->aliasValue() - (char *)sample);

                Record_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->color() - (char *)sample);

                Record_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->aliasColor() - (char *)sample);

                Record_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->colors() - (char *)sample);

                Record_g_sampleAccessInfo.memberAccessInfos = 
                Record_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnums::Record);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Record_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Record_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Record_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Record_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnums::Record >;

                Record_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Record_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Record_g_typePlugin = 
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

                return &Record_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusEnums::Record >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnums::Record >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusEnums::Record >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusEnums::RecordPlugin_new,
                ::CorpusEnums::RecordPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusEnums::Record >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusEnums::Record& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = RecordPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = RecordPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusEnums::Record >::from_cdr_buffer(::CorpusEnums::Record& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = RecordPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusEnums::Record from cdr buffer");
        }

        void topic_type_support< ::CorpusEnums::Record >::reset_sample(::CorpusEnums::Record& sample) 
        {
            sample.value(0);
            sample.aliasValue(0);
            sample.color(CorpusEnums::Color::RED);
            sample.aliasColor(CorpusEnums::Color::RED);
            ::rti::topic::reset_sample(sample.colors());
        }

        void topic_type_support< ::CorpusEnums::Record >::allocate_sample(::CorpusEnums::Record& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.color(),  -1, -1);
            ::rti::topic::allocate_sample(sample.aliasColor(),  -1, -1);
            ::rti::topic::allocate_sample(sample.colors(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
