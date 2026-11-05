/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
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

namespace AutoIds
{

    namespace Implementation
    {

        public struct HashedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<AutoIds.Hashed>
        {

            private int first;
            private int second;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(AutoIds.Hashed sample, bool keysOnly = false)
            {

                sample.first = first;
                sample.second = second;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                first = (int) 0;
                second = (int) 0;
            }

            public void ToNative(AutoIds.Hashed sample, bool keysOnly = false)
            {
                first = sample.first;
                second = sample.second;
            }
        }

        internal class HashedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<AutoIds.Hashed, HashedUnmanaged>
        {
            internal HashedPlugin() : base("AutoIds.Hashed", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Hashed struct
                var HashedStructMembers = new StructMember[]
                {
                    new StructMember("first", dtf.GetPrimitiveType<int>(), id: 64291979),
                    new StructMember("second", dtf.GetPrimitiveType<int>(), id: 182907049)
                };

                return tsf.CreateTypeWithAccessInfo<HashedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("AutoIds::Hashed")
                    .AddMembers(HashedStructMembers));
            }
        }
    }
    public class HashedSupport : Rti.Dds.Topics.TypeSupport<AutoIds.Hashed>
    {
        public HashedSupport() : base(
            new Implementation.HashedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.HashedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static HashedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<HashedSupport, AutoIds.Hashed>();
    }

} // namespace AutoIds

namespace AutoIds
{

    namespace Implementation
    {

        public struct ReopenedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<AutoIds.Reopened>
        {

            private int first;
            private int second;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(AutoIds.Reopened sample, bool keysOnly = false)
            {

                sample.first = first;
                sample.second = second;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                first = (int) 0;
                second = (int) 0;
            }

            public void ToNative(AutoIds.Reopened sample, bool keysOnly = false)
            {
                first = sample.first;
                second = sample.second;
            }
        }

        internal class ReopenedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<AutoIds.Reopened, ReopenedUnmanaged>
        {
            internal ReopenedPlugin() : base("AutoIds.Reopened", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Reopened struct
                var ReopenedStructMembers = new StructMember[]
                {
                    new StructMember("first", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("second", dtf.GetPrimitiveType<int>(), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ReopenedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("AutoIds::Reopened")
                    .AddMembers(ReopenedStructMembers));
            }
        }
    }
    public class ReopenedSupport : Rti.Dds.Topics.TypeSupport<AutoIds.Reopened>
    {
        public ReopenedSupport() : base(
            new Implementation.ReopenedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ReopenedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ReopenedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ReopenedSupport, AutoIds.Reopened>();
    }

} // namespace AutoIds

namespace Implementation
{

    public struct SequentialUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<Sequential>
    {

        private int first;
        private int second;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(Sequential sample, bool keysOnly = false)
        {

            sample.first = first;
            sample.second = second;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            first = (int) 0;
            second = (int) 0;
        }

        public void ToNative(Sequential sample, bool keysOnly = false)
        {
            first = sample.first;
            second = sample.second;
        }
    }

    internal class SequentialPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<Sequential, SequentialUnmanaged>
    {
        internal SequentialPlugin() : base("Sequential", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // Sequential struct
            var SequentialStructMembers = new StructMember[]
            {
                new StructMember("first", dtf.GetPrimitiveType<int>(), id: 0),
                new StructMember("second", dtf.GetPrimitiveType<int>(), id: 1)
            };

            return tsf.CreateTypeWithAccessInfo<SequentialUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("Sequential")
                .AddMembers(SequentialStructMembers));
        }
    }
}
public class SequentialSupport : Rti.Dds.Topics.TypeSupport<Sequential>
{
    public SequentialSupport() : base(
        new Implementation.SequentialPlugin(),
        new Lazy<DynamicType>(() =>Implementation.SequentialPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static SequentialSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SequentialSupport, Sequential>();
}

namespace Implementation
{

    public struct UsesIncludeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<UsesInclude>
    {

        private global::Implementation.IncludedUnmanaged included;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            included.Destroy(optionalsOnly);
        }

        public void FromNative(UsesInclude sample, bool keysOnly = false)
        {

            included.FromNative(sample.included, keysOnly: false);
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            included.Initialize(allocatePointers, allocateMemory);
        }

        public void ToNative(UsesInclude sample, bool keysOnly = false)
        {
            included.ToNative(sample.included, keysOnly: false);
        }
    }

    internal class UsesIncludePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<UsesInclude, UsesIncludeUnmanaged>
    {
        internal UsesIncludePlugin() : base("UsesInclude", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // UsesInclude struct
            var UsesIncludeStructMembers = new StructMember[]
            {
                new StructMember("included", IncludedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)
            };

            return tsf.CreateTypeWithAccessInfo<UsesIncludeUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("UsesInclude")
                .AddMembers(UsesIncludeStructMembers));
        }
    }
}
public class UsesIncludeSupport : Rti.Dds.Topics.TypeSupport<UsesInclude>
{
    public UsesIncludeSupport() : base(
        new Implementation.UsesIncludePlugin(),
        new Lazy<DynamicType>(() =>Implementation.UsesIncludePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static UsesIncludeSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<UsesIncludeSupport, UsesInclude>();
}

