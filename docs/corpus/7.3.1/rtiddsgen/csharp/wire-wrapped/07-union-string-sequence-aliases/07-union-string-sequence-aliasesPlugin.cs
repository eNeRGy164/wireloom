/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-aliases.idl
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

namespace RuntimeUnionStringSequences
{

    namespace Implementation
    {

        public struct StringSequenceUnionsSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::RuntimeUnionStringSequences.StringSequenceUnionsSample>
        {

            private global::RuntimeUnionStringSequences.Implementation.DirectStringSequencesUnmanaged direct;
            private global::RuntimeUnionStringSequences.Implementation.StringSequenceAliasesUnmanaged aliases;

            public void Destroy(bool optionalsOnly)
            {
                direct.Destroy(optionalsOnly);
                aliases.Destroy(optionalsOnly);
            }

            public void FromNative(global::RuntimeUnionStringSequences.StringSequenceUnionsSample sample, bool keysOnly = false)
            {

                direct.FromNative(sample.direct, keysOnly: false);
                aliases.FromNative(sample.aliases, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                direct.Initialize(allocatePointers, allocateMemory);
                aliases.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::RuntimeUnionStringSequences.StringSequenceUnionsSample sample, bool keysOnly = false)
            {
                direct.ToNative(sample.direct, keysOnly: false);
                aliases.ToNative(sample.aliases, keysOnly: false);
            }
        }

        internal class StringSequenceUnionsSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::RuntimeUnionStringSequences.StringSequenceUnionsSample, StringSequenceUnionsSampleUnmanaged>
        {

            internal StringSequenceUnionsSamplePlugin() : base("global::RuntimeUnionStringSequences.StringSequenceUnionsSample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // StringSequenceUnionsSample struct
                var StringSequenceUnionsSampleStructMembers = new StructMember[]
                {
                    new StructMember("direct", global::RuntimeUnionStringSequences.DirectStringSequencesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("aliases", global::RuntimeUnionStringSequences.StringSequenceAliasesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<StringSequenceUnionsSampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("RuntimeUnionStringSequences::StringSequenceUnionsSample")
                    .AddMembers(StringSequenceUnionsSampleStructMembers));

                return result;
            }
        }
    }
    public class StringSequenceUnionsSampleSupport : Rti.Dds.Topics.TypeSupport<global::RuntimeUnionStringSequences.StringSequenceUnionsSample>
    {
        public StringSequenceUnionsSampleSupport() : base(
            new Implementation.StringSequenceUnionsSamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StringSequenceUnionsSamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StringSequenceUnionsSampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StringSequenceUnionsSampleSupport, global::RuntimeUnionStringSequences.StringSequenceUnionsSample>();

    }

} // namespace RuntimeUnionStringSequences

