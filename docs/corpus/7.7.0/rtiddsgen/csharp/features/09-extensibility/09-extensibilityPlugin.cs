/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-extensibility.idl
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

namespace CorpusExtensibility
{

    namespace Implementation
    {

        public struct AppendableUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusExtensibility.Appendable>
        {

            private int id;
            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(global::CorpusExtensibility.Appendable sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusExtensibility.Appendable sample, bool keysOnly = false)
            {
                id = sample.id;
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class AppendablePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusExtensibility.Appendable, AppendableUnmanaged>
        {

            internal AppendablePlugin() : base("global::CorpusExtensibility.Appendable", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Appendable struct
                var AppendableStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<AppendableUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusExtensibility::Appendable")
                    .AddMembers(AppendableStructMembers));

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
    public class AppendableSupport : Rti.Dds.Topics.TypeSupport<global::CorpusExtensibility.Appendable>
    {
        public AppendableSupport() : base(
            new Implementation.AppendablePlugin(),
            new Lazy<DynamicType>(() =>Implementation.AppendablePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static AppendableSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<AppendableSupport, global::CorpusExtensibility.Appendable>();

    }

    namespace Implementation
    {

        public struct FinalUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusExtensibility.Final>
        {

            private int id;
            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(global::CorpusExtensibility.Final sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusExtensibility.Final sample, bool keysOnly = false)
            {
                id = sample.id;
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class FinalPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusExtensibility.Final, FinalUnmanaged>
        {

            internal FinalPlugin() : base("global::CorpusExtensibility.Final", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Final struct
                var FinalStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<FinalUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Final)
                    .WithName("CorpusExtensibility::Final")
                    .AddMembers(FinalStructMembers));

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
    public class FinalSupport : Rti.Dds.Topics.TypeSupport<global::CorpusExtensibility.Final>
    {
        public FinalSupport() : base(
            new Implementation.FinalPlugin(),
            new Lazy<DynamicType>(() =>Implementation.FinalPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static FinalSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<FinalSupport, global::CorpusExtensibility.Final>();

    }

    namespace Implementation
    {

        public struct MutableUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusExtensibility.Mutable>
        {

            private int id;
            private NativeUnmanagedOptional optionalValue;

            public void Destroy(bool optionalsOnly)
            {
                optionalValue.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusExtensibility.Mutable sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.optionalValue = optionalValue.FromNative<int>();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
            }

            public void ToNative(global::CorpusExtensibility.Mutable sample, bool keysOnly = false)
            {
                id = sample.id;
                optionalValue.ToNative<int>(sample.optionalValue);
            }
        }

        internal class MutablePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusExtensibility.Mutable, MutableUnmanaged>
        {

            internal MutablePlugin() : base("global::CorpusExtensibility.Mutable", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Mutable struct
                var MutableStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("optionalValue", dtf.GetPrimitiveType<int>(), isOptional: true, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<MutableUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Mutable)
                    .WithName("CorpusExtensibility::Mutable")
                    .AddMembers(MutableStructMembers));

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
    public class MutableSupport : Rti.Dds.Topics.TypeSupport<global::CorpusExtensibility.Mutable>
    {
        public MutableSupport() : base(
            new Implementation.MutablePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MutablePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MutableSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MutableSupport, global::CorpusExtensibility.Mutable>();

    }

} // namespace CorpusExtensibility

