

/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-case-default.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

#ifndef _12_interface_case_default_379864112_hpp
#define _12_interface_case_default_379864112_hpp

#include <iosfwd>

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef RTIUSERDllExport
#define RTIUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef RTIUSERDllExport
#define RTIUSERDllExport __attribute__((visibility("default")))
#endif

#include "dds/core/SafeEnumeration.hpp"
#include "dds/core/String.hpp"
#include "dds/core/array.hpp"
#include "dds/core/vector.hpp"
#include "dds/core/External.hpp"
#include "rti/core/LongDouble.hpp"
#include "rti/core/Pointer.hpp"
#include "rti/core/array.hpp"
#include "rti/topic/TopicTraits.hpp"

#include "omg/types/string_view.hpp"

#include "omg/types/sequence.hpp"
#include "omg/types/optional.hpp"

#ifndef NDDS_STANDALONE_TYPE
#include "cdr/cdr_typeCode.h"
#include "dds/domain/DomainParticipant.hpp"
#include "dds/topic/TopicTraits.hpp"
#include "dds/core/xtypes/DynamicType.hpp"
#include "dds/core/xtypes/StructType.hpp"
#include "dds/core/xtypes/UnionType.hpp"
#include "dds/core/xtypes/EnumType.hpp"
#include "dds/core/xtypes/AliasType.hpp"
#include "rti/util/StreamFlagSaver.hpp"
#include "rti/domain/PluginSupport.hpp"
#endif

#include "dds/rpc/ClientEndpoint.hpp"
#include "dds/rpc/ServiceEndpoint.hpp"
#include "rti/rpc/ExceptionUtils.hpp"

#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef RTIUSERDllExport
#define RTIUSERDllExport
#endif

#if defined(NDDS_USER_DLL_EXPORT) && defined(RTI_WIN32)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __declspec(dllexport)
#endif

#if !defined(RTI_WIN32) && defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport __attribute__((visibility("default")))
#endif

namespace CorpusInterfaceCase {

    struct NDDSUSERDllExport Service_ping_In {

        Service_ping_In();

    };

    NDDSUSERDllExport bool operator == (const Service_ping_In& a, const Service_ping_In& b);
    NDDSUSERDllExport bool operator != (const Service_ping_In& a, const Service_ping_In& b);
    NDDSUSERDllExport void swap(Service_ping_In& a, Service_ping_In& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_ping_In& sample);

    struct NDDSUSERDllExport Service_ping_Out {

        Service_ping_Out();

    };

    NDDSUSERDllExport bool operator == (const Service_ping_Out& a, const Service_ping_Out& b);
    NDDSUSERDllExport bool operator != (const Service_ping_Out& a, const Service_ping_Out& b);
    NDDSUSERDllExport void swap(Service_ping_Out& a, Service_ping_Out& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_ping_Out& sample);

    class NDDSUSERDllExport Service_ping_Result {
      public:

        Service_ping_Result();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::CorpusInterfaceCase::Service_ping_Out& result()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_ping_Result::result not selected by the discriminator" );
            }
            return m_u_.result;
        }

        const ::CorpusInterfaceCase::Service_ping_Out& result() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_ping_Result::result not selected by the discriminator" );
            }
            return m_u_.result;
        }

        void result(const ::CorpusInterfaceCase::Service_ping_Out& value) {
            m_u_.result = value;
            m_d_= 0;
        }

        void result(::CorpusInterfaceCase::Service_ping_Out&& value) {
            m_u_.result = std::move(value);
            m_d_= 0;
        }
        void _default() {
            _d(::CorpusInterfaceCase::Service_ping_Result::default_discriminator());
        }

        bool operator == (const Service_ping_Result& other_) const;
        bool operator != (const Service_ping_Result& other_) const;
        static int32_t default_discriminator();

        void swap(Service_ping_Result& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            ::CorpusInterfaceCase::Service_ping_Out result {};
            Union_();
            Union_(const ::CorpusInterfaceCase::Service_ping_Out& result_);
        };
        Union_ m_u_;

    };

    inline void swap(Service_ping_Result& a, Service_ping_Result& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_ping_Result& sample);

    RTI_CONSTEXPR_OR_CONST_STRING int32_t Service_ping_Hash = 18846175L;

    struct NDDSUSERDllExport Service_Ping_In {

        Service_Ping_In();

    };

    NDDSUSERDllExport bool operator == (const Service_Ping_In& a, const Service_Ping_In& b);
    NDDSUSERDllExport bool operator != (const Service_Ping_In& a, const Service_Ping_In& b);
    NDDSUSERDllExport void swap(Service_Ping_In& a, Service_Ping_In& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_Ping_In& sample);

    struct NDDSUSERDllExport Service_Ping_Out {

        Service_Ping_Out();

    };

    NDDSUSERDllExport bool operator == (const Service_Ping_Out& a, const Service_Ping_Out& b);
    NDDSUSERDllExport bool operator != (const Service_Ping_Out& a, const Service_Ping_Out& b);
    NDDSUSERDllExport void swap(Service_Ping_Out& a, Service_Ping_Out& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_Ping_Out& sample);

    class NDDSUSERDllExport Service_Ping_Result {
      public:

        Service_Ping_Result();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::CorpusInterfaceCase::Service_Ping_Out& result()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Ping_Result::result not selected by the discriminator" );
            }
            return m_u_.result;
        }

        const ::CorpusInterfaceCase::Service_Ping_Out& result() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Ping_Result::result not selected by the discriminator" );
            }
            return m_u_.result;
        }

        void result(const ::CorpusInterfaceCase::Service_Ping_Out& value) {
            m_u_.result = value;
            m_d_= 0;
        }

        void result(::CorpusInterfaceCase::Service_Ping_Out&& value) {
            m_u_.result = std::move(value);
            m_d_= 0;
        }
        void _default() {
            _d(::CorpusInterfaceCase::Service_Ping_Result::default_discriminator());
        }

        bool operator == (const Service_Ping_Result& other_) const;
        bool operator != (const Service_Ping_Result& other_) const;
        static int32_t default_discriminator();

        void swap(Service_Ping_Result& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            ::CorpusInterfaceCase::Service_Ping_Out result {};
            Union_();
            Union_(const ::CorpusInterfaceCase::Service_Ping_Out& result_);
        };
        Union_ m_u_;

    };

    inline void swap(Service_Ping_Result& a, Service_Ping_Result& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_Ping_Result& sample);

    RTI_CONSTEXPR_OR_CONST_STRING int32_t Service_Ping_Hash = -803907400;

    class NDDSUSERDllExport Service_Call {
      public:

        Service_Call();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        ::CorpusInterfaceCase::Service_ping_In& ping()  {

            if (_d() != CorpusInterfaceCase::Service_ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Call::ping not selected by the discriminator" );
            }
            return m_u_.ping;
        }

        const ::CorpusInterfaceCase::Service_ping_In& ping() const  {

            if (_d() != CorpusInterfaceCase::Service_ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Call::ping not selected by the discriminator" );
            }
            return m_u_.ping;
        }

        void ping(const ::CorpusInterfaceCase::Service_ping_In& value) {
            m_u_.ping = value;
            m_d_= CorpusInterfaceCase::Service_ping_Hash;
        }

        void ping(::CorpusInterfaceCase::Service_ping_In&& value) {
            m_u_.ping = std::move(value);
            m_d_= CorpusInterfaceCase::Service_ping_Hash;
        }
        ::CorpusInterfaceCase::Service_Ping_In& Ping()  {

            if (_d() != CorpusInterfaceCase::Service_Ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Call::Ping not selected by the discriminator" );
            }
            return m_u_.Ping;
        }

        const ::CorpusInterfaceCase::Service_Ping_In& Ping() const  {

            if (_d() != CorpusInterfaceCase::Service_Ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Call::Ping not selected by the discriminator" );
            }
            return m_u_.Ping;
        }

        void Ping(const ::CorpusInterfaceCase::Service_Ping_In& value) {
            m_u_.Ping = value;
            m_d_= CorpusInterfaceCase::Service_Ping_Hash;
        }

        void Ping(::CorpusInterfaceCase::Service_Ping_In&& value) {
            m_u_.Ping = std::move(value);
            m_d_= CorpusInterfaceCase::Service_Ping_Hash;
        }
        void _default() {
            _d(::CorpusInterfaceCase::Service_Call::default_discriminator());
        }

        bool operator == (const Service_Call& other_) const;
        bool operator != (const Service_Call& other_) const;
        static int32_t default_discriminator();

        void swap(Service_Call& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            ::CorpusInterfaceCase::Service_ping_In ping {};
            ::CorpusInterfaceCase::Service_Ping_In Ping {};
            Union_();
            Union_(const ::CorpusInterfaceCase::Service_ping_In& ping_,const ::CorpusInterfaceCase::Service_Ping_In& Ping_);
        };
        Union_ m_u_;

    };

    inline void swap(Service_Call& a, Service_Call& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_Call& sample);

    class NDDSUSERDllExport Service_Return {
      public:

        Service_Return();

        int32_t& _d()  {
            return m_d_;
        }

        const int32_t& _d() const  {
            return m_d_;
        }

        void _d(int32_t value) {

            m_d_ = value;
        }

        int32_t& remoteEx()  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Return::remoteEx not selected by the discriminator" );
            }
            return m_u_.remoteEx;
        }

        const int32_t& remoteEx() const  {

            if (_d() != 0 ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Return::remoteEx not selected by the discriminator" );
            }
            return m_u_.remoteEx;
        }

        void remoteEx(int32_t value) {
            m_u_.remoteEx = value;
            m_d_= 0;
        }

        ::CorpusInterfaceCase::Service_ping_Result& ping()  {

            if (_d() != CorpusInterfaceCase::Service_ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Return::ping not selected by the discriminator" );
            }
            return m_u_.ping;
        }

        const ::CorpusInterfaceCase::Service_ping_Result& ping() const  {

            if (_d() != CorpusInterfaceCase::Service_ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Return::ping not selected by the discriminator" );
            }
            return m_u_.ping;
        }

        void ping(const ::CorpusInterfaceCase::Service_ping_Result& value) {
            m_u_.ping = value;
            m_d_= CorpusInterfaceCase::Service_ping_Hash;
        }

        void ping(::CorpusInterfaceCase::Service_ping_Result&& value) {
            m_u_.ping = std::move(value);
            m_d_= CorpusInterfaceCase::Service_ping_Hash;
        }
        ::CorpusInterfaceCase::Service_Ping_Result& Ping()  {

            if (_d() != CorpusInterfaceCase::Service_Ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Return::Ping not selected by the discriminator" );
            }
            return m_u_.Ping;
        }

        const ::CorpusInterfaceCase::Service_Ping_Result& Ping() const  {

            if (_d() != CorpusInterfaceCase::Service_Ping_Hash ) {

                throw ::dds::core::PreconditionNotMetError(
                    "::CorpusInterfaceCase::Service_Return::Ping not selected by the discriminator" );
            }
            return m_u_.Ping;
        }

        void Ping(const ::CorpusInterfaceCase::Service_Ping_Result& value) {
            m_u_.Ping = value;
            m_d_= CorpusInterfaceCase::Service_Ping_Hash;
        }

        void Ping(::CorpusInterfaceCase::Service_Ping_Result&& value) {
            m_u_.Ping = std::move(value);
            m_d_= CorpusInterfaceCase::Service_Ping_Hash;
        }
        void _default() {
            _d(::CorpusInterfaceCase::Service_Return::default_discriminator());
        }

        bool operator == (const Service_Return& other_) const;
        bool operator != (const Service_Return& other_) const;
        static int32_t default_discriminator();

        void swap(Service_Return& other_) noexcept ;

      private:

        int32_t m_d_ {};
        struct NDDSUSERDllExport Union_ {
            int32_t remoteEx {};
            ::CorpusInterfaceCase::Service_ping_Result ping {};
            ::CorpusInterfaceCase::Service_Ping_Result Ping {};
            Union_();
            Union_(int32_t remoteEx_,const ::CorpusInterfaceCase::Service_ping_Result& ping_,const ::CorpusInterfaceCase::Service_Ping_Result& Ping_);
        };
        Union_ m_u_;

    };

    inline void swap(Service_Return& a, Service_Return& b)  noexcept 
    {
        a.swap(b);
    }

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Service_Return& sample);

    // Service synchronous interface (implemented by Client and Service)
    class Service {
      public:
        virtual void ping(
            ) = 0;
        virtual void Ping(
            ) = 0;
        virtual ~Service()
        {
        }
    };
    // Service asynchronous interface (implemented by Client only)
    class ServiceAsync {
      public:
        virtual std::future< void> ping_async(
            ) = 0;
        virtual std::future< void> Ping_async(
            ) = 0;
        virtual ~ServiceAsync()
        {
        }
    };

    using ServiceClientEndpoint =
    dds::rpc::ClientEndpoint< Service_Call, Service_Return>;
    // ServiceClient provides the client-side functions for the Service
    // DDS Service
    class ServiceClient:
    public Service,
    public ServiceAsync,
    public ServiceClientEndpoint {

      public:
        using InterfaceType = Service;
        using InterfaceAsyncType = ServiceAsync;
        explicit ServiceClient(const dds::rpc::ClientParams& params);

        //Implementation of Service:
        void ping (
            ) override;
        void Ping (
            ) override;

        std::future< void> ping_async (
            ) override;
        std::future< void> Ping_async (
            ) override;

      private:

        rti::core::SampleIdentity send_ping (
            );
        rti::core::SampleIdentity send_Ping (
            );

        void receive_ping (
            const rti::core::SampleIdentity& request_id);
        void receive_Ping (
            const rti::core::SampleIdentity& request_id);

    };
    class ServiceDispatcher final {
      public:
        using InterfaceType = Service;
        using RequestType = Service_Call;
        using ReplyType = Service_Return;

        static void dispatch_request(
            InterfaceType *service_impl,
            ReplyType& result,
            const RequestType& request);

      private:
        static void dispatch_ping (
            InterfaceType* service_impl,
            ReplyType& result,
            const RequestType& request);
        static void dispatch_Ping (
            InterfaceType* service_impl,
            ReplyType& result,
            const RequestType& request);

    };
    using ServiceService =
    dds::rpc::ServiceEndpoint< ServiceDispatcher>;

    struct NDDSUSERDllExport Sample {
        int32_t number {};

        Sample();

        explicit Sample(int32_t number_);

    };

    NDDSUSERDllExport bool operator == (const Sample& a, const Sample& b);
    NDDSUSERDllExport bool operator != (const Sample& a, const Sample& b);
    NDDSUSERDllExport void swap(Sample& a, Sample& b)  noexcept;

    NDDSUSERDllExport std::ostream& operator<<(std::ostream& o, const Sample& sample);

} // namespace CorpusInterfaceCase  

#ifdef NDDS_STANDALONE_TYPE
namespace rti { 
    namespace topic {
    }
}
#else

namespace rti {
    namespace flat {
        namespace topic {
        }
    }
}
namespace dds {
    namespace topic {

        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_ping_In > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_ping_In";
            }
        };

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_ping_In > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_ping_In& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_ping_In& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_ping_Out > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_ping_Out";
            }
        };

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_ping_Out > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_ping_Out& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_ping_Out& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_ping_Result > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_ping_Result";
            }
        };

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_ping_Result > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_ping_Result& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_ping_Result& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_Ping_In > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_Ping_In";
            }
        };

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_Ping_In > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_Ping_In& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_Ping_In& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_Ping_Out > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_Ping_Out";
            }
        };

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_Ping_Out > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_Ping_Out& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_Ping_Out& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_Ping_Result > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_Ping_Result";
            }
        };

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_Ping_Result > {
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_Ping_Result& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_Ping_Result& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_Call > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_Call";
            }
        };

        template<>
        struct is_topic_type< ::CorpusInterfaceCase::Service_Call > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_Call > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusInterfaceCase::Service_Call& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusInterfaceCase::Service_Call& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_Call& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_Call& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Service_Return > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Service_Return";
            }
        };

        template<>
        struct is_topic_type< ::CorpusInterfaceCase::Service_Return > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Service_Return > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusInterfaceCase::Service_Return& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusInterfaceCase::Service_Return& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Service_Return& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Service_Return& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
        template<>
        struct topic_type_name< ::CorpusInterfaceCase::Sample > {
            NDDSUSERDllExport static std::string value() {
                return "CorpusInterfaceCase::Sample";
            }
        };

        template<>
        struct is_topic_type< ::CorpusInterfaceCase::Sample > : public ::dds::core::true_type {};

        template<>
        struct topic_type_support< ::CorpusInterfaceCase::Sample > {
            NDDSUSERDllExport 
            static void register_type(
                ::dds::domain::DomainParticipant& participant,
                const std::string & type_name);

            NDDSUSERDllExport 
            static std::vector<char>& to_cdr_buffer(
                std::vector<char>& buffer, 
                const ::CorpusInterfaceCase::Sample& sample,
                ::dds::core::policy::DataRepresentationId representation 
                = ::dds::core::policy::DataRepresentation::auto_id());

            NDDSUSERDllExport 
            static void from_cdr_buffer(::CorpusInterfaceCase::Sample& sample, const std::vector<char>& buffer);
            NDDSUSERDllExport 
            static void reset_sample(::CorpusInterfaceCase::Sample& sample);

            NDDSUSERDllExport 
            static void allocate_sample(::CorpusInterfaceCase::Sample& sample, int, int);

            static const ::rti::topic::TypePluginKind::type type_plugin_kind = 
            ::rti::topic::TypePluginKind::STL;
        };
    }
}

namespace rti { 
    namespace topic {

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_ping_In > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_ping_In > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_ping_Out > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_ping_Out > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_ping_Result > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_ping_Result > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_Ping_In > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_Ping_In > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_Ping_Out > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_Ping_Out > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_Ping_Result > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_Ping_Result > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_Call > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_Call > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Service_Return > {
            typedef ::dds::core::xtypes::UnionType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::UnionType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Service_Return > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

        template<>
        struct dynamic_type< ::CorpusInterfaceCase::Sample > {
            typedef ::dds::core::xtypes::StructType type;
            NDDSUSERDllExport static const ::dds::core::xtypes::StructType& get();
        };

        template <>
        struct extensibility< ::CorpusInterfaceCase::Sample > {
            static const ::dds::core::xtypes::ExtensibilityKind::type kind =
            ::dds::core::xtypes::ExtensibilityKind::EXTENSIBLE;    };

    }
}

#endif // NDDS_STANDALONE_TYPE
#if defined(NDDS_USER_DLL_EXPORT) || defined(NDDS_USER_SYMBOL_EXPORT)
#undef NDDSUSERDllExport
#define NDDSUSERDllExport
#endif

#endif // _12_interface_case_default_379864112_hpp
