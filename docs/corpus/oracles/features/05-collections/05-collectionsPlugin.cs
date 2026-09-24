/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 05-collections.idl
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

namespace CorpusCollections
{

    namespace Implementation
    {

        public struct ItemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollections.Item>
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

            public void FromNative(global::CorpusCollections.Item sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                label.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollections.Item sample, bool keysOnly = false)
            {
                id = sample.id;
                label.ToNative(sample.label, ((int) 16));
            }
        }

        internal class ItemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollections.Item, ItemUnmanaged>
        {

            internal ItemPlugin() : base("global::CorpusCollections.Item", isKeyed: false, CreateDynamicType(isPublic: false))
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

                DynamicType result = tsf.CreateTypeWithAccessInfo<ItemUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollections::Item")
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
    public class ItemSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollections.Item>
    {
        public ItemSupport() : base(
            new Implementation.ItemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemSupport, global::CorpusCollections.Item>();

    }

    namespace Implementation
    {

        public struct BoundedLongsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollections.BoundedLongs>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusCollections.BoundedLongs sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: ((int)4), absoluteMax: ((int)4), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollections.BoundedLongs sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class BoundedLongsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollections.BoundedLongs, BoundedLongsUnmanaged>
        {

            internal BoundedLongsPlugin() : base("global::CorpusCollections.BoundedLongs", isKeyed: false, CreateDynamicType(isPublic: false))
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
    public class BoundedLongsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollections.BoundedLongs>
    {
        public BoundedLongsSupport() : base(
            new Implementation.BoundedLongsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedLongsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedLongsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedLongsSupport, global::CorpusCollections.BoundedLongs>();

    }

    namespace Implementation
    {

        public struct CoordinateGridUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollections.CoordinateGrid>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusCollections.CoordinateGrid sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (2)*(3));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int>(dimension: (2)*(3), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusCollections.CoordinateGrid sample, bool keysOnly = false)
            {
                Value.ToNative<int>(sample.Value, dimension: (2)*(3));
            }
        }

        internal class CoordinateGridPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollections.CoordinateGrid, CoordinateGridUnmanaged>
        {

            internal CoordinateGridPlugin() : base("global::CorpusCollections.CoordinateGrid", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<CoordinateGridUnmanaged>(
                    dtf,
                    "CoordinateGrid",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2, 3}));
                return aliasType;

            }
        }
    }
    public class CoordinateGridSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollections.CoordinateGrid>
    {
        public CoordinateGridSupport() : base(
            new Implementation.CoordinateGridPlugin(),
            new Lazy<DynamicType>(() =>Implementation.CoordinateGridPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static CoordinateGridSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<CoordinateGridSupport, global::CorpusCollections.CoordinateGrid>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCollections.Sample>
        {

            private NativeUnmanagedArray values;
            private NativeSeq unbounded;
            private global::CorpusCollections.Implementation.BoundedLongsUnmanaged bounded;
            private NativeSeq items;
            private global::CorpusCollections.Implementation.CoordinateGridUnmanaged grid;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                values.Destroy(optionalsOnly);
                unbounded.Destroy(optionalsOnly);
                bounded.Destroy(optionalsOnly);
                items.Destroy<global::CorpusCollections.Item, global::CorpusCollections.Implementation.ItemUnmanaged>(optionalsOnly);
                grid.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusCollections.Sample sample, bool keysOnly = false)
            {

                values.FromNative(sample.values, dimension: (2)*(3));
                unbounded.FromNative((Sequence<int>) sample.unbounded);
                bounded.FromNative(sample.bounded, keysOnly: false);
                items.FromNative<global::CorpusCollections.Item, global::CorpusCollections.Implementation.ItemUnmanaged>(sample.items);
                grid.FromNative(sample.grid, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize<int>(dimension: (2)*(3), allocateMemory: allocateMemory);
                unbounded.Initialize<int >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
                bounded.Initialize(allocatePointers, allocateMemory);
                items.Initialize<global::CorpusCollections.Item , global::CorpusCollections.Implementation.ItemUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                grid.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusCollections.Sample sample, bool keysOnly = false)
            {
                values.ToNative<int>(sample.values, dimension: (2)*(3));
                unbounded.ToNative((Sequence<int>) sample.unbounded);
                bounded.ToNative(sample.bounded, keysOnly: false);
                items.ToNative<global::CorpusCollections.Item, global::CorpusCollections.Implementation.ItemUnmanaged>(sample.items);
                grid.ToNative(sample.grid, keysOnly: false);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCollections.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusCollections.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("values", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2, 3}), id: 0),
                    new StructMember("unbounded", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)100)), id: 1),
                    new StructMember("bounded", global::CorpusCollections.BoundedLongsSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                    new StructMember("items", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 3),
                    new StructMember("grid", global::CorpusCollections.CoordinateGridSupport.Instance.GetDynamicTypeInternal(isPublic), id: 4)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollections::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCollections.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusCollections.Sample>();

    }

} // namespace CorpusCollections

