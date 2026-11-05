

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-primitives.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _01_primitives_1101677217_hpp
#define _01_primitives_1101677217_hpp

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

namespace CorpusPrimitives {

    class NDDSUSERDllExport Sample {
      public:
        Sample();

        Sample(int16_t signed16,int32_t signed32,int64_t signed64,uint16_t unsigned16,uint32_t unsigned32,uint64_t unsigned64,int8_t fixedInt8,int64_t fixedInt64,uint8_t fixedUint8,uint64_t fixedUint64,uint8_t octetValue,bool boolValue,char charValue,wchar_t wideCharValue,float floatValue,double doubleValue,const ::rti::core::LongDouble& longDoubleValue);

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

        int16_t& signed16() OMG_NOEXCEPT {
            return m_signed16_;
        }

        const int16_t& signed16() const OMG_NOEXCEPT {
            return m_signed16_;
        }

        void signed16(int16_t value) {
            m_signed16_ = value;
        }

        int32_t& signed32() OMG_NOEXCEPT {
            return m_signed32_;
        }

        const int32_t& signed32() const OMG_NOEXCEPT {
            return m_signed32_;
        }

        void signed32(int32_t value) {
            m_signed32_ = value;
        }

        int64_t& signed64() OMG_NOEXCEPT {
            return m_signed64_;
        }

        const int64_t& signed64() const OMG_NOEXCEPT {
            return m_signed64_;
        }

        void signed64(int64_t value) {
            m_signed64_ = value;
        }

        uint16_t& unsigned16() OMG_NOEXCEPT {
            return m_unsigned16_;
        }

        const uint16_t& unsigned16() const OMG_NOEXCEPT {
            return m_unsigned16_;
        }

        void unsigned16(uint16_t value) {
            m_unsigned16_ = value;
        }

        uint32_t& unsigned32() OMG_NOEXCEPT {
            return m_unsigned32_;
        }

        const uint32_t& unsigned32() const OMG_NOEXCEPT {
            return m_unsigned32_;
        }

        void unsigned32(uint32_t value) {
            m_unsigned32_ = value;
        }

        uint64_t& unsigned64() OMG_NOEXCEPT {
            return m_unsigned64_;
        }

        const uint64_t& unsigned64() const OMG_NOEXCEPT {
            return m_unsigned64_;
        }

        void unsigned64(uint64_t value) {
            m_unsigned64_ = value;
        }

        int8_t& fixedInt8() OMG_NOEXCEPT {
            return m_fixedInt8_;
        }

        const int8_t& fixedInt8() const OMG_NOEXCEPT {
            return m_fixedInt8_;
        }

        void fixedInt8(int8_t value) {
            m_fixedInt8_ = value;
        }

        int64_t& fixedInt64() OMG_NOEXCEPT {
            return m_fixedInt64_;
        }

        const int64_t& fixedInt64() const OMG_NOEXCEPT {
            return m_fixedInt64_;
        }

        void fixedInt64(int64_t value) {
            m_fixedInt64_ = value;
        }

        uint8_t& fixedUint8() OMG_NOEXCEPT {
            return m_fixedUint8_;
        }

        const uint8_t& fixedUint8() const OMG_NOEXCEPT {
            return m_fixedUint8_;
        }

        void fixedUint8(uint8_t value) {
            m_fixedUint8_ = value;
        }

        uint64_t& fixedUint64() OMG_NOEXCEPT {
            return m_fixedUint64_;
        }

        const uint64_t& fixedUint64() const OMG_NOEXCEPT {
            return m_fixedUint64_;
        }

        void fixedUint64(uint64_t value) {
            m_fixedUint64_ = value;
        }

        uint8_t& octetValue() OMG_NOEXCEPT {
            return m_octetValue_;
        }

        const uint8_t& octetValue() const OMG_NOEXCEPT {
            return m_octetValue_;
        }

        void octetValue(uint8_t value) {
            m_octetValue_ = value;
        }

        bool& boolValue() OMG_NOEXCEPT {
            return m_boolValue_;
        }

        const bool& boolValue() const OMG_NOEXCEPT {
            return m_boolValue_;
        }

        void boolValue(bool value) {
            m_boolValue_ = value;
        }

        char& charValue() OMG_NOEXCEPT {
            return m_charValue_;
        }

        const char& charValue() const OMG_NOEXCEPT {
            return m_charValue_;
        }

        void charValue(char value) {
            m_charValue_ = value;
        }

        wchar_t& wideCharValue() OMG_NOEXCEPT {
            return m_wideCharValue_;
        }

        const wchar_t& wideCharValue() const OMG_NOEXCEPT {
            return m_wideCharValue_;
        }

        void wideCharValue(wchar_t value) {
            m_wideCharValue_ = value;
        }

        float& floatValue() OMG_NOEXCEPT {
            return m_floatValue_;
        }

        const float& floatValue() const OMG_NOEXCEPT {
            return m_floatValue_;
        }

        void floatValue(float value) {
            m_floatValue_ = value;
        }

        double& doubleValue() OMG_NOEXCEPT {
            return m_doubleValue_;
        }

        const double& doubleValue() const OMG_NOEXCEPT {
            return m_doubleValue_;
        }

        void doubleValue(double value) {
            m_doubleValue_ = value;
        }

        ::rti::core::LongDouble& longDoubleValue() OMG_NOEXCEPT {
            return m_longDoubleValue_;
        }

        const ::rti::core::LongDouble& longDoubleValue() const OMG_NOEXCEPT {
            return m_longDoubleValue_;
        }

        void longDoubleValue(const ::rti::core::LongDouble& value) {
            m_longDoubleValue_ = value;
        }

        void longDoubleValue(::rti::core::LongDouble&& value) {
            m_longDoubleValue_ = std::move(value);
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) OMG_NOEXCEPT ;

      private:

        int16_t m_signed16_;
        int32_t m_signed32_;
        int64_t m_signed64_;
        uint16_t m_unsigned16_;
        uint32_t m_unsigned32_;
        uint64_t m_unsigned64_;
        int8_t m_fixedInt8_;
        int64_t m_fixedInt64_;
        uint8_t m_fixedUint8_;
        uint64_t m_fixedUint64_;
        uint8_t m_octetValue_;
        bool m_boolValue_;
        char m_charValue_;
        wchar_t m_wideCharValue_;
        float m_floatValue_;
        double m_doubleValue_;
        ::rti::core::LongDouble m_longDoubleValue_;

    };

    inline void swap(Sample& a, Sample& b)  OMG_NOEXCEPT 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusPrimitives  

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< CorpusPrimitives::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusPrimitives::Sample";
            }
        };

        template<>
        struct is_topic_type< CorpusPrimitives::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< CorpusPrimitives::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const CorpusPrimitives::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(CorpusPrimitives::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(CorpusPrimitives::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(CorpusPrimitives::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };

    }
}

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct dynamic_type< CorpusPrimitives::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };
        #endif

        template <>
        struct extensibility< CorpusPrimitives::Sample > {
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

#endif // _01_primitives_1101677217_hpp

