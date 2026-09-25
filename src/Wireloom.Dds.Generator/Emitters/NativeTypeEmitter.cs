namespace Wireloom;

/// <summary>Emits native storage and lifecycle/conversion methods for a data type.</summary>
internal sealed class NativeTypeEmitter
{
    public static void Emit(
        GeneratedSourceWriter writer,
        string typeName,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        string? implementationNamespace,
        string? baseUnmanagedType)
    {
        EmitFields(writer, fields, implementationNamespace);
        EmitDestroy(writer, fields, implementationNamespace, baseUnmanagedType);
        EmitFromNative(writer, typeName, fields, implementationNamespace, baseUnmanagedType, inheritedHasKeys: inheritedFields.Any(field => field.IsKey));
        EmitInitialize(writer, fields, implementationNamespace, baseUnmanagedType);
        EmitToNative(writer, typeName, fields, implementationNamespace, baseUnmanagedType, inheritedHasKeys: inheritedFields.Any(field => field.IsKey));
    }
    private static void EmitFields(GeneratedSourceWriter writer, IReadOnlyList<MemberEmissionPlan> fields, string? implementationNamespace)
    {
        foreach (var field in fields)
        {
            writer.WriteLine($"private {field.NativeStorageTypeFor(implementationNamespace)} {field.EscapedName};");
        }
    }

    private static void EmitDestroy(GeneratedSourceWriter writer, IReadOnlyList<MemberEmissionPlan> fields, string? currentNamespace, string? baseUnmanagedType)
    {
        writer.BlankLine();

        writer.WriteXmlSummary("Releases native resources held by this instance.");
        writer.WriteXmlParam("optionalsOnly", "Indicates whether only optional members should be released.");
        writer.OpenBlock("public void Destroy(bool optionalsOnly)");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine("parent.Destroy(optionalsOnly);");
        }

        var destroyableFields = fields .Where(f => f.DestroyKind != NativeDestroyKind.None).ToArray();
        var optionalFields = destroyableFields.Where(f => f.IsOptional).ToArray();
        var requiredFields = fields
            .Where(f => !f.IsOptional && f.DestroyKind == NativeDestroyKind.Nested)
            .Concat(fields.Where(f => !f.IsOptional && f.DestroyKind == NativeDestroyKind.Collection))
            .Concat(fields.Where(f => !f.IsOptional && f.DestroyKind == NativeDestroyKind.String))
            .Concat(fields.Where(f => !f.IsOptional && f.DestroyKind == NativeDestroyKind.OptionalPrimitive))
            .ToArray();

        foreach (var field in optionalFields)
        {
            writer.WriteLine(field.BuildDestroyStatement(currentNamespace)!);
        }

        if (requiredFields.Length > 0)
        {
            if (optionalFields.Length > 0 || baseUnmanagedType is not null)
            {
                writer.BlankLine();
            }

            writer.OpenBlock("if (optionalsOnly)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();

            foreach (var field in requiredFields)
            {
                writer.WriteLine(field.BuildDestroyStatement(currentNamespace)!);
            }
        }

        writer.CloseBlock();
    }

    private static void EmitFromNative(
        GeneratedSourceWriter writer,
        string typeName,
        IReadOnlyList<MemberEmissionPlan> fields,
        string? currentNamespace,
        string? baseUnmanagedType,
        bool inheritedHasKeys)
    {
        writer.BlankLine();

        writer.WriteXmlSummary("Copies native values into a managed DDS sample.");
        writer.WriteXmlParam("sample", "The managed sample to populate.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void FromNative({typeName} sample, bool keysOnly = false)");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine("parent.FromNative(sample, keysOnly);");
        }

        var hasKeys = inheritedHasKeys || fields.Any(field => field.IsKey);

        foreach (var field in hasKeys ? fields.Where(field => field.IsKey) : fields)
        {
            writer.WriteLine(field.BuildFromNativeStatement(field.IsKey && hasKeys, currentNamespace));
        }

        if (hasKeys && fields.Any(field => !field.IsKey))
        {
            writer.BlankLine();
            writer.OpenBlock("if (keysOnly)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();

            foreach (var field in fields.Where(field => !field.IsKey))
            {
                writer.WriteLine(field.BuildFromNativeStatement(false, currentNamespace));
            }
        }

        writer.CloseBlock();
    }

    private static void EmitInitialize(GeneratedSourceWriter writer, IReadOnlyList<MemberEmissionPlan> fields, string? currentNamespace, string? baseUnmanagedType)
    {
        writer.BlankLine();

        writer.WriteXmlSummary("Initializes this native representation to its IDL default values.");
        writer.WriteXmlParam("allocatePointers", "Whether pointer members should be allocated.");
        writer.WriteXmlParam("allocateMemory", "Whether native memory should be allocated.");
        writer.OpenBlock("public void Initialize(bool allocatePointers = true, bool allocateMemory = true)");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine("parent.Initialize(allocatePointers, allocateMemory);");
        }

        foreach (var field in fields)
        {
            var statement = field.BuildInitializeStatement(currentNamespace);
            if (statement is not null)
            {
                writer.WriteLine(statement);
            }
        }

        writer.CloseBlock();
    }

    private static void EmitToNative(
        GeneratedSourceWriter writer,
        string typeName,
        IReadOnlyList<MemberEmissionPlan> fields,
        string? currentNamespace,
        string? baseUnmanagedType,
        bool inheritedHasKeys)
    {
        writer.BlankLine();

        writer.WriteXmlSummary("Copies a managed DDS sample into this native representation.");
        writer.WriteXmlParam("sample", "The managed sample to copy.");
        writer.WriteXmlParam("keysOnly", "Whether to copy only key members.");
        writer.OpenBlock($"public void ToNative({typeName} sample, bool keysOnly = false)");

        if (baseUnmanagedType is not null)
        {
            writer.WriteLine("parent.ToNative(sample, keysOnly);");
        }

        var hasKeys = inheritedHasKeys || fields.Any(field => field.IsKey);

        foreach (var field in hasKeys ? fields.Where(field => field.IsKey) : fields)
        {
            writer.WriteLine(field.BuildToNativeStatement(field.IsKey && hasKeys, currentNamespace));
        }

        if (hasKeys && fields.Any(field => !field.IsKey))
        {
            writer.BlankLine();
            writer.OpenBlock("if (keysOnly)");
            writer.WriteLine("return;");
            writer.CloseBlock();
            writer.BlankLine();

            foreach (var field in fields.Where(field => !field.IsKey))
            {
                writer.WriteLine(field.BuildToNativeStatement(false, currentNamespace));
            }
        }

        writer.CloseBlock();
    }
}
