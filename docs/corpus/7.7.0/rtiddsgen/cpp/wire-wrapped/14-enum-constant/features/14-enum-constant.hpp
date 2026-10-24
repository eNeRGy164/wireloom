

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_enum_constant_1907946297_hpp
#define _14_enum_constant_1907946297_hpp

#include <iosfwd>

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef RTIUSERDllExport
#define RTIUSERDllExport __attribute__((visibility("default")))
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

#include "omg/types/sequence.hpp"
#include "omg/types/optional.hpp"

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

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef RTIUSERDllExport
#define RTIUSERDllExport
#endif

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

enum class Color {
    RED, 
    GREEN
};

NDDSUSERDllExport std::ostream& operator << (std::ostream& o,const Color& sample);

RTI_CONSTEXPR_OR_CONST_STRING ::Color Selected = (Color::GREEN);

struct NDDSUSERDllExport EnumConstant {
    ::Color color {};

    EnumConstant();

    explicit EnumConstant(const ::Color& color_);

};

NDDSUSERDllExport bool operator == (const EnumConstant& a, const EnumConstant& b);
NDDSUSERDllExport bool operator != (const EnumConstant& a, const EnumConstant& b);
NDDSUSERDllExport void swap(EnumConstant& a, EnumConstant& b)  noexcept;

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const EnumConstant& sample);

class NDDSUSERDllExport EnumUnion {
  public:

    EnumUnion();

    ::Color& _d()  {
        return m_d_;
    }

    const ::Color& _d() const  {
        return m_d_;
    }

    void _d(::Color value) {

        m_d_ = value;
    }

    int32_t& selected()  {

        if (_d() != Selected ) {

            throw ::dds::core::PreconditionNotMetError(
                "::EnumUnion::selected not selected by the discriminator" );
        }
        return m_u_.selected;
    }

    const int32_t& selected() const  {

        if (_d() != Selected ) {

            throw ::dds::core::PreconditionNotMetError(
                "::EnumUnion::selected not selected by the discriminator" );
        }
        return m_u_.selected;
    }

    void selected(int32_t value) {
        m_u_.selected = value;
        m_d_= Selected;
    }

    int32_t& red()  {

        if (_d() != (Color::RED) ) {

            throw ::dds::core::PreconditionNotMetError(
                "::EnumUnion::red not selected by the discriminator" );
        }
        return m_u_.red;
    }

    const int32_t& red() const  {

        if (_d() != (Color::RED) ) {

            throw ::dds::core::PreconditionNotMetError(
                "::EnumUnion::red not selected by the discriminator" );
        }
        return m_u_.red;
    }

    void red(int32_t value) {
        m_u_.red = value;
        m_d_= (Color::RED);
    }

    bool operator == (const EnumUnion& other_) const;
    bool operator != (const EnumUnion& other_) const;
    static ::Color default_discriminator();

    void swap(EnumUnion& other_) noexcept ;

  private:

    ::Color m_d_ {};
    struct NDDSUSERDllExport Union_ {
        int32_t selected {};
        int32_t red {};
        Union_();
        Union_(int32_t selected_,int32_t red_);
    };
    Union_ m_u_;

};

inline void swap(EnumUnion& a, EnumUnion& b)  noexcept 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const EnumUnion& sample);

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
        template <>
        struct default_enumerator< ::Color>
        {
            static const ::Color value;
        };
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
        struct topic_type_name< ::EnumConstant > {
            NDDSUSERDllExport static std::string value() {
                return "EnumConstant";
            }
        };

        template<>
        struct is_topic_type< ::EnumConstant > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::EnumConstant > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::EnumConstant& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::EnumConstant& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::EnumConstant& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::EnumConstant& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::EnumUnion > {
            NDDSUSERDllExport static std::string value() {
                return "EnumUnion";
            }
        };

        template<>
        struct is_topic_type< ::EnumUnion > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::EnumUnion > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::EnumUnion& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::EnumUnion& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::EnumUnion& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::EnumUnion& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template <>
        struct default_enumerator< ::Color>
        {
            static const ::Color value;
        };
        template<>
        struct dynamic_type< ::Color > {
            typedef ::dds::core::xtypes::EnumType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::EnumType& get();
        };

        template <>
        struct extensibility< ::Color > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::EnumConstant > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::EnumConstant > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::EnumUnion > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::EnumUnion > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _14_enum_constant_1907946297_hpp
