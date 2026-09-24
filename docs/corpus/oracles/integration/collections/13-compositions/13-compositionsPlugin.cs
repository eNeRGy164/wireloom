/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 13-compositions.idl
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

namespace CorpusIntegrationCollections
{

    namespace Implementation
    {

        public struct ItemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusIntegrationCollections.Item>
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

            public void FromNative(global::CorpusIntegrationCollections.Item sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.label = label.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                label.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusIntegrationCollections.Item sample, bool keysOnly = false)
            {
                id = sample.id;
                label.ToNative(sample.label, ((int) 16));
            }
        }

        internal class ItemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusIntegrationCollections.Item, ItemUnmanaged>
        {

            internal ItemPlugin() : base("global::CorpusIntegrationCollections.Item", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    .WithName("CorpusIntegrationCollections::Item")
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
    public class ItemSupport : Rti.Dds.Topics.TypeSupport<global::CorpusIntegrationCollections.Item>
    {
        public ItemSupport() : base(
            new Implementation.ItemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemSupport, global::CorpusIntegrationCollections.Item>();

    }

    namespace Implementation
    {

        public struct RowUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusIntegrationCollections.Row>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusIntegrationCollections.Row sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (2));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int>(dimension: (2), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusIntegrationCollections.Row sample, bool keysOnly = false)
            {
                Value.ToNative<int>(sample.Value, dimension: (2));
            }
        }

        internal class RowPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusIntegrationCollections.Row, RowUnmanaged>
        {

            internal RowPlugin() : base("global::CorpusIntegrationCollections.Row", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<RowUnmanaged>(
                    dtf,
                    "Row",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2}));
                return aliasType;

            }
        }
    }
    public class RowSupport : Rti.Dds.Topics.TypeSupport<global::CorpusIntegrationCollections.Row>
    {
        public RowSupport() : base(
            new Implementation.RowPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RowPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RowSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RowSupport, global::CorpusIntegrationCollections.Row>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusIntegrationCollections.Sample>
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
                unboundedItems.Destroy<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(optionalsOnly);
                boundedItems.Destroy<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(optionalsOnly);
                aggregateArray.Destroy<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
                rows.Destroy<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
                sequenceOfArrays.Destroy<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusIntegrationCollections.Sample sample, bool keysOnly = false)
            {

                unboundedItems.FromNative<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.FromNative<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.boundedItems);
                aggregateArray.FromNative<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.aggregateArray, keysOnly: keysOnly, dimension: (2));
                rows.FromNative<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.FromNative<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                unboundedItems.Initialize<global::CorpusIntegrationCollections.Item , global::CorpusIntegrationCollections.Implementation.ItemUnmanaged >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
                boundedItems.Initialize<global::CorpusIntegrationCollections.Item , global::CorpusIntegrationCollections.Implementation.ItemUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                aggregateArray.Initialize<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
                rows.Initialize<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
                sequenceOfArrays.Initialize<global::CorpusIntegrationCollections.Row , global::CorpusIntegrationCollections.Implementation.RowUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusIntegrationCollections.Sample sample, bool keysOnly = false)
            {
                unboundedItems.ToNative<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.unboundedItems);
                boundedItems.ToNative<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.boundedItems);
                aggregateArray.ToNative<global::CorpusIntegrationCollections.Item, global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(sample.aggregateArray, keysOnly: keysOnly, dimension: (2));
                rows.ToNative<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.rows, keysOnly: keysOnly, dimension: (2));
                sequenceOfArrays.ToNative<global::CorpusIntegrationCollections.Row, global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(sample.sequenceOfArrays);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusIntegrationCollections.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusIntegrationCollections.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("unboundedItems", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusIntegrationCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)100)), id: 0),
                    new StructMember("boundedItems", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusIntegrationCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 1),
                    new StructMember("aggregateArray", tsf.CreateArrayWithAccessInfo<global::CorpusIntegrationCollections.Implementation.ItemUnmanaged>(dtf, global::CorpusIntegrationCollections.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}), id: 2),
                    new StructMember("rows", tsf.CreateArrayWithAccessInfo<global::CorpusIntegrationCollections.Implementation.RowUnmanaged>(dtf, global::CorpusIntegrationCollections.RowSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}), id: 3),
                    new StructMember("sequenceOfArrays", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusIntegrationCollections.RowSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)), id: 4)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIntegrationCollections::Sample")
                    .AddMembers(SampleStructMembers));

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusIntegrationCollections.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusIntegrationCollections.Sample>();

    }

} // namespace CorpusIntegrationCollections

