using static Wireloom.Dds.Generator.Tests.CompilerTestSupport;

namespace Wireloom.Compiler.FrontEnd.Preprocessing.Tests;

public abstract class IdlPreprocessorTestBase
{
    protected static string Process(string text) =>
        new IdlPreprocessor([], []).Process(Input("preprocessor.idl", text), (_, _, _) => { });
}
