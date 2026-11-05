

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-integration.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_integration_1009554025_hpp
#define _13_integration_1009554025_hpp

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

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "../shared/13-status.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

class NDDSUSERDllExport CorpusIntegrationMessage {
  public:
    CorpusIntegrationMessage();

    explicit CorpusIntegrationMessage(const std::string& text);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    CorpusIntegrationMessage (CorpusIntegrationMessage&&) = default;
    CorpusIntegrationMessage& operator=(CorpusIntegrationMessage&&) = default;
    CorpusIntegrationMessage& operator=(const CorpusIntegrationMessage&) = default;
    CorpusIntegrationMessage(const CorpusIntegrationMessage&) = default;
    #else
    CorpusIntegrationMessage(CorpusIntegrationMessage&& other_) OMG_NOEXCEPT;  
    CorpusIntegrationMessage& operator=(CorpusIntegrationMessage&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    std::string& text() OMG_NOEXCEPT {
        return m_text_;
    }

    const std::string& text() const OMG_NOEXCEPT {
        return m_text_;
    }

    void text(const std::string& value) {
        m_text_ = value;
    }

    void text(std::string&& value) {
        m_text_ = std::move(value);
    }

    bool operator == (const CorpusIntegrationMessage& other_) const;
    bool operator != (const CorpusIntegrationMessage& other_) const;

    void swap(CorpusIntegrationMessage& other_) OMG_NOEXCEPT ;

  private:

    std::string m_text_;

};

inline void swap(CorpusIntegrationMessage& a, CorpusIntegrationMessage& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const CorpusIntegrationMessage& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusIntegrationMessage > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationMessage";
            }
        };

        template<>
        struct is_topic_type< CorpusIntegrationMessage > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusIntegrationMessage > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusIntegrationMessage& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusIntegrationMessage& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusIntegrationMessage& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusIntegrationMessage& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationMessage > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusIntegrationMessage > {
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

#endif // _13_integration_1009554025_hpp

