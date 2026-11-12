using Wireloom.Dds.Idl.Compiler.FrontEnd.Semantic;

using Wireloom.Dds.Cpp.Emission.Model;
using Wireloom.Dds.Cpp.Naming;

namespace Wireloom.Dds.Cpp.Emission.Planning;

/// <summary>Resolves bound IDL types and typedef targets to C++ storage and metadata types.</summary>
internal sealed class MemberTypeResolver(
    IReadOnlyDictionary<string, EnumPlan> enums,
    IReadOnlySet<string> structs,
    IReadOnlySet<string> unions,
    IReadOnlyDictionary<string, AliasPlan> aliases)
{
    /// <summary>Gets the planned enums indexed by their qualified IDL names.</summary>
    internal IReadOnlyDictionary<string, EnumPlan> Enums => enums;

    /// <summary>Checks whether a qualified IDL name identifies a struct in this compilation.</summary>
    internal bool ContainsStruct(string qualifiedName) => structs.Contains(qualifiedName);

    /// <summary>Resolves the supported C++ storage type of a bound member.</summary>
    internal bool TryResolveMemberType(
        IdlType type,
        out MemberTypePlan memberType,
        bool unboundedSupport = false)
    {
        switch (type)
        {
            case IdlType.Primitive when PrimitiveType.TryResolve(type, out var primitive):
                memberType = new MemberTypePlan.Primitive(primitive, primitive.DefaultValue);
                return true;
            case IdlType.Enum enumeration when enums.TryGetValue(enumeration.QualifiedName, out var enumPlan):
                memberType = new MemberTypePlan.Enumeration(
                    enumPlan.QualifiedName,
                    enumeration.DefaultMemberName);
                return true;
            case IdlType.Struct structure when structs.Contains(structure.QualifiedName):
                memberType = new MemberTypePlan.Struct(IdentifierRules.ToCppQualifiedName(structure.QualifiedName));
                return true;
            case IdlType.Union union when unions.Contains(union.QualifiedName):
                memberType = new MemberTypePlan.Union(IdentifierRules.ToCppQualifiedName(union.QualifiedName));
                return true;
            case IdlType.Alias alias when aliases.ContainsKey(alias.QualifiedName):
                var aliasPlan = aliases[alias.QualifiedName];
                memberType = aliasPlan.MetadataType;
                return true;
            case IdlType.StringType stringType:
                memberType = new MemberTypePlan.String(
                    stringType.IsUnbounded ? 0 : stringType.Bound,
                    stringType.IsWide);
                return true;
            case IdlType.Sequence
                {
                    IsUnbounded: true,
                    Dimensions.Count: 0,
                } unboundedSequence
                when TryResolveSequenceElement(unboundedSequence.Element, out var unboundedElementType)
                     && (unboundedElementType is MemberTypePlan.Primitive or MemberTypePlan.String { IsWide: false }
                         || unboundedSupport
                         && unboundedElementType is MemberTypePlan.String { IsWide: true } or MemberTypePlan.Struct):
                memberType = new MemberTypePlan.Sequence(unboundedElementType, unboundedSequence.EffectiveBound, IsUnbounded: true);
                return true;
            case IdlType.Sequence
                {
                    IsUnbounded: false,
                    EffectiveBound: > 0,
                } sequence when sequence.Dimensions.All(dimension => dimension > 0)
                                && TryResolveSequenceElement(sequence.Element, out var elementType):
                var plannedSequence = new MemberTypePlan.Sequence(elementType, sequence.EffectiveBound);
                memberType = sequence.Dimensions.Count == 0
                    ? plannedSequence
                    : new MemberTypePlan.Array(plannedSequence, sequence.Dimensions);
                return true;
            case IdlType.Array
                { Dimensions.Count: > 0 } array when array.Dimensions.All(dimension => dimension > 0)
                             && TryResolveArrayElement(array.Element, out var arrayElementType):
                memberType = new MemberTypePlan.Array(
                    arrayElementType,
                    array.Dimensions);
                return true;
            default:
                memberType = null!;
                return false;
        }
    }

    /// <summary>Resolves a typedef to its C++ spelling and canonical member type.</summary>
    internal bool TryResolveAliasTarget(
        IdlTypedef alias,
        out string cppType,
        out MemberTypePlan resolvedType,
        out string? structCppType)
    {
        if (alias.IsString)
        {
            resolvedType = new MemberTypePlan.String(
                alias.IsUnboundedString ? 0 : alias.StringBound,
                alias.IsWideString);
            cppType = resolvedType.StorageType;
            structCppType = null;
            return true;
        }

        if (alias.IsCollection)
        {
            if (alias.IsArray)
            {
                if (alias.Dimensions.Count == 0
                    || alias.Dimensions.Any(dimension => dimension <= 0)
                    || alias.ElementTypeWasEscaped
                    || !TryResolveNamedAliasTarget(alias.ElementType!, alias.Namespace,
                        out var arrayElementCppType, out var resolvedArrayElementType, out _)
                    || resolvedArrayElementType is not (MemberTypePlan.Primitive or MemberTypePlan.Enumeration))
                {
                    cppType = string.Empty;
                    resolvedType = null!;
                    structCppType = null;
                    return false;
                }

                var arrayElementType = arrayElementCppType == resolvedArrayElementType.StorageType
                    ? resolvedArrayElementType
                    : aliases.Values.Single(candidate => candidate.CppType == arrayElementCppType).MetadataType;
                resolvedType = new MemberTypePlan.Array(arrayElementType, alias.Dimensions);
                cppType = resolvedType.StorageType;
                structCppType = null;
                return true;
            }

            if (alias.Target != "sequence" || alias.Dimensions.Count != 0 || alias.ElementTypeWasEscaped
                || alias.Bound is <= 0
                || !TryResolveSequenceTypedefElement(alias,
                    out var elementCppType, out var resolvedElementType, out var elementStructCppType)
                || (alias.IsUnboundedSequence
                    ? !IsSupportedUnboundedSequenceTypedefElement(resolvedElementType)
                    : resolvedElementType is not (MemberTypePlan.Primitive
                        or MemberTypePlan.Enumeration
                        or MemberTypePlan.Struct
                        or MemberTypePlan.String
                        or MemberTypePlan.Sequence
                        or MemberTypePlan.Array)))
            {
                cppType = string.Empty;
                resolvedType = null!;
                structCppType = null;
                return false;
            }

            var elementType = aliases.Values.FirstOrDefault(candidate => candidate.CppType == elementCppType) is { } elementAlias
                ? elementAlias.MetadataType
                : resolvedElementType;
            resolvedType = new MemberTypePlan.Sequence(elementType, alias.EffectiveSequenceBound, alias.IsUnboundedSequence);
            cppType = resolvedType.StorageType;
            structCppType = elementStructCppType;
            return true;
        }

        return TryResolveNamedAliasTarget(alias.Target, alias.Namespace,
            out cppType, out resolvedType, out structCppType);
    }

    private bool TryResolveNamedAliasTarget(
        string target,
        string? @namespace,
        out string cppType,
        out MemberTypePlan resolvedType,
        out string? structCppType)
    {
        var normalized = target.TrimStart(':').Replace("::", ".", StringComparison.Ordinal);
        if (PrimitiveType.TryResolve(new IdlType.Primitive(target), out var primitive))
        {
            cppType = primitive.StorageType;
            resolvedType = new MemberTypePlan.Primitive(primitive, primitive.DefaultValue);
            structCppType = null;
            return true;
        }

        var scope = target.StartsWith("::", StringComparison.Ordinal) ? null : @namespace;
        while (true)
        {
            var candidate = string.IsNullOrEmpty(scope) ? normalized : $"{scope}.{normalized}";
            if (enums.TryGetValue(candidate, out var enumPlan))
            {
                cppType = enumPlan.CppType;
                resolvedType = new MemberTypePlan.Enumeration(enumPlan.QualifiedName, enumPlan.DefaultMember.Name);
                structCppType = null;
                return true;
            }

            if (structs.Contains(candidate))
            {
                cppType = $"::{IdentifierRules.ToCppQualifiedName(candidate)}";
                resolvedType = new MemberTypePlan.Struct(IdentifierRules.ToCppQualifiedName(candidate));
                structCppType = IdentifierRules.ToCppQualifiedName(candidate);
                return true;
            }

            if (unions.Contains(candidate))
            {
                cppType = $"::{IdentifierRules.ToCppQualifiedName(candidate)}";
                resolvedType = new MemberTypePlan.Union(IdentifierRules.ToCppQualifiedName(candidate));
                structCppType = null;
                return true;
            }

            if (aliases.TryGetValue(candidate, out var aliasPlan))
            {
                cppType = aliasPlan.CppType;
                resolvedType = aliasPlan.ResolvedType;
                structCppType = aliasPlan.StructTargetType;
                return true;
            }

            if (string.IsNullOrEmpty(scope))
            {
                cppType = string.Empty;
                resolvedType = null!;
                structCppType = null;
                return false;
            }

            var separator = scope.LastIndexOf('.');
            scope = separator < 0 ? null : scope[..separator];
        }
    }

    private bool TryResolveSequenceTypedefElement(
        IdlTypedef alias,
        out string cppType,
        out MemberTypePlan elementType,
        out string? structCppType)
    {
        if (alias is { ElementTypeWasEscaped: false, ElementTypeIsString: true })
        {
            cppType = alias.ElementTypeIsWideString ? "::std::wstring" : "::std::string";
            elementType = new MemberTypePlan.String(
                alias.IsUnboundedStringElement ? 0 : alias.StringBound,
                alias.ElementTypeIsWideString);
            structCppType = null;
            return true;
        }

        return TryResolveNamedAliasTarget(alias.ElementType!, alias.Namespace,
            out cppType, out elementType, out structCppType);
    }

    private static bool IsSupportedUnboundedSequenceTypedefElement(MemberTypePlan elementType) =>
        elementType is MemberTypePlan.Primitive
            or MemberTypePlan.Enumeration
            or MemberTypePlan.Struct
            or MemberTypePlan.String
        || elementType is MemberTypePlan.Alias { Target: MemberTypePlan.Primitive };

    private bool TryResolveSequenceElement(
        IdlType type,
        out MemberTypePlan elementType)
    {
        switch (type)
        {
            case IdlType.Primitive when PrimitiveType.TryResolve(type, out var primitive):
                elementType = new MemberTypePlan.Primitive(primitive, primitive.DefaultValue);
                return true;
            case IdlType.Enum enumeration when enums.TryGetValue(enumeration.QualifiedName, out var enumPlan):
                elementType = new MemberTypePlan.Enumeration(
                    enumPlan.QualifiedName,
                    enumeration.DefaultMemberName);
                return true;
            case IdlType.Struct structure when structs.Contains(structure.QualifiedName):
                elementType = new MemberTypePlan.Struct(IdentifierRules.ToCppQualifiedName(structure.QualifiedName));
                return true;
            case IdlType.StringType stringType:
                elementType = new MemberTypePlan.String(
                    stringType.IsUnbounded ? 0 : stringType.Bound,
                    stringType.IsWide);
                return true;
            case IdlType.Alias alias when aliases.TryGetValue(alias.QualifiedName, out var aliasPlan)
                && aliasPlan.ResolvedType is MemberTypePlan.Primitive
                    or MemberTypePlan.Enumeration
                    or MemberTypePlan.Struct
                    or MemberTypePlan.String
                    or MemberTypePlan.Sequence
                    or MemberTypePlan.Array:
                elementType = aliasPlan.MetadataType;
                return true;
            default:
                elementType = null!;
                return false;
        }
    }

    private bool TryResolveArrayElement(
        IdlType type,
        out MemberTypePlan elementType)
    {
        switch (type)
        {
            case IdlType.Primitive when PrimitiveType.TryResolve(type, out var primitive):
                elementType = new MemberTypePlan.Primitive(primitive, primitive.DefaultValue);
                return true;
            case IdlType.Enum enumeration when enums.TryGetValue(enumeration.QualifiedName, out var enumPlan):
                elementType = new MemberTypePlan.Enumeration(
                    enumPlan.QualifiedName,
                    enumPlan.DefaultMember.Name);
                return true;
            case IdlType.Struct structure when structs.Contains(structure.QualifiedName):
                elementType = new MemberTypePlan.Struct(IdentifierRules.ToCppQualifiedName(structure.QualifiedName));
                return true;
            case IdlType.StringType stringType:
                elementType = new MemberTypePlan.String(
                    stringType.IsUnbounded ? 0 : stringType.Bound,
                    stringType.IsWide);
                return true;
            case IdlType.Alias alias when aliases.TryGetValue(alias.QualifiedName, out var aliasPlan)
                && aliasPlan.ResolvedType is MemberTypePlan.Primitive
                    or MemberTypePlan.Enumeration
                    or MemberTypePlan.Array:
                elementType = aliasPlan.MetadataType;
                return true;
            default:
                elementType = null!;
                return false;
        }
    }
}
