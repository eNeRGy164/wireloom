

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-boundaries.idl
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
#include "08-key-boundariesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "08-key-boundaries.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusKeyBoundaries {

    // ---- ArrayKey:

    ArrayKey::ArrayKey() :
        payload (0)  {

    }

    ArrayKey::ArrayKey (const ::dds::core::array< int32_t, 2L>& coordinates_,int32_t payload_) {
        coordinates = coordinates_;
        payload = payload_;
    }

    bool operator == (const ArrayKey& a, const ArrayKey& b) {

        if (a.coordinates != b.coordinates) {
            return false;
        }
        if (a.payload != b.payload) {
            return false;
        }

        return true;
    }

    bool operator != (const ArrayKey& a, const ArrayKey& b) {
        return !operator ==(a, b);
    }

    void swap(ArrayKey& a, ArrayKey& b) noexcept
    {
        using std::swap;

        swap(a.coordinates, b.coordinates);
        swap(a.payload, b.payload);

    }  
    std::ostream& operator << (std::ostream& o,const ArrayKey& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "coordinates: " << sample.coordinates<<", ";
        o << "payload: " << sample.payload;
        o <<"]";
        return o;
    }

    // ---- BoundedStringKey:

    BoundedStringKey::BoundedStringKey() :
        name ("") ,
        payload (0)  {

    }

    BoundedStringKey::BoundedStringKey (const ::omg::types::string_view& name_,int32_t payload_) {
        name = name_;
        payload = payload_;
    }

    bool operator == (const BoundedStringKey& a, const BoundedStringKey& b) {

        if (a.name != b.name) {
            return false;
        }
        if (a.payload != b.payload) {
            return false;
        }

        return true;
    }

    bool operator != (const BoundedStringKey& a, const BoundedStringKey& b) {
        return !operator ==(a, b);
    }

    void swap(BoundedStringKey& a, BoundedStringKey& b) noexcept
    {
        using std::swap;

        swap(a.name, b.name);
        swap(a.payload, b.payload);

    }  
    std::ostream& operator << (std::ostream& o,const BoundedStringKey& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "name: " << sample.name<<", ";
        o << "payload: " << sample.payload;
        o <<"]";
        return o;
    }

    // ---- UnboundedStringKey:

    UnboundedStringKey::UnboundedStringKey() :
        name ("") ,
        payload (0)  {

    }

    UnboundedStringKey::UnboundedStringKey (const ::omg::types::string_view& name_,int32_t payload_) {
        name = name_;
        payload = payload_;
    }

    bool operator == (const UnboundedStringKey& a, const UnboundedStringKey& b) {

        if (a.name != b.name) {
            return false;
        }
        if (a.payload != b.payload) {
            return false;
        }

        return true;
    }

    bool operator != (const UnboundedStringKey& a, const UnboundedStringKey& b) {
        return !operator ==(a, b);
    }

    void swap(UnboundedStringKey& a, UnboundedStringKey& b) noexcept
    {
        using std::swap;

        swap(a.name, b.name);
        swap(a.payload, b.payload);

    }  
    std::ostream& operator << (std::ostream& o,const UnboundedStringKey& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "name: " << sample.name<<", ";
        o << "payload: " << sample.payload;
        o <<"]";
        return o;
    }

} // namespace CorpusKeyBoundaries  

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
        struct native_type_code< ::CorpusKeyBoundaries::ArrayKey > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode ArrayKey_g_tc_coordinates_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);

                static DDS_TypeCode_Member ArrayKey_g_tc_members[2]=
                {

                    {
                        (char *)"coordinates",/* Member name */
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

                static DDS_TypeCode ArrayKey_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusKeyBoundaries::ArrayKey", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        ArrayKey_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ArrayKey*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ArrayKey_g_tc;
                }

                ArrayKey_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ArrayKey_g_tc_coordinates_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                ArrayKey_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)& ArrayKey_g_tc_coordinates_array;
                ArrayKey_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                ArrayKey_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ArrayKey_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                ArrayKey_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ArrayKey_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ArrayKey_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ArrayKey_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                ArrayKey_g_tc._data._sampleAccessInfo = sample_access_info();
                ArrayKey_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &ArrayKey_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusKeyBoundaries::ArrayKey *sample;

                static RTIXCdrMemberAccessInfo ArrayKey_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ArrayKey_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ArrayKey_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusKeyBoundaries::ArrayKey);
                if (sample == NULL) {
                    return NULL;
                }

                ArrayKey_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->coordinates - (char *)sample);

                ArrayKey_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->payload - (char *)sample);

                ArrayKey_g_sampleAccessInfo.memberAccessInfos =
                ArrayKey_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusKeyBoundaries::ArrayKey);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ArrayKey_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ArrayKey_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ArrayKey_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ArrayKey_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusKeyBoundaries::ArrayKey >;

                ArrayKey_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ArrayKey_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ArrayKey_g_typePlugin =
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

                return &ArrayKey_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusKeyBoundaries::ArrayKey >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusKeyBoundaries::ArrayKey >::get())));
        }

        template<>
        struct native_type_code< ::CorpusKeyBoundaries::BoundedStringKey > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedStringKey_g_tc_name_string;

                static DDS_TypeCode_Member BoundedStringKey_g_tc_members[2]=
                {

                    {
                        (char *)"name",/* Member name */
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

                static DDS_TypeCode BoundedStringKey_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusKeyBoundaries::BoundedStringKey", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        BoundedStringKey_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for BoundedStringKey*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedStringKey_g_tc;
                }

                BoundedStringKey_g_tc_name_string = initialize_string_typecode((12L));

                BoundedStringKey_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedStringKey_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&BoundedStringKey_g_tc_name_string;
                BoundedStringKey_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                BoundedStringKey_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                BoundedStringKey_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                BoundedStringKey_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                BoundedStringKey_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                BoundedStringKey_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                BoundedStringKey_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                BoundedStringKey_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                BoundedStringKey_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                BoundedStringKey_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedStringKey_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &BoundedStringKey_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusKeyBoundaries::BoundedStringKey *sample;

                static RTIXCdrMemberAccessInfo BoundedStringKey_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedStringKey_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedStringKey_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusKeyBoundaries::BoundedStringKey);
                if (sample == NULL) {
                    return NULL;
                }

                BoundedStringKey_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->name - (char *)sample);

                BoundedStringKey_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->payload - (char *)sample);

                BoundedStringKey_g_sampleAccessInfo.memberAccessInfos =
                BoundedStringKey_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusKeyBoundaries::BoundedStringKey);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedStringKey_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedStringKey_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedStringKey_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedStringKey_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusKeyBoundaries::BoundedStringKey >;

                BoundedStringKey_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedStringKey_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedStringKey_g_typePlugin =
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

                return &BoundedStringKey_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusKeyBoundaries::BoundedStringKey >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusKeyBoundaries::BoundedStringKey >::get())));
        }

        template<>
        struct native_type_code< ::CorpusKeyBoundaries::UnboundedStringKey > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode UnboundedStringKey_g_tc_name_string;

                static DDS_TypeCode_Member UnboundedStringKey_g_tc_members[2]=
                {

                    {
                        (char *)"name",/* Member name */
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

                static DDS_TypeCode UnboundedStringKey_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusKeyBoundaries::UnboundedStringKey", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        UnboundedStringKey_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for UnboundedStringKey*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &UnboundedStringKey_g_tc;
                }

                UnboundedStringKey_g_tc_name_string = initialize_string_typecode((255L));

                UnboundedStringKey_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                UnboundedStringKey_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&UnboundedStringKey_g_tc_name_string;
                UnboundedStringKey_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                UnboundedStringKey_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                UnboundedStringKey_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                UnboundedStringKey_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                UnboundedStringKey_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                UnboundedStringKey_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                UnboundedStringKey_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                UnboundedStringKey_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                UnboundedStringKey_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                UnboundedStringKey_g_tc._data._sampleAccessInfo = sample_access_info();
                UnboundedStringKey_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &UnboundedStringKey_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusKeyBoundaries::UnboundedStringKey *sample;

                static RTIXCdrMemberAccessInfo UnboundedStringKey_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo UnboundedStringKey_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &UnboundedStringKey_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusKeyBoundaries::UnboundedStringKey);
                if (sample == NULL) {
                    return NULL;
                }

                UnboundedStringKey_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->name - (char *)sample);

                UnboundedStringKey_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->payload - (char *)sample);

                UnboundedStringKey_g_sampleAccessInfo.memberAccessInfos =
                UnboundedStringKey_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusKeyBoundaries::UnboundedStringKey);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        UnboundedStringKey_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        UnboundedStringKey_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                UnboundedStringKey_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                UnboundedStringKey_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusKeyBoundaries::UnboundedStringKey >;

                UnboundedStringKey_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &UnboundedStringKey_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin UnboundedStringKey_g_typePlugin =
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

                return &UnboundedStringKey_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusKeyBoundaries::UnboundedStringKey >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusKeyBoundaries::UnboundedStringKey >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusKeyBoundaries::ArrayKey >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusKeyBoundaries::ArrayKeyPlugin_new,
                ::CorpusKeyBoundaries::ArrayKeyPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusKeyBoundaries::ArrayKey >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusKeyBoundaries::ArrayKey& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ArrayKeyPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ArrayKeyPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusKeyBoundaries::ArrayKey >::from_cdr_buffer(::CorpusKeyBoundaries::ArrayKey& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ArrayKeyPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusKeyBoundaries::ArrayKey from cdr buffer");
        }

        void topic_type_support< ::CorpusKeyBoundaries::ArrayKey >::reset_sample(::CorpusKeyBoundaries::ArrayKey& sample) 
        {
            ::rti::topic::reset_sample(sample.coordinates);
            sample.payload = 0;
        }

        void topic_type_support< ::CorpusKeyBoundaries::ArrayKey >::allocate_sample(::CorpusKeyBoundaries::ArrayKey& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.coordinates,  -1, -1);
        }
        void topic_type_support< ::CorpusKeyBoundaries::BoundedStringKey >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusKeyBoundaries::BoundedStringKeyPlugin_new,
                ::CorpusKeyBoundaries::BoundedStringKeyPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusKeyBoundaries::BoundedStringKey >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusKeyBoundaries::BoundedStringKey& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = BoundedStringKeyPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = BoundedStringKeyPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusKeyBoundaries::BoundedStringKey >::from_cdr_buffer(::CorpusKeyBoundaries::BoundedStringKey& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = BoundedStringKeyPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusKeyBoundaries::BoundedStringKey from cdr buffer");
        }

        void topic_type_support< ::CorpusKeyBoundaries::BoundedStringKey >::reset_sample(::CorpusKeyBoundaries::BoundedStringKey& sample) 
        {
            sample.name = "";
            sample.payload = 0;
        }

        void topic_type_support< ::CorpusKeyBoundaries::BoundedStringKey >::allocate_sample(::CorpusKeyBoundaries::BoundedStringKey& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.name,  -1, 12L);
        }
        void topic_type_support< ::CorpusKeyBoundaries::UnboundedStringKey >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusKeyBoundaries::UnboundedStringKeyPlugin_new,
                ::CorpusKeyBoundaries::UnboundedStringKeyPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusKeyBoundaries::UnboundedStringKey >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusKeyBoundaries::UnboundedStringKey& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = UnboundedStringKeyPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = UnboundedStringKeyPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusKeyBoundaries::UnboundedStringKey >::from_cdr_buffer(::CorpusKeyBoundaries::UnboundedStringKey& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = UnboundedStringKeyPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusKeyBoundaries::UnboundedStringKey from cdr buffer");
        }

        void topic_type_support< ::CorpusKeyBoundaries::UnboundedStringKey >::reset_sample(::CorpusKeyBoundaries::UnboundedStringKey& sample) 
        {
            sample.name = "";
            sample.payload = 0;
        }

        void topic_type_support< ::CorpusKeyBoundaries::UnboundedStringKey >::allocate_sample(::CorpusKeyBoundaries::UnboundedStringKey& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.name,  -1, 255L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
