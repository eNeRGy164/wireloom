

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-aliases.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_string_sequence_aliases_25436842_hpp
#define _07_union_string_sequence_aliases_25436842_hpp

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

namespace RuntimeUnionStringSequences {

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< std::string >;
    template class NDDSUSERDllExport std::vector< std::string >;
    #endif
    typedef ::omg::types::bounded_sequence< std::string, 3L > NarrowTexts;
    struct NarrowTexts_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< std::wstring >;
    template class NDDSUSERDllExport std::vector< std::wstring >;
    #endif
    typedef ::omg::types::bounded_sequence< std::wstring, 3L > WideTexts;
    struct WideTexts_AliasTag_t {};

    typedef ::omg::types::bounded_sequence< std::string, 3L > BoundedNarrowTexts;
    struct BoundedNarrowTexts_AliasTag_t {};

    typedef ::omg::types::bounded_sequence< std::wstring, 3L > BoundedWideTexts;
    struct BoundedWideTexts_AliasTag_t {};

    class NDDSUSERDllExport DirectStringSequences {
      public:

        DirectStringSequences();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::omg::types::bounded_sequence< std::string, 3L >& narrow()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::DirectStringSequences::narrow not selected by the discriminator" );
            }
            return m_u_.narrow;
        }

        const ::omg::types::bounded_sequence< std::string, 3L >& narrow() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::DirectStringSequences::narrow not selected by the discriminator" );
            }
            return m_u_.narrow;
        }

        void narrow(const ::omg::types::bounded_sequence< std::string, 3L >& value) {
            m_u_.narrow = value;
            m_d_= 0;
        }

        void narrow(::omg::types::bounded_sequence< std::string, 3L >&& value) {
            m_u_.narrow = std::move(value);
            m_d_= 0;
        }
        ::omg::types::bounded_sequence< std::wstring, 3L >& wide()  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::DirectStringSequences::wide not selected by the discriminator" );
            }
            return m_u_.wide;
        }

        const ::omg::types::bounded_sequence< std::wstring, 3L >& wide() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::DirectStringSequences::wide not selected by the discriminator" );
            }
            return m_u_.wide;
        }

        void wide(const ::omg::types::bounded_sequence< std::wstring, 3L >& value) {
            m_u_.wide = value;
            m_d_= 1;
        }

        void wide(::omg::types::bounded_sequence< std::wstring, 3L >&& value) {
            m_u_.wide = std::move(value);
            m_d_= 1;
        }
        void _default() {
            _d(::RuntimeUnionStringSequences::DirectStringSequences::default_discriminator());
        }

        bool operator == (const DirectStringSequences& other_) const;
        bool operator != (const DirectStringSequences& other_) const;
        static int32_t default_discriminator();

        void swap(DirectStringSequences& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            ::omg::types::bounded_sequence< std::string, 3L > narrow {};
            ::omg::types::bounded_sequence< std::wstring, 3L > wide {};
            Union_();
            Union_(const ::omg::types::bounded_sequence< std::string, 3L >& narrow_,const ::omg::types::bounded_sequence< std::wstring, 3L >& wide_);
        };
        Union_ m_u_;

    };

    inline void swap(DirectStringSequences& a, DirectStringSequences& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const DirectStringSequences& sample);

    class NDDSUSERDllExport StringSequenceAliases {
      public:

        StringSequenceAliases();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::RuntimeUnionStringSequences::NarrowTexts& narrow()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::narrow not selected by the discriminator" );
            }
            return m_u_.narrow;
        }

        const ::RuntimeUnionStringSequences::NarrowTexts& narrow() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::narrow not selected by the discriminator" );
            }
            return m_u_.narrow;
        }

        void narrow(const ::RuntimeUnionStringSequences::NarrowTexts& value) {
            m_u_.narrow = value;
            m_d_= 0;
        }

        void narrow(::RuntimeUnionStringSequences::NarrowTexts&& value) {
            m_u_.narrow = std::move(value);
            m_d_= 0;
        }
        ::RuntimeUnionStringSequences::WideTexts& wide()  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::wide not selected by the discriminator" );
            }
            return m_u_.wide;
        }

        const ::RuntimeUnionStringSequences::WideTexts& wide() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::wide not selected by the discriminator" );
            }
            return m_u_.wide;
        }

        void wide(const ::RuntimeUnionStringSequences::WideTexts& value) {
            m_u_.wide = value;
            m_d_= 1;
        }

        void wide(::RuntimeUnionStringSequences::WideTexts&& value) {
            m_u_.wide = std::move(value);
            m_d_= 1;
        }
        ::RuntimeUnionStringSequences::BoundedNarrowTexts& boundedNarrow()  {

            if (_d() != 2 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::boundedNarrow not selected by the discriminator" );
            }
            return m_u_.boundedNarrow;
        }

        const ::RuntimeUnionStringSequences::BoundedNarrowTexts& boundedNarrow() const  {

            if (_d() != 2 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::boundedNarrow not selected by the discriminator" );
            }
            return m_u_.boundedNarrow;
        }

        void boundedNarrow(const ::RuntimeUnionStringSequences::BoundedNarrowTexts& value) {
            m_u_.boundedNarrow = value;
            m_d_= 2;
        }

        void boundedNarrow(::RuntimeUnionStringSequences::BoundedNarrowTexts&& value) {
            m_u_.boundedNarrow = std::move(value);
            m_d_= 2;
        }
        ::RuntimeUnionStringSequences::BoundedWideTexts& boundedWide()  {

            if (_d() != 3 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::boundedWide not selected by the discriminator" );
            }
            return m_u_.boundedWide;
        }

        const ::RuntimeUnionStringSequences::BoundedWideTexts& boundedWide() const  {

            if (_d() != 3 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::RuntimeUnionStringSequences::StringSequenceAliases::boundedWide not selected by the discriminator" );
            }
            return m_u_.boundedWide;
        }

        void boundedWide(const ::RuntimeUnionStringSequences::BoundedWideTexts& value) {
            m_u_.boundedWide = value;
            m_d_= 3;
        }

        void boundedWide(::RuntimeUnionStringSequences::BoundedWideTexts&& value) {
            m_u_.boundedWide = std::move(value);
            m_d_= 3;
        }
        void _default() {
            _d(::RuntimeUnionStringSequences::StringSequenceAliases::default_discriminator());
        }

        bool operator == (const StringSequenceAliases& other_) const;
        bool operator != (const StringSequenceAliases& other_) const;
        static int32_t default_discriminator();

        void swap(StringSequenceAliases& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            ::RuntimeUnionStringSequences::NarrowTexts narrow {};
            ::RuntimeUnionStringSequences::WideTexts wide {};
            ::RuntimeUnionStringSequences::BoundedNarrowTexts boundedNarrow {};
            ::RuntimeUnionStringSequences::BoundedWideTexts boundedWide {};
            Union_();
            Union_(const ::RuntimeUnionStringSequences::NarrowTexts& narrow_,const ::RuntimeUnionStringSequences::WideTexts& wide_,const ::RuntimeUnionStringSequences::BoundedNarrowTexts& boundedNarrow_,const ::RuntimeUnionStringSequences::BoundedWideTexts& boundedWide_);
        };
        Union_ m_u_;

    };

    inline void swap(StringSequenceAliases& a, StringSequenceAliases& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const StringSequenceAliases& sample);

} // namespace RuntimeUnionStringSequences  

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
        struct topic_type_name< ::RuntimeUnionStringSequences::DirectStringSequences > {
            NDDSUSERDllExport static std::string value() {
                return "RuntimeUnionStringSequences::DirectStringSequences";
            }
        };

        template<>
        struct is_topic_type< ::RuntimeUnionStringSequences::DirectStringSequences > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::RuntimeUnionStringSequences::DirectStringSequences > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::RuntimeUnionStringSequences::DirectStringSequences& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::RuntimeUnionStringSequences::DirectStringSequences& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::RuntimeUnionStringSequences::DirectStringSequences& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::RuntimeUnionStringSequences::DirectStringSequences& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::RuntimeUnionStringSequences::StringSequenceAliases > {
            NDDSUSERDllExport static std::string value() {
                return "RuntimeUnionStringSequences::StringSequenceAliases";
            }
        };

        template<>
        struct is_topic_type< ::RuntimeUnionStringSequences::StringSequenceAliases > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::RuntimeUnionStringSequences::StringSequenceAliases > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::RuntimeUnionStringSequences::StringSequenceAliases& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::RuntimeUnionStringSequences::StringSequenceAliases& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::RuntimeUnionStringSequences::StringSequenceAliases& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::RuntimeUnionStringSequences::StringSequenceAliases& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::NarrowTexts_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::WideTexts_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::BoundedNarrowTexts_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::BoundedWideTexts_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::DirectStringSequences > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::RuntimeUnionStringSequences::DirectStringSequences > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::StringSequenceAliases > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::RuntimeUnionStringSequences::StringSequenceAliases > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _07_union_string_sequence_aliases_25436842_hpp
