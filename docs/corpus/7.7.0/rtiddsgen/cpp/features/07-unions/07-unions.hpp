

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-unions.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_unions_876006758_hpp
#define _07_unions_876006758_hpp

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

namespace CorpusUnions {

    class NDDSUSERDllExport Choice {
      public:

        Choice();

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
                    "::CorpusUnions::Choice::number not selected by the discriminator" );
            }
            return m_u_.number;
        }

        const int32_t& number() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnions::Choice::number not selected by the discriminator" );
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
                    "::CorpusUnions::Choice::text not selected by the discriminator" );
            }
            return m_u_.text;
        }

        const std::string& text() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnions::Choice::text not selected by the discriminator" );
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
        bool& flag()  {

            if (!(_d() != 0 &&
            _d() != 1 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnions::Choice::flag not selected by the discriminator" );
            }
            return m_u_.flag;
        }

        const bool& flag() const  {

            if (!(_d() != 0 &&
            _d() != 1 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnions::Choice::flag not selected by the discriminator" );
            }
            return m_u_.flag;
        }

        void flag(bool value) {

            if (!(_d() != 0 &&
            _d() != 1 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnions::Choice::flag not selected by the discriminator" );
            }
            m_u_.flag = value;
        }

        void flag(bool value, int32_t discriminator) {

            if (!(discriminator != 0 &&
            discriminator != 1 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "The discriminator value is not valid for ::CorpusUnions::Choice::flag" );
            }
            m_u_.flag = value;
            m_d_= discriminator;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static int32_t default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            int32_t number {};
            std::string text {};
            bool flag {};
            Union_();
            Union_(int32_t number_,const ::omg::types::string_view& text_,bool flag_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusUnions  

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
        struct topic_type_name< ::CorpusUnions::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnions::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnions::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnions::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnions::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnions::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnions::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnions::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusUnions::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusUnions::Choice > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _07_unions_876006758_hpp
