

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662736047_hpp
#define idl_1662736047_hpp

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
#include "features/14-autoid-scope.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

class NDDSUSERDllExport AutoIdScopeSample {
  public:
    AutoIdScopeSample();

    AutoIdScopeSample(const AutoIds::Hashed& hashed,const AutoIds::Reopened& reopened,const Sequential& sequential,const UsesInclude& usesInclude);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    AutoIdScopeSample (AutoIdScopeSample&&) = default;
    AutoIdScopeSample& operator=(AutoIdScopeSample&&) = default;
    AutoIdScopeSample& operator=(const AutoIdScopeSample&) = default;
    AutoIdScopeSample(const AutoIdScopeSample&) = default;
    #else
    AutoIdScopeSample(AutoIdScopeSample&& other_) OMG_NOEXCEPT;  
    AutoIdScopeSample& operator=(AutoIdScopeSample&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    AutoIds::Hashed& hashed() OMG_NOEXCEPT {
        return m_hashed_;
    }

    const AutoIds::Hashed& hashed() const OMG_NOEXCEPT {
        return m_hashed_;
    }

    void hashed(const AutoIds::Hashed& value) {
        m_hashed_ = value;
    }

    void hashed(AutoIds::Hashed&& value) {
        m_hashed_ = std::move(value);
    }
    AutoIds::Reopened& reopened() OMG_NOEXCEPT {
        return m_reopened_;
    }

    const AutoIds::Reopened& reopened() const OMG_NOEXCEPT {
        return m_reopened_;
    }

    void reopened(const AutoIds::Reopened& value) {
        m_reopened_ = value;
    }

    void reopened(AutoIds::Reopened&& value) {
        m_reopened_ = std::move(value);
    }
    Sequential& sequential() OMG_NOEXCEPT {
        return m_sequential_;
    }

    const Sequential& sequential() const OMG_NOEXCEPT {
        return m_sequential_;
    }

    void sequential(const Sequential& value) {
        m_sequential_ = value;
    }

    void sequential(Sequential&& value) {
        m_sequential_ = std::move(value);
    }
    UsesInclude& usesInclude() OMG_NOEXCEPT {
        return m_usesInclude_;
    }

    const UsesInclude& usesInclude() const OMG_NOEXCEPT {
        return m_usesInclude_;
    }

    void usesInclude(const UsesInclude& value) {
        m_usesInclude_ = value;
    }

    void usesInclude(UsesInclude&& value) {
        m_usesInclude_ = std::move(value);
    }

    bool operator == (const AutoIdScopeSample& other_) const;
    bool operator != (const AutoIdScopeSample& other_) const;

    void swap(AutoIdScopeSample& other_) OMG_NOEXCEPT ;

  private:

    AutoIds::Hashed m_hashed_;
    AutoIds::Reopened m_reopened_;
    Sequential m_sequential_;
    UsesInclude m_usesInclude_;

};

inline void swap(AutoIdScopeSample& a, AutoIdScopeSample& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const AutoIdScopeSample& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< AutoIdScopeSample > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIdScopeSample";
            }
        };

        template<>
        struct is_topic_type< AutoIdScopeSample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< AutoIdScopeSample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const AutoIdScopeSample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(AutoIdScopeSample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(AutoIdScopeSample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(AutoIdScopeSample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< AutoIdScopeSample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< AutoIdScopeSample > {
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

#endif // idl_1662736047_hpp

