/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from common.idl
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

    public struct IncludedTypeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<IncludedType>
    {

        private int value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(IncludedType sample, bool keysOnly = false)
        {

            sample.value = value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            value = (int) 0;
        }

        public void ToNative(IncludedType sample, bool keysOnly = false)
        {
            value = sample.value;
        }
    }

    internal class IncludedTypePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<IncludedType, IncludedTypeUnmanaged>
    {
        internal IncludedTypePlugin() : base("IncludedType", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // IncludedType struct
            var IncludedTypeStructMembers = new StructMember[]
            {
                new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)
            };

            return tsf.CreateTypeWithAccessInfo<IncludedTypeUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("IncludedType")
                .AddMembers(IncludedTypeStructMembers));
        }
    }
}
public class IncludedTypeSupport : Rti.Dds.Topics.TypeSupport<IncludedType>
{
    public IncludedTypeSupport() : base(
        new Implementation.IncludedTypePlugin(),
        new Lazy<DynamicType>(() =>Implementation.IncludedTypePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static IncludedTypeSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IncludedTypeSupport, IncludedType>();
}

