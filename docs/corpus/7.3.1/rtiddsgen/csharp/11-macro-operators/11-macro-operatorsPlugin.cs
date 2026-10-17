/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-macro-operators.idl
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

namespace CorpusMacroOperators
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusMacroOperators.Sample>
        {

            private int field;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusMacroOperators.Sample sample, bool keysOnly = false)
            {

                sample.field = field;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                field = (int) (0);
            }

            public void ToNative(global::CorpusMacroOperators.Sample sample, bool keysOnly = false)
            {
                field = sample.field;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusMacroOperators.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusMacroOperators.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("field", dtf.GetPrimitiveType<int>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusMacroOperators::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusMacroOperators.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusMacroOperators.Sample>();

    }

} // namespace CorpusMacroOperators

