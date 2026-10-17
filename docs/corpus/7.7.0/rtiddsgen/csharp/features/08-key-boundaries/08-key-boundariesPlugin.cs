/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-boundaries.idl
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

namespace CorpusKeyBoundaries
{

    namespace Implementation
    {

        public struct ArrayKeyUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusKeyBoundaries.ArrayKey>
        {

            private NativeUnmanagedArray coordinates;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                coordinates.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusKeyBoundaries.ArrayKey sample, bool keysOnly = false)
            {

                coordinates.FromNative(sample.coordinates, dimension: (2));
                if (keysOnly)
                {
                    return;
                }
                sample.payload = payload;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                coordinates.Initialize<int>(dimension: (2), allocateMemory: allocateMemory);
                payload = (int) (0);
            }

            public void ToNative(global::CorpusKeyBoundaries.ArrayKey sample, bool keysOnly = false)
            {
                coordinates.ToNative<int>(sample.coordinates, dimension: (2));
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class ArrayKeyPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusKeyBoundaries.ArrayKey, ArrayKeyUnmanaged>
        {

            internal ArrayKeyPlugin() : base("global::CorpusKeyBoundaries.ArrayKey", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // ArrayKey struct
                var ArrayKeyStructMembers = new StructMember[]
                {
                    new StructMember("coordinates", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2}), isKey: true, id: 0),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ArrayKeyUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusKeyBoundaries::ArrayKey")
                    .AddMembers(ArrayKeyStructMembers));

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
    public class ArrayKeySupport : Rti.Dds.Topics.TypeSupport<global::CorpusKeyBoundaries.ArrayKey>
    {
        public ArrayKeySupport() : base(
            new Implementation.ArrayKeyPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ArrayKeyPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ArrayKeySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ArrayKeySupport, global::CorpusKeyBoundaries.ArrayKey>();

    }

    namespace Implementation
    {

        public struct BoundedStringKeyUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusKeyBoundaries.BoundedStringKey>
        {

            private NativeString name;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                name.Destroy();
            }

            public void FromNative(global::CorpusKeyBoundaries.BoundedStringKey sample, bool keysOnly = false)
            {

                sample.name = name.FromNative();
                if (keysOnly)
                {
                    return;
                }
                sample.payload = payload;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                name.Initialize(size: ((int) 12), allocateMemory: allocateMemory);
                payload = (int) (0);
            }

            public void ToNative(global::CorpusKeyBoundaries.BoundedStringKey sample, bool keysOnly = false)
            {
                name.ToNative(sample.name, ((int) 12));
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class BoundedStringKeyPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusKeyBoundaries.BoundedStringKey, BoundedStringKeyUnmanaged>
        {

            internal BoundedStringKeyPlugin() : base("global::CorpusKeyBoundaries.BoundedStringKey", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // BoundedStringKey struct
                var BoundedStringKeyStructMembers = new StructMember[]
                {
                    new StructMember("name", dtf.CreateString(((int) 12)), isKey: true, id: 0),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<BoundedStringKeyUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusKeyBoundaries::BoundedStringKey")
                    .AddMembers(BoundedStringKeyStructMembers));

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
    public class BoundedStringKeySupport : Rti.Dds.Topics.TypeSupport<global::CorpusKeyBoundaries.BoundedStringKey>
    {
        public BoundedStringKeySupport() : base(
            new Implementation.BoundedStringKeyPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedStringKeyPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedStringKeySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedStringKeySupport, global::CorpusKeyBoundaries.BoundedStringKey>();

    }

    namespace Implementation
    {

        public struct UnboundedStringKeyUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusKeyBoundaries.UnboundedStringKey>
        {

            private NativeString name;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                name.Destroy();
            }

            public void FromNative(global::CorpusKeyBoundaries.UnboundedStringKey sample, bool keysOnly = false)
            {

                sample.name = name.FromNative();
                if (keysOnly)
                {
                    return;
                }
                sample.payload = payload;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                name.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                payload = (int) (0);
            }

            public void ToNative(global::CorpusKeyBoundaries.UnboundedStringKey sample, bool keysOnly = false)
            {
                name.ToNative(sample.name, ((int) 255));
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class UnboundedStringKeyPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusKeyBoundaries.UnboundedStringKey, UnboundedStringKeyUnmanaged>
        {

            internal UnboundedStringKeyPlugin() : base("global::CorpusKeyBoundaries.UnboundedStringKey", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // UnboundedStringKey struct
                var UnboundedStringKeyStructMembers = new StructMember[]
                {
                    new StructMember("name", dtf.CreateString(((int) 255)), isKey: true, id: 0),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<UnboundedStringKeyUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusKeyBoundaries::UnboundedStringKey")
                    .AddMembers(UnboundedStringKeyStructMembers));

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
    public class UnboundedStringKeySupport : Rti.Dds.Topics.TypeSupport<global::CorpusKeyBoundaries.UnboundedStringKey>
    {
        public UnboundedStringKeySupport() : base(
            new Implementation.UnboundedStringKeyPlugin(),
            new Lazy<DynamicType>(() =>Implementation.UnboundedStringKeyPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static UnboundedStringKeySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<UnboundedStringKeySupport, global::CorpusKeyBoundaries.UnboundedStringKey>();

    }

} // namespace CorpusKeyBoundaries

