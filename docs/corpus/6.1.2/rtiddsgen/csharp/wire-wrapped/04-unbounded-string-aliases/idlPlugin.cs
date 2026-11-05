/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from idl.idl
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

namespace CorpusUnboundedStrings
{

    namespace Implementation
    {

        public struct StringAliasesSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedStrings.StringAliasesSample>
        {

            private NativeString narrow;
            private NativeWstring wide;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                narrow.Destroy();
                wide.Destroy();
            }

            public void FromNative(CorpusUnboundedStrings.StringAliasesSample sample, bool keysOnly = false)
            {

                sample.narrow = narrow.FromNative();
                sample.wide = wide.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                narrow.Initialize(size: 0, allocateMemory: allocateMemory);
                wide.Initialize(size: 0, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedStrings.StringAliasesSample sample, bool keysOnly = false)
            {
                narrow.ToNative(sample.narrow, StringType.Unbounded);
                wide.ToNative(sample.wide, WideStringType.Unbounded);
            }
        }

        internal class StringAliasesSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedStrings.StringAliasesSample, StringAliasesSampleUnmanaged>
        {
            internal StringAliasesSamplePlugin() : base("CorpusUnboundedStrings.StringAliasesSample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // StringAliasesSample struct
                var StringAliasesSampleStructMembers = new StructMember[]
                {
                    new StructMember("narrow", CorpusUnboundedStrings.TextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("wide", CorpusUnboundedStrings.WideTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)
                };

                return tsf.CreateTypeWithAccessInfo<StringAliasesSampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedStrings::StringAliasesSample")
                    .AddMembers(StringAliasesSampleStructMembers));
            }
        }
    }
    public class StringAliasesSampleSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedStrings.StringAliasesSample>
    {
        public StringAliasesSampleSupport() : base(
            new Implementation.StringAliasesSamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StringAliasesSamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StringAliasesSampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StringAliasesSampleSupport, CorpusUnboundedStrings.StringAliasesSample>();
    }

} // namespace CorpusUnboundedStrings

