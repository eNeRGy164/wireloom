/*
WARNING: THIS FILE IS AUTO-GENERATED. DO NOT MODIFY.

This file was generated from 12-identifier-control.idl
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

namespace CorpusIdentifierControl
{

    namespace Implementation
    {

        public struct _StructUnmanaged : Rti.Dds.NativeInterface.TypePlugin.INativeTopicType<global::CorpusIdentifierControl._Struct>
        {

            private int @event;

            public void Destroy(bool optionalsOnly)
            {
            }

            public void FromNative(global::CorpusIdentifierControl._Struct sample, bool keysOnly = false)
            {

                sample.@event = @event;
            }

            public void Initialize(bool allocatePointers = true, bool allocateMemory = true)
            {
                @event = (int) (0);
            }

            public void ToNative(global::CorpusIdentifierControl._Struct sample, bool keysOnly = false)
            {
                @event = sample.@event;
            }
        }

        internal class _StructPlugin : Rti.Dds.NativeInterface.TypePlugin.InterpretedTypePlugin<global::CorpusIdentifierControl._Struct, _StructUnmanaged>
        {

            internal _StructPlugin() : base("global::CorpusIdentifierControl._Struct", isKeyed: false, CreateDynamicType(isPublic: false))
            {
            }

            public static DynamicType CreateDynamicType(bool isPublic = true)
            {
                var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);
                var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;

                // _Struct struct
                var _StructStructMembers = new StructMember[]
                {
                    new StructMember("event", dtf.GetPrimitiveType<int>(), id: 0)

                };

                DynamicType result = tsf.CreateTypeWithAccessInfo<_StructUnmanaged>(
                    dtf.BuildStruct()
                    .WithExtensibility(ExtensibilityKind.Extensible)
                    .WithName("CorpusIdentifierControl::_Struct")
                    .AddMembers(_StructStructMembers));

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
    public class _StructSupport : Rti.Dds.Topics.TypeSupport<global::CorpusIdentifierControl._Struct>
    {
        public _StructSupport() : base(
            new Implementation._StructPlugin(),
            new Lazy<DynamicType>(() =>Implementation._StructPlugin.CreateDynamicType(isPublic: true)))
        {
        }

        public static _StructSupport Instance { get; } =
        ServiceEnvironment.Instance.Internal.TypeSupportFactory.CreateTypeSupport<_StructSupport, global::CorpusIdentifierControl._Struct>();

    }

} // namespace CorpusIdentifierControl

