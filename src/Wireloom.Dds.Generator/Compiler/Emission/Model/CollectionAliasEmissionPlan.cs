using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Model;

/// <summary>Resolved facts shared by every emitter for one IDL typedef.</summary>
internal sealed class CollectionAliasEmissionPlan(
    EmissionTypePlan elementPlan,
    bool isSequence,
    bool isArray,
    bool nativeValueRequiresCast,
    string? elementNativeType = null,
    int? sequenceBound = null,
    RtiApiProfile? profile = null,
    bool isSequenceArrayElement = false)
{
    /// <summary>Gets whether the wrapper is generated for an anonymous sequence-array element.</summary>
    internal bool IsSequenceArrayElement { get; } = isSequenceArrayElement;

    private RtiApiProfile Profile { get; } = profile ?? RtiApiProfile.Rtiddsgen470;
    public EmissionTypePlan ElementPlan { get; } = elementPlan;

    public string ElementType => ElementPlan.CSharpType;

    public string ElementNativeType { get; } = elementNativeType ?? elementPlan.CSharpType;

    public bool IsSequence { get; } = isSequence;

    /// <summary>Gets the effective sequence limit shared by managed, native, and metadata emission.</summary>
    public int SequenceBound { get; } = sequenceBound ?? 100;

    public bool IsArray { get; } = isArray;

    public bool IsCollection => IsSequence || IsArray;

    public bool IsString => ElementPlan.Shape.IsString;

    public bool IsPrimitive => ElementPlan.Shape.IsPrimitive;

    public bool IsEnum => ElementPlan.Shape.IsEnum;

    public bool IsAggregate => ElementPlan.Shape.IsAggregate;

    public bool IsUnion => ElementPlan.Shape.IsUnion;

    public bool NativeValueRequiresCast { get; } = nativeValueRequiresCast;

    public bool CollectionElementIsAggregate => CollectionElementCopyPolicy.RequiresManagedCopy(ElementPlan);

    public bool IsWideString => ElementPlan.IsWideString;

    public bool IsStringBounded =>
        EmissionTypeProjector.UnwrapValueEmissionType(ElementPlan) is StringEmissionType { IsBounded: true };

    public int StringBound => ElementPlan.Bound ?? 255;

    /// <summary>Gets the RTI dynamic-type primitive name for a primitive element.</summary>
    public string PrimitiveDynamicType
    {
        get
        {
            var element = EmissionTypeProjector.UnwrapValueEmissionType(ElementPlan);

            if (element is not PrimitiveEmissionType primitive)
            {
                throw new InvalidOperationException("Expected a primitive collection element.");
            }

            var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
            return primitive.IdlName == "octet" ? Profile.OctetDynamicType : mapping.DynamicType;
        }
    }
}
