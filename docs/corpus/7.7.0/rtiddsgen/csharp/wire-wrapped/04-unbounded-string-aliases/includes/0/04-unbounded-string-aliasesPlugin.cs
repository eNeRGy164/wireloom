/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-unbounded-string-aliases.idl
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

namespace CorpusUnboundedStrings
{

    namespace Implementation
    {

        public struct TextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedStrings.Text>
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

            public void FromNative(global::CorpusUnboundedStrings.Text sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedStrings.Text sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class TextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedStrings.Text, TextUnmanaged>
        {

            internal TextPlugin() : base("global::CorpusUnboundedStrings.Text", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateString(((int) 255));
                var aliasType = tsf.CreateAliasWithAccessInfo<TextUnmanaged>(
                    dtf,
                    "Text",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class TextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedStrings.Text>
    {
        public TextSupport() : base(
            new Implementation.TextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.TextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static TextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<TextSupport, global::CorpusUnboundedStrings.Text>();

    }

    namespace Implementation
    {

        public struct WideTextUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnboundedStrings.WideText>
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

            public void FromNative(global::CorpusUnboundedStrings.WideText sample, bool keysOnly = false)
            {

                sample.Value = Value.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                Value.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusUnboundedStrings.WideText sample, bool keysOnly = false)
            {
                Value.ToNative(sample.Value, ((int) 255));
            }
        }

        internal class WideTextPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnboundedStrings.WideText, WideTextUnmanaged>
        {

            internal WideTextPlugin() : base("global::CorpusUnboundedStrings.WideText", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                using var dtString = dtf.CreateWideString(((int) 255));
                var aliasType = tsf.CreateAliasWithAccessInfo<WideTextUnmanaged>(
                    dtf,
                    "WideText",
                    dtString
                    );
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    aliasType.SetAnnotations(
                        annotations);
                }
                return aliasType;

            }
        }
    }
    public class WideTextSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnboundedStrings.WideText>
    {
        public WideTextSupport() : base(
            new Implementation.WideTextPlugin(),
            new Lazy<DynamicType>(() =>Implementation.WideTextPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static WideTextSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<WideTextSupport, global::CorpusUnboundedStrings.WideText>();

    }

} // namespace CorpusUnboundedStrings

