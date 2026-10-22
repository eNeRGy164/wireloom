/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-union-array-branch.idl
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

    public struct ArrayBranchUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::ArrayBranch>
    {

        private int _d;

        private NativeUnmanagedArray values;
        private int scalar;

        public void Destroy(bool optionalsOnly)
        {
            values.Destroy(optionalsOnly);
        }

        public void FromNative(global::ArrayBranch sample, bool keysOnly = false)
        {
            switch (_d)
            {
                case 0:
                if(sample.Discriminator != _d)
                {
                    sample.values = new int[2,3];

                    ;
                }
                values.FromNative(sample.values, dimension: (2)*(3));
                break;
                case 1:
                sample.scalar = scalar;
                break;
            }
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            _d = ArrayBranch.DefaultDiscriminator;
            values.Initialize<int>(dimension: (2)*(3), allocateMemory: allocateMemory);
            scalar = (int) (0);
        }

        public void ToNative(global::ArrayBranch sample, bool keysOnly = false)
        {
            _d = sample.Discriminator;
            switch (_d)
            {
                case 0:
                values.ToNative<int>(sample.values, dimension: (2)*(3));
                break;
                case 1:
                scalar = sample.scalar;
                break;
            }
        }
    }

    internal class ArrayBranchPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::ArrayBranch, ArrayBranchUnmanaged>
    {

        internal ArrayBranchPlugin() : base("global::ArrayBranch", isKeyed: false, CreateDynamicType(isPublic: false))
        {
            xTypesComplianceMask = 0x0000068C;
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // ArrayBranch union
            var ArrayBranchStructMembers = new UnionMember[]
            {
                new UnionMember("values", tsf.CreateArrayWithAccessInfo<int>(dtf, dtf.GetPrimitiveType<int>(), new uint[] {2, 3}), new int[] {(int) 0}, id: 1),
                new UnionMember("scalar", dtf.GetPrimitiveType<int>(), new int[] {(int) 1}, id: 2)
            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<ArrayBranchUnmanaged>(
                dtf.BuildUnion()
                .WithDiscriminator(dtf.GetPrimitiveType<int>())
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("ArrayBranch")
                .AddMembers(ArrayBranchStructMembers));

            return result;
        }
    }
}
public class ArrayBranchSupport : Rti.Dds.Topics.TypeSupport<global::ArrayBranch>
{
    public ArrayBranchSupport() : base(
        new Implementation.ArrayBranchPlugin(),
        new Lazy<DynamicType>(() =>Implementation.ArrayBranchPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static ArrayBranchSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ArrayBranchSupport, global::ArrayBranch>();

}

