using FsCheck;
using FsCheck.Fluent;

namespace Wireloom.Dds.Generator.Tests;

public sealed class FsCheckIdlCompilerFuzzingSpecs
{
    [Fact]
    [Trait("Category", "Fuzzing")]
    public void MutatedIdlTextDoesNotEscapeTheCompilerErrorBoundary()
    {
        // Arrange
        var property = Prop.ForAll(
            Arb.From(StructuredInputGenerator),
            DoesNotEscapeCompilerErrorBoundary);
        var configuration = Config.QuickThrowOnFailure.WithMaxTest(GetMaxTestCases());

        // Act
        Check.One(configuration, property);

        // Assert
        // Check.One throws when FsCheck finds a counterexample.
    }

    private static bool DoesNotEscapeCompilerErrorBoundary(string source)
    {
        try
        {
            IdlCompiler.CompileSourcesWithDiagnostics(
                [new IdlInput("fscheck.idl", source)],
                [],
                CancellationToken.None);
        }
        catch (IdlException)
        {
            // Expected for malformed or unsupported IDL.
        }

        return true;
    }

    private static Gen<string> BuildMutationGenerator(Gen<string> source) =>
        source.SelectMany(text =>
            Gen.Choose(0, 2).SelectMany(operation =>
                Gen.Choose(0, Math.Max(0, text.Length - 1)).SelectMany(position =>
                    Gen.Elements(MutationCharacters).Select(character =>
                        Mutate(text, operation, position, character)))));

    private static string Mutate(string text, int operation, int position, char character)
    {
        if (operation == 0 && text.Length > 0)
        {
            return text.Remove(position, 1);
        }

        if (operation == 1)
        {
            return text.Insert(Math.Min(position, text.Length), character.ToString());
        }

        if (text.Length == 0)
        {
            return character.ToString();
        }

        return text[..position] + character + text[(position + 1)..];
    }

    private static int GetMaxTestCases() =>
        int.TryParse(Environment.GetEnvironmentVariable("WIRELOOM_FSCHECK_TESTS"), out var testCases)
            ? Math.Clamp(testCases, 1, 100_000)
            : 1_000;

    private static Gen<string> StructuredInputGenerator =>
        Gen.Frequency(
            (1, Gen.Elements(Seeds)),
            (8, BuildMutationGenerator(Gen.Elements(Seeds))),
            (4, BuildMutationGenerator(BuildMutationGenerator(Gen.Elements(Seeds)))));

    private static readonly char[] MutationCharacters =
    [
        ' ', '\t', '\n', '\r', ';', ',', ':', '.', '#', '@', '(', ')', '[', ']', '{', '}', '<', '>',
        '+', '-', '/', '*', '=', '&', '|', '!', '?', '0', '1', '7', '9', '_', 'a', 'A', 'Z', '"', '\'', '\\'
    ];

    private static readonly string[] Seeds =
    [
        "module Sample { struct Value { long value; }; };",
        "module Sample { struct Value { boolean flag; char letter; wchar wide; float ratio; double precision; }; };",
        "module Sample { struct Value { sequence<long, 4> values; string<16> name; long numbers[2]; }; };",
        "module Sample { typedef sequence<long, 4> Values; struct Value { Values values; }; };",
        "module Outer { module Inner { struct Value { long value; }; }; };",
        "module Sample { enum State { Idle, Running, Stopped }; struct Value { State state; }; };",
        "module Sample { enum State { Idle, Running, Stopped }; union Choice switch(State) { case Idle: long value; default: string<16> text; }; };",
        "module Sample { union Choice switch(long) { case 0: long value; case 1: string<16> text; default: boolean flag; }; };",
        "#define VALUE 7\n#if VALUE\nconst long Constant = VALUE;\n#else\nconst long Other = 1;\n#endif",
        "#define ADD(a, b) a + b\nconst long Constant = ADD(1, 2);",
        "@appendable struct Value { long value; };",
        "struct Value { @key long id; @optional string<32> name; };",
        "struct Value { @id(3) long value; @hashid long hashed; };",
        "module Sample { struct Base { long base; }; struct Value : Base { long value; }; };",
        "module Sample { struct Value { sequence<string<16>, 4> values; }; };"
    ];
}
