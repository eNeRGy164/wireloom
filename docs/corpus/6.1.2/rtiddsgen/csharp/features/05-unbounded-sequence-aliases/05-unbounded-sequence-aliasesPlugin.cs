/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-unbounded-sequence-aliases.idl
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

namespace CorpusUnboundedSequences
{

    namespace Implementation
    {

        public struct NumbersUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.Numbers>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusUnboundedSequences.Numbers sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.Numbers sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class NumbersPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.Numbers, NumbersUnmanaged>
        {
            internal NumbersPlugin() : base("CorpusUnboundedSequences.Numbers", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NumbersUnmanaged>(
                    dtf,
                    "Numbers",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class NumbersSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.Numbers>
    {
        public NumbersSupport() : base(
            new Implementation.NumbersPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NumbersPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NumbersSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NumbersSupport, CorpusUnboundedSequences.Numbers>();
    }

    namespace Implementation
    {
        internal class StatePlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public StatePlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                return dtf.BuildEnum()
                .WithName("CorpusUnboundedSequences::State")
                .AddMember(new EnumMember("Idle", 0))
                .AddMember(new EnumMember("Ready", 1))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
            }
        }
    }

    public class StateSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.State>
    {
        public StateSupport() : base(
            new Implementation.StatePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StatePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StateSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StateSupport, CorpusUnboundedSequences.State>();
    }

    namespace Implementation
    {

        public struct ChildUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.Child>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusUnboundedSequences.Child sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) 0;
            }

            public void ToNative(CorpusUnboundedSequences.Child sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class ChildPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.Child, ChildUnmanaged>
        {
            internal ChildPlugin() : base("CorpusUnboundedSequences.Child", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Child struct
                var ChildStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<ChildUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedSequences::Child")
                    .AddMembers(ChildStructMembers));
            }
        }
    }
    public class ChildSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.Child>
    {
        public ChildSupport() : base(
            new Implementation.ChildPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChildPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChildSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChildSupport, CorpusUnboundedSequences.Child>();
    }

    namespace Implementation
    {

        public struct ScalarUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.Scalar>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusUnboundedSequences.Scalar sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) 0;
            }

            public void ToNative(CorpusUnboundedSequences.Scalar sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ScalarPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.Scalar, ScalarUnmanaged>
        {
            internal ScalarPlugin() : base("CorpusUnboundedSequences.Scalar", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ScalarUnmanaged>(
                    dtf,
                    "Scalar",
                    dtf.GetPrimitiveType<int>());
            }
        }
    }
    public class ScalarSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.Scalar>
    {
        public ScalarSupport() : base(
            new Implementation.ScalarPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarSupport, CorpusUnboundedSequences.Scalar>();
    }

    namespace Implementation
    {

        public struct StatesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.States>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusUnboundedSequences.States sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<CorpusUnboundedSequences.State>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusUnboundedSequences.State >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.States sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<CorpusUnboundedSequences.State>) sample.Value);
            }
        }

        internal class StatesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.States, StatesUnmanaged>
        {
            internal StatesPlugin() : base("CorpusUnboundedSequences.States", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<StatesUnmanaged>(
                    dtf,
                    "States",
                    tsf.CreateSequenceWithAccessInfo(dtf, CorpusUnboundedSequences.StateSupport.Instance.GetDynamicTypeInternal(isPublic), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class StatesSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.States>
    {
        public StatesSupport() : base(
            new Implementation.StatesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.StatesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StatesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StatesSupport, CorpusUnboundedSequences.States>();
    }

    namespace Implementation
    {

        public struct ChildrenUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.Children>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<CorpusUnboundedSequences.Child, CorpusUnboundedSequences.Implementation.ChildUnmanaged>(optionalsOnly);
            }

            public void FromNative(CorpusUnboundedSequences.Children sample, bool keysOnly = false)
            {

                Value.FromNative<CorpusUnboundedSequences.Child, CorpusUnboundedSequences.Implementation.ChildUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusUnboundedSequences.Child , CorpusUnboundedSequences.Implementation.ChildUnmanaged >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.Children sample, bool keysOnly = false)
            {
                Value.ToNative<CorpusUnboundedSequences.Child, CorpusUnboundedSequences.Implementation.ChildUnmanaged>(sample.Value);
            }
        }

        internal class ChildrenPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.Children, ChildrenUnmanaged>
        {
            internal ChildrenPlugin() : base("CorpusUnboundedSequences.Children", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ChildrenUnmanaged>(
                    dtf,
                    "Children",
                    tsf.CreateSequenceWithAccessInfo(dtf, CorpusUnboundedSequences.ChildSupport.Instance.GetDynamicTypeInternal(isPublic), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class ChildrenSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.Children>
    {
        public ChildrenSupport() : base(
            new Implementation.ChildrenPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChildrenPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChildrenSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChildrenSupport, CorpusUnboundedSequences.Children>();
    }

    namespace Implementation
    {

        public struct ScalarsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.Scalars>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusUnboundedSequences.Scalars sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.Scalars sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class ScalarsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.Scalars, ScalarsUnmanaged>
        {
            internal ScalarsPlugin() : base("CorpusUnboundedSequences.Scalars", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ScalarsUnmanaged>(
                    dtf,
                    "Scalars",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class ScalarsSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.Scalars>
    {
        public ScalarsSupport() : base(
            new Implementation.ScalarsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarsSupport, CorpusUnboundedSequences.Scalars>();
    }

    namespace Implementation
    {

        public struct TextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.Texts>
        {

            private NativeStringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusUnboundedSequences.Texts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: StringType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.Texts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, StringType.Unbounded);
            }
        }

        internal class TextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.Texts, TextsUnmanaged>
        {
            internal TextsPlugin() : base("CorpusUnboundedSequences.Texts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<TextsUnmanaged>(
                    dtf,
                    "Texts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class TextsSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.Texts>
    {
        public TextsSupport() : base(
            new Implementation.TextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.TextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static TextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TextsSupport, CorpusUnboundedSequences.Texts>();
    }

    namespace Implementation
    {

        public struct WideTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.WideTexts>
        {

            private NativeWstringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusUnboundedSequences.WideTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: WideStringType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.WideTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, WideStringType.Unbounded);
            }
        }

        internal class WideTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.WideTexts, WideTextsUnmanaged>
        {
            internal WideTextsPlugin() : base("CorpusUnboundedSequences.WideTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<WideTextsUnmanaged>(
                    dtf,
                    "WideTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class WideTextsSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.WideTexts>
    {
        public WideTextsSupport() : base(
            new Implementation.WideTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextsSupport, CorpusUnboundedSequences.WideTexts>();
    }

    namespace Implementation
    {

        public struct BoundedTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.BoundedTexts>
        {

            private NativeStringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusUnboundedSequences.BoundedTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.BoundedTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.BoundedTexts, BoundedTextsUnmanaged>
        {
            internal BoundedTextsPlugin() : base("CorpusUnboundedSequences.BoundedTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedTextsUnmanaged>(
                    dtf,
                    "BoundedTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 8)), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class BoundedTextsSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.BoundedTexts>
    {
        public BoundedTextsSupport() : base(
            new Implementation.BoundedTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedTextsSupport, CorpusUnboundedSequences.BoundedTexts>();
    }

    namespace Implementation
    {

        public struct BoundedWideTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.BoundedWideTexts>
        {

            private NativeWstringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusUnboundedSequences.BoundedWideTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: ((int) 4), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.BoundedWideTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 4));
            }
        }

        internal class BoundedWideTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.BoundedWideTexts, BoundedWideTextsUnmanaged>
        {
            internal BoundedWideTextsPlugin() : base("CorpusUnboundedSequences.BoundedWideTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextsUnmanaged>(
                    dtf,
                    "BoundedWideTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 4)), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class BoundedWideTextsSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.BoundedWideTexts>
    {
        public BoundedWideTextsSupport() : base(
            new Implementation.BoundedWideTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextsSupport, CorpusUnboundedSequences.BoundedWideTexts>();
    }

    namespace Implementation
    {

        public struct NumbersChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedSequences.NumbersChain>
        {

            private CorpusUnboundedSequences.Implementation.NumbersUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusUnboundedSequences.NumbersChain sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusUnboundedSequences.NumbersChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class NumbersChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedSequences.NumbersChain, NumbersChainUnmanaged>
        {
            internal NumbersChainPlugin() : base("CorpusUnboundedSequences.NumbersChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NumbersChainUnmanaged>(
                    dtf,
                    "NumbersChain",
                    CorpusUnboundedSequences.NumbersSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class NumbersChainSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedSequences.NumbersChain>
    {
        public NumbersChainSupport() : base(
            new Implementation.NumbersChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NumbersChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NumbersChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NumbersChainSupport, CorpusUnboundedSequences.NumbersChain>();
    }

} // namespace CorpusUnboundedSequences

