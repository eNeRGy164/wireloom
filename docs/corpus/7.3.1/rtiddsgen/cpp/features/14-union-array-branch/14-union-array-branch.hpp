

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-array-branch.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_union_array_branch_804243923_hpp
#define _14_union_array_branch_804243923_hpp

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
        return m_u_.m_values_;
    }

    const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values() const  {

        if (_d() != 0 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::values not selected by the discriminator" );
        }
        return m_u_.m_values_;
    }

    void values(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& value) {
        m_u_.m_values_ = value;
        m_d_= 0;
    }

    void values(::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>&& value) {
        m_u_.m_values_ = std::move(value);
        m_d_= 0;
    }
    int32_t& scalar()  {

        if (_d() != 1 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::scalar not selected by the discriminator" );
        }
        return m_u_.m_scalar_;
    }

    const int32_t& scalar() const  {

        if (_d() != 1 ) {

            throw ::dds::core::PreconditionNotMetError(
                "::ArrayBranch::scalar not selected by the discriminator" );
        }
        return m_u_.m_scalar_;
    }

    void scalar(int32_t value) {
        m_u_.m_scalar_ = value;
        m_d_= 1;
    }

    bool operator == (const ArrayBranch& other_) const;
    bool operator != (const ArrayBranch& other_) const;
    static int32_t default_discriminator();

    void swap(ArrayBranch& other_) noexcept ;

  private:

    int32_t m_d_;
    struct NDDSUSERDllExport Union_ {
        ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L> m_values_ {};
        int32_t m_scalar_;
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
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _14_union_array_branch_804243923_hpp

