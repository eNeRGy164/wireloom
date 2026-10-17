

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-keys.idl
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
#include "08-keysPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "08-keys.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusKeys {

    // ---- Simple: 

    Simple::Simple() :
        m_id_ (0) ,
        m_name_ ("")  {

    }   

    Simple::Simple (int32_t id_,const std::string& name_):
        m_id_(id_), 
        m_name_(name_) {
    }

    void Simple::swap(Simple& other_)  noexcept 
    {
        using std::swap;
        swap(m_id_, other_.m_id_);
        swap(m_name_, other_.m_name_);
    }  

    bool Simple::operator == (const Simple& other_) const {
        if (m_id_ != other_.m_id_) {
            return false;
        }
        if (m_name_ != other_.m_name_) {
            return false;
        }
        return true;
    }

    bool Simple::operator != (const Simple& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Simple& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id ()<<", ";
        o << "name: " << sample.name ();
        o <<"]";
        return o;
    }

    // ---- Identity: 

    Identity::Identity() :
        m_tenant_ (0) ,
        m_name_ ("")  {

    }   

    Identity::Identity (int32_t tenant_,const std::string& name_):
        m_tenant_(tenant_), 
        m_name_(name_) {
    }

    void Identity::swap(Identity& other_)  noexcept 
    {
        using std::swap;
        swap(m_tenant_, other_.m_tenant_);
        swap(m_name_, other_.m_name_);
    }  

    bool Identity::operator == (const Identity& other_) const {
        if (m_tenant_ != other_.m_tenant_) {
            return false;
        }
        if (m_name_ != other_.m_name_) {
            return false;
        }
        return true;
    }

    bool Identity::operator != (const Identity& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Identity& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "tenant: " << sample.tenant ()<<", ";
        o << "name: " << sample.name ();
        o <<"]";
        return o;
    }

    // ---- Sample: 

    Sample::Sample() :
        m_payload_ (0)  {

    }   

    Sample::Sample (const ::CorpusKeys::Identity& identity_,int32_t payload_):
        m_identity_(identity_), 
        m_payload_(payload_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_identity_, other_.m_identity_);
        swap(m_payload_, other_.m_payload_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_identity_ != other_.m_identity_) {
            return false;
        }
        if (m_payload_ != other_.m_payload_) {
            return false;
        }
        return true;
    }

    bool Sample::operator != (const Sample& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "identity: " << sample.identity ()<<", ";
        o << "payload: " << sample.payload ();
        o <<"]";
        return o;
    }

} // namespace CorpusKeys  

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
        struct native_type_code< ::CorpusKeys::Simple > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Simple_g_tc_name_string;

                static DDS_TypeCode_Member Simple_g_tc_members[2]=
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Simple_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusKeys::Simple", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Simple_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Simple*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Simple_g_tc;
                }

                Simple_g_tc_name_string = initialize_string_typecode((16L));

                Simple_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Simple_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Simple_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&Simple_g_tc_name_string;

                /* Initialize the values for member annotations. */
                Simple_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Simple_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Simple_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Simple_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Simple_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Simple_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Simple_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Simple_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Simple_g_tc._data._sampleAccessInfo = sample_access_info();
                Simple_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Simple_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusKeys::Simple *sample;

                static RTIXCdrMemberAccessInfo Simple_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Simple_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Simple_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusKeys::Simple);
                if (sample == NULL) {
                    return NULL;
                }

                Simple_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->id() - (char *)sample);

                Simple_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->name() - (char *)sample);

                Simple_g_sampleAccessInfo.memberAccessInfos = 
                Simple_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusKeys::Simple);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Simple_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Simple_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Simple_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Simple_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusKeys::Simple >;

                Simple_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Simple_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Simple_g_typePlugin = 
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

                return &Simple_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusKeys::Simple >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusKeys::Simple >::get())));
        }

        template<>
        struct native_type_code< ::CorpusKeys::Identity > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Identity_g_tc_name_string;

                static DDS_TypeCode_Member Identity_g_tc_members[2]=
                {

                    {
                        (char *)"tenant",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Identity_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusKeys::Identity", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Identity_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Identity*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Identity_g_tc;
                }

                Identity_g_tc_name_string = initialize_string_typecode((16L));

                Identity_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Identity_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Identity_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&Identity_g_tc_name_string;

                /* Initialize the values for member annotations. */
                Identity_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Identity_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Identity_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Identity_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Identity_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Identity_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Identity_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Identity_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Identity_g_tc._data._sampleAccessInfo = sample_access_info();
                Identity_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Identity_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusKeys::Identity *sample;

                static RTIXCdrMemberAccessInfo Identity_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Identity_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Identity_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusKeys::Identity);
                if (sample == NULL) {
                    return NULL;
                }

                Identity_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->tenant() - (char *)sample);

                Identity_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->name() - (char *)sample);

                Identity_g_sampleAccessInfo.memberAccessInfos = 
                Identity_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusKeys::Identity);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Identity_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Identity_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Identity_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Identity_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusKeys::Identity >;

                Identity_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Identity_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Identity_g_typePlugin = 
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

                return &Identity_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusKeys::Identity >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusKeys::Identity >::get())));
        }

        template<>
        struct native_type_code< ::CorpusKeys::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sample_g_tc_members[2]=
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusKeys::Sample", /* Name */
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

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusKeys::Identity>::get().native();
                Sample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
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

                ::CorpusKeys::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusKeys::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->identity() - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->payload() - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos = 
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusKeys::Sample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Sample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Sample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Sample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Sample_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusKeys::Sample >;

                Sample_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusKeys::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusKeys::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusKeys::Simple >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusKeys::SimplePlugin_new,
                ::CorpusKeys::SimplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusKeys::Simple >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusKeys::Simple& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SimplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SimplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusKeys::Simple >::from_cdr_buffer(::CorpusKeys::Simple& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SimplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusKeys::Simple from cdr buffer");
        }

        void topic_type_support< ::CorpusKeys::Simple >::reset_sample(::CorpusKeys::Simple& sample) 
        {
            sample.id(0);
            sample.name("");
        }

        void topic_type_support< ::CorpusKeys::Simple >::allocate_sample(::CorpusKeys::Simple& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.name(),  -1, 16L);
        }
        void topic_type_support< ::CorpusKeys::Identity >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusKeys::IdentityPlugin_new,
                ::CorpusKeys::IdentityPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusKeys::Identity >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusKeys::Identity& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = IdentityPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = IdentityPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusKeys::Identity >::from_cdr_buffer(::CorpusKeys::Identity& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = IdentityPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusKeys::Identity from cdr buffer");
        }

        void topic_type_support< ::CorpusKeys::Identity >::reset_sample(::CorpusKeys::Identity& sample) 
        {
            sample.tenant(0);
            sample.name("");
        }

        void topic_type_support< ::CorpusKeys::Identity >::allocate_sample(::CorpusKeys::Identity& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.name(),  -1, 16L);
        }
        void topic_type_support< ::CorpusKeys::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusKeys::SamplePlugin_new,
                ::CorpusKeys::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusKeys::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusKeys::Sample& sample,
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

        void topic_type_support< ::CorpusKeys::Sample >::from_cdr_buffer(::CorpusKeys::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusKeys::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusKeys::Sample >::reset_sample(::CorpusKeys::Sample& sample) 
        {
            ::rti::topic::reset_sample(sample.identity());
            sample.payload(0);
        }

        void topic_type_support< ::CorpusKeys::Sample >::allocate_sample(::CorpusKeys::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.identity(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
