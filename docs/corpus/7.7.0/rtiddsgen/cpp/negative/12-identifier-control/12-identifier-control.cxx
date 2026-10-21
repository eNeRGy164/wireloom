

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-identifier-control.idl
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
#include "12-identifier-controlPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "12-identifier-control.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusIdentifierControl {

    // ---- _Struct:

    _Struct::_Struct() :
        event (0)  {

    }

    _Struct::_Struct (int32_t event_) {
        event = event_;
    }

    bool operator == (const _Struct& a, const _Struct& b) {

        if (a.event != b.event) {
            return false;
        }

        return true;
    }

    bool operator != (const _Struct& a, const _Struct& b) {
        return !operator ==(a, b);
    }

    void swap(_Struct& a, _Struct& b) noexcept
    {
        using std::swap;

        swap(a.event, b.event);

    }  
    std::ostream& operator << (std::ostream& o,const _Struct& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "event: " << sample.event;
        o <<"]";
        return o;
    }

} // namespace CorpusIdentifierControl  

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
        struct native_type_code< ::CorpusIdentifierControl::_Struct > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member _Struct_g_tc_members[1]=
                {

                    {
                        (char *)"event",/* Member name */
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

                static DDS_TypeCode _Struct_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIdentifierControl::_Struct", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        _Struct_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for _Struct*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &_Struct_g_tc;
                }

                _Struct_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                _Struct_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                _Struct_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                _Struct_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                _Struct_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                _Struct_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                _Struct_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                _Struct_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                _Struct_g_tc._data._sampleAccessInfo = sample_access_info();
                _Struct_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &_Struct_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIdentifierControl::_Struct *sample;

                static RTIXCdrMemberAccessInfo _Struct_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo _Struct_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &_Struct_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusIdentifierControl::_Struct);
                if (sample == NULL) {
                    return NULL;
                }

                _Struct_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->event - (char *)sample);

                _Struct_g_sampleAccessInfo.memberAccessInfos =
                _Struct_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIdentifierControl::_Struct);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        _Struct_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        _Struct_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                _Struct_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                _Struct_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIdentifierControl::_Struct >;

                _Struct_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &_Struct_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin _Struct_g_typePlugin =
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

                return &_Struct_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIdentifierControl::_Struct >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIdentifierControl::_Struct >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusIdentifierControl::_Struct >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusIdentifierControl::_StructPlugin_new,
                ::CorpusIdentifierControl::_StructPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusIdentifierControl::_Struct >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusIdentifierControl::_Struct& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = _StructPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = _StructPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusIdentifierControl::_Struct >::from_cdr_buffer(::CorpusIdentifierControl::_Struct& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = _StructPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusIdentifierControl::_Struct from cdr buffer");
        }

        void topic_type_support< ::CorpusIdentifierControl::_Struct >::reset_sample(::CorpusIdentifierControl::_Struct& sample) 
        {
            sample.event = 0;
        }

        void topic_type_support< ::CorpusIdentifierControl::_Struct >::allocate_sample(::CorpusIdentifierControl::_Struct& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
