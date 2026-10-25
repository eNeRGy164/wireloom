using System.Diagnostics;

namespace Wireloom.Dds.Generator.RuntimeContracts.TypeSupport;

/// <summary>Runs support startup with fresh static state in both project and package consumers.</summary>
public sealed class RecursiveStartupSpecs
{
    private const string ModeVariable = "WIRELOOM_RECURSIVE_STARTUP_MODE";
    private const string EvidenceVariable = "WIRELOOM_RECURSIVE_STARTUP_EVIDENCE";

    /// <summary>Checks first access and discovery in separate processes, without plugin initialization.</summary>
    [Fact]
    public async Task InitializesRecursiveSupportInFreshProcesses()
    {
        // Arrange
        var childMode = Environment.GetEnvironmentVariable(ModeVariable);
        if (childMode is not null)
        {
            // Act
            RecursiveStartupCases.Verify(childMode);

            // Assert
            await File.WriteAllTextAsync(
                Environment.GetEnvironmentVariable(EvidenceVariable)!,
                childMode,
                TestContext.Current.CancellationToken);
            return;
        }

        foreach (var mode in new[] { "instance", "discovery" })
        {
            var evidence = Path.Combine(Path.GetTempPath(), $"wireloom-startup-{Guid.NewGuid():N}.txt");
            var start = new ProcessStartInfo("dotnet")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            start.ArgumentList.Add(typeof(RecursiveStartupSpecs).Assembly.Location);
            start.ArgumentList.Add("--filter-class");
            start.ArgumentList.Add(typeof(RecursiveStartupSpecs).FullName!);
            start.Environment[ModeVariable] = mode;
            start.Environment[EvidenceVariable] = evidence;

            // Act
            using var process = Process.Start(start)!;
            var output = process.StandardOutput.ReadToEndAsync(TestContext.Current.CancellationToken);
            var error = process.StandardError.ReadToEndAsync(TestContext.Current.CancellationToken);
            try
            {
                using var timeout = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
                timeout.CancelAfter(TimeSpan.FromMinutes(2));
                await process.WaitForExitAsync(timeout.Token);

                // Assert
                var diagnostics = $"{mode}: {await output}\n{await error}";
                if (diagnostics.Length > 16_000)
                {
                    diagnostics = diagnostics[..16_000] + "\n[Child diagnostics truncated]";
                }

                process.ExitCode.ShouldBe(0, diagnostics);
                File.Exists(evidence).ShouldBeTrue("The child must execute the startup assertions.");
                (await File.ReadAllTextAsync(evidence, TestContext.Current.CancellationToken)).ShouldBe(mode);
            }
            catch (OperationCanceledException) when (!TestContext.Current.CancellationToken.IsCancellationRequested)
            {
                throw new TimeoutException($"Recursive support {mode} startup exceeded two minutes.");
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
    }
}
