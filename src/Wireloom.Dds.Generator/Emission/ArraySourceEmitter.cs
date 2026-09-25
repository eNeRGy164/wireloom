namespace Wireloom;

/// <summary>Writes the repeated loop shapes used by generated fixed arrays.</summary>
internal static class ArraySourceEmitter
{
    public static string ElementCount(IReadOnlyList<int> dimensions) =>
        string.Join(" * ", dimensions);

    public static void EmitAggregateInitialization(
        GeneratedSourceWriter writer,
        string target,
        string elementType,
        IReadOnlyList<int> dimensions)
    {
        var indices = OpenLoops(writer, dimensions);
        writer.WriteLine($"{target}{IndexExpression(indices)} = new {elementType}();");
        CloseLoops(writer, indices.Count);
    }

    public static void EmitAggregateCopy(
        GeneratedSourceWriter writer,
        string target,
        string source,
        string elementType,
        IReadOnlyList<int> dimensions)
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
