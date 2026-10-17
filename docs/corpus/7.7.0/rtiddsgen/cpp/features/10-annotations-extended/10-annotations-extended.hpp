

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-annotations-extended.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _10_annotations_extended_192864544_hpp
#define _10_annotations_extended_192864544_hpp

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

namespace CorpusAnnotationVariants {

    struct NDDSUSERDllExport Sample {
        int32_t id {};
        int32_t value {};

        Sample();

        Sample(int32_t id_,int32_t value_);

    };

    NDDSUSERDllExport bool operator == (const Sample& a, const Sample& b);
    NDDSUSERDllExport bool operator != (const Sample& a, const Sample& b);
    NDDSUSERDllExport void swap(Sample& a, Sample& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

    struct NDDSUSERDllExport NestedValue {
        int32_t value {};

        NestedValue();

        explicit NestedValue(int32_t value_);

    };

    NDDSUSERDllExport bool operator == (const NestedValue& a, const NestedValue& b);
    NDDSUSERDllExport bool operator != (const NestedValue& a, const NestedValue& b);
    NDDSUSERDllExport void swap(NestedValue& a, NestedValue& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const NestedValue& sample);

} // namespace CorpusAnnotationVariants  

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
        struct topic_type_name< ::CorpusAnnotationVariants::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAnnotationVariants::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAnnotationVariants::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAnnotationVariants::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAnnotationVariants::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAnnotationVariants::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAnnotationVariants::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAnnotationVariants::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusAnnotationVariants::NestedValue > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAnnotationVariants::NestedValue";
            }
        };

        template<>
        struct topic_type_support< ::CorpusAnnotationVariants::NestedValue > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAnnotationVariants::NestedValue& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAnnotationVariants::NestedValue& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusAnnotationVariants::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAnnotationVariants::Sample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusAnnotationVariants::NestedValue > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAnnotationVariants::NestedValue > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _10_annotations_extended_192864544_hpp
