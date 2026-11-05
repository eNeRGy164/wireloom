/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-topic-module.idl
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

namespace CorpusTopicModule
{

    namespace Implementation
    {

        public struct IdentifierUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusTopicModule.Identifier>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusTopicModule.Identifier sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) 0;
            }

            public void ToNative(CorpusTopicModule.Identifier sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class IdentifierPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusTopicModule.Identifier, IdentifierUnmanaged>
        {
            internal IdentifierPlugin() : base("CorpusTopicModule.Identifier", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<IdentifierUnmanaged>(
                    dtf,
                    "Identifier",
                    dtf.GetPrimitiveType<int>());
            }
        }
    }
    public class IdentifierSupport : Rti.Dds.Topics.TypeSupport<CorpusTopicModule.Identifier>
    {
        public IdentifierSupport() : base(
            new Implementation.IdentifierPlugin(),
            new Lazy<DynamicType>(() =>Implementation.IdentifierPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static IdentifierSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IdentifierSupport, CorpusTopicModule.Identifier>();
    }

    namespace Implementation
    {

        public struct MessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusTopicModule.Message>
        {

            private int id;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusTopicModule.Message sample, bool keysOnly = false)
            {

                sample.id = id;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
            }

            public void ToNative(CorpusTopicModule.Message sample, bool keysOnly = false)
            {
                id = sample.id;
            }
        }

        internal class MessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusTopicModule.Message, MessageUnmanaged>
        {
            internal MessagePlugin() : base("CorpusTopicModule.Message", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Message struct
                var MessageStructMembers = new StructMember[]
                {
                    new StructMember("id", CorpusTopicModule.IdentifierSupport.Instance.GetDynamicTypeInternal(isPublic), isKey: true, id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<MessageUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusTopicModule::Message")
                    .AddMembers(MessageStructMembers));
            }
        }
    }
    public class MessageSupport : Rti.Dds.Topics.TypeSupport<CorpusTopicModule.Message>
    {
        public MessageSupport() : base(
            new Implementation.MessagePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MessagePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MessageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MessageSupport, CorpusTopicModule.Message>();
    }

} // namespace CorpusTopicModule

