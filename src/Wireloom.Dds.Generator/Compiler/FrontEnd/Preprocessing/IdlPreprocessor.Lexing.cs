namespace Wireloom.Compiler.FrontEnd.Preprocessing;

internal sealed partial class IdlPreprocessor
{
    private static bool IsIdentifier(string text) => PreprocessorLexicalService.IsIdentifier(text);

    private static bool IsLiteralStart(string text, int index) => PreprocessorLexicalService.IsLiteralStart(text, index);

    private static bool StartsPrefixedLiteral(string text, int index) =>
        PreprocessorLexicalService.StartsPrefixedLiteral(text, index);

    private static int SkipLiteral(string text, int index) => PreprocessorLexicalService.SkipLiteral(text, index);

    private static bool IsIdentifierStart(char character) => PreprocessorLexicalService.IsIdentifierStart(character);

    private static bool IsIdentifierPart(char character) => PreprocessorLexicalService.IsIdentifierPart(character);
}
