using System.Security.Cryptography;
using FsCheck;
using FsCheck.Fluent;
using Wireloom.Dds.Idl;

namespace Wireloom.Dds.Cpp.Fuzzing.Tests;

/// <summary>Exercises malformed and supported IDL through the public C++ compiler boundary.</summary>
public sealed class FsCheckIdlCompilerFuzzingSpecs
{
    /// <summary>Requires bounded input mutations to return deterministic documents or diagnostics.</summary>
    [Theory]
    [Trait("Category", "Fuzzing")]
    [InlineData(CppCompilerProfile.Rti612)]
    [InlineData(CppCompilerProfile.Rti731)]
    [InlineData(CppCompilerProfile.Rti770)]
    public void MutatedIdlPreservesThePublicCompilerBoundary(CppCompilerProfile profile)
    {
        // Arrange
        var inputs = Gen.Frequency(
            (1, Gen.Elements(Seeds)),
            (8, BuildMutationGenerator(Gen.Elements(Seeds))),
            (4, BuildMutationGenerator(BuildMutationGenerator(Gen.Elements(Seeds)))));
        var property = Prop.ForAll(Arb.From(inputs), source => CheckCompilerBoundary(source, profile));
        var maxTests = int.TryParse(Environment.GetEnvironmentVariable("WIRELOOM_FSCHECK_TESTS"), out var count)
            ? Math.Clamp(count, 1, 100_000)
            : 1_000;
        var configuration = Config.QuickThrowOnFailure.WithMaxTest(maxTests);

        // Act
        Check.One(configuration, property);

        // Assert
        // FsCheck throws with the generated input and replay information on failure.
    }

    private static bool CheckCompilerBoundary(string source, CppCompilerProfile profile)
    {
        var input = new IdlInput("fscheck.idl", source, strict: source.Length % 2 == 0);
        var request = new IdlCompilationRequest([input], MD5.HashData, TestContext.Current.CancellationToken);
        var first = CppCompiler.Compile(request, profile);
        var repeated = CppCompiler.Compile(request, profile);

        repeated.Success.ShouldBe(first.Success);
        repeated.Documents.Select(document => (document.FileName, document.Source))
            .ShouldBe(first.Documents.Select(document => (document.FileName, document.Source)));
        repeated.Diagnostics.Select(diagnostic => (diagnostic.Id, diagnostic.Severity, diagnostic.Offset, diagnostic.Message))
            .ShouldBe(first.Diagnostics.Select(diagnostic => (diagnostic.Id, diagnostic.Severity, diagnostic.Offset, diagnostic.Message)));
        if (!first.Success)
        {
            first.Documents.ShouldBeEmpty();
            first.Diagnostics.ShouldContain(diagnostic => diagnostic.Severity == IdlDiagnosticSeverity.Error);
        }

        first.Documents.Select(document => document.FileName).Distinct().Count().ShouldBe(first.Documents.Count);
        first.Documents.ShouldAllBe(document => !document.Source.Contains('\r'));
        return true;
    }

    private static Gen<string> BuildMutationGenerator(Gen<string> source) =>
        source.SelectMany(text =>
            Gen.Choose(0, 2).SelectMany(operation =>
                Gen.Choose(0, text.Length).SelectMany(position =>
                    Gen.Elements(MutationCharacters).Select(character =>
                        Mutate(text, operation, position, character)))));

    private static string Mutate(string text, int operation, int position, char character)
    {
        if (operation == 0 && text.Length > 0)
        {
            return text.Remove(Math.Min(position, text.Length - 1), 1);
        }

        if (operation == 1)
        {
            return text.Insert(Math.Min(position, text.Length), character.ToString());
        }

        if (text.Length == 0)
        {
            return character.ToString();
        }

        if (position == text.Length)
        {
            return text + character;
        }

        return text[..position] + character + text[(position + 1)..];
    }

    // Keep this input corpus identical to the C# fuzzing suite.
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
        """
        module Sample {
            struct Primitives {
                short signed16; long signed32; long long signed64;
                unsigned short unsigned16; unsigned long unsigned32; unsigned long long unsigned64;
                int8 fixedInt8; int16 fixedInt16; int32 fixedInt32; int64 fixedInt64;
                uint8 fixedUint8; uint16 fixedUint16; uint32 fixedUint32; uint64 fixedUint64;
                octet octetValue; boolean flag; char letter; wchar wideLetter;
                float ratio; double precision; long double extended;
            };
        };
        """,
        """
        module Sample {
            const long Base = 2 + 3;
            const long Bound = (Base << 2) | 1;
            typedef long Count;
            typedef Count Matrix[2][3];
            struct Value { sequence<string<16>, Bound> names; Matrix values; };
        };
        """,
        """
        module Sample {
            valuetype BaseValue { public long baseValue; };
            valuetype DerivedValue : BaseValue { public string<16> name; };
            struct Holder { DerivedValue value; };
        };
        """,
        """
        @language_binding(PLAIN)
        @transfer_mode(INBAND)
        module Sample {
            @autoid(HASH) @appendable struct Value {
                @key @hashid("stable_id") long id;
                @min(0) @max(100) @default(50) long count;
                @range(min = -32, max = 31) @unit("meters") long distance;
                @resolve_name(false) string<16> name;
                @external string<16> externalName;
            };
        };
        """,
        "@service(\"DDS\") interface Service { void ping(); };",
        """
        #define VALUE 7
        #if VALUE
        const long Constant = VALUE;
        #else
        const long Other = 1;
        #endif
        """,
        """
        #define ADD(a, b) a + b
        const long Constant = ADD(1, 2);
        """,
        "@appendable struct Value { long value; };",
        "struct Value { @key long id; @optional string<32> name; };",
        "struct Value { @id(3) long value; @hashid long hashed; };",
        "module Sample { struct Base { long base; }; struct Value : Base { long value; }; };",
        "module Sample { struct Value { sequence<string<16>, 4> values; }; };"
    ];

}
