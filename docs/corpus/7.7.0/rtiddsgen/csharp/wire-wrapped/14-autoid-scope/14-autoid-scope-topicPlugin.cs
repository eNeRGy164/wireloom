/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope-topic.idl
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

    public struct AutoIdScopeSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::AutoIdScopeSample>
    {

        private global::AutoIds.Implementation.HashedUnmanaged hashed;
        private global::AutoIds.Implementation.ReopenedUnmanaged reopened;
        private global::Implementation.SequentialUnmanaged sequential;
        private global::Implementation.UsesIncludeUnmanaged usesInclude;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            hashed.Destroy(optionalsOnly);
            reopened.Destroy(optionalsOnly);
            sequential.Destroy(optionalsOnly);
            usesInclude.Destroy(optionalsOnly);
        }

        public void FromNative(global::AutoIdScopeSample sample, bool keysOnly = false)
        {

            hashed.FromNative(sample.hashed, keysOnly: false);
            reopened.FromNative(sample.reopened, keysOnly: false);
            sequential.FromNative(sample.sequential, keysOnly: false);
            usesInclude.FromNative(sample.usesInclude, keysOnly: false);
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            hashed.Initialize(allocatePointers, allocateMemory);
            reopened.Initialize(allocatePointers, allocateMemory);
            sequential.Initialize(allocatePointers, allocateMemory);
            usesInclude.Initialize(allocatePointers, allocateMemory);
        }

        public void ToNative(global::AutoIdScopeSample sample, bool keysOnly = false)
        {
            hashed.ToNative(sample.hashed, keysOnly: false);
            reopened.ToNative(sample.reopened, keysOnly: false);
            sequential.ToNative(sample.sequential, keysOnly: false);
            usesInclude.ToNative(sample.usesInclude, keysOnly: false);
        }
    }

    internal class AutoIdScopeSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::AutoIdScopeSample, AutoIdScopeSampleUnmanaged>
    {

        internal AutoIdScopeSamplePlugin() : base("global::AutoIdScopeSample", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // AutoIdScopeSample struct
            var AutoIdScopeSampleStructMembers = new StructMember[]
            {
                new StructMember("hashed", global::AutoIds.HashedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                new StructMember("reopened", global::AutoIds.ReopenedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                new StructMember("sequential", global::SequentialSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                new StructMember("usesInclude", global::UsesIncludeSupport.Instance.GetDynamicTypeInternal(isPublic), id: 3)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<AutoIdScopeSampleUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("AutoIdScopeSample")
                .AddMembers(AutoIdScopeSampleStructMembers));

            return result;

        }
    }
}
public class AutoIdScopeSampleSupport : Rti.Dds.Topics.TypeSupport<global::AutoIdScopeSample>
{
    public AutoIdScopeSampleSupport() : base(
        new Implementation.AutoIdScopeSamplePlugin(),
        new Lazy<DynamicType>(() =>Implementation.AutoIdScopeSamplePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static AutoIdScopeSampleSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<AutoIdScopeSampleSupport, global::AutoIdScopeSample>();

}

