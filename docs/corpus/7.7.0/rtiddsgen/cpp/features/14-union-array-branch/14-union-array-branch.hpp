

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-array-branch.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_union_array_branch_804243923_hpp
#define _14_union_array_branch_804243923_hpp

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

class NDDSUSERDllExport ArrayBranch {
  public:

    ArrayBranch();

    int32_t& _d()  {
        return m_d_;
    }

    const int32_t& _d() const  {
        return m_d_;
    }

    void _d(int32_t value) {

        m_d_ = value;
    }

    ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values()  {

        if (_d() != 0 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::values not selected by the discriminator" );
        }
        return m_u_.values;
    }

    const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values() const  {

        if (_d() != 0 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::values not selected by the discriminator" );
        }
        return m_u_.values;
    }

    void values(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& value) {
        m_u_.values = value;
        m_d_= 0;
    }

    void values(::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>&& value) {
        m_u_.values = std::move(value);
        m_d_= 0;
    }
    int32_t& scalar()  {

        if (_d() != 1 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::scalar not selected by the discriminator" );
        }
        return m_u_.scalar;
    }

    const int32_t& scalar() const  {

        if (_d() != 1 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::scalar not selected by the discriminator" );
        }
        return m_u_.scalar;
    }

    void scalar(int32_t value) {
        m_u_.scalar = value;
        m_d_= 1;
    }

    void _default() {
        _d(::ArrayBranch::default_discriminator());
    }

    bool operator == (const ArrayBranch& other_) const;
    bool operator != (const ArrayBranch& other_) const;
    static int32_t default_discriminator();

    void swap(ArrayBranch& other_) noexcept ;

  private:

    int32_t m_d_ {};
    struct NDDSUSERDllExport Union_ {
        ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L> values {};
        int32_t scalar {};
        Union_();
        Union_(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values_,int32_t scalar_);
    };
    Union_ m_u_;

};

inline void swap(ArrayBranch& a, ArrayBranch& b)  noexcept 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const ArrayBranch& sample);

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
        struct topic_type_name< ::ArrayBranch > {
            NDDSUSERDllExport static std::string value() {
                return "ArrayBranch";
            }
        };

        template<>
        struct is_topic_type< ::ArrayBranch > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::ArrayBranch > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::ArrayBranch& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::ArrayBranch& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::ArrayBranch& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::ArrayBranch& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::ArrayBranch > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::ArrayBranch > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _14_union_array_branch_804243923_hpp
