/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-duplicate-label.idl
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

namespace CorpusNegativeDuplicateLabel
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNegativeDuplicateLabel.Choice>
        {

            private int _d;

            private int first;
            private int second;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNegativeDuplicateLabel.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 1:
                    sample.first = first;
                    break;
                    case 1:
                    sample.second = second;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                first = (int) (0);
                second = (int) (0);
            }

            public void ToNative(global::CorpusNegativeDuplicateLabel.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 1:
                    first = sample.first;
                    break;
                    case 1:
                    second = sample.second;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNegativeDuplicateLabel.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusNegativeDuplicateLabel.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new UnionMember("first", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 1),
                    new UnionMember("second", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 2)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNegativeDuplicateLabel::Choice")
                    .AddMembers(ChoiceStructMembers));

                return result;
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNegativeDuplicateLabel.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusNegativeDuplicateLabel.Choice>();

    }

} // namespace CorpusNegativeDuplicateLabel

