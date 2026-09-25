using static Wireloom.EmissionTypeProjector;
using static Wireloom.IdlCompiler;

namespace Wireloom;

/// <summary>Emits the managed data class and its RTI type-support documents.</summary>
internal sealed class ClassEmitter
{
    public static void Emit(
        CompilationState compilation,
        string name,
        string? currentNamespace,
        IReadOnlyList<IdlMember> fields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        IReadOnlyList<IdlMember> inheritedFields,
        bool isTopic)
    {
        var emissionFields = fields.Select(field => ToEmissionField(field, currentNamespace)).ToArray();
        var emissionInheritedFields = inheritedFields.Select(field => ToEmissionField(field, currentNamespace)).ToArray();
        var fieldPlans = emissionFields.Select(field => new MemberEmissionPlan(field, currentNamespace)).ToArray();
        var inheritedFieldPlans = emissionInheritedFields.Select(field => new MemberEmissionPlan(field, currentNamespace)).ToArray();
        var hasTypeSupport = fieldPlans.All(field => field.HasTypeSupport);
        var runtimeTypeName = currentNamespace is null ? name : $"{currentNamespace}.{name}";
        var isRecursive = fieldPlans.Any(field => field.IsRecursive(runtimeTypeName));
        var dataTypeUsings = fieldPlans.Any(field => field.IsSequence || field.IsArray) ? DataTypeUsings.Concat(["System.Linq"]) : DataTypeUsings;
        var writer = CreateSource(currentNamespace, dataTypeUsings, sourceIdlFileName);
        var escapedName = EscapeIdentifier(name);
        var typeSummary = $"Represents the <c>{name}</c> DDS type declared in <c>{sourceIdlFileName}</c>.";

        if (inheritedFieldPlans.Concat(fieldPlans).Any(field => field.IsKey))
        {
            typeSummary += " Its key members form the DDS instance key.";
        }

        if (baseType is not null)
        {
            typeSummary += $" It derives from <see cref=\"{TypeReference(baseType, currentNamespace)}\"/>.";
        }

        typeSummary += $" It is marked as <c>{extensibility.ToString().ToLowerInvariant()}</c>.";
        if (isTopic)
        {
            typeSummary += " It is marked as a DDS topic type.";
        }

        writer.WriteXmlSummary(typeSummary);
        var baseReference = baseType is null ? null : TypeReference(baseType, currentNamespace);
        writer.OpenBlock($"public partial class {escapedName} : {(baseReference is null ? "" : baseReference + ", ")}IEquatable<{escapedName}>");

        ManagedDataTypeEmitter.Emit(
            writer,
            escapedName,
            currentNamespace,
            fieldPlans,
            inheritedFieldPlans,
            baseType);

        if (hasTypeSupport)
        {
            writer.BlankLine();
            writer.WriteXmlSummary("Returns the RTI Connext DDS type-support representation of this sample.");
            writer.WriteXmlReturns("The RTI Connext DDS representation of this sample.");
            writer.WriteLine($"public override string ToString() => {escapedName}Support.Instance.ToString(this);");
        }

        writer.CloseBlock();
        compilation.AddSource(new GeneratedIdlSource(CreateHintName(currentNamespace, name), writer.ToString()));

        if (hasTypeSupport)
        {
            TypeSupportEmitter.Emit(
                compilation,
                name,
                currentNamespace,
                fieldPlans,
                inheritedFieldPlans,
                extensibility,
                sourceIdlFileName,
                baseType,
                isRecursive);
        }
    }

}
