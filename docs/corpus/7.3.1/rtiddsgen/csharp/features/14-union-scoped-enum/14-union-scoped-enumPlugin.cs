/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-scoped-enum.idl
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

public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::Choice>
{
    public ChoiceSupport() : base(
        new Implementation.ChoicePlugin(),
        new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ChoiceSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::Choice>();

}

namespace Implementation
{

    public struct ScopedEnumUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::ScopedEnum>
    {

        private global::Choice _d;

        private int selected;
        private int first;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::ScopedEnum sample, bool keysOnly = false)
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
            selected = (int) (0);
            first = (int) (0);
        }

        public void ToNative(global::ScopedEnum sample, bool keysOnly = false)
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

    internal class ScopedEnumPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::ScopedEnum, ScopedEnumUnmanaged>
    {

        internal ScopedEnumPlugin() : base("global::ScopedEnum", isKeyed: false, CreateDynamicType(isPublic: false))
        {
            xTypesComplianceMask = 0x0000068C;
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

            DynamicType result = tsf.CreateTypeWithAccessInfo<ScopedEnumUnmanaged>(
                dtf.BuildUnion()
                .WithDiscriminator(global::ChoiceSupport.Instance.GetDynamicTypeInternal(isPublic))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("ScopedEnum")
                .AddMembers(ScopedEnumStructMembers));

            return result;
        }
    }
}
public class ScopedEnumSupport : Rti.Dds.Topics.TypeSupport<global::ScopedEnum>
{
    public ScopedEnumSupport() : base(
        new Implementation.ScopedEnumPlugin(),
        new Lazy<DynamicType>(() =>Implementation.ScopedEnumPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ScopedEnumSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ScopedEnumSupport, global::ScopedEnum>();

}

