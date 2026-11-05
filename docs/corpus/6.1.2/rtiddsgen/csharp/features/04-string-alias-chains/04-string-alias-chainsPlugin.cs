/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
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

namespace CorpusStringAliasChains
{

    namespace Implementation
    {

        public struct NarrowTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.NarrowText>
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

            public void FromNative(CorpusStringAliasChains.NarrowText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.NarrowText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class NarrowTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.NarrowText, NarrowTextUnmanaged>
        {
            internal NarrowTextPlugin() : base("CorpusStringAliasChains.NarrowText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NarrowTextUnmanaged>(
                    dtf,
                    "NarrowText",
                    dtf.CreateString(((int) 255)));
            }
        }
    }
    public class NarrowTextSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.NarrowText>
    {
        public NarrowTextSupport() : base(
            new Implementation.NarrowTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NarrowTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NarrowTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NarrowTextSupport, CorpusStringAliasChains.NarrowText>();
    }

    namespace Implementation
    {

        public struct NarrowTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.NarrowTextChain>
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

            public void FromNative(CorpusStringAliasChains.NarrowTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.NarrowTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class NarrowTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.NarrowTextChain, NarrowTextChainUnmanaged>
        {
            internal NarrowTextChainPlugin() : base("CorpusStringAliasChains.NarrowTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NarrowTextChainUnmanaged>(
                    dtf,
                    "NarrowTextChain",
                    dtf.CreateString(((int) 255)));
            }
        }
    }
    public class NarrowTextChainSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.NarrowTextChain>
    {
        public NarrowTextChainSupport() : base(
            new Implementation.NarrowTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NarrowTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NarrowTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NarrowTextChainSupport, CorpusStringAliasChains.NarrowTextChain>();
    }

    namespace Implementation
    {

        public struct BoundedNarrowTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.BoundedNarrowText>
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

            public void FromNative(CorpusStringAliasChains.BoundedNarrowText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.BoundedNarrowText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class BoundedNarrowTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.BoundedNarrowText, BoundedNarrowTextUnmanaged>
        {
            internal BoundedNarrowTextPlugin() : base("CorpusStringAliasChains.BoundedNarrowText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedNarrowTextUnmanaged>(
                    dtf,
                    "BoundedNarrowText",
                    dtf.CreateString(((int) 16)));
            }
        }
    }
    public class BoundedNarrowTextSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.BoundedNarrowText>
    {
        public BoundedNarrowTextSupport() : base(
            new Implementation.BoundedNarrowTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextSupport, CorpusStringAliasChains.BoundedNarrowText>();
    }

    namespace Implementation
    {

        public struct BoundedNarrowTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.BoundedNarrowTextChain>
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

            public void FromNative(CorpusStringAliasChains.BoundedNarrowTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.BoundedNarrowTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class BoundedNarrowTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.BoundedNarrowTextChain, BoundedNarrowTextChainUnmanaged>
        {
            internal BoundedNarrowTextChainPlugin() : base("CorpusStringAliasChains.BoundedNarrowTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedNarrowTextChainUnmanaged>(
                    dtf,
                    "BoundedNarrowTextChain",
                    dtf.CreateString(((int) 16)));
            }
        }
    }
    public class BoundedNarrowTextChainSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.BoundedNarrowTextChain>
    {
        public BoundedNarrowTextChainSupport() : base(
            new Implementation.BoundedNarrowTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextChainSupport, CorpusStringAliasChains.BoundedNarrowTextChain>();
    }

    namespace Implementation
    {

        public struct BoundedNarrowTextChain2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.BoundedNarrowTextChain2>
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

            public void FromNative(CorpusStringAliasChains.BoundedNarrowTextChain2 sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.BoundedNarrowTextChain2 sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class BoundedNarrowTextChain2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.BoundedNarrowTextChain2, BoundedNarrowTextChain2Unmanaged>
        {
            internal BoundedNarrowTextChain2Plugin() : base("CorpusStringAliasChains.BoundedNarrowTextChain2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedNarrowTextChain2Unmanaged>(
                    dtf,
                    "BoundedNarrowTextChain2",
                    dtf.CreateString(((int) 16)));
            }
        }
    }
    public class BoundedNarrowTextChain2Support : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.BoundedNarrowTextChain2>
    {
        public BoundedNarrowTextChain2Support() : base(
            new Implementation.BoundedNarrowTextChain2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextChain2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextChain2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextChain2Support, CorpusStringAliasChains.BoundedNarrowTextChain2>();
    }

    namespace Implementation
    {

        public struct WideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.WideText>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusStringAliasChains.WideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.WideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class WideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.WideText, WideTextUnmanaged>
        {
            internal WideTextPlugin() : base("CorpusStringAliasChains.WideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<WideTextUnmanaged>(
                    dtf,
                    "WideText",
                    dtf.CreateWideString(((int) 255)));
            }
        }
    }
    public class WideTextSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.WideText>
    {
        public WideTextSupport() : base(
            new Implementation.WideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextSupport, CorpusStringAliasChains.WideText>();
    }

    namespace Implementation
    {

        public struct WideTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.WideTextChain>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusStringAliasChains.WideTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.WideTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class WideTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.WideTextChain, WideTextChainUnmanaged>
        {
            internal WideTextChainPlugin() : base("CorpusStringAliasChains.WideTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<WideTextChainUnmanaged>(
                    dtf,
                    "WideTextChain",
                    dtf.CreateWideString(((int) 255)));
            }
        }
    }
    public class WideTextChainSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.WideTextChain>
    {
        public WideTextChainSupport() : base(
            new Implementation.WideTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextChainSupport, CorpusStringAliasChains.WideTextChain>();
    }

    namespace Implementation
    {

        public struct BoundedWideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.BoundedWideText>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusStringAliasChains.BoundedWideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.BoundedWideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedWideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.BoundedWideText, BoundedWideTextUnmanaged>
        {
            internal BoundedWideTextPlugin() : base("CorpusStringAliasChains.BoundedWideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextUnmanaged>(
                    dtf,
                    "BoundedWideText",
                    dtf.CreateWideString(((int) 8)));
            }
        }
    }
    public class BoundedWideTextSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.BoundedWideText>
    {
        public BoundedWideTextSupport() : base(
            new Implementation.BoundedWideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextSupport, CorpusStringAliasChains.BoundedWideText>();
    }

    namespace Implementation
    {

        public struct BoundedWideTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.BoundedWideTextChain>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusStringAliasChains.BoundedWideTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.BoundedWideTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedWideTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.BoundedWideTextChain, BoundedWideTextChainUnmanaged>
        {
            internal BoundedWideTextChainPlugin() : base("CorpusStringAliasChains.BoundedWideTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextChainUnmanaged>(
                    dtf,
                    "BoundedWideTextChain",
                    dtf.CreateWideString(((int) 8)));
            }
        }
    }
    public class BoundedWideTextChainSupport : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.BoundedWideTextChain>
    {
        public BoundedWideTextChainSupport() : base(
            new Implementation.BoundedWideTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextChainSupport, CorpusStringAliasChains.BoundedWideTextChain>();
    }

    namespace Implementation
    {

        public struct BoundedWideTextChain2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusStringAliasChains.BoundedWideTextChain2>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusStringAliasChains.BoundedWideTextChain2 sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusStringAliasChains.BoundedWideTextChain2 sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedWideTextChain2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusStringAliasChains.BoundedWideTextChain2, BoundedWideTextChain2Unmanaged>
        {
            internal BoundedWideTextChain2Plugin() : base("CorpusStringAliasChains.BoundedWideTextChain2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextChain2Unmanaged>(
                    dtf,
                    "BoundedWideTextChain2",
                    dtf.CreateWideString(((int) 8)));
            }
        }
    }
    public class BoundedWideTextChain2Support : Rti.Dds.Topics.TypeSupport<CorpusStringAliasChains.BoundedWideTextChain2>
    {
        public BoundedWideTextChain2Support() : base(
            new Implementation.BoundedWideTextChain2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextChain2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextChain2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextChain2Support, CorpusStringAliasChains.BoundedWideTextChain2>();
    }

} // namespace CorpusStringAliasChains

