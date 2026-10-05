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

        public struct HolderUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalAggregate.Holder>
        {

            private NativeManagedOptional payload;

            public void Destroy(bool optionalsOnly)
            {
                payload.Destroy<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalAggregate.Holder sample, bool keysOnly = false)
            {

                payload.FromNative<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(out var payloadTemporary_);
                sample.payload = payloadTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
            }

            public void ToNative(global::CorpusOptionalAggregate.Holder sample, bool keysOnly = false)
            {
                payload.ToNative<global::CorpusOptionalAggregate.Payload, global::CorpusOptionalAggregate.Implementation.PayloadUnmanaged>(sample.payload);
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
                    new StructMember("payload", global::CorpusOptionalAggregate.PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 0)

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
