

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

namespace CorpusStringAliasChains {

    // ---- StringAliasChainsSample: 

    StringAliasChainsSample::StringAliasChainsSample() :
        m_narrow_ ("") ,
        m_narrowChain_ ("") ,
        m_boundedNarrow_ ("") ,
        m_boundedNarrowChain_ ("") ,
        m_boundedNarrowChain2_ ("")  {

            m_wide_ = L"";
            m_wideChain_ = L"";
            m_boundedWide_ = L"";
            m_boundedWideChain_ = L"";
            m_boundedWideChain2_ = L"";
    }   

    StringAliasChainsSample::StringAliasChainsSample (const std::string& narrow_,const std::string& narrowChain_,const std::string& boundedNarrow_,const std::string& boundedNarrowChain_,const std::string& boundedNarrowChain2_,const std::wstring& wide_,const std::wstring& wideChain_,const std::wstring& boundedWide_,const std::wstring& boundedWideChain_,const std::wstring& boundedWideChain2_):
        m_narrow_(narrow_), 
        m_narrowChain_(narrowChain_), 
        m_boundedNarrow_(boundedNarrow_), 
        m_boundedNarrowChain_(boundedNarrowChain_), 
        m_boundedNarrowChain2_(boundedNarrowChain2_), 
        m_wide_(wide_), 
        m_wideChain_(wideChain_), 
        m_boundedWide_(boundedWide_), 
        m_boundedWideChain_(boundedWideChain_), 
        m_boundedWideChain2_(boundedWideChain2_) {
    }

    void StringAliasChainsSample::swap(StringAliasChainsSample& other_)  noexcept 
    {
        using std::swap;
        swap(m_narrow_, other_.m_narrow_);
        swap(m_narrowChain_, other_.m_narrowChain_);
        swap(m_boundedNarrow_, other_.m_boundedNarrow_);
        swap(m_boundedNarrowChain_, other_.m_boundedNarrowChain_);
        swap(m_boundedNarrowChain2_, other_.m_boundedNarrowChain2_);
        swap(m_wide_, other_.m_wide_);
        swap(m_wideChain_, other_.m_wideChain_);
        swap(m_boundedWide_, other_.m_boundedWide_);
        swap(m_boundedWideChain_, other_.m_boundedWideChain_);
        swap(m_boundedWideChain2_, other_.m_boundedWideChain2_);
    }  

    bool StringAliasChainsSample::operator == (const StringAliasChainsSample& other_) const {
        if (m_narrow_ != other_.m_narrow_) {
            return false;
        }
        if (m_narrowChain_ != other_.m_narrowChain_) {
            return false;
        }
        if (m_boundedNarrow_ != other_.m_boundedNarrow_) {
            return false;
        }
        if (m_boundedNarrowChain_ != other_.m_boundedNarrowChain_) {
            return false;
        }
        if (m_boundedNarrowChain2_ != other_.m_boundedNarrowChain2_) {
            return false;
        }
        if (m_wide_ != other_.m_wide_) {
            return false;
        }
        if (m_wideChain_ != other_.m_wideChain_) {
            return false;
        }
        if (m_boundedWide_ != other_.m_boundedWide_) {
            return false;
        }
        if (m_boundedWideChain_ != other_.m_boundedWideChain_) {
            return false;
        }
        if (m_boundedWideChain2_ != other_.m_boundedWideChain2_) {
            return false;
        }
        return true;
    }

    bool StringAliasChainsSample::operator != (const StringAliasChainsSample& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const StringAliasChainsSample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "narrow: " << sample.narrow ()<<", ";
        o << "narrowChain: " << sample.narrowChain ()<<", ";
        o << "boundedNarrow: " << sample.boundedNarrow ()<<", ";
        o << "boundedNarrowChain: " << sample.boundedNarrowChain ()<<", ";
        o << "boundedNarrowChain2: " << sample.boundedNarrowChain2 ()<<", ";
        o << "wide: " << sample.wide ()<<", ";
        o << "wideChain: " << sample.wideChain ()<<", ";
        o << "boundedWide: " << sample.boundedWide ()<<", ";
        o << "boundedWideChain: " << sample.boundedWideChain ()<<", ";
        o << "boundedWideChain2: " << sample.boundedWideChain2 ();
        o <<"]";
        return o;
    }

} // namespace CorpusStringAliasChains  

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
        struct native_type_code< ::CorpusStringAliasChains::StringAliasChainsSample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member StringAliasChainsSample_g_tc_members[10]=
                {

                    {
                        (char *)"narrow",/* Member name */
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
                        (char *)"narrowChain",/* Member name */
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
                        (char *)"boundedNarrow",/* Member name */
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
                        (char *)"boundedNarrowChain",/* Member name */
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
                        (char *)"boundedNarrowChain2",/* Member name */
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
                        (char *)"wide",/* Member name */
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
                        (char *)"wideChain",/* Member name */
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
                        (char *)"boundedWide",/* Member name */
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
                    }, 
                    {
                        (char *)"boundedWideChain",/* Member name */
                        {
                            8,/* Representation ID */
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
                        (char *)"boundedWideChain2",/* Member name */
                        {
                            9,/* Representation ID */
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

                static DDS_TypeCode StringAliasChainsSample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusStringAliasChains::StringAliasChainsSample", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        10, /* Number of members */
                        StringAliasChainsSample_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for StringAliasChainsSample*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &StringAliasChainsSample_g_tc;
                }

                StringAliasChainsSample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                StringAliasChainsSample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::NarrowText_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::NarrowTextChain_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedNarrowText_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedNarrowTextChain_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedNarrowTextChain2_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[5]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::WideText_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[6]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::WideTextChain_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[7]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedWideText_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[8]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedWideTextChain_AliasTag_t>::get().native();
                StringAliasChainsSample_g_tc_members[9]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusStringAliasChains::BoundedWideTextChain2_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                StringAliasChainsSample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringAliasChainsSample_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringAliasChainsSample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringAliasChainsSample_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringAliasChainsSample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringAliasChainsSample_g_tc_members[2]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringAliasChainsSample_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringAliasChainsSample_g_tc_members[3]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringAliasChainsSample_g_tc_members[4]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                StringAliasChainsSample_g_tc_members[4]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                StringAliasChainsSample_g_tc_members[5]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringAliasChainsSample_g_tc_members[5]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";StringAliasChainsSample_g_tc_members[6]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringAliasChainsSample_g_tc_members[6]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";StringAliasChainsSample_g_tc_members[7]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringAliasChainsSample_g_tc_members[7]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";StringAliasChainsSample_g_tc_members[8]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringAliasChainsSample_g_tc_members[8]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";StringAliasChainsSample_g_tc_members[9]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                StringAliasChainsSample_g_tc_members[9]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";

                StringAliasChainsSample_g_tc._data._sampleAccessInfo = sample_access_info();
                StringAliasChainsSample_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &StringAliasChainsSample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusStringAliasChains::StringAliasChainsSample *sample;

                static RTIXCdrMemberAccessInfo StringAliasChainsSample_g_memberAccessInfos[10] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo StringAliasChainsSample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &StringAliasChainsSample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusStringAliasChains::StringAliasChainsSample);
                if (sample == NULL) {
                    return NULL;
                }

                StringAliasChainsSample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->narrow() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->narrowChain() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedNarrow() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedNarrowChain() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedNarrowChain2() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[5].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wide() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[6].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wideChain() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[7].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedWide() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[8].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedWideChain() - (char *)sample);

                StringAliasChainsSample_g_memberAccessInfos[9].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedWideChain2() - (char *)sample);

                StringAliasChainsSample_g_sampleAccessInfo.memberAccessInfos = 
                StringAliasChainsSample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusStringAliasChains::StringAliasChainsSample);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        StringAliasChainsSample_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        StringAliasChainsSample_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                StringAliasChainsSample_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                StringAliasChainsSample_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusStringAliasChains::StringAliasChainsSample >;

                StringAliasChainsSample_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &StringAliasChainsSample_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin StringAliasChainsSample_g_typePlugin = 
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

                return &StringAliasChainsSample_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusStringAliasChains::StringAliasChainsSample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusStringAliasChains::StringAliasChainsSample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusStringAliasChains::StringAliasChainsSample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusStringAliasChains::StringAliasChainsSamplePlugin_new,
                ::CorpusStringAliasChains::StringAliasChainsSamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusStringAliasChains::StringAliasChainsSample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusStringAliasChains::StringAliasChainsSample& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = StringAliasChainsSamplePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = StringAliasChainsSamplePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusStringAliasChains::StringAliasChainsSample >::from_cdr_buffer(::CorpusStringAliasChains::StringAliasChainsSample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = StringAliasChainsSamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusStringAliasChains::StringAliasChainsSample from cdr buffer");
        }

        void topic_type_support< ::CorpusStringAliasChains::StringAliasChainsSample >::reset_sample(::CorpusStringAliasChains::StringAliasChainsSample& sample) 
        {
            sample.narrow("");
            sample.narrowChain("");
            sample.boundedNarrow("");
            sample.boundedNarrowChain("");
            sample.boundedNarrowChain2("");
            sample.wide(L"");
            sample.wideChain(L"");
            sample.boundedWide(L"");
            sample.boundedWideChain(L"");
            sample.boundedWideChain2(L"");
        }

        void topic_type_support< ::CorpusStringAliasChains::StringAliasChainsSample >::allocate_sample(::CorpusStringAliasChains::StringAliasChainsSample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.narrow(),  -1, 255L);
            ::rti::topic::allocate_sample(sample.narrowChain(),  -1, 255L);
            ::rti::topic::allocate_sample(sample.boundedNarrow(),  -1, 16L);
            ::rti::topic::allocate_sample(sample.boundedNarrowChain(),  -1, 16L);
            ::rti::topic::allocate_sample(sample.boundedNarrowChain2(),  -1, 16L);
            ::rti::topic::allocate_sample(sample.wide(),  -1, 255L);
            ::rti::topic::allocate_sample(sample.wideChain(),  -1, 255L);
            ::rti::topic::allocate_sample(sample.boundedWide(),  -1, 8L);
            ::rti::topic::allocate_sample(sample.boundedWideChain(),  -1, 8L);
            ::rti::topic::allocate_sample(sample.boundedWideChain2(),  -1, 8L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
