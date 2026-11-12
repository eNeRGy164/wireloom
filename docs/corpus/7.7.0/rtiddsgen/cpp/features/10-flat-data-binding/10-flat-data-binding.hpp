

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-flat-data-binding.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _10_flat_data_binding_532422622_hpp
#define _10_flat_data_binding_532422622_hpp

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

#include "rti/topic/flat/FlatData.hpp"

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

namespace CorpusFlatDataBinding {

    class NDDSUSERDllExport SampleConstOffset 
    : public ::rti::flat::FinalOffset< SampleConstOffset > {
      public:
        enum { required_alignment = 4 };

        static ::rti::flat::offset_t serialized_size(::rti::flat::offset_t initial_position)
        {
            static const ::rti::flat::offset_t sizes[] = {8, 8, 8, 8};
            return sizes[initial_position % 4];
        }

        // Null offset
        SampleConstOffset()
        {
        }

        // Constructor is for internal use only
        SampleConstOffset(
            const ::rti::flat::SampleBase *sample, 
            ::rti::flat::offset_t offset)
            : ::rti::flat::FinalOffset< SampleConstOffset >(const_cast< ::rti::flat::SampleBase * >(sample), offset)
        {
        }

        int32_t id() const; 
        int32_t value() const; 
    };
    class NDDSUSERDllExport SampleOffset 
    : public ::rti::flat::FinalOffset< SampleOffset > {
      public:
        typedef SampleConstOffset ConstOffset;

        enum { required_alignment = 4 };

        static ::rti::flat::offset_t serialized_size(::rti::flat::offset_t initial_position)
        {
            static const ::rti::flat::offset_t sizes[] = {8, 8, 8, 8};
            return sizes[initial_position % 4];
        }

        // Null offset
        SampleOffset()
        {
        }

        // Constructor is for internal use only
        SampleOffset(
            ::rti::flat::SampleBase *sample, 
            ::rti::flat::offset_t offset)
            : ::rti::flat::FinalOffset< SampleOffset >(sample, offset)
        {
        }

        // Const accessors
        int32_t id() const; 
        int32_t value() const; 

        // Modifiers
        bool id(int32_t value);
        bool value(int32_t value);
    };

    typedef ::rti::flat::Sample< SampleOffset > Sample;

    struct NDDSUSERDllExport SamplePlainHelper {
        int32_t id {};
        int32_t value {};

        SamplePlainHelper();

        SamplePlainHelper(int32_t id_,int32_t value_);

    };

    NDDSUSERDllExport bool operator == (const SamplePlainHelper& a, const SamplePlainHelper& b);
    NDDSUSERDllExport bool operator != (const SamplePlainHelper& a, const SamplePlainHelper& b);
    NDDSUSERDllExport void swap(SamplePlainHelper& a, SamplePlainHelper& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const SamplePlainHelper& sample);

} // namespace CorpusFlatDataBinding  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
    }
}
#else

namespace rti {
    namespace flat {
        namespace topic {
            template<>
            struct is_flat_data_type_only< ::CorpusFlatDataBinding::Sample > : public ::dds::core::true_type {};
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< ::CorpusFlatDataBinding::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusFlatDataBinding::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusFlatDataBinding::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusFlatDataBinding::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusFlatDataBinding::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusFlatDataBinding::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusFlatDataBinding::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusFlatDataBinding::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusFlatDataBinding::SamplePlainHelper > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusFlatDataBinding::SamplePlainHelper";
            }
        };

        template<>
        struct is_topic_type< ::CorpusFlatDataBinding::SamplePlainHelper > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusFlatDataBinding::SamplePlainHelper > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusFlatDataBinding::SamplePlainHelper& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusFlatDataBinding::SamplePlainHelper& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusFlatDataBinding::SamplePlainHelper& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusFlatDataBinding::SamplePlainHelper& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusFlatDataBinding::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusFlatDataBinding::Sample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::FINAL;    };

        template<>
        struct dynamic_type< ::CorpusFlatDataBinding::SamplePlainHelper > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusFlatDataBinding::SamplePlainHelper > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::FINAL;    };

    }
}

namespace rti { namespace xcdr {
        template <>
        struct type_programs< ::CorpusFlatDataBinding::Sample > {
            static RTIXCdrInterpreterPrograms * get();
        };
    } }

namespace rti { namespace flat {

        template <>
        struct flat_type_traits< ::CorpusFlatDataBinding::Sample > {
            typedef ::CorpusFlatDataBinding::SamplePlainHelper plain_type;
            typedef ::CorpusFlatDataBinding::SampleOffset offset;
        };

        template <>
        struct flat_type_traits< ::CorpusFlatDataBinding::SampleOffset > {
            typedef ::CorpusFlatDataBinding::Sample flat_type;
            typedef ::CorpusFlatDataBinding::SamplePlainHelper plain_type;
        };

        template <>
        struct flat_type_traits< ::CorpusFlatDataBinding::SampleConstOffset > 
        : flat_type_traits< ::CorpusFlatDataBinding::SampleOffset > {
        };

    } } // namespace ::rti::flat

#include "rti/flat/FlatSampleImpl.hpp"

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _10_flat_data_binding_532422622_hpp
