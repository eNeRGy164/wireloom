

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-extensibility.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _09_extensibility_747423966_hpp
#define _09_extensibility_747423966_hpp

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

namespace CorpusExtensibility {

    class NDDSUSERDllExport Appendable {
      public:
        Appendable();

        Appendable(int32_t id,const std::string& text);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Appendable (Appendable&&) = default;
        Appendable& operator=(Appendable&&) = default;
        Appendable& operator=(const Appendable&) = default;
        Appendable(const Appendable&) = default;
        #else
        Appendable(Appendable&& other_) OMG_NOEXCEPT;  
        Appendable& operator=(Appendable&&  other_) OMG_NOEXCEPT;
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

        std::string& text() OMG_NOEXCEPT {
            return m_text_;
        }

        const std::string& text() const OMG_NOEXCEPT {
            return m_text_;
        }

        void text(const std::string& value) {
            m_text_ = value;
        }

        void text(std::string&& value) {
            m_text_ = std::move(value);
        }

        bool operator == (const Appendable& other_) const;
        bool operator != (const Appendable& other_) const;

        void swap(Appendable& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_id_;
        std::string m_text_;

    };

    inline void swap(Appendable& a, Appendable& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Appendable& sample);

    class NDDSUSERDllExport Final {
      public:
        Final();

        Final(int32_t id,const std::string& text);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Final (Final&&) = default;
        Final& operator=(Final&&) = default;
        Final& operator=(const Final&) = default;
        Final(const Final&) = default;
        #else
        Final(Final&& other_) OMG_NOEXCEPT;  
        Final& operator=(Final&&  other_) OMG_NOEXCEPT;
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

        std::string& text() OMG_NOEXCEPT {
            return m_text_;
        }

        const std::string& text() const OMG_NOEXCEPT {
            return m_text_;
        }

        void text(const std::string& value) {
            m_text_ = value;
        }

        void text(std::string&& value) {
            m_text_ = std::move(value);
        }

        bool operator == (const Final& other_) const;
        bool operator != (const Final& other_) const;

        void swap(Final& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_id_;
        std::string m_text_;

    };

    inline void swap(Final& a, Final& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Final& sample);

    class NDDSUSERDllExport Mutable {
      public:
        Mutable();

        Mutable(int32_t id,const ::dds::core::optional< int32_t >& optionalValue);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Mutable (Mutable&&) = default;
        Mutable& operator=(Mutable&&) = default;
        Mutable& operator=(const Mutable&) = default;
        Mutable(const Mutable&) = default;
        #else
        Mutable(Mutable&& other_) OMG_NOEXCEPT;  
        Mutable& operator=(Mutable&&  other_) OMG_NOEXCEPT;
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

        ::dds::core::optional< int32_t >& optionalValue() OMG_NOEXCEPT {
            return m_optionalValue_;
        }

        const ::dds::core::optional< int32_t >& optionalValue() const OMG_NOEXCEPT {
            return m_optionalValue_;
        }

        void optionalValue(const ::dds::core::optional< int32_t >& value) {
            m_optionalValue_ = value;
        }

        void optionalValue(::dds::core::optional< int32_t >&& value) {
            m_optionalValue_ = std::move(value);
        }

        bool operator == (const Mutable& other_) const;
        bool operator != (const Mutable& other_) const;

        void swap(Mutable& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_id_;
        ::dds::core::optional< int32_t > m_optionalValue_;

    };

    inline void swap(Mutable& a, Mutable& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Mutable& sample);

} // namespace CorpusExtensibility  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusExtensibility::Appendable > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusExtensibility::Appendable";
            }
        };

        template<>
        struct is_topic_type< CorpusExtensibility::Appendable > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusExtensibility::Appendable > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusExtensibility::Appendable& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusExtensibility::Appendable& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusExtensibility::Appendable& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusExtensibility::Appendable& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusExtensibility::Final > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusExtensibility::Final";
            }
        };

        template<>
        struct is_topic_type< CorpusExtensibility::Final > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusExtensibility::Final > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusExtensibility::Final& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusExtensibility::Final& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusExtensibility::Final& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusExtensibility::Final& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusExtensibility::Mutable > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusExtensibility::Mutable";
            }
        };

        template<>
        struct is_topic_type< CorpusExtensibility::Mutable > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusExtensibility::Mutable > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusExtensibility::Mutable& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusExtensibility::Mutable& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusExtensibility::Mutable& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusExtensibility::Mutable& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusExtensibility::Appendable > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusExtensibility::Appendable > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusExtensibility::Final > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusExtensibility::Final > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::FINAL;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusExtensibility::Mutable > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusExtensibility::Mutable > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::MUTABLE;                
        };

    }
}

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _09_extensibility_747423966_hpp

