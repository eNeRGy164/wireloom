

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-aggregates.idl
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
#include "06-aggregatesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "06-aggregates.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusAggregates {

    // ---- Base:

    Base::Base() :
        base (0)  {

    }

    Base::Base (int32_t base_) {
        base = base_;
    }

    bool operator == (const Base& a, const Base& b) {

        if (a.base != b.base) {
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

        swap(a.base, b.base);

    }  
    std::ostream& operator << (std::ostream& o,const Base& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "base: " << sample.base;
        o <<"]";
        return o;
    }

    // ---- Derived:

    Derived::Derived() :
        derived ("")  {

    }

    Derived::Derived (int32_t base_,const ::omg::types::string_view& derived_) :
        ::CorpusAggregates::Base(base_) {
            derived = derived_;
    }

    bool operator == (const Derived& a, const Derived& b) {
        if (!operator == ((::CorpusAggregates::Base) a, (::CorpusAggregates::Base) b)){
            return false;
        }

        if (a.derived != b.derived) {
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

        swap((::CorpusAggregates::Base&) a, (::CorpusAggregates::Base&) b);
        swap(a.derived, b.derived);

    }  
    std::ostream& operator << (std::ostream& o,const Derived& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << static_cast<const ::CorpusAggregates::Base &>(sample);
        o << "derived: " << sample.derived;
        o <<"]";
        return o;
    }

    // ---- Recursive:

    Recursive::Recursive() :
        value (0)  {

    }

    Recursive::Recursive (int32_t value_,const ::omg::types::sequence< ::CorpusAggregates::Recursive >& children_) {
        value = value_;
        children = children_;
    }

    bool operator == (const Recursive& a, const Recursive& b) {

        if (a.value != b.value) {
            return false;
        }
        if (a.children != b.children) {
            return false;
        }

        return true;
    }

    bool operator != (const Recursive& a, const Recursive& b) {
        return !operator ==(a, b);
    }

    void swap(Recursive& a, Recursive& b) noexcept
    {
        using std::swap;

        swap(a.value, b.value);
        swap(a.children, b.children);

    }  
    std::ostream& operator << (std::ostream& o,const Recursive& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value<<", ";
        o << "children: " << sample.children;
        o <<"]";
        return o;
    }

    // ---- Composed:

    Composed::Composed()  {

    }

    Composed::Composed (const ::CorpusAggregates::Base& base_,const ::CorpusAggregates::Derived& derived_) {
        base = base_;
        derived = derived_;
    }

    bool operator == (const Composed& a, const Composed& b) {

        if (a.base != b.base) {
            return false;
        }
        if (a.derived != b.derived) {
            return false;
        }

        return true;
    }

    bool operator != (const Composed& a, const Composed& b) {
        return !operator ==(a, b);
    }

    void swap(Composed& a, Composed& b) noexcept
    {
        using std::swap;

        swap(a.base, b.base);
        swap(a.derived, b.derived);

    }  
    std::ostream& operator << (std::ostream& o,const Composed& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "base: " << sample.base<<", ";
        o << "derived: " << sample.derived;
        o <<"]";
        return o;
    }

} // namespace CorpusAggregates  

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
        struct native_type_code< ::CorpusAggregates::Base > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Base_g_tc_members[1]=
                {

                    {
                        (char *)"base",/* Member name */
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
                        (char *)"CorpusAggregates::Base", /* Name */
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

                ::CorpusAggregates::Base *sample;

                static RTIXCdrMemberAccessInfo Base_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Base_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Base_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusAggregates::Base);
                if (sample == NULL) {
                    return NULL;
                }

                Base_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->base - (char *)sample);

                Base_g_sampleAccessInfo.memberAccessInfos =
                Base_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusAggregates::Base);

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
                interpreter::get_aggregation_value_pointer< ::CorpusAggregates::Base >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusAggregates::Base >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusAggregates::Base >::get())));
        }

        template<>
        struct native_type_code< ::CorpusAggregates::Derived > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Derived_g_tc_derived_string;

                static DDS_TypeCode_Member Derived_g_tc_members[1]=
                {

                    {
                        (char *)"derived",/* Member name */
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
                    }
                };

                static DDS_TypeCode Derived_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusAggregates::Derived", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
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

                Derived_g_tc_derived_string = initialize_string_typecode((16L));

                Derived_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Derived_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&Derived_g_tc_derived_string;

                /* Initialize the values for member annotations. */
                Derived_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Derived_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Derived_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusAggregates::Base >::get().native(); /* Base class */

                Derived_g_tc._data._sampleAccessInfo = sample_access_info();
                Derived_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Derived_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusAggregates::Derived *sample;

                static RTIXCdrMemberAccessInfo Derived_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Derived_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusAggregates::Derived);
                if (sample == NULL) {
                    return NULL;
                }

                Derived_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->derived - (char *)sample);

                Derived_g_sampleAccessInfo.memberAccessInfos =
                Derived_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusAggregates::Derived);

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
                interpreter::get_aggregation_value_pointer< ::CorpusAggregates::Derived >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusAggregates::Derived >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusAggregates::Derived >::get())));
        }

        template<>
        struct native_type_code< ::CorpusAggregates::Recursive > {
            static DDS_TypeCode * get()
            {
                RTIXCdrResourceNode node = RTIXCdrResourceNode_INITIALIZER;
                return (DDS_TypeCode *) &::rti::topic::dynamic_type< ::CorpusAggregates::Recursive>::get(&node).native();
            }

            static DDS_TypeCode * get(RTIXCdrResourceNode *parentVisitedNode)
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};
                RTIXCdrResourceNode node = RTIXCdrResourceNode_INITIALIZER;

                static DDS_TypeCode Recursive_g_tc_children_sequence;

                static DDS_TypeCode_Member Recursive_g_tc_members[2]=
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"children",/* Member name */
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
                    }
                };

                static DDS_TypeCode Recursive_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusAggregates::Recursive", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Recursive_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Recursive*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Recursive_g_tc;
                }

                /* Return if the type has already been visited stopping recursion */
                node.prev = parentVisitedNode;
                node.resource = &Recursive_g_tc;
                if (RTIXCdrTypeCode_isResourceVisited(node.resource, parentVisitedNode, NULL)) {
                    return &Recursive_g_tc;
                }

                Recursive_g_tc_children_sequence = initialize_sequence_typecode< ::omg::types::sequence< ::CorpusAggregates::Recursive > >(RTIXCdrLong_MAX);

                Recursive_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Recursive_g_tc_children_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusAggregates::Recursive>::get(&node).native();
                Recursive_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Recursive_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)& Recursive_g_tc_children_sequence;

                /* Initialize the values for member annotations. */
                Recursive_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Recursive_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Recursive_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Recursive_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Recursive_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Recursive_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Recursive_g_tc._data._sampleAccessInfo = sample_access_info();
                Recursive_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Recursive_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusAggregates::Recursive *sample;

                static RTIXCdrMemberAccessInfo Recursive_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Recursive_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Recursive_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusAggregates::Recursive);
                if (sample == NULL) {
                    return NULL;
                }

                Recursive_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->value - (char *)sample);

                Recursive_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->children - (char *)sample);

                Recursive_g_sampleAccessInfo.memberAccessInfos =
                Recursive_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusAggregates::Recursive);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Recursive_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Recursive_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Recursive_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Recursive_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusAggregates::Recursive >;

                Recursive_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Recursive_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Recursive_g_typePlugin =
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

                return &Recursive_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusAggregates::Recursive >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusAggregates::Recursive >::get())));
        }
        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusAggregates::Recursive >::get(RTIXCdrResourceNode *parentVisitedNode)
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusAggregates::Recursive >::get(parentVisitedNode))));
        }

        template<>
        struct native_type_code< ::CorpusAggregates::Composed > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Composed_g_tc_members[2]=
                {

                    {
                        (char *)"base",/* Member name */
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
                        (char *)"derived",/* Member name */
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
                    }
                };

                static DDS_TypeCode Composed_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusAggregates::Composed", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Composed_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Composed*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Composed_g_tc;
                }

                Composed_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Composed_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusAggregates::Base>::get().native();
                Composed_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusAggregates::Derived>::get().native();

                /* Initialize the values for member annotations. */

                Composed_g_tc._data._sampleAccessInfo = sample_access_info();
                Composed_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Composed_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusAggregates::Composed *sample;

                static RTIXCdrMemberAccessInfo Composed_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Composed_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Composed_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusAggregates::Composed);
                if (sample == NULL) {
                    return NULL;
                }

                Composed_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->base - (char *)sample);

                Composed_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->derived - (char *)sample);

                Composed_g_sampleAccessInfo.memberAccessInfos =
                Composed_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusAggregates::Composed);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Composed_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Composed_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Composed_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Composed_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusAggregates::Composed >;

                Composed_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Composed_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Composed_g_typePlugin =
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

                return &Composed_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusAggregates::Composed >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusAggregates::Composed >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusAggregates::Base >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusAggregates::BasePlugin_new,
                ::CorpusAggregates::BasePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusAggregates::Base >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusAggregates::Base& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = BasePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = BasePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusAggregates::Base >::from_cdr_buffer(::CorpusAggregates::Base& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = BasePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusAggregates::Base from cdr buffer");
        }

        void topic_type_support< ::CorpusAggregates::Base >::reset_sample(::CorpusAggregates::Base& sample) 
        {
            sample.base = 0;
        }

        void topic_type_support< ::CorpusAggregates::Base >::allocate_sample(::CorpusAggregates::Base& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusAggregates::Derived >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusAggregates::DerivedPlugin_new,
                ::CorpusAggregates::DerivedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusAggregates::Derived >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusAggregates::Derived& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = DerivedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = DerivedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusAggregates::Derived >::from_cdr_buffer(::CorpusAggregates::Derived& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DerivedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusAggregates::Derived from cdr buffer");
        }

        void topic_type_support< ::CorpusAggregates::Derived >::reset_sample(::CorpusAggregates::Derived& sample) 
        {
            // Initialize base
            topic_type_support< ::CorpusAggregates::Base >::reset_sample(sample);

            sample.derived = "";
        }

        void topic_type_support< ::CorpusAggregates::Derived >::allocate_sample(::CorpusAggregates::Derived& sample, int, int) 
        {
            // Initialize base
            topic_type_support< ::CorpusAggregates::Base >::allocate_sample(sample, -1, -1);

            ::rti::topic::allocate_sample(sample.derived,  -1, 16L);
        }
        void topic_type_support< ::CorpusAggregates::Recursive >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusAggregates::RecursivePlugin_new,
                ::CorpusAggregates::RecursivePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusAggregates::Recursive >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusAggregates::Recursive& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = RecursivePlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = RecursivePlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusAggregates::Recursive >::from_cdr_buffer(::CorpusAggregates::Recursive& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = RecursivePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusAggregates::Recursive from cdr buffer");
        }

        void topic_type_support< ::CorpusAggregates::Recursive >::reset_sample(::CorpusAggregates::Recursive& sample) 
        {
            sample.value = 0;
            ::rti::topic::reset_sample(sample.children);
        }

        void topic_type_support< ::CorpusAggregates::Recursive >::allocate_sample(::CorpusAggregates::Recursive& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.children,  -1, -1);
        }
        void topic_type_support< ::CorpusAggregates::Composed >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusAggregates::ComposedPlugin_new,
                ::CorpusAggregates::ComposedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusAggregates::Composed >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusAggregates::Composed& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ComposedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ComposedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusAggregates::Composed >::from_cdr_buffer(::CorpusAggregates::Composed& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ComposedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusAggregates::Composed from cdr buffer");
        }

        void topic_type_support< ::CorpusAggregates::Composed >::reset_sample(::CorpusAggregates::Composed& sample) 
        {
            ::rti::topic::reset_sample(sample.base);
            ::rti::topic::reset_sample(sample.derived);
        }

        void topic_type_support< ::CorpusAggregates::Composed >::allocate_sample(::CorpusAggregates::Composed& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.base,  -1, -1);
            ::rti::topic::allocate_sample(sample.derived,  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
