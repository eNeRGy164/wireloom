/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from autoid-hash-include.idl
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

    public struct IncludedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<Included>
    {

        private int first;
        private int second;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(Included sample, bool keysOnly = false)
        {

            sample.first = first;
            sample.second = second;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            first = (int) 0;
            second = (int) 0;
        }

        public void ToNative(Included sample, bool keysOnly = false)
        {
            first = sample.first;
            second = sample.second;
        }
    }

    internal class IncludedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<Included, IncludedUnmanaged>
    {
        internal IncludedPlugin() : base("Included", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // Included struct
            var IncludedStructMembers = new StructMember[]
            {
                new StructMember("first", dtf.GetPrimitiveType<int>(), id: 0),
                new StructMember("second", dtf.GetPrimitiveType<int>(), id: 1)
            };

            return tsf.CreateTypeWithAccessInfo<IncludedUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("Included")
                .AddMembers(IncludedStructMembers));
        }
    }
}
public class IncludedSupport : Rti.Dds.Topics.TypeSupport<Included>
{
    public IncludedSupport() : base(
        new Implementation.IncludedPlugin(),
        new Lazy<DynamicType>(() =>Implementation.IncludedPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static IncludedSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IncludedSupport, Included>();
}

