/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 02-multiple.idl
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

    public struct FirstUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::First>
    {

        private int value;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::First sample, bool keysOnly = false)
        {

            sample.value = value;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            value = (int) (0);
        }

        public void ToNative(global::First sample, bool keysOnly = false)
        {
            value = sample.value;
        }
    }

    internal class FirstPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::First, FirstUnmanaged>
    {

        internal FirstPlugin() : base("global::First", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // First struct
            var FirstStructMembers = new StructMember[]
            {
                new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<FirstUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("First")
                .AddMembers(FirstStructMembers));

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
public class FirstSupport : Rti.Dds.Topics.TypeSupport<global::First>
{
    public FirstSupport() : base(
        new Implementation.FirstPlugin(),
        new Lazy<DynamicType>(() =>Implementation.FirstPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static FirstSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<FirstSupport, global::First>();

}

namespace Implementation
{

    public struct SecondUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::Second>
    {

        private NativeString name;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            name.Destroy();
        }

        public void FromNative(global::Second sample, bool keysOnly = false)
        {

            sample.name = name.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            name.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
        }

        public void ToNative(global::Second sample, bool keysOnly = false)
        {
            name.ToNative(sample.name, ((int) 255));
        }
    }

    internal class SecondPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::Second, SecondUnmanaged>
    {

        internal SecondPlugin() : base("global::Second", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // Second struct
            var SecondStructMembers = new StructMember[]
            {
                new StructMember("name", dtf.CreateString(((int) 255)), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<SecondUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("Second")
                .AddMembers(SecondStructMembers));

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
public class SecondSupport : Rti.Dds.Topics.TypeSupport<global::Second>
{
    public SecondSupport() : base(
        new Implementation.SecondPlugin(),
        new Lazy<DynamicType>(() =>Implementation.SecondPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static SecondSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SecondSupport, global::Second>();

}

