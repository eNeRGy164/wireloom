using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the public managed representation of one IDL data type.</summary>
internal static class ManagedDataTypeEmitter
{
    public static void Emit(GeneratedSourceWriter writer, string typeName, string? currentNamespace, IReadOnlyList<MemberEmissionPlan> fields, IReadOnlyList<MemberEmissionPlan> inheritedFields, string? baseType)
    {
        EmitDataMembers(writer, typeName, fields);
        EmitDefaultConstructor(writer, typeName, fields);

        if (fields.Count > 0 || inheritedFields.Count > 0)
        {
            EmitValueConstructor(writer, typeName, fields, inheritedFields, baseType is null ? null : IdlNaming.TypeReference(baseType, currentNamespace));
        }

        EmitCopyConstructor(writer, typeName, fields, baseType is null ? null : IdlNaming.TypeReference(baseType, currentNamespace));
        EmitHashCode(writer, fields, baseType is not null);
        EmitEquality(writer, typeName, fields, baseType is not null);
    }

    private static void EmitDataMembers(GeneratedSourceWriter writer, string typeName, IReadOnlyList<MemberEmissionPlan> fields)
    {
        var rangedFields = fields.Where(field => field.HasManagedRange).ToArray();
        foreach (var field in rangedFields)
        {
            writer.WriteLine($"private {IdlNaming.TypeReference(field.CSharpType, field.CurrentNamespace)} {field.ManagedBackingFieldName};");
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
                propertySummary += " A DDS reader that does not understand this member cannot safely read the sample.";
            }

            if (field.IsArray)
            {
                propertySummary += $" It is a fixed array with dimensions {string.Join(" × ", field.Dimensions)}.";
            }

            if (field.IsOptional && (field.IsArray || field.IsSequence))
            {
                propertySummary += " A null value means absent; an empty collection is present with no elements.";
            }
            else if (field.IsOptional)
            {
                propertySummary += " A null value means the member is absent.";
            }

            if (field.MemberId is int memberId)
            {
                propertySummary += $" Its DDS member ID is <c>{memberId}</c>, which identifies this member for type compatibility; it is separate from the DDS instance key.";
            }

            writer.WriteXmlSummary(propertySummary);

            if (field.HasManagedRange)
            {
                if (field is { MinimumValue: { } minimum, MaximumValue: { } maximum })
                {
                    writer.WriteXmlException("global::System.ArgumentOutOfRangeException", $"The assigned value is outside the inclusive range {minimum} through {maximum}.");
                }
                else if (field.MinimumValue is { } minimumOnly)
                {
                    writer.WriteXmlException("global::System.ArgumentOutOfRangeException", $"The assigned value is less than {minimumOnly}.");
                }
                else if (field.MaximumValue is { } maximumOnly)
                {
                    writer.WriteXmlException("global::System.ArgumentOutOfRangeException", $"The assigned value is greater than {maximumOnly}.");
                }
            }

            if (field.IsString)
            {
                writer.WriteXmlSeeAlso("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/manuals/connext_dds_professional/users_manual/users_manual/Strings_and_Wide_Strings.htm", "RTI Connext 7.7.0 string and wide-string bounds");
            }

            if (field.IsSequence)
            {
                writer.WriteXmlRemarks("The property exposes a mutable sequence. Add or remove elements through the sequence instance; the generated property does not cap mutations at the DDS bound. For an unbounded IDL sequence, Wireloom currently generates an effective limit of 100 elements. RTI uses the bound from the type metadata when processing DDS data.");
                writer.WriteXmlSeeAlso("https://community.rti.com/static/documentation/connext-dds/7.7.0/doc/api/connext_dds/api_csharp/namespaceOmg_1_1Types.html", "RTI Connext 7.7.0 ISequence API");
                var sequenceExample = field.IsOptional
                    ? $"var sample = new {typeName}();\nsample.{field.EscapedName} = new Sequence<{IdlNaming.TypeReference(field.ElementCSharpType!, field.CurrentNamespace)}>();\nsample.{field.EscapedName}.Add(default!);\nsample.{field.EscapedName}.RemoveAt(sample.{field.EscapedName}.Count - 1);"
                    : $"var sample = new {typeName}();\nsample.{field.EscapedName}.Add(default!);\nsample.{field.EscapedName}.RemoveAt(sample.{field.EscapedName}.Count - 1);";
                writer.WriteXmlExample(sequenceExample);
            }

            if (field.IsString)
            {
                var stringKind = field.IsWideString ? "wide" : "narrow";
                var unit = MemberEmissionRenderer.StringBoundUnit(field.IsWideString);
                var remark = !field.IsBoundedString
                    ? $"This unbounded {stringKind} IDL string has an effective limit of 255 {unit}. The generated C# property does not enforce the effective limit when assigned."
                    : $"The bound on this {stringKind} IDL string is measured in {unit}. The generated C# property does not check the bound when assigned.";
                writer.WriteXmlRemarks(remark);
            }

            if (field.IsOptional)
            {
                writer.WriteXmlExample($"var sample = new {typeName}();\nsample.{field.EscapedName} = null; // the member is absent");
            }

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
                writer.WriteLine($"public {IdlNaming.TypeReference(field.CSharpType, field.CurrentNamespace)} {IdlNaming.EscapeIdentifier(field.Name)}{field.ManagedPropertyAccessors}{field.ManagedPropertyInitializer}");
            }
        }
    }

    private static void EmitRangedProperty(GeneratedSourceWriter writer, MemberEmissionPlan field)
    {
        var type = IdlNaming.TypeReference(field.CSharpType, field.CurrentNamespace);
        var name = IdlNaming.EscapeIdentifier(field.Name);
        var backingField = field.ManagedBackingFieldName;

        writer.OpenBlock($"public {type} {name}");
        writer.OpenBlock("get");
        writer.WriteLine($"return {backingField};");
        writer.CloseBlock();
        writer.OpenBlock("set");

        if (field.MinimumValue is { } minimum)
        {
            var minimumText = field.FormatCSharpValue(field.CSharpType.TrimEnd('?'), minimum);
            writer.WriteLine($"global::System.ArgumentOutOfRangeException.ThrowIfLessThan(value, {minimumText});");
        }

        if (field.MaximumValue is { } maximum)
        {
            var maximumText = field.FormatCSharpValue(field.CSharpType.TrimEnd('?'), maximum);
            writer.WriteLine($"global::System.ArgumentOutOfRangeException.ThrowIfGreaterThan(value, {maximumText});");
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
        var initializationDetails = new List<string>();
        if (fields.Any(field => field.IsSequence && !field.IsOptional))
        {
            initializationDetails.Add("Non-optional sequences start empty");
        }

        if (fields.Any(field => field.IsArray && !field.IsOptional))
        {
            initializationDetails.Add("fixed arrays are allocated at their declared dimensions");
        }

        if (fields.Any(field => field.IsAggregate && !field.IsOptional))
        {
            initializationDetails.Add("nested aggregate members start as new instances");
        }

        if (fields.Any(field => (field.IsArray || field.IsSequence) && field.IsOptional))
        {
            initializationDetails.Add("optional collections start null (absent)");
        }

        if (fields.Any(field => field.HasExplicitDefault))
        {
            initializationDetails.Add("explicit IDL defaults are applied");
        }

        if (initializationDetails.Count > 0)
        {
            writer.WriteXmlRemarks($"{string.Join(", ", initializationDetails)}.");
        }
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

            field.EmitAggregateArrayInitialization(writer, ManagedFieldPrefix(field, "dimension") + field.EscapedName, index < arrayFields.Length - 1);
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
            writer.WriteXmlParam(IdlNaming.EscapeIdentifier(field.Name), $"The value for the <c>{field.Name}</c> member.");
        }

        if (fields.Concat(inheritedFields).Any(field => field.IsSequence || field.IsArray || field.IsAggregate))
        {
            writer.WriteXmlRemarks("The constructor stores supplied reference-type member values as provided. It does not clone their arrays, sequences, or nested objects.");
        }

        if (fields.Concat(inheritedFields).Any(field => field.HasManagedRange))
        {
            writer.WriteXmlException("global::System.ArgumentOutOfRangeException", "A supplied member value is outside its declared range.");
        }

        var parameters = inheritedFields.Concat(fields).Select(field => $"{IdlNaming.TypeReference(field.CSharpType, field.CurrentNamespace)} {IdlNaming.EscapeIdentifier(field.Name)}");
        var constructor = $"public {typeName}({string.Join(", ", parameters)})";

        if (baseReference is not null)
        {
            constructor += $" : base({string.Join(", ", inheritedFields.Select(field => IdlNaming.EscapeIdentifier(field.Name)))})";
        }

        writer.OpenBlock(constructor);

        foreach (var field in fields)
        {
            writer.WriteLine($"this.{IdlNaming.EscapeIdentifier(field.Name)} = {IdlNaming.EscapeIdentifier(field.Name)};");
        }

        writer.CloseBlock();
    }

    private static void EmitCopyConstructor(GeneratedSourceWriter writer, string typeName, IReadOnlyList<MemberEmissionPlan> fields, string? baseReference)
    {
        writer.BlankLine();

        writer.WriteXmlSummary($"Initializes a copy of the specified <see cref=\"{typeName}\"/> instance.");
        writer.WriteXmlParam("other", "The instance to copy, or <see langword=\"null\"/>.");
        writer.WriteXmlRemarks("Arrays and sequences are copied into new containers, and nested aggregate members are copied through their generated copy constructors. When <paramref name=\"other\"/> is null, the constructor returns without copying; property initializers remain in effect, but values created only by the parameterless constructor are not initialized.");
        writer.WriteXmlExample($"var original = new {typeName}();\nvar copy = new {typeName}(original);");
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
                var targetPrefix = ManagedFieldPrefix(field, "other");
                var arrayTarget = ManagedFieldPrefix(field, "dimension", "other") + field.EscapedName;
                writer.WriteLine($"{targetPrefix}{target} = {source};");
                field.EmitAggregateArrayCopy(writer, arrayTarget, index < fields.Count - 1);
            }
        }

        writer.CloseBlock();
    }

    private static void EmitHashCode(GeneratedSourceWriter writer, IReadOnlyList<MemberEmissionPlan> fields, bool hasBase)
    {
        writer.BlankLine();

        writer.WriteXmlInheritdoc();
        writer.OpenBlock("public override int GetHashCode()");
        writer.WriteLine("var hash = new global::System.HashCode();");
        writer.BlankLine();

        if (hasBase)
        {
            writer.WriteLine("hash.Add(base.GetHashCode());");
        }

        foreach (var field in fields)
        {
            var fieldPrefix = field.Name == "hash" ? "this." : string.Empty;
            writer.WriteLine($"hash.Add({field.HashValue(fieldPrefix)});");
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
        writer.OpenBlock("if (global::System.Object.ReferenceEquals(this, other))");
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

                var field = fields[index];
                var thisPrefix = ManagedFieldPrefix(field, "other");
                WriteEqualityExpression(writer, prefix, field.EqualityExpression(thisPrefix: thisPrefix), suffix);
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

    private static string ManagedFieldPrefix(MemberEmissionPlan field, params string[] shadowedNames) =>
        shadowedNames.Contains(field.Name, StringComparer.Ordinal) || field.IsArrayLoopLocalCollision ? "this." : string.Empty;
}
