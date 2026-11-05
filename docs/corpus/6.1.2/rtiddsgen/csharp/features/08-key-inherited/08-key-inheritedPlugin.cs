/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-inherited.idl
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

namespace CorpusInheritedKeys
{

    namespace Implementation
    {

        public struct BaseUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInheritedKeys.Base>
        {

            private int tenant;
            private int baseValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusInheritedKeys.Base sample, bool keysOnly = false)
            {

                sample.tenant = tenant;
                if (keysOnly)
                {
                    return;
                }
                sample.baseValue = baseValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                tenant = (int) 0;
                baseValue = (int) 0;
            }

            public void ToNative(CorpusInheritedKeys.Base sample, bool keysOnly = false)
            {
                tenant = sample.tenant;
                if (keysOnly)
                {
                    return;
                }
                baseValue = sample.baseValue;
            }
        }

        internal class BasePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInheritedKeys.Base, BaseUnmanaged>
        {
            internal BasePlugin() : base("CorpusInheritedKeys.Base", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Base struct
                var BaseStructMembers = new StructMember[]
                {
                    new StructMember("tenant", dtf.GetPrimitiveType<int>(), isKey: true, id: 0),
                    new StructMember("baseValue", dtf.GetPrimitiveType<int>(), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<BaseUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInheritedKeys::Base")
                    .AddMembers(BaseStructMembers));
            }
        }
    }
    public class BaseSupport : Rti.Dds.Topics.TypeSupport<CorpusInheritedKeys.Base>
    {
        public BaseSupport() : base(
            new Implementation.BasePlugin(),
            new Lazy<DynamicType>(() =>Implementation.BasePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BaseSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BaseSupport, CorpusInheritedKeys.Base>();
    }

    namespace Implementation
    {

        public struct DerivedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusInheritedKeys.Derived>
        {
            private CorpusInheritedKeys.Implementation.BaseUnmanaged parent;

            private int localId;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                parent.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusInheritedKeys.Derived sample, bool keysOnly = false)
            {

                sample.localId = localId;
                parent.FromNative(sample, keysOnly);
                if (keysOnly)
                {
                    return;
                }
                sample.payload = payload;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                parent.Initialize(allocatePointers, allocateMemory);
                localId = (int) 0;
                payload = (int) 0;
            }

            public void ToNative(CorpusInheritedKeys.Derived sample, bool keysOnly = false)
            {
                localId = sample.localId;
                parent.ToNative(sample, keysOnly);
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class DerivedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusInheritedKeys.Derived, DerivedUnmanaged>
        {
            internal DerivedPlugin() : base("CorpusInheritedKeys.Derived", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Derived struct
                var DerivedStructMembers = new StructMember[]
                {
                    new StructMember("localId", dtf.GetPrimitiveType<int>(), isKey: true, id: 2),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 3)
                };

                return tsf.CreateTypeWithAccessInfo<DerivedUnmanaged>(
                    dtf.BuildStruct()
                    .WithParent((StructType) CorpusInheritedKeys.BaseSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInheritedKeys::Derived")
                    .AddMembers(DerivedStructMembers));
            }
        }
    }
    public class DerivedSupport : Rti.Dds.Topics.TypeSupport<CorpusInheritedKeys.Derived>
    {
        public DerivedSupport() : base(
            new Implementation.DerivedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DerivedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DerivedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DerivedSupport, CorpusInheritedKeys.Derived>();
    }

} // namespace CorpusInheritedKeys

