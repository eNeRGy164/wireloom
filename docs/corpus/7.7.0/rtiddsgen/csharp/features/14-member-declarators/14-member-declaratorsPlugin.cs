/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-member-declarators.idl
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

    public struct DeclaratorsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::Declarators>
    {

        private int first;
        private int second;
        private NativeUnmanagedArray numbers;
        private NativeUnmanagedArray matrix;

        public void Destroy(bool optionalsOnly)
        {
            numbers.Destroy(optionalsOnly);
            matrix.Destroy(optionalsOnly);
        }

        public void FromNative(global::Declarators sample, bool keysOnly = false)
        {

            sample.second = second;
            if (keysOnly)
            {
                return;
            }
            sample.first = first;
            numbers.FromNative(sample.numbers, dimension: (2));
            matrix.FromNative(sample.matrix, dimension: (2)*(3));
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            first = (int) (0);
            second = (int) (0);
            numbers.Initialize<int>(dimension: (2), allocateMemory: allocateMemory);
            matrix.Initialize<int>(dimension: (2)*(3), allocateMemory: allocateMemory);
        }

        public void ToNative(global::Declarators sample, bool keysOnly = false)
        {
            second = sample.second;
            if (keysOnly)
            {
                return;
            }
            first = sample.first;
            numbers.ToNative<int>(sample.numbers, dimension: (2));
            matrix.ToNative<int>(sample.matrix, dimension: (2)*(3));
        }
    }

    internal class DeclaratorsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::Declarators, DeclaratorsUnmanaged>
    {

        internal DeclaratorsPlugin() : base("global::Declarators", isKeyed: true, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // Declarators struct
            var DeclaratorsStructMembers = new StructMember[]
            {
                new StructMember("first", dtf.GetPrimitiveType<int>(), id: 0),
                new StructMember("second", dtf.GetPrimitiveType<int>(), isKey: true, id: 4),
                new StructMember("numbers", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2}), id: 5),
                new StructMember("matrix", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2, 3}), id: 6)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<DeclaratorsUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("Declarators")
                .AddMembers(DeclaratorsStructMembers));

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
                    1,
                    annotations);
            }

            return result;

        }
    }
}
public class DeclaratorsSupport : Rti.Dds.Topics.TypeSupport<global::Declarators>
{
    public DeclaratorsSupport() : base(
        new Implementation.DeclaratorsPlugin(),
        new Lazy<DynamicType>(() =>Implementation.DeclaratorsPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static DeclaratorsSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<DeclaratorsSupport, global::Declarators>();

}

