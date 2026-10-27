namespace Wireloom.Dds.Generator.WireCompatibility.Runner;

/// <summary>Renders retained report artifacts from validated evidence within the runner process.</summary>
internal static class WireReportRenderer
{
    /// <summary>Validates inputs, then writes JSON and Markdown while allowing finalization to classify failures.</summary>
    internal static void Render(ReportRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = ReportEvidenceBuilder.Build(request);

        ReportJsonWriter.Write(report, request.JsonPath);
        MarkdownReportWriter.Write(report, request.MarkdownPath);
    }
}
