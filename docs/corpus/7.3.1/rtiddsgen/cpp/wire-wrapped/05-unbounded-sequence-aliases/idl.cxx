

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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
#include "idlPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "idl.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusUnboundedSequences {

    // ---- SequenceAliasesSample: 

    SequenceAliasesSample::SequenceAliasesSample()  {

    }   

    SequenceAliasesSample::SequenceAliasesSample (const ::CorpusUnboundedSequences::Numbers& numbers_,const ::CorpusUnboundedSequences::States& states_,const ::CorpusUnboundedSequences::Children& children_,const ::CorpusUnboundedSequences::Scalars& scalars_,const ::CorpusUnboundedSequences::Texts& texts_,const ::CorpusUnboundedSequences::WideTexts& wideTexts_,const ::CorpusUnboundedSequences::BoundedTexts& boundedTexts_,const ::CorpusUnboundedSequences::BoundedWideTexts& boundedWideTexts_):
        m_numbers_(numbers_), 
        m_states_(states_), 
        m_children_(children_), 
        m_scalars_(scalars_), 
        m_texts_(texts_), 
        m_wideTexts_(wideTexts_), 
        m_boundedTexts_(boundedTexts_), 
        m_boundedWideTexts_(boundedWideTexts_) {
    }

    void SequenceAliasesSample::swap(SequenceAliasesSample& other_)  noexcept 
    {
        using std::swap;
        swap(m_numbers_, other_.m_numbers_);
        swap(m_states_, other_.m_states_);
        swap(m_children_, other_.m_children_);
        swap(m_scalars_, other_.m_scalars_);
        swap(m_texts_, other_.m_texts_);
        swap(m_wideTexts_, other_.m_wideTexts_);
        swap(m_boundedTexts_, other_.m_boundedTexts_);
        swap(m_boundedWideTexts_, other_.m_boundedWideTexts_);
    }  

    bool SequenceAliasesSample::operator == (const SequenceAliasesSample& other_) const {
        if (m_numbers_ != other_.m_numbers_) {
            return false;
        }
        if (m_states_ != other_.m_states_) {
            return false;
        }
        if (m_children_ != other_.m_children_) {
            return false;
        }
        if (m_scalars_ != other_.m_scalars_) {
            return false;
        }
        if (m_texts_ != other_.m_texts_) {
            return false;
        }
        if (m_wideTexts_ != other_.m_wideTexts_) {
            return false;
        }
        if (m_boundedTexts_ != other_.m_boundedTexts_) {
            return false;
        }
        if (m_boundedWideTexts_ != other_.m_boundedWideTexts_) {
            return false;
        }
        return true;
    }

    bool SequenceAliasesSample::operator != (const SequenceAliasesSample& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const SequenceAliasesSample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "numbers: " << sample.numbers ()<<", ";
        o << "states: " << sample.states ()<<", ";
        o << "children: " << sample.children ()<<", ";
        o << "scalars: " << sample.scalars ()<<", ";
        o << "texts: " << sample.texts ()<<", ";
        o << "wideTexts: " << sample.wideTexts ()<<", ";
        o << "boundedTexts: " << sample.boundedTexts ()<<", ";
        o << "boundedWideTexts: " << sample.boundedWideTexts ();
        o <<"]";
        return o;
    }

} // namespace CorpusUnboundedSequences  

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
        struct native_type_code< ::CorpusUnboundedSequences::SequenceAliasesSample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member SequenceAliasesSample_g_tc_members[8]=
                {

                    {
                        (char *)"numbers",/* Member name */
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
                        (char *)"states",/* Member name */
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
                    }, 
                    {
                        (char *)"children",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"scalars",/* Member name */
                        {
                            3,/* Representation ID */
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
                        (char *)"texts",/* Member name */
                        {
                            4,/* Representation ID */
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
                        (char *)"wideTexts",/* Member name */
                        {
                            5,/* Representation ID */
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
                        (char *)"boundedTexts",/* Member name */
                        {
                            6,/* Representation ID */
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
                        (char *)"boundedWideTexts",/* Member name */
                        {
                            7,/* Representation ID */
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

                static DDS_TypeCode SequenceAliasesSample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnboundedSequences::SequenceAliasesSample", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        8, /* Number of members */
                        SequenceAliasesSample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for SequenceAliasesSample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &SequenceAliasesSample_g_tc;
                }

                SequenceAliasesSample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                SequenceAliasesSample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::NumbersChain_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::States_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::Children_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::Scalars_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::Texts_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[5]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::WideTexts_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[6]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::BoundedTexts_AliasTag_t>::get().native();
                SequenceAliasesSample_g_tc_members[7]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::BoundedWideTexts_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                SequenceAliasesSample_g_tc._data._sampleAccessInfo = sample_access_info();
                SequenceAliasesSample_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &SequenceAliasesSample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnboundedSequences::SequenceAliasesSample *sample;

                static RTIXCdrMemberAccessInfo SequenceAliasesSample_g_memberAccessInfos[8] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo SequenceAliasesSample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &SequenceAliasesSample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusUnboundedSequences::SequenceAliasesSample);
                if (sample == NULL) {
                    return NULL;
                }

                SequenceAliasesSample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->numbers() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->states() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->children() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->scalars() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->texts() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[5].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wideTexts() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[6].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedTexts() - (char *)sample);

                SequenceAliasesSample_g_memberAccessInfos[7].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedWideTexts() - (char *)sample);

                SequenceAliasesSample_g_sampleAccessInfo.memberAccessInfos = 
                SequenceAliasesSample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::SequenceAliasesSample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        SequenceAliasesSample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        SequenceAliasesSample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                SequenceAliasesSample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                SequenceAliasesSample_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::SequenceAliasesSample >;

                SequenceAliasesSample_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &SequenceAliasesSample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin SequenceAliasesSample_g_typePlugin = 
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

                return &SequenceAliasesSample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnboundedSequences::SequenceAliasesSample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::SequenceAliasesSample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnboundedSequences::SequenceAliasesSample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnboundedSequences::SequenceAliasesSamplePlugin_new,
                ::CorpusUnboundedSequences::SequenceAliasesSamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnboundedSequences::SequenceAliasesSample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnboundedSequences::SequenceAliasesSample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = SequenceAliasesSamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = SequenceAliasesSamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusUnboundedSequences::SequenceAliasesSample >::from_cdr_buffer(::CorpusUnboundedSequences::SequenceAliasesSample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SequenceAliasesSamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnboundedSequences::SequenceAliasesSample from cdr buffer");
        }

        void topic_type_support< ::CorpusUnboundedSequences::SequenceAliasesSample >::reset_sample(::CorpusUnboundedSequences::SequenceAliasesSample& sample) 
        {
            ::rti::topic::reset_sample(sample.numbers());
            ::rti::topic::reset_sample(sample.states());
            ::rti::topic::reset_sample(sample.children());
            ::rti::topic::reset_sample(sample.scalars());
            ::rti::topic::reset_sample(sample.texts());
            ::rti::topic::reset_sample(sample.wideTexts());
            ::rti::topic::reset_sample(sample.boundedTexts());
            ::rti::topic::reset_sample(sample.boundedWideTexts());
        }

        void topic_type_support< ::CorpusUnboundedSequences::SequenceAliasesSample >::allocate_sample(::CorpusUnboundedSequences::SequenceAliasesSample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.numbers(),  -1, -1);
            ::rti::topic::allocate_sample(sample.states(),  -1, -1);
            ::rti::topic::allocate_sample(sample.children(),  -1, -1);
            ::rti::topic::allocate_sample(sample.scalars(),  -1, -1);
            ::rti::topic::allocate_sample(sample.texts(),  -1, -1);
            ::rti::topic::allocate_sample(sample.wideTexts(),  -1, -1);
            ::rti::topic::allocate_sample(sample.boundedTexts(),  -1, -1);
            ::rti::topic::allocate_sample(sample.boundedWideTexts(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
