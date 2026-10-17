

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-scoped.idl
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
#include "07-union-scopedPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-scoped.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusScopedUnion {

    // ---- Payload:

    Payload::Payload() :
        code (0) ,
        label ("")  {

    }

    Payload::Payload (int32_t code_,const ::omg::types::string_view& label_) {
        code = code_;
        label = label_;
    }

    bool operator == (const Payload& a, const Payload& b) {

        if (a.code != b.code) {
            return false;
        }
        if (a.label != b.label) {
            return false;
        }

        return true;
    }

    bool operator != (const Payload& a, const Payload& b) {
        return !operator ==(a, b);
    }

    void swap(Payload& a, Payload& b) noexcept
    {
        using std::swap;

        swap(a.code, b.code);
        swap(a.label, b.label);

    }  
    std::ostream& operator << (std::ostream& o,const Payload& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "code: " << sample.code<<", ";
        o << "label: " << sample.label;
        o <<"]";
        return o;
    }

    // ---- Choice:

    Choice::Union_::Union_()  {
    }

    Choice::Union_::Union_(const ::CorpusScopedUnion::Payload& qualifiedPayload_,const ::omg::types::bounded_sequence< int32_t, 4L >& values_):
        qualifiedPayload(qualifiedPayload_), 
        values(values_) {
    }
    Choice::Choice() :m_d_(default_discriminator())
    {
    }

    void Choice::swap(Choice& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 10:
            {  
                swap(m_u_.qualifiedPayload, other_.m_u_.qualifiedPayload);
            } break;
            case 11:
            {  
                swap(m_u_.values, other_.m_u_.values);
            } break;

            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 10:
                {  
                    swap(m_u_.qualifiedPayload, other_.m_u_.qualifiedPayload);
                } break;
                case 11:
                {  
                    swap(m_u_.values, other_.m_u_.values);
                } break;

                default:
                {
                    /*
                    * Prevents compiler warnings when discriminator is an enum.
                    */
                }
            }
        }
    }  

    bool Choice::operator == (const Choice& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 10:
            {  
                if (m_u_.qualifiedPayload != other_.m_u_.qualifiedPayload) {
                    return false;
                }
            } break ;
            case 11:
            {  
                if (m_u_.values != other_.m_u_.values) {
                    return false;
                }
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        return true;
    }

    bool Choice::operator != (const Choice& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Choice& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 10:
            {  
                o << "qualifiedPayload: " << sample.qualifiedPayload()<<", ";
            } break ;
            case 11:
            {  
                o << "values: " << sample.values();
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        o <<"]";
        return o;
    }

    int32_t Choice::default_discriminator() {
        return 0;
    }

} // namespace CorpusScopedUnion  

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
        struct native_type_code< ::CorpusScopedUnion::Payload > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Payload_g_tc_label_string;

                static DDS_TypeCode_Member Payload_g_tc_members[2]=
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
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"label",/* Member name */
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

                static DDS_TypeCode Payload_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusScopedUnion::Payload", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Payload_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Payload*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Payload_g_tc;
                }

                Payload_g_tc_label_string = initialize_string_typecode((16L));

                Payload_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Payload_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Payload_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Payload_g_tc_label_string;

                /* Initialize the values for member annotations. */
                Payload_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Payload_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Payload_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Payload_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Payload_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Payload_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Payload_g_tc._data._sampleAccessInfo = sample_access_info();
                Payload_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Payload_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusScopedUnion::Payload *sample;

                static RTIXCdrMemberAccessInfo Payload_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Payload_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Payload_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusScopedUnion::Payload);
                if (sample == NULL) {
                    return NULL;
                }

                Payload_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->code - (char *)sample);

                Payload_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->label - (char *)sample);

                Payload_g_sampleAccessInfo.memberAccessInfos =
                Payload_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusScopedUnion::Payload);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Payload_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Payload_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Payload_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Payload_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusScopedUnion::Payload >;

                Payload_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Payload_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Payload_g_typePlugin =
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

                return &Payload_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusScopedUnion::Payload >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusScopedUnion::Payload >::get())));
        }

        template<>
        struct native_type_code< ::CorpusScopedUnion::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Choice_g_tc_values_sequence;

                static DDS_TypeCode_Member Choice_g_tc_members[2]=
                {

                    {
                        (char *)"qualifiedPayload",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(10),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"values",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(11),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Choice_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusScopedUnion::Choice", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Choice_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Choice*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Choice_g_tc;
                }

                Choice_g_tc_values_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< int32_t, 4L > >((4L));

                Choice_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Choice_g_tc_values_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Choice_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusScopedUnion::Payload>::get().native();
                Choice_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)& Choice_g_tc_values_sequence;

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Choice_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusScopedUnion::Choice *sample;

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusScopedUnion::Choice);
                if (sample == NULL) {
                    return NULL;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(10);
                Choice_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->qualifiedPayload() - (char *)sample);

                sample->_d(11);
                Choice_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->values() - (char *)sample);

                Choice_g_sampleAccessInfo.memberAccessInfos =
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusScopedUnion::Choice);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Choice_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Choice_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Choice_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Choice_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusScopedUnion::Choice >;

                Choice_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Choice_g_typePlugin =
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

                return &Choice_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusScopedUnion::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusScopedUnion::Choice >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusScopedUnion::Payload >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusScopedUnion::PayloadPlugin_new,
                ::CorpusScopedUnion::PayloadPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusScopedUnion::Payload >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusScopedUnion::Payload& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = PayloadPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = PayloadPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusScopedUnion::Payload >::from_cdr_buffer(::CorpusScopedUnion::Payload& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = PayloadPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusScopedUnion::Payload from cdr buffer");
        }

        void topic_type_support< ::CorpusScopedUnion::Payload >::reset_sample(::CorpusScopedUnion::Payload& sample) 
        {
            sample.code = 0;
            sample.label = "";
        }

        void topic_type_support< ::CorpusScopedUnion::Payload >::allocate_sample(::CorpusScopedUnion::Payload& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.label,  -1, 16L);
        }
        void topic_type_support< ::CorpusScopedUnion::Choice >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusScopedUnion::ChoicePlugin_new,
                ::CorpusScopedUnion::ChoicePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusScopedUnion::Choice >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusScopedUnion::Choice& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ChoicePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ChoicePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusScopedUnion::Choice >::from_cdr_buffer(::CorpusScopedUnion::Choice& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChoicePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusScopedUnion::Choice from cdr buffer");
        }

        void topic_type_support< ::CorpusScopedUnion::Choice >::reset_sample(::CorpusScopedUnion::Choice& sample) 
        {
            sample._d(10);
            ::rti::topic::reset_sample(sample.qualifiedPayload());
            sample._d(11);
            ::rti::topic::reset_sample(sample.values());

            sample._d(::CorpusScopedUnion::Choice::default_discriminator());
        }

        void topic_type_support< ::CorpusScopedUnion::Choice >::allocate_sample(::CorpusScopedUnion::Choice& sample, int, int) 
        {
            sample._d(10);
            ::rti::topic::allocate_sample(sample.qualifiedPayload(),  -1, -1);
            sample._d(11);
            ::rti::topic::allocate_sample(sample.values(),  4L, -1);

            sample._d(::CorpusScopedUnion::Choice::default_discriminator());
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
