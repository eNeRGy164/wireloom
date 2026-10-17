

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-inherited.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_inherited_663625449_hpp
#define _08_key_inherited_663625449_hpp

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

namespace CorpusInheritedKeys {

    class NDDSUSERDllExport Base {
      public:

        Base();

        Base(int32_t tenant_,int32_t baseValue_);

        int32_t& tenant() noexcept {
            return m_tenant_;
        }

        const int32_t& tenant() const noexcept {
            return m_tenant_;
        }

        void tenant(int32_t value) {

            m_tenant_ = value;
        }

        int32_t& baseValue() noexcept {
            return m_baseValue_;
        }

        const int32_t& baseValue() const noexcept {
            return m_baseValue_;
        }

        void baseValue(int32_t value) {

            m_baseValue_ = value;
        }

        bool operator == (const Base& other_) const;
        bool operator != (const Base& other_) const;

        void swap(Base& other_) noexcept ;

      private:

        int32_t m_tenant_;
        int32_t m_baseValue_;

    };

    inline void swap(Base& a, Base& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Base& sample);

    class NDDSUSERDllExport Derived
    : public ::CorpusInheritedKeys::Base {
      public:

        Derived();

        Derived(int32_t tenant_,int32_t baseValue_,int32_t localId_,int32_t payload_);

        int32_t& localId() noexcept {
            return m_localId_;
        }

        const int32_t& localId() const noexcept {
            return m_localId_;
        }

        void localId(int32_t value) {

            m_localId_ = value;
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

        bool operator == (const Derived& other_) const;
        bool operator != (const Derived& other_) const;

        void swap(Derived& other_) noexcept ;

      private:

        int32_t m_localId_;
        int32_t m_payload_;

    };

    inline void swap(Derived& a, Derived& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Derived& sample);

} // namespace CorpusInheritedKeys  

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
        struct topic_type_name< ::CorpusInheritedKeys::Base > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInheritedKeys::Base";
            }
        };

        template<>
        struct is_topic_type< ::CorpusInheritedKeys::Base > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusInheritedKeys::Base > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusInheritedKeys::Base& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusInheritedKeys::Base& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInheritedKeys::Base& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInheritedKeys::Base& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInheritedKeys::Derived > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInheritedKeys::Derived";
            }
        };

        template<>
        struct is_topic_type< ::CorpusInheritedKeys::Derived > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusInheritedKeys::Derived > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusInheritedKeys::Derived& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusInheritedKeys::Derived& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInheritedKeys::Derived& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInheritedKeys::Derived& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusInheritedKeys::Base > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInheritedKeys::Base > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInheritedKeys::Derived > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInheritedKeys::Derived > {
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

#endif // _08_key_inherited_663625449_hpp

