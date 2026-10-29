

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-unbounded-sequence-aliases.idl
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
#include "05-unbounded-sequence-aliasesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "05-unbounded-sequence-aliases.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusUnboundedSequences {

    std::ostream& operator << (std::ostream& o,const State& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case State::Idle:
            o << "State::Idle" << " ";
            break;
            case State::Ready:
            o << "State::Ready" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }

    // ---- Child: 

    Child::Child() :
        m_value_ (0)  {

    }   

    Child::Child (int32_t value_):
        m_value_(value_) {
    }

    void Child::swap(Child& other_)  noexcept 
    {
        using std::swap;
        swap(m_value_, other_.m_value_);
    }  

    bool Child::operator == (const Child& other_) const {
        if (m_value_ != other_.m_value_) {
            return false;
        }
        return true;
    }

    bool Child::operator != (const Child& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Child& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value ();
        o <<"]";
        return o;
    }

} // namespace CorpusUnboundedSequences  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusUnboundedSequences::State default_enumerator< ::CorpusUnboundedSequences::State>::value = ::CorpusUnboundedSequences::State::Idle;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::Numbers_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Numbers_g_tc_sequence;

                static DDS_TypeCode Numbers_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::Numbers", /* Name */
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
                    }}; /* Type code for  Numbers */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Numbers_g_tc;
                }

                Numbers_g_tc_sequence = initialize_sequence_typecode< std::vector< int32_t > >(RTIXCdrLong_MAX);

                Numbers_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Numbers_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Numbers_g_tc._data._typeCode =  (RTICdrTypeCode *)& Numbers_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Numbers_g_tc._data._sampleAccessInfo = sample_access_info();
                Numbers_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Numbers_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Numbers_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Numbers_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Numbers_g_sampleAccessInfo;
                }

                Numbers_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Numbers_g_sampleAccessInfo.memberAccessInfos = 
                Numbers_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::Numbers);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Numbers_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Numbers_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Numbers_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Numbers_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::Numbers >;

                Numbers_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Numbers_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Numbers_g_typePlugin = 
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

                return &Numbers_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::Numbers_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::Numbers_AliasTag_t >::get())));
        }

        const ::CorpusUnboundedSequences::State default_enumerator< ::CorpusUnboundedSequences::State>::value = ::CorpusUnboundedSequences::State::Idle;
        template<>
        struct native_type_code< ::CorpusUnboundedSequences::State > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member State_g_tc_members[2]=
                {

                    {
                        (char *)"Idle",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusUnboundedSequences::State::Idle), 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */ 

                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"Ready",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusUnboundedSequences::State::Ready), 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */ 

                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode State_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnboundedSequences::State", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        State_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for State*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &State_g_tc;
                }

                State_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                /* Initialize the values for annotations. */
                State_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                State_g_tc._data._annotations._defaultValue._u.long_value = 0;

                State_g_tc._data._sampleAccessInfo = sample_access_info();
                State_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &State_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo State_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo State_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &State_g_sampleAccessInfo;
                }

                State_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                State_g_sampleAccessInfo.memberAccessInfos = 
                State_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::State);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        State_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        State_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                State_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                State_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::State >;

                State_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &State_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin State_g_typePlugin = 
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

                return &State_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusUnboundedSequences::State >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::State >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::Child > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Child_g_tc_members[1]=
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

                static DDS_TypeCode Child_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusUnboundedSequences::Child", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Child_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Child*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Child_g_tc;
                }

                Child_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Child_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Child_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Child_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Child_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Child_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Child_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Child_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Child_g_tc._data._sampleAccessInfo = sample_access_info();
                Child_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Child_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusUnboundedSequences::Child *sample;

                static RTIXCdrMemberAccessInfo Child_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Child_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Child_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusUnboundedSequences::Child);
                if (sample == NULL) {
                    return NULL;
                }

                Child_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                Child_g_sampleAccessInfo.memberAccessInfos = 
                Child_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::Child);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Child_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Child_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Child_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Child_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::Child >;

                Child_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Child_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Child_g_typePlugin = 
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

                return &Child_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusUnboundedSequences::Child >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::Child >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::Scalar_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Scalar_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::Scalar", /* Name */
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
                    }}; /* Type code for  Scalar */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Scalar_g_tc;
                }

                Scalar_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Scalar_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Scalar_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Scalar_g_tc._data._annotations._defaultValue._u.long_value = 0;
                Scalar_g_tc._data._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Scalar_g_tc._data._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Scalar_g_tc._data._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Scalar_g_tc._data._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Scalar_g_tc._data._sampleAccessInfo = sample_access_info();
                Scalar_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Scalar_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Scalar_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Scalar_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Scalar_g_sampleAccessInfo;
                }

                Scalar_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Scalar_g_sampleAccessInfo.memberAccessInfos = 
                Scalar_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::Scalar);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Scalar_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Scalar_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Scalar_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Scalar_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::Scalar >;

                Scalar_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Scalar_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Scalar_g_typePlugin = 
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

                return &Scalar_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::Scalar_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::Scalar_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::States_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode States_g_tc_sequence;

                static DDS_TypeCode States_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::States", /* Name */
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
                    }}; /* Type code for  States */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &States_g_tc;
                }

                States_g_tc_sequence = initialize_sequence_typecode< std::vector< ::CorpusUnboundedSequences::State > >(RTIXCdrLong_MAX);

                States_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                States_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::State>::get().native();
                States_g_tc._data._typeCode =  (RTICdrTypeCode *)& States_g_tc_sequence;

                /* Initialize the values for member annotations. */

                States_g_tc._data._sampleAccessInfo = sample_access_info();
                States_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &States_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo States_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo States_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &States_g_sampleAccessInfo;
                }

                States_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                States_g_sampleAccessInfo.memberAccessInfos = 
                States_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::States);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        States_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        States_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                States_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                States_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::States >;

                States_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &States_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin States_g_typePlugin = 
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

                return &States_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::States_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::States_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::Children_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Children_g_tc_sequence;

                static DDS_TypeCode Children_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::Children", /* Name */
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
                    }}; /* Type code for  Children */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Children_g_tc;
                }

                Children_g_tc_sequence = initialize_sequence_typecode< std::vector< ::CorpusUnboundedSequences::Child > >(RTIXCdrLong_MAX);

                Children_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Children_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::Child>::get().native();
                Children_g_tc._data._typeCode =  (RTICdrTypeCode *)& Children_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Children_g_tc._data._sampleAccessInfo = sample_access_info();
                Children_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Children_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Children_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Children_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Children_g_sampleAccessInfo;
                }

                Children_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Children_g_sampleAccessInfo.memberAccessInfos = 
                Children_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::Children);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Children_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Children_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Children_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Children_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::Children >;

                Children_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Children_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Children_g_typePlugin = 
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

                return &Children_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::Children_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::Children_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::Scalars_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Scalars_g_tc_sequence;

                static DDS_TypeCode Scalars_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::Scalars", /* Name */
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
                    }}; /* Type code for  Scalars */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Scalars_g_tc;
                }

                Scalars_g_tc_sequence = initialize_sequence_typecode< std::vector< ::CorpusUnboundedSequences::Scalar > >(RTIXCdrLong_MAX);

                Scalars_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Scalars_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::Scalar_AliasTag_t>::get().native();
                Scalars_g_tc._data._typeCode =  (RTICdrTypeCode *)& Scalars_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Scalars_g_tc._data._sampleAccessInfo = sample_access_info();
                Scalars_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Scalars_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Scalars_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Scalars_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Scalars_g_sampleAccessInfo;
                }

                Scalars_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Scalars_g_sampleAccessInfo.memberAccessInfos = 
                Scalars_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::Scalars);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Scalars_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Scalars_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Scalars_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Scalars_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::Scalars >;

                Scalars_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Scalars_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Scalars_g_typePlugin = 
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

                return &Scalars_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::Scalars_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::Scalars_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::Texts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Texts_g_tc_string;
                static DDS_TypeCode Texts_g_tc_sequence;

                static DDS_TypeCode Texts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::Texts", /* Name */
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
                    }}; /* Type code for  Texts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Texts_g_tc;
                }

                Texts_g_tc_string = initialize_string_typecode(RTIXCdrLong_MAX);
                Texts_g_tc_sequence = initialize_sequence_typecode< std::vector< std::string > >(RTIXCdrLong_MAX);

                Texts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Texts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&Texts_g_tc_string;
                Texts_g_tc._data._typeCode =  (RTICdrTypeCode *)& Texts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Texts_g_tc._data._sampleAccessInfo = sample_access_info();
                Texts_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Texts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Texts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Texts_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Texts_g_sampleAccessInfo;
                }

                Texts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Texts_g_sampleAccessInfo.memberAccessInfos = 
                Texts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::Texts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Texts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Texts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Texts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Texts_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::Texts >;

                Texts_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Texts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Texts_g_typePlugin = 
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

                return &Texts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::Texts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::Texts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::WideTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode WideTexts_g_tc_string;
                static DDS_TypeCode WideTexts_g_tc_sequence;

                static DDS_TypeCode WideTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::WideTexts", /* Name */
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
                    }}; /* Type code for  WideTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &WideTexts_g_tc;
                }

                WideTexts_g_tc_string = initialize_wstring_typecode(RTIXCdrLong_MAX);
                WideTexts_g_tc_sequence = initialize_sequence_typecode< std::vector< std::wstring > >(RTIXCdrLong_MAX);

                WideTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                WideTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&WideTexts_g_tc_string;
                WideTexts_g_tc._data._typeCode =  (RTICdrTypeCode *)& WideTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                WideTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                WideTexts_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &WideTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo WideTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo WideTexts_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &WideTexts_g_sampleAccessInfo;
                }

                WideTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                WideTexts_g_sampleAccessInfo.memberAccessInfos = 
                WideTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::WideTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        WideTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        WideTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                WideTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                WideTexts_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::WideTexts >;

                WideTexts_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &WideTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin WideTexts_g_typePlugin = 
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

                return &WideTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::WideTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::WideTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::BoundedTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedTexts_g_tc_string;
                static DDS_TypeCode BoundedTexts_g_tc_sequence;

                static DDS_TypeCode BoundedTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::BoundedTexts", /* Name */
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
                    }}; /* Type code for  BoundedTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedTexts_g_tc;
                }

                BoundedTexts_g_tc_string = initialize_string_typecode((8L));
                BoundedTexts_g_tc_sequence = initialize_sequence_typecode< std::vector< std::string > >(RTIXCdrLong_MAX);

                BoundedTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&BoundedTexts_g_tc_string;
                BoundedTexts_g_tc._data._typeCode =  (RTICdrTypeCode *)& BoundedTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                BoundedTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedTexts_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedTexts_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedTexts_g_sampleAccessInfo;
                }

                BoundedTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedTexts_g_sampleAccessInfo.memberAccessInfos = 
                BoundedTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::BoundedTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedTexts_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::BoundedTexts >;

                BoundedTexts_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedTexts_g_typePlugin = 
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

                return &BoundedTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::BoundedTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::BoundedTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::BoundedWideTexts_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedWideTexts_g_tc_string;
                static DDS_TypeCode BoundedWideTexts_g_tc_sequence;

                static DDS_TypeCode BoundedWideTexts_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::BoundedWideTexts", /* Name */
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
                    }}; /* Type code for  BoundedWideTexts */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedWideTexts_g_tc;
                }

                BoundedWideTexts_g_tc_string = initialize_wstring_typecode((4L));
                BoundedWideTexts_g_tc_sequence = initialize_sequence_typecode< std::vector< std::wstring > >(RTIXCdrLong_MAX);

                BoundedWideTexts_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedWideTexts_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&BoundedWideTexts_g_tc_string;
                BoundedWideTexts_g_tc._data._typeCode =  (RTICdrTypeCode *)& BoundedWideTexts_g_tc_sequence;

                /* Initialize the values for member annotations. */

                BoundedWideTexts_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedWideTexts_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedWideTexts_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedWideTexts_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedWideTexts_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedWideTexts_g_sampleAccessInfo;
                }

                BoundedWideTexts_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedWideTexts_g_sampleAccessInfo.memberAccessInfos = 
                BoundedWideTexts_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::BoundedWideTexts);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedWideTexts_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedWideTexts_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedWideTexts_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedWideTexts_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::BoundedWideTexts >;

                BoundedWideTexts_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedWideTexts_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedWideTexts_g_typePlugin = 
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

                return &BoundedWideTexts_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::BoundedWideTexts_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::BoundedWideTexts_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusUnboundedSequences::NumbersChain_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode NumbersChain_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusUnboundedSequences::NumbersChain", /* Name */
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
                    }}; /* Type code for  NumbersChain */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &NumbersChain_g_tc;
                }

                NumbersChain_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                NumbersChain_g_tc._data._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusUnboundedSequences::Numbers_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                NumbersChain_g_tc._data._sampleAccessInfo = sample_access_info();
                NumbersChain_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &NumbersChain_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo NumbersChain_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo NumbersChain_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &NumbersChain_g_sampleAccessInfo;
                }

                NumbersChain_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                NumbersChain_g_sampleAccessInfo.memberAccessInfos = 
                NumbersChain_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusUnboundedSequences::NumbersChain);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        NumbersChain_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        NumbersChain_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                NumbersChain_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                NumbersChain_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusUnboundedSequences::NumbersChain >;

                NumbersChain_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &NumbersChain_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin NumbersChain_g_typePlugin = 
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

                return &NumbersChain_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusUnboundedSequences::NumbersChain_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusUnboundedSequences::NumbersChain_AliasTag_t >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusUnboundedSequences::Child >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusUnboundedSequences::ChildPlugin_new,
                ::CorpusUnboundedSequences::ChildPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusUnboundedSequences::Child >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusUnboundedSequences::Child& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ChildPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ChildPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusUnboundedSequences::Child >::from_cdr_buffer(::CorpusUnboundedSequences::Child& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ChildPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusUnboundedSequences::Child from cdr buffer");
        }

        void topic_type_support< ::CorpusUnboundedSequences::Child >::reset_sample(::CorpusUnboundedSequences::Child& sample) 
        {
            sample.value(0);
        }

        void topic_type_support< ::CorpusUnboundedSequences::Child >::allocate_sample(::CorpusUnboundedSequences::Child& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
