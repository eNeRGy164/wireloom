/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-evolution-optional.idl
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

namespace CorpusOptionalEvolution
{

    namespace Implementation
    {

        public struct MessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalEvolution.Message>
        {

            private int requiredValue;
            private NativeUnmanagedOptional optionalValue;
            private NativeString optionalText;

            public void Destroy(bool optionalsOnly)
            {
                optionalValue.Destroy(optionalsOnly);
                optionalText.Destroy();
            }

            public void FromNative(global::CorpusOptionalEvolution.Message sample, bool keysOnly = false)
            {

                sample.requiredValue = requiredValue;
                sample.optionalValue = optionalValue.FromNative<int>();
                sample.optionalText = optionalText.FromNativeOptional();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                requiredValue = (int) (0);
            }

            public void ToNative(global::CorpusOptionalEvolution.Message sample, bool keysOnly = false)
            {
                requiredValue = sample.requiredValue;
                optionalValue.ToNative<int>(sample.optionalValue);
                optionalText.ToNativeOptional(sample.optionalText, ((int) 16));
            }
        }

        internal class MessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalEvolution.Message, MessageUnmanaged>
        {

            internal MessagePlugin() : base("global::CorpusOptionalEvolution.Message", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Message struct
                var MessageStructMembers = new StructMember[]
                {
                    new StructMember("requiredValue", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("optionalValue", dtf.GetPrimitiveType<int>(), isOptional: true, id: 2),
                    new StructMember("optionalText", dtf.CreateString(((int) 16)), isOptional: true, id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<MessageUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalEvolution::Message")
                    .AddMembers(MessageStructMembers));

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
    public class MessageSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalEvolution.Message>
    {
        public MessageSupport() : base(
            new Implementation.MessagePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MessagePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MessageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MessageSupport, global::CorpusOptionalEvolution.Message>();

    }

} // namespace CorpusOptionalEvolution

