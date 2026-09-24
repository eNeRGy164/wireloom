/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-zero-bound.idl
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

    public struct ZeroBoundUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::ZeroBound>
    {

        private NativeString value;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            value.Destroy();
        }

        public void FromNative(global::ZeroBound sample, bool keysOnly = false)
        {

            sample.value = value.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            value.Initialize(size: ((int) 0), allocateMemory: allocateMemory);
        }

        public void ToNative(global::ZeroBound sample, bool keysOnly = false)
        {
            value.ToNative(sample.value, ((int) 0));
        }
    }

    internal class ZeroBoundPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::ZeroBound, ZeroBoundUnmanaged>
    {

        internal ZeroBoundPlugin() : base("global::ZeroBound", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // ZeroBound struct
            var ZeroBoundStructMembers = new StructMember[]
            {
                new StructMember("value", dtf.CreateString(((int) 0)), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<ZeroBoundUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("ZeroBound")
                .AddMembers(ZeroBoundStructMembers));

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
public class ZeroBoundSupport : Rti.Dds.Topics.TypeSupport<global::ZeroBound>
{
    public ZeroBoundSupport() : base(
        new Implementation.ZeroBoundPlugin(),
        new Lazy<DynamicType>(() =>Implementation.ZeroBoundPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ZeroBoundSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ZeroBoundSupport, global::ZeroBound>();

}

