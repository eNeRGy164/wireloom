

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enums-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _03_enums_aliases_1935353038_hpp
#define _03_enums_aliases_1935353038_hpp

#include <iosfwd>

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#include "dds/core/SafeEnumeration.hpp"
#include "dds/core/String.hpp"
#include "dds/core/array.hpp"
#include "dds/core/vector.hpp"
#include "dds/core/External.hpp"
#include "rti/core/LongDouble.hpp"
#include "rti/core/Pointer.hpp"
#include "rti/core/array.hpp"
#include "rti/topic/TopicTraits.hpp"

#include "omg/types/string_view.hpp"

#include "rti/core/BoundedSequence.hpp"
#include "dds/core/Optional.hpp"

#ifndef NDDS_STANDALONE_TYPE
#include "cdr/cdr_typeCode.h"
#include "dds/domain/DomainParticipant.hpp"
#include "dds/topic/TopicTraits.hpp"
#include "dds/core/xtypes/DynamicType.hpp"
#include "dds/core/xtypes/StructType.hpp"
#include "dds/core/xtypes/UnionType.hpp"
#include "dds/core/xtypes/EnumType.hpp"
#include "dds/core/xtypes/AliasType.hpp"
#include "rti/util/StreamFlagSaver.hpp"
#include "rti/domain/PluginSupport.hpp"
#endif

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

namespace CorpusEnums {
    enum class Color {
        RED, 
        GREEN, 
        BLUE
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Color& sample);

    typedef int32_t Scalar;
    struct Scalar_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< ::CorpusEnums::Color >;
    template class NDDSUSERDllExport std::vector< ::CorpusEnums::Color >;
    #endif
    typedef ::rti::core::bounded_sequence< ::CorpusEnums::Color, 3L > ColorSequence;
    struct ColorSequence_AliasTag_t {};

    typedef int32_t ScalarAlias;
    struct ScalarAlias_AliasTag_t {};

    typedef ::CorpusEnums::Color ColorAlias;
    struct ColorAlias_AliasTag_t {};

    typedef ::CorpusEnums::Color ColorAlias2;
    struct ColorAlias2_AliasTag_t {};

    class NDDSUSERDllExport Record {
      public:

        Record();

        Record(int32_t value_,int32_t aliasValue_,const ::CorpusEnums::Color& color_,const ::CorpusEnums::Color& aliasColor_,const ::CorpusEnums::ColorSequence& colors_);

        int32_t& value() noexcept {
            return m_value_;
        }

        const int32_t& value() const noexcept {
            return m_value_;
        }

        void value(int32_t value) {

            m_value_ = value;
        }

        int32_t& aliasValue() noexcept {
            return m_aliasValue_;
        }

        const int32_t& aliasValue() const noexcept {
            return m_aliasValue_;
        }

        void aliasValue(int32_t value) {

            m_aliasValue_ = value;
        }

        ::CorpusEnums::Color& color() noexcept {
            return m_color_;
        }

        const ::CorpusEnums::Color& color() const noexcept {
            return m_color_;
        }

        void color(const ::CorpusEnums::Color& value) {

            m_color_ = value;
        }

        void color(::CorpusEnums::Color&& value) {
            m_color_ = std::move(value);
        }
        ::CorpusEnums::Color& aliasColor() noexcept {
            return m_aliasColor_;
        }

        const ::CorpusEnums::Color& aliasColor() const noexcept {
            return m_aliasColor_;
        }

        void aliasColor(const ::CorpusEnums::Color& value) {

            m_aliasColor_ = value;
        }

        void aliasColor(::CorpusEnums::Color&& value) {
            m_aliasColor_ = std::move(value);
        }
        ::CorpusEnums::ColorSequence& colors() noexcept {
            return m_colors_;
        }

        const ::CorpusEnums::ColorSequence& colors() const noexcept {
            return m_colors_;
        }

        void colors(const ::CorpusEnums::ColorSequence& value) {

            m_colors_ = value;
        }

        void colors(::CorpusEnums::ColorSequence&& value) {
            m_colors_ = std::move(value);
        }
        bool operator == (const Record& other_) const;
        bool operator != (const Record& other_) const;

        void swap(Record& other_) noexcept ;

      private:

        int32_t m_value_;
        int32_t m_aliasValue_;
        ::CorpusEnums::Color m_color_;
        ::CorpusEnums::Color m_aliasColor_;
        ::CorpusEnums::ColorSequence m_colors_;

    };

    inline void swap(Record& a, Record& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Record& sample);

} // namespace CorpusEnums  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
        template <>
        struct default_enumerator< ::CorpusEnums::Color>
        {
            static const ::CorpusEnums::Color value;
        };
    }
}
#else

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< ::CorpusEnums::Record > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusEnums::Record";
            }
        };

        template<>
        struct is_topic_type< ::CorpusEnums::Record > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusEnums::Record > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusEnums::Record& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusEnums::Record& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusEnums::Record& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusEnums::Record& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template <>
        struct default_enumerator< ::CorpusEnums::Color>
        {
            static const ::CorpusEnums::Color value;
        };
        template<>
        struct dynamic_type< ::CorpusEnums::Color > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };

        template <>
        struct extensibility< ::CorpusEnums::Color > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusEnums::Scalar_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusEnums::ColorSequence_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusEnums::ScalarAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusEnums::ColorAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusEnums::ColorAlias2_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusEnums::Record > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusEnums::Record > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _03_enums_aliases_1935353038_hpp

