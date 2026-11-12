namespace Wireloom.Dds.Idl.Tests;

internal static class IdlTestSupport
{
    internal static IdlInput Input(string name, string content, bool generate = true) =>
        new(name, content, generate);
}
