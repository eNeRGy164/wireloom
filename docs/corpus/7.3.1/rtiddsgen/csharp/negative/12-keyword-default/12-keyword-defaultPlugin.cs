/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-keyword-default.idl
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

namespace CorpusStrictKeyword
{

    namespace Implementation
    {

        public struct StructUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusStrictKeyword.Struct>
        {

            private int number;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusStrictKeyword.Struct sample, bool keysOnly = false)
            {

                sample.number = number;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                number = (int) (0);
            }

            public void ToNative(global::CorpusStrictKeyword.Struct sample, bool keysOnly = false)
            {
                number = sample.number;
            }
        }

        internal class StructPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusStrictKeyword.Struct, StructUnmanaged>
        {

            internal StructPlugin() : base("global::CorpusStrictKeyword.Struct", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Struct struct
                var StructStructMembers = new StructMember[]
                {
                    new StructMember("number", dtf.GetPrimitiveType<int>(), id: 0)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<StructUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusStrictKeyword::Struct")
                    .AddMembers(StructStructMembers));

                return result;
            }
        }
    }
    public class StructSupport : Rti.Dds.Topics.TypeSupport<global::CorpusStrictKeyword.Struct>
    {
        public StructSupport() : base(
            new Implementation.StructPlugin(),
            new Lazy<DynamicType>(() =>Implementation.StructPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static StructSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<StructSupport, global::CorpusStrictKeyword.Struct>();

    }

} // namespace CorpusStrictKeyword

