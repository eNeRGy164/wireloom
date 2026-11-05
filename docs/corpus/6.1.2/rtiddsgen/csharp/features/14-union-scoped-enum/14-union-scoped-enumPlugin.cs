/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-scoped-enum.idl
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
    internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
    {
        public ChoicePlugin() : base(CreateDynamicType(isPublic: false))
        {
        }

        internal static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            return dtf.BuildEnum()
            .WithName("Choice")
            .AddMember(new EnumMember("FIRST", 0))
            .AddMember(new EnumMember("SECOND", 1))
            .WithExtensibility(ExtensibilityKind.Extensible)
            .Create();
        }
    }
}

public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<Choice>
{
    public ChoiceSupport() : base(
        new Implementation.ChoicePlugin(),
        new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ChoiceSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, Choice>();
}

namespace Implementation
{

    public struct ScopedEnumUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<ScopedEnum>
    {

        private Choice _d;

        private int selected;
        private int first;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(ScopedEnum sample, bool keysOnly = false)
        {
            switch (_d)
            {
                case (Selected.Value):
                    sample.selected = selected;
                    break;
                case Choice.FIRST:
                sample.first = first;
                break;
            }
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            _d = ScopedEnum.DefaultDiscriminator;
            selected = (int) 0;
            first = (int) 0;
        }

        public void ToNative(ScopedEnum sample, bool keysOnly = false)
        {
            _d = sample.Discriminator;
            switch (_d)
            {
                case (Selected.Value):
                    selected = sample.selected;
                    break;
                case Choice.FIRST:
                first = sample.first;
                break;
            }
        }
    }

    internal class ScopedEnumPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<ScopedEnum, ScopedEnumUnmanaged>
    {
        internal ScopedEnumPlugin() : base("ScopedEnum", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // ScopedEnum union
            var ScopedEnumStructMembers = new UnionMember[]
            {
                new UnionMember("selected", dtf.GetPrimitiveType<int>(), new int[] {(int) (Selected.Value)}, id: 1),
                new UnionMember("first", dtf.GetPrimitiveType<int>(), new int[] {(int) Choice.FIRST}, id: 2)
            };

            return tsf.CreateTypeWithAccessInfo<ScopedEnumUnmanaged>(
                dtf.BuildUnion()
                .WithDiscriminator(ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("ScopedEnum")
                .AddMembers(ScopedEnumStructMembers));
        }
    }
}
public class ScopedEnumSupport : Rti.Dds.Topics.TypeSupport<ScopedEnum>
{
    public ScopedEnumSupport() : base(
        new Implementation.ScopedEnumPlugin(),
        new Lazy<DynamicType>(() =>Implementation.ScopedEnumPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ScopedEnumSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScopedEnumSupport, ScopedEnum>();
}

