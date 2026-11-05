/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-alias-collections.idl
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

namespace CorpusCollectionAliases
{

    namespace Implementation
    {

        public struct LongSequenceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionAliases.LongSequence>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusCollectionAliases.LongSequence sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<int>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<int >(max: ((int)3), absoluteMax: ((int)3), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionAliases.LongSequence sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<int>) sample.Value);
            }
        }

        internal class LongSequencePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionAliases.LongSequence, LongSequenceUnmanaged>
        {
            internal LongSequencePlugin() : base("CorpusCollectionAliases.LongSequence", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<LongSequenceUnmanaged>(
                    dtf,
                    "LongSequence",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)3)));
            }
        }
    }
    public class LongSequenceSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionAliases.LongSequence>
    {
        public LongSequenceSupport() : base(
            new Implementation.LongSequencePlugin(),
            new Lazy<DynamicType>(() =>Implementation.LongSequencePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static LongSequenceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<LongSequenceSupport, CorpusCollectionAliases.LongSequence>();
    }

    namespace Implementation
    {

        public struct NestedSequenceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionAliases.NestedSequence>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy<CorpusCollectionAliases.LongSequence, CorpusCollectionAliases.Implementation.LongSequenceUnmanaged>(optionalsOnly);
            }

            public void FromNative(CorpusCollectionAliases.NestedSequence sample, bool keysOnly = false)
            {

                Value.FromNative<CorpusCollectionAliases.LongSequence, CorpusCollectionAliases.Implementation.LongSequenceUnmanaged>(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusCollectionAliases.LongSequence , CorpusCollectionAliases.Implementation.LongSequenceUnmanaged >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusCollectionAliases.NestedSequence sample, bool keysOnly = false)
            {
                Value.ToNative<CorpusCollectionAliases.LongSequence, CorpusCollectionAliases.Implementation.LongSequenceUnmanaged>(sample.Value);
            }
        }

        internal class NestedSequencePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionAliases.NestedSequence, NestedSequenceUnmanaged>
        {
            internal NestedSequencePlugin() : base("CorpusCollectionAliases.NestedSequence", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NestedSequenceUnmanaged>(
                    dtf,
                    "NestedSequence",
                    tsf.CreateSequenceWithAccessInfo(dtf, CorpusCollectionAliases.LongSequenceSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)2)));
            }
        }
    }
    public class NestedSequenceSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionAliases.NestedSequence>
    {
        public NestedSequenceSupport() : base(
            new Implementation.NestedSequencePlugin(),
            new Lazy<DynamicType>(() =>Implementation.NestedSequencePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NestedSequenceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NestedSequenceSupport, CorpusCollectionAliases.NestedSequence>();
    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusCollectionAliases.Sample>
        {

            private CorpusCollectionAliases.Implementation.NestedSequenceUnmanaged values;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                values.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusCollectionAliases.Sample sample, bool keysOnly = false)
            {

                values.FromNative(sample.values, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                values.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusCollectionAliases.Sample sample, bool keysOnly = false)
            {
                values.ToNative(sample.values, keysOnly: false);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusCollectionAliases.Sample, SampleUnmanaged>
        {
            internal SamplePlugin() : base("CorpusCollectionAliases.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("values", CorpusCollectionAliases.NestedSequenceSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)
                };

                return tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCollectionAliases::Sample")
                    .AddMembers(SampleStructMembers));
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<CorpusCollectionAliases.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, CorpusCollectionAliases.Sample>();
    }

} // namespace CorpusCollectionAliases

