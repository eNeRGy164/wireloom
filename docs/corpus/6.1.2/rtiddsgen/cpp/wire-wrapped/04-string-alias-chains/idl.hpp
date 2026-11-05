

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef idl_1662736386_hpp
#define idl_1662736386_hpp

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

#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, stop exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif
#include "features/04-string-alias-chains.hpp"
#if (defined(RTI_WIN32) || defined (RTI_WINCE) || defined(RTI_INTIME)) && defined(NDDS_USER_DLL_EXPORT)
/* If the code is building on Windows, start exporting symbols.
*/
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif
namespace CorpusStringAliasChains {

    class NDDSUSERDllExport StringAliasChainsSample {
      public:
        StringAliasChainsSample();

        StringAliasChainsSample(const std::string& narrow,const std::string& narrowChain,const std::string& boundedNarrow,const std::string& boundedNarrowChain,const std::string& boundedNarrowChain2,const std::wstring& wide,const std::wstring& wideChain,const std::wstring& boundedWide,const std::wstring& boundedWideChain,const std::wstring& boundedWideChain2);

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifndef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        StringAliasChainsSample (StringAliasChainsSample&&) = default;
        StringAliasChainsSample& operator=(StringAliasChainsSample&&) = default;
        StringAliasChainsSample& operator=(const StringAliasChainsSample&) = default;
        StringAliasChainsSample(const StringAliasChainsSample&) = default;
        #else
        StringAliasChainsSample(StringAliasChainsSample&& other_) OMG_NOEXCEPT;  
        StringAliasChainsSample& operator=(StringAliasChainsSample&&  other_) OMG_NOEXCEPT;
        #endif
        #endif 

        std::string& narrow() OMG_NOEXCEPT {
            return m_narrow_;
        }

        const std::string& narrow() const OMG_NOEXCEPT {
            return m_narrow_;
        }

        void narrow(const std::string& value) {
            m_narrow_ = value;
        }

        void narrow(std::string&& value) {
            m_narrow_ = std::move(value);
        }
        std::string& narrowChain() OMG_NOEXCEPT {
            return m_narrowChain_;
        }

        const std::string& narrowChain() const OMG_NOEXCEPT {
            return m_narrowChain_;
        }

        void narrowChain(const std::string& value) {
            m_narrowChain_ = value;
        }

        void narrowChain(std::string&& value) {
            m_narrowChain_ = std::move(value);
        }
        std::string& boundedNarrow() OMG_NOEXCEPT {
            return m_boundedNarrow_;
        }

        const std::string& boundedNarrow() const OMG_NOEXCEPT {
            return m_boundedNarrow_;
        }

        void boundedNarrow(const std::string& value) {
            m_boundedNarrow_ = value;
        }

        void boundedNarrow(std::string&& value) {
            m_boundedNarrow_ = std::move(value);
        }
        std::string& boundedNarrowChain() OMG_NOEXCEPT {
            return m_boundedNarrowChain_;
        }

        const std::string& boundedNarrowChain() const OMG_NOEXCEPT {
            return m_boundedNarrowChain_;
        }

        void boundedNarrowChain(const std::string& value) {
            m_boundedNarrowChain_ = value;
        }

        void boundedNarrowChain(std::string&& value) {
            m_boundedNarrowChain_ = std::move(value);
        }
        std::string& boundedNarrowChain2() OMG_NOEXCEPT {
            return m_boundedNarrowChain2_;
        }

        const std::string& boundedNarrowChain2() const OMG_NOEXCEPT {
            return m_boundedNarrowChain2_;
        }

        void boundedNarrowChain2(const std::string& value) {
            m_boundedNarrowChain2_ = value;
        }

        void boundedNarrowChain2(std::string&& value) {
            m_boundedNarrowChain2_ = std::move(value);
        }
        std::wstring& wide() OMG_NOEXCEPT {
            return m_wide_;
        }

        const std::wstring& wide() const OMG_NOEXCEPT {
            return m_wide_;
        }

        void wide(const std::wstring& value) {
            m_wide_ = value;
        }

        void wide(std::wstring&& value) {
            m_wide_ = std::move(value);
        }
        std::wstring& wideChain() OMG_NOEXCEPT {
            return m_wideChain_;
        }

        const std::wstring& wideChain() const OMG_NOEXCEPT {
            return m_wideChain_;
        }

        void wideChain(const std::wstring& value) {
            m_wideChain_ = value;
        }

        void wideChain(std::wstring&& value) {
            m_wideChain_ = std::move(value);
        }
        std::wstring& boundedWide() OMG_NOEXCEPT {
            return m_boundedWide_;
        }

        const std::wstring& boundedWide() const OMG_NOEXCEPT {
            return m_boundedWide_;
        }

        void boundedWide(const std::wstring& value) {
            m_boundedWide_ = value;
        }

        void boundedWide(std::wstring&& value) {
            m_boundedWide_ = std::move(value);
        }
        std::wstring& boundedWideChain() OMG_NOEXCEPT {
            return m_boundedWideChain_;
        }

        const std::wstring& boundedWideChain() const OMG_NOEXCEPT {
            return m_boundedWideChain_;
        }

        void boundedWideChain(const std::wstring& value) {
            m_boundedWideChain_ = value;
        }

        void boundedWideChain(std::wstring&& value) {
            m_boundedWideChain_ = std::move(value);
        }
        std::wstring& boundedWideChain2() OMG_NOEXCEPT {
            return m_boundedWideChain2_;
        }

        const std::wstring& boundedWideChain2() const OMG_NOEXCEPT {
            return m_boundedWideChain2_;
        }

        void boundedWideChain2(const std::wstring& value) {
            m_boundedWideChain2_ = value;
        }

        void boundedWideChain2(std::wstring&& value) {
            m_boundedWideChain2_ = std::move(value);
        }

        bool operator == (const StringAliasChainsSample& other_) const;
        bool operator != (const StringAliasChainsSample& other_) const;

        void swap(StringAliasChainsSample& other_) OMG_NOEXCEPT ;

      private:

        std::string m_narrow_;
        std::string m_narrowChain_;
        std::string m_boundedNarrow_;
        std::string m_boundedNarrowChain_;
        std::string m_boundedNarrowChain2_;
        std::wstring m_wide_;
        std::wstring m_wideChain_;
        std::wstring m_boundedWide_;
        std::wstring m_boundedWideChain_;
        std::wstring m_boundedWideChain2_;

    };

    inline void swap(StringAliasChainsSample& a, StringAliasChainsSample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const StringAliasChainsSample& sample);

} // namespace CorpusStringAliasChains  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusStringAliasChains::StringAliasChainsSample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusStringAliasChains::StringAliasChainsSample";
            }
        };

        template<>
        struct is_topic_type< CorpusStringAliasChains::StringAliasChainsSample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusStringAliasChains::StringAliasChainsSample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusStringAliasChains::StringAliasChainsSample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusStringAliasChains::StringAliasChainsSample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusStringAliasChains::StringAliasChainsSample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusStringAliasChains::StringAliasChainsSample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusStringAliasChains::StringAliasChainsSample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusStringAliasChains::StringAliasChainsSample > {
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

#endif // idl_1662736386_hpp

