/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-inherited.idl
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

namespace CorpusInheritedKeys
{

    namespace Implementation
    {

        public struct BaseUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInheritedKeys.Base>
        {

            private int tenant;
            private int baseValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInheritedKeys.Base sample, bool keysOnly = false)
            {

                sample.tenant = tenant;
                if (keysOnly)
                {
                    return;
                }
                sample.baseValue = baseValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                tenant = (int) (0);
                baseValue = (int) (0);
            }

            public void ToNative(global::CorpusInheritedKeys.Base sample, bool keysOnly = false)
            {
                tenant = sample.tenant;
                if (keysOnly)
                {
                    return;
                }
                baseValue = sample.baseValue;
            }
        }

        internal class BasePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInheritedKeys.Base, BaseUnmanaged>
        {

            internal BasePlugin() : base("global::CorpusInheritedKeys.Base", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Base struct
                var BaseStructMembers = new StructMember[]
                {
                    new StructMember("tenant", dtf.GetPrimitiveType<int>(), isKey: true, id: 0),
                    new StructMember("baseValue", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<BaseUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInheritedKeys::Base")
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
    public class BaseSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInheritedKeys.Base>
    {
        public BaseSupport() : base(
            new Implementation.BasePlugin(),
            new Lazy<DynamicType>(() =>Implementation.BasePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BaseSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BaseSupport, global::CorpusInheritedKeys.Base>();

    }

    namespace Implementation
    {

        public struct DerivedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInheritedKeys.Derived>
        {
            private global::CorpusInheritedKeys.Implementation.BaseUnmanaged parent;

            private int localId;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                parent.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusInheritedKeys.Derived sample, bool keysOnly = false)
            {

                sample.localId = localId;
                parent.FromNative(sample, keysOnly);
                if (keysOnly)
                {
                    return;
                }
                sample.payload = payload;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                parent.Initialize(allocatePointers, allocateMemory);
                localId = (int) (0);
                payload = (int) (0);
            }

            public void ToNative(global::CorpusInheritedKeys.Derived sample, bool keysOnly = false)
            {
                localId = sample.localId;
                parent.ToNative(sample, keysOnly);
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class DerivedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInheritedKeys.Derived, DerivedUnmanaged>
        {

            internal DerivedPlugin() : base("global::CorpusInheritedKeys.Derived", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Derived struct
                var DerivedStructMembers = new StructMember[]
                {
                    new StructMember("localId", dtf.GetPrimitiveType<int>(), isKey: true, id: 2),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<DerivedUnmanaged>(
                    dtf.BuildStruct()
                    .WithParent((StructType) global::CorpusInheritedKeys.BaseSupport.Instance.GetDynamicTypeInternal(isPublic))
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInheritedKeys::Derived")
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
    public class DerivedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInheritedKeys.Derived>
    {
        public DerivedSupport() : base(
            new Implementation.DerivedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DerivedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DerivedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DerivedSupport, global::CorpusInheritedKeys.Derived>();

    }

} // namespace CorpusInheritedKeys

