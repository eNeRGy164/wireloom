

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-default-literal-enum-array.idl
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
#include "10-default-literal-enum-arrayPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "10-default-literal-enum-array.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusDefaultLiteralEnumArray {

    std::ostream& operator << (std::ostream& o,const Color& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case Color::GREEN:
            o << "Color::GREEN" << " ";
            break;
            case Color::RED:
            o << "Color::RED" << " ";
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

    // ---- Shape: 

    Shape::Shape() :
        m_color_(CorpusDefaultLiteralEnumArray::Color::RED)  {

    }   

    Shape::Shape (const ::CorpusDefaultLiteralEnumArray::Color& color_,const ::CorpusDefaultLiteralEnumArray::ColorArray& colorArray_):
        m_color_(color_), 
        m_colorArray_(colorArray_) {
    }

    void Shape::swap(Shape& other_)  noexcept 
    {
        using std::swap;
        swap(m_color_, other_.m_color_);
        swap(m_colorArray_, other_.m_colorArray_);
    }  

    bool Shape::operator == (const Shape& other_) const {
        if (m_color_ != other_.m_color_) {
            return false;
        }
        if (m_colorArray_ != other_.m_colorArray_) {
            return false;
        }
        return true;
    }

    bool Shape::operator != (const Shape& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Shape& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "color: " << sample.color ()<<", ";
        o << "colorArray: " << sample.colorArray ();
        o <<"]";
        return o;
    }

} // namespace CorpusDefaultLiteralEnumArray  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusDefaultLiteralEnumArray::Color default_enumerator< ::CorpusDefaultLiteralEnumArray::Color>::value = ::CorpusDefaultLiteralEnumArray::Color::RED;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusDefaultLiteralEnumArray::Color default_enumerator< ::CorpusDefaultLiteralEnumArray::Color>::value = ::CorpusDefaultLiteralEnumArray::Color::RED;
        template<>
        struct native_type_code< ::CorpusDefaultLiteralEnumArray::Color > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Color_g_tc_members[3]=
                {

                    {
                        (char *)"GREEN",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusDefaultLiteralEnumArray::Color::GREEN), 
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
                        (char *)"RED",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusDefaultLiteralEnumArray::Color::RED), 
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
                        static_cast<int>(::CorpusDefaultLiteralEnumArray::Color::BLUE), 
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
                        (char *)"CorpusDefaultLiteralEnumArray::Color", /* Name */
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
                Color_g_tc._data._annotations._defaultValue._u.long_value = 1;

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
                    size_t candidateTypeSize = sizeof(::CorpusDefaultLiteralEnumArray::Color);

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
                interpreter::get_aggregation_value_pointer< ::CorpusDefaultLiteralEnumArray::Color >;

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

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusDefaultLiteralEnumArray::Color >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusDefaultLiteralEnumArray::Color >::get())));
        }

        template<>
        struct native_type_code< ::CorpusDefaultLiteralEnumArray::ColorArray_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ColorArray_g_tc_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,5L, NULL,NULL);

                static DDS_TypeCode ColorArray_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusDefaultLiteralEnumArray::ColorArray", /* Name */
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
                    }}; /* Type code for  ColorArray */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ColorArray_g_tc;
                }

                ColorArray_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ColorArray_g_tc_array._data._typeCode =(RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusDefaultLiteralEnumArray::Color>::get().native();
                ColorArray_g_tc._data._typeCode =  (RTICdrTypeCode *)& ColorArray_g_tc_array;

                /* Initialize the values for member annotations. */

                ColorArray_g_tc._data._sampleAccessInfo = sample_access_info();
                ColorArray_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &ColorArray_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo ColorArray_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ColorArray_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ColorArray_g_sampleAccessInfo;
                }

                ColorArray_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                ColorArray_g_sampleAccessInfo.memberAccessInfos = 
                ColorArray_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusDefaultLiteralEnumArray::ColorArray);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ColorArray_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ColorArray_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ColorArray_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ColorArray_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusDefaultLiteralEnumArray::ColorArray >;

                ColorArray_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ColorArray_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ColorArray_g_typePlugin = 
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

                return &ColorArray_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusDefaultLiteralEnumArray::ColorArray_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusDefaultLiteralEnumArray::ColorArray_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusDefaultLiteralEnumArray::Shape > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Shape_g_tc_members[2]=
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
                    }, 
                    {
                        (char *)"colorArray",/* Member name */
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

                static DDS_TypeCode Shape_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusDefaultLiteralEnumArray::Shape", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Shape_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Shape*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Shape_g_tc;
                }

                Shape_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Shape_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusDefaultLiteralEnumArray::Color>::get().native();
                Shape_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusDefaultLiteralEnumArray::ColorArray_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                Shape_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Shape_g_tc_members[0]._annotations._defaultValue._u.enumerated_value = 1;

                Shape_g_tc._data._sampleAccessInfo = sample_access_info();
                Shape_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Shape_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusDefaultLiteralEnumArray::Shape *sample;

                static RTIXCdrMemberAccessInfo Shape_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Shape_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Shape_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusDefaultLiteralEnumArray::Shape);
                if (sample == NULL) {
                    return NULL;
                }

                Shape_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->color() - (char *)sample);

                Shape_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->colorArray() - (char *)sample);

                Shape_g_sampleAccessInfo.memberAccessInfos = 
                Shape_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusDefaultLiteralEnumArray::Shape);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Shape_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Shape_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Shape_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Shape_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusDefaultLiteralEnumArray::Shape >;

                Shape_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Shape_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Shape_g_typePlugin = 
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

                return &Shape_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusDefaultLiteralEnumArray::Shape >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusDefaultLiteralEnumArray::Shape >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusDefaultLiteralEnumArray::Shape >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusDefaultLiteralEnumArray::ShapePlugin_new,
                ::CorpusDefaultLiteralEnumArray::ShapePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusDefaultLiteralEnumArray::Shape >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusDefaultLiteralEnumArray::Shape& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ShapePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ShapePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusDefaultLiteralEnumArray::Shape >::from_cdr_buffer(::CorpusDefaultLiteralEnumArray::Shape& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ShapePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusDefaultLiteralEnumArray::Shape from cdr buffer");
        }

        void topic_type_support< ::CorpusDefaultLiteralEnumArray::Shape >::reset_sample(::CorpusDefaultLiteralEnumArray::Shape& sample) 
        {
            sample.color(CorpusDefaultLiteralEnumArray::Color::RED);
            ::rti::topic::reset_sample(sample.colorArray());
        }

        void topic_type_support< ::CorpusDefaultLiteralEnumArray::Shape >::allocate_sample(::CorpusDefaultLiteralEnumArray::Shape& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.color(),  -1, -1);
            ::rti::topic::allocate_sample(sample.colorArray(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
