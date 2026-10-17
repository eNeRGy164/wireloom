

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-uint16.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_uint16_1647623329_hpp
#define _07_union_uint16_1647623329_hpp

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

namespace CorpusUnionUint16 {

    class NDDSUSERDllExport Choice {
      public:

        Choice();

        uint16_t& _d()  {
            return m_d_;
        }

        const uint16_t& _d() const  {
            return m_d_;
        }

        void _d(uint16_t value) {

            m_d_ = value;
        }

        int32_t& minimum()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::minimum not selected by the discriminator" );
            }
            return m_u_.m_minimum_;
        }

        const int32_t& minimum() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::minimum not selected by the discriminator" );
            }
            return m_u_.m_minimum_;
        }

        void minimum(int32_t value) {
            m_u_.m_minimum_ = value;
            m_d_= 0;
        }

        int32_t& maximum()  {

            if (_d() != 65535 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::maximum not selected by the discriminator" );
            }
            return m_u_.m_maximum_;
        }

        const int32_t& maximum() const  {

            if (_d() != 65535 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::maximum not selected by the discriminator" );
            }
            return m_u_.m_maximum_;
        }

        void maximum(int32_t value) {
            m_u_.m_maximum_ = value;
            m_d_= 65535;
        }

        int32_t& fallback()  {

            if (!(_d() != 0 &&
            _d() != 65535 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::fallback not selected by the discriminator" );
            }
            return m_u_.m_fallback_;
        }

        const int32_t& fallback() const  {

            if (!(_d() != 0 &&
            _d() != 65535 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::fallback not selected by the discriminator" );
            }
            return m_u_.m_fallback_;
        }

        void fallback(int32_t value) {

            if (!(_d() != 0 &&
            _d() != 65535 )) {
                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusUnionUint16::Choice::fallback not selected by the discriminator" );
            }
            m_u_.m_fallback_ = value;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static uint16_t default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        uint16_t m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_minimum_;
            int32_t m_maximum_;
            int32_t m_fallback_;
            Union_();
            Union_(int32_t minimum_,int32_t maximum_,int32_t fallback_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusUnionUint16  

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
        struct topic_type_name< ::CorpusUnionUint16::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusUnionUint16::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusUnionUint16::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusUnionUint16::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusUnionUint16::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusUnionUint16::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusUnionUint16::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusUnionUint16::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusUnionUint16::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusUnionUint16::Choice > {
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

#endif // _07_union_uint16_1647623329_hpp

