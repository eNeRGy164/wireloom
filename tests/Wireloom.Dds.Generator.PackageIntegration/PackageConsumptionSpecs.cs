namespace Wireloom.Dds.Generator.PackageIntegration;

/// <summary>
/// Verifies the packed Wireloom analyzer against the corpus integration entry points.
/// The broader positive and negative matrix is covered by CorpusCompliance.
/// </summary>
public sealed class PackageConsumptionSpecs
{
    [Fact]
    public void GeneratesTheIntegrationContractAndIncludedSupportType()
    {
        var message = new CorpusIntegrationMessage { text = "Package integration" };
        var status = new CorpusIntegrationStatus { active = true };

        message.text.ShouldBe("Package integration");
        status.active.ShouldBeTrue();
        new CorpusIntegrationMessage(message).ShouldBe(message);
    }

    [Fact]
    public void GeneratesNestedModuleNamespaces()
    {
        var message = new CorpusIntegration.Contracts.NamespacedMessage { enabled = true };

        message.enabled.ShouldBeTrue();
        new CorpusIntegration.Contracts.NamespacedMessage(message).ShouldBe(message);
    }

    [Fact]
    public void GeneratesTypedefsOptionalMembersAndInheritance()
    {
        var context = new CorpusIntegrationTrace.Context
        {
            correlation = "correlation",
            producer = "producer"
        };
        var derived = new CorpusIntegrationTrace.Derived
        {
            baseValue = 7,
            state = "state",
            traceContext = context
        };

        context.correlation.ShouldBe("correlation");
        context.producer.ShouldBe("producer");
        derived.baseValue.ShouldBe(7);
        derived.state.ShouldBe("state");
        derived.traceContext.ShouldBe(context);
    }

    [Fact]
    public void GeneratesComposedCollectionMembers()
    {
        var sample = new CorpusIntegrationCollections.Sample();
        var item = new CorpusIntegrationCollections.Item { id = 7, label = "item" };

        sample.unboundedItems.Add(item);
        sample.boundedItems.Add(new CorpusIntegrationCollections.Item { id = 8, label = "bounded" });
        sample.aggregateArray[0] = item;
        sample.rows[0].Value[0] = 11;
        sample.sequenceOfArrays.Add(new CorpusIntegrationCollections.Row());

        sample.unboundedItems.Count.ShouldBe(1);
        sample.boundedItems.Count.ShouldBe(1);
        sample.aggregateArray[0].id.ShouldBe(7);
        sample.rows[0].Value[0].ShouldBe(11);
        sample.sequenceOfArrays.Count.ShouldBe(1);
    }
}
