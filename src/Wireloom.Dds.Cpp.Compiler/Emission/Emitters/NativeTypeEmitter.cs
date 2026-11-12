using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits RTI-private TypeCode annotations and aggregate sample-access metadata.</summary>
internal static class NativeTypeEmitter
{
    /// <summary>Writes TypeCode sample-access information for a referenced aggregate.</summary>
    internal static void EmitAggregateTypeCodeAccessInfo(
        SourceWriter source,
        StructPlan declaration,
        string typeCodeExpression,
        string namePrefix,
        GenerationPlan plan,
        int indentation,
        bool includeTypePluginMetadata = true)
    {
        var prefix = new string(' ', indentation);
        var typePointer = PluginSymbols.ContextTypePointer(
            declaration.QualifiedName,
            ownerQualifiedName: null,
            optionalSelfRecursiveArray: false);
        var pluginInfoName = $"{namePrefix}_type_plugin_info";
        source.WriteLine($"{prefix}{typeCodeExpression}->_sampleAccessInfo =");
        source.WriteLine($"{prefix}    {typePointer}");
        source.WriteLine($"{prefix}        ->native()._data._sampleAccessInfo;");
        if (includeTypePluginMetadata)
        {
            source.WriteLine($"{prefix}static ::RTIXCdrTypePlugin {pluginInfoName} = RTIXCdrTypePlugin_INITIALIZER;");
            source.WriteLine($"{prefix}{typeCodeExpression}->_typePlugin = &{pluginInfoName};");
        }
        else
        {
            source.WriteLine($"{prefix}{typeCodeExpression}->_typePlugin =");
            source.WriteLine($"{prefix}    {typePointer}");
            source.WriteLine($"{prefix}        ->native()._data._typePlugin;");
        }

        for (var memberIndex = 0; memberIndex < declaration.Members.Count; memberIndex++)
        {
            var member = declaration.Members[memberIndex];
            if (member.Type is MemberTypePlan.String stringType)
            {
                var nestedMemberTypeCode = $"{namePrefix}_member_{memberIndex}_type_code";
                source.WriteLine($"{prefix}auto* {nestedMemberTypeCode} =");
                source.WriteLine($"{prefix}    reinterpret_cast<::DDS_TypeCode*>({typeCodeExpression})->_data._members[");
                source.WriteLine(FormattableString.Invariant($"{prefix}        {memberIndex}]._representation._typeCode;"));
                source.WriteLine($"{prefix}{nestedMemberTypeCode}->_sampleAccessInfo =");
                source.WriteLine($"{prefix}    ::rti::topic::interpreter::detail::sequence_helper<");
                source.WriteLine($"{prefix}        {stringType.StorageType}>::sample_access_info();");
            }
            else if (member.Type is MemberTypePlan.Struct nestedStructure)
            {
                var nestedMemberTypeCode = $"{declaration.Name}_{typeCodeExpression}_member_{memberIndex}_type_code";
                source.WriteLine($"{prefix}auto* {nestedMemberTypeCode} =");
                source.WriteLine($"{prefix}    reinterpret_cast<::DDS_TypeCode*>({typeCodeExpression})->_data._members[");
                source.WriteLine(FormattableString.Invariant($"{prefix}        {memberIndex}]._representation._typeCode;"));
                if (plan.Structs.SingleOrDefault(candidate => candidate.QualifiedName == nestedStructure.QualifiedName)
                    is { } nestedDeclaration)
                {
                    EmitAggregateTypeCodeAccessInfo(
                        source,
                        nestedDeclaration,
                        nestedMemberTypeCode,
                        $"{namePrefix}_member_{memberIndex}",
                        plan,
                        indentation,
                        includeTypePluginMetadata);
                }
            }
            else if (member.Type.ContainsCollection)
            {
                var nestedMemberTypeCode = $"{declaration.Name}_{typeCodeExpression}_member_{memberIndex}_type_code";
                source.WriteLine($"{prefix}auto* {nestedMemberTypeCode} =");
                source.WriteLine($"{prefix}    reinterpret_cast<::DDS_TypeCode*>({typeCodeExpression})->_data._members[");
                source.WriteLine(FormattableString.Invariant($"{prefix}        {memberIndex}]._representation._typeCode;"));
                var nodeIndex = 0;
                EmitNestedTypeCodeAccessInfo(
                    source,
                    member.Type,
                    memberIndex,
                    nestedMemberTypeCode,
                    ref nodeIndex,
                    plan,
                    stringSequenceElement: true);
            }
        }

        if (declaration.CppBaseType is { } baseType
            && plan.Structs.SingleOrDefault(candidate => candidate.CppType == baseType) is { } baseDeclaration)
        {
            var baseTypeCodeName = $"{namePrefix}_base_type_code";
            source.WriteLine(
                $"{prefix}auto* {baseTypeCodeName} = reinterpret_cast<::DDS_TypeCode*>({typeCodeExpression})->_data._typeCode;");
            EmitAggregateTypeCodeAccessInfo(
                source,
                baseDeclaration,
                baseTypeCodeName,
                $"{namePrefix}_base",
                plan,
                indentation,
                includeTypePluginMetadata);
        }
    }

    /// <summary>Writes sample-access TypeCode metadata for nested values.</summary>
    internal static void EmitNestedTypeCodeAccessInfo(
        SourceWriter source,
        MemberTypePlan type,
        int memberIndex,
        string typeCodeExpression,
        ref int nodeIndex,
        GenerationPlan plan,
        bool stringSequenceElement)
    {
        if (type is MemberTypePlan.Alias alias)
        {
            var aliasTypeCodeName = $"member_{memberIndex}_alias_type_code_{nodeIndex++}";
            source.WriteLine($"        auto* {aliasTypeCodeName} = {typeCodeExpression};");
            if (alias.Target.ContainsCollection || RtiSampleAccess.IsAggregateAliasTarget(alias.Target) || alias.Target.ContainsString)
            {
                source.WriteLine($"        if ({aliasTypeCodeName} == nullptr");
                source.WriteLine($"            || RTIXCdrTypeCode_getKind({aliasTypeCodeName})");
                source.WriteLines("""
                                    != RTI_XCDR_TK_ALIAS) {
                                throw std::runtime_error("RTI 7.7 alias TypeCode is missing");
                            }
                    """);
                if (alias.Target is { ContainsString: true, ContainsCollection: false } stringAlias
                    && !RtiSampleAccess.IsAggregateAliasTarget(stringAlias))
                {
                    source.WriteLine($"        {aliasTypeCodeName}->_sampleAccessInfo =");
                    source.WriteLine("            ::rti::topic::interpreter::detail::sequence_helper<");
                    source.WriteLine($"                {alias.AliasStorageType}>::sample_access_info();");
                }
                else
                {
                    EmitAliasTypeCodeAccessInfo(source, alias, memberIndex, nodeIndex++, aliasTypeCodeName);
                }
            }

            if (TryGetSequenceOfStruct(alias.Target, out var structSequence, out var structType))
            {
                EmitAliasStructSequenceTypeCode(
                    source,
                    structSequence,
                    structType,
                    memberIndex,
                    nodeIndex++,
                    aliasTypeCodeName);
            }

            if (alias.Target.ContainsCollection
                || RtiSampleAccess.IsAggregateAliasTarget(alias.Target)
                || alias.Target.ContainsString)
            {
                EmitNestedTypeCodeAccessInfo(
                    source,
                    alias.Target,
                    memberIndex,
                    $"{aliasTypeCodeName}->_typeCode",
                    ref nodeIndex,
                    plan,
                    stringSequenceElement);
            }

            return;
        }

        if (type is MemberTypePlan.Sequence sequence)
        {
            var sequenceTypeCodeName = $"member_{memberIndex}_sequence_type_code_{nodeIndex++}";
            source.WriteLine($"        auto* {sequenceTypeCodeName} = {typeCodeExpression};");
            source.WriteLine($"        if ({sequenceTypeCodeName} == nullptr");
            source.WriteLine($"            || RTIXCdrTypeCode_getKind({sequenceTypeCodeName})");
            source.WriteLines("""
                                != RTI_XCDR_TK_SEQUENCE) {
                            throw std::runtime_error("RTI 7.7 sequence TypeCode is missing");
                        }
                """);
            source.WriteLine($"        {sequenceTypeCodeName}->_sampleAccessInfo =");
            source.WriteLine("            ::rti::topic::interpreter::detail::sequence_helper<");
            source.WriteLine($"                {sequence.StorageType}>::sample_access_info();");
            EmitNestedTypeCodeAccessInfo(
                source,
                sequence.ElementType,
                memberIndex,
                $"{sequenceTypeCodeName}->_typeCode",
                ref nodeIndex,
                plan,
                stringSequenceElement: true);
            return;
        }

        if (type is MemberTypePlan.Array array)
        {
            var arrayTypeCodeName = $"member_{memberIndex}_array_type_code_{nodeIndex++}";
            source.WriteLine($"        auto* {arrayTypeCodeName} = {typeCodeExpression};");
            source.WriteLine($"        if ({arrayTypeCodeName} == nullptr");
            source.WriteLine($"            || RTIXCdrTypeCode_getKind({arrayTypeCodeName})");
            source.WriteLines("""
                                != RTI_XCDR_TK_ARRAY) {
                            throw std::runtime_error("RTI 7.7 array TypeCode is missing");
                        }
                """);
            EmitNestedTypeCodeAccessInfo(
                source,
                array.ElementType,
                memberIndex,
                $"{arrayTypeCodeName}->_typeCode",
                ref nodeIndex,
                plan,
                stringSequenceElement: true);
            return;
        }

        if (type is MemberTypePlan.Union union
            && plan.Unions.SingleOrDefault(item => item.QualifiedName == union.QualifiedName) is { } unionDeclaration)
        {
            var unionTypeCodeName = $"member_{memberIndex}_union_type_code_{nodeIndex++}";
            var unionTypeCodeAccessor = PluginSymbols.UnionNativeTypeCodeAccessorName(union.QualifiedName);
            source.WriteLine($"        auto* {unionTypeCodeName} = {typeCodeExpression};");
            source.WriteLine($"        auto* {unionTypeCodeName}_native = {unionTypeCodeAccessor}();");
            source.WriteLine($"        {unionTypeCodeName}->_sampleAccessInfo =");
            source.WriteLine($"            {unionTypeCodeName}_native->_sampleAccessInfo;");
            source.WriteLine($"        {unionTypeCodeName}->_typePlugin =");
            source.WriteLine($"            {unionTypeCodeName}_native->_typePlugin;");
            for (var branchIndex = 0; branchIndex < unionDeclaration.Branches.Count; branchIndex++)
            {
                var branch = unionDeclaration.Branches[branchIndex];
                EmitNestedTypeCodeAccessInfo(
                    source,
                    branch.Type,
                    memberIndex,
                    $"{unionTypeCodeName}->_members[{branchIndex}]._representation._typeCode",
                    ref nodeIndex,
                    plan,
                    stringSequenceElement);
            }

            return;
        }

        if (stringSequenceElement && type is MemberTypePlan.String stringType)
        {
            var stringTypeCodeName = $"member_{memberIndex}_string_type_code_{nodeIndex++}";
            source.WriteLine($"        auto* {stringTypeCodeName} = {typeCodeExpression};");
            source.WriteLine($"        if ({stringTypeCodeName} == nullptr) {{");
            source.WriteLine("            throw std::runtime_error(\"RTI 7.7 string TypeCode is missing\");");
            source.WriteLine("        }");
            source.WriteLine($"        {stringTypeCodeName}->_sampleAccessInfo =");
            source.WriteLine("            ::rti::topic::interpreter::detail::sequence_helper<");
            source.WriteLine($"                {stringType.StorageType}>::sample_access_info();");
        }
    }

    private static bool TryGetSequenceOfStruct(
        MemberTypePlan type,
        out MemberTypePlan.Sequence sequence,
        out MemberTypePlan.Struct structure)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        if (type is MemberTypePlan.Sequence candidate)
        {
            var elementType = candidate.ElementType;
            while (elementType is MemberTypePlan.Alias elementAlias)
            {
                elementType = elementAlias.Target;
            }

            if (elementType is MemberTypePlan.Struct structElement)
            {
                sequence = candidate;
                structure = structElement;
                return true;
            }
        }

        sequence = null!;
        structure = null!;
        return false;
    }

    /// <summary>Writes native TypeCode helpers for aliases of aggregates.</summary>
    internal static void EmitAggregateAliasTypeCodeHelpers(
        SourceWriter source,
        IReadOnlyList<AliasPlan> aliases)
    {
            source.WriteComment("Construct RTI raw alias nodes for aggregate typedef chains.");
        source.WriteLine("namespace");
        source.WriteLine("{");
        foreach (var alias in aliases)
        {
            source.WriteLine($"::DDS_TypeCode* {PluginSymbols.AggregateAliasTypeCodeFunctionName(alias.CppType)}();");
        }

        source.BlankLine();
        foreach (var alias in aliases)
        {
            var functionName = PluginSymbols.AggregateAliasTypeCodeFunctionName(alias.CppType);
            var childExpression = alias.MetadataTargetType switch
            {
                MemberTypePlan.Alias targetAlias => PluginSymbols.AggregateAliasTypeCodeFunctionName(targetAlias.AliasStorageType) + "()",
                MemberTypePlan.Struct structure =>
                    "const_cast<::DDS_TypeCode*>(reinterpret_cast<const ::DDS_TypeCode*>(&::rti::topic::dynamic_type<"
                    + structure.StorageType + ">::get().native()))",
                MemberTypePlan.Union union => PluginSymbols.UnionNativeTypeCodeAccessorName(union.QualifiedName) + "()",
                _ => throw new InvalidOperationException($"Aggregate alias '{alias.QualifiedName}' has an unsupported metadata target."),
            };

            source.OpenBlock($"::DDS_TypeCode* {functionName}() ");
            source.WriteLines("""
                static ::DDS_TypeCode type_code = []()
                {
                    ::DDS_TypeCode value = {
                        RTI_XCDR_INITIALIZE_ALIAS_TYPECODE(
                            0, nullptr, DDS_BOOLEAN_FALSE)
                    };
                """);
            source.WriteLine("    value._data._name = const_cast<char*>(\""
                + $"{alias.QualifiedName.Replace(".", "::", StringComparison.Ordinal)}\");");
            source.WriteLine("    value._data._typeCode =");
            if (alias.MetadataTargetType is MemberTypePlan.Struct structureTarget)
            {
                source.WriteLines("""
                            reinterpret_cast<::RTICdrTypeCode*>(
                                const_cast<::DDS_TypeCode*>(
                                    reinterpret_cast<const ::DDS_TypeCode*>(&
                    """);
                source.WriteLine($"                    ::rti::topic::dynamic_type<{structureTarget.StorageType}>::get().native())));");
            }
            else if (alias.MetadataTargetType is MemberTypePlan.Union)
            {
                source.WriteLine("        reinterpret_cast<::RTICdrTypeCode*>(");
                source.WriteLine($"            {childExpression});");
            }
            else
            {
                source.WriteLine("        reinterpret_cast<::RTICdrTypeCode*>(");
                source.WriteLine($"            {childExpression});");
            }
            source.WriteLines("""
                    value._data._annotations._allowedDataRepresentationMask = 5;
                    value._data._sampleAccessInfo = []() -> ::RTIXCdrSampleAccessInfo*
                    {
                        struct AccessInfo
                        {
                            ::RTIXCdrSampleAccessInfo sample;
                            ::RTIXCdrMemberAccessInfo members[1];
                """);
            source.BlankLine();
            source.WriteLines("""
                            AccessInfo()
                                : sample(RTIXCdrSampleAccessInfo_INITIALIZER)
                                , members{}
                            {
                                members[0].bindingMemberValueOffset[0] = 0;
                                sample.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;
                                sample.typeSize[0] =
                                    static_cast<::RTIXCdrUnsignedLong>(
                """);
            source.WriteLine($"                        sizeof({alias.CppType}));");
            source.WriteLines("""
                                sample.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;
                                sample.getMemberValuePointerFcn =
                                    ::rti::topic::interpreter::get_aggregation_value_pointer<
                """);
            source.WriteLine($"                        {alias.CppType}>;");
            source.WriteLines("""
                                sample.memberAccessInfos = members;
                            }
                        };
                        static AccessInfo access_info;
                        return &access_info.sample;
                    }();
                    value._data._typePlugin = []() -> ::RTIXCdrTypePlugin*
                    {
                        static ::RTIXCdrTypePlugin type_plugin = {};
                        return &type_plugin;
                    }();
                    return value;
                }();
                return &type_code;
                """);
            source.CloseBlock();
            source.BlankLine();
        }

        source.WriteLine("} // namespace");
    }

    private static void EmitAliasStructSequenceTypeCode(
        SourceWriter source,
        MemberTypePlan.Sequence sequence,
        MemberTypePlan.Struct structure,
        int memberIndex,
        int nodeIndex,
        string aliasTypeCodeName)
    {
        var sequenceTypeCodeName = $"member_{memberIndex}_alias_sequence_type_code_{nodeIndex}";
        source.WriteLine($"        static ::DDS_TypeCode {sequenceTypeCodeName} = []()");
        source.WriteLines("""
                    {
                        auto type_code =
                            ::rti::topic::interpreter::initialize_sequence_typecode<
            """);
        source.WriteLine($"                    {sequence.StorageType}>(");
        source.WriteLine($"                    {(sequence.IsUnbounded ? "RTIXCdrUnsignedLong_MAX" : sequence.Bound.ToString())});");
        source.WriteLines("""
                        type_code._data._typeCode =
                            reinterpret_cast<::RTICdrTypeCode*>(
                                const_cast<::DDS_TypeCode*>(&
            """);
        source.WriteLine($"                        ::rti::topic::dynamic_type<{structure.StorageType}>::get().native()));");
        source.WriteLine("            return type_code;");
        source.WriteLine("        }();");
        source.WriteLine($"        {aliasTypeCodeName}->_typeCode =");
        source.WriteLine($"            reinterpret_cast<::RTICdrTypeCode*>(&{sequenceTypeCodeName});");
    }

    private static void EmitAliasTypeCodeAccessInfo(
        SourceWriter source,
        MemberTypePlan.Alias alias,
        int memberIndex,
        int aliasIndex,
        string aliasTypeCodeName)
    {
        var accessInfoTypeName = $"Member{memberIndex}AliasAccessInfo{aliasIndex}";
        var accessInfoInstanceName = $"member_{memberIndex}_alias_access_info_{aliasIndex}";
        source.WriteLine($"        struct {accessInfoTypeName}");
        source.WriteLines("""
                    {
                        ::RTIXCdrSampleAccessInfo sample;
                        ::RTIXCdrMemberAccessInfo members[1];
            """);
        source.BlankLine();
        source.WriteLine($"            {accessInfoTypeName}()");
        source.WriteLines("""
                            : sample(RTIXCdrSampleAccessInfo_INITIALIZER)
                            , members{}
                        {
                            members[0].bindingMemberValueOffset[0] = 0;
                            sample.languageBinding = RTI_XCDR_TYPE_BINDING_CPP_11_STL;
                            sample.typeSize[0] =
            """);
        source.WriteLine($"                    static_cast<::RTIXCdrUnsignedLong>(sizeof({alias.AliasStorageType}));");
        source.WriteLines("""
                            sample.useGetMemberValueOnlyWithRef = RTI_XCDR_TRUE;
                            sample.getMemberValuePointerFcn =
                                ::rti::topic::interpreter::get_aggregation_value_pointer<
            """);
        source.WriteLine($"                        {alias.AliasStorageType}>;");
        source.WriteLines("""
                            sample.memberAccessInfos = members;
                        }
                    };
            """);
        source.BlankLine();
        source.WriteLine($"        static {accessInfoTypeName} {accessInfoInstanceName};");
        source.WriteLine($"        {aliasTypeCodeName}->_sampleAccessInfo =");
        source.WriteLine($"            &{accessInfoInstanceName}.sample;");
    }

    /// <summary>Writes the RTI-private type annotations required by the selected profile.</summary>
    internal static void EmitPrivateTypeCodeAnnotations(
        SourceWriter source,
        StructPlan declaration,
        string indentation)
    {
        if (declaration.IsXcdr2Only)
        {
            source.WriteLine($"{indentation}// Generated TypeCode annotation: XCDR2-only representation.");
            source.WriteLine($"{indentation}type->native()._data._annotations._allowedDataRepresentationMask = RTI_XCDR_XCDR2;");
        }

        if (declaration.IsNested && source.RtiVersion == "7.7.0")
        {
            source.WriteLine($"{indentation}// Generated TypeCode annotation: nested values are not topic types.");
            source.WriteLine($"{indentation}type->native()._data._annotations._isNested = RTI_XCDR_TRUE;");
        }

        if (declaration.IsFlatData)
        {
            source.WriteLine($"{indentation}// Generated TypeCode kind flag: FlatData language binding.");
            source.WriteLine($"{indentation}type->native()._data._kind |= DDS_TK_FLAT_DATA_LANGUAGE_BINDING;");
        }
    }

    /// <summary>Writes supported explicit defaults into native TypeCode members.</summary>
    internal static void EmitMemberTypeCodeDefaults(
        SourceWriter source,
        StructPlan declaration,
        string indentation,
        string memberStorage = "type->native()._data._members")
    {
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            if (member.TypeCodeDefaultValue is { } value)
            {
                var isEnumeration = IsEnumerationType(member.Type);
                var isOctet = IsOctetType(member.Type);
                var annotationKind = isEnumeration ? "RTI_XCDR_TK_ENUM" : isOctet ? "RTI_XCDR_TK_OCTET" : "RTI_XCDR_TK_LONG";
                var valueField = isEnumeration ? "enumerated_value" : isOctet ? "octet_value" : "long_value";
                source.WriteLine($"{indentation}// Generated TypeCode annotation: @default({value}).");
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._defaultValue._d = {annotationKind};");
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._defaultValue._u.{valueField} = "
                    + $"{(isOctet ? $"static_cast<::RTIXCdrOctet>({value})" : value)};");
            }

            if (member is { Minimum: { } minimum, Maximum: { } maximum })
            {
                var isOctet = IsOctetType(member.Type);
                var annotationKind = isOctet ? "RTI_XCDR_TK_OCTET" : "RTI_XCDR_TK_LONG";
                var valueField = isOctet ? "octet_value" : "long_value";
                var min = minimum.ToString(System.Globalization.CultureInfo.InvariantCulture) + "L";
                var max = maximum.ToString(System.Globalization.CultureInfo.InvariantCulture) + "L";
                source.WriteLine($"{indentation}// Generated TypeCode annotation: @range({min}.. {max}).");
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._minValue._d = {annotationKind};");
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._minValue._u.{valueField} = "
                    + $"{(isOctet ? $"static_cast<::RTIXCdrOctet>({min})" : min)};");
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._maxValue._d = {annotationKind};");
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._maxValue._u.{valueField} = "
                    + $"{(isOctet ? $"static_cast<::RTIXCdrOctet>({max})" : max)};");
            }

            if (member.TypeCodeUnit is { } unit && source.RtiVersion == "7.7.0")
            {
                source.WriteLine(
                    FormattableString.Invariant($"{indentation}{memberStorage}[{index}")
                    + $"]._annotations._unitValue = (RTIXCdrChar *) {LiteralFormatter.FormatStringLiteral(unit)};");
            }
        }
    }

    private static bool IsEnumerationType(MemberTypePlan type)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type is MemberTypePlan.Enumeration;
    }

    private static bool IsOctetType(MemberTypePlan type)
    {
        while (type is MemberTypePlan.Alias alias)
        {
            type = alias.Target;
        }

        return type is MemberTypePlan.Primitive primitive
            && (primitive.Mapping.IsOctet
                || primitive.StorageType is "::std::int8_t" or "std::int8_t" or "int8_t");
    }
}
