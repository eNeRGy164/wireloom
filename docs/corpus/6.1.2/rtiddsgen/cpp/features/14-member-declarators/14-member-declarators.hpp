

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-member-declarators.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _14_member_declarators_544658739_hpp
#define _14_member_declarators_544658739_hpp

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

class NDDSUSERDllExport Declarators {
  public:
    Declarators();

    Declarators(int32_t first,int32_t second,const ::dds::core::array< int32_t, 2L>& numbers,const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& matrix);

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Declarators (Declarators&&) = default;
    Declarators& operator=(Declarators&&) = default;
    Declarators& operator=(const Declarators&) = default;
    Declarators(const Declarators&) = default;
    #else
    Declarators(Declarators&& other_) OMG_NOEXCEPT;  
    Declarators& operator=(Declarators&&  other_) OMG_NOEXCEPT;
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

    ::dds::core::array< int32_t, 2L>& numbers() OMG_NOEXCEPT {
        return m_numbers_;
    }

    const ::dds::core::array< int32_t, 2L>& numbers() const OMG_NOEXCEPT {
        return m_numbers_;
    }

    void numbers(const ::dds::core::array< int32_t, 2L>& value) {
        m_numbers_ = value;
    }

    void numbers(::dds::core::array< int32_t, 2L>&& value) {
        m_numbers_ = std::move(value);
    }
    ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& matrix() OMG_NOEXCEPT {
        return m_matrix_;
    }

    const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& matrix() const OMG_NOEXCEPT {
        return m_matrix_;
    }

    void matrix(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& value) {
        m_matrix_ = value;
    }

    void matrix(::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>&& value) {
        m_matrix_ = std::move(value);
    }

    bool operator == (const Declarators& other_) const;
    bool operator != (const Declarators& other_) const;

    void swap(Declarators& other_) OMG_NOEXCEPT ;

  private:

    int32_t m_first_;
    int32_t m_second_;
    ::dds::core::array< int32_t, 2L> m_numbers_;
    ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L> m_matrix_;

};

inline void swap(Declarators& a, Declarators& b)  OMG_NOEXCEPT 
{
    a.swap(b);
}

NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Declarators& sample);

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< Declarators > {
            NDDSUSERDllExport static std::string value() {
                return "Declarators";
            }
        };

        template<>
        struct is_topic_type< Declarators > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< Declarators > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const Declarators& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(Declarators& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(Declarators& sample);

            NDDSUSERDllExport 
            static void allocate_sample(Declarators& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {
        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< Declarators > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< Declarators > {
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

#endif // _14_member_declarators_544658739_hpp

