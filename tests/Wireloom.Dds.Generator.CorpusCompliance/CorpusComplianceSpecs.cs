using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Wireloom.Dds.Generator.CorpusCompliance;

public sealed class CorpusComplianceSpecs
{
    public static IEnumerable<object[]> CorpusCases() =>
        CorpusRepository.Cases
            .Where(corpusCase =>
                string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CORPUS_CASE")) ||
                string.Equals(corpusCase.Id, Environment.GetEnvironmentVariable("CORPUS_CASE"), StringComparison.Ordinal))
            .Select(corpusCase => new object[] { corpusCase });

    [Fact]
    public void CorpusManifestAndOracleLibraryHaveTheSameCaseSet()
    {
        var cases = CorpusRepository.Cases.Select(corpusCase => corpusCase.Id).ToArray();
        cases.Length.ShouldBe(111);
        cases.ShouldBeUnique();
        CorpusRepository.OracleCaseIds.ShouldBe(cases.OrderBy(id => id, StringComparer.Ordinal).ToArray());

        foreach (var corpusCase in CorpusRepository.Cases)
        {
            File.Exists(CorpusRepository.GetIdlPath(corpusCase.Idl)).ShouldBeTrue(corpusCase.Idl);
            CorpusRepository.OracleFiles(corpusCase).Count.ShouldBe(
                corpusCase.HasOracleOutput ? 2 : 0,
                corpusCase.Id);
        }
    }

    [Fact]
    public void ExportAcceptedCorpusSources()
    {
        var exportRoot = Environment.GetEnvironmentVariable("CORPUS_GENERATOR_EXPORT_ROOT");
        if (string.IsNullOrWhiteSpace(exportRoot))
        {
            Assert.Skip("Set CORPUS_GENERATOR_EXPORT_ROOT to publish generated corpus sources.");
        }

        var root = Path.GetFullPath(exportRoot);
        Directory.CreateDirectory(root);

        foreach (var corpusCase in CorpusRepository.Cases.Where(corpusCase => corpusCase.ExpectedToCompile))
        {
            var caseRoot = Path.Combine(root, corpusCase.SourceKind, corpusCase.Id);
            Directory.CreateDirectory(caseRoot);

            foreach (var source in Compile(corpusCase).Values)
            {
                var fileName = source.HintName.EndsWith(".g.cs", StringComparison.Ordinal)
                    ? source.HintName
                    : $"{source.HintName}.g.cs";
                File.WriteAllText(Path.Combine(caseRoot, fileName), source.Source);
            }
        }
    }

    [Fact]
    public void EveryCorpusIdlIsADeclaredRootOrAnIncludedSupportFile()
    {
        var roots = CorpusRepository.ManifestRoots
            .Select(entry => Path.GetFullPath(CorpusRepository.GetIdlPath(entry.Idl)))
            .ToHashSet(GetPathComparer());
        var allText = CorpusRepository.AllIdlFiles.Select(File.ReadAllText).ToArray();

        foreach (var path in CorpusRepository.AllIdlFiles)
        {
            if (roots.Contains(path))
            {
                continue;
            }

            var fileName = Path.GetFileName(path);
            allText.Any(text => text.Contains(fileName, StringComparison.Ordinal)).ShouldBeTrue(
                $"Unreferenced corpus IDL: {path}");
        }
    }

    [Theory]
    [MemberData(nameof(CorpusCases))]
    public void EveryCorpusCaseMatchesItsExpectedAcceptance(CorpusCase corpusCase)
    {
        if (corpusCase.IsUnsafeInProcess)
        {
            Assert.Skip("The current alias resolver overflows on this cyclic-alias probe; run it in an isolated compiler process before enabling this assertion.");
        }

        if (corpusCase.ExpectedToCompile)
        {
            Should.NotThrow(() => Compile(corpusCase), $"Case {corpusCase.Id} should compile.");
            return;
        }

        var exception = Should.Throw<IdlException>(() => Compile(corpusCase));
        exception.Message.ShouldNotBeNullOrWhiteSpace($"Case {corpusCase.Id} produced an empty diagnostic.");
    }

    [Theory]
    [MemberData(nameof(CorpusCases))]
    public void EveryAcceptedCorpusCasePreservesTheOracleDataContractShape(CorpusCase corpusCase)
    {
        if (!corpusCase.ExpectedToCompile)
        {
            return;
        }

        var generated = Compile(corpusCase);
        var oraclePaths = CorpusRepository.OracleFiles(corpusCase);
        var expected = ReadFileShapes(oraclePaths);
        var actual = ReadSourceShapes(generated.Values.Select(source => source.Source));

        foreach (var expectedType in expected)
        {
            actual.ShouldContain(
                candidate => candidate.Name == expectedType.Name && candidate.Kind == expectedType.Kind,
                $"{corpusCase.Id}: generated output is missing oracle type {expectedType.Name}.");

            var actualType = actual.Single(candidate => candidate.Name == expectedType.Name && candidate.Kind == expectedType.Kind);
            foreach (var expectedProperty in expectedType.Properties)
            {
                actualType.Properties.ShouldContain(
                    candidate => candidate.Name == expectedProperty.Name && candidate.Type == expectedProperty.Type,
                    $"{corpusCase.Id}: generated output is missing {expectedType.Name}.{expectedProperty.Name} with type {expectedProperty.Type}.");
            }

            expectedType.EnumMembers.ShouldBeSubsetOf(actualType.EnumMembers,
                $"{corpusCase.Id}: generated enum {expectedType.Name} does not preserve all oracle members.");
        }
    }

    [Fact]
    public void P09OptionalCollectionsAndStringMatchTheCurrentCorpusContract()
    {
        var optionalCollections = Compile(CorpusRepository.Cases.Single(corpusCase => corpusCase.Id == "09-optional-collections"));
        var sample = optionalCollections["CorpusOptionalCollections.Sample.g.cs"].Source;
        var sampleUnmanaged = optionalCollections["CorpusOptionalCollections.Implementation.SampleUnmanaged.g.cs"].Source;

        sample.ShouldContain("[Optional]\n    [Bound(4)]\n    public ISequence<int> values { get; set; }");
        sample.ShouldContain("[Optional]\n    public int[] items { get; set; }");
        sample.ShouldNotContain("values = new Sequence<int>();");
        sample.ShouldNotContain("items = new int[2]");

        sampleUnmanaged.ShouldContain("private NativeOptionalSeq values;");
        sampleUnmanaged.ShouldContain("private NativeUnmanagedOptionalArray items;");
        sampleUnmanaged.ShouldContain("values.FromNative<int>(out Sequence<int> valuesTemporary_);");
        sampleUnmanaged.ShouldContain("items.FromNative<int>(out int[] itemsTemporary_, dimensions: new int[] { 2 });");
        sampleUnmanaged.ShouldContain("values.ToNative<int>((Sequence<int>)sample.values, 4);");
        sampleUnmanaged.ShouldContain("items.ToNative<int>(sample.items, dimension: 2);");
        sampleUnmanaged.ShouldContain("values.Destroy(optionalsOnly);");
        sampleUnmanaged.ShouldContain("items.Destroy(optionalsOnly);");

        var optionalString = Compile(CorpusRepository.Cases.Single(corpusCase => corpusCase.Id == "09-evolution-optional"));
        var message = optionalString["CorpusOptionalEvolution.Message.g.cs"].Source;
        var messageUnmanaged = optionalString["CorpusOptionalEvolution.Implementation.MessageUnmanaged.g.cs"].Source;

        message.ShouldContain("[Optional]\n    [Bound(16)]\n    public string? optionalText { get; set; }");
        messageUnmanaged.ShouldContain("optionalText.FromNativeOptional();");
        messageUnmanaged.ShouldContain("optionalText.ToNativeOptional(sample.optionalText, 16);");
    }

    private static IReadOnlyDictionary<string, GeneratedIdlSource> Compile(CorpusCase corpusCase) =>
        IdlCompiler.CompileSources(CorpusRepository.BuildInputs(corpusCase), TestContext.Current.CancellationToken);

    private static IReadOnlyList<TypeShape> ReadFileShapes(IEnumerable<string> sourcePaths) =>
        ReadSourceShapes(sourcePaths.Select(File.ReadAllText), validateSyntax: false);

    private static IReadOnlyList<TypeShape> ReadSourceShapes(IEnumerable<string> sources, bool validateSyntax = true)
    {
        var shapes = new List<TypeShape>();
        foreach (var source in sources)
        {
            var tree = CSharpSyntaxTree.ParseText(source);
            if (validateSyntax)
            {
                tree.GetDiagnostics().Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                    .ShouldBeEmpty("Generated C# source contains a syntax error.");
            }

            foreach (var declaration in tree.GetRoot().DescendantNodes().OfType<BaseTypeDeclarationSyntax>())
            {
                var name = GetQualifiedName(declaration);
                if (IsSupportImplementation(declaration.Identifier.ValueText))
                {
                    continue;
                }

                var namespaceName = name.Contains('.')
                    ? name[..name.LastIndexOf('.')]
                    : string.Empty;
                var typeDeclaration = declaration as TypeDeclarationSyntax;
                shapes.Add(new TypeShape(
                    name,
                    declaration.Kind().ToString(),
                    typeDeclaration is null
                        ? []
                        : typeDeclaration.Members.OfType<PropertyDeclarationSyntax>()
                            .Where(property => property.Modifiers.Any(SyntaxKind.PublicKeyword))
                            .Select(property => new PropertyShape(property.Identifier.ValueText, Normalize(property.Type.ToString(), namespaceName)))
                            .ToArray(),
                    declaration is EnumDeclarationSyntax enumDeclaration
                        ? enumDeclaration.Members
                            .Select(member => member.Identifier.ValueText)
                            .ToHashSet(StringComparer.Ordinal)
                        : []));
            }
        }

        return shapes
            .GroupBy(shape => (shape.Name, shape.Kind))
            .Select(group => new TypeShape(
                group.Key.Name,
                group.Key.Kind,
                group.SelectMany(shape => shape.Properties).Distinct().ToArray(),
                group.SelectMany(shape => shape.EnumMembers).ToHashSet(StringComparer.Ordinal)))
            .OrderBy(shape => shape.Name, StringComparer.Ordinal)
            .ToArray();
    }

    private static string GetQualifiedName(BaseTypeDeclarationSyntax declaration)
    {
        var names = new Stack<string>();
        names.Push(declaration.Identifier.ValueText);
        for (var parent = declaration.Parent; parent is not null; parent = parent.Parent)
        {
            switch (parent)
            {
                case NamespaceDeclarationSyntax namespaceDeclaration:
                    names.Push(namespaceDeclaration.Name.ToString());
                    break;
                case FileScopedNamespaceDeclarationSyntax fileScopedNamespace:
                    names.Push(fileScopedNamespace.Name.ToString());
                    break;
                case BaseTypeDeclarationSyntax typeDeclaration:
                    names.Push(typeDeclaration.Identifier.ValueText);
                    break;
            }
        }

        return string.Join('.', names);
    }

    private static bool IsSupportImplementation(string name) =>
        name.EndsWith("Support", StringComparison.Ordinal) ||
        name.EndsWith("Unmanaged", StringComparison.Ordinal) ||
        name.EndsWith("Plugin", StringComparison.Ordinal);

    private static string Normalize(string type, string namespaceName) =>
        type.Replace("global::", string.Empty, StringComparison.Ordinal)
            .Replace(namespaceName + ".", string.Empty, StringComparison.Ordinal)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("?", string.Empty, StringComparison.Ordinal)
            .Replace("\r", string.Empty, StringComparison.Ordinal)
            .Replace("\n", string.Empty, StringComparison.Ordinal);

    private static StringComparer GetPathComparer() =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    private sealed record TypeShape(
        string Name,
        string Kind,
        IReadOnlyList<PropertyShape> Properties,
        IReadOnlySet<string> EnumMembers);

    private sealed record PropertyShape(string Name, string Type);
}
