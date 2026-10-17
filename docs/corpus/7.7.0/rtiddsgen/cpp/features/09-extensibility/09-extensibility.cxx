

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-extensibility.idl
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
#include "09-extensibilityPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "09-extensibility.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusExtensibility {

    // ---- Appendable:

    Appendable::Appendable() :
        id (0) ,
        text ("")  {

    }

    Appendable::Appendable (int32_t id_,const ::omg::types::string_view& text_) {
        id = id_;
        text = text_;
    }

    bool operator == (const Appendable& a, const Appendable& b) {

        if (a.id != b.id) {
            return false;
        }
        if (a.text != b.text) {
            return false;
        }

        return true;
    }

    bool operator != (const Appendable& a, const Appendable& b) {
        return !operator ==(a, b);
    }

    void swap(Appendable& a, Appendable& b) noexcept
    {
        using std::swap;

        swap(a.id, b.id);
        swap(a.text, b.text);

    }  
    std::ostream& operator << (std::ostream& o,const Appendable& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id<<", ";
        o << "text: " << sample.text;
        o <<"]";
        return o;
    }

    // ---- Final:

    Final::Final() :
        id (0) ,
        text ("")  {

    }

    Final::Final (int32_t id_,const ::omg::types::string_view& text_) {
        id = id_;
        text = text_;
    }

    bool operator == (const Final& a, const Final& b) {

        if (a.id != b.id) {
            return false;
        }
        if (a.text != b.text) {
            return false;
        }

        return true;
    }

    bool operator != (const Final& a, const Final& b) {
        return !operator ==(a, b);
    }

    void swap(Final& a, Final& b) noexcept
    {
        using std::swap;

        swap(a.id, b.id);
        swap(a.text, b.text);

    }  
    std::ostream& operator << (std::ostream& o,const Final& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id<<", ";
        o << "text: " << sample.text;
        o <<"]";
        return o;
    }

    // ---- Mutable:

    Mutable::Mutable() :
        id (0)  {

    }

    Mutable::Mutable (int32_t id_,const ::omg::types::optional< int32_t >& optionalValue_) {
        id = id_;
        optionalValue = optionalValue_;
    }

    bool operator == (const Mutable& a, const Mutable& b) {

        if (a.id != b.id) {
            return false;
        }
        if (a.optionalValue != b.optionalValue) {
            return false;
        }

        return true;
    }

    bool operator != (const Mutable& a, const Mutable& b) {
        return !operator ==(a, b);
    }

    void swap(Mutable& a, Mutable& b) noexcept
    {
        using std::swap;

        swap(a.id, b.id);
        swap(a.optionalValue, b.optionalValue);

    }  
    std::ostream& operator << (std::ostream& o,const Mutable& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id<<", ";
        o << "optionalValue: " << sample.optionalValue;
        o <<"]";
        return o;
    }

} // namespace CorpusExtensibility  

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
        struct native_type_code< ::CorpusExtensibility::Appendable > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Appendable_g_tc_text_string;

                static DDS_TypeCode_Member Appendable_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                    }, 
                    {
                        (char *)"text",/* Member name */
                        {
                            2,/* Representation ID */
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

                static DDS_TypeCode Appendable_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusExtensibility::Appendable", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Appendable_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Appendable*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Appendable_g_tc;
                }

                Appendable_g_tc_text_string = initialize_string_typecode((16L));

                Appendable_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Appendable_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Appendable_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Appendable_g_tc_text_string;

                /* Initialize the values for member annotations. */
                Appendable_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Appendable_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Appendable_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Appendable_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Appendable_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Appendable_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Appendable_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Appendable_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Appendable_g_tc._data._sampleAccessInfo = sample_access_info();
                Appendable_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Appendable_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusExtensibility::Appendable *sample;

                static RTIXCdrMemberAccessInfo Appendable_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Appendable_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Appendable_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusExtensibility::Appendable);
                if (sample == NULL) {
                    return NULL;
                }

                Appendable_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->id - (char *)sample);

                Appendable_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->text - (char *)sample);

                Appendable_g_sampleAccessInfo.memberAccessInfos =
                Appendable_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusExtensibility::Appendable);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Appendable_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Appendable_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Appendable_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Appendable_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusExtensibility::Appendable >;

                Appendable_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Appendable_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Appendable_g_typePlugin =
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

                return &Appendable_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusExtensibility::Appendable >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusExtensibility::Appendable >::get())));
        }

        template<>
        struct native_type_code< ::CorpusExtensibility::Final > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Final_g_tc_text_string;

                static DDS_TypeCode_Member Final_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                    }, 
                    {
                        (char *)"text",/* Member name */
                        {
                            2,/* Representation ID */
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

                static DDS_TypeCode Final_g_tc =
                {{
                        DDS_TK_STRUCT| DDS_TK_FINAL_EXTENSIBILITY, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusExtensibility::Final", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Final_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Final*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Final_g_tc;
                }

                Final_g_tc_text_string = initialize_string_typecode((16L));

                Final_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Final_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Final_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Final_g_tc_text_string;

                /* Initialize the values for member annotations. */
                Final_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Final_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Final_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Final_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Final_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Final_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Final_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Final_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Final_g_tc._data._sampleAccessInfo = sample_access_info();
                Final_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Final_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusExtensibility::Final *sample;

                static RTIXCdrMemberAccessInfo Final_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Final_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Final_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusExtensibility::Final);
                if (sample == NULL) {
                    return NULL;
                }

                Final_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->id - (char *)sample);

                Final_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->text - (char *)sample);

                Final_g_sampleAccessInfo.memberAccessInfos =
                Final_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusExtensibility::Final);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Final_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Final_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Final_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Final_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusExtensibility::Final >;

                Final_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Final_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Final_g_typePlugin =
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

                return &Final_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusExtensibility::Final >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusExtensibility::Final >::get())));
        }

        template<>
        struct native_type_code< ::CorpusExtensibility::Mutable > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Mutable_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                    }, 
                    {
                        (char *)"optionalValue",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Mutable_g_tc =
                {{
                        DDS_TK_STRUCT| DDS_TK_MUTABLE_EXTENSIBILITY, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusExtensibility::Mutable", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Mutable_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Mutable*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Mutable_g_tc;
                }

                Mutable_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Mutable_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Mutable_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Mutable_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Mutable_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Mutable_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Mutable_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Mutable_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Mutable_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Mutable_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Mutable_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Mutable_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Mutable_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Mutable_g_tc._data._sampleAccessInfo = sample_access_info();
                Mutable_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Mutable_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusExtensibility::Mutable *sample;

                static RTIXCdrMemberAccessInfo Mutable_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Mutable_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Mutable_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusExtensibility::Mutable);
                if (sample == NULL) {
                    return NULL;
                }

                Mutable_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->id - (char *)sample);

                Mutable_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->optionalValue - (char *)sample);

                Mutable_g_sampleAccessInfo.memberAccessInfos =
                Mutable_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusExtensibility::Mutable);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Mutable_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Mutable_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Mutable_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Mutable_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusExtensibility::Mutable >;

                Mutable_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Mutable_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Mutable_g_typePlugin =
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

                return &Mutable_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusExtensibility::Mutable >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusExtensibility::Mutable >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusExtensibility::Appendable >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusExtensibility::AppendablePlugin_new,
                ::CorpusExtensibility::AppendablePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusExtensibility::Appendable >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusExtensibility::Appendable& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = AppendablePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = AppendablePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusExtensibility::Appendable >::from_cdr_buffer(::CorpusExtensibility::Appendable& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = AppendablePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusExtensibility::Appendable from cdr buffer");
        }

        void topic_type_support< ::CorpusExtensibility::Appendable >::reset_sample(::CorpusExtensibility::Appendable& sample) 
        {
            sample.id = 0;
            sample.text = "";
        }

        void topic_type_support< ::CorpusExtensibility::Appendable >::allocate_sample(::CorpusExtensibility::Appendable& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text,  -1, 16L);
        }
        void topic_type_support< ::CorpusExtensibility::Final >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusExtensibility::FinalPlugin_new,
                ::CorpusExtensibility::FinalPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusExtensibility::Final >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusExtensibility::Final& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = FinalPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = FinalPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusExtensibility::Final >::from_cdr_buffer(::CorpusExtensibility::Final& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = FinalPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusExtensibility::Final from cdr buffer");
        }

        void topic_type_support< ::CorpusExtensibility::Final >::reset_sample(::CorpusExtensibility::Final& sample) 
        {
            sample.id = 0;
            sample.text = "";
        }

        void topic_type_support< ::CorpusExtensibility::Final >::allocate_sample(::CorpusExtensibility::Final& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.text,  -1, 16L);
        }
        void topic_type_support< ::CorpusExtensibility::Mutable >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusExtensibility::MutablePlugin_new,
                ::CorpusExtensibility::MutablePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusExtensibility::Mutable >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusExtensibility::Mutable& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = MutablePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = MutablePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusExtensibility::Mutable >::from_cdr_buffer(::CorpusExtensibility::Mutable& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = MutablePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusExtensibility::Mutable from cdr buffer");
        }

        void topic_type_support< ::CorpusExtensibility::Mutable >::reset_sample(::CorpusExtensibility::Mutable& sample) 
        {
            sample.id = 0;
            ::rti::topic::reset_sample(sample.optionalValue);
        }

        void topic_type_support< ::CorpusExtensibility::Mutable >::allocate_sample(::CorpusExtensibility::Mutable& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
