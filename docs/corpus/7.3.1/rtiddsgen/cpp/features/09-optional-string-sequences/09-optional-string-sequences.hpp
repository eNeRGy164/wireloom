

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-string-sequences.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_optional_string_sequences_2019990320_hpp
#define _09_optional_string_sequences_2019990320_hpp

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

namespace CorpusOptionalStringSequences {

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< std::string >;
    template class NDDSUSERDllExport std::vector< std::string >;
    template class NDDSUSERDllExport std::allocator< std::wstring >;
    template class NDDSUSERDllExport std::vector< std::wstring >;
    #endif
    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(const ::dds::core::optional< ::rti::core::bounded_sequence< std::string, 4L > >& narrowValues_,const ::dds::core::optional< ::rti::core::bounded_sequence< std::wstring, 4L > >& wideValues_);

        ::dds::core::optional< ::rti::core::bounded_sequence< std::string, 4L > >& narrowValues() noexcept {
            return m_narrowValues_;
        }

        const ::dds::core::optional< ::rti::core::bounded_sequence< std::string, 4L > >& narrowValues() const noexcept {
            return m_narrowValues_;
        }

        void narrowValues(const ::dds::core::optional< ::rti::core::bounded_sequence< std::string, 4L > >& value) {

            m_narrowValues_ = value;
        }

        void narrowValues(::dds::core::optional< ::rti::core::bounded_sequence< std::string, 4L > >&& value) {
            m_narrowValues_ = std::move(value);
        }
        ::dds::core::optional< ::rti::core::bounded_sequence< std::wstring, 4L > >& wideValues() noexcept {
            return m_wideValues_;
        }

        const ::dds::core::optional< ::rti::core::bounded_sequence< std::wstring, 4L > >& wideValues() const noexcept {
            return m_wideValues_;
        }

        void wideValues(const ::dds::core::optional< ::rti::core::bounded_sequence< std::wstring, 4L > >& value) {

            m_wideValues_ = value;
        }

        void wideValues(::dds::core::optional< ::rti::core::bounded_sequence< std::wstring, 4L > >&& value) {
            m_wideValues_ = std::move(value);
        }
        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        ::dds::core::optional< ::rti::core::bounded_sequence< std::string, 4L > > m_narrowValues_;
        ::dds::core::optional< ::rti::core::bounded_sequence< std::wstring, 4L > > m_wideValues_;

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusOptionalStringSequences  

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
        struct topic_type_name< ::CorpusOptionalStringSequences::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalStringSequences::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusOptionalStringSequences::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusOptionalStringSequences::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusOptionalStringSequences::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusOptionalStringSequences::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusOptionalStringSequences::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusOptionalStringSequences::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusOptionalStringSequences::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusOptionalStringSequences::Sample > {
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

#endif // _09_optional_string_sequences_2019990320_hpp

