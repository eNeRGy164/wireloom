

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant.idl
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
#include "14-enum-constantPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-enum-constant.hpp"

#include <rti/util/ostream_operators.hpp>

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
        default:
        {
            /* Prevents compiler warnings */
        }
    }
    return o;
}

// ---- EnumConstant: 

EnumConstant::EnumConstant() :
    m_color_(Color::RED)  {

}   

EnumConstant::EnumConstant (const ::Color& color_):
    m_color_(color_) {
}

void EnumConstant::swap(EnumConstant& other_)  noexcept 
{
    using std::swap;
    swap(m_color_, other_.m_color_);
}  

bool EnumConstant::operator == (const EnumConstant& other_) const {
    if (m_color_ != other_.m_color_) {
        return false;
    }
    return true;
}

bool EnumConstant::operator != (const EnumConstant& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const EnumConstant& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "color: " << sample.color ();
    o <<"]";
    return o;
}

// ---- EnumUnion: 

EnumUnion::Union_::Union_() :
    m_selected_ (0) ,
    m_red_ (0)  {
}

EnumUnion::Union_::Union_(int32_t selected_,int32_t red_):
    m_selected_(selected_), 
    m_red_(red_) {
}

EnumUnion::EnumUnion() :m_d_(default_discriminator())
{
}

void EnumUnion::swap(EnumUnion& other_)  noexcept 
{
    using std::swap;
    swap (m_d_,other_.m_d_);
    switch (::rti::topic::cdr::integer_case(_d())) {
        case Selected:
        {  
            swap(m_u_.m_selected_, other_.m_u_.m_selected_);
        } break;
        case (Color::RED):
            {  
                swap(m_u_.m_red_, other_.m_u_.m_red_);
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
                swap(m_u_.m_selected_, other_.m_u_.m_selected_);
            } break;
            case (Color::RED):
                {  
                    swap(m_u_.m_red_, other_.m_u_.m_red_);
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

bool EnumUnion::operator == (const EnumUnion& other_) const {
    if (_d() != other_._d()){
        return false;
    }
    switch(::rti::topic::cdr::integer_case(_d())){
        case Selected:
        {  
            if (m_u_.m_selected_ != other_.m_u_.m_selected_) {
                return false;
            }
        } break ;
        case (Color::RED):
            {  
                if (m_u_.m_red_ != other_.m_u_.m_red_) {
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

bool EnumUnion::operator != (const EnumUnion& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const EnumUnion& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "_d: " << sample._d () <<", ";
    switch(::rti::topic::cdr::integer_case(sample._d())){
        case Selected:
        {  
            o << "selected: " << sample.selected ()<<", ";
        } break ;
        case (Color::RED):
            {  
                o << "red: " << sample.red ();
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

::Color EnumUnion::default_discriminator() {
    return ::Color(static_cast< ::Color >(0));
}

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::Color default_enumerator< ::Color>::value = ::Color::RED;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::Color default_enumerator< ::Color>::value = ::Color::RED;
        template<>
        struct native_type_code< ::Color > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Color_g_tc_members[2]=
                {

                    {
                        (char *)"RED",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::Color::RED), 
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
                        static_cast<int>(::Color::GREEN), 
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
                        (char *)"Color", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
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
                    size_t candidateTypeSize = sizeof(::Color);

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
                interpreter::get_aggregation_value_pointer< ::Color >;

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

        const ::dds::core::xtypes::EnumType& dynamic_type< ::Color >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Color >::get())));
        }

        template<>
        struct native_type_code< ::EnumConstant > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member EnumConstant_g_tc_members[1]=
                {

                    {
                        (char *)"color",/* Member name */
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

                static DDS_TypeCode EnumConstant_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"EnumConstant", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        EnumConstant_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for EnumConstant*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &EnumConstant_g_tc;
                }

                EnumConstant_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                EnumConstant_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Color>::get().native();

                /* Initialize the values for member annotations. */
                EnumConstant_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                EnumConstant_g_tc_members[0]._annotations._defaultValue._u.enumerated_value = 0;

                EnumConstant_g_tc._data._sampleAccessInfo = sample_access_info();
                EnumConstant_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &EnumConstant_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::EnumConstant *sample;

                static RTIXCdrMemberAccessInfo EnumConstant_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo EnumConstant_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &EnumConstant_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::EnumConstant);
                if (sample == NULL) {
                    return NULL;
                }

                EnumConstant_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->color() - (char *)sample);

                EnumConstant_g_sampleAccessInfo.memberAccessInfos = 
                EnumConstant_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::EnumConstant);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        EnumConstant_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        EnumConstant_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                EnumConstant_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                EnumConstant_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::EnumConstant >;

                EnumConstant_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &EnumConstant_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin EnumConstant_g_typePlugin = 
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

                return &EnumConstant_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::EnumConstant >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::EnumConstant >::get())));
        }

        template<>
        struct native_type_code< ::EnumUnion > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member EnumUnion_g_tc_members[2]=
                {

                    {
                        (char *)"selected",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"red",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((Color::RED)),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode EnumUnion_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"EnumUnion", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        EnumUnion_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for EnumUnion*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &EnumUnion_g_tc;
                }

                EnumUnion_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                EnumUnion_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                EnumUnion_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                EnumUnion_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                EnumUnion_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                EnumUnion_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                EnumUnion_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                EnumUnion_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                EnumUnion_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                EnumUnion_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                EnumUnion_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                EnumUnion_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                EnumUnion_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                EnumUnion_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                EnumUnion_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                EnumUnion_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Color>::get().native();

                EnumUnion_g_tc._data._sampleAccessInfo = sample_access_info();
                EnumUnion_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &EnumUnion_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::EnumUnion *sample;

                static RTIXCdrMemberAccessInfo EnumUnion_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo EnumUnion_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &EnumUnion_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::EnumUnion);
                if (sample == NULL) {
                    return NULL;
                }

                EnumUnion_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = Selected;
                EnumUnion_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->selected() - (char *)sample);

                sample->_d() = (Color::RED);
                EnumUnion_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->red() - (char *)sample);

                EnumUnion_g_sampleAccessInfo.memberAccessInfos = 
                EnumUnion_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::EnumUnion);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        EnumUnion_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        EnumUnion_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                EnumUnion_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                EnumUnion_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::EnumUnion >;

                EnumUnion_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &EnumUnion_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin EnumUnion_g_typePlugin = 
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

                return &EnumUnion_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::EnumUnion >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::EnumUnion >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::EnumConstant >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::EnumConstantPlugin_new,
                ::EnumConstantPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::EnumConstant >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::EnumConstant& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = EnumConstantPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = EnumConstantPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::EnumConstant >::from_cdr_buffer(::EnumConstant& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = EnumConstantPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::EnumConstant from cdr buffer");
        }

        void topic_type_support< ::EnumConstant >::reset_sample(::EnumConstant& sample) 
        {
            sample.color(Color::RED);
        }

        void topic_type_support< ::EnumConstant >::allocate_sample(::EnumConstant& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.color(),  -1, -1);
        }
        void topic_type_support< ::EnumUnion >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::EnumUnionPlugin_new,
                ::EnumUnionPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::EnumUnion >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::EnumUnion& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = EnumUnionPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = EnumUnionPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::EnumUnion >::from_cdr_buffer(::EnumUnion& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = EnumUnionPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::EnumUnion from cdr buffer");
        }

        void topic_type_support< ::EnumUnion >::reset_sample(::EnumUnion& sample) 
        {
            sample._d() = Selected;
            sample.selected(0);
            sample._d() = (Color::RED);
            sample.red(0);

            sample._d() = ::EnumUnion::default_discriminator();
        }

        void topic_type_support< ::EnumUnion >::allocate_sample(::EnumUnion& sample, int, int) 
        {

            sample._d() = ::EnumUnion::default_discriminator();
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
