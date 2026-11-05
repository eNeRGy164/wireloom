/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-union-64bit-label.idl
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

namespace CorpusNegativeWideUnionLabel
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusNegativeWideUnionLabel.Choice>
        {

            private long _d;

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusNegativeWideUnionLabel.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 4294967296:
                    sample.value = value;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                value = (int) 0;
            }

            public void ToNative(CorpusNegativeWideUnionLabel.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 4294967296:
                    value = sample.value;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusNegativeWideUnionLabel.Choice, ChoiceUnmanaged>
        {
            internal ChoicePlugin() : base("CorpusNegativeWideUnionLabel.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("value", dtf.GetPrimitiveType<int>(), new int[] {(int) 4294967296}, id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<long>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNegativeWideUnionLabel::Choice")
                    .AddMembers(ChoiceStructMembers));
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<CorpusNegativeWideUnionLabel.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, CorpusNegativeWideUnionLabel.Choice>();
    }

} // namespace CorpusNegativeWideUnionLabel

