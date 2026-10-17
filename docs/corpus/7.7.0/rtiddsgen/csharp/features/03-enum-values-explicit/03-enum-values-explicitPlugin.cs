/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 03-enum-values-explicit.idl
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

namespace CorpusEnumValues
{

    namespace Implementation
    {
        internal class ExplicitPlugin : Rti.Dds.NativeInterface.TypePlugin.EnumTypePlugin
        {
            public ExplicitPlugin() : base(CreateDynamicType(isPublic: false))
            {
            }

            internal static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var enumType = dtf.BuildEnum()
                .WithName("CorpusEnumValues::Explicit")
                .AddMember(new EnumMember("NEGATIVE", -2))
                .AddMember(new EnumMember("ZERO", 0))
                .AddMember(new EnumMember("GAP", 7))
                .AddMember(new EnumMember("NEXT", 8))
                .WithExtensibility(ExtensibilityKind.Extensible)
                .Create();
                {
                    AnnotationParameterValue defaultValueParam = new AnnotationParameterValue();
                    defaultValueParam.EnumValue = (int) -2;
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

    public class ExplicitSupport : Rti.Dds.Topics.TypeSupport<global::CorpusEnumValues.Explicit>
    {
        public ExplicitSupport() : base(
            new Implementation.ExplicitPlugin(),
            new Lazy<DynamicType>(() =>Implementation.ExplicitPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static ExplicitSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<ExplicitSupport, global::CorpusEnumValues.Explicit>();

    }

} // namespace CorpusEnumValues

