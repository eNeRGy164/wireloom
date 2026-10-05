/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from common.idl
using RTI Code Generator (rtiddsgen) version 4.7.0.1.
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

    public struct IncludedTypeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::IncludedType>
    {

        private int value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::IncludedType sample, bool keysOnly = false)
        {

            sample.value = value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            value = (int) (0);
        }

        public void ToNative(global::IncludedType sample, bool keysOnly = false)
        {
            value = sample.value;
        }
    }

    internal class IncludedTypePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::IncludedType, IncludedTypeUnmanaged>
    {

        internal IncludedTypePlugin() : base("global::IncludedType", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // IncludedType struct
            var IncludedTypeStructMembers = new StructMember[]
            {
                new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<IncludedTypeUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("IncludedType")
                .AddMembers(IncludedTypeStructMembers));

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
public class IncludedTypeSupport : Rti.Dds.Topics.TypeSupport<global::IncludedType>
{
    public IncludedTypeSupport() : base(
        new Implementation.IncludedTypePlugin(),
        new Lazy<DynamicType>(() =>Implementation.IncludedTypePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static IncludedTypeSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<IncludedTypeSupport, global::IncludedType>();

}

