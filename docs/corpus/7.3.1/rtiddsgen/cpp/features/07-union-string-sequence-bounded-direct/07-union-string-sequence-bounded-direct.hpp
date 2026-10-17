

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-bounded-direct.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_string_sequence_bounded_direct_586090969_hpp
#define _07_union_string_sequence_bounded_direct_586090969_hpp

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

namespace RuntimeUnionBoundedStringSequences {

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< std::string >;
    template class NDDSUSERDllExport std::vector< std::string >;
    template class NDDSUSERDllExport std::allocator< std::wstring >;
    template class NDDSUSERDllExport std::vector< std::wstring >;
    #endif
    class NDDSUSERDllExport DirectBoundedStringSequences {
      public:

        DirectBoundedStringSequences();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::rti::core::bounded_sequence< std::string, 3L >& narrow()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences::narrow not selected by the discriminator" );
            }
            return m_u_.m_narrow_;
        }

        const ::rti::core::bounded_sequence< std::string, 3L >& narrow() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences::narrow not selected by the discriminator" );
            }
            return m_u_.m_narrow_;
        }

        void narrow(const ::rti::core::bounded_sequence< std::string, 3L >& value) {
            m_u_.m_narrow_ = value;
            m_d_= 0;
        }

        void narrow(::rti::core::bounded_sequence< std::string, 3L >&& value) {
            m_u_.m_narrow_ = std::move(value);
            m_d_= 0;
        }
        ::rti::core::bounded_sequence< std::wstring, 3L >& wide()  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences::wide not selected by the discriminator" );
            }
            return m_u_.m_wide_;
        }

        const ::rti::core::bounded_sequence< std::wstring, 3L >& wide() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences::wide not selected by the discriminator" );
            }
            return m_u_.m_wide_;
        }

        void wide(const ::rti::core::bounded_sequence< std::wstring, 3L >& value) {
            m_u_.m_wide_ = value;
            m_d_= 1;
        }

        void wide(::rti::core::bounded_sequence< std::wstring, 3L >&& value) {
            m_u_.m_wide_ = std::move(value);
            m_d_= 1;
        }
        bool operator == (const DirectBoundedStringSequences& other_) const;
        bool operator != (const DirectBoundedStringSequences& other_) const;
        static int32_t default_discriminator();

        void swap(DirectBoundedStringSequences& other_) noexcept ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            ::rti::core::bounded_sequence< std::string, 3L > m_narrow_;
            ::rti::core::bounded_sequence< std::wstring, 3L > m_wide_;
            Union_();
            Union_(const ::rti::core::bounded_sequence< std::string, 3L >& narrow_,const ::rti::core::bounded_sequence< std::wstring, 3L >& wide_);
        };
        Union_ m_u_;

    };

    inline void swap(DirectBoundedStringSequences& a, DirectBoundedStringSequences& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const DirectBoundedStringSequences& sample);

} // namespace RuntimeUnionBoundedStringSequences  

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
        struct topic_type_name< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences > {
            NDDSUSERDllExport static std::string value() {
                return "RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences";
            }
        };

        template<>
        struct is_topic_type< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences > {
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

#endif // _07_union_string_sequence_bounded_direct_586090969_hpp

