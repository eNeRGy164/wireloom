/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-constant-expressions.idl
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

namespace CorpusConstantExpressions
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusConstantExpressions.Sample>
        {

            private NativeSeq values;
            private NativeUnmanagedArray array;

            public void Destroy(bool optionalsOnly)
            {
                values.Destroy(optionalsOnly);
                array.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusConstantExpressions.Sample sample, bool keysOnly = false)
            {

                values.FromNative((Sequence<int>) sample.values);
                array.FromNative(sample.array, dimension: ((CorpusConstantExpressions.Add.Value)));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize<int >(max: ((int)(CorpusConstantExpressions.Bound.Value)), absoluteMax: ((int)(CorpusConstantExpressions.Bound.Value)), allocateMemory: allocateMemory);
                array.Initialize<int>(dimension: ((CorpusConstantExpressions.Add.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusConstantExpressions.Sample sample, bool keysOnly = false)
            {
                values.ToNative((Sequence<int>) sample.values);
                array.ToNative<int>(sample.array, dimension: ((CorpusConstantExpressions.Add.Value)));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusConstantExpressions.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusConstantExpressions.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)(CorpusConstantExpressions.Bound.Value))), id: 0),
                    new StructMember("array", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {(CorpusConstantExpressions.Add.Value)}), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusConstantExpressions::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusConstantExpressions.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusConstantExpressions.Sample>();

    }

} // namespace CorpusConstantExpressions

