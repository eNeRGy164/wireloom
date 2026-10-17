/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-alias-composition.idl
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

namespace CorpusAggregateComposition
{

    namespace Implementation
    {

        public struct IdentifierUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregateComposition.Identifier>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusAggregateComposition.Identifier sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) (0);
            }

            public void ToNative(global::CorpusAggregateComposition.Identifier sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class IdentifierPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregateComposition.Identifier, IdentifierUnmanaged>
        {

            internal IdentifierPlugin() : base("global::CorpusAggregateComposition.Identifier", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<IdentifierUnmanaged>(
                    dtf,
                    "Identifier",
                    dtf.GetPrimitiveType<int>());
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
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class IdentifierSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregateComposition.Identifier>
    {
        public IdentifierSupport() : base(
            new Implementation.IdentifierPlugin(),
            new Lazy<DynamicType>(() =>Implementation.IdentifierPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static IdentifierSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IdentifierSupport, global::CorpusAggregateComposition.Identifier>();

    }

    namespace Implementation
    {
        internal class StatePlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public StatePlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var enumType = dtf.BuildEnum()
                .WithName("CorpusAggregateComposition::State")
                .AddMember(new EnumMember("idle", 0))
                .AddMember(new EnumMember("run", 1))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 0;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    enumType.SetAnnotations(
                        annotations);
                }
                return enumType;
            }
        }
    }

    public class StateSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregateComposition.State>
    {
        public StateSupport() : base(
            new Implementation.StatePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StatePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StateSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StateSupport, global::CorpusAggregateComposition.State>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregateComposition.Sample>
        {

            private int id;
            private global::CorpusAggregateComposition.State state;
            private NativeSeq values;

            public void Destroy(bool optionalsOnly)
            {
                values.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusAggregateComposition.Sample sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.state = state;
                values.FromNative((Sequence<float>) sample.values);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                state = (global::CorpusAggregateComposition.State) (0);
                values.Initialize<float >(max: ((int)(CorpusAggregateComposition.Max.Value)), absoluteMax: ((int)(CorpusAggregateComposition.Max.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusAggregateComposition.Sample sample, bool keysOnly = false)
            {
                id = sample.id;
                state = sample.state;
                values.ToNative((Sequence<float>) sample.values);
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregateComposition.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusAggregateComposition.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("id", global::CorpusAggregateComposition.IdentifierSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("state", global::CorpusAggregateComposition.StateSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<float>(), ((int)(CorpusAggregateComposition.Max.Value))), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregateComposition::Sample")
                    .AddMembers(SampleStructMembers));

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
                    defaultValueParam.EnumValue = (int) 0;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
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
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregateComposition.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusAggregateComposition.Sample>();

    }

    namespace Implementation
    {

        public struct NamedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregateComposition.Named>
        {

            private global::CorpusAggregateComposition.Implementation.SampleUnmanaged sample;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                sample.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusAggregateComposition.Named sample, bool keysOnly = false)
            {

                sample.FromNative(sample.sample, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                sample.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusAggregateComposition.Named sample, bool keysOnly = false)
            {
                sample.ToNative(sample.sample, keysOnly: false);
            }
        }

        internal class NamedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregateComposition.Named, NamedUnmanaged>
        {

            internal NamedPlugin() : base("global::CorpusAggregateComposition.Named", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Named struct
                var NamedStructMembers = new StructMember[]
                {
                    new StructMember("sample", global::CorpusAggregateComposition.SampleSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<NamedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregateComposition::Named")
                    .AddMembers(NamedStructMembers));

                return result;

            }
        }
    }
    public class NamedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregateComposition.Named>
    {
        public NamedSupport() : base(
            new Implementation.NamedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NamedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NamedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NamedSupport, global::CorpusAggregateComposition.Named>();

    }

} // namespace CorpusAggregateComposition

