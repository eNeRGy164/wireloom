

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-shapes.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _05_shapes_1457046950_hpp
#define _05_shapes_1457046950_hpp

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

namespace CorpusCollectionShapes {

    struct NDDSUSERDllExport Item {
        int32_t id {};
        std::string label {};

        Item();

        Item(int32_t id_,const ::omg::types::string_view& label_);

    };

    NDDSUSERDllExport bool operator == (const Item& a, const Item& b);
    NDDSUSERDllExport bool operator != (const Item& a, const Item& b);
    NDDSUSERDllExport void swap(Item& a, Item& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Item& sample);

    typedef ::dds::core::array< int32_t, 3L> Row;
    struct Row_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    typedef ::omg::types::bounded_sequence< int32_t, 4L > BoundedLongs;
    struct BoundedLongs_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< ::CorpusCollectionShapes::Row >;
    template class NDDSUSERDllExport std::vector< ::CorpusCollectionShapes::Row >;
    #endif
    typedef ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Row, 2L > Rows;
    struct Rows_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< ::CorpusCollectionShapes::Item >;
    template class NDDSUSERDllExport std::vector< ::CorpusCollectionShapes::Item >;
    #endif
    typedef ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Item, 3L > Items;
    struct Items_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< std::string >;
    template class NDDSUSERDllExport std::vector< std::string >;
    #endif
    typedef ::omg::types::bounded_sequence< std::string, 4L > Names;
    struct Names_AliasTag_t {};

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< ::CorpusCollectionShapes::BoundedLongs >;
    template class NDDSUSERDllExport std::vector< ::CorpusCollectionShapes::BoundedLongs >;
    template class NDDSUSERDllExport std::allocator< ::CorpusCollectionShapes::Rows >;
    template class NDDSUSERDllExport std::vector< ::CorpusCollectionShapes::Rows >;
    #endif
    struct NDDSUSERDllExport Sample {
        ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Item, 100L > unboundedItems {};
        ::CorpusCollectionShapes::Items boundedItems {};
        ::omg::types::bounded_sequence< ::CorpusCollectionShapes::BoundedLongs, 2L > nestedSequences {};
        ::dds::core::array< ::CorpusCollectionShapes::Row, 2L> rows {};
        ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Row, 2L > sequenceOfArrays {};
        ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Rows, 2L > sequenceOfArrayAliases {};
        ::CorpusCollectionShapes::Names names {};

        Sample();

        Sample(const ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Item, 100L >& unboundedItems_,const ::CorpusCollectionShapes::Items& boundedItems_,const ::omg::types::bounded_sequence< ::CorpusCollectionShapes::BoundedLongs, 2L >& nestedSequences_,const ::dds::core::array< ::CorpusCollectionShapes::Row, 2L>& rows_,const ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Row, 2L >& sequenceOfArrays_,const ::omg::types::bounded_sequence< ::CorpusCollectionShapes::Rows, 2L >& sequenceOfArrayAliases_,const ::CorpusCollectionShapes::Names& names_);

    };

    NDDSUSERDllExport bool operator == (const Sample& a, const Sample& b);
    NDDSUSERDllExport bool operator != (const Sample& a, const Sample& b);
    NDDSUSERDllExport void swap(Sample& a, Sample& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusCollectionShapes  

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
        struct topic_type_name< ::CorpusCollectionShapes::Item > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusCollectionShapes::Item";
            }
        };

        template<>
        struct is_topic_type< ::CorpusCollectionShapes::Item > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusCollectionShapes::Item > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusCollectionShapes::Item& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusCollectionShapes::Item& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusCollectionShapes::Item& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusCollectionShapes::Item& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusCollectionShapes::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusCollectionShapes::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusCollectionShapes::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusCollectionShapes::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusCollectionShapes::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusCollectionShapes::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusCollectionShapes::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusCollectionShapes::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::Item > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusCollectionShapes::Item > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::Row_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::BoundedLongs_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::Rows_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::Items_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::Names_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };

        template<>
        struct dynamic_type< ::CorpusCollectionShapes::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusCollectionShapes::Sample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _05_shapes_1457046950_hpp
