

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _13_alias_inheritance_745606614_hpp
#define _13_alias_inheritance_745606614_hpp

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

namespace CorpusIntegrationTrace {

    typedef std::string Producer;
    struct Producer_AliasTag_t {};

    class NDDSUSERDllExport Context {
      public:
        Context();

        Context(const std::string& correlation,const ::dds::core::optional< std::string >& producer);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Context (Context&&) = default;
        Context& operator=(Context&&) = default;
        Context& operator=(const Context&) = default;
        Context(const Context&) = default;
        #else
        Context(Context&& other_) OMG_NOEXCEPT;  
        Context& operator=(Context&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        std::string& correlation() OMG_NOEXCEPT {
            return m_correlation_;
        }

        const std::string& correlation() const OMG_NOEXCEPT {
            return m_correlation_;
        }

        void correlation(const std::string& value) {
            m_correlation_ = value;
        }

        void correlation(std::string&& value) {
            m_correlation_ = std::move(value);
        }
        ::dds::core::optional< std::string >& producer() OMG_NOEXCEPT {
            return m_producer_;
        }

        const ::dds::core::optional< std::string >& producer() const OMG_NOEXCEPT {
            return m_producer_;
        }

        void producer(const ::dds::core::optional< std::string >& value) {
            m_producer_ = value;
        }

        void producer(::dds::core::optional< std::string >&& value) {
            m_producer_ = std::move(value);
        }

        bool operator == (const Context& other_) const;
        bool operator != (const Context& other_) const;

        void swap(Context& other_) OMG_NOEXCEPT ;

      private:

        std::string m_correlation_;
        ::dds::core::optional< std::string > m_producer_;

    };

    inline void swap(Context& a, Context& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Context& sample);

    class NDDSUSERDllExport Base {
      public:
        Base();

        explicit Base(int32_t baseValue);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Base (Base&&) = default;
        Base& operator=(Base&&) = default;
        Base& operator=(const Base&) = default;
        Base(const Base&) = default;
        #else
        Base(Base&& other_) OMG_NOEXCEPT;  
        Base& operator=(Base&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& baseValue() OMG_NOEXCEPT {
            return m_baseValue_;
        }

        const int32_t& baseValue() const OMG_NOEXCEPT {
            return m_baseValue_;
        }

        void baseValue(int32_t value) {
            m_baseValue_ = value;
        }

        bool operator == (const Base& other_) const;
        bool operator != (const Base& other_) const;

        void swap(Base& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_baseValue_;

    };

    inline void swap(Base& a, Base& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Base& sample);

    class NDDSUSERDllExport Derived
    : public CorpusIntegrationTrace::Base {
      public:
        Derived();

        Derived(int32_t baseValue,const std::string& state,const CorpusIntegrationTrace::Context& traceContext);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Derived (Derived&&) = default;
        Derived& operator=(Derived&&) = default;
        Derived& operator=(const Derived&) = default;
        Derived(const Derived&) = default;
        #else
        Derived(Derived&& other_) OMG_NOEXCEPT;  
        Derived& operator=(Derived&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        std::string& state() OMG_NOEXCEPT {
            return m_state_;
        }

        const std::string& state() const OMG_NOEXCEPT {
            return m_state_;
        }

        void state(const std::string& value) {
            m_state_ = value;
        }

        void state(std::string&& value) {
            m_state_ = std::move(value);
        }
        CorpusIntegrationTrace::Context& traceContext() OMG_NOEXCEPT {
            return m_traceContext_;
        }

        const CorpusIntegrationTrace::Context& traceContext() const OMG_NOEXCEPT {
            return m_traceContext_;
        }

        void traceContext(const CorpusIntegrationTrace::Context& value) {
            m_traceContext_ = value;
        }

        void traceContext(CorpusIntegrationTrace::Context&& value) {
            m_traceContext_ = std::move(value);
        }

        bool operator == (const Derived& other_) const;
        bool operator != (const Derived& other_) const;

        void swap(Derived& other_) OMG_NOEXCEPT ;

      private:

        std::string m_state_;
        CorpusIntegrationTrace::Context m_traceContext_;

    };

    inline void swap(Derived& a, Derived& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Derived& sample);

} // namespace CorpusIntegrationTrace  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusIntegrationTrace::Context > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationTrace::Context";
            }
        };

        template<>
        struct is_topic_type< CorpusIntegrationTrace::Context > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusIntegrationTrace::Context > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusIntegrationTrace::Context& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusIntegrationTrace::Context& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusIntegrationTrace::Context& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusIntegrationTrace::Context& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusIntegrationTrace::Base > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationTrace::Base";
            }
        };

        template<>
        struct is_topic_type< CorpusIntegrationTrace::Base > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusIntegrationTrace::Base > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusIntegrationTrace::Base& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusIntegrationTrace::Base& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusIntegrationTrace::Base& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusIntegrationTrace::Base& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusIntegrationTrace::Derived > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusIntegrationTrace::Derived";
            }
        };

        template<>
        struct is_topic_type< CorpusIntegrationTrace::Derived > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusIntegrationTrace::Derived > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusIntegrationTrace::Derived& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusIntegrationTrace::Derived& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusIntegrationTrace::Derived& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusIntegrationTrace::Derived& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationTrace::Producer_AliasTag_t > {
            typedef ::dds::core::xtypes::AliasType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::AliasType& get();
        };
        #endif

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationTrace::Context > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusIntegrationTrace::Context > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationTrace::Base > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusIntegrationTrace::Base > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusIntegrationTrace::Derived > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusIntegrationTrace::Derived > {
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

#endif // _13_alias_inheritance_745606614_hpp

