/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-enum-constant.idl
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
    internal class ColorPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
    {
        public ColorPlugin() : base(CreateDynamicType(isPublic: false))
        {
        }

        internal static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var enumType = dtf.BuildEnum()
            .WithName("Color")
            .AddMember(new EnumMember("RED", 0))
            .AddMember(new EnumMember("GREEN", 1))
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

public class ColorSupport : Rti.Dds.Topics.TypeSupport<global::Color>
{
    public ColorSupport() : base(
        new Implementation.ColorPlugin(),
        new Lazy<DynamicType>(() =>Implementation.ColorPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ColorSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSupport, global::Color>();

}

namespace Implementation
{

    public struct EnumConstantUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::EnumConstant>
    {

        private global::Color color;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::EnumConstant sample, bool keysOnly = false)
        {

            sample.color = color;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            color = (global::Color) (0);
        }

        public void ToNative(global::EnumConstant sample, bool keysOnly = false)
        {
            color = sample.color;
        }
    }

    internal class EnumConstantPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::EnumConstant, EnumConstantUnmanaged>
    {

        internal EnumConstantPlugin() : base("global::EnumConstant", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // EnumConstant struct
            var EnumConstantStructMembers = new StructMember[]
            {
                new StructMember("color", global::ColorSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<EnumConstantUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("EnumConstant")
                .AddMembers(EnumConstantStructMembers));

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
                    0,
                    annotations);
            }

            return result;

        }
    }
}
public class EnumConstantSupport : Rti.Dds.Topics.TypeSupport<global::EnumConstant>
{
    public EnumConstantSupport() : base(
        new Implementation.EnumConstantPlugin(),
        new Lazy<DynamicType>(() =>Implementation.EnumConstantPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static EnumConstantSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnumConstantSupport, global::EnumConstant>();

}

namespace Implementation
{

    public struct EnumUnionUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::EnumUnion>
    {
        private global::Color _d;

        private int selected;
        private int red;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::EnumUnion sample, bool keysOnly = false)
        {
            switch (_d)
            {
                case (Selected.Value):
                    sample.selected = selected;
                    break;
                case Color.RED:
                sample.red = red;
                break;
            }
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            _d = EnumUnion.DefaultDiscriminator;
            selected = (int) (0);
            red = (int) (0);
        }

        public void ToNative(global::EnumUnion sample, bool keysOnly = false)
        {
            _d = sample.Discriminator;
            switch (_d)
            {
                case (Selected.Value):
                    selected = sample.selected;
                    break;
                case Color.RED:
                red = sample.red;
                break;
            }
        }
    }

    internal class EnumUnionPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::EnumUnion, EnumUnionUnmanaged>
    {

        internal EnumUnionPlugin() : base("global::EnumUnion", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // EnumUnion union
            var EnumUnionStructMembers = new UnionMember[]
            {
                new UnionMember("selected", dtf.GetPrimitiveType<int>(), new int[] {(int) (Selected.Value)}, id: 1),
                new UnionMember("red", dtf.GetPrimitiveType<int>(), new int[] {(int) Color.RED}, id: 2)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<EnumUnionUnmanaged>(
                dtf.BuildUnion()
                .WithDiscriminator(global::ColorSupport.Instance.GetDynamicTypeInternal(isPublic))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("EnumUnion")
                .AddMembers(EnumUnionStructMembers));

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
public class EnumUnionSupport : Rti.Dds.Topics.TypeSupport<global::EnumUnion>
{
    public EnumUnionSupport() : base(
        new Implementation.EnumUnionPlugin(),
        new Lazy<DynamicType>(() =>Implementation.EnumUnionPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static EnumUnionSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnumUnionSupport, global::EnumUnion>();

}

