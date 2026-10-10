using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits the managed data class and its RTI type-support documents.</summary>
internal static class ClassEmitter
{
    /// <summary>Emits the managed class and optional type-support documents.</summary>
    public static IReadOnlyList<GeneratedIdlSource> Emit(
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
        var writer = EmissionSupport.CreateSource(currentNamespace, EmissionSupport.GetManagedTypeUsings(fieldPlans.Concat(inheritedFieldPlans)), sourceIdlFileName);
        var typeSummary = $"Represents the <c>{name}</c> DDS type declared in <c>{sourceIdlFileName}</c>.";

        if (inheritedFieldPlans.Concat(fieldPlans).Any(field => field.IsKey))
        {
            typeSummary += " Its key members identify a DDS instance across samples; they do not determine whether two complete samples are equal.";
        }

        if (baseType is not null)
        {
            typeSummary += $" It derives from <see cref=\"{IdlNaming.ResolvedTypeReference(baseType, currentNamespace)}\"/>.";
        }

        typeSummary += $" It is marked as <c>{extensibility.ToString().ToLowerInvariant()}</c>.";
        typeSummary += extensibility switch
        {
            IdlExtensibilityKind.Final => " The type's members and layout cannot be extended compatibly.",
            IdlExtensibilityKind.Extensible => " New members may be appended while preserving the existing member order for compatible type evolution.",
            IdlExtensibilityKind.Mutable => " Members may be identified and reordered by DDS member IDs during compatible type evolution.",
            _ => string.Empty
        };
        if (isTopic)
        {
            typeSummary += " It is marked for use as a DDS topic data type; the annotation does not create a Topic or publish data.";
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
            writer.WriteXmlSummary("Formats this sample as readable text.");
            writer.WriteXmlReturns("A readable string formatted by the type-support instance.");
            writer.WriteLine($"public override string ToString() => {escapedName}Support.Instance.ToString(this);");
        }

        writer.CloseBlock();
        var documents = new List<GeneratedIdlSource>
        {
            new(names.Managed.HintName, writer.ToString())
        };

        if (hasTypeSupport)
        {
            documents.AddRange(TypeSupportEmitter.Emit(
                names,
                fieldPlans,
                inheritedFieldPlans,
                extensibility,
                sourceIdlFileName,
                baseType,
                isRecursive));
        }

        return documents;
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
