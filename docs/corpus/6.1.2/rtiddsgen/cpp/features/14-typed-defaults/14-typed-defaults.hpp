

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-typed-defaults.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_typed_defaults_188549010_hpp
#define _14_typed_defaults_188549010_hpp

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

typedef int32_t Seven;
struct Seven_AliasTag_t {};

typedef std::string TextAlias;
struct TextAlias_AliasTag_t {};

typedef double Ratio;
struct Ratio_AliasTag_t {};

class NDDSUSERDllExport TypedDefaults {
  public:
    TypedDefaults();

    TypedDefaults(int32_t inherited,int32_t overridden,const std::string& text,double ratio,float single);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    TypedDefaults (TypedDefaults&&) = default;
    TypedDefaults& operator=(TypedDefaults&&) = default;
    TypedDefaults& operator=(const TypedDefaults&) = default;
    TypedDefaults(const TypedDefaults&) = default;
    #else
    TypedDefaults(TypedDefaults&& other_) OMG_NOEXCEPT;  
    TypedDefaults& operator=(TypedDefaults&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    int32_t& inherited() OMG_NOEXCEPT {
        return m_inherited_;
    }

    const int32_t& inherited() const OMG_NOEXCEPT {
        return m_inherited_;
    }

    void inherited(int32_t value) {
        m_inherited_ = value;
    }

    int32_t& overridden() OMG_NOEXCEPT {
        return m_overridden_;
    }

    const int32_t& overridden() const OMG_NOEXCEPT {
        return m_overridden_;
    }

    void overridden(int32_t value) {
        m_overridden_ = value;
    }

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
    double& ratio() OMG_NOEXCEPT {
        return m_ratio_;
    }

    const double& ratio() const OMG_NOEXCEPT {
        return m_ratio_;
    }

    void ratio(double value) {
        m_ratio_ = value;
    }

    float& single() OMG_NOEXCEPT {
        return m_single_;
    }

    const float& single() const OMG_NOEXCEPT {
        return m_single_;
    }

    void single(float value) {
        m_single_ = value;
    }

    bool operator == (const TypedDefaults& other_) const;
    bool operator != (const TypedDefaults& other_) const;

    void swap(TypedDefaults& other_) OMG_NOEXCEPT ;

  private:

    int32_t m_inherited_;
    int32_t m_overridden_;
    std::string m_text_;
    double m_ratio_;
    float m_single_;

};

inline void swap(TypedDefaults& a, TypedDefaults& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const TypedDefaults& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< TypedDefaults > {
            NDDSUSERDllExport static std::string value() {
                return "TypedDefaults";
            }
        };

        template<>
        struct is_topic_type< TypedDefaults > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< TypedDefaults > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const TypedDefaults& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(TypedDefaults& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(TypedDefaults& sample);

            NDDSUSERDllExport 
            static void allocate_sample(TypedDefaults& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {
        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< Seven_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< TextAlias_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< Ratio_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< TypedDefaults > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< TypedDefaults > {
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

#endif // _14_typed_defaults_188549010_hpp

