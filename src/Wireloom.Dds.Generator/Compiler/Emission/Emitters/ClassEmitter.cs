using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the managed data class and its RTI type-support documents.</summary>
internal static class ClassEmitter
{
    public static void Emit(
        EmissionResult result,
        string name,
        string? currentNamespace,
        IReadOnlyList<IdlMember> fields,
        IdlExtensibilityKind extensibility,
        string sourceIdlFileName,
        string? baseType,
        IReadOnlyList<IdlMember> inheritedFields,
        bool isTopic)
    {
        var names = IdlNaming.CreateGeneratedTypeNames(currentNamespace, name);
        var escapedName = names.ManagedTypeName;
        var emissionFields = fields.Select(field => EmissionTypeProjector.ToEmissionField(field, currentNamespace)).ToArray();
        var emissionInheritedFields = inheritedFields.Select(field => EmissionTypeProjector.ToEmissionField(field, currentNamespace)).ToArray();
        var initialFieldPlans = emissionFields.Select(field => new MemberEmissionPlan(field, currentNamespace)).ToArray();
        var managedBackingNames = ResolveManagedBackingNames(escapedName: names.ManagedTypeName, initialFieldPlans);
        var fieldPlans = initialFieldPlans
            .Select((field, index) => new MemberEmissionPlan(field.Field, currentNamespace, managedBackingNames[index]))
            .ToArray();
        var inheritedFieldPlans = emissionInheritedFields.Select(field => new MemberEmissionPlan(field, currentNamespace)).ToArray();
        var hasTypeSupport = fieldPlans.All(field => field.HasTypeSupport);
        var runtimeTypeName = currentNamespace is null ? name : $"{currentNamespace}.{name}";
        var isRecursive = fieldPlans.Any(field => field.IsRecursive(runtimeTypeName));
        var writer = EmissionSupport.CreateSource(currentNamespace, EmissionSupport.DataTypeUsings, sourceIdlFileName);
        var typeSummary = $"Represents the <c>{name}</c> DDS type declared in <c>{sourceIdlFileName}</c>.";

        if (inheritedFieldPlans.Concat(fieldPlans).Any(field => field.IsKey))
        {
            typeSummary += " Its key members form the DDS instance key.";
        }

        if (baseType is not null)
        {
            typeSummary += $" It derives from <see cref=\"{IdlNaming.ResolvedTypeReference(baseType, currentNamespace)}\"/>.";
        }

        typeSummary += $" It is marked as <c>{extensibility.ToString().ToLowerInvariant()}</c>.";
        if (isTopic)
        {
            typeSummary += " It is marked as a DDS topic type.";
        }

        writer.WriteXmlSummary(typeSummary);
        var baseReference = baseType is null ? null : IdlNaming.ResolvedTypeReference(baseType, currentNamespace);
        writer.OpenBlock($"public partial class {escapedName} : {(baseReference is null ? "" : baseReference + ", ")}global::System.IEquatable<{escapedName}>");

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
        result.Add(names.Managed, writer.ToString());

        if (hasTypeSupport)
        {
            TypeSupportEmitter.Emit(
                result,
                names,
                fieldPlans,
                inheritedFieldPlans,
                extensibility,
                sourceIdlFileName,
                baseType,
                isRecursive);
        }
    }

    private static string?[] ResolveManagedBackingNames(string escapedName, IReadOnlyList<MemberEmissionPlan> fields)
    {
        var occupied = new HashSet<string>(StringComparer.Ordinal)
        {
            escapedName
        };
        occupied.UnionWith(fields.Select(field => field.EscapedName));

        var result = new string?[fields.Count];
        foreach (var (field, index) in fields.Select((field, index) => (field, index)))
        {
            if (!field.HasManagedRange)
            {
                continue;
            }

            var candidate = IdlNaming.EscapeIdentifier("_" + field.Name);
            while (!occupied.Add(candidate))
            {
                candidate = IdlNaming.EscapeIdentifier("_" + candidate);
            }

            result[index] = candidate;
        }

        return result;
    }
}
