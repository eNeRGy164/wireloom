/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 06-aggregates.idl
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

namespace CorpusAggregates
{

    namespace Implementation
    {

        public struct BaseUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregates.Base>
        {

            private int @base;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusAggregates.Base sample, bool keysOnly = false)
            {

                sample.@base = @base;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                @base = (int) (0);
            }

            public void ToNative(global::CorpusAggregates.Base sample, bool keysOnly = false)
            {
                @base = sample.@base;
            }
        }

        internal class BasePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregates.Base, BaseUnmanaged>
        {

            internal BasePlugin() : base("global::CorpusAggregates.Base", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Base struct
                var BaseStructMembers = new StructMember[]
                {
                    new StructMember("base", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<BaseUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregates::Base")
                    .AddMembers(BaseStructMembers));

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
    public class BaseSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregates.Base>
    {
        public BaseSupport() : base(
            new Implementation.BasePlugin(),
            new Lazy<DynamicType>(() =>Implementation.BasePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BaseSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BaseSupport, global::CorpusAggregates.Base>();

    }

    namespace Implementation
    {

        public struct DerivedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregates.Derived>
        {
            private global::CorpusAggregates.Implementation.BaseUnmanaged parent;

            private NativeString derived;

            public void Destroy(bool optionalsOnly)
            {
                parent.Destroy(optionalsOnly);
                if (optionalsOnly)
                {
                    return;
                }
                derived.Destroy();
            }

            public void FromNative(global::CorpusAggregates.Derived sample, bool keysOnly = false)
            {

                parent.FromNative(sample, keysOnly);
                sample.derived = derived.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                parent.Initialize(allocatePointers, allocateMemory);
                derived.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusAggregates.Derived sample, bool keysOnly = false)
            {
                parent.ToNative(sample, keysOnly);
                derived.ToNative(sample.derived, ((int) 16));
            }
        }

        internal class DerivedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregates.Derived, DerivedUnmanaged>
        {

            internal DerivedPlugin() : base("global::CorpusAggregates.Derived", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Derived struct
                var DerivedStructMembers = new StructMember[]
                {
                    new StructMember("derived", dtf.CreateString(((int) 16)), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<DerivedUnmanaged>(
                    dtf.BuildStruct()
                    .WithParent((StructType) global::CorpusAggregates.BaseSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregates::Derived")
                    .AddMembers(DerivedStructMembers));

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
    public class DerivedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregates.Derived>
    {
        public DerivedSupport() : base(
            new Implementation.DerivedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DerivedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DerivedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DerivedSupport, global::CorpusAggregates.Derived>();

    }

    namespace Implementation
    {

        public struct RecursiveUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregates.Recursive>
        {

            private int value;
            private NativeSeq children;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                children.Destroy<global::CorpusAggregates.Recursive, global::CorpusAggregates.Implementation.RecursiveUnmanaged>(optionalsOnly);
            }

            public void FromNative(global::CorpusAggregates.Recursive sample, bool keysOnly = false)
            {

                sample.value = value;
                children.FromNative<global::CorpusAggregates.Recursive, global::CorpusAggregates.Implementation.RecursiveUnmanaged>(sample.children);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
                children.Initialize<global::CorpusAggregates.Recursive , global::CorpusAggregates.Implementation.RecursiveUnmanaged >(max: ((int)100), absoluteMax: ((int)100), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusAggregates.Recursive sample, bool keysOnly = false)
            {
                value = sample.value;
                children.ToNative<global::CorpusAggregates.Recursive, global::CorpusAggregates.Implementation.RecursiveUnmanaged>(sample.children);
            }
        }

        internal class RecursivePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregates.Recursive, RecursiveUnmanaged>
        {
            static DynamicType dynamicType = null;
            static bool isInitialized = false;

            internal RecursivePlugin() : base("global::CorpusAggregates.Recursive", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                isRecursiveType = true;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                if (isPublic)
                {
                    throw new NotSupportedException(
                        "Recursive types don't support the property TypeSupport.DynamicType");
                }

                if (isInitialized)
                {
                    dynamicType = tsf.CreateTypeWithAccessInfo<RecursiveUnmanaged>(
                        dtf.BuildStruct()
                        .WithExtensibility(ExtensibilityKind.Extensible)
                        .WithName("CorpusAggregates::Recursive"));
                    return dynamicType;
                }

                isInitialized = true;

                // Recursive struct
                var RecursiveStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("children", tsf.CreateSequenceWithAccessInfo(dtf, global::CorpusAggregates.RecursiveSupport.GetOrCreateInstanceImpl().GetDynamicTypeInternal(isPublic), ((int)100)), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<RecursiveUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregates::Recursive")
                    .AddMembers(RecursiveStructMembers));

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

                if (dynamicType != null && ((Rti.Types.Dynamic.StructType)dynamicType).MemberCount == 0)
                {
                    tsf.SetStructMembers((Rti.Types.Dynamic.StructType) dynamicType, RecursiveStructMembers);
                }
                isInitialized = false;

                return result;

            }
        }
    }
    public class RecursiveSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregates.Recursive>
    {
        public RecursiveSupport() : base(
            new Implementation.RecursivePlugin(),
            new Lazy<DynamicType>(() =>Implementation.RecursivePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static RecursiveSupport Instance { get; private set; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RecursiveSupport, global::CorpusAggregates.Recursive>();

        internal static RecursiveSupport GetOrCreateInstanceImpl() {
            if (Instance == null) {
                Instance =
                ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<RecursiveSupport, global::CorpusAggregates.Recursive>();
            }
            return Instance;
        }
    }

    namespace Implementation
    {

        public struct ComposedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAggregates.Composed>
        {

            private global::CorpusAggregates.Implementation.BaseUnmanaged @base;
            private global::CorpusAggregates.Implementation.DerivedUnmanaged derived;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                @base.Destroy(optionalsOnly);
                derived.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusAggregates.Composed sample, bool keysOnly = false)
            {

                @base.FromNative(sample.@base, keysOnly: false);
                derived.FromNative(sample.derived, keysOnly: false);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                @base.Initialize(allocatePointers, allocateMemory);
                derived.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(global::CorpusAggregates.Composed sample, bool keysOnly = false)
            {
                @base.ToNative(sample.@base, keysOnly: false);
                derived.ToNative(sample.derived, keysOnly: false);
            }
        }

        internal class ComposedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAggregates.Composed, ComposedUnmanaged>
        {

            internal ComposedPlugin() : base("global::CorpusAggregates.Composed", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Composed struct
                var ComposedStructMembers = new StructMember[]
                {
                    new StructMember("base", global::CorpusAggregates.BaseSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("derived", global::CorpusAggregates.DerivedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ComposedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAggregates::Composed")
                    .AddMembers(ComposedStructMembers));

                return result;

            }
        }
    }
    public class ComposedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAggregates.Composed>
    {
        public ComposedSupport() : base(
            new Implementation.ComposedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ComposedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ComposedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ComposedSupport, global::CorpusAggregates.Composed>();

    }

} // namespace CorpusAggregates

