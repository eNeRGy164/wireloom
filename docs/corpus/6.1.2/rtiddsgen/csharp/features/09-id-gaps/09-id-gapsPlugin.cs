/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-id-gaps.idl
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

namespace CorpusIdGaps
{

    namespace Implementation
    {

        public struct MessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIdGaps.Message>
        {

            private int first;
            private int seventh;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusIdGaps.Message sample, bool keysOnly = false)
            {

                sample.first = first;
                sample.seventh = seventh;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                first = (int) 0;
                seventh = (int) 0;
            }

            public void ToNative(CorpusIdGaps.Message sample, bool keysOnly = false)
            {
                first = sample.first;
                seventh = sample.seventh;
            }
        }

        internal class MessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIdGaps.Message, MessageUnmanaged>
        {
            internal MessagePlugin() : base("CorpusIdGaps.Message", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Message struct
                var MessageStructMembers = new StructMember[]
                {
                    new StructMember("first", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("seventh", dtf.GetPrimitiveType<int>(), id: 7)
                };

                return tsf.CreateTypeWithAccessInfo<MessageUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIdGaps::Message")
                    .AddMembers(MessageStructMembers));
            }
        }
    }
    public class MessageSupport : Rti.Dds.Topics.TypeSupport<CorpusIdGaps.Message>
    {
        public MessageSupport() : base(
            new Implementation.MessagePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MessagePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MessageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MessageSupport, CorpusIdGaps.Message>();
    }

} // namespace CorpusIdGaps

