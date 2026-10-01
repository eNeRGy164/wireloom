using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Coordinates emission of collection and value typedef documents.</summary>
internal static class CollectionAliasEmitter
{
    public static void Emit(CompilationContext compilation, EmissionResult result, IdlTypedef declaration, string sourceIdlFileName)
    {
        var plan = CreatePlan(compilation, declaration);

        CollectionAliasManagedEmitter.Emit(result, declaration, plan, sourceIdlFileName);
        CollectionAliasPluginEmitter.Emit(compilation, result, declaration, plan, sourceIdlFileName);
        CollectionAliasNativeEmitter.Emit(compilation, result, declaration, plan, sourceIdlFileName);
        CollectionAliasTypeSupportEmitter.Emit(result, declaration, sourceIdlFileName);
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

        var elementIdlType = element;

        if (declaration is { IsString: true } || IsStringType(element))
        {
            element = "string";
        }

        var resolvedElement = IdlNaming.IsPrimitive(element)
            ? IdlNaming.MapPrimitive(element)
            : IdlNaming.EscapeQualifiedIdentifier(IdlNaming.ResolveTypeName(element, declaration.Namespace));

        var isString = declaration.IsString || IsStringType(elementIdlType);
        var isPrimitive = IdlNaming.IsPrimitive(element);
        var isEnum = compilation.IsEnum(element, declaration.Namespace);

        return new CollectionAliasEmissionPlan(
            resolvedElement,
            elementIdlType,
            isString,
            isPrimitive,
            isEnum,
            !isString && !isPrimitive && !isEnum,
            declaration is { IsCollection: false, IsString: false } && compilation.IsUnion(element, declaration.Namespace),
            !IdlNaming.IsPrimitive(resolvedElement));
    }

    private static bool IsStringType(string? typeName) =>
        typeName is not null
        && (typeName.StartsWith("string", StringComparison.Ordinal) || typeName.StartsWith("wstring", StringComparison.Ordinal));

}
