

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-compositions.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_compositions_1376405715_hpp
#define _13_compositions_1376405715_hpp

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

namespace CorpusIntegrationCollections {

    class NDDSUSERDllExport Item {
      public:
        Item();

        Item(int32_t id,const std::string& label);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Item (Item&&) = default;
        Item& operator=(Item&&) = default;
        Item& operator=(const Item&) = default;
        Item(const Item&) = default;
        #else
        Item(Item&& other_) OMG_NOEXCEPT;  
        Item& operator=(Item&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& id() OMG_NOEXCEPT {
            return m_id_;
        }

        const int32_t& id() const OMG_NOEXCEPT {
            return m_id_;
        }

        void id(int32_t value) {
            m_id_ = value;
        }

        std::string& label() OMG_NOEXCEPT {
            return m_label_;
        }

        const std::string& label() const OMG_NOEXCEPT {
            return m_label_;
        }

        void label(const std::string& value) {
            m_label_ = value;
        }

        void label(std::string&& value) {
            m_label_ = std::move(value);
        }

        bool operator == (const Item& other_) const;
        bool operator != (const Item& other_) const;

        void swap(Item& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_id_;
        std::string m_label_;

    };

    inline void swap(Item& a, Item& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Item& sample);

    typedef ::dds::core::array< int32_t, 2L> Row;
    struct Row_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< CorpusIntegrationCollections::Item >;
    template class NDDSUSERDllExport std::vector< CorpusIntegrationCollections::Item >;
    template class NDDSUSERDllExport std::allocator< CorpusIntegrationCollections::Row >;
    template class NDDSUSERDllExport std::vector< CorpusIntegrationCollections::Row >;
    #endif
    class NDDSUSERDllExport Sample {
      public:
        Sample();

        Sample(const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 100L >& unboundedItems,const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 2L >& boundedItems,const ::dds::core::array< CorpusIntegrationCollections::Item, 2L>& aggregateArray,const ::dds::core::array< CorpusIntegrationCollections::Row, 2L>& rows,const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Row, 2L >& sequenceOfArrays);

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

        ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 100L >& unboundedItems() OMG_NOEXCEPT {
            return m_unboundedItems_;
        }

        const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 100L >& unboundedItems() const OMG_NOEXCEPT {
            return m_unboundedItems_;
        }

        void unboundedItems(const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 100L >& value) {
            m_unboundedItems_ = value;
        }

        void unboundedItems(::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 100L >&& value) {
            m_unboundedItems_ = std::move(value);
        }
        ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 2L >& boundedItems() OMG_NOEXCEPT {
            return m_boundedItems_;
        }

        const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 2L >& boundedItems() const OMG_NOEXCEPT {
            return m_boundedItems_;
        }

        void boundedItems(const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 2L >& value) {
            m_boundedItems_ = value;
        }

        void boundedItems(::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 2L >&& value) {
            m_boundedItems_ = std::move(value);
        }
        ::dds::core::array< CorpusIntegrationCollections::Item, 2L>& aggregateArray() OMG_NOEXCEPT {
            return m_aggregateArray_;
        }

        const ::dds::core::array< CorpusIntegrationCollections::Item, 2L>& aggregateArray() const OMG_NOEXCEPT {
            return m_aggregateArray_;
        }

        void aggregateArray(const ::dds::core::array< CorpusIntegrationCollections::Item, 2L>& value) {
            m_aggregateArray_ = value;
        }

        void aggregateArray(::dds::core::array< CorpusIntegrationCollections::Item, 2L>&& value) {
            m_aggregateArray_ = std::move(value);
        }
        ::dds::core::array< CorpusIntegrationCollections::Row, 2L>& rows() OMG_NOEXCEPT {
            return m_rows_;
        }

        const ::dds::core::array< CorpusIntegrationCollections::Row, 2L>& rows() const OMG_NOEXCEPT {
            return m_rows_;
        }

        void rows(const ::dds::core::array< CorpusIntegrationCollections::Row, 2L>& value) {
            m_rows_ = value;
        }

        void rows(::dds::core::array< CorpusIntegrationCollections::Row, 2L>&& value) {
            m_rows_ = std::move(value);
        }
        ::rti::core::bounded_sequence< CorpusIntegrationCollections::Row, 2L >& sequenceOfArrays() OMG_NOEXCEPT {
            return m_sequenceOfArrays_;
        }

        const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Row, 2L >& sequenceOfArrays() const OMG_NOEXCEPT {
            return m_sequenceOfArrays_;
        }

        void sequenceOfArrays(const ::rti::core::bounded_sequence< CorpusIntegrationCollections::Row, 2L >& value) {
            m_sequenceOfArrays_ = value;
        }

        void sequenceOfArrays(::rti::core::bounded_sequence< CorpusIntegrationCollections::Row, 2L >&& value) {
            m_sequenceOfArrays_ = std::move(value);
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 100L > m_unboundedItems_;
        ::rti::core::bounded_sequence< CorpusIntegrationCollections::Item, 2L > m_boundedItems_;
        ::dds::core::array< CorpusIntegrationCollections::Item, 2L> m_aggregateArray_;
        ::dds::core::array< CorpusIntegrationCollections::Row, 2L> m_rows_;
        ::rti::core::bounded_sequence< CorpusIntegrationCollections::Row, 2L > m_sequenceOfArrays_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusIntegrationCollections  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusIntegrationCollections::Item > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationCollections::Item";
            }
        };

        template<>
        struct is_topic_type< CorpusIntegrationCollections::Item > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusIntegrationCollections::Item > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusIntegrationCollections::Item& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusIntegrationCollections::Item& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusIntegrationCollections::Item& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusIntegrationCollections::Item& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusIntegrationCollections::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationCollections::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusIntegrationCollections::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusIntegrationCollections::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusIntegrationCollections::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusIntegrationCollections::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusIntegrationCollections::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusIntegrationCollections::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationCollections::Item > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusIntegrationCollections::Item > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationCollections::Row_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationCollections::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusIntegrationCollections::Sample > {
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

#endif // _13_compositions_1376405715_hpp

