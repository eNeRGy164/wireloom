

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-default-literal-enum-array.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _10_default_literal_enum_array_1866069445_hpp
#define _10_default_literal_enum_array_1866069445_hpp

#include <iosfwd>

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#include "dds/domain/DomainParticipant.hpp"
#include "dds/topic/TopicTraits.hpp"
#include "dds/core/SafeEnumeration.hpp"
#include "dds/core/String.hpp"
#include "dds/core/array.hpp"
#include "dds/core/vector.hpp"
#include "dds/core/Optional.hpp"
#include "dds/core/xtypes/DynamicType.hpp"
#include "dds/core/xtypes/StructType.hpp"
#include "dds/core/xtypes/UnionType.hpp"
#include "dds/core/xtypes/EnumType.hpp"
#include "dds/core/xtypes/AliasType.hpp"
#include "rti/core/array.hpp"
#include "rti/core/BoundedSequence.hpp"
#include "rti/util/StreamFlagSaver.hpp"
#include "rti/domain/PluginSupport.hpp"
#include "rti/core/LongDouble.hpp"
#include "dds/core/External.hpp"
#include "rti/core/Pointer.hpp"
#include "rti/topic/TopicTraits.hpp"

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef RTIUSERDllExport
#define RTIUSERDllExport
#endif

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

namespace CorpusDefaultLiteralEnumArray {
    enum class Color {
        GREEN, 
        RED, 
        BLUE
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Color& sample);

    typedef ::dds::core::array< CorpusDefaultLiteralEnumArray::Color, 5L> ColorArray;
    struct ColorArray_AliasTag_t {};

    class NDDSUSERDllExport Shape {
      public:
        Shape();

        Shape(const CorpusDefaultLiteralEnumArray::Color& color,const CorpusDefaultLiteralEnumArray::ColorArray& colorArray);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Shape (Shape&&) = default;
        Shape& operator=(Shape&&) = default;
        Shape& operator=(const Shape&) = default;
        Shape(const Shape&) = default;
        #else
        Shape(Shape&& other_) OMG_NOEXCEPT;  
        Shape& operator=(Shape&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        CorpusDefaultLiteralEnumArray::Color& color() OMG_NOEXCEPT {
            return m_color_;
        }

        const CorpusDefaultLiteralEnumArray::Color& color() const OMG_NOEXCEPT {
            return m_color_;
        }

        void color(const CorpusDefaultLiteralEnumArray::Color& value) {
            m_color_ = value;
        }

        void color(CorpusDefaultLiteralEnumArray::Color&& value) {
            m_color_ = std::move(value);
        }
        CorpusDefaultLiteralEnumArray::ColorArray& colorArray() OMG_NOEXCEPT {
            return m_colorArray_;
        }

        const CorpusDefaultLiteralEnumArray::ColorArray& colorArray() const OMG_NOEXCEPT {
            return m_colorArray_;
        }

        void colorArray(const CorpusDefaultLiteralEnumArray::ColorArray& value) {
            m_colorArray_ = value;
        }

        void colorArray(CorpusDefaultLiteralEnumArray::ColorArray&& value) {
            m_colorArray_ = std::move(value);
        }

        bool operator == (const Shape& other_) const;
        bool operator != (const Shape& other_) const;

        void swap(Shape& other_) OMG_NOEXCEPT ;

      private:

        CorpusDefaultLiteralEnumArray::Color m_color_;
        CorpusDefaultLiteralEnumArray::ColorArray m_colorArray_;

    };

    inline void swap(Shape& a, Shape& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Shape& sample);

} // namespace CorpusDefaultLiteralEnumArray  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusDefaultLiteralEnumArray::Shape > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusDefaultLiteralEnumArray::Shape";
            }
        };

        template<>
        struct is_topic_type< CorpusDefaultLiteralEnumArray::Shape > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusDefaultLiteralEnumArray::Shape > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusDefaultLiteralEnumArray::Shape& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusDefaultLiteralEnumArray::Shape& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusDefaultLiteralEnumArray::Shape& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusDefaultLiteralEnumArray::Shape& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template <>
        struct default_enumerator<CorpusDefaultLiteralEnumArray::Color>
        {
            static const CorpusDefaultLiteralEnumArray::Color value;
        };
        template<>
        struct dynamic_type< CorpusDefaultLiteralEnumArray::Color > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusDefaultLiteralEnumArray::Color > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusDefaultLiteralEnumArray::ColorArray_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusDefaultLiteralEnumArray::Shape > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusDefaultLiteralEnumArray::Shape > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

    }
}

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _10_default_literal_enum_array_1866069445_hpp

