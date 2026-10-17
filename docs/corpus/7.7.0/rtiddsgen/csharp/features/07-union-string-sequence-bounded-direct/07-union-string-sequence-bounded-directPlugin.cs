/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-bounded-direct.idl
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

namespace RuntimeUnionBoundedStringSequences
{

    namespace Implementation
    {

        public struct DirectBoundedStringSequencesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences>
        {
            private int _d;

            private NativeStringSeq narrow;
            private NativeWstringSeq wide;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                narrow.Destroy();
                wide.Destroy();
            }

            public void FromNative(global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.narrow = new Rti.Types.Sequence<string>();
                    }
                    narrow.FromNative(sample.narrow);
                    break;
                    case 1:
                    if(sample.Discriminator != _d)
                    {
                        sample.wide = new Rti.Types.Sequence<string>();
                    }
                    wide.FromNative(sample.wide);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = DirectBoundedStringSequences.DefaultDiscriminator;
                narrow.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 8), allocateMemory: allocateMemory);
                wide.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 4), allocateMemory: allocateMemory);
            }

            public void ToNative(global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    narrow.ToNative(sample.narrow, ((int) 8));
                    break;
                    case 1:
                    wide.ToNative(sample.wide, ((int) 4));
                    break;
                }
            }
        }

        internal class DirectBoundedStringSequencesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences, DirectBoundedStringSequencesUnmanaged>
        {

            internal DirectBoundedStringSequencesPlugin() : base("global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // DirectBoundedStringSequences union
                var DirectBoundedStringSequencesStructMembers = new UnionMember[]
                {
                    new UnionMember("narrow", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 8)), ((int)3)), new int[] {(int) 0}, id: 1),
                    new UnionMember("wide", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 4)), ((int)3)), new int[] {(int) 1}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<DirectBoundedStringSequencesUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("RuntimeUnionBoundedStringSequences::DirectBoundedStringSequences")
                    .AddMembers(DirectBoundedStringSequencesStructMembers));

                return result;

            }
        }
    }
    public class DirectBoundedStringSequencesSupport : Rti.Dds.Topics.TypeSupport<global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences>
    {
        public DirectBoundedStringSequencesSupport() : base(
            new Implementation.DirectBoundedStringSequencesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DirectBoundedStringSequencesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DirectBoundedStringSequencesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DirectBoundedStringSequencesSupport, global::RuntimeUnionBoundedStringSequences.DirectBoundedStringSequences>();

    }

} // namespace RuntimeUnionBoundedStringSequences

