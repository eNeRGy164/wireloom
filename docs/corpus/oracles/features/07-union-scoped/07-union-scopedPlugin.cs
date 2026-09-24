/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-scoped.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.
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

namespace CorpusScopedUnion
{

    namespace Implementation
    {

        public struct PayloadUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusScopedUnion.Payload>
        {

            private int code;
            private NativeString label;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                label.Destroy();
            }

            public void FromNative(global::CorpusScopedUnion.Payload sample, bool keysOnly = false)
            {

                sample.code = code;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                code = (int) (0);
                label.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusScopedUnion.Payload sample, bool keysOnly = false)
            {
                code = sample.code;
                label.ToNative(sample.label, ((int) 16));
            }
        }

        internal class PayloadPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusScopedUnion.Payload, PayloadUnmanaged>
        {

            internal PayloadPlugin() : base("global::CorpusScopedUnion.Payload", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Payload struct
                var PayloadStructMembers = new StructMember[]
                {
                    new StructMember("code", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("label", dtf.CreateString(((int) 16)), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<PayloadUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusScopedUnion::Payload")
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
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        1,
                        annotations);
                }

                return result;

            }
        }
    }
    public class PayloadSupport : Rti.Dds.Topics.TypeSupport<global::CorpusScopedUnion.Payload>
    {
        public PayloadSupport() : base(
            new Implementation.PayloadPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PayloadPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PayloadSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PayloadSupport, global::CorpusScopedUnion.Payload>();

    }

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusScopedUnion.Choice>
        {
            private int _d;

            private global::CorpusScopedUnion.Implementation.PayloadUnmanaged qualifiedPayload;
            private NativeSeq values;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                qualifiedPayload.Destroy(optionalsOnly);
                values.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusScopedUnion.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 10:
                    if(sample.Discriminator != _d)
                    {
                        sample.qualifiedPayload = new global::CorpusScopedUnion.Payload();
                    }
                    qualifiedPayload.FromNative(sample.qualifiedPayload, keysOnly: false);
                    break;
                    case 11:
                    if(sample.Discriminator != _d)
                    {
                        sample.values = new Rti.Types.Sequence<int>();
                    }
                    values.FromNative((Sequence<int>) sample.values);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                qualifiedPayload.Initialize(allocatePointers, allocateMemory);
                values.Initialize<int >(max: ((int)4), absoluteMax: ((int)4), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusScopedUnion.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 10:
                    qualifiedPayload.ToNative(sample.qualifiedPayload, keysOnly: false);
                    break;
                    case 11:
                    values.ToNative((Sequence<int>) sample.values);
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusScopedUnion.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusScopedUnion.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("qualifiedPayload", global::CorpusScopedUnion.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 10}, id: 1),
                    new UnionMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)4)), new int[] {(int) 11}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusScopedUnion::Choice")
                    .AddMembers(ChoiceStructMembers));

                return result;

            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusScopedUnion.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusScopedUnion.Choice>();

    }

} // namespace CorpusScopedUnion

