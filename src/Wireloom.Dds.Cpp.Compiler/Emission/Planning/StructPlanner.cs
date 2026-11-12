using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Validates and projects IDL structs and their member metadata into C++ plans.</summary>
internal static class StructPlanner
{
    /// <summary>Validates and projects supported struct declarations in source order.</summary>
    internal static IReadOnlyList<StructPlan> PlanStructs(
        IReadOnlyList<IdlClassDeclaration> classes,
        IReadOnlyList<IdlDeclaration> declarations,
        bool unboundedSupport,
        string rtiVersion,
        ICollection<IdlDiagnostic> diagnostics,
        MemberTypeResolver typeResolver)
    {
        var plans = new List<StructPlan>(classes.Count);
        foreach (var declaration in classes)
        {
            IdentifierRules.Validate(declaration.Name, declaration.SourceInput, 0, diagnostics, "struct");
            foreach (var segment in IdentifierRules.NamespaceSegments(declaration.Namespace))
            {
                IdentifierRules.Validate(segment, declaration.SourceInput, 0, diagnostics, "namespace");
            }

            var baseTypeName = declaration.BaseType is null ? null : IdentifierRules.ToCppQualifiedName(declaration.BaseType);
            if (baseTypeName is not null && !typeResolver.ContainsStruct(declaration.BaseType!))
            {
                ProfileValidation.AddError(diagnostics, declaration.SourceInput, declaration.BaseTypeOffset,
                    $"The C++ generator does not support base type '{declaration.BaseType}' on '{declaration.Name}'.");
            }

            var moduleApplications = ProfileValidation.GetModuleApplications(declarations, declaration);
            var effectiveApplications = moduleApplications.Concat(declaration.Annotations).ToArray();
            ProfileValidation.AddUnsupportedAnnotations(declaration.Annotations,
                ["topic", "nested", "final", "appendable", "mutable", "extensibility", "autoid", "data_representation", "allowed_data_representation", "language_binding", "transfer_mode"], declaration.SourceInput,
                diagnostics, declaration.Name);
            ProfileValidation.ValidateLanguageBinding(effectiveApplications, declaration.SourceInput, declaration.Name, diagnostics);
            ProfileValidation.ValidateRtiDefaultAnnotation(effectiveApplications, "transfer_mode", "INBAND",
                declaration.SourceInput, declaration.Name, diagnostics);
            var languageBinding = effectiveApplications
                .FirstOrDefault(application => application.Name.Equals("language_binding", StringComparison.OrdinalIgnoreCase))
                ?.Arguments.SingleOrDefault()?.Expression;
            var hasPlainLanguageBinding = string.Equals(languageBinding, "PLAIN", StringComparison.OrdinalIgnoreCase);
            var isFlatData = string.Equals(languageBinding, "FLAT_DATA", StringComparison.OrdinalIgnoreCase);
            var hasInbandTransferMode = effectiveApplications.Any(application =>
                application.Name.Equals("transfer_mode", StringComparison.OrdinalIgnoreCase));
            var topicApplications = declaration.Annotations.Where(application =>
                application.Name.Equals("topic", StringComparison.OrdinalIgnoreCase)).ToArray();
            var nestedApplications = declaration.Annotations.Where(application =>
                application.Name.Equals("nested", StringComparison.OrdinalIgnoreCase)).ToArray();
            if (topicApplications.Length > 1 || nestedApplications.Length > 1)
            {
                ProfileValidation.AddError(diagnostics, declaration.SourceInput,
                    (topicApplications.Length > 1 ? topicApplications[1] : nestedApplications[1]).Offset,
                    $"The C++ generator supports one @topic or @nested annotation on struct '{declaration.Name}'.");
            }

            foreach (var annotation in topicApplications.Concat(nestedApplications))
            {
                if (annotation.Arguments.Count != 0)
                {
                    ProfileValidation.AddError(diagnostics, declaration.SourceInput, annotation.Offset,
                        $"The C++ generator supports @{annotation.Name} without arguments on a struct.");
                }
            }

            var hasTopicAnnotation = topicApplications.Length > 0;
            var isNested = nestedApplications.Length > 0;
            if (hasTopicAnnotation && isNested)
            {
                ProfileValidation.AddError(diagnostics, declaration.SourceInput, nestedApplications[0].Offset,
                    $"The C++ generator does not support combining @topic and @nested on struct '{declaration.Name}'.");
            }

            var isTopic = !isNested;
            var representationApplications = effectiveApplications.Where(application =>
                application.Name.Equals("data_representation", StringComparison.OrdinalIgnoreCase)
                || application.Name.Equals("allowed_data_representation", StringComparison.OrdinalIgnoreCase)).ToArray();
            var isXcdr2Only = representationApplications.Length > 0;
            if (representationApplications.Length > 1)
            {
                ProfileValidation.AddError(diagnostics, declaration.SourceInput, representationApplications[1].Offset,
                    "The C++ generator supports one @data_representation or @allowed_data_representation annotation per struct.");
            }

            foreach (var representationApplication in representationApplications)
            {
                var arguments = representationApplication.Arguments;
                if (arguments.Count != 1
                    || arguments[0].Name is not null
                    || !arguments[0].Expression.Equals("XCDR2", StringComparison.OrdinalIgnoreCase))
                {
                    ProfileValidation.AddError(diagnostics, declaration.SourceInput,
                        representationApplication.Offset,
                        $"The C++ generator supports only @{representationApplication.Name}(XCDR2) on a struct.");
                }
            }
            ProfileValidation.ValidateFlatDataStruct(declaration, isFlatData, diagnostics);
            ProfileValidation.ValidateHashAutoId(declaration.Annotations, declaration.SourceInput,
                declaration.Name, diagnostics);
            var fields = PlanMembers(declaration, unboundedSupport, rtiVersion, diagnostics, typeResolver);

            if (fields.Count == declaration.Fields.Count)
            {
                plans.Add(new StructPlan(
                    declaration.Name,
                    declaration.Namespace,
                    declaration.Extensibility,
                    fields,
                    baseTypeName,
                    isXcdr2Only,
                    isTopic,
                    isNested,
                    hasPlainLanguageBinding,
                    hasInbandTransferMode,
                    isFlatData,
                    declaration.IsValueType));
            }
        }

        return plans;
    }

    private static IReadOnlyList<MemberPlan> PlanMembers(
        IdlClassDeclaration declaration,
        bool unboundedSupport,
        string rtiVersion,
        ICollection<IdlDiagnostic> diagnostics,
        MemberTypeResolver typeResolver)
    {
        var fields = new List<MemberPlan>(declaration.Fields.Count);
        foreach (var field in declaration.Fields)
        {
            IdentifierRules.Validate(field.Name, field.SourceInput ?? declaration.SourceInput,
                field.SourceOffset, diagnostics, "member");
            if (field.Name.Equals(declaration.Name, StringComparison.Ordinal))
            {
                ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                    $"The C++ member name '{field.Name}' conflicts with its containing struct name.");
            }

            ProfileValidation.AddUnsupportedAnnotations(field.Annotations,
                ["key", "default", "min", "max", "range", "unit", "id", "optional", "hashid", "must_understand", "resolve_name", "external"],
                field.SourceInput ?? declaration.SourceInput, diagnostics, field.Name);
            ProfileValidation.ValidateResolveNameFalse(
                field.Annotations,
                field.SourceInput ?? declaration.SourceInput,
                field.Name,
                diagnostics);

            if (!typeResolver.TryResolveMemberType(
                    field.Type,
                    out var memberType,
                    unboundedSupport))
            {
                ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                    $"The C++ generator does not support member '{field.Name}' with type '{ProfileValidation.FormatType(field.Type)}'.");
                continue;
            }

            if (field.Metadata.IsExternal
                && (field.Metadata.IsOptional
                    || field.Type is not IdlType.StringType
                    {
                        IsWide: false,
                        IsBounded: true,
                        IsUnbounded: false,
                        Bound: > 0,
                    }
                    || memberType is not MemberTypePlan.String { IsWide: false, MetadataBound: > 0 }))
            {
                ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                    $"The C++ generator supports @external only on required directly bounded narrow string members ('{field.Name}').");
                continue;
            }

            var typeCodeUnit = field.Metadata.ValueMetadata?.Unit;
            if (typeCodeUnit is not null
                && (field.Type is not IdlType.Primitive { Name: "long" }
                    || field.Metadata.ValueMetadata?.Minimum is null
                    || field.Metadata.ValueMetadata.Maximum is null))
            {
                var unitOffset = field.Annotations.FirstOrDefault(application =>
                    application.Name.Equals("unit", StringComparison.OrdinalIgnoreCase))?.Offset
                    ?? field.SourceOffset;
                ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, unitOffset,
                    $"The C++ generator supports @unit only with a bounded @range on a long member ('{field.Name}').");
                continue;
            }

            var isMustUnderstand = field.Metadata.IsMustUnderstand;
            if (isMustUnderstand
                && (declaration.Extensibility != IdlExtensibilityKind.Appendable
                    || field.Metadata.IsKey
                    || field.Metadata.IsOptional
                    || field.Annotations.Any(application =>
                        application.Name.Equals("hashid", StringComparison.OrdinalIgnoreCase))
                    || memberType is not MemberTypePlan.Primitive))
            {
                ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                    $"The C++ generator supports must-understand only on a required primitive member of an appendable type ('{field.Name}').");
                continue;
            }

            var supportsAnyOptionalPrimitiveCollection = rtiVersion is "6.1.2" or "7.3.1";
            var isSupportedOptionalType = ProfileValidation.IsSupportedOptionalType(
                field.Type, memberType, unboundedSupport, supportsAnyOptionalPrimitiveCollection)
                || ProfileValidation.IsOptionalSelfRecursiveArray(
                    field.Type, memberType, IdentifierRules.ToIdlQualifiedName(declaration))
                || ProfileValidation.IsOptionalAggregateType(memberType);
            var isSupportedOptionalCollection = ProfileValidation.IsSupportedOptionalCollectionMember(
                field.Name, field.Metadata.EffectiveMemberId, declaration.Name, declaration.Extensibility,
                field.Type, memberType, IdentifierRules.ToIdlQualifiedName(declaration), unboundedSupport,
                supportsAnyOptionalPrimitiveCollection);
            if (field.Metadata.IsOptional
                && (!isSupportedOptionalType || !isSupportedOptionalCollection))
            {
                ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                    $"The C++ generator does not support optional member '{field.Name}' with type '{ProfileValidation.FormatType(field.Type)}'.");
                continue;
            }

            var plannedMemberType = memberType;
            if (field.Metadata.ValueMetadata?.DefaultValue is { } boundDefault)
            {
                if (!LiteralFormatter.TryFormatMemberDefault(boundDefault, memberType, out var boundExpression))
                {
                    ProfileValidation.AddError(diagnostics, field.SourceInput ?? declaration.SourceInput, field.SourceOffset,
                        $"The C++ generator cannot format the default value for member '{field.Name}'.");
                    continue;
                }

                plannedMemberType = memberType with { DefaultValue = boundExpression };
            }

            var typeCodeDefaultValue = LiteralFormatter.GetTypeCodeDefaultValue(
                field.Type,
                memberType,
                plannedMemberType,
                field.Metadata.ValueMetadata?.DefaultValue,
                typeResolver.Enums);
            fields.Add(new MemberPlan(
                field.Name,
                plannedMemberType,
                field.Metadata.EffectiveMemberId ?? fields.Count,
                field.Metadata.IsKey,
                field.Metadata.IsOptional,
                field.Annotations.Any(application =>
                    application.Name.Equals("hashid", StringComparison.OrdinalIgnoreCase)),
                isMustUnderstand,
                field.Metadata.ValueMetadata?.Minimum,
                field.Metadata.ValueMetadata?.Maximum,
                typeCodeDefaultValue,
                typeCodeUnit,
                field.Metadata.IsExternal,
                field.Annotations.Any(application =>
                    application.Name.Equals("resolve_name", StringComparison.OrdinalIgnoreCase))));
        }

        return fields;
    }
}
