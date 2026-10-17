/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-autoid-hash.idl
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

namespace CorpusAutoId
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusAutoId.Sample>
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

            public void FromNative(global::CorpusAutoId.Sample sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) (0);
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusAutoId.Sample sample, bool keysOnly = false)
            {
                id = sample.id;
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusAutoId.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusAutoId.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), isMustUnderstand: true, id: 75475440),
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 206680604)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusAutoId::Sample")
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
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusAutoId.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusAutoId.Sample>();

    }

} // namespace CorpusAutoId

