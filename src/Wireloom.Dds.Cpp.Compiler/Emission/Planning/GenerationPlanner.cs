using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Coordinates C++ declaration validation and projection into an emission plan.</summary>
internal static class GenerationPlanner
{
    /// <summary>Validates declarations and creates an immutable emission plan.</summary>
    internal static GenerationPlan? Create(
        IReadOnlyList<IdlDeclaration> declarations,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics,
        RtiApiProfile? profile = null)
    {
        ValidateDeclarations(declarations, input, diagnostics);
        var constants = PlanConstants(declarations.OfType<IdlConstantDeclaration>(), diagnostics);

        var enumDeclarations = declarations.OfType<IdlEnumDeclaration>().ToArray();
        var classes = declarations.OfType<IdlClassDeclaration>().ToArray();
        var unionDeclarations = declarations.OfType<IdlUnionDeclaration>().ToArray();
        var unionNames = unionDeclarations
            .Select(item => IdentifierRules.QualifiedIdlName(item.Declaration.Namespace, item.Declaration.Name))
            .ToHashSet(StringComparer.Ordinal);
        var typedefDeclarations = declarations.OfType<IdlTypedefDeclaration>().ToArray();
        if (classes.Length == 0 && enumDeclarations.Length == 0 && unionDeclarations.Length == 0
            && typedefDeclarations.Length == 0)
        {
            ProfileValidation.AddError(diagnostics, input, 0,
                "The C++ generator requires at least one struct, enum, union, or typedef declaration.");
            return null;
        }

        var enumPlans = PlanEnums(enumDeclarations, input, diagnostics);
        var enumLookup = enumPlans.ToDictionary(item => item.IdlQualifiedName, StringComparer.Ordinal);
        var structNames = classes
            .Select(item => IdentifierRules.QualifiedIdlName(item.Namespace, item.Name))
            .ToHashSet(StringComparer.Ordinal);
        var aliasNames = new Dictionary<string, AliasPlan>(StringComparer.Ordinal);
        var typeResolver = new MemberTypeResolver(enumLookup, structNames, unionNames, aliasNames);
        var aliases = PlanAliases(typedefDeclarations, input, diagnostics, typeResolver, aliasNames);
        var structs = StructPlanner.PlanStructs(
            classes, declarations, input.UnboundedSupport, (profile ?? RtiApiProfile.Rti770).RtiVersion,
            diagnostics, typeResolver);
        var unions = UnionPlanner.PlanUnions(unionDeclarations, input, diagnostics, typeResolver);
        var initialization = TypeInitializationPlanner.Create(structs, unions, input, diagnostics);

        if (diagnostics.Any(diagnostic => diagnostic.Severity == IdlDiagnosticSeverity.Error))
        {
            return null;
        }

        var plan = new GenerationPlan(
            Path.GetFileNameWithoutExtension(input.Path), constants, enumPlans, aliases, structs, unions,
            initialization, profile ?? RtiApiProfile.Rti770);
        NativeProfileAbiValidation.Validate(plan, input, diagnostics);
        Rti612ProfileValidation.Validate(plan, input, diagnostics);
        return diagnostics.Any(diagnostic => diagnostic.Severity == IdlDiagnosticSeverity.Error)
            ? null
            : plan;
    }

    private static void ValidateDeclarations(
        IReadOnlyList<IdlDeclaration> declarations, IdlInput input, ICollection<IdlDiagnostic> diagnostics)
    {
        foreach (var module in declarations.OfType<IdlModuleDeclaration>())
        {
            ProfileValidation.AddUnsupportedAnnotations(module.Annotations,
                ["autoid", "language_binding", "data_representation", "allowed_data_representation"],
                input, diagnostics, module.Name);
            ProfileValidation.ValidateHashAutoId(module.Annotations, input, module.Name, diagnostics);
            ProfileValidation.ValidateLanguageBinding(module.Annotations, input, module.Name, diagnostics);
            ProfileValidation.ValidateDataRepresentation(module.Annotations, input, module.Name, diagnostics);
        }

        foreach (var declaration in declarations.Where(declaration =>
                     declaration is not (IdlModuleDeclaration or IdlClassDeclaration or IdlUnionDeclaration
                         or IdlConstantDeclaration or IdlEnumDeclaration or IdlTypedefDeclaration)))
        {
            ProfileValidation.AddError(diagnostics, input, 0,
                $"The C++ generator does not support declaration '{declaration.GetType().Name}'.");
        }
    }

    private static IReadOnlyList<ConstantPlan> PlanConstants(
        IEnumerable<IdlConstantDeclaration> declarations, ICollection<IdlDiagnostic> diagnostics)
    {
        var constants = new List<ConstantPlan>();
        foreach (var declaration in declarations)
        {
            IdentifierRules.Validate(declaration.Name, declaration.SourceInput,
                declaration.SourceOffset, diagnostics, "constant");
            ProfileValidation.AddUnsupportedAnnotations(declaration.Annotations, [], declaration.SourceInput,
                diagnostics, declaration.Name);
            if (string.Equals(declaration.Type, "string", StringComparison.Ordinal)
                && declaration.Value is IdlScalarValue.Text { IsWide: false } text)
            {
                constants.Add(new ConstantPlan(
                    declaration.Name,
                    declaration.Namespace,
                    "const char*",
                    LiteralFormatter.FormatStringLiteral(text.Value)));
                continue;
            }

            if (declaration.Value is IdlScalarValue.Enumeration enumeration)
            {
                var enumType = enumeration.TypeQualifiedName.Replace(".", "::", StringComparison.Ordinal);
                constants.Add(new ConstantPlan(
                    declaration.Name,
                    declaration.Namespace,
                    $"::{enumType}",
                    $"::{enumType}::{enumeration.MemberName}",
                    IsEnum: true));
                continue;
            }

            if (!PrimitiveType.TryResolve(new IdlType.Primitive(declaration.Type), out var primitive)
                || declaration.Value is not IdlScalarValue.Integral value)
            {
                ProfileValidation.AddError(diagnostics, declaration.SourceInput, declaration.SourceOffset,
                    $"The C++ scalar generator does not support the value or type of constant '{declaration.Name}'.");
                continue;
            }

            constants.Add(new ConstantPlan(
                declaration.Name,
                declaration.Namespace,
                primitive.StorageType,
                LiteralFormatter.FormatDefault(value, primitive)));
        }

        return constants;
    }

    private static IReadOnlyList<EnumPlan> PlanEnums(
        IReadOnlyList<IdlEnumDeclaration> enumDeclarations, IdlInput input, ICollection<IdlDiagnostic> diagnostics)
    {
        var enumPlans = new List<EnumPlan>(enumDeclarations.Count);
        foreach (var enumDeclaration in enumDeclarations)
        {
            var declaration = enumDeclaration.Declaration;
            ProfileValidation.AddUnsupportedAnnotations(enumDeclaration.Annotations,
                ["final", "appendable", "extensibility"], input, diagnostics, declaration.Name);
            IdentifierRules.Validate(declaration.Name, input, 0, diagnostics, "enum");
            foreach (var segment in IdentifierRules.NamespaceSegments(declaration.Namespace))
            {
                IdentifierRules.Validate(segment, input, 0, diagnostics, "namespace");
            }

            var members = new List<EnumMemberPlan>(declaration.Members.Count);
            foreach (var member in declaration.Members)
            {
                IdentifierRules.Validate(member.Name, input, 0, diagnostics, "enumerator");
                members.Add(new EnumMemberPlan(
                    member.Name,
                    member.Value,
                    member.HasExplicitValue,
                    member == declaration.DefaultMember));
            }

            enumPlans.Add(new EnumPlan(
                declaration.Name,
                declaration.Namespace,
                declaration.Extensibility,
                members));
        }

        return enumPlans;
    }

    private static IReadOnlyList<AliasPlan> PlanAliases(
        IReadOnlyList<IdlTypedefDeclaration> typedefDeclarations,
        IdlInput input,
        ICollection<IdlDiagnostic> diagnostics,
        MemberTypeResolver typeResolver,
        Dictionary<string, AliasPlan> aliasNames)
    {
        var aliasPlans = new List<AliasPlan>();
        foreach (var typedef in typedefDeclarations)
        {
            var alias = typedef.Declaration;
            if (alias.TargetWasEscaped
                || !typeResolver.TryResolveAliasTarget(alias,
                    out var targetType, out var resolvedType, out var structTargetType))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator does not support typedef '{alias.Name}' with this target type.");
                continue;
            }

            IdentifierRules.Validate(alias.Name, input, 0, diagnostics, "typedef");
            foreach (var segment in IdentifierRules.NamespaceSegments(alias.Namespace))
            {
                IdentifierRules.Validate(segment, input, 0, diagnostics, "namespace");
            }

            ProfileValidation.AddUnsupportedAnnotations(typedef.Annotations, ["default"], input, diagnostics, alias.Name);
            if (typedef.Annotations.Any(application =>
                    application.Name.Equals("default", StringComparison.OrdinalIgnoreCase))
                && (alias.ValueMetadata?.DefaultValue is not { } typedefDefault
                    || !LiteralFormatter.TryFormatMemberDefault(typedefDefault, resolvedType, out _)))
            {
                ProfileValidation.AddError(diagnostics, input, 0,
                    $"The C++ generator does not support the typed default on typedef '{alias.Name}'.");
            }
            var metadataTargetType = aliasNames.Values.FirstOrDefault(candidate =>
                candidate.CppType.Equals(targetType, StringComparison.Ordinal))?.MetadataType ?? resolvedType;
            var aliasPlan = new AliasPlan(
                alias.Name,
                alias.Namespace,
                targetType,
                structTargetType,
                resolvedType,
                metadataTargetType);
            aliasPlans.Add(aliasPlan);
            aliasNames.Add(aliasPlan.QualifiedName, aliasPlan);
        }

        return aliasPlans;
    }
}
