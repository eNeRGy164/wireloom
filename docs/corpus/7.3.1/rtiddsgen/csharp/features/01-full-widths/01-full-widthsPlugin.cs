/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-full-widths.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

namespace CorpusFullWidths
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusFullWidths.Sample>
        {

            private sbyte i8;
            private short i16;
            private int i32;
            private long i64;
            private byte u8;
            private ushort u16;
            private uint u32;
            private ulong u64;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusFullWidths.Sample sample, bool keysOnly = false)
            {

                sample.i8 = i8;
                sample.i16 = i16;
                sample.i32 = i32;
                sample.i64 = i64;
                sample.u8 = u8;
                sample.u16 = u16;
                sample.u32 = u32;
                sample.u64 = u64;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                i8 = (sbyte) (0);
                i16 = (short) (0);
                i32 = (int) (0);
                i64 = (long) (0L);
                u8 = (byte) (0);
                u16 = (ushort) (0);
                u32 = (uint) (0u);
                u64 = (ulong) (0uL);
            }

            public void ToNative(global::CorpusFullWidths.Sample sample, bool keysOnly = false)
            {
                i8 = sample.i8;
                i16 = sample.i16;
                i32 = sample.i32;
                i64 = sample.i64;
                u8 = sample.u8;
                u16 = sample.u16;
                u32 = sample.u32;
                u64 = sample.u64;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusFullWidths.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusFullWidths.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("i8", dtf.GetPrimitiveType<sbyte>(), id: 0),
                    new StructMember("i16", dtf.GetPrimitiveType<short>(), id: 1),
                    new StructMember("i32", dtf.GetPrimitiveType<int>(), id: 2),
                    new StructMember("i64", dtf.GetPrimitiveType<long>(), id: 3),
                    new StructMember("u8", dtf.GetPrimitiveType<byte>(), id: 4),
                    new StructMember("u16", dtf.GetPrimitiveType<ushort>(), id: 5),
                    new StructMember("u32", dtf.GetPrimitiveType<uint>(), id: 6),
                    new StructMember("u64", dtf.GetPrimitiveType<ulong>(), id: 7)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusFullWidths::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusFullWidths.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusFullWidths.Sample>();

    }

} // namespace CorpusFullWidths

