/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-scopes.idl
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

namespace CorpusScopes
{

    namespace Nested
    {

        namespace Implementation
        {

            public struct PointUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusScopes.Nested.Point>
            {

                private int x;
                private int y;

                public void Destroy(bool optionalsOnly)
                {
                }

                public void FromNative(global::CorpusScopes.Nested.Point sample, bool keysOnly = false)
                {

                    sample.x = x;
                    sample.y = y;
                }

                public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
                {
                    x = (int) (0);
                    y = (int) (0);
                }

                public void ToNative(global::CorpusScopes.Nested.Point sample, bool keysOnly = false)
                {
                    x = sample.x;
                    y = sample.y;
                }
            }

            internal class PointPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusScopes.Nested.Point, PointUnmanaged>
            {

                internal PointPlugin() : base("global::CorpusScopes.Nested.Point", isKeyed: false, CreateDynamicType(isPublic: false))
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

                    DynamicType result = tsf.CreateTypeWithAccessInfo<PointUnmanaged>(
                        dtf.BuildStruct()
                        .WithExtensibility(ExtensibilityKind.Extensible)
                        .WithName("CorpusScopes::Nested::Point")
                        .AddMembers(PointStructMembers));

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
                            1,
                            annotations);
                    }

                    return result;

                }
            }
        }
        public class PointSupport : Rti.Dds.Topics.TypeSupport<global::CorpusScopes.Nested.Point>
        {
            public PointSupport() : base(
                new Implementation.PointPlugin(),
                new Lazy<DynamicType>(() =>Implementation.PointPlugin.CreateDynamicType(isPublic: true)))
            {
            }

            public static PointSupport Instance { get; } =
            ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PointSupport, global::CorpusScopes.Nested.Point>();

        }

    } // namespace Nested

    namespace Implementation
    {

        public struct KeywordRecordUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusScopes.KeywordRecord>
        {

            private int @event;
            private global::CorpusScopes.Nested.Implementation.PointUnmanaged relative;
            private global::CorpusScopes.Nested.Implementation.PointUnmanaged absolute;
            private NativeSeq values;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                relative.Destroy(optionalsOnly);
                absolute.Destroy(optionalsOnly);
                values.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusScopes.KeywordRecord sample, bool keysOnly = false)
            {

                sample.@event = @event;
                relative.FromNative(sample.relative, keysOnly: false);
                absolute.FromNative(sample.absolute, keysOnly: false);
                values.FromNative((Sequence<int>) sample.values);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                @event = (int) (0);
                relative.Initialize(allocatePointers, allocateMemory);
                absolute.Initialize(allocatePointers, allocateMemory);
                values.Initialize<int >(max: ((int)(CorpusScopes.Bound.Value)), absoluteMax: ((int)(CorpusScopes.Bound.Value)), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusScopes.KeywordRecord sample, bool keysOnly = false)
            {
                @event = sample.@event;
                relative.ToNative(sample.relative, keysOnly: false);
                absolute.ToNative(sample.absolute, keysOnly: false);
                values.ToNative((Sequence<int>) sample.values);
            }
        }

        internal class KeywordRecordPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusScopes.KeywordRecord, KeywordRecordUnmanaged>
        {

            internal KeywordRecordPlugin() : base("global::CorpusScopes.KeywordRecord", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // KeywordRecord struct
                var KeywordRecordStructMembers = new StructMember[]
                {
                    new StructMember("event", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("relative", global::CorpusScopes.Nested.PointSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("absolute", global::CorpusScopes.Nested.PointSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                    new StructMember("values", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)(CorpusScopes.Bound.Value))), id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<KeywordRecordUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusScopes::KeywordRecord")
                    .AddMembers(KeywordRecordStructMembers));

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

                return result;

            }
        }
    }
    public class KeywordRecordSupport : Rti.Dds.Topics.TypeSupport<global::CorpusScopes.KeywordRecord>
    {
        public KeywordRecordSupport() : base(
            new Implementation.KeywordRecordPlugin(),
            new Lazy<DynamicType>(() =>Implementation.KeywordRecordPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static KeywordRecordSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<KeywordRecordSupport, global::CorpusScopes.KeywordRecord>();

    }

} // namespace CorpusScopes

namespace CorpusScopes
{

    namespace Implementation
    {

        public struct ReopenedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusScopes.Reopened>
        {

            private global::CorpusScopes.Implementation.KeywordRecordUnmanaged value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusScopes.Reopened sample, bool keysOnly = false)
            {

                value.FromNative(sample.value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusScopes.Reopened sample, bool keysOnly = false)
            {
                value.ToNative(sample.value, keysOnly: false);
            }
        }

        internal class ReopenedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusScopes.Reopened, ReopenedUnmanaged>
        {

            internal ReopenedPlugin() : base("global::CorpusScopes.Reopened", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Reopened struct
                var ReopenedStructMembers = new StructMember[]
                {
                    new StructMember("value", global::CorpusScopes.KeywordRecordSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ReopenedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusScopes::Reopened")
                    .AddMembers(ReopenedStructMembers));

                return result;

            }
        }
    }
    public class ReopenedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusScopes.Reopened>
    {
        public ReopenedSupport() : base(
            new Implementation.ReopenedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ReopenedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ReopenedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ReopenedSupport, global::CorpusScopes.Reopened>();

    }

} // namespace CorpusScopes

