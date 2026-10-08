using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the RTI plugin and dynamic type for a collection or value typedef.</summary>
internal static class CollectionAliasPluginEmitter
{
    /// <summary>Emits the typedef plugin document.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlTypedef declaration, GeneratedTypeNames names, CollectionAliasEmissionPlan plan, string sourceIdlFileName)
    {
        var typeName = names.ManagedTypeName;
        var implementationTypeName = IdlNaming.TypeReference(typeName, names.Namespace, names.ImplementationNamespace);
        var isString = plan.IsString;
        var collectionElementIsAggregate = plan is { IsCollection: true, CollectionElementIsAggregate: true };

        var collectionElementUnmanagedType = string.Empty;
        if (collectionElementIsAggregate)
        {
            collectionElementUnmanagedType = IdlNaming.TypeReference(plan.ElementNativeType, names.ImplementationNamespace);
        }

        var writer = EmissionSupport.CreateSource(names.ImplementationNamespace, EmissionSupport.PluginUsings, sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI runtime plugin for the {typeName} typedef. This implementation detail is not intended for application code.");
        writer.OpenBlock($"internal class {names.PluginTypeName} : InterpretedTypePlugin<{implementationTypeName}, {names.UnmanagedTypeName}>");
        writer.WriteXmlSummary($"Initializes the RTI plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal {names.PluginTypeName}() : base(\"{names.RuntimeTypeName}\", isKeyed: false, CreateDynamicType(isPublic: false))");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteXmlSummary($"Creates the RTI dynamic type description for <see cref=\"{typeName}\"/>.");
        writer.WriteXmlParam("isPublic", "Whether the resulting dynamic type is publicly visible to RTI.");
        writer.WriteXmlReturns("The RTI dynamic type description.");
        writer.OpenBlock("public static DynamicType CreateDynamicType(bool isPublic = true)");
        writer.WriteLine("var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);");
        writer.WriteLine("var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;");
        writer.BlankLine();

        if (plan.IsSequence)
        {
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", tsf.CreateSequenceWithAccessInfo(dtf, {GetDynamicElementType(plan, names.ImplementationNamespace)}, {declaration.Bound}));");
        }
        else if (plan.IsArray)
        {
            var arrayElementType = collectionElementUnmanagedType;
            if (!collectionElementIsAggregate)
            {
                arrayElementType = IdlNaming.TypeReference(plan.ElementNativeType, names.ImplementationNamespace);
            }

            var dimensions = string.Join(", ", declaration.Dimensions);
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", tsf.CreateArrayWithAccessInfo<{arrayElementType}>(dtf, {GetDynamicElementType(plan, names.ImplementationNamespace)}, new uint[] {{ {dimensions} }}));");
        }
        else
        {
            string? dynamicType;

            if (isString)
            {
                if (declaration.IsWideString)
                {
                    writer.WriteLine($"using var dtWString = dtf.CreateWideString({declaration.StringBound});");
                    dynamicType = "dtWString";
                }
                else
                {
                    writer.WriteLine($"using var dtString = dtf.CreateString({declaration.StringBound});");
                    dynamicType = "dtString";
                }
            }
            else
            {
                dynamicType = GetDynamicElementType(plan, names.ImplementationNamespace);
            }

            if (plan.IsString || plan.IsPrimitive)
            {
                writer.WriteLine($"var aliasType = tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", {dynamicType});");
            }
            else
            {
                writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", {dynamicType});");
            }
        }

        if (!plan.IsCollection && (plan.IsString || plan.IsPrimitive))
        {
            EmitAliasAnnotations(writer, declaration, plan);

            writer.BlankLine();
            writer.WriteLine("return aliasType;");
        }

        writer.CloseBlock();
        writer.CloseBlock();
        return [new GeneratedIdlSource(names.Plugin.HintName, writer.ToString())];
    }

    private static void EmitAliasAnnotations(GeneratedSourceWriter writer, IdlTypedef declaration, CollectionAliasEmissionPlan plan)
    {
        if (declaration.IsString)
        {
            writer.BlankLine();
            writer.OpenBrace();
            writer.WriteLine("var annotations = new Annotations(");
            writer.Indent();
            writer.WriteLine("TypeKind.String,");
            writer.WriteLine("defaultValue: new AnnotationParameterValue { StringValue = \"\" },");
            writer.WriteLine("minValue: null,");
            writer.WriteLine("maxValue: null,");
            writer.WriteLine("unit: null);");
            writer.Unindent();
            writer.WriteLine("aliasType.SetAnnotations(annotations);");
            writer.CloseBlock();
            return;
        }

        var primitive = EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan) as PrimitiveEmissionType;
        if (primitive is null)
        {
            return;
        }

        var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
        if (mapping.AnnotationTypeKind is null || mapping.AnnotationValueProperty is null)
        {
            return;
        }

        writer.BlankLine();
        writer.OpenBrace();
        writer.WriteLine("var annotations = new Annotations(");
        writer.Indent();
        writer.WriteLine($"TypeKind.{mapping.AnnotationTypeKind},");
        writer.WriteLine($"defaultValue: new AnnotationParameterValue {{ {mapping.AnnotationValueProperty} = {mapping.AnnotationDefaultLiteral} }},");
        writer.WriteLine($"minValue: {BuildAnnotationValue(mapping.AnnotationValueProperty, mapping.MinimumLiteral)},");
        writer.WriteLine($"maxValue: {BuildAnnotationValue(mapping.AnnotationValueProperty, mapping.MaximumLiteral)},");
        writer.WriteLine("unit: null);");
        writer.Unindent();
        writer.WriteLine("aliasType.SetAnnotations(annotations);");
        writer.CloseBlock();
    }

    private static string BuildAnnotationValue(string property, string? value)
    {
        if (value is null)
        {
            return "null";
        }

        return $"new AnnotationParameterValue {{ {property} = {value} }}";
    }

    private static string GetDynamicElementType(CollectionAliasEmissionPlan plan, string? currentNamespace)
    {
        if (plan.IsString)
        {
            return plan.IsWideString
                ? $"dtf.CreateWideString({plan.StringBound})"
                : $"dtf.CreateString({plan.StringBound})";
        }

        if (plan.IsPrimitive)
        {
            return $"dtf.GetPrimitiveType<{plan.PrimitiveDynamicType}>()";
        }

        var supportType = plan.ElementPlan.SupportType ?? plan.ElementType;
        return $"{IdlNaming.GeneratedSupportTypeReference(supportType, currentNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic)";
    }
}
