/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-typed-defaults.idl
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

namespace Implementation
{

    public struct SevenUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::Seven>
    {

        private int Value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::Seven sample, bool keysOnly = false)
        {

            sample.Value = Value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            Value = (int) (7);
        }

        public void ToNative(global::Seven sample, bool keysOnly = false)
        {
            Value = sample.Value;
        }
    }

    internal class SevenPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::Seven, SevenUnmanaged>
    {

        internal SevenPlugin() : base("global::Seven", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            var aliasType = tsf.CreateAliasWithAccessInfo<SevenUnmanaged>(
                dtf,
                "Seven",
                dtf.GetPrimitiveType<int>());
            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.Int32Value = (int) 7;
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
public class SevenSupport : Rti.Dds.Topics.TypeSupport<global::Seven>
{
    public SevenSupport() : base(
        new Implementation.SevenPlugin(),
        new Lazy<DynamicType>(() =>Implementation.SevenPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static SevenSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SevenSupport, global::Seven>();

}

namespace Implementation
{

    public struct TextAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::TextAlias>
    {

        private NativeString Value;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            Value.Destroy();
        }

        public void FromNative(global::TextAlias sample, bool keysOnly = false)
        {

            sample.Value = Value.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            Value.ToNative("alias", ((int) 8));
        }

        public void ToNative(global::TextAlias sample, bool keysOnly = false)
        {
            Value.ToNative(sample.Value, ((int) 8));
        }
    }

    internal class TextAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::TextAlias, TextAliasUnmanaged>
    {

        internal TextAliasPlugin() : base("global::TextAlias", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            using var dtString = dtf.CreateString(((int) 8));
            var aliasType = tsf.CreateAliasWithAccessInfo<TextAliasUnmanaged>(
                dtf,
                "TextAlias",
                dtString
                );
            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.StringValue = "alias";
                Annotations annotations = new Annotations(
                    TypeKind.String,
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
public class TextAliasSupport : Rti.Dds.Topics.TypeSupport<global::TextAlias>
{
    public TextAliasSupport() : base(
        new Implementation.TextAliasPlugin(),
        new Lazy<DynamicType>(() =>Implementation.TextAliasPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static TextAliasSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TextAliasSupport, global::TextAlias>();

}

namespace Implementation
{

    public struct RatioUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::Ratio>
    {

        private double Value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::Ratio sample, bool keysOnly = false)
        {

            sample.Value = Value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            Value = (double) (1.5);
        }

        public void ToNative(global::Ratio sample, bool keysOnly = false)
        {
            Value = sample.Value;
        }
    }

    internal class RatioPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::Ratio, RatioUnmanaged>
    {

        internal RatioPlugin() : base("global::Ratio", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            var aliasType = tsf.CreateAliasWithAccessInfo<RatioUnmanaged>(
                dtf,
                "Ratio",
                dtf.GetPrimitiveType<double>());
            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.Float64Value = (double) 1.5;
                AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                minValueParam.Float64Value = (double) double.MinValue;
                AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                maxValueParam.Float64Value = (double) double.MaxValue;
                Annotations annotations = new Annotations(
                    TypeKind.Float64,
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
public class RatioSupport : Rti.Dds.Topics.TypeSupport<global::Ratio>
{
    public RatioSupport() : base(
        new Implementation.RatioPlugin(),
        new Lazy<DynamicType>(() =>Implementation.RatioPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static RatioSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RatioSupport, global::Ratio>();

}

namespace Implementation
{

    public struct TypedDefaultsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::TypedDefaults>
    {

        private int inherited;
        private int overridden;
        private NativeString text;
        private double ratio;
        private float single;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            text.Destroy();
        }

        public void FromNative(global::TypedDefaults sample, bool keysOnly = false)
        {

            sample.inherited = inherited;
            sample.overridden = overridden;
            sample.text = text.FromNative();
            sample.ratio = ratio;
            sample.single = single;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            inherited = (int) (7);
            overridden = (int) (9);
            text.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            text.ToNative("alias", ((int) 8));
            ratio = (double) (1.5);
            single = (float) (1.25f);
        }

        public void ToNative(global::TypedDefaults sample, bool keysOnly = false)
        {
            inherited = sample.inherited;
            overridden = sample.overridden;
            text.ToNative(sample.text, ((int) 8));
            ratio = sample.ratio;
            single = sample.single;
        }
    }

    internal class TypedDefaultsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::TypedDefaults, TypedDefaultsUnmanaged>
    {

        internal TypedDefaultsPlugin() : base("global::TypedDefaults", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // TypedDefaults struct
            var TypedDefaultsStructMembers = new StructMember[]
            {
                new StructMember("inherited", global::SevenSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                new StructMember("overridden", global::SevenSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                new StructMember("text", global::TextAliasSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                new StructMember("ratio", global::RatioSupport.Instance.GetDynamicTypeInternal(isPublic), id: 3),
                new StructMember("single", dtf.GetPrimitiveType<float>(), id: 4)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<TypedDefaultsUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("TypedDefaults")
                .AddMembers(TypedDefaultsStructMembers));

            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.Int32Value = (int) 7;
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
                defaultValueParam.Int32Value = (int) 9;
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
                defaultValueParam.StringValue = "alias";
                Annotations annotations = new Annotations(
                    TypeKind.String,
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
                defaultValueParam.Float64Value = (double) 1.5;
                AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                minValueParam.Float64Value = (double) double.MinValue;
                AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                maxValueParam.Float64Value = (double) double.MaxValue;
                Annotations annotations = new Annotations(
                    TypeKind.Float64,
                    defaultValueParam,
                    minValueParam,
                    maxValueParam,
                    null);
                result.SetMemberAnnotations(
                    3,
                    annotations);
            }
            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.Float32Value = (float) 1.25f;
                AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                minValueParam.Float32Value = (float) float.MinValue;
                AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                maxValueParam.Float32Value = (float) float.MaxValue;
                Annotations annotations = new Annotations(
                    TypeKind.Float32,
                    defaultValueParam,
                    minValueParam,
                    maxValueParam,
                    null);
                result.SetMemberAnnotations(
                    4,
                    annotations);
            }

            return result;

        }
    }
}
public class TypedDefaultsSupport : Rti.Dds.Topics.TypeSupport<global::TypedDefaults>
{
    public TypedDefaultsSupport() : base(
        new Implementation.TypedDefaultsPlugin(),
        new Lazy<DynamicType>(() =>Implementation.TypedDefaultsPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static TypedDefaultsSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TypedDefaultsSupport, global::TypedDefaults>();

}

