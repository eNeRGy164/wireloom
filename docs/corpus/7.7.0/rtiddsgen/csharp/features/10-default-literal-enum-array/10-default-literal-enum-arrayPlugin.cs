/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-default-literal-enum-array.idl
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

namespace CorpusDefaultLiteralEnumArray
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
                .WithName("CorpusDefaultLiteralEnumArray::Color")
                .AddMember(new EnumMember("GREEN", 0))
                .AddMember(new EnumMember("RED", 1))
                .AddMember(new EnumMember("BLUE", 2))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 1;
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

    public class ColorSupport : Rti.Dds.Topics.TypeSupport<global::CorpusDefaultLiteralEnumArray.Color>
    {
        public ColorSupport() : base(
            new Implementation.ColorPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSupport, global::CorpusDefaultLiteralEnumArray.Color>();

    }

    namespace Implementation
    {

        public struct ColorArrayUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusDefaultLiteralEnumArray.ColorArray>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusDefaultLiteralEnumArray.ColorArray sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (5));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.InitializeEnum(dimension: (5), allocateMemory: allocateMemory, defaultValue: 1);
            }

            public void ToNative(global::CorpusDefaultLiteralEnumArray.ColorArray sample, bool keysOnly = false)
            {
                Value.ToNative<global::CorpusDefaultLiteralEnumArray.Color>(sample.Value, dimension: (5));
            }
        }

        internal class ColorArrayPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusDefaultLiteralEnumArray.ColorArray, ColorArrayUnmanaged>
        {

            internal ColorArrayPlugin() : base("global::CorpusDefaultLiteralEnumArray.ColorArray", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ColorArrayUnmanaged>(
                    dtf,
                    "ColorArray",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, global::CorpusDefaultLiteralEnumArray.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {5}));
                return aliasType;

            }
        }
    }
    public class ColorArraySupport : Rti.Dds.Topics.TypeSupport<global::CorpusDefaultLiteralEnumArray.ColorArray>
    {
        public ColorArraySupport() : base(
            new Implementation.ColorArrayPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorArrayPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorArraySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorArraySupport, global::CorpusDefaultLiteralEnumArray.ColorArray>();

    }

    namespace Implementation
    {

        public struct ShapeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusDefaultLiteralEnumArray.Shape>
        {

            private global::CorpusDefaultLiteralEnumArray.Color color;
            private global::CorpusDefaultLiteralEnumArray.Implementation.ColorArrayUnmanaged colorArray;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                colorArray.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusDefaultLiteralEnumArray.Shape sample, bool keysOnly = false)
            {

                sample.color = color;
                colorArray.FromNative(sample.colorArray, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                color = (global::CorpusDefaultLiteralEnumArray.Color) (1);
                colorArray.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusDefaultLiteralEnumArray.Shape sample, bool keysOnly = false)
            {
                color = sample.color;
                colorArray.ToNative(sample.colorArray, keysOnly: false);
            }
        }

        internal class ShapePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusDefaultLiteralEnumArray.Shape, ShapeUnmanaged>
        {

            internal ShapePlugin() : base("global::CorpusDefaultLiteralEnumArray.Shape", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Shape struct
                var ShapeStructMembers = new StructMember[]
                {
                    new StructMember("color", global::CorpusDefaultLiteralEnumArray.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("colorArray", global::CorpusDefaultLiteralEnumArray.ColorArraySupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ShapeUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusDefaultLiteralEnumArray::Shape")
                    .AddMembers(ShapeStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 1;
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
    public class ShapeSupport : Rti.Dds.Topics.TypeSupport<global::CorpusDefaultLiteralEnumArray.Shape>
    {
        public ShapeSupport() : base(
            new Implementation.ShapePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ShapePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ShapeSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ShapeSupport, global::CorpusDefaultLiteralEnumArray.Shape>();

    }

} // namespace CorpusDefaultLiteralEnumArray

