

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-scopes.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _02_scopes_1527551736_hpp
#define _02_scopes_1527551736_hpp

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

namespace CorpusScopes {

    static const int32_t Bound = 4L;
    namespace Nested {

        class NDDSUSERDllExport Point {
          public:

            Point();

            Point(int32_t x_,int32_t y_);

            int32_t& x() noexcept {
                return m_x_;
            }

            const int32_t& x() const noexcept {
                return m_x_;
            }

            void x(int32_t value) {

                m_x_ = value;
            }

            int32_t& y() noexcept {
                return m_y_;
            }

            const int32_t& y() const noexcept {
                return m_y_;
            }

            void y(int32_t value) {

                m_y_ = value;
            }

            bool operator == (const Point& other_) const;
            bool operator != (const Point& other_) const;

            void swap(Point& other_) noexcept ;

          private:

            int32_t m_x_;
            int32_t m_y_;

        };

        inline void swap(Point& a, Point& b)  noexcept 
        {
            a.swap(b);
        }

        NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Point& sample);

    } // namespace Nested  

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    class NDDSUSERDllExport KeywordRecord {
      public:

        KeywordRecord();

        KeywordRecord(int32_t event_,const ::CorpusScopes::Nested::Point& relative_,const ::CorpusScopes::Nested::Point& absolute_,const ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) >& values_);

        int32_t& event() noexcept {
            return m_event_;
        }

        const int32_t& event() const noexcept {
            return m_event_;
        }

        void event(int32_t value) {

            m_event_ = value;
        }

        ::CorpusScopes::Nested::Point& relative() noexcept {
            return m_relative_;
        }

        const ::CorpusScopes::Nested::Point& relative() const noexcept {
            return m_relative_;
        }

        void relative(const ::CorpusScopes::Nested::Point& value) {

            m_relative_ = value;
        }

        void relative(::CorpusScopes::Nested::Point&& value) {
            m_relative_ = std::move(value);
        }
        ::CorpusScopes::Nested::Point& absolute() noexcept {
            return m_absolute_;
        }

        const ::CorpusScopes::Nested::Point& absolute() const noexcept {
            return m_absolute_;
        }

        void absolute(const ::CorpusScopes::Nested::Point& value) {

            m_absolute_ = value;
        }

        void absolute(::CorpusScopes::Nested::Point&& value) {
            m_absolute_ = std::move(value);
        }
        ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) >& values() noexcept {
            return m_values_;
        }

        const ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) >& values() const noexcept {
            return m_values_;
        }

        void values(const ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) >& value) {

            m_values_ = value;
        }

        void values(::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) >&& value) {
            m_values_ = std::move(value);
        }
        bool operator == (const KeywordRecord& other_) const;
        bool operator != (const KeywordRecord& other_) const;

        void swap(KeywordRecord& other_) noexcept ;

      private:

        int32_t m_event_;
        ::CorpusScopes::Nested::Point m_relative_;
        ::CorpusScopes::Nested::Point m_absolute_;
        ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) > m_values_;

    };

    inline void swap(KeywordRecord& a, KeywordRecord& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const KeywordRecord& sample);

} // namespace CorpusScopes  
namespace CorpusScopes {

    class NDDSUSERDllExport Reopened {
      public:

        Reopened();

        explicit Reopened(const ::CorpusScopes::KeywordRecord& value_);

        ::CorpusScopes::KeywordRecord& value() noexcept {
            return m_value_;
        }

        const ::CorpusScopes::KeywordRecord& value() const noexcept {
            return m_value_;
        }

        void value(const ::CorpusScopes::KeywordRecord& value) {

            m_value_ = value;
        }

        void value(::CorpusScopes::KeywordRecord&& value) {
            m_value_ = std::move(value);
        }
        bool operator == (const Reopened& other_) const;
        bool operator != (const Reopened& other_) const;

        void swap(Reopened& other_) noexcept ;

      private:

        ::CorpusScopes::KeywordRecord m_value_;

    };

    inline void swap(Reopened& a, Reopened& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Reopened& sample);

} // namespace CorpusScopes  

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
        struct topic_type_name< ::CorpusScopes::Nested::Point > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusScopes::Nested::Point";
            }
        };

        template<>
        struct is_topic_type< ::CorpusScopes::Nested::Point > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusScopes::Nested::Point > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusScopes::Nested::Point& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusScopes::Nested::Point& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusScopes::Nested::Point& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusScopes::Nested::Point& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusScopes::KeywordRecord > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusScopes::KeywordRecord";
            }
        };

        template<>
        struct is_topic_type< ::CorpusScopes::KeywordRecord > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusScopes::KeywordRecord > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusScopes::KeywordRecord& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusScopes::KeywordRecord& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusScopes::KeywordRecord& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusScopes::KeywordRecord& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusScopes::Reopened > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusScopes::Reopened";
            }
        };

        template<>
        struct is_topic_type< ::CorpusScopes::Reopened > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusScopes::Reopened > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusScopes::Reopened& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusScopes::Reopened& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusScopes::Reopened& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusScopes::Reopened& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusScopes::Nested::Point > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusScopes::Nested::Point > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusScopes::KeywordRecord > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusScopes::KeywordRecord > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusScopes::Reopened > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusScopes::Reopened > {
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

#endif // _02_scopes_1527551736_hpp

