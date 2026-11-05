/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 10-default-literal-enum-array.idl
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
                return dtf.BuildEnum()
                .WithName("CorpusDefaultLiteralEnumArray::Color")
                .AddMember(new EnumMember("GREEN", 0))
                .AddMember(new EnumMember("RED", 1))
                .AddMember(new EnumMember("BLUE", 2))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
            }
        }
    }

    public class ColorSupport : Rti.Dds.Topics.TypeSupport<CorpusDefaultLiteralEnumArray.Color>
    {
        public ColorSupport() : base(
            new Implementation.ColorPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSupport, CorpusDefaultLiteralEnumArray.Color>();
    }

    namespace Implementation
    {

        public struct ColorArrayUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusDefaultLiteralEnumArray.ColorArray>
        {

            private NativeUnmanagedArray Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusDefaultLiteralEnumArray.ColorArray sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, dimension: (5));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusDefaultLiteralEnumArray.Color>(dimension: (5), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusDefaultLiteralEnumArray.ColorArray sample, bool keysOnly = false)
            {
                Value.ToNative<CorpusDefaultLiteralEnumArray.Color>(sample.Value, dimension: (5));
            }
        }

        internal class ColorArrayPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusDefaultLiteralEnumArray.ColorArray, ColorArrayUnmanaged>
        {
            internal ColorArrayPlugin() : base("CorpusDefaultLiteralEnumArray.ColorArray", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ColorArrayUnmanaged>(
                    dtf,
                    "ColorArray",
                    tsf.CreateArrayWithAccessInfo<int>(dtf, CorpusDefaultLiteralEnumArray.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {5}
                ));
            }
        }
    }
    public class ColorArraySupport : Rti.Dds.Topics.TypeSupport<CorpusDefaultLiteralEnumArray.ColorArray>
    {
        public ColorArraySupport() : base(
            new Implementation.ColorArrayPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorArrayPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorArraySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorArraySupport, CorpusDefaultLiteralEnumArray.ColorArray>();
    }

    namespace Implementation
    {

        public struct ShapeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusDefaultLiteralEnumArray.Shape>
        {

            private CorpusDefaultLiteralEnumArray.Color color;
            private CorpusDefaultLiteralEnumArray.Implementation.ColorArrayUnmanaged colorArray;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                colorArray.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusDefaultLiteralEnumArray.Shape sample, bool keysOnly = false)
            {

                sample.color = color;
                colorArray.FromNative(sample.colorArray, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                color = (CorpusDefaultLiteralEnumArray.Color) 1;
                colorArray.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusDefaultLiteralEnumArray.Shape sample, bool keysOnly = false)
            {
                color = sample.color;
                colorArray.ToNative(sample.colorArray, keysOnly: false);
            }
        }

        internal class ShapePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusDefaultLiteralEnumArray.Shape, ShapeUnmanaged>
        {
            internal ShapePlugin() : base("CorpusDefaultLiteralEnumArray.Shape", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Shape struct
                var ShapeStructMembers = new StructMember[]
                {
                    new StructMember("color", CorpusDefaultLiteralEnumArray.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("colorArray", CorpusDefaultLiteralEnumArray.ColorArraySupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<ShapeUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusDefaultLiteralEnumArray::Shape")
                    .AddMembers(ShapeStructMembers));
            }
        }
    }
    public class ShapeSupport : Rti.Dds.Topics.TypeSupport<CorpusDefaultLiteralEnumArray.Shape>
    {
        public ShapeSupport() : base(
            new Implementation.ShapePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ShapePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ShapeSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ShapeSupport, CorpusDefaultLiteralEnumArray.Shape>();
    }

} // namespace CorpusDefaultLiteralEnumArray

