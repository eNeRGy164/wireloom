using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Coordinates emission of collection and value typedef documents.</summary>
internal static class CollectionAliasEmitter
{
    public static void Emit(CompilationContext compilation, IdlTypedef declaration, string sourceIdlFileName)
    {
        var elementType = ResolveElementType(compilation, declaration);

        CollectionAliasManagedEmitter.Emit(compilation, declaration, sourceIdlFileName);
        CollectionAliasPluginEmitter.Emit(compilation, declaration, elementType, sourceIdlFileName);
        CollectionAliasNativeEmitter.Emit(compilation, declaration, elementType, sourceIdlFileName);
        CollectionAliasTypeSupportEmitter.Emit(compilation, declaration, sourceIdlFileName);
    }

    private static string ResolveElementType(CompilationContext compilation, IdlTypedef declaration)
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

        if (declaration.IsString || IsStringType(element))
        {
            return "string";
        }

        return IdlNaming.IsPrimitive(element)
            ? IdlNaming.MapPrimitive(element)
            : IdlNaming.EscapeQualifiedIdentifier(IdlNaming.ResolveTypeName(element, declaration.Namespace));
    }

    private static bool IsStringType(string? typeName) =>
        typeName is not null
        && (typeName.StartsWith("string", StringComparison.Ordinal) || typeName.StartsWith("wstring", StringComparison.Ordinal));
}
