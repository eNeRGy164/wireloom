/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-int8.idl
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

namespace CorpusUnionInt8
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnionInt8.Choice>
        {

            private byte _d;

            private int minimum;
            private int maximum;
            private int fallback;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusUnionInt8.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case -128:
                    sample.minimum = minimum;
                    break;
                    case 127:
                    sample.maximum = maximum;
                    break;
                    default:
                    sample.fallback = fallback;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                minimum = (int) 0;
                maximum = (int) 0;
                fallback = (int) 0;
            }

            public void ToNative(CorpusUnionInt8.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case -128:
                    minimum = sample.minimum;
                    break;
                    case 127:
                    maximum = sample.maximum;
                    break;
                    default:
                    fallback = sample.fallback;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnionInt8.Choice, ChoiceUnmanaged>
        {
            internal ChoicePlugin() : base("CorpusUnionInt8.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("minimum", dtf.GetPrimitiveType<int>(), new int[] {(int) -128}, id: 1),
                    new UnionMember("maximum", dtf.GetPrimitiveType<int>(), new int[] {(int) 127}, id: 2),
                    new UnionMember("fallback", dtf.GetPrimitiveType<int>(), new int[] {(int) UnionMember.DefaultLabel}, id: 3)
                };

                return tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<byte>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionInt8::Choice")
                    .AddMembers(ChoiceStructMembers));
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<CorpusUnionInt8.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, CorpusUnionInt8.Choice>();
    }

} // namespace CorpusUnionInt8

