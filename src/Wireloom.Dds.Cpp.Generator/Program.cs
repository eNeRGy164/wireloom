namespace Wireloom.Dds.Cpp.Generator;

internal static class Program
{
    internal static int Main(string[] args) => CppCommand.Run(args, Console.Out, Console.Error);
}
