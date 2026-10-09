/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-aggregate-member.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.1.
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

        public struct PayloadUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalAggregate.Payload>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusOptionalAggregate.Payload sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusOptionalAggregate.Payload sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class PayloadPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalAggregate.Payload, PayloadUnmanaged>
        {

            internal PayloadPlugin() : base("global::CorpusOptionalAggregate.Payload", isKeyed: false, CreateDynamicType(isPublic: false))
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

                DynamicType result = tsf.CreateTypeWithAccessInfo<PayloadUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalAggregate::Payload")
                    .AddMembers(PayloadStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }

                return result;

            }
        }
    }
    public class PayloadSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalAggregate.Payload>
    {
        public PayloadSupport() : base(
            new Implementation.PayloadPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PayloadPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PayloadSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PayloadSupport, global::CorpusOptionalAggregate.Payload>();

    }

    namespace Implementation
    {

        public struct PayloadAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalAggregate.PayloadAlias>
        {

            private global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalAggregate.PayloadAlias sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusOptionalAggregate.PayloadAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class PayloadAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalAggregate.PayloadAlias, PayloadAliasUnmanaged>
        {

            internal PayloadAliasPlugin() : base("global::CorpusOptionalAggregate.PayloadAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<PayloadAliasUnmanaged>(
                    dtf,
                    "PayloadAlias",
                    global::CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic));
                return aliasType;

            }
        }
    }
    public class PayloadAliasSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalAggregate.PayloadAlias>
    {
        public PayloadAliasSupport() : base(
            new Implementation.PayloadAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PayloadAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PayloadAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PayloadAliasSupport, global::CorpusOptionalAggregate.PayloadAlias>();

    }

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalAggregate.Choice>
        {
            private int _d;

            private global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged payload;
            private int number;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                payload.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalAggregate.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.payload = new global::CorpusOptionalAggregate.Payload();
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
                number = (int) (0);
            }

            public void ToNative(global::CorpusOptionalAggregate.Choice sample, bool keysOnly = false)
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

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalAggregate.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusOptionalAggregate.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("payload", global::CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 0}, id: 1),
                    new UnionMember("number", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalAggregate::Choice")
                    .AddMembers(ChoiceStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        1,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalAggregate.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusOptionalAggregate.Choice>();

    }

    namespace Implementation
    {

        public struct ChoiceAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalAggregate.ChoiceAlias>
        {

            private global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalAggregate.ChoiceAlias sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusOptionalAggregate.ChoiceAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class ChoiceAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalAggregate.ChoiceAlias, ChoiceAliasUnmanaged>
        {

            internal ChoiceAliasPlugin() : base("global::CorpusOptionalAggregate.ChoiceAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ChoiceAliasUnmanaged>(
                    dtf,
                    "ChoiceAlias",
                    global::CorpusOptionalAggregate.ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic));
                return aliasType;

            }
        }
    }
    public class ChoiceAliasSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalAggregate.ChoiceAlias>
    {
        public ChoiceAliasSupport() : base(
            new Implementation.ChoiceAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoiceAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceAliasSupport, global::CorpusOptionalAggregate.ChoiceAlias>();

    }

    namespace Implementation
    {

        public struct HolderUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalAggregate.Holder>
        {

            private NativeManagedOptional payload;
            private NativeManagedOptional payloadAlias;
            private NativeManagedOptional choice;
            private NativeManagedOptional choiceAlias;

            public void Destroy(bool optionalsOnly)
            {
                payload.Destroy<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(optionalsOnly);
                payloadAlias.Destroy<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(optionalsOnly);
                choice.Destroy<global::CorpusOptionalAggregate.Choice, global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(optionalsOnly);
                choiceAlias.Destroy<global::CorpusOptionalAggregate.Choice, global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalAggregate.Holder sample, bool keysOnly = false)
            {

                payload.FromNative<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(out var payloadTemporary_);
                sample.payload = payloadTemporary_;
                payloadAlias.FromNative<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(out var payloadAliasTemporary_);
                sample.payloadAlias = payloadAliasTemporary_;
                choice.FromNative<global::CorpusOptionalAggregate.Choice, global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(out var choiceTemporary_);
                sample.choice = choiceTemporary_;
                choiceAlias.FromNative<global::CorpusOptionalAggregate.Choice, global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(out var choiceAliasTemporary_);
                sample.choiceAlias = choiceAliasTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
            }

            public void ToNative(global::CorpusOptionalAggregate.Holder sample, bool keysOnly = false)
            {
                payload.ToNative<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(sample.payload);
                payloadAlias.ToNative<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(sample.payloadAlias);
                choice.ToNative<global::CorpusOptionalAggregate.Choice, global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(sample.choice);
                choiceAlias.ToNative<global::CorpusOptionalAggregate.Choice, global::CorpusOptionalAggregate.Implementation.ChoiceUnmanaged>(sample.choiceAlias);
            }
        }

        internal class HolderPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalAggregate.Holder, HolderUnmanaged>
        {

            internal HolderPlugin() : base("global::CorpusOptionalAggregate.Holder", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Holder struct
                var HolderStructMembers = new StructMember[]
                {
                    new StructMember("payload", global::CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 0),
                    new StructMember("payloadAlias", global::CorpusOptionalAggregate.PayloadAliasSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 1),
                    new StructMember("choice", global::CorpusOptionalAggregate.ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 2),
                    new StructMember("choiceAlias", global::CorpusOptionalAggregate.ChoiceAliasSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<HolderUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalAggregate::Holder")
                    .AddMembers(HolderStructMembers));

                return result;

            }
        }
    }
    public class HolderSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalAggregate.Holder>
    {
        public HolderSupport() : base(
            new Implementation.HolderPlugin(),
            new Lazy<DynamicType>(() =>Implementation.HolderPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static HolderSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<HolderSupport, global::CorpusOptionalAggregate.Holder>();

    }

} // namespace CorpusOptionalAggregate
