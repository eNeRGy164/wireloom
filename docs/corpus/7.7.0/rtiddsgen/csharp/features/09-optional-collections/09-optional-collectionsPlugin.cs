/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-collections.idl
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

namespace CorpusOptionalCollections
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalCollections.Sample>
        {

            private NativeOptionalSeq values;
            private NativeUnmanagedOptionalArray items;

            public void Destroy(bool optionalsOnly)
            {
                values.Destroy(optionalsOnly);
                items.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalCollections.Sample sample, bool keysOnly = false)
            {

                values.FromNative(out Sequence<int> valuesTemporary_);
                sample.values = valuesTemporary_;
                items.FromNative<int>(out int[] itemsTemporary_, new int[] {2});
                sample.items = itemsTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize ();
            }

            public void ToNative(global::CorpusOptionalCollections.Sample sample, bool keysOnly = false)
            {
                values.ToNative((Sequence<int>) sample.values, ((int)4));
                items.ToNative<int>(sample.items, (2));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalCollections.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusOptionalCollections.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)4)), isOptional: true, id: 1),
                    new StructMember("items", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2}), isOptional: true, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalCollections::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalCollections.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusOptionalCollections.Sample>();

    }

} // namespace CorpusOptionalCollections

