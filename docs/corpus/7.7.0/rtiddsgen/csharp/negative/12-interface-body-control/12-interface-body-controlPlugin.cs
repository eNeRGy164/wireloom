/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-interface-body-control.idl
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

namespace CorpusInterfaceControl
{

    namespace Implementation
    {

        public struct SampleUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusInterfaceControl.Sample>
        {

            private int number;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusInterfaceControl.Sample sample, bool keysOnly = false)
            {

                sample.number = number;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                number = (int) (0);
            }

            public void ToNative(global::CorpusInterfaceControl.Sample sample, bool keysOnly = false)
            {
                number = sample.number;
            }
        }

        internal class SamplePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusInterfaceControl.Sample, SampleUnmanaged>
        {

            internal SamplePlugin() : base("global::CorpusInterfaceControl.Sample", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Sample struct
                var SampleStructMembers = new StructMember[]
                {
                    new StructMember("number", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<SampleUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusInterfaceControl::Sample")
                    .AddMembers(SampleStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }

                return result;

            }
        }
    }
    public class SampleSupport : Rti.Dds.Topics.TypeSupport<global::CorpusInterfaceControl.Sample>
    {
        public SampleSupport() : base(
            new Implementation.SamplePlugin(),
            new Lazy<DynamicType>(() =>Implementation.SamplePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static SampleSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SampleSupport, global::CorpusInterfaceControl.Sample>();

    }

} // namespace CorpusInterfaceControl

