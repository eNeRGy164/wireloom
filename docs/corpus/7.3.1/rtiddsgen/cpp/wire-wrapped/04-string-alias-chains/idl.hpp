

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

        StringAliasChainsSample(const std::string& narrow_,const std::string& narrowChain_,const std::string& boundedNarrow_,const std::string& boundedNarrowChain_,const std::string& boundedNarrowChain2_,const std::wstring& wide_,const std::wstring& wideChain_,const std::wstring& boundedWide_,const std::wstring& boundedWideChain_,const std::wstring& boundedWideChain2_);

        std::string& narrow() noexcept {
            return m_narrow_;
        }

        const std::string& narrow() const noexcept {
            return m_narrow_;
        }

        void narrow(const std::string& value) {

            m_narrow_ = value;
        }

        void narrow(std::string&& value) {
            m_narrow_ = std::move(value);
        }
        std::string& narrowChain() noexcept {
            return m_narrowChain_;
        }

        const std::string& narrowChain() const noexcept {
            return m_narrowChain_;
        }

        void narrowChain(const std::string& value) {

            m_narrowChain_ = value;
        }

        void narrowChain(std::string&& value) {
            m_narrowChain_ = std::move(value);
        }
        std::string& boundedNarrow() noexcept {
            return m_boundedNarrow_;
        }

        const std::string& boundedNarrow() const noexcept {
            return m_boundedNarrow_;
        }

        void boundedNarrow(const std::string& value) {

            m_boundedNarrow_ = value;
        }

        void boundedNarrow(std::string&& value) {
            m_boundedNarrow_ = std::move(value);
        }
        std::string& boundedNarrowChain() noexcept {
            return m_boundedNarrowChain_;
        }

        const std::string& boundedNarrowChain() const noexcept {
            return m_boundedNarrowChain_;
        }

        void boundedNarrowChain(const std::string& value) {

            m_boundedNarrowChain_ = value;
        }

        void boundedNarrowChain(std::string&& value) {
            m_boundedNarrowChain_ = std::move(value);
        }
        std::string& boundedNarrowChain2() noexcept {
            return m_boundedNarrowChain2_;
        }

        const std::string& boundedNarrowChain2() const noexcept {
            return m_boundedNarrowChain2_;
        }

        void boundedNarrowChain2(const std::string& value) {

            m_boundedNarrowChain2_ = value;
        }

        void boundedNarrowChain2(std::string&& value) {
            m_boundedNarrowChain2_ = std::move(value);
        }
        std::wstring& wide() noexcept {
            return m_wide_;
        }

        const std::wstring& wide() const noexcept {
            return m_wide_;
        }

        void wide(const std::wstring& value) {

            m_wide_ = value;
        }

        void wide(std::wstring&& value) {
            m_wide_ = std::move(value);
        }
        std::wstring& wideChain() noexcept {
            return m_wideChain_;
        }

        const std::wstring& wideChain() const noexcept {
            return m_wideChain_;
        }

        void wideChain(const std::wstring& value) {

            m_wideChain_ = value;
        }

        void wideChain(std::wstring&& value) {
            m_wideChain_ = std::move(value);
        }
        std::wstring& boundedWide() noexcept {
            return m_boundedWide_;
        }

        const std::wstring& boundedWide() const noexcept {
            return m_boundedWide_;
        }

        void boundedWide(const std::wstring& value) {

            m_boundedWide_ = value;
        }

        void boundedWide(std::wstring&& value) {
            m_boundedWide_ = std::move(value);
        }
        std::wstring& boundedWideChain() noexcept {
            return m_boundedWideChain_;
        }

        const std::wstring& boundedWideChain() const noexcept {
            return m_boundedWideChain_;
        }

        void boundedWideChain(const std::wstring& value) {

            m_boundedWideChain_ = value;
        }

        void boundedWideChain(std::wstring&& value) {
            m_boundedWideChain_ = std::move(value);
        }
        std::wstring& boundedWideChain2() noexcept {
            return m_boundedWideChain2_;
        }

        const std::wstring& boundedWideChain2() const noexcept {
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

        void swap(StringAliasChainsSample& other_) noexcept ;

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

    inline void swap(StringAliasChainsSample& a, StringAliasChainsSample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const StringAliasChainsSample& sample);

} // namespace CorpusStringAliasChains  

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
        struct topic_type_name< ::CorpusStringAliasChains::StringAliasChainsSample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusStringAliasChains::StringAliasChainsSample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusStringAliasChains::StringAliasChainsSample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusStringAliasChains::StringAliasChainsSample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusStringAliasChains::StringAliasChainsSample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusStringAliasChains::StringAliasChainsSample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusStringAliasChains::StringAliasChainsSample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusStringAliasChains::StringAliasChainsSample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusStringAliasChains::StringAliasChainsSample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusStringAliasChains::StringAliasChainsSample > {
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

#endif // idl_1662736386_hpp

