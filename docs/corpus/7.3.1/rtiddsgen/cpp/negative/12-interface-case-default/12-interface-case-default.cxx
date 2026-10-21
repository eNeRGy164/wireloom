

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-case-default.idl
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
#include "12-interface-case-defaultPlugin.hpp"
#else
#include "rti/topic/cdr/SerializationHelpers.hpp"
#endif

#include "12-interface-case-default.hpp"

#include <rti/util/ostream_operators.hpp>

namespace CorpusInterfaceCase {

    // ---- Service_ping_In: 

    Service_ping_In::Service_ping_In() :
        m_dummy_ (0)  {

    }   

    Service_ping_In::Service_ping_In (uint8_t dummy_):
        m_dummy_(dummy_) {
    }

    void Service_ping_In::swap(Service_ping_In& other_)  noexcept 
    {
        using std::swap;
        swap(m_dummy_, other_.m_dummy_);
    }  

    bool Service_ping_In::operator == (const Service_ping_In& other_) const {
        if (m_dummy_ != other_.m_dummy_) {
            return false;
        }
        return true;
    }

    bool Service_ping_In::operator != (const Service_ping_In& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_ping_In& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "dummy: " << std::hex<<(int) sample.dummy ();
        o <<"]";
        return o;
    }

    // ---- Service_ping_Out: 

    Service_ping_Out::Service_ping_Out() :
        m_dummy_ (0)  {

    }   

    Service_ping_Out::Service_ping_Out (uint8_t dummy_):
        m_dummy_(dummy_) {
    }

    void Service_ping_Out::swap(Service_ping_Out& other_)  noexcept 
    {
        using std::swap;
        swap(m_dummy_, other_.m_dummy_);
    }  

    bool Service_ping_Out::operator == (const Service_ping_Out& other_) const {
        if (m_dummy_ != other_.m_dummy_) {
            return false;
        }
        return true;
    }

    bool Service_ping_Out::operator != (const Service_ping_Out& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_ping_Out& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "dummy: " << std::hex<<(int) sample.dummy ();
        o <<"]";
        return o;
    }

    // ---- Service_ping_Result: 

    Service_ping_Result::Union_::Union_()  {
    }

    Service_ping_Result::Union_::Union_(const ::CorpusInterfaceCase::Service_ping_Out& result_):
        m_result_(result_) {
    }

    Service_ping_Result::Service_ping_Result() :m_d_(default_discriminator())
    {
    }

    void Service_ping_Result::swap(Service_ping_Result& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.m_result_, other_.m_u_.m_result_);
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
                    swap(m_u_.m_result_, other_.m_u_.m_result_);
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

    bool Service_ping_Result::operator == (const Service_ping_Result& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.m_result_ != other_.m_u_.m_result_) {
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

    bool Service_ping_Result::operator != (const Service_ping_Result& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_ping_Result& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "result: " << sample.result ();
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

    int32_t Service_ping_Result::default_discriminator() {
        return 0;
    }

    // ---- Service_Ping_In: 

    Service_Ping_In::Service_Ping_In() :
        m_dummy_ (0)  {

    }   

    Service_Ping_In::Service_Ping_In (uint8_t dummy_):
        m_dummy_(dummy_) {
    }

    void Service_Ping_In::swap(Service_Ping_In& other_)  noexcept 
    {
        using std::swap;
        swap(m_dummy_, other_.m_dummy_);
    }  

    bool Service_Ping_In::operator == (const Service_Ping_In& other_) const {
        if (m_dummy_ != other_.m_dummy_) {
            return false;
        }
        return true;
    }

    bool Service_Ping_In::operator != (const Service_Ping_In& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_Ping_In& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "dummy: " << std::hex<<(int) sample.dummy ();
        o <<"]";
        return o;
    }

    // ---- Service_Ping_Out: 

    Service_Ping_Out::Service_Ping_Out() :
        m_dummy_ (0)  {

    }   

    Service_Ping_Out::Service_Ping_Out (uint8_t dummy_):
        m_dummy_(dummy_) {
    }

    void Service_Ping_Out::swap(Service_Ping_Out& other_)  noexcept 
    {
        using std::swap;
        swap(m_dummy_, other_.m_dummy_);
    }  

    bool Service_Ping_Out::operator == (const Service_Ping_Out& other_) const {
        if (m_dummy_ != other_.m_dummy_) {
            return false;
        }
        return true;
    }

    bool Service_Ping_Out::operator != (const Service_Ping_Out& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_Ping_Out& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "dummy: " << std::hex<<(int) sample.dummy ();
        o <<"]";
        return o;
    }

    // ---- Service_Ping_Result: 

    Service_Ping_Result::Union_::Union_()  {
    }

    Service_Ping_Result::Union_::Union_(const ::CorpusInterfaceCase::Service_Ping_Out& result_):
        m_result_(result_) {
    }

    Service_Ping_Result::Service_Ping_Result() :m_d_(default_discriminator())
    {
    }

    void Service_Ping_Result::swap(Service_Ping_Result& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case 0:
            {  
                swap(m_u_.m_result_, other_.m_u_.m_result_);
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
                    swap(m_u_.m_result_, other_.m_u_.m_result_);
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

    bool Service_Ping_Result::operator == (const Service_Ping_Result& other_) const {
        if (_d() != other_._d()){
            return false;
        }
        switch(::rti::topic::cdr::integer_case(_d())){
            case 0:
            {  
                if (m_u_.m_result_ != other_.m_u_.m_result_) {
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

    bool Service_Ping_Result::operator != (const Service_Ping_Result& other_) const {
        return !this->operator ==(other_);
    }

    std::ostream& operator << (std::ostream& o,const Service_Ping_Result& sample)
    {
        ::rti::util::StreamFlagSaver flag_saver (o);
        o <<"[";
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case 0:
            {  
                o << "result: " << sample.result ();
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

    int32_t Service_Ping_Result::default_discriminator() {
        return 0;
    }

    // ---- Service_Call: 

    Service_Call::Union_::Union_()  {
    }

    Service_Call::Union_::Union_(const ::CorpusInterfaceCase::Service_ping_In& ping_,const ::CorpusInterfaceCase::Service_Ping_In& Ping_):
        m_ping_(ping_), 
        m_Ping_(Ping_) {
    }

    Service_Call::Service_Call() :m_d_(default_discriminator())
    {
    }

    void Service_Call::swap(Service_Call& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case CorpusInterfaceCase::Service_ping_Hash:
            {  
                swap(m_u_.m_ping_, other_.m_u_.m_ping_);
            } break;
            case CorpusInterfaceCase::Service_Ping_Hash:
            {  
                swap(m_u_.m_Ping_, other_.m_u_.m_Ping_);
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
                case CorpusInterfaceCase::Service_ping_Hash:
                {  
                    swap(m_u_.m_ping_, other_.m_u_.m_ping_);
                } break;
                case CorpusInterfaceCase::Service_Ping_Hash:
                {  
                    swap(m_u_.m_Ping_, other_.m_u_.m_Ping_);
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
            case CorpusInterfaceCase::Service_ping_Hash:
            {  
                if (m_u_.m_ping_ != other_.m_u_.m_ping_) {
                    return false;
                }
            } break ;
            case CorpusInterfaceCase::Service_Ping_Hash:
            {  
                if (m_u_.m_Ping_ != other_.m_u_.m_Ping_) {
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
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case CorpusInterfaceCase::Service_ping_Hash:
            {  
                o << "ping: " << sample.ping ()<<", ";
            } break ;
            case CorpusInterfaceCase::Service_Ping_Hash:
            {  
                o << "Ping: " << sample.Ping ();
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
        return -803907400;
    }

    // ---- Service_Return: 

    Service_Return::Union_::Union_()  {
    }

    Service_Return::Union_::Union_(const ::CorpusInterfaceCase::Service_ping_Result& ping_,const ::CorpusInterfaceCase::Service_Ping_Result& Ping_):
        m_ping_(ping_), 
        m_Ping_(Ping_) {
    }

    Service_Return::Service_Return() :m_d_(default_discriminator())
    {
    }

    void Service_Return::swap(Service_Return& other_)  noexcept 
    {
        using std::swap;
        swap (m_d_,other_.m_d_);
        switch (::rti::topic::cdr::integer_case(_d())) {
            case CorpusInterfaceCase::Service_ping_Hash:
            {  
                swap(m_u_.m_ping_, other_.m_u_.m_ping_);
            } break;
            case CorpusInterfaceCase::Service_Ping_Hash:
            {  
                swap(m_u_.m_Ping_, other_.m_u_.m_Ping_);
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
                case CorpusInterfaceCase::Service_ping_Hash:
                {  
                    swap(m_u_.m_ping_, other_.m_u_.m_ping_);
                } break;
                case CorpusInterfaceCase::Service_Ping_Hash:
                {  
                    swap(m_u_.m_Ping_, other_.m_u_.m_Ping_);
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
            case CorpusInterfaceCase::Service_ping_Hash:
            {  
                if (m_u_.m_ping_ != other_.m_u_.m_ping_) {
                    return false;
                }
            } break ;
            case CorpusInterfaceCase::Service_Ping_Hash:
            {  
                if (m_u_.m_Ping_ != other_.m_u_.m_Ping_) {
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
        o << "_d: " << sample._d () <<", ";
        switch(::rti::topic::cdr::integer_case(sample._d())){
            case CorpusInterfaceCase::Service_ping_Hash:
            {  
                o << "ping: " << sample.ping ()<<", ";
            } break ;
            case CorpusInterfaceCase::Service_Ping_Hash:
            {  
                o << "Ping: " << sample.Ping ();
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
        return -803907400;
    }

    ServiceClient::ServiceClient(const dds::rpc::ClientParams& params)
    : ServiceClientEndpoint(params)
    {
    }

    //
    // @service functions:
    //

    void ServiceClient::ping (
        ) {
        auto request_id = send_ping(
            );
        receive_ping(request_id);

    }

    void ServiceClient::Ping (
        ) {
        auto request_id = send_Ping(
            );
        receive_Ping(request_id);

    }

    std::future< void> ServiceClient::ping_async (
        ) {
        auto request_id = send_ping(
            );
        auto service = *this;
        return std::async(std::launch::deferred, [service, request_id]() mutable {
            return service.receive_ping(request_id);
        });
    }

    std::future< void> ServiceClient::Ping_async (
        ) {
        auto request_id = send_Ping(
            );
        auto service = *this;
        return std::async(std::launch::deferred, [service, request_id]() mutable {
            return service.receive_Ping(request_id);
        });
    }

    //
    // Helper functions (send, receive)
    //

    // --- ServiceClient: ----------------------------------------------------

    rti::core::SampleIdentity ServiceClient::send_ping (
        ) {
        rti::core::EntityLock lock(request_datawriter());
        auto& request = scratchpad_request();
        request.ping ( Service_ping_In(
            ));
        return send_request();
    }

    rti::core::SampleIdentity ServiceClient::send_Ping (
        ) {
        rti::core::EntityLock lock(request_datawriter());
        auto& request = scratchpad_request();
        request.Ping ( Service_Ping_In(
            ));
        return send_request();
    }

    void     ServiceClient::receive_ping (
        const rti::core::SampleIdentity& request_id) {
        auto replies = receive_reply(request_id, Service_ping_Hash);
        const auto& result = replies[0].data().ping();

        switch (result._d()) {
            case 0:
            return ;
            default:
            throw dds::core::Error("unknown remote exception");
            break;
        }
    }
    void     ServiceClient::receive_Ping (
        const rti::core::SampleIdentity& request_id) {
        auto replies = receive_reply(request_id, Service_Ping_Hash);
        const auto& result = replies[0].data().Ping();

        switch (result._d()) {
            case 0:
            return ;
            default:
            throw dds::core::Error("unknown remote exception");
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
            case Service_ping_Hash:
            dispatch_ping(service_impl, result, request);
            break;
            case Service_Ping_Hash:
            dispatch_Ping(service_impl, result, request);
            break;
            default:  // Unknown operation
            result._d(0); // NOTE: using 0 to not select any union case (default union case)
        }
    }

    void ServiceDispatcher::dispatch_ping(
        InterfaceType* service_impl,
        ReplyType& result,
        const RequestType& )
    {
        result._d(Service_ping_Hash);
        try {
            result.ping()._d(0);

            service_impl->ping(
                );
        }
        catch (...) {
            result.ping()._d(/* dds::REMOTE_EX_UNKNOWN_EXCEPTION */ -1);
        }
    }
    void ServiceDispatcher::dispatch_Ping(
        InterfaceType* service_impl,
        ReplyType& result,
        const RequestType& )
    {
        result._d(Service_Ping_Hash);
        try {
            result.Ping()._d(0);

            service_impl->Ping(
                );
        }
        catch (...) {
            result.Ping()._d(/* dds::REMOTE_EX_UNKNOWN_EXCEPTION */ -1);
        }
    }

    // ---- Sample: 

    Sample::Sample() :
        m_number_ (0)  {

    }   

    Sample::Sample (int32_t number_):
        m_number_(number_) {
    }

    void Sample::swap(Sample& other_)  noexcept 
    {
        using std::swap;
        swap(m_number_, other_.m_number_);
    }  

    bool Sample::operator == (const Sample& other_) const {
        if (m_number_ != other_.m_number_) {
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
        o << "number: " << sample.number ();
        o <<"]";
        return o;
    }

} // namespace CorpusInterfaceCase  

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
        struct native_type_code< ::CorpusInterfaceCase::Service_ping_In > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_ping_In_g_tc_members[1]=
                {

                    {
                        (char *)"dummy",/* Member name */
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

                static DDS_TypeCode Service_ping_In_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_ping_In", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_ping_In_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_ping_In*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_ping_In_g_tc;
                }

                Service_ping_In_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_ping_In_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;

                /* Initialize the values for member annotations. */
                Service_ping_In_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Service_ping_In_g_tc_members[0]._annotations._defaultValue._u.octet_value = 0;
                Service_ping_In_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Service_ping_In_g_tc_members[0]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Service_ping_In_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Service_ping_In_g_tc_members[0]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;

                Service_ping_In_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_ping_In_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_ping_In_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_ping_In *sample;

                static RTIXCdrMemberAccessInfo Service_ping_In_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_ping_In_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_ping_In_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_ping_In);
                if (sample == NULL) {
                    return NULL;
                }

                Service_ping_In_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->dummy() - (char *)sample);

                Service_ping_In_g_sampleAccessInfo.memberAccessInfos = 
                Service_ping_In_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_ping_In);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_ping_In_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_ping_In_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_ping_In_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_ping_In_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_ping_In >;

                Service_ping_In_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_ping_In_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_ping_In_g_typePlugin = 
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

                return &Service_ping_In_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceCase::Service_ping_In >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_ping_In >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_ping_Out > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_ping_Out_g_tc_members[1]=
                {

                    {
                        (char *)"dummy",/* Member name */
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

                static DDS_TypeCode Service_ping_Out_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_ping_Out", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_ping_Out_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_ping_Out*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_ping_Out_g_tc;
                }

                Service_ping_Out_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_ping_Out_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;

                /* Initialize the values for member annotations. */
                Service_ping_Out_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Service_ping_Out_g_tc_members[0]._annotations._defaultValue._u.octet_value = 0;
                Service_ping_Out_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Service_ping_Out_g_tc_members[0]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Service_ping_Out_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Service_ping_Out_g_tc_members[0]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;

                Service_ping_Out_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_ping_Out_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_ping_Out_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_ping_Out *sample;

                static RTIXCdrMemberAccessInfo Service_ping_Out_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_ping_Out_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_ping_Out_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_ping_Out);
                if (sample == NULL) {
                    return NULL;
                }

                Service_ping_Out_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->dummy() - (char *)sample);

                Service_ping_Out_g_sampleAccessInfo.memberAccessInfos = 
                Service_ping_Out_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_ping_Out);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_ping_Out_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_ping_Out_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_ping_Out_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_ping_Out_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_ping_Out >;

                Service_ping_Out_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_ping_Out_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_ping_Out_g_typePlugin = 
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

                return &Service_ping_Out_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceCase::Service_ping_Out >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_ping_Out >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_ping_Result > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_ping_Result_g_tc_members[1]=
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_ping_Result_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_ping_Result", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_ping_Result_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_ping_Result*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_ping_Result_g_tc;
                }

                Service_ping_Result_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_ping_Result_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceCase::Service_ping_Out>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Service_ping_Result_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_ping_Result_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_ping_Result_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_ping_Result_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_ping_Result *sample;

                static RTIXCdrMemberAccessInfo Service_ping_Result_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_ping_Result_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_ping_Result_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_ping_Result);
                if (sample == NULL) {
                    return NULL;
                }

                Service_ping_Result_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 0;
                Service_ping_Result_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->result() - (char *)sample);

                Service_ping_Result_g_sampleAccessInfo.memberAccessInfos = 
                Service_ping_Result_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_ping_Result);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_ping_Result_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_ping_Result_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_ping_Result_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_ping_Result_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_ping_Result >;

                Service_ping_Result_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_ping_Result_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_ping_Result_g_typePlugin = 
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

                return &Service_ping_Result_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceCase::Service_ping_Result >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_ping_Result >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_Ping_In > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Ping_In_g_tc_members[1]=
                {

                    {
                        (char *)"dummy",/* Member name */
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

                static DDS_TypeCode Service_Ping_In_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_Ping_In", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_Ping_In_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Ping_In*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Ping_In_g_tc;
                }

                Service_Ping_In_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_Ping_In_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;

                /* Initialize the values for member annotations. */
                Service_Ping_In_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Service_Ping_In_g_tc_members[0]._annotations._defaultValue._u.octet_value = 0;
                Service_Ping_In_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Service_Ping_In_g_tc_members[0]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Service_Ping_In_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Service_Ping_In_g_tc_members[0]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;

                Service_Ping_In_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Ping_In_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_Ping_In_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_Ping_In *sample;

                static RTIXCdrMemberAccessInfo Service_Ping_In_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Ping_In_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Ping_In_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_Ping_In);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Ping_In_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->dummy() - (char *)sample);

                Service_Ping_In_g_sampleAccessInfo.memberAccessInfos = 
                Service_Ping_In_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_Ping_In);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Ping_In_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Ping_In_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Ping_In_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Ping_In_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_Ping_In >;

                Service_Ping_In_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Ping_In_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Ping_In_g_typePlugin = 
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

                return &Service_Ping_In_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceCase::Service_Ping_In >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_Ping_In >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_Ping_Out > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Ping_Out_g_tc_members[1]=
                {

                    {
                        (char *)"dummy",/* Member name */
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

                static DDS_TypeCode Service_Ping_Out_g_tc =
                {{
                        DDS_TK_STRUCT, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_Ping_Out", /* Name */
                        NULL, /* Ignored */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_Ping_Out_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Ping_Out*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Ping_Out_g_tc;
                }

                Service_Ping_Out_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_Ping_Out_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_octet;

                /* Initialize the values for member annotations. */
                Service_Ping_Out_g_tc_members[0]._annotations._defaultValue._d = RTI_XCDR_TK_OCTET;
                Service_Ping_Out_g_tc_members[0]._annotations._defaultValue._u.octet_value = 0;
                Service_Ping_Out_g_tc_members[0]._annotations._minValue._d = RTI_XCDR_TK_OCTET;
                Service_Ping_Out_g_tc_members[0]._annotations._minValue._u.octet_value = RTIXCdrOctet_MIN;
                Service_Ping_Out_g_tc_members[0]._annotations._maxValue._d = RTI_XCDR_TK_OCTET;
                Service_Ping_Out_g_tc_members[0]._annotations._maxValue._u.octet_value = RTIXCdrOctet_MAX;

                Service_Ping_Out_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Ping_Out_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_Ping_Out_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_Ping_Out *sample;

                static RTIXCdrMemberAccessInfo Service_Ping_Out_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Ping_Out_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Ping_Out_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_Ping_Out);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Ping_Out_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->dummy() - (char *)sample);

                Service_Ping_Out_g_sampleAccessInfo.memberAccessInfos = 
                Service_Ping_Out_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_Ping_Out);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Ping_Out_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Ping_Out_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Ping_Out_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Ping_Out_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_Ping_Out >;

                Service_Ping_Out_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Ping_Out_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Ping_Out_g_typePlugin = 
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

                return &Service_Ping_Out_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceCase::Service_Ping_Out >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_Ping_Out >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_Ping_Result > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Ping_Result_g_tc_members[1]=
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
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_Ping_Result_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_Ping_Result", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        1, /* Number of members */
                        Service_Ping_Result_g_tc_members, /* Members */
                        DDS_VM_NONE, /* Type Modifier */
                        RTICdrTypeCodeAnnotations_INITIALIZER,
                        DDS_BOOLEAN_TRUE, /* _isCopyable */
                        NULL, /* _sampleAccessInfo: assigned later */
                        NULL /* _typePlugin: assigned later */
                    }}; /* Type code for Service_Ping_Result*/

                if (is_initialized.load(std::memory_order_acquire)) {
                    return &Service_Ping_Result_g_tc;
                }

                Service_Ping_Result_g_tc._data._annotations._allowedDataRepresentationMask = 5;

                Service_Ping_Result_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceCase::Service_Ping_Out>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Service_Ping_Result_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_Ping_Result_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Ping_Result_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_Ping_Result_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_Ping_Result *sample;

                static RTIXCdrMemberAccessInfo Service_Ping_Result_g_memberAccessInfos[2] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Ping_Result_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Ping_Result_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_Ping_Result);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Ping_Result_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = 0;
                Service_Ping_Result_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->result() - (char *)sample);

                Service_Ping_Result_g_sampleAccessInfo.memberAccessInfos = 
                Service_Ping_Result_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_Ping_Result);

                    if (candidateTypeSize > RTIXCdrLong_MAX) {
                        Service_Ping_Result_g_sampleAccessInfo.typeSize[0] =
                        RTIXCdrLong_MAX;
                    } else {
                        Service_Ping_Result_g_sampleAccessInfo.typeSize[0] =
                        (RTIXCdrUnsignedLong) candidateTypeSize;
                    }
                }

                Service_Ping_Result_g_sampleAccessInfo.useGetMemberValueOnlyWithRef =
                RTI_XCDR_TRUE;

                Service_Ping_Result_g_sampleAccessInfo.getMemberValuePointerFcn = 
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_Ping_Result >;

                Service_Ping_Result_g_sampleAccessInfo.languageBinding = 
                RTI_XCDR_TYPE_BINDING_CPP_11_STL ;

                RTIXCdrHeap_freeStruct(sample);
                is_initialized.store(true, std::memory_order_release);
                return (RTIXCdrSampleAccessInfo*) &Service_Ping_Result_g_sampleAccessInfo;
            }
            static RTIXCdrTypePlugin * type_plugin_info()
            {
                static RTIXCdrTypePlugin Service_Ping_Result_g_typePlugin = 
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

                return &Service_Ping_Result_g_typePlugin;
            }
        }; // native_type_code

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceCase::Service_Ping_Result >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_Ping_Result >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_Call > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Call_g_tc_members[2]=
                {

                    {
                        (char *)"ping",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(CorpusInterfaceCase::Service_ping_Hash),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"Ping",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(CorpusInterfaceCase::Service_Ping_Hash),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_Call_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_Call", /* Name */
                        NULL,     /* Base class type code is assigned later */      
                        0, /* Ignored */
                        0, /* Ignored */
                        NULL, /* Ignored */
                        2, /* Number of members */
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

                Service_Call_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceCase::Service_ping_In>::get().native();
                Service_Call_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceCase::Service_Ping_In>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Service_Call_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_Call_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Call_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_Call_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_Call *sample;

                static RTIXCdrMemberAccessInfo Service_Call_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Call_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Call_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_Call);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Call_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = CorpusInterfaceCase::Service_ping_Hash;
                Service_Call_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->ping() - (char *)sample);

                sample->_d() = CorpusInterfaceCase::Service_Ping_Hash;
                Service_Call_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->Ping() - (char *)sample);

                Service_Call_g_sampleAccessInfo.memberAccessInfos = 
                Service_Call_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_Call);

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
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_Call >;

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

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceCase::Service_Call >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_Call >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Service_Return > {

            static DDS_TypeCode * get()
            {
                using namespace ::rti::topic::interpreter;

                static std::atomic_bool is_initialized {false};

                static DDS_TypeCode_Member Service_Return_g_tc_members[2]=
                {

                    {
                        (char *)"ping",/* Member name */
                        {
                            1,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(CorpusInterfaceCase::Service_ping_Hash),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }, 
                    {
                        (char *)"Ping",/* Member name */
                        {
                            2,/* Representation ID */
                            DDS_BOOLEAN_FALSE,/* Is a pointer? */
                            -1, /* Bitfield bits */
                            NULL/* Member type code is assigned later */
                        },
                        0, /* Ignored */
                        1, /* Number of labels */
                        static_cast<int>(CorpusInterfaceCase::Service_Ping_Hash),
                        NULL, /* Labels (it is NULL when there is only one label)*/
                        RTI_CDR_NONKEY_MEMBER, /* Is a key? */
                        DDS_PUBLIC_MEMBER,/* Member visibility */
                        1,
                        NULL, /* Ignored */
                        RTICdrTypeCodeAnnotations_INITIALIZER
                    }
                };

                static DDS_TypeCode Service_Return_g_tc =
                {{
                        DDS_TK_UNION, /* Kind */
                        DDS_BOOLEAN_FALSE, /* Ignored */
                        -1, /*Ignored*/
                        (char *)"CorpusInterfaceCase::Service_Return", /* Name */
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

                Service_Return_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceCase::Service_ping_Result>::get().native();
                Service_Return_g_tc_members[1]._representation._typeCode = (RTICdrTypeCode *)&::rti::topic::dynamic_type< ::CorpusInterfaceCase::Service_Ping_Result>::get().native();

                /* Initialize the values for member annotations. */

                /* Discriminator type code */
                Service_Return_g_tc._data._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

                Service_Return_g_tc._data._sampleAccessInfo = sample_access_info();
                Service_Return_g_tc._data._typePlugin = type_plugin_info();    

                is_initialized.store(true, std::memory_order_release);

                return &Service_Return_g_tc;
            }

            static RTIXCdrSampleAccessInfo * sample_access_info()
            {
                static std::atomic_bool is_initialized {false};

                ::CorpusInterfaceCase::Service_Return *sample;

                static RTIXCdrMemberAccessInfo Service_Return_g_memberAccessInfos[3] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Service_Return_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Service_Return_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Service_Return);
                if (sample == NULL) {
                    return NULL;
                }

                Service_Return_g_memberAccessInfos[0].bindingMemberValueOffset[0] =
                (RTIXCdrUnsignedLong) ((char *)&sample->_d() - (char *)sample);

                sample->_d() = CorpusInterfaceCase::Service_ping_Hash;
                Service_Return_g_memberAccessInfos[1].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->ping() - (char *)sample);

                sample->_d() = CorpusInterfaceCase::Service_Ping_Hash;
                Service_Return_g_memberAccessInfos[2].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->Ping() - (char *)sample);

                Service_Return_g_sampleAccessInfo.memberAccessInfos = 
                Service_Return_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Service_Return);

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
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Service_Return >;

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

        const ::dds::core::xtypes::UnionType& dynamic_type< ::CorpusInterfaceCase::Service_Return >::get()
        {
            return static_cast<const ::dds::core::xtypes::UnionType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Service_Return >::get())));
        }

        template<>
        struct native_type_code< ::CorpusInterfaceCase::Sample > {

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
                        (char *)"CorpusInterfaceCase::Sample", /* Name */
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

                Sample_g_tc_members[0]._representation._typeCode = (RTICdrTypeCode *)&DDS_g_tc_long;

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

                ::CorpusInterfaceCase::Sample *sample;

                static RTIXCdrMemberAccessInfo Sample_g_memberAccessInfos[1] =
                {RTIXCdrMemberAccessInfo_INITIALIZER};

                static RTIXCdrSampleAccessInfo Sample_g_sampleAccessInfo = 
                RTIXCdrSampleAccessInfo_INITIALIZER;

                if (is_initialized.load(std::memory_order_acquire)) {
                    return (RTIXCdrSampleAccessInfo*) &Sample_g_sampleAccessInfo;
                }

                RTIXCdrHeap_allocateStruct(
                    &sample, 
                    ::CorpusInterfaceCase::Sample);
                if (sample == NULL) {
                    return NULL;
                }

                Sample_g_memberAccessInfos[0].bindingMemberValueOffset[0] = 
                (RTIXCdrUnsignedLong) ((char *)&sample->number() - (char *)sample);

                Sample_g_sampleAccessInfo.memberAccessInfos = 
                Sample_g_memberAccessInfos;

                {
                    size_t candidateTypeSize = sizeof(::CorpusInterfaceCase::Sample);

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
                interpreter::get_aggregation_value_pointer< ::CorpusInterfaceCase::Sample >;

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

        const ::dds::core::xtypes::StructType& dynamic_type< ::CorpusInterfaceCase::Sample >::get()
        {
            return static_cast<const ::dds::core::xtypes::StructType&>(
                ::rti::core::native_conversions::cast_from_native< ::dds::core::xtypes::DynamicType >(
                    *(native_type_code< ::CorpusInterfaceCase::Sample >::get())));
        }
    }
}

namespace dds { 
    namespace topic {

        void topic_type_support< ::CorpusInterfaceCase::Service_ping_In >::reset_sample(::CorpusInterfaceCase::Service_ping_In& sample) 
        {
            sample.dummy(0);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_ping_In >::allocate_sample(::CorpusInterfaceCase::Service_ping_In& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_ping_Out >::reset_sample(::CorpusInterfaceCase::Service_ping_Out& sample) 
        {
            sample.dummy(0);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_ping_Out >::allocate_sample(::CorpusInterfaceCase::Service_ping_Out& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_ping_Result >::reset_sample(::CorpusInterfaceCase::Service_ping_Result& sample) 
        {
            sample._d() = 0;
            ::rti::topic::reset_sample(sample.result());

            sample._d() = ::CorpusInterfaceCase::Service_ping_Result::default_discriminator();
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_ping_Result >::allocate_sample(::CorpusInterfaceCase::Service_ping_Result& sample, int, int) 
        {
            sample._d() = 0;
            ::rti::topic::allocate_sample(sample.result(),  -1, -1);

            sample._d() = ::CorpusInterfaceCase::Service_ping_Result::default_discriminator();
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Ping_In >::reset_sample(::CorpusInterfaceCase::Service_Ping_In& sample) 
        {
            sample.dummy(0);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Ping_In >::allocate_sample(::CorpusInterfaceCase::Service_Ping_In& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Ping_Out >::reset_sample(::CorpusInterfaceCase::Service_Ping_Out& sample) 
        {
            sample.dummy(0);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Ping_Out >::allocate_sample(::CorpusInterfaceCase::Service_Ping_Out& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Ping_Result >::reset_sample(::CorpusInterfaceCase::Service_Ping_Result& sample) 
        {
            sample._d() = 0;
            ::rti::topic::reset_sample(sample.result());

            sample._d() = ::CorpusInterfaceCase::Service_Ping_Result::default_discriminator();
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Ping_Result >::allocate_sample(::CorpusInterfaceCase::Service_Ping_Result& sample, int, int) 
        {
            sample._d() = 0;
            ::rti::topic::allocate_sample(sample.result(),  -1, -1);

            sample._d() = ::CorpusInterfaceCase::Service_Ping_Result::default_discriminator();
        }
        void topic_type_support< ::CorpusInterfaceCase::Service_Call >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInterfaceCase::Service_CallPlugin_new,
                ::CorpusInterfaceCase::Service_CallPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInterfaceCase::Service_Call >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInterfaceCase::Service_Call& sample,
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

        void topic_type_support< ::CorpusInterfaceCase::Service_Call >::from_cdr_buffer(::CorpusInterfaceCase::Service_Call& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = Service_CallPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInterfaceCase::Service_Call from cdr buffer");
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Call >::reset_sample(::CorpusInterfaceCase::Service_Call& sample) 
        {
            sample._d() = CorpusInterfaceCase::Service_ping_Hash;
            ::rti::topic::reset_sample(sample.ping());
            sample._d() = CorpusInterfaceCase::Service_Ping_Hash;
            ::rti::topic::reset_sample(sample.Ping());

            sample._d() = ::CorpusInterfaceCase::Service_Call::default_discriminator();
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Call >::allocate_sample(::CorpusInterfaceCase::Service_Call& sample, int, int) 
        {
            sample._d() = CorpusInterfaceCase::Service_ping_Hash;
            ::rti::topic::allocate_sample(sample.ping(),  -1, -1);
            sample._d() = CorpusInterfaceCase::Service_Ping_Hash;
            ::rti::topic::allocate_sample(sample.Ping(),  -1, -1);

            sample._d() = ::CorpusInterfaceCase::Service_Call::default_discriminator();
        }
        void topic_type_support< ::CorpusInterfaceCase::Service_Return >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInterfaceCase::Service_ReturnPlugin_new,
                ::CorpusInterfaceCase::Service_ReturnPlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInterfaceCase::Service_Return >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInterfaceCase::Service_Return& sample,
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

        void topic_type_support< ::CorpusInterfaceCase::Service_Return >::from_cdr_buffer(::CorpusInterfaceCase::Service_Return& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = Service_ReturnPlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInterfaceCase::Service_Return from cdr buffer");
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Return >::reset_sample(::CorpusInterfaceCase::Service_Return& sample) 
        {
            sample._d() = CorpusInterfaceCase::Service_ping_Hash;
            ::rti::topic::reset_sample(sample.ping());
            sample._d() = CorpusInterfaceCase::Service_Ping_Hash;
            ::rti::topic::reset_sample(sample.Ping());

            sample._d() = ::CorpusInterfaceCase::Service_Return::default_discriminator();
        }

        void topic_type_support< ::CorpusInterfaceCase::Service_Return >::allocate_sample(::CorpusInterfaceCase::Service_Return& sample, int, int) 
        {
            sample._d() = CorpusInterfaceCase::Service_ping_Hash;
            ::rti::topic::allocate_sample(sample.ping(),  -1, -1);
            sample._d() = CorpusInterfaceCase::Service_Ping_Hash;
            ::rti::topic::allocate_sample(sample.Ping(),  -1, -1);

            sample._d() = ::CorpusInterfaceCase::Service_Return::default_discriminator();
        }
        void topic_type_support< ::CorpusInterfaceCase::Sample >:: register_type(
            ::dds::domain::DomainParticipant& participant,
            const std::string& type_name) 
        {

            ::rti::domain::register_type_plugin(
                participant,
                type_name,
                ::CorpusInterfaceCase::SamplePlugin_new,
                ::CorpusInterfaceCase::SamplePlugin_delete);
        }

        std::vector<char>& topic_type_support< ::CorpusInterfaceCase::Sample >::to_cdr_buffer(
            std::vector<char>& buffer, 
            const ::CorpusInterfaceCase::Sample& sample,
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

        void topic_type_support< ::CorpusInterfaceCase::Sample >::from_cdr_buffer(::CorpusInterfaceCase::Sample& sample, 
        const std::vector<char>& buffer)
        {

            RTIBool ok  = SamplePlugin_deserialize_from_cdr_buffer(
                &sample, 
                &buffer[0], 
                static_cast<unsigned int>(buffer.size()));
            ::rti::core::check_return_code(ok ? DDS_RETCODE_OK : DDS_RETCODE_ERROR,
            "Failed to create ::CorpusInterfaceCase::Sample from cdr buffer");
        }

        void topic_type_support< ::CorpusInterfaceCase::Sample >::reset_sample(::CorpusInterfaceCase::Sample& sample) 
        {
            sample.number(0);
        }

        void topic_type_support< ::CorpusInterfaceCase::Sample >::allocate_sample(::CorpusInterfaceCase::Sample& sample, int, int) 
        {
            RTIOsapiUtility_unusedParameter(sample);
        }
    }
}  

#endif // NDDS_STANDALONE_TYPE
