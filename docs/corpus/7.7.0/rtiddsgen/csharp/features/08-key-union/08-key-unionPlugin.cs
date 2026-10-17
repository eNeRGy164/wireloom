/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 08-key-union.idl
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

namespace CorpusUnionKeys
{

    namespace Implementation
    {

        public struct IdentityUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionKeys.Identity>
        {
            private int _d;

            private int number;
            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(global::CorpusUnionKeys.Identity sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    sample.number = number;
                    break;
                    case 1:
                    sample.text = text.FromNative();
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Identity.DefaultDiscriminator;
                number = (int) (0);
                text.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnionKeys.Identity sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    number = sample.number;
                    break;
                    case 1:
                    text.ToNative(sample.text, ((int) 255));
                    break;
                }
            }
        }

        internal class IdentityPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionKeys.Identity, IdentityUnmanaged>
        {

            internal IdentityPlugin() : base("global::CorpusUnionKeys.Identity", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Identity union
                var IdentityStructMembers = new UnionMember[]
                {
                    new UnionMember("number", dtf.GetPrimitiveType<int>(), new int[] {(int) 0}, id: 1),
                    new UnionMember("text", dtf.CreateString(((int) 255)), new int[] {(int) 1}, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<IdentityUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionKeys::Identity")
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
    public class IdentitySupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionKeys.Identity>
    {
        public IdentitySupport() : base(
            new Implementation.IdentityPlugin(),
            new Lazy<DynamicType>(() =>Implementation.IdentityPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static IdentitySupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IdentitySupport, global::CorpusUnionKeys.Identity>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionKeys.Sample>
        {

            private global::CorpusUnionKeys.Implementation.IdentityUnmanaged identity;
            private int payload;

            public void Destroy(bool optionalsOnly)
            {
                identity.Destroy(optionalsOnly);
            }

            public void FromNative(global::CorpusUnionKeys.Sample sample, bool keysOnly = false)
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

            public void ToNative(global::CorpusUnionKeys.Sample sample, bool keysOnly = false)
            {
                identity.ToNative(sample.identity, keysOnly: keysOnly);
                if (keysOnly)
                {
                    return;
                }
                payload = sample.payload;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionKeys.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusUnionKeys.Sample", isKeyed: true, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("identity", global::CorpusUnionKeys.IdentitySupport.Instance.GetDynamicTypeInternal(isPublic), isKey: true, id: 0),
                    new StructMember("payload", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionKeys::Sample")
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
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionKeys.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusUnionKeys.Sample>();

    }

} // namespace CorpusUnionKeys

