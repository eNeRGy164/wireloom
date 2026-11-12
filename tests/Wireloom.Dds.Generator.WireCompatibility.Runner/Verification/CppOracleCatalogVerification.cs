namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Checks RTI C++ oracle selection against the retained generation inventory.</summary>
internal static class CppOracleCatalogVerification
{
    /// <summary>Verifies retained 6.1.2 generated sources and rejects missing oracle cases.</summary>
    internal static void Run(MatrixOptions options)
    {
        // Arrange
        var manifest = Path.Combine(options.RepositoryRoot, "docs", "corpus", "6.1.2", "rtiddsgen", "cpp", "manifest.json");
        var catalog = new CppOracleCatalog(manifest, options.RepositoryRoot);
        var cases = new CorpusCaseCatalog(Path.Combine(options.RepositoryRoot, "docs", "corpus", "manifest.json"));

        // Act
        var optionalCollections = catalog.FindSource("09-optional-collections", cases.Get("09-optional-collections"), "6.1.2");
        var optionalStrings = catalog.FindSource("09-optional-string-sequences", cases.Get("09-optional-string-sequences"), "6.1.2");
        var flatDataBinding = catalog.FindSource("10-flat-data-binding", cases.Get("10-flat-data-binding"), "6.1.2");
        var absent = catalog.FindSource("case-without-retained-cpp-output", cases.Get("09-optional-collections"), "6.1.2");

        // Assert
        Check(optionalCollections == "cpp/features/09-optional-collections",
            "RTI 6.1.2 optional collections must resolve to their retained generated C++ source directory.");
        Check(optionalStrings == "cpp/features/09-optional-string-sequences",
            "RTI 6.1.2 optional string sequences must resolve to their retained generated C++ source directory.");
        Check(flatDataBinding == "cpp/features/10-flat-data-binding",
            "The verified RTI 6.1.2 FlatData adaptation must resolve to its retained generated C++ source directory.");
        Check(absent is null, "A missing RTI C++ capture must not be inferred from an arbitrary directory.");

        // Arrange
        var rti731Manifest = Path.Combine(options.RepositoryRoot, "docs", "corpus", "7.3.1", "rtiddsgen", "cpp", "manifest.json");
        var rti731Catalog = new CppOracleCatalog(rti731Manifest, options.RepositoryRoot);

        // Act
        var rti731OptionalCollections = rti731Catalog.FindSource("09-optional-collections",
            cases.Get("09-optional-collections"), "7.3.1");
        var rti731FlatDataBinding = rti731Catalog.FindSource("10-flat-data-binding",
            cases.Get("10-flat-data-binding"), "7.3.1");
        var wrongProfile = rti731Catalog.FindSource("09-optional-collections",
            cases.Get("09-optional-collections"), "6.1.2");
        var rti731TopicType = rti731Catalog.FindTopicType("09-optional-collections",
            cases.Get("09-optional-collections"), "7.3.1");

        // Assert
        Check(rti731OptionalCollections == "cpp/features/09-optional-collections",
            "The legacy RTI 7.3.1 generatedFiles entry must derive its source directory and verify pinned outputs.");
        Check(rti731FlatDataBinding == "cpp/features/10-flat-data-binding",
            "The verified RTI 7.3.1 FlatData adaptation must resolve to its retained generated C++ source directory.");
        Check(wrongProfile is null, "A retained RTI C++ oracle must not be selected for a different profile.");
        Check(rti731TopicType == "::CorpusOptionalCollections::Sample",
            "The verified legacy RTI 7.3.1 output must resolve its exact topic type.");

        // Arrange / Act: RTI 7.7.0's earliest retained entries use the pre-hash inventory shape.
        var rti770Manifest = Path.Combine(options.RepositoryRoot, "docs", "corpus", "7.7.0", "rtiddsgen", "cpp", "manifest.json");
        var rti770Catalog = new CppOracleCatalog(rti770Manifest, options.RepositoryRoot);
        var rti770Primitives = rti770Catalog.FindSource("01-primitives", cases.Get("01-primitives"), "7.7.0");
        var rti770Boundaries = rti770Catalog.FindSource("01-boundaries", cases.Get("01-boundaries"), "7.7.0");
        var rti770Int8Boundaries = rti770Catalog.FindSource("01-int8-boundaries", cases.Get("01-int8-boundaries"), "7.7.0");
        var rti770TopicType = rti770Catalog.FindTopicType("01-primitives", cases.Get("01-primitives"), "7.7.0");

        // Assert
        Check(rti770Primitives == "cpp/features/01-primitives",
            "The exact pre-hash RTI 7.7.0 source inventory must resolve its four named generated files.");
        Check(rti770Boundaries == "cpp/features/01-boundaries",
            "The retained RTI 7.7.0 boundary files must resolve from their exact named-file inventory.");
        Check(rti770Int8Boundaries == "cpp/features/01-int8-boundaries",
            "The retained RTI 7.7.0 int8-boundary files must resolve from their exact named-file inventory.");
        Check(rti770TopicType == "::CorpusPrimitives::Sample",
            "The legacy RTI 7.7.0 source inventory must resolve its topic type.");

        Console.WriteLine("RTI C++ oracle provenance passed: 6.1.2 outputs and pinned 7.3.1 legacy generatedFiles outputs verified.");
    }

    private static void Check(bool condition, string message)
    {
        if (!condition)
        {
            throw new InvalidOperationException(message);
        }
    }
}
