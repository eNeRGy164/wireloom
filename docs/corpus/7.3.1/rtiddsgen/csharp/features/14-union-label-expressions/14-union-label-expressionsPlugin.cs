/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-label-expressions.idl
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

    public struct LabelExpressionsUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::LabelExpressions>
    {

        private int _d;

        private int symbolic;
        private int arithmetic;
        private int hexadecimal;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::LabelExpressions sample, bool keysOnly = false)
        {
            switch (_d)
            {
                case (Selected.Value):
                    sample.symbolic = symbolic;
                    break;
                case (3+1):
                    sample.arithmetic = arithmetic;
                    break;
                case 0x10:
                sample.hexadecimal = hexadecimal;
                break;
            }
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            _d = LabelExpressions.DefaultDiscriminator;
            symbolic = (int) (0);
            arithmetic = (int) (0);
            hexadecimal = (int) (0);
        }

        public void ToNative(global::LabelExpressions sample, bool keysOnly = false)
        {
            _d = sample.Discriminator;
            switch (_d)
            {
                case (Selected.Value):
                    symbolic = sample.symbolic;
                    break;
                case (3+1):
                    arithmetic = sample.arithmetic;
                    break;
                case 0x10:
                hexadecimal = sample.hexadecimal;
                break;
            }
        }
    }

    internal class LabelExpressionsPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::LabelExpressions, LabelExpressionsUnmanaged>
    {

        internal LabelExpressionsPlugin() : base("global::LabelExpressions", isKeyed: false, CreateDynamicType(isPublic: false))
        {
            xTypesComplianceMask = 0x0000068C;
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // LabelExpressions union
            var LabelExpressionsStructMembers = new UnionMember[]
            {
                new UnionMember("symbolic", dtf.GetPrimitiveType<int>(), new int[] {(int) (Selected.Value)}, id: 1),
                new UnionMember("arithmetic", dtf.GetPrimitiveType<int>(), new int[] {(int) (3+1)}, id: 2),
                new UnionMember("hexadecimal", dtf.GetPrimitiveType<int>(), new int[] {(int) 0x10}, id: 3)
            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<LabelExpressionsUnmanaged>(
                dtf.BuildUnion()
                .WithDiscriminator(dtf.GetPrimitiveType<int>())
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("LabelExpressions")
                .AddMembers(LabelExpressionsStructMembers));

            return result;
        }
    }
}
public class LabelExpressionsSupport : Rti.Dds.Topics.TypeSupport<global::LabelExpressions>
{
    public LabelExpressionsSupport() : base(
        new Implementation.LabelExpressionsPlugin(),
        new Lazy<DynamicType>(() =>Implementation.LabelExpressionsPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static LabelExpressionsSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<LabelExpressionsSupport, global::LabelExpressions>();

}

