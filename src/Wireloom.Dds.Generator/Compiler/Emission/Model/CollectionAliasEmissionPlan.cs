using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Model;

/// <summary>Resolved facts shared by every emitter for one IDL typedef.</summary>
internal sealed class CollectionAliasEmissionPlan(
    EmissionTypePlan elementPlan,
    bool isSequence,
    bool isArray,
    bool nativeValueRequiresCast,
    string? elementNativeType = null)
{
    public EmissionTypePlan ElementPlan { get; } = elementPlan;

    public string ElementType => ElementPlan.CSharpType;

    public string ElementNativeType { get; } = elementNativeType ?? elementPlan.CSharpType;

    public bool IsSequence { get; } = isSequence;

    public bool IsArray { get; } = isArray;

    public bool IsCollection => IsSequence || IsArray;

    public bool IsString => ElementPlan is StringEmissionType;

    public bool IsPrimitive => ElementPlan.Shape.IsPrimitive;

    public bool IsEnum => ElementPlan.Shape.IsEnum;

    public bool IsAggregate => ElementPlan.Shape.IsAggregate;

    public bool IsUnion => ElementPlan.Shape.IsUnion;

    public bool NativeValueRequiresCast { get; } = nativeValueRequiresCast;

    public bool CollectionElementIsAggregate => ElementPlan is AliasEmissionType || ElementPlan.Shape.IsAggregate;

    public bool IsWideString => ElementPlan.IsWideString;

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

            return PrimitiveTypeMapping.Resolve(primitive.IdlName).DynamicType;
        }
    }
}
