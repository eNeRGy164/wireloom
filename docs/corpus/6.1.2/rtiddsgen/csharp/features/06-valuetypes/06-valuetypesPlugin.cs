/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-valuetypes.idl
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

namespace CorpusValueTypes
{

    namespace Implementation
    {

        public struct BaseValueUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusValueTypes.BaseValue>
        {

            private int baseValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusValueTypes.BaseValue sample, bool keysOnly = false)
            {

                sample.baseValue = baseValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                baseValue = (int) 0;
            }

            public void ToNative(CorpusValueTypes.BaseValue sample, bool keysOnly = false)
            {
                baseValue = sample.baseValue;
            }
        }

        internal class BaseValuePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusValueTypes.BaseValue, BaseValueUnmanaged>
        {
            internal BaseValuePlugin() : base("CorpusValueTypes.BaseValue", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // BaseValue struct
                var BaseValueStructMembers = new StructMember[]
                {
                    new StructMember("baseValue", dtf.GetPrimitiveType<int>(), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<BaseValueUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusValueTypes::BaseValue")
                    .AddMembers(BaseValueStructMembers));
            }
        }
    }
    public class BaseValueSupport : Rti.Dds.Topics.TypeSupport<CorpusValueTypes.BaseValue>
    {
        public BaseValueSupport() : base(
            new Implementation.BaseValuePlugin(),
            new Lazy<DynamicType>(() =>Implementation.BaseValuePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BaseValueSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BaseValueSupport, CorpusValueTypes.BaseValue>();
    }

    namespace Implementation
    {

        public struct DerivedValueUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusValueTypes.DerivedValue>
        {
            private CorpusValueTypes.Implementation.BaseValueUnmanaged parent;

            private NativeString name;

            public void Destroy(bool optionalsOnly)
            {
                parent.Destroy(optionalsOnly);
                if (optionalsOnly)
                {
                    return;
                }
                name.Destroy();
            }

            public void FromNative(CorpusValueTypes.DerivedValue sample, bool keysOnly = false)
            {

                parent.FromNative(sample, keysOnly);
                sample.name = name.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                parent.Initialize(allocatePointers, allocateMemory);
                name.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusValueTypes.DerivedValue sample, bool keysOnly = false)
            {
                parent.ToNative(sample, keysOnly);
                name.ToNative(sample.name, ((int) 16));
            }
        }

        internal class DerivedValuePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusValueTypes.DerivedValue, DerivedValueUnmanaged>
        {
            internal DerivedValuePlugin() : base("CorpusValueTypes.DerivedValue", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // DerivedValue struct
                var DerivedValueStructMembers = new StructMember[]
                {
                    new StructMember("name", dtf.CreateString(((int) 16)), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<DerivedValueUnmanaged>(
                    dtf.BuildStruct()
                    .WithParent((StructType) CorpusValueTypes.BaseValueSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusValueTypes::DerivedValue")
                    .AddMembers(DerivedValueStructMembers));
            }
        }
    }
    public class DerivedValueSupport : Rti.Dds.Topics.TypeSupport<CorpusValueTypes.DerivedValue>
    {
        public DerivedValueSupport() : base(
            new Implementation.DerivedValuePlugin(),
            new Lazy<DynamicType>(() =>Implementation.DerivedValuePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DerivedValueSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DerivedValueSupport, CorpusValueTypes.DerivedValue>();
    }

    namespace Implementation
    {

        public struct HolderUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusValueTypes.Holder>
        {

            private CorpusValueTypes.Implementation.DerivedValueUnmanaged value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusValueTypes.Holder sample, bool keysOnly = false)
            {

                value.FromNative(sample.value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusValueTypes.Holder sample, bool keysOnly = false)
            {
                value.ToNative(sample.value, keysOnly: false);
            }
        }

        internal class HolderPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusValueTypes.Holder, HolderUnmanaged>
        {
            internal HolderPlugin() : base("CorpusValueTypes.Holder", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Holder struct
                var HolderStructMembers = new StructMember[]
                {
                    new StructMember("value", CorpusValueTypes.DerivedValueSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<HolderUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusValueTypes::Holder")
                    .AddMembers(HolderStructMembers));
            }
        }
    }
    public class HolderSupport : Rti.Dds.Topics.TypeSupport<CorpusValueTypes.Holder>
    {
        public HolderSupport() : base(
            new Implementation.HolderPlugin(),
            new Lazy<DynamicType>(() =>Implementation.HolderPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static HolderSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<HolderSupport, CorpusValueTypes.Holder>();
    }

} // namespace CorpusValueTypes

