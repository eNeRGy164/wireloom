/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
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

namespace CorpusOptionalAggregate
{

    namespace Implementation
    {

        public struct PayloadUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusOptionalAggregate.Payload>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusOptionalAggregate.Payload sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) 0;
            }

            public void ToNative(CorpusOptionalAggregate.Payload sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class PayloadPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusOptionalAggregate.Payload, PayloadUnmanaged>
        {
            internal PayloadPlugin() : base("CorpusOptionalAggregate.Payload", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Payload struct
                var PayloadStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<PayloadUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalAggregate::Payload")
                    .AddMembers(PayloadStructMembers));
            }
        }
    }
    public class PayloadSupport : Rti.Dds.Topics.TypeSupport<CorpusOptionalAggregate.Payload>
    {
        public PayloadSupport() : base(
            new Implementation.PayloadPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PayloadPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PayloadSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PayloadSupport, CorpusOptionalAggregate.Payload>();
    }

    namespace Implementation
    {

        public struct PayloadAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusOptionalAggregate.PayloadAlias>
        {

            private CorpusOptionalAggregate.Implementation.PayloadUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusOptionalAggregate.PayloadAlias sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusOptionalAggregate.PayloadAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class PayloadAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusOptionalAggregate.PayloadAlias, PayloadAliasUnmanaged>
        {
            internal PayloadAliasPlugin() : base("CorpusOptionalAggregate.PayloadAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<PayloadAliasUnmanaged>(
                    dtf,
                    "PayloadAlias",
                    CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class PayloadAliasSupport : Rti.Dds.Topics.TypeSupport<CorpusOptionalAggregate.PayloadAlias>
    {
        public PayloadAliasSupport() : base(
            new Implementation.PayloadAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PayloadAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PayloadAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PayloadAliasSupport, CorpusOptionalAggregate.PayloadAlias>();
    }

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusOptionalAggregate.Choice>
        {

            private int _d;

            private CorpusOptionalAggregate.Implementation.PayloadUnmanaged payload;
            private int number;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                payload.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusOptionalAggregate.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.payload = new CorpusOptionalAggregate.Payload();
                    }
                    payload.FromNative(sample.payload, keysOnly: false);
                    break;
                    case 1:
                    sample.number = number;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                payload.Initialize(allocatePointers, allocateMemory);
                number = (int) 0;
            }

            public void ToNative(CorpusOptionalAggregate.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    payload.ToNative(sample.payload, keysOnly: false);
                    break;
                    case 1:
                    number = sample.number;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusOptionalAggregate.Choice, ChoiceUnmanaged>
        {
            internal ChoicePlugin() : base("CorpusOptionalAggregate.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("payload", CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 0}, id: 1),
                    new UnionMember("number", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 2)
                };

                return tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalAggregate::Choice")
                    .AddMembers(ChoiceStructMembers));
            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<CorpusOptionalAggregate.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, CorpusOptionalAggregate.Choice>();
    }

    namespace Implementation
    {

        public struct ChoiceAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusOptionalAggregate.ChoiceAlias>
        {

            private CorpusOptionalAggregate.Implementation.ChoiceUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusOptionalAggregate.ChoiceAlias sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusOptionalAggregate.ChoiceAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class ChoiceAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusOptionalAggregate.ChoiceAlias, ChoiceAliasUnmanaged>
        {
            internal ChoiceAliasPlugin() : base("CorpusOptionalAggregate.ChoiceAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ChoiceAliasUnmanaged>(
                    dtf,
                    "ChoiceAlias",
                    CorpusOptionalAggregate.ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class ChoiceAliasSupport : Rti.Dds.Topics.TypeSupport<CorpusOptionalAggregate.ChoiceAlias>
    {
        public ChoiceAliasSupport() : base(
            new Implementation.ChoiceAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoiceAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceAliasSupport, CorpusOptionalAggregate.ChoiceAlias>();
    }

    namespace Implementation
    {

        public struct HolderUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusOptionalAggregate.Holder>
        {

            private NativeManagedOptional payload;
            private NativeManagedOptional payloadAlias;
            private NativeManagedOptional choice;
            private NativeManagedOptional choiceAlias;

            public void Destroy(bool optionalsOnly)
            {
                payload.Destroy<CorpusOptionalAggregate.Payload, CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(optionalsOnly);
                payloadAlias.Destroy<CorpusOptionalAggregate.Payload, CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(optionalsOnly);
                choice.Destroy<CorpusOptionalAggregate.Choice, CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(optionalsOnly);
                choiceAlias.Destroy<CorpusOptionalAggregate.Choice, CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(optionalsOnly);
            }

            public void FromNative(CorpusOptionalAggregate.Holder sample, bool keysOnly = false)
            {

                payload.FromNative<CorpusOptionalAggregate.Payload, CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(out var payloadTemporary_);
                sample.payload = payloadTemporary_;
                payloadAlias.FromNative<CorpusOptionalAggregate.Payload, CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(out var payloadAliasTemporary_);
                sample.payloadAlias = payloadAliasTemporary_;
                choice.FromNative<CorpusOptionalAggregate.Choice, CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(out var choiceTemporary_);
                sample.choice = choiceTemporary_;
                choiceAlias.FromNative<CorpusOptionalAggregate.Choice, CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(out var choiceAliasTemporary_);
                sample.choiceAlias = choiceAliasTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
            }

            public void ToNative(CorpusOptionalAggregate.Holder sample, bool keysOnly = false)
            {
                payload.ToNative<CorpusOptionalAggregate.Payload, CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(sample.payload);
                payloadAlias.ToNative<CorpusOptionalAggregate.Payload, CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(sample.payloadAlias);
                choice.ToNative<CorpusOptionalAggregate.Choice, CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(sample.choice);
                choiceAlias.ToNative<CorpusOptionalAggregate.Choice, CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(sample.choiceAlias);
            }
        }

        internal class HolderPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusOptionalAggregate.Holder, HolderUnmanaged>
        {
            internal HolderPlugin() : base("CorpusOptionalAggregate.Holder", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Holder struct
                var HolderStructMembers = new StructMember[]
                {
                    new StructMember("payload", CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 0),
                    new StructMember("payloadAlias", CorpusOptionalAggregate.PayloadAliasSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 1),
                    new StructMember("choice", CorpusOptionalAggregate.ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 2),
                    new StructMember("choiceAlias", CorpusOptionalAggregate.ChoiceAliasSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 3)
                };

                return tsf.CreateTypeWithAccessInfo<HolderUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalAggregate::Holder")
                    .AddMembers(HolderStructMembers));
            }
        }
    }
    public class HolderSupport : Rti.Dds.Topics.TypeSupport<CorpusOptionalAggregate.Holder>
    {
        public HolderSupport() : base(
            new Implementation.HolderPlugin(),
            new Lazy<DynamicType>(() =>Implementation.HolderPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static HolderSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<HolderSupport, CorpusOptionalAggregate.Holder>();
    }

} // namespace CorpusOptionalAggregate

