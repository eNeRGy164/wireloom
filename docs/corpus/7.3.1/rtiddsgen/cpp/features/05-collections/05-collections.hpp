

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-collections.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _05_collections_1968779365_hpp
#define _05_collections_1968779365_hpp

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

namespace CorpusCollections {

    class NDDSUSERDllExport Item {
      public:

        Item();

        Item(int32_t id_,const std::string& label_);

        int32_t& id() noexcept {
            return m_id_;
        }

        const int32_t& id() const noexcept {
            return m_id_;
        }

        void id(int32_t value) {

            m_id_ = value;
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
        bool operator == (const Item& other_) const;
        bool operator != (const Item& other_) const;

        void swap(Item& other_) noexcept ;

      private:

        int32_t m_id_;
        std::string m_label_;

    };

    inline void swap(Item& a, Item& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Item& sample);

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    typedef ::rti::core::bounded_sequence< int32_t, 4L > BoundedLongs;
    struct BoundedLongs_AliasTag_t {};

    typedef ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L> CoordinateGrid;
    struct CoordinateGrid_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< ::CorpusCollections::Item >;
    template class NDDSUSERDllExport std::vector< ::CorpusCollections::Item >;
    #endif
    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values_,const ::rti::core::bounded_sequence< int32_t, 100L >& unbounded_,const ::CorpusCollections::BoundedLongs& bounded_,const ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L >& items_,const ::CorpusCollections::CoordinateGrid& grid_);

        ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values() noexcept {
            return m_values_;
        }

        const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values() const noexcept {
            return m_values_;
        }

        void values(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& value) {

            m_values_ = value;
        }

        void values(::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>&& value) {
            m_values_ = std::move(value);
        }
        ::rti::core::bounded_sequence< int32_t, 100L >& unbounded() noexcept {
            return m_unbounded_;
        }

        const ::rti::core::bounded_sequence< int32_t, 100L >& unbounded() const noexcept {
            return m_unbounded_;
        }

        void unbounded(const ::rti::core::bounded_sequence< int32_t, 100L >& value) {

            m_unbounded_ = value;
        }

        void unbounded(::rti::core::bounded_sequence< int32_t, 100L >&& value) {
            m_unbounded_ = std::move(value);
        }
        ::CorpusCollections::BoundedLongs& bounded() noexcept {
            return m_bounded_;
        }

        const ::CorpusCollections::BoundedLongs& bounded() const noexcept {
            return m_bounded_;
        }

        void bounded(const ::CorpusCollections::BoundedLongs& value) {

            m_bounded_ = value;
        }

        void bounded(::CorpusCollections::BoundedLongs&& value) {
            m_bounded_ = std::move(value);
        }
        ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L >& items() noexcept {
            return m_items_;
        }

        const ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L >& items() const noexcept {
            return m_items_;
        }

        void items(const ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L >& value) {

            m_items_ = value;
        }

        void items(::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L >&& value) {
            m_items_ = std::move(value);
        }
        ::CorpusCollections::CoordinateGrid& grid() noexcept {
            return m_grid_;
        }

        const ::CorpusCollections::CoordinateGrid& grid() const noexcept {
            return m_grid_;
        }

        void grid(const ::CorpusCollections::CoordinateGrid& value) {

            m_grid_ = value;
        }

        void grid(::CorpusCollections::CoordinateGrid&& value) {
            m_grid_ = std::move(value);
        }
        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L> m_values_ {};
        ::rti::core::bounded_sequence< int32_t, 100L > m_unbounded_;
        ::CorpusCollections::BoundedLongs m_bounded_;
        ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L > m_items_;
        ::CorpusCollections::CoordinateGrid m_grid_ {};

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusCollections  

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
        struct topic_type_name< ::CorpusCollections::Item > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusCollections::Item";
            }
        };

        template<>
        struct is_topic_type< ::CorpusCollections::Item > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusCollections::Item > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusCollections::Item& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusCollections::Item& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusCollections::Item& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusCollections::Item& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusCollections::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusCollections::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusCollections::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusCollections::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusCollections::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusCollections::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusCollections::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusCollections::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusCollections::Item > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusCollections::Item > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusCollections::BoundedLongs_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollections::CoordinateGrid_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollections::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusCollections::Sample > {
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

#endif // _05_collections_1968779365_hpp

