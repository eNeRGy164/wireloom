/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 11-conditionals.idl
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

namespace CorpusConditionals
{

    namespace Implementation
    {

        public struct EnabledUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusConditionals.Enabled>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusConditionals.Enabled sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusConditionals.Enabled sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class EnabledPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusConditionals.Enabled, EnabledUnmanaged>
        {

            internal EnabledPlugin() : base("global::CorpusConditionals.Enabled", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Enabled struct
                var EnabledStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<EnabledUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusConditionals::Enabled")
                    .AddMembers(EnabledStructMembers));

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

                return result;

            }
        }
    }
    public class EnabledSupport : Rti.Dds.Topics.TypeSupport<global::CorpusConditionals.Enabled>
    {
        public EnabledSupport() : base(
            new Implementation.EnabledPlugin(),
            new Lazy<DynamicType>(() =>Implementation.EnabledPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static EnabledSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<EnabledSupport, global::CorpusConditionals.Enabled>();

    }

} // namespace CorpusConditionals

namespace CorpusConditionals
{

    namespace Implementation
    {

        public struct UndefBranchUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusConditionals.UndefBranch>
        {

            private int value;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusConditionals.UndefBranch sample, bool keysOnly = false)
            {

                sample.value = value;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                value = (int) (0);
            }

            public void ToNative(global::CorpusConditionals.UndefBranch sample, bool keysOnly = false)
            {
                value = sample.value;
            }
        }

        internal class UndefBranchPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusConditionals.UndefBranch, UndefBranchUnmanaged>
        {

            internal UndefBranchPlugin() : base("global::CorpusConditionals.UndefBranch", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // UndefBranch struct
                var UndefBranchStructMembers = new StructMember[]
                {
                    new StructMember("value", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<UndefBranchUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusConditionals::UndefBranch")
                    .AddMembers(UndefBranchStructMembers));

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

                return result;

            }
        }
    }
    public class UndefBranchSupport : Rti.Dds.Topics.TypeSupport<global::CorpusConditionals.UndefBranch>
    {
        public UndefBranchSupport() : base(
            new Implementation.UndefBranchPlugin(),
            new Lazy<DynamicType>(() =>Implementation.UndefBranchPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static UndefBranchSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<UndefBranchSupport, global::CorpusConditionals.UndefBranch>();

    }

} // namespace CorpusConditionals

