/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-prefix.idl
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

namespace CorpusEnumValuePrefix
{

    namespace Implementation
    {

        public struct EnumTopicUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnumValuePrefix.EnumTopic>
        {

            private global::CorpusEnumValuePrefix.Annotated value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusEnumValuePrefix.EnumTopic sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (global::CorpusEnumValuePrefix.Annotated) (7);
            }

            public void ToNative(global::CorpusEnumValuePrefix.EnumTopic sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class EnumTopicPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnumValuePrefix.EnumTopic, EnumTopicUnmanaged>
        {

            internal EnumTopicPlugin() : base("global::CorpusEnumValuePrefix.EnumTopic", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // EnumTopic struct
                var EnumTopicStructMembers = new StructMember[]
                {
                    new StructMember("value", global::CorpusEnumValuePrefix.AnnotatedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<EnumTopicUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusEnumValuePrefix::EnumTopic")
                    .AddMembers(EnumTopicStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 7;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }

                return result;

            }
        }
    }
    public class EnumTopicSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumValuePrefix.EnumTopic>
    {
        public EnumTopicSupport() : base(
            new Implementation.EnumTopicPlugin(),
            new Lazy<DynamicType>(() =>Implementation.EnumTopicPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static EnumTopicSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnumTopicSupport, global::CorpusEnumValuePrefix.EnumTopic>();

    }

} // namespace CorpusEnumValuePrefix

