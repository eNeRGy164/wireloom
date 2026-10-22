namespace Wireloom.Compiler.FrontEnd.Symbols;

/// <summary>Checks the syntax of an IDL identifier with optional module qualification.</summary>
internal static class IdlScopedName
{
    /// <summary>Checks identifier segments separated by double colons, preserving an optional absolute prefix.</summary>
    internal static bool IsValid(string name)
    {
        var index = name.StartsWith("::", StringComparison.Ordinal) ? 2 : 0;
        while (index < name.Length)
        {
            if (name[index] != '_' && !char.IsLetter(name[index]))
            {
                return false;
            }

            index++;
            while (index < name.Length && (name[index] == '_' || char.IsLetterOrDigit(name[index])))
            {
                index++;
            }

            if (index == name.Length)
            {
                return true;
            }

            if (index + 1 >= name.Length || name[index] != ':' || name[index + 1] != ':')
            {
                return false;
            }

            index += 2;
        }

        return false;
    }
}
