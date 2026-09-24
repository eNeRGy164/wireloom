/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-formatting.idl
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

namespace CorpusFormatting
{

    namespace Implementation
    {

        public struct ValueUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusFormatting.Value>
        {

            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(global::CorpusFormatting.Value sample, bool keysOnly = false)
            {

                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusFormatting.Value sample, bool keysOnly = false)
            {
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class ValuePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusFormatting.Value, ValueUnmanaged>
        {

            internal ValuePlugin() : base("global::CorpusFormatting.Value", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Value struct
                var ValueStructMembers = new StructMember[]
                {
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ValueUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusFormatting::Value")
                    .AddMembers(ValueStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ValueSupport : Rti.Dds.Topics.TypeSupport<global::CorpusFormatting.Value>
    {
        public ValueSupport() : base(
            new Implementation.ValuePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ValuePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ValueSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ValueSupport, global::CorpusFormatting.Value>();

    }

} // namespace CorpusFormatting

