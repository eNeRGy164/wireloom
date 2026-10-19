/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-resolve-name-wstring-aliases.idl
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

namespace CorpusResolveNameWstringAliases
{

    namespace Implementation
    {

        public struct BoundedWideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusResolveNameWstringAliases.BoundedWideText>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(global::CorpusResolveNameWstringAliases.BoundedWideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) (CorpusResolveNameWstringAliases.WSTRING_BOUND.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusResolveNameWstringAliases.BoundedWideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) (CorpusResolveNameWstringAliases.WSTRING_BOUND.Value)));
            }
        }

        internal class BoundedWideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusResolveNameWstringAliases.BoundedWideText, BoundedWideTextUnmanaged>
        {

            internal BoundedWideTextPlugin() : base("global::CorpusResolveNameWstringAliases.BoundedWideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextUnmanaged>(
                    dtf,
                    "BoundedWideText",
                    dtf.CreateWideString(((int) (CorpusResolveNameWstringAliases.WSTRING_BOUND.Value))));
            }
        }
    }
    public class BoundedWideTextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusResolveNameWstringAliases.BoundedWideText>
    {
        public BoundedWideTextSupport() : base(
            new Implementation.BoundedWideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextSupport, global::CorpusResolveNameWstringAliases.BoundedWideText>();

    }

    namespace Implementation
    {

        public struct BoundedWideTextAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusResolveNameWstringAliases.BoundedWideTextAlias>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(global::CorpusResolveNameWstringAliases.BoundedWideTextAlias sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) (CorpusResolveNameWstringAliases.WSTRING_BOUND.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusResolveNameWstringAliases.BoundedWideTextAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) (CorpusResolveNameWstringAliases.WSTRING_BOUND.Value)));
            }
        }

        internal class BoundedWideTextAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusResolveNameWstringAliases.BoundedWideTextAlias, BoundedWideTextAliasUnmanaged>
        {

            internal BoundedWideTextAliasPlugin() : base("global::CorpusResolveNameWstringAliases.BoundedWideTextAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextAliasUnmanaged>(
                    dtf,
                    "BoundedWideTextAlias",
                    dtf.CreateWideString(((int) (CorpusResolveNameWstringAliases.WSTRING_BOUND.Value))));
            }
        }
    }
    public class BoundedWideTextAliasSupport : Rti.Dds.Topics.TypeSupport<global::CorpusResolveNameWstringAliases.BoundedWideTextAlias>
    {
        public BoundedWideTextAliasSupport() : base(
            new Implementation.BoundedWideTextAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextAliasSupport, global::CorpusResolveNameWstringAliases.BoundedWideTextAlias>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusResolveNameWstringAliases.Sample>
        {

            private global::Implementation.BoundedWideTextUnmanaged firstLevel;
            private global::Implementation.BoundedWideTextAliasUnmanaged secondLevel;

            public void Destroy(bool optionalsOnly)
            {
                firstLevel.Destroy(optionalsOnly);
                secondLevel.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusResolveNameWstringAliases.Sample sample, bool keysOnly = false)
            {

                firstLevel.FromNative(sample.firstLevel, keysOnly: false);
                secondLevel.FromNative(sample.secondLevel, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                firstLevel.Initialize(allocatePointers, allocateMemory);
                secondLevel.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusResolveNameWstringAliases.Sample sample, bool keysOnly = false)
            {
                firstLevel.ToNative(sample.firstLevel, keysOnly: false);
                secondLevel.ToNative(sample.secondLevel, keysOnly: false);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusResolveNameWstringAliases.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusResolveNameWstringAliases.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("firstLevel", BoundedWideTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("secondLevel", BoundedWideTextAliasSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusResolveNameWstringAliases::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusResolveNameWstringAliases.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusResolveNameWstringAliases.Sample>();

    }

} // namespace CorpusResolveNameWstringAliases

