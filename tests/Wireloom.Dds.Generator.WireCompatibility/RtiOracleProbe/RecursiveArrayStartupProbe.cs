using CorpusOptionalRecursiveArray;
using Rti.Dds.Topics;

// Compile with RtiOracleProbe.csproj against the untouched C134 oracle.
if (args.Length != 1 || args[0] is not ("instance" or "discovery" or "bootstrap"))
{
    throw new ArgumentException("Expected instance, discovery, or bootstrap.");
}

try
{
    if (args[0] == "bootstrap")
    {
        CorpusOptionalRecursiveArray.Implementation.NodePlugin.CreateDynamicType(isPublic: false);
    }

    var support = args[0] == "discovery"
        ? (TypeSupport<Node>)TypeSupportHelper.GetTypeSupportForType(typeof(Node))
        : NodeSupport.Instance;
    using var serializer = support.CreateSerializer();
    if (args[0] != "bootstrap")
    {
        throw new InvalidOperationException("RTI cold startup now succeeds; update the recorded limitation.");
    }

    var sample = new Node { value = 9801 };
    Verify(sample, serializer.Deserialize(serializer.Serialize(sample)));
    sample.children = [new Node { value = 9811 }, new Node { value = 9812 }];
    Verify(sample, serializer.Deserialize(serializer.Serialize(sample)));
    sample.children[0].children = [new Node { value = 9821 }, new Node { value = 9822 }];
    Verify(sample, serializer.Deserialize(serializer.Serialize(sample)));
    Console.WriteLine("PASS RTI bootstrap: absent, present, nested values and presence");
}
catch (Exception exception) when (args[0] != "bootstrap")
{
    while (exception is System.Reflection.TargetInvocationException or TypeInitializationException
        && exception.InnerException is not null)
    {
        exception = exception.InnerException;
    }

    if (exception is not NullReferenceException
        || exception.StackTrace?.Contains("NodePlugin.CreateDynamicType", StringComparison.Ordinal) != true)
    {
        throw;
    }

    Console.WriteLine($"PASS retained RTI limitation: {args[0]} fails with NullReferenceException");
}

static void Verify(Node expected, Node actual)
{
    if (actual.value != expected.value || (actual.children is null) != (expected.children is null))
    {
        throw new InvalidDataException("Recursive value or optional presence mismatch.");
    }

    if (expected.children is null)
    {
        return;
    }

    if (actual.children.Length != 2)
    {
        throw new InvalidDataException("Recursive array dimension mismatch.");
    }

    for (var index = 0; index < 2; index++)
    {
        Verify(expected.children[index], actual.children[index]);
    }
}
