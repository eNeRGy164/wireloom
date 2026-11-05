

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_optional_aggregate_member_916542322_hpp
#define _09_optional_aggregate_member_916542322_hpp

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

namespace CorpusOptionalAggregate {

    class NDDSUSERDllExport Payload {
      public:
        Payload();

        explicit Payload(int32_t value);

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

        int32_t& value() OMG_NOEXCEPT {
            return m_value_;
        }

        const int32_t& value() const OMG_NOEXCEPT {
            return m_value_;
        }

        void value(int32_t value) {
            m_value_ = value;
        }

        bool operator == (const Payload& other_) const;
        bool operator != (const Payload& other_) const;

        void swap(Payload& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_value_;

    };

    inline void swap(Payload& a, Payload& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Payload& sample);

    typedef CorpusOptionalAggregate::Payload PayloadAlias;
    struct PayloadAlias_AliasTag_t {};

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

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {
            m_d_ = value;
        }

        CorpusOptionalAggregate::Payload& payload()  {
            if ( _d() != 0) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusOptionalAggregate::Choice::payload not selected by the discriminator" );
            }
            return m_u_.m_payload_;
        }

        const CorpusOptionalAggregate::Payload& payload() const  {
            if ( _d() != 0) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusOptionalAggregate::Choice::payload not selected by the discriminator" );
            }
            return m_u_.m_payload_;
        }

        void payload(const CorpusOptionalAggregate::Payload& value) {
            m_u_.m_payload_ = value;
            m_d_= 0;
        }

        void payload(CorpusOptionalAggregate::Payload&& value) {
            m_u_.m_payload_ = std::move(value);
            m_d_= 0;
        }
        int32_t& number()  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusOptionalAggregate::Choice::number not selected by the discriminator" );
            }
            return m_u_.m_number_;
        }

        const int32_t& number() const  {
            if ( _d() != 1) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusOptionalAggregate::Choice::number not selected by the discriminator" );
            }
            return m_u_.m_number_;
        }

        void number(int32_t value) {
            m_u_.m_number_ = value;
            m_d_= 1;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;

        static int32_t default_discriminator();  

        void swap(Choice& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            CorpusOptionalAggregate::Payload m_payload_;
            int32_t m_number_;
            Union_();
            Union_(const CorpusOptionalAggregate::Payload& payload,int32_t number);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

    typedef CorpusOptionalAggregate::Choice ChoiceAlias;
    struct ChoiceAlias_AliasTag_t {};

    class NDDSUSERDllExport Holder {
      public:
        Holder();

        Holder(const ::dds::core::optional< CorpusOptionalAggregate::Payload >& payload,const ::dds::core::optional< CorpusOptionalAggregate::Payload >& payloadAlias,const ::dds::core::optional< CorpusOptionalAggregate::Choice >& choice,const ::dds::core::optional< CorpusOptionalAggregate::Choice >& choiceAlias);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Holder (Holder&&) = default;
        Holder& operator=(Holder&&) = default;
        Holder& operator=(const Holder&) = default;
        Holder(const Holder&) = default;
        #else
        Holder(Holder&& other_) OMG_NOEXCEPT;  
        Holder& operator=(Holder&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        ::dds::core::optional< CorpusOptionalAggregate::Payload >& payload() OMG_NOEXCEPT {
            return m_payload_;
        }

        const ::dds::core::optional< CorpusOptionalAggregate::Payload >& payload() const OMG_NOEXCEPT {
            return m_payload_;
        }

        void payload(const ::dds::core::optional< CorpusOptionalAggregate::Payload >& value) {
            m_payload_ = value;
        }

        void payload(::dds::core::optional< CorpusOptionalAggregate::Payload >&& value) {
            m_payload_ = std::move(value);
        }
        ::dds::core::optional< CorpusOptionalAggregate::Payload >& payloadAlias() OMG_NOEXCEPT {
            return m_payloadAlias_;
        }

        const ::dds::core::optional< CorpusOptionalAggregate::Payload >& payloadAlias() const OMG_NOEXCEPT {
            return m_payloadAlias_;
        }

        void payloadAlias(const ::dds::core::optional< CorpusOptionalAggregate::Payload >& value) {
            m_payloadAlias_ = value;
        }

        void payloadAlias(::dds::core::optional< CorpusOptionalAggregate::Payload >&& value) {
            m_payloadAlias_ = std::move(value);
        }
        ::dds::core::optional< CorpusOptionalAggregate::Choice >& choice() OMG_NOEXCEPT {
            return m_choice_;
        }

        const ::dds::core::optional< CorpusOptionalAggregate::Choice >& choice() const OMG_NOEXCEPT {
            return m_choice_;
        }

        void choice(const ::dds::core::optional< CorpusOptionalAggregate::Choice >& value) {
            m_choice_ = value;
        }

        void choice(::dds::core::optional< CorpusOptionalAggregate::Choice >&& value) {
            m_choice_ = std::move(value);
        }
        ::dds::core::optional< CorpusOptionalAggregate::Choice >& choiceAlias() OMG_NOEXCEPT {
            return m_choiceAlias_;
        }

        const ::dds::core::optional< CorpusOptionalAggregate::Choice >& choiceAlias() const OMG_NOEXCEPT {
            return m_choiceAlias_;
        }

        void choiceAlias(const ::dds::core::optional< CorpusOptionalAggregate::Choice >& value) {
            m_choiceAlias_ = value;
        }

        void choiceAlias(::dds::core::optional< CorpusOptionalAggregate::Choice >&& value) {
            m_choiceAlias_ = std::move(value);
        }

        bool operator == (const Holder& other_) const;
        bool operator != (const Holder& other_) const;

        void swap(Holder& other_) OMG_NOEXCEPT ;

      private:

        ::dds::core::optional< CorpusOptionalAggregate::Payload > m_payload_;
        ::dds::core::optional< CorpusOptionalAggregate::Payload > m_payloadAlias_;
        ::dds::core::optional< CorpusOptionalAggregate::Choice > m_choice_;
        ::dds::core::optional< CorpusOptionalAggregate::Choice > m_choiceAlias_;

    };

    inline void swap(Holder& a, Holder& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Holder& sample);

} // namespace CorpusOptionalAggregate  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusOptionalAggregate::Payload > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalAggregate::Payload";
            }
        };

        template<>
        struct is_topic_type< CorpusOptionalAggregate::Payload > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusOptionalAggregate::Payload > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusOptionalAggregate::Payload& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusOptionalAggregate::Payload& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusOptionalAggregate::Payload& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusOptionalAggregate::Payload& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusOptionalAggregate::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalAggregate::Choice";
            }
        };

        template<>
        struct is_topic_type< CorpusOptionalAggregate::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusOptionalAggregate::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusOptionalAggregate::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusOptionalAggregate::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusOptionalAggregate::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusOptionalAggregate::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusOptionalAggregate::Holder > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalAggregate::Holder";
            }
        };

        template<>
        struct is_topic_type< CorpusOptionalAggregate::Holder > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusOptionalAggregate::Holder > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusOptionalAggregate::Holder& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusOptionalAggregate::Holder& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusOptionalAggregate::Holder& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusOptionalAggregate::Holder& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusOptionalAggregate::Payload > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusOptionalAggregate::Payload > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusOptionalAggregate::PayloadAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusOptionalAggregate::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusOptionalAggregate::Choice > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusOptionalAggregate::ChoiceAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusOptionalAggregate::Holder > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusOptionalAggregate::Holder > {
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

#endif // _09_optional_aggregate_member_916542322_hpp

