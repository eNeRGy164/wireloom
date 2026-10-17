

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-duplicate-default.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_duplicate_default_1765279741_hpp
#define _07_duplicate_default_1765279741_hpp

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

namespace CorpusNegativeDuplicateDefault {

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

        int32_t& second()  {

            if (!()) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateDefault::Choice::second not selected by the discriminator" );
            }
            return m_u_.second;
        }

        const int32_t& second() const  {

            if (!()) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateDefault::Choice::second not selected by the discriminator" );
            }
            return m_u_.second;
        }

        void second(int32_t value) {

            if (!()) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateDefault::Choice::second not selected by the discriminator" );
            }
            m_u_.second = value;
        }

        void second(int32_t value, int32_t discriminator) {

            if (!()) {
                throw ::dds::core::PreconditionNotMetError(
                    "The discriminator value is not valid for ::CorpusNegativeDuplicateDefault::Choice::second" );
            }
            m_u_.second = value;
            m_d_= discriminator;
        }

        int32_t& first()  {

            if (_d() != default ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateDefault::Choice::first not selected by the discriminator" );
            }
            return m_u_.first;
        }

        const int32_t& first() const  {

            if (_d() != default ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateDefault::Choice::first not selected by the discriminator" );
            }
            return m_u_.first;
        }

        void first(int32_t value) {

            if (_d() != default ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateDefault::Choice::first not selected by the discriminator" );
            }
            m_u_.first = value;
        }

        void first(int32_t value, int32_t discriminator) {

            if (discriminator != default ) {

                throw ::dds::core::PreconditionNotMetError(
                    "The discriminator value is not valid for ::CorpusNegativeDuplicateDefault::Choice::first" );
            }
            m_u_.first = value;
            m_d_= discriminator;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static int32_t default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            int32_t second {};
            int32_t first {};
            Union_();
            Union_(int32_t second_,int32_t first_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusNegativeDuplicateDefault  

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
        struct topic_type_name< ::CorpusNegativeDuplicateDefault::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNegativeDuplicateDefault::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusNegativeDuplicateDefault::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusNegativeDuplicateDefault::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusNegativeDuplicateDefault::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusNegativeDuplicateDefault::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusNegativeDuplicateDefault::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusNegativeDuplicateDefault::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusNegativeDuplicateDefault::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusNegativeDuplicateDefault::Choice > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _07_duplicate_default_1765279741_hpp
