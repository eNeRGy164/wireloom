/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-int8-boundaries.idl
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

namespace CorpusInt8Boundaries
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInt8Boundaries.Sample>
        {

            private sbyte minimum;
            private sbyte maximum;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInt8Boundaries.Sample sample, bool keysOnly = false)
            {

                sample.minimum = minimum;
                sample.maximum = maximum;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                minimum = (sbyte) (0);
                maximum = (sbyte) (0);
            }

            public void ToNative(global::CorpusInt8Boundaries.Sample sample, bool keysOnly = false)
            {
                minimum = sample.minimum;
                maximum = sample.maximum;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInt8Boundaries.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusInt8Boundaries.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("minimum", dtf.GetPrimitiveType<sbyte>(), id: 0),
                    new StructMember("maximum", dtf.GetPrimitiveType<sbyte>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInt8Boundaries::Sample")
                    .AddMembers(SampleStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int8Value = (sbyte) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int8Value = (sbyte) sbyte.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int8Value = (sbyte) sbyte.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int8,
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
                    defaultValueParam.Int8Value = (sbyte) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int8Value = (sbyte) sbyte.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int8Value = (sbyte) sbyte.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int8,
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
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInt8Boundaries.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusInt8Boundaries.Sample>();

    }

} // namespace CorpusInt8Boundaries

