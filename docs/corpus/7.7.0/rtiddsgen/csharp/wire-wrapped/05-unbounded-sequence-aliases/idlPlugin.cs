/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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

        public struct SequenceAliasesSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedSequences.SequenceAliasesSample>
        {

            private global::CorpusUnboundedSequences.Implementation.NumbersUnmanaged numbers;
            private global::CorpusUnboundedSequences.Implementation.StatesUnmanaged states;
            private global::CorpusUnboundedSequences.Implementation.ChildrenUnmanaged children;
            private global::CorpusUnboundedSequences.Implementation.ScalarsUnmanaged scalars;
            private global::CorpusUnboundedSequences.Implementation.TextsUnmanaged texts;
            private global::CorpusUnboundedSequences.Implementation.WideTextsUnmanaged wideTexts;
            private global::CorpusUnboundedSequences.Implementation.BoundedTextsUnmanaged boundedTexts;
            private global::CorpusUnboundedSequences.Implementation.BoundedWideTextsUnmanaged boundedWideTexts;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                numbers.Destroy(optionalsOnly);
                states.Destroy(optionalsOnly);
                children.Destroy(optionalsOnly);
                scalars.Destroy(optionalsOnly);
                texts.Destroy(optionalsOnly);
                wideTexts.Destroy(optionalsOnly);
                boundedTexts.Destroy(optionalsOnly);
                boundedWideTexts.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnboundedSequences.SequenceAliasesSample sample, bool keysOnly = false)
            {

                numbers.FromNative(sample.numbers, keysOnly: false);
                states.FromNative(sample.states, keysOnly: false);
                children.FromNative(sample.children, keysOnly: false);
                scalars.FromNative(sample.scalars, keysOnly: false);
                texts.FromNative(sample.texts, keysOnly: false);
                wideTexts.FromNative(sample.wideTexts, keysOnly: false);
                boundedTexts.FromNative(sample.boundedTexts, keysOnly: false);
                boundedWideTexts.FromNative(sample.boundedWideTexts, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                numbers.Initialize(allocatePointers, allocateMemory);
                states.Initialize(allocatePointers, allocateMemory);
                children.Initialize(allocatePointers, allocateMemory);
                scalars.Initialize(allocatePointers, allocateMemory);
                texts.Initialize(allocatePointers, allocateMemory);
                wideTexts.Initialize(allocatePointers, allocateMemory);
                boundedTexts.Initialize(allocatePointers, allocateMemory);
                boundedWideTexts.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedSequences.SequenceAliasesSample sample, bool keysOnly = false)
            {
                numbers.ToNative(sample.numbers, keysOnly: false);
                states.ToNative(sample.states, keysOnly: false);
                children.ToNative(sample.children, keysOnly: false);
                scalars.ToNative(sample.scalars, keysOnly: false);
                texts.ToNative(sample.texts, keysOnly: false);
                wideTexts.ToNative(sample.wideTexts, keysOnly: false);
                boundedTexts.ToNative(sample.boundedTexts, keysOnly: false);
                boundedWideTexts.ToNative(sample.boundedWideTexts, keysOnly: false);
            }
        }

        internal class SequenceAliasesSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedSequences.SequenceAliasesSample, SequenceAliasesSampleUnmanaged>
        {

            internal SequenceAliasesSamplePlugin() : base("global::CorpusUnboundedSequences.SequenceAliasesSample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // SequenceAliasesSample struct
                var SequenceAliasesSampleStructMembers = new StructMember[]
                {
                    new StructMember("numbers", global::CorpusUnboundedSequences.NumbersChainSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("states", global::CorpusUnboundedSequences.StatesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("children", global::CorpusUnboundedSequences.ChildrenSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                    new StructMember("scalars", global::CorpusUnboundedSequences.ScalarsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 3),
                    new StructMember("texts", global::CorpusUnboundedSequences.TextsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 4),
                    new StructMember("wideTexts", global::CorpusUnboundedSequences.WideTextsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 5),
                    new StructMember("boundedTexts", global::CorpusUnboundedSequences.BoundedTextsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 6),
                    new StructMember("boundedWideTexts", global::CorpusUnboundedSequences.BoundedWideTextsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 7)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SequenceAliasesSampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedSequences::SequenceAliasesSample")
                    .AddMembers(SequenceAliasesSampleStructMembers));

                return result;

            }
        }
    }
    public class SequenceAliasesSampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedSequences.SequenceAliasesSample>
    {
        public SequenceAliasesSampleSupport() : base(
            new Implementation.SequenceAliasesSamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SequenceAliasesSamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SequenceAliasesSampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SequenceAliasesSampleSupport, global::CorpusUnboundedSequences.SequenceAliasesSample>();

    }

} // namespace CorpusUnboundedSequences

