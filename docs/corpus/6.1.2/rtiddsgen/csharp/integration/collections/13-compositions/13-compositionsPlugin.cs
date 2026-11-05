/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-compositions.idl
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

namespace CorpusIntegrationCollections
{

    namespace Implementation
    {

        public struct ItemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationCollections.Item>
        {

            private int id;
            private NativeString label;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                label.Destroy();
            }

            public void FromNative(CorpusIntegrationCollections.Item sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
                label.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusIntegrationCollections.Item sample, bool keysOnly = false)
            {
                id = sample.id;
                label.ToNative(sample.label, ((int) 16));
            }
        }

        internal class ItemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationCollections.Item, ItemUnmanaged>
        {
            internal ItemPlugin() : base("CorpusIntegrationCollections.Item", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Item struct
                var ItemStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("label", dtf.CreateString(((int) 16)), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ItemUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIntegrationCollections::Item")
                    .AddMembers(ItemStructMembers));
            }
        }
    }
    public class ItemSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationCollections.Item>
    {
        public ItemSupport() : base(
            new Implementation.ItemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemSupport, CorpusIntegrationCollections.Item>();
    }

    namespace Implementation
    {

        public struct RowUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationCollections.Row>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusIntegrationCollections.Row sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (2));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int>(dimension: (2), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusIntegrationCollections.Row sample, bool keysOnly = false)
            {
                Value.ToNative<int>(sample.Value, dimension: (2));
            }
        }

        internal class RowPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationCollections.Row, RowUnmanaged>
        {
            internal RowPlugin() : base("CorpusIntegrationCollections.Row", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<RowUnmanaged>(
                    dtf,
                    "Row",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2}
                ));
            }
        }
    }
    public class RowSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationCollections.Row>
    {
        public RowSupport() : base(
            new Implementation.RowPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RowPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RowSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RowSupport, CorpusIntegrationCollections.Row>();
    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusIntegrationCollections.Sample>
        {

            private NativeSeq unboundedItems;
            private NativeSeq boundedItems;
            private NativeManagedArray aggregateArray;
            private NativeManagedArray rows;
            private NativeSeq sequenceOfArrays;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                unboundedItems.Destroy<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(optionalsOnly);
                boundedItems.Destroy<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(optionalsOnly);
                aggregateArray.Destroy<CorpusIntegrationCollections.Item,CorpusIntegrationCollections.Implementation.ItemUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
                rows.Destroy<CorpusIntegrationCollections.Row,CorpusIntegrationCollections.Implementation.RowUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
                sequenceOfArrays.Destroy<CorpusIntegrationCollections.Row, CorpusIntegrationCollections.Implementation.RowUnmanaged>(optionalsOnly);
            }

            public void FromNative(CorpusIntegrationCollections.Sample sample, bool keysOnly = false)
            {

                unboundedItems.FromNative<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.FromNative<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.boundedItems);
                aggregateArray.FromNative<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.aggregateArray, keysOnly: keysOnly, dimension: (2));
                rows.FromNative<CorpusIntegrationCollections.Row, CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.FromNative<CorpusIntegrationCollections.Row, CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                unboundedItems.Initialize<CorpusIntegrationCollections.Item , CorpusIntegrationCollections.Implementation.ItemUnmanaged >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
                boundedItems.Initialize<CorpusIntegrationCollections.Item , CorpusIntegrationCollections.Implementation.ItemUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                aggregateArray.Initialize<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
                rows.Initialize<CorpusIntegrationCollections.Row, CorpusIntegrationCollections.Implementation.RowUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
                sequenceOfArrays.Initialize<CorpusIntegrationCollections.Row , CorpusIntegrationCollections.Implementation.RowUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusIntegrationCollections.Sample sample, bool keysOnly = false)
            {
                unboundedItems.ToNative<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.ToNative<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.boundedItems);
                aggregateArray.ToNative<CorpusIntegrationCollections.Item, CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.aggregateArray, keysOnly: keysOnly, dimension: (2));
                rows.ToNative<CorpusIntegrationCollections.Row, CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.ToNative<CorpusIntegrationCollections.Row, CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusIntegrationCollections.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusIntegrationCollections.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("unboundedItems", tsf.CreateSequenceWithAccessInfo(dtf, CorpusIntegrationCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)100)), id: 0),
                    new StructMember("boundedItems", tsf.CreateSequenceWithAccessInfo(dtf, CorpusIntegrationCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 1),
                    new StructMember("aggregateArray", tsf.CreateArrayWithAccessInfo<CorpusIntegrationCollections.Implementation.ItemUnmanaged>(dtf, CorpusIntegrationCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}
                    ), id: 2),
                    new StructMember("rows", tsf.CreateArrayWithAccessInfo<CorpusIntegrationCollections.Implementation.RowUnmanaged>(dtf, CorpusIntegrationCollections.RowSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}
                    ), id: 3),
                    new StructMember("sequenceOfArrays", tsf.CreateSequenceWithAccessInfo(dtf, CorpusIntegrationCollections.RowSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 4)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIntegrationCollections::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusIntegrationCollections.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusIntegrationCollections.Sample>();
    }

} // namespace CorpusIntegrationCollections

