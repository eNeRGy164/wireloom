/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-alias-aggregate.idl
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

namespace CorpusAggregateAliases
{

    namespace Implementation
    {

        public struct PointUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusAggregateAliases.Point>
        {

            private int x;
            private int y;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusAggregateAliases.Point sample, bool keysOnly = false)
            {

                sample.x = x;
                sample.y = y;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                x = (int) 0;
                y = (int) 0;
            }

            public void ToNative(CorpusAggregateAliases.Point sample, bool keysOnly = false)
            {
                x = sample.x;
                y = sample.y;
            }
        }

        internal class PointPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusAggregateAliases.Point, PointUnmanaged>
        {
            internal PointPlugin() : base("CorpusAggregateAliases.Point", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Point struct
                var PointStructMembers = new StructMember[]
                {
                    new StructMember("x", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("y", dtf.GetPrimitiveType<int>(), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<PointUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregateAliases::Point")
                    .AddMembers(PointStructMembers));
            }
        }
    }
    public class PointSupport : Rti.Dds.Topics.TypeSupport<CorpusAggregateAliases.Point>
    {
        public PointSupport() : base(
            new Implementation.PointPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PointPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PointSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PointSupport, CorpusAggregateAliases.Point>();
    }

    namespace Implementation
    {

        public struct PointAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusAggregateAliases.PointAlias>
        {

            private CorpusAggregateAliases.Implementation.PointUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusAggregateAliases.PointAlias sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusAggregateAliases.PointAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class PointAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusAggregateAliases.PointAlias, PointAliasUnmanaged>
        {
            internal PointAliasPlugin() : base("CorpusAggregateAliases.PointAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<PointAliasUnmanaged>(
                    dtf,
                    "PointAlias",
                    CorpusAggregateAliases.PointSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class PointAliasSupport : Rti.Dds.Topics.TypeSupport<CorpusAggregateAliases.PointAlias>
    {
        public PointAliasSupport() : base(
            new Implementation.PointAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.PointAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PointAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PointAliasSupport, CorpusAggregateAliases.PointAlias>();
    }

    namespace Implementation
    {

        public struct PointAlias2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusAggregateAliases.PointAlias2>
        {

            private CorpusAggregateAliases.Implementation.PointUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusAggregateAliases.PointAlias2 sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusAggregateAliases.PointAlias2 sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class PointAlias2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusAggregateAliases.PointAlias2, PointAlias2Unmanaged>
        {
            internal PointAlias2Plugin() : base("CorpusAggregateAliases.PointAlias2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<PointAlias2Unmanaged>(
                    dtf,
                    "PointAlias2",
                    CorpusAggregateAliases.PointSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class PointAlias2Support : Rti.Dds.Topics.TypeSupport<CorpusAggregateAliases.PointAlias2>
    {
        public PointAlias2Support() : base(
            new Implementation.PointAlias2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.PointAlias2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static PointAlias2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PointAlias2Support, CorpusAggregateAliases.PointAlias2>();
    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusAggregateAliases.Sample>
        {

            private CorpusAggregateAliases.Implementation.PointUnmanaged point;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                point.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusAggregateAliases.Sample sample, bool keysOnly = false)
            {

                point.FromNative(sample.point, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                point.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusAggregateAliases.Sample sample, bool keysOnly = false)
            {
                point.ToNative(sample.point, keysOnly: false);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusAggregateAliases.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusAggregateAliases.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("point", CorpusAggregateAliases.PointAlias2Support.Instance.GetDynamicTypeInternal(isPublic), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregateAliases::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusAggregateAliases.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusAggregateAliases.Sample>();
    }

} // namespace CorpusAggregateAliases

