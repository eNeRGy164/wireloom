

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-multiple.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _02_multiple_223670400_hpp
#define _02_multiple_223670400_hpp

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

class NDDSUSERDllExport First {
  public:
    First();

    explicit First(int32_t value);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    First (First&&) = default;
    First& operator=(First&&) = default;
    First& operator=(const First&) = default;
    First(const First&) = default;
    #else
    First(First&& other_) OMG_NOEXCEPT;  
    First& operator=(First&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

    int32_t& value() OMG_NOEXCEPT {
        return m_value_;
    }

    const int32_t& value() const OMG_NOEXCEPT {
        return m_value_;
    }

    void value(int32_t value) {
        m_value_ = value;
    }

    bool operator == (const First& other_) const;
    bool operator != (const First& other_) const;

    void swap(First& other_) OMG_NOEXCEPT ;

  private:

    int32_t m_value_;

};

inline void swap(First& a, First& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const First& sample);

class NDDSUSERDllExport Second {
  public:
    Second();

    explicit Second(const std::string& name);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Second (Second&&) = default;
    Second& operator=(Second&&) = default;
    Second& operator=(const Second&) = default;
    Second(const Second&) = default;
    #else
    Second(Second&& other_) OMG_NOEXCEPT;  
    Second& operator=(Second&&  other_) OMG_NOEXCEPT;
    #endif
    #endif 

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

    bool operator == (const Second& other_) const;
    bool operator != (const Second& other_) const;

    void swap(Second& other_) OMG_NOEXCEPT ;

  private:

    std::string m_name_;

};

inline void swap(Second& a, Second& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Second& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< First > {
            NDDSUSERDllExport static std::string value() {
                return "First";
            }
        };

        template<>
        struct is_topic_type< First > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< First > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const First& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(First& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(First& sample);

            NDDSUSERDllExport 
            static void allocate_sample(First& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

        template<>
        struct topic_type_name< Second > {
            NDDSUSERDllExport static std::string value() {
                return "Second";
            }
        };

        template<>
        struct is_topic_type< Second > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< Second > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const Second& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(Second& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(Second& sample);

            NDDSUSERDllExport 
            static void allocate_sample(Second& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {
        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< First > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< First > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;                
        };

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< Second > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< Second > {
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

#endif // _02_multiple_223670400_hpp

