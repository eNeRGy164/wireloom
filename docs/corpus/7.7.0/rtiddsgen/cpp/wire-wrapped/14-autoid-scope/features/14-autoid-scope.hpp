

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_autoid_scope_2088995224_hpp
#define _14_autoid_scope_2088995224_hpp

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

namespace AutoIds {

    struct NDDSUSERDllExport Hashed {
        int32_t first {};
        int32_t second {};

        Hashed();

        Hashed(int32_t first_,int32_t second_);

    };

    NDDSUSERDllExport bool operator == (const Hashed& a, const Hashed& b);
    NDDSUSERDllExport bool operator != (const Hashed& a, const Hashed& b);
    NDDSUSERDllExport void swap(Hashed& a, Hashed& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Hashed& sample);

} // namespace AutoIds  
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "includes/autoid-hash-include.hpp"
#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif
namespace AutoIds {

    struct NDDSUSERDllExport Reopened {
        int32_t first {};
        int32_t second {};

        Reopened();

        Reopened(int32_t first_,int32_t second_);

    };

    NDDSUSERDllExport bool operator == (const Reopened& a, const Reopened& b);
    NDDSUSERDllExport bool operator != (const Reopened& a, const Reopened& b);
    NDDSUSERDllExport void swap(Reopened& a, Reopened& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Reopened& sample);

} // namespace AutoIds  

struct NDDSUSERDllExport Sequential {
    int32_t first {};
    int32_t second {};

    Sequential();

    Sequential(int32_t first_,int32_t second_);

};

NDDSUSERDllExport bool operator == (const Sequential& a, const Sequential& b);
NDDSUSERDllExport bool operator != (const Sequential& a, const Sequential& b);
NDDSUSERDllExport void swap(Sequential& a, Sequential& b)  noexcept;

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sequential& sample);

struct NDDSUSERDllExport UsesInclude {
    ::Included included {};

    UsesInclude();

    explicit UsesInclude(const ::Included& included_);

};

NDDSUSERDllExport bool operator == (const UsesInclude& a, const UsesInclude& b);
NDDSUSERDllExport bool operator != (const UsesInclude& a, const UsesInclude& b);
NDDSUSERDllExport void swap(UsesInclude& a, UsesInclude& b)  noexcept;

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const UsesInclude& sample);

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
        struct topic_type_name< ::AutoIds::Hashed > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIds::Hashed";
            }
        };

        template<>
        struct is_topic_type< ::AutoIds::Hashed > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::AutoIds::Hashed > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::AutoIds::Hashed& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::AutoIds::Hashed& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::AutoIds::Hashed& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::AutoIds::Hashed& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::AutoIds::Reopened > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIds::Reopened";
            }
        };

        template<>
        struct is_topic_type< ::AutoIds::Reopened > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::AutoIds::Reopened > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::AutoIds::Reopened& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::AutoIds::Reopened& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::AutoIds::Reopened& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::AutoIds::Reopened& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::Sequential > {
            NDDSUSERDllExport static std::string value() {
                return "Sequential";
            }
        };

        template<>
        struct is_topic_type< ::Sequential > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::Sequential > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::Sequential& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::Sequential& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::Sequential& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::Sequential& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::UsesInclude > {
            NDDSUSERDllExport static std::string value() {
                return "UsesInclude";
            }
        };

        template<>
        struct is_topic_type< ::UsesInclude > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::UsesInclude > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::UsesInclude& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::UsesInclude& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::UsesInclude& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::UsesInclude& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::AutoIds::Hashed > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::AutoIds::Hashed > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::AutoIds::Reopened > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::AutoIds::Reopened > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::Sequential > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::Sequential > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::UsesInclude > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::UsesInclude > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _14_autoid_scope_2088995224_hpp
