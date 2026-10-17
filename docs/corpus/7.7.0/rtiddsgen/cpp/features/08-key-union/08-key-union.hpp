

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-union.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_key_union_1909099165_hpp
#define _08_key_union_1909099165_hpp

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

namespace CorpusUnionKeys {

    class NDDSUSERDllExport Identity {
      public:

        Identity();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        int32_t& number()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionKeys::Identity::number not selected by the discriminator" );
            }
            return m_u_.number;
        }

        const int32_t& number() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionKeys::Identity::number not selected by the discriminator" );
            }
            return m_u_.number;
        }

        void number(int32_t value) {
            m_u_.number = value;
            m_d_= 0;
        }

        std::string& text()  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionKeys::Identity::text not selected by the discriminator" );
            }
            return m_u_.text;
        }

        const std::string& text() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionKeys::Identity::text not selected by the discriminator" );
            }
            return m_u_.text;
        }

        void text(const ::omg::types::string_view& value) {
            m_u_.text = value;
            m_d_= 1;
        }

        void text(::omg::types::string_view&& value) {
            m_u_.text = std::move(value);
            m_d_= 1;
        }
        void _default() {
            _d(::CorpusUnionKeys::Identity::default_discriminator());
        }

        bool operator == (const Identity& other_) const;
        bool operator != (const Identity& other_) const;
        static int32_t default_discriminator();

        void swap(Identity& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            int32_t number {};
            std::string text {};
            Union_();
            Union_(int32_t number_,const ::omg::types::string_view& text_);
        };
        Union_ m_u_;

    };

    inline void swap(Identity& a, Identity& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Identity& sample);

    struct NDDSUSERDllExport Sample {
        ::CorpusUnionKeys::Identity identity {};
        int32_t payload {};

        Sample();

        Sample(const ::CorpusUnionKeys::Identity& identity_,int32_t payload_);

    };

    NDDSUSERDllExport bool operator == (const Sample& a, const Sample& b);
    NDDSUSERDllExport bool operator != (const Sample& a, const Sample& b);
    NDDSUSERDllExport void swap(Sample& a, Sample& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusUnionKeys  

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
        struct topic_type_name< ::CorpusUnionKeys::Identity > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnionKeys::Identity";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnionKeys::Identity > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnionKeys::Identity > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnionKeys::Identity& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnionKeys::Identity& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnionKeys::Identity& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnionKeys::Identity& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusUnionKeys::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnionKeys::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnionKeys::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnionKeys::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnionKeys::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnionKeys::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnionKeys::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnionKeys::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusUnionKeys::Identity > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusUnionKeys::Identity > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusUnionKeys::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusUnionKeys::Sample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _08_key_union_1909099165_hpp
