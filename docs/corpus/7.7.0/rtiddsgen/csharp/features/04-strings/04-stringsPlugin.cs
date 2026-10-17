/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 04-strings.idl
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

namespace CorpusStrings
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStrings.Sample>
        {

            private NativeString unbounded;
            private NativeString bounded;
            private NativeWstring wideUnbounded;
            private NativeWstring wideBounded;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                unbounded.Destroy();
                bounded.Destroy();
                wideUnbounded.Destroy();
                wideBounded.Destroy();
            }

            public void FromNative(global::CorpusStrings.Sample sample, bool keysOnly = false)
            {

                sample.unbounded = unbounded.FromNative();
                sample.bounded = bounded.FromNative();
                sample.wideUnbounded = wideUnbounded.FromNative();
                sample.wideBounded = wideBounded.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                unbounded.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                bounded.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
                wideUnbounded.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                wideBounded.Initialize(size: ((int) 8), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusStrings.Sample sample, bool keysOnly = false)
            {
                unbounded.ToNative(sample.unbounded, ((int) 255));
                bounded.ToNative(sample.bounded, ((int) 8));
                wideUnbounded.ToNative(sample.wideUnbounded, ((int) 255));
                wideBounded.ToNative(sample.wideBounded, ((int) 8));
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStrings.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusStrings.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("unbounded", dtf.CreateString(((int) 255)), id: 0),
                    new StructMember("bounded", dtf.CreateString(((int) 8)), id: 1),
                    new StructMember("wideUnbounded", dtf.CreateWideString(((int) 255)), id: 2),
                    new StructMember("wideBounded", dtf.CreateWideString(((int) 8)), id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusStrings::Sample")
                    .AddMembers(SampleStructMembers));

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
                        1,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        2,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.WideStringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.WideString,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        3,
                        annotations);
                }

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStrings.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusStrings.Sample>();

    }

} // namespace CorpusStrings

