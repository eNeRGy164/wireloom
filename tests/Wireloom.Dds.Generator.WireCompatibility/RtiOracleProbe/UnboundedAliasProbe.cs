using CorpusUnboundedSequences;
using CorpusUnboundedSequences.Implementation;
using CorpusUnboundedStrings;
using CorpusUnboundedStrings.Implementation;
using Omg.Types.Dynamic;
using Rti.Dds.NativeInterface.TypePlugin;
using Rti.Types;
using Rti.Types.Dynamic;

// This probe characterizes finite and unbounded retained RTI C# oracles without DDS exchanges.
if (args.Length > 1 || (args.Length == 1 && args[0] != "--unbounded"))
{
    throw new ArgumentException("Use no arguments for finite fallbacks or --unbounded for RTI unbounded support.");
}
var unboundedSupport = args.Length == 1;
var expectedStringBound = unboundedSupport ? StringType.Unbounded : 255;
var expectedSequenceBound = unboundedSupport ? SequenceType.Unbounded : 100;
var stringLength = unboundedSupport ? 300 : 255;
var sequenceLength = unboundedSupport ? 129 : 100;
var assertions = 0;

// Arrange
var text = new Text("hello");
var wideText = new WideText("世界");
var numbers = new Numbers(new Sequence<int> { 17, 18 });
var children = new Children(new Sequence<Child> { new Child { value = 17 } });

// Act
var textCopy = new Text(text);
var wideCopy = new WideText(wideText);
var numbersCopy = new Numbers(numbers);
var childrenCopy = new Children(children);

// Assert
Expect(new Text().Value == "" && new WideText().Value == "", "string defaults");
Expect(new Text((Text)null).Value == "" && new WideText((WideText)null).Value == "", "null string copies");
Expect(new Numbers().Value.Count == 0, "sequence default");
Expect(new Numbers((Numbers)null).Value == null, "null sequence copy characterization");
Expect(textCopy.Equals(text) && textCopy.GetHashCode() == text.GetHashCode(), "text copy value methods");
Expect(wideCopy.Equals(wideText) && wideCopy.GetHashCode() == wideText.GetHashCode(), "wide text copy value methods");
Expect(numbersCopy.Equals(numbers) && numbersCopy.GetHashCode() == numbers.GetHashCode(), "sequence copy value methods");
numbersCopy.Value[0] = 99;
Expect(numbers.Value[0] == 17, "sequence copy storage independence");
childrenCopy.Value[0].value = 99;
Expect(children.Value[0].value == 17, "aggregate sequence copy independence (F5 RTI baseline)");

foreach (var isPublic in new[] { false, true })
{
    // Arrange / Act
    var narrow = (StringType)((AliasType)TextPlugin.CreateDynamicType(isPublic)).RelatedType;
    var wide = (WideStringType)((AliasType)WideTextPlugin.CreateDynamicType(isPublic)).RelatedType;
    var textSequence = Sequence(TextsPlugin.CreateDynamicType(isPublic));
    var wideSequence = Sequence(WideTextsPlugin.CreateDynamicType(isPublic));

    // Assert
    Expect(narrow.Bounds == expectedStringBound && wide.Bounds == expectedStringBound, "string metadata bounds");
    Expect(Sequence(NumbersPlugin.CreateDynamicType(isPublic)).Bounds == expectedSequenceBound, "primitive sequence bound");
    Expect(Sequence(StatesPlugin.CreateDynamicType(isPublic)).Bounds == expectedSequenceBound, "enum sequence bound");
    Expect(Sequence(ChildrenPlugin.CreateDynamicType(isPublic)).Bounds == expectedSequenceBound, "aggregate sequence bound");
    Expect(Sequence(ScalarsPlugin.CreateDynamicType(isPublic)).Bounds == expectedSequenceBound, "scalar alias sequence bound");
    Expect(textSequence.Bounds == expectedSequenceBound && ((StringType)textSequence.ContentType).Bounds == expectedStringBound, "string sequence bounds");
    Expect(wideSequence.Bounds == expectedSequenceBound && ((WideStringType)wideSequence.ContentType).Bounds == expectedStringBound, "wide string sequence bounds");
    Expect(((StringType)Sequence(BoundedTextsPlugin.CreateDynamicType(isPublic)).ContentType).Bounds == 8, "bounded string elements");
    Expect(((WideStringType)Sequence(BoundedWideTextsPlugin.CreateDynamicType(isPublic)).ContentType).Bounds == 4, "bounded wide string elements");
}

Expect(TextSupport.Instance != null && WideTextSupport.Instance != null, "string support construction");
Expect(NumbersSupport.Instance != null && NumbersChainSupport.Instance != null, "sequence support construction");

RoundTrip<Text, TextUnmanaged>(new Text(new string('x', stringLength)), result => Expect(result.Value == new string('x', stringLength), "native string"));
RoundTrip<WideText, WideTextUnmanaged>(new WideText(new string('Ω', stringLength)), result => Expect(result.Value == new string('Ω', stringLength), "native wide string"));
RoundTrip<Numbers, NumbersUnmanaged>(new Numbers(new Sequence<int>(Enumerable.Range(1, sequenceLength))), result => Expect(result.Value.SequenceEqual(Enumerable.Range(1, sequenceLength)), "native primitive sequence"));
RoundTrip<States, StatesUnmanaged>(new States(new Sequence<State> { State.Ready }), result => Expect(result.Value.Count == 1 && result.Value[0] == State.Ready, "native enum sequence"));
RoundTrip<Children, ChildrenUnmanaged>(new Children(new Sequence<Child> { new Child { value = 17 } }), result => Expect(result.Value.Count == 1 && result.Value[0].value == 17, "native aggregate sequence"));
RoundTrip<Scalars, ScalarsUnmanaged>(new Scalars(new Sequence<int> { 17 }), result => Expect(result.Value.Count == 1 && result.Value[0] == 17, "native scalar alias sequence (RTI erases the element typedef)"));
RoundTrip<Texts, TextsUnmanaged>(new Texts(new Sequence<string> { new string('x', stringLength) }), result => Expect(result.Value.SequenceEqual(new[] { new string('x', stringLength) }), "native string sequence"));
RoundTrip<WideTexts, WideTextsUnmanaged>(new WideTexts(new Sequence<string> { new string('Ω', stringLength) }), result => Expect(result.Value.SequenceEqual(new[] { new string('Ω', stringLength) }), "native wide string sequence"));
RoundTrip<BoundedTexts, BoundedTextsUnmanaged>(new BoundedTexts(new Sequence<string> { "12345678" }), result => Expect(result.Value.SequenceEqual(new[] { "12345678" }), "native bounded string elements"));
RoundTrip<BoundedWideTexts, BoundedWideTextsUnmanaged>(new BoundedWideTexts(new Sequence<string> { "世界世界" }), result => Expect(result.Value.SequenceEqual(new[] { "世界世界" }), "native bounded wide string elements"));
RoundTrip<NumbersChain, NumbersChainUnmanaged>(new NumbersChain(numbers), result => Expect(result.Value.Value.SequenceEqual(new[] { 17, 18 }), "native sequence alias chain"));

Console.WriteLine($"RTI C# 7.7.0 alias oracle (unboundedSupport={unboundedSupport}): {assertions} assertions passed; 88 native flag/shape combinations. No DDS wire exchange performed.");

void Expect(bool condition, string contract)
{
    if (!condition)
    {
        throw new InvalidOperationException($"RTI oracle contract failed: {contract}");
    }

    assertions++;
}

static SequenceType Sequence(DynamicType type) => (SequenceType)((AliasType)type).RelatedType;

static void RoundTrip<TManaged, TNative>(TManaged source, Action<TManaged> verify)
    where TManaged : class, new()
    where TNative : struct, INativeTopicType<TManaged>
{
    foreach (var keysOnly in new[] { false, true })
    {
        foreach (var allocatePointers in new[] { false, true })
        {
            foreach (var allocateMemory in new[] { false, true })
            {
                // Arrange: allocateMemory=false requires existing backing storage.
                var native = new TNative();
                try
                {
                    if (!allocateMemory)
                    {
                        native.Initialize(allocatePointers: true, allocateMemory: true);
                    }

                    native.Initialize(allocatePointers, allocateMemory);
                    var destination = new TManaged();

                    // Act
                    native.ToNative(source, keysOnly);
                    native.FromNative(destination, keysOnly);

                    // Assert
                    verify(destination);
                    native.Destroy(optionalsOnly: true);
                    native.FromNative(destination, keysOnly);
                    verify(destination);
                }
                finally
                {
                    native.Destroy(optionalsOnly: false);
                }
            }
        }
    }
}
