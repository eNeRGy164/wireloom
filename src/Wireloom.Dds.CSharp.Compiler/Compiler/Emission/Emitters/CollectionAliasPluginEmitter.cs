using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the RTI plugin and dynamic type for a collection or value typedef.</summary>
internal static class CollectionAliasPluginEmitter
{
    /// <summary>Emits the typedef plugin document.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(IdlTypedef declaration, GeneratedTypeNames names, CollectionAliasEmissionPlan plan, string sourceIdlFileName, RtiApiProfile profile)
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

        var writer = EmissionSupport.CreateSource(
            names.ImplementationNamespace,
            EmissionSupport.GetPluginUsings(
                [plan.ElementPlan],
                usesExtensibility: false,
                usesAnnotations: profile.SupportsDynamicTypeAnnotations && !plan.IsCollection && (plan.IsString || plan.IsPrimitive || plan.IsEnum)),
            sourceIdlFileName);

        writer.WriteXmlSummary($"Provides the RTI runtime plugin for the {typeName} typedef. This implementation detail is not intended for application code.");
        writer.OpenBlock($"internal class {names.PluginTypeName} : InterpretedTypePlugin<{implementationTypeName}, {names.UnmanagedTypeName}>");
        writer.WriteXmlSummary($"Initializes the RTI plugin for <see cref=\"{typeName}\"/>.");
        writer.OpenBlock($"internal {names.PluginTypeName}() : base(\"{names.RuntimeTypeName}\", isKeyed: false, CreateDynamicType(isPublic: false))");
        EmissionSupport.EmitPluginCompatibility(writer, profile);
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
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", tsf.CreateSequenceWithAccessInfo(dtf, {GetDynamicElementType(plan, names.ImplementationNamespace, plan.IsRecursiveUnboundedSequenceElement)}, {(plan.IsUnboundedSequence ? "SequenceType.Unbounded" : plan.SequenceBound.ToString())}));");
        }
        else if (plan.IsArray)
        {
            var arrayElementType = collectionElementUnmanagedType;
            if (plan.IsString)
            {
                arrayElementType = plan.IsWideString ? "NativeWstring" : "NativeString";
            }
            else if (!collectionElementIsAggregate)
            {
                arrayElementType = IdlNaming.TypeReference(plan.ElementNativeType, names.ImplementationNamespace);
            }

            var dimensions = string.Join(", ", declaration.Dimensions);
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", tsf.CreateArrayWithAccessInfo<{arrayElementType}>(dtf, {GetDynamicElementType(plan, names.ImplementationNamespace)}, [{dimensions}]));");
        }
        else
        {
            string? dynamicType;

            if (isString)
            {
                if (plan.IsWideString)
                {
                    writer.WriteLine(plan.IsUnboundedString ? "var dtWString = dtf.CreateWideString();" : $"var dtWString = dtf.CreateWideString({plan.StringBound});");
                    dynamicType = "dtWString";
                }
                else
                {
                    writer.WriteLine(plan.IsUnboundedString ? "var dtString = dtf.CreateString();" : $"var dtString = dtf.CreateString({plan.StringBound});");
                    dynamicType = "dtString";
                }
            }
            else
            {
                dynamicType = GetDynamicElementType(plan, names.ImplementationNamespace);
            }

            if (plan.IsString || plan.IsPrimitive || plan.IsEnum)
            {
                writer.WriteLine($"var aliasType = tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", {dynamicType});");
            }
            else
            {
                writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{names.UnmanagedTypeName}>(dtf, \"{typeName}\", {dynamicType});");
            }
        }

        if (!plan.IsCollection && (plan.IsString || plan.IsPrimitive || plan.IsEnum))
        {
            if (profile.SupportsDynamicTypeAnnotations)
            {
                EmitAliasAnnotations(writer, plan);
            }

            writer.BlankLine();
            writer.WriteLine("return aliasType;");
        }

        writer.CloseBlock();
        writer.CloseBlock();
        return [new GeneratedIdlSource(names.Plugin.HintName, writer.ToString())];
    }

    private static void EmitAliasAnnotations(GeneratedSourceWriter writer, CollectionAliasEmissionPlan plan)
    {
        if (plan.ScalarValue.Annotation is not { } annotation)
        {
            return;
        }

        var (kind, property, value, minimum, maximum, unit) = annotation;
        writer.BlankLine();
        writer.OpenBrace();
        writer.WriteLine("var annotations = new Annotations(");
        writer.Indent();
        writer.WriteLine($"TypeKind.{kind},");
        writer.WriteLine($"defaultValue: new AnnotationParameterValue {{ {property} = {value} }},");
        writer.WriteLine($"minValue: {BuildAnnotationValue(property, minimum)},");
        writer.WriteLine($"maxValue: {BuildAnnotationValue(property, maximum)},");
        writer.WriteLine($"unit: {unit ?? "null"});");
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

    private static string GetDynamicElementType(CollectionAliasEmissionPlan plan, string? currentNamespace, bool recursiveElementType = false)
    {
        if (plan.IsString)
        {
            return plan.IsWideString
                ? plan.IsUnboundedString ? "dtf.CreateWideString()" : $"dtf.CreateWideString({plan.StringBound})"
                : plan.IsUnboundedString ? "dtf.CreateString()" : $"dtf.CreateString({plan.StringBound})";
        }

        if (plan.IsPrimitive)
        {
            return $"dtf.GetPrimitiveType<{plan.PrimitiveDynamicType}>()";
        }

        var elementPlan = plan.IsEnum
            ? EmissionTypeProjector.UnwrapValueEmissionType(plan.ElementPlan)
            : plan.ElementPlan;
        var supportType = elementPlan.SupportType ?? elementPlan.CSharpType;
        var supportInstance = recursiveElementType
            ? $"{IdlNaming.GeneratedSupportTypeReference(supportType, currentNamespace)}Support.GetOrCreateInstanceImpl()"
            : $"{IdlNaming.GeneratedSupportTypeReference(supportType, currentNamespace)}Support.Instance";
        return $"{supportInstance}.GetDynamicTypeInternal(isPublic)";
    }
}
