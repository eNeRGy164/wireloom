

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_alias_inheritance_745606614_hpp
#define _13_alias_inheritance_745606614_hpp

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

namespace CorpusIntegrationTrace {

    typedef std::string Producer;
    struct Producer_AliasTag_t {};

    struct NDDSUSERDllExport Context {
        std::string correlation {};
        ::omg::types::optional< std::string > producer {};

        Context();

        Context(const ::omg::types::string_view& correlation_,const ::omg::types::optional< std::string >& producer_);

    };

    NDDSUSERDllExport bool operator == (const Context& a, const Context& b);
    NDDSUSERDllExport bool operator != (const Context& a, const Context& b);
    NDDSUSERDllExport void swap(Context& a, Context& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Context& sample);

    struct NDDSUSERDllExport Base {
        int32_t baseValue {};

        Base();

        explicit Base(int32_t baseValue_);

    };

    NDDSUSERDllExport bool operator == (const Base& a, const Base& b);
    NDDSUSERDllExport bool operator != (const Base& a, const Base& b);
    NDDSUSERDllExport void swap(Base& a, Base& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Base& sample);

    struct NDDSUSERDllExport Derived
    : public ::CorpusIntegrationTrace::Base {
        std::string state {};
        ::CorpusIntegrationTrace::Context traceContext {};

        Derived();

        Derived(int32_t baseValue_,const ::omg::types::string_view& state_,const ::CorpusIntegrationTrace::Context& traceContext_);

    };

    NDDSUSERDllExport bool operator == (const Derived& a, const Derived& b);
    NDDSUSERDllExport bool operator != (const Derived& a, const Derived& b);
    NDDSUSERDllExport void swap(Derived& a, Derived& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Derived& sample);

} // namespace CorpusIntegrationTrace  

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
        struct topic_type_name< ::CorpusIntegrationTrace::Context > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationTrace::Context";
            }
        };

        template<>
        struct topic_type_support< ::CorpusIntegrationTrace::Context > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusIntegrationTrace::Context& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusIntegrationTrace::Context& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusIntegrationTrace::Base > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationTrace::Base";
            }
        };

        template<>
        struct topic_type_support< ::CorpusIntegrationTrace::Base > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusIntegrationTrace::Base& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusIntegrationTrace::Base& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusIntegrationTrace::Derived > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationTrace::Derived";
            }
        };

        template<>
        struct topic_type_support< ::CorpusIntegrationTrace::Derived > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusIntegrationTrace::Derived& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusIntegrationTrace::Derived& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusIntegrationTrace::Producer_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusIntegrationTrace::Context > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusIntegrationTrace::Context > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusIntegrationTrace::Base > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusIntegrationTrace::Base > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusIntegrationTrace::Derived > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusIntegrationTrace::Derived > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _13_alias_inheritance_745606614_hpp
