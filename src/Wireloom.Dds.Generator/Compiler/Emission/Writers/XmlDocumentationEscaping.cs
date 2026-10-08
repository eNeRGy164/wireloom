namespace Wireloom.Compiler.Emission.Writers;

/// <summary>Escapes text placed inside XML documentation elements.</summary>
internal static class XmlDocumentationEscaping
{
    /// <summary>Escapes XML markup characters while preserving text content.</summary>
    public static string EscapeText(string text) => text
        .Replace("&", "&amp;")
        .Replace("<", "&lt;")
        .Replace(">", "&gt;");
}
