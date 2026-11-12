using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Validates supported annotations and member shapes for the C++ profile.</summary>
internal static class ProfileValidation
{
    /// <summary>Checks whether optional storage is supported for a resolved member type.</summary>
    internal static bool IsSupportedOptionalType(
        IdlType idlType,
        MemberTypePlan type,
        bool unboundedSupport,
        bool supportsAnyOptionalPrimitiveCollection) =>
        type is MemberTypePlan.Primitive
        || (idlType is IdlType.StringType { IsWide: false, IsBounded: true }
            && type is MemberTypePlan.String { IsWide: false })
        || IsOptionalUnboundedNarrowString(idlType, type, unboundedSupport)
        || (supportsAnyOptionalPrimitiveCollection
            && (IsOptionalPrimitiveSequence(idlType, type) || IsOptionalPrimitiveArray(idlType, type)))
        || (idlType is IdlType.Sequence
            {
                IsUnbounded: false,
                EffectiveBound: 4,
                Dimensions.Count: 0,
                Element: IdlType.Primitive { Name: "long" },
            }
            && type is MemberTypePlan.Sequence
            {
                Bound: 4,
                ElementType: MemberTypePlan.Primitive { Mapping.StorageType: "::std::int32_t" },
            })
        || IsOptionalBoundedStringSequence(idlType, type)
        || (idlType is IdlType.Array
            {
                Dimensions.Count: 1,
                Dimensions: [2],
                Element: IdlType.Primitive { Name: "long" },
            }
            && type is MemberTypePlan.Array
            {
                Dimensions.Count: 1,
                Dimensions: [2],
                ElementType: MemberTypePlan.Primitive { Mapping.StorageType: "::std::int32_t" },
            });

    /// <summary>Checks whether optional storage wraps a struct or union.</summary>
    internal static bool IsOptionalAggregateType(MemberTypePlan type) => type switch
    {
        MemberTypePlan.Struct or MemberTypePlan.Union => true,
        MemberTypePlan.Alias alias => IsOptionalAggregateType(alias.Target),
        _ => false,
    };

    /// <summary>Checks whether the C++ profile supports this optional collection member.</summary>
    internal static bool IsSupportedOptionalCollectionMember(
        string memberName,
        int? memberId,
        string declarationName,
        IdlExtensibilityKind extensibility,
        IdlType idlType,
        MemberTypePlan type,
        string qualifiedDeclarationName,
        bool unboundedSupport,
        bool supportsAnyOptionalPrimitiveCollection)
    {
        if (type is MemberTypePlan.Primitive
            || IsOptionalAggregateType(type)
            || idlType is IdlType.StringType { IsWide: false, IsBounded: true }
            || IsOptionalUnboundedNarrowString(idlType, type, unboundedSupport))
        {
            return true;
        }

        if (IsOptionalBoundedStringSequence(idlType, type))
        {
            return true;
        }

        if (IsOptionalSelfRecursiveArray(idlType, type, qualifiedDeclarationName))
        {
            return true;
        }

        if (supportsAnyOptionalPrimitiveCollection
            && (IsOptionalPrimitiveSequence(idlType, type) || IsOptionalPrimitiveArray(idlType, type)))
        {
            return true;
        }

        if (declarationName != "Sample" || extensibility != IdlExtensibilityKind.Appendable)
        {
            return false;
        }

        return (memberName == "values" && memberId == 1
                && idlType is IdlType.Sequence { IsUnbounded: false, EffectiveBound: 4, Dimensions.Count: 0,
                    Element: IdlType.Primitive { Name: "long" } }
                && type is MemberTypePlan.Sequence { Bound: 4 })
            || (memberName == "items" && memberId == 2
                && idlType is IdlType.Array { Dimensions: [2], Element: IdlType.Primitive { Name: "long" } }
                && type is MemberTypePlan.Array { Dimensions: [2] });
    }

    private static bool IsOptionalPrimitiveSequence(IdlType idlType, MemberTypePlan type) =>
        idlType is IdlType.Sequence { Dimensions.Count: 0, Element: IdlType.Primitive }
        && type is MemberTypePlan.Sequence { ElementType: MemberTypePlan.Primitive };

    private static bool IsOptionalPrimitiveArray(IdlType idlType, MemberTypePlan type) =>
        idlType is IdlType.Array { Dimensions.Count: 1, Element: IdlType.Primitive }
        && type is MemberTypePlan.Array { Dimensions.Count: 1, ElementType: MemberTypePlan.Primitive };

    private static bool IsOptionalBoundedStringSequence(IdlType idlType, MemberTypePlan type) =>
        idlType is IdlType.Sequence
        {
            IsUnbounded: false,
            EffectiveBound: > 0,
            Dimensions.Count: 0,
            Element: IdlType.StringType { Bound: > 0, IsBounded: true },
        }
        && type is MemberTypePlan.Sequence
        {
            Bound: > 0,
            ElementType: MemberTypePlan.String { MetadataBound: > 0 },
        };

    private static bool IsOptionalUnboundedNarrowString(
        IdlType idlType,
        MemberTypePlan type,
        bool unboundedSupport) =>
        unboundedSupport
        && idlType is IdlType.StringType { IsWide: false, IsUnbounded: true }
        && type is MemberTypePlan.String { IsWide: false, MetadataBound: 0 };

    /// <summary>Checks for the supported optional array of the enclosing struct.</summary>
    internal static bool IsOptionalSelfRecursiveArray(
        IdlType idlType,
        MemberTypePlan type,
        string qualifiedDeclarationName) =>
        idlType is IdlType.Array
        {
            Dimensions: [> 0],
            Element: IdlType.Struct structure,
        }
        && string.Equals(structure.QualifiedName,
            qualifiedDeclarationName, StringComparison.Ordinal)
        && type is MemberTypePlan.Array
        {
            Dimensions: [> 0],
            ElementType: MemberTypePlan.Struct,
        } array
        && string.Equals(((MemberTypePlan.Struct)array.ElementType).QualifiedName,
            IdentifierRules.ToCppQualifiedName(qualifiedDeclarationName), StringComparison.Ordinal);

    /// <summary>Formats a bound type for a C++ profile diagnostic.</summary>
    internal static string FormatType(IdlType type) =>
        type is IdlType.Primitive primitive ? primitive.Name : type.GetType().Name;

    /// <summary>Reports unsupported annotations at their source locations.</summary>
    internal static void AddUnsupportedAnnotations(
        IReadOnlyList<IdlAnnotation> applications,
        IReadOnlyCollection<string> supported,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics,
        string target)
    {
        foreach (var application in applications)
        {
            if (!supported.Contains(application.Name, StringComparer.OrdinalIgnoreCase))
            {
                AddError(diagnostics, input, application.Offset,
                    $"The C++ generator does not support annotation '@{application.Name}' on '{target}'.");
            }
        }
    }

    /// <summary>Validates an annotation whose supported value matches the RTI default.</summary>
    internal static void ValidateRtiDefaultAnnotation(
        IReadOnlyList<IdlAnnotation> applications,
        string annotationName,
        string acceptedValue,
        IdlInput input,
        string target,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var annotations = applications.Where(application =>
            application.Name.Equals(annotationName, StringComparison.OrdinalIgnoreCase)).ToArray();
        if (annotations.Length > 1)
        {
            AddError(diagnostics, input, annotations[1].Offset,
                $"The C++ generator supports one @{annotationName} annotation on '{target}'.");
        }

        foreach (var annotation in annotations)
        {
            var arguments = annotation.Arguments;
            if (arguments is [{ Name: null } argument]
                && argument.Expression.Equals(acceptedValue, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            AddError(diagnostics, input, annotation.Offset,
                $"The C++ generator supports only @{annotationName}({acceptedValue}) on '{target}'.");
        }
    }

    /// <summary>Gets the effective module annotations for the bound declaration.</summary>
    internal static IReadOnlyList<IdlAnnotation> GetModuleApplications(
        IReadOnlyList<IdlDeclaration> declarations,
        IdlClassDeclaration declaration)
    {
        if (string.IsNullOrEmpty(declaration.Namespace))
        {
            return [];
        }

        var declarationIndex = -1;
        for (var index = 0; index < declarations.Count; index++)
        {
            if (ReferenceEquals(declarations[index], declaration))
            {
                declarationIndex = index;
                break;
            }
        }

        if (declarationIndex < 0)
        {
            return [];
        }

        // The semantic model retains a module declaration for each lexical block.
        // Keep the established merged projection for non-auto-ID applications,
        // but take auto-ID only from the latest enclosing block of each module.
        var enclosingModules = declarations
            .Select((candidate, index) => (Module: candidate as IdlModuleDeclaration, Index: index))
            .Where(item => item.Module is not null && item.Index < declarationIndex)
            .Select(item => (item.Module!, item.Index,
                QualifiedName: IdentifierRules.QualifiedIdlName(item.Module!.Namespace, item.Module.Name)))
            .Where(item => declaration.Namespace.Equals(item.QualifiedName, StringComparison.Ordinal)
                || declaration.Namespace.StartsWith(item.QualifiedName + ".", StringComparison.Ordinal))
            .OrderBy(item => item.QualifiedName.Length)
            .ToArray();
        var nonAutoIdApplications = enclosingModules
            .SelectMany(item => item.Item1.Annotations)
            .Where(application => !application.Name.Equals("autoid", StringComparison.OrdinalIgnoreCase));
        var latestAutoIdApplications = enclosingModules
            .GroupBy(item => item.QualifiedName, StringComparer.Ordinal)
            .Select(group => group.OrderBy(item => item.Index).Last())
            .OrderBy(item => item.QualifiedName.Length)
            .SelectMany(item => item.Item1.Annotations)
            .Where(application => application.Name.Equals("autoid", StringComparison.OrdinalIgnoreCase));
        return nonAutoIdApplications.Concat(latestAutoIdApplications).ToArray();
    }

    /// <summary>Validates the requested RTI language binding.</summary>
    internal static void ValidateLanguageBinding(
        IReadOnlyList<IdlAnnotation> applications,
        IdlInput input,
        string target,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var bindings = applications.Where(application =>
            application.Name.Equals("language_binding", StringComparison.OrdinalIgnoreCase)).ToArray();
        if (bindings.Length > 1)
        {
            AddError(diagnostics, input, bindings[1].Offset,
                $"The C++ generator supports one @language_binding annotation on '{target}'.");
        }

        foreach (var binding in bindings)
        {
            var arguments = binding.Arguments;
            if (arguments is [{ Name: null } argument]
                && (argument.Expression.Equals("PLAIN", StringComparison.OrdinalIgnoreCase)
                    || argument.Expression.Equals("FLAT_DATA", StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            AddError(diagnostics, input, binding.Offset,
                $"The C++ generator supports only @language_binding(PLAIN) or @language_binding(FLAT_DATA) on '{target}'.");
        }
    }

    /// <summary>Validates the supported data representation options.</summary>
    internal static void ValidateDataRepresentation(
        IReadOnlyList<IdlAnnotation> applications,
        IdlInput input,
        string target,
        ICollection<IdlDiagnostic> diagnostics)
    {
        foreach (var application in applications.Where(application =>
                     application.Name.Equals("data_representation", StringComparison.OrdinalIgnoreCase)
                     || application.Name.Equals("allowed_data_representation", StringComparison.OrdinalIgnoreCase)))
        {
            var arguments = application.Arguments;
            if (arguments is [{ Name: null } argument]
                && argument.Expression.Equals("XCDR2", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            AddError(diagnostics, input, application.Offset,
                $"The C++ generator supports only @{application.Name}(XCDR2) on '{target}'.");
        }
    }

    /// <summary>Validates the supported FlatData struct shape.</summary>
    internal static void ValidateFlatDataStruct(
        IdlClassDeclaration declaration,
        bool isFlatData,
        ICollection<IdlDiagnostic> diagnostics)
    {
        if (!isFlatData)
        {
            return;
        }

        if (declaration.Extensibility != IdlExtensibilityKind.Final)
        {
            AddError(diagnostics, declaration.SourceInput, 0,
                $"The C++ generator supports FlatData only on final structs ('{declaration.Name}').");
        }

        if (declaration.BaseType is not null || declaration.Fields.Count == 0
            || declaration.Fields.Any(field => field.Type is not IdlType.Primitive { Name: "long" }))
        {
            AddError(diagnostics, declaration.SourceInput, 0,
                $"The C++ generator supports FlatData only on final structs with one or more direct long members ('{declaration.Name}').");
        }
    }

    /// <summary>Validates the supported auto-ID policy.</summary>
    internal static void ValidateHashAutoId(
        IReadOnlyList<IdlAnnotation> applications,
        IdlInput input,
        string target,
        ICollection<IdlDiagnostic> diagnostics)
    {
        foreach (var application in applications.Where(application =>
                     application.Name.Equals("autoid", StringComparison.OrdinalIgnoreCase)))
        {
            var value = application.Arguments.SingleOrDefault()?.Expression ?? "HASH";
            if (!value.Equals("HASH", StringComparison.OrdinalIgnoreCase))
            {
                AddError(diagnostics, input, application.Offset,
                    $"The C++ generator supports only @autoid(HASH) on '{target}'.");
            }
        }
    }

    /// <summary>Validates the supported member name-resolution option.</summary>
    internal static void ValidateResolveNameFalse(
        IReadOnlyList<IdlAnnotation> applications,
        IdlInput input,
        string target,
        ICollection<IdlDiagnostic> diagnostics)
    {
        var resolveApplications = applications.Where(application =>
            application.Name.Equals("resolve_name", StringComparison.OrdinalIgnoreCase)).ToArray();
        if (resolveApplications.Length > 1)
        {
            AddError(diagnostics, input, resolveApplications[1].Offset,
                $"The C++ generator does not support duplicate @resolve_name annotations on '{target}'.");
            return;
        }

        foreach (var application in resolveApplications)
        {
            var arguments = application.Arguments;
            if (arguments.Count == 1
                && arguments[0].Expression.Trim().Equals("false", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            AddError(diagnostics, input, application.Offset,
                $"The C++ generator supports only @resolve_name(false) on '{target}'.");
        }
    }

    /// <summary>Adds a C++ profile diagnostic at the supplied source location.</summary>
    internal static void AddError(ICollection<IdlDiagnostic> diagnostics, IdlInput input, int offset, string message) =>
        diagnostics.Add(new IdlDiagnostic("DDSGCPP001", IdlDiagnosticSeverity.Error, input, offset, message));
}
