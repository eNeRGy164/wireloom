

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-defaults-ranges.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_defaults_ranges_492397801_hpp
#define _09_defaults_ranges_492397801_hpp

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

namespace CorpusDefaultsAndRanges {
    enum class Color {
        GREEN, 
        RED, 
        BLUE
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Color& sample);

    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(int32_t value_,int32_t ranged_,const ::CorpusDefaultsAndRanges::Color& color_);

        int32_t& value() noexcept {
            return m_value_;
        }

        const int32_t& value() const noexcept {
            return m_value_;
        }

        void value(int32_t value) {

            m_value_ = value;
        }

        int32_t& ranged() noexcept {
            return m_ranged_;
        }

        const int32_t& ranged() const noexcept {
            return m_ranged_;
        }

        void ranged(int32_t value) {

            m_ranged_ = value;
        }

        ::CorpusDefaultsAndRanges::Color& color() noexcept {
            return m_color_;
        }

        const ::CorpusDefaultsAndRanges::Color& color() const noexcept {
            return m_color_;
        }

        void color(const ::CorpusDefaultsAndRanges::Color& value) {

            m_color_ = value;
        }

        void color(::CorpusDefaultsAndRanges::Color&& value) {
            m_color_ = std::move(value);
        }
        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        int32_t m_value_;
        int32_t m_ranged_;
        ::CorpusDefaultsAndRanges::Color m_color_;

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusDefaultsAndRanges  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
        template <>
        struct default_enumerator< ::CorpusDefaultsAndRanges::Color>
        {
            static const ::CorpusDefaultsAndRanges::Color value;
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
        struct topic_type_name< ::CorpusDefaultsAndRanges::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusDefaultsAndRanges::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusDefaultsAndRanges::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusDefaultsAndRanges::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusDefaultsAndRanges::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusDefaultsAndRanges::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusDefaultsAndRanges::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusDefaultsAndRanges::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template <>
        struct default_enumerator< ::CorpusDefaultsAndRanges::Color>
        {
            static const ::CorpusDefaultsAndRanges::Color value;
        };
        template<>
        struct dynamic_type< ::CorpusDefaultsAndRanges::Color > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };

        template <>
        struct extensibility< ::CorpusDefaultsAndRanges::Color > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusDefaultsAndRanges::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusDefaultsAndRanges::Sample > {
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

#endif // _09_defaults_ranges_492397801_hpp

