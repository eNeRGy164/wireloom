/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 14-autoid-scope.idl
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

namespace AutoIds
{

    namespace Implementation
    {

        public struct HashedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::AutoIds.Hashed>
        {

            private int first;
            private int second;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::AutoIds.Hashed sample, bool keysOnly = false)
            {

                sample.first = first;
                sample.second = second;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                first = (int) (0);
                second = (int) (0);
            }

            public void ToNative(global::AutoIds.Hashed sample, bool keysOnly = false)
            {
                first = sample.first;
                second = sample.second;
            }
        }

        internal class HashedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::AutoIds.Hashed, HashedUnmanaged>
        {

            internal HashedPlugin() : base("global::AutoIds.Hashed", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Hashed struct
                var HashedStructMembers = new StructMember[]
                {
                    new StructMember("first", dtf.GetPrimitiveType<int>(), id: 64291979),
                    new StructMember("second", dtf.GetPrimitiveType<int>(), id: 182907049)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<HashedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("AutoIds::Hashed")
                    .AddMembers(HashedStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        1,
                        annotations);
                }

                return result;

            }
        }
    }
    public class HashedSupport : Rti.Dds.Topics.TypeSupport<global::AutoIds.Hashed>
    {
        public HashedSupport() : base(
            new Implementation.HashedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.HashedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static HashedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<HashedSupport, global::AutoIds.Hashed>();

    }

} // namespace AutoIds

namespace AutoIds
{

    namespace Implementation
    {

        public struct ReopenedUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::AutoIds.Reopened>
        {

            private int first;
            private int second;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::AutoIds.Reopened sample, bool keysOnly = false)
            {

                sample.first = first;
                sample.second = second;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                first = (int) (0);
                second = (int) (0);
            }

            public void ToNative(global::AutoIds.Reopened sample, bool keysOnly = false)
            {
                first = sample.first;
                second = sample.second;
            }
        }

        internal class ReopenedPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::AutoIds.Reopened, ReopenedUnmanaged>
        {

            internal ReopenedPlugin() : base("global::AutoIds.Reopened", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Reopened struct
                var ReopenedStructMembers = new StructMember[]
                {
                    new StructMember("first", dtf.GetPrimitiveType<int>(), id: 0),
                    new StructMember("second", dtf.GetPrimitiveType<int>(), id: 1)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ReopenedUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("AutoIds::Reopened")
                    .AddMembers(ReopenedStructMembers));

                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        0,
                        annotations);
                }
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.Int32Value = (int) 0;
                    AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                    minValueParam.Int32Value = (int) int.MinValue;
                    AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                    maxValueParam.Int32Value = (int) int.MaxValue;
                    Annotations annotations = new Annotations(
                        TypeKind.Int32,
                        defaultValueParam,
                        minValueParam,
                        maxValueParam,
                        null);
                    result.SetMemberAnnotations(
                        1,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ReopenedSupport : Rti.Dds.Topics.TypeSupport<global::AutoIds.Reopened>
    {
        public ReopenedSupport() : base(
            new Implementation.ReopenedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ReopenedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ReopenedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ReopenedSupport, global::AutoIds.Reopened>();

    }

} // namespace AutoIds

namespace Implementation
{

    public struct SequentialUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::Sequential>
    {

        private int first;
        private int second;

        public void Destroy(bool optionalsOnly)
        {
        }

        public void FromNative(global::Sequential sample, bool keysOnly = false)
        {

            sample.first = first;
            sample.second = second;
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            first = (int) (0);
            second = (int) (0);
        }

        public void ToNative(global::Sequential sample, bool keysOnly = false)
        {
            first = sample.first;
            second = sample.second;
        }
    }

    internal class SequentialPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::Sequential, SequentialUnmanaged>
    {

        internal SequentialPlugin() : base("global::Sequential", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // Sequential struct
            var SequentialStructMembers = new StructMember[]
            {
                new StructMember("first", dtf.GetPrimitiveType<int>(), id: 0),
                new StructMember("second", dtf.GetPrimitiveType<int>(), id: 1)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<SequentialUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("Sequential")
                .AddMembers(SequentialStructMembers));

            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.Int32Value = (int) 0;
                AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                minValueParam.Int32Value = (int) int.MinValue;
                AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                maxValueParam.Int32Value = (int) int.MaxValue;
                Annotations annotations = new Annotations(
                    TypeKind.Int32,
                    defaultValueParam,
                    minValueParam,
                    maxValueParam,
                    null);
                result.SetMemberAnnotations(
                    0,
                    annotations);
            }
            {
                AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                defaultValueParam.Int32Value = (int) 0;
                AnnotationParameterValue minValueParam = new AnnotationParameterValue();
                minValueParam.Int32Value = (int) int.MinValue;
                AnnotationParameterValue maxValueParam = new AnnotationParameterValue();
                maxValueParam.Int32Value = (int) int.MaxValue;
                Annotations annotations = new Annotations(
                    TypeKind.Int32,
                    defaultValueParam,
                    minValueParam,
                    maxValueParam,
                    null);
                result.SetMemberAnnotations(
                    1,
                    annotations);
            }

            return result;

        }
    }
}
public class SequentialSupport : Rti.Dds.Topics.TypeSupport<global::Sequential>
{
    public SequentialSupport() : base(
        new Implementation.SequentialPlugin(),
        new Lazy<DynamicType>(() =>Implementation.SequentialPlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static SequentialSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<SequentialSupport, global::Sequential>();

}

namespace Implementation
{

    public struct UsesIncludeUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::UsesInclude>
    {

        private global::Implementation.IncludedUnmanaged included;

        public void Destroy(bool optionalsOnly)
        {
            if (optionalsOnly)
            {
                return;
            }
            included.Destroy(optionalsOnly);
        }

        public void FromNative(global::UsesInclude sample, bool keysOnly = false)
        {

            included.FromNative(sample.included, keysOnly: false);
        }

        public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
        {
            included.Initialize(allocatePointers, allocateMemory);
        }

        public void ToNative(global::UsesInclude sample, bool keysOnly = false)
        {
            included.ToNative(sample.included, keysOnly: false);
        }
    }

    internal class UsesIncludePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::UsesInclude, UsesIncludeUnmanaged>
    {

        internal UsesIncludePlugin() : base("global::UsesInclude", isKeyed: false, CreateDynamicType(isPublic: false))
        {
        }

        public static DynamicType CreateDynamicType(bool isPublic = true)
        {
            var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
            var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

            // UsesInclude struct
            var UsesIncludeStructMembers = new StructMember[]
            {
                new StructMember("included", global::IncludedSupport.Instance.GetDynamicTypeInternal(isPublic), id: 0)

            };

            DynamicType result = tsf.CreateTypeWithAccessInfo<UsesIncludeUnmanaged>(
                dtf.BuildStruct()
                .WithExtensibility(ExtensibilityKind.Extensible)
                .WithName("UsesInclude")
                .AddMembers(UsesIncludeStructMembers));

            return result;

        }
    }
}
public class UsesIncludeSupport : Rti.Dds.Topics.TypeSupport<global::UsesInclude>
{
    public UsesIncludeSupport() : base(
        new Implementation.UsesIncludePlugin(),
        new Lazy<DynamicType>(() =>Implementation.UsesIncludePlugin.CreateDynamicType(isPublic: true)))
    {
    }

    public static UsesIncludeSupport Instance { get; } =
    ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<UsesIncludeSupport, global::UsesInclude>();

}

