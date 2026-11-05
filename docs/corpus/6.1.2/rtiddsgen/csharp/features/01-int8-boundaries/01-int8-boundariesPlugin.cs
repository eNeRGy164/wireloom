/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-int8-boundaries.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

namespace CorpusInt8Boundaries
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInt8Boundaries.Sample>
        {

            private byte minimum;
            private byte maximum;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusInt8Boundaries.Sample sample, bool keysOnly = false)
            {

                sample.minimum = minimum;
                sample.maximum = maximum;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                minimum = (byte) 0;
                maximum = (byte) 0;
            }

            public void ToNative(CorpusInt8Boundaries.Sample sample, bool keysOnly = false)
            {
                minimum = sample.minimum;
                maximum = sample.maximum;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInt8Boundaries.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusInt8Boundaries.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("minimum", dtf.GetPrimitiveType<byte>(), id: 0),
                    new StructMember("maximum", dtf.GetPrimitiveType<byte>(), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInt8Boundaries::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusInt8Boundaries.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusInt8Boundaries.Sample>();
    }

} // namespace CorpusInt8Boundaries

