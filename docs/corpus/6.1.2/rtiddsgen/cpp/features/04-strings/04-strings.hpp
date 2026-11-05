

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-strings.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _04_strings_849264045_hpp
#define _04_strings_849264045_hpp

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

namespace CorpusStrings {

    class NDDSUSERDllExport Sample {
      public:
        Sample();

        Sample(const std::string& unbounded,const std::string& bounded,const std::wstring& wideUnbounded,const std::wstring& wideBounded);

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

        std::string& unbounded() OMG_NOEXCEPT {
            return m_unbounded_;
        }

        const std::string& unbounded() const OMG_NOEXCEPT {
            return m_unbounded_;
        }

        void unbounded(const std::string& value) {
            m_unbounded_ = value;
        }

        void unbounded(std::string&& value) {
            m_unbounded_ = std::move(value);
        }
        std::string& bounded() OMG_NOEXCEPT {
            return m_bounded_;
        }

        const std::string& bounded() const OMG_NOEXCEPT {
            return m_bounded_;
        }

        void bounded(const std::string& value) {
            m_bounded_ = value;
        }

        void bounded(std::string&& value) {
            m_bounded_ = std::move(value);
        }
        std::wstring& wideUnbounded() OMG_NOEXCEPT {
            return m_wideUnbounded_;
        }

        const std::wstring& wideUnbounded() const OMG_NOEXCEPT {
            return m_wideUnbounded_;
        }

        void wideUnbounded(const std::wstring& value) {
            m_wideUnbounded_ = value;
        }

        void wideUnbounded(std::wstring&& value) {
            m_wideUnbounded_ = std::move(value);
        }
        std::wstring& wideBounded() OMG_NOEXCEPT {
            return m_wideBounded_;
        }

        const std::wstring& wideBounded() const OMG_NOEXCEPT {
            return m_wideBounded_;
        }

        void wideBounded(const std::wstring& value) {
            m_wideBounded_ = value;
        }

        void wideBounded(std::wstring&& value) {
            m_wideBounded_ = std::move(value);
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        std::string m_unbounded_;
        std::string m_bounded_;
        std::wstring m_wideUnbounded_;
        std::wstring m_wideBounded_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusStrings  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusStrings::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusStrings::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusStrings::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusStrings::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusStrings::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusStrings::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusStrings::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusStrings::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusStrings::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusStrings::Sample > {
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

#endif // _04_strings_849264045_hpp

