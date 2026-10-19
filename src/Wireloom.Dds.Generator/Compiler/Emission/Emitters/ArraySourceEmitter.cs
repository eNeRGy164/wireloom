using Wireloom.Compiler.Emission.Model;
using Wireloom.Compiler.Emission.Planning;
using Wireloom.Compiler.Emission.Writers;
using Wireloom.Compiler.Naming;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Writes the repeated loop shapes used by generated fixed arrays.</summary>
internal static class ArraySourceEmitter
{
    public static string ElementCount(IReadOnlyList<int> dimensions) =>
        string.Join(" * ", dimensions);

    /// <summary>Finds the enum type that supplies defaults for one collection element.</summary>
    public static EnumEmissionType? EnumElementType(EmissionTypePlan? elementType) =>
        elementType is null
            ? null
            : EmissionTypeProjector.UnwrapValueEmissionType(elementType) as EnumEmissionType;

    /// <summary>Builds the qualified C# expression for an enum's declared default literal.</summary>
    private static string EnumDefaultExpression(EnumEmissionType enumType, string? currentNamespace)
    {
        var enumTypeReference = IdlNaming.TypeReference(enumType.CSharpType, currentNamespace);
        var defaultMemberName = IdlNaming.EscapeIdentifier(enumType.DefaultMemberName);

        return $"{enumTypeReference}.{defaultMemberName}";
    }

    /// <summary>Emits managed loops that assign an enum's declared default to every array element.</summary>
    public static void EmitEnumDefaultInitialization(GeneratedSourceWriter writer, string target, EnumEmissionType enumType, IReadOnlyList<int> dimensions, string? currentNamespace)
    {
        var indices = OpenLoops(writer, dimensions);

        writer.WriteLine($"{target}{IndexExpression(indices)} = {EnumDefaultExpression(enumType, currentNamespace)};");

        CloseLoops(writer, indices.Count);
    }

    /// <summary>Emits a populated managed array and copies it into initialized native array storage.</summary>
    public static void EmitNativeEnumDefaultInitialization(
        GeneratedSourceWriter writer,
        string target,
        string elementType,
        string localName,
        EnumEmissionType enumType,
        IReadOnlyList<int> dimensions,
        string? currentNamespace)
    {
        var typeReference = IdlNaming.TypeReference(elementType, currentNamespace);
        var dimensionList = string.Join(", ", dimensions);
        writer.WriteLine($"var {localName} = new {typeReference}[{dimensionList}];");

        writer.BlankLine();
        EmitEnumDefaultInitialization(writer, localName, enumType, dimensions, currentNamespace);
        writer.BlankLine();

        writer.WriteLine($"{target}.ToNative({localName}, dimension: {ElementCount(dimensions)});");
    }

    public static void EmitAggregateInitialization(GeneratedSourceWriter writer, string target, string elementType, IReadOnlyList<int> dimensions)
    {
        var indices = OpenLoops(writer, dimensions);
        writer.WriteLine($"{target}{IndexExpression(indices)} = new {elementType}();");
        CloseLoops(writer, indices.Count);
    }

    public static void EmitAggregateCopy(GeneratedSourceWriter writer, string target, string source, string elementType, IReadOnlyList<int> dimensions)
    {
        var indices = OpenLoops(writer, dimensions);
        writer.WriteLine($"{target}{IndexExpression(indices)} = new {elementType}({source}{IndexExpression(indices)});");
        CloseLoops(writer, indices.Count);
    }

    public static List<string> OpenLoops(GeneratedSourceWriter writer, IReadOnlyList<int> dimensions)
    {
        var indices = new List<string>(dimensions.Count);
        for (var dimension = 0; dimension < dimensions.Count; dimension++)
        {
            var index = $"dimension{dimension}";
            indices.Add(index);
            writer.OpenBlock($"for (var {index} = 0; {index} < {dimensions[dimension]}; {index}++)");
        }

        return indices;
    }

    public static void CloseLoops(GeneratedSourceWriter writer, int count)
    {
        for (var index = 0; index < count; index++)
        {
            writer.CloseBlock();
        }
    }

    public static string IndexExpression(IReadOnlyList<string> indices) =>
        $"[{string.Join(", ", indices)}]";
}
