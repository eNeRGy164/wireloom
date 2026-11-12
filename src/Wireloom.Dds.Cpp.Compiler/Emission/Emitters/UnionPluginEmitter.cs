using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits native union metadata contexts, sample access, and context accessors.</summary>
internal static class UnionPluginEmitter
{
    internal static bool RequiresStlInterpreterPluginBridge(UnionPlan declaration) =>
        declaration.Branches.Any(branch =>
            RtiSampleAccess.UnwrapAlias(branch.Type) is MemberTypePlan.Struct
            || branch.Type.ContainsCollection);

    /// <summary>Checks whether another declaration needs to reach a union during staged initialization.</summary>
    internal static bool RequiresStagedUnionAccessor(GenerationPlan plan, UnionPlan target) =>
        plan.Structs.Any(declaration => declaration.Members.Any(member =>
            ReferencesUnion(member.Type, target.QualifiedName)))
        || plan.Unions.Any(declaration => declaration.Branches.Any(branch =>
            ReferencesUnion(branch.Type, target.QualifiedName)));

    private static bool ReferencesUnion(MemberTypePlan type, string qualifiedName) => type switch
    {
        MemberTypePlan.Union union => string.Equals(union.QualifiedName, qualifiedName, StringComparison.Ordinal),
        MemberTypePlan.Alias alias => ReferencesUnion(alias.Target, qualifiedName),
        MemberTypePlan.Sequence sequence => ReferencesUnion(sequence.ElementType, qualifiedName),
        MemberTypePlan.Array array => ReferencesUnion(array.ElementType, qualifiedName),
        _ => false,
    };

    /// <summary>Writes the native context for a union and its discriminator and branch metadata.</summary>
    internal static void EmitUnionPluginContext(SourceWriter source, UnionPlan declaration, GenerationPlan plan)
    {
        source.WriteLine($"struct {declaration.ContextIdentifier}");
        source.WriteLines("""
            {
                ::rti::topic::cdr::GenericTypePluginFactory& factory;
                ::dds::core::xtypes::UnionType* type;
                ::rti::topic::cdr::CTypePlugin* type_plugin;
            """);
        source.BlankLine();
        EmitUnionStringTypeCodeHelpers(source, declaration);
        EmitUnionSampleAccessInfo(source, declaration);
        source.BlankLine();
        source.WriteLine($"    {declaration.ContextIdentifier}()");
        source.WriteLines("""
                    : factory(::rti::topic::cdr::GenericTypePluginFactory::instance())
                    , type(nullptr)
                    , type_plugin(nullptr)
                {
                }
            """);
        source.BlankLine();
        source.WriteLine("    void create_type()");
        source.WriteLine("    {");
        var discriminatorMetadataType = declaration.DiscriminatorMetadataTypeExpression == "::rti::core::xtypes::octet_tag_t"
            && source.RtiVersion != "7.7.0"
                ? "::std::uint8_t"
                : declaration.DiscriminatorMetadataTypeExpression;
        var discriminatorType = declaration.DiscriminatorEnumQualifiedName is { } enumName
            ? $"::rti::topic::dynamic_type<::{enumName}>::get()"
            : $"::dds::core::xtypes::primitive_type<{discriminatorMetadataType}>()";
        source.WriteLine($"        {declaration.CppType} sample_value{{}};");
        source.WriteLine("        const auto* sample_address = reinterpret_cast<const char*>(&sample_value);");
        source.WriteLine("        ::std::vector<::RTIXCdrUnsignedLong> member_offsets;");
        source.WriteLine(FormattableString.Invariant($"        member_offsets.reserve({declaration.Branches.Count + 1});"));
        source.WriteLine("        const auto* discriminator_address = reinterpret_cast<const char*>(&sample_value._d());");
        source.WriteLine(
            "        member_offsets.push_back(static_cast<::RTIXCdrUnsignedLong>(discriminator_address - sample_address));");
        foreach (var branch in declaration.Branches)
        {
            var sampleDiscriminator = branch.IsDefault
                ? branch.DefaultBranchAccessDiscriminator
                : branch.LabelExpressions.First();
            source.WriteLine($"        sample_value._d({sampleDiscriminator});");
            source.WriteLine($"        const auto* member_{branch.Name} =");
            source.WriteLine($"            reinterpret_cast<const char*>(&sample_value.{branch.Name}());");
            source.WriteLine("        member_offsets.push_back(");
            source.WriteLine("            static_cast<::RTIXCdrUnsignedLong>(");
            source.WriteLine($"                member_{branch.Name} - sample_address));");
        }

        source.WriteLine($"        type = factory.create_union(\"{declaration.QualifiedName}\",");
        source.WriteLine($"            {discriminatorType},");
        source.WriteLine($"            {declaration.ExtensibilityExpression},");
        source.WriteLine("            static_cast<::RTIXCdrUnsignedLong>(");
        source.WriteLine($"                sizeof(::{declaration.QualifiedName})),");
        source.WriteLine("            member_offsets);");
        source.WriteLine("    }");
        source.BlankLine();
        source.WriteLines("""
                void add_members()
                {
                    // Add union metadata after every struct and union shell exists.
            """);
        foreach (var branch in declaration.Branches)
        {
            var memberType = DynamicTypeEmitter.FormatTypeExpression(
                branch.Type,
                16,
                stagedInitialization: true,
                rtiVersion: source.RtiVersion);
            var labelArgument = branch.IsDefault
                ? "::dds::core::xtypes::UnionMember::DEFAULT_LABEL"
                : branch.MetadataLabels.Count == 1
                    ? branch.MetadataLabels[0].ToString(System.Globalization.CultureInfo.InvariantCulture)
                    : "::dds::core::xtypes::UnionMember::LabelSeq{" + string.Join(", ", branch.MetadataLabels) + "}";
            source.WriteLine("        type->add_member(");
            source.WriteLine("            ::dds::core::xtypes::UnionMember(");
            source.WriteLine($"                \"{branch.Name}\", {memberType + ","}");
            source.WriteLine($"                {labelArgument}));");
        }

        source.WriteLine("    }");
        source.BlankLine();
        source.WriteLines("""
                void create_type_plugin()
                {
                    auto* native_type_code = reinterpret_cast<::RTIXCdrTypeCode*>
                        (&type->native());
            """);
        if (declaration.DiscriminatorEnumQualifiedName is { } discriminatorEnum)
        {
            source.WriteLines("""
                        native_type_code->_typeCode =
                            reinterpret_cast<::RTIXCdrTypeCode*>(
                                const_cast<::DDS_TypeCode*>(
                """);
            source.WriteLine($"                    &::rti::topic::dynamic_type<::{discriminatorEnum}>::get().native()));");
        }
        else if (TryGetPrimitiveCdrTypeCode(declaration.DiscriminatorMetadataTypeExpression, source.RtiVersion, out var discriminatorTypeCode))
        {
            source.WriteLine("        native_type_code->_typeCode =");
            source.WriteLine($"            {discriminatorTypeCode};");
        }

        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            var branchType = RtiSampleAccess.UnwrapAlias(declaration.Branches[index].Type);
            var normalizedPrimitiveSequence = branchType is MemberTypePlan.Sequence
                {
                    IsUnbounded: false,
                    ElementType: MemberTypePlan.Primitive,
                }
                && declaration.Branches.Any(branch => RtiSampleAccess.UnwrapAlias(branch.Type) is MemberTypePlan.Struct);
            var typeCodeExpression = branchType is MemberTypePlan.Struct branchStructure
                ? $"reinterpret_cast<::RTIXCdrTypeCode*>(const_cast<::DDS_TypeCode*>(&{PluginSymbols.ContextTypePointer(branchStructure.QualifiedName, ownerQualifiedName: null, optionalSelfRecursiveArray: false)}->native()))"
                : normalizedPrimitiveSequence
                    ? $"native_type_code->_members[{index}]._representation._typeCode"
                    : NativeCdrTypeCodeExpression(branchType, source.RtiVersion);
            if (typeCodeExpression is null && !normalizedPrimitiveSequence)
            {
                continue;
            }

            source.WriteLine(FormattableString.Invariant($"        native_type_code->_members[{index}]._representation._typeCode ="));
            if (branchType is MemberTypePlan.Struct structure)
            {
                var nestedTypeName = $"::{structure.QualifiedName}";
                var nestedPluginName = $"{declaration.Name}_{declaration.Branches[index].Name}_sample_plugin";
                source.WriteLine("            reinterpret_cast<::RTIXCdrTypeCode*>(");
                source.WriteLine("                const_cast<::DDS_TypeCode*>(");
                source.WriteLine(
                    "                    &"
                    + PluginSymbols.ContextTypePointer(structure.QualifiedName, ownerQualifiedName: null, optionalSelfRecursiveArray: false)
                    + "->native()));");
                source.WriteLine($"        static ::RTIXCdrTypePlugin {nestedPluginName} = RTIXCdrTypePlugin_INITIALIZER;");
                source.WriteLine($"        {nestedPluginName}.initializeSampleFnc = [](");
                source.WriteLine("            void* sample, ::RTIXCdrLong, ::RTIXCdrLong) -> ::RTIXCdrLong {");
                source.WriteLine("                if (sample == nullptr) return RTI_FALSE;");
                source.WriteLine($"                *static_cast<{nestedTypeName}*>(sample) = {{}};");
                source.WriteLine("                return RTI_TRUE;");
                source.WriteLine("            };");
                source.WriteLine(FormattableString.Invariant($"        native_type_code->_members[{index}]"));
                source.WriteLine("            ._representation._typeCode->_typePlugin =");
                source.WriteLine($"            &{nestedPluginName};");
            }
            else if (branchType is MemberTypePlan.Sequence
                {
                    IsUnbounded: false,
                    ElementType: MemberTypePlan.Primitive,
                } sequence
                && declaration.Branches.Any(branch => RtiSampleAccess.UnwrapAlias(branch.Type) is MemberTypePlan.Struct))
            {
                var sequenceTypeCodeName = $"union_member_{index}_sequence_type_code";
                var elementTypeCode = NativeCdrTypeCodeExpression(sequence.ElementType, source.RtiVersion);
                if (elementTypeCode is null)
                {
                    throw new InvalidOperationException(
                        $"Union sequence element type '{sequence.ElementType.StorageType}' has no RTI 7.7.0 TypeCode mapping.");
                }

                source.WriteLine($"            {typeCodeExpression};");
                source.WriteLine(
                    FormattableString.Invariant($"        auto* {sequenceTypeCodeName} = native_type_code->_members[{index}")
                    + "]._representation._typeCode;");
                source.WriteLine($"        if ({sequenceTypeCodeName} == nullptr");
                source.WriteLine($"            || RTIXCdrTypeCode_getKind({sequenceTypeCodeName}) != RTI_XCDR_TK_SEQUENCE");
                source.WriteLine(
                    FormattableString.Invariant($"            || {sequenceTypeCodeName}->_maximumLength != {sequence.Bound}")
                    + ") {");
                source.WriteLine("            throw std::runtime_error(\"RTI 7.7 bounded union sequence TypeCode is missing\");");
                source.WriteLine("        }");
                source.WriteLine($"        {sequenceTypeCodeName}->_kind = RTI_XCDR_TK_SEQUENCE;");
                source.WriteLine($"        {sequenceTypeCodeName}->_typeCode = {elementTypeCode};");
                source.WriteLine($"        {sequenceTypeCodeName}->_sampleAccessInfo =");
                source.WriteLine("            ::rti::topic::interpreter::detail::sequence_helper<");
                source.WriteLine($"                {sequence.StorageType}>::sample_access_info();");
            }
            else
            {
                source.WriteLine($"            {typeCodeExpression};");
            }
        }

        if (!RequiresStlInterpreterPluginBridge(declaration))
        {
            source.WriteLine("        type_plugin = &factory.create_type_plugin(*type);");
        }
        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            source.WriteLine(FormattableString.Invariant($"        native_type_code->_members[{index}]._memberFlags = 0;"));
            var nodeIndex = 0;
            NativeTypeEmitter.EmitNestedTypeCodeAccessInfo(
                source,
                declaration.Branches[index].Type,
                index,
                $"native_type_code->_members[{index}]._representation._typeCode",
                ref nodeIndex,
                plan,
                stringSequenceElement: true);
        }
        source.WriteLine("        type->native()._data._sampleAccessInfo = stl_sample_access_info();");
        if (declaration.Branches.Any(branch =>
                RtiSampleAccess.UnwrapAlias(branch.Type) is MemberTypePlan.Struct))
        {
            source.WriteLines("""
                        static ::RTIXCdrTypePlugin sample_plugin = RTIXCdrTypePlugin_INITIALIZER;
                        sample_plugin.initializeSampleFnc = [](
                            void* sample, ::RTIXCdrLong, ::RTIXCdrLong) -> ::RTIXCdrLong {
                                if (sample == nullptr) return RTI_FALSE;
                """);
            source.WriteLine($"                *static_cast<{declaration.CppType}*>(sample) = {{}};");
            source.WriteLines("""
                                return RTI_TRUE;
                            };
                        type->native()._data._typePlugin = &sample_plugin;
                """);
        }
        else
        {
            source.WriteLine("        static ::RTIXCdrTypePlugin interpreted_type_plugin = RTIXCdrTypePlugin_INITIALIZER;");
            source.WriteLine("        type->native()._data._typePlugin = &interpreted_type_plugin;");
        }
        source.WriteLine("    }");
        source.WriteLine("};");
        source.BlankLine();
        source.WriteLine($"{declaration.ContextIdentifier}& {declaration.ContextIdentifier}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {declaration.ContextIdentifier} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
    }

    private static string? NativeCdrTypeCodeExpression(MemberTypePlan type, string rtiVersion) => type switch
    {
        MemberTypePlan.Primitive primitive => TryGetPrimitiveCdrTypeCode(primitive.Mapping.MetadataExpression, rtiVersion, out var typeCode)
            ? typeCode
            : null,
        MemberTypePlan.String { IsWide: false, MetadataBound: var bound } =>
            $"reinterpret_cast<::RTIXCdrTypeCode*>(&wireloom_string_typecode_{bound}())",
        MemberTypePlan.String { IsWide: true, MetadataBound: var bound } =>
            $"reinterpret_cast<::RTIXCdrTypeCode*>(&wireloom_wstring_typecode_{bound}())",
        MemberTypePlan.Alias alias => NativeCdrTypeCodeExpression(alias.Target, rtiVersion),
        _ => null,
    };

    private static void EmitUnionStringTypeCodeHelpers(SourceWriter source, UnionPlan declaration)
    {
        source.WriteLines("""
                static ::RTIXCdrTypeCode* wireloom_bool_typecode()
                {
                    const auto& type_code =
                        ::rti::topic::interpreter::initialize_bool_typecode();
                    return reinterpret_cast<::RTIXCdrTypeCode*>(
                        const_cast<::DDS_TypeCode*>(&type_code));
                }
                static ::RTIXCdrTypeCode* wireloom_wchar_typecode()
                {
                    const auto& type_code =
                        ::rti::topic::interpreter::initialize_wchar_typecode();
                    return reinterpret_cast<::RTIXCdrTypeCode*>(
                        const_cast<::DDS_TypeCode*>(&type_code));
                }
            """);
        var strings = declaration.Branches
            .Select(branch => RtiSampleAccess.UnwrapAlias(branch.Type))
            .OfType<MemberTypePlan.String>()
            .Distinct()
            .ToArray();
        foreach (var stringType in strings)
        {
            var typeName = stringType.IsWide ? "wstring" : "string";
            var initializer = stringType.IsWide ? "initialize_wstring_typecode" : "initialize_string_typecode";
            source.WriteLine(
                $"    static ::DDS_TypeCode& wireloom_{typeName}_typecode_"
                + FormattableString.Invariant($"{stringType.MetadataBound}()"));
            source.WriteLine("    {");
            source.WriteLine(
                $"        static ::DDS_TypeCode value = ::rti::topic::interpreter::{initializer}("
                + FormattableString.Invariant($"{stringType.MetadataBound});"));
            source.WriteLine("        return value;");
            source.WriteLine("    }");
        }
    }

    private static bool TryGetPrimitiveCdrTypeCode(string metadataExpression, string rtiVersion, out string typeCode)
    {
        var is770 = rtiVersion == "7.7.0";
        typeCode = metadataExpression switch
        {
            "bool" => "wireloom_bool_typecode()",
            "::std::int16_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_short)",
            "::std::uint16_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_ushort)",
            "::std::int32_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_long)",
            "::std::uint32_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_ulong)",
            "::std::int64_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_longlong)",
            "::std::uint64_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_ulonglong)",
            "::std::int8_t" => is770
                ? "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_int8)"
                : "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_octet)",
            "::std::uint8_t" => is770
                ? "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_uint8)"
                : "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_octet)",
            "::rti::core::xtypes::octet_tag_t" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_octet)",
            "char" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_char)",
            "wchar_t" => "wireloom_wchar_typecode()",
            "float" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_float)",
            "double" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_double)",
            "::rti::core::LongDouble" => "reinterpret_cast<::RTIXCdrTypeCode*>(&::DDS_g_tc_longdouble)",
            _ => string.Empty,
        };

        return typeCode.Length > 0;
    }

    private static void EmitUnionSampleAccessInfo(SourceWriter source, UnionPlan declaration)
    {
        var typeName = declaration.CppType;
        source.WriteLines("""
                static ::RTIXCdrSampleAccessInfo* stl_sample_access_info()
                {
                    struct AccessInfo
                    {
                        ::RTIXCdrSampleAccessInfo sample;
            """);
        source.WriteLine(
            FormattableString.Invariant($"            ::RTIXCdrMemberAccessInfo members[{declaration.Branches.Count + 1}];"));
        source.WriteLines("""
                        AccessInfo()
                            : sample(RTIXCdrSampleAccessInfo_INITIALIZER)
                            , members{}
                        {
            """);
        source.WriteLine($"                {typeName} sample_value{{}};");
        source.WriteLines("""
                            const auto* sample_address = reinterpret_cast<const char*>(&sample_value);
                            const auto* discriminator = reinterpret_cast<const char*>(&sample_value._d());
                            members[0].bindingMemberValueOffset[0] =
                                static_cast<::RTIXCdrUnsignedLong>(discriminator - sample_address);
            """);
        for (var index = 0; index < declaration.Branches.Count; index++)
        {
            var branch = declaration.Branches[index];
            var discriminator = branch.IsDefault
                ? branch.DefaultBranchAccessDiscriminator
                : branch.LabelExpressions.First();
            source.WriteLine($"                sample_value._d({discriminator});");
            source.WriteLine(
                $"                const auto* branch_{branch.Name} = reinterpret_cast<const char*>(&sample_value.{branch.Name}"
                + "());");
            source.WriteLine(FormattableString.Invariant($"                members[{index + 1}].bindingMemberValueOffset[0] ="));
            source.WriteLine($"                    static_cast<::RTIXCdrUnsignedLong>(branch_{branch.Name} - sample_address);");
        }

        source.WriteLines("""
                            sample.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;
                            sample.typeSize[0] =
                                static_cast<::RTIXCdrUnsignedLong>(
            """);
        source.WriteLine($"                        sizeof({typeName}));");
        source.WriteLines("""
                            sample.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;
                            sample.getMemberValuePointerFcn =
                                ::rti::topic::interpreter::get_aggregation_value_pointer<
            """);
        source.WriteLine($"                        {typeName}>;");
        source.WriteLines("""
                            sample.memberAccessInfos = members;
                        }
                    };
                    static AccessInfo access_info;
                    return &access_info.sample;
                }
            """);
    }

    /// <summary>Declares native union metadata accessors used before their definitions.</summary>
    internal static void EmitUnionContextAccessorDeclarations(
        SourceWriter source,
        IReadOnlyList<UnionPlan> unions)
    {
        foreach (var declaration in unions)
        {
            source.OpenNamespaces(declaration.NamespaceSegments);
            source.WriteLine("namespace");
            source.WriteLine("{");
            source.WriteLine($"::dds::core::xtypes::UnionType* {PluginSymbols.UnionTypeAccessorLocalName(declaration.Name)}();");
            source.WriteLine($"::RTIXCdrTypeCode* {PluginSymbols.UnionNativeTypeCodeAccessorLocalName(declaration.Name)}();");
            source.WriteLine("} // namespace");
            source.CloseNamespaces(declaration.NamespaceSegments);
        }
    }

    /// <summary>Writes the accessors for a staged union context.</summary>
    internal static void EmitUnionContextAccessors(SourceWriter source, UnionPlan declaration)
    {
        source.OpenBlock($"::dds::core::xtypes::UnionType* {PluginSymbols.UnionTypeAccessorLocalName(declaration.Name)}() ");
        source.WriteLine($"return {declaration.ContextIdentifier}_instance().type;");
        source.CloseBlock();
        source.WriteLine($"::RTIXCdrTypeCode* {PluginSymbols.UnionNativeTypeCodeAccessorLocalName(declaration.Name)}() ");
        source.WriteLine("{");
        source.WriteLines(
            $"    return reinterpret_cast<::RTIXCdrTypeCode*>(\n        &{declaration.ContextIdentifier}"
            + "_instance().type->native());");
        source.WriteLine("}");
    }
}
