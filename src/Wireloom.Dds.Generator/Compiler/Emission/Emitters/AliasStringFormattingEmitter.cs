using Wireloom.Compiler.Emission.Writers;

namespace Wireloom.Compiler.Emission.Emitters;

/// <summary>Emits string quoting for alias collection diagnostic output.</summary>
internal static class AliasStringFormattingEmitter
{
    /// <summary>Emits a private helper that preserves Unicode and escapes quotes, slashes, and controls.</summary>
    internal static void Emit(GeneratedSourceWriter writer, string methodName)
    {
        writer.OpenBlock($"private static string {methodName}(string? value)");
        writer.OpenBlock("if (value is null)");
        writer.WriteLine("return \"null\";");
        writer.CloseBlock();
        writer.BlankLine();
        writer.WriteLine("var text = new global::System.Text.StringBuilder();");
        writer.WriteLine("text.Append('\"');");
        writer.OpenBlock("foreach (var character in value)");
        writer.OpenBlock("switch (character)");
        writer.WriteLines("""
            case '"':
                text.Append("\\\"");
                break;
            case '\\':
                text.Append("\\\\");
                break;
            case '\n':
                text.Append("\\n");
                break;
            case '\r':
                text.Append("\\r");
                break;
            case '\t':
                text.Append("\\t");
                break;
            case '\b':
                text.Append("\\b");
                break;
            case '\f':
                text.Append("\\f");
                break;
            """);

        writer.WriteLine("default:");
        writer.Indent();
        writer.OpenBlock("if (character < ' ')");
        writer.WriteLine("text.Append(\"\\\\u\");");
        writer.WriteLine("text.Append(((int)character).ToString(\"x4\", global::System.Globalization.CultureInfo.InvariantCulture));");
        writer.CloseBlock();
        writer.OpenBlock("else");
        writer.WriteLine("text.Append(character);");
        writer.CloseBlock();
        writer.WriteLine("break;");
        writer.Unindent();
        writer.CloseBlock();
        writer.CloseBlock();
        writer.WriteLine("text.Append('\"');");
        writer.WriteLine("return text.ToString();");
        writer.CloseBlock();
    }
}
