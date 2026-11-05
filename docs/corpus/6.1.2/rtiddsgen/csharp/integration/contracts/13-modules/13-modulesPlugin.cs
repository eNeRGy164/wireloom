/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-modules.idl
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

namespace CorpusIntegration
{

    namespace Contracts
    {

        namespace Implementation
        {

            public struct NamespacedMessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegration.Contracts.NamespacedMessage>
            {

                private byte enabled;

                public void Destroy(bool optionalsOnly)
                {
                }

                public void FromNative(CorpusIntegration.Contracts.NamespacedMessage sample, bool keysOnly = false)
                {

                    sample.enabled = Convert.ToBoolean(enabled);
                }

                public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
                {
                    enabled = 0;
                }

                public void ToNative(CorpusIntegration.Contracts.NamespacedMessage sample, bool keysOnly = false)
                {
                    enabled = Convert.ToByte(sample.enabled);
                }
            }

            internal class NamespacedMessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegration.Contracts.NamespacedMessage, NamespacedMessageUnmanaged>
            {
                internal NamespacedMessagePlugin() : base("CorpusIntegration.Contracts.NamespacedMessage", isKeyed: false, CreateDynamicType(isPublic: false))
                {
                }

                public static DynamicType CreateDynamicType(bool isPublic = true)
                {
                    var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                    var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                    // NamespacedMessage struct
                    var NamespacedMessageStructMembers = new StructMember[]
                    {
                        new StructMember("enabled", dtf.GetPrimitiveType<bool>(), id: 0)
                    };

                    return tsf.CreateTypeWithAccessInfo<NamespacedMessageUnmanaged>(
                        dtf.BuildStruct()
                        .WithExtensibility(ExtensibilityKind.Extensible)
                        .WithName("CorpusIntegration::Contracts::NamespacedMessage")
                        .AddMembers(NamespacedMessageStructMembers));
                }
            }
        }
        public class NamespacedMessageSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegration.Contracts.NamespacedMessage>
        {
            public NamespacedMessageSupport() : base(
                new Implementation.NamespacedMessagePlugin(),
                new Lazy<DynamicType>(() =>Implementation.NamespacedMessagePlugin.CreateDynamicType(isPublic: true)))
            {
            }

            public static NamespacedMessageSupport Instance { get; } =
            ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NamespacedMessageSupport, CorpusIntegration.Contracts.NamespacedMessage>();
        }

    } // namespace Contracts

} // namespace CorpusIntegration

