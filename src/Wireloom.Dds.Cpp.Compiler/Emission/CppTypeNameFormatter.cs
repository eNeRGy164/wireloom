namespace Wireloom.Dds.Cpp.Emission;

using System.Text;

/// <summary>Formats absolute generated type references for use inside their owning namespace.</summary>
internal static class CppTypeNameFormatter
{
    /// <summary>Removes the current namespace prefix from generated type spellings.</summary>
    internal static string MakeRelative(string spelling, IReadOnlyList<string> namespaceSegments)
    {
        if (namespaceSegments.Count > 0)
        {
            var prefix = $"::{string.Join("::", namespaceSegments)}::";
            return spelling.Replace(prefix, string.Empty, StringComparison.Ordinal);
        }

        // A global qualifier adds no value for an IDL type in the global
        // namespace. Keep external namespace qualifiers such as ::std and
        // ::omg while allowing IDL names nested in their templates to go bare.
        var result = new StringBuilder(spelling.Length);
        for (var index = 0; index < spelling.Length;)
        {
            if (index + 2 < spelling.Length
                && spelling[index] == ':'
                && spelling[index + 1] == ':'
                && IsIdentifierStart(spelling[index + 2]))
            {
                var pathEnd = index + 2;
                while (pathEnd < spelling.Length
                    && (IsIdentifierPart(spelling[pathEnd])
                        || pathEnd + 1 < spelling.Length
                        && spelling[pathEnd] == ':'
                        && spelling[pathEnd + 1] == ':'))
                {
                    pathEnd += spelling[pathEnd] == ':' ? 2 : 1;
                }

                var rootEnd = index + 2;
                while (rootEnd < spelling.Length && IsIdentifierPart(spelling[rootEnd]))
                {
                    rootEnd++;
                }

                var root = spelling[(index + 2)..rootEnd];
                var isExternal = root is "std" or "dds" or "rti" or "omg" or "RTI" or "NDDS";
                result.Append(spelling, isExternal ? index : index + 2, pathEnd - (isExternal ? index : index + 2));
                index = pathEnd;
                continue;
            }

            result.Append(spelling[index]);
            index++;
        }

        return result.ToString();
    }

    private static bool IsIdentifierStart(char value) => char.IsLetter(value) || value == '_';

    private static bool IsIdentifierPart(char value) => char.IsLetterOrDigit(value) || value == '_';
}
