

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-support.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_unbounded_support_1263732412_hpp
#define _14_unbounded_support_1263732412_hpp

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

namespace CorpusUnboundedSupport {

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    typedef std::vector< int32_t > LongValues;
    struct LongValues_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< std::string >;
    template class NDDSUSERDllExport std::vector< std::string >;
    #endif
    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(const std::vector< int32_t >& values_,const std::string& text_,const std::wstring& wideText_,const std::vector< std::string >& labels_,const ::CorpusUnboundedSupport::LongValues& aliasValues_,const ::rti::core::bounded_sequence< int32_t, 4L >& boundedValues_,const std::string& boundedText_);

        std::vector< int32_t >& values() noexcept {
            return m_values_;
        }

        const std::vector< int32_t >& values() const noexcept {
            return m_values_;
        }

        void values(const std::vector< int32_t >& value) {

            m_values_ = value;
        }

        void values(std::vector< int32_t >&& value) {
            m_values_ = std::move(value);
        }
        std::string& text() noexcept {
            return m_text_;
        }

        const std::string& text() const noexcept {
            return m_text_;
        }

        void text(const std::string& value) {

            m_text_ = value;
        }

        void text(std::string&& value) {
            m_text_ = std::move(value);
        }
        std::wstring& wideText() noexcept {
            return m_wideText_;
        }

        const std::wstring& wideText() const noexcept {
            return m_wideText_;
        }

        void wideText(const std::wstring& value) {

            m_wideText_ = value;
        }

        void wideText(std::wstring&& value) {
            m_wideText_ = std::move(value);
        }
        std::vector< std::string >& labels() noexcept {
            return m_labels_;
        }

        const std::vector< std::string >& labels() const noexcept {
            return m_labels_;
        }

        void labels(const std::vector< std::string >& value) {

            m_labels_ = value;
        }

        void labels(std::vector< std::string >&& value) {
            m_labels_ = std::move(value);
        }
        ::CorpusUnboundedSupport::LongValues& aliasValues() noexcept {
            return m_aliasValues_;
        }

        const ::CorpusUnboundedSupport::LongValues& aliasValues() const noexcept {
            return m_aliasValues_;
        }

        void aliasValues(const ::CorpusUnboundedSupport::LongValues& value) {

            m_aliasValues_ = value;
        }

        void aliasValues(::CorpusUnboundedSupport::LongValues&& value) {
            m_aliasValues_ = std::move(value);
        }
        ::rti::core::bounded_sequence< int32_t, 4L >& boundedValues() noexcept {
            return m_boundedValues_;
        }

        const ::rti::core::bounded_sequence< int32_t, 4L >& boundedValues() const noexcept {
            return m_boundedValues_;
        }

        void boundedValues(const ::rti::core::bounded_sequence< int32_t, 4L >& value) {

            m_boundedValues_ = value;
        }

        void boundedValues(::rti::core::bounded_sequence< int32_t, 4L >&& value) {
            m_boundedValues_ = std::move(value);
        }
        std::string& boundedText() noexcept {
            return m_boundedText_;
        }

        const std::string& boundedText() const noexcept {
            return m_boundedText_;
        }

        void boundedText(const std::string& value) {

            m_boundedText_ = value;
        }

        void boundedText(std::string&& value) {
            m_boundedText_ = std::move(value);
        }
        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        std::vector< int32_t > m_values_;
        std::string m_text_;
        std::wstring m_wideText_;
        std::vector< std::string > m_labels_;
        ::CorpusUnboundedSupport::LongValues m_aliasValues_;
        ::rti::core::bounded_sequence< int32_t, 4L > m_boundedValues_;
        std::string m_boundedText_;

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusUnboundedSupport  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
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
        struct topic_type_name< ::CorpusUnboundedSupport::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnboundedSupport::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnboundedSupport::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnboundedSupport::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnboundedSupport::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnboundedSupport::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnboundedSupport::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnboundedSupport::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusUnboundedSupport::LongValues_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusUnboundedSupport::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusUnboundedSupport::Sample > {
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

#endif // _14_unbounded_support_1263732412_hpp

