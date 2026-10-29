/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-unbounded-support.idl
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

namespace CorpusUnboundedSupport
{

    namespace Implementation
    {

        public struct LongValuesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSupport.LongValues>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSupport.LongValues sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSupport.LongValues sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class LongValuesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSupport.LongValues, LongValuesUnmanaged>
        {

            internal LongValuesPlugin() : base("global::CorpusUnboundedSupport.LongValues", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<LongValuesUnmanaged>(
                    dtf,
                    "LongValues",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded));
            }
        }
    }
    public class LongValuesSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSupport.LongValues>
    {
        public LongValuesSupport() : base(
            new Implementation.LongValuesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.LongValuesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static LongValuesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<LongValuesSupport, global::CorpusUnboundedSupport.LongValues>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSupport.Sample>
        {

            private NativeSeq values;
            private NativeString text;
            private NativeWstring wideText;
            private NativeStringSeq labels;
            private global::CorpusUnboundedSupport.Implementation.LongValuesUnmanaged aliasValues;
            private NativeSeq boundedValues;
            private NativeString boundedText;

            public void Destroy(bool optionalsOnly)
            {
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

            public void FromNative(global::CorpusUnboundedSupport.Sample sample, bool keysOnly = false)
            {

                values.FromNative((Sequence<int>) sample.values);
                sample.text = text.FromNative();
                sample.wideText = wideText.FromNative();
                labels.FromNative(sample.labels);
                aliasValues.FromNative(sample.aliasValues, keysOnly: false);
                boundedValues.FromNative((Sequence<int>) sample.boundedValues);
                sample.boundedText = boundedText.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize<int >(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, allocateMemory: allocateMemory);
                text.Initialize(size: 0, allocateMemory: allocateMemory);
                wideText.Initialize(size: 0, allocateMemory: allocateMemory);
                labels.Initialize(max: 0, absoluteMax: Rti.Types.Dynamic.SequenceType.Unbounded, maxStrLen: StringType.Unbounded, allocateMemory: allocateMemory);
                aliasValues.Initialize(allocatePointers, allocateMemory);
                boundedValues.Initialize<int >(max: ((int)4), absoluteMax: ((int)4), allocateMemory: allocateMemory);
                boundedText.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSupport.Sample sample, bool keysOnly = false)
            {
                values.ToNative((Sequence<int>) sample.values);
                text.ToNative(sample.text, StringType.Unbounded);
                wideText.ToNative(sample.wideText, WideStringType.Unbounded);
                labels.ToNative(sample.labels, StringType.Unbounded);
                aliasValues.ToNative(sample.aliasValues, keysOnly: false);
                boundedValues.ToNative((Sequence<int>) sample.boundedValues);
                boundedText.ToNative(sample.boundedText, ((int) 8));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSupport.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusUnboundedSupport.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), Rti.Types.Dynamic.SequenceType.Unbounded), id: 0),
                    new StructMember("text", dtf.CreateString(), id: 1),
                    new StructMember("wideText", dtf.CreateWideString(), id: 2),
                    new StructMember("labels", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(), Rti.Types.Dynamic.SequenceType.Unbounded), id: 3),
                    new StructMember("aliasValues", global::CorpusUnboundedSupport.LongValuesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 4),
                    new StructMember("boundedValues", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)4)), id: 5),
                    new StructMember("boundedText", dtf.CreateString(((int) 8)), id: 6)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedSupport::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSupport.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusUnboundedSupport.Sample>();

    }

} // namespace CorpusUnboundedSupport

