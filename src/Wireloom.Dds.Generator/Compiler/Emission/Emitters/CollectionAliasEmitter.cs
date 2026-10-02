using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Coordinates emission of collection and value typedef documents.</summary>
internal static class CollectionAliasEmitter
{
    public static void Emit(CompilationContext compilation, EmissionResult result, IdlTypedef declaration, string sourceIdlFileName)
    {
        var plan = CreatePlan(compilation, declaration);
        var names = IdlNaming.CreateGeneratedTypeNames(declaration.Namespace, declaration.Name);

        CollectionAliasManagedEmitter.Emit(result, declaration, names, plan, sourceIdlFileName);
        CollectionAliasPluginEmitter.Emit(compilation, result, declaration, names, plan, sourceIdlFileName);
        CollectionAliasNativeEmitter.Emit(compilation, result, declaration, names, plan, sourceIdlFileName);
        CollectionAliasTypeSupportEmitter.Emit(result, declaration, names, sourceIdlFileName);
    }

    private static CollectionAliasEmissionPlan CreatePlan(CompilationContext compilation, IdlTypedef declaration)
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


        if (declaration is { IsCollection: false, IsString: false })
        {
            element = compilation.ResolveUnderlyingType(element, declaration.Namespace);
        }

        IdlType? resolvedElementType = null;
        EmissionTypePlan elementPlan;
        if (declaration.IsString)
        {
            elementPlan = new StringEmissionType(declaration.IsWideString, declaration.StringBound);
        }
        else
        {
            resolvedElementType = compilation.ResolveType(element, declaration.Namespace);
            elementPlan = EmissionTypeProjector.ToCollectionElementType(resolvedElementType, declaration.Namespace);
        }

        string? elementNativeType = null;
        if (resolvedElementType is not null and not IdlType.StringType)
        {
            elementNativeType = compilation.ResolveNativeType(resolvedElementType, declaration.Namespace);
        }

        return new CollectionAliasEmissionPlan(
            elementPlan,
            declaration.IsSequence,
            declaration.IsArray,
            RequiresNativeValueCast(elementPlan),
            elementNativeType);
    }

    private static bool RequiresNativeValueCast(EmissionTypePlan elementPlan)
    {
        if (elementPlan is PrimitiveEmissionType primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.IdlName).NativeValueRequiresCast;
        }

        return true;
    }

}
