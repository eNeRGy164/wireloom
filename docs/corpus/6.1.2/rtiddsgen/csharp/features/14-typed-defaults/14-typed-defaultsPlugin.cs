/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-typed-defaults.idl
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

namespace Implementation
{

    public struct SevenUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<Seven>
    {

        private int Value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(Seven sample, bool keysOnly = false)
        {

            sample.Value = Value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            Value = (int) 7;
        }

        public void ToNative(Seven sample, bool keysOnly = false)
        {
            Value = sample.Value;
        }
    }

    internal class SevenPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<Seven, SevenUnmanaged>
    {
        internal SevenPlugin() : base("Seven", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            return tsf.CreateAliasWithAccessInfo<SevenUnmanaged>(
                dtf,
                "Seven",
                dtf.GetPrimitiveType<int>());
        }
    }
}
public class SevenSupport : Rti.Dds.Topics.TypeSupport<Seven>
{
    public SevenSupport() : base(
        new Implementation.SevenPlugin(),
        new Lazy<DynamicType>(() =>Implementation.SevenPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static SevenSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SevenSupport, Seven>();
}

namespace Implementation
{

    public struct TextAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<TextAlias>
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

        public void FromNative(TextAlias sample, bool keysOnly = false)
        {

            sample.Value = Value.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            Value.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            Value.ToNative("alias", ((int) 8));
        }

        public void ToNative(TextAlias sample, bool keysOnly = false)
        {
            Value.ToNative(sample.Value, ((int) 8));
        }
    }

    internal class TextAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<TextAlias, TextAliasUnmanaged>
    {
        internal TextAliasPlugin() : base("TextAlias", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            return tsf.CreateAliasWithAccessInfo<TextAliasUnmanaged>(
                dtf,
                "TextAlias",
                dtf.CreateString(((int) 8)));
        }
    }
}
public class TextAliasSupport : Rti.Dds.Topics.TypeSupport<TextAlias>
{
    public TextAliasSupport() : base(
        new Implementation.TextAliasPlugin(),
        new Lazy<DynamicType>(() =>Implementation.TextAliasPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static TextAliasSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TextAliasSupport, TextAlias>();
}

namespace Implementation
{

    public struct RatioUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<Ratio>
    {

        private double Value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(Ratio sample, bool keysOnly = false)
        {

            sample.Value = Value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            Value = (double) 1.5;
        }

        public void ToNative(Ratio sample, bool keysOnly = false)
        {
            Value = sample.Value;
        }
    }

    internal class RatioPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<Ratio, RatioUnmanaged>
    {
        internal RatioPlugin() : base("Ratio", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            return tsf.CreateAliasWithAccessInfo<RatioUnmanaged>(
                dtf,
                "Ratio",
                dtf.GetPrimitiveType<double>());
        }
    }
}
public class RatioSupport : Rti.Dds.Topics.TypeSupport<Ratio>
{
    public RatioSupport() : base(
        new Implementation.RatioPlugin(),
        new Lazy<DynamicType>(() =>Implementation.RatioPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static RatioSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RatioSupport, Ratio>();
}

namespace Implementation
{

    public struct TypedDefaultsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<TypedDefaults>
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

        public void FromNative(TypedDefaults sample, bool keysOnly = false)
        {

            sample.inherited = inherited;
            sample.overridden = overridden;
            sample.text = text.FromNative();
            sample.ratio = ratio;
            sample.single = single;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            inherited = (int) 7;
            overridden = (int) 9;
            text.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            text.ToNative("alias", ((int) 8));
            ratio = (double) 1.5;
            single = (float) 1.25f;
        }

        public void ToNative(TypedDefaults sample, bool keysOnly = false)
        {
            inherited = sample.inherited;
            overridden = sample.overridden;
            text.ToNative(sample.text, ((int) 8));
            ratio = sample.ratio;
            single = sample.single;
        }
    }

    internal class TypedDefaultsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<TypedDefaults, TypedDefaultsUnmanaged>
    {
        internal TypedDefaultsPlugin() : base("TypedDefaults", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // TypedDefaults struct
            var TypedDefaultsStructMembers = new StructMember[]
            {
                new StructMember("inherited", SevenSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                new StructMember("overridden", SevenSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                new StructMember("text", TextAliasSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                new StructMember("ratio", RatioSupport.Instance.GetDynamicTypeInternal(isPublic), id: 3),
                new StructMember("single", dtf.GetPrimitiveType<float>(), id: 4)
            };

            return tsf.CreateTypeWithAccessInfo<TypedDefaultsUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("TypedDefaults")
                .AddMembers(TypedDefaultsStructMembers));
        }
    }
}
public class TypedDefaultsSupport : Rti.Dds.Topics.TypeSupport<TypedDefaults>
{
    public TypedDefaultsSupport() : base(
        new Implementation.TypedDefaultsPlugin(),
        new Lazy<DynamicType>(() =>Implementation.TypedDefaultsPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static TypedDefaultsSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TypedDefaultsSupport, TypedDefaults>();
}

