/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-evolution-optional.idl
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

namespace CorpusOptionalEvolution
{

    namespace Implementation
    {

        public struct MessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusOptionalEvolution.Message>
        {

            private int requiredValue;
            private NativeUnmanagedOptional optionalValue;
            private NativeString optionalText;

            public void Destroy(bool optionalsOnly)
            {
                optionalValue.Destroy(optionalsOnly);
                optionalText.Destroy();
            }

            public void FromNative(CorpusOptionalEvolution.Message sample, bool keysOnly = false)
            {

                sample.requiredValue = requiredValue;
                sample.optionalValue = optionalValue.FromNative<int>();
                sample.optionalText = optionalText.FromNativeOptional();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                requiredValue = (int) 0;
            }

            public void ToNative(CorpusOptionalEvolution.Message sample, bool keysOnly = false)
            {
                requiredValue = sample.requiredValue;
                optionalValue.ToNative<int>(sample.optionalValue);
                optionalText.ToNativeOptional(sample.optionalText, ((int) 16));
            }
        }

        internal class MessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusOptionalEvolution.Message, MessageUnmanaged>
        {
            internal MessagePlugin() : base("CorpusOptionalEvolution.Message", isKeyed: false, CreateDynamicType(isPublic: false))
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

                return tsf.CreateTypeWithAccessInfo<MessageUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalEvolution::Message")
                    .AddMembers(MessageStructMembers));
            }
        }
    }
    public class MessageSupport : Rti.Dds.Topics.TypeSupport<CorpusOptionalEvolution.Message>
    {
        public MessageSupport() : base(
            new Implementation.MessagePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MessagePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MessageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MessageSupport, CorpusOptionalEvolution.Message>();
    }

} // namespace CorpusOptionalEvolution

