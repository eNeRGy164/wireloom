/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-ulong.idl
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

namespace CorpusUnionUnsignedLong
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusUnionUnsignedLong.Choice>
        {
            private uint _d;

            private int minimum;
            private int maximum;
            private int fallback;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusUnionUnsignedLong.Choice sample, bool keysOnly = false)
            {
                switch (_d)
                {
                    case 0:
                    sample.minimum = minimum;
                    break;
                    case 4294967295:
                    sample.maximum = maximum;
                    break;
                    default:
                    sample.fallback = fallback;
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = Choice.DefaultDiscriminator;
                minimum = (int) (0);
                maximum = (int) (0);
                fallback = (int) (0);
            }

            public void ToNative(global::CorpusUnionUnsignedLong.Choice sample, bool keysOnly = false)
            {
                _d = sample.Discriminator;
                switch (_d)
                {
                    case 0:
                    minimum = sample.minimum;
                    break;
                    case 4294967295:
                    maximum = sample.maximum;
                    break;
                    default:
                    fallback = sample.fallback;
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusUnionUnsignedLong.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusUnionUnsignedLong.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("minimum", dtf.GetPrimitiveType<int>(), new int[] {(int) 0}, id: 1),
                    new UnionMember("maximum", dtf.GetPrimitiveType<int>(), new int[] {unchecked((int) 4294967295)}, id: 2),
                    new UnionMember("fallback", dtf.GetPrimitiveType<int>(), new int[] {(int) UnionMember.DefaultLabel}, id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<uint>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusUnionUnsignedLong::Choice")
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
                        2,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusUnionUnsignedLong.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusUnionUnsignedLong.Choice>();

    }

} // namespace CorpusUnionUnsignedLong

