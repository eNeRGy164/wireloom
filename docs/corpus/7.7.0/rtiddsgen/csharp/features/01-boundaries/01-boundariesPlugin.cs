/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-boundaries.idl
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

namespace CorpusPrimitiveBoundaries
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusPrimitiveBoundaries.Sample>
        {

            private short shortValue;
            private int longValue;
            private long longLongValue;
            private sbyte int8Value;
            private long int64Value;
            private float floatValue;
            private double doubleValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusPrimitiveBoundaries.Sample sample, bool keysOnly = false)
            {

                sample.shortValue = shortValue;
                sample.longValue = longValue;
                sample.longLongValue = longLongValue;
                sample.int8Value = int8Value;
                sample.int64Value = int64Value;
                sample.floatValue = floatValue;
                sample.doubleValue = doubleValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                shortValue = (short) (0);
                longValue = (int) (0);
                longLongValue = (long) (0L);
                int8Value = (sbyte) (0);
                int64Value = (long) (0L);
                floatValue = (float) (0.0f);
                doubleValue = (double) (0.0);
            }

            public void ToNative(global::CorpusPrimitiveBoundaries.Sample sample, bool keysOnly = false)
            {
                shortValue = sample.shortValue;
                longValue = sample.longValue;
                longLongValue = sample.longLongValue;
                int8Value = sample.int8Value;
                int64Value = sample.int64Value;
                floatValue = sample.floatValue;
                doubleValue = sample.doubleValue;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusPrimitiveBoundaries.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusPrimitiveBoundaries.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("shortValue", dtf.GetPrimitiveType<short>(), id: 0),
                    new StructMember("longValue", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("longLongValue", dtf.GetPrimitiveType<long>(), id: 2),
                    new StructMember("int8Value", dtf.GetPrimitiveType<sbyte>(), id: 3),
                    new StructMember("int64Value", dtf.GetPrimitiveType<long>(), id: 4),
                    new StructMember("floatValue", dtf.GetPrimitiveType<float>(), id: 5),
                    new StructMember("doubleValue", dtf.GetPrimitiveType<double>(), id: 6)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusPrimitiveBoundaries::Sample")
                    .AddMembers(SampleStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int16Value = (short) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int16Value = (short) short.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int16Value = (short) short.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int16,
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
                    defaultValueParam.Int64Value = (long) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int64Value = (long) long.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int64Value = (long) long.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int64,
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
                        3,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int64Value = (long) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int64Value = (long) long.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int64Value = (long) long.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int64,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        4,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Float32Value = (float) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Float32Value = (float) float.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Float32Value = (float) float.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Float32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        5,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Float64Value = (double) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Float64Value = (double) double.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Float64Value = (double) double.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Float64,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        6,
                        annotations);
                }

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusPrimitiveBoundaries.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusPrimitiveBoundaries.Sample>();

    }

} // namespace CorpusPrimitiveBoundaries

