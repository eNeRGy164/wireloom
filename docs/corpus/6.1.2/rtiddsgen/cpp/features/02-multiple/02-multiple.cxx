

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-multiple.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>

#include "rti/topic/cdr/Serialization.hpp"

#include "02-multiple.hpp"
#include "02-multiplePlugin.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- First: 

First::First() :
    m_value_ (0)  {
}   

First::First (int32_t value):
    m_value_( value ) {
}

#ifdef RTI_CXX11_RVALUE_REFERENCES
#ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
First::First(First&& other_) OMG_NOEXCEPT  :m_value_ (std::move(other_.m_value_))
{
} 

First& First::operator=(First&&  other_) OMG_NOEXCEPT {
    First tmp(std::move(other_));
    swap(tmp); 
    return *this;
}
#endif
#endif   

void First::swap(First& other_)  OMG_NOEXCEPT 
{
    using std::swap;
    swap(m_value_, other_.m_value_);
}  

bool First::operator == (const First& other_) const {
    if (m_value_ != other_.m_value_) {
        return false;
    }
    return true;
}
bool First::operator != (const First& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const First& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "value: " << sample.value() ;
    o <<"]";
    return o;
}

// ---- Second: 

Second::Second() :
    m_name_ ("")  {
}   

Second::Second (const std::string& name):
    m_name_( name ) {
}

#ifdef RTI_CXX11_RVALUE_REFERENCES
#ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
Second::Second(Second&& other_) OMG_NOEXCEPT  :m_name_ (std::move(other_.m_name_))
{
} 

Second& Second::operator=(Second&&  other_) OMG_NOEXCEPT {
    Second tmp(std::move(other_));
    swap(tmp); 
    return *this;
}
#endif
#endif   

void Second::swap(Second& other_)  OMG_NOEXCEPT 
{
    using std::swap;
    swap(m_name_, other_.m_name_);
}  

bool Second::operator == (const Second& other_) const {
    if (m_name_ != other_.m_name_) {
        return false;
    }
    return true;
}
bool Second::operator != (const Second& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const Second& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "name: " << sample.name() ;
    o <<"]";
    return o;
}

// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< First > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member First_g_tc_members[1]=
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode First_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"First", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        First_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for First*/

                if (is_initialized) {
                    return &First_g_tc;
                }

                First_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                First_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                First_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                First_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                First_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                First_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                First_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                First_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                First_g_tc._data._sampleAccessInfo = sample_access_info();
                First_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &First_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                First *sample;

                static RTIXCdrMemberAccessInfo First_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo First_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &First_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    First);
                if (sample == NULL) {
                    return NULL;
                }

                First_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                First_g_sampleAccessInfo.memberAccessInfos = 
                First_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(First);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        First_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        First_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                First_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                First_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< First >;

                First_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &First_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin First_g_typePlugin = 
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
                    NULL
                };

                return &First_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< First >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< First >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< Second > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode Second_g_tc_name_string;

                static DDS_TypeCode_Member Second_g_tc_members[1]=
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
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Second_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"Second", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Second_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Second*/

                if (is_initialized) {
                    return &Second_g_tc;
                }

                Second_g_tc_name_string = initialize_string_typecode((255L));

                Second_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Second_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&Second_g_tc_name_string;

                /* Initialize the values for member annotations. */
                Second_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Second_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Second_g_tc._data._sampleAccessInfo = sample_access_info();
                Second_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Second_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                Second *sample;

                static RTIXCdrMemberAccessInfo Second_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Second_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Second_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    Second);
                if (sample == NULL) {
                    return NULL;
                }

                Second_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->name() - (char *)sample);

                Second_g_sampleAccessInfo.memberAccessInfos = 
                Second_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(Second);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Second_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Second_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Second_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Second_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< Second >;

                Second_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &Second_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Second_g_typePlugin = 
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
                    NULL
                };

                return &Second_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< Second >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< Second >::get())));
        }

    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< First >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                FirstPlugin_new,
                FirstPlugin_delete);
        }

        std::vector<char>& topic_type_support< First >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const First& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = FirstPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = FirstPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< First >::from_cdr_buffer(First& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = FirstPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create First from cdr buffer");
        }

        void topic_type_support< First >::reset_sample(First& sample) 
        {
            sample.value(0);
        }

        void topic_type_support< First >::allocate_sample(First& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< Second >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                SecondPlugin_new,
                SecondPlugin_delete);
        }

        std::vector<char>& topic_type_support< Second >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const Second& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SecondPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SecondPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< Second >::from_cdr_buffer(Second& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SecondPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create Second from cdr buffer");
        }

        void topic_type_support< Second >::reset_sample(Second& sample) 
        {
            sample.name("");
        }

        void topic_type_support< Second >::allocate_sample(Second& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.name(),  -1, 255L);
        }

    }
}  

