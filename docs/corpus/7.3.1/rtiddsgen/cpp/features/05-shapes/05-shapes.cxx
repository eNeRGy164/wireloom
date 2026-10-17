

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-shapes.idl
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
#include "05-shapesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "05-shapes.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusCollectionShapes {

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

    Sample::Sample (const ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Item, 100L >& unboundedItems_,const ::CorpusCollectionShapes::Items& boundedItems_,const ::rti::core::bounded_sequence< ::CorpusCollectionShapes::BoundedLongs, 2L >& nestedSequences_,const ::dds::core::array< ::CorpusCollectionShapes::Row, 2L>& rows_,const ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Row, 2L >& sequenceOfArrays_,const ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Rows, 2L >& sequenceOfArrayAliases_,const ::CorpusCollectionShapes::Names& names_):
        m_unboundedItems_(unboundedItems_), 
        m_boundedItems_(boundedItems_), 
        m_nestedSequences_(nestedSequences_), 
        m_rows_(rows_), 
        m_sequenceOfArrays_(sequenceOfArrays_), 
        m_sequenceOfArrayAliases_(sequenceOfArrayAliases_), 
        m_names_(names_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_unboundedItems_, other_.m_unboundedItems_);
        swap(m_boundedItems_, other_.m_boundedItems_);
        swap(m_nestedSequences_, other_.m_nestedSequences_);
        swap(m_rows_, other_.m_rows_);
        swap(m_sequenceOfArrays_, other_.m_sequenceOfArrays_);
        swap(m_sequenceOfArrayAliases_, other_.m_sequenceOfArrayAliases_);
        swap(m_names_, other_.m_names_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_unboundedItems_ != other_.m_unboundedItems_) {
            return false;
        }
        if (m_boundedItems_ != other_.m_boundedItems_) {
            return false;
        }
        if (m_nestedSequences_ != other_.m_nestedSequences_) {
            return false;
        }
        if (m_rows_ != other_.m_rows_) {
            return false;
        }
        if (m_sequenceOfArrays_ != other_.m_sequenceOfArrays_) {
            return false;
        }
        if (m_sequenceOfArrayAliases_ != other_.m_sequenceOfArrayAliases_) {
            return false;
        }
        if (m_names_ != other_.m_names_) {
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
        o << "unboundedItems: " << sample.unboundedItems ()<<", ";
        o << "boundedItems: " << sample.boundedItems ()<<", ";
        o << "nestedSequences: " << sample.nestedSequences ()<<", ";
        o << "rows: " << sample.rows ()<<", ";
        o << "sequenceOfArrays: " << sample.sequenceOfArrays ()<<", ";
        o << "sequenceOfArrayAliases: " << sample.sequenceOfArrayAliases ()<<", ";
        o << "names: " << sample.names ();
        o <<"]";
        return o;
    }

} // namespace CorpusCollectionShapes  

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
        struct native_type_code< ::CorpusCollectionShapes::Item > {

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
                        (char *)"CorpusCollectionShapes::Item", /* Name */
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

                Item_g_tc_label_string = initialize_string_typecode((12L));

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

                ::CorpusCollectionShapes::Item *sample;

                static RTIXCdrMemberAccessInfo Item_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Item_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Item_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusCollectionShapes::Item);
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
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::Item);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::Item >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusCollectionShapes::Item >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::Item >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollectionShapes::Row_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Row_g_tc_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,3L, NULL,NULL);

                static DDS_TypeCode Row_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusCollectionShapes::Row", /* Name */
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
                Row_g_tc._data._typeCode =  (RTICdrTypeCode *)& Row_g_tc_array;

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
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::Row);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::Row >;

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

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollectionShapes::Row_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::Row_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollectionShapes::BoundedLongs_AliasTag_t > {

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
                        (char *)"CorpusCollectionShapes::BoundedLongs", /* Name */
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
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::BoundedLongs);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::BoundedLongs >;

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

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollectionShapes::BoundedLongs_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::BoundedLongs_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollectionShapes::Rows_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Rows_g_tc_sequence;

                static DDS_TypeCode Rows_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusCollectionShapes::Rows", /* Name */
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
                    }}; /* Type code for  Rows */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Rows_g_tc;
                }

                Rows_g_tc_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Row, 2L > >((2L));

                Rows_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Rows_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Row_AliasTag_t>::get().native();
                Rows_g_tc._data._typeCode =  (RTICdrTypeCode *)& Rows_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Rows_g_tc._data._sampleAccessInfo = sample_access_info();
                Rows_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Rows_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Rows_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Rows_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Rows_g_sampleAccessInfo;
                }

                Rows_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Rows_g_sampleAccessInfo.memberAccessInfos = 
                Rows_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::Rows);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Rows_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Rows_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Rows_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Rows_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::Rows >;

                Rows_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Rows_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Rows_g_typePlugin = 
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

                return &Rows_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollectionShapes::Rows_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::Rows_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollectionShapes::Items_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Items_g_tc_sequence;

                static DDS_TypeCode Items_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusCollectionShapes::Items", /* Name */
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
                    }}; /* Type code for  Items */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Items_g_tc;
                }

                Items_g_tc_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Item, 3L > >((3L));

                Items_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Items_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Item>::get().native();
                Items_g_tc._data._typeCode =  (RTICdrTypeCode *)& Items_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Items_g_tc._data._sampleAccessInfo = sample_access_info();
                Items_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Items_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Items_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Items_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Items_g_sampleAccessInfo;
                }

                Items_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Items_g_sampleAccessInfo.memberAccessInfos = 
                Items_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::Items);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Items_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Items_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Items_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Items_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::Items >;

                Items_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Items_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Items_g_typePlugin = 
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

                return &Items_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollectionShapes::Items_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::Items_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollectionShapes::Names_AliasTag_t > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Names_g_tc_string;
                static DDS_TypeCode Names_g_tc_sequence;

                static DDS_TypeCode Names_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusCollectionShapes::Names", /* Name */
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
                    }}; /* Type code for  Names */

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Names_g_tc;
                }

                Names_g_tc_string = initialize_string_typecode((32L));
                Names_g_tc_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< std::string, 4L > >((4L));

                Names_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Names_g_tc_sequence._data._typeCode = (RTICdrTypeCode *)&Names_g_tc_string;
                Names_g_tc._data._typeCode =  (RTICdrTypeCode *)& Names_g_tc_sequence;

                /* Initialize the values for member annotations. */

                Names_g_tc._data._sampleAccessInfo = sample_access_info();
                Names_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Names_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Names_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Names_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Names_g_sampleAccessInfo;
                }

                Names_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Names_g_sampleAccessInfo.memberAccessInfos = 
                Names_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::Names);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Names_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Names_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Names_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Names_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::Names >;

                Names_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Names_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Names_g_typePlugin = 
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

                return &Names_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::AliasType& dynamic_type< ::CorpusCollectionShapes::Names_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::Names_AliasTag_t >::get())));
        }

        template<>
        struct native_type_code< ::CorpusCollectionShapes::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Sample_g_tc_unboundedItems_sequence;
                static DDS_TypeCode Sample_g_tc_nestedSequences_sequence;
                static DDS_TypeCode Sample_g_tc_rows_array =DDS_INITIALIZE_ARRAY_TYPECODE(1,2L, NULL,NULL);
                static DDS_TypeCode Sample_g_tc_sequenceOfArrays_sequence;
                static DDS_TypeCode Sample_g_tc_sequenceOfArrayAliases_sequence;

                static DDS_TypeCode_Member Sample_g_tc_members[7]=
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"nestedSequences",/* Member name */
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
                        1,
                        NULL, /* Ignored */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"sequenceOfArrayAliases",/* Member name */
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
                        (char *)"names",/* Member name */
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
                    }
                };

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusCollectionShapes::Sample", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        7, /* Number of members */
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

                Sample_g_tc_unboundedItems_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Item, 100L > >((100L));
                Sample_g_tc_nestedSequences_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollectionShapes::BoundedLongs, 2L > >((2L));
                Sample_g_tc_sequenceOfArrays_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Row, 2L > >((2L));
                Sample_g_tc_sequenceOfArrayAliases_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< ::CorpusCollectionShapes::Rows, 2L > >((2L));

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_unboundedItems_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Item>::get().native();
                Sample_g_tc_nestedSequences_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::BoundedLongs_AliasTag_t>::get().native();
                Sample_g_tc_rows_array._data._typeCode =(RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Row_AliasTag_t>::get().native();
                Sample_g_tc_sequenceOfArrays_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Row_AliasTag_t>::get().native();
                Sample_g_tc_sequenceOfArrayAliases_sequence._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Rows_AliasTag_t>::get().native();
                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_unboundedItems_sequence;
                Sample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Items_AliasTag_t>::get().native();
                Sample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_nestedSequences_sequence;
                Sample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_rows_array;
                Sample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_sequenceOfArrays_sequence;
                Sample_g_tc_members[5]._representation._typeCode = (RTICdrTypeCode *)& Sample_g_tc_sequenceOfArrayAliases_sequence;
                Sample_g_tc_members[6]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusCollectionShapes::Names_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusCollectionShapes::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[7] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusCollectionShapes::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->unboundedItems() - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boundedItems() - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->nestedSequences() - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->rows() - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->sequenceOfArrays() - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->sequenceOfArrayAliases() - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->names() - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos = 
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusCollectionShapes::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusCollectionShapes::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusCollectionShapes::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusCollectionShapes::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusCollectionShapes::Item >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusCollectionShapes::ItemPlugin_new,
                ::CorpusCollectionShapes::ItemPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusCollectionShapes::Item >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusCollectionShapes::Item& sample,
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

        void topic_type_support< ::CorpusCollectionShapes::Item >::from_cdr_buffer(::CorpusCollectionShapes::Item& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ItemPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusCollectionShapes::Item from cdr buffer");
        }

        void topic_type_support< ::CorpusCollectionShapes::Item >::reset_sample(::CorpusCollectionShapes::Item& sample) 
        {
            sample.id(0);
            sample.label("");
        }

        void topic_type_support< ::CorpusCollectionShapes::Item >::allocate_sample(::CorpusCollectionShapes::Item& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.label(),  -1, 12L);
        }
        void topic_type_support< ::CorpusCollectionShapes::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusCollectionShapes::SamplePlugin_new,
                ::CorpusCollectionShapes::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusCollectionShapes::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusCollectionShapes::Sample& sample,
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

        void topic_type_support< ::CorpusCollectionShapes::Sample >::from_cdr_buffer(::CorpusCollectionShapes::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusCollectionShapes::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusCollectionShapes::Sample >::reset_sample(::CorpusCollectionShapes::Sample& sample) 
        {
            ::rti::topic::reset_sample(sample.unboundedItems());
            ::rti::topic::reset_sample(sample.boundedItems());
            ::rti::topic::reset_sample(sample.nestedSequences());
            ::rti::topic::reset_sample(sample.rows());
            ::rti::topic::reset_sample(sample.sequenceOfArrays());
            ::rti::topic::reset_sample(sample.sequenceOfArrayAliases());
            ::rti::topic::reset_sample(sample.names());
        }

        void topic_type_support< ::CorpusCollectionShapes::Sample >::allocate_sample(::CorpusCollectionShapes::Sample& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.unboundedItems(),  100L, -1);
            ::rti::topic::allocate_sample(sample.boundedItems(),  -1, -1);
            ::rti::topic::allocate_sample(sample.nestedSequences(),  2L, -1);
            ::rti::topic::allocate_sample(sample.rows(),  -1, -1);
            ::rti::topic::allocate_sample(sample.sequenceOfArrays(),  2L, -1);
            ::rti::topic::allocate_sample(sample.sequenceOfArrayAliases(),  2L, -1);
            ::rti::topic::allocate_sample(sample.names(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
