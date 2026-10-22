

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "14-autoid-scopePlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-autoid-scope.hpp"

#include <rti/util/ostream_operators.hpp>

namespace AutoIds {

    // ---- Hashed: 

    Hashed::Hashed() :
        m_first_ (0) ,
        m_second_ (0)  {

    }   

    Hashed::Hashed (int32_t first_,int32_t second_):
        m_first_(first_), 
        m_second_(second_) {
    }

    void Hashed::swap(Hashed& other_)  noexcept 
    {
        using std::swap;
        swap(m_first_, other_.m_first_);
        swap(m_second_, other_.m_second_);
    }  

    bool Hashed::operator == (const Hashed& other_) const {
        if (m_first_ != other_.m_first_) {
            return false;
        }
        if (m_second_ != other_.m_second_) {
            return false;
        }
        return true;
    }

    bool Hashed::operator != (const Hashed& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Hashed& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "first: " << sample.first ()<<", ";
        o << "second: " << sample.second ();
        o <<"]";
        return o;
    }

} // namespace AutoIds  

namespace AutoIds {

    // ---- Reopened: 

    Reopened::Reopened() :
        m_first_ (0) ,
        m_second_ (0)  {

    }   

    Reopened::Reopened (int32_t first_,int32_t second_):
        m_first_(first_), 
        m_second_(second_) {
    }

    void Reopened::swap(Reopened& other_)  noexcept 
    {
        using std::swap;
        swap(m_first_, other_.m_first_);
        swap(m_second_, other_.m_second_);
    }  

    bool Reopened::operator == (const Reopened& other_) const {
        if (m_first_ != other_.m_first_) {
            return false;
        }
        if (m_second_ != other_.m_second_) {
            return false;
        }
        return true;
    }

    bool Reopened::operator != (const Reopened& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Reopened& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "first: " << sample.first ()<<", ";
        o << "second: " << sample.second ();
        o <<"]";
        return o;
    }

} // namespace AutoIds  

// ---- Sequential: 

Sequential::Sequential() :
    m_first_ (0) ,
    m_second_ (0)  {

}   

Sequential::Sequential (int32_t first_,int32_t second_):
    m_first_(first_), 
    m_second_(second_) {
}

void Sequential::swap(Sequential& other_)  noexcept 
{
    using std::swap;
    swap(m_first_, other_.m_first_);
    swap(m_second_, other_.m_second_);
}  

bool Sequential::operator == (const Sequential& other_) const {
    if (m_first_ != other_.m_first_) {
        return false;
    }
    if (m_second_ != other_.m_second_) {
        return false;
    }
    return true;
}

bool Sequential::operator != (const Sequential& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const Sequential& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "first: " << sample.first ()<<", ";
    o << "second: " << sample.second ();
    o <<"]";
    return o;
}

// ---- UsesInclude: 

UsesInclude::UsesInclude()  {

}   

UsesInclude::UsesInclude (const ::Included& included_):
    m_included_(included_) {
}

void UsesInclude::swap(UsesInclude& other_)  noexcept 
{
    using std::swap;
    swap(m_included_, other_.m_included_);
}  

bool UsesInclude::operator == (const UsesInclude& other_) const {
    if (m_included_ != other_.m_included_) {
        return false;
    }
    return true;
}

bool UsesInclude::operator != (const UsesInclude& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const UsesInclude& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "included: " << sample.included ();
    o <<"]";
    return o;
}

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
        struct native_type_code< ::AutoIds::Hashed > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Hashed_g_tc_members[2]=
                {

                    {
                        (char *)"first",/* Member name */
                        {
                            64291979,/* Representation ID */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"second",/* Member name */
                        {
                            182907049,/* Representation ID */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Hashed_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"AutoIds::Hashed", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Hashed_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Hashed*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Hashed_g_tc;
                }

                Hashed_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Hashed_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Hashed_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Hashed_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Hashed_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Hashed_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Hashed_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Hashed_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Hashed_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Hashed_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Hashed_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Hashed_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Hashed_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Hashed_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Hashed_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Hashed_g_tc._data._sampleAccessInfo = sample_access_info();
                Hashed_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Hashed_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::AutoIds::Hashed *sample;

                static RTIXCdrMemberAccessInfo Hashed_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Hashed_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Hashed_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::AutoIds::Hashed);
                if (sample == NULL) {
                    return NULL;
                }

                Hashed_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->first() - (char *)sample);

                Hashed_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->second() - (char *)sample);

                Hashed_g_sampleAccessInfo.memberAccessInfos = 
                Hashed_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::AutoIds::Hashed);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Hashed_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Hashed_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Hashed_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Hashed_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::AutoIds::Hashed >;

                Hashed_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Hashed_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Hashed_g_typePlugin = 
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

                return &Hashed_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::AutoIds::Hashed >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::AutoIds::Hashed >::get())));
        }

        template<>
        struct native_type_code< ::AutoIds::Reopened > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Reopened_g_tc_members[2]=
                {

                    {
                        (char *)"first",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"second",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Reopened_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"AutoIds::Reopened", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Reopened_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Reopened*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Reopened_g_tc;
                }

                Reopened_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Reopened_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Reopened_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Reopened_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Reopened_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Reopened_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Reopened_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Reopened_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Reopened_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Reopened_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Reopened_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Reopened_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Reopened_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Reopened_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Reopened_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Reopened_g_tc._data._sampleAccessInfo = sample_access_info();
                Reopened_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Reopened_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::AutoIds::Reopened *sample;

                static RTIXCdrMemberAccessInfo Reopened_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Reopened_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Reopened_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::AutoIds::Reopened);
                if (sample == NULL) {
                    return NULL;
                }

                Reopened_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->first() - (char *)sample);

                Reopened_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->second() - (char *)sample);

                Reopened_g_sampleAccessInfo.memberAccessInfos = 
                Reopened_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::AutoIds::Reopened);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Reopened_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Reopened_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Reopened_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Reopened_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::AutoIds::Reopened >;

                Reopened_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Reopened_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Reopened_g_typePlugin = 
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

                return &Reopened_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::AutoIds::Reopened >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::AutoIds::Reopened >::get())));
        }

        template<>
        struct native_type_code< ::Sequential > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sequential_g_tc_members[2]=
                {

                    {
                        (char *)"first",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"second",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Sequential_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"Sequential", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Sequential_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Sequential*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Sequential_g_tc;
                }

                Sequential_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sequential_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sequential_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Sequential_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sequential_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Sequential_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sequential_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sequential_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sequential_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Sequential_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sequential_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Sequential_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sequential_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sequential_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sequential_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Sequential_g_tc._data._sampleAccessInfo = sample_access_info();
                Sequential_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Sequential_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::Sequential *sample;

                static RTIXCdrMemberAccessInfo Sequential_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sequential_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sequential_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::Sequential);
                if (sample == NULL) {
                    return NULL;
                }

                Sequential_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->first() - (char *)sample);

                Sequential_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->second() - (char *)sample);

                Sequential_g_sampleAccessInfo.memberAccessInfos = 
                Sequential_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::Sequential);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Sequential_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Sequential_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Sequential_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Sequential_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::Sequential >;

                Sequential_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Sequential_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Sequential_g_typePlugin = 
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

                return &Sequential_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::Sequential >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Sequential >::get())));
        }

        template<>
        struct native_type_code< ::UsesInclude > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member UsesInclude_g_tc_members[1]=
                {

                    {
                        (char *)"included",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode UsesInclude_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"UsesInclude", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        UsesInclude_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for UsesInclude*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &UsesInclude_g_tc;
                }

                UsesInclude_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                UsesInclude_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Included>::get().native();

                /* Initialize the values for member annotations. */

                UsesInclude_g_tc._data._sampleAccessInfo = sample_access_info();
                UsesInclude_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &UsesInclude_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::UsesInclude *sample;

                static RTIXCdrMemberAccessInfo UsesInclude_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo UsesInclude_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &UsesInclude_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::UsesInclude);
                if (sample == NULL) {
                    return NULL;
                }

                UsesInclude_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->included() - (char *)sample);

                UsesInclude_g_sampleAccessInfo.memberAccessInfos = 
                UsesInclude_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::UsesInclude);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        UsesInclude_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        UsesInclude_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                UsesInclude_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                UsesInclude_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::UsesInclude >;

                UsesInclude_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &UsesInclude_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin UsesInclude_g_typePlugin = 
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

                return &UsesInclude_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::UsesInclude >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::UsesInclude >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::AutoIds::Hashed >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::AutoIds::HashedPlugin_new,
                ::AutoIds::HashedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::AutoIds::Hashed >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::AutoIds::Hashed& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = HashedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = HashedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::AutoIds::Hashed >::from_cdr_buffer(::AutoIds::Hashed& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = HashedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::AutoIds::Hashed from cdr buffer");
        }

        void topic_type_support< ::AutoIds::Hashed >::reset_sample(::AutoIds::Hashed& sample) 
        {
            sample.first(0);
            sample.second(0);
        }

        void topic_type_support< ::AutoIds::Hashed >::allocate_sample(::AutoIds::Hashed& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::AutoIds::Reopened >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::AutoIds::ReopenedPlugin_new,
                ::AutoIds::ReopenedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::AutoIds::Reopened >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::AutoIds::Reopened& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ReopenedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ReopenedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::AutoIds::Reopened >::from_cdr_buffer(::AutoIds::Reopened& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ReopenedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::AutoIds::Reopened from cdr buffer");
        }

        void topic_type_support< ::AutoIds::Reopened >::reset_sample(::AutoIds::Reopened& sample) 
        {
            sample.first(0);
            sample.second(0);
        }

        void topic_type_support< ::AutoIds::Reopened >::allocate_sample(::AutoIds::Reopened& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::Sequential >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::SequentialPlugin_new,
                ::SequentialPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::Sequential >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::Sequential& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SequentialPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SequentialPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::Sequential >::from_cdr_buffer(::Sequential& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SequentialPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::Sequential from cdr buffer");
        }

        void topic_type_support< ::Sequential >::reset_sample(::Sequential& sample) 
        {
            sample.first(0);
            sample.second(0);
        }

        void topic_type_support< ::Sequential >::allocate_sample(::Sequential& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::UsesInclude >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::UsesIncludePlugin_new,
                ::UsesIncludePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::UsesInclude >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::UsesInclude& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = UsesIncludePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = UsesIncludePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::UsesInclude >::from_cdr_buffer(::UsesInclude& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = UsesIncludePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::UsesInclude from cdr buffer");
        }

        void topic_type_support< ::UsesInclude >::reset_sample(::UsesInclude& sample) 
        {
            ::rti::topic::reset_sample(sample.included());
        }

        void topic_type_support< ::UsesInclude >::allocate_sample(::UsesInclude& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.included(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
