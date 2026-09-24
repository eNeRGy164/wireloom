/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-array-of-sequences.idl
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

namespace CorpusArrayOfSequences
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusArrayOfSequences.Sample>
        {

            private NativeSeq values;
            private NativeStringSeq names;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                values.Destroy(optionalsOnly);
                names.Destroy();
            }

            public void FromNative(global::CorpusArrayOfSequences.Sample sample, bool keysOnly = false)
            {

                values.FromNative((Sequence<int>) sample.values);
                names.FromNative(sample.names);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize<int >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
                names.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusArrayOfSequences.Sample sample, bool keysOnly = false)
            {
                values.ToNative((Sequence<int>) sample.values);
                names.ToNative(sample.names, ((int) 16));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusArrayOfSequences.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusArrayOfSequences.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)100)), id: 0),
                    new StructMember("names", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 16)), ((int)3)), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusArrayOfSequences::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusArrayOfSequences.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusArrayOfSequences.Sample>();

    }

} // namespace CorpusArrayOfSequences

