

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662736227_hpp
#define idl_1662736227_hpp

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

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "features/05-unbounded-sequence-aliases.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif
namespace CorpusUnboundedSequences {

    class NDDSUSERDllExport SequenceAliasesSample {
      public:

        SequenceAliasesSample();

        SequenceAliasesSample(const ::CorpusUnboundedSequences::Numbers& numbers_,const ::CorpusUnboundedSequences::States& states_,const ::CorpusUnboundedSequences::Children& children_,const ::CorpusUnboundedSequences::Scalars& scalars_,const ::CorpusUnboundedSequences::Texts& texts_,const ::CorpusUnboundedSequences::WideTexts& wideTexts_,const ::CorpusUnboundedSequences::BoundedTexts& boundedTexts_,const ::CorpusUnboundedSequences::BoundedWideTexts& boundedWideTexts_);

        ::CorpusUnboundedSequences::Numbers& numbers() noexcept {
            return m_numbers_;
        }

        const ::CorpusUnboundedSequences::Numbers& numbers() const noexcept {
            return m_numbers_;
        }

        void numbers(const ::CorpusUnboundedSequences::Numbers& value) {

            m_numbers_ = value;
        }

        void numbers(::CorpusUnboundedSequences::Numbers&& value) {
            m_numbers_ = std::move(value);
        }
        ::CorpusUnboundedSequences::States& states() noexcept {
            return m_states_;
        }

        const ::CorpusUnboundedSequences::States& states() const noexcept {
            return m_states_;
        }

        void states(const ::CorpusUnboundedSequences::States& value) {

            m_states_ = value;
        }

        void states(::CorpusUnboundedSequences::States&& value) {
            m_states_ = std::move(value);
        }
        ::CorpusUnboundedSequences::Children& children() noexcept {
            return m_children_;
        }

        const ::CorpusUnboundedSequences::Children& children() const noexcept {
            return m_children_;
        }

        void children(const ::CorpusUnboundedSequences::Children& value) {

            m_children_ = value;
        }

        void children(::CorpusUnboundedSequences::Children&& value) {
            m_children_ = std::move(value);
        }
        ::CorpusUnboundedSequences::Scalars& scalars() noexcept {
            return m_scalars_;
        }

        const ::CorpusUnboundedSequences::Scalars& scalars() const noexcept {
            return m_scalars_;
        }

        void scalars(const ::CorpusUnboundedSequences::Scalars& value) {

            m_scalars_ = value;
        }

        void scalars(::CorpusUnboundedSequences::Scalars&& value) {
            m_scalars_ = std::move(value);
        }
        ::CorpusUnboundedSequences::Texts& texts() noexcept {
            return m_texts_;
        }

        const ::CorpusUnboundedSequences::Texts& texts() const noexcept {
            return m_texts_;
        }

        void texts(const ::CorpusUnboundedSequences::Texts& value) {

            m_texts_ = value;
        }

        void texts(::CorpusUnboundedSequences::Texts&& value) {
            m_texts_ = std::move(value);
        }
        ::CorpusUnboundedSequences::WideTexts& wideTexts() noexcept {
            return m_wideTexts_;
        }

        const ::CorpusUnboundedSequences::WideTexts& wideTexts() const noexcept {
            return m_wideTexts_;
        }

        void wideTexts(const ::CorpusUnboundedSequences::WideTexts& value) {

            m_wideTexts_ = value;
        }

        void wideTexts(::CorpusUnboundedSequences::WideTexts&& value) {
            m_wideTexts_ = std::move(value);
        }
        ::CorpusUnboundedSequences::BoundedTexts& boundedTexts() noexcept {
            return m_boundedTexts_;
        }

        const ::CorpusUnboundedSequences::BoundedTexts& boundedTexts() const noexcept {
            return m_boundedTexts_;
        }

        void boundedTexts(const ::CorpusUnboundedSequences::BoundedTexts& value) {

            m_boundedTexts_ = value;
        }

        void boundedTexts(::CorpusUnboundedSequences::BoundedTexts&& value) {
            m_boundedTexts_ = std::move(value);
        }
        ::CorpusUnboundedSequences::BoundedWideTexts& boundedWideTexts() noexcept {
            return m_boundedWideTexts_;
        }

        const ::CorpusUnboundedSequences::BoundedWideTexts& boundedWideTexts() const noexcept {
            return m_boundedWideTexts_;
        }

        void boundedWideTexts(const ::CorpusUnboundedSequences::BoundedWideTexts& value) {

            m_boundedWideTexts_ = value;
        }

        void boundedWideTexts(::CorpusUnboundedSequences::BoundedWideTexts&& value) {
            m_boundedWideTexts_ = std::move(value);
        }
        bool operator == (const SequenceAliasesSample& other_) const;
        bool operator != (const SequenceAliasesSample& other_) const;

        void swap(SequenceAliasesSample& other_) noexcept ;

      private:

        ::CorpusUnboundedSequences::Numbers m_numbers_;
        ::CorpusUnboundedSequences::States m_states_;
        ::CorpusUnboundedSequences::Children m_children_;
        ::CorpusUnboundedSequences::Scalars m_scalars_;
        ::CorpusUnboundedSequences::Texts m_texts_;
        ::CorpusUnboundedSequences::WideTexts m_wideTexts_;
        ::CorpusUnboundedSequences::BoundedTexts m_boundedTexts_;
        ::CorpusUnboundedSequences::BoundedWideTexts m_boundedWideTexts_;

    };

    inline void swap(SequenceAliasesSample& a, SequenceAliasesSample& b)  noexcept 
    {
        a.swap(b);
    }

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
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // idl_1662736227_hpp

