/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-alias-inheritance.idl
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

namespace CorpusIntegrationTrace
{

    namespace Implementation
    {

        public struct ProducerUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationTrace.Producer>
        {

            private NativeString Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusIntegrationTrace.Producer sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusIntegrationTrace.Producer sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class ProducerPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationTrace.Producer, ProducerUnmanaged>
        {
            internal ProducerPlugin() : base("CorpusIntegrationTrace.Producer", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ProducerUnmanaged>(
                    dtf,
                    "Producer",
                    dtf.CreateString(((int) 16)));
            }
        }
    }
    public class ProducerSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationTrace.Producer>
    {
        public ProducerSupport() : base(
            new Implementation.ProducerPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ProducerPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ProducerSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ProducerSupport, CorpusIntegrationTrace.Producer>();
    }

    namespace Implementation
    {

        public struct ContextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationTrace.Context>
        {

            private NativeString correlation;
            private NativeString producer;

            public void Destroy(bool optionalsOnly)
            {
                producer.Destroy();
                if (optionalsOnly)
                {
                    return;
                }
                correlation.Destroy();
            }

            public void FromNative(CorpusIntegrationTrace.Context sample, bool keysOnly = false)
            {

                sample.correlation = correlation.FromNative();
                sample.producer = producer.FromNativeOptional();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                correlation.Initialize(size: ((int) 32), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusIntegrationTrace.Context sample, bool keysOnly = false)
            {
                correlation.ToNative(sample.correlation, ((int) 32));
                producer.ToNativeOptional(sample.producer, ((int) 16));
            }
        }

        internal class ContextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationTrace.Context, ContextUnmanaged>
        {
            internal ContextPlugin() : base("CorpusIntegrationTrace.Context", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Context struct
                var ContextStructMembers = new StructMember[]
                {
                    new StructMember("correlation", dtf.CreateString(((int) 32)), id: 0),
                    new StructMember("producer", CorpusIntegrationTrace.ProducerSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ContextUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIntegrationTrace::Context")
                    .AddMembers(ContextStructMembers));
            }
        }
    }
    public class ContextSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationTrace.Context>
    {
        public ContextSupport() : base(
            new Implementation.ContextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ContextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ContextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ContextSupport, CorpusIntegrationTrace.Context>();
    }

    namespace Implementation
    {

        public struct BaseUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationTrace.Base>
        {

            private int baseValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusIntegrationTrace.Base sample, bool keysOnly = false)
            {

                sample.baseValue = baseValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                baseValue = (int) 0;
            }

            public void ToNative(CorpusIntegrationTrace.Base sample, bool keysOnly = false)
            {
                baseValue = sample.baseValue;
            }
        }

        internal class BasePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationTrace.Base, BaseUnmanaged>
        {
            internal BasePlugin() : base("CorpusIntegrationTrace.Base", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Base struct
                var BaseStructMembers = new StructMember[]
                {
                    new StructMember("baseValue", dtf.GetPrimitiveType<int>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<BaseUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIntegrationTrace::Base")
                    .AddMembers(BaseStructMembers));
            }
        }
    }
    public class BaseSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationTrace.Base>
    {
        public BaseSupport() : base(
            new Implementation.BasePlugin(),
            new Lazy<DynamicType>(() =>Implementation.BasePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BaseSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BaseSupport, CorpusIntegrationTrace.Base>();
    }

    namespace Implementation
    {

        public struct DerivedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationTrace.Derived>
        {
            private CorpusIntegrationTrace.Implementation.BaseUnmanaged parent;

            private NativeString state;
            private CorpusIntegrationTrace.Implementation.ContextUnmanaged traceContext;

            public void Destroy(bool optionalsOnly)
            {
                parent.Destroy(optionalsOnly);
                traceContext.Destroy(optionalsOnly);
                if (optionalsOnly)
                {
                    return;
                }
                state.Destroy();
            }

            public void FromNative(CorpusIntegrationTrace.Derived sample, bool keysOnly = false)
            {

                parent.FromNative(sample, keysOnly);
                sample.state = state.FromNative();
                traceContext.FromNative(sample.traceContext, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                parent.Initialize(allocatePointers, allocateMemory);
                state.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
                traceContext.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusIntegrationTrace.Derived sample, bool keysOnly = false)
            {
                parent.ToNative(sample, keysOnly);
                state.ToNative(sample.state, ((int) 16));
                traceContext.ToNative(sample.traceContext, keysOnly: false);
            }
        }

        internal class DerivedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationTrace.Derived, DerivedUnmanaged>
        {
            internal DerivedPlugin() : base("CorpusIntegrationTrace.Derived", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Derived struct
                var DerivedStructMembers = new StructMember[]
                {
                    new StructMember("state", dtf.CreateString(((int) 16)), id: 1),
                    new StructMember("traceContext", CorpusIntegrationTrace.ContextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2)
                };

                return tsf.CreateTypeWithAccessInfo<DerivedUnmanaged>(
                    dtf.BuildStruct()
                    .WithParent((StructType) CorpusIntegrationTrace.BaseSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIntegrationTrace::Derived")
                    .AddMembers(DerivedStructMembers));
            }
        }
    }
    public class DerivedSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationTrace.Derived>
    {
        public DerivedSupport() : base(
            new Implementation.DerivedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DerivedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DerivedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DerivedSupport, CorpusIntegrationTrace.Derived>();
    }

} // namespace CorpusIntegrationTrace

