/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-case-collision-default.idl
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

namespace CorpusStrictCase
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStrictCase.Sample>
        {

            private int number;
            private int Number;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusStrictCase.Sample sample, bool keysOnly = false)
            {

                sample.number = number;
                sample.Number = Number;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                number = (int) (0);
                Number = (int) (0);
            }

            public void ToNative(global::CorpusStrictCase.Sample sample, bool keysOnly = false)
            {
                number = sample.number;
                Number = sample.Number;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStrictCase.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusStrictCase.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("number", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("Number", dtf.GetPrimitiveType<int>(), id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusStrictCase::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStrictCase.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusStrictCase.Sample>();

    }

} // namespace CorpusStrictCase

