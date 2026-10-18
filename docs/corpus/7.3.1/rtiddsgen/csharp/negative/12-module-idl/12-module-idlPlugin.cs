/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-module-idl.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

namespace idl
{

    namespace Implementation
    {

        public struct SystemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::idl.System>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::idl.System sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::idl.System sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class SystemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::idl.System, SystemUnmanaged>
        {

            internal SystemPlugin() : base("global::idl.System", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // System struct
                var SystemStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SystemUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("idl::System")
                    .AddMembers(SystemStructMembers));

                return result;
            }
        }
    }
    public class SystemSupport : Rti.Dds.Topics.TypeSupport<global::idl.System>
    {
        public SystemSupport() : base(
            new Implementation.SystemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.SystemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SystemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SystemSupport, global::idl.System>();

    }

} // namespace idl

