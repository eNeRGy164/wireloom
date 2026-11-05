

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-scopes.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>

#include "rti/topic/cdr/Serialization.hpp"

#include "02-scopes.hpp"
#include "02-scopesPlugin.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusScopes {

    namespace Nested {

        // ---- Point: 

        Point::Point() :
            m_x_ (0) ,
            m_y_ (0)  {
        }   

        Point::Point (int32_t x,int32_t y):
            m_x_( x ),
            m_y_( y ) {
        }

        #ifdef RTI_CXX11_RVALUE_REFERENCES
        #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
        Point::Point(Point&& other_) OMG_NOEXCEPT  :m_x_ (std::move(other_.m_x_))
        ,
        m_y_ (std::move(other_.m_y_))
        {
        } 

        Point& Point::operator=(Point&&  other_) OMG_NOEXCEPT {
            Point tmp(std::move(other_));
            swap(tmp); 
            return *this;
        }
        #endif
        #endif   

        void Point::swap(Point& other_)  OMG_NOEXCEPT 
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
            o << "x: " << sample.x()<<", ";
            o << "y: " << sample.y() ;
            o <<"]";
            return o;
        }

    } // namespace Nested  

    // ---- KeywordRecord: 

    KeywordRecord::KeywordRecord() :
        m_event_ (0)  {
    }   

    KeywordRecord::KeywordRecord (int32_t event,const CorpusScopes::Nested::Point& relative,const CorpusScopes::Nested::Point& absolute,const ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) >& values):
        m_event_( event ),
        m_relative_( relative ),
        m_absolute_( absolute ),
        m_values_( values ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    KeywordRecord::KeywordRecord(KeywordRecord&& other_) OMG_NOEXCEPT  :m_event_ (std::move(other_.m_event_))
    ,
    m_relative_ (std::move(other_.m_relative_))
    ,
    m_absolute_ (std::move(other_.m_absolute_))
    ,
    m_values_ (std::move(other_.m_values_))
    {
    } 

    KeywordRecord& KeywordRecord::operator=(KeywordRecord&&  other_) OMG_NOEXCEPT {
        KeywordRecord tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void KeywordRecord::swap(KeywordRecord& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_event_, other_.m_event_);
        swap(m_relative_, other_.m_relative_);
        swap(m_absolute_, other_.m_absolute_);
        swap(m_values_, other_.m_values_);
    }  

    bool KeywordRecord::operator == (const KeywordRecord& other_) const {
        if (m_event_ != other_.m_event_) {
            return false;
        }
        if (m_relative_ != other_.m_relative_) {
            return false;
        }
        if (m_absolute_ != other_.m_absolute_) {
            return false;
        }
        if (m_values_ != other_.m_values_) {
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
        o << "event: " << sample.event()<<", ";
        o << "relative: " << sample.relative()<<", ";
        o << "absolute: " << sample.absolute()<<", ";
        o << "values: " << sample.values() ;
        o <<"]";
        return o;
    }

} // namespace CorpusScopes  

namespace CorpusScopes {

    // ---- Reopened: 

    Reopened::Reopened()  {
    }   

    Reopened::Reopened (const CorpusScopes::KeywordRecord& value):
        m_value_( value ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Reopened::Reopened(Reopened&& other_) OMG_NOEXCEPT  :m_value_ (std::move(other_.m_value_))
    {
    } 

    Reopened& Reopened::operator=(Reopened&&  other_) OMG_NOEXCEPT {
        Reopened tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void Reopened::swap(Reopened& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_value_, other_.m_value_);
    }  

    bool Reopened::operator == (const Reopened& other_) const {
        if (m_value_ != other_.m_value_) {
            return false;
        }
        return true;
    }
    bool Reopened::operator != (const Reopened& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Reopened& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "value: " << sample.value() ;
        o <<"]";
        return o;
    }

} // namespace CorpusScopes  

// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusScopes::Nested::Point > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

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
                        (char *)"CorpusScopes::Nested::Point", /* Name */
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

                if (is_initialized) {
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

                is_initialized = RTI_TRUE;

                return &Point_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusScopes::Nested::Point *sample;

                static RTIXCdrMemberAccessInfo Point_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Point_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Point_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusScopes::Nested::Point);
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
                    size_t candidateTypeSize = sizeof(CorpusScopes::Nested::Point);

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
                interpreter::get_aggregation_value_pointer< CorpusScopes::Nested::Point >;

                Point_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
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
                    NULL
                };

                return &Point_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusScopes::Nested::Point >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusScopes::Nested::Point >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusScopes::KeywordRecord > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode KeywordRecord_g_tc_values_sequence;

                static DDS_TypeCode_Member KeywordRecord_g_tc_members[4]=
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
                        (char *)"relative",/* Member name */
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
                        (char *)"absolute",/* Member name */
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
                        (char *)"values",/* Member name */
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
                    }
                };

                static DDS_TypeCode KeywordRecord_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusScopes::KeywordRecord", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        KeywordRecord_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for KeywordRecord*/

                if (is_initialized) {
                    return &KeywordRecord_g_tc;
                }

                KeywordRecord_g_tc_values_sequence = initialize_sequence_typecode< ::rti::core::bounded_sequence< int32_t, (CorpusScopes::Bound) > >(((CorpusScopes::Bound)));

                KeywordRecord_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                KeywordRecord_g_tc_values_sequence._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                KeywordRecord_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                KeywordRecord_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusScopes::Nested::Point>::get().native();
                KeywordRecord_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusScopes::Nested::Point>::get().native();
                KeywordRecord_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)& KeywordRecord_g_tc_values_sequence;

                /* Initialize the values for member annotations. */
                KeywordRecord_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                KeywordRecord_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                KeywordRecord_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                KeywordRecord_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                KeywordRecord_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                KeywordRecord_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                KeywordRecord_g_tc._data._sampleAccessInfo = sample_access_info();
                KeywordRecord_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &KeywordRecord_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusScopes::KeywordRecord *sample;

                static RTIXCdrMemberAccessInfo KeywordRecord_g_memberAccessInfos[4] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo KeywordRecord_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &KeywordRecord_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusScopes::KeywordRecord);
                if (sample == NULL) {
                    return NULL;
                }

                KeywordRecord_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->event() - (char *)sample);

                KeywordRecord_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->relative() - (char *)sample);

                KeywordRecord_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->absolute() - (char *)sample);

                KeywordRecord_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->values() - (char *)sample);

                KeywordRecord_g_sampleAccessInfo.memberAccessInfos = 
                KeywordRecord_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusScopes::KeywordRecord);

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
                interpreter::get_aggregation_value_pointer< CorpusScopes::KeywordRecord >;

                KeywordRecord_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
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
                    NULL
                };

                return &KeywordRecord_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusScopes::KeywordRecord >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusScopes::KeywordRecord >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusScopes::Reopened > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member Reopened_g_tc_members[1]=
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

                static DDS_TypeCode Reopened_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusScopes::Reopened", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Reopened_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Reopened*/

                if (is_initialized) {
                    return &Reopened_g_tc;
                }

                Reopened_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Reopened_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusScopes::KeywordRecord>::get().native();

                /* Initialize the values for member annotations. */

                Reopened_g_tc._data._sampleAccessInfo = sample_access_info();
                Reopened_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Reopened_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusScopes::Reopened *sample;

                static RTIXCdrMemberAccessInfo Reopened_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Reopened_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Reopened_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusScopes::Reopened);
                if (sample == NULL) {
                    return NULL;
                }

                Reopened_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->value() - (char *)sample);

                Reopened_g_sampleAccessInfo.memberAccessInfos = 
                Reopened_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusScopes::Reopened);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Reopened_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Reopened_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Reopened_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Reopened_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusScopes::Reopened >;

                Reopened_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &Reopened_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Reopened_g_typePlugin = 
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
                    NULL
                };

                return &Reopened_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusScopes::Reopened >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusScopes::Reopened >::get())));
        }

    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< CorpusScopes::Nested::Point >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusScopes::Nested::PointPlugin_new,
                CorpusScopes::Nested::PointPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusScopes::Nested::Point >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusScopes::Nested::Point& sample,
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

        void topic_type_support< CorpusScopes::Nested::Point >::from_cdr_buffer(CorpusScopes::Nested::Point& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = PointPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusScopes::Nested::Point from cdr buffer");
        }

        void topic_type_support< CorpusScopes::Nested::Point >::reset_sample(CorpusScopes::Nested::Point& sample) 
        {
            sample.x(0);
            sample.y(0);
        }

        void topic_type_support< CorpusScopes::Nested::Point >::allocate_sample(CorpusScopes::Nested::Point& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< CorpusScopes::KeywordRecord >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusScopes::KeywordRecordPlugin_new,
                CorpusScopes::KeywordRecordPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusScopes::KeywordRecord >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusScopes::KeywordRecord& sample,
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

        void topic_type_support< CorpusScopes::KeywordRecord >::from_cdr_buffer(CorpusScopes::KeywordRecord& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = KeywordRecordPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusScopes::KeywordRecord from cdr buffer");
        }

        void topic_type_support< CorpusScopes::KeywordRecord >::reset_sample(CorpusScopes::KeywordRecord& sample) 
        {
            sample.event(0);
            ::rti::topic::reset_sample(sample.relative());
            ::rti::topic::reset_sample(sample.absolute());
            ::rti::topic::reset_sample(sample.values());
        }

        void topic_type_support< CorpusScopes::KeywordRecord >::allocate_sample(CorpusScopes::KeywordRecord& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.relative(),  -1, -1);
            ::rti::topic::allocate_sample(sample.absolute(),  -1, -1);
            ::rti::topic::allocate_sample(sample.values(),  (CorpusScopes::Bound), -1);
        }

        void topic_type_support< CorpusScopes::Reopened >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusScopes::ReopenedPlugin_new,
                CorpusScopes::ReopenedPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusScopes::Reopened >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusScopes::Reopened& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ReopenedPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ReopenedPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusScopes::Reopened >::from_cdr_buffer(CorpusScopes::Reopened& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ReopenedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusScopes::Reopened from cdr buffer");
        }

        void topic_type_support< CorpusScopes::Reopened >::reset_sample(CorpusScopes::Reopened& sample) 
        {
            ::rti::topic::reset_sample(sample.value());
        }

        void topic_type_support< CorpusScopes::Reopened >::allocate_sample(CorpusScopes::Reopened& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.value(),  -1, -1);
        }

    }
}  

