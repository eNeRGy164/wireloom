

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-nested.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_nested_1591529600_hpp
#define _08_key_nested_1591529600_hpp

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

namespace CorpusNestedKeys {

    class NDDSUSERDllExport Inner {
      public:

        Inner();

        Inner(int32_t code_,int32_t revision_);

        int32_t& code() noexcept {
            return m_code_;
        }

        const int32_t& code() const noexcept {
            return m_code_;
        }

        void code(int32_t value) {

            m_code_ = value;
        }

        int32_t& revision() noexcept {
            return m_revision_;
        }

        const int32_t& revision() const noexcept {
            return m_revision_;
        }

        void revision(int32_t value) {

            m_revision_ = value;
        }

        bool operator == (const Inner& other_) const;
        bool operator != (const Inner& other_) const;

        void swap(Inner& other_) noexcept ;

      private:

        int32_t m_code_;
        int32_t m_revision_;

    };

    inline void swap(Inner& a, Inner& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Inner& sample);

    class NDDSUSERDllExport Outer {
      public:

        Outer();

        Outer(const ::CorpusNestedKeys::Inner& identity_,int32_t payload_);

        ::CorpusNestedKeys::Inner& identity() noexcept {
            return m_identity_;
        }

        const ::CorpusNestedKeys::Inner& identity() const noexcept {
            return m_identity_;
        }

        void identity(const ::CorpusNestedKeys::Inner& value) {

            m_identity_ = value;
        }

        void identity(::CorpusNestedKeys::Inner&& value) {
            m_identity_ = std::move(value);
        }
        int32_t& payload() noexcept {
            return m_payload_;
        }

        const int32_t& payload() const noexcept {
            return m_payload_;
        }

        void payload(int32_t value) {

            m_payload_ = value;
        }

        bool operator == (const Outer& other_) const;
        bool operator != (const Outer& other_) const;

        void swap(Outer& other_) noexcept ;

      private:

        ::CorpusNestedKeys::Inner m_identity_;
        int32_t m_payload_;

    };

    inline void swap(Outer& a, Outer& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Outer& sample);

} // namespace CorpusNestedKeys  

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
        struct topic_type_name< ::CorpusNestedKeys::Inner > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNestedKeys::Inner";
            }
        };

        template<>
        struct is_topic_type< ::CorpusNestedKeys::Inner > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusNestedKeys::Inner > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusNestedKeys::Inner& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusNestedKeys::Inner& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusNestedKeys::Inner& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusNestedKeys::Inner& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusNestedKeys::Outer > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNestedKeys::Outer";
            }
        };

        template<>
        struct is_topic_type< ::CorpusNestedKeys::Outer > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusNestedKeys::Outer > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusNestedKeys::Outer& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusNestedKeys::Outer& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusNestedKeys::Outer& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusNestedKeys::Outer& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusNestedKeys::Inner > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusNestedKeys::Inner > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusNestedKeys::Outer > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusNestedKeys::Outer > {
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

#endif // _08_key_nested_1591529600_hpp

