namespace Wireloom;

using static IdlCompiler;

/// <summary>Emits the public managed representation of one IDL data type.</summary>
internal static class ManagedDataTypeEmitter
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
        var rangedFields = fields.Where(field => field.HasManagedRange).ToArray();
        foreach (var field in rangedFields)
        {
            writer.WriteLine($"private {TypeReference(field.CSharpType, field.CurrentNamespace)} {field.ManagedBackingFieldName};");
        }

        if (rangedFields.Length > 0 && fields.Count > 0)
        {
            writer.BlankLine();
        }

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

            if (field.ValueConstraintSummary is string valueConstraintSummary)
            {
                propertySummary += $" {valueConstraintSummary}";
            }

            if (field.MemberIdHashSource is string memberIdHashSource)
            {
                var escapedHashSource = System.Security.SecurityElement.Escape(memberIdHashSource);
                var annotation = field.UsesAutoIdHash ? "<c>@autoid(HASH)</c>" : "<c>@hashid</c>";
                propertySummary += $" Its DDS member ID is generated from the hash of <c>{escapedHashSource}</c> through {annotation}.";
            }

            if (field.IsMustUnderstand)
            {
                propertySummary += " This member is marked as must-understand by DDS.";
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

            if (field.HasManagedRange)
            {
                EmitRangedProperty(writer, field);
            }
            else
            {
                writer.WriteLine($"public {TypeReference(field.CSharpType, field.CurrentNamespace)} {EscapeIdentifier(field.Name)}{field.ManagedPropertyAccessors}{field.ManagedPropertyInitializer}");
            }
        }
    }

    private static void EmitRangedProperty(GeneratedSourceWriter writer, MemberEmissionPlan field)
    {
        var type = TypeReference(field.CSharpType, field.CurrentNamespace);
        var name = EscapeIdentifier(field.Name);
        var backingField = field.ManagedBackingFieldName;

        writer.OpenBlock($"public {type} {name}");
        writer.OpenBlock("get");
        writer.WriteLine($"return {backingField};");
        writer.CloseBlock();
        writer.OpenBlock("set");

        if (field.MinimumValue is { } minimum)
        {
            var minimumText = field.FormatCSharpValue(field.CSharpType.TrimEnd('?'), minimum);
            writer.WriteLine($"ArgumentOutOfRangeException.ThrowIfLessThan(value, {minimumText});");
        }

        if (field.MaximumValue is { } maximum)
        {
            var maximumText = field.FormatCSharpValue(field.CSharpType.TrimEnd('?'), maximum);
            writer.WriteLine($"ArgumentOutOfRangeException.ThrowIfGreaterThan(value, {maximumText});");
        }

        if (field.MinimumValue is not null && field.MaximumValue is not null)
        {
            writer.BlankLine();
        }

        writer.WriteLine($"{backingField} = value;");
        writer.CloseBlock();
        writer.CloseBlock();
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

        foreach (var field in fields.Where(field => field.HasExplicitDefault))
        {
            writer.WriteLine(field.ManagedDefaultInitializationStatement!);
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
            var target = field.HasManagedRange ? field.ManagedBackingFieldName : EscapeIdentifier(field.Name);
            writer.WriteLine($"this.{target} = {EscapeIdentifier(field.Name)};");
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
                var target = field.HasManagedRange ? field.ManagedBackingFieldName : field.EscapedName;
                var source = field.HasManagedRange ? $"other.{field.ManagedBackingFieldName}" : field.BuildCopyExpression();
                writer.WriteLine($"{target} = {source};");
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

                WriteEqualityExpression(writer, prefix, fields[index].EqualityExpression(), suffix);
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

    private static void WriteEqualityExpression(GeneratedSourceWriter writer, string prefix, string expression, string suffix)
    {
        var terms = expression.Split([" && "], StringSplitOptions.None);
        writer.WriteLine($"{prefix}{terms[0]}{(terms.Length == 1 ? suffix : string.Empty)}");

        if (terms.Length == 1)
        {
            return;
        }

        writer.Indent();
        for (var index = 1; index < terms.Length; index++)
        {
            writer.WriteLine($"&& {terms[index]}{(index == terms.Length - 1 ? suffix : string.Empty)}");
        }

        writer.Unindent();
    }
}
