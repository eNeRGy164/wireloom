/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-annotation-arguments.idl
using RTI Code Generator (rtiddsgen) version 4.3.1.
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

    public struct AnnotationArgumentsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::AnnotationArguments>
    {

        private int plain;
        private int selected;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::AnnotationArguments sample, bool keysOnly = false)
        {

            sample.plain = plain;
            sample.selected = selected;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            plain = (int) (0);
            selected = (int) ((Selected.Value));
        }

        public void ToNative(global::AnnotationArguments sample, bool keysOnly = false)
        {
            plain = sample.plain;
            selected = sample.selected;
        }
    }

    internal class AnnotationArgumentsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::AnnotationArguments, AnnotationArgumentsUnmanaged>
    {

        internal AnnotationArgumentsPlugin() : base("global::AnnotationArguments", isKeyed: false, CreateDynamicType(isPublic: false))
        {
            xTypesComplianceMask = 0x0000068C;
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // AnnotationArguments struct
            var AnnotationArgumentsStructMembers = new StructMember[]
            {
                new StructMember("plain", dtf.GetPrimitiveType<int>(), id: 0),
                new StructMember("selected", dtf.GetPrimitiveType<int>(), id: 16)
            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<AnnotationArgumentsUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("AnnotationArguments")
                .AddMembers(AnnotationArgumentsStructMembers));

            return result;
        }
    }
}
public class AnnotationArgumentsSupport : Rti.Dds.Topics.TypeSupport<global::AnnotationArguments>
{
    public AnnotationArgumentsSupport() : base(
        new Implementation.AnnotationArgumentsPlugin(),
        new Lazy<DynamicType>(() =>Implementation.AnnotationArgumentsPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static AnnotationArgumentsSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<AnnotationArgumentsSupport, global::AnnotationArguments>();

}

