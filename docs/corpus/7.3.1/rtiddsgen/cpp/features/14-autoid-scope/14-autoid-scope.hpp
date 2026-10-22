

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

namespace AutoIds {

    class NDDSUSERDllExport Hashed {
      public:

        Hashed();

        Hashed(int32_t first_,int32_t second_);

        int32_t& first() noexcept {
            return m_first_;
        }

        const int32_t& first() const noexcept {
            return m_first_;
        }

        void first(int32_t value) {

            m_first_ = value;
        }

        int32_t& second() noexcept {
            return m_second_;
        }

        const int32_t& second() const noexcept {
            return m_second_;
        }

        void second(int32_t value) {

            m_second_ = value;
        }

        bool operator == (const Hashed& other_) const;
        bool operator != (const Hashed& other_) const;

        void swap(Hashed& other_) noexcept ;

      private:

        int32_t m_first_;
        int32_t m_second_;

    };

    inline void swap(Hashed& a, Hashed& b)  noexcept 
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

        Reopened(int32_t first_,int32_t second_);

        int32_t& first() noexcept {
            return m_first_;
        }

        const int32_t& first() const noexcept {
            return m_first_;
        }

        void first(int32_t value) {

            m_first_ = value;
        }

        int32_t& second() noexcept {
            return m_second_;
        }

        const int32_t& second() const noexcept {
            return m_second_;
        }

        void second(int32_t value) {

            m_second_ = value;
        }

        bool operator == (const Reopened& other_) const;
        bool operator != (const Reopened& other_) const;

        void swap(Reopened& other_) noexcept ;

      private:

        int32_t m_first_;
        int32_t m_second_;

    };

    inline void swap(Reopened& a, Reopened& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Reopened& sample);

} // namespace AutoIds  

class NDDSUSERDllExport Sequential {
  public:

    Sequential();

    Sequential(int32_t first_,int32_t second_);

    int32_t& first() noexcept {
        return m_first_;
    }

    const int32_t& first() const noexcept {
        return m_first_;
    }

    void first(int32_t value) {

        m_first_ = value;
    }

    int32_t& second() noexcept {
        return m_second_;
    }

    const int32_t& second() const noexcept {
        return m_second_;
    }

    void second(int32_t value) {

        m_second_ = value;
    }

    bool operator == (const Sequential& other_) const;
    bool operator != (const Sequential& other_) const;

    void swap(Sequential& other_) noexcept ;

  private:

    int32_t m_first_;
    int32_t m_second_;

};

inline void swap(Sequential& a, Sequential& b)  noexcept 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sequential& sample);

class NDDSUSERDllExport UsesInclude {
  public:

    UsesInclude();

    explicit UsesInclude(const ::Included& included_);

    ::Included& included() noexcept {
        return m_included_;
    }

    const ::Included& included() const noexcept {
        return m_included_;
    }

    void included(const ::Included& value) {

        m_included_ = value;
    }

    void included(::Included&& value) {
        m_included_ = std::move(value);
    }
    bool operator == (const UsesInclude& other_) const;
    bool operator != (const UsesInclude& other_) const;

    void swap(UsesInclude& other_) noexcept ;

  private:

    ::Included m_included_;

};

inline void swap(UsesInclude& a, UsesInclude& b)  noexcept 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const UsesInclude& sample);

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
        struct topic_type_name< ::AutoIds::Hashed > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIds::Hashed";
            }
        };

        template<>
        struct is_topic_type< ::AutoIds::Hashed > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::AutoIds::Hashed > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::AutoIds::Hashed& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::AutoIds::Hashed& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::AutoIds::Hashed& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::AutoIds::Hashed& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::AutoIds::Reopened > {
            NDDSUSERDllExport static std::string value() {
                return "AutoIds::Reopened";
            }
        };

        template<>
        struct is_topic_type< ::AutoIds::Reopened > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::AutoIds::Reopened > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::AutoIds::Reopened& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::AutoIds::Reopened& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::AutoIds::Reopened& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::AutoIds::Reopened& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::Sequential > {
            NDDSUSERDllExport static std::string value() {
                return "Sequential";
            }
        };

        template<>
        struct is_topic_type< ::Sequential > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::Sequential > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::Sequential& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::Sequential& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::Sequential& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::Sequential& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::UsesInclude > {
            NDDSUSERDllExport static std::string value() {
                return "UsesInclude";
            }
        };

        template<>
        struct is_topic_type< ::UsesInclude > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::UsesInclude > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::UsesInclude& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::UsesInclude& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::UsesInclude& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::UsesInclude& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::AutoIds::Hashed > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::AutoIds::Hashed > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::AutoIds::Reopened > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::AutoIds::Reopened > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::Sequential > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::Sequential > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::UsesInclude > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::UsesInclude > {
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

#endif // _14_autoid_scope_2088995224_hpp

