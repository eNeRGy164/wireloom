/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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

namespace CorpusEnumValuePrefix
{

    namespace Implementation
    {

        public struct EnumTopicUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnumValuePrefix.EnumTopic>
        {

            private CorpusEnumValuePrefix.Annotated value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusEnumValuePrefix.EnumTopic sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (CorpusEnumValuePrefix.Annotated) 7;
            }

            public void ToNative(CorpusEnumValuePrefix.EnumTopic sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class EnumTopicPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnumValuePrefix.EnumTopic, EnumTopicUnmanaged>
        {
            internal EnumTopicPlugin() : base("CorpusEnumValuePrefix.EnumTopic", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // EnumTopic struct
                var EnumTopicStructMembers = new StructMember[]
                {
                    new StructMember("value", CorpusEnumValuePrefix.AnnotatedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<EnumTopicUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusEnumValuePrefix::EnumTopic")
                    .AddMembers(EnumTopicStructMembers));
            }
        }
    }
    public class EnumTopicSupport : Rti.Dds.Topics.TypeSupport<CorpusEnumValuePrefix.EnumTopic>
    {
        public EnumTopicSupport() : base(
            new Implementation.EnumTopicPlugin(),
            new Lazy<DynamicType>(() =>Implementation.EnumTopicPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static EnumTopicSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnumTopicSupport, CorpusEnumValuePrefix.EnumTopic>();
    }

} // namespace CorpusEnumValuePrefix

