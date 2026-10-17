

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
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
#include "13-alias-inheritancePlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "13-alias-inheritance.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusIntegrationTrace {

    // ---- Context:

    Context::Context() :
        correlation ("")  {

    }

    Context::Context (const ::omg::types::string_view& correlation_,const ::omg::types::optional< std::string >& producer_) {
        correlation = correlation_;
        producer = producer_;
    }

    bool operator == (const Context& a, const Context& b) {

        if (a.correlation != b.correlation) {
            return false;
        }
        if (a.producer != b.producer) {
            return false;
        }

        return true;
    }

    bool operator != (const Context& a, const Context& b) {
        return !operator ==(a, b);
    }

    void swap(Context& a, Context& b) noexcept
    {
        using std::swap;

        swap(a.correlation, b.correlation);
        swap(a.producer, b.producer);

    }  
    std::ostream& operator << (std::ostream& o,const Context& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "correlation: " << sample.correlation<<", ";
        o << "producer: " << sample.producer;
        o <<"]";
        return o;
    }

    // ---- Base:

    Base::Base() :
        baseValue (0)  {

    }

    Base::Base (int32_t baseValue_) {
        baseValue = baseValue_;
    }

    bool operator == (const Base& a, const Base& b) {

        if (a.baseValue != b.baseValue) {
            return false;
        }

        return true;
    }

    bool operator != (const Base& a, const Base& b) {
        return !operator ==(a, b);
    }

    void swap(Base& a, Base& b) noexcept
    {
        using std::swap;

        swap(a.baseValue, b.baseValue);

    }  
    std::ostream& operator << (std::ostream& o,const Base& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "baseValue: " << sample.baseValue;
        o <<"]";
        return o;
    }

    // ---- Derived:

    Derived::Derived() :
        state ("")  {

    }

    Derived::Derived (int32_t baseValue_,const ::omg::types::string_view& state_,const ::CorpusIntegrationTrace::Context& traceContext_) :
        ::CorpusIntegrationTrace::Base(baseValue_) {
            state = state_;
            traceContext = traceContext_;
    }

    bool operator == (const Derived& a, const Derived& b) {
        if (!operator == ((::CorpusIntegrationTrace::Base) a, (::CorpusIntegrationTrace::Base) b)){
            return false;
        }

        if (a.state != b.state) {
            return false;
        }
        if (a.traceContext != b.traceContext) {
            return false;
        }

        return true;
    }

    bool operator != (const Derived& a, const Derived& b) {
        return !operator ==(a, b);
    }

    void swap(Derived& a, Derived& b) noexcept
    {
        using std::swap;

        swap((::CorpusIntegrationTrace::Base&) a, (::CorpusIntegrationTrace::Base&) b);
        swap(a.state, b.state);
        swap(a.traceContext, b.traceContext);

    }  
    std::ostream& operator << (std::ostream& o,const Derived& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << static_cast<const ::CorpusIntegrationTrace::Base &>(sample);
        o << "state: " << sample.state<<", ";
        o << "traceContext: " << sample.traceContext;
        o <<"]";
        return o;
    }

} // namespace CorpusIntegrationTrace  

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
        struct native_type_code< ::CorpusIntegrationTrace::Producer_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Producer_g_tc_string;

                static DDS_TypeCode Producer_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusIntegrationTrace::Producer", /* Name */
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
                    }}; /* Type code for  Producer */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Producer_g_tc;
                }

                Producer_g_tc_string = initialize_string_typecode((16L));

                Producer_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Producer_g_tc._data._typeCode =   (RTICdrTypeCode *)&Producer_g_tc_string;

                /* Initialize the values for member annotations. */
                Producer_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Producer_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Producer_g_tc._data._sampleAccessInfo = sample_access_info();
                Producer_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Producer_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Producer_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Producer_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Producer_g_sampleAccessInfo;
                }

                Producer_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Producer_g_sampleAccessInfo.memberAccessInfos =
                Producer_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationTrace::Producer);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Producer_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Producer_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Producer_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Producer_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationTrace::Producer >;

                Producer_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Producer_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Producer_g_typePlugin =
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

                return &Producer_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusIntegrationTrace::Producer_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationTrace::Producer_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusIntegrationTrace::Context > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Context_g_tc_correlation_string;

                static DDS_TypeCode_Member Context_g_tc_members[2]=
                {

                    {
                        (char *)"correlation",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"producer",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Context_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationTrace::Context", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Context_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Context*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Context_g_tc;
                }

                Context_g_tc_correlation_string = initialize_string_typecode((32L));

                Context_g_tc._data._annotations._allowedDataRepresentationMask = 5;
                Context_g_tc._data._annotations._isNested = RTI_XCDR_TRUE;

                Context_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&Context_g_tc_correlation_string;
                Context_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationTrace::Producer_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                Context_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Context_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Context_g_tc._data._sampleAccessInfo = sample_access_info();
                Context_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Context_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegrationTrace::Context *sample;

                static RTIXCdrMemberAccessInfo Context_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Context_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Context_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusIntegrationTrace::Context);
                if (sample == NULL) {
                    return NULL;
                }

                Context_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->correlation - (char *)sample);

                Context_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->producer - (char *)sample);

                Context_g_sampleAccessInfo.memberAccessInfos =
                Context_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationTrace::Context);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Context_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Context_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Context_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Context_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationTrace::Context >;

                Context_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Context_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Context_g_typePlugin =
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

                return &Context_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegrationTrace::Context >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationTrace::Context >::get())));
        }

        template<>
        struct native_type_code< ::CorpusIntegrationTrace::Base > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Base_g_tc_members[1]=
                {

                    {
                        (char *)"baseValue",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Base_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationTrace::Base", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Base_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Base*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Base_g_tc;
                }

                Base_g_tc._data._annotations._allowedDataRepresentationMask = 5;
                Base_g_tc._data._annotations._isNested = RTI_XCDR_TRUE;

                Base_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Base_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Base_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Base_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Base_g_tc._data._sampleAccessInfo = sample_access_info();
                Base_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Base_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegrationTrace::Base *sample;

                static RTIXCdrMemberAccessInfo Base_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Base_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Base_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusIntegrationTrace::Base);
                if (sample == NULL) {
                    return NULL;
                }

                Base_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->baseValue - (char *)sample);

                Base_g_sampleAccessInfo.memberAccessInfos =
                Base_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationTrace::Base);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Base_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Base_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Base_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Base_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationTrace::Base >;

                Base_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Base_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Base_g_typePlugin =
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

                return &Base_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegrationTrace::Base >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationTrace::Base >::get())));
        }

        template<>
        struct native_type_code< ::CorpusIntegrationTrace::Derived > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Derived_g_tc_state_string;

                static DDS_TypeCode_Member Derived_g_tc_members[2]=
                {

                    {
                        (char *)"state",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"traceContext",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Derived_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationTrace::Derived", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Derived_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Derived*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Derived_g_tc;
                }

                Derived_g_tc_state_string = initialize_string_typecode((16L));

                Derived_g_tc._data._annotations._allowedDataRepresentationMask = 5;
                Derived_g_tc._data._annotations._isNested = RTI_XCDR_TRUE;

                Derived_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&Derived_g_tc_state_string;
                Derived_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationTrace::Context>::get().native();

                /* Initialize the values for member annotations. */
                Derived_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Derived_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Derived_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationTrace::Base >::get().native(); /* Base class */

                Derived_g_tc._data._sampleAccessInfo = sample_access_info();
                Derived_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Derived_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegrationTrace::Derived *sample;

                static RTIXCdrMemberAccessInfo Derived_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Derived_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusIntegrationTrace::Derived);
                if (sample == NULL) {
                    return NULL;
                }

                Derived_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->state - (char *)sample);

                Derived_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->traceContext - (char *)sample);

                Derived_g_sampleAccessInfo.memberAccessInfos =
                Derived_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationTrace::Derived);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Derived_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Derived_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Derived_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Derived_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationTrace::Derived >;

                Derived_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Derived_g_typePlugin =
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

                return &Derived_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegrationTrace::Derived >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationTrace::Derived >::get())));
        }
    }
}

namespace dds { 
    namespace topic {

        void topic_type_support< ::CorpusIntegrationTrace::Context >::reset_sample(::CorpusIntegrationTrace::Context& sample) 
        {
            sample.correlation = "";
            ::rti::topic::reset_sample(sample.producer);
        }

        void topic_type_support< ::CorpusIntegrationTrace::Context >::allocate_sample(::CorpusIntegrationTrace::Context& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.correlation,  -1, 32L);
        }

        void topic_type_support< ::CorpusIntegrationTrace::Base >::reset_sample(::CorpusIntegrationTrace::Base& sample) 
        {
            sample.baseValue = 0;
        }

        void topic_type_support< ::CorpusIntegrationTrace::Base >::allocate_sample(::CorpusIntegrationTrace::Base& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusIntegrationTrace::Derived >::reset_sample(::CorpusIntegrationTrace::Derived& sample) 
        {
            // Initialize base
            topic_type_support< ::CorpusIntegrationTrace::Base >::reset_sample(sample);

            sample.state = "";
            ::rti::topic::reset_sample(sample.traceContext);
        }

        void topic_type_support< ::CorpusIntegrationTrace::Derived >::allocate_sample(::CorpusIntegrationTrace::Derived& sample, int, int) 
        {
            // Initialize base
            topic_type_support< ::CorpusIntegrationTrace::Base >::allocate_sample(sample, -1, -1);

            ::rti::topic::allocate_sample(sample.state,  -1, 16L);
            ::rti::topic::allocate_sample(sample.traceContext,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
