/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
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

namespace CorpusUnboundedStrings
{

    namespace Implementation
    {

        public struct StringAliasesSampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedStrings.StringAliasesSample>
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

            public void FromNative(global::CorpusUnboundedStrings.StringAliasesSample sample, bool keysOnly = false)
            {

                sample.narrow = narrow.FromNative();
                sample.wide = wide.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                narrow.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                wide.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedStrings.StringAliasesSample sample, bool keysOnly = false)
            {
                narrow.ToNative(sample.narrow, ((int) 255));
                wide.ToNative(sample.wide, ((int) 255));
            }
        }

        internal class StringAliasesSamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedStrings.StringAliasesSample, StringAliasesSampleUnmanaged>
        {

            internal StringAliasesSamplePlugin() : base("global::CorpusUnboundedStrings.StringAliasesSample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // StringAliasesSample struct
                var StringAliasesSampleStructMembers = new StructMember[]
                {
                    new StructMember("narrow", global::CorpusUnboundedStrings.TextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0),
                    new StructMember("wide", global::CorpusUnboundedStrings.WideTextSupport.Instance.GetDynamicTypeInternal(isPublic), id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<StringAliasesSampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnboundedStrings::StringAliasesSample")
                    .AddMembers(StringAliasesSampleStructMembers));

                return result;
            }
        }
    }
    public class StringAliasesSampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedStrings.StringAliasesSample>
    {
        public StringAliasesSampleSupport() : base(
            new Implementation.StringAliasesSamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.StringAliasesSamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StringAliasesSampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StringAliasesSampleSupport, global::CorpusUnboundedStrings.StringAliasesSample>();

    }

} // namespace CorpusUnboundedStrings

