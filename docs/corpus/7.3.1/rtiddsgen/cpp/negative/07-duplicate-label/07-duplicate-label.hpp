

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-duplicate-label.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_duplicate_label_516395718_hpp
#define _07_duplicate_label_516395718_hpp

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

namespace CorpusNegativeDuplicateLabel {

    class NDDSUSERDllExport Choice {
      public:

        Choice();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        int32_t& first()  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateLabel::Choice::first not selected by the discriminator" );
            }
            return m_u_.m_first_;
        }

        const int32_t& first() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateLabel::Choice::first not selected by the discriminator" );
            }
            return m_u_.m_first_;
        }

        void first(int32_t value) {
            m_u_.m_first_ = value;
            m_d_= 1;
        }

        int32_t& second()  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateLabel::Choice::second not selected by the discriminator" );
            }
            return m_u_.m_second_;
        }

        const int32_t& second() const  {

            if (_d() != 1 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusNegativeDuplicateLabel::Choice::second not selected by the discriminator" );
            }
            return m_u_.m_second_;
        }

        void second(int32_t value) {
            m_u_.m_second_ = value;
            m_d_= 1;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;
        static int32_t default_discriminator();

        void swap(Choice& other_) noexcept ;

      private:

        int32_t m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_first_;
            int32_t m_second_;
            Union_();
            Union_(int32_t first_,int32_t second_);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusNegativeDuplicateLabel  

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
        struct topic_type_name< ::CorpusNegativeDuplicateLabel::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusNegativeDuplicateLabel::Choice";
            }
        };

        template<>
        struct is_topic_type< ::CorpusNegativeDuplicateLabel::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusNegativeDuplicateLabel::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusNegativeDuplicateLabel::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusNegativeDuplicateLabel::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusNegativeDuplicateLabel::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusNegativeDuplicateLabel::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusNegativeDuplicateLabel::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusNegativeDuplicateLabel::Choice > {
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

#endif // _07_duplicate_label_516395718_hpp

