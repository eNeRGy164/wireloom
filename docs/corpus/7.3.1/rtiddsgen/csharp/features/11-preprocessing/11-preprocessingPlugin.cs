/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-preprocessing.idl
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

namespace CorpusPreprocessing
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusPreprocessing.Sample>
        {

            private global::Implementation.IncludedTypeUnmanaged item;
            private int value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                item.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusPreprocessing.Sample sample, bool keysOnly = false)
            {

                item.FromNative(sample.item, keysOnly: false);
                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                item.Initialize(allocatePointers, allocateMemory);
                value = (int) (0);
            }

            public void ToNative(global::CorpusPreprocessing.Sample sample, bool keysOnly = false)
            {
                item.ToNative(sample.item, keysOnly: false);
                value = sample.value;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusPreprocessing.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusPreprocessing.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("item", global::IncludedTypeSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusPreprocessing::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusPreprocessing.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusPreprocessing.Sample>();

    }

} // namespace CorpusPreprocessing

