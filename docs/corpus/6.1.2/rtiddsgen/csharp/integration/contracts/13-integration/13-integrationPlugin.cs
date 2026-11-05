/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-integration.idl
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

namespace Implementation
{

    public struct CorpusIntegrationMessageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationMessage>
    {

        private NativeString text;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            text.Destroy();
        }

        public void FromNative(CorpusIntegrationMessage sample, bool keysOnly = false)
        {

            sample.text = text.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            text.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
        }

        public void ToNative(CorpusIntegrationMessage sample, bool keysOnly = false)
        {
            text.ToNative(sample.text, ((int) 255));
        }
    }

    internal class CorpusIntegrationMessagePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationMessage, CorpusIntegrationMessageUnmanaged>
    {
        internal CorpusIntegrationMessagePlugin() : base("CorpusIntegrationMessage", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // CorpusIntegrationMessage struct
            var CorpusIntegrationMessageStructMembers = new StructMember[]
            {
                new StructMember("text", dtf.CreateString(((int) 255)), id: 0)
            };

            return tsf.CreateTypeWithAccessInfo<CorpusIntegrationMessageUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("CorpusIntegrationMessage")
                .AddMembers(CorpusIntegrationMessageStructMembers));
        }
    }
}
public class CorpusIntegrationMessageSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationMessage>
{
    public CorpusIntegrationMessageSupport() : base(
        new Implementation.CorpusIntegrationMessagePlugin(),
        new Lazy<DynamicType>(() =>Implementation.CorpusIntegrationMessagePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static CorpusIntegrationMessageSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<CorpusIntegrationMessageSupport, CorpusIntegrationMessage>();
}

