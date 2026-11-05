

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-scoped-enum.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_union_scoped_enum_1634330211_hpp
#define _14_union_scoped_enum_1634330211_hpp

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

enum class Choice {
    FIRST, 
    SECOND
};

NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Choice& sample);

static const Choice Selected = (Choice::SECOND);

class NDDSUSERDllExport ScopedEnum {
  public:
    ScopedEnum();

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    ScopedEnum (ScopedEnum&&) = default;
    ScopedEnum& operator=(ScopedEnum&&) = default;
    ScopedEnum& operator=(const ScopedEnum&) = default;
    ScopedEnum(const ScopedEnum&) = default;
    #else
    ScopedEnum(ScopedEnum&& other_) OMG_NOEXCEPT;  
    ScopedEnum& operator=(ScopedEnum&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    Choice& _d()  {
        return m_d_;
    }

    const Choice& _d() const  {
        return m_d_;
    }

    void _d(const Choice& value) {
        m_d_ = value;
    }

    int32_t& selected()  {
        if ( _d() != Selected) {
            throw ::dds::core::PreconditionNotMetError(
                "ScopedEnum::selected not selected by the discriminator" );
        }
        return m_u_.m_selected_;
    }

    const int32_t& selected() const  {
        if ( _d() != Selected) {
            throw ::dds::core::PreconditionNotMetError(
                "ScopedEnum::selected not selected by the discriminator" );
        }
        return m_u_.m_selected_;
    }

    void selected(int32_t value) {
        m_u_.m_selected_ = value;
        m_d_= Selected;
    }

    int32_t& first()  {
        if ( _d() != (Choice::FIRST)) {
            throw ::dds::core::PreconditionNotMetError(
                "ScopedEnum::first not selected by the discriminator" );
        }
        return m_u_.m_first_;
    }

    const int32_t& first() const  {
        if ( _d() != (Choice::FIRST)) {
            throw ::dds::core::PreconditionNotMetError(
                "ScopedEnum::first not selected by the discriminator" );
        }
        return m_u_.m_first_;
    }

    void first(int32_t value) {
        m_u_.m_first_ = value;
        m_d_= (Choice::FIRST);
    }

    bool operator == (const ScopedEnum& other_) const;
    bool operator != (const ScopedEnum& other_) const;

    static Choice default_discriminator();  

    void swap(ScopedEnum& other_) OMG_NOEXCEPT ;

  private:

    Choice m_d_;
    struct NDDSUSERDllExport Union_ {
        int32_t m_selected_;
        int32_t m_first_;
        Union_();
        Union_(int32_t selected,int32_t first);
    };
    Union_ m_u_;

};

inline void swap(ScopedEnum& a, ScopedEnum& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ScopedEnum& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< ScopedEnum > {
            NDDSUSERDllExport static std::string value() {
                return "ScopedEnum";
            }
        };

        template<>
        struct is_topic_type< ScopedEnum > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ScopedEnum > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ScopedEnum& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(ScopedEnum& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(ScopedEnum& sample);

            NDDSUSERDllExport 
            static void allocate_sample(ScopedEnum& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {
        #ifndef NDDS_STANDALONE_TYPE
        template <>
        struct default_enumerator<Choice>
        {
            static const Choice value;
        };
        template<>
        struct dynamic_type< Choice > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };
        #endif

        template <>
        struct extensibility< Choice > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< ScopedEnum > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< ScopedEnum > {
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

#endif // _14_union_scoped_enum_1634330211_hpp

