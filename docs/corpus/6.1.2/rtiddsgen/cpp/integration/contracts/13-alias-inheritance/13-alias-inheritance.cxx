

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#include <iosfwd>
#include <iomanip>

#include "rti/topic/cdr/Serialization.hpp"

#include "13-alias-inheritance.hpp"
#include "13-alias-inheritancePlugin.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusIntegrationTrace {

    // ---- Context: 

    Context::Context() :
        m_correlation_ ("")  {
    }   

    Context::Context (const std::string& correlation,const ::dds::core::optional< std::string >& producer):
        m_correlation_( correlation ),
        m_producer_( producer ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Context::Context(Context&& other_) OMG_NOEXCEPT  :m_correlation_ (std::move(other_.m_correlation_))
    ,
    m_producer_ (std::move(other_.m_producer_))
    {
    } 

    Context& Context::operator=(Context&&  other_) OMG_NOEXCEPT {
        Context tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void Context::swap(Context& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_correlation_, other_.m_correlation_);
        swap(m_producer_, other_.m_producer_);
    }  

    bool Context::operator == (const Context& other_) const {
        if (m_correlation_ != other_.m_correlation_) {
            return false;
        }
        if (m_producer_ != other_.m_producer_) {
            return false;
        }
        return true;
    }
    bool Context::operator != (const Context& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Context& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "correlation: " << sample.correlation()<<", ";
        o << "producer: " << sample.producer() ;
        o <<"]";
        return o;
    }

    // ---- Base: 

    Base::Base() :
        m_baseValue_ (0)  {
    }   

    Base::Base (int32_t baseValue):
        m_baseValue_( baseValue ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Base::Base(Base&& other_) OMG_NOEXCEPT  :m_baseValue_ (std::move(other_.m_baseValue_))
    {
    } 

    Base& Base::operator=(Base&&  other_) OMG_NOEXCEPT {
        Base tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void Base::swap(Base& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        swap(m_baseValue_, other_.m_baseValue_);
    }  

    bool Base::operator == (const Base& other_) const {
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
        o << "baseValue: " << sample.baseValue() ;
        o <<"]";
        return o;
    }

    // ---- Derived: 

    Derived::Derived() :
        m_state_ ("")  {
    }   

    Derived::Derived (int32_t baseValue,const std::string& state,const CorpusIntegrationTrace::Context& traceContext) :
        CorpusIntegrationTrace::Base(baseValue ),
        m_state_( state ),
        m_traceContext_( traceContext ) {
    }

    #ifdef RTI_CXX11_RVALUE_REFERENCES
    #ifdef RTI_CXX11_NO_IMPLICIT_MOVE_OPERATIONS
    Derived::Derived(Derived&& other_) OMG_NOEXCEPT  : 
    CorpusIntegrationTrace::Base(std::move(other_)),
    m_state_ (std::move(other_.m_state_))
    ,
    m_traceContext_ (std::move(other_.m_traceContext_))
    {
    } 

    Derived& Derived::operator=(Derived&&  other_) OMG_NOEXCEPT {
        Derived tmp(std::move(other_));
        swap(tmp); 
        return *this;
    }
    #endif
    #endif   

    void Derived::swap(Derived& other_)  OMG_NOEXCEPT 
    {
        using std::swap;
        CorpusIntegrationTrace::Base::swap(other_);
        swap(m_state_, other_.m_state_);
        swap(m_traceContext_, other_.m_traceContext_);
    }  

    bool Derived::operator == (const Derived& other_) const {
        if (!CorpusIntegrationTrace::Base::operator == (other_)){
            return false;
        }
        if (m_state_ != other_.m_state_) {
            return false;
        }
        if (m_traceContext_ != other_.m_traceContext_) {
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
        o << static_cast<const CorpusIntegrationTrace::Base &>(sample);
        o << "state: " << sample.state()<<", ";
        o << "traceContext: " << sample.traceContext() ;
        o <<"]";
        return o;
    }

} // namespace CorpusIntegrationTrace  

// --- Type traits: -------------------------------------------------

namespace rti { 
    namespace topic {

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusIntegrationTrace::Producer_AliasTag_t > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode Producer_g_tc_string;

                static DDS_TypeCode Producer_g_tc =
                {{
                        DDS_TK_ALIAS, /* Kind*/
                        DDS_BOOLEAN_FALSE,/* Is a pointer? */
                        -1, /* Ignored */
                        (char *)"CorpusIntegrationTrace::Producer", /* Name */
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
                    }}; /* Type code for  Producer */

                if (is_initialized) {
                    return &Producer_g_tc;
                }

                Producer_g_tc_string = initialize_string_typecode((16L));

                Producer_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Producer_g_tc._data._typeCode =  (RTICdrTypeCode *)&Producer_g_tc_string;

                /* Initialize the values for member annotations. */
                Producer_g_tc._data._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Producer_g_tc._data._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Producer_g_tc._data._sampleAccessInfo = sample_access_info();
                Producer_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Producer_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                static RTIXCdrMemberAccessInfo Producer_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Producer_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Producer_g_sampleAccessInfo;
                }

                Producer_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 0;

                Producer_g_sampleAccessInfo.memberAccessInfos = 
                Producer_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusIntegrationTrace::Producer);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Producer_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Producer_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Producer_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Producer_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusIntegrationTrace::Producer >;

                Producer_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &Producer_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Producer_g_typePlugin = 
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

                return &Producer_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::AliasType& dynamic_type< CorpusIntegrationTrace::Producer_AliasTag_t >::get()
        {
            return static_cast<const ::dds::core::xtypes::AliasType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusIntegrationTrace::Producer_AliasTag_t >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusIntegrationTrace::Context > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode Context_g_tc_correlation_string;

                static DDS_TypeCode_Member Context_g_tc_members[2]=
                {

                    {
                        (char *)"correlation",/* Member name */
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
                        (char *)"producer",/* Member name */
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
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Context_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationTrace::Context", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Context_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Context*/

                if (is_initialized) {
                    return &Context_g_tc;
                }

                Context_g_tc_correlation_string = initialize_string_typecode((32L));

                Context_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Context_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&Context_g_tc_correlation_string;
                Context_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusIntegrationTrace::Producer_AliasTag_t>::get().native();

                /* Initialize the values for member annotations. */
                Context_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Context_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Context_g_tc._data._sampleAccessInfo = sample_access_info();
                Context_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Context_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusIntegrationTrace::Context *sample;

                static RTIXCdrMemberAccessInfo Context_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Context_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Context_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusIntegrationTrace::Context);
                if (sample == NULL) {
                    return NULL;
                }

                Context_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->correlation() - (char *)sample);

                Context_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->producer() - (char *)sample);

                Context_g_sampleAccessInfo.memberAccessInfos = 
                Context_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusIntegrationTrace::Context);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Context_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Context_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Context_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Context_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< CorpusIntegrationTrace::Context >;

                Context_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
                return (RTIXCdrSampleAccessInfo*) &Context_g_sampleAccessInfo;
            }

            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Context_g_typePlugin = 
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

                return &Context_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusIntegrationTrace::Context >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusIntegrationTrace::Context >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusIntegrationTrace::Base > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode_Member Base_g_tc_members[1]=
                {

                    {
                        (char *)"baseValue",/* Member name */
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

                static DDS_TypeCode Base_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationTrace::Base", /* Name */
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

                if (is_initialized) {
                    return &Base_g_tc;
                }

                Base_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Base_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Base_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Base_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Base_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Base_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Base_g_tc._data._sampleAccessInfo = sample_access_info();
                Base_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Base_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusIntegrationTrace::Base *sample;

                static RTIXCdrMemberAccessInfo Base_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Base_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Base_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusIntegrationTrace::Base);
                if (sample == NULL) {
                    return NULL;
                }

                Base_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->baseValue() - (char *)sample);

                Base_g_sampleAccessInfo.memberAccessInfos = 
                Base_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusIntegrationTrace::Base);

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
                interpreter::get_aggregation_value_pointer< CorpusIntegrationTrace::Base >;

                Base_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
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
                    NULL
                };

                return &Base_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusIntegrationTrace::Base >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusIntegrationTrace::Base >::get())));
        }

        #ifndef NDDS_STANDALONE_TYPE
        template<>
        struct native_type_code< CorpusIntegrationTrace::Derived > {
            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static RTIBool is_initialized = RTI_FALSE;

                static DDS_TypeCode Derived_g_tc_state_string;

                static DDS_TypeCode_Member Derived_g_tc_members[2]=
                {

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
                        (char *)"traceContext",/* Member name */
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

                static DDS_TypeCode Derived_g_tc =
                {{
                        DDS_TK_VALUE, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusIntegrationTrace::Derived", /* Name */
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

                if (is_initialized) {
                    return &Derived_g_tc;
                }

                Derived_g_tc_state_string = initialize_string_typecode((16L));

                Derived_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Derived_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&Derived_g_tc_state_string;
                Derived_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusIntegrationTrace::Context>::get().native();

                /* Initialize the values for member annotations. */
                Derived_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_STRING;
                Derived_g_tc_members[0]._annotations._defaultValue._u.string_value = (DDS_Char *) "";

                Derived_g_tc._data._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< CorpusIntegrationTrace::Base >::get().native(); /* Base class */

                Derived_g_tc._data._sampleAccessInfo = sample_access_info();
                Derived_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized = RTI_TRUE;

                return &Derived_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static RTIBool is_initialized = RTI_FALSE;

                CorpusIntegrationTrace::Derived *sample;

                static RTIXCdrMemberAccessInfo Derived_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Derived_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized) {
                    return (RTIXCdrSampleAccessInfo*) &Derived_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    CorpusIntegrationTrace::Derived);
                if (sample == NULL) {
                    return NULL;
                }

                Derived_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->state() - (char *)sample);

                Derived_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->traceContext() - (char *)sample);

                Derived_g_sampleAccessInfo.memberAccessInfos = 
                Derived_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(CorpusIntegrationTrace::Derived);

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
                interpreter::get_aggregation_value_pointer< CorpusIntegrationTrace::Derived >;

                Derived_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized = RTI_TRUE;
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
                    NULL
                };

                return &Derived_g_typePlugin;
            }
        }; // native_type_code
        #endif

        const ::dds::core::xtypes::StructType& dynamic_type< CorpusIntegrationTrace::Derived >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< CorpusIntegrationTrace::Derived >::get())));
        }

    }
}

namespace dds { 
    namespace topic {
        void topic_type_support< CorpusIntegrationTrace::Context >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusIntegrationTrace::ContextPlugin_new,
                CorpusIntegrationTrace::ContextPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusIntegrationTrace::Context >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusIntegrationTrace::Context& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = ContextPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = ContextPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< CorpusIntegrationTrace::Context >::from_cdr_buffer(CorpusIntegrationTrace::Context& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = ContextPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusIntegrationTrace::Context from cdr buffer");
        }

        void topic_type_support< CorpusIntegrationTrace::Context >::reset_sample(CorpusIntegrationTrace::Context& sample) 
        {
            sample.correlation("");
            ::rti::topic::reset_sample(sample.producer());
        }

        void topic_type_support< CorpusIntegrationTrace::Context >::allocate_sample(CorpusIntegrationTrace::Context& sample, int, int) 
        {
            ::rti::topic::allocate_sample(sample.correlation(),  -1, 32L);
        }

        void topic_type_support< CorpusIntegrationTrace::Base >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusIntegrationTrace::BasePlugin_new,
                CorpusIntegrationTrace::BasePlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusIntegrationTrace::Base >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusIntegrationTrace::Base& sample,
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

        void topic_type_support< CorpusIntegrationTrace::Base >::from_cdr_buffer(CorpusIntegrationTrace::Base& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = BasePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusIntegrationTrace::Base from cdr buffer");
        }

        void topic_type_support< CorpusIntegrationTrace::Base >::reset_sample(CorpusIntegrationTrace::Base& sample) 
        {
            sample.baseValue(0);
        }

        void topic_type_support< CorpusIntegrationTrace::Base >::allocate_sample(CorpusIntegrationTrace::Base& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< CorpusIntegrationTrace::Derived >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                CorpusIntegrationTrace::DerivedPlugin_new,
                CorpusIntegrationTrace::DerivedPlugin_delete);
        }

        std::vector<char>& topic_type_support< CorpusIntegrationTrace::Derived >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const CorpusIntegrationTrace::Derived& sample,
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

        void topic_type_support< CorpusIntegrationTrace::Derived >::from_cdr_buffer(CorpusIntegrationTrace::Derived& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = DerivedPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create CorpusIntegrationTrace::Derived from cdr buffer");
        }

        void topic_type_support< CorpusIntegrationTrace::Derived >::reset_sample(CorpusIntegrationTrace::Derived& sample) 
        {
            // Initialize base
            topic_type_support< CorpusIntegrationTrace::Base >::reset_sample(sample);

            sample.state("");
            ::rti::topic::reset_sample(sample.traceContext());
        }

        void topic_type_support< CorpusIntegrationTrace::Derived >::allocate_sample(CorpusIntegrationTrace::Derived& sample, int, int) 
        {
            // Initialize base
            topic_type_support< CorpusIntegrationTrace::Base >::allocate_sample(sample, -1, -1);

            ::rti::topic::allocate_sample(sample.state(),  -1, 16L);
            ::rti::topic::allocate_sample(sample.traceContext(),  -1, -1);
        }

    }
}  

