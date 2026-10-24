

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662736082_hpp
#define idl_1662736082_hpp

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

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "features/07-union-string-sequence-aliases.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif
namespace RuntimeUnionStringSequences {

    class NDDSUSERDllExport StringSequenceUnionsSample {
      public:

        StringSequenceUnionsSample();

        StringSequenceUnionsSample(const ::RuntimeUnionStringSequences::DirectStringSequences& direct_,const ::RuntimeUnionStringSequences::StringSequenceAliases& aliases_);

        ::RuntimeUnionStringSequences::DirectStringSequences& direct() noexcept {
            return m_direct_;
        }

        const ::RuntimeUnionStringSequences::DirectStringSequences& direct() const noexcept {
            return m_direct_;
        }

        void direct(const ::RuntimeUnionStringSequences::DirectStringSequences& value) {

            m_direct_ = value;
        }

        void direct(::RuntimeUnionStringSequences::DirectStringSequences&& value) {
            m_direct_ = std::move(value);
        }
        ::RuntimeUnionStringSequences::StringSequenceAliases& aliases() noexcept {
            return m_aliases_;
        }

        const ::RuntimeUnionStringSequences::StringSequenceAliases& aliases() const noexcept {
            return m_aliases_;
        }

        void aliases(const ::RuntimeUnionStringSequences::StringSequenceAliases& value) {

            m_aliases_ = value;
        }

        void aliases(::RuntimeUnionStringSequences::StringSequenceAliases&& value) {
            m_aliases_ = std::move(value);
        }
        bool operator == (const StringSequenceUnionsSample& other_) const;
        bool operator != (const StringSequenceUnionsSample& other_) const;

        void swap(StringSequenceUnionsSample& other_) noexcept ;

      private:

        ::RuntimeUnionStringSequences::DirectStringSequences m_direct_;
        ::RuntimeUnionStringSequences::StringSequenceAliases m_aliases_;

    };

    inline void swap(StringSequenceUnionsSample& a, StringSequenceUnionsSample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const StringSequenceUnionsSample& sample);

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
        struct topic_type_name< ::RuntimeUnionStringSequences::StringSequenceUnionsSample > {
            NDDSUSERDllExport static std::string value() {
                return "RuntimeUnionStringSequences::StringSequenceUnionsSample";
            }
        };

        template<>
        struct is_topic_type< ::RuntimeUnionStringSequences::StringSequenceUnionsSample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::RuntimeUnionStringSequences::StringSequenceUnionsSample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::RuntimeUnionStringSequences::StringSequenceUnionsSample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::RuntimeUnionStringSequences::StringSequenceUnionsSample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::RuntimeUnionStringSequences::StringSequenceUnionsSample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::RuntimeUnionStringSequences::StringSequenceUnionsSample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::RuntimeUnionStringSequences::StringSequenceUnionsSample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::RuntimeUnionStringSequences::StringSequenceUnionsSample > {
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

#endif // idl_1662736082_hpp

