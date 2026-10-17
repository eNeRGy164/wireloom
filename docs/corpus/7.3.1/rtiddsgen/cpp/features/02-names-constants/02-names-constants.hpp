

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-names-constants.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _02_names_constants_98786474_hpp
#define _02_names_constants_98786474_hpp

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

namespace CorpusNames {

    static const int32_t Base = 4L;

    static const int32_t Derived = ((CorpusNames::Base)<<2L)|1L;
    enum class State {
        Idle, 
        Running, 
        Complete
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const State& sample);
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

    class NDDSUSERDllExport KeywordRecord {
      public:

        KeywordRecord();

        KeywordRecord(int32_t event_,const ::CorpusNames::State& state_,const ::CorpusNames::Nested::Point& point_);

        int32_t& event() noexcept {
            return m_event_;
        }

        const int32_t& event() const noexcept {
            return m_event_;
        }

        void event(int32_t value) {

            m_event_ = value;
        }

        ::CorpusNames::State& state() noexcept {
            return m_state_;
        }

        const ::CorpusNames::State& state() const noexcept {
            return m_state_;
        }

        void state(const ::CorpusNames::State& value) {

            m_state_ = value;
        }

        void state(::CorpusNames::State&& value) {
            m_state_ = std::move(value);
        }
        ::CorpusNames::Nested::Point& point() noexcept {
            return m_point_;
        }

        const ::CorpusNames::Nested::Point& point() const noexcept {
            return m_point_;
        }

        void point(const ::CorpusNames::Nested::Point& value) {

            m_point_ = value;
        }

        void point(::CorpusNames::Nested::Point&& value) {
            m_point_ = std::move(value);
        }
        bool operator == (const KeywordRecord& other_) const;
        bool operator != (const KeywordRecord& other_) const;

        void swap(KeywordRecord& other_) noexcept ;

      private:

        int32_t m_event_;
        ::CorpusNames::State m_state_;
        ::CorpusNames::Nested::Point m_point_;

    };

    inline void swap(KeywordRecord& a, KeywordRecord& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const KeywordRecord& sample);

} // namespace CorpusNames  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
        template <>
        struct default_enumerator< ::CorpusNames::State>
        {
            static const ::CorpusNames::State value;
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
        struct topic_type_name< ::CorpusNames::Nested::Point > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNames::Nested::Point";
            }
        };

        template<>
        struct is_topic_type< ::CorpusNames::Nested::Point > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusNames::Nested::Point > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusNames::Nested::Point& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusNames::Nested::Point& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusNames::Nested::Point& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusNames::Nested::Point& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusNames::KeywordRecord > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNames::KeywordRecord";
            }
        };

        template<>
        struct is_topic_type< ::CorpusNames::KeywordRecord > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusNames::KeywordRecord > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusNames::KeywordRecord& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusNames::KeywordRecord& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusNames::KeywordRecord& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusNames::KeywordRecord& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template <>
        struct default_enumerator< ::CorpusNames::State>
        {
            static const ::CorpusNames::State value;
        };
        template<>
        struct dynamic_type< ::CorpusNames::State > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };

        template <>
        struct extensibility< ::CorpusNames::State > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusNames::Nested::Point > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusNames::Nested::Point > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusNames::KeywordRecord > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusNames::KeywordRecord > {
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

#endif // _02_names_constants_98786474_hpp

