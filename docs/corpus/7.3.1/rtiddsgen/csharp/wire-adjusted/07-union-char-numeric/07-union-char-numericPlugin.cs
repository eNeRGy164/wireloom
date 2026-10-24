/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char-numeric.idl
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

namespace CorpusNumericCharUnion
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNumericCharUnion.Choice>
        {

            private byte _d;

            private int value;
            private int fallback;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNumericCharUnion.Choice sample, bool keysOnly = false)
            {
                switch (NativeChar.FromUtf8(_d))
                {
                    case (char)127:
                    sample.value = value;
                    break;
                    default:
                    sample.Setfallback(fallback, NativeChar.FromUtf8(_d));
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = NativeChar.ToUtf8(Choice.DefaultDiscriminator);
                value = (int) (0);
                fallback = (int) (0);
            }

            public void ToNative(global::CorpusNumericCharUnion.Choice sample, bool keysOnly = false)
            {
                _d = NativeChar.ToUtf8(sample.Discriminator);
                switch (NativeChar.FromUtf8(_d))
                {
                    case (char)127:
                    value = sample.value;
                    break;
                    default:
                    fallback = sample.fallback;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNumericCharUnion.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusNumericCharUnion.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("value", dtf.GetPrimitiveType<int>(), new int[] {(int) 127}, id: 1),
                    new UnionMember("fallback", dtf.GetPrimitiveType<int>(), new int[] {(int) UnionMember.DefaultLabel}, id: 2)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<char>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNumericCharUnion::Choice")
                    .AddMembers(ChoiceStructMembers));

                return result;
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNumericCharUnion.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusNumericCharUnion.Choice>();

    }

} // namespace CorpusNumericCharUnion

