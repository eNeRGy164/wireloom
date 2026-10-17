/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-names-constants.idl
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

namespace CorpusNames
{

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
                .WithName("CorpusNames::State")
                .AddMember(new EnumMember("Idle", 0))
                .AddMember(new EnumMember("Running", 1))
                .AddMember(new EnumMember("Complete", 2))
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

    public class StateSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNames.State>
    {
        public StateSupport() : base(
            new Implementation.StatePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StatePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StateSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StateSupport, global::CorpusNames.State>();

    }

    namespace Nested
    {

        namespace Implementation
        {

            public struct PointUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNames.Nested.Point>
            {

                private int x;
                private int y;

                public void Destroy(bool optionalsOnly)
                {
                }

                public void FromNative(global::CorpusNames.Nested.Point sample, bool keysOnly = false)
                {

                    sample.x = x;
                    sample.y = y;
                }

                public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
                {
                    x = (int) (0);
                    y = (int) (0);
                }

                public void ToNative(global::CorpusNames.Nested.Point sample, bool keysOnly = false)
                {
                    x = sample.x;
                    y = sample.y;
                }
            }

            internal class PointPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNames.Nested.Point, PointUnmanaged>
            {

                internal PointPlugin() : base("global::CorpusNames.Nested.Point", isKeyed: false, CreateDynamicType(isPublic: false))
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
                        .WithName("CorpusNames::Nested::Point")
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
        public class PointSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNames.Nested.Point>
        {
            public PointSupport() : base(
                new Implementation.PointPlugin(),
                new Lazy<DynamicType>(() =>Implementation.PointPlugin.CreateDynamicType(isPublic: true)))
            {
            }

            public static PointSupport Instance { get; } =
            ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<PointSupport, global::CorpusNames.Nested.Point>();

        }

    } // namespace Nested

    namespace Implementation
    {

        public struct KeywordRecordUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNames.KeywordRecord>
        {

            private int @event;
            private global::CorpusNames.State state;
            private global::CorpusNames.Nested.Implementation.PointUnmanaged point;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                point.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusNames.KeywordRecord sample, bool keysOnly = false)
            {

                sample.@event = @event;
                sample.state = state;
                point.FromNative(sample.point, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                @event = (int) (0);
                state = (global::CorpusNames.State) (0);
                point.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusNames.KeywordRecord sample, bool keysOnly = false)
            {
                @event = sample.@event;
                state = sample.state;
                point.ToNative(sample.point, keysOnly: false);
            }
        }

        internal class KeywordRecordPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNames.KeywordRecord, KeywordRecordUnmanaged>
        {

            internal KeywordRecordPlugin() : base("global::CorpusNames.KeywordRecord", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("state", global::CorpusNames.StateSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("point", global::CorpusNames.Nested.PointSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<KeywordRecordUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNames::KeywordRecord")
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
    public class KeywordRecordSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNames.KeywordRecord>
    {
        public KeywordRecordSupport() : base(
            new Implementation.KeywordRecordPlugin(),
            new Lazy<DynamicType>(() =>Implementation.KeywordRecordPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static KeywordRecordSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<KeywordRecordSupport, global::CorpusNames.KeywordRecord>();

    }

} // namespace CorpusNames

