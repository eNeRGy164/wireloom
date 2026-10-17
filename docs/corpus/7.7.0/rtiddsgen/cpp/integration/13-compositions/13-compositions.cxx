

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-compositions.idl
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
#include "13-compositionsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "13-compositions.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusIntegrationCollections {

    // ---- Item:

    Item::Item() :
        id (0) ,
        label ("")  {

    }

    Item::Item (int32_t id_,const ::omg::types::string_view& label_) {
        id = id_;
        label = label_;
    }

    bool operator == (const Item& a, const Item& b) {

        if (a.id != b.id) {
            return false;
        }
        if (a.label != b.label) {
            return false;
        }

        return true;
    }

    bool operator != (const Item& a, const Item& b) {
        return !operator ==(a, b);
    }

    void swap(Item& a, Item& b) noexcept
    {
        using std::swap;

        swap(a.id, b.id);
        swap(a.label, b.label);

    }  
    std::ostream& operator << (std::ostream& o,const Item& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id<<", ";
        o << "label: " << sample.label;
        o <<"]";
        return o;
    }

    // ---- Sample:

    Sample::Sample()  {

    }

    Sample::Sample (const ::omg::types::bounded_sequence< ::CorpusIntegrationCollections::Item, 100L >& unboundedItems_,const ::omg::types::bounded_sequence< ::CorpusIntegrationCollections::Item, 2L >& boundedItems_,const ::dds::core::array< ::CorpusIntegrationCollections::Item, 2L>& aggregateArray_,const ::dds::core::array< ::CorpusIntegrationCollections::Row, 2L>& rows_,const ::omg::types::bounded_sequence< ::CorpusIntegrationCollections::Row, 2L >& sequenceOfArrays_) {
        unboundedItems = unboundedItems_;
        boundedItems = boundedItems_;
        aggregateArray = aggregateArray_;
        rows = rows_;
        sequenceOfArrays = sequenceOfArrays_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.unboundedItems != b.unboundedItems) {
            return false;
        }
        if (a.boundedItems != b.boundedItems) {
            return false;
        }
        if (a.aggregateArray != b.aggregateArray) {
            return false;
        }
        if (a.rows != b.rows) {
            return false;
        }
        if (a.sequenceOfArrays != b.sequenceOfArrays) {
            return false;
        }

        return true;
    }

    bool operator != (const Sample& a, const Sample& b) {
        return !operator ==(a, b);
    }

    void swap(Sample& a, Sample& b) noexcept
    {
        using std::swap;

        swap(a.unboundedItems, b.unboundedItems);
        swap(a.boundedItems, b.boundedItems);
        swap(a.aggregateArray, b.aggregateArray);
        swap(a.rows, b.rows);
        swap(a.sequenceOfArrays, b.sequenceOfArrays);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "unboundedItems: " << sample.unboundedItems<<", ";
        o << "boundedItems: " << sample.boundedItems<<", ";
        o << "aggregateArray: " << sample.aggregateArray<<", ";
        o << "rows: " << sample.rows<<", ";
        o << "sequenceOfArrays: " << sample.sequenceOfArrays;
        o <<"]";
        return o;
    }

} // namespace CorpusIntegrationCollections  

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
        struct native_type_code< ::CorpusIntegrationCollections::Item > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Item_g_tc_label_string;

                static DDS_TypeCode_Member Item_g_tc_members[2]=
                {

                    {
                        (char *)"id",/* Member name */
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
                        (char *)"label",/* Member name */
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

                static DDS_TypeCode Item_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationCollections::Item", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Item_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Item*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Item_g_tc;
                }

                Item_g_tc_label_string = initialize_string_typecode((16L));

                Item_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Item_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Item_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&Item_g_tc_label_string;

                /* Initialize the values for member annotations. */
                Item_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Item_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Item_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Item_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Item_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Item_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Item_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Item_g_tc_members[1]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Item_g_tc._data._sampleAccessInfo = sample_access_info();
                Item_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Item_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegrationCollections::Item *sample;

                static RTIXCdrMemberAccessInfo Item_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Item_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Item_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusIntegrationCollections::Item);
                if (sample == NULL) {
                    return NULL;
                }

                Item_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->id - (char *)sample);

                Item_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->label - (char *)sample);

                Item_g_sampleAccessInfo.memberAccessInfos =
                Item_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationCollections::Item);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Item_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Item_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Item_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Item_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationCollections::Item >;

                Item_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Item_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Item_g_typePlugin =
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

                return &Item_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegrationCollections::Item >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationCollections::Item >::get())));
        }

        template<>
        struct native_type_code< ::CorpusIntegrationCollections::Row_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Row_g_tc_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);

                static DDS_TypeCode Row_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusIntegrationCollections::Row", /* Name */
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
                    }}; /* Type code for  Row */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Row_g_tc;
                }

                Row_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Row_g_tc_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                Row_g_tc._data._typeCode =   (RTICdrTypeCode *)& Row_g_tc_array;

                /* Initialize the values for member annotations. */

                Row_g_tc._data._sampleAccessInfo = sample_access_info();
                Row_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Row_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Row_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Row_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Row_g_sampleAccessInfo;
                }

                Row_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Row_g_sampleAccessInfo.memberAccessInfos =
                Row_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationCollections::Row);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Row_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Row_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Row_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Row_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationCollections::Row >;

                Row_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Row_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Row_g_typePlugin =
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

                return &Row_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusIntegrationCollections::Row_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationCollections::Row_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusIntegrationCollections::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Sample_g_tc_unboundedItems_sequence;
                static DDS_TypeCode Sample_g_tc_boundedItems_sequence;
                static DDS_TypeCode Sample_g_tc_aggregateArray_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);
                static DDS_TypeCode Sample_g_tc_rows_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);
                static DDS_TypeCode Sample_g_tc_sequenceOfArrays_sequence;

                static DDS_TypeCode_Member Sample_g_tc_members[5]=
                {

                    {
                        (char *)"unboundedItems",/* Member name */
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
                        (char *)"boundedItems",/* Member name */
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
                        (char *)"aggregateArray",/* Member name */
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
                    }, 
                    {
                        (char *)"rows",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"sequenceOfArrays",/* Member name */
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
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationCollections::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        5, /* Number of members */
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

                Sample_g_tc_unboundedItems_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< ::CorpusIntegrationCollections::Item, 100L > >((100L));
                Sample_g_tc_boundedItems_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< ::CorpusIntegrationCollections::Item, 2L > >((2L));
                Sample_g_tc_sequenceOfArrays_sequence = initialize_sequence_typecode< ::omg::types::bounded_sequence< ::CorpusIntegrationCollections::Row, 2L > >((2L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_unboundedItems_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationCollections::Item>::get().native();
                Sample_g_tc_boundedItems_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationCollections::Item>::get().native();
                Sample_g_tc_aggregateArray_array._data._typeCode =(RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationCollections::Item>::get().native();
                Sample_g_tc_rows_array._data._typeCode =(RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationCollections::Row_AliasTag_t>::get().native();
                Sample_g_tc_sequenceOfArrays_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusIntegrationCollections::Row_AliasTag_t>::get().native();
                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_unboundedItems_sequence;
                Sample_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_boundedItems_sequence;
                Sample_g_tc_members[2]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_aggregateArray_array;
                Sample_g_tc_members[3]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_rows_array;
                Sample_g_tc_members[4]._representation._typeCode =  (RTICdrTypeCode *)& Sample_g_tc_sequenceOfArrays_sequence;

                /* Initialize the values for member annotations. */

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusIntegrationCollections::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[5] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusIntegrationCollections::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->unboundedItems - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedItems - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->aggregateArray - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->rows - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->sequenceOfArrays - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusIntegrationCollections::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusIntegrationCollections::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusIntegrationCollections::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusIntegrationCollections::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusIntegrationCollections::Item >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusIntegrationCollections::ItemPlugin_new,
                ::CorpusIntegrationCollections::ItemPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusIntegrationCollections::Item >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusIntegrationCollections::Item& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ItemPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ItemPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusIntegrationCollections::Item >::from_cdr_buffer(::CorpusIntegrationCollections::Item& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ItemPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusIntegrationCollections::Item from cdr buffer");
        }

        void topic_type_support< ::CorpusIntegrationCollections::Item >::reset_sample(::CorpusIntegrationCollections::Item& sample) 
        {
            sample.id = 0;
            sample.label = "";
        }

        void topic_type_support< ::CorpusIntegrationCollections::Item >::allocate_sample(::CorpusIntegrationCollections::Item& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.label,  -1, 16L);
        }
        void topic_type_support< ::CorpusIntegrationCollections::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusIntegrationCollections::SamplePlugin_new,
                ::CorpusIntegrationCollections::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusIntegrationCollections::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusIntegrationCollections::Sample& sample,
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

        void topic_type_support< ::CorpusIntegrationCollections::Sample >::from_cdr_buffer(::CorpusIntegrationCollections::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusIntegrationCollections::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusIntegrationCollections::Sample >::reset_sample(::CorpusIntegrationCollections::Sample& sample) 
        {
            ::rti::topic::reset_sample(sample.unboundedItems);
            ::rti::topic::reset_sample(sample.boundedItems);
            ::rti::topic::reset_sample(sample.aggregateArray);
            ::rti::topic::reset_sample(sample.rows);
            ::rti::topic::reset_sample(sample.sequenceOfArrays);
        }

        void topic_type_support< ::CorpusIntegrationCollections::Sample >::allocate_sample(::CorpusIntegrationCollections::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.unboundedItems,  100L, -1);
            ::rti::topic::allocate_sample(sample.boundedItems,  2L, -1);
            ::rti::topic::allocate_sample(sample.aggregateArray,  -1, -1);
            ::rti::topic::allocate_sample(sample.rows,  -1, -1);
            ::rti::topic::allocate_sample(sample.sequenceOfArrays,  2L, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
