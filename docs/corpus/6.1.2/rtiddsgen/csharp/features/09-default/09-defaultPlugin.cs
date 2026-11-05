/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-default.idl
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

namespace CorpusDefaultExtensibility
{

    namespace Implementation
    {

        public struct MessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusDefaultExtensibility.Message>
        {

            private int id;
            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(CorpusDefaultExtensibility.Message sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusDefaultExtensibility.Message sample, bool keysOnly = false)
            {
                id = sample.id;
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class MessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusDefaultExtensibility.Message, MessageUnmanaged>
        {
            internal MessagePlugin() : base("CorpusDefaultExtensibility.Message", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Message struct
                var MessageStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<MessageUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusDefaultExtensibility::Message")
                    .AddMembers(MessageStructMembers));
            }
        }
    }
    public class MessageSupport : Rti.Dds.Topics.TypeSupport<CorpusDefaultExtensibility.Message>
    {
        public MessageSupport() : base(
            new Implementation.MessagePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MessagePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MessageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MessageSupport, CorpusDefaultExtensibility.Message>();
    }

} // namespace CorpusDefaultExtensibility

