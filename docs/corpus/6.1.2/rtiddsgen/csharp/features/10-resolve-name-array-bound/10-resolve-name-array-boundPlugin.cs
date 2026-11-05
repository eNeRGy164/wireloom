/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-array-bound.idl
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

namespace CorpusResolveNameArrayBound
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusResolveNameArrayBound.Sample>
        {

            private NativeUnmanagedArray bytes;

            public void Destroy(bool optionalsOnly)
            {
                bytes.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusResolveNameArrayBound.Sample sample, bool keysOnly = false)
            {

                bytes.FromNative(sample.bytes, dimension: ((ARRAY_BOUND.Value)));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                bytes.Initialize<byte>(dimension: ((ARRAY_BOUND.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusResolveNameArrayBound.Sample sample, bool keysOnly = false)
            {
                bytes.ToNative<byte>(sample.bytes, dimension: ((ARRAY_BOUND.Value)));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusResolveNameArrayBound.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusResolveNameArrayBound.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("bytes", tsf.CreateArrayWithAccessInfo<byte>(dtf, dtf.GetPrimitiveType<byte>(), new uint[] {(ARRAY_BOUND.Value)}
                    ), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusResolveNameArrayBound::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusResolveNameArrayBound.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusResolveNameArrayBound.Sample>();
    }

} // namespace CorpusResolveNameArrayBound

