

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662735979_hpp
#define idl_1662735979_hpp

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
#include "features/14-enum-constant.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

class NDDSUSERDllExport EnumConstantSample {
  public:
    EnumConstantSample();

    EnumConstantSample(const EnumConstant& value,const EnumUnion& choice);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    EnumConstantSample (EnumConstantSample&&) = default;
    EnumConstantSample& operator=(EnumConstantSample&&) = default;
    EnumConstantSample& operator=(const EnumConstantSample&) = default;
    EnumConstantSample(const EnumConstantSample&) = default;
    #else
    EnumConstantSample(EnumConstantSample&& other_) OMG_NOEXCEPT;  
    EnumConstantSample& operator=(EnumConstantSample&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    EnumConstant& value() OMG_NOEXCEPT {
        return m_value_;
    }

    const EnumConstant& value() const OMG_NOEXCEPT {
        return m_value_;
    }

    void value(const EnumConstant& value) {
        m_value_ = value;
    }

    void value(EnumConstant&& value) {
        m_value_ = std::move(value);
    }
    EnumUnion& choice() OMG_NOEXCEPT {
        return m_choice_;
    }

    const EnumUnion& choice() const OMG_NOEXCEPT {
        return m_choice_;
    }

    void choice(const EnumUnion& value) {
        m_choice_ = value;
    }

    void choice(EnumUnion&& value) {
        m_choice_ = std::move(value);
    }

    bool operator == (const EnumConstantSample& other_) const;
    bool operator != (const EnumConstantSample& other_) const;

    void swap(EnumConstantSample& other_) OMG_NOEXCEPT ;

  private:

    EnumConstant m_value_;
    EnumUnion m_choice_;

};

inline void swap(EnumConstantSample& a, EnumConstantSample& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const EnumConstantSample& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< EnumConstantSample > {
            NDDSUSERDllExport static std::string value() {
                return "EnumConstantSample";
            }
        };

        template<>
        struct is_topic_type< EnumConstantSample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< EnumConstantSample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const EnumConstantSample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(EnumConstantSample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(EnumConstantSample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(EnumConstantSample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< EnumConstantSample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< EnumConstantSample > {
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

#endif // idl_1662735979_hpp

