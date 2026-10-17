using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Coordinates emission of collection and value typedef documents.</summary>
internal static class CollectionAliasEmitter
{
    /// <summary>Emits all documents for a collection or value typedef.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(CompilationContext compilation, IdlTypedef declaration, string sourceIdlFileName, RtiApiProfile profile)
    {
        var plan = CreatePlan(compilation, declaration, profile);
        var names = IdlNaming.CreateGeneratedTypeNames(declaration.Namespace, declaration.Name);

        return [
            .. CollectionAliasManagedEmitter.Emit(declaration, names, plan, sourceIdlFileName, profile),
            .. CollectionAliasPluginEmitter.Emit(declaration, names, plan, sourceIdlFileName, profile),
            .. CollectionAliasNativeEmitter.Emit(declaration, names, plan, sourceIdlFileName),
            .. CollectionAliasTypeSupportEmitter.Emit(declaration, names, plan, sourceIdlFileName)
        ];
    }

    private static CollectionAliasEmissionPlan CreatePlan(CompilationContext compilation, IdlTypedef declaration, RtiApiProfile profile)
    {
        string? element;
        if (declaration.IsString)
        {
            element = "string";
        }
        else
        {
            if (declaration.IsCollection)
            {
                element = declaration.ElementType!;
            }
            else
            {
                element = declaration.Target;
            }
        }


        IdlType? resolvedElementType = null;
        EmissionTypePlan elementPlan;
        if (declaration.IsString)
        {
            elementPlan = new StringEmissionType(declaration.IsWideString, declaration.StringBound, declaration.IsStringBounded);
        }
        else
        {
            if (!declaration.IsCollection)
            {
                resolvedElementType = compilation.ResolveType(element, declaration.Namespace);
                elementPlan = EmissionTypeProjector.ToTypedefValueType(resolvedElementType, declaration.Namespace);

                if (elementPlan.Shape.IsString)
                {
                    elementPlan = EmissionTypeProjector.UnwrapValueEmissionType(elementPlan);
                }
                else
                {
                    element = compilation.ResolveUnderlyingType(element, declaration.Namespace);
                    resolvedElementType = compilation.ResolveType(element, declaration.Namespace);
                    elementPlan = EmissionTypeProjector.ToTypedefValueType(resolvedElementType, declaration.Namespace);
                }
            }
            else
            {
                resolvedElementType = compilation.ResolveType(element, declaration.Namespace);
                elementPlan = EmissionTypeProjector.ToCollectionElementType(resolvedElementType, declaration.Namespace);
            }
        }

        string? elementNativeType = null;
        if (resolvedElementType is not null)
        {
            var nativeElementType = resolvedElementType;
            if (elementPlan is not AliasEmissionType aliasPlan
                || string.Equals(aliasPlan.CSharpType, aliasPlan.Target.CSharpType, StringComparison.Ordinal))
            {
                while (nativeElementType is IdlType.Alias nativeAlias)
                {
                    nativeElementType = nativeAlias.Target;
                }
            }

            if (nativeElementType is not IdlType.StringType)
            {
                elementNativeType = compilation.ResolveNativeType(nativeElementType, declaration.Namespace);
            }
        }

        return new CollectionAliasEmissionPlan(
            elementPlan,
            declaration.IsSequence,
            declaration.IsArray,
            RequiresNativeValueCast(elementPlan),
            elementNativeType,
            sequenceBound: declaration.Bound,
            profile: profile);
    }

    private static bool RequiresNativeValueCast(EmissionTypePlan elementPlan)
    {
        var valueType = EmissionTypeProjector.UnwrapValueEmissionType(elementPlan);
        if (valueType is PrimitiveEmissionType primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.IdlName).NativeValueRequiresCast;
        }

        return true;
    }

}
