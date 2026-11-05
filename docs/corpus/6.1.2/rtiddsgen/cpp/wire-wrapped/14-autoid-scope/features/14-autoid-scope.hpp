

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_autoid_scope_2088995224_hpp
#define _14_autoid_scope_2088995224_hpp

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

namespace AutoIds {

    class NDDSUSERDllExport Hashed {
      public:
        Hashed();

        Hashed(int32_t first,int32_t second);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Hashed (Hashed&&) = default;
        Hashed& operator=(Hashed&&) = default;
        Hashed& operator=(const Hashed&) = default;
        Hashed(const Hashed&) = default;
        #else
        Hashed(Hashed&& other_) OMG_NOEXCEPT;  
        Hashed& operator=(Hashed&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& first() OMG_NOEXCEPT {
            return m_first_;
        }

        const int32_t& first() const OMG_NOEXCEPT {
            return m_first_;
        }

        void first(int32_t value) {
            m_first_ = value;
        }

        int32_t& second() OMG_NOEXCEPT {
            return m_second_;
        }

        const int32_t& second() const OMG_NOEXCEPT {
            return m_second_;
        }

        void second(int32_t value) {
            m_second_ = value;
        }

        bool operator == (const Hashed& other_) const;
        bool operator != (const Hashed& other_) const;

        void swap(Hashed& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_first_;
        int32_t m_second_;

    };

    inline void swap(Hashed& a, Hashed& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Hashed& sample);

} // namespace AutoIds  
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "includes/autoid-hash-include.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif
namespace AutoIds {

    class NDDSUSERDllExport Reopened {
      public:
        Reopened();

        Reopened(int32_t first,int32_t second);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Reopened (Reopened&&) = default;
        Reopened& operator=(Reopened&&) = default;
        Reopened& operator=(const Reopened&) = default;
        Reopened(const Reopened&) = default;
        #else
        Reopened(Reopened&& other_) OMG_NOEXCEPT;  
        Reopened& operator=(Reopened&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        int32_t& first() OMG_NOEXCEPT {
            return m_first_;
        }

        const int32_t& first() const OMG_NOEXCEPT {
            return m_first_;
        }

        void first(int32_t value) {
            m_first_ = value;
        }

        int32_t& second() OMG_NOEXCEPT {
            return m_second_;
        }

        const int32_t& second() const OMG_NOEXCEPT {
            return m_second_;
        }

        void second(int32_t value) {
            m_second_ = value;
        }

        bool operator == (const Reopened& other_) const;
        bool operator != (const Reopened& other_) const;

        void swap(Reopened& other_) OMG_NOEXCEPT ;

      private:

        int32_t m_first_;
        int32_t m_second_;

    };

    inline void swap(Reopened& a, Reopened& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Reopened& sample);

} // namespace AutoIds  

class NDDSUSERDllExport Sequential {
  public:
    Sequential();

    Sequential(int32_t first,int32_t second);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Sequential (Sequential&&) = default;
    Sequential& operator=(Sequential&&) = default;
    Sequential& operator=(const Sequential&) = default;
    Sequential(const Sequential&) = default;
    #else
    Sequential(Sequential&& other_) OMG_NOEXCEPT;  
    Sequential& operator=(Sequential&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    int32_t& first() OMG_NOEXCEPT {
        return m_first_;
    }

    const int32_t& first() const OMG_NOEXCEPT {
        return m_first_;
    }

    void first(int32_t value) {
        m_first_ = value;
    }

    int32_t& second() OMG_NOEXCEPT {
        return m_second_;
    }

    const int32_t& second() const OMG_NOEXCEPT {
        return m_second_;
    }

    void second(int32_t value) {
        m_second_ = value;
    }

    bool operator == (const Sequential& other_) const;
    bool operator != (const Sequential& other_) const;

    void swap(Sequential& other_) OMG_NOEXCEPT ;

  private:

    int32_t m_first_;
    int32_t m_second_;

};

inline void swap(Sequential& a, Sequential& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sequential& sample);

class NDDSUSERDllExport UsesInclude {
  public:
    UsesInclude();

    explicit UsesInclude(const Included& included);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    UsesInclude (UsesInclude&&) = default;
    UsesInclude& operator=(UsesInclude&&) = default;
    UsesInclude& operator=(const UsesInclude&) = default;
    UsesInclude(const UsesInclude&) = default;
    #else
    UsesInclude(UsesInclude&& other_) OMG_NOEXCEPT;  
    UsesInclude& operator=(UsesInclude&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    Included& included() OMG_NOEXCEPT {
        return m_included_;
    }

    const Included& included() const OMG_NOEXCEPT {
        return m_included_;
    }

    void included(const Included& value) {
        m_included_ = value;
    }

    void included(Included&& value) {
        m_included_ = std::move(value);
    }

    bool operator == (const UsesInclude& other_) const;
    bool operator != (const UsesInclude& other_) const;

    void swap(UsesInclude& other_) OMG_NOEXCEPT ;

  private:

    Included m_included_;

};

inline void swap(UsesInclude& a, UsesInclude& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const UsesInclude& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< AutoIds::Hashed > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIds::Hashed";
            }
        };

        template<>
        struct is_topic_type< AutoIds::Hashed > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< AutoIds::Hashed > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const AutoIds::Hashed& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(AutoIds::Hashed& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(AutoIds::Hashed& sample);

            NDDSUSERDllExport 
            static void allocate_sample(AutoIds::Hashed& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< AutoIds::Reopened > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIds::Reopened";
            }
        };

        template<>
        struct is_topic_type< AutoIds::Reopened > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< AutoIds::Reopened > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const AutoIds::Reopened& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(AutoIds::Reopened& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(AutoIds::Reopened& sample);

            NDDSUSERDllExport 
            static void allocate_sample(AutoIds::Reopened& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< Sequential > {
            NDDSUSERDllExport static std::string value() {
                return "Sequential";
            }
        };

        template<>
        struct is_topic_type< Sequential > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< Sequential > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const Sequential& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(Sequential& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(Sequential& sample);

            NDDSUSERDllExport 
            static void allocate_sample(Sequential& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< UsesInclude > {
            NDDSUSERDllExport static std::string value() {
                return "UsesInclude";
            }
        };

        template<>
        struct is_topic_type< UsesInclude > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< UsesInclude > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const UsesInclude& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(UsesInclude& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(UsesInclude& sample);

            NDDSUSERDllExport 
            static void allocate_sample(UsesInclude& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< AutoIds::Hashed > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< AutoIds::Hashed > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< AutoIds::Reopened > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< AutoIds::Reopened > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< Sequential > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< Sequential > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< UsesInclude > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< UsesInclude > {
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

#endif // _14_autoid_scope_2088995224_hpp

