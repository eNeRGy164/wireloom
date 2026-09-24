/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-keys.idl
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

namespace CorpusKeys
{

    namespace Implementation
    {

        public struct SimpleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusKeys.Simple>
        {

            private int id;
            private NativeString name;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                name.Destroy();
            }

            public void FromNative(global::CorpusKeys.Simple sample, bool keysOnly = false)
            {

                sample.id = id;
                if (keysOnly)
                {
                    return;
                }
                sample.name = name.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                name.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusKeys.Simple sample, bool keysOnly = false)
            {
                id = sample.id;
                if (keysOnly)
                {
                    return;
                }
                name.ToNative(sample.name, ((int) 16));
            }
        }

        internal class SimplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusKeys.Simple, SimpleUnmanaged>
        {

            internal SimplePlugin() : base("global::CorpusKeys.Simple", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Simple struct
                var SimpleStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), isKey: true, id: 0),
                    new StructMember("name", dtf.CreateString(((int) 16)), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SimpleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusKeys::Simple")
                    .AddMembers(SimpleStructMembers));

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
    public class SimpleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusKeys.Simple>
    {
        public SimpleSupport() : base(
            new Implementation.SimplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SimplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SimpleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SimpleSupport, global::CorpusKeys.Simple>();

    }

    namespace Implementation
    {

        public struct IdentityUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusKeys.Identity>
        {

            private int tenant;
            private NativeString name;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                name.Destroy();
            }

            public void FromNative(global::CorpusKeys.Identity sample, bool keysOnly = false)
            {

                sample.tenant = tenant;
                sample.name = name.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                tenant = (int) (0);
                name.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusKeys.Identity sample, bool keysOnly = false)
            {
                tenant = sample.tenant;
                name.ToNative(sample.name, ((int) 16));
            }
        }

        internal class IdentityPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusKeys.Identity, IdentityUnmanaged>
        {

            internal IdentityPlugin() : base("global::CorpusKeys.Identity", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Identity struct
                var IdentityStructMembers = new StructMember[]
                {
                    new StructMember("tenant", dtf.GetPrimitiveType<int>(), isKey: true, id: 0),
                    new StructMember("name", dtf.CreateString(((int) 16)), isKey: true, id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<IdentityUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusKeys::Identity")
                    .AddMembers(IdentityStructMembers));

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
    public class IdentitySupport : Rti.Dds.Topics.TypeSupport<global::CorpusKeys.Identity>
    {
        public IdentitySupport() : base(
            new Implementation.IdentityPlugin(),
            new Lazy<DynamicType>(() =>Implementation.IdentityPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static IdentitySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IdentitySupport, global::CorpusKeys.Identity>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusKeys.Sample>
        {

            private global::CorpusKeys.Implementation.IdentityUnmanaged identity;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                identity.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusKeys.Sample sample, bool keysOnly = false)
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

            public void ToNative(global::CorpusKeys.Sample sample, bool keysOnly = false)
            {
                identity.ToNative(sample.identity, keysOnly: keysOnly);
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusKeys.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusKeys.Sample", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("identity", global::CorpusKeys.IdentitySupport.Instance.GetDynamicTypeInternal(isPublic), isKey: true, id: 0),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusKeys::Sample")
                    .AddMembers(SampleStructMembers));

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
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusKeys.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusKeys.Sample>();

    }

} // namespace CorpusKeys

