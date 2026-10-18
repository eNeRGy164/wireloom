/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 01-primitives.idl
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
                xTypesComplianceMask = 0x0000068C;
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
                    new StructMember("octetValue", dtf.GetPrimitiveType<byte>(), id: 10),
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

