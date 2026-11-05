

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_enum_constant_1907946297_hpp
#define _14_enum_constant_1907946297_hpp

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

enum class Color {
    RED, 
    GREEN
};

NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Color& sample);

static const Color Selected = (Color::GREEN);

class NDDSUSERDllExport EnumConstant {
  public:
    EnumConstant();

    explicit EnumConstant(const Color& color);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    EnumConstant (EnumConstant&&) = default;
    EnumConstant& operator=(EnumConstant&&) = default;
    EnumConstant& operator=(const EnumConstant&) = default;
    EnumConstant(const EnumConstant&) = default;
    #else
    EnumConstant(EnumConstant&& other_) OMG_NOEXCEPT;  
    EnumConstant& operator=(EnumConstant&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    Color& color() OMG_NOEXCEPT {
        return m_color_;
    }

    const Color& color() const OMG_NOEXCEPT {
        return m_color_;
    }

    void color(const Color& value) {
        m_color_ = value;
    }

    void color(Color&& value) {
        m_color_ = std::move(value);
    }

    bool operator == (const EnumConstant& other_) const;
    bool operator != (const EnumConstant& other_) const;

    void swap(EnumConstant& other_) OMG_NOEXCEPT ;

  private:

    Color m_color_;

};

inline void swap(EnumConstant& a, EnumConstant& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const EnumConstant& sample);

class NDDSUSERDllExport EnumUnion {
  public:
    EnumUnion();

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    EnumUnion (EnumUnion&&) = default;
    EnumUnion& operator=(EnumUnion&&) = default;
    EnumUnion& operator=(const EnumUnion&) = default;
    EnumUnion(const EnumUnion&) = default;
    #else
    EnumUnion(EnumUnion&& other_) OMG_NOEXCEPT;  
    EnumUnion& operator=(EnumUnion&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    Color& _d()  {
        return m_d_;
    }

    const Color& _d() const  {
        return m_d_;
    }

    void _d(const Color& value) {
        m_d_ = value;
    }

    int32_t& selected()  {
        if ( _d() != Selected) {
            throw ::dds::core::PreconditionNotMetError(
                "EnumUnion::selected not selected by the discriminator" );
        }
        return m_u_.m_selected_;
    }

    const int32_t& selected() const  {
        if ( _d() != Selected) {
            throw ::dds::core::PreconditionNotMetError(
                "EnumUnion::selected not selected by the discriminator" );
        }
        return m_u_.m_selected_;
    }

    void selected(int32_t value) {
        m_u_.m_selected_ = value;
        m_d_= Selected;
    }

    int32_t& red()  {
        if ( _d() != (Color::RED)) {
            throw ::dds::core::PreconditionNotMetError(
                "EnumUnion::red not selected by the discriminator" );
        }
        return m_u_.m_red_;
    }

    const int32_t& red() const  {
        if ( _d() != (Color::RED)) {
            throw ::dds::core::PreconditionNotMetError(
                "EnumUnion::red not selected by the discriminator" );
        }
        return m_u_.m_red_;
    }

    void red(int32_t value) {
        m_u_.m_red_ = value;
        m_d_= (Color::RED);
    }

    bool operator == (const EnumUnion& other_) const;
    bool operator != (const EnumUnion& other_) const;

    static Color default_discriminator();  

    void swap(EnumUnion& other_) OMG_NOEXCEPT ;

  private:

    Color m_d_;
    struct NDDSUSERDllExport Union_ {
        int32_t m_selected_;
        int32_t m_red_;
        Union_();
        Union_(int32_t selected,int32_t red);
    };
    Union_ m_u_;

};

inline void swap(EnumUnion& a, EnumUnion& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const EnumUnion& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< EnumConstant > {
            NDDSUSERDllExport static std::string value() {
                return "EnumConstant";
            }
        };

        template<>
        struct is_topic_type< EnumConstant > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< EnumConstant > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const EnumConstant& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(EnumConstant& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(EnumConstant& sample);

            NDDSUSERDllExport 
            static void allocate_sample(EnumConstant& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< EnumUnion > {
            NDDSUSERDllExport static std::string value() {
                return "EnumUnion";
            }
        };

        template<>
        struct is_topic_type< EnumUnion > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< EnumUnion > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const EnumUnion& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(EnumUnion& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(EnumUnion& sample);

            NDDSUSERDllExport 
            static void allocate_sample(EnumUnion& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {
        #ifndef NDDS_STANDALONE_TYPE
        template <>
        struct default_enumerator<Color>
        {
            static const Color value;
        };
        template<>
        struct dynamic_type< Color > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };
        #endif

        template <>
        struct extensibility< Color > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< EnumConstant > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< EnumConstant > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< EnumUnion > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< EnumUnion > {
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

#endif // _14_enum_constant_1907946297_hpp

