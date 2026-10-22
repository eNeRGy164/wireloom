

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-array-branch.idl
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
#include "14-union-array-branchPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-union-array-branch.hpp"

#include <rti/util/ostream_operators.hpp>

// ---- ArrayBranch:

ArrayBranch::Union_::Union_() :
    scalar (0)  {
}

ArrayBranch::Union_::Union_(const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values_,int32_t scalar_):
    values(values_), 
    scalar(scalar_) {
}
ArrayBranch::ArrayBranch() :m_d_(default_discriminator())
{
}

void ArrayBranch::swap(ArrayBranch& other_)  noexcept 
{
    using std::swap;
    swap (m_d_,other_.m_d_);
    switch (::rti::topic::cdr::integer_case(_d())) {
        case 0:
        {  
            swap(m_u_.values, other_.m_u_.values);
        } break;
        case 1:
        {  
            swap(m_u_.scalar, other_.m_u_.scalar);
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
            case 0:
            {  
                swap(m_u_.values, other_.m_u_.values);
            } break;
            case 1:
            {  
                swap(m_u_.scalar, other_.m_u_.scalar);
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

bool ArrayBranch::operator == (const ArrayBranch& other_) const {
    if (_d() != other_._d()){
        return false;
    }
    switch(::rti::topic::cdr::integer_case(_d())){
        case 0:
        {  
            if (m_u_.values != other_.m_u_.values) {
                return false;
            }
        } break ;
        case 1:
        {  
            if (m_u_.scalar != other_.m_u_.scalar) {
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

bool ArrayBranch::operator != (const ArrayBranch& other_) const {
    return !this->operator ==(other_);
}

std::ostream& operator << (std::ostream& o,const ArrayBranch& sample)
{
    ::rti::util::StreamFlagSaver flag_saver (o);
    o <<"[";
    o << "_d: " << sample._d() <<", ";
    switch(::rti::topic::cdr::integer_case(sample._d())){
        case 0:
        {  
            o << "values: " << sample.values()<<", ";
        } break ;
        case 1:
        {  
            o << "scalar: " << sample.scalar();
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

int32_t ArrayBranch::default_discriminator() {
    return 0;
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
        struct native_type_code< ::ArrayBranch > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};
                static DDS_UnsignedLong ArrayBranch_g_tc_values_dimensions [2] = {2L,3L} ;

                static DDS_TypeCode ArrayBranch_g_tc_values_array =DDS_INITIALIZE_ARRAY_TYPECODE(2,2L,ArrayBranch_g_tc_values_dimensions,NULL);

                static DDS_TypeCode_Member ArrayBranch_g_tc_members[2]=
                {

                    {
                        (char *)"values",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(0),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"scalar",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(1),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode ArrayBranch_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"ArrayBranch", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        ArrayBranch_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for ArrayBranch*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &ArrayBranch_g_tc;
                }

                ArrayBranch_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                ArrayBranch_g_tc_values_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                ArrayBranch_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)& ArrayBranch_g_tc_values_array;
                ArrayBranch_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                ArrayBranch_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                ArrayBranch_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                ArrayBranch_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                ArrayBranch_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                ArrayBranch_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                ArrayBranch_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                ArrayBranch_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                ArrayBranch_g_tc._data._sampleAccessInfo = sample_access_info();
                ArrayBranch_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &ArrayBranch_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::ArrayBranch *sample;

                static RTIXCdrMemberAccessInfo ArrayBranch_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo ArrayBranch_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &ArrayBranch_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::ArrayBranch);
                if (sample == NULL) {
                    return NULL;
                }

                ArrayBranch_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(0);
                ArrayBranch_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->values() - (char *)sample);

                sample->_d(1);
                ArrayBranch_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->scalar() - (char *)sample);

                ArrayBranch_g_sampleAccessInfo.memberAccessInfos =
                ArrayBranch_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::ArrayBranch);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        ArrayBranch_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        ArrayBranch_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                ArrayBranch_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                ArrayBranch_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::ArrayBranch >;

                ArrayBranch_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &ArrayBranch_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin ArrayBranch_g_typePlugin =
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

                return &ArrayBranch_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::ArrayBranch >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::ArrayBranch >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::ArrayBranch >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::ArrayBranchPlugin_new,
                ::ArrayBranchPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::ArrayBranch >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::ArrayBranch& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ArrayBranchPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ArrayBranchPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::ArrayBranch >::from_cdr_buffer(::ArrayBranch& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ArrayBranchPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::ArrayBranch from cdr buffer");
        }

        void topic_type_support< ::ArrayBranch >::reset_sample(::ArrayBranch& sample) 
        {
            sample._d(0);
            ::rti::topic::reset_sample(sample.values());
            sample._d(1);
            sample.scalar(0);

            sample._d(::ArrayBranch::default_discriminator());
        }

        void topic_type_support< ::ArrayBranch >::allocate_sample(::ArrayBranch& sample, int, int) 
        {
            sample._d(0);
            ::rti::topic::allocate_sample(sample.values(),  -1, -1);

            sample._d(::ArrayBranch::default_discriminator());
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
