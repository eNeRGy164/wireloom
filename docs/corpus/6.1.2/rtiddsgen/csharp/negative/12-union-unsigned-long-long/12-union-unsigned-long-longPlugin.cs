/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-union-unsigned-long-long.idl
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

namespace CorpusNegativeUnionunsignedlonglong
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusNegativeUnionunsignedlonglong.Choice>
        {

            private ulong _d;

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusNegativeUnionunsignedlonglong.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 1:
                    sample.value = value;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                value = (int) 0;
            }

            public void ToNative(CorpusNegativeUnionunsignedlonglong.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 1:
                    value = sample.value;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusNegativeUnionunsignedlonglong.Choice, ChoiceUnmanaged>
        {
            internal ChoicePlugin() : base("CorpusNegativeUnionunsignedlonglong.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("value", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<ulong>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNegativeUnionunsignedlonglong::Choice")
                    .AddMembers(ChoiceStructMembers));
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<CorpusNegativeUnionunsignedlonglong.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, CorpusNegativeUnionunsignedlonglong.Choice>();
    }

} // namespace CorpusNegativeUnionunsignedlonglong

