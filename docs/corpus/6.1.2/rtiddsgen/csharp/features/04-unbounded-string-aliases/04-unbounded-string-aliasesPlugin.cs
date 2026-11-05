/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
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

        public struct TextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedStrings.Text>
        {

            private NativeString Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusUnboundedStrings.Text sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: 0, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedStrings.Text sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, StringType.Unbounded);
            }
        }

        internal class TextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedStrings.Text, TextUnmanaged>
        {
            internal TextPlugin() : base("CorpusUnboundedStrings.Text", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<TextUnmanaged>(
                    dtf,
                    "Text",
                    dtf.CreateString());
            }
        }
    }
    public class TextSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedStrings.Text>
    {
        public TextSupport() : base(
            new Implementation.TextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.TextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static TextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TextSupport, CorpusUnboundedStrings.Text>();
    }

    namespace Implementation
    {

        public struct WideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusUnboundedStrings.WideText>
        {

            private NativeWstring Value;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                Value.Destroy();
            }

            public void FromNative(CorpusUnboundedStrings.WideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: 0, allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusUnboundedStrings.WideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, WideStringType.Unbounded);
            }
        }

        internal class WideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusUnboundedStrings.WideText, WideTextUnmanaged>
        {
            internal WideTextPlugin() : base("CorpusUnboundedStrings.WideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                return tsf.CreateAliasWithAccessInfo<WideTextUnmanaged>(
                    dtf,
                    "WideText",
                    dtf.CreateWideString());
            }
        }
    }
    public class WideTextSupport : Rti.Dds.Topics.TypeSupport<CorpusUnboundedStrings.WideText>
    {
        public WideTextSupport() : base(
            new Implementation.WideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextSupport, CorpusUnboundedStrings.WideText>();
    }

} // namespace CorpusUnboundedStrings

