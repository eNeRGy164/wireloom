

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662735984_hpp
#define idl_1662735984_hpp

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
#include "features/02-multiple.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

class NDDSUSERDllExport WireloomWireCompatibilityMultipleTypes {
  public:
    WireloomWireCompatibilityMultipleTypes();

    WireloomWireCompatibilityMultipleTypes(const First& first,const Second& second);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    WireloomWireCompatibilityMultipleTypes (WireloomWireCompatibilityMultipleTypes&&) = default;
    WireloomWireCompatibilityMultipleTypes& operator=(WireloomWireCompatibilityMultipleTypes&&) = default;
    WireloomWireCompatibilityMultipleTypes& operator=(const WireloomWireCompatibilityMultipleTypes&) = default;
    WireloomWireCompatibilityMultipleTypes(const WireloomWireCompatibilityMultipleTypes&) = default;
    #else
    WireloomWireCompatibilityMultipleTypes(WireloomWireCompatibilityMultipleTypes&& other_) OMG_NOEXCEPT;  
    WireloomWireCompatibilityMultipleTypes& operator=(WireloomWireCompatibilityMultipleTypes&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    First& first() OMG_NOEXCEPT {
        return m_first_;
    }

    const First& first() const OMG_NOEXCEPT {
        return m_first_;
    }

    void first(const First& value) {
        m_first_ = value;
    }

    void first(First&& value) {
        m_first_ = std::move(value);
    }
    Second& second() OMG_NOEXCEPT {
        return m_second_;
    }

    const Second& second() const OMG_NOEXCEPT {
        return m_second_;
    }

    void second(const Second& value) {
        m_second_ = value;
    }

    void second(Second&& value) {
        m_second_ = std::move(value);
    }

    bool operator == (const WireloomWireCompatibilityMultipleTypes& other_) const;
    bool operator != (const WireloomWireCompatibilityMultipleTypes& other_) const;

    void swap(WireloomWireCompatibilityMultipleTypes& other_) OMG_NOEXCEPT ;

  private:

    First m_first_;
    Second m_second_;

};

inline void swap(WireloomWireCompatibilityMultipleTypes& a, WireloomWireCompatibilityMultipleTypes& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const WireloomWireCompatibilityMultipleTypes& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< WireloomWireCompatibilityMultipleTypes > {
            NDDSUSERDllExport static std::string value() {
                return "WireloomWireCompatibilityMultipleTypes";
            }
        };

        template<>
        struct is_topic_type< WireloomWireCompatibilityMultipleTypes > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< WireloomWireCompatibilityMultipleTypes > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const WireloomWireCompatibilityMultipleTypes& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(WireloomWireCompatibilityMultipleTypes& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(WireloomWireCompatibilityMultipleTypes& sample);

            NDDSUSERDllExport 
            static void allocate_sample(WireloomWireCompatibilityMultipleTypes& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< WireloomWireCompatibilityMultipleTypes > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< WireloomWireCompatibilityMultipleTypes > {
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

#endif // idl_1662735984_hpp

