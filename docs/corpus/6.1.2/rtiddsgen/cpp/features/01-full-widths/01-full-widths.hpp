

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-full-widths.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _01_full_widths_1491609070_hpp
#define _01_full_widths_1491609070_hpp

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

namespace CorpusFullWidths {

    class NDDSUSERDllExport Sample {
      public:
        Sample();

        Sample(int8_t i8,int16_t i16,int32_t i32,int64_t i64,uint8_t u8,uint16_t u16,uint32_t u32,uint64_t u64);

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

        int8_t& i8() OMG_NOEXCEPT {
            return m_i8_;
        }

        const int8_t& i8() const OMG_NOEXCEPT {
            return m_i8_;
        }

        void i8(int8_t value) {
            m_i8_ = value;
        }

        int16_t& i16() OMG_NOEXCEPT {
            return m_i16_;
        }

        const int16_t& i16() const OMG_NOEXCEPT {
            return m_i16_;
        }

        void i16(int16_t value) {
            m_i16_ = value;
        }

        int32_t& i32() OMG_NOEXCEPT {
            return m_i32_;
        }

        const int32_t& i32() const OMG_NOEXCEPT {
            return m_i32_;
        }

        void i32(int32_t value) {
            m_i32_ = value;
        }

        int64_t& i64() OMG_NOEXCEPT {
            return m_i64_;
        }

        const int64_t& i64() const OMG_NOEXCEPT {
            return m_i64_;
        }

        void i64(int64_t value) {
            m_i64_ = value;
        }

        uint8_t& u8() OMG_NOEXCEPT {
            return m_u8_;
        }

        const uint8_t& u8() const OMG_NOEXCEPT {
            return m_u8_;
        }

        void u8(uint8_t value) {
            m_u8_ = value;
        }

        uint16_t& u16() OMG_NOEXCEPT {
            return m_u16_;
        }

        const uint16_t& u16() const OMG_NOEXCEPT {
            return m_u16_;
        }

        void u16(uint16_t value) {
            m_u16_ = value;
        }

        uint32_t& u32() OMG_NOEXCEPT {
            return m_u32_;
        }

        const uint32_t& u32() const OMG_NOEXCEPT {
            return m_u32_;
        }

        void u32(uint32_t value) {
            m_u32_ = value;
        }

        uint64_t& u64() OMG_NOEXCEPT {
            return m_u64_;
        }

        const uint64_t& u64() const OMG_NOEXCEPT {
            return m_u64_;
        }

        void u64(uint64_t value) {
            m_u64_ = value;
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        int8_t m_i8_;
        int16_t m_i16_;
        int32_t m_i32_;
        int64_t m_i64_;
        uint8_t m_u8_;
        uint16_t m_u16_;
        uint32_t m_u32_;
        uint64_t m_u64_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusFullWidths  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusFullWidths::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusFullWidths::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusFullWidths::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusFullWidths::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusFullWidths::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusFullWidths::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusFullWidths::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusFullWidths::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusFullWidths::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusFullWidths::Sample > {
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

#endif // _01_full_widths_1491609070_hpp

