/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-prefix.idl
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

namespace CorpusEnumValuePrefix
{

    namespace Implementation
    {
        internal class AnnotatedPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public AnnotatedPlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var enumType = dtf.BuildEnum()
                .WithName("CorpusEnumValuePrefix::Annotated")
                .AddMember(new EnumMember("FIRST", 7))
                .AddMember(new EnumMember("SECOND", 8))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) 7;
                    Annotations annotations = new Annotations(
                        TypeKind.Enumeration,
                        defaultValueParam,
                        null,
                        null,
                        null);
                    enumType.SetAnnotations(
                        annotations);
                }
                return enumType;
            }
        }
    }

    public class AnnotatedSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumValuePrefix.Annotated>
    {
        public AnnotatedSupport() : base(
            new Implementation.AnnotatedPlugin(),
            new Lazy<DynamicType>(() =>Implementation.AnnotatedPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static AnnotatedSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<AnnotatedSupport, global::CorpusEnumValuePrefix.Annotated>();

    }

} // namespace CorpusEnumValuePrefix

