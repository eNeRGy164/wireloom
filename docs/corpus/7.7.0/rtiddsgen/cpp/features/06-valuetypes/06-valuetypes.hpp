

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-valuetypes.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _06_valuetypes_932148422_hpp
#define _06_valuetypes_932148422_hpp

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

namespace CorpusValueTypes {

    class NDDSUSERDllExport BaseValue {
      public:

        BaseValue();

        explicit BaseValue(int32_t m_baseValue__);

        int32_t& baseValue() noexcept {
            return m_baseValue_;
        }

        const int32_t& baseValue() const noexcept {
            return m_baseValue_;
        }

        void baseValue(int32_t value) {

            m_baseValue_ = value;
        }

        bool operator == (const BaseValue& other_) const;
        bool operator != (const BaseValue& other_) const;

        void swap(BaseValue& other_) noexcept ;

      private:

        int32_t m_baseValue_ {};

    };

    inline void swap(BaseValue& a, BaseValue& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const BaseValue& sample);

    class NDDSUSERDllExport DerivedValue
    : public ::CorpusValueTypes::BaseValue {
      public:

        DerivedValue();

        explicit DerivedValue(int32_t m_baseValue__,const ::omg::types::string_view& m_name__);

        std::string& name() noexcept {
            return m_name_;
        }

        const std::string& name() const noexcept {
            return m_name_;
        }

        void name(const ::omg::types::string_view& value) {

            m_name_ = value;
        }

        void name(::omg::types::string_view&& value) {
            m_name_ = std::move(value);
        }
        bool operator == (const DerivedValue& other_) const;
        bool operator != (const DerivedValue& other_) const;

        void swap(DerivedValue& other_) noexcept ;

      private:

        std::string m_name_ {};

    };

    inline void swap(DerivedValue& a, DerivedValue& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const DerivedValue& sample);

    struct NDDSUSERDllExport Holder {
        ::CorpusValueTypes::DerivedValue value {};

        Holder();

        explicit Holder(const ::CorpusValueTypes::DerivedValue& value_);

    };

    NDDSUSERDllExport bool operator == (const Holder& a, const Holder& b);
    NDDSUSERDllExport bool operator != (const Holder& a, const Holder& b);
    NDDSUSERDllExport void swap(Holder& a, Holder& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Holder& sample);

} // namespace CorpusValueTypes  

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
        struct topic_type_name< ::CorpusValueTypes::BaseValue > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusValueTypes::BaseValue";
            }
        };

        template<>
        struct is_topic_type< ::CorpusValueTypes::BaseValue > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusValueTypes::BaseValue > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusValueTypes::BaseValue& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusValueTypes::BaseValue& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusValueTypes::BaseValue& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusValueTypes::BaseValue& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusValueTypes::DerivedValue > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusValueTypes::DerivedValue";
            }
        };

        template<>
        struct is_topic_type< ::CorpusValueTypes::DerivedValue > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusValueTypes::DerivedValue > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusValueTypes::DerivedValue& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusValueTypes::DerivedValue& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusValueTypes::DerivedValue& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusValueTypes::DerivedValue& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusValueTypes::Holder > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusValueTypes::Holder";
            }
        };

        template<>
        struct is_topic_type< ::CorpusValueTypes::Holder > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusValueTypes::Holder > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusValueTypes::Holder& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusValueTypes::Holder& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusValueTypes::Holder& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusValueTypes::Holder& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusValueTypes::BaseValue > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusValueTypes::BaseValue > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusValueTypes::DerivedValue > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusValueTypes::DerivedValue > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusValueTypes::Holder > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusValueTypes::Holder > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _06_valuetypes_932148422_hpp
