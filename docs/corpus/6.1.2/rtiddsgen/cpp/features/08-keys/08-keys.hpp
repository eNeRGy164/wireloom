

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-keys.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _08_keys_1982703403_hpp
#define _08_keys_1982703403_hpp

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

namespace CorpusKeys {

    class NDDSUSERDllExport Simple {
      public:
        Simple();

        Simple(int32_t id,const std::string& name);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Simple (Simple&&) = default;
        Simple& operator=(Simple&&) = default;
        Simple& operator=(const Simple&) = default;
        Simple(const Simple&) = default;
        #else
        Simple(Simple&& other_) OMG_NOEXCEPT;  
        Simple& operator=(Simple&&  other_) OMG_NOEXCEPT;
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

        std::string& name() OMG_NOEXCEPT {
            return m_name_;
        }

        const std::string& name() const OMG_NOEXCEPT {
            return m_name_;
        }

        void name(const std::string& value) {
            m_name_ = value;
        }

        void name(std::string&& value) {
            m_name_ = std::move(value);
        }

        bool operator == (const Simple& other_) const;
        bool operator != (const Simple& other_) const;

        void swap(Simple& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_id_;
        std::string m_name_;

    };

    inline void swap(Simple& a, Simple& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Simple& sample);

    class NDDSUSERDllExport Identity {
      public:
        Identity();

        Identity(int32_t tenant,const std::string& name);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Identity (Identity&&) = default;
        Identity& operator=(Identity&&) = default;
        Identity& operator=(const Identity&) = default;
        Identity(const Identity&) = default;
        #else
        Identity(Identity&& other_) OMG_NOEXCEPT;  
        Identity& operator=(Identity&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& tenant() OMG_NOEXCEPT {
            return m_tenant_;
        }

        const int32_t& tenant() const OMG_NOEXCEPT {
            return m_tenant_;
        }

        void tenant(int32_t value) {
            m_tenant_ = value;
        }

        std::string& name() OMG_NOEXCEPT {
            return m_name_;
        }

        const std::string& name() const OMG_NOEXCEPT {
            return m_name_;
        }

        void name(const std::string& value) {
            m_name_ = value;
        }

        void name(std::string&& value) {
            m_name_ = std::move(value);
        }

        bool operator == (const Identity& other_) const;
        bool operator != (const Identity& other_) const;

        void swap(Identity& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_tenant_;
        std::string m_name_;

    };

    inline void swap(Identity& a, Identity& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Identity& sample);

    class NDDSUSERDllExport Sample {
      public:
        Sample();

        Sample(const CorpusKeys::Identity& identity,int32_t payload);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Sample (Sample&&) = default;
        Sample& operator=(Sample&&) = default;
        Sample& operator=(const Sample&) = default;
        Sample(const Sample&) = default;
        #else
        Sample(Sample&& other_) OMG_NOEXCEPT;  
        Sample& operator=(Sample&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        CorpusKeys::Identity& identity() OMG_NOEXCEPT {
            return m_identity_;
        }

        const CorpusKeys::Identity& identity() const OMG_NOEXCEPT {
            return m_identity_;
        }

        void identity(const CorpusKeys::Identity& value) {
            m_identity_ = value;
        }

        void identity(CorpusKeys::Identity&& value) {
            m_identity_ = std::move(value);
        }
        int32_t& payload() OMG_NOEXCEPT {
            return m_payload_;
        }

        const int32_t& payload() const OMG_NOEXCEPT {
            return m_payload_;
        }

        void payload(int32_t value) {
            m_payload_ = value;
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        CorpusKeys::Identity m_identity_;
        int32_t m_payload_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusKeys  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusKeys::Simple > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusKeys::Simple";
            }
        };

        template<>
        struct is_topic_type< CorpusKeys::Simple > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusKeys::Simple > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusKeys::Simple& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusKeys::Simple& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusKeys::Simple& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusKeys::Simple& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusKeys::Identity > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusKeys::Identity";
            }
        };

        template<>
        struct is_topic_type< CorpusKeys::Identity > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusKeys::Identity > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusKeys::Identity& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusKeys::Identity& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusKeys::Identity& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusKeys::Identity& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< CorpusKeys::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusKeys::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusKeys::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusKeys::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusKeys::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusKeys::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusKeys::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusKeys::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusKeys::Simple > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusKeys::Simple > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusKeys::Identity > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusKeys::Identity > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusKeys::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusKeys::Sample > {
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

#endif // _08_keys_1982703403_hpp

