

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-scoped.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_scoped_724189690_hpp
#define _07_union_scoped_724189690_hpp

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

namespace CorpusScopedUnion {

    class NDDSUSERDllExport Payload {
      public:

        Payload();

        Payload(int32_t code_,const std::string& label_);

        int32_t& code() noexcept {
            return m_code_;
        }

        const int32_t& code() const noexcept {
            return m_code_;
        }

        void code(int32_t value) {

            m_code_ = value;
        }

        std::string& label() noexcept {
            return m_label_;
        }

        const std::string& label() const noexcept {
            return m_label_;
        }

        void label(const std::string& value) {

            m_label_ = value;
        }

        void label(std::string&& value) {
            m_label_ = std::move(value);
        }
        bool operator == (const Payload& other_) const;
        bool operator != (const Payload& other_) const;

        void swap(Payload& other_) noexcept ;

      private:

        int32_t m_code_;
        std::string m_label_;

    };

    inline void swap(Payload& a, Payload& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Payload& sample);

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    class NDDSUSERDllExport Choice {
      public:

        Choice();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::CorpusScopedUnion::Payload& qualifiedPayload()  {

            if (_d() != 10 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusScopedUnion::Choice::qualifiedPayload not selected by the discriminator" );
            }
            return m_u_.m_qualifiedPayload_;
        }

        const ::CorpusScopedUnion::Payload& qualifiedPayload() const  {

            if (_d() != 10 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusScopedUnion::Choice::qualifiedPayload not selected by the discriminator" );
            }
            return m_u_.m_qualifiedPayload_;
        }

        void qualifiedPayload(const ::CorpusScopedUnion::Payload& value) {
            m_u_.m_qualifiedPayload_ = value;
            m_d_= 10;
        }

        void qualifiedPayload(::CorpusScopedUnion::Payload&& value) {
            m_u_.m_qualifiedPayload_ = std::move(value);
            m_d_= 10;
        }
        ::rti::core::bounded_sequence< int32_t, 4L >& values()  {

            if (_d() != 11 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusScopedUnion::Choice::values not selected by the discriminator" );
            }
            return m_u_.m_values_;
        }

        const ::rti::core::bounded_sequence< int32_t, 4L >& values() const  {

            if (_d() != 11 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusScopedUnion::Choice::values not selected by the discriminator" );
            }
            return m_u_.m_values_;
        }

        void values(const ::rti::core::bounded_sequence< int32_t, 4L >& value) {
            m_u_.m_values_ = value;
            m_d_= 11;
        }

        void values(::rti::core::bounded_sequence< int32_t, 4L >&& value) {
            m_u_.m_values_ = std::move(value);
            m_d_= 11;
        }
        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static int32_t default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            ::CorpusScopedUnion::Payload m_qualifiedPayload_;
            ::rti::core::bounded_sequence< int32_t, 4L > m_values_;
            Union_();
            Union_(const ::CorpusScopedUnion::Payload& qualifiedPayload_,const ::rti::core::bounded_sequence< int32_t, 4L >& values_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusScopedUnion  

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
        struct topic_type_name< ::CorpusScopedUnion::Payload > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusScopedUnion::Payload";
            }
        };

        template<>
        struct is_topic_type< ::CorpusScopedUnion::Payload > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusScopedUnion::Payload > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusScopedUnion::Payload& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusScopedUnion::Payload& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusScopedUnion::Payload& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusScopedUnion::Payload& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusScopedUnion::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusScopedUnion::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusScopedUnion::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusScopedUnion::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusScopedUnion::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusScopedUnion::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusScopedUnion::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusScopedUnion::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusScopedUnion::Payload > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusScopedUnion::Payload > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusScopedUnion::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusScopedUnion::Choice > {
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

#endif // _07_union_scoped_724189690_hpp

