

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-boundaries.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_boundaries_919417863_hpp
#define _08_key_boundaries_919417863_hpp

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

namespace CorpusKeyBoundaries {

    struct NDDSUSERDllExport ArrayKey {
        ::dds::core::array< int32_t, 2L> coordinates {};
        int32_t payload {};

        ArrayKey();

        ArrayKey(const ::dds::core::array< int32_t, 2L>& coordinates_,int32_t payload_);

    };

    NDDSUSERDllExport bool operator == (const ArrayKey& a, const ArrayKey& b);
    NDDSUSERDllExport bool operator != (const ArrayKey& a, const ArrayKey& b);
    NDDSUSERDllExport void swap(ArrayKey& a, ArrayKey& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ArrayKey& sample);

    struct NDDSUSERDllExport BoundedStringKey {
        std::string name {};
        int32_t payload {};

        BoundedStringKey();

        BoundedStringKey(const ::omg::types::string_view& name_,int32_t payload_);

    };

    NDDSUSERDllExport bool operator == (const BoundedStringKey& a, const BoundedStringKey& b);
    NDDSUSERDllExport bool operator != (const BoundedStringKey& a, const BoundedStringKey& b);
    NDDSUSERDllExport void swap(BoundedStringKey& a, BoundedStringKey& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const BoundedStringKey& sample);

    struct NDDSUSERDllExport UnboundedStringKey {
        std::string name {};
        int32_t payload {};

        UnboundedStringKey();

        UnboundedStringKey(const ::omg::types::string_view& name_,int32_t payload_);

    };

    NDDSUSERDllExport bool operator == (const UnboundedStringKey& a, const UnboundedStringKey& b);
    NDDSUSERDllExport bool operator != (const UnboundedStringKey& a, const UnboundedStringKey& b);
    NDDSUSERDllExport void swap(UnboundedStringKey& a, UnboundedStringKey& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const UnboundedStringKey& sample);

} // namespace CorpusKeyBoundaries  

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
        struct topic_type_name< ::CorpusKeyBoundaries::ArrayKey > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusKeyBoundaries::ArrayKey";
            }
        };

        template<>
        struct is_topic_type< ::CorpusKeyBoundaries::ArrayKey > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusKeyBoundaries::ArrayKey > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusKeyBoundaries::ArrayKey& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusKeyBoundaries::ArrayKey& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusKeyBoundaries::ArrayKey& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusKeyBoundaries::ArrayKey& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusKeyBoundaries::BoundedStringKey > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusKeyBoundaries::BoundedStringKey";
            }
        };

        template<>
        struct is_topic_type< ::CorpusKeyBoundaries::BoundedStringKey > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusKeyBoundaries::BoundedStringKey > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusKeyBoundaries::BoundedStringKey& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusKeyBoundaries::BoundedStringKey& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusKeyBoundaries::BoundedStringKey& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusKeyBoundaries::BoundedStringKey& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusKeyBoundaries::UnboundedStringKey > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusKeyBoundaries::UnboundedStringKey";
            }
        };

        template<>
        struct is_topic_type< ::CorpusKeyBoundaries::UnboundedStringKey > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusKeyBoundaries::UnboundedStringKey > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusKeyBoundaries::UnboundedStringKey& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusKeyBoundaries::UnboundedStringKey& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusKeyBoundaries::UnboundedStringKey& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusKeyBoundaries::UnboundedStringKey& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusKeyBoundaries::ArrayKey > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusKeyBoundaries::ArrayKey > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusKeyBoundaries::BoundedStringKey > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusKeyBoundaries::BoundedStringKey > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusKeyBoundaries::UnboundedStringKey > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusKeyBoundaries::UnboundedStringKey > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _08_key_boundaries_919417863_hpp
