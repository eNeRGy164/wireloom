

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-constant-expressions.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _02_constant_expressions_2027315572_hpp
#define _02_constant_expressions_2027315572_hpp

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

namespace CorpusConstantExpressions {

    static const int32_t Add = 2L+3L;

    static const int32_t Shifted = ((CorpusConstantExpressions::Add)<<2L)|1L;

    static const int32_t Masked = ((CorpusConstantExpressions::Shifted)&15L)^3L;

    static const int32_t Bound = (CorpusConstantExpressions::Masked)-1L;

    #if (defined(RTI_WIN32) || defined (RTI_WINCE)) && defined(NDDS_USER_DLL_EXPORT)
    // On Windows, dll-export template instantiations of standard types used by
    // other dll-exported types
    template class NDDSUSERDllExport std::allocator< int32_t >;
    template class NDDSUSERDllExport std::vector< int32_t >;
    #endif
    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(const ::rti::core::bounded_sequence< int32_t, (CorpusConstantExpressions::Bound) >& values_,const ::dds::core::array< int32_t, (CorpusConstantExpressions::Add)>& array_);

        ::rti::core::bounded_sequence< int32_t, (CorpusConstantExpressions::Bound) >& values() noexcept {
            return m_values_;
        }

        const ::rti::core::bounded_sequence< int32_t, (CorpusConstantExpressions::Bound) >& values() const noexcept {
            return m_values_;
        }

        void values(const ::rti::core::bounded_sequence< int32_t, (CorpusConstantExpressions::Bound) >& value) {

            m_values_ = value;
        }

        void values(::rti::core::bounded_sequence< int32_t, (CorpusConstantExpressions::Bound) >&& value) {
            m_values_ = std::move(value);
        }
        ::dds::core::array< int32_t, (CorpusConstantExpressions::Add)>& array() noexcept {
            return m_array_;
        }

        const ::dds::core::array< int32_t, (CorpusConstantExpressions::Add)>& array() const noexcept {
            return m_array_;
        }

        void array(const ::dds::core::array< int32_t, (CorpusConstantExpressions::Add)>& value) {

            m_array_ = value;
        }

        void array(::dds::core::array< int32_t, (CorpusConstantExpressions::Add)>&& value) {
            m_array_ = std::move(value);
        }
        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        ::rti::core::bounded_sequence< int32_t, (CorpusConstantExpressions::Bound) > m_values_;
        ::dds::core::array< int32_t, (CorpusConstantExpressions::Add)> m_array_ {};

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusConstantExpressions  

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
        struct topic_type_name< ::CorpusConstantExpressions::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusConstantExpressions::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusConstantExpressions::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusConstantExpressions::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusConstantExpressions::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusConstantExpressions::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusConstantExpressions::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusConstantExpressions::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusConstantExpressions::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusConstantExpressions::Sample > {
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

#endif // _02_constant_expressions_2027315572_hpp

