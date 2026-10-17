

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-explicit.idl
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
#include "03-enum-values-explicitPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "03-enum-values-explicit.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusEnumValues {

    std::ostream& operator << (std::ostream& o,const Explicit& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case Explicit::NEGATIVE:
            o << "Explicit::NEGATIVE" << " ";
            break;
            case Explicit::ZERO:
            o << "Explicit::ZERO" << " ";
            break;
            case Explicit::GAP:
            o << "Explicit::GAP" << " ";
            break;
            case Explicit::NEXT:
            o << "Explicit::NEXT" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }

} // namespace CorpusEnumValues  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusEnumValues::Explicit default_enumerator< ::CorpusEnumValues::Explicit>::value = ::CorpusEnumValues::Explicit::NEGATIVE;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusEnumValues::Explicit default_enumerator< ::CorpusEnumValues::Explicit>::value = ::CorpusEnumValues::Explicit::NEGATIVE;
        template<>
        struct native_type_code< ::CorpusEnumValues::Explicit > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Explicit_g_tc_members[4]=
                {

                    {
                        (char *)"NEGATIVE",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumValues::Explicit::NEGATIVE), 
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
                        (char *)"ZERO",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumValues::Explicit::ZERO), 
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
                        (char *)"GAP",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumValues::Explicit::GAP), 
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
                        (char *)"NEXT",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumValues::Explicit::NEXT), 
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

                static DDS_TypeCode Explicit_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusEnumValues::Explicit", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        4, /* Number of members */
                        Explicit_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Explicit*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Explicit_g_tc;
                }

                Explicit_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                /* Initialize the values for annotations. */
                Explicit_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Explicit_g_tc._data._annotations._defaultValue._u.long_value = -2;

                Explicit_g_tc._data._sampleAccessInfo = sample_access_info();
                Explicit_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Explicit_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Explicit_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Explicit_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Explicit_g_sampleAccessInfo;
                }

                Explicit_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Explicit_g_sampleAccessInfo.memberAccessInfos = 
                Explicit_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnumValues::Explicit);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Explicit_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Explicit_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Explicit_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Explicit_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusEnumValues::Explicit >;

                Explicit_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Explicit_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Explicit_g_typePlugin = 
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

                return &Explicit_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusEnumValues::Explicit >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnumValues::Explicit >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
    }
}  

#endif // NDDS_STANDALONE_TYPE
