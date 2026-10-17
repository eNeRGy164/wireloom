/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-unknown-annotation.idl
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

namespace CorpusNegativeIgnored
{

    namespace Implementation
    {

        public struct UnknownUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusNegativeIgnored.Unknown>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusNegativeIgnored.Unknown sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusNegativeIgnored.Unknown sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class UnknownPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusNegativeIgnored.Unknown, UnknownUnmanaged>
        {

            internal UnknownPlugin() : base("global::CorpusNegativeIgnored.Unknown", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Unknown struct
                var UnknownStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<UnknownUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusNegativeIgnored::Unknown")
                    .AddMembers(UnknownStructMembers));

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
    public class UnknownSupport : Rti.Dds.Topics.TypeSupport<global::CorpusNegativeIgnored.Unknown>
    {
        public UnknownSupport() : base(
            new Implementation.UnknownPlugin(),
            new Lazy<DynamicType>(() =>Implementation.UnknownPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static UnknownSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<UnknownSupport, global::CorpusNegativeIgnored.Unknown>();

    }

} // namespace CorpusNegativeIgnored

