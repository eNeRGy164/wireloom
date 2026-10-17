

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-bounded-direct.idl
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
#include "07-union-string-sequence-bounded-directPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "07-union-string-sequence-bounded-direct.hpp"

#include <rti/util/ostream_operators.hpp>

namespace RuntimeUnionBoundedStringSequences {

    // ---- DirectBoundedStringSequences: 

    DirectBoundedStringSequences::Union_::Union_()  {
    }

    DirectBoundedStringSequences::Union_::Union_(const ::rti::core::bounded_sequence< std::string, 3L >& narrow_,const ::rti::core::bounded_sequence< std::wstring, 3L >& wide_):
        m_narrow_(narrow_), 
        m_wide_(wide_) {
    }

    DirectBoundedStringSequences::DirectBoundedStringSequences() :m_d_(default_discriminator())
    {
    }

    void DirectBoundedStringSequences::swap(DirectBoundedStringSequences& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.m_narrow_, other_.m_u_.m_narrow_);
            } break;
            case 1:
            {  
                swap(m_u_.m_wide_, other_.m_u_.m_wide_);
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
                    swap(m_u_.m_narrow_, other_.m_u_.m_narrow_);
                } break;
                case 1:
                {  
                    swap(m_u_.m_wide_, other_.m_u_.m_wide_);
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

    bool DirectBoundedStringSequences::operator == (const DirectBoundedStringSequences& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.m_narrow_ != other_.m_u_.m_narrow_) {
                    return false;
                }
            } break ;
            case 1:
            {  
                if (m_u_.m_wide_ != other_.m_u_.m_wide_) {
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

    bool DirectBoundedStringSequences::operator != (const DirectBoundedStringSequences& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const DirectBoundedStringSequences& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "narrow: " << sample.narrow ()<<", ";
            } break ;
            case 1:
            {  
                o << "wide: " << sample.wide ();
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

    int32_t DirectBoundedStringSequences::default_discriminator() {
        return 0;
    }

} // namespace RuntimeUnionBoundedStringSequences  

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
        struct native_type_code< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode DirectBoundedStringSequences_g_tc_narrow_string;
                static DDS_TypeCode DirectBoundedStringSequences_g_tc_narrow_sequence;
                static DDS_TypeCode DirectBoundedStringSequences_g_tc_wide_string;
                static DDS_TypeCode DirectBoundedStringSequences_g_tc_wide_sequence;

                static DDS_TypeCode_Member DirectBoundedStringSequences_g_tc_members[2]=
                {

                    {
                        (char *)"narrow",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"wide",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode DirectBoundedStringSequences_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        DirectBoundedStringSequences_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for DirectBoundedStringSequences*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &DirectBoundedStringSequences_g_tc;
                }

                DirectBoundedStringSequences_g_tc_narrow_string = initialize_string_typecode((8L));
                DirectBoundedStringSequences_g_tc_narrow_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< std::string, 3L > >((3L));
                DirectBoundedStringSequences_g_tc_wide_string = initialize_wstring_typecode((4L));
                DirectBoundedStringSequences_g_tc_wide_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< std::wstring, 3L > >((3L));

                DirectBoundedStringSequences_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                DirectBoundedStringSequences_g_tc_narrow_sequence._data._typeCode = (RTICdrTypeCode *)&DirectBoundedStringSequences_g_tc_narrow_string;
                DirectBoundedStringSequences_g_tc_wide_sequence._data._typeCode = (RTICdrTypeCode *)&DirectBoundedStringSequences_g_tc_wide_string;
                DirectBoundedStringSequences_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)& DirectBoundedStringSequences_g_tc_narrow_sequence;
                DirectBoundedStringSequences_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)& DirectBoundedStringSequences_g_tc_wide_sequence;

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                DirectBoundedStringSequences_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                DirectBoundedStringSequences_g_tc._data._sampleAccessInfo = sample_access_info();
                DirectBoundedStringSequences_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &DirectBoundedStringSequences_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences *sample;

                static RTIXCdrMemberAccessInfo DirectBoundedStringSequences_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo DirectBoundedStringSequences_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &DirectBoundedStringSequences_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences);
                if (sample == NULL) {
                    return NULL;
                }

                DirectBoundedStringSequences_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 0;
                DirectBoundedStringSequences_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->narrow() - (char *)sample);

                sample->_d() = 1;
                DirectBoundedStringSequences_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wide() - (char *)sample);

                DirectBoundedStringSequences_g_sampleAccessInfo.memberAccessInfos = 
                DirectBoundedStringSequences_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        DirectBoundedStringSequences_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        DirectBoundedStringSequences_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                DirectBoundedStringSequences_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                DirectBoundedStringSequences_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >;

                DirectBoundedStringSequences_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &DirectBoundedStringSequences_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin DirectBoundedStringSequences_g_typePlugin = 
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

                return &DirectBoundedStringSequences_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequencesPlugin_new,
                ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequencesPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = DirectBoundedStringSequencesPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = DirectBoundedStringSequencesPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >::from_cdr_buffer(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DirectBoundedStringSequencesPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences from cdr buffer");
        }

        void topic_type_support< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >::reset_sample(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample) 
        {
            sample._d() = 0;
            ::rti::topic::reset_sample(sample.narrow());
            sample._d() = 1;
            ::rti::topic::reset_sample(sample.wide());

            sample._d() = ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences::default_discriminator();
        }

        void topic_type_support< ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences >::allocate_sample(::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences& sample, int, int) 
        {
            sample._d() = 0;
            ::rti::topic::allocate_sample(sample.narrow(),  3L, 8L);
            sample._d() = 1;
            ::rti::topic::allocate_sample(sample.wide(),  3L, 4L);

            sample._d() = ::RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences::default_discriminator();
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
