/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-string-alias-chains.idl
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

namespace CorpusStringAliasChains
{

    namespace Implementation
    {

        public struct StringAliasChainsSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStringAliasChains.StringAliasChainsSample>
        {

            private NativeString narrow;
            private NativeString narrowChain;
            private NativeString boundedNarrow;
            private NativeString boundedNarrowChain;
            private NativeString boundedNarrowChain2;
            private NativeWstring wide;
            private NativeWstring wideChain;
            private NativeWstring boundedWide;
            private NativeWstring boundedWideChain;
            private NativeWstring boundedWideChain2;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                narrow.Destroy();
                narrowChain.Destroy();
                boundedNarrow.Destroy();
                boundedNarrowChain.Destroy();
                boundedNarrowChain2.Destroy();
                wide.Destroy();
                wideChain.Destroy();
                boundedWide.Destroy();
                boundedWideChain.Destroy();
                boundedWideChain2.Destroy();
            }

            public void FromNative(global::CorpusStringAliasChains.StringAliasChainsSample sample, bool keysOnly = false)
            {

                sample.narrow = narrow.FromNative();
                sample.narrowChain = narrowChain.FromNative();
                sample.boundedNarrow = boundedNarrow.FromNative();
                sample.boundedNarrowChain = boundedNarrowChain.FromNative();
                sample.boundedNarrowChain2 = boundedNarrowChain2.FromNative();
                sample.wide = wide.FromNative();
                sample.wideChain = wideChain.FromNative();
                sample.boundedWide = boundedWide.FromNative();
                sample.boundedWideChain = boundedWideChain.FromNative();
                sample.boundedWideChain2 = boundedWideChain2.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                narrow.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                narrowChain.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                boundedNarrow.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
                boundedNarrowChain.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
                boundedNarrowChain2.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
                wide.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                wideChain.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                boundedWide.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
                boundedWideChain.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
                boundedWideChain2.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStringAliasChains.StringAliasChainsSample sample, bool keysOnly = false)
            {
                narrow.ToNative(sample.narrow, ((int) 255));
                narrowChain.ToNative(sample.narrowChain, ((int) 255));
                boundedNarrow.ToNative(sample.boundedNarrow, ((int) 16));
                boundedNarrowChain.ToNative(sample.boundedNarrowChain, ((int) 16));
                boundedNarrowChain2.ToNative(sample.boundedNarrowChain2, ((int) 16));
                wide.ToNative(sample.wide, ((int) 255));
                wideChain.ToNative(sample.wideChain, ((int) 255));
                boundedWide.ToNative(sample.boundedWide, ((int) 8));
                boundedWideChain.ToNative(sample.boundedWideChain, ((int) 8));
                boundedWideChain2.ToNative(sample.boundedWideChain2, ((int) 8));
            }
        }

        internal class StringAliasChainsSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStringAliasChains.StringAliasChainsSample, StringAliasChainsSampleUnmanaged>
        {

            internal StringAliasChainsSamplePlugin() : base("global::CorpusStringAliasChains.StringAliasChainsSample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // StringAliasChainsSample struct
                var StringAliasChainsSampleStructMembers = new StructMember[]
                {
                    new StructMember("narrow", global::CorpusStringAliasChains.NarrowTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("narrowChain", global::CorpusStringAliasChains.NarrowTextChainSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1),
                    new StructMember("boundedNarrow", global::CorpusStringAliasChains.BoundedNarrowTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 2),
                    new StructMember("boundedNarrowChain", global::CorpusStringAliasChains.BoundedNarrowTextChainSupport.Instance.GetDynamicTypeInternal(isPublic), id: 3),
                    new StructMember("boundedNarrowChain2", global::CorpusStringAliasChains.BoundedNarrowTextChain2Support.Instance.GetDynamicTypeInternal(isPublic), id: 4),
                    new StructMember("wide", global::CorpusStringAliasChains.WideTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 5),
                    new StructMember("wideChain", global::CorpusStringAliasChains.WideTextChainSupport.Instance.GetDynamicTypeInternal(isPublic), id: 6),
                    new StructMember("boundedWide", global::CorpusStringAliasChains.BoundedWideTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 7),
                    new StructMember("boundedWideChain", global::CorpusStringAliasChains.BoundedWideTextChainSupport.Instance.GetDynamicTypeInternal(isPublic), id: 8),
                    new StructMember("boundedWideChain2", global::CorpusStringAliasChains.BoundedWideTextChain2Support.Instance.GetDynamicTypeInternal(isPublic), id: 9)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<StringAliasChainsSampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusStringAliasChains::StringAliasChainsSample")
                    .AddMembers(StringAliasChainsSampleStructMembers));

                return result;
            }
        }
    }
    public class StringAliasChainsSampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStringAliasChains.StringAliasChainsSample>
    {
        public StringAliasChainsSampleSupport() : base(
            new Implementation.StringAliasChainsSamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StringAliasChainsSamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StringAliasChainsSampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StringAliasChainsSampleSupport, global::CorpusStringAliasChains.StringAliasChainsSample>();

    }

} // namespace CorpusStringAliasChains

