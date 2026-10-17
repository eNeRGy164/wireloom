

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-short.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_short_1861173159_hpp
#define _07_union_short_1861173159_hpp

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

namespace CorpusShortUnion {

    class NDDSUSERDllExport Choice {
      public:

        Choice();

        int16_t& _d()  {
            return m_d_;
        }

        const int16_t& _d() const  {
            return m_d_;
        }

        void _d(int16_t value) {

            m_d_ = value;
        }

        int32_t& negative()  {

            if (_d() != -1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::negative not selected by the discriminator" );
            }
            return m_u_.m_negative_;
        }

        const int32_t& negative() const  {

            if (_d() != -1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::negative not selected by the discriminator" );
            }
            return m_u_.m_negative_;
        }

        void negative(int32_t value) {
            m_u_.m_negative_ = value;
            m_d_= -1;
        }

        std::string& text()  {

            if (_d() != 2 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::text not selected by the discriminator" );
            }
            return m_u_.m_text_;
        }

        const std::string& text() const  {

            if (_d() != 2 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::text not selected by the discriminator" );
            }
            return m_u_.m_text_;
        }

        void text(const std::string& value) {
            m_u_.m_text_ = value;
            m_d_= 2;
        }

        void text(std::string&& value) {
            m_u_.m_text_ = std::move(value);
            m_d_= 2;
        }
        bool& other()  {

            if (!(_d() != -1 &&
            _d() != 2 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::other not selected by the discriminator" );
            }
            return m_u_.m_other_;
        }

        const bool& other() const  {

            if (!(_d() != -1 &&
            _d() != 2 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::other not selected by the discriminator" );
            }
            return m_u_.m_other_;
        }

        void other(bool value) {

            if (!(_d() != -1 &&
            _d() != 2 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusShortUnion::Choice::other not selected by the discriminator" );
            }
            m_u_.m_other_ = value;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static int16_t default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        int16_t m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_negative_;
            std::string m_text_;
            bool m_other_;
            Union_();
            Union_(int32_t negative_,const std::string& text_,bool other_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusShortUnion  

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
        struct topic_type_name< ::CorpusShortUnion::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusShortUnion::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusShortUnion::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusShortUnion::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusShortUnion::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusShortUnion::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusShortUnion::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusShortUnion::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusShortUnion::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusShortUnion::Choice > {
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

#endif // _07_union_short_1861173159_hpp

