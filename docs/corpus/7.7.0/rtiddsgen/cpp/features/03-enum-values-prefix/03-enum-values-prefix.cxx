

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-prefix.idl
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
#include "03-enum-values-prefixPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "03-enum-values-prefix.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusEnumValuePrefix {
    std::ostream& operator << (std::ostream& o,const Annotated& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        switch(sample){
            case Annotated::FIRST:
            o << "Annotated::FIRST" << " ";
            break;
            case Annotated::SECOND:
            o << "Annotated::SECOND" << " ";
            break;
            default:
            {
                /* Prevents compiler warnings */
            }
        }
        return o;
    }
} // namespace CorpusEnumValuePrefix  

#ifdef NDDS_STANDALONE_TYPE
namespace rti {
    namespace topic {
        const ::CorpusEnumValuePrefix::Annotated default_enumerator< ::CorpusEnumValuePrefix::Annotated>::value = ::CorpusEnumValuePrefix::Annotated::FIRST;
    }
}

#else
// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        const ::CorpusEnumValuePrefix::Annotated default_enumerator< ::CorpusEnumValuePrefix::Annotated>::value = ::CorpusEnumValuePrefix::Annotated::FIRST;

        template<>
        struct native_type_code< ::CorpusEnumValuePrefix::Annotated > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Annotated_g_tc_members[2]=
                {

                    {
                        (char *)"FIRST",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumValuePrefix::Annotated::FIRST),
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"SECOND",/* Member name */
                        {
                            0, /* Ignored */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        static_cast<int>(::CorpusEnumValuePrefix::Annotated::SECOND),
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        RTI_CDR_REQUIRED_MEMBER, /* Is a key? */
                        DDS_PRIVATE_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Annotated_g_tc =
                {{
                        DDS_TK_ENUM, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusEnumValuePrefix::Annotated", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Annotated_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Annotated*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Annotated_g_tc;
                }

                Annotated_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                /* Initialize the values for annotations. */
                Annotated_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_ENUM;
                Annotated_g_tc._data._annotations._defaultValue._u.long_value = 7;

                Annotated_g_tc._data._sampleAccessInfo = sample_access_info();
                Annotated_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Annotated_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrMemberAccessInfo Annotated_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Annotated_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Annotated_g_sampleAccessInfo;
                }

                Annotated_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Annotated_g_sampleAccessInfo.memberAccessInfos =
                Annotated_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusEnumValuePrefix::Annotated);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Annotated_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Annotated_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Annotated_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Annotated_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusEnumValuePrefix::Annotated >;

                Annotated_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Annotated_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Annotated_g_typePlugin =
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

                return &Annotated_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::EnumType& dynamic_type< ::CorpusEnumValuePrefix::Annotated >::get()
        {
            return static_cast<const ::dds::core::xtypes::EnumType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusEnumValuePrefix::Annotated >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
    }
}  

#endif // NDDS_STANDALONE_TYPE
