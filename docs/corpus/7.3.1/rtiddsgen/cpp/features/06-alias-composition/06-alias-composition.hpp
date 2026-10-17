

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-alias-composition.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _06_alias_composition_39749492_hpp
#define _06_alias_composition_39749492_hpp

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

namespace CorpusAggregateComposition {

    typedef int32_t Identifier;
    struct Identifier_AliasTag_t {};
    enum class State {
        idle, 
        run
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const State& sample);

    static const int32_t Max = 4L;

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< float >;
    template class NDDSUSERDllExport std::vector< float >;
    #endif
    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(int32_t id_,const ::CorpusAggregateComposition::State& state_,const ::rti::core::bounded_sequence< float, (CorpusAggregateComposition::Max) >& values_);

        int32_t& id() noexcept {
            return m_id_;
        }

        const int32_t& id() const noexcept {
            return m_id_;
        }

        void id(int32_t value) {

            m_id_ = value;
        }

        ::CorpusAggregateComposition::State& state() noexcept {
            return m_state_;
        }

        const ::CorpusAggregateComposition::State& state() const noexcept {
            return m_state_;
        }

        void state(const ::CorpusAggregateComposition::State& value) {

            m_state_ = value;
        }

        void state(::CorpusAggregateComposition::State&& value) {
            m_state_ = std::move(value);
        }
        ::rti::core::bounded_sequence< float, (CorpusAggregateComposition::Max) >& values() noexcept {
            return m_values_;
        }

        const ::rti::core::bounded_sequence< float, (CorpusAggregateComposition::Max) >& values() const noexcept {
            return m_values_;
        }

        void values(const ::rti::core::bounded_sequence< float, (CorpusAggregateComposition::Max) >& value) {

            m_values_ = value;
        }

        void values(::rti::core::bounded_sequence< float, (CorpusAggregateComposition::Max) >&& value) {
            m_values_ = std::move(value);
        }
        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        int32_t m_id_;
        ::CorpusAggregateComposition::State m_state_;
        ::rti::core::bounded_sequence< float, (CorpusAggregateComposition::Max) > m_values_;

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

    class NDDSUSERDllExport Named {
      public:

        Named();

        explicit Named(const ::CorpusAggregateComposition::Sample& sample_);

        ::CorpusAggregateComposition::Sample& sample() noexcept {
            return m_sample_;
        }

        const ::CorpusAggregateComposition::Sample& sample() const noexcept {
            return m_sample_;
        }

        void sample(const ::CorpusAggregateComposition::Sample& value) {

            m_sample_ = value;
        }

        void sample(::CorpusAggregateComposition::Sample&& value) {
            m_sample_ = std::move(value);
        }
        bool operator == (const Named& other_) const;
        bool operator != (const Named& other_) const;

        void swap(Named& other_) noexcept ;

      private:

        ::CorpusAggregateComposition::Sample m_sample_;

    };

    inline void swap(Named& a, Named& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Named& sample);

} // namespace CorpusAggregateComposition  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
        template <>
        struct default_enumerator< ::CorpusAggregateComposition::State>
        {
            static const ::CorpusAggregateComposition::State value;
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
        struct topic_type_name< ::CorpusAggregateComposition::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregateComposition::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAggregateComposition::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAggregateComposition::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAggregateComposition::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAggregateComposition::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAggregateComposition::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAggregateComposition::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusAggregateComposition::Named > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusAggregateComposition::Named";
            }
        };

        template<>
        struct is_topic_type< ::CorpusAggregateComposition::Named > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusAggregateComposition::Named > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusAggregateComposition::Named& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusAggregateComposition::Named& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusAggregateComposition::Named& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusAggregateComposition::Named& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusAggregateComposition::Identifier_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template <>
        struct default_enumerator< ::CorpusAggregateComposition::State>
        {
            static const ::CorpusAggregateComposition::State value;
        };
        template<>
        struct dynamic_type< ::CorpusAggregateComposition::State > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };

        template <>
        struct extensibility< ::CorpusAggregateComposition::State > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusAggregateComposition::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAggregateComposition::Sample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusAggregateComposition::Named > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusAggregateComposition::Named > {
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

#endif // _06_alias_composition_39749492_hpp

