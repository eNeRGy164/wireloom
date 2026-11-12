using static Wireloom.Dds.Idl.Tests.IdlTestSupport;

namespace Wireloom.Dds.Idl.Tests.Preprocessing;

public abstract class IdlPreprocessorTestBase
{
    protected static string Process(string text) =>
        new IdlPreprocessor([], []).Process(Input("preprocessor.idl", text), (_, _, _) => { });
}
