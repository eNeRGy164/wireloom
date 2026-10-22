

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-scoped-enum.idl
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
#include "14-union-scoped-enumPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-union-scoped-enum.hpp"

#include <rti/util/ostream_operators.hpp>

std::ostream& operator << (std::ostream& o,const Choice& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    switch(sample){
        case Choice::FIRST:
        o << "Choice::FIRST" << " ";
        break;
        case Choice::SECOND:
        o << "Choice::SECOND" << " ";
        break;
        default:
        {
            /* Prevents compiler warnings */
        }
    }
    return o;
}

// ---- ScopedEnum:

ScopedEnum::Union_::Union_() :
    selected (0) ,
    first (0)  {
}

ScopedEnum::Union_::Union_(int32_t selected_,int32_t first_):
    selected(selected_), 
    first(first_) {
}
ScopedEnum::ScopedEnum() :m_d_(default_discriminator())
{
}

void ScopedEnum::swap(ScopedEnum& other_)  noexcept 
{
    using std::swap;
    swap (m_d_,other_.m_d_);
    switch (::rti::topic::cdr::integer_case(_d())) {
        case Selected:
        {  
            swap(m_u_.selected, other_.m_u_.selected);
        } break;
        case (Choice::FIRST):
            {  
                swap(m_u_.first, other_.m_u_.first);
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
            case Selected:
            {  
                swap(m_u_.selected, other_.m_u_.selected);
            } break;
            case (Choice::FIRST):
                {  
                    swap(m_u_.first, other_.m_u_.first);
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

bool ScopedEnum::operator == (const ScopedEnum& other_) const {
    if (_d() != other_._d()){
        return false;
    }
    switch(::rti::topic::cdr::integer_case(_d())){
        case Selected:
        {  
            if (m_u_.selected != other_.m_u_.selected) {
                return false;
            }
        } break ;
        case (Choice::FIRST):
            {  
                if (m_u_.first != other_.m_u_.first) {
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

bool ScopedEnum::operator != (const ScopedEnum& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const ScopedEnum& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "_d: " << sample._d() <<", ";
    switch(::rti::topic::cdr::integer_case(sample._d())){
        case Selected:
        {  
            o << "selected: " << sample.selected()<<", ";
        } break ;
        case (Choice::FIRST):
            {  
                o << "first: " << sample.first();
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

::Choice ScopedEnum::default_discriminator() {
    return ::Choice(static_cast< ::Choice >(0));
}

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::Choice default_enumerator< ::Choice>::value = ::Choice::FIRST;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::Choice default_enumerator< ::Choice>::value = ::Choice::FIRST;

        template<>
        struct native_type_code< ::Choice > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Choice_g_tc_members[2]=
                {

                    {
                        (char *)"FIRST",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::Choice::FIRST),
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"SECOND",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::Choice::SECOND),
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Choice_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"Choice", /* Name */
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

                Choice_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                /* Initialize the values for annotations. */
                Choice_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Choice_g_tc._data._annotations._defaultValue._u.long_value = 0;

                Choice_g_tc._data._sampleAccessInfo = sample_access_info();
                Choice_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Choice_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Choice_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Choice_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Choice_g_sampleAccessInfo;
                }

                Choice_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Choice_g_sampleAccessInfo.memberAccessInfos =
                Choice_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::Choice);

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
                interpreter::get_aggregation_value_pointer< ::Choice >;

                Choice_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

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

        const ::dds::core::xtypes::EnumType& dynamic_type< ::Choice >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::Choice >::get())));
        }

        template<>
        struct native_type_code< ::ScopedEnum > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member ScopedEnum_g_tc_members[2]=
                {

                    {
                        (char *)"selected",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(Selected),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"first",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>((Choice::FIRST)),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode ScopedEnum_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"ScopedEnum", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        ScopedEnum_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ScopedEnum*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ScopedEnum_g_tc;
                }

                ScopedEnum_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ScopedEnum_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                ScopedEnum_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                ScopedEnum_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ScopedEnum_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                ScopedEnum_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ScopedEnum_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ScopedEnum_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ScopedEnum_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                ScopedEnum_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ScopedEnum_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                ScopedEnum_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ScopedEnum_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ScopedEnum_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ScopedEnum_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                ScopedEnum_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::Choice>::get().native();

                ScopedEnum_g_tc._data._sampleAccessInfo = sample_access_info();
                ScopedEnum_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &ScopedEnum_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::ScopedEnum *sample;

                static RTIXCdrMemberAccessInfo ScopedEnum_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ScopedEnum_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ScopedEnum_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::ScopedEnum);
                if (sample == NULL) {
                    return NULL;
                }

                ScopedEnum_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(Selected);
                ScopedEnum_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->selected() - (char *)sample);

                sample->_d((Choice::FIRST));
                ScopedEnum_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->first() - (char *)sample);

                ScopedEnum_g_sampleAccessInfo.memberAccessInfos =
                ScopedEnum_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::ScopedEnum);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ScopedEnum_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ScopedEnum_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ScopedEnum_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ScopedEnum_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::ScopedEnum >;

                ScopedEnum_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ScopedEnum_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ScopedEnum_g_typePlugin =
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

                return &ScopedEnum_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::ScopedEnum >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::ScopedEnum >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::ScopedEnum >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::ScopedEnumPlugin_new,
                ::ScopedEnumPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::ScopedEnum >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::ScopedEnum& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ScopedEnumPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ScopedEnumPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::ScopedEnum >::from_cdr_buffer(::ScopedEnum& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ScopedEnumPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::ScopedEnum from cdr buffer");
        }

        void topic_type_support< ::ScopedEnum >::reset_sample(::ScopedEnum& sample) 
        {
            sample._d(Selected);
            sample.selected(0);
            sample._d((Choice::FIRST));
            sample.first(0);

            sample._d(::ScopedEnum::default_discriminator());
        }

        void topic_type_support< ::ScopedEnum >::allocate_sample(::ScopedEnum& sample, int, int) 
        {

            sample._d(::ScopedEnum::default_discriminator());
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
