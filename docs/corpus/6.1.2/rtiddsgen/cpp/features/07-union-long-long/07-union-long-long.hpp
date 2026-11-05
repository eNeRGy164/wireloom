

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-long-long.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _07_union_long_long_479170005_hpp
#define _07_union_long_long_479170005_hpp

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

namespace CorpusLongLongUnion {

    class NDDSUSERDllExport Choice {
      public:
        Choice();

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Choice (Choice&&) = default;
        Choice& operator=(Choice&&) = default;
        Choice& operator=(const Choice&) = default;
        Choice(const Choice&) = default;
        #else
        Choice(Choice&& other_) OMG_NOEXCEPT;  
        Choice& operator=(Choice&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int64_t& _d()  {
            return m_d_;
        }

        const int64_t& _d() const  {
            return m_d_;
        }

        void _d(int64_t value) {
            m_d_ = value;
        }

        int32_t& negative()  {
            if ( _d() != -7) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::negative not selected by the discriminator" );
            }
            return m_u_.m_negative_;
        }

        const int32_t& negative() const  {
            if ( _d() != -7) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::negative not selected by the discriminator" );
            }
            return m_u_.m_negative_;
        }

        void negative(int32_t value) {
            m_u_.m_negative_ = value;
            m_d_= -7;
        }

        std::string& text()  {
            if ( _d() != 7) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::text not selected by the discriminator" );
            }
            return m_u_.m_text_;
        }

        const std::string& text() const  {
            if ( _d() != 7) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::text not selected by the discriminator" );
            }
            return m_u_.m_text_;
        }

        void text(const std::string& value) {
            m_u_.m_text_ = value;
            m_d_= 7;
        }

        void text(std::string&& value) {
            m_u_.m_text_ = std::move(value);
            m_d_= 7;
        }
        bool& other()  {
            if (!(_d() != -7 &&
            _d() != 7)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::other not selected by the discriminator" );
            }
            return m_u_.m_other_;
        }

        const bool& other() const  {
            if (!(_d() != -7 &&
            _d() != 7)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::other not selected by the discriminator" );
            }
            return m_u_.m_other_;
        }

        void other(bool value) {
            if (!(_d() != -7 &&
            _d() != 7)) {
                throw ::dds::core::PreconditionNotMetError(
                    "CorpusLongLongUnion::Choice::other not selected by the discriminator" );
            }
            m_u_.m_other_ = value;
        }

        bool operator == (const Choice& other_) const;
        bool operator != (const Choice& other_) const;

        static int64_t default_discriminator();  

        void swap(Choice& other_) OMG_NOEXCEPT ;

      private:

        int64_t m_d_;
        struct NDDSUSERDllExport Union_ {
            int32_t m_negative_;
            std::string m_text_;
            bool m_other_;
            Union_();
            Union_(int32_t negative,const std::string& text,bool other);
        };
        Union_ m_u_;

    };

    inline void swap(Choice& a, Choice& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Choice& sample);

} // namespace CorpusLongLongUnion  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusLongLongUnion::Choice > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusLongLongUnion::Choice";
            }
        };

        template<>
        struct is_topic_type< CorpusLongLongUnion::Choice > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusLongLongUnion::Choice > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusLongLongUnion::Choice& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusLongLongUnion::Choice& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusLongLongUnion::Choice& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusLongLongUnion::Choice& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusLongLongUnion::Choice > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusLongLongUnion::Choice > {
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

#endif // _07_union_long_long_479170005_hpp

