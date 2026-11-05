/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enums-aliases.idl
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
                return dtf.BuildEnum()
                .WithName("CorpusEnums::Color")
                .AddMember(new EnumMember("RED", 0))
                .AddMember(new EnumMember("GREEN", 1))
                .AddMember(new EnumMember("BLUE", 2))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
            }
        }
    }

    public class ColorSupport : Rti.Dds.Topics.TypeSupport<CorpusEnums.Color>
    {
        public ColorSupport() : base(
            new Implementation.ColorPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSupport, CorpusEnums.Color>();
    }

    namespace Implementation
    {

        public struct ScalarUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnums.Scalar>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusEnums.Scalar sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) 0;
            }

            public void ToNative(CorpusEnums.Scalar sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ScalarPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnums.Scalar, ScalarUnmanaged>
        {
            internal ScalarPlugin() : base("CorpusEnums.Scalar", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ScalarUnmanaged>(
                    dtf,
                    "Scalar",
                    dtf.GetPrimitiveType<int>());
            }
        }
    }
    public class ScalarSupport : Rti.Dds.Topics.TypeSupport<CorpusEnums.Scalar>
    {
        public ScalarSupport() : base(
            new Implementation.ScalarPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarSupport, CorpusEnums.Scalar>();
    }

    namespace Implementation
    {

        public struct ColorSequenceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnums.ColorSequence>
        {

            private NativeSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusEnums.ColorSequence sample, bool keysOnly = false)
            {

                Value.FromNative((Sequence<CorpusEnums.Color>) sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize<CorpusEnums.Color >(max: ((int)3), absoluteMax: ((int)3), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusEnums.ColorSequence sample, bool keysOnly = false)
            {
                Value.ToNative((Sequence<CorpusEnums.Color>) sample.Value);
            }
        }

        internal class ColorSequencePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnums.ColorSequence, ColorSequenceUnmanaged>
        {
            internal ColorSequencePlugin() : base("CorpusEnums.ColorSequence", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ColorSequenceUnmanaged>(
                    dtf,
                    "ColorSequence",
                    tsf.CreateSequenceWithAccessInfo(dtf, CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), ((int)3)));
            }
        }
    }
    public class ColorSequenceSupport : Rti.Dds.Topics.TypeSupport<CorpusEnums.ColorSequence>
    {
        public ColorSequenceSupport() : base(
            new Implementation.ColorSequencePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorSequencePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSequenceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSequenceSupport, CorpusEnums.ColorSequence>();
    }

    namespace Implementation
    {

        public struct ScalarAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnums.ScalarAlias>
        {

            private int Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusEnums.ScalarAlias sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (int) 0;
            }

            public void ToNative(CorpusEnums.ScalarAlias sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ScalarAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnums.ScalarAlias, ScalarAliasUnmanaged>
        {
            internal ScalarAliasPlugin() : base("CorpusEnums.ScalarAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ScalarAliasUnmanaged>(
                    dtf,
                    "ScalarAlias",
                    dtf.GetPrimitiveType<int>());
            }
        }
    }
    public class ScalarAliasSupport : Rti.Dds.Topics.TypeSupport<CorpusEnums.ScalarAlias>
    {
        public ScalarAliasSupport() : base(
            new Implementation.ScalarAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ScalarAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ScalarAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScalarAliasSupport, CorpusEnums.ScalarAlias>();
    }

    namespace Implementation
    {

        public struct ColorAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnums.ColorAlias>
        {

            private CorpusEnums.Color Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusEnums.ColorAlias sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (CorpusEnums.Color) 0;
            }

            public void ToNative(CorpusEnums.ColorAlias sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ColorAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnums.ColorAlias, ColorAliasUnmanaged>
        {
            internal ColorAliasPlugin() : base("CorpusEnums.ColorAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ColorAliasUnmanaged>(
                    dtf,
                    "ColorAlias",
                    CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class ColorAliasSupport : Rti.Dds.Topics.TypeSupport<CorpusEnums.ColorAlias>
    {
        public ColorAliasSupport() : base(
            new Implementation.ColorAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorAliasSupport, CorpusEnums.ColorAlias>();
    }

    namespace Implementation
    {

        public struct ColorAlias2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnums.ColorAlias2>
        {

            private CorpusEnums.Color Value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(CorpusEnums.ColorAlias2 sample, bool keysOnly = false)
            {

                sample.Value = Value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value = (CorpusEnums.Color) 0;
            }

            public void ToNative(CorpusEnums.ColorAlias2 sample, bool keysOnly = false)
            {
                Value = sample.Value;
            }
        }

        internal class ColorAlias2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnums.ColorAlias2, ColorAlias2Unmanaged>
        {
            internal ColorAlias2Plugin() : base("CorpusEnums.ColorAlias2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<ColorAlias2Unmanaged>(
                    dtf,
                    "ColorAlias2",
                    CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic));
            }
        }
    }
    public class ColorAlias2Support : Rti.Dds.Topics.TypeSupport<CorpusEnums.ColorAlias2>
    {
        public ColorAlias2Support() : base(
            new Implementation.ColorAlias2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorAlias2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorAlias2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorAlias2Support, CorpusEnums.ColorAlias2>();
    }

    namespace Implementation
    {

        public struct RecordUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusEnums.Record>
        {

            private int value;
            private int aliasValue;
            private CorpusEnums.Color color;
            private CorpusEnums.Color aliasColor;
            private CorpusEnums.Implementation.ColorSequenceUnmanaged colors;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                colors.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusEnums.Record sample, bool keysOnly = false)
            {

                sample.value = value;
                sample.aliasValue = aliasValue;
                sample.color = color;
                sample.aliasColor = aliasColor;
                colors.FromNative(sample.colors, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) 0;
                aliasValue = (int) 0;
                color = (CorpusEnums.Color) 0;
                aliasColor = (CorpusEnums.Color) 0;
                colors.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(CorpusEnums.Record sample, bool keysOnly = false)
            {
                value = sample.value;
                aliasValue = sample.aliasValue;
                color = sample.color;
                aliasColor = sample.aliasColor;
                colors.ToNative(sample.colors, keysOnly: false);
            }
        }

        internal class RecordPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusEnums.Record, RecordUnmanaged>
        {
            internal RecordPlugin() : base("CorpusEnums.Record", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Record struct
                var RecordStructMembers = new StructMember[]
                {
                    new StructMember("value", CorpusEnums.ScalarSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("aliasValue", CorpusEnums.ScalarAliasSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("color", CorpusEnums.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                    new StructMember("aliasColor", CorpusEnums.ColorAlias2Support.Instance.GetDynamicTypeInternal(isPublic), id: 3),
                    new StructMember("colors", CorpusEnums.ColorSequenceSupport.Instance.GetDynamicTypeInternal(isPublic), id: 4)
                };

                return tsf.CreateTypeWithAccessInfo<RecordUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusEnums::Record")
                    .AddMembers(RecordStructMembers));
            }
        }
    }
    public class RecordSupport : Rti.Dds.Topics.TypeSupport<CorpusEnums.Record>
    {
        public RecordSupport() : base(
            new Implementation.RecordPlugin(),
            new Lazy<DynamicType>(() =>Implementation.RecordPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RecordSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RecordSupport, CorpusEnums.Record>();
    }

} // namespace CorpusEnums

