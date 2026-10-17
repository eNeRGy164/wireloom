/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
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

        public struct NarrowTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.NarrowText>
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

            public void FromNative(global::CorpusStringAliasChains.NarrowText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.NarrowText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class NarrowTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.NarrowText, NarrowTextUnmanaged>
        {

            internal NarrowTextPlugin() : base("global::CorpusStringAliasChains.NarrowText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 255));
                var aliasType = tsf.CreateAliasWithAccessInfo<NarrowTextUnmanaged>(
                    dtf,
                    "NarrowText",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class NarrowTextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.NarrowText>
    {
        public NarrowTextSupport() : base(
            new Implementation.NarrowTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NarrowTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NarrowTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NarrowTextSupport, global::CorpusStringAliasChains.NarrowText>();

    }

    namespace Implementation
    {

        public struct NarrowTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.NarrowTextChain>
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

            public void FromNative(global::CorpusStringAliasChains.NarrowTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.NarrowTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class NarrowTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.NarrowTextChain, NarrowTextChainUnmanaged>
        {

            internal NarrowTextChainPlugin() : base("global::CorpusStringAliasChains.NarrowTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 255));
                var aliasType = tsf.CreateAliasWithAccessInfo<NarrowTextChainUnmanaged>(
                    dtf,
                    "NarrowTextChain",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class NarrowTextChainSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.NarrowTextChain>
    {
        public NarrowTextChainSupport() : base(
            new Implementation.NarrowTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NarrowTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NarrowTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NarrowTextChainSupport, global::CorpusStringAliasChains.NarrowTextChain>();

    }

    namespace Implementation
    {

        public struct BoundedNarrowTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.BoundedNarrowText>
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

            public void FromNative(global::CorpusStringAliasChains.BoundedNarrowText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.BoundedNarrowText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class BoundedNarrowTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.BoundedNarrowText, BoundedNarrowTextUnmanaged>
        {

            internal BoundedNarrowTextPlugin() : base("global::CorpusStringAliasChains.BoundedNarrowText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 16));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedNarrowTextUnmanaged>(
                    dtf,
                    "BoundedNarrowText",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class BoundedNarrowTextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.BoundedNarrowText>
    {
        public BoundedNarrowTextSupport() : base(
            new Implementation.BoundedNarrowTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextSupport, global::CorpusStringAliasChains.BoundedNarrowText>();

    }

    namespace Implementation
    {

        public struct BoundedNarrowTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.BoundedNarrowTextChain>
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

            public void FromNative(global::CorpusStringAliasChains.BoundedNarrowTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.BoundedNarrowTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class BoundedNarrowTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.BoundedNarrowTextChain, BoundedNarrowTextChainUnmanaged>
        {

            internal BoundedNarrowTextChainPlugin() : base("global::CorpusStringAliasChains.BoundedNarrowTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 16));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedNarrowTextChainUnmanaged>(
                    dtf,
                    "BoundedNarrowTextChain",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class BoundedNarrowTextChainSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.BoundedNarrowTextChain>
    {
        public BoundedNarrowTextChainSupport() : base(
            new Implementation.BoundedNarrowTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextChainSupport, global::CorpusStringAliasChains.BoundedNarrowTextChain>();

    }

    namespace Implementation
    {

        public struct BoundedNarrowTextChain2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.BoundedNarrowTextChain2>
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

            public void FromNative(global::CorpusStringAliasChains.BoundedNarrowTextChain2 sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.BoundedNarrowTextChain2 sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 16));
            }
        }

        internal class BoundedNarrowTextChain2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.BoundedNarrowTextChain2, BoundedNarrowTextChain2Unmanaged>
        {

            internal BoundedNarrowTextChain2Plugin() : base("global::CorpusStringAliasChains.BoundedNarrowTextChain2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 16));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedNarrowTextChain2Unmanaged>(
                    dtf,
                    "BoundedNarrowTextChain2",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class BoundedNarrowTextChain2Support : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.BoundedNarrowTextChain2>
    {
        public BoundedNarrowTextChain2Support() : base(
            new Implementation.BoundedNarrowTextChain2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextChain2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextChain2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextChain2Support, global::CorpusStringAliasChains.BoundedNarrowTextChain2>();

    }

    namespace Implementation
    {

        public struct WideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.WideText>
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

            public void FromNative(global::CorpusStringAliasChains.WideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.WideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class WideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.WideText, WideTextUnmanaged>
        {

            internal WideTextPlugin() : base("global::CorpusStringAliasChains.WideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 255));
                var aliasType = tsf.CreateAliasWithAccessInfo<WideTextUnmanaged>(
                    dtf,
                    "WideText",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class WideTextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.WideText>
    {
        public WideTextSupport() : base(
            new Implementation.WideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextSupport, global::CorpusStringAliasChains.WideText>();

    }

    namespace Implementation
    {

        public struct WideTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.WideTextChain>
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

            public void FromNative(global::CorpusStringAliasChains.WideTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.WideTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class WideTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.WideTextChain, WideTextChainUnmanaged>
        {

            internal WideTextChainPlugin() : base("global::CorpusStringAliasChains.WideTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 255));
                var aliasType = tsf.CreateAliasWithAccessInfo<WideTextChainUnmanaged>(
                    dtf,
                    "WideTextChain",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class WideTextChainSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.WideTextChain>
    {
        public WideTextChainSupport() : base(
            new Implementation.WideTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextChainSupport, global::CorpusStringAliasChains.WideTextChain>();

    }

    namespace Implementation
    {

        public struct BoundedWideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.BoundedWideText>
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

            public void FromNative(global::CorpusStringAliasChains.BoundedWideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.BoundedWideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedWideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.BoundedWideText, BoundedWideTextUnmanaged>
        {

            internal BoundedWideTextPlugin() : base("global::CorpusStringAliasChains.BoundedWideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 8));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedWideTextUnmanaged>(
                    dtf,
                    "BoundedWideText",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class BoundedWideTextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.BoundedWideText>
    {
        public BoundedWideTextSupport() : base(
            new Implementation.BoundedWideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextSupport, global::CorpusStringAliasChains.BoundedWideText>();

    }

    namespace Implementation
    {

        public struct BoundedWideTextChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.BoundedWideTextChain>
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

            public void FromNative(global::CorpusStringAliasChains.BoundedWideTextChain sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.BoundedWideTextChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedWideTextChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.BoundedWideTextChain, BoundedWideTextChainUnmanaged>
        {

            internal BoundedWideTextChainPlugin() : base("global::CorpusStringAliasChains.BoundedWideTextChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 8));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedWideTextChainUnmanaged>(
                    dtf,
                    "BoundedWideTextChain",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class BoundedWideTextChainSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.BoundedWideTextChain>
    {
        public BoundedWideTextChainSupport() : base(
            new Implementation.BoundedWideTextChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextChainSupport, global::CorpusStringAliasChains.BoundedWideTextChain>();

    }

    namespace Implementation
    {

        public struct BoundedWideTextChain2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.BoundedWideTextChain2>
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

            public void FromNative(global::CorpusStringAliasChains.BoundedWideTextChain2 sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.BoundedWideTextChain2 sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedWideTextChain2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.BoundedWideTextChain2, BoundedWideTextChain2Unmanaged>
        {

            internal BoundedWideTextChain2Plugin() : base("global::CorpusStringAliasChains.BoundedWideTextChain2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 8));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedWideTextChain2Unmanaged>(
                    dtf,
                    "BoundedWideTextChain2",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class BoundedWideTextChain2Support : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.BoundedWideTextChain2>
    {
        public BoundedWideTextChain2Support() : base(
            new Implementation.BoundedWideTextChain2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextChain2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextChain2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextChain2Support, global::CorpusStringAliasChains.BoundedWideTextChain2>();

    }

} // namespace CorpusStringAliasChains
