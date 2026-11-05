/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-extensibility.idl
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

namespace CorpusExtensibility
{

    namespace Implementation
    {

        public struct AppendableUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusExtensibility.Appendable>
        {

            private int id;
            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(CorpusExtensibility.Appendable sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusExtensibility.Appendable sample, bool keysOnly = false)
            {
                id = sample.id;
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class AppendablePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusExtensibility.Appendable, AppendableUnmanaged>
        {
            internal AppendablePlugin() : base("CorpusExtensibility.Appendable", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Appendable struct
                var AppendableStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 2)
                };

                return tsf.CreateTypeWithAccessInfo<AppendableUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusExtensibility::Appendable")
                    .AddMembers(AppendableStructMembers));
            }
        }
    }
    public class AppendableSupport : Rti.Dds.Topics.TypeSupport<CorpusExtensibility.Appendable>
    {
        public AppendableSupport() : base(
            new Implementation.AppendablePlugin(),
            new Lazy<DynamicType>(() =>Implementation.AppendablePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static AppendableSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<AppendableSupport, CorpusExtensibility.Appendable>();
    }

    namespace Implementation
    {

        public struct FinalUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusExtensibility.Final>
        {

            private int id;
            private NativeString text;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(CorpusExtensibility.Final sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.text = text.FromNative();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
                text.Initialize(size: ((int) 16), allocateMemory: allocateMemory);
            }

            public void ToNative(CorpusExtensibility.Final sample, bool keysOnly = false)
            {
                id = sample.id;
                text.ToNative(sample.text, ((int) 16));
            }
        }

        internal class FinalPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusExtensibility.Final, FinalUnmanaged>
        {
            internal FinalPlugin() : base("CorpusExtensibility.Final", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Final struct
                var FinalStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("text", dtf.CreateString(((int) 16)), id: 2)
                };

                return tsf.CreateTypeWithAccessInfo<FinalUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Final)
                    .WithName("CorpusExtensibility::Final")
                    .AddMembers(FinalStructMembers));
            }
        }
    }
    public class FinalSupport : Rti.Dds.Topics.TypeSupport<CorpusExtensibility.Final>
    {
        public FinalSupport() : base(
            new Implementation.FinalPlugin(),
            new Lazy<DynamicType>(() =>Implementation.FinalPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static FinalSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<FinalSupport, CorpusExtensibility.Final>();
    }

    namespace Implementation
    {

        public struct MutableUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<CorpusExtensibility.Mutable>
        {

            private int id;
            private NativeUnmanagedOptional optionalValue;

            public void Destroy(bool optionalsOnly)
            {
                optionalValue.Destroy(optionalsOnly);
            }

            public void FromNative(CorpusExtensibility.Mutable sample, bool keysOnly = false)
            {

                sample.id = id;
                sample.optionalValue = optionalValue.FromNative<int>();
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                id = (int) 0;
            }

            public void ToNative(CorpusExtensibility.Mutable sample, bool keysOnly = false)
            {
                id = sample.id;
                optionalValue.ToNative<int>(sample.optionalValue);
            }
        }

        internal class MutablePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<CorpusExtensibility.Mutable, MutableUnmanaged>
        {
            internal MutablePlugin() : base("CorpusExtensibility.Mutable", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Mutable struct
                var MutableStructMembers = new StructMember[]
                {
                    new StructMember("id", dtf.GetPrimitiveType<int>(), id: 1),
                    new StructMember("optionalValue", dtf.GetPrimitiveType<int>(), isOptional: true, id: 2)
                };

                return tsf.CreateTypeWithAccessInfo<MutableUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Mutable)
                    .WithName("CorpusExtensibility::Mutable")
                    .AddMembers(MutableStructMembers));
            }
        }
    }
    public class MutableSupport : Rti.Dds.Topics.TypeSupport<CorpusExtensibility.Mutable>
    {
        public MutableSupport() : base(
            new Implementation.MutablePlugin(),
            new Lazy<DynamicType>(() =>Implementation.MutablePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static MutableSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<MutableSupport, CorpusExtensibility.Mutable>();
    }

} // namespace CorpusExtensibility

