

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-enum.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_enum_1535773468_hpp
#define _07_union_enum_1535773468_hpp

#include <iosfwd>

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#include "dds/domain/DomainParticipant.hpp"
#include "dds/topic/TopicTraits.hpp"
#include "dds/core/SafeEnumeration.hpp"
#include "dds/core/String.hpp"
#include "dds/core/array.hpp"
#include "dds/core/vector.hpp"
#include "dds/core/Optional.hpp"
#include "dds/core/xtypes/DynamicType.hpp"
#include "dds/core/xtypes/StructType.hpp"
#include "dds/core/xtypes/UnionType.hpp"
#include "dds/core/xtypes/EnumType.hpp"
#include "dds/core/xtypes/AliasType.hpp"
#include "rti/core/array.hpp"
#include "rti/core/BoundedSequence.hpp"
#include "rti/util/StreamFlagSaver.hpp"
#include "rti/domain/PluginSupport.hpp"
#include "rti/core/LongDouble.hpp"
#include "dds/core/External.hpp"
#include "rti/core/Pointer.hpp"
#include "rti/topic/TopicTraits.hpp"

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

namespace CorpusEnumUnion {
    enum class Kind {
        Number, 
        Text, 
        PayloadValue
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Kind& sample);

    class NDDSUSERDllExport Payload {
      public:
        Payload();

        Payload(int32_t code,const std::string& label);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Payload (Payload&&) = default;
        Payload& operator=(Payload&&) = default;
        Payload& operator=(const Payload&) = default;
        Payload(const Payload&) = default;
        #else
        Payload(Payload&& other_) OMG_NOEXCEPT;  
        Payload& operator=(Payload&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& code() OMG_NOEXCEPT {
            return m_code_;
        }

        const int32_t& code() const OMG_NOEXCEPT {
            return m_code_;
        }

        void code(int32_t value) {
            m_code_ = value;
        }

        std::string& label() OMG_NOEXCEPT {
            return m_label_;
        }

        const std::string& label() const OMG_NOEXCEPT {
            return m_label_;
        }

        void label(const std::string& value) {
            m_label_ = value;
        }

        void label(std::string&& value) {
            m_label_ = std::move(value);
        }

        bool operator == (const Payload& other_) const;
        bool operator != (const Payload& other_) const;

        void swap(Payload& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_code_;
        std::string m_label_;

    };

    inline void swap(Payload& a, Payload& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Payload& sample);

    class NDDSUSERDllExport Choice {
      public:
        Choice();

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Choice (Choice&&) = default;
        Choice& operator=(Choice&&) = default;
        Choice& operator=(const Choice&) = default;
        Choice(const Choice&) = default;
        #else
        Choice(Choice&& other_) OMG_NOEXCEPT;  
        Choice& operator=(Choice&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        CorpusEnumUnion::Kind& _d()  {
            return m_d_;
        }

        const CorpusEnumUnion::Kind& _d() const  {
            return m_d_;
        }

        void _d(const CorpusEnumUnion::Kind& value) {
            m_d_ = value;
        }

        int32_t& number()  {
            if ( _d() != (CorpusEnumUnion::Kind::Number)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusEnumUnion::Choice::number not selected by the discriminator" );
            }
            return m_u_.m_number_;
        }

        const int32_t& number() const  {
            if ( _d() != (CorpusEnumUnion::Kind::Number)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusEnumUnion::Choice::number not selected by the discriminator" );
            }
            return m_u_.m_number_;
        }

        void number(int32_t value) {
            m_u_.m_number_ = value;
            m_d_= (CorpusEnumUnion::Kind::Number);
        }

        std::string& text()  {
            if ( _d() != (CorpusEnumUnion::Kind::Text)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusEnumUnion::Choice::text not selected by the discriminator" );
            }
            return m_u_.m_text_;
        }

        const std::string& text() const  {
            if ( _d() != (CorpusEnumUnion::Kind::Text)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusEnumUnion::Choice::text not selected by the discriminator" );
            }
            return m_u_.m_text_;
        }

        void text(const std::string& value) {
            m_u_.m_text_ = value;
            m_d_= (CorpusEnumUnion::Kind::Text);
        }

        void text(std::string&& value) {
            m_u_.m_text_ = std::move(value);
            m_d_= (CorpusEnumUnion::Kind::Text);
        }
        CorpusEnumUnion::Payload& payload()  {
            if ( _d() != (CorpusEnumUnion::Kind::PayloadValue)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusEnumUnion::Choice::payload not selected by the discriminator" );
            }
            return m_u_.m_payload_;
        }

        const CorpusEnumUnion::Payload& payload() const  {
            if ( _d() != (CorpusEnumUnion::Kind::PayloadValue)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusEnumUnion::Choice::payload not selected by the discriminator" );
            }
            return m_u_.m_payload_;
        }

        void payload(const CorpusEnumUnion::Payload& value) {
            m_u_.m_payload_ = value;
            m_d_= (CorpusEnumUnion::Kind::PayloadValue);
        }

        void payload(CorpusEnumUnion::Payload&& value) {
            m_u_.m_payload_ = std::move(value);
            m_d_= (CorpusEnumUnion::Kind::PayloadValue);
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;

        static CorpusEnumUnion::Kind default_discriminator();  

        void swap(Choice& other_) OMG_NOEXCEPT ;

      private:

        CorpusEnumUnion::Kind m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_number_;
            std::string m_text_;
            CorpusEnumUnion::Payload m_payload_;
            Union_();
            Union_(int32_t number,const std::string& text,const CorpusEnumUnion::Payload& payload);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusEnumUnion  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusEnumUnion::Payload > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusEnumUnion::Payload";
            }
        };

        template<>
        struct is_topic_type< CorpusEnumUnion::Payload > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusEnumUnion::Payload > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusEnumUnion::Payload& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusEnumUnion::Payload& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusEnumUnion::Payload& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusEnumUnion::Payload& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusEnumUnion::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusEnumUnion::Choice";
            }
        };

        template<>
        struct is_topic_type< CorpusEnumUnion::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusEnumUnion::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusEnumUnion::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusEnumUnion::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusEnumUnion::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusEnumUnion::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template <>
        struct default_enumerator<CorpusEnumUnion::Kind>
        {
            static const CorpusEnumUnion::Kind value;
        };
        template<>
        struct dynamic_type< CorpusEnumUnion::Kind > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusEnumUnion::Kind > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusEnumUnion::Payload > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusEnumUnion::Payload > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusEnumUnion::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusEnumUnion::Choice > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

    }
}

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _07_union_enum_1535773468_hpp

