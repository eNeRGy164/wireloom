

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-boundaries.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_boundaries_919417863_hpp
#define _08_key_boundaries_919417863_hpp

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

namespace CorpusKeyBoundaries {

    class NDDSUSERDllExport ArrayKey {
      public:

        ArrayKey();

        ArrayKey(const ::dds::core::array< int32_t, 2L>& coordinates_,int32_t payload_);

        ::dds::core::array< int32_t, 2L>& coordinates() noexcept {
            return m_coordinates_;
        }

        const ::dds::core::array< int32_t, 2L>& coordinates() const noexcept {
            return m_coordinates_;
        }

        void coordinates(const ::dds::core::array< int32_t, 2L>& value) {

            m_coordinates_ = value;
        }

        void coordinates(::dds::core::array< int32_t, 2L>&& value) {
            m_coordinates_ = std::move(value);
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

        bool operator == (const ArrayKey& other_) const;
        bool operator != (const ArrayKey& other_) const;

        void swap(ArrayKey& other_) noexcept ;

      private:

        ::dds::core::array< int32_t, 2L> m_coordinates_ {};
        int32_t m_payload_;

    };

    inline void swap(ArrayKey& a, ArrayKey& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ArrayKey& sample);

    class NDDSUSERDllExport BoundedStringKey {
      public:

        BoundedStringKey();

        BoundedStringKey(const std::string& name_,int32_t payload_);

        std::string& name() noexcept {
            return m_name_;
        }

        const std::string& name() const noexcept {
            return m_name_;
        }

        void name(const std::string& value) {

            m_name_ = value;
        }

        void name(std::string&& value) {
            m_name_ = std::move(value);
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

        bool operator == (const BoundedStringKey& other_) const;
        bool operator != (const BoundedStringKey& other_) const;

        void swap(BoundedStringKey& other_) noexcept ;

      private:

        std::string m_name_;
        int32_t m_payload_;

    };

    inline void swap(BoundedStringKey& a, BoundedStringKey& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const BoundedStringKey& sample);

    class NDDSUSERDllExport UnboundedStringKey {
      public:

        UnboundedStringKey();

        UnboundedStringKey(const std::string& name_,int32_t payload_);

        std::string& name() noexcept {
            return m_name_;
        }

        const std::string& name() const noexcept {
            return m_name_;
        }

        void name(const std::string& value) {

            m_name_ = value;
        }

        void name(std::string&& value) {
            m_name_ = std::move(value);
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

        bool operator == (const UnboundedStringKey& other_) const;
        bool operator != (const UnboundedStringKey& other_) const;

        void swap(UnboundedStringKey& other_) noexcept ;

      private:

        std::string m_name_;
        int32_t m_payload_;

    };

    inline void swap(UnboundedStringKey& a, UnboundedStringKey& b)  noexcept 
    {
        a.swap(b);
    }

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
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _08_key_boundaries_919417863_hpp

