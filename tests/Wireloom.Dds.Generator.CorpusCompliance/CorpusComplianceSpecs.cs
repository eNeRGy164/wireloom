using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Xml.Linq;

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
        cases.Length.ShouldBe(116);
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
    public void NewNegativeCasesDoNotRenumberExistingIntegrationTags()
    {
        // Arrange
        var cases = CorpusRepository.Cases.ToDictionary(corpusCase => corpusCase.Id, StringComparer.Ordinal);

        // Act
        var integrationTags = new[]
        {
            cases["13-integration"].Tag,
            cases["13-modules"].Tag,
            cases["13-alias-inheritance"].Tag,
            cases["13-compositions"].Tag
        };

        // Assert
        integrationTags.ShouldBe(["C108", "C109", "C110", "C111"]);
        cases["12-name-collisions"].Tag.ShouldBe("C112");
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
    public void EveryGeneratedXmlDocumentationBlockIsWellFormed()
    {
        // Arrange
        var acceptedCases = CorpusRepository.Cases.Where(corpusCase => corpusCase.ExpectedToCompile);

        // Act
        var documentationBlocks = acceptedCases
            .SelectMany(corpusCase => Compile(corpusCase).Values)
            .SelectMany(source => GetDocumentationBlocks(source.Source))
            .ToArray();

        // Assert
        documentationBlocks.ShouldNotBeEmpty();

        foreach (var documentation in documentationBlocks)
        {
            Should.NotThrow(() => XDocument.Parse($"<doc>{documentation}</doc>"));
        }
    }

    private static IEnumerable<string> GetDocumentationBlocks(string source)
    {
        var lines = new List<string>();

        foreach (var line in source.Split('\n'))
        {
            var trimmed = line.TrimStart();
            if (trimmed.StartsWith("///", StringComparison.Ordinal))
            {
                lines.Add(trimmed[3..].TrimStart());
                continue;
            }

            if (lines.Count == 0)
            {
                continue;
            }

            yield return string.Join(Environment.NewLine, lines);
            lines.Clear();
        }

        if (lines.Count > 0)
        {
            yield return string.Join(Environment.NewLine, lines);
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

        sample.ShouldContain("[Optional]\n    [Bound(4)]\n    public ISequence<int>? values { get; set; }");
        sample.ShouldContain("[Optional]\n    public int[]? items { get; set; }");
        sample.ShouldNotContain("values = new Sequence<int>();");
        sample.ShouldNotContain("items = new int[2]");

        sampleUnmanaged.ShouldContain("private NativeOptionalSeq values;");
        sampleUnmanaged.ShouldContain("private NativeUnmanagedOptionalArray items;");
        sampleUnmanaged.ShouldContain("values.FromNative<int>(out Sequence<int> valuesTemporary_);");
        sampleUnmanaged.ShouldContain("items.FromNative<int>(out int[] itemsTemporary_, dimensions: new int[] { 2 });");
        sampleUnmanaged.ShouldContain("values.ToNative<int>((Sequence<int>)sample.values!, 4);");
        sampleUnmanaged.ShouldContain("items.ToNative<int>(sample.items, 2);");
        sampleUnmanaged.ShouldContain("values.Destroy(optionalsOnly);");
        sampleUnmanaged.ShouldContain("items.Destroy(optionalsOnly);");

        var optionalString = Compile(CorpusRepository.Cases.Single(corpusCase => corpusCase.Id == "09-evolution-optional"));
        var message = optionalString["CorpusOptionalEvolution.Message.g.cs"].Source;
        var messageUnmanaged = optionalString["CorpusOptionalEvolution.Implementation.MessageUnmanaged.g.cs"].Source;

        message.ShouldContain("[Optional]\n    [Bound(16)]\n    public string? optionalText { get; set; }");
        messageUnmanaged.ShouldContain("optionalText.FromNativeOptional();");
        messageUnmanaged.ShouldContain("optionalText.ToNativeOptional(sample.optionalText, 16);");
    }

    [Fact]
    public void P09OptionalAggregateMemberMatchesTheRtiContract()
    {
        // Arrange
        var corpusCase = CorpusRepository.Cases.Single(corpusCase => corpusCase.Id == "09-optional-aggregate-member");

        // Act
        var generated = Compile(corpusCase);

        // Assert
        var holder = generated["CorpusOptionalAggregate.Holder.g.cs"].Source;
        var unmanaged = generated["CorpusOptionalAggregate.Implementation.HolderUnmanaged.g.cs"].Source;
        var plugin = generated["CorpusOptionalAggregate.Implementation.HolderPlugin.g.cs"].Source;

        holder.ShouldContain("[Optional]\n    public Payload? payload { get; set; }");
        holder.ShouldContain("[Optional]\n    public Payload? payloadAlias { get; set; }");
        holder.ShouldContain("[Optional]\n    public Choice? choice { get; set; }");
        holder.ShouldContain("[Optional]\n    public Choice? choiceAlias { get; set; }");
        unmanaged.ShouldContain("private NativeManagedOptional payload;");
        unmanaged.ShouldContain("private NativeManagedOptional payloadAlias;");
        unmanaged.ShouldContain("private NativeManagedOptional choice;");
        unmanaged.ShouldContain("private NativeManagedOptional choiceAlias;");
        unmanaged.ShouldContain("payload.FromNative<Payload, Implementation.PayloadUnmanaged>(out var payloadTemporary_);");
        unmanaged.ShouldContain("payload.ToNative<Payload, Implementation.PayloadUnmanaged>(sample.payload!);");
        unmanaged.ShouldContain("payloadAlias.ToNative<Payload, Implementation.PayloadUnmanaged>(sample.payloadAlias!);");
        unmanaged.ShouldContain("choice.ToNative<Choice, Implementation.ChoiceUnmanaged>(sample.choice!);");
        unmanaged.ShouldContain("choiceAlias.ToNative<Choice, Implementation.ChoiceUnmanaged>(sample.choiceAlias!);");
        unmanaged.ShouldContain("payload.Destroy<Payload, Implementation.PayloadUnmanaged>(optionalsOnly);");
        plugin.ShouldContain("new StructMember(\"payload\", PayloadSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 0)");
        plugin.ShouldContain("new StructMember(\"payloadAlias\", PayloadAliasSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 1)");
        plugin.ShouldContain("new StructMember(\"choiceAlias\", ChoiceAliasSupport.Instance.GetDynamicTypeInternal(isPublic), isOptional: true, id: 3)");
        plugin.ShouldContain("isOptional: true");
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
