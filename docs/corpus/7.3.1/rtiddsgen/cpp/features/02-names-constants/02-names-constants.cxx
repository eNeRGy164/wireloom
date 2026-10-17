

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-names-constants.idl
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
#include "02-names-constantsPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "02-names-constants.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusNames {

    std::ostream& operator << (std::ostream& o,const State& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case State::Idle:
            o << "State::Idle" << " ";
            break;
            case State::Running:
            o << "State::Running" << " ";
            break;
            case State::Complete:
            o << "State::Complete" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }

    namespace Nested {

        // ---- Point: 

        Point::Point() :
            m_x_ (0) ,
            m_y_ (0)  {

        }   

        Point::Point (int32_t x_,int32_t y_):
            m_x_(x_), 
            m_y_(y_) {
        }

        void Point::swap(Point& other_)  noexcept 
        {
            using std::swap;
            swap(m_x_, other_.m_x_);
            swap(m_y_, other_.m_y_);
        }  

        bool Point::operator == (const Point& other_) const {
            if (m_x_ != other_.m_x_) {
                return false;
            }
            if (m_y_ != other_.m_y_) {
                return false;
            }
            return true;
        }

        bool Point::operator != (const Point& other_) const {
            return !this->operator ==(other_);
        }

        std::ostream& operator << (std::ostream& o,const Point& sample)
        {
            ::rti::util::StreamFlagSaver flag_saver (o);
            o <<"[";
            o << "x: " << sample.x ()<<", ";
            o << "y: " << sample.y ();
            o <<"]";
            return o;
        }

    } // namespace Nested  

    // ---- KeywordRecord: 

    KeywordRecord::KeywordRecord() :
        m_event_ (0) ,
        m_state_(CorpusNames::State::Idle)  {

    }   

    KeywordRecord::KeywordRecord (int32_t event_,const ::CorpusNames::State& state_,const ::CorpusNames::Nested::Point& point_):
        m_event_(event_), 
        m_state_(state_), 
        m_point_(point_) {
    }

    void KeywordRecord::swap(KeywordRecord& other_)  noexcept 
    {
        using std::swap;
        swap(m_event_, other_.m_event_);
        swap(m_state_, other_.m_state_);
        swap(m_point_, other_.m_point_);
    }  

    bool KeywordRecord::operator == (const KeywordRecord& other_) const {
        if (m_event_ != other_.m_event_) {
            return false;
        }
        if (m_state_ != other_.m_state_) {
            return false;
        }
        if (m_point_ != other_.m_point_) {
            return false;
        }
        return true;
    }

    bool KeywordRecord::operator != (const KeywordRecord& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const KeywordRecord& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "event: " << sample.event ()<<", ";
        o << "state: " << sample.state ()<<", ";
        o << "point: " << sample.point ();
        o <<"]";
        return o;
    }

} // namespace CorpusNames  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusNames::State default_enumerator< ::CorpusNames::State>::value = ::CorpusNames::State::Idle;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusNames::State default_enumerator< ::CorpusNames::State>::value = ::CorpusNames::State::Idle;
        template<>
        struct native_type_code< ::CorpusNames::State > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member State_g_tc_members[3]=
                {

                    {
                        (char *)"Idle",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusNames::State::Idle), 
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
                        (char *)"Running",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusNames::State::Running), 
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
                        (char *)"Complete",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusNames::State::Complete), 
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
                        (char *)"CorpusNames::State", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
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
                    size_t candidateTypeSize = sizeof(::CorpusNames::State);

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
                interpreter::get_aggregation_value_pointer< ::CorpusNames::State >;

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

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusNames::State >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusNames::State >::get())));
        }

        template<>
        struct native_type_code< ::CorpusNames::Nested::Point > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Point_g_tc_members[2]=
                {

                    {
                        (char *)"x",/* Member name */
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
                        (char *)"y",/* Member name */
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

                static DDS_TypeCode Point_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNames::Nested::Point", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Point_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Point*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Point_g_tc;
                }

                Point_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Point_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Point_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Point_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Point_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Point_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Point_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Point_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Point_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Point_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Point_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Point_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Point_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Point_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Point_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Point_g_tc._data._sampleAccessInfo = sample_access_info();
                Point_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Point_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusNames::Nested::Point *sample;

                static RTIXCdrMemberAccessInfo Point_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Point_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Point_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusNames::Nested::Point);
                if (sample == NULL) {
                    return NULL;
                }

                Point_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->x() - (char *)sample);

                Point_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->y() - (char *)sample);

                Point_g_sampleAccessInfo.memberAccessInfos = 
                Point_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusNames::Nested::Point);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Point_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Point_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Point_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Point_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusNames::Nested::Point >;

                Point_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Point_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Point_g_typePlugin = 
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

                return &Point_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusNames::Nested::Point >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusNames::Nested::Point >::get())));
        }

        template<>
        struct native_type_code< ::CorpusNames::KeywordRecord > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member KeywordRecord_g_tc_members[3]=
                {

                    {
                        (char *)"event",/* Member name */
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"point",/* Member name */
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

                static DDS_TypeCode KeywordRecord_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusNames::KeywordRecord", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        3, /* Number of members */
                        KeywordRecord_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for KeywordRecord*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &KeywordRecord_g_tc;
                }

                KeywordRecord_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                KeywordRecord_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                KeywordRecord_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusNames::State>::get().native();
                KeywordRecord_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusNames::Nested::Point>::get().native();

                /* Initialize the values for member annotations. */
                KeywordRecord_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                KeywordRecord_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                KeywordRecord_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                KeywordRecord_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                KeywordRecord_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                KeywordRecord_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                KeywordRecord_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                KeywordRecord_g_tc_members[1]._annotations._defaultValue._u.enumerated_value = 0;

                KeywordRecord_g_tc._data._sampleAccessInfo = sample_access_info();
                KeywordRecord_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &KeywordRecord_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusNames::KeywordRecord *sample;

                static RTIXCdrMemberAccessInfo KeywordRecord_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo KeywordRecord_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &KeywordRecord_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusNames::KeywordRecord);
                if (sample == NULL) {
                    return NULL;
                }

                KeywordRecord_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->event() - (char *)sample);

                KeywordRecord_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->state() - (char *)sample);

                KeywordRecord_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->point() - (char *)sample);

                KeywordRecord_g_sampleAccessInfo.memberAccessInfos = 
                KeywordRecord_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusNames::KeywordRecord);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        KeywordRecord_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        KeywordRecord_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                KeywordRecord_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                KeywordRecord_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusNames::KeywordRecord >;

                KeywordRecord_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &KeywordRecord_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin KeywordRecord_g_typePlugin = 
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

                return &KeywordRecord_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusNames::KeywordRecord >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusNames::KeywordRecord >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusNames::Nested::Point >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusNames::Nested::PointPlugin_new,
                ::CorpusNames::Nested::PointPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusNames::Nested::Point >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusNames::Nested::Point& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = PointPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = PointPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusNames::Nested::Point >::from_cdr_buffer(::CorpusNames::Nested::Point& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = PointPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusNames::Nested::Point from cdr buffer");
        }

        void topic_type_support< ::CorpusNames::Nested::Point >::reset_sample(::CorpusNames::Nested::Point& sample) 
        {
            sample.x(0);
            sample.y(0);
        }

        void topic_type_support< ::CorpusNames::Nested::Point >::allocate_sample(::CorpusNames::Nested::Point& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusNames::KeywordRecord >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusNames::KeywordRecordPlugin_new,
                ::CorpusNames::KeywordRecordPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusNames::KeywordRecord >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusNames::KeywordRecord& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = KeywordRecordPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = KeywordRecordPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusNames::KeywordRecord >::from_cdr_buffer(::CorpusNames::KeywordRecord& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = KeywordRecordPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusNames::KeywordRecord from cdr buffer");
        }

        void topic_type_support< ::CorpusNames::KeywordRecord >::reset_sample(::CorpusNames::KeywordRecord& sample) 
        {
            sample.event(0);
            sample.state(CorpusNames::State::Idle);
            ::rti::topic::reset_sample(sample.point());
        }

        void topic_type_support< ::CorpusNames::KeywordRecord >::allocate_sample(::CorpusNames::KeywordRecord& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.state(),  -1, -1);
            ::rti::topic::allocate_sample(sample.point(),  -1, -1);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
