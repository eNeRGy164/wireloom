#nullable enable annotations

using Rti.Types;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Builds collection and string-sequence union fixture values.</summary>
internal static partial class ManagedFixtures
{
#if WIRE_COMPATIBILITY_UNBOUNDED_SEQUENCE_ALIASES
    private static T CreateUnboundedSequenceAliasesFixture<T>(string fixture)
    {
        var sample = new CorpusUnboundedSequences.SequenceAliasesSample();
        if (fixture == "sequence-empty")
        {
            return (T)(object)sample;
        }
        if (fixture != "sequence-multiple")
        {
            throw new FixtureException($"Unknown unbounded sequence fixture '{fixture}'.");
        }

        sample.numbers.Value.Add(101);
        sample.numbers.Value.Add(-102);
        sample.states.Value.Add(CorpusUnboundedSequences.State.Idle);
        sample.states.Value.Add(CorpusUnboundedSequences.State.Ready);
        sample.children.Value.Add(new CorpusUnboundedSequences.Child(301));
        sample.children.Value.Add(new CorpusUnboundedSequences.Child(-302));
        sample.scalars.Value.Add(201);
        sample.scalars.Value.Add(-202);
        sample.texts.Value.Add("narrow-1");
        sample.texts.Value.Add("narrow-2");
        sample.wideTexts.Value.Add("wide-λ");
        sample.wideTexts.Value.Add("wide-2");
        sample.boundedTexts.Value.Add("12345678");
        sample.boundedTexts.Value.Add("bounded");
        sample.boundedWideTexts.Value.Add("ABCD");
        sample.boundedWideTexts.Value.Add("wide");
        return (T)(object)sample;
    }
#endif

#if WIRE_COMPATIBILITY_UNION_STRING_SEQUENCE_ALIASES
    private static T CreateStringSequenceUnionFixture<T>(string fixture)
    {
        var sample = new RuntimeUnionStringSequences.StringSequenceUnionsSample();
        var wide = fixture is "union-direct-wide" or "union-alias-wide" or "union-bounded-wide";
        switch (fixture)
        {
            case "union-direct-narrow":
            case "union-direct-wide":
                var directValues = new Sequence<string>();
                directValues.Add(wide ? "wide-λ" : "narrow-1");
                directValues.Add(wide ? "wide-2" : "narrow-2");
                directValues.Add(wide ? "wide-3" : "narrow-3");
                var direct = new RuntimeUnionStringSequences.DirectStringSequences();
                if (wide)
                {
                    direct.wide = directValues;
                }
                else
                {
                    direct.narrow = directValues;
                }
                sample.direct = direct;
                break;
            case "union-alias-narrow":
            case "union-alias-wide":
                var aliasValues = new RuntimeUnionStringSequences.StringSequenceAliases();
                if (wide)
                {
                    var values = new RuntimeUnionStringSequences.WideTexts();
                    values.Value.Add("wide-λ");
                    values.Value.Add("wide-2");
                    values.Value.Add("wide-3");
                    aliasValues.wide = values;
                }
                else
                {
                    var values = new RuntimeUnionStringSequences.NarrowTexts();
                    values.Value.Add("narrow-1");
                    values.Value.Add("narrow-2");
                    values.Value.Add("narrow-3");
                    aliasValues.narrow = values;
                }
                sample.aliases = aliasValues;
                break;
            case "union-bounded-narrow":
            case "union-bounded-wide":
                var boundedValues = new RuntimeUnionStringSequences.StringSequenceAliases();
                if (wide)
                {
                    var values = new RuntimeUnionStringSequences.BoundedWideTexts();
                    values.Value.Add("ABCD");
                    values.Value.Add("wide");
                    values.Value.Add("LAST");
                    boundedValues.boundedWide = values;
                }
                else
                {
                    var values = new RuntimeUnionStringSequences.BoundedNarrowTexts();
                    values.Value.Add("12345678");
                    values.Value.Add("bounded");
                    values.Value.Add("ABCDEFGH");
                    boundedValues.boundedNarrow = values;
                }
                sample.aliases = boundedValues;
                break;
            default:
                throw new FixtureException($"Unknown string-sequence union fixture '{fixture}'.");
        }
        return (T)(object)sample;
    }
#endif
}
