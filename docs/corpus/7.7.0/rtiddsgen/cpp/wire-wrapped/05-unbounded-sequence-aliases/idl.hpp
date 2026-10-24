

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662736227_hpp
#define idl_1662736227_hpp

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

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "features/05-unbounded-sequence-aliases.hpp"
#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif
namespace CorpusUnboundedSequences {

    struct NDDSUSERDllExport SequenceAliasesSample {
        ::CorpusUnboundedSequences::Numbers numbers {};
        ::CorpusUnboundedSequences::States states {};
        ::CorpusUnboundedSequences::Children children {};
        ::CorpusUnboundedSequences::Scalars scalars {};
        ::CorpusUnboundedSequences::Texts texts {};
        ::CorpusUnboundedSequences::WideTexts wideTexts {};
        ::CorpusUnboundedSequences::BoundedTexts boundedTexts {};
        ::CorpusUnboundedSequences::BoundedWideTexts boundedWideTexts {};

        SequenceAliasesSample();

        SequenceAliasesSample(const ::CorpusUnboundedSequences::Numbers& numbers_,const ::CorpusUnboundedSequences::States& states_,const ::CorpusUnboundedSequences::Children& children_,const ::CorpusUnboundedSequences::Scalars& scalars_,const ::CorpusUnboundedSequences::Texts& texts_,const ::CorpusUnboundedSequences::WideTexts& wideTexts_,const ::CorpusUnboundedSequences::BoundedTexts& boundedTexts_,const ::CorpusUnboundedSequences::BoundedWideTexts& boundedWideTexts_);

    };

    NDDSUSERDllExport bool operator == (const SequenceAliasesSample& a, const SequenceAliasesSample& b);
    NDDSUSERDllExport bool operator != (const SequenceAliasesSample& a, const SequenceAliasesSample& b);
    NDDSUSERDllExport void swap(SequenceAliasesSample& a, SequenceAliasesSample& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const SequenceAliasesSample& sample);

} // namespace CorpusUnboundedSequences  

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
        struct topic_type_name< ::CorpusUnboundedSequences::SequenceAliasesSample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnboundedSequences::SequenceAliasesSample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnboundedSequences::SequenceAliasesSample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnboundedSequences::SequenceAliasesSample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnboundedSequences::SequenceAliasesSample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnboundedSequences::SequenceAliasesSample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnboundedSequences::SequenceAliasesSample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnboundedSequences::SequenceAliasesSample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusUnboundedSequences::SequenceAliasesSample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusUnboundedSequences::SequenceAliasesSample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // idl_1662736227_hpp
