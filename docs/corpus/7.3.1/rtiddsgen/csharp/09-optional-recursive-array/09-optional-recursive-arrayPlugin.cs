/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 09-optional-recursive-array.idl
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

namespace CorpusOptionalRecursiveArray
{

    namespace Implementation
    {

        public struct NodeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusOptionalRecursiveArray.Node>
        {

            private int value;
            private NativeManagedOptionalArray children;

            public void Destroy(bool optionalsOnly)
            {
                children.Destroy<global::CorpusOptionalRecursiveArray.Node,global::CorpusOptionalRecursiveArray.Implementation.NodeUnmanaged>(dimension: (2), optionalsOnly: optionalsOnly);
            }

            public void FromNative(global::CorpusOptionalRecursiveArray.Node sample, bool keysOnly = false)
            {

                sample.value = value;
                children.FromNative<global::CorpusOptionalRecursiveArray.Node, global::CorpusOptionalRecursiveArray.Implementation.NodeUnmanaged>(out global::CorpusOptionalRecursiveArray.Node[] childrenTemporary_, keysOnly: keysOnly, dimensions: new int[] {2});
                sample.children = childrenTemporary_;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusOptionalRecursiveArray.Node sample, bool keysOnly = false)
            {
                value = sample.value;
                children.ToNative<global::CorpusOptionalRecursiveArray.Node, global::CorpusOptionalRecursiveArray.Implementation.NodeUnmanaged>(sample.children, keysOnly: keysOnly, dimension: (2));

            }
        }

        internal class NodePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusOptionalRecursiveArray.Node, NodeUnmanaged>
        {
            static DynamicType dynamicType = null;
            static bool isInitialized = false;

            internal NodePlugin() : base("global::CorpusOptionalRecursiveArray.Node", isKeyed: false, CreateDynamicType(isPublic: false))
            {
                xTypesComplianceMask = 0x0000068C;
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                if (isPublic)
                {
                    throw new NotSupportedException(
                        "Recursive types don't support the property TypeSupport.DynamicType");
                }

                if (isInitialized)
                {
                    dynamicType = tsf.CreateTypeWithAccessInfo<NodeUnmanaged>(
                        dtf.BuildStruct()
                        .WithExtensibility(ExtensibilityKind.Extensible)
                        .WithName("CorpusOptionalRecursiveArray::Node"));
                    return dynamicType;
                }

                isInitialized = true;

                // Node struct
                var NodeStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("children", tsf.CreateArrayWithAccessInfo<global::CorpusOptionalRecursiveArray.Implementation.NodeUnmanaged>(dtf, global::CorpusOptionalRecursiveArray.NodeSupport.Instance.GetDynamicTypeInternal(isPublic), new uint[] {2}), isOptional: true, id: 1)
                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<NodeUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusOptionalRecursiveArray::Node")
                    .AddMembers(NodeStructMembers));

                if (dynamicType != null && ((Rti.Types.Dynamic.StructType)dynamicType).MemberCount == 0)
                {
                    tsf.SetStructMembers((Rti.Types.Dynamic.StructType) dynamicType, NodeStructMembers);
                }
                isInitialized = false;

                return result;
            }
        }
    }
    public class NodeSupport : Rti.Dds.Topics.TypeSupport<global::CorpusOptionalRecursiveArray.Node>
    {
        public NodeSupport() : base(
            new Implementation.NodePlugin(),
            new Lazy<DynamicType>(() =>Implementation.NodePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static NodeSupport Instance { get; private set; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NodeSupport, global::CorpusOptionalRecursiveArray.Node>();

        internal static NodeSupport GetOrCreateInstanceImpl() {
            if (Instance == null) {
                Instance =
                ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<NodeSupport, global::CorpusOptionalRecursiveArray.Node>();
            }
            return Instance;
        }
    }

} // namespace CorpusOptionalRecursiveArray

