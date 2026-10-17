/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-unions.idl
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

namespace CorpusUnions
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnions.Choice>
        {

            private int _d;

            private int number;
            private NativeString text;
            private byte flag;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(global::CorpusUnions.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    sample.number = number;
                    break;
                    case 1:
                    sample.text = text.FromNative();
                    break;
                    default:
                    sample.flag = Convert.ToBoolean(flag);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                number = (int) (0);
                text.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                flag = 0;
            }

            public void ToNative(global::CorpusUnions.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    number = sample.number;
                    break;
                    case 1:
                    text.ToNative(sample.text, ((int) 255));
                    break;
                    default:
                    flag = Convert.ToByte(sample.flag);
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnions.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusUnions.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new UnionMember("number", dtf.GetPrimitiveType<int>(), new int[] {(int) 0}, id: 1),
                    new UnionMember("text", dtf.CreateString(((int) 255)), new int[] {(int) 1}, id: 2),
                    new UnionMember("flag", dtf.GetPrimitiveType<bool>(), new int[] {(int) UnionMember.DefaultLabel}, id: 3)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnions::Choice")
                    .AddMembers(ChoiceStructMembers));

                return result;
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnions.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusUnions.Choice>();

    }

} // namespace CorpusUnions

