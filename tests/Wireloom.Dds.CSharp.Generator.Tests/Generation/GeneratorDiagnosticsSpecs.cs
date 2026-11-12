using Microsoft.CodeAnalysis;
using Wireloom.Dds.Generator.Roslyn;

namespace Wireloom.Dds.Generator.Tests;

public sealed class GeneratorDiagnosticsSpecs
{
    [Theory]
    [InlineData("DDSG0001", "DDSG0001", DiagnosticSeverity.Error)]
    [InlineData("DDSG0004", "DDSG0004", DiagnosticSeverity.Error)]
    [InlineData("DDSG0102", "DDSG0102", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0103", "DDSG0103", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0104", "DDSG0104", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0105", "DDSG0105", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0106", "DDSG0106", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0107", "DDSG0107", DiagnosticSeverity.Info)]
    [InlineData("DDSG0108", "DDSG0108", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0109", "DDSG0109", DiagnosticSeverity.Warning)]
    [InlineData("DDSG0101", "DDSG0101", DiagnosticSeverity.Warning)]
    [InlineData("DDSG9999", "DDSG0101", DiagnosticSeverity.Warning)]
    public void ResolvesDiagnosticIdsToTheirPublicDescriptors(
        string id,
        string expectedId,
        DiagnosticSeverity expectedSeverity)
    {
        // Arrange

        // Act
        var descriptor = GeneratorDiagnostics.GetDescriptor(id);

        // Assert
        descriptor.Id.ShouldBe(expectedId);
        descriptor.DefaultSeverity.ShouldBe(expectedSeverity);
    }
}
