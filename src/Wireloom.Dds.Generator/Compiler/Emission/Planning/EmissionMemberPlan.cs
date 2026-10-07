using JetBrains.Annotations;
using Wireloom.Compiler.Emission.Emitters;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

using static Wireloom.Compiler.Naming.IdlNaming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>
/// Per-member emission decisions shared by managed and native source emitters.
/// The plan retains the typed emission type and builds context-sensitive source
/// operations on demand, so names, namespaces, keysOnly, and recursive type
/// support are not frozen into reusable source fragments.
/// </summary>
[PublicAPI]
internal sealed partial class MemberEmissionPlan(IdlEmissionField field, string? currentNamespace, string? managedBackingFieldName = null)
{
    private readonly MemberEmissionFacts facts = new(field, currentNamespace);
    private readonly MemberEmissionShape shape = new(field.Type);
    private MemberEmissionRenderer Renderer => new(facts, currentNamespace);

    public IdlEmissionField Field { get; } = field;
    public string? CurrentNamespace => facts.CurrentNamespace;
    public string Name => facts.Name;
    public string EscapedName => EscapeIdentifier(Name);
    public EmissionTypePlan Type => facts.Type;
    public bool IsKey => facts.IsKey;
    public int? MemberId => facts.MemberId;
    public bool IsOptional => facts.IsOptional;
    public string CSharpType => facts.CSharpType;
    public int? Bound => facts.Bound;
    public string? SupportType => facts.SupportType;
    public EmissionTypePlan? ElementType => facts.ElementType;
    public string? ElementCSharpType => facts.ElementCSharpType;
    public string? ElementSupportType => facts.ElementSupportType;
    public IReadOnlyList<int> Dimensions => facts.Dimensions;
    public BigInteger? DefaultValue => facts.ValueMetadata?.DefaultValue;
    public BigInteger? MinimumValue => facts.ValueMetadata?.Minimum;
    public BigInteger? MaximumValue => facts.ValueMetadata?.Maximum;
    public string? DefaultExpression => facts.ValueMetadata?.DefaultExpression;
    public string? Unit => facts.ValueMetadata?.Unit;
    public bool IsExternal => facts.IsExternal;
    public bool IsMustUnderstand => facts.IsMustUnderstand;
    public string? MemberIdHashSource => facts.MemberIdHashSource;
    public bool UsesAutoIdHash => facts.UsesAutoIdHash;
    public bool HasExplicitDefault => DefaultValue is not null;
    public bool HasManagedRange => MinimumValue is not null || MaximumValue is not null;
    public string ManagedBackingFieldName => managedBackingFieldName ?? EscapeIdentifier("_" + Name);
    public bool IsString => facts.IsString;
    public bool IsSequence => facts.IsSequence;
    public bool IsArray => facts.IsArray;
    public bool IsSequenceArray => IsSequence && Dimensions.Count > 0;
    public bool IsArrayLoopLocalCollision
    {
        get
        {
            if (!IsArray || !Name.StartsWith("dimension", StringComparison.Ordinal))
            {
                return false;
            }

            var suffix = Name["dimension".Length..];
            if (suffix.Length == 0)
            {
                return Dimensions.Count > 0;
            }

            return int.TryParse(suffix, out var dimension)
                && dimension >= 0
                && dimension < Dimensions.Count;
        }
    }
    public bool IsStringSequence => facts.IsStringSequence;
    public bool IsAggregate => facts.IsAggregate;
    public bool IsUnion => facts.IsUnion;
    public bool HasAggregateElement => facts.HasAggregateElement;
    public bool HasSequenceElement => IsArray && ElementType is not null && EmissionTypeProjector.HasSequenceType(ElementType);
    public string? BoundSummary => Renderer.BoundSummary;

    public string? ValueConstraintSummary => Renderer.ValueConstraintSummary;

    public ManagedInitializationKind ManagedInitialization => MemberEmissionPolicies.GetManagedInitialization(
        shape.Kind,
        IsOptional,
        IsSequence,
        IsArray,
        IsAggregate);

    public NativeDestroyKind DestroyKind => MemberEmissionPolicies.GetDestroyKind(
        IsAggregate,
        IsSequence,
        IsArray,
        IsString,
        IsOptionalScalar);

    public bool HasTypeSupport => Bound is null || EmissionTypeProjector.HasSequenceType(Type) || IsString;

    /// <summary>Determines whether this member recursively refers to the containing runtime type.</summary>
    public bool IsRecursive(string runtimeTypeName) =>
        IsSequence && string.Equals(ElementSupportType ?? ElementCSharpType, runtimeTypeName, StringComparison.Ordinal);

    public string ManagedPropertyAccessors => IsSequence && !IsOptional ? " { get; }" : " { get; set; }";
    public string ManagedPropertySummaryVerb => IsSequence && !IsOptional ? "Gets" : "Gets or sets";

    public string? ManagedPropertyInitializer => Renderer.ManagedPropertyInitializer;

    public string? ManagedDefaultInitializationStatement => Renderer.ManagedDefaultInitializationStatement(ManagedInitialization);

    public string ManagedDefaultValue => Renderer.ManagedDefaultValue;

    /// <summary>Formats an IDL constant value as a C# literal for the requested type.</summary>
    public string FormatCSharpValue(string typeName, BigInteger value) => Renderer.FormatCSharpValue(typeName, value);


    /// <summary>Resolves the native storage type for this member.</summary>
    public string NativeStorageTypeFor(string? namespaceOverride)
    {
        var namespaceName = namespaceOverride ?? currentNamespace;

        switch (shape.Kind)
        {
            case EmissionShapeKind.Sequence:
                if (IsStringSequence)
                {
                    if (IsOptional)
                    {
                        return ElementType!.IsWideString ? "NativeOptionalWstringSeq" : "NativeOptionalStringSeq";
                    }

                    return ElementType!.IsWideString ? "NativeWstringSeq" : "NativeStringSeq";
                }

                return IsOptional ? "NativeOptionalSeq" : "NativeSeq";
            case EmissionShapeKind.Array:
                if (IsOptional)
                {
                    return HasAggregateElement ? "NativeManagedOptionalArray" : "NativeUnmanagedOptionalArray";
                }

                return HasAggregateElement ? "NativeManagedArray" : "NativeUnmanagedArray";
        }

        if (IsOptionalScalar)
        {
            return "NativeUnmanagedOptional";
        }

        if (IsAggregate)
        {
            return GetReferencedUnmanagedType(namespaceName);
        }

        return BuildScalarNativeStorageType(namespaceName);
    }

    private string BuildScalarNativeStorageType(string? namespaceName)
    {
        if (ValueType is PrimitiveEmissionType primitive)
        {
            return PrimitiveTypeMapping.Resolve(primitive.IdlName).NativeStorageType;
        }

        if (ValueType is StringEmissionType stringType)
        {
            return stringType.IsWide ? "NativeWstring" : "NativeString";
        }

        var valueType = EmissionTypeProjector.UnwrapValueEmissionType(ValueType);
        if (valueType is EnumEmissionType { SupportType: not null } enumType)
        {
            return IdlNaming.ResolvedTypeReference(enumType.SupportType, namespaceName);
        }

        return TypeReference(CSharpType, namespaceName);
    }


    /// <summary>Builds the managed copy expression for this member.</summary>
    public string BuildCopyExpression(string sourcePrefix = "other.")
    {
        var source = sourcePrefix + EscapedName;

        if (IsSequence)
        {
            var elementType = TypeReference(ElementCSharpType!, currentNamespace);
            var copiedSequence = HasAggregateElement
                ? $"global::System.Linq.Enumerable.Select({source}, element => new {elementType}(element))"
                : source;
            if (IsOptional)
            {
                return $"{source} is null ? null! : new Sequence<{elementType}>({copiedSequence})";
            }

            return $"new Sequence<{elementType}>({copiedSequence})";
        }

        if (IsArray)
        {
            var copy = $"({TypeReference(CSharpType.TrimEnd('?'), currentNamespace)}){source}.Clone()";
            return IsOptional ? $"{source} is null ? null! : {copy}" : copy;
        }

        if (IsAggregate)
        {
            return $"new {TypeReference(CSharpType, currentNamespace)}({source})";
        }

        return source;
    }

    /// <summary>Builds the managed union copy expression for this member.</summary>
    public string BuildUnionCopyExpression(string sourcePrefix = "other.")
    {
        var source = sourcePrefix + EscapedName;

        if (IsSequence)
        {
            return $"new Sequence<{TypeReference(ElementCSharpType!, currentNamespace)}>({source})";
        }

        if (IsAggregate)
        {
            return $"new {TypeReference(CSharpType, currentNamespace)}({source})";
        }

        return source;
    }

    /// <summary>Emits initialization code for an aggregate array member.</summary>
    public void EmitAggregateArrayInitialization(GeneratedSourceWriter writer, string target, bool hasFollowingStatements)
    {
        if (!IsArray || !HasAggregateElement)
        {
            return;
        }

        writer.BlankLine();
        var indices = ArraySourceEmitter.OpenLoops(writer, Dimensions);
        writer.WriteLine($"{target}{ArraySourceEmitter.IndexExpression(indices)} = new {TypeReference(ElementCSharpType!, currentNamespace)}();");

        ArraySourceEmitter.CloseLoops(writer, indices.Count);

        if (hasFollowingStatements)
        {
            writer.BlankLine();
        }
    }

    /// <summary>Emits copy code for an aggregate array member.</summary>
    public void EmitAggregateArrayCopy(GeneratedSourceWriter writer, string target, bool hasFollowingStatements)
    {
        if (!IsArray || !HasAggregateElement)
        {
            return;
        }

        writer.BlankLine();

        if (IsOptional)
        {
            writer.OpenBlock($"if (other.{EscapedName} is not null)");
        }

        var indices = ArraySourceEmitter.OpenLoops(writer, Dimensions);
        writer.WriteLine($"{target}{ArraySourceEmitter.IndexExpression(indices)} = new {TypeReference(ElementCSharpType!, currentNamespace)}(other.{EscapedName}{ArraySourceEmitter.IndexExpression(indices)});");

        ArraySourceEmitter.CloseLoops(writer, indices.Count);

        if (IsOptional)
        {
            writer.CloseBlock();
        }

        if (hasFollowingStatements)
        {
            writer.BlankLine();
        }
    }

    /// <summary>Builds the hash expression for this member.</summary>
    public string HashValue(string targetPrefix = "")
    {
        if (IsOptional && IsSequence)
        {
            return $"{targetPrefix}{EscapedName}?.Count ?? -1";
        }

        if (IsOptional && IsArray)
        {
            return $"{targetPrefix}{EscapedName} is null ? -1 : {targetPrefix}{EscapedName}{ArraySourceEmitter.IndexExpression(ZeroIndices())}";
        }

        var suffix = shape.Kind switch
        {
            EmissionShapeKind.Array => ArraySourceEmitter.IndexExpression(ZeroIndices()),
            EmissionShapeKind.Sequence => ".Count",
            _ => string.Empty
        };

        return targetPrefix + EscapedName + suffix;
    }

    private IReadOnlyList<string> ZeroIndices() => Enumerable.Repeat("0", Dimensions.Count).ToArray();

    /// <summary>Builds the equality expression for this member.</summary>
    public string EqualityExpression(string otherPrefix = "other.", string thisPrefix = "")
    {
        if (IsArray)
        {
            string arrayEquality;
            if (Dimensions.Count == 1)
            {
                arrayEquality = $"global::System.Linq.Enumerable.SequenceEqual({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
            }
            else
            {
                arrayEquality = $"{thisPrefix}{EscapedName}.Rank == {otherPrefix}{EscapedName}.Rank && global::System.Linq.Enumerable.All(global::System.Linq.Enumerable.Range(0, {thisPrefix}{EscapedName}.Rank), dimension => {thisPrefix}{EscapedName}.GetLength(dimension) == {otherPrefix}{EscapedName}.GetLength(dimension)) && global::System.Linq.Enumerable.SequenceEqual(global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({thisPrefix}{EscapedName}), global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({otherPrefix}{EscapedName}))";
            }

            if (IsOptional)
            {
                return $"(global::System.Object.ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && {arrayEquality}))";
            }

            return arrayEquality;
        }

        if (IsOptional && IsSequence)
        {
            return $"(global::System.Object.ReferenceEquals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName}) || ({thisPrefix}{EscapedName} is not null && {otherPrefix}{EscapedName} is not null && global::System.Linq.Enumerable.SequenceEqual({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})))";
        }

        if (IsArray && HasAggregateElement)
        {
            return $"global::System.Linq.Enumerable.SequenceEqual(global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({thisPrefix}{EscapedName}), global::System.Linq.Enumerable.Cast<{TypeReference(ElementCSharpType!, currentNamespace)}>({otherPrefix}{EscapedName}))";
        }

        if (IsArray || IsSequence)
        {
            return $"global::System.Linq.Enumerable.SequenceEqual({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
        }

        if (IsOptional)
        {
            return $"Equals({thisPrefix}{EscapedName}, {otherPrefix}{EscapedName})";
        }

        return $"{thisPrefix}{EscapedName}.Equals({otherPrefix}{EscapedName})";
    }

    /// <summary>Builds the annotation metadata for a supported primitive member.</summary>
    public (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? PrimitiveAnnotation() =>
        Renderer.PrimitiveAnnotation();

    private bool IsOptionalScalar => IsOptional && !IsSequence && !IsArray && !IsString && !IsAggregate;

    private EmissionTypePlan ValueType => EmissionTypeProjector.UnwrapValueEmissionType(Type);

    private string GetReferencedUnmanagedType(string? namespaceOverride) =>
        EmissionSupport.GetUnmanagedType(SupportType ?? CSharpType, namespaceOverride);

    private string ElementUnmanagedType(string? namespaceOverride = null)
    {
        var typeName = ElementSupportType ?? ElementCSharpType!;

        return EmissionSupport.GetUnmanagedType(typeName, namespaceOverride ?? currentNamespace);
    }

    private string NullableValueType() => CSharpType.TrimEnd('?');

}
