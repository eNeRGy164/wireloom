/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-unbounded-sequence-aliases.idl
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

namespace CorpusUnboundedSequences
{

    namespace Implementation
    {

        public struct NumbersUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.Numbers>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSequences.Numbers sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.Numbers sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class NumbersPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.Numbers, NumbersUnmanaged>
        {

            internal NumbersPlugin() : base("global::CorpusUnboundedSequences.Numbers", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<NumbersUnmanaged>(
                    dtf,
                    "Numbers",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class NumbersSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.Numbers>
    {
        public NumbersSupport() : base(
            new Implementation.NumbersPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NumbersPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NumbersSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NumbersSupport, global::CorpusUnboundedSequences.Numbers>();

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
                var enumType = dtf.BuildEnum()
                .WithName("CorpusUnboundedSequences::State")
                .AddMember(new EnumMember("Idle", 0))
                .AddMember(new EnumMember("Ready", 1))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 0;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    enumType.SetAnnotations(
                        annotations);
                }
                return enumType;
            }
        }
    }

    public class StateSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.State>
    {
        public StateSupport() : base(
            new Implementation.StatePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StatePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StateSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StateSupport, global::CorpusUnboundedSequences.State>();

    }

    namespace Implementation
    {

        public struct ChildUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.Child>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusUnboundedSequences.Child sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusUnboundedSequences.Child sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class ChildPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.Child, ChildUnmanaged>
        {

            internal ChildPlugin() : base("global::CorpusUnboundedSequences.Child", isKeyed: false, CreateDynamicType(isPublic: false))
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

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChildUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedSequences::Child")
                    .AddMembers(ChildStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ChildSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.Child>
    {
        public ChildSupport() : base(
            new Implementation.ChildPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChildPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChildSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChildSupport, global::CorpusUnboundedSequences.Child>();

    }

    namespace Implementation
    {

        public struct ScalarUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.Scalar>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusUnboundedSequences.Scalar sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) (0);
            }

            public void ToNative(global::CorpusUnboundedSequences.Scalar sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ScalarPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.Scalar, ScalarUnmanaged>
        {

            internal ScalarPlugin() : base("global::CorpusUnboundedSequences.Scalar", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ScalarUnmanaged>(
                    dtf,
                    "Scalar",
                    dtf.GetPrimitiveType<int>());
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class ScalarSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.Scalar>
    {
        public ScalarSupport() : base(
            new Implementation.ScalarPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarSupport, global::CorpusUnboundedSequences.Scalar>();

    }

    namespace Implementation
    {

        public struct StatesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.States>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSequences.States sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<global::CorpusUnboundedSequences.State>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<global::CorpusUnboundedSequences.State >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.States sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<global::CorpusUnboundedSequences.State>) sample.Value);
            }
        }

        internal class StatesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.States, StatesUnmanaged>
        {

            internal StatesPlugin() : base("global::CorpusUnboundedSequences.States", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<StatesUnmanaged>(
                    dtf,
                    "States",
                    tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusUnboundedSequences.StateSupport.Instance.GetDynamicTypeInternal(isPublic), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class StatesSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.States>
    {
        public StatesSupport() : base(
            new Implementation.StatesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.StatesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StatesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StatesSupport, global::CorpusUnboundedSequences.States>();

    }

    namespace Implementation
    {

        public struct ChildrenUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.Children>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<global::CorpusUnboundedSequences.Child, global::CorpusUnboundedSequences.Implementation.ChildUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSequences.Children sample, bool keysOnly = false)
            {

                Value.FromNative<global::CorpusUnboundedSequences.Child, global::CorpusUnboundedSequences.Implementation.ChildUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<global::CorpusUnboundedSequences.Child , global::CorpusUnboundedSequences.Implementation.ChildUnmanaged >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.Children sample, bool keysOnly = false)
            {
                Value.ToNative<global::CorpusUnboundedSequences.Child, global::CorpusUnboundedSequences.Implementation.ChildUnmanaged>(sample.Value);
            }
        }

        internal class ChildrenPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.Children, ChildrenUnmanaged>
        {

            internal ChildrenPlugin() : base("global::CorpusUnboundedSequences.Children", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ChildrenUnmanaged>(
                    dtf,
                    "Children",
                    tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusUnboundedSequences.ChildSupport.Instance.GetDynamicTypeInternal(isPublic), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class ChildrenSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.Children>
    {
        public ChildrenSupport() : base(
            new Implementation.ChildrenPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChildrenPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChildrenSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChildrenSupport, global::CorpusUnboundedSequences.Children>();

    }

    namespace Implementation
    {

        public struct ScalarsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.Scalars>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSequences.Scalars sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.Scalars sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class ScalarsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.Scalars, ScalarsUnmanaged>
        {

            internal ScalarsPlugin() : base("global::CorpusUnboundedSequences.Scalars", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ScalarsUnmanaged>(
                    dtf,
                    "Scalars",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class ScalarsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.Scalars>
    {
        public ScalarsSupport() : base(
            new Implementation.ScalarsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarsSupport, global::CorpusUnboundedSequences.Scalars>();

    }

    namespace Implementation
    {

        public struct TextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.Texts>
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

            public void FromNative(global::CorpusUnboundedSequences.Texts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: StringType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.Texts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, StringType.Unbounded);
            }
        }

        internal class TextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.Texts, TextsUnmanaged>
        {

            internal TextsPlugin() : base("global::CorpusUnboundedSequences.Texts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString();
                var aliasType = tsf.CreateAliasWithAccessInfo<TextsUnmanaged>(
                    dtf,
                    "Texts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class TextsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.Texts>
    {
        public TextsSupport() : base(
            new Implementation.TextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.TextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static TextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TextsSupport, global::CorpusUnboundedSequences.Texts>();

    }

    namespace Implementation
    {

        public struct WideTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.WideTexts>
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

            public void FromNative(global::CorpusUnboundedSequences.WideTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: WideStringType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.WideTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, WideStringType.Unbounded);
            }
        }

        internal class WideTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.WideTexts, WideTextsUnmanaged>
        {

            internal WideTextsPlugin() : base("global::CorpusUnboundedSequences.WideTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString();
                var aliasType = tsf.CreateAliasWithAccessInfo<WideTextsUnmanaged>(
                    dtf,
                    "WideTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class WideTextsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.WideTexts>
    {
        public WideTextsSupport() : base(
            new Implementation.WideTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextsSupport, global::CorpusUnboundedSequences.WideTexts>();

    }

    namespace Implementation
    {

        public struct BoundedTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.BoundedTexts>
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

            public void FromNative(global::CorpusUnboundedSequences.BoundedTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.BoundedTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.BoundedTexts, BoundedTextsUnmanaged>
        {

            internal BoundedTextsPlugin() : base("global::CorpusUnboundedSequences.BoundedTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 8));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedTextsUnmanaged>(
                    dtf,
                    "BoundedTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 8)), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class BoundedTextsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.BoundedTexts>
    {
        public BoundedTextsSupport() : base(
            new Implementation.BoundedTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedTextsSupport, global::CorpusUnboundedSequences.BoundedTexts>();

    }

    namespace Implementation
    {

        public struct BoundedWideTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.BoundedWideTexts>
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

            public void FromNative(global::CorpusUnboundedSequences.BoundedWideTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: ((int) 4), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.BoundedWideTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 4));
            }
        }

        internal class BoundedWideTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.BoundedWideTexts, BoundedWideTextsUnmanaged>
        {

            internal BoundedWideTextsPlugin() : base("global::CorpusUnboundedSequences.BoundedWideTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 4));
                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedWideTextsUnmanaged>(
                    dtf,
                    "BoundedWideTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 4)), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class BoundedWideTextsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.BoundedWideTexts>
    {
        public BoundedWideTextsSupport() : base(
            new Implementation.BoundedWideTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextsSupport, global::CorpusUnboundedSequences.BoundedWideTexts>();

    }

    namespace Implementation
    {

        public struct NumbersChainUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.NumbersChain>
        {

            private global::CorpusUnboundedSequences.Implementation.NumbersUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSequences.NumbersChain sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.NumbersChain sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class NumbersChainPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.NumbersChain, NumbersChainUnmanaged>
        {

            internal NumbersChainPlugin() : base("global::CorpusUnboundedSequences.NumbersChain", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<NumbersChainUnmanaged>(
                    dtf,
                    "NumbersChain",
                    global::CorpusUnboundedSequences.NumbersSupport.Instance.GetDynamicTypeInternal(isPublic));
                return aliasType;

            }
        }
    }
    public class NumbersChainSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.NumbersChain>
    {
        public NumbersChainSupport() : base(
            new Implementation.NumbersChainPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NumbersChainPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NumbersChainSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NumbersChainSupport, global::CorpusUnboundedSequences.NumbersChain>();

    }

} // namespace CorpusUnboundedSequences

