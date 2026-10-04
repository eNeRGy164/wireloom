/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-string-sequences.idl
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

namespace CorpusOptionalStringSequences
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalStringSequences.Sample>
        {

            private NativeOptionalStringSeq narrowValues;
            private NativeOptionalWstringSeq wideValues;

            public void Destroy(bool optionalsOnly)
            {
                narrowValues.Destroy();
                wideValues.Destroy();
            }

            public void FromNative(global::CorpusOptionalStringSequences.Sample sample, bool keysOnly = false)
            {

                narrowValues.FromNative(out ISequence<string> narrowValuesTemporary_);
                sample.narrowValues = narrowValuesTemporary_;
                wideValues.FromNative(out ISequence<string> wideValuesTemporary_);
                sample.wideValues = wideValuesTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                narrowValues.Initialize ();
                wideValues.Initialize ();
            }

            public void ToNative(global::CorpusOptionalStringSequences.Sample sample, bool keysOnly = false)
            {
                narrowValues.ToNative(sample.narrowValues, ((int)4), ((int) 16));
                wideValues.ToNative(sample.wideValues, ((int)4), ((int) 16));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalStringSequences.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusOptionalStringSequences.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("narrowValues", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 16)), ((int)4)), isOptional: true, id: 1),
                    new StructMember("wideValues", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 16)), ((int)4)), isOptional: true, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalStringSequences::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalStringSequences.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusOptionalStringSequences.Sample>();

    }

} // namespace CorpusOptionalStringSequences
