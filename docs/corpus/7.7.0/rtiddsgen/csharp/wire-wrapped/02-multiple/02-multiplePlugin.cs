/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-multiple.idl
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

namespace Implementation
{

    public struct WireloomWireCompatibilityMultipleTypesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::WireloomWireCompatibilityMultipleTypes>
    {

        private global::Implementation.FirstUnmanaged first;
        private global::Implementation.SecondUnmanaged second;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            first.Destroy(optionalsOnly);
            second.Destroy(optionalsOnly);
        }

        public void FromNative(global::WireloomWireCompatibilityMultipleTypes sample, bool keysOnly = false)
        {

            first.FromNative(sample.first, keysOnly: false);
            second.FromNative(sample.second, keysOnly: false);
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            first.Initialize(allocatePointers, allocateMemory);
            second.Initialize(allocatePointers, allocateMemory);
        }

        public void ToNative(global::WireloomWireCompatibilityMultipleTypes sample, bool keysOnly = false)
        {
            first.ToNative(sample.first, keysOnly: false);
            second.ToNative(sample.second, keysOnly: false);
        }
    }

    internal class WireloomWireCompatibilityMultipleTypesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::WireloomWireCompatibilityMultipleTypes, WireloomWireCompatibilityMultipleTypesUnmanaged>
    {

        internal WireloomWireCompatibilityMultipleTypesPlugin() : base("global::WireloomWireCompatibilityMultipleTypes", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // WireloomWireCompatibilityMultipleTypes struct
            var WireloomWireCompatibilityMultipleTypesStructMembers = new StructMember[]
            {
                new StructMember("first", global::FirstSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                new StructMember("second", global::SecondSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<WireloomWireCompatibilityMultipleTypesUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("WireloomWireCompatibilityMultipleTypes")
                .AddMembers(WireloomWireCompatibilityMultipleTypesStructMembers));

            return result;

        }
    }
}
public class WireloomWireCompatibilityMultipleTypesSupport : Rti.Dds.Topics.TypeSupport<global::WireloomWireCompatibilityMultipleTypes>
{
    public WireloomWireCompatibilityMultipleTypesSupport() : base(
        new Implementation.WireloomWireCompatibilityMultipleTypesPlugin(),
        new Lazy<DynamicType>(() =>Implementation.WireloomWireCompatibilityMultipleTypesPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static WireloomWireCompatibilityMultipleTypesSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WireloomWireCompatibilityMultipleTypesSupport, global::WireloomWireCompatibilityMultipleTypes>();

}

