

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-primitives.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>
#include <atomic>
#include <cmath>
#include <limits>

#ifndef NDDS_STANDALONE_TYPE
#include "rti/topic/cdr/Serialization.hpp"
#include "01-primitivesPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "01-primitives.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusPrimitives {

    // ---- Sample: 

    Sample::Sample() :
        m_signed16_ (0) ,
        m_signed32_ (0) ,
        m_signed64_ (0ll) ,
        m_unsigned16_ (0) ,
        m_unsigned32_ (0u) ,
        m_unsigned64_ (0ull) ,
        m_fixedInt8_ (0) ,
        m_fixedInt64_ (0ll) ,
        m_fixedUint8_ (0) ,
        m_fixedUint64_ (0ull) ,
        m_octetValue_ (0) ,
        m_boolValue_ (0) ,
        m_charValue_ (0) ,
        m_wideCharValue_ (0) ,
        m_floatValue_ (0.0f) ,
        m_doubleValue_ (0.0) ,
        m_longDoubleValue_ (0.0)  {

    }   

    Sample::Sample (int16_t signed16_,int32_t signed32_,int64_t signed64_,uint16_t unsigned16_,uint32_t unsigned32_,uint64_t unsigned64_,int8_t fixedInt8_,int64_t fixedInt64_,uint8_t fixedUint8_,uint64_t fixedUint64_,uint8_t octetValue_,bool boolValue_,char charValue_,wchar_t wideCharValue_,float floatValue_,double doubleValue_,const ::rti::core::LongDouble& longDoubleValue_):
        m_signed16_(signed16_), 
        m_signed32_(signed32_), 
        m_signed64_(signed64_), 
        m_unsigned16_(unsigned16_), 
        m_unsigned32_(unsigned32_), 
        m_unsigned64_(unsigned64_), 
        m_fixedInt8_(fixedInt8_), 
        m_fixedInt64_(fixedInt64_), 
        m_fixedUint8_(fixedUint8_), 
        m_fixedUint64_(fixedUint64_), 
        m_octetValue_(octetValue_), 
        m_boolValue_(boolValue_), 
        m_charValue_(charValue_), 
        m_wideCharValue_(wideCharValue_), 
        m_floatValue_(floatValue_), 
        m_doubleValue_(doubleValue_), 
        m_longDoubleValue_(longDoubleValue_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_signed16_, other_.m_signed16_);
        swap(m_signed32_, other_.m_signed32_);
        swap(m_signed64_, other_.m_signed64_);
        swap(m_unsigned16_, other_.m_unsigned16_);
        swap(m_unsigned32_, other_.m_unsigned32_);
        swap(m_unsigned64_, other_.m_unsigned64_);
        swap(m_fixedInt8_, other_.m_fixedInt8_);
        swap(m_fixedInt64_, other_.m_fixedInt64_);
        swap(m_fixedUint8_, other_.m_fixedUint8_);
        swap(m_fixedUint64_, other_.m_fixedUint64_);
        swap(m_octetValue_, other_.m_octetValue_);
        swap(m_boolValue_, other_.m_boolValue_);
        swap(m_charValue_, other_.m_charValue_);
        swap(m_wideCharValue_, other_.m_wideCharValue_);
        swap(m_floatValue_, other_.m_floatValue_);
        swap(m_doubleValue_, other_.m_doubleValue_);
        swap(m_longDoubleValue_, other_.m_longDoubleValue_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_signed16_ != other_.m_signed16_) {
            return false;
        }
        if (m_signed32_ != other_.m_signed32_) {
            return false;
        }
        if (m_signed64_ != other_.m_signed64_) {
            return false;
        }
        if (m_unsigned16_ != other_.m_unsigned16_) {
            return false;
        }
        if (m_unsigned32_ != other_.m_unsigned32_) {
            return false;
        }
        if (m_unsigned64_ != other_.m_unsigned64_) {
            return false;
        }
        if (m_fixedInt8_ != other_.m_fixedInt8_) {
            return false;
        }
        if (m_fixedInt64_ != other_.m_fixedInt64_) {
            return false;
        }
        if (m_fixedUint8_ != other_.m_fixedUint8_) {
            return false;
        }
        if (m_fixedUint64_ != other_.m_fixedUint64_) {
            return false;
        }
        if (m_octetValue_ != other_.m_octetValue_) {
            return false;
        }
        if (m_boolValue_ != other_.m_boolValue_) {
            return false;
        }
        if (m_charValue_ != other_.m_charValue_) {
            return false;
        }
        if (m_wideCharValue_ != other_.m_wideCharValue_) {
            return false;
        }
        if (std::fabs(m_floatValue_ - other_.m_floatValue_) > std::numeric_limits< float>::epsilon()
        && !(std::fabs(m_floatValue_ - other_.m_floatValue_) < (std::numeric_limits< float>::min)())) {
            return false;
        }
        if (std::fabs(m_doubleValue_ - other_.m_doubleValue_) > std::numeric_limits< double>::epsilon()
        && !(std::fabs(m_doubleValue_ - other_.m_doubleValue_) < (std::numeric_limits< double>::min)())) {
            return false;
        }
        if (m_longDoubleValue_ != other_.m_longDoubleValue_) {
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
        o << "signed16: " << sample.signed16 ()<<", ";
        o << "signed32: " << sample.signed32 ()<<", ";
        o << "signed64: " << sample.signed64 ()<<", ";
        o << "unsigned16: " << sample.unsigned16 ()<<", ";
        o << "unsigned32: " << sample.unsigned32 ()<<", ";
        o << "unsigned64: " << sample.unsigned64 ()<<", ";
        o << "fixedInt8: " << (int) sample.fixedInt8 ()<<", ";
        o << "fixedInt64: " << sample.fixedInt64 ()<<", ";
        o << "fixedUint8: " << (int) sample.fixedUint8 ()<<", ";
        o << "fixedUint64: " << sample.fixedUint64 ()<<", ";
        o << "octetValue: " << std::hex<<(int) sample.octetValue ()<<", ";
        o << "boolValue: " << sample.boolValue ()<<", ";
        o << "charValue: " << sample.charValue ()<<", ";
        o << "wideCharValue: " << std::hex<<(int) sample.wideCharValue ()<<", ";
        o << "floatValue: " << std::setprecision(9) << sample.floatValue ()<<", ";
        o << "doubleValue: " << std::setprecision(15) << sample.doubleValue ()<<", ";
        o << "longDoubleValue: " << sample.longDoubleValue ();
        o <<"]";
        return o;
    }

} // namespace CorpusPrimitives  

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
        struct native_type_code< ::CorpusPrimitives::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sample_g_tc_members[17]=
                {

                    {
                        (char *)"signed16",/* Member name */
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
                        (char *)"signed32",/* Member name */
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
                        (char *)"signed64",/* Member name */
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
                        (char *)"unsigned16",/* Member name */
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
                        (char *)"unsigned32",/* Member name */
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
                        (char *)"unsigned64",/* Member name */
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
                        (char *)"fixedInt8",/* Member name */
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
                        (char *)"fixedInt64",/* Member name */
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
                        (char *)"fixedUint8",/* Member name */
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
                        (char *)"fixedUint64",/* Member name */
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
                    }, 
                    {
                        (char *)"octetValue",/* Member name */
                        {
                            10,/* Representation ID */
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
                        (char *)"boolValue",/* Member name */
                        {
                            11,/* Representation ID */
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
                        (char *)"charValue",/* Member name */
                        {
                            12,/* Representation ID */
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
                        (char *)"wideCharValue",/* Member name */
                        {
                            13,/* Representation ID */
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
                        (char *)"floatValue",/* Member name */
                        {
                            14,/* Representation ID */
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
                        (char *)"doubleValue",/* Member name */
                        {
                            15,/* Representation ID */
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
                        (char *)"longDoubleValue",/* Member name */
                        {
                            16,/* Representation ID */
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
                        (char *)"CorpusPrimitives::Sample", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        17, /* Number of members */
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

                Sample_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_short;
                Sample_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Sample_g_tc_members[2]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_longlong;
                Sample_g_tc_members[3]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ushort;
                Sample_g_tc_members[4]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ulong;
                Sample_g_tc_members[5]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ulonglong;
                Sample_g_tc_members[6]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;
                Sample_g_tc_members[7]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_longlong;
                Sample_g_tc_members[8]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;
                Sample_g_tc_members[9]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_ulonglong;
                Sample_g_tc_members[10]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;
                Sample_g_tc_members[11]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_bool_typecode();
                Sample_g_tc_members[12]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_char;
                Sample_g_tc_members[13]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::interpreter::initialize_wchar_typecode();
                Sample_g_tc_members[14]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_float;
                Sample_g_tc_members[15]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_double;
                Sample_g_tc_members[16]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_longdouble;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[0]._annotations._defaultValue._u.short_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[0]._annotations._minValue._u.short_value = RTIXCdrShort_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_SHORT;
                Sample_g_tc_members[0]._annotations._maxValue._u.short_value = RTIXCdrShort_MAX;
                Sample_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Sample_g_tc_members[2]._annotations._defaultValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[2]._annotations._defaultValue._u.long_long_value = 0ll;
                Sample_g_tc_members[2]._annotations._minValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[2]._annotations._minValue._u.long_long_value = RTIXCdrLongLong_MIN;
                Sample_g_tc_members[2]._annotations._maxValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[2]._annotations._maxValue._u.long_long_value = RTIXCdrLongLong_MAX;
                Sample_g_tc_members[3]._annotations._defaultValue._d = RTI_XCDR_TK_USHORT;
                Sample_g_tc_members[3]._annotations._defaultValue._u.ushort_value = 0;
                Sample_g_tc_members[3]._annotations._minValue._d = RTI_XCDR_TK_USHORT;
                Sample_g_tc_members[3]._annotations._minValue._u.ushort_value = RTIXCdrUnsignedShort_MIN;
                Sample_g_tc_members[3]._annotations._maxValue._d = RTI_XCDR_TK_USHORT;
                Sample_g_tc_members[3]._annotations._maxValue._u.ushort_value = RTIXCdrUnsignedShort_MAX;
                Sample_g_tc_members[4]._annotations._defaultValue._d = RTI_XCDR_TK_ULONG;
                Sample_g_tc_members[4]._annotations._defaultValue._u.ulong_value = 0u;
                Sample_g_tc_members[4]._annotations._minValue._d = RTI_XCDR_TK_ULONG;
                Sample_g_tc_members[4]._annotations._minValue._u.ulong_value = RTIXCdrUnsignedLong_MIN;
                Sample_g_tc_members[4]._annotations._maxValue._d = RTI_XCDR_TK_ULONG;
                Sample_g_tc_members[4]._annotations._maxValue._u.ulong_value = RTIXCdrUnsignedLong_MAX;
                Sample_g_tc_members[5]._annotations._defaultValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[5]._annotations._defaultValue._u.ulong_long_value = 0ull;
                Sample_g_tc_members[5]._annotations._minValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[5]._annotations._minValue._u.ulong_long_value = RTIXCdrUnsignedLongLong_MIN;
                Sample_g_tc_members[5]._annotations._maxValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[5]._annotations._maxValue._u.ulong_long_value = RTIXCdrUnsignedLongLong_MAX;
                Sample_g_tc_members[6]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[6]._annotations._defaultValue._u.octet_value = 0;
                Sample_g_tc_members[6]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[6]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Sample_g_tc_members[6]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[6]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;
                Sample_g_tc_members[7]._annotations._defaultValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[7]._annotations._defaultValue._u.long_long_value = 0ll;
                Sample_g_tc_members[7]._annotations._minValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[7]._annotations._minValue._u.long_long_value = RTIXCdrLongLong_MIN;
                Sample_g_tc_members[7]._annotations._maxValue._d = RTI_XCDR_TK_LONGLONG;
                Sample_g_tc_members[7]._annotations._maxValue._u.long_long_value = RTIXCdrLongLong_MAX;
                Sample_g_tc_members[8]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[8]._annotations._defaultValue._u.octet_value = 0;
                Sample_g_tc_members[8]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[8]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Sample_g_tc_members[8]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[8]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;
                Sample_g_tc_members[9]._annotations._defaultValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[9]._annotations._defaultValue._u.ulong_long_value = 0ull;
                Sample_g_tc_members[9]._annotations._minValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[9]._annotations._minValue._u.ulong_long_value = RTIXCdrUnsignedLongLong_MIN;
                Sample_g_tc_members[9]._annotations._maxValue._d = RTI_XCDR_TK_ULONGLONG;
                Sample_g_tc_members[9]._annotations._maxValue._u.ulong_long_value = RTIXCdrUnsignedLongLong_MAX;
                Sample_g_tc_members[10]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[10]._annotations._defaultValue._u.octet_value = 0;
                Sample_g_tc_members[10]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[10]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Sample_g_tc_members[10]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Sample_g_tc_members[10]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;
                Sample_g_tc_members[11]._annotations._defaultValue._d = RTI_XCDR_TK_BOOLEAN;
                Sample_g_tc_members[11]._annotations._defaultValue._u.boolean_value = 0;
                Sample_g_tc_members[12]._annotations._defaultValue._d = RTI_XCDR_TK_CHAR;
                Sample_g_tc_members[12]._annotations._defaultValue._u.char_value = 0;
                Sample_g_tc_members[13]._annotations._defaultValue._d = RTI_XCDR_TK_WCHAR;
                Sample_g_tc_members[13]._annotations._defaultValue._u.wchar_value = 0;
                Sample_g_tc_members[14]._annotations._defaultValue._d = RTI_XCDR_TK_FLOAT;
                Sample_g_tc_members[14]._annotations._defaultValue._u.float_value = 0.0f;
                Sample_g_tc_members[14]._annotations._minValue._d = RTI_XCDR_TK_FLOAT;
                Sample_g_tc_members[14]._annotations._minValue._u.float_value = RTIXCdrFloat_MIN;
                Sample_g_tc_members[14]._annotations._maxValue._d = RTI_XCDR_TK_FLOAT;
                Sample_g_tc_members[14]._annotations._maxValue._u.float_value = RTIXCdrFloat_MAX;
                Sample_g_tc_members[15]._annotations._defaultValue._d = RTI_XCDR_TK_DOUBLE;
                Sample_g_tc_members[15]._annotations._defaultValue._u.double_value = 0.0;
                Sample_g_tc_members[15]._annotations._minValue._d = RTI_XCDR_TK_DOUBLE;
                Sample_g_tc_members[15]._annotations._minValue._u.double_value = RTIXCdrDouble_MIN;
                Sample_g_tc_members[15]._annotations._maxValue._d = RTI_XCDR_TK_DOUBLE;
                Sample_g_tc_members[15]._annotations._maxValue._u.double_value = RTIXCdrDouble_MAX;

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusPrimitives::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[17] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusPrimitives::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->signed16() - (char *)sample);

                Sample_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->signed32() - (char *)sample);

                Sample_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->signed64() - (char *)sample);

                Sample_g_memberAccessInfos[3].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->unsigned16() - (char *)sample);

                Sample_g_memberAccessInfos[4].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->unsigned32() - (char *)sample);

                Sample_g_memberAccessInfos[5].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->unsigned64() - (char *)sample);

                Sample_g_memberAccessInfos[6].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->fixedInt8() - (char *)sample);

                Sample_g_memberAccessInfos[7].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->fixedInt64() - (char *)sample);

                Sample_g_memberAccessInfos[8].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->fixedUint8() - (char *)sample);

                Sample_g_memberAccessInfos[9].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->fixedUint64() - (char *)sample);

                Sample_g_memberAccessInfos[10].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->octetValue() - (char *)sample);

                Sample_g_memberAccessInfos[11].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->boolValue() - (char *)sample);

                Sample_g_memberAccessInfos[12].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->charValue() - (char *)sample);

                Sample_g_memberAccessInfos[13].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->wideCharValue() - (char *)sample);

                Sample_g_memberAccessInfos[14].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->floatValue() - (char *)sample);

                Sample_g_memberAccessInfos[15].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->doubleValue() - (char *)sample);

                Sample_g_memberAccessInfos[16].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->longDoubleValue() - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos = 
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusPrimitives::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusPrimitives::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusPrimitives::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusPrimitives::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusPrimitives::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusPrimitives::SamplePlugin_new,
                ::CorpusPrimitives::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusPrimitives::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusPrimitives::Sample& sample,
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

        void topic_type_support< ::CorpusPrimitives::Sample >::from_cdr_buffer(::CorpusPrimitives::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusPrimitives::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusPrimitives::Sample >::reset_sample(::CorpusPrimitives::Sample& sample) 
        {
            sample.signed16(0);
            sample.signed32(0);
            sample.signed64(0ll);
            sample.unsigned16(0);
            sample.unsigned32(0u);
            sample.unsigned64(0ull);
            sample.fixedInt8(0);
            sample.fixedInt64(0ll);
            sample.fixedUint8(0);
            sample.fixedUint64(0ull);
            sample.octetValue(0);
            sample.boolValue(0);
            sample.charValue(0);
            sample.wideCharValue(0);
            sample.floatValue(0.0f);
            sample.doubleValue(0.0);
            ::rti::topic::reset_sample(sample.longDoubleValue());
        }

        void topic_type_support< ::CorpusPrimitives::Sample >::allocate_sample(::CorpusPrimitives::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
