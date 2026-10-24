/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant-topic.idl
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

    public struct EnumConstantSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::EnumConstantSample>
    {

        private global::Implementation.EnumConstantUnmanaged value;
        private global::Implementation.EnumUnionUnmanaged choice;

        public void Destroy(bool optionalsOnly)
        {
            choice.Destroy(optionalsOnly);
            if (optionalsOnly)
            {
                return;
            }
            value.Destroy(optionalsOnly);
        }

        public void FromNative(global::EnumConstantSample sample, bool keysOnly = false)
        {

            value.FromNative(sample.value, keysOnly: false);
            choice.FromNative(sample.choice, keysOnly: false);
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            value.Initialize(allocatePointers, allocateMemory);
            choice.Initialize(allocatePointers, allocateMemory);
        }

        public void ToNative(global::EnumConstantSample sample, bool keysOnly = false)
        {
            value.ToNative(sample.value, keysOnly: false);
            choice.ToNative(sample.choice, keysOnly: false);
        }
    }

    internal class EnumConstantSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::EnumConstantSample, EnumConstantSampleUnmanaged>
    {

        internal EnumConstantSamplePlugin() : base("global::EnumConstantSample", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // EnumConstantSample struct
            var EnumConstantSampleStructMembers = new StructMember[]
            {
                new StructMember("value", global::EnumConstantSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                new StructMember("choice", global::EnumUnionSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<EnumConstantSampleUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("EnumConstantSample")
                .AddMembers(EnumConstantSampleStructMembers));

            return result;

        }
    }
}
public class EnumConstantSampleSupport : Rti.Dds.Topics.TypeSupport<global::EnumConstantSample>
{
    public EnumConstantSampleSupport() : base(
        new Implementation.EnumConstantSamplePlugin(),
        new Lazy<DynamicType>(() =>Implementation.EnumConstantSamplePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static EnumConstantSampleSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnumConstantSampleSupport, global::EnumConstantSample>();

}

