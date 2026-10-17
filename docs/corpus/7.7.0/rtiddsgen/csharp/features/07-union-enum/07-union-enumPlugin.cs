/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-enum.idl
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

namespace CorpusEnumUnion
{

    namespace Implementation
    {
        internal class KindPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public KindPlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var enumType = dtf.BuildEnum()
                .WithName("CorpusEnumUnion::Kind")
                .AddMember(new EnumMember("Number", 0))
                .AddMember(new EnumMember("Text", 1))
                .AddMember(new EnumMember("PayloadValue", 2))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 0;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    enumType.SetAnnotations(
                        annotations);
                }
                return enumType;
            }
        }
    }

    public class KindSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumUnion.Kind>
    {
        public KindSupport() : base(
            new Implementation.KindPlugin(),
            new Lazy<DynamicType>(() =>Implementation.KindPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static KindSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<KindSupport, global::CorpusEnumUnion.Kind>();

    }

    namespace Implementation
    {

        public struct PayloadUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnumUnion.Payload>
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

            public void FromNative(global::CorpusEnumUnion.Payload sample, bool keysOnly = false)
            {

                sample.code = code;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                code = (int) (0);
                label.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusEnumUnion.Payload sample, bool keysOnly = false)
            {
                code = sample.code;
                label.ToNative(sample.label, ((int) 16));
            }
        }

        internal class PayloadPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnumUnion.Payload, PayloadUnmanaged>
        {

            internal PayloadPlugin() : base("global::CorpusEnumUnion.Payload", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    .WithName("CorpusEnumUnion::Payload")
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
    public class PayloadSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumUnion.Payload>
    {
        public PayloadSupport() : base(
            new Implementation.PayloadPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PayloadPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PayloadSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PayloadSupport, global::CorpusEnumUnion.Payload>();

    }

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnumUnion.Choice>
        {
            private global::CorpusEnumUnion.Kind _d;

            private int number;
            private NativeString text;
            private global::CorpusEnumUnion.Implementation.PayloadUnmanaged payload;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
                payload.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusEnumUnion.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case CorpusEnumUnion.Kind.Number:
                    sample.number = number;
                    break;
                    case CorpusEnumUnion.Kind.Text:
                    sample.text = text.FromNative();
                    break;
                    case CorpusEnumUnion.Kind.PayloadValue:
                    if(sample.Discriminator != _d)
                    {
                        sample.payload = new global::CorpusEnumUnion.Payload();
                    }
                    payload.FromNative(sample.payload, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                number = (int) (0);
                text.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                payload.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusEnumUnion.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case CorpusEnumUnion.Kind.Number:
                    number = sample.number;
                    break;
                    case CorpusEnumUnion.Kind.Text:
                    text.ToNative(sample.text, ((int) 255));
                    break;
                    case CorpusEnumUnion.Kind.PayloadValue:
                    payload.ToNative(sample.payload, keysOnly: false);
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnumUnion.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusEnumUnion.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("number", dtf.GetPrimitiveType<int>(), new int[] {(int) CorpusEnumUnion.Kind.Number}, id: 1),
                    new UnionMember("text", dtf.CreateString(((int) 255)), new int[] {(int) CorpusEnumUnion.Kind.Text}, id: 2),
                    new UnionMember("payload", global::CorpusEnumUnion.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) CorpusEnumUnion.Kind.PayloadValue}, id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(global::CorpusEnumUnion.KindSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusEnumUnion::Choice")
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
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumUnion.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusEnumUnion.Choice>();

    }

} // namespace CorpusEnumUnion

