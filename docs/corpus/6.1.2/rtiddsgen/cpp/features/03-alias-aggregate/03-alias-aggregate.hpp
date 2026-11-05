

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-alias-aggregate.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _03_alias_aggregate_937154352_hpp
#define _03_alias_aggregate_937154352_hpp

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

namespace CorpusAggregateAliases {

    class NDDSUSERDllExport Point {
      public:
        Point();

        Point(int32_t x,int32_t y);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Point (Point&&) = default;
        Point& operator=(Point&&) = default;
        Point& operator=(const Point&) = default;
        Point(const Point&) = default;
        #else
        Point(Point&& other_) OMG_NOEXCEPT;  
        Point& operator=(Point&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& x() OMG_NOEXCEPT {
            return m_x_;
        }

        const int32_t& x() const OMG_NOEXCEPT {
            return m_x_;
        }

        void x(int32_t value) {
            m_x_ = value;
        }

        int32_t& y() OMG_NOEXCEPT {
            return m_y_;
        }

        const int32_t& y() const OMG_NOEXCEPT {
            return m_y_;
        }

        void y(int32_t value) {
            m_y_ = value;
        }

        bool operator == (const Point& other_) const;
        bool operator != (const Point& other_) const;

        void swap(Point& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_x_;
        int32_t m_y_;

    };

    inline void swap(Point& a, Point& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Point& sample);

    typedef CorpusAggregateAliases::Point PointAlias;
    struct PointAlias_AliasTag_t {};

    typedef CorpusAggregateAliases::Point PointAlias2;
    struct PointAlias2_AliasTag_t {};

    class NDDSUSERDllExport Sample {
      public:
        Sample();

        explicit Sample(const CorpusAggregateAliases::Point& point);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Sample (Sample&&) = default;
        Sample& operator=(Sample&&) = default;
        Sample& operator=(const Sample&) = default;
        Sample(const Sample&) = default;
        #else
        Sample(Sample&& other_) OMG_NOEXCEPT;  
        Sample& operator=(Sample&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        CorpusAggregateAliases::Point& point() OMG_NOEXCEPT {
            return m_point_;
        }

        const CorpusAggregateAliases::Point& point() const OMG_NOEXCEPT {
            return m_point_;
        }

        void point(const CorpusAggregateAliases::Point& value) {
            m_point_ = value;
        }

        void point(CorpusAggregateAliases::Point&& value) {
            m_point_ = std::move(value);
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        CorpusAggregateAliases::Point m_point_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusAggregateAliases  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusAggregateAliases::Point > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregateAliases::Point";
            }
        };

        template<>
        struct is_topic_type< CorpusAggregateAliases::Point > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusAggregateAliases::Point > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusAggregateAliases::Point& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusAggregateAliases::Point& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusAggregateAliases::Point& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusAggregateAliases::Point& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusAggregateAliases::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregateAliases::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusAggregateAliases::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusAggregateAliases::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusAggregateAliases::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusAggregateAliases::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusAggregateAliases::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusAggregateAliases::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusAggregateAliases::Point > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusAggregateAliases::Point > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusAggregateAliases::PointAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusAggregateAliases::PointAlias2_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusAggregateAliases::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusAggregateAliases::Sample > {
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

#endif // _03_alias_aggregate_937154352_hpp

