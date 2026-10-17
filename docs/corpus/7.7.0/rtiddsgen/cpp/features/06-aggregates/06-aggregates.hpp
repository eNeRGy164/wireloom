

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-aggregates.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _06_aggregates_2110920171_hpp
#define _06_aggregates_2110920171_hpp

#include <iosfwd>

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef RTIUSERDllExport
#define RTIUSERDllExport __attribute__((visibility("default")))
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

#include "omg/types/sequence.hpp"
#include "omg/types/optional.hpp"

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

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef RTIUSERDllExport
#define RTIUSERDllExport
#endif

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace CorpusAggregates {

    struct NDDSUSERDllExport Base {
        int32_t base {};

        Base();

        explicit Base(int32_t base_);

    };

    NDDSUSERDllExport bool operator == (const Base& a, const Base& b);
    NDDSUSERDllExport bool operator != (const Base& a, const Base& b);
    NDDSUSERDllExport void swap(Base& a, Base& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Base& sample);

    struct NDDSUSERDllExport Derived
    : public ::CorpusAggregates::Base {
        std::string derived {};

        Derived();

        explicit Derived(int32_t base_,const ::omg::types::string_view& derived_);

    };

    NDDSUSERDllExport bool operator == (const Derived& a, const Derived& b);
    NDDSUSERDllExport bool operator != (const Derived& a, const Derived& b);
    NDDSUSERDllExport void swap(Derived& a, Derived& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Derived& sample);

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< ::CorpusAggregates::Recursive >;
    template class NDDSUSERDllExport std::vector< ::CorpusAggregates::Recursive >;
    #endif
    struct NDDSUSERDllExport Recursive {
        int32_t value {};
        ::omg::types::sequence< ::CorpusAggregates::Recursive > children {};

        Recursive();

        Recursive(int32_t value_,const ::omg::types::sequence< ::CorpusAggregates::Recursive >& children_);

    };

    NDDSUSERDllExport bool operator == (const Recursive& a, const Recursive& b);
    NDDSUSERDllExport bool operator != (const Recursive& a, const Recursive& b);
    NDDSUSERDllExport void swap(Recursive& a, Recursive& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Recursive& sample);

    struct NDDSUSERDllExport Composed {
        ::CorpusAggregates::Base base {};
        ::CorpusAggregates::Derived derived {};

        Composed();

        Composed(const ::CorpusAggregates::Base& base_,const ::CorpusAggregates::Derived& derived_);

    };

    NDDSUSERDllExport bool operator == (const Composed& a, const Composed& b);
    NDDSUSERDllExport bool operator != (const Composed& a, const Composed& b);
    NDDSUSERDllExport void swap(Composed& a, Composed& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Composed& sample);

} // namespace CorpusAggregates  

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
        struct topic_type_name< ::CorpusAggregates::Base > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregates::Base";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAggregates::Base > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAggregates::Base > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAggregates::Base& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAggregates::Base& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAggregates::Base& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAggregates::Base& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusAggregates::Derived > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregates::Derived";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAggregates::Derived > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAggregates::Derived > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAggregates::Derived& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAggregates::Derived& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAggregates::Derived& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAggregates::Derived& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusAggregates::Recursive > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregates::Recursive";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAggregates::Recursive > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAggregates::Recursive > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAggregates::Recursive& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAggregates::Recursive& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAggregates::Recursive& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAggregates::Recursive& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusAggregates::Composed > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregates::Composed";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAggregates::Composed > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAggregates::Composed > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAggregates::Composed& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAggregates::Composed& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAggregates::Composed& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAggregates::Composed& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusAggregates::Base > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAggregates::Base > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusAggregates::Derived > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAggregates::Derived > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusAggregates::Recursive > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get(RTIXCdrResourceNode *parentVisitedNode);
        };

        template <>
        struct extensibility< ::CorpusAggregates::Recursive > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusAggregates::Composed > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAggregates::Composed > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _06_aggregates_2110920171_hpp
