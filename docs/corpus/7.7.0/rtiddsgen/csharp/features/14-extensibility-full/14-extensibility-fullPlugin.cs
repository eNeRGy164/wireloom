/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-extensibility-full.idl
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

namespace Implementation
{

    public struct FullExtensibilityUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::FullExtensibility>
    {

        private int value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::FullExtensibility sample, bool keysOnly = false)
        {

            sample.value = value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            value = (int) (0);
        }

        public void ToNative(global::FullExtensibility sample, bool keysOnly = false)
        {
            value = sample.value;
        }
    }

    internal class FullExtensibilityPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::FullExtensibility, FullExtensibilityUnmanaged>
    {

        internal FullExtensibilityPlugin() : base("global::FullExtensibility", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // FullExtensibility struct
            var FullExtensibilityStructMembers = new StructMember[]
            {
                new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<FullExtensibilityUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Mutable)
                .WithName("FullExtensibility")
                .AddMembers(FullExtensibilityStructMembers));

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
public class FullExtensibilitySupport : Rti.Dds.Topics.TypeSupport<global::FullExtensibility>
{
    public FullExtensibilitySupport() : base(
        new Implementation.FullExtensibilityPlugin(),
        new Lazy<DynamicType>(() =>Implementation.FullExtensibilityPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static FullExtensibilitySupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<FullExtensibilitySupport, global::FullExtensibility>();

}

