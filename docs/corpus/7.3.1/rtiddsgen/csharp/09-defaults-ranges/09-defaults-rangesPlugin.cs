/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-defaults-ranges.idl
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

namespace CorpusDefaultsAndRanges
{

    namespace Implementation
    {
        internal class ColorPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public ColorPlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                return dtf.BuildEnum()
                .WithName("CorpusDefaultsAndRanges::Color")
                .AddMember(new EnumMember("GREEN", 0))
                .AddMember(new EnumMember("RED", 1))
                .AddMember(new EnumMember("BLUE", 2))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
            }
        }
    }

    public class ColorSupport : Rti.Dds.Topics.TypeSupport<global::CorpusDefaultsAndRanges.Color>
    {
        public ColorSupport() : base(
            new Implementation.ColorPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ColorPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ColorSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ColorSupport, global::CorpusDefaultsAndRanges.Color>();

    }

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusDefaultsAndRanges.Sample>
        {

            private int value;
            private int ranged;
            private global::CorpusDefaultsAndRanges.Color color;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusDefaultsAndRanges.Sample sample, bool keysOnly = false)
            {

                sample.value = value;
                sample.ranged = ranged;
                sample.color = color;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (50);
                ranged = (int) (0);
                color = (global::CorpusDefaultsAndRanges.Color) (2);
            }

            public void ToNative(global::CorpusDefaultsAndRanges.Sample sample, bool keysOnly = false)
            {
                value = sample.value;
                ranged = sample.ranged;
                color = sample.color;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusDefaultsAndRanges.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusDefaultsAndRanges.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
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
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("ranged", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("color", global::CorpusDefaultsAndRanges.ColorSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusDefaultsAndRanges::Sample")
                    .AddMembers(SampleStructMembers));

                return result;
            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusDefaultsAndRanges.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusDefaultsAndRanges.Sample>();

    }

} // namespace CorpusDefaultsAndRanges

