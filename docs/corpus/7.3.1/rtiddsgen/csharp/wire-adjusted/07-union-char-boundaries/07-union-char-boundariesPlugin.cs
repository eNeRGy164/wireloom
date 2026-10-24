/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char-boundaries.idl
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

namespace CorpusCharUnionBoundaries
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCharUnionBoundaries.Choice>
        {

            private byte _d;

            private int minimum;
            private int maximum;
            private int fallback;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusCharUnionBoundaries.Choice sample, bool keysOnly = false)
            {
                switch (NativeChar.FromUtf8(_d))
                {
                    case '\0':
                    sample.minimum = minimum;
                    break;
                    case '~':
                    sample.maximum = maximum;
                    break;
                    default:
                    sample.fallback = fallback;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = NativeChar.ToUtf8(Choice.DefaultDiscriminator);
                minimum = (int) (0);
                maximum = (int) (0);
                fallback = (int) (0);
            }

            public void ToNative(global::CorpusCharUnionBoundaries.Choice sample, bool keysOnly = false)
            {
                _d = NativeChar.ToUtf8(sample.Discriminator);
                switch (NativeChar.FromUtf8(_d))
                {
                    case '\0':
                    minimum = sample.minimum;
                    break;
                    case '~':
                    maximum = sample.maximum;
                    break;
                    default:
                    fallback = sample.fallback;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCharUnionBoundaries.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusCharUnionBoundaries.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new UnionMember("minimum", dtf.GetPrimitiveType<int>(), new int[] {(int) '\0'}, id: 1),
                    new UnionMember("maximum", dtf.GetPrimitiveType<int>(), new int[] {(int) '~'}, id: 2),
                    new UnionMember("fallback", dtf.GetPrimitiveType<int>(), new int[] {(int) UnionMember.DefaultLabel}, id: 3)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<char>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCharUnionBoundaries::Choice")
                    .AddMembers(ChoiceStructMembers));

                return result;
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCharUnionBoundaries.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusCharUnionBoundaries.Choice>();

    }

} // namespace CorpusCharUnionBoundaries

