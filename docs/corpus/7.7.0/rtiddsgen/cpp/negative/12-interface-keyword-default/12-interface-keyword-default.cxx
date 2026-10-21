

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-keyword-default.idl
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
#include "12-interface-keyword-defaultPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "12-interface-keyword-default.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusInterfaceKeyword {

    // ---- Service_Context_In:

    Service_Context_In::Service_Context_In()  {

    }

    bool operator == (const Service_Context_In& , const Service_Context_In& ) {

        return true;
    }

    bool operator != (const Service_Context_In& a, const Service_Context_In& b) {
        return !operator ==(a, b);
    }

    void swap(Service_Context_In& , Service_Context_In& ) noexcept
    {
        using std::swap;

    }  
    std::ostream& operator << (std::ostream& o,const Service_Context_In& )
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o <<"]";
        return o;
    }

    // ---- Service_Context_Out:

    Service_Context_Out::Service_Context_Out()  {

    }

    bool operator == (const Service_Context_Out& , const Service_Context_Out& ) {

        return true;
    }

    bool operator != (const Service_Context_Out& a, const Service_Context_Out& b) {
        return !operator ==(a, b);
    }

    void swap(Service_Context_Out& , Service_Context_Out& ) noexcept
    {
        using std::swap;

    }  
    std::ostream& operator << (std::ostream& o,const Service_Context_Out& )
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o <<"]";
        return o;
    }

    // ---- Service_Context_Result:

    Service_Context_Result::Union_::Union_()  {
    }

    Service_Context_Result::Union_::Union_(const ::CorpusInterfaceKeyword::Service_Context_Out& result_):
        result(result_) {
    }
    Service_Context_Result::Service_Context_Result() :m_d_(default_discriminator())
    {
    }

    void Service_Context_Result::swap(Service_Context_Result& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.result, other_.m_u_.result);
            } break;

            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 0:
                {  
                    swap(m_u_.result, other_.m_u_.result);
                } break;

                default:
                {
                    /*
                    * Prevents compiler warnings when discriminator is an enum.
                    */
                }
            }
        }
    }  

    bool Service_Context_Result::operator == (const Service_Context_Result& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.result != other_.m_u_.result) {
                    return false;
                }
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        return true;
    }

    bool Service_Context_Result::operator != (const Service_Context_Result& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_Context_Result& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "result: " << sample.result();
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        o <<"]";
        return o;
    }

    int32_t Service_Context_Result::default_discriminator() {
        return 0;
    }

    // ---- Service_Call:

    Service_Call::Union_::Union_()  {
    }

    Service_Call::Union_::Union_(const ::CorpusInterfaceKeyword::Service_Context_In& Context_):
        Context(Context_) {
    }
    Service_Call::Service_Call() :m_d_(default_discriminator())
    {
    }

    void Service_Call::swap(Service_Call& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case CorpusInterfaceKeyword::Service_Context_Hash:
            {  
                swap(m_u_.Context, other_.m_u_.Context);
            } break;

            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case CorpusInterfaceKeyword::Service_Context_Hash:
                {  
                    swap(m_u_.Context, other_.m_u_.Context);
                } break;

                default:
                {
                    /*
                    * Prevents compiler warnings when discriminator is an enum.
                    */
                }
            }
        }
    }  

    bool Service_Call::operator == (const Service_Call& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case CorpusInterfaceKeyword::Service_Context_Hash:
            {  
                if (m_u_.Context != other_.m_u_.Context) {
                    return false;
                }
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        return true;
    }

    bool Service_Call::operator != (const Service_Call& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_Call& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case CorpusInterfaceKeyword::Service_Context_Hash:
            {  
                o << "Context: " << sample.Context();
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        o <<"]";
        return o;
    }

    int32_t Service_Call::default_discriminator() {
        return 0;
    }

    // ---- Service_Return:

    Service_Return::Union_::Union_() :
        remoteEx (0)  {
    }

    Service_Return::Union_::Union_(int32_t remoteEx_,const ::CorpusInterfaceKeyword::Service_Context_Result& Context_):
        remoteEx(remoteEx_), 
        Context(Context_) {
    }
    Service_Return::Service_Return() :m_d_(default_discriminator())
    {
    }

    void Service_Return::swap(Service_Return& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.remoteEx, other_.m_u_.remoteEx);
            } break;
            case CorpusInterfaceKeyword::Service_Context_Hash:
            {  
                swap(m_u_.Context, other_.m_u_.Context);
            } break;

            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        if (_d() != other_._d()){
            switch(::rti::topic::cdr::integer_case(other_._d())){
                case 0:
                {  
                    swap(m_u_.remoteEx, other_.m_u_.remoteEx);
                } break;
                case CorpusInterfaceKeyword::Service_Context_Hash:
                {  
                    swap(m_u_.Context, other_.m_u_.Context);
                } break;

                default:
                {
                    /*
                    * Prevents compiler warnings when discriminator is an enum.
                    */
                }
            }
        }
    }  

    bool Service_Return::operator == (const Service_Return& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.remoteEx != other_.m_u_.remoteEx) {
                    return false;
                }
            } break ;
            case CorpusInterfaceKeyword::Service_Context_Hash:
            {  
                if (m_u_.Context != other_.m_u_.Context) {
                    return false;
                }
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        return true;
    }

    bool Service_Return::operator != (const Service_Return& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_Return& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d() <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "remoteEx: " << sample.remoteEx()<<", ";
            } break ;
            case CorpusInterfaceKeyword::Service_Context_Hash:
            {  
                o << "Context: " << sample.Context();
            } break ;
            default:
            {
                /*
                * Prevents compiler warnings when discriminator is an enum.
                */
            }
        }
        o <<"]";
        return o;
    }

    int32_t Service_Return::default_discriminator() {
        return 0;
    }

    ServiceClient::ServiceClient(const dds::rpc::ClientParams& params)
    : ServiceClientEndpoint(params)
    {
    }

    //
    // @service functions:
    //

    void ServiceClient::Context (
        ) {
        auto request_id = send_Context(
            );
        receive_Context(request_id);

    }

    std::future< void> ServiceClient::Context_async (
        ) {
        auto request_id = send_Context(
            );
        auto service = *this;
        return std::async(std::launch::deferred, [service, request_id]() mutable {
            return service.receive_Context(request_id);
        });
    }

    //
    // Helper functions (send, receive)
    //

    // --- ServiceClient: ----------------------------------------------------

    rti::core::SampleIdentity ServiceClient::send_Context (
        ) {
        rti::core::EntityLock lock(request_datawriter());
        auto& request = scratchpad_request();
        request.Context ( Service_Context_In(
            ));
        return send_request();
    }

    void     ServiceClient::receive_Context (
        const rti::core::SampleIdentity& request_id) {
        auto replies = receive_reply(request_id, Service_Context_Hash);
        const auto& result = replies[0].data().Context();

        switch (result._d()) {
            case 0:
            return ;
            default:
            throw ::dds::rpc::RemoteUnknownExceptionError();
            break;
        }
    }

    // --- ServiceDispatcher: ------------------------------------------------

    void ServiceDispatcher::dispatch_request(
        InterfaceType* service_impl,
        ReplyType& result,
        const RequestType& request)
    {
        switch (request._d()) {
            case Service_Context_Hash:
            dispatch_Context(service_impl, result, request);
            break;
            default:  // Unknown operation
            result.remoteEx(static_cast<int32_t>(rti::rpc::detail::RemoteExceptionCode::unknown_operation));
        }
    }

    void ServiceDispatcher::dispatch_Context(
        InterfaceType* service_impl,
        ReplyType& result,
        const RequestType& )
    {
        result._d(Service_Context_Hash);
        result.Context()._d(0);

        service_impl->Context(
            );
    }

    // ---- Sample:

    Sample::Sample() :
        number (0)  {

    }

    Sample::Sample (int32_t number_) {
        number = number_;
    }

    bool operator == (const Sample& a, const Sample& b) {

        if (a.number != b.number) {
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

        swap(a.number, b.number);

    }  
    std::ostream& operator << (std::ostream& o,const Sample& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "number: " << sample.number;
        o <<"]";
        return o;
    }

} // namespace CorpusInterfaceKeyword  

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
        struct native_type_code< ::CorpusInterfaceKeyword::Service_Context_In > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Service_Context_In_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceKeyword::Service_Context_In", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Number of members */
                        NULL,
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Context_In*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Context_In_g_tc;
                }

                Service_Context_In_g_tc._data._annotations._allowedDataRepresentationMask = 5;
                Service_Context_In_g_tc._data._annotations._isNested = RTI_XCDR_TRUE;
                Service_Context_In_g_tc._data._annotations._isAutoIdHash = RTI_XCDR_TRUE;

                /* Initialize the values for member annotations. */

                Service_Context_In_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Context_In_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Service_Context_In_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrSampleAccessInfo Service_Context_In_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Context_In_g_sampleAccessInfo;
                }

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceKeyword::Service_Context_In);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Context_In_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Context_In_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Context_In_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Context_In_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceKeyword::Service_Context_In >;

                Service_Context_In_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Context_In_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Context_In_g_typePlugin =
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

                return &Service_Context_In_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceKeyword::Service_Context_In >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceKeyword::Service_Context_In >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceKeyword::Service_Context_Out > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode Service_Context_Out_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceKeyword::Service_Context_Out", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        0, /* Number of members */
                        NULL,
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Context_Out*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Context_Out_g_tc;
                }

                Service_Context_Out_g_tc._data._annotations._allowedDataRepresentationMask = 5;
                Service_Context_Out_g_tc._data._annotations._isNested = RTI_XCDR_TRUE;
                Service_Context_Out_g_tc._data._annotations._isAutoIdHash = RTI_XCDR_TRUE;

                /* Initialize the values for member annotations. */

                Service_Context_Out_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Context_Out_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Service_Context_Out_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                static RTIXCdrSampleAccessInfo Service_Context_Out_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Context_Out_g_sampleAccessInfo;
                }

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceKeyword::Service_Context_Out);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Context_Out_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Context_Out_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Context_Out_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Context_Out_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceKeyword::Service_Context_Out >;

                Service_Context_Out_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Context_Out_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Context_Out_g_typePlugin =
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

                return &Service_Context_Out_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceKeyword::Service_Context_Out >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceKeyword::Service_Context_Out >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceKeyword::Service_Context_Result > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Context_Result_g_tc_members[1]=
                {

                    {
                        (char *)"result",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(0),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_Context_Result_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceKeyword::Service_Context_Result", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_Context_Result_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Context_Result*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Context_Result_g_tc;
                }

                Service_Context_Result_g_tc._data._annotations._allowedDataRepresentationMask = 5;
                Service_Context_Result_g_tc._data._annotations._isNested = RTI_XCDR_TRUE;

                Service_Context_Result_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceKeyword::Service_Context_Out>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Service_Context_Result_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_Context_Result_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Context_Result_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Service_Context_Result_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceKeyword::Service_Context_Result *sample;

                static RTIXCdrMemberAccessInfo Service_Context_Result_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Context_Result_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Context_Result_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusInterfaceKeyword::Service_Context_Result);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Context_Result_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(0);
                Service_Context_Result_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->result() - (char *)sample);

                Service_Context_Result_g_sampleAccessInfo.memberAccessInfos =
                Service_Context_Result_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceKeyword::Service_Context_Result);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Context_Result_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Context_Result_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Context_Result_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Context_Result_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceKeyword::Service_Context_Result >;

                Service_Context_Result_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Context_Result_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Context_Result_g_typePlugin =
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

                return &Service_Context_Result_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceKeyword::Service_Context_Result >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceKeyword::Service_Context_Result >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceKeyword::Service_Call > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Call_g_tc_members[1]=
                {

                    {
                        (char *)"Context",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(CorpusInterfaceKeyword::Service_Context_Hash),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_Call_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceKeyword::Service_Call", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_Call_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Call*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Call_g_tc;
                }

                Service_Call_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_Call_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceKeyword::Service_Context_In>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Service_Call_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_Call_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Call_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Service_Call_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceKeyword::Service_Call *sample;

                static RTIXCdrMemberAccessInfo Service_Call_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Call_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Call_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusInterfaceKeyword::Service_Call);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Call_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(CorpusInterfaceKeyword::Service_Context_Hash);
                Service_Call_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->Context() - (char *)sample);

                Service_Call_g_sampleAccessInfo.memberAccessInfos =
                Service_Call_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceKeyword::Service_Call);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Call_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Call_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Call_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Call_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceKeyword::Service_Call >;

                Service_Call_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Call_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Call_g_typePlugin =
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

                return &Service_Call_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceKeyword::Service_Call >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceKeyword::Service_Call >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceKeyword::Service_Return > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Return_g_tc_members[2]=
                {

                    {
                        (char *)"remoteEx",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(0),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"Context",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(CorpusInterfaceKeyword::Service_Context_Hash),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_Return_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceKeyword::Service_Return", /* Name */
                        NULL,     /* Base class type code is assigned later */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
                        Service_Return_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Return*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Return_g_tc;
                }

                Service_Return_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_Return_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;
                Service_Return_g_tc_members[1]._representation._typeCode =  (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceKeyword::Service_Context_Result>::get().native();

                /* Initialize the values for member annotations. */
                Service_Return_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Service_Return_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Service_Return_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Service_Return_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Service_Return_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Service_Return_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                /* Discriminator type code */
                Service_Return_g_tc._data._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_Return_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Return_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Service_Return_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceKeyword::Service_Return *sample;

                static RTIXCdrMemberAccessInfo Service_Return_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Return_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Return_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusInterfaceKeyword::Service_Return);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Return_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d(0);
                Service_Return_g_memberAccessInfos[1].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->remoteEx() - (char *)sample);

                sample->_d(CorpusInterfaceKeyword::Service_Context_Hash);
                Service_Return_g_memberAccessInfos[2].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->Context() - (char *)sample);

                Service_Return_g_sampleAccessInfo.memberAccessInfos =
                Service_Return_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceKeyword::Service_Return);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Return_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Return_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Return_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Return_g_sampleAccessInfo.getMemberValuePointerFcn =
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceKeyword::Service_Return >;

                Service_Return_g_sampleAccessInfo.languageBinding =
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Return_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Return_g_typePlugin =
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

                return &Service_Return_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceKeyword::Service_Return >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceKeyword::Service_Return >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceKeyword::Sample > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Sample_g_tc_members[1]=
                {

                    {
                        (char *)"number",/* Member name */
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
                    }
                };

                static DDS_TypeCode Sample_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceKeyword::Sample", /* Name */
                        NULL, /* Ignored */ 
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
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

                Sample_g_tc_members[0]._representation._typeCode =  (RTICdrTypeCode *)&DDS_g_tc_long;

                /* Initialize the values for member annotations. */
                Sample_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._defaultValue._u.long_value = 0;
                Sample_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._minValue._u.long_value = RTIXCdrLong_MIN;
                Sample_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_LONG;
                Sample_g_tc_members[0]._annotations._maxValue._u.long_value = RTIXCdrLong_MAX;

                Sample_g_tc._data._sampleAccessInfo = sample_access_info();
                Sample_g_tc._data._typePlugin = type_plugin_info();

                is_initialized.store(true, std::memory_order_release);

                return &Sample_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceKeyword::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo =
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample,
                    ::CorpusInterfaceKeyword::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->number - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos =
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceKeyword::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceKeyword::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceKeyword::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceKeyword::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Context_In >::reset_sample(::CorpusInterfaceKeyword::Service_Context_In& sample) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Context_In >::allocate_sample(::CorpusInterfaceKeyword::Service_Context_In& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Context_Out >::reset_sample(::CorpusInterfaceKeyword::Service_Context_Out& sample) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Context_Out >::allocate_sample(::CorpusInterfaceKeyword::Service_Context_Out& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Context_Result >::reset_sample(::CorpusInterfaceKeyword::Service_Context_Result& sample) 
        {
            sample._d(0);
            ::rti::topic::reset_sample(sample.result());

            sample._d(::CorpusInterfaceKeyword::Service_Context_Result::default_discriminator());
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Context_Result >::allocate_sample(::CorpusInterfaceKeyword::Service_Context_Result& sample, int, int) 
        {
            sample._d(0);
            ::rti::topic::allocate_sample(sample.result(),  -1, -1);

            sample._d(::CorpusInterfaceKeyword::Service_Context_Result::default_discriminator());
        }
        void topic_type_support< ::CorpusInterfaceKeyword::Service_Call >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInterfaceKeyword::Service_CallPlugin_new,
                ::CorpusInterfaceKeyword::Service_CallPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInterfaceKeyword::Service_Call >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInterfaceKeyword::Service_Call& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = Service_CallPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = Service_CallPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Call >::from_cdr_buffer(::CorpusInterfaceKeyword::Service_Call& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = Service_CallPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInterfaceKeyword::Service_Call from cdr buffer");
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Call >::reset_sample(::CorpusInterfaceKeyword::Service_Call& sample) 
        {
            sample._d(CorpusInterfaceKeyword::Service_Context_Hash);
            ::rti::topic::reset_sample(sample.Context());

            sample._d(::CorpusInterfaceKeyword::Service_Call::default_discriminator());
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Call >::allocate_sample(::CorpusInterfaceKeyword::Service_Call& sample, int, int) 
        {
            sample._d(CorpusInterfaceKeyword::Service_Context_Hash);
            ::rti::topic::allocate_sample(sample.Context(),  -1, -1);

            sample._d(::CorpusInterfaceKeyword::Service_Call::default_discriminator());
        }
        void topic_type_support< ::CorpusInterfaceKeyword::Service_Return >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInterfaceKeyword::Service_ReturnPlugin_new,
                ::CorpusInterfaceKeyword::Service_ReturnPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInterfaceKeyword::Service_Return >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInterfaceKeyword::Service_Return& sample,
            ::dds::core::policy::DataRepresentationId representation)
        {
            // First get the length of the buffer
            unsigned int length = 0;
            RTIBool ok = Service_ReturnPlugin_serialize_to_cdr_buffer(
                NULL, 
                &length,
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to calculate cdr buffer size");

            // Create a vector with that size and copy the cdr buffer into it
            buffer.resize(length);
            ok = Service_ReturnPlugin_serialize_to_cdr_buffer(
                &buffer[0], 
                &length, 
                &sample,
                representation);
            ::rti::core::check_return_code(
                ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
                "Failed to copy cdr buffer");

            return buffer;
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Return >::from_cdr_buffer(::CorpusInterfaceKeyword::Service_Return& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = Service_ReturnPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInterfaceKeyword::Service_Return from cdr buffer");
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Return >::reset_sample(::CorpusInterfaceKeyword::Service_Return& sample) 
        {
            sample._d(0);
            sample.remoteEx(0);
            sample._d(CorpusInterfaceKeyword::Service_Context_Hash);
            ::rti::topic::reset_sample(sample.Context());

            sample._d(::CorpusInterfaceKeyword::Service_Return::default_discriminator());
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Service_Return >::allocate_sample(::CorpusInterfaceKeyword::Service_Return& sample, int, int) 
        {
            sample._d(CorpusInterfaceKeyword::Service_Context_Hash);
            ::rti::topic::allocate_sample(sample.Context(),  -1, -1);

            sample._d(::CorpusInterfaceKeyword::Service_Return::default_discriminator());
        }
        void topic_type_support< ::CorpusInterfaceKeyword::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInterfaceKeyword::SamplePlugin_new,
                ::CorpusInterfaceKeyword::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInterfaceKeyword::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInterfaceKeyword::Sample& sample,
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

        void topic_type_support< ::CorpusInterfaceKeyword::Sample >::from_cdr_buffer(::CorpusInterfaceKeyword::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInterfaceKeyword::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Sample >::reset_sample(::CorpusInterfaceKeyword::Sample& sample) 
        {
            sample.number = 0;
        }

        void topic_type_support< ::CorpusInterfaceKeyword::Sample >::allocate_sample(::CorpusInterfaceKeyword::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
