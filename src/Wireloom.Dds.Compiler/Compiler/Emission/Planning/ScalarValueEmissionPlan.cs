using System.Globalization;
using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Planning;

/// <summary>Projects one bound scalar default into managed, native, and annotation values.</summary>
internal sealed class ScalarValueEmissionPlan(EmissionTypePlan declaredType, IdlMemberValueMetadata? metadata)
{
    private EmissionTypePlan ValueType { get; } = EmissionTypeProjector.UnwrapValueEmissionType(declaredType);

    /// <summary>Gets the explicit default or the enum's default literal, when applicable.</summary>
    internal string? ManagedDefaultLiteral
    {
        get
        {
            if (metadata?.DefaultValue is IdlScalarValue.Text { Value: var text })
            {
                return CSharpLiteralFormatter.FormatString(text);
            }

            if (metadata?.DefaultValue is IdlScalarValue.Floating { Value: var floating })
            {
                return CSharpScalarValueFormatter.FormatFloating(ValueType.CSharpType, floating);
            }

            if (ValueType is EnumEmissionType enumType)
            {
                var value = metadata?.DefaultValue is IdlScalarValue.Enumeration enumeration ? enumeration.Value : enumType.DefaultValue;
                return value == 0 ? "0" : $"({ValueType.CSharpType})({value.ToString(CultureInfo.InvariantCulture)})";
            }

            return metadata?.DefaultValue switch
            {
                IdlScalarValue.Integral integer => CSharpScalarValueFormatter.Format(ValueType.CSharpType, integer.Value),
                IdlScalarValue.Boolean boolean => boolean.Value ? "true" : "false",
                IdlScalarValue.Character character => CSharpScalarValueFormatter.Format("char", character.Value),
                _ => null
            };
        }
    }

    /// <summary>Gets the native scalar initialization expression.</summary>
    internal string NativeDefaultLiteral
    {
        get
        {
            if (ValueType is PrimitiveEmissionType primitive)
            {
                var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
                return metadata?.HasDefault == true
                    ? mapping.ToNativeExpression(ManagedDefaultLiteral!)
                    : mapping.NativeDefaultLiteral;
            }

            return ManagedDefaultLiteral ?? throw new InvalidOperationException("Expected a scalar native value.");
        }
    }

    /// <summary>Gets the runtime annotation values for a supported scalar type.</summary>
    internal (string TypeKind, string ValueProperty, string DefaultValue, string? Minimum, string? Maximum, string? Unit)? Annotation
    {
        get
        {
            var unit = metadata?.Unit is { } text ? CSharpLiteralFormatter.FormatString(text) : null;
            if (ValueType is StringEmissionType stringType)
            {
                var kind = stringType.IsWide ? "WideString" : "String";
                return (kind, $"{kind}Value", ManagedDefaultLiteral ?? "\"\"", null, null, null);
            }

            if (ValueType is EnumEmissionType enumType)
            {
                var value = metadata?.DefaultValue is IdlScalarValue.Enumeration enumeration ? enumeration.Value : enumType.DefaultValue;
                return ("Enumeration", "EnumValue", value.ToString(CultureInfo.InvariantCulture), null, null, unit);
            }

            if (ValueType is not PrimitiveEmissionType primitive)
            {
                return null;
            }

            var mapping = PrimitiveTypeMapping.Resolve(primitive.IdlName);
            if (mapping.AnnotationTypeKind is null || mapping.AnnotationValueProperty is null)
            {
                return null;
            }

            var minimum = metadata?.Minimum is { } lower
                ? CSharpScalarValueFormatter.Format(mapping.ManagedType, lower)
                : mapping.MinimumLiteral;
            var maximum = metadata?.Maximum is { } upper
                ? CSharpScalarValueFormatter.Format(mapping.ManagedType, upper)
                : mapping.MaximumLiteral;

            return (mapping.AnnotationTypeKind, mapping.AnnotationValueProperty,
                ManagedDefaultLiteral ?? mapping.AnnotationDefaultLiteral, minimum, maximum, unit);
        }
    }
}
