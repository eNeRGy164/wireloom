#nullable enable annotations

using System.Reflection;
using Rti.Config;

namespace Wireloom.Dds.Generator.WireCompatibility;

/// <summary>Parses the peer command line, selects its generated type, and reports failures.</summary>
internal static class Program
{
    private static int Main(string[] args)
    {
        try
        {
            Logger.Instance.SetVerbosity(Verbosity.Warning);
            var options = PeerOptions.Parse(args);
            Run(options);
            Console.WriteLine($"PASS {options.CaseId} {options.Role} {options.RuntimeVersion}");
            return 0;
        }
        catch (Exception exception)
        {
            while (exception is TargetInvocationException or TypeInitializationException && exception.InnerException is not null)
            {
                exception = exception.InnerException;
            }

            var detail = exception is FixtureException fixtureException
                ? $" {fixtureException.Message}"
                : string.Empty;
            Console.Error.WriteLine($"FAIL {exception.GetType().Name} {exception.TargetSite?.Name}{detail}");
            if (exception is not FixtureException)
            {
                Console.Error.WriteLine($"FAIL_DETAIL {exception.Message.Replace(Environment.NewLine, " ")}");
            }
            return 1;
        }
    }

    private static void Run(PeerOptions options)
    {
        var sampleType = Assembly.GetExecutingAssembly().GetType(options.TypeName, throwOnError: true)!;
        var runTypedPeer = typeof(TypedPeer).GetMethod(
            nameof(TypedPeer.Run),
            BindingFlags.NonPublic | BindingFlags.Static)!.MakeGenericMethod(sampleType);
        try
        {
            runTypedPeer.Invoke(null, [options]);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }
}
