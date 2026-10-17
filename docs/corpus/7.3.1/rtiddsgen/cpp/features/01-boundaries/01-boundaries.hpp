

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-boundaries.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _01_boundaries_1164299082_hpp
#define _01_boundaries_1164299082_hpp

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

namespace CorpusPrimitiveBoundaries {

    static const int16_t shortMin = -32768;

    static const int16_t shortMax = 32767;

    static const int32_t longMin = -2147483648L;

    static const int32_t longMax = 2147483647L;

    static const int64_t longLongMin = -9223372036854775807LL-1LL;

    static const int64_t longLongMax = 9223372036854775807LL;

    static const int8_t int8Min = -128;

    static const int8_t int8Max = 127;

    static const int64_t int64Min = -9223372036854775807LL-1LL;

    static const int64_t int64Max = 9223372036854775807LL;

    class NDDSUSERDllExport Sample {
      public:

        Sample();

        Sample(int16_t shortValue_,int32_t longValue_,int64_t longLongValue_,int8_t int8Value_,int64_t int64Value_,float floatValue_,double doubleValue_);

        int16_t& shortValue() noexcept {
            return m_shortValue_;
        }

        const int16_t& shortValue() const noexcept {
            return m_shortValue_;
        }

        void shortValue(int16_t value) {

            m_shortValue_ = value;
        }

        int32_t& longValue() noexcept {
            return m_longValue_;
        }

        const int32_t& longValue() const noexcept {
            return m_longValue_;
        }

        void longValue(int32_t value) {

            m_longValue_ = value;
        }

        int64_t& longLongValue() noexcept {
            return m_longLongValue_;
        }

        const int64_t& longLongValue() const noexcept {
            return m_longLongValue_;
        }

        void longLongValue(int64_t value) {

            m_longLongValue_ = value;
        }

        int8_t& int8Value() noexcept {
            return m_int8Value_;
        }

        const int8_t& int8Value() const noexcept {
            return m_int8Value_;
        }

        void int8Value(int8_t value) {

            m_int8Value_ = value;
        }

        int64_t& int64Value() noexcept {
            return m_int64Value_;
        }

        const int64_t& int64Value() const noexcept {
            return m_int64Value_;
        }

        void int64Value(int64_t value) {

            m_int64Value_ = value;
        }

        float& floatValue() noexcept {
            return m_floatValue_;
        }

        const float& floatValue() const noexcept {
            return m_floatValue_;
        }

        void floatValue(float value) {

            m_floatValue_ = value;
        }

        double& doubleValue() noexcept {
            return m_doubleValue_;
        }

        const double& doubleValue() const noexcept {
            return m_doubleValue_;
        }

        void doubleValue(double value) {

            m_doubleValue_ = value;
        }

        bool operator == (const Sample& other_) const;
        bool operator != (const Sample& other_) const;

        void swap(Sample& other_) noexcept ;

      private:

        int16_t m_shortValue_;
        int32_t m_longValue_;
        int64_t m_longLongValue_;
        int8_t m_int8Value_;
        int64_t m_int64Value_;
        float m_floatValue_;
        double m_doubleValue_;

    };

    inline void swap(Sample& a, Sample& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusPrimitiveBoundaries  

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
        struct topic_type_name< ::CorpusPrimitiveBoundaries::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusPrimitiveBoundaries::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusPrimitiveBoundaries::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusPrimitiveBoundaries::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusPrimitiveBoundaries::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusPrimitiveBoundaries::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusPrimitiveBoundaries::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusPrimitiveBoundaries::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusPrimitiveBoundaries::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusPrimitiveBoundaries::Sample > {
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

#endif // _01_boundaries_1164299082_hpp

