

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-recursive-array.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_optional_recursive_array_1458846562_hpp
#define _09_optional_recursive_array_1458846562_hpp

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

namespace CorpusOptionalRecursiveArray {

    class NDDSUSERDllExport Node {
      public:

        Node();

        Node(int32_t value_,const ::dds::core::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> >& children_);

        int32_t& value() noexcept {
            return m_value_;
        }

        const int32_t& value() const noexcept {
            return m_value_;
        }

        void value(int32_t value) {

            m_value_ = value;
        }

        ::dds::core::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> >& children() noexcept; 
        const ::dds::core::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> >& children() const noexcept;
        void children(const ::dds::core::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> >& value);
        void children(::dds::core::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> >&& value);
        bool operator == (const Node& other_) const;
        bool operator != (const Node& other_) const;

        void swap(Node& other_) noexcept ;

      private:

        int32_t m_value_;
        ::dds::core::optional< ::dds::core::array< ::CorpusOptionalRecursiveArray::Node, 2L> > m_children_;

    };

    inline void swap(Node& a, Node& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Node& sample);

} // namespace CorpusOptionalRecursiveArray  

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
        struct topic_type_name< ::CorpusOptionalRecursiveArray::Node > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusOptionalRecursiveArray::Node";
            }
        };

        template<>
        struct is_topic_type< ::CorpusOptionalRecursiveArray::Node > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusOptionalRecursiveArray::Node > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusOptionalRecursiveArray::Node& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusOptionalRecursiveArray::Node& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusOptionalRecursiveArray::Node& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusOptionalRecursiveArray::Node& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusOptionalRecursiveArray::Node > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get(RTICdrResourceNode *parentVisitedNode);
        };

        template <>
        struct extensibility< ::CorpusOptionalRecursiveArray::Node > {
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

#endif // _09_optional_recursive_array_1458846562_hpp

