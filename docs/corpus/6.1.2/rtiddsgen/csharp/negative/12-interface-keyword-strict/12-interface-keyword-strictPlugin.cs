/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-keyword-strict.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
The rtiddsgen tool is part of the RTI Connext DDS distribution.
For more information, type 'rtiddsgen -help' at a command shell
or consult the Code Generator User's Manual.
*/

using System;
using System.Runtime.InteropServices;
using Omg.Types;
using Omg.Types.Dynamic;
using Rti.Types;
using Rti.Dds.Core;
using Rti.Types.Dynamic;
using Rti.Dds.NativeInterface.TypePlugin;

namespace CorpusInterfaceKeyword
{

    namespace Implementation
    {

        public struct Service_Context_InUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Service_Context_In>
        {

            private byte dummy;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusInterfaceKeyword.Service_Context_In sample, bool keysOnly = false)
            {

                sample.dummy = dummy;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dummy = (byte) 0;
            }

            public void ToNative(CorpusInterfaceKeyword.Service_Context_In sample, bool keysOnly = false)
            {
                dummy = sample.dummy;
            }
        }

        internal class Service_Context_InPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Service_Context_In, Service_Context_InUnmanaged>
        {
            internal Service_Context_InPlugin() : base("CorpusInterfaceKeyword.Service_Context_In", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Context_In struct
                var Service_Context_InStructMembers = new StructMember[]
                {
                    new StructMember("dummy", dtf.GetPrimitiveType<byte>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<Service_Context_InUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Service_Context_In")
                    .AddMembers(Service_Context_InStructMembers));
            }
        }
    }
    public class Service_Context_InSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Service_Context_In>
    {
        public Service_Context_InSupport() : base(
            new Implementation.Service_Context_InPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_Context_InPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_Context_InSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_Context_InSupport, CorpusInterfaceKeyword.Service_Context_In>();
    }

    namespace Implementation
    {

        public struct Service_Context_OutUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Service_Context_Out>
        {

            private byte dummy;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusInterfaceKeyword.Service_Context_Out sample, bool keysOnly = false)
            {

                sample.dummy = dummy;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dummy = (byte) 0;
            }

            public void ToNative(CorpusInterfaceKeyword.Service_Context_Out sample, bool keysOnly = false)
            {
                dummy = sample.dummy;
            }
        }

        internal class Service_Context_OutPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Service_Context_Out, Service_Context_OutUnmanaged>
        {
            internal Service_Context_OutPlugin() : base("CorpusInterfaceKeyword.Service_Context_Out", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Context_Out struct
                var Service_Context_OutStructMembers = new StructMember[]
                {
                    new StructMember("dummy", dtf.GetPrimitiveType<byte>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<Service_Context_OutUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Service_Context_Out")
                    .AddMembers(Service_Context_OutStructMembers));
            }
        }
    }
    public class Service_Context_OutSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Service_Context_Out>
    {
        public Service_Context_OutSupport() : base(
            new Implementation.Service_Context_OutPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_Context_OutPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_Context_OutSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_Context_OutSupport, CorpusInterfaceKeyword.Service_Context_Out>();
    }

    namespace Implementation
    {

        public struct Service_Context_ResultUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Service_Context_Result>
        {

            private int _d;

            private CorpusInterfaceKeyword.Implementation.Service_Context_OutUnmanaged result;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                result.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusInterfaceKeyword.Service_Context_Result sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.result = new CorpusInterfaceKeyword.Service_Context_Out();
                    }
                    result.FromNative(sample.result, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_Context_Result.DefaultDiscriminator;
                result.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusInterfaceKeyword.Service_Context_Result sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    result.ToNative(sample.result, keysOnly: false);
                    break;
                }
            }
        }

        internal class Service_Context_ResultPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Service_Context_Result, Service_Context_ResultUnmanaged>
        {
            internal Service_Context_ResultPlugin() : base("CorpusInterfaceKeyword.Service_Context_Result", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Context_Result union
                var Service_Context_ResultStructMembers = new UnionMember[]
                {
                    new UnionMember("result", CorpusInterfaceKeyword.Service_Context_OutSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 0}, id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<Service_Context_ResultUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Service_Context_Result")
                    .AddMembers(Service_Context_ResultStructMembers));
            }
        }
    }
    public class Service_Context_ResultSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Service_Context_Result>
    {
        public Service_Context_ResultSupport() : base(
            new Implementation.Service_Context_ResultPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_Context_ResultPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_Context_ResultSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_Context_ResultSupport, CorpusInterfaceKeyword.Service_Context_Result>();
    }

    namespace Implementation
    {

        public struct Service_CallUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Service_Call>
        {

            private int _d;

            private CorpusInterfaceKeyword.Implementation.Service_Context_InUnmanaged Context;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Context.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusInterfaceKeyword.Service_Call sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                        if(sample.Discriminator != _d)
                        {
                            sample.Context = new CorpusInterfaceKeyword.Service_Context_In();
                    }
                    Context.FromNative(sample.Context, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_Call.DefaultDiscriminator;
                Context.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusInterfaceKeyword.Service_Call sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                        Context.ToNative(sample.Context, keysOnly: false);
                        break;
                }
            }
        }

        internal class Service_CallPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Service_Call, Service_CallUnmanaged>
        {
            internal Service_CallPlugin() : base("CorpusInterfaceKeyword.Service_Call", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Call union
                var Service_CallStructMembers = new UnionMember[]
                {
                    new UnionMember("Context", CorpusInterfaceKeyword.Service_Context_InSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) (CorpusInterfaceKeyword.Service_Context_Hash.Value)}, id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<Service_CallUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Service_Call")
                    .AddMembers(Service_CallStructMembers));
            }
        }
    }
    public class Service_CallSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Service_Call>
    {
        public Service_CallSupport() : base(
            new Implementation.Service_CallPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_CallPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_CallSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_CallSupport, CorpusInterfaceKeyword.Service_Call>();
    }

    namespace Implementation
    {

        public struct Service_ReturnUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Service_Return>
        {

            private int _d;

            private CorpusInterfaceKeyword.Implementation.Service_Context_ResultUnmanaged Context;

            public void Destroy(bool optionalsOnly)
            {
                Context.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusInterfaceKeyword.Service_Return sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                        if(sample.Discriminator != _d)
                        {
                            sample.Context = new CorpusInterfaceKeyword.Service_Context_Result();
                    }
                    Context.FromNative(sample.Context, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_Return.DefaultDiscriminator;
                Context.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusInterfaceKeyword.Service_Return sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case (CorpusInterfaceKeyword.Service_Context_Hash.Value):
                        Context.ToNative(sample.Context, keysOnly: false);
                        break;
                }
            }
        }

        internal class Service_ReturnPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Service_Return, Service_ReturnUnmanaged>
        {
            internal Service_ReturnPlugin() : base("CorpusInterfaceKeyword.Service_Return", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Return union
                var Service_ReturnStructMembers = new UnionMember[]
                {
                    new UnionMember("Context", CorpusInterfaceKeyword.Service_Context_ResultSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) (CorpusInterfaceKeyword.Service_Context_Hash.Value)}, id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<Service_ReturnUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Service_Return")
                    .AddMembers(Service_ReturnStructMembers));
            }
        }
    }
    public class Service_ReturnSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Service_Return>
    {
        public Service_ReturnSupport() : base(
            new Implementation.Service_ReturnPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_ReturnPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_ReturnSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_ReturnSupport, CorpusInterfaceKeyword.Service_Return>();
    }

    namespace Implementation
    {

        public struct ServiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Service>
        {

            private Non_basic Context;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusInterfaceKeyword.Service sample, bool keysOnly = false)
            {

                sample.Context = Context;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Context = (Non_basic) ${member.unmanageDefaultValue};
            }

            public void ToNative(CorpusInterfaceKeyword.Service sample, bool keysOnly = false)
            {
                Context = sample.Context;
            }
        }

        internal class ServicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Service, ServiceUnmanaged>
        {
            internal ServicePlugin() : base("CorpusInterfaceKeyword.Service", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service struct
                var ServiceStructMembers = new StructMember[]
                {
                    new StructMember("Context", dtf.GetPrimitiveType<Non_basic>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<ServiceUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Service")
                    .AddMembers(ServiceStructMembers));
            }
        }
    }
    public class ServiceSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Service>
    {
        public ServiceSupport() : base(
            new Implementation.ServicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ServicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ServiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ServiceSupport, CorpusInterfaceKeyword.Service>();
    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInterfaceKeyword.Sample>
        {

            private int number;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusInterfaceKeyword.Sample sample, bool keysOnly = false)
            {

                sample.number = number;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                number = (int) 0;
            }

            public void ToNative(CorpusInterfaceKeyword.Sample sample, bool keysOnly = false)
            {
                number = sample.number;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInterfaceKeyword.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusInterfaceKeyword.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("number", dtf.GetPrimitiveType<int>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceKeyword::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusInterfaceKeyword.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusInterfaceKeyword.Sample>();
    }

} // namespace CorpusInterfaceKeyword

