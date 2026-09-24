/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 07-union-char.idl
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

namespace CorpusCharUnion
{

    namespace Implementation
    {

        public struct ChoiceUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusCharUnion.Choice>
        {
            private byte _d;

            private int letter;
            private NativeString text;
            private byte other;

            public void Destroy(bool optionalsOnly)
            {
                if (optionalsOnly)
                {
                    return;
                }
                text.Destroy();
            }

            public void FromNative(global::CorpusCharUnion.Choice sample, bool keysOnly = false)
            {
                switch (NativeChar.FromUtf8(_d))
                {
                    case 'a':
                    sample.letter = letter;
                    break;
                    case 'z':
                    sample.text = text.FromNative();
                    break;
                    default:
                    sample.other = Convert.ToBoolean(other);
                    break;
                }
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                _d = NativeChar.ToUtf8(Choice.DefaultDiscriminator);
                letter = (int) (0);
                text.Initialize(size: ((int) 255), allocateMemory: allocateMemory);
                other = 0;
            }

            public void ToNative(global::CorpusCharUnion.Choice sample, bool keysOnly = false)
            {
                _d = NativeChar.ToUtf8(sample.Discriminator);
                switch (NativeChar.FromUtf8(_d))
                {
                    case 'a':
                    letter = sample.letter;
                    break;
                    case 'z':
                    text.ToNative(sample.text, ((int) 255));
                    break;
                    default:
                    other = Convert.ToByte(sample.other);
                    break;
                }
            }
        }

        internal class ChoicePlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusCharUnion.Choice, ChoiceUnmanaged>
        {

            internal ChoicePlugin() : base("global::CorpusCharUnion.Choice", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // Choice union
                var ChoiceStructMembers = new UnionMember[]
                {
                    new UnionMember("letter", dtf.GetPrimitiveType<int>(), new int[] {(int) 'a'}, id: 1),
                    new UnionMember("text", dtf.CreateString(((int) 255)), new int[] {(int) 'z'}, id: 2),
                    new UnionMember("other", dtf.GetPrimitiveType<bool>(), new int[] {(int) UnionMember.DefaultLabel}, id: 3)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<ChoiceUnmanaged>(
                    dtf.BuildUnion()
                    .WithDiscriminator(dtf.GetPrimitiveType<char>())
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusCharUnion::Choice")
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
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.BoolValue = false;
                    Annotations annotations = new Annotations(
                        TypeKind.Boolean,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    result.SetMemberAnnotations(
                        2,
                        annotations);
                }

                return result;

            }
        }
    }
    public class ChoiceSupport : Rti.Dds.Topics.TypeSupport<global::CorpusCharUnion.Choice>
    {
        public ChoiceSupport() : base(
            new Implementation.ChoicePlugin(),
            new Lazy<DynamicType>(() =>Implementation.ChoicePlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ChoiceSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ChoiceSupport, global::CorpusCharUnion.Choice>();

    }

} // namespace CorpusCharUnion

