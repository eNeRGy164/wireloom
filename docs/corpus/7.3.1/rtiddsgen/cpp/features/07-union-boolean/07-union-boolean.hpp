

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-boolean.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_boolean_690435126_hpp
#define _07_union_boolean_690435126_hpp

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

namespace CorpusBooleanUnion {

    class NDDSUSERDllExport Choice {
      public:

        Choice();

        bool& _d()  {
            return m_d_;
        }

        const bool& _d() const  {
            return m_d_;
        }

        void _d(bool value) {

            m_d_ = value;
        }

        int32_t& enabled()  {

            if (_d() != DDS_BOOLEAN_TRUE ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusBooleanUnion::Choice::enabled not selected by the discriminator" );
            }
            return m_u_.m_enabled_;
        }

        const int32_t& enabled() const  {

            if (_d() != DDS_BOOLEAN_TRUE ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusBooleanUnion::Choice::enabled not selected by the discriminator" );
            }
            return m_u_.m_enabled_;
        }

        void enabled(int32_t value) {
            m_u_.m_enabled_ = value;
            m_d_= DDS_BOOLEAN_TRUE;
        }

        std::string& disabled()  {

            if (_d() != DDS_BOOLEAN_FALSE ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusBooleanUnion::Choice::disabled not selected by the discriminator" );
            }
            return m_u_.m_disabled_;
        }

        const std::string& disabled() const  {

            if (_d() != DDS_BOOLEAN_FALSE ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusBooleanUnion::Choice::disabled not selected by the discriminator" );
            }
            return m_u_.m_disabled_;
        }

        void disabled(const std::string& value) {
            m_u_.m_disabled_ = value;
            m_d_= DDS_BOOLEAN_FALSE;
        }

        void disabled(std::string&& value) {
            m_u_.m_disabled_ = std::move(value);
            m_d_= DDS_BOOLEAN_FALSE;
        }
        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static bool default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        bool m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_enabled_;
            std::string m_disabled_;
            Union_();
            Union_(int32_t enabled_,const std::string& disabled_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusBooleanUnion  

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
        struct topic_type_name< ::CorpusBooleanUnion::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusBooleanUnion::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusBooleanUnion::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusBooleanUnion::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusBooleanUnion::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusBooleanUnion::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusBooleanUnion::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusBooleanUnion::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusBooleanUnion::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusBooleanUnion::Choice > {
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

#endif // _07_union_boolean_690435126_hpp

