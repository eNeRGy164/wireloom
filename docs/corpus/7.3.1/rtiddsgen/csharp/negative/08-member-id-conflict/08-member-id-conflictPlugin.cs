/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-member-id-conflict.idl
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

namespace CorpusNegativeMemberIds
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNegativeMemberIds.Sample>
        {

            private int first;
            private int second;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNegativeMemberIds.Sample sample, bool keysOnly = false)
            {

                sample.first = first;
                sample.second = second;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                first = (int) (0);
                second = (int) (0);
            }

            public void ToNative(global::CorpusNegativeMemberIds.Sample sample, bool keysOnly = false)
            {
                first = sample.first;
                second = sample.second;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNegativeMemberIds.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusNegativeMemberIds.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("first", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("second", dtf.GetPrimitiveType<int>(), id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Mutable)
                    .WithName("CorpusNegativeMemberIds::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNegativeMemberIds.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusNegativeMemberIds.Sample>();

    }

} // namespace CorpusNegativeMemberIds

