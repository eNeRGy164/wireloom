

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-receive-lifecycle.idl
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
#include "14-unbounded-receive-lifecyclePlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "14-unbounded-receive-lifecycle.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusUnboundedReceiveLifecycle {

    // ---- OptionalState: 

    OptionalState::OptionalState() :
        m_value_ (0)  {

    }   

    OptionalState::OptionalState (int32_t value_,const ::dds::core::optional< std::string >& note_,const std::vector< std::string >& labels_):
        m_value_(value_), 
        m_note_(note_), 
        m_labels_(labels_) {
    }

    void OptionalState::swap(OptionalState& other_)  noexcept 
    {
        using std::swap;
        swap(m_value_, other_.m_value_);
        swap(m_note_, other_.m_note_);
        swap(m_labels_, other_.m_labels_);
    }  

    bool OptionalState::operator == (const OptionalState& other_) const {
        if (m_value_ != other_.m_value_) {
            return false;
        }
        if (m_note_ != other_.m_note_) {
            return false;
        }
        if (m_labels_ != other_.m_labels_) {
            return false;
        }
        return true;
    }

    bool OptionalState::operator != (const OptionalState& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const OptionalState& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value ()<<", ";
        o << "note: " << sample.note ()<<", ";
        o << "labels: " << sample.labels ();
        o <<"]";
        return o;
    }

    // ---- Sample: 

    Sample::Sample() :
        m_phaseId_ (0) ,
        m_text_ ("") ,
        m_boundedText_ ("")  {

            m_wideText_ = L"";
    }   

    Sample::Sample (int32_t phaseId_,const std::vector< int32_t >& values_,const std::string& text_,const std::wstring& wideText_,const std::vector< std::string >& labels_,const ::CorpusUnboundedReceiveLifecycle::LongValues& aliasValues_,const ::rti::core::bounded_sequence< int32_t, 4L >& boundedValues_,const std::string& boundedText_,const ::dds::core::optional< ::CorpusUnboundedReceiveLifecycle::OptionalState >& optionalState_):
        m_phaseId_(phaseId_), 
        m_values_(values_), 
        m_text_(text_), 
        m_wideText_(wideText_), 
        m_labels_(labels_), 
        m_aliasValues_(aliasValues_), 
        m_boundedValues_(boundedValues_), 
        m_boundedText_(boundedText_), 
        m_optionalState_(optionalState_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_phaseId_, other_.m_phaseId_);
        swap(m_values_, other_.m_values_);
        swap(m_text_, other_.m_text_);
        swap(m_wideText_, other_.m_wideText_);
        swap(m_labels_, other_.m_labels_);
        swap(m_aliasValues_, other_.m_aliasValues_);
        swap(m_boundedValues_, other_.m_boundedValues_);
        swap(m_boundedText_, other_.m_boundedText_);
        swap(m_optionalState_, other_.m_optionalState_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_phaseId_ != other_.m_phaseId_) {
            return false;
        }
        if (m_values_ != other_.m_values_) {
            return false;
        }
        if (m_text_ != other_.m_text_) {
            return false;
        }
        if (m_wideText_ != other_.m_wideText_) {
            return false;
        }
        if (m_labels_ != other_.m_labels_) {
            return false;
        }
        if (m_aliasValues_ != other_.m_aliasValues_) {
            return false;
        }
        if (m_boundedValues_ != other_.m_boundedValues_) {
            return false;
        }
        if (m_boundedText_ != other_.m_boundedText_) {
            return false;
        }
        if (m_optionalState_ != other_.m_optionalState_) {
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
        o << "phaseId: " << sample.phaseId ()<<", ";
        o << "values: " << sample.values ()<<", ";
        o << "text: " << sample.text ()<<", ";
        o << "wideText: " << sample.wideText ()<<", ";
        o << "labels: " << sample.labels ()<<", ";
        o << "aliasValues: " << sample.aliasValues ()<<", ";
        o << "boundedValues: " << sample.boundedValues ()<<", ";
        o << "boundedText: " << sample.boundedText ()<<", ";
        o << "optionalState: " << sample.optionalState ();
        o <<"]";
        return o;
    }

} // namespace CorpusUnboundedReceiveLifecycle  

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
        struct native_type_code< ::CorpusUnboundedReceiveLifecycle::LongValues_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode LongValues_g_tc_sequence;

                static DDS_TypeCode LongValues_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedReceiveLifecycle::LongValues", /* Name */
                        NULL, /* Content type code is assigned later */
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for  LongValues */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &LongValues_g_tc;
                }

                LongValues_g_tc_sequence = initialize_sequence_typecode< std::vector< int32_t > >(RTIXCdrLong_MAX);

                LongValues_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                LongValues_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                LongValues_g_tc._data._typeCode =  (RTICdrTypeCode *)& LongValues_g_tc_sequence;

                /* Initialize the values for member annotations. */

                LongValues_g_tc._data._sampleAccessInfo = sample_access_info();
                LongValues_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &LongValues_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo LongValues_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo LongValues_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &LongValues_g_sampleAccessInfo;
                }

                LongValues_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                LongValues_g_sampleAccessInfo.memberAccessInfos = 
                LongValues_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedReceiveLifecycle::LongValues);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        LongValues_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        LongValues_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                LongValues_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                LongValues_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedReceiveLifecycle::LongValues >;

                LongValues_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &LongValues_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin LongValues_g_typePlugin = 
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

                return &LongValues_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedReceiveLifecycle::LongValues_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedReceiveLifecycle::LongValues_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedReceiveLifecycle::OptionalState > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode OptionalState_g_tc_note_string;
                static DDS_TypeCode OptionalState_g_tc_labels_string;
                static DDS_TypeCode OptionalState_g_tc_labels_sequence;

                static DDS_TypeCode_Member OptionalState_g_tc_members[3]=
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
                    }, 
                    {
                        (char *)"note",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"labels",/* Member name */
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
                    }
                };

                static DDS_TypeCode OptionalState_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnboundedReceiveLifecycle::OptionalState", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        OptionalState_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for OptionalState*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &OptionalState_g_tc;
                }

                OptionalState_g_tc_note_string = initialize_string_typecode(RTIXCdrLong_MAX);
                OptionalState_g_tc_labels_string = initialize_string_typecode(RTIXCdrLong_MAX);
                OptionalState_g_tc_labels_sequence = initialize_sequence_typecode< std::vector< std::string > >(RTIXCdrLong_MAX);

                OptionalState_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                OptionalState_g_tc_labels_sequence._data._typeCode = (RTICdrTypeCode *)&OptionalState_g_tc_labels_string;
                OptionalState_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                OptionalState_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&OptionalState_g_tc_note_string;
                OptionalState_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)& OptionalState_g_tc_labels_sequence;

                /* Initialize the values for member annotations. */
                OptionalState_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                OptionalState_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                OptionalState_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                OptionalState_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                OptionalState_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                OptionalState_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                OptionalState_g_tc._data._sampleAccessInfo = sample_access_info();
                OptionalState_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &OptionalState_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnboundedReceiveLifecycle::OptionalState *sample;

                static RTIXCdrMemberAccessInfo OptionalState_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo OptionalState_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &OptionalState_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusUnboundedReceiveLifecycle::OptionalState);
                if (sample == NULL) {
                    return NULL;
                }

                OptionalState_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                OptionalState_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->note() - (char *)sample);

                OptionalState_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->labels() - (char *)sample);

                OptionalState_g_sampleAccessInfo.memberAccessInfos = 
                OptionalState_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedReceiveLifecycle::OptionalState);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        OptionalState_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        OptionalState_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                OptionalState_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                OptionalState_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedReceiveLifecycle::OptionalState >;

                OptionalState_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &OptionalState_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin OptionalState_g_typePlugin = 
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

                return &OptionalState_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnboundedReceiveLifecycle::OptionalState >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedReceiveLifecycle::OptionalState >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedReceiveLifecycle::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Sample_g_tc_values_sequence;
                static DDS_TypeCode Sample_g_tc_text_string;
                static DDS_TypeCode Sample_g_tc_wideText_string;
                static DDS_TypeCode Sample_g_tc_labels_string;
                static DDS_TypeCode Sample_g_tc_labels_sequence;
                static DDS_TypeCode Sample_g_tc_boundedValues_sequence;
                static DDS_TypeCode Sample_g_tc_boundedText_string;

                static DDS_TypeCode_Member Sample_g_tc_members[9]=
                {

                    {
                        (char *)"phaseId",/* Member name */
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
                        (char *)"values",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"wideText",/* Member name */
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
                        (char *)"labels",/* Member name */
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
                        (char *)"aliasValues",/* Member name */
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
                        (char *)"boundedValues",/* Member name */
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
                        (char *)"boundedText",/* Member name */
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
                        (char *)"optionalState",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
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
                        (char *)"CorpusUnboundedReceiveLifecycle::Sample", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        9, /* Number of members */
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

                Sample_g_tc_values_sequence = initialize_sequence_typecode< std::vector< int32_t > >(RTIXCdrLong_MAX);
                Sample_g_tc_text_string = initialize_string_typecode(RTIXCdrLong_MAX);
                Sample_g_tc_wideText_string = initialize_wstring_typecode(RTIXCdrLong_MAX);
                Sample_g_tc_labels_string = initialize_string_typecode(RTIXCdrLong_MAX);
                Sample_g_tc_labels_sequence = initialize_sequence_typecode< std::vector< std::string > >(RTIXCdrLong_MAX);
                Sample_g_tc_boundedValues_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< int32_t, 4L > >((4L));
                Sample_g_tc_boundedText_string = initialize_string_typecode((8L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_values_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_labels_sequence._data._typeCode = (RTICdrTypeCode *)&Sample_g_tc_labels_string;
                Sample_g_tc_boundedValues_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_values_sequence;
                Sample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&Sample_g_tc_text_string;
                Sample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&Sample_g_tc_wideText_string;
                Sample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_labels_sequence;
                Sample_g_tc_members[5]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedReceiveLifecycle::LongValues_AliasTag_t>::get().native();
                Sample_g_tc_members[6]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_boundedValues_sequence;
                Sample_g_tc_members[7]._representation._typeCode = (RTICdrTypeCode *)&Sample_g_tc_boundedText_string;
                Sample_g_tc_members[8]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedReceiveLifecycle::OptionalState>::get().native();

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Sample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Sample_g_tc_members[2]._annotations._defaultValue._u.string_value = (DDS_Char *) "";
                Sample_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_WSTRING;
                Sample_g_tc_members[3]._annotations._defaultValue._u.wstring_value = (DDS_Wchar *) L"";Sample_g_tc_members[7]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Sample_g_tc_members[7]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnboundedReceiveLifecycle::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[9] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusUnboundedReceiveLifecycle::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->phaseId() - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->values() - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->text() - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wideText() - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->labels() - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->aliasValues() - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedValues() - (char *)sample);

                Sample_g_memberAccessInfos[7].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedText() - (char *)sample);

                Sample_g_memberAccessInfos[8].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->optionalState() - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos = 
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedReceiveLifecycle::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedReceiveLifecycle::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnboundedReceiveLifecycle::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedReceiveLifecycle::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::OptionalState >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnboundedReceiveLifecycle::OptionalStatePlugin_new,
                ::CorpusUnboundedReceiveLifecycle::OptionalStatePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnboundedReceiveLifecycle::OptionalState >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnboundedReceiveLifecycle::OptionalState& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = OptionalStatePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = OptionalStatePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::OptionalState >::from_cdr_buffer(::CorpusUnboundedReceiveLifecycle::OptionalState& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = OptionalStatePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnboundedReceiveLifecycle::OptionalState from cdr buffer");
        }

        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::OptionalState >::reset_sample(::CorpusUnboundedReceiveLifecycle::OptionalState& sample) 
        {
            sample.value(0);
            ::rti::topic::reset_sample(sample.note());
            ::rti::topic::reset_sample(sample.labels());
        }

        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::OptionalState >::allocate_sample(::CorpusUnboundedReceiveLifecycle::OptionalState& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.labels(),  -1, -1);
        }
        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnboundedReceiveLifecycle::SamplePlugin_new,
                ::CorpusUnboundedReceiveLifecycle::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnboundedReceiveLifecycle::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnboundedReceiveLifecycle::Sample& sample,
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

        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::Sample >::from_cdr_buffer(::CorpusUnboundedReceiveLifecycle::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnboundedReceiveLifecycle::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::Sample >::reset_sample(::CorpusUnboundedReceiveLifecycle::Sample& sample) 
        {
            sample.phaseId(0);
            ::rti::topic::reset_sample(sample.values());
            sample.text("");
            sample.wideText(L"");
            ::rti::topic::reset_sample(sample.labels());
            ::rti::topic::reset_sample(sample.aliasValues());
            ::rti::topic::reset_sample(sample.boundedValues());
            sample.boundedText("");
            ::rti::topic::reset_sample(sample.optionalState());
        }

        void topic_type_support< ::CorpusUnboundedReceiveLifecycle::Sample >::allocate_sample(::CorpusUnboundedReceiveLifecycle::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.values(),  -1, -1);
            ::rti::topic::allocate_sample(sample.text(),  -1, -1);
            ::rti::topic::allocate_sample(sample.wideText(),  -1, -1);
            ::rti::topic::allocate_sample(sample.labels(),  -1, -1);
            ::rti::topic::allocate_sample(sample.aliasValues(),  -1, -1);
            ::rti::topic::allocate_sample(sample.boundedValues(),  4L, -1);
            ::rti::topic::allocate_sample(sample.boundedText(),  -1, 8L);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
