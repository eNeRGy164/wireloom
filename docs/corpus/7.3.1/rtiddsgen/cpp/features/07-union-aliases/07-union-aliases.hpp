

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_aliases_1295009180_hpp
#define _07_union_aliases_1295009180_hpp

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

namespace CorpusUnionAliases {
    enum class Kind {
        Zero = 0, 
        Ten = 10
    };

    NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Kind& sample);

    class NDDSUSERDllExport Choice {
      public:

        Choice();

        ::CorpusUnionAliases::Kind& _d()  {
            return m_d_;
        }

        const ::CorpusUnionAliases::Kind& _d() const  {
            return m_d_;
        }

        void _d(::CorpusUnionAliases::Kind value) {

            m_d_ = value;
        }

        int32_t& zero()  {

            if (_d() != (CorpusUnionAliases::Kind::Zero) ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionAliases::Choice::zero not selected by the discriminator" );
            }
            return m_u_.m_zero_;
        }

        const int32_t& zero() const  {

            if (_d() != (CorpusUnionAliases::Kind::Zero) ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionAliases::Choice::zero not selected by the discriminator" );
            }
            return m_u_.m_zero_;
        }

        void zero(int32_t value) {
            m_u_.m_zero_ = value;
            m_d_= (CorpusUnionAliases::Kind::Zero);
        }

        std::string& ten()  {

            if (_d() != (CorpusUnionAliases::Kind::Ten) ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionAliases::Choice::ten not selected by the discriminator" );
            }
            return m_u_.m_ten_;
        }

        const std::string& ten() const  {

            if (_d() != (CorpusUnionAliases::Kind::Ten) ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionAliases::Choice::ten not selected by the discriminator" );
            }
            return m_u_.m_ten_;
        }

        void ten(const std::string& value) {
            m_u_.m_ten_ = value;
            m_d_= (CorpusUnionAliases::Kind::Ten);
        }

        void ten(std::string&& value) {
            m_u_.m_ten_ = std::move(value);
            m_d_= (CorpusUnionAliases::Kind::Ten);
        }
        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static ::CorpusUnionAliases::Kind default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        ::CorpusUnionAliases::Kind m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_zero_;
            std::string m_ten_;
            Union_();
            Union_(int32_t zero_,const std::string& ten_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

    typedef ::CorpusUnionAliases::Choice ChoiceAlias;
    struct ChoiceAlias_AliasTag_t {};

    typedef ::CorpusUnionAliases::Choice ChoiceAlias2;
    struct ChoiceAlias2_AliasTag_t {};

    class NDDSUSERDllExport Holder {
      public:

        Holder();

        explicit Holder(const ::CorpusUnionAliases::Choice& value_);

        ::CorpusUnionAliases::Choice& value() noexcept {
            return m_value_;
        }

        const ::CorpusUnionAliases::Choice& value() const noexcept {
            return m_value_;
        }

        void value(const ::CorpusUnionAliases::Choice& value) {

            m_value_ = value;
        }

        void value(::CorpusUnionAliases::Choice&& value) {
            m_value_ = std::move(value);
        }
        bool operator == (const Holder& other_) const;
        bool operator != (const Holder& other_) const;

        void swap(Holder& other_) noexcept ;

      private:

        ::CorpusUnionAliases::Choice m_value_;

    };

    inline void swap(Holder& a, Holder& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Holder& sample);

} // namespace CorpusUnionAliases  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
        template <>
        struct default_enumerator< ::CorpusUnionAliases::Kind>
        {
            static const ::CorpusUnionAliases::Kind value;
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
        struct topic_type_name< ::CorpusUnionAliases::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnionAliases::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnionAliases::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnionAliases::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnionAliases::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnionAliases::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnionAliases::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnionAliases::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusUnionAliases::Holder > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnionAliases::Holder";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnionAliases::Holder > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnionAliases::Holder > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnionAliases::Holder& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnionAliases::Holder& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnionAliases::Holder& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnionAliases::Holder& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template <>
        struct default_enumerator< ::CorpusUnionAliases::Kind>
        {
            static const ::CorpusUnionAliases::Kind value;
        };
        template<>
        struct dynamic_type< ::CorpusUnionAliases::Kind > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };

        template <>
        struct extensibility< ::CorpusUnionAliases::Kind > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusUnionAliases::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusUnionAliases::Choice > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusUnionAliases::ChoiceAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusUnionAliases::ChoiceAlias2_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusUnionAliases::Holder > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusUnionAliases::Holder > {
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

#endif // _07_union_aliases_1295009180_hpp

