using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the RTI plugin and dynamic type for a collection or value typedef.</summary>
internal static class CollectionAliasPluginEmitter
{
    public static void Emit(CompilationContext compilation, IdlTypedef declaration, string elementType, string sourceIdlFileName)
    {
        var typeName = IdlNaming.EscapeIdentifier(declaration.Name);
        var implementation = declaration.Namespace is null ? "Implementation" : $"{declaration.Namespace}.Implementation";
        var implementationTypeName = IdlNaming.TypeReference(typeName, declaration.Namespace, implementation);
        var elementIdlType = declaration.IsCollection ? declaration.ElementType! : elementType;
        var isString = declaration.IsString;
        var collectionElementIsAggregate = declaration.IsCollection &&
            !IsStringType(elementIdlType) &&
            !IdlNaming.IsPrimitive(elementType) &&
            !IsCSharpPrimitive(elementType) &&
            !compilation.IsEnum(elementType, declaration.Namespace);

        var collectionElementUnmanagedType = string.Empty;
        if (collectionElementIsAggregate)
        {
            collectionElementUnmanagedType = IdlNaming.TypeReference(compilation.ResolveAliasNativeType(elementType, declaration.Namespace), implementation);
        }

        var writer = EmissionSupport.CreateSource(implementation, EmissionSupport.PluginUsings, sourceIdlFileName);

        writer.OpenBlock($"internal class {typeName}Plugin : InterpretedTypePlugin<{implementationTypeName}, {typeName}Unmanaged>");
        writer.OpenBlock($"internal {typeName}Plugin() : base(\"{(declaration.Namespace is null ? typeName : $"{IdlNaming.EscapeQualifiedIdentifier(declaration.Namespace)}.{typeName}")}\", isKeyed: false, CreateDynamicType(isPublic: false))");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteXmlSummary($"Creates the RTI dynamic type description for <see cref=\"{typeName}\"/>.");
        writer.WriteXmlParam("isPublic", "Whether the resulting dynamic type is publicly visible to RTI.");
        writer.WriteXmlReturns("The RTI dynamic type description.");
        writer.OpenBlock("public static DynamicType CreateDynamicType(bool isPublic = true)");
        writer.WriteLine("var dtf = ServiceEnvironment.Instance.Internal.GetTypeFactory(isPublic);");
        writer.WriteLine("var tsf = ServiceEnvironment.Instance.Internal.TypeSupportFactory;");
        writer.BlankLine();

        if (declaration.IsSequence)
        {
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", tsf.CreateSequenceWithAccessInfo(dtf, {GetDynamicElementType(elementIdlType, implementation)}, {declaration.Bound}));");
        }
        else if (declaration.IsArray)
        {
            writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", tsf.CreateArrayWithAccessInfo<{(collectionElementIsAggregate ? collectionElementUnmanagedType : IdlNaming.TypeReference(elementType, implementation))}>(dtf, {GetDynamicElementType(elementIdlType, implementation)}, new uint[] {{ {string.Join(", ", declaration.Dimensions)} }}));");
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
                dynamicType = $"dtf.GetPrimitiveType<{IdlNaming.TypeReference(elementType, implementation)}>()";
            }

            if (declaration.IsString || IdlNaming.IsPrimitive(declaration.Target))
            {
                writer.WriteLine($"var aliasType = tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", {dynamicType});");
            }
            else
            {
                writer.WriteLine($"return tsf.CreateAliasWithAccessInfo<{typeName}Unmanaged>(dtf, \"{typeName}\", {dynamicType});");
            }
        }

        if (!declaration.IsCollection && (declaration.IsString || IdlNaming.IsPrimitive(declaration.Target)))
        {
            EmitAliasAnnotations(writer, declaration);

            writer.BlankLine();
            writer.WriteLine("return aliasType;");
        }

        writer.CloseBlock();
        writer.CloseBlock();
        compilation.AddSource(new GeneratedIdlSource(IdlNaming.CreateHintName(implementation, declaration.Name + "Plugin"), writer.ToString()));
    }

    private static void EmitAliasAnnotations(GeneratedSourceWriter writer, IdlTypedef declaration)
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

        var annotation = declaration.Target switch
        {
            "short" or "int16" => ("Int16", "Int16Value", "(short)0", "short.MinValue", "short.MaxValue"),
            "long" or "int32" => ("Int32", "Int32Value", "0", "int.MinValue", "int.MaxValue"),
            "long long" or "int64" => ("Int64", "Int64Value", "0L", "long.MinValue", "long.MaxValue"),
            "unsigned short" or "uint16" => ("Uint16", "Uint16Value", "(ushort)0", "ushort.MinValue", "ushort.MaxValue"),
            "unsigned long" or "uint32" => ("UInt32", "Uint32Value", "0U", "uint.MinValue", "uint.MaxValue"),
            "unsigned long long" or "uint64" => ("UInt64", "Uint64Value", "0UL", "ulong.MinValue", "ulong.MaxValue"),
            "int8" => ("Int8", "Int8Value", "(sbyte)0", "sbyte.MinValue", "sbyte.MaxValue"),
            "uint8" => ("Uint8", "Uint8Value", "(byte)0", "byte.MinValue", "byte.MaxValue"),
            "octet" => ("Octet", "OctetValue", "(byte)0", "byte.MinValue", "byte.MaxValue"),
            "float" => ("Float32", "Float32Value", "0F", "float.MinValue", "float.MaxValue"),
            "double" => ("Float64", "Float64Value", "0D", "double.MinValue", "double.MaxValue"),
            "boolean" => ("Boolean", "BoolValue", "false", "null", "null"),
            "char" => ("Char8", "Char8Value", "'\\0'", "null", "null"),
            "wchar" => ("Char16", "Char16Value", "'\\0'", "null", "null"),
            _ => default
        };

        if (annotation == default)
        {
            return;
        }

        writer.BlankLine();
        writer.OpenBrace();
        writer.WriteLine("var annotations = new Annotations(");
        writer.Indent();
        writer.WriteLine($"TypeKind.{annotation.Item1},");
        writer.WriteLine($"defaultValue: new AnnotationParameterValue {{ {annotation.Item2} = {annotation.Item3} }},");
        writer.WriteLine($"minValue: {(annotation.Item4 == "null" ? "null" : $"new AnnotationParameterValue {{ {annotation.Item2} = {annotation.Item4} }}")},");
        writer.WriteLine($"maxValue: {(annotation.Item5 == "null" ? "null" : $"new AnnotationParameterValue {{ {annotation.Item2} = {annotation.Item5} }}")},");
        writer.WriteLine("unit: null);");
        writer.Unindent();
        writer.WriteLine("aliasType.SetAnnotations(annotations);");
        writer.CloseBlock();
    }

    private static string GetDynamicElementType(string typeName, string? currentNamespace)
    {
        if (IsStringType(typeName))
        {
            var isWide = typeName.StartsWith("wstring", StringComparison.Ordinal);
            var bound = ParseStringBound(typeName);

            return isWide
                ? $"dtf.CreateWideString({bound})"
                : $"dtf.CreateString({bound})";
        }

        if (IdlNaming.IsPrimitive(typeName))
        {
            return $"dtf.GetPrimitiveType<{IdlNaming.MapPrimitive(typeName)}>()";
        }

        if (IsCSharpPrimitive(typeName))
        {
            return $"dtf.GetPrimitiveType<{typeName}>()";
        }

        return $"{IdlNaming.TypeReference(IdlNaming.EscapeQualifiedIdentifier(IdlNaming.ResolveTypeName(typeName, null)), currentNamespace)}Support.Instance.GetDynamicTypeInternal(isPublic)";
    }

    /// <summary>Determines whether a type is already expressed as a C# primitive keyword.</summary>
    private static bool IsCSharpPrimitive(string typeName) => typeName is
        "sbyte" or "byte" or "short" or "ushort" or "int" or "uint" or "long" or
        "ulong" or "char" or "bool" or "float" or "double";

    private static bool IsStringType(string? typeName) =>
        typeName is not null
        && (typeName.StartsWith("string", StringComparison.Ordinal) || typeName.StartsWith("wstring", StringComparison.Ordinal));

    private static int ParseStringBound(string typeName)
    {
        var open = typeName.IndexOf('<');
        return open < 0 || !int.TryParse(typeName[(open + 1)..^1].Trim(), out var bound)
            ? 255
            : bound;
    }
}
