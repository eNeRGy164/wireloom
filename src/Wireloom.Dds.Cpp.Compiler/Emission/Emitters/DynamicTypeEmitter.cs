using Wireloom.Dds.Cpp.Emission.Planning;

using Wireloom.Dds.Cpp.Emission.Model;

namespace Wireloom.Dds.Cpp.Emission;

/// <summary>Emits public DynamicType accessors and registers DynamicType members.</summary>
internal static class DynamicTypeEmitter
{
    /// <summary>Writes the translation-unit context for an enum DynamicType.</summary>
    internal static void EmitEnumPluginContext(SourceWriter source, EnumPlan declaration)
    {
        var contextName = declaration.ContextIdentifier;
        source.OpenBlock($"struct {contextName}");
        source.WriteLine("::rti::topic::cdr::GenericTypePluginFactory& factory;");
        source.WriteLine("::dds::core::xtypes::EnumType* type;");
        source.BlankLine();
        source.WriteLine($"{contextName}()");
        source.WriteLine("    : factory(::rti::topic::cdr::GenericTypePluginFactory::instance())");
        source.WriteLine("    , type(factory.create_enum(");
        source.WriteLine($"          \"{declaration.QualifiedName}\",");
        source.WriteLine($"          {declaration.ExtensibilityExpression},");
        source.WriteLine("          {");
        for (var index = 0; index < declaration.Members.Count; index++)
        {
            var member = declaration.Members[index];
            var suffix = index == declaration.Members.Count - 1 ? string.Empty : ",";
            source.WriteLine($"              ::dds::core::xtypes::EnumMember(\"{member.Name}\", "
                + FormattableString.Invariant($"{member.Value}){suffix}"));
        }

        source.WriteLines("""
                      }))
            {
            }
            """);
        source.CloseBlock(";");
        source.BlankLine();
        source.WriteLine($"{contextName}& {contextName}_instance()");
        source.OpenBrace();
        source.WriteLine($"static {contextName} value;");
        source.WriteLine("return value;");
        source.CloseBlock();
    }

    /// <summary>Writes DynamicType member registration and its supported annotations.</summary>
    internal static void EmitMemberRegistration(
        SourceWriter source,
        MemberPlan member,
        bool stagedInitialization = false,
        string? ownerQualifiedName = null,
        string typeMember = "type",
        bool useRecursiveResourceNode = false,
        bool useStructTypeApi = false)
    {
        var optionalSelfRecursiveArray = member.IsOptional
            && ownerQualifiedName is not null
            && RtiSampleAccess.IsSelfRecursiveArrayPlan(member.Type, ownerQualifiedName);
        if (member.IsMustUnderstand)
        {
            source.WriteLine(
                $"        {typeMember}->add_member(::dds::core::xtypes::Member(\"{member.Name}\", "
                + FormatTypeExpression(member.Type, 12, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, source.RtiVersion)
                + ")");
            source.WriteLine(FormattableString.Invariant($"            .id({member.MemberId})"));
            source.WriteLine("            .must_understand(true));");
            return;
        }

        if (member.IsOptional)
        {
            var memberType = FormatTypeExpression(member.Type, 12, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, source.RtiVersion);
            var memberExpression = $"            ::dds::core::xtypes::Member(\"{member.Name}\", {memberType})";
            source.WriteLine($"        {typeMember}->add_member(");
            if (memberExpression.Split('\n')[0].Length <= 120)
            {
                source.WriteLine(memberExpression);
            }
            else
            {
                source.WriteLine("            ::dds::core::xtypes::Member(");
                source.WriteLine($"                \"{member.Name}\",");
                source.WriteLine($"                {memberType})");
            }
            source.WriteLine(FormattableString.Invariant($"                .id({member.MemberId})"));
            source.WriteLine("                .optional(true));");
            return;
        }

        if (useStructTypeApi)
        {
            source.WriteLine($"        {typeMember}->add_member(");
            source.WriteLine($"            ::dds::core::xtypes::Member(\"{member.Name}\",");
            source.WriteLine(
                "                "
                + FormatTypeExpression(member.Type, 16, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, source.RtiVersion)
                + ")");
            source.WriteLine(FormattableString.Invariant($"                .id({member.MemberId})"));
            if (member.IsKey)
            {
                source.WriteLine("                .key(true)");
            }
            source.WriteLine("            );");
            return;
        }

        if (member.IsExternal)
        {
            source.WriteLine($"        factory.add_member(*{typeMember}, \"{member.Name}\",");
            source.WriteLine(
                "            "
                + FormatTypeExpression(member.Type, 12, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, source.RtiVersion)
                + ",");
            source.WriteLine(FormattableString.Invariant($"            {member.MemberId}, false, false, true);"));
            return;
        }

        source.WriteLine($"        factory.add_member(*{typeMember}, \"{member.Name}\",");
        var typeExpression = FormatTypeExpression(
            member.Type, 12, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, source.RtiVersion);
        var memberFlags = FormattableString.Invariant($"{member.MemberId}, {(member.IsKey ? "true" : "false")});");
        if (member.Type is MemberTypePlan.Primitive)
        {
            source.WriteLine($"            {typeExpression}, {memberFlags}");
        }
        else
        {
            source.WriteLine($"            {typeExpression},");
            source.WriteLine($"            {memberFlags}");
        }
    }

    /// <summary>Formats the DynamicType expression for a planned member type.</summary>
    internal static string FormatTypeExpression(
        MemberTypePlan type,
        int indentation,
        bool stagedInitialization = false,
        string? ownerQualifiedName = null,
        bool optionalSelfRecursiveArray = false,
        bool useRecursiveResourceNode = false,
        string rtiVersion = "7.7.0") => type switch
    {
        MemberTypePlan.Primitive primitive =>
            $"::dds::core::xtypes::primitive_type<{(primitive.Mapping.IsOctet && rtiVersion != "7.7.0" ? primitive.StorageType : primitive.Mapping.MetadataExpression)}>()",
        MemberTypePlan.String { IsWide: false } text =>
            $"::dds::core::xtypes::StringType({FormatMetadataBound(text.MetadataBound)})",
        MemberTypePlan.String { IsWide: true } text =>
            $"::dds::core::xtypes::WStringType({FormatMetadataBound(text.MetadataBound)})",
        MemberTypePlan.Enumeration =>
            $"::rti::topic::dynamic_type<{type.StorageType}>::get()",
        MemberTypePlan.Alias alias => FormatAliasTypeExpression(
            alias, indentation, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, rtiVersion),
        MemberTypePlan.Struct structure =>
            stagedInitialization
                && useRecursiveResourceNode
                && !optionalSelfRecursiveArray
                && ownerQualifiedName is not null
                && string.Equals(structure.QualifiedName, ownerQualifiedName, StringComparison.Ordinal)
                    ? $"::rti::topic::dynamic_type<::{structure.QualifiedName}>::get(&wireloom_recursive_resource_node)"
            : stagedInitialization
                ? $"*{PluginSymbols.ContextTypePointer(structure.QualifiedName, ownerQualifiedName, optionalSelfRecursiveArray)}"
                : $"::rti::topic::dynamic_type<::{structure.QualifiedName}>::get()",
        MemberTypePlan.Union union =>
            stagedInitialization
                ? $"*{PluginSymbols.UnionTypeAccessorName(union.QualifiedName)}()"
                : $"::rti::topic::dynamic_type<::{union.QualifiedName}>::get()",
        MemberTypePlan.Sequence sequence =>
            $"*factory.create_sequence(\n{PluginSymbols.Spaces(indentation + 4)}{FormatTypeExpression(sequence.ElementType, indentation + 4, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, rtiVersion)},"
            + $"\n{PluginSymbols.Spaces(indentation + 4)}{(sequence.IsUnbounded ? "RTIXCdrLong_MAX" : sequence.Bound.ToString(System.Globalization.CultureInfo.InvariantCulture))})",
        MemberTypePlan.Array array =>
            $"*factory.create_array(\n{PluginSymbols.Spaces(indentation + 4)}{FormatTypeExpression(array.ElementType, indentation + 4, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, rtiVersion)},"
            + $"\n{PluginSymbols.Spaces(indentation + 4)}{FormatArrayDimensions(array.Dimensions)})",
        _ => throw new InvalidOperationException($"Unsupported C++ member plan '{type.GetType().Name}'."),
    };

    private static string FormatMetadataBound(int bound) => bound == 0
        ? "RTIXCdrLong_MAX"
        : bound.ToString(System.Globalization.CultureInfo.InvariantCulture);

    private static string FormatAliasTypeExpression(
        MemberTypePlan.Alias alias,
        int indentation,
        bool stagedInitialization,
        string? ownerQualifiedName,
        bool optionalSelfRecursiveArray,
        bool useRecursiveResourceNode,
        string rtiVersion)
    {
        var aliasName = alias.AliasStorageType.TrimStart(':');
        return "::dds::core::xtypes::AliasType(\n"
            + $"{PluginSymbols.Spaces(indentation + 4)}\"{aliasName}\",\n"
            + $"{PluginSymbols.Spaces(indentation + 4)}{FormatTypeExpression(alias.Target, indentation + 4, stagedInitialization, ownerQualifiedName, optionalSelfRecursiveArray, useRecursiveResourceNode, rtiVersion)}\n"
            + $"{PluginSymbols.Spaces(indentation)})";
    }

    private static string FormatArrayDimensions(IReadOnlyList<int> dimensions) => dimensions.Count switch
    {
        1 => dimensions[0].ToString(System.Globalization.CultureInfo.InvariantCulture),
        _ => "{" + string.Join(", ", dimensions) + "}",
    };

    /// <summary>Writes the public RTI DynamicType accessor for a planned type.</summary>
    internal static void EmitDynamicTypeAccessor(
        SourceWriter source,
        StructPlan declaration,
        bool stagedInitialization,
        bool metadataOnlyTypeInitialization)
    {
        source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
        source.OpenBlock($"dynamic_type<{declaration.CppType}>::get()");
        if (stagedInitialization)
        {
            source.WriteLine(""
                + $"{(metadataOnlyTypeInitialization ? "wireloom_initialize_dynamic_types();" : "wireloom_initialize_type_plugins();")}");
        }

        source.WriteLine($"return *{PluginSymbols.ContextFunctionName(declaration)}().type;");
        source.CloseBlock();
        source.BlankLine();
        if (declaration.HasSelfRecursiveSequence
            || RtiSampleAccess.HasOptionalSelfRecursiveArray(declaration))
        {
            source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
            var resourceNodeType = source.RtiVersion == "7.7.0"
                ? "RTIXCdrResourceNode"
                : "RTICdrResourceNode";
            var resourceNodeInitializer = resourceNodeType + "_INITIALIZER";
            var typeCodeVisitFunction = source.RtiVersion == "7.7.0"
                ? "RTIXCdrTypeCode_isResourceVisited"
                : "RTICdrTypeCode_isResourceVisited";
            source.OpenBlock($"dynamic_type<{declaration.CppType}>::get(::{resourceNodeType}* parentVisitedNode)");
            source.WriteLine($"auto& context = {PluginSymbols.ContextFunctionName(declaration)}();");
            source.WriteLine($"::{resourceNodeType} node = {resourceNodeInitializer};");
            source.WriteLines($$"""
                if (parentVisitedNode != nullptr && context.type != nullptr) {
                    node.prev = parentVisitedNode;
                    node.resource = &context.type->native();
                    if ({{typeCodeVisitFunction}}(
                            node.resource, parentVisitedNode, nullptr)) {
                        return *context.type;
                    }
                }
                wireloom_initialize_type_plugins();
                """);
            source.WriteLine($"return *{PluginSymbols.ContextFunctionName(declaration)}().type;");
            source.CloseBlock();
            source.BlankLine();
        }

        if (declaration.IsFlatData)
        {
            source.WriteLine($"const dynamic_type<{declaration.FlatDataPlainHelperType}>::type&");
            source.OpenBlock($"dynamic_type<{declaration.FlatDataPlainHelperType}>::get()");
            source.WriteLine("wireloom_initialize_dynamic_types();");
            source.WriteLine($"return *{PluginSymbols.ContextFunctionName(declaration)}().plain_helper_type;");
            source.CloseBlock();
            source.BlankLine();
        }
    }

    /// <summary>Writes the public RTI DynamicType accessor for a planned type.</summary>
    internal static void EmitDynamicTypeAccessor(SourceWriter source, EnumPlan declaration)
    {
        source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
        source.OpenBlock($"dynamic_type<{declaration.CppType}>::get()");
        source.WriteLine($"return *{PluginSymbols.ContextFunctionName(declaration)}().type;");
        source.CloseBlock();
        source.BlankLine();
    }

    /// <summary>Writes the public RTI DynamicType accessor for a planned type.</summary>
    internal static void EmitDynamicTypeAccessor(
        SourceWriter source,
        UnionPlan declaration,
        bool metadataOnlyTypeInitialization)
    {
        source.WriteLine($"const dynamic_type<{declaration.CppType}>::type&");
        source.OpenBlock($"dynamic_type<{declaration.CppType}>::get()");
        source.WriteLine(""
            + $"{(metadataOnlyTypeInitialization ? "wireloom_initialize_dynamic_types();" : "wireloom_initialize_type_plugins();")}");
        source.WriteLine($"return *{PluginSymbols.ContextFunctionName(declaration)}().type;");
        source.CloseBlock();
        source.BlankLine();
    }

    /// <summary>Writes the public RTI DynamicType accessor for a planned type.</summary>
    internal static void EmitDynamicTypeAccessor(SourceWriter source, AliasPlan declaration)
    {
        source.WriteLine($"const dynamic_type<{declaration.CppType}_AliasTag_t>::type&");
        source.OpenBlock($"dynamic_type<{declaration.CppType}_AliasTag_t>::get()");
        if (declaration.MetadataTargetType.ContainsCollection)
        {
            source.WriteLine("auto& factory = ::rti::topic::cdr::GenericTypePluginFactory::instance();");
        }
        source.WriteLine("static const dynamic_type<");
        source.WriteLine($"    {declaration.CppType}_AliasTag_t>::type type(");
        source.WriteLine($"    \"{declaration.QualifiedName.Replace(".", "::", StringComparison.Ordinal)}\",");
        source.WriteLine($"    {FormatTypeExpression(declaration.MetadataTargetType, 8, rtiVersion: source.RtiVersion)});");
        if (declaration.ResolvedType is MemberTypePlan.Union)
        {
            source.WriteLines("""
                auto* native_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(
                    const_cast<::DDS_TypeCode*>(&type.native()));
                auto* alias_type_code = reinterpret_cast<::RTIXCdrTypeCode*>(
                """);
            source.WriteLine($"    {PluginSymbols.AggregateAliasTypeCodeFunctionName(declaration.CppType)}());");
            source.WriteLine("native_type_code->_sampleAccessInfo = alias_type_code->_sampleAccessInfo;");
            source.WriteLine("native_type_code->_typePlugin = alias_type_code->_typePlugin;");
        }
        source.WriteLine("return type;");
        source.CloseBlock();
        source.BlankLine();
    }
}
