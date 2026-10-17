

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-valuetypes.idl
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
#include "06-valuetypesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "06-valuetypes.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusValueTypes {

    // ---- BaseValue:

    BaseValue::BaseValue() :
        m_baseValue_ (0)  {

    }

    BaseValue::BaseValue (int32_t m_baseValue__) {
        m_baseValue_ = m_baseValue__;
    }

    void BaseValue::swap(BaseValue& other_)  noexcept 
    {
        using std::swap;
        swap(m_baseValue_, other_.m_baseValue_);
    }  

    bool BaseValue::operator == (const BaseValue& other_) const {
        if (m_baseValue_ != other_.m_baseValue_) {
            return false;
        }
        return true;
    }

    bool BaseValue::operator != (const BaseValue& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const BaseValue& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "baseValue: " << sample.baseValue();
        o <<"]";
        return o;
    }

    // ---- DerivedValue:

    DerivedValue::DerivedValue() :
        m_name_ ("")  {

    }

    DerivedValue::DerivedValue (int32_t m_baseValue__,const ::omg::types::string_view& m_name__) :
        ::CorpusValueTypes::BaseValue(m_baseValue__) {
            m_name_ = m_name__;
    }

    void DerivedValue::swap(DerivedValue& other_)  noexcept 
    {
        using std::swap;
        ::CorpusValueTypes::BaseValue::swap(other_);
        swap(m_name_, other_.m_name_);
    }  

    bool DerivedValue::operator == (const DerivedValue& other_) const {
        if (!::CorpusValueTypes::BaseValue::operator == (other_)){
            return false;
        }
        if (m_name_ != other_.m_name_) {
            return false;
        }
        return true;
    }

    bool DerivedValue::operator != (const DerivedValue& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const DerivedValue& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << static_cast<const ::CorpusValueTypes::BaseValue &>(sample);
        o << "name: " << sample.name();
        o <<"]";
        return o;
    }

    // ---- Holder:

    Holder::Holder()  {

    }

    Holder::Holder (const ::CorpusValueTypes::DerivedValue& value_) {
        value = value_;
    }

    bool operator == (const Holder& a, const Holder& b) {

        if (a.value != b.value) {
            return false;
        }

        return true;
    }

    bool operator != (const Holder& a, const Holder& b) {
        return !operator ==(a, b);
    }

    void swap(Holder& a, Holder& b) noexcept
    {
        using std::swap;

        swap(a.value, b.value);

    }  
    std::ostream& operator << (std::ostream& o,const Holder& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value;
        o <<"]";
        return o;
    }

} // namespace CorpusValueTypes  

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
        struct native_type_code< ::CorpusValueTypes::BaseValue > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member BaseValue_g_tc_members[1]=
                {

                    {
                        (char *)"baseValue",/* Member name */
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

                static DDS_TypeCode BaseValue_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusValueTypes::BaseValue", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        BaseValue_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for BaseValue*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BaseValue_g_tc;
                }

                BaseValue_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BaseValue_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                BaseValue_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                BaseValue_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                BaseValue_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                BaseValue_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                BaseValue_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                BaseValue_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                BaseValue_g_tc._data._typeCode = (RTICdrTypeCode *) &DDS_g_tc_null;  /* Base class */

                BaseValue_g_tc._data._sampleAccessInfo = sample_access_info();
                BaseValue_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &BaseValue_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusValueTypes::BaseValue *sample;

                static RTIXCdrMemberAccessInfo BaseValue_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BaseValue_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BaseValue_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusValueTypes::BaseValue);
                if (sample == NULL) {
                    return NULL;
                }

                BaseValue_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->baseValue() - (char *)sample);

                BaseValue_g_sampleAccessInfo.memberAccessInfos =
                BaseValue_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusValueTypes::BaseValue);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BaseValue_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BaseValue_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BaseValue_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BaseValue_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusValueTypes::BaseValue >;

                BaseValue_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BaseValue_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BaseValue_g_typePlugin =
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

                return &BaseValue_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusValueTypes::BaseValue >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusValueTypes::BaseValue >::get())));
        }

        template<>
        struct native_type_code< ::CorpusValueTypes::DerivedValue > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode DerivedValue_g_tc_name_string;

                static DDS_TypeCode_Member DerivedValue_g_tc_members[1]=
                {

                    {
                        (char *)"name",/* Member name */
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

                static DDS_TypeCode DerivedValue_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusValueTypes::DerivedValue", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        DerivedValue_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for DerivedValue*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &DerivedValue_g_tc;
                }

                DerivedValue_g_tc_name_string = initialize_string_typecode((16L));

                DerivedValue_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                DerivedValue_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DerivedValue_g_tc_name_string;

                /* Initialize the values for member annotations. */
                DerivedValue_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                DerivedValue_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                DerivedValue_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusValueTypes::BaseValue >::get().native(); /* Base class */

                DerivedValue_g_tc._data._sampleAccessInfo = sample_access_info();
                DerivedValue_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &DerivedValue_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusValueTypes::DerivedValue *sample;

                static RTIXCdrMemberAccessInfo DerivedValue_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo DerivedValue_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &DerivedValue_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusValueTypes::DerivedValue);
                if (sample == NULL) {
                    return NULL;
                }

                DerivedValue_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->name() - (char *)sample);

                DerivedValue_g_sampleAccessInfo.memberAccessInfos =
                DerivedValue_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusValueTypes::DerivedValue);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        DerivedValue_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        DerivedValue_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                DerivedValue_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                DerivedValue_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusValueTypes::DerivedValue >;

                DerivedValue_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &DerivedValue_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin DerivedValue_g_typePlugin =
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

                return &DerivedValue_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusValueTypes::DerivedValue >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusValueTypes::DerivedValue >::get())));
        }

        template<>
        struct native_type_code< ::CorpusValueTypes::Holder > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Holder_g_tc_members[1]=
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

                static DDS_TypeCode Holder_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusValueTypes::Holder", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Holder_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Holder*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Holder_g_tc;
                }

                Holder_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Holder_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusValueTypes::DerivedValue>::get().native();

                /* Initialize the values for member annotations. */

                Holder_g_tc._data._sampleAccessInfo = sample_access_info();
                Holder_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Holder_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusValueTypes::Holder *sample;

                static RTIXCdrMemberAccessInfo Holder_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Holder_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Holder_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusValueTypes::Holder);
                if (sample == NULL) {
                    return NULL;
                }

                Holder_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                Holder_g_sampleAccessInfo.memberAccessInfos =
                Holder_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusValueTypes::Holder);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Holder_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Holder_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Holder_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Holder_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusValueTypes::Holder >;

                Holder_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Holder_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Holder_g_typePlugin =
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

                return &Holder_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusValueTypes::Holder >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusValueTypes::Holder >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusValueTypes::BaseValue >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusValueTypes::BaseValuePlugin_new,
                ::CorpusValueTypes::BaseValuePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusValueTypes::BaseValue >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusValueTypes::BaseValue& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = BaseValuePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = BaseValuePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusValueTypes::BaseValue >::from_cdr_buffer(::CorpusValueTypes::BaseValue& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = BaseValuePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusValueTypes::BaseValue from cdr buffer");
        }

        void topic_type_support< ::CorpusValueTypes::BaseValue >::reset_sample(::CorpusValueTypes::BaseValue& sample) 
        {
            sample.baseValue(0);
        }

        void topic_type_support< ::CorpusValueTypes::BaseValue >::allocate_sample(::CorpusValueTypes::BaseValue& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusValueTypes::DerivedValue >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusValueTypes::DerivedValuePlugin_new,
                ::CorpusValueTypes::DerivedValuePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusValueTypes::DerivedValue >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusValueTypes::DerivedValue& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = DerivedValuePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = DerivedValuePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusValueTypes::DerivedValue >::from_cdr_buffer(::CorpusValueTypes::DerivedValue& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DerivedValuePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusValueTypes::DerivedValue from cdr buffer");
        }

        void topic_type_support< ::CorpusValueTypes::DerivedValue >::reset_sample(::CorpusValueTypes::DerivedValue& sample) 
        {
            // Initialize base
            topic_type_support< ::CorpusValueTypes::BaseValue >::reset_sample(sample);

            sample.name("");
        }

        void topic_type_support< ::CorpusValueTypes::DerivedValue >::allocate_sample(::CorpusValueTypes::DerivedValue& sample, int, int) 
        {
            // Initialize base
            topic_type_support< ::CorpusValueTypes::BaseValue >::allocate_sample(sample, -1, -1);

            ::rti::topic::allocate_sample(sample.name(),  -1, 16L);
        }
        void topic_type_support< ::CorpusValueTypes::Holder >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusValueTypes::HolderPlugin_new,
                ::CorpusValueTypes::HolderPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusValueTypes::Holder >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusValueTypes::Holder& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = HolderPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = HolderPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusValueTypes::Holder >::from_cdr_buffer(::CorpusValueTypes::Holder& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = HolderPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusValueTypes::Holder from cdr buffer");
        }

        void topic_type_support< ::CorpusValueTypes::Holder >::reset_sample(::CorpusValueTypes::Holder& sample) 
        {
            ::rti::topic::reset_sample(sample.value);
        }

        void topic_type_support< ::CorpusValueTypes::Holder >::allocate_sample(::CorpusValueTypes::Holder& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.value,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
