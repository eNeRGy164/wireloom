namespace Wireloom.Compiler.FrontEnd.Preprocessing;

/// <summary>Builds source-line indexes used by logical location expansion.</summary>
internal static class PreprocessorSourceOrigin
{
    internal static int[] BuildLineStarts(string source)
    {
        var starts = new List<int> { 0 };
        for (var index = 0; index < source.Length; index++)
        {
            if (source[index] == '\n')
            {
                starts.Add(index + 1);
            }
        }

        return [.. starts];
    }
}
