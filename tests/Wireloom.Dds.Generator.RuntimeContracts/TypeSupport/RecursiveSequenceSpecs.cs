using System.Diagnostics;
using RuntimeRecursiveStartup;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Checks typed support for direct recursive sequence members.</summary>
public sealed class RecursiveSequenceSpecs
{
    private const string ModeVariable = "WIRELOOM_RECURSIVE_SEQUENCE_MODE";
    private const string EvidenceVariable = "WIRELOOM_RECURSIVE_SEQUENCE_EVIDENCE";

    [Fact]
    public async Task CreatesDirectRecursiveSequenceSerializerInFreshProcess()
    {
        var childMode = Environment.GetEnvironmentVariable(ModeVariable);
        if (childMode == "direct-serializer")
        {
            VerifyDirectSerializer();
            await File.WriteAllTextAsync(
                Environment.GetEnvironmentVariable(EvidenceVariable)!,
                childMode,
                TestContext.Current.CancellationToken);
            return;
        }

        if (childMode is not null)
        {
            return;
        }

        var evidence = Path.Combine(Path.GetTempPath(), $"wireloom-recursive-sequence-{Guid.NewGuid():N}.txt");
        var start = new ProcessStartInfo("dotnet")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        start.ArgumentList.Add(typeof(RecursiveSequenceSpecs).Assembly.Location);
        start.ArgumentList.Add("--filter-class");
        start.ArgumentList.Add(typeof(RecursiveSequenceSpecs).FullName!);
        start.Environment[ModeVariable] = "direct-serializer";
        start.Environment[EvidenceVariable] = evidence;

        using var process = Process.Start(start)!;
        var output = process.StandardOutput.ReadToEndAsync(TestContext.Current.CancellationToken);
        var error = process.StandardError.ReadToEndAsync(TestContext.Current.CancellationToken);
        try
        {
            using var timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
            timeout.CancelAfter(TimeSpan.FromMinutes(2));
            await process.WaitForExitAsync(timeout.Token);
            process.ExitCode.ShouldBe(0, $"{await output}\n{await error}");
            File.Exists(evidence).ShouldBeTrue("The child must complete the recursive serializer round-trip.");
            (await File.ReadAllTextAsync(evidence, TestContext.Current.CancellationToken)).ShouldBe("direct-serializer");
        }
        catch (OperationCanceledException) when (!TestContext.Current.CancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException("Direct recursive sequence serializer creation exceeded two minutes.");
        }
        finally
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync(CancellationToken.None);
            }

            File.Delete(evidence);
        }
    }

    private static void VerifyDirectSerializer()
    {
        var expected = new SequenceNode { value = 31 };
        var child = new SequenceNode { value = 32 };
        child.children.Add(new SequenceNode { value = 33 });
        expected.children.Add(child);

        using var serializer = SequenceNodeSupport.Instance.CreateSerializer();
        var actual = serializer.Deserialize(serializer.Serialize(expected));

        actual.value.ShouldBe(31);
        actual.children.Count.ShouldBe(1);
        actual.children[0].value.ShouldBe(32);
        actual.children[0].children.Count.ShouldBe(1);
        actual.children[0].children[0].value.ShouldBe(33);
    }
}
