/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-receive-lifecycle.idl
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

namespace CorpusUnboundedReceiveLifecycle
{

    namespace Implementation
    {

        public struct LongValuesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedReceiveLifecycle.LongValues>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedReceiveLifecycle.LongValues sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedReceiveLifecycle.LongValues sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class LongValuesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedReceiveLifecycle.LongValues, LongValuesUnmanaged>
        {

            internal LongValuesPlugin() : base("global::CorpusUnboundedReceiveLifecycle.LongValues", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<LongValuesUnmanaged>(
                    dtf,
                    "LongValues",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded));
                return aliasType;

            }
        }
    }
    public class LongValuesSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedReceiveLifecycle.LongValues>
    {
        public LongValuesSupport() : base(
            new Implementation.LongValuesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.LongValuesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static LongValuesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<LongValuesSupport, global::CorpusUnboundedReceiveLifecycle.LongValues>();

    }

    namespace Implementation
    {

        public struct OptionalStateUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedReceiveLifecycle.OptionalState>
        {

            private int value;
            private NativeString note;
            private NativeStringSeq labels;

            public void Destroy(bool optionalsOnly)
            {
                note.Destroy();
                if (optionalsOnly)
                {
                    return;
                }
                labels.Destroy();
            }

            public void FromNative(global::CorpusUnboundedReceiveLifecycle.OptionalState sample, bool keysOnly = false)
            {

                sample.value = value;
                sample.note = note.FromNativeOptional();
                labels.FromNative(sample.labels);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
                labels.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: StringType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedReceiveLifecycle.OptionalState sample, bool keysOnly = false)
            {
                value = sample.value;
                note.ToNativeOptional(sample.note, StringType.Unbounded);
                labels.ToNative(sample.labels, StringType.Unbounded);
            }
        }

        internal class OptionalStatePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedReceiveLifecycle.OptionalState, OptionalStateUnmanaged>
        {

            internal OptionalStatePlugin() : base("global::CorpusUnboundedReceiveLifecycle.OptionalState", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // OptionalState struct
                var OptionalStateStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("note", dtf.CreateString(), isOptional: true, id: 1),
                    new StructMember("labels", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(), Rti.Types.Dynamic.SequenceType.Unbounded), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<OptionalStateUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedReceiveLifecycle::OptionalState")
                    .AddMembers(OptionalStateStructMembers));

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
    public class OptionalStateSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedReceiveLifecycle.OptionalState>
    {
        public OptionalStateSupport() : base(
            new Implementation.OptionalStatePlugin(),
            new Lazy<DynamicType>(() =>Implementation.OptionalStatePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static OptionalStateSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<OptionalStateSupport, global::CorpusUnboundedReceiveLifecycle.OptionalState>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedReceiveLifecycle.Sample>
        {

            private int phaseId;
            private NativeSeq values;
            private NativeString text;
            private NativeWstring wideText;
            private NativeStringSeq labels;
            private global::CorpusUnboundedReceiveLifecycle.Implementation.LongValuesUnmanaged aliasValues;
            private NativeSeq boundedValues;
            private NativeString boundedText;
            private NativeManagedOptional optionalState;

            public void Destroy(bool optionalsOnly)
            {
                optionalState.Destroy<global::CorpusUnboundedReceiveLifecycle.OptionalState, global::CorpusUnboundedReceiveLifecycle.Implementation.OptionalStateUnmanaged>(optionalsOnly);
                if (optionalsOnly)
                {
                    return;
                }
                values.Destroy(optionalsOnly);
                text.Destroy();
                wideText.Destroy();
                labels.Destroy();
                aliasValues.Destroy(optionalsOnly);
                boundedValues.Destroy(optionalsOnly);
                boundedText.Destroy();
            }

            public void FromNative(global::CorpusUnboundedReceiveLifecycle.Sample sample, bool keysOnly = false)
            {

                sample.phaseId = phaseId;
                values.FromNative((Sequence<int>) sample.values);
                sample.text = text.FromNative();
                sample.wideText = wideText.FromNative();
                labels.FromNative(sample.labels);
                aliasValues.FromNative(sample.aliasValues, keysOnly: false);
                boundedValues.FromNative((Sequence<int>) sample.boundedValues);
                sample.boundedText = boundedText.FromNative();
                optionalState.FromNative<global::CorpusUnboundedReceiveLifecycle.OptionalState, global::CorpusUnboundedReceiveLifecycle.Implementation.OptionalStateUnmanaged>(out var optionalStateTemporary_);
                sample.optionalState = optionalStateTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                phaseId = (int) (0);
                values.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
                text.Initialize(size: 0, allocateMemory: allocateMemory);
                wideText.Initialize(size: 0, allocateMemory: allocateMemory);
                labels.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: StringType.Unbounded, allocateMemory: allocateMemory);
                aliasValues.Initialize(allocatePointers, allocateMemory);
                boundedValues.Initialize<int >(max: ((int)4), absoluteMax: ((int)4), allocateMemory: allocateMemory);
                boundedText.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedReceiveLifecycle.Sample sample, bool keysOnly = false)
            {
                phaseId = sample.phaseId;
                values.ToNative((Sequence<int>) sample.values);
                text.ToNative(sample.text, StringType.Unbounded);
                wideText.ToNative(sample.wideText, WideStringType.Unbounded);
                labels.ToNative(sample.labels, StringType.Unbounded);
                aliasValues.ToNative(sample.aliasValues, keysOnly: false);
                boundedValues.ToNative((Sequence<int>) sample.boundedValues);
                boundedText.ToNative(sample.boundedText, ((int) 8));
                optionalState.ToNative<global::CorpusUnboundedReceiveLifecycle.OptionalState, global::CorpusUnboundedReceiveLifecycle.Implementation.OptionalStateUnmanaged>(sample.optionalState);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedReceiveLifecycle.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusUnboundedReceiveLifecycle.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("phaseId", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded), id: 1),
                    new StructMember("text", dtf.CreateString(), id: 2),
                    new StructMember("wideText", dtf.CreateWideString(), id: 3),
                    new StructMember("labels", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(), Rti.Types.Dynamic.SequenceType.Unbounded), id: 4),
                    new StructMember("aliasValues", global::CorpusUnboundedReceiveLifecycle.LongValuesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 5),
                    new StructMember("boundedValues", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)4)), id: 6),
                    new StructMember("boundedText", dtf.CreateString(((int) 8)), id: 7),
                    new StructMember("optionalState", global::CorpusUnboundedReceiveLifecycle.OptionalStateSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 8)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedReceiveLifecycle::Sample")
                    .AddMembers(SampleStructMembers));

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
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        2,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        3,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        7,
                        annotations);
                }

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedReceiveLifecycle.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusUnboundedReceiveLifecycle.Sample>();

    }

} // namespace CorpusUnboundedReceiveLifecycle

