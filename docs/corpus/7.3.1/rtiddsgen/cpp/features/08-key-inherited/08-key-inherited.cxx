

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-inherited.idl
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
#include "08-key-inheritedPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "08-key-inherited.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusInheritedKeys {

    // ---- Base: 

    Base::Base() :
        m_tenant_ (0) ,
        m_baseValue_ (0)  {

    }   

    Base::Base (int32_t tenant_,int32_t baseValue_):
        m_tenant_(tenant_), 
        m_baseValue_(baseValue_) {
    }

    void Base::swap(Base& other_)  noexcept 
    {
        using std::swap;
        swap(m_tenant_, other_.m_tenant_);
        swap(m_baseValue_, other_.m_baseValue_);
    }  

    bool Base::operator == (const Base& other_) const {
        if (m_tenant_ != other_.m_tenant_) {
            return false;
        }
        if (m_baseValue_ != other_.m_baseValue_) {
            return false;
        }
        return true;
    }

    bool Base::operator != (const Base& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Base& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "tenant: " << sample.tenant ()<<", ";
        o << "baseValue: " << sample.baseValue ();
        o <<"]";
        return o;
    }

    // ---- Derived: 

    Derived::Derived() :
        m_localId_ (0) ,
        m_payload_ (0)  {

    }   

    Derived::Derived (int32_t tenant_,int32_t baseValue_,int32_t localId_,int32_t payload_) :
        ::CorpusInheritedKeys::Base(tenant_, baseValue_), 
        m_localId_(localId_), 
        m_payload_(payload_) {
    }

    void Derived::swap(Derived& other_)  noexcept 
    {
        using std::swap;
        ::CorpusInheritedKeys::Base::swap(other_);
        swap(m_localId_, other_.m_localId_);
        swap(m_payload_, other_.m_payload_);
    }  

    bool Derived::operator == (const Derived& other_) const {
        if (!::CorpusInheritedKeys::Base::operator == (other_)){
            return false;
        }
        if (m_localId_ != other_.m_localId_) {
            return false;
        }
        if (m_payload_ != other_.m_payload_) {
            return false;
        }
        return true;
    }

    bool Derived::operator != (const Derived& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Derived& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << static_cast<const ::CorpusInheritedKeys::Base &>(sample);
        o << "localId: " << sample.localId ()<<", ";
        o << "payload: " << sample.payload ();
        o <<"]";
        return o;
    }

} // namespace CorpusInheritedKeys  

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
        struct native_type_code< ::CorpusInheritedKeys::Base > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Base_g_tc_members[2]=
                {

                    {
                        (char *)"tenant",/* Member name */
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"baseValue",/* Member name */
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

                static DDS_TypeCode Base_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInheritedKeys::Base", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
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

                Base_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Base_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Base_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Base_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Base_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Base_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Base_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Base_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Base_g_tc._data._sampleAccessInfo = sample_access_info();
                Base_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Base_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInheritedKeys::Base *sample;

                static RTIXCdrMemberAccessInfo Base_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Base_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Base_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInheritedKeys::Base);
                if (sample == NULL) {
                    return NULL;
                }

                Base_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->tenant() - (char *)sample);

                Base_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->baseValue() - (char *)sample);

                Base_g_sampleAccessInfo.memberAccessInfos = 
                Base_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInheritedKeys::Base);

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
                interpreter::get_aggregation_value_pointer< ::CorpusInheritedKeys::Base >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInheritedKeys::Base >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInheritedKeys::Base >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInheritedKeys::Derived > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Derived_g_tc_members[2]=
                {

                    {
                        (char *)"localId",/* Member name */
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
                        RTI_CDR_KEY_MEMBER , /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"payload",/* Member name */
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

                static DDS_TypeCode Derived_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInheritedKeys::Derived", /* Name */
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

                Derived_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Derived_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;
                Derived_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Derived_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Derived_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Derived_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;
                Derived_g_tc_members[1]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[1]._annotations._defaultValue._u.long_value = 0;
                Derived_g_tc_members[1]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[1]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Derived_g_tc_members[1]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Derived_g_tc_members[1]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Derived_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInheritedKeys::Base >::get().native(); /* Base class */

                Derived_g_tc._data._sampleAccessInfo = sample_access_info();
                Derived_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Derived_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInheritedKeys::Derived *sample;

                static RTIXCdrMemberAccessInfo Derived_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Derived_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInheritedKeys::Derived);
                if (sample == NULL) {
                    return NULL;
                }

                Derived_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->localId() - (char *)sample);

                Derived_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->payload() - (char *)sample);

                Derived_g_sampleAccessInfo.memberAccessInfos = 
                Derived_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInheritedKeys::Derived);

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
                interpreter::get_aggregation_value_pointer< ::CorpusInheritedKeys::Derived >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInheritedKeys::Derived >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInheritedKeys::Derived >::get())));
        }
    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< ::CorpusInheritedKeys::Base >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInheritedKeys::BasePlugin_new,
                ::CorpusInheritedKeys::BasePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInheritedKeys::Base >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInheritedKeys::Base& sample,
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

        void topic_type_support< ::CorpusInheritedKeys::Base >::from_cdr_buffer(::CorpusInheritedKeys::Base& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = BasePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInheritedKeys::Base from cdr buffer");
        }

        void topic_type_support< ::CorpusInheritedKeys::Base >::reset_sample(::CorpusInheritedKeys::Base& sample) 
        {
            sample.tenant(0);
            sample.baseValue(0);
        }

        void topic_type_support< ::CorpusInheritedKeys::Base >::allocate_sample(::CorpusInheritedKeys::Base& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
        void topic_type_support< ::CorpusInheritedKeys::Derived >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInheritedKeys::DerivedPlugin_new,
                ::CorpusInheritedKeys::DerivedPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInheritedKeys::Derived >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInheritedKeys::Derived& sample,
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

        void topic_type_support< ::CorpusInheritedKeys::Derived >::from_cdr_buffer(::CorpusInheritedKeys::Derived& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DerivedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInheritedKeys::Derived from cdr buffer");
        }

        void topic_type_support< ::CorpusInheritedKeys::Derived >::reset_sample(::CorpusInheritedKeys::Derived& sample) 
        {
            // Initialize base
            topic_type_support< ::CorpusInheritedKeys::Base >::reset_sample(sample);

            sample.localId(0);
            sample.payload(0);
        }

        void topic_type_support< ::CorpusInheritedKeys::Derived >::allocate_sample(::CorpusInheritedKeys::Derived& sample, int, int) 
        {
            // Initialize base
            topic_type_support< ::CorpusInheritedKeys::Base >::allocate_sample(sample, -1, -1);

        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
