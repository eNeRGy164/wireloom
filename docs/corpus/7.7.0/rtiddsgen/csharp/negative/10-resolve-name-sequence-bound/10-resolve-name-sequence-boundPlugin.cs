/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-sequence-bound.idl
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

namespace CorpusResolveNameSequenceBound
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusResolveNameSequenceBound.Sample>
        {

            private NativeSeq values;

            public void Destroy(bool optionalsOnly)
            {
                values.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusResolveNameSequenceBound.Sample sample, bool keysOnly = false)
            {

                values.FromNative((Sequence<int>) sample.values);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize<int >(max: ((int)(SEQUENCE_BOUND.Value)), absoluteMax: ((int)(SEQUENCE_BOUND.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusResolveNameSequenceBound.Sample sample, bool keysOnly = false)
            {
                values.ToNative((Sequence<int>) sample.values);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusResolveNameSequenceBound.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusResolveNameSequenceBound.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)(SEQUENCE_BOUND.Value))), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusResolveNameSequenceBound::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusResolveNameSequenceBound.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusResolveNameSequenceBound.Sample>();

    }

} // namespace CorpusResolveNameSequenceBound

