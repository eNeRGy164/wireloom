

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-flat-data-binding.idl
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
#include "10-flat-data-bindingPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "10-flat-data-binding.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusFlatDataBinding {

    // ---- Sample:

    int32_t SampleConstOffset::id() const
    {
        static const ::rti::flat::offset_t offsets[] = {0, 0, 0, 0};
        return deserialize< int32_t >(offsets);
    }

    int32_t SampleConstOffset::value() const
    {
        static const ::rti::flat::offset_t offsets[] = {4, 4, 4, 4};
        return deserialize< int32_t >(offsets);
    }

    int32_t SampleOffset::id() const
    {
        static const ::rti::flat::offset_t offsets[] = {0, 0, 0, 0};
        return deserialize< int32_t >(offsets);
    }

    bool SampleOffset::id(int32_t value)
    {
        static const ::rti::flat::offset_t offsets[] = {0, 0, 0, 0};
        return serialize(offsets, value);

    }

    int32_t SampleOffset::value() const
    {
        static const ::rti::flat::offset_t offsets[] = {4, 4, 4, 4};
        return deserialize< int32_t >(offsets);
    }

    bool SampleOffset::value(int32_t value)
    {
        static const ::rti::flat::offset_t offsets[] = {4, 4, 4, 4};
        return serialize(offsets, value);

    }

    // ---- SamplePlainHelper:

    SamplePlainHelper::SamplePlainHelper() :
        id (0) ,
        value (0)  {

    }

    SamplePlainHelper::SamplePlainHelper (int32_t id_,int32_t value_) {
        id = id_;
        value = value_;
    }

    bool operator == (const SamplePlainHelper& a, const SamplePlainHelper& b) {

        if (a.id != b.id) {
            return false;
        }
        if (a.value != b.value) {
            return false;
        }

        return true;
    }

    bool operator != (const SamplePlainHelper& a, const SamplePlainHelper& b) {
        return !operator ==(a, b);
    }

    void swap(SamplePlainHelper& a, SamplePlainHelper& b) noexcept
    {
        using std::swap;

        swap(a.id, b.id);
        swap(a.value, b.value);

    }  
    std::ostream& operator << (std::ostream& o,const SamplePlainHelper& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id<<", ";
        o << "value: " << sample.value;
        o <<"]";
        return o;
    }

} // namespace CorpusFlatDataBinding  

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
        struct native_type_code< ::CorpusFlatDataBinding::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sample_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                    }, 
                    {
                        (char *)"value",/* Member name */
                        {
                            1,/* Representation ID */
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

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT| DDS_TK_FINAL_EXTENSIBILITY| DDS_TK_FLAT_DATA_LANGUAGE_BINDING, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusFlatDataBinding::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Sample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Sample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Sample_g_tc;
                }

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 4;

                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Sample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) 0;
                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[1] =
                (RTIXCdrUnsignedLong) 0;
                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[2] =
                (RTIXCdrUnsignedLong) 0;
                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[3] =
                (RTIXCdrUnsignedLong) 0;

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) 4;
                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[1] =
                (RTIXCdrUnsignedLong) 4;
                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[2] =
                (RTIXCdrUnsignedLong) 4;
                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[3] =
                (RTIXCdrUnsignedLong) 4;

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                Sample_g_sampleAccessInfo.typeSize[0] = (RTIXCdrUnsignedLong)
                ::CorpusFlatDataBinding::SampleOffset::serialized_size(0);
                Sample_g_sampleAccessInfo.typeSize[1] = (RTIXCdrUnsignedLong)
                ::CorpusFlatDataBinding::SampleOffset::serialized_size(1);
                Sample_g_sampleAccessInfo.typeSize[2] = (RTIXCdrUnsignedLong)
                ::CorpusFlatDataBinding::SampleOffset::serialized_size(2);
                Sample_g_sampleAccessInfo.typeSize[3] = (RTIXCdrUnsignedLong)
                ::CorpusFlatDataBinding::SampleOffset::serialized_size(3);

                Sample_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_FLAT_DATA_CPP_03_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Sample_g_typePlugin =
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

                return &Sample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusFlatDataBinding::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusFlatDataBinding::Sample >::get())));
        }

        template<>
        struct native_type_code< ::CorpusFlatDataBinding::SamplePlainHelper > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member SamplePlainHelper_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                    }, 
                    {
                        (char *)"value",/* Member name */
                        {
                            1,/* Representation ID */
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

                static DDS_TypeCode SamplePlainHelper_g_tc =
                {{
                        DDS_TK_STRUCT| DDS_TK_FINAL_EXTENSIBILITY, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusFlatDataBinding::SamplePlainHelper", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        SamplePlainHelper_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for SamplePlainHelper*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &SamplePlainHelper_g_tc;
                }

                SamplePlainHelper_g_tc._data._annotations._allowedDataRepresentationMask = 4;

                SamplePlainHelper_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                SamplePlainHelper_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                SamplePlainHelper_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                SamplePlainHelper_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                SamplePlainHelper_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                SamplePlainHelper_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                SamplePlainHelper_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                SamplePlainHelper_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                SamplePlainHelper_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                SamplePlainHelper_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                SamplePlainHelper_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                SamplePlainHelper_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                SamplePlainHelper_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                SamplePlainHelper_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                SamplePlainHelper_g_tc._data._sampleAccessInfo = sample_access_info();
                SamplePlainHelper_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &SamplePlainHelper_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusFlatDataBinding::SamplePlainHelper *sample;

                static RTIXCdrMemberAccessInfo SamplePlainHelper_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo SamplePlainHelper_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &SamplePlainHelper_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusFlatDataBinding::SamplePlainHelper);
                if (sample == NULL) {
                    return NULL;
                }

                SamplePlainHelper_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->id - (char *)sample);

                SamplePlainHelper_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                SamplePlainHelper_g_sampleAccessInfo.memberAccessInfos =
                SamplePlainHelper_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusFlatDataBinding::SamplePlainHelper);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        SamplePlainHelper_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        SamplePlainHelper_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                SamplePlainHelper_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                SamplePlainHelper_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusFlatDataBinding::SamplePlainHelper >;

                SamplePlainHelper_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &SamplePlainHelper_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin SamplePlainHelper_g_typePlugin =
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

                return &SamplePlainHelper_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusFlatDataBinding::SamplePlainHelper >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusFlatDataBinding::SamplePlainHelper >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusFlatDataBinding::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusFlatDataBinding::SamplePlugin_new,
                ::CorpusFlatDataBinding::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusFlatDataBinding::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusFlatDataBinding::Sample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusFlatDataBinding::Sample >::from_cdr_buffer(::CorpusFlatDataBinding::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusFlatDataBinding::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusFlatDataBinding::Sample >::reset_sample(::CorpusFlatDataBinding::Sample& sample) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusFlatDataBinding::Sample >::allocate_sample(::CorpusFlatDataBinding::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusFlatDataBinding::SamplePlainHelper >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusFlatDataBinding::SamplePlainHelperPlugin_new,
                ::CorpusFlatDataBinding::SamplePlainHelperPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusFlatDataBinding::SamplePlainHelper >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusFlatDataBinding::SamplePlainHelper& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SamplePlainHelperPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SamplePlainHelperPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusFlatDataBinding::SamplePlainHelper >::from_cdr_buffer(::CorpusFlatDataBinding::SamplePlainHelper& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlainHelperPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusFlatDataBinding::SamplePlainHelper from cdr buffer");
        }

        void topic_type_support< ::CorpusFlatDataBinding::SamplePlainHelper >::reset_sample(::CorpusFlatDataBinding::SamplePlainHelper& sample) 
        {
            sample.id = 0;
            sample.value = 0;
        }

        void topic_type_support< ::CorpusFlatDataBinding::SamplePlainHelper >::allocate_sample(::CorpusFlatDataBinding::SamplePlainHelper& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

namespace rti { 
    namespace xcdr {
        RTIXCdrInterpreterPrograms * type_programs< ::CorpusFlatDataBinding::Sample >::get() 
        {
            return ::rti::xcdr::ProgramsSingleton<
            ::CorpusFlatDataBinding::Sample,
            RTI_XCDR_INITIALIZE_SAMPLE_PROGRAM,
            true, true, true
            >::get_instance();
        }
    } 
}

#endif // NDDS_STANDALONE_TYPE
