using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Wireloom.Dds.CSharp.Compiler.Tests;

public sealed class Rti612ProfileSpecs
{
    /// <summary>Executes the profile-specific ownership contract without invoking native DDS support.</summary>
    [Theory]
    [InlineData(IdlCompilerProfile.Rti612, true)]
    [InlineData(IdlCompilerProfile.Rti731, false)]
    [InlineData(IdlCompilerProfile.Rti770, false)]
    public void CopiesCollectionAliasMembersUsingTheSelectedProfile(IdlCompilerProfile profile, bool sharesAliases)
    {
        // Arrange
        const string source = """
            module CopyOwnership {
                typedef sequence<long> Values;
                typedef Values ValuesAlias;
                typedef long Fixed[2];
                struct Child { long value; };
                struct Sample {
                    Values values;
                    ValuesAlias chained;
                    Fixed fixedValues;
                    sequence<long> directValues;
                    long directArray[2];
                    Child child;
                };
                union Choice switch(long) {
                    case 0: Values values;
                    case 1: Fixed fixedValues;
                    default: Child child;
                };
            };
            """;
        var result = IdlCompiler.Compile([new("copy.idl", source)], profile,
            CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);
        var version = profile switch
        {
            IdlCompilerProfile.Rti612 => "6.1.2",
            IdlCompilerProfile.Rti731 => "7.3.1",
            _ => "7.7.0"
        };
        var runtimePath = Path.Combine(AppContext.BaseDirectory, "RuntimeReferences", version, "Rti.ConnextDds.dll");
        var references = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator).Select(path => MetadataReference.CreateFromFile(path))
            .Append(MetadataReference.CreateFromFile(runtimePath));
        var trees = result.Documents.Select(document => CSharpSyntaxTree.ParseText(document.Source,
                new CSharpParseOptions(LanguageVersion.CSharp12), document.HintName));
        var compilation = CSharpCompilation.Create("CopyOwnership", trees, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
        using var output = new MemoryStream();
        var emitted = compilation.Emit(output, cancellationToken: TestContext.Current.CancellationToken);
        emitted.Success.ShouldBeTrue(string.Join(Environment.NewLine, emitted.Diagnostics));
        var context = new AssemblyLoadContext("CopyOwnership", isCollectible: true);
        context.Resolving += (_, name) => name.Name == "Rti.ConnextDds" ? context.LoadFromAssemblyPath(runtimePath) : null;

        try
        {
            output.Position = 0;
            var assembly = context.LoadFromStream(output);
            var sampleType = assembly.GetType("CopyOwnership.Sample")!;
            var original = Activator.CreateInstance(sampleType)!;

            // Act
            var copy = Activator.CreateInstance(sampleType, original)!;

            // Assert
            result.Success.ShouldBeTrue();
            foreach (var name in new[] { "values", "chained", "fixedValues" })
            {
                var property = sampleType.GetProperty(name)!;
                var originalAlias = property.GetValue(original)!;
                var copiedAlias = property.GetValue(copy)!;
                ReferenceEquals(originalAlias, copiedAlias).ShouldBe(sharesAliases);

                var valueProperty = originalAlias.GetType().GetProperty("Value")!;
                var originalValue = valueProperty.GetValue(originalAlias)!;
                var copiedValue = valueProperty.GetValue(copiedAlias)!;
                ReferenceEquals(originalValue, copiedValue).ShouldBe(sharesAliases);
            }

            foreach (var name in new[] { "directValues", "directArray", "child" })
            {
                var property = sampleType.GetProperty(name)!;
                property.GetValue(copy).ShouldNotBeSameAs(property.GetValue(original));
            }

            var arrayProperty = sampleType.GetProperty("fixedValues")!;
            var copiedArrayAlias = arrayProperty.GetValue(copy)!;
            var arrayValue = copiedArrayAlias.GetType().GetProperty("Value")!;
            ((int[])arrayValue.GetValue(copiedArrayAlias)!)[0] = 42;
            ((int[])arrayValue.GetValue(arrayProperty.GetValue(original))!)[0].ShouldBe(sharesAliases ? 42 : 0);

            var aliasType = assembly.GetType("CopyOwnership.Values")!;
            var alias = Activator.CreateInstance(aliasType)!;
            var aliasCopy = Activator.CreateInstance(aliasType, alias)!;
            aliasType.GetProperty("Value")!.GetValue(aliasCopy).ShouldNotBeSameAs(aliasType.GetProperty("Value")!.GetValue(alias));

            var chainType = assembly.GetType("CopyOwnership.ValuesAlias")!;
            var chain = Activator.CreateInstance(chainType)!;
            var chainCopy = Activator.CreateInstance(chainType, chain)!;
            var chainValue = chainType.GetProperty("Value")!;
            ReferenceEquals(chainValue.GetValue(chain), chainValue.GetValue(chainCopy)).ShouldBe(sharesAliases);

            var unionType = assembly.GetType("CopyOwnership.Choice")!;
            foreach (var branch in new[] { "values", "fixedValues" })
            {
                var union = Activator.CreateInstance(unionType)!;
                var property = unionType.GetProperty(branch)!;
                property.SetValue(union, Activator.CreateInstance(property.PropertyType));
                var unionCopy = Activator.CreateInstance(unionType, union)!;
                ReferenceEquals(property.GetValue(union), property.GetValue(unionCopy)).ShouldBe(sharesAliases);
            }
        }
        finally
        {
            context.Unload();
        }
    }

    /// <summary>Checks that each profile applies one int8 representation throughout generated APIs and support code.</summary>
    [Theory]
    [InlineData(IdlCompilerProfile.Rti612, "byte")]
    [InlineData(IdlCompilerProfile.Rti731, "sbyte")]
    [InlineData(IdlCompilerProfile.Rti770, "sbyte")]
    public void MapsInt8AcrossScalarAliasCollectionAndUnionShapes(IdlCompilerProfile profile, string expectedType)
    {
        // Arrange
        const string source = """
            typedef int8 Signed;
            typedef Signed SignedAlias;
            typedef sequence<int8, 3> SignedSequence;
            typedef int8 SignedArray[2];
            const int8 positive = 127;
            union Choice switch(int8) { case 1: int8 selected; default: long fallback; };
            struct Sample {
                int8 scalar;
                @optional int8 optionalScalar;
                SignedAlias aliased;
                int8 fixedValues[2];
                sequence<int8, 3> values;
                SignedSequence namedSequence;
                SignedArray namedArray;
                uint8 unsignedScalar;
            };
            """;

        // Act
        var result = IdlCompiler.Compile([new("sample.idl", source)], profile,
            CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);
        var generated = string.Concat(result.Documents.Select(document => document.Source));

        // Assert
        result.Success.ShouldBeTrue();
        generated.ShouldContain($"public {expectedType} scalar {{ get; set; }}");
        generated.ShouldContain($"private {expectedType} scalar;");
        generated.ShouldContain($"public {expectedType}? optionalScalar");
        generated.ShouldContain($"public {expectedType} aliased");
        generated.ShouldContain($"public {expectedType}[] fixedValues");
        generated.ShouldContain($"ISequence<{expectedType}>");
        generated.ShouldContain($"public {expectedType} Discriminator");
        generated.ShouldContain($"public const {expectedType} Value = 127;");
        generated.ShouldContain($"dtf.GetPrimitiveType<{expectedType}>()");
        generated.ShouldContain("public byte unsignedScalar");
    }

    /// <summary>Keeps IDL int8 range validation signed despite its legacy C# byte representation.</summary>
    [Theory]
    [InlineData(-129)]
    [InlineData(128)]
    public void RejectsInt8DefaultsOutsideTheSignedIdlRange(int value)
    {
        // Arrange
        var input = new IdlInput("sample.idl", $"struct Sample {{ @default({value}) int8 value; }};");

        // Act
        var result = IdlCompiler.Compile([input], IdlCompilerProfile.Rti612,
            CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeFalse();
        result.Documents.ShouldBeEmpty();
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0001");
    }

    [Theory]
    [InlineData("struct Sample { @optional sequence<long> values; };", "Optional sequences and arrays")]
    [InlineData("typedef sequence<string> Texts; typedef Texts Alias; struct Sample { @optional Alias values; };", "Optional sequences and arrays")]
    [InlineData("struct Sample { @optional long values[2]; };", "Optional sequences and arrays")]
    [InlineData("typedef string Texts[2]; struct Sample { @optional Texts values; };", "Optional sequences and arrays")]
    [InlineData("struct Sample { sequence<Sample> children; };", "Recursive aggregate types")]
    [InlineData("struct A; struct B { sequence<A> values; }; struct A { sequence<B> values; };", "Recursive aggregate types")]
    [InlineData("struct B; struct A { sequence<B> values; }; struct B { A value; };", "Recursive aggregate types")]
    [InlineData("struct Node; union Choice switch(long) { case 0: sequence<Node> children; }; struct Node { Choice choice; };", "Recursive aggregate types")]
    [InlineData("struct Sample { @must_understand long value; };", "@must_understand")]
    public void RejectsUnavailableShapesBeforeEmission(string source, string feature)
    {
        // Arrange
        var input = new IdlInput("/workspace/profile.idl", source);

        // Act
        var result = IdlCompiler.Compile([input], IdlCompilerProfile.Rti612, CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeFalse();
        result.Documents.ShouldBeEmpty();
        result.Diagnostics.ShouldContain(diagnostic => diagnostic.Id == "DDSG0004"
            && diagnostic.Path == input.Path && diagnostic.Message.Contains(feature, StringComparison.Ordinal)
            && diagnostic.Message.Contains("6.1.2", StringComparison.Ordinal));
        result.Diagnostics.ShouldAllBe(diagnostic => diagnostic.SourceText == source
            && diagnostic.Offset >= 0 && diagnostic.Offset < source.Length);
    }

    [Fact]
    public void ReportsIncludedUnavailableShapesAndEmitsNothingFromOtherRoots()
    {
        // Arrange
        const string shared = "typedef sequence<long> Values; struct Shared { @optional Values values; };";
        IdlInput[] inputs =
        [
            new("/workspace/main.idl", "#include \"shared.idl\"\nstruct Root { Shared value; };"),
            new("/workspace/shared.idl", shared, generate: false),
            new("/workspace/valid.idl", "struct Valid { long value; };")
        ];

        // Act
        var result = IdlCompiler.Compile(inputs, IdlCompilerProfile.Rti612, CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeFalse();
        result.Documents.ShouldBeEmpty();
        var diagnostic = result.Diagnostics.ShouldHaveSingleItem();
        diagnostic.Id.ShouldBe("DDSG0004");
        diagnostic.Path.ShouldBe("/workspace/shared.idl");
        diagnostic.SourceText.ShouldBe(shared);
    }

    [Theory]
    [InlineData("struct Sample { octet byteValue; int8 signedValue; uint8 unsignedValue; long value; };")]
    [InlineData("typedef int8 Signed; typedef Signed Alias; typedef sequence<int8, 3> Values; typedef int8 Fixed[2]; struct Sample { int8 value; @optional int8 optionalValue; Alias aliased; int8 fixedValues[2]; sequence<int8, 3> values; Values namedValues; Fixed namedArray; };")]
    [InlineData("const int8 minimum = -128; const int8 maximum = 127; struct Sample { @default(-1) int8 value; }; union Choice switch(int8) { case -128: int8 minimum; case 127: int8 maximum; default: int8 fallback; };")]
    [InlineData("struct Child { long value; }; struct Sample { @optional long number; @optional string text; @optional Child child; };")]
    [InlineData("typedef sequence<char> Chars; typedef sequence<wstring> Texts; struct Sample { Chars characters; Texts texts; sequence<long> values; long fixedValues[2]; };")]
    [InlineData("enum Color { Red, Green }; typedef Color Colors[2]; struct Sample { Colors colors; @default(12) long number; };")]
    [InlineData("union Choice switch(long) { case 0: case 1: string selected; default: long fallback; };")]
    [InlineData("struct Sample { sequence<char, 4> values; sequence<string, 4> texts; };")]
    [InlineData("struct Child { long value; }; union Choice switch(long) { case 0: Child child; default: long number; }; struct Holder { Choice choice; sequence<Child> children; };")]
    public void CompilesSupportedShapesAgainstTheExact612Assembly(string source)
    {
        // Arrange
        var result = IdlCompiler.Compile([new("sample.idl", source)], IdlCompilerProfile.Rti612,
            CompilerTestSupport.ComputeMd5, TestContext.Current.CancellationToken);
        var runtimePath = Path.Combine(AppContext.BaseDirectory, "RuntimeReferences", "6.1.2", "Rti.ConnextDds.dll");
        var platformPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator);
        var references = platformPaths.Select(path => MetadataReference.CreateFromFile(path))
            .Append(MetadataReference.CreateFromFile(runtimePath));
        var trees = result.Documents.Select(document => CSharpSyntaxTree.ParseText(document.Source,
            new CSharpParseOptions(LanguageVersion.CSharp12), document.HintName));
        var compilation = CSharpCompilation.Create("Scoped612", trees, references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));
        using var assembly = new MemoryStream();

        // Act
        var emitted = compilation.Emit(assembly, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        result.Success.ShouldBeTrue();
        result.Documents.ShouldNotBeEmpty();
        result.Documents.ShouldAllBe(document => !document.Source.Contains("xTypesComplianceMask", StringComparison.Ordinal));
        emitted.Success.ShouldBeTrue(string.Join(Environment.NewLine, emitted.Diagnostics));
    }
}
