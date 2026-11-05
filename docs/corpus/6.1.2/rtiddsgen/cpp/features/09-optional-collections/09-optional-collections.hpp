

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-collections.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_optional_collections_1995411031_hpp
#define _09_optional_collections_1995411031_hpp

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

namespace CorpusOptionalCollections {

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    class NDDSUSERDllExport Sample {
      public:
        Sample();

        Sample(const ::dds::core::optional< ::rti::core::bounded_sequence< int32_t, 4L > >& values,const ::dds::core::optional< ::dds::core::array< int32_t, 2L> >& items);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Sample (Sample&&) = default;
        Sample& operator=(Sample&&) = default;
        Sample& operator=(const Sample&) = default;
        Sample(const Sample&) = default;
        #else
        Sample(Sample&& other_) OMG_NOEXCEPT;  
        Sample& operator=(Sample&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        ::dds::core::optional< ::rti::core::bounded_sequence< int32_t, 4L > >& values() OMG_NOEXCEPT {
            return m_values_;
        }

        const ::dds::core::optional< ::rti::core::bounded_sequence< int32_t, 4L > >& values() const OMG_NOEXCEPT {
            return m_values_;
        }

        void values(const ::dds::core::optional< ::rti::core::bounded_sequence< int32_t, 4L > >& value) {
            m_values_ = value;
        }

        void values(::dds::core::optional< ::rti::core::bounded_sequence< int32_t, 4L > >&& value) {
            m_values_ = std::move(value);
        }
        ::dds::core::optional< ::dds::core::array< int32_t, 2L> >& items() OMG_NOEXCEPT {
            return m_items_;
        }

        const ::dds::core::optional< ::dds::core::array< int32_t, 2L> >& items() const OMG_NOEXCEPT {
            return m_items_;
        }

        void items(const ::dds::core::optional< ::dds::core::array< int32_t, 2L> >& value) {
            m_items_ = value;
        }

        void items(::dds::core::optional< ::dds::core::array< int32_t, 2L> >&& value) {
            m_items_ = std::move(value);
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        ::dds::core::optional< ::rti::core::bounded_sequence< int32_t, 4L > > m_values_;
        ::dds::core::optional< ::dds::core::array< int32_t, 2L> > m_items_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusOptionalCollections  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusOptionalCollections::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalCollections::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusOptionalCollections::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusOptionalCollections::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusOptionalCollections::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusOptionalCollections::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusOptionalCollections::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusOptionalCollections::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusOptionalCollections::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusOptionalCollections::Sample > {
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

#endif // _09_optional_collections_1995411031_hpp

