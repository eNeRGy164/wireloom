/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-string-expression-bound.idl
using RTI Code Generator (rtiddsgen) version 3.1.2.
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

    public struct StringBoundedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<StringBounded>
    {

        private NativeString text;
        private NativeWstring wideText;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            text.Destroy();
            wideText.Destroy();
        }

        public void FromNative(StringBounded sample, bool keysOnly = false)
        {

            sample.text = text.FromNative();
            sample.wideText = wideText.FromNative();
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            text.Initialize(size: ((int) (StringBound.Value)), allocateMemory: allocateMemory);
            wideText.Initialize(size: ((int) 2*3), allocateMemory: allocateMemory);
        }

        public void ToNative(StringBounded sample, bool keysOnly = false)
        {
            text.ToNative(sample.text, ((int) (StringBound.Value)));
            wideText.ToNative(sample.wideText, ((int) 2*3));
        }
    }

    internal class StringBoundedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<StringBounded, StringBoundedUnmanaged>
    {
        internal StringBoundedPlugin() : base("StringBounded", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // StringBounded struct
            var StringBoundedStructMembers = new StructMember[]
            {
                new StructMember("text", dtf.CreateString(((int) (StringBound.Value))), id: 0),
                new StructMember("wideText", dtf.CreateWideString(((int) 2*3)), id: 1)
            };

            return tsf.CreateTypeWithAccessInfo<StringBoundedUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("StringBounded")
                .AddMembers(StringBoundedStructMembers));
        }
    }
}
public class StringBoundedSupport : Rti.Dds.Topics.TypeSupport<StringBounded>
{
    public StringBoundedSupport() : base(
        new Implementation.StringBoundedPlugin(),
        new Lazy<DynamicType>(() =>Implementation.StringBoundedPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static StringBoundedSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StringBoundedSupport, StringBounded>();
}

