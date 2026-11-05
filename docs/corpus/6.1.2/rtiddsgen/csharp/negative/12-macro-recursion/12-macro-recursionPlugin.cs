/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-macro-recursion.idl
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

namespace CorpusNegativeMacroRecursion
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusNegativeMacroRecursion.Sample>
        {

            private int CORPUS_A;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusNegativeMacroRecursion.Sample sample, bool keysOnly = false)
            {

                sample.CORPUS_A = CORPUS_A;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                CORPUS_A = (int) 0;
            }

            public void ToNative(CorpusNegativeMacroRecursion.Sample sample, bool keysOnly = false)
            {
                CORPUS_A = sample.CORPUS_A;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusNegativeMacroRecursion.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusNegativeMacroRecursion.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("CORPUS_A", dtf.GetPrimitiveType<int>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNegativeMacroRecursion::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusNegativeMacroRecursion.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusNegativeMacroRecursion.Sample>();
    }

} // namespace CorpusNegativeMacroRecursion

