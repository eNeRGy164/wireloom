/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-shapes.idl
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

namespace CorpusCollectionShapes
{

    namespace Implementation
    {

        public struct ItemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.Item>
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

            public void FromNative(global::CorpusCollectionShapes.Item sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                label.Initialize(size: ((int) 12), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.Item sample, bool keysOnly = false)
            {
                id = sample.id;
                label.ToNative(sample.label, ((int) 12));
            }
        }

        internal class ItemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.Item, ItemUnmanaged>
        {

            internal ItemPlugin() : base("global::CorpusCollectionShapes.Item", isKeyed: false, CreateDynamicType(isPublic: false))
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

                DynamicType result = tsf.CreateTypeWithAccessInfo<ItemUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollectionShapes::Item")
                    .AddMembers(ItemStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        1,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ItemSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.Item>
    {
        public ItemSupport() : base(
            new Implementation.ItemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemSupport, global::CorpusCollectionShapes.Item>();

    }

    namespace Implementation
    {

        public struct RowUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.Row>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusCollectionShapes.Row sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (3));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int>(dimension: (3), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.Row sample, bool keysOnly = false)
            {
                Value.ToNative<int>(sample.Value, dimension: (3));
            }
        }

        internal class RowPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.Row, RowUnmanaged>
        {

            internal RowPlugin() : base("global::CorpusCollectionShapes.Row", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<RowUnmanaged>(
                    dtf,
                    "Row",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {3}));
                return aliasType;

            }
        }
    }
    public class RowSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.Row>
    {
        public RowSupport() : base(
            new Implementation.RowPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RowPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RowSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RowSupport, global::CorpusCollectionShapes.Row>();

    }

    namespace Implementation
    {

        public struct BoundedLongsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.BoundedLongs>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusCollectionShapes.BoundedLongs sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: ((int)4), absoluteMax: ((int)4), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.BoundedLongs sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class BoundedLongsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.BoundedLongs, BoundedLongsUnmanaged>
        {

            internal BoundedLongsPlugin() : base("global::CorpusCollectionShapes.BoundedLongs", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<BoundedLongsUnmanaged>(
                    dtf,
                    "BoundedLongs",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)4)));
                return aliasType;

            }
        }
    }
    public class BoundedLongsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.BoundedLongs>
    {
        public BoundedLongsSupport() : base(
            new Implementation.BoundedLongsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedLongsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedLongsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedLongsSupport, global::CorpusCollectionShapes.BoundedLongs>();

    }

    namespace Implementation
    {

        public struct RowsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.Rows>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusCollectionShapes.Rows sample, bool keysOnly = false)
            {

                Value.FromNative<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<global::CorpusCollectionShapes.Row , global::CorpusCollectionShapes.Implementation.RowUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.Rows sample, bool keysOnly = false)
            {
                Value.ToNative<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.Value);
            }
        }

        internal class RowsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.Rows, RowsUnmanaged>
        {

            internal RowsPlugin() : base("global::CorpusCollectionShapes.Rows", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<RowsUnmanaged>(
                    dtf,
                    "Rows",
                    tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollectionShapes.RowSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)));
                return aliasType;

            }
        }
    }
    public class RowsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.Rows>
    {
        public RowsSupport() : base(
            new Implementation.RowsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RowsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RowsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RowsSupport, global::CorpusCollectionShapes.Rows>();

    }

    namespace Implementation
    {

        public struct ItemsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.Items>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<global::CorpusCollectionShapes.Item, global::CorpusCollectionShapes.Implementation.ItemUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusCollectionShapes.Items sample, bool keysOnly = false)
            {

                Value.FromNative<global::CorpusCollectionShapes.Item, global::CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<global::CorpusCollectionShapes.Item , global::CorpusCollectionShapes.Implementation.ItemUnmanaged >(max: ((int)3), absoluteMax: ((int)3), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.Items sample, bool keysOnly = false)
            {
                Value.ToNative<global::CorpusCollectionShapes.Item, global::CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.Value);
            }
        }

        internal class ItemsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.Items, ItemsUnmanaged>
        {

            internal ItemsPlugin() : base("global::CorpusCollectionShapes.Items", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ItemsUnmanaged>(
                    dtf,
                    "Items",
                    tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollectionShapes.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)3)));
                return aliasType;

            }
        }
    }
    public class ItemsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.Items>
    {
        public ItemsSupport() : base(
            new Implementation.ItemsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemsSupport, global::CorpusCollectionShapes.Items>();

    }

    namespace Implementation
    {

        public struct NamesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.Names>
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

            public void FromNative(global::CorpusCollectionShapes.Names sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: ((int)4), absoluteMax: ((int)4), maxStrLen: ((int) 32), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.Names sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 32));
            }
        }

        internal class NamesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.Names, NamesUnmanaged>
        {

            internal NamesPlugin() : base("global::CorpusCollectionShapes.Names", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 32));
                var aliasType = tsf.CreateAliasWithAccessInfo<NamesUnmanaged>(
                    dtf,
                    "Names",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 32)), ((int)4)));
                return aliasType;

            }
        }
    }
    public class NamesSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.Names>
    {
        public NamesSupport() : base(
            new Implementation.NamesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NamesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NamesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NamesSupport, global::CorpusCollectionShapes.Names>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollectionShapes.Sample>
        {

            private NativeSeq unboundedItems;
            private global::CorpusCollectionShapes.Implementation.ItemsUnmanaged boundedItems;
            private NativeSeq nestedSequences;
            private NativeManagedArray rows;
            private NativeSeq sequenceOfArrays;
            private NativeSeq sequenceOfArrayAliases;
            private global::CorpusCollectionShapes.Implementation.NamesUnmanaged names;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                unboundedItems.Destroy<global::CorpusCollectionShapes.Item, global::CorpusCollectionShapes.Implementation.ItemUnmanaged>(optionalsOnly);
                boundedItems.Destroy(optionalsOnly);
                nestedSequences.Destroy<global::CorpusCollectionShapes.BoundedLongs, global::CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged>(optionalsOnly);
                rows.Destroy<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
                sequenceOfArrays.Destroy<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(optionalsOnly);
                sequenceOfArrayAliases.Destroy<global::CorpusCollectionShapes.Rows, global::CorpusCollectionShapes.Implementation.RowsUnmanaged>(optionalsOnly);
                names.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusCollectionShapes.Sample sample, bool keysOnly = false)
            {

                unboundedItems.FromNative<global::CorpusCollectionShapes.Item, global::CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.FromNative(sample.boundedItems, keysOnly: false);
                nestedSequences.FromNative<global::CorpusCollectionShapes.BoundedLongs, global::CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged>(sample.nestedSequences);
                rows.FromNative<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.FromNative<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
                sequenceOfArrayAliases.FromNative<global::CorpusCollectionShapes.Rows, global::CorpusCollectionShapes.Implementation.RowsUnmanaged>(sample.sequenceOfArrayAliases);
                names.FromNative(sample.names, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                unboundedItems.Initialize<global::CorpusCollectionShapes.Item , global::CorpusCollectionShapes.Implementation.ItemUnmanaged >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
                boundedItems.Initialize(allocatePointers, allocateMemory);
                nestedSequences.Initialize<global::CorpusCollectionShapes.BoundedLongs , global::CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                rows.Initialize<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
                sequenceOfArrays.Initialize<global::CorpusCollectionShapes.Row , global::CorpusCollectionShapes.Implementation.RowUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                sequenceOfArrayAliases.Initialize<global::CorpusCollectionShapes.Rows , global::CorpusCollectionShapes.Implementation.RowsUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                names.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusCollectionShapes.Sample sample, bool keysOnly = false)
            {
                unboundedItems.ToNative<global::CorpusCollectionShapes.Item, global::CorpusCollectionShapes.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.ToNative(sample.boundedItems, keysOnly: false);
                nestedSequences.ToNative<global::CorpusCollectionShapes.BoundedLongs, global::CorpusCollectionShapes.Implementation.BoundedLongsUnmanaged>(sample.nestedSequences);
                rows.ToNative<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.ToNative<global::CorpusCollectionShapes.Row, global::CorpusCollectionShapes.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
                sequenceOfArrayAliases.ToNative<global::CorpusCollectionShapes.Rows, global::CorpusCollectionShapes.Implementation.RowsUnmanaged>(sample.sequenceOfArrayAliases);
                names.ToNative(sample.names, keysOnly: false);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollectionShapes.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusCollectionShapes.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("unboundedItems", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollectionShapes.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)100)), id: 0),
                    new StructMember("boundedItems", global::CorpusCollectionShapes.ItemsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("nestedSequences", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollectionShapes.BoundedLongsSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 2),
                    new StructMember("rows", tsf.CreateArrayWithAccessInfo<global::CorpusCollectionShapes.Implementation.RowUnmanaged>(dtf, global::CorpusCollectionShapes.RowSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}), id: 3),
                    new StructMember("sequenceOfArrays", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollectionShapes.RowSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 4),
                    new StructMember("sequenceOfArrayAliases", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollectionShapes.RowsSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 5),
                    new StructMember("names", global::CorpusCollectionShapes.NamesSupport.Instance.GetDynamicTypeInternal(isPublic), id: 6)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollectionShapes::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollectionShapes.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusCollectionShapes.Sample>();

    }

} // namespace CorpusCollectionShapes

