/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-shapes.idl
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

namespace CorpusCollectionShapes
{

    namespace Implementation
    {

        public struct ItemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.Item>
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

            public void FromNative(CorpusCollectionShapes.Item sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
                label.Initialize(size: ((int) 12), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.Item sample, bool keysOnly = false)
            {
                id = sample.id;
                label.ToNative(sample.label, ((int) 12));
            }
        }

        internal class ItemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.Item, ItemUnmanaged>
        {
            internal ItemPlugin() : base("CorpusCollectionShapes.Item", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("label", dtf.CreateString(((int) 12)), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ItemUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollectionShapes::Item")
                    .AddMembers(ItemStructMembers));
            }
        }
    }
    public class ItemSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.Item>
    {
        public ItemSupport() : base(
            new Implementation.ItemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemSupport, CorpusCollectionShapes.Item>();
    }

    namespace Implementation
    {

        public struct RowUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.Row>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusCollectionShapes.Row sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (3));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int>(dimension: (3), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.Row sample, bool keysOnly = false)
            {
                Value.ToNative<int>(sample.Value, dimension: (3));
            }
        }

        internal class RowPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.Row, RowUnmanaged>
        {
            internal RowPlugin() : base("CorpusCollectionShapes.Row", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<RowUnmanaged>(
                    dtf,
                    "Row",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {3}
                ));
            }
        }
    }
    public class RowSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.Row>
    {
        public RowSupport() : base(
            new Implementation.RowPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RowPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RowSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RowSupport, CorpusCollectionShapes.Row>();
    }

    namespace Implementation
    {

        public struct BoundedLongsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.BoundedLongs>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusCollectionShapes.BoundedLongs sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: ((int)4), absoluteMax: ((int)4), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.BoundedLongs sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class BoundedLongsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.BoundedLongs, BoundedLongsUnmanaged>
        {
            internal BoundedLongsPlugin() : base("CorpusCollectionShapes.BoundedLongs", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedLongsUnmanaged>(
                    dtf,
                    "BoundedLongs",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)4)));
            }
        }
    }
    public class BoundedLongsSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.BoundedLongs>
    {
        public BoundedLongsSupport() : base(
            new Implementation.BoundedLongsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedLongsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedLongsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedLongsSupport, CorpusCollectionShapes.BoundedLongs>();
    }

    namespace Implementation
    {

        public struct RowsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.Rows>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(optionalsOnly);
            }

            public void FromNative(CorpusCollectionShapes.Rows sample, bool keysOnly = false)
            {

                Value.FromNative<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusCollectionShapes.Row , CorpusCollectionShapes.Implementation.RowUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.Rows sample, bool keysOnly = false)
            {
                Value.ToNative<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.Value);
            }
        }

        internal class RowsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.Rows, RowsUnmanaged>
        {
            internal RowsPlugin() : base("CorpusCollectionShapes.Rows", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<RowsUnmanaged>(
                    dtf,
                    "Rows",
                    tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionShapes.RowSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)));
            }
        }
    }
    public class RowsSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.Rows>
    {
        public RowsSupport() : base(
            new Implementation.RowsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RowsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RowsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RowsSupport, CorpusCollectionShapes.Rows>();
    }

    namespace Implementation
    {

        public struct ItemsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.Items>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<CorpusCollectionShapes.Item, CorpusCollectionShapes.Implementation.ItemUnmanaged>(optionalsOnly);
            }

            public void FromNative(CorpusCollectionShapes.Items sample, bool keysOnly = false)
            {

                Value.FromNative<CorpusCollectionShapes.Item, CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusCollectionShapes.Item , CorpusCollectionShapes.Implementation.ItemUnmanaged >(max: ((int)3), absoluteMax: ((int)3), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.Items sample, bool keysOnly = false)
            {
                Value.ToNative<CorpusCollectionShapes.Item, CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.Value);
            }
        }

        internal class ItemsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.Items, ItemsUnmanaged>
        {
            internal ItemsPlugin() : base("CorpusCollectionShapes.Items", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ItemsUnmanaged>(
                    dtf,
                    "Items",
                    tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionShapes.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)3)));
            }
        }
    }
    public class ItemsSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.Items>
    {
        public ItemsSupport() : base(
            new Implementation.ItemsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemsSupport, CorpusCollectionShapes.Items>();
    }

    namespace Implementation
    {

        public struct NamesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.Names>
        {

            private NativeStringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusCollectionShapes.Names sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: ((int)4), absoluteMax: ((int)4), maxStrLen: ((int) 32), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.Names sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 32));
            }
        }

        internal class NamesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.Names, NamesUnmanaged>
        {
            internal NamesPlugin() : base("CorpusCollectionShapes.Names", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NamesUnmanaged>(
                    dtf,
                    "Names",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 32)), ((int)4)));
            }
        }
    }
    public class NamesSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.Names>
    {
        public NamesSupport() : base(
            new Implementation.NamesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NamesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NamesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NamesSupport, CorpusCollectionShapes.Names>();
    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionShapes.Sample>
        {

            private NativeSeq unboundedItems;
            private CorpusCollectionShapes.Implementation.ItemsUnmanaged boundedItems;
            private NativeSeq nestedSequences;
            private NativeManagedArray rows;
            private NativeSeq sequenceOfArrays;
            private NativeSeq sequenceOfArrayAliases;
            private CorpusCollectionShapes.Implementation.NamesUnmanaged names;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                unboundedItems.Destroy<CorpusCollectionShapes.Item, CorpusCollectionShapes.Implementation.ItemUnmanaged>(optionalsOnly);
                boundedItems.Destroy(optionalsOnly);
                nestedSequences.Destroy<CorpusCollectionShapes.BoundedLongs, CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged>(optionalsOnly);
                rows.Destroy<CorpusCollectionShapes.Row,CorpusCollectionShapes.Implementation.RowUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
                sequenceOfArrays.Destroy<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(optionalsOnly);
                sequenceOfArrayAliases.Destroy<CorpusCollectionShapes.Rows, CorpusCollectionShapes.Implementation.RowsUnmanaged>(optionalsOnly);
                names.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusCollectionShapes.Sample sample, bool keysOnly = false)
            {

                unboundedItems.FromNative<CorpusCollectionShapes.Item, CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.FromNative(sample.boundedItems, keysOnly: false);
                nestedSequences.FromNative<CorpusCollectionShapes.BoundedLongs, CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged>(sample.nestedSequences);
                rows.FromNative<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.FromNative<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
                sequenceOfArrayAliases.FromNative<CorpusCollectionShapes.Rows, CorpusCollectionShapes.Implementation.RowsUnmanaged>(sample.sequenceOfArrayAliases);
                names.FromNative(sample.names, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                unboundedItems.Initialize<CorpusCollectionShapes.Item , CorpusCollectionShapes.Implementation.ItemUnmanaged >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
                boundedItems.Initialize(allocatePointers, allocateMemory);
                nestedSequences.Initialize<CorpusCollectionShapes.BoundedLongs , CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                rows.Initialize<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
                sequenceOfArrays.Initialize<CorpusCollectionShapes.Row , CorpusCollectionShapes.Implementation.RowUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                sequenceOfArrayAliases.Initialize<CorpusCollectionShapes.Rows , CorpusCollectionShapes.Implementation.RowsUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                names.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusCollectionShapes.Sample sample, bool keysOnly = false)
            {
                unboundedItems.ToNative<CorpusCollectionShapes.Item, CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.ToNative(sample.boundedItems, keysOnly: false);
                nestedSequences.ToNative<CorpusCollectionShapes.BoundedLongs, CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged>(sample.nestedSequences);
                rows.ToNative<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.ToNative<CorpusCollectionShapes.Row, CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
                sequenceOfArrayAliases.ToNative<CorpusCollectionShapes.Rows, CorpusCollectionShapes.Implementation.RowsUnmanaged>(sample.sequenceOfArrayAliases);
                names.ToNative(sample.names, keysOnly: false);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionShapes.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusCollectionShapes.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("unboundedItems", tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionShapes.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)100)), id: 0),
                    new StructMember("boundedItems", CorpusCollectionShapes.ItemsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("nestedSequences", tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionShapes.BoundedLongsSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 2),
                    new StructMember("rows", tsf.CreateArrayWithAccessInfo<CorpusCollectionShapes.Implementation.RowUnmanaged>(dtf, CorpusCollectionShapes.RowSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}
                    ), id: 3),
                    new StructMember("sequenceOfArrays", tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionShapes.RowSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 4),
                    new StructMember("sequenceOfArrayAliases", tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionShapes.RowsSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 5),
                    new StructMember("names", CorpusCollectionShapes.NamesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 6)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollectionShapes::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionShapes.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusCollectionShapes.Sample>();
    }

} // namespace CorpusCollectionShapes

