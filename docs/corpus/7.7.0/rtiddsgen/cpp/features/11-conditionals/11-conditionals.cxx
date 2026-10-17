

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-conditionals.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "11-conditionalsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "11-conditionals.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusConditionals {

    // ---- Enabled:

    Enabled::Enabled() :
        value (0)  {

    }

    Enabled::Enabled (int32_t value_) {
        value = value_;
    }

    bool operator == (const Enabled& a, const Enabled& b) {

        if (a.value != b.value) {
            return false;
        }

        return true;
    }

    bool operator != (const Enabled& a, const Enabled& b) {
        return !operator ==(a, b);
    }

    void swap(Enabled& a, Enabled& b) noexcept
    {
        using std::swap;

        swap(a.value, b.value);

    }  
    std::ostream& operator << (std::ostream& o,const Enabled& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value;
        o <<"]";
        return o;
    }

} // namespace CorpusConditionals  
namespace CorpusConditionals {

    // ---- UndefBranch:

    UndefBranch::UndefBranch() :
        value (0)  {

    }

    UndefBranch::UndefBranch (int32_t value_) {
        value = value_;
    }

    bool operator == (const UndefBranch& a, const UndefBranch& b) {

        if (a.value != b.value) {
            return false;
        }

        return true;
    }

    bool operator != (const UndefBranch& a, const UndefBranch& b) {
        return !operator ==(a, b);
    }

    void swap(UndefBranch& a, UndefBranch& b) noexcept
    {
        using std::swap;

        swap(a.value, b.value);

    }  
    std::ostream& operator << (std::ostream& o,const UndefBranch& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value;
        o <<"]";
        return o;
    }

} // namespace CorpusConditionals  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        template<>
        struct native_type_code< ::CorpusConditionals::Enabled > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Enabled_g_tc_members[1]=
                {

                    {
                        (char *)"value",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Enabled_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusConditionals::Enabled", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Enabled_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Enabled*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Enabled_g_tc;
                }

                Enabled_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Enabled_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Enabled_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Enabled_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Enabled_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Enabled_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Enabled_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Enabled_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Enabled_g_tc._data._sampleAccessInfo = sample_access_info();
                Enabled_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Enabled_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusConditionals::Enabled *sample;

                static RTIXCdrMemberAccessInfo Enabled_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Enabled_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Enabled_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusConditionals::Enabled);
                if (sample == NULL) {
                    return NULL;
                }

                Enabled_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                Enabled_g_sampleAccessInfo.memberAccessInfos =
                Enabled_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusConditionals::Enabled);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Enabled_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Enabled_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Enabled_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Enabled_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusConditionals::Enabled >;

                Enabled_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Enabled_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Enabled_g_typePlugin =
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &Enabled_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusConditionals::Enabled >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusConditionals::Enabled >::get())));
        }

        template<>
        struct native_type_code< ::CorpusConditionals::UndefBranch > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member UndefBranch_g_tc_members[1]=
                {

                    {
                        (char *)"value",/* Member name */
                        {
                            0,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode UndefBranch_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusConditionals::UndefBranch", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        UndefBranch_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for UndefBranch*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &UndefBranch_g_tc;
                }

                UndefBranch_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                UndefBranch_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                UndefBranch_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                UndefBranch_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                UndefBranch_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                UndefBranch_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                UndefBranch_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                UndefBranch_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                UndefBranch_g_tc._data._sampleAccessInfo = sample_access_info();
                UndefBranch_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &UndefBranch_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusConditionals::UndefBranch *sample;

                static RTIXCdrMemberAccessInfo UndefBranch_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo UndefBranch_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &UndefBranch_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusConditionals::UndefBranch);
                if (sample == NULL) {
                    return NULL;
                }

                UndefBranch_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                UndefBranch_g_sampleAccessInfo.memberAccessInfos =
                UndefBranch_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusConditionals::UndefBranch);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        UndefBranch_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        UndefBranch_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                UndefBranch_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                UndefBranch_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusConditionals::UndefBranch >;

                UndefBranch_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &UndefBranch_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin UndefBranch_g_typePlugin =
                {
                    NULL, /* serialize */
                    NULL, /* serialize_key */
                    NULL, /* deserialize_sample */
                    NULL, /* deserialize_key_sample */
                    NULL, /* skip */
                    NULL, /* get_serialized_sample_size */
                    NULL, /* get_serialized_sample_max_size_ex */
                    NULL, /* get_serialized_key_max_size_ex */
                    NULL, /* get_serialized_sample_min_size */
                    NULL, /* serialized_sample_to_key */
                    NULL,
                    NULL,
                    NULL,
                    NULL,
                    NULL
                };

                return &UndefBranch_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusConditionals::UndefBranch >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusConditionals::UndefBranch >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusConditionals::Enabled >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusConditionals::EnabledPlugin_new,
                ::CorpusConditionals::EnabledPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusConditionals::Enabled >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusConditionals::Enabled& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = EnabledPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = EnabledPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusConditionals::Enabled >::from_cdr_buffer(::CorpusConditionals::Enabled& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = EnabledPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusConditionals::Enabled from cdr buffer");
        }

        void topic_type_support< ::CorpusConditionals::Enabled >::reset_sample(::CorpusConditionals::Enabled& sample) 
        {
            sample.value = 0;
        }

        void topic_type_support< ::CorpusConditionals::Enabled >::allocate_sample(::CorpusConditionals::Enabled& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusConditionals::UndefBranch >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusConditionals::UndefBranchPlugin_new,
                ::CorpusConditionals::UndefBranchPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusConditionals::UndefBranch >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusConditionals::UndefBranch& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = UndefBranchPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = UndefBranchPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusConditionals::UndefBranch >::from_cdr_buffer(::CorpusConditionals::UndefBranch& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = UndefBranchPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusConditionals::UndefBranch from cdr buffer");
        }

        void topic_type_support< ::CorpusConditionals::UndefBranch >::reset_sample(::CorpusConditionals::UndefBranch& sample) 
        {
            sample.value = 0;
        }

        void topic_type_support< ::CorpusConditionals::UndefBranch >::allocate_sample(::CorpusConditionals::UndefBranch& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
