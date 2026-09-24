/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enums-aliases.idl
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

namespace CorpusEnums
{

    namespace Implementation
    {
        internal class ColorPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public ColorPlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var enumType = dtf.BuildEnum()
                .WithName("CorpusEnums::Color")
                .AddMember(new EnumMember("RED", 0))
                .AddMember(new EnumMember("GREEN", 1))
                .AddMember(new EnumMember("BLUE", 2))
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

    public class ColorSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.Color>
    {
        public ColorSupport() : base(
            new Implementation.ColorPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSupport, global::CorpusEnums.Color>();

    }

    namespace Implementation
    {

        public struct ScalarUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnums.Scalar>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusEnums.Scalar sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) (0);
            }

            public void ToNative(global::CorpusEnums.Scalar sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ScalarPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnums.Scalar, ScalarUnmanaged>
        {

            internal ScalarPlugin() : base("global::CorpusEnums.Scalar", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ScalarUnmanaged>(
                    dtf,
                    "Scalar",
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
    public class ScalarSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.Scalar>
    {
        public ScalarSupport() : base(
            new Implementation.ScalarPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarSupport, global::CorpusEnums.Scalar>();

    }

    namespace Implementation
    {

        public struct ColorSequenceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnums.ColorSequence>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusEnums.ColorSequence sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<global::CorpusEnums.Color>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<global::CorpusEnums.Color >(max: ((int)3), absoluteMax: ((int)3), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusEnums.ColorSequence sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<global::CorpusEnums.Color>) sample.Value);
            }
        }

        internal class ColorSequencePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnums.ColorSequence, ColorSequenceUnmanaged>
        {

            internal ColorSequencePlugin() : base("global::CorpusEnums.ColorSequence", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ColorSequenceUnmanaged>(
                    dtf,
                    "ColorSequence",
                    tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)3)));
                return aliasType;

            }
        }
    }
    public class ColorSequenceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.ColorSequence>
    {
        public ColorSequenceSupport() : base(
            new Implementation.ColorSequencePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorSequencePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSequenceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSequenceSupport, global::CorpusEnums.ColorSequence>();

    }

    namespace Implementation
    {

        public struct ScalarAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnums.ScalarAlias>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusEnums.ScalarAlias sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) (0);
            }

            public void ToNative(global::CorpusEnums.ScalarAlias sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ScalarAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnums.ScalarAlias, ScalarAliasUnmanaged>
        {

            internal ScalarAliasPlugin() : base("global::CorpusEnums.ScalarAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ScalarAliasUnmanaged>(
                    dtf,
                    "ScalarAlias",
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
    public class ScalarAliasSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.ScalarAlias>
    {
        public ScalarAliasSupport() : base(
            new Implementation.ScalarAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarAliasSupport, global::CorpusEnums.ScalarAlias>();

    }

    namespace Implementation
    {

        public struct ColorAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnums.ColorAlias>
        {

            private global::CorpusEnums.Color Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusEnums.ColorAlias sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (global::CorpusEnums.Color) (0);
            }

            public void ToNative(global::CorpusEnums.ColorAlias sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ColorAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnums.ColorAlias, ColorAliasUnmanaged>
        {

            internal ColorAliasPlugin() : base("global::CorpusEnums.ColorAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ColorAliasUnmanaged>(
                    dtf,
                    "ColorAlias",
                    global::CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic));
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 0;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class ColorAliasSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.ColorAlias>
    {
        public ColorAliasSupport() : base(
            new Implementation.ColorAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorAliasSupport, global::CorpusEnums.ColorAlias>();

    }

    namespace Implementation
    {

        public struct ColorAlias2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnums.ColorAlias2>
        {

            private global::CorpusEnums.Color Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusEnums.ColorAlias2 sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (global::CorpusEnums.Color) (0);
            }

            public void ToNative(global::CorpusEnums.ColorAlias2 sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ColorAlias2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnums.ColorAlias2, ColorAlias2Unmanaged>
        {

            internal ColorAlias2Plugin() : base("global::CorpusEnums.ColorAlias2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ColorAlias2Unmanaged>(
                    dtf,
                    "ColorAlias2",
                    global::CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic));
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 0;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class ColorAlias2Support : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.ColorAlias2>
    {
        public ColorAlias2Support() : base(
            new Implementation.ColorAlias2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorAlias2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorAlias2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorAlias2Support, global::CorpusEnums.ColorAlias2>();

    }

    namespace Implementation
    {

        public struct RecordUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusEnums.Record>
        {

            private int value;
            private int aliasValue;
            private global::CorpusEnums.Color color;
            private global::CorpusEnums.Color aliasColor;
            private global::CorpusEnums.Implementation.ColorSequenceUnmanaged colors;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                colors.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusEnums.Record sample, bool keysOnly = false)
            {

                sample.value = value;
                sample.aliasValue = aliasValue;
                sample.color = color;
                sample.aliasColor = aliasColor;
                colors.FromNative(sample.colors, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
                aliasValue = (int) (0);
                color = (global::CorpusEnums.Color) (0);
                aliasColor = (global::CorpusEnums.Color) (0);
                colors.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusEnums.Record sample, bool keysOnly = false)
            {
                value = sample.value;
                aliasValue = sample.aliasValue;
                color = sample.color;
                aliasColor = sample.aliasColor;
                colors.ToNative(sample.colors, keysOnly: false);
            }
        }

        internal class RecordPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusEnums.Record, RecordUnmanaged>
        {

            internal RecordPlugin() : base("global::CorpusEnums.Record", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Record struct
                var RecordStructMembers = new StructMember[]
                {
                    new StructMember("value", global::CorpusEnums.ScalarSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("aliasValue", global::CorpusEnums.ScalarAliasSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("color", global::CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                    new StructMember("aliasColor", global::CorpusEnums.ColorAlias2Support.Instance.GetDynamicTypeInternal(isPublic), id: 3),
                    new StructMember("colors", global::CorpusEnums.ColorSequenceSupport.Instance.GetDynamicTypeInternal(isPublic), id: 4)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<RecordUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusEnums::Record")
                    .AddMembers(RecordStructMembers));

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
                        2,
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
                        3,
                        annotations);
                }

                return result;

            }
        }
    }
    public class RecordSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnums.Record>
    {
        public RecordSupport() : base(
            new Implementation.RecordPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RecordPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RecordSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RecordSupport, global::CorpusEnums.Record>();

    }

} // namespace CorpusEnums

