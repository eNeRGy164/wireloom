/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-name-collisions.idl
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

namespace CorpusNameCollisions
{

    namespace Implementation
    {

        public struct ItemUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.Item>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNameCollisions.Item sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusNameCollisions.Item sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class ItemPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.Item, ItemUnmanaged>
        {

            internal ItemPlugin() : base("global::CorpusNameCollisions.Item", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Item struct
                var ItemStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ItemUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::Item")
                    .AddMembers(ItemStructMembers));

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
    public class ItemSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.Item>
    {
        public ItemSupport() : base(
            new Implementation.ItemPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ItemPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ItemSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ItemSupport, global::CorpusNameCollisions.Item>();

    }

    namespace Implementation
    {

        public struct LifecycleParametersUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.LifecycleParameters>
        {

            private NativeString optionalsOnly;
            private global::CorpusNameCollisions.Implementation.ItemUnmanaged allocatePointers;
            private NativeSeq allocateMemory;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                optionalsOnly.Destroy();
                allocatePointers.Destroy(optionalsOnly);
                allocateMemory.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusNameCollisions.LifecycleParameters sample, bool keysOnly = false)
            {

                sample.optionalsOnly = optionalsOnly.FromNative();
                allocatePointers.FromNative(sample.allocatePointers, keysOnly: false);
                allocateMemory.FromNative((Sequence<int>) sample.allocateMemory);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                optionalsOnly.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
                allocatePointers.Initialize(allocatePointers, allocateMemory);
                allocateMemory.Initialize<int >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusNameCollisions.LifecycleParameters sample, bool keysOnly = false)
            {
                optionalsOnly.ToNative(sample.optionalsOnly, ((int) 16));
                allocatePointers.ToNative(sample.allocatePointers, keysOnly: false);
                allocateMemory.ToNative((Sequence<int>) sample.allocateMemory);
            }
        }

        internal class LifecycleParametersPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.LifecycleParameters, LifecycleParametersUnmanaged>
        {

            internal LifecycleParametersPlugin() : base("global::CorpusNameCollisions.LifecycleParameters", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // LifecycleParameters struct
                var LifecycleParametersStructMembers = new StructMember[]
                {
                    new StructMember("optionalsOnly", dtf.CreateString(((int) 16)), id: 0),
                    new StructMember("allocatePointers", global::CorpusNameCollisions.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("allocateMemory", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)2)), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<LifecycleParametersUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::LifecycleParameters")
                    .AddMembers(LifecycleParametersStructMembers));

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
                        0,
                        annotations);
                }

                return result;

            }
        }
    }
    public class LifecycleParametersSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.LifecycleParameters>
    {
        public LifecycleParametersSupport() : base(
            new Implementation.LifecycleParametersPlugin(),
            new Lazy<DynamicType>(() =>Implementation.LifecycleParametersPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static LifecycleParametersSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<LifecycleParametersSupport, global::CorpusNameCollisions.LifecycleParameters>();

    }

    namespace Implementation
    {

        public struct ManagedMethodParametersUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.ManagedMethodParameters>
        {

            private int other;
            private int hash;
            private int sample;
            private int keysOnly;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNameCollisions.ManagedMethodParameters sample, bool keysOnly = false)
            {

                sample.other = other;
                sample.hash = hash;
                sample.sample = sample;
                sample.keysOnly = keysOnly;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                other = (int) (0);
                hash = (int) (0);
                sample = (int) (0);
                keysOnly = (int) (0);
            }

            public void ToNative(global::CorpusNameCollisions.ManagedMethodParameters sample, bool keysOnly = false)
            {
                other = sample.other;
                hash = sample.hash;
                sample = sample.sample;
                keysOnly = sample.keysOnly;
            }
        }

        internal class ManagedMethodParametersPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.ManagedMethodParameters, ManagedMethodParametersUnmanaged>
        {

            internal ManagedMethodParametersPlugin() : base("global::CorpusNameCollisions.ManagedMethodParameters", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // ManagedMethodParameters struct
                var ManagedMethodParametersStructMembers = new StructMember[]
                {
                    new StructMember("other", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("hash", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("sample", dtf.GetPrimitiveType<int>(), id: 2),
                    new StructMember("keysOnly", dtf.GetPrimitiveType<int>(), id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ManagedMethodParametersUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::ManagedMethodParameters")
                    .AddMembers(ManagedMethodParametersStructMembers));

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
                        2,
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
                        3,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ManagedMethodParametersSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.ManagedMethodParameters>
    {
        public ManagedMethodParametersSupport() : base(
            new Implementation.ManagedMethodParametersPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ManagedMethodParametersPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ManagedMethodParametersSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ManagedMethodParametersSupport, global::CorpusNameCollisions.ManagedMethodParameters>();

    }

    namespace Implementation
    {

        public struct ArrayInitializationLocalsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.ArrayInitializationLocals>
        {

            private NativeManagedArray dimension0;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                dimension0.Destroy<global::CorpusNameCollisions.Item, global::CorpusNameCollisions.Implementation.ItemUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
            }

            public void FromNative(global::CorpusNameCollisions.ArrayInitializationLocals sample, bool keysOnly = false)
            {

                dimension0.FromNative<global::CorpusNameCollisions.Item, global::CorpusNameCollisions.Implementation.ItemUnmanaged>(sample.dimension0, keysOnly: keysOnly, dimension: (2));
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                dimension0.Initialize<global::CorpusNameCollisions.Item, global::CorpusNameCollisions.Implementation.ItemUnmanaged>(dimension: (2), allocatePointers: allocatePointers, allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusNameCollisions.ArrayInitializationLocals sample, bool keysOnly = false)
            {
                dimension0.ToNative<global::CorpusNameCollisions.Item, global::CorpusNameCollisions.Implementation.ItemUnmanaged>(sample.dimension0, keysOnly: keysOnly, dimension: (2));
            }
        }

        internal class ArrayInitializationLocalsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.ArrayInitializationLocals, ArrayInitializationLocalsUnmanaged>
        {

            internal ArrayInitializationLocalsPlugin() : base("global::CorpusNameCollisions.ArrayInitializationLocals", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // ArrayInitializationLocals struct
                var ArrayInitializationLocalsStructMembers = new StructMember[]
                {
                    new StructMember("dimension0", tsf.CreateArrayWithAccessInfo<global::CorpusNameCollisions.Implementation.ItemUnmanaged>(dtf, global::CorpusNameCollisions.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ArrayInitializationLocalsUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::ArrayInitializationLocals")
                    .AddMembers(ArrayInitializationLocalsStructMembers));

                return result;

            }
        }
    }
    public class ArrayInitializationLocalsSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.ArrayInitializationLocals>
    {
        public ArrayInitializationLocalsSupport() : base(
            new Implementation.ArrayInitializationLocalsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ArrayInitializationLocalsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ArrayInitializationLocalsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ArrayInitializationLocalsSupport, global::CorpusNameCollisions.ArrayInitializationLocals>();

    }

    namespace Implementation
    {

        public struct NativeInheritanceStorageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.NativeInheritanceStorage>
        {

            private int baseValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNameCollisions.NativeInheritanceStorage sample, bool keysOnly = false)
            {

                sample.baseValue = baseValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                baseValue = (int) (0);
            }

            public void ToNative(global::CorpusNameCollisions.NativeInheritanceStorage sample, bool keysOnly = false)
            {
                baseValue = sample.baseValue;
            }
        }

        internal class NativeInheritanceStoragePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.NativeInheritanceStorage, NativeInheritanceStorageUnmanaged>
        {

            internal NativeInheritanceStoragePlugin() : base("global::CorpusNameCollisions.NativeInheritanceStorage", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // NativeInheritanceStorage struct
                var NativeInheritanceStorageStructMembers = new StructMember[]
                {
                    new StructMember("baseValue", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<NativeInheritanceStorageUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::NativeInheritanceStorage")
                    .AddMembers(NativeInheritanceStorageStructMembers));

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
    public class NativeInheritanceStorageSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.NativeInheritanceStorage>
    {
        public NativeInheritanceStorageSupport() : base(
            new Implementation.NativeInheritanceStoragePlugin(),
            new Lazy<DynamicType>(() =>Implementation.NativeInheritanceStoragePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NativeInheritanceStorageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NativeInheritanceStorageSupport, global::CorpusNameCollisions.NativeInheritanceStorage>();

    }

    namespace Implementation
    {

        public struct DerivedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.Derived>
        {
            private global::CorpusNameCollisions.Implementation.NativeInheritanceStorageUnmanaged parent;

            private int parent;
            private int value;

            public void Destroy(bool optionalsOnly)
            {
                parent.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusNameCollisions.Derived sample, bool keysOnly = false)
            {

                parent.FromNative(sample, keysOnly);
                sample.parent = parent;
                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                parent.Initialize(allocatePointers, allocateMemory);
                parent = (int) (0);
                value = (int) (0);
            }

            public void ToNative(global::CorpusNameCollisions.Derived sample, bool keysOnly = false)
            {
                parent.ToNative(sample, keysOnly);
                parent = sample.parent;
                value = sample.value;
            }
        }

        internal class DerivedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.Derived, DerivedUnmanaged>
        {

            internal DerivedPlugin() : base("global::CorpusNameCollisions.Derived", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Derived struct
                var DerivedStructMembers = new StructMember[]
                {
                    new StructMember("parent", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<DerivedUnmanaged>(
                    dtf.BuildStruct()
                    .WithParent((StructType) global::CorpusNameCollisions.NativeInheritanceStorageSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::Derived")
                    .AddMembers(DerivedStructMembers));

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
    public class DerivedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.Derived>
    {
        public DerivedSupport() : base(
            new Implementation.DerivedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DerivedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DerivedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DerivedSupport, global::CorpusNameCollisions.Derived>();

    }

    namespace Implementation
    {

        public struct NativeLifecycleParametersUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.NativeLifecycleParameters>
        {
            private int _d;

            private NativeString optionalsOnly;
            private global::CorpusNameCollisions.Implementation.ItemUnmanaged allocatePointers;
            private NativeSeq allocateMemory;
            private byte flag;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                optionalsOnly.Destroy();
                allocatePointers.Destroy(optionalsOnly);
                allocateMemory.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusNameCollisions.NativeLifecycleParameters sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 1:
                    sample.optionalsOnly = optionalsOnly.FromNative();
                    break;
                    case 2:
                    if(sample.Discriminator != _d)
                    {
                        sample.allocatePointers = new global::CorpusNameCollisions.Item();
                    }
                    allocatePointers.FromNative(sample.allocatePointers, keysOnly: false);
                    break;
                    case 3:
                    if(sample.Discriminator != _d)
                    {
                        sample.allocateMemory = new Rti.Types.Sequence<int>();
                    }
                    allocateMemory.FromNative((Sequence<int>) sample.allocateMemory);
                    break;
                    default:
                    sample.flag = Convert.ToBoolean(flag);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = NativeLifecycleParameters.DefaultDiscriminator;
                optionalsOnly.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
                allocatePointers.Initialize(allocatePointers, allocateMemory);
                allocateMemory.Initialize<int >(max: ((int)2), absoluteMax: ((int)2), allocateMemory: allocateMemory);
                flag = 0;
            }

            public void ToNative(global::CorpusNameCollisions.NativeLifecycleParameters sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 1:
                    optionalsOnly.ToNative(sample.optionalsOnly, ((int) 16));
                    break;
                    case 2:
                    allocatePointers.ToNative(sample.allocatePointers, keysOnly: false);
                    break;
                    case 3:
                    allocateMemory.ToNative((Sequence<int>) sample.allocateMemory);
                    break;
                    default:
                    flag = Convert.ToByte(sample.flag);
                    break;
                }
            }
        }

        internal class NativeLifecycleParametersPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.NativeLifecycleParameters, NativeLifecycleParametersUnmanaged>
        {

            internal NativeLifecycleParametersPlugin() : base("global::CorpusNameCollisions.NativeLifecycleParameters", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // NativeLifecycleParameters union
                var NativeLifecycleParametersStructMembers = new UnionMember[]
                {
                    new UnionMember("optionalsOnly", dtf.CreateString(((int) 16)), new int[] {(int) 1}, id: 1),
                    new UnionMember("allocatePointers", global::CorpusNameCollisions.ItemSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 2}, id: 2),
                    new UnionMember("allocateMemory", tsf.CreateSequenceWithAccessInfo(dtf, dtf.GetPrimitiveType<int>(), ((int)2)), new int[] {(int) 3}, id: 3),
                    new UnionMember("flag", dtf.GetPrimitiveType<bool>(), new int[] {(int) UnionMember.DefaultLabel}, id: 4)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<NativeLifecycleParametersUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::NativeLifecycleParameters")
                    .AddMembers(NativeLifecycleParametersStructMembers));

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
                        0,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.BoolValue = false;
                    Annotations annotations = new Annotations(
                        TypeKind.Boolean,
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
    public class NativeLifecycleParametersSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.NativeLifecycleParameters>
    {
        public NativeLifecycleParametersSupport() : base(
            new Implementation.NativeLifecycleParametersPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NativeLifecycleParametersPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NativeLifecycleParametersSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NativeLifecycleParametersSupport, global::CorpusNameCollisions.NativeLifecycleParameters>();

    }

    namespace Implementation
    {

        public struct NativeDiscriminatorStorageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.NativeDiscriminatorStorage>
        {
            private int _d;

            private int _discriminator;
            private byte flag;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNameCollisions.NativeDiscriminatorStorage sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 1:
                    sample._discriminator = _discriminator;
                    break;
                    default:
                    sample.flag = Convert.ToBoolean(flag);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = NativeDiscriminatorStorage.DefaultDiscriminator;
                _discriminator = (int) (0);
                flag = 0;
            }

            public void ToNative(global::CorpusNameCollisions.NativeDiscriminatorStorage sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 1:
                    _discriminator = sample._discriminator;
                    break;
                    default:
                    flag = Convert.ToByte(sample.flag);
                    break;
                }
            }
        }

        internal class NativeDiscriminatorStoragePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.NativeDiscriminatorStorage, NativeDiscriminatorStorageUnmanaged>
        {

            internal NativeDiscriminatorStoragePlugin() : base("global::CorpusNameCollisions.NativeDiscriminatorStorage", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // NativeDiscriminatorStorage union
                var NativeDiscriminatorStorageStructMembers = new UnionMember[]
                {
                    new UnionMember("_discriminator", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 1),
                    new UnionMember("flag", dtf.GetPrimitiveType<bool>(), new int[] {(int) UnionMember.DefaultLabel}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<NativeDiscriminatorStorageUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::NativeDiscriminatorStorage")
                    .AddMembers(NativeDiscriminatorStorageStructMembers));

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
                    defaultValueParam.BoolValue = false;
                    Annotations annotations = new Annotations(
                        TypeKind.Boolean,
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
    public class NativeDiscriminatorStorageSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.NativeDiscriminatorStorage>
    {
        public NativeDiscriminatorStorageSupport() : base(
            new Implementation.NativeDiscriminatorStoragePlugin(),
            new Lazy<DynamicType>(() =>Implementation.NativeDiscriminatorStoragePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NativeDiscriminatorStorageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NativeDiscriminatorStorageSupport, global::CorpusNameCollisions.NativeDiscriminatorStorage>();

    }

    namespace Implementation
    {

        public struct ManagedDiscriminatorStorageUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNameCollisions.ManagedDiscriminatorStorage>
        {
            private int _d;

            private int Discriminator;
            private byte flag;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNameCollisions.ManagedDiscriminatorStorage sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 1:
                    sample.Discriminator = Discriminator;
                    break;
                    default:
                    sample.flag = Convert.ToBoolean(flag);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = ManagedDiscriminatorStorage.DefaultDiscriminator;
                Discriminator = (int) (0);
                flag = 0;
            }

            public void ToNative(global::CorpusNameCollisions.ManagedDiscriminatorStorage sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 1:
                    Discriminator = sample.Discriminator;
                    break;
                    default:
                    flag = Convert.ToByte(sample.flag);
                    break;
                }
            }
        }

        internal class ManagedDiscriminatorStoragePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNameCollisions.ManagedDiscriminatorStorage, ManagedDiscriminatorStorageUnmanaged>
        {

            internal ManagedDiscriminatorStoragePlugin() : base("global::CorpusNameCollisions.ManagedDiscriminatorStorage", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // ManagedDiscriminatorStorage union
                var ManagedDiscriminatorStorageStructMembers = new UnionMember[]
                {
                    new UnionMember("Discriminator", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 1),
                    new UnionMember("flag", dtf.GetPrimitiveType<bool>(), new int[] {(int) UnionMember.DefaultLabel}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ManagedDiscriminatorStorageUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNameCollisions::ManagedDiscriminatorStorage")
                    .AddMembers(ManagedDiscriminatorStorageStructMembers));

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
                    defaultValueParam.BoolValue = false;
                    Annotations annotations = new Annotations(
                        TypeKind.Boolean,
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
    public class ManagedDiscriminatorStorageSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNameCollisions.ManagedDiscriminatorStorage>
    {
        public ManagedDiscriminatorStorageSupport() : base(
            new Implementation.ManagedDiscriminatorStoragePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ManagedDiscriminatorStoragePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ManagedDiscriminatorStorageSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ManagedDiscriminatorStorageSupport, global::CorpusNameCollisions.ManagedDiscriminatorStorage>();

    }

} // namespace CorpusNameCollisions

