/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-aliases.idl
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

namespace CorpusUnionAliases
{

    namespace Implementation
    {
        internal class KindPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public KindPlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var enumType = dtf.BuildEnum()
                .WithName("CorpusUnionAliases::Kind")
                .AddMember(new EnumMember("Zero", 0))
                .AddMember(new EnumMember("Ten", 10))
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

    public class KindSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionAliases.Kind>
    {
        public KindSupport() : base(
            new Implementation.KindPlugin(),
            new Lazy<DynamicType>(() =>Implementation.KindPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static KindSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<KindSupport, global::CorpusUnionAliases.Kind>();

    }

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionAliases.Choice>
        {
            private global::CorpusUnionAliases.Kind _d;

            private int zero;
            private NativeString ten;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                ten.Destroy();
            }

            public void FromNative(global::CorpusUnionAliases.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case CorpusUnionAliases.Kind.Zero:
                    sample.zero = zero;
                    break;
                    case CorpusUnionAliases.Kind.Ten:
                    sample.ten = ten.FromNative();
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                zero = (int) (0);
                ten.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnionAliases.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case CorpusUnionAliases.Kind.Zero:
                    zero = sample.zero;
                    break;
                    case CorpusUnionAliases.Kind.Ten:
                    ten.ToNative(sample.ten, ((int) 255));
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionAliases.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusUnionAliases.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("zero", dtf.GetPrimitiveType<int>(), new int[] {(int) CorpusUnionAliases.Kind.Zero}, id: 1),
                    new UnionMember("ten", dtf.CreateString(((int) 255)), new int[] {(int) CorpusUnionAliases.Kind.Ten}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(global::CorpusUnionAliases.KindSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionAliases::Choice")
                    .AddMembers(ChoiceStructMembers));

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
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
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
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionAliases.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusUnionAliases.Choice>();

    }

    namespace Implementation
    {

        public struct ChoiceAliasUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionAliases.ChoiceAlias>
        {

            private global::CorpusUnionAliases.Implementation.ChoiceUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnionAliases.ChoiceAlias sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusUnionAliases.ChoiceAlias sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class ChoiceAliasPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionAliases.ChoiceAlias, ChoiceAliasUnmanaged>
        {

            internal ChoiceAliasPlugin() : base("global::CorpusUnionAliases.ChoiceAlias", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ChoiceAliasUnmanaged>(
                    dtf,
                    "ChoiceAlias",
                    global::CorpusUnionAliases.ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic));
                return aliasType;

            }
        }
    }
    public class ChoiceAliasSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionAliases.ChoiceAlias>
    {
        public ChoiceAliasSupport() : base(
            new Implementation.ChoiceAliasPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoiceAliasPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceAliasSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceAliasSupport, global::CorpusUnionAliases.ChoiceAlias>();

    }

    namespace Implementation
    {

        public struct ChoiceAlias2Unmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionAliases.ChoiceAlias2>
        {

            private global::CorpusUnionAliases.Implementation.ChoiceUnmanaged Value;

            public void Destroy(bool optionalsOnly)
            {
                Value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnionAliases.ChoiceAlias2 sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusUnionAliases.ChoiceAlias2 sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, keysOnly: false);
            }
        }

        internal class ChoiceAlias2Plugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionAliases.ChoiceAlias2, ChoiceAlias2Unmanaged>
        {

            internal ChoiceAlias2Plugin() : base("global::CorpusUnionAliases.ChoiceAlias2", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                var aliasType = tsf.CreateAliasWithAccessInfo<ChoiceAlias2Unmanaged>(
                    dtf,
                    "ChoiceAlias2",
                    global::CorpusUnionAliases.ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic));
                return aliasType;

            }
        }
    }
    public class ChoiceAlias2Support : Rti.Dds.Topics.TypeSupport<global::CorpusUnionAliases.ChoiceAlias2>
    {
        public ChoiceAlias2Support() : base(
            new Implementation.ChoiceAlias2Plugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoiceAlias2Plugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceAlias2Support Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceAlias2Support, global::CorpusUnionAliases.ChoiceAlias2>();

    }

    namespace Implementation
    {

        public struct HolderUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionAliases.Holder>
        {

            private global::CorpusUnionAliases.Implementation.ChoiceUnmanaged value;

            public void Destroy(bool optionalsOnly)
            {
                value.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnionAliases.Holder sample, bool keysOnly = false)
            {

                value.FromNative(sample.value, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusUnionAliases.Holder sample, bool keysOnly = false)
            {
                value.ToNative(sample.value, keysOnly: false);
            }
        }

        internal class HolderPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionAliases.Holder, HolderUnmanaged>
        {

            internal HolderPlugin() : base("global::CorpusUnionAliases.Holder", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Holder struct
                var HolderStructMembers = new StructMember[]
                {
                    new StructMember("value", global::CorpusUnionAliases.ChoiceAlias2Support.Instance.GetDynamicTypeInternal(isPublic), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<HolderUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionAliases::Holder")
                    .AddMembers(HolderStructMembers));

                return result;

            }
        }
    }
    public class HolderSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionAliases.Holder>
    {
        public HolderSupport() : base(
            new Implementation.HolderPlugin(),
            new Lazy<DynamicType>(() =>Implementation.HolderPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static HolderSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<HolderSupport, global::CorpusUnionAliases.Holder>();

    }

} // namespace CorpusUnionAliases

