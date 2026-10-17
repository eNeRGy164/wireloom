/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-boolean.idl
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

namespace CorpusBooleanUnion
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusBooleanUnion.Choice>
        {
            private byte _d;

            private int enabled;
            private NativeString disabled;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                disabled.Destroy();
            }

            public void FromNative(global::CorpusBooleanUnion.Choice sample, bool keysOnly = false)
            {
                switch (Convert.ToBoolean(_d))
                {
                    case true:
                    sample.enabled = enabled;
                    break;
                    case false:
                    sample.disabled = disabled.FromNative();
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Convert.ToByte(Choice.DefaultDiscriminator);
                enabled = (int) (0);
                disabled.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
            }

            public void ToNative(global::CorpusBooleanUnion.Choice sample, bool keysOnly = false)
            {
                _d = Convert.ToByte(sample.Discriminator);
                switch (Convert.ToBoolean(_d))
                {
                    case true:
                    enabled = sample.enabled;
                    break;
                    case false:
                    disabled.ToNative(sample.disabled, ((int) 255));
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusBooleanUnion.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusBooleanUnion.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("enabled", dtf.GetPrimitiveType<int>(), new int[] {1
                    }, id: 1),
                    new UnionMember("disabled", dtf.CreateString(((int) 255)), new int[] {0
                    }, id: 2)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<bool>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusBooleanUnion::Choice")
                    .AddMembers(ChoiceStructMembers));

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
                    defaultValueParam.StringValue = "";
                    Annotations annotations = new Annotations(
                        TypeKind.String,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        1,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusBooleanUnion.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusBooleanUnion.Choice>();

    }

} // namespace CorpusBooleanUnion

