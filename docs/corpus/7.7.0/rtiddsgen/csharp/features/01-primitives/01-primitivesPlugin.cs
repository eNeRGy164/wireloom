/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-primitives.idl
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

namespace CorpusPrimitives
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusPrimitives.Sample>
        {

            private short signed16;
            private int signed32;
            private long signed64;
            private ushort unsigned16;
            private uint unsigned32;
            private ulong unsigned64;
            private sbyte fixedInt8;
            private long fixedInt64;
            private byte fixedUint8;
            private ulong fixedUint64;
            private byte octetValue;
            private byte boolValue;
            private byte charValue;
            private short wideCharValue;
            private float floatValue;
            private double doubleValue;
            private LongDouble longDoubleValue;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusPrimitives.Sample sample, bool keysOnly = false)
            {

                sample.signed16 = signed16;
                sample.signed32 = signed32;
                sample.signed64 = signed64;
                sample.unsigned16 = unsigned16;
                sample.unsigned32 = unsigned32;
                sample.unsigned64 = unsigned64;
                sample.fixedInt8 = fixedInt8;
                sample.fixedInt64 = fixedInt64;
                sample.fixedUint8 = fixedUint8;
                sample.fixedUint64 = fixedUint64;
                sample.octetValue = octetValue;
                sample.boolValue = Convert.ToBoolean(boolValue);
                sample.charValue = NativeChar.FromUtf8(charValue);
                sample.wideCharValue = (char) wideCharValue;
                sample.floatValue = floatValue;
                sample.doubleValue = doubleValue;
                sample.longDoubleValue = longDoubleValue;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                signed16 = (short) (0);
                signed32 = (int) (0);
                signed64 = (long) (0L);
                unsigned16 = (ushort) (0);
                unsigned32 = (uint) (0u);
                unsigned64 = (ulong) (0uL);
                fixedInt8 = (sbyte) (0);
                fixedInt64 = (long) (0L);
                fixedUint8 = (byte) (0);
                fixedUint64 = (ulong) (0uL);
                octetValue = (byte) (0);
                boolValue = 0;
                charValue = (byte) 0;
                wideCharValue = (byte) 0;
                floatValue = (float) (0.0f);
                doubleValue = (double) (0.0);
                longDoubleValue = (LongDouble) (0);
            }

            public void ToNative(global::CorpusPrimitives.Sample sample, bool keysOnly = false)
            {
                signed16 = sample.signed16;
                signed32 = sample.signed32;
                signed64 = sample.signed64;
                unsigned16 = sample.unsigned16;
                unsigned32 = sample.unsigned32;
                unsigned64 = sample.unsigned64;
                fixedInt8 = sample.fixedInt8;
                fixedInt64 = sample.fixedInt64;
                fixedUint8 = sample.fixedUint8;
                fixedUint64 = sample.fixedUint64;
                octetValue = sample.octetValue;
                boolValue = Convert.ToByte(sample.boolValue);
                charValue = NativeChar.ToUtf8(sample.charValue);
                wideCharValue = (short) sample.wideCharValue;
                floatValue = sample.floatValue;
                doubleValue = sample.doubleValue;
                longDoubleValue = sample.longDoubleValue;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusPrimitives.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusPrimitives.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("signed16", dtf.GetPrimitiveType<short>(), id: 0),
                    new StructMember("signed32", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("signed64", dtf.GetPrimitiveType<long>(), id: 2),
                    new StructMember("unsigned16", dtf.GetPrimitiveType<ushort>(), id: 3),
                    new StructMember("unsigned32", dtf.GetPrimitiveType<uint>(), id: 4),
                    new StructMember("unsigned64", dtf.GetPrimitiveType<ulong>(), id: 5),
                    new StructMember("fixedInt8", dtf.GetPrimitiveType<sbyte>(), id: 6),
                    new StructMember("fixedInt64", dtf.GetPrimitiveType<long>(), id: 7),
                    new StructMember("fixedUint8", dtf.GetPrimitiveType<byte>(), id: 8),
                    new StructMember("fixedUint64", dtf.GetPrimitiveType<ulong>(), id: 9),
                    new StructMember("octetValue", dtf.GetPrimitiveType<Octet>(), id: 10),
                    new StructMember("boolValue", dtf.GetPrimitiveType<bool>(), id: 11),
                    new StructMember("charValue", dtf.GetPrimitiveType<char>(), id: 12),
                    new StructMember("wideCharValue", dtf.GetPrimitiveType<DynamicTypeFactory.WideCharType>(), id: 13),
                    new StructMember("floatValue", dtf.GetPrimitiveType<float>(), id: 14),
                    new StructMember("doubleValue", dtf.GetPrimitiveType<double>(), id: 15),
                    new StructMember("longDoubleValue", dtf.GetPrimitiveType<LongDouble>(), id: 16)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusPrimitives::Sample")
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
                    defaultValueParam.Uint16Value = (ushort) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Uint16Value = (ushort) ushort.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Uint16Value = (ushort) ushort.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Uint16,
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
                    defaultValueParam.Uint32Value = (uint) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Uint32Value = (uint) uint.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Uint32Value = (uint) uint.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.UInt32,
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
                    defaultValueParam.Uint64Value = (ulong) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Uint64Value = (ulong) ulong.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Uint64Value = (ulong) ulong.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.UInt64,
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
                        6,
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
                        7,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Uint8Value = (byte) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Uint8Value = (byte) byte.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Uint8Value = (byte) byte.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Uint8,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        8,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Uint64Value = (ulong) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Uint64Value = (ulong) ulong.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Uint64Value = (ulong) ulong.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.UInt64,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        9,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.OctetValue = (byte) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.OctetValue = (byte) byte.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.OctetValue = (byte) byte.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Octet,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        10,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.BoolValue = false;
                    Annotations annotations = new Annotations(
                        TypeKind.Boolean,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        11,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Char8Value = '\0';
                    Annotations annotations = new Annotations(
                        TypeKind.Char8,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        12,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Char16Value = '\0';
                    Annotations annotations = new Annotations(
                        TypeKind.Char16,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        13,
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
                        14,
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
                        15,
                        annotations);
                }

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusPrimitives.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusPrimitives.Sample>();

    }

} // namespace CorpusPrimitives

