namespace Wireloom;

using static IdlCompiler;

/// <summary>Emits the public managed representation of one IDL data type.</summary>
internal sealed class ManagedDataTypeEmitter
{
    public static void Emit(GeneratedSourceWriter writer, string typeName, string? currentNamespace, IReadOnlyList<MemberEmissionPlan> fields, IReadOnlyList<MemberEmissionPlan> inheritedFields, string? baseType)
    {
        EmitDataMembers(writer, fields);
        EmitDefaultConstructor(writer, typeName, fields);

        if (fields.Count > 0 || inheritedFields.Count > 0)
        {
            EmitValueConstructor(writer, typeName, fields, inheritedFields, baseType is null ? null : TypeReference(baseType, currentNamespace));
        }

        EmitCopyConstructor(writer, typeName, fields, baseType is null ? null : TypeReference(baseType, currentNamespace));
        EmitHashCode(writer, fields, baseType is not null);
        EmitEquality(writer, typeName, fields, baseType is not null);
    }

    private static void EmitDataMembers(GeneratedSourceWriter writer, IReadOnlyList<MemberEmissionPlan> fields)
    {
        for (var index = 0; index < fields.Count; index++)
        {
            if (index > 0)
            {
                writer.BlankLine();
            }

            var field = fields[index];
            var propertySummary = $"{field.ManagedPropertySummaryVerb} the <c>{field.Name}</c> member.";

            if (field.IsKey)
            {
                propertySummary += " This member forms part of the DDS instance key.";
            }

            if (field.IsOptional)
            {
                propertySummary += " This member is optional.";
            }

            if (field.BoundSummary is string boundSummary)
            {
                propertySummary += $" {boundSummary}";
            }

            writer.WriteXmlSummary(propertySummary);

            if (field.IsKey)
            {
                writer.WriteLine("[Key]");
            }

            if (field.IsOptional)
            {
                writer.WriteLine("[Optional]");
            }

            if (field.Bound is int bound)
            {
                writer.WriteLine($"[Bound({bound})]");
            }

            writer.WriteLine($"public {TypeReference(field.CSharpType, field.CurrentNamespace)} {EscapeIdentifier(field.Name)}{field.ManagedPropertyAccessors}{field.ManagedPropertyInitializer}");
        }
    }

    private static void EmitDefaultConstructor(GeneratedSourceWriter writer, string typeName, IReadOnlyList<MemberEmissionPlan> fields)
    {
        writer.BlankLine();

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{typeName}\"/> class.");
        writer.OpenBlock($"public {typeName}()");

        var sequenceFields = fields.Where(field => field.ManagedInitialization == ManagedInitializationKind.Sequence).ToArray();
        var arrayFields = fields.Where(field => field.ManagedInitialization == ManagedInitializationKind.Array).ToArray();

        foreach (var field in sequenceFields)
        {
            writer.WriteLine(field.ManagedDefaultInitializationStatement!);
        }

        for (var index = 0; index < arrayFields.Length; index++)
        {
            var field = arrayFields[index];

            writer.WriteLine(field.ManagedDefaultInitializationStatement!);

            field.EmitAggregateArrayInitialization(writer, field.EscapedName, index < arrayFields.Length - 1);
        }

        writer.CloseBlock();
    }

    private static void EmitValueConstructor(
        GeneratedSourceWriter writer,
        string typeName,
        IReadOnlyList<MemberEmissionPlan> fields,
        IReadOnlyList<MemberEmissionPlan> inheritedFields,
        string? baseReference)
    {
        writer.BlankLine();

        writer.WriteXmlSummary($"Initializes a new instance of the <see cref=\"{typeName}\"/> class with the supplied member values.");

        foreach (var field in inheritedFields.Concat(fields))
        {
            writer.WriteXmlParam(EscapeIdentifier(field.Name), $"The value for the <c>{field.Name}</c> member.");
        }

        var parameters = inheritedFields.Concat(fields).Select(field => $"{TypeReference(field.CSharpType, field.CurrentNamespace)} {EscapeIdentifier(field.Name)}");
        var constructor = $"public {typeName}({string.Join(", ", parameters)})";

        if (baseReference is not null)
        {
            constructor += $" : base({string.Join(", ", inheritedFields.Select(field => EscapeIdentifier(field.Name)))})";
        }

        writer.OpenBlock(constructor);

        foreach (var field in fields)
        {
            writer.WriteLine($"this.{EscapeIdentifier(field.Name)} = {EscapeIdentifier(field.Name)};");
        }

        writer.CloseBlock();
    }

    private static void EmitCopyConstructor(GeneratedSourceWriter writer, string typeName, IReadOnlyList<MemberEmissionPlan> fields, string? baseReference)
    {
        writer.BlankLine();

        writer.WriteXmlSummary($"Initializes a copy of the specified <see cref=\"{typeName}\"/> instance.");
        writer.WriteXmlParam("other", "The instance to copy, or <see langword=\"null\"/>.");
        writer.OpenBlock($"public {typeName}({typeName}? other){(baseReference is null ? string.Empty : " : base(other)")}");
        writer.OpenBlock("if (other is null)");
        writer.WriteLine("return;");
        writer.CloseBlock();

        if (fields.Count > 0)
        {
            writer.BlankLine();

            for (var index = 0; index < fields.Count; index++)
            {
                var field = fields[index];
                writer.WriteLine($"{field.EscapedName} = {field.BuildCopyExpression()};");
                field.EmitAggregateArrayCopy(writer, field.EscapedName, index < fields.Count - 1);
            }
        }

        writer.CloseBlock();
    }

    private static void EmitHashCode(GeneratedSourceWriter writer, IReadOnlyList<MemberEmissionPlan> fields, bool hasBase)
    {
        writer.BlankLine();

        writer.WriteXmlInheritdoc();
        writer.OpenBlock("public override int GetHashCode()");
        writer.WriteLine("var hash = new HashCode();");
        writer.BlankLine();

        if (hasBase)
        {
            writer.WriteLine("hash.Add(base.GetHashCode());");
        }

        foreach (var field in fields)
        {
            writer.WriteLine($"hash.Add({field.HashValue()});");
        }

        writer.BlankLine();
        writer.WriteLine("return hash.ToHashCode();");
        writer.CloseBlock();
    }

    private static void EmitEquality(GeneratedSourceWriter writer, string typeName,
        IReadOnlyList<MemberEmissionPlan> fields, bool hasBase)
    {
        writer.BlankLine();

        writer.WriteXmlSummary("Determines whether this instance and <paramref name=\"other\"/> have identical member values.");
        writer.WriteXmlParam("other", "The instance to compare, or <see langword=\"null\"/>.");
        writer.WriteXmlReturns("<see langword=\"true\"/> when the member values are equal; otherwise, <see langword=\"false\"/>.");
        writer.OpenBlock($"public bool Equals({typeName}? other)");
        writer.OpenBlock("if (other is null)");
        writer.WriteLine("return false;");
        writer.CloseBlock();
        writer.BlankLine();
        writer.OpenBlock("if (ReferenceEquals(this, other))");
        writer.WriteLine("return true;");
        writer.CloseBlock();
        writer.BlankLine();

        if (hasBase)
        {
            writer.OpenBlock("if (!base.Equals(other))");
            writer.WriteLine("return false;");
            writer.CloseBlock();
            writer.BlankLine();
        }

        if (fields.Count == 0)
        {
            writer.WriteLine("return true;");
        }
        else
        {
            for (var index = 0; index < fields.Count; index++)
            {
                var prefix = index == 0 ? "return " : "&& ";
                var suffix = index == fields.Count - 1 ? ";" : string.Empty;

                if (index == 1)
                {
                    writer.Indent();
                }

                writer.WriteLine($"{prefix}{fields[index].EqualityExpression()}{suffix}");
            }

            if (fields.Count > 1)
            {
                writer.Unindent();
            }
        }

        writer.CloseBlock();
        writer.BlankLine();

        writer.WriteXmlInheritdoc();
        writer.WriteLine($"public override bool Equals(object? obj) => Equals(obj as {typeName});");
    }
}
