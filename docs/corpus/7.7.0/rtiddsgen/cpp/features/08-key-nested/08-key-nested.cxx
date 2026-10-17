

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-nested.idl
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
#include "08-key-nestedPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "08-key-nested.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusNestedKeys {

    // ---- Inner:

    Inner::Inner() :
        code (0) ,
        revision (0)  {

    }

    Inner::Inner (int32_t code_,int32_t revision_) {
        code = code_;
        revision = revision_;
    }

    bool operator == (const Inner& a, const Inner& b) {

        if (a.code != b.code) {
            return false;
        }
        if (a.revision != b.revision) {
            return false;
        }

        return true;
    }

    bool operator != (const Inner& a, const Inner& b) {
        return !operator ==(a, b);
    }

    void swap(Inner& a, Inner& b) noexcept
    {
        using std::swap;

        swap(a.code, b.code);
        swap(a.revision, b.revision);

    }  
    std::ostream& operator << (std::ostream& o,const Inner& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "code: " << sample.code<<", ";
        o << "revision: " << sample.revision;
        o <<"]";
        return o;
    }

    // ---- Outer:

    Outer::Outer() :
        payload (0)  {

    }

    Outer::Outer (const ::CorpusNestedKeys::Inner& identity_,int32_t payload_) {
        identity = identity_;
        payload = payload_;
    }

    bool operator == (const Outer& a, const Outer& b) {

        if (a.identity != b.identity) {
            return false;
        }
        if (a.payload != b.payload) {
            return false;
        }

        return true;
    }

    bool operator != (const Outer& a, const Outer& b) {
        return !operator ==(a, b);
    }

    void swap(Outer& a, Outer& b) noexcept
    {
        using std::swap;

        swap(a.identity, b.identity);
        swap(a.payload, b.payload);

    }  
    std::ostream& operator << (std::ostream& o,const Outer& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "identity: " << sample.identity<<", ";
        o << "payload: " << sample.payload;
        o <<"]";
        return o;
    }

} // namespace CorpusNestedKeys  

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
        struct native_type_code< ::CorpusNestedKeys::Inner > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Inner_g_tc_members[2]=
                {

                    {
                        (char *)"code",/* Member name */
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"revision",/* Member name */
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

                static DDS_TypeCode Inner_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNestedKeys::Inner", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Inner_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Inner*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Inner_g_tc;
                }

                Inner_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Inner_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Inner_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Inner_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Inner_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Inner_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Inner_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Inner_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Inner_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Inner_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Inner_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Inner_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Inner_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Inner_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Inner_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Inner_g_tc._data._sampleAccessInfo = sample_access_info();
                Inner_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Inner_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusNestedKeys::Inner *sample;

                static RTIXCdrMemberAccessInfo Inner_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Inner_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Inner_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusNestedKeys::Inner);
                if (sample == NULL) {
                    return NULL;
                }

                Inner_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->code - (char *)sample);

                Inner_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->revision - (char *)sample);

                Inner_g_sampleAccessInfo.memberAccessInfos =
                Inner_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusNestedKeys::Inner);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Inner_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Inner_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Inner_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Inner_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusNestedKeys::Inner >;

                Inner_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Inner_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Inner_g_typePlugin =
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

                return &Inner_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusNestedKeys::Inner >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusNestedKeys::Inner >::get())));
        }

        template<>
        struct native_type_code< ::CorpusNestedKeys::Outer > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Outer_g_tc_members[2]=
                {

                    {
                        (char *)"identity",/* Member name */
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"payload",/* Member name */
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

                static DDS_TypeCode Outer_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNestedKeys::Outer", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Outer_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Outer*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Outer_g_tc;
                }

                Outer_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Outer_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusNestedKeys::Inner>::get().native();
                Outer_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Outer_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Outer_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Outer_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Outer_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Outer_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Outer_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Outer_g_tc._data._sampleAccessInfo = sample_access_info();
                Outer_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Outer_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusNestedKeys::Outer *sample;

                static RTIXCdrMemberAccessInfo Outer_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Outer_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Outer_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusNestedKeys::Outer);
                if (sample == NULL) {
                    return NULL;
                }

                Outer_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->identity - (char *)sample);

                Outer_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->payload - (char *)sample);

                Outer_g_sampleAccessInfo.memberAccessInfos =
                Outer_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusNestedKeys::Outer);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Outer_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Outer_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Outer_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Outer_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusNestedKeys::Outer >;

                Outer_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Outer_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Outer_g_typePlugin =
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

                return &Outer_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusNestedKeys::Outer >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusNestedKeys::Outer >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusNestedKeys::Inner >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusNestedKeys::InnerPlugin_new,
                ::CorpusNestedKeys::InnerPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusNestedKeys::Inner >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusNestedKeys::Inner& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = InnerPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = InnerPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusNestedKeys::Inner >::from_cdr_buffer(::CorpusNestedKeys::Inner& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = InnerPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusNestedKeys::Inner from cdr buffer");
        }

        void topic_type_support< ::CorpusNestedKeys::Inner >::reset_sample(::CorpusNestedKeys::Inner& sample) 
        {
            sample.code = 0;
            sample.revision = 0;
        }

        void topic_type_support< ::CorpusNestedKeys::Inner >::allocate_sample(::CorpusNestedKeys::Inner& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusNestedKeys::Outer >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusNestedKeys::OuterPlugin_new,
                ::CorpusNestedKeys::OuterPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusNestedKeys::Outer >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusNestedKeys::Outer& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = OuterPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = OuterPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusNestedKeys::Outer >::from_cdr_buffer(::CorpusNestedKeys::Outer& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = OuterPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusNestedKeys::Outer from cdr buffer");
        }

        void topic_type_support< ::CorpusNestedKeys::Outer >::reset_sample(::CorpusNestedKeys::Outer& sample) 
        {
            ::rti::topic::reset_sample(sample.identity);
            sample.payload = 0;
        }

        void topic_type_support< ::CorpusNestedKeys::Outer >::allocate_sample(::CorpusNestedKeys::Outer& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.identity,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
