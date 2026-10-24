/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-explicit.idl
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

namespace CorpusEnumValues
{

    namespace Implementation
    {

        public struct EnumTopicUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnumValues.EnumTopic>
        {

            private global::CorpusEnumValues.Explicit value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusEnumValues.EnumTopic sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (global::CorpusEnumValues.Explicit) (-2);
            }

            public void ToNative(global::CorpusEnumValues.EnumTopic sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class EnumTopicPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnumValues.EnumTopic, EnumTopicUnmanaged>
        {

            internal EnumTopicPlugin() : base("global::CorpusEnumValues.EnumTopic", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // EnumTopic struct
                var EnumTopicStructMembers = new StructMember[]
                {
                    new StructMember("value", global::CorpusEnumValues.ExplicitSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<EnumTopicUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusEnumValues::EnumTopic")
                    .AddMembers(EnumTopicStructMembers));

                return result;
            }
        }
    }
    public class EnumTopicSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumValues.EnumTopic>
    {
        public EnumTopicSupport() : base(
            new Implementation.EnumTopicPlugin(),
            new Lazy<DynamicType>(() =>Implementation.EnumTopicPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static EnumTopicSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnumTopicSupport, global::CorpusEnumValues.EnumTopic>();

    }

} // namespace CorpusEnumValues

