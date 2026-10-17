

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-evolution-optional.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_evolution_optional_1240273223_hpp
#define _09_evolution_optional_1240273223_hpp

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

namespace CorpusOptionalEvolution {

    class NDDSUSERDllExport Message {
      public:

        Message();

        Message(int32_t requiredValue_,const ::dds::core::optional< int32_t >& optionalValue_,const ::dds::core::optional< std::string >& optionalText_);

        int32_t& requiredValue() noexcept {
            return m_requiredValue_;
        }

        const int32_t& requiredValue() const noexcept {
            return m_requiredValue_;
        }

        void requiredValue(int32_t value) {

            m_requiredValue_ = value;
        }

        ::dds::core::optional< int32_t >& optionalValue() noexcept {
            return m_optionalValue_;
        }

        const ::dds::core::optional< int32_t >& optionalValue() const noexcept {
            return m_optionalValue_;
        }

        void optionalValue(const ::dds::core::optional< int32_t >& value) {

            m_optionalValue_ = value;
        }

        void optionalValue(::dds::core::optional< int32_t >&& value) {
            m_optionalValue_ = std::move(value);
        }
        ::dds::core::optional< std::string >& optionalText() noexcept {
            return m_optionalText_;
        }

        const ::dds::core::optional< std::string >& optionalText() const noexcept {
            return m_optionalText_;
        }

        void optionalText(const ::dds::core::optional< std::string >& value) {

            m_optionalText_ = value;
        }

        void optionalText(::dds::core::optional< std::string >&& value) {
            m_optionalText_ = std::move(value);
        }
        bool operator == (const Message& other_) const;
        bool operator != (const Message& other_) const;

        void swap(Message& other_) noexcept ;

      private:

        int32_t m_requiredValue_;
        ::dds::core::optional< int32_t > m_optionalValue_;
        ::dds::core::optional< std::string > m_optionalText_;

    };

    inline void swap(Message& a, Message& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Message& sample);

} // namespace CorpusOptionalEvolution  

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
        struct topic_type_name< ::CorpusOptionalEvolution::Message > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalEvolution::Message";
            }
        };

        template<>
        struct is_topic_type< ::CorpusOptionalEvolution::Message > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusOptionalEvolution::Message > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusOptionalEvolution::Message& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusOptionalEvolution::Message& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusOptionalEvolution::Message& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusOptionalEvolution::Message& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusOptionalEvolution::Message > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusOptionalEvolution::Message > {
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

#endif // _09_evolution_optional_1240273223_hpp

