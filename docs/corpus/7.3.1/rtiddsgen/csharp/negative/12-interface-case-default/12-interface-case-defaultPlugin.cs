/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-case-default.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

namespace CorpusInterfaceCase
{

    namespace Implementation
    {

        public struct Service_ping_InUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_ping_In>
        {

            private byte dummy;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceCase.Service_ping_In sample, bool keysOnly = false)
            {

                sample.dummy = dummy;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dummy = (byte) (0);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_ping_In sample, bool keysOnly = false)
            {
                dummy = sample.dummy;
            }
        }

        internal class Service_ping_InPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_ping_In, Service_ping_InUnmanaged>
        {

            internal Service_ping_InPlugin() : base("global::CorpusInterfaceCase.Service_ping_In", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_ping_In struct
                var Service_ping_InStructMembers = new StructMember[]
                {
                    new StructMember("dummy", dtf.GetPrimitiveType<byte>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_ping_InUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_ping_In")
                    .AddMembers(Service_ping_InStructMembers));

                return result;
            }
        }
    }
    public class Service_ping_InSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_ping_In>
    {
        public Service_ping_InSupport() : base(
            new Implementation.Service_ping_InPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_ping_InPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_ping_InSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_ping_InSupport, global::CorpusInterfaceCase.Service_ping_In>();

    }

    namespace Implementation
    {

        public struct Service_ping_OutUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_ping_Out>
        {

            private byte dummy;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceCase.Service_ping_Out sample, bool keysOnly = false)
            {

                sample.dummy = dummy;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dummy = (byte) (0);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_ping_Out sample, bool keysOnly = false)
            {
                dummy = sample.dummy;
            }
        }

        internal class Service_ping_OutPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_ping_Out, Service_ping_OutUnmanaged>
        {

            internal Service_ping_OutPlugin() : base("global::CorpusInterfaceCase.Service_ping_Out", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_ping_Out struct
                var Service_ping_OutStructMembers = new StructMember[]
                {
                    new StructMember("dummy", dtf.GetPrimitiveType<byte>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_ping_OutUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_ping_Out")
                    .AddMembers(Service_ping_OutStructMembers));

                return result;
            }
        }
    }
    public class Service_ping_OutSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_ping_Out>
    {
        public Service_ping_OutSupport() : base(
            new Implementation.Service_ping_OutPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_ping_OutPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_ping_OutSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_ping_OutSupport, global::CorpusInterfaceCase.Service_ping_Out>();

    }

    namespace Implementation
    {

        public struct Service_ping_ResultUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_ping_Result>
        {

            private int _d;

            private global::CorpusInterfaceCase.Implementation.Service_ping_OutUnmanaged result;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                result.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusInterfaceCase.Service_ping_Result sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.result = new global::CorpusInterfaceCase.Service_ping_Out();
                    }
                    result.FromNative(sample.result, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_ping_Result.DefaultDiscriminator;
                result.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_ping_Result sample, bool keysOnly = false)
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

        internal class Service_ping_ResultPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_ping_Result, Service_ping_ResultUnmanaged>
        {

            internal Service_ping_ResultPlugin() : base("global::CorpusInterfaceCase.Service_ping_Result", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_ping_Result union
                var Service_ping_ResultStructMembers = new UnionMember[]
                {
                    new UnionMember("result", global::CorpusInterfaceCase.Service_ping_OutSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 0}, id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_ping_ResultUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_ping_Result")
                    .AddMembers(Service_ping_ResultStructMembers));

                return result;
            }
        }
    }
    public class Service_ping_ResultSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_ping_Result>
    {
        public Service_ping_ResultSupport() : base(
            new Implementation.Service_ping_ResultPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_ping_ResultPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_ping_ResultSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_ping_ResultSupport, global::CorpusInterfaceCase.Service_ping_Result>();

    }

    namespace Implementation
    {

        public struct Service_Ping_InUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_Ping_In>
        {

            private byte dummy;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceCase.Service_Ping_In sample, bool keysOnly = false)
            {

                sample.dummy = dummy;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dummy = (byte) (0);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_Ping_In sample, bool keysOnly = false)
            {
                dummy = sample.dummy;
            }
        }

        internal class Service_Ping_InPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_Ping_In, Service_Ping_InUnmanaged>
        {

            internal Service_Ping_InPlugin() : base("global::CorpusInterfaceCase.Service_Ping_In", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Ping_In struct
                var Service_Ping_InStructMembers = new StructMember[]
                {
                    new StructMember("dummy", dtf.GetPrimitiveType<byte>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_Ping_InUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_Ping_In")
                    .AddMembers(Service_Ping_InStructMembers));

                return result;
            }
        }
    }
    public class Service_Ping_InSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_Ping_In>
    {
        public Service_Ping_InSupport() : base(
            new Implementation.Service_Ping_InPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_Ping_InPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_Ping_InSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_Ping_InSupport, global::CorpusInterfaceCase.Service_Ping_In>();

    }

    namespace Implementation
    {

        public struct Service_Ping_OutUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_Ping_Out>
        {

            private byte dummy;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceCase.Service_Ping_Out sample, bool keysOnly = false)
            {

                sample.dummy = dummy;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dummy = (byte) (0);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_Ping_Out sample, bool keysOnly = false)
            {
                dummy = sample.dummy;
            }
        }

        internal class Service_Ping_OutPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_Ping_Out, Service_Ping_OutUnmanaged>
        {

            internal Service_Ping_OutPlugin() : base("global::CorpusInterfaceCase.Service_Ping_Out", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Ping_Out struct
                var Service_Ping_OutStructMembers = new StructMember[]
                {
                    new StructMember("dummy", dtf.GetPrimitiveType<byte>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_Ping_OutUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_Ping_Out")
                    .AddMembers(Service_Ping_OutStructMembers));

                return result;
            }
        }
    }
    public class Service_Ping_OutSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_Ping_Out>
    {
        public Service_Ping_OutSupport() : base(
            new Implementation.Service_Ping_OutPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_Ping_OutPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_Ping_OutSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_Ping_OutSupport, global::CorpusInterfaceCase.Service_Ping_Out>();

    }

    namespace Implementation
    {

        public struct Service_Ping_ResultUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_Ping_Result>
        {

            private int _d;

            private global::CorpusInterfaceCase.Implementation.Service_Ping_OutUnmanaged result;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                result.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusInterfaceCase.Service_Ping_Result sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.result = new global::CorpusInterfaceCase.Service_Ping_Out();
                    }
                    result.FromNative(sample.result, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_Ping_Result.DefaultDiscriminator;
                result.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_Ping_Result sample, bool keysOnly = false)
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

        internal class Service_Ping_ResultPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_Ping_Result, Service_Ping_ResultUnmanaged>
        {

            internal Service_Ping_ResultPlugin() : base("global::CorpusInterfaceCase.Service_Ping_Result", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Ping_Result union
                var Service_Ping_ResultStructMembers = new UnionMember[]
                {
                    new UnionMember("result", global::CorpusInterfaceCase.Service_Ping_OutSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 0}, id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_Ping_ResultUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_Ping_Result")
                    .AddMembers(Service_Ping_ResultStructMembers));

                return result;
            }
        }
    }
    public class Service_Ping_ResultSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_Ping_Result>
    {
        public Service_Ping_ResultSupport() : base(
            new Implementation.Service_Ping_ResultPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_Ping_ResultPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_Ping_ResultSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_Ping_ResultSupport, global::CorpusInterfaceCase.Service_Ping_Result>();

    }

    namespace Implementation
    {

        public struct Service_CallUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_Call>
        {

            private int _d;

            private global::CorpusInterfaceCase.Implementation.Service_ping_InUnmanaged ping;
            private global::CorpusInterfaceCase.Implementation.Service_Ping_InUnmanaged Ping;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                ping.Destroy(optionalsOnly);
                Ping.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusInterfaceCase.Service_Call sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case (CorpusInterfaceCase.Service_ping_Hash.Value):
                        if(sample.Discriminator != _d)
                        {
                            sample.ping = new global::CorpusInterfaceCase.Service_ping_In();
                    }
                    ping.FromNative(sample.ping, keysOnly: false);
                    break;
                    case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                        if(sample.Discriminator != _d)
                        {
                            sample.Ping = new global::CorpusInterfaceCase.Service_Ping_In();
                    }
                    Ping.FromNative(sample.Ping, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_Call.DefaultDiscriminator;
                ping.Initialize(allocatePointers, allocateMemory);
                Ping.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_Call sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case (CorpusInterfaceCase.Service_ping_Hash.Value):
                        ping.ToNative(sample.ping, keysOnly: false);
                        break;
                    case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                        Ping.ToNative(sample.Ping, keysOnly: false);
                        break;
                }
            }
        }

        internal class Service_CallPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_Call, Service_CallUnmanaged>
        {

            internal Service_CallPlugin() : base("global::CorpusInterfaceCase.Service_Call", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Call union
                var Service_CallStructMembers = new UnionMember[]
                {
                    new UnionMember("ping", global::CorpusInterfaceCase.Service_ping_InSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) (CorpusInterfaceCase.Service_ping_Hash.Value)}, id: 1),
                    new UnionMember("Ping", global::CorpusInterfaceCase.Service_Ping_InSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) (CorpusInterfaceCase.Service_Ping_Hash.Value)}, id: 2)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_CallUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_Call")
                    .AddMembers(Service_CallStructMembers));

                return result;
            }
        }
    }
    public class Service_CallSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_Call>
    {
        public Service_CallSupport() : base(
            new Implementation.Service_CallPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_CallPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_CallSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_CallSupport, global::CorpusInterfaceCase.Service_Call>();

    }

    namespace Implementation
    {

        public struct Service_ReturnUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service_Return>
        {

            private int _d;

            private global::CorpusInterfaceCase.Implementation.Service_ping_ResultUnmanaged ping;
            private global::CorpusInterfaceCase.Implementation.Service_Ping_ResultUnmanaged Ping;

            public void Destroy(bool optionalsOnly)
            {
                ping.Destroy(optionalsOnly);
                Ping.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusInterfaceCase.Service_Return sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case (CorpusInterfaceCase.Service_ping_Hash.Value):
                        if(sample.Discriminator != _d)
                        {
                            sample.ping = new global::CorpusInterfaceCase.Service_ping_Result();
                    }
                    ping.FromNative(sample.ping, keysOnly: false);
                    break;
                    case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                        if(sample.Discriminator != _d)
                        {
                            sample.Ping = new global::CorpusInterfaceCase.Service_Ping_Result();
                    }
                    Ping.FromNative(sample.Ping, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Service_Return.DefaultDiscriminator;
                ping.Initialize(allocatePointers, allocateMemory);
                Ping.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusInterfaceCase.Service_Return sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case (CorpusInterfaceCase.Service_ping_Hash.Value):
                        ping.ToNative(sample.ping, keysOnly: false);
                        break;
                    case (CorpusInterfaceCase.Service_Ping_Hash.Value):
                        Ping.ToNative(sample.Ping, keysOnly: false);
                        break;
                }
            }
        }

        internal class Service_ReturnPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service_Return, Service_ReturnUnmanaged>
        {

            internal Service_ReturnPlugin() : base("global::CorpusInterfaceCase.Service_Return", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service_Return union
                var Service_ReturnStructMembers = new UnionMember[]
                {
                    new UnionMember("ping", global::CorpusInterfaceCase.Service_ping_ResultSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) (CorpusInterfaceCase.Service_ping_Hash.Value)}, id: 1),
                    new UnionMember("Ping", global::CorpusInterfaceCase.Service_Ping_ResultSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) (CorpusInterfaceCase.Service_Ping_Hash.Value)}, id: 2)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<Service_ReturnUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service_Return")
                    .AddMembers(Service_ReturnStructMembers));

                return result;
            }
        }
    }
    public class Service_ReturnSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service_Return>
    {
        public Service_ReturnSupport() : base(
            new Implementation.Service_ReturnPlugin(),
            new Lazy<DynamicType>(() =>Implementation.Service_ReturnPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static Service_ReturnSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<Service_ReturnSupport, global::CorpusInterfaceCase.Service_Return>();

    }

    namespace Implementation
    {

        public struct ServiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Service>
        {

            private Non_basic ping;
            private Non_basic Ping;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceCase.Service sample, bool keysOnly = false)
            {

                sample.ping = ping;
                sample.Ping = Ping;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                ping = (Non_basic) (${member.unmanageDefaultValue});
                Ping = (Non_basic) (${member.unmanageDefaultValue});
            }

            public void ToNative(global::CorpusInterfaceCase.Service sample, bool keysOnly = false)
            {
                ping = sample.ping;
                Ping = sample.Ping;
            }
        }

        internal class ServicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Service, ServiceUnmanaged>
        {

            internal ServicePlugin() : base("global::CorpusInterfaceCase.Service", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Service struct
                var ServiceStructMembers = new StructMember[]
                {
                    new StructMember("ping", dtf.GetPrimitiveType<Non_basic>(), id: 0),
                    new StructMember("Ping", dtf.GetPrimitiveType<Non_basic>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ServiceUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Service")
                    .AddMembers(ServiceStructMembers));

                return result;
            }
        }
    }
    public class ServiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Service>
    {
        public ServiceSupport() : base(
            new Implementation.ServicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ServicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ServiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ServiceSupport, global::CorpusInterfaceCase.Service>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceCase.Sample>
        {

            private int number;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceCase.Sample sample, bool keysOnly = false)
            {

                sample.number = number;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                number = (int) (0);
            }

            public void ToNative(global::CorpusInterfaceCase.Sample sample, bool keysOnly = false)
            {
                number = sample.number;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceCase.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusInterfaceCase.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
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

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceCase::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceCase.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusInterfaceCase.Sample>();

    }

} // namespace CorpusInterfaceCase

