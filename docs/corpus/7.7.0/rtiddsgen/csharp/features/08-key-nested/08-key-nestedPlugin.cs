/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-nested.idl
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

namespace CorpusNestedKeys
{

    namespace Implementation
    {

        public struct InnerUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNestedKeys.Inner>
        {

            private int code;
            private int revision;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNestedKeys.Inner sample, bool keysOnly = false)
            {

                sample.code = code;
                if (keysOnly)
                {
                    return;
                }
                sample.revision = revision;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                code = (int) (0);
                revision = (int) (0);
            }

            public void ToNative(global::CorpusNestedKeys.Inner sample, bool keysOnly = false)
            {
                code = sample.code;
                if (keysOnly)
                {
                    return;
                }
                revision = sample.revision;
            }
        }

        internal class InnerPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNestedKeys.Inner, InnerUnmanaged>
        {

            internal InnerPlugin() : base("global::CorpusNestedKeys.Inner", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Inner struct
                var InnerStructMembers = new StructMember[]
                {
                    new StructMember("code", dtf.GetPrimitiveType<int>(), isKey: true, id: 0),
                    new StructMember("revision", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<InnerUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNestedKeys::Inner")
                    .AddMembers(InnerStructMembers));

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
    public class InnerSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNestedKeys.Inner>
    {
        public InnerSupport() : base(
            new Implementation.InnerPlugin(),
            new Lazy<DynamicType>(() =>Implementation.InnerPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static InnerSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<InnerSupport, global::CorpusNestedKeys.Inner>();

    }

    namespace Implementation
    {

        public struct OuterUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNestedKeys.Outer>
        {

            private global::CorpusNestedKeys.Implementation.InnerUnmanaged identity;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                identity.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusNestedKeys.Outer sample, bool keysOnly = false)
            {

                identity.FromNative(sample.identity, keysOnly: keysOnly);
                if (keysOnly)
                {
                    return;
                }
                sample.payload = payload;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                identity.Initialize(allocatePointers, allocateMemory);
                payload = (int) (0);
            }

            public void ToNative(global::CorpusNestedKeys.Outer sample, bool keysOnly = false)
            {
                identity.ToNative(sample.identity, keysOnly: keysOnly);
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class OuterPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNestedKeys.Outer, OuterUnmanaged>
        {

            internal OuterPlugin() : base("global::CorpusNestedKeys.Outer", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Outer struct
                var OuterStructMembers = new StructMember[]
                {
                    new StructMember("identity", global::CorpusNestedKeys.InnerSupport.Instance.GetDynamicTypeInternal(isPublic), isKey: true, id: 0),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<OuterUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNestedKeys::Outer")
                    .AddMembers(OuterStructMembers));

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
    public class OuterSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNestedKeys.Outer>
    {
        public OuterSupport() : base(
            new Implementation.OuterPlugin(),
            new Lazy<DynamicType>(() =>Implementation.OuterPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static OuterSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<OuterSupport, global::CorpusNestedKeys.Outer>();

    }

} // namespace CorpusNestedKeys

