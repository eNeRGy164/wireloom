

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-collections.idl
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
#include "05-collectionsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "05-collections.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusCollections {

    // ---- Item: 

    Item::Item() :
        m_id_ (0) ,
        m_label_ ("")  {

    }   

    Item::Item (int32_t id_,const std::string& label_):
        m_id_(id_), 
        m_label_(label_) {
    }

    void Item::swap(Item& other_)  noexcept 
    {
        using std::swap;
        swap(m_id_, other_.m_id_);
        swap(m_label_, other_.m_label_);
    }  

    bool Item::operator == (const Item& other_) const {
        if (m_id_ != other_.m_id_) {
            return false;
        }
        if (m_label_ != other_.m_label_) {
            return false;
        }
        return true;
    }

    bool Item::operator != (const Item& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Item& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "id: " << sample.id ()<<", ";
        o << "label: " << sample.label ();
        o <<"]";
        return o;
    }

    // ---- Sample: 

    Sample::Sample()  {

    }   

    Sample::Sample (const ::dds::core::array< ::dds::core::array< int32_t, 3L>, 2L>& values_,const ::rti::core::bounded_sequence< int32_t, 100L >& unbounded_,const ::CorpusCollections::BoundedLongs& bounded_,const ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L >& items_,const ::CorpusCollections::CoordinateGrid& grid_):
        m_values_(values_), 
        m_unbounded_(unbounded_), 
        m_bounded_(bounded_), 
        m_items_(items_), 
        m_grid_(grid_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_values_, other_.m_values_);
        swap(m_unbounded_, other_.m_unbounded_);
        swap(m_bounded_, other_.m_bounded_);
        swap(m_items_, other_.m_items_);
        swap(m_grid_, other_.m_grid_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_values_ != other_.m_values_) {
            return false;
        }
        if (m_unbounded_ != other_.m_unbounded_) {
            return false;
        }
        if (m_bounded_ != other_.m_bounded_) {
            return false;
        }
        if (m_items_ != other_.m_items_) {
            return false;
        }
        if (m_grid_ != other_.m_grid_) {
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
        o << "values: " << sample.values ()<<", ";
        o << "unbounded: " << sample.unbounded ()<<", ";
        o << "bounded: " << sample.bounded ()<<", ";
        o << "items: " << sample.items ()<<", ";
        o << "grid: " << sample.grid ();
        o <<"]";
        return o;
    }

} // namespace CorpusCollections  

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
        struct native_type_code< ::CorpusCollections::Item > {

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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Item_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusCollections::Item", /* Name */
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

                Item_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Item_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&Item_g_tc_label_string;

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

                ::CorpusCollections::Item *sample;

                static RTIXCdrMemberAccessInfo Item_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Item_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Item_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusCollections::Item);
                if (sample == NULL) {
                    return NULL;
                }

                Item_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->id() - (char *)sample);

                Item_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->label() - (char *)sample);

                Item_g_sampleAccessInfo.memberAccessInfos = 
                Item_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollections::Item);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCollections::Item >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusCollections::Item >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollections::Item >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollections::BoundedLongs_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode BoundedLongs_g_tc_sequence;

                static DDS_TypeCode BoundedLongs_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusCollections::BoundedLongs", /* Name */
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
                    }}; /* Type code for  BoundedLongs */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &BoundedLongs_g_tc;
                }

                BoundedLongs_g_tc_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< int32_t, 4L > >((4L));

                BoundedLongs_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                BoundedLongs_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                BoundedLongs_g_tc._data._typeCode =  (RTICdrTypeCode *)& BoundedLongs_g_tc_sequence;

                /* Initialize the values for member annotations. */

                BoundedLongs_g_tc._data._sampleAccessInfo = sample_access_info();
                BoundedLongs_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &BoundedLongs_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo BoundedLongs_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo BoundedLongs_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &BoundedLongs_g_sampleAccessInfo;
                }

                BoundedLongs_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                BoundedLongs_g_sampleAccessInfo.memberAccessInfos = 
                BoundedLongs_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollections::BoundedLongs);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        BoundedLongs_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        BoundedLongs_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                BoundedLongs_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                BoundedLongs_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusCollections::BoundedLongs >;

                BoundedLongs_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &BoundedLongs_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin BoundedLongs_g_typePlugin = 
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

                return &BoundedLongs_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollections::BoundedLongs_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollections::BoundedLongs_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollections::CoordinateGrid_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};
                static DDS_UnsignedLong CoordinateGrid_g_tc_dimensions [2] = {2L,3L} ;

                static DDS_TypeCode CoordinateGrid_g_tc_array =DDS_INITIALIZE_ARRAY_TYPECODE(2,2L,CoordinateGrid_g_tc_dimensions,NULL);

                static DDS_TypeCode CoordinateGrid_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusCollections::CoordinateGrid", /* Name */
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
                    }}; /* Type code for  CoordinateGrid */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &CoordinateGrid_g_tc;
                }

                CoordinateGrid_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                CoordinateGrid_g_tc_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                CoordinateGrid_g_tc._data._typeCode =  (RTICdrTypeCode *)& CoordinateGrid_g_tc_array;

                /* Initialize the values for member annotations. */

                CoordinateGrid_g_tc._data._sampleAccessInfo = sample_access_info();
                CoordinateGrid_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &CoordinateGrid_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo CoordinateGrid_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo CoordinateGrid_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &CoordinateGrid_g_sampleAccessInfo;
                }

                CoordinateGrid_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                CoordinateGrid_g_sampleAccessInfo.memberAccessInfos = 
                CoordinateGrid_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollections::CoordinateGrid);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        CoordinateGrid_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        CoordinateGrid_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                CoordinateGrid_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                CoordinateGrid_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusCollections::CoordinateGrid >;

                CoordinateGrid_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &CoordinateGrid_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin CoordinateGrid_g_typePlugin = 
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

                return &CoordinateGrid_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollections::CoordinateGrid_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollections::CoordinateGrid_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollections::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};
                static DDS_UnsignedLong Sample_g_tc_values_dimensions [2] = {2L,3L} ;

                static DDS_TypeCode Sample_g_tc_values_array =DDS_INITIALIZE_ARRAY_TYPECODE(2,2L,Sample_g_tc_values_dimensions,NULL);
                static DDS_TypeCode Sample_g_tc_unbounded_sequence;
                static DDS_TypeCode Sample_g_tc_items_sequence;

                static DDS_TypeCode_Member Sample_g_tc_members[5]=
                {

                    {
                        (char *)"values",/* Member name */
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
                        (char *)"unbounded",/* Member name */
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
                        (char *)"bounded",/* Member name */
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
                        (char *)"items",/* Member name */
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
                        (char *)"grid",/* Member name */
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
                    }
                };

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusCollections::Sample", /* Name */
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

                Sample_g_tc_unbounded_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< int32_t, 100L > >((100L));
                Sample_g_tc_items_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollections::Item, 2L > >((2L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_values_array._data._typeCode =(RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_unbounded_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_items_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollections::Item>::get().native();
                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_values_array;
                Sample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_unbounded_sequence;
                Sample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollections::BoundedLongs_AliasTag_t>::get().native();
                Sample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_items_sequence;
                Sample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollections::CoordinateGrid_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusCollections::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[5] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusCollections::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->values() - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->unbounded() - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->bounded() - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->items() - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->grid() - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos = 
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollections::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCollections::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusCollections::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollections::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusCollections::Item >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusCollections::ItemPlugin_new,
                ::CorpusCollections::ItemPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusCollections::Item >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusCollections::Item& sample,
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

        void topic_type_support< ::CorpusCollections::Item >::from_cdr_buffer(::CorpusCollections::Item& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ItemPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusCollections::Item from cdr buffer");
        }

        void topic_type_support< ::CorpusCollections::Item >::reset_sample(::CorpusCollections::Item& sample) 
        {
            sample.id(0);
            sample.label("");
        }

        void topic_type_support< ::CorpusCollections::Item >::allocate_sample(::CorpusCollections::Item& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.label(),  -1, 16L);
        }
        void topic_type_support< ::CorpusCollections::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusCollections::SamplePlugin_new,
                ::CorpusCollections::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusCollections::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusCollections::Sample& sample,
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

        void topic_type_support< ::CorpusCollections::Sample >::from_cdr_buffer(::CorpusCollections::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusCollections::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusCollections::Sample >::reset_sample(::CorpusCollections::Sample& sample) 
        {
            ::rti::topic::reset_sample(sample.values());
            ::rti::topic::reset_sample(sample.unbounded());
            ::rti::topic::reset_sample(sample.bounded());
            ::rti::topic::reset_sample(sample.items());
            ::rti::topic::reset_sample(sample.grid());
        }

        void topic_type_support< ::CorpusCollections::Sample >::allocate_sample(::CorpusCollections::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.values(),  -1, -1);
            ::rti::topic::allocate_sample(sample.unbounded(),  100L, -1);
            ::rti::topic::allocate_sample(sample.bounded(),  -1, -1);
            ::rti::topic::allocate_sample(sample.items(),  2L, -1);
            ::rti::topic::allocate_sample(sample.grid(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
