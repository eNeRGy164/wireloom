/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-string-sequence-aliases.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

namespace RuntimeUnionStringSequences
{

    namespace Implementation
    {

        public struct NarrowTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<RuntimeUnionStringSequences.NarrowTexts>
        {

            private NativeStringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(RuntimeUnionStringSequences.NarrowTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(RuntimeUnionStringSequences.NarrowTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class NarrowTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<RuntimeUnionStringSequences.NarrowTexts, NarrowTextsUnmanaged>
        {
            internal NarrowTextsPlugin() : base("RuntimeUnionStringSequences.NarrowTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<NarrowTextsUnmanaged>(
                    dtf,
                    "NarrowTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 255)), ((int)3)));
            }
        }
    }
    public class NarrowTextsSupport : Rti.Dds.Topics.TypeSupport<RuntimeUnionStringSequences.NarrowTexts>
    {
        public NarrowTextsSupport() : base(
            new Implementation.NarrowTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.NarrowTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NarrowTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NarrowTextsSupport, RuntimeUnionStringSequences.NarrowTexts>();
    }

    namespace Implementation
    {

        public struct WideTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<RuntimeUnionStringSequences.WideTexts>
        {

            private NativeWstringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(RuntimeUnionStringSequences.WideTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(RuntimeUnionStringSequences.WideTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class WideTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<RuntimeUnionStringSequences.WideTexts, WideTextsUnmanaged>
        {
            internal WideTextsPlugin() : base("RuntimeUnionStringSequences.WideTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<WideTextsUnmanaged>(
                    dtf,
                    "WideTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 255)), ((int)3)));
            }
        }
    }
    public class WideTextsSupport : Rti.Dds.Topics.TypeSupport<RuntimeUnionStringSequences.WideTexts>
    {
        public WideTextsSupport() : base(
            new Implementation.WideTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextsSupport, RuntimeUnionStringSequences.WideTexts>();
    }

    namespace Implementation
    {

        public struct BoundedNarrowTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<RuntimeUnionStringSequences.BoundedNarrowTexts>
        {

            private NativeStringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(RuntimeUnionStringSequences.BoundedNarrowTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(RuntimeUnionStringSequences.BoundedNarrowTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 8));
            }
        }

        internal class BoundedNarrowTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<RuntimeUnionStringSequences.BoundedNarrowTexts, BoundedNarrowTextsUnmanaged>
        {
            internal BoundedNarrowTextsPlugin() : base("RuntimeUnionStringSequences.BoundedNarrowTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedNarrowTextsUnmanaged>(
                    dtf,
                    "BoundedNarrowTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 8)), ((int)3)));
            }
        }
    }
    public class BoundedNarrowTextsSupport : Rti.Dds.Topics.TypeSupport<RuntimeUnionStringSequences.BoundedNarrowTexts>
    {
        public BoundedNarrowTextsSupport() : base(
            new Implementation.BoundedNarrowTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedNarrowTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedNarrowTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedNarrowTextsSupport, RuntimeUnionStringSequences.BoundedNarrowTexts>();
    }

    namespace Implementation
    {

        public struct BoundedWideTextsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<RuntimeUnionStringSequences.BoundedWideTexts>
        {

            private NativeWstringSeq Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(RuntimeUnionStringSequences.BoundedWideTexts sample, bool keysOnly = false)
            {

                Value.FromNative(sample.Value);
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 4), allocateMemory: allocateMemory);
            }

            public void ToNative(RuntimeUnionStringSequences.BoundedWideTexts sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 4));
            }
        }

        internal class BoundedWideTextsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<RuntimeUnionStringSequences.BoundedWideTexts, BoundedWideTextsUnmanaged>
        {
            internal BoundedWideTextsPlugin() : base("RuntimeUnionStringSequences.BoundedWideTexts", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<BoundedWideTextsUnmanaged>(
                    dtf,
                    "BoundedWideTexts",
                    tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 4)), ((int)3)));
            }
        }
    }
    public class BoundedWideTextsSupport : Rti.Dds.Topics.TypeSupport<RuntimeUnionStringSequences.BoundedWideTexts>
    {
        public BoundedWideTextsSupport() : base(
            new Implementation.BoundedWideTextsPlugin(),
            new Lazy<DynamicType>(() =>Implementation.BoundedWideTextsPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static BoundedWideTextsSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<BoundedWideTextsSupport, RuntimeUnionStringSequences.BoundedWideTexts>();
    }

    namespace Implementation
    {

        public struct DirectStringSequencesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<RuntimeUnionStringSequences.DirectStringSequences>
        {

            private int _d;

            private NativeStringSeq narrow;
            private NativeWstringSeq wide;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                narrow.Destroy();
                wide.Destroy();
            }

            public void FromNative(RuntimeUnionStringSequences.DirectStringSequences sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.narrow = new Rti.Types.Sequence<string>();
                    }
                    narrow.FromNative(sample.narrow);
                    break;
                    case 1:
                    if(sample.Discriminator != _d)
                    {
                        sample.wide = new Rti.Types.Sequence<string>();
                    }
                    wide.FromNative(sample.wide);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = DirectStringSequences.DefaultDiscriminator;
                narrow.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 255), allocateMemory: allocateMemory);
                wide.Initialize(max: ((int)3), absoluteMax: ((int)3), maxStrLen: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(RuntimeUnionStringSequences.DirectStringSequences sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    narrow.ToNative(sample.narrow, ((int) 255));
                    break;
                    case 1:
                    wide.ToNative(sample.wide, ((int) 255));
                    break;
                }
            }
        }

        internal class DirectStringSequencesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<RuntimeUnionStringSequences.DirectStringSequences, DirectStringSequencesUnmanaged>
        {
            internal DirectStringSequencesPlugin() : base("RuntimeUnionStringSequences.DirectStringSequences", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // DirectStringSequences union
                var DirectStringSequencesStructMembers = new UnionMember[]
                {
                    new UnionMember("narrow", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateString(((int) 255)), ((int)3)), new int[] {(int) 0}, id: 1),
                    new UnionMember("wide", tsf.CreateSequenceWithAccessInfo(dtf, dtf.CreateWideString(((int) 255)), ((int)3)), new int[] {(int) 1}, id: 2)
                };

                return tsf.CreateTypeWithAccessInfo<DirectStringSequencesUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("RuntimeUnionStringSequences::DirectStringSequences")
                    .AddMembers(DirectStringSequencesStructMembers));
            }
        }
    }
    public class DirectStringSequencesSupport : Rti.Dds.Topics.TypeSupport<RuntimeUnionStringSequences.DirectStringSequences>
    {
        public DirectStringSequencesSupport() : base(
            new Implementation.DirectStringSequencesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.DirectStringSequencesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static DirectStringSequencesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DirectStringSequencesSupport, RuntimeUnionStringSequences.DirectStringSequences>();
    }

    namespace Implementation
    {

        public struct StringSequenceAliasesUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<RuntimeUnionStringSequences.StringSequenceAliases>
        {

            private int _d;

            private RuntimeUnionStringSequences.Implementation.NarrowTextsUnmanaged narrow;
            private RuntimeUnionStringSequences.Implementation.WideTextsUnmanaged wide;
            private RuntimeUnionStringSequences.Implementation.BoundedNarrowTextsUnmanaged boundedNarrow;
            private RuntimeUnionStringSequences.Implementation.BoundedWideTextsUnmanaged boundedWide;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                narrow.Destroy(optionalsOnly);
                wide.Destroy(optionalsOnly);
                boundedNarrow.Destroy(optionalsOnly);
                boundedWide.Destroy(optionalsOnly);
            }

            public void FromNative(RuntimeUnionStringSequences.StringSequenceAliases sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    if(sample.Discriminator != _d)
                    {
                        sample.narrow = new RuntimeUnionStringSequences.NarrowTexts();
                    }
                    narrow.FromNative(sample.narrow, keysOnly: false);
                    break;
                    case 1:
                    if(sample.Discriminator != _d)
                    {
                        sample.wide = new RuntimeUnionStringSequences.WideTexts();
                    }
                    wide.FromNative(sample.wide, keysOnly: false);
                    break;
                    case 2:
                    if(sample.Discriminator != _d)
                    {
                        sample.boundedNarrow = new RuntimeUnionStringSequences.BoundedNarrowTexts();
                    }
                    boundedNarrow.FromNative(sample.boundedNarrow, keysOnly: false);
                    break;
                    case 3:
                    if(sample.Discriminator != _d)
                    {
                        sample.boundedWide = new RuntimeUnionStringSequences.BoundedWideTexts();
                    }
                    boundedWide.FromNative(sample.boundedWide, keysOnly: false);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = StringSequenceAliases.DefaultDiscriminator;
                narrow.Initialize(allocatePointers, allocateMemory);
                wide.Initialize(allocatePointers, allocateMemory);
                boundedNarrow.Initialize(allocatePointers, allocateMemory);
                boundedWide.Initialize(allocatePointers, allocateMemory);
            }

            public void ToNative(RuntimeUnionStringSequences.StringSequenceAliases sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    narrow.ToNative(sample.narrow, keysOnly: false);
                    break;
                    case 1:
                    wide.ToNative(sample.wide, keysOnly: false);
                    break;
                    case 2:
                    boundedNarrow.ToNative(sample.boundedNarrow, keysOnly: false);
                    break;
                    case 3:
                    boundedWide.ToNative(sample.boundedWide, keysOnly: false);
                    break;
                }
            }
        }

        internal class StringSequenceAliasesPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<RuntimeUnionStringSequences.StringSequenceAliases, StringSequenceAliasesUnmanaged>
        {
            internal StringSequenceAliasesPlugin() : base("RuntimeUnionStringSequences.StringSequenceAliases", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // StringSequenceAliases union
                var StringSequenceAliasesStructMembers = new UnionMember[]
                {
                    new UnionMember("narrow", RuntimeUnionStringSequences.NarrowTextsSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 0}, id: 1),
                    new UnionMember("wide", RuntimeUnionStringSequences.WideTextsSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 1}, id: 2),
                    new UnionMember("boundedNarrow", RuntimeUnionStringSequences.BoundedNarrowTextsSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 2}, id: 3),
                    new UnionMember("boundedWide", RuntimeUnionStringSequences.BoundedWideTextsSupport.Instance.GetDynamicTypeInternal(isPublic), new int[] {(int) 3}, id: 4)
                };

                return tsf.CreateTypeWithAccessInfo<StringSequenceAliasesUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<int>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("RuntimeUnionStringSequences::StringSequenceAliases")
                    .AddMembers(StringSequenceAliasesStructMembers));
            }
        }
    }
    public class StringSequenceAliasesSupport : Rti.Dds.Topics.TypeSupport<RuntimeUnionStringSequences.StringSequenceAliases>
    {
        public StringSequenceAliasesSupport() : base(
            new Implementation.StringSequenceAliasesPlugin(),
            new Lazy<DynamicType>(() =>Implementation.StringSequenceAliasesPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StringSequenceAliasesSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StringSequenceAliasesSupport, RuntimeUnionStringSequences.StringSequenceAliases>();
    }

} // namespace RuntimeUnionStringSequences

