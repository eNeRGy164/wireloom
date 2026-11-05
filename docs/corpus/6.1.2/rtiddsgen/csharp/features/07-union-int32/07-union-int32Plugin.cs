/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-int32.idl
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

namespace CorpusUnionInt32
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnionInt32.Choice>
        {

            private int _d;

            private int minimum;
            private int maximum;
            private int fallback;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusUnionInt32.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case -2147483648:
                    sample.minimum = minimum;
                    break;
                    case 2147483647:
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

            public void ToNative(CorpusUnionInt32.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case -2147483648:
                    minimum = sample.minimum;
                    break;
                    case 2147483647:
                    maximum = sample.maximum;
                    break;
                    default:
                    fallback = sample.fallback;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnionInt32.Choice, ChoiceUnmanaged>
        {
            internal ChoicePlugin() : base("CorpusUnionInt32.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("minimum", dtf.GetPrimitiveType<int>(), new int[] {(int) -2147483648}, id: 1),
                    new UnionMember("maximum", dtf.GetPrimitiveType<int>(), new int[] {(int) 2147483647}, id: 2),
                    new UnionMember("fallback", dtf.GetPrimitiveType<int>(), new int[] {(int) UnionMember.DefaultLabel}, id: 3)
                };

                return tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionInt32::Choice")
                    .AddMembers(ChoiceStructMembers));
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<CorpusUnionInt32.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, CorpusUnionInt32.Choice>();
    }

} // namespace CorpusUnionInt32

