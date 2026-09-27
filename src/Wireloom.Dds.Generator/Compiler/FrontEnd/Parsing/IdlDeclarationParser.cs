using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;

namespace Wireloom.Compiler.FrontEnd.Parsing;

internal sealed class IdlDeclarationParser
{
    private readonly IdlParseContext context;
    private readonly IdlConstantParser constantParser;
    private readonly IdlTypedefParser typedefParser;
    private readonly IdlUnionParser unionParser;
    private readonly IdlStructDeclarationParser structParser;
    private readonly IdlEnumDeclarationParser enumParser;

    internal IdlDeclarationParser(IdlSymbolTable symbols, CancellationToken cancellationToken, ICollection<IdlDiagnostic>? diagnostics = null)
    {
        context = new IdlParseContext(symbols, cancellationToken, diagnostics);
        constantParser = new IdlConstantParser(context);
        typedefParser = new IdlTypedefParser(context);
        unionParser = new IdlUnionParser(context);
        structParser = new IdlStructDeclarationParser(context);
        enumParser = new IdlEnumDeclarationParser(context);
    }

    internal IReadOnlyList<IdlDeclaration> Declarations =>
        context.Declarations;

    internal bool TryGetClass(string name, out IdlClassDeclaration declaration) =>
        context.Classes.TryGetValue(name, out declaration);

    /// <summary>Parses declarations in one module and emits their documents.</summary>
    public void Parse(string declarations, IdlInput input, int baseOffset, string? currentNamespace)
    {
        var position = 0;

        while (position < declarations.Length)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            if (char.IsWhiteSpace(declarations[position]))
            {
                position++;
                continue;
            }

            var languageBindingAnnotation = LanguageBindingAnnotationPattern.Match(declarations[position..]);
            if (languageBindingAnnotation.Success)
            {
                position += languageBindingAnnotation.Length;
                continue;
            }

            var transferModeAnnotation = TransferModeAnnotationPattern.Match(declarations[position..]);
            if (transferModeAnnotation.Success)
            {
                position += transferModeAnnotation.Length;
                continue;
            }

            var dataRepresentationAnnotation = DataRepresentationAnnotationPattern.Match(declarations[position..]);
            if (dataRepresentationAnnotation.Success)
            {
                position += dataRepresentationAnnotation.Length;
                continue;
            }

            var allowedDataRepresentationAnnotation = AllowedDataRepresentationAnnotationPattern.Match(declarations[position..]);
            if (allowedDataRepresentationAnnotation.Success)
            {
                position += allowedDataRepresentationAnnotation.Length;
                continue;
            }

            var defaultNested = DefaultNestedAnnotationPattern.Match(declarations.Substring(position));
            if (defaultNested.Success)
            {
                position += defaultNested.Length;
                continue;
            }

            var unknownAnnotation = UnknownAnnotationPattern.Match(declarations[position..]);
            if (unknownAnnotation.Success && !KnownDeclarationAnnotationNames.Contains(unknownAnnotation.Groups["name"].Value))
            {
                var name = unknownAnnotation.Groups["name"].Value;
                if (UnsupportedAnnotationNames.Contains(name))
                {
                    context.Diagnostics?.Add(new IdlDiagnostic("DDSG0102", input, baseOffset + position, $"Annotation '{name}' is recognized but unsupported and will be ignored."));
                    throw new IdlException(
                        input,
                        baseOffset + position,
                        $"Annotation '@{name}' is not supported in this context.");
                }

                context.Diagnostics?.Add(new IdlDiagnostic("DDSG0101", input, baseOffset + position, $"Annotation '@{name}' is not recognized and will be ignored."));
                position += unknownAnnotation.Length;
                continue;
            }

            if (TryParseInterface(declarations, input, baseOffset, ref position) ||
                TryParseModule(declarations, input, baseOffset, currentNamespace, ref position) ||
                constantParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position) ||
                enumParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position) ||
                typedefParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position))
            {
                continue;
            }

            if (unionParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position))
            {
                continue;
            }

            position += structParser.Parse(declarations, input, baseOffset, currentNamespace, position);
        }

        foreach (var typedefName in context.Symbols.TypedefNames.ToArray())
        {
            typedefParser.Validate(input, baseOffset, typedefName);
        }
    }

    private static readonly HashSet<string> UnsupportedAnnotationNames =
        ["position", "bit_bound", "service"];

    private static readonly HashSet<string> KnownDeclarationAnnotationNames =
    [
        "topic", "autoid", "nested", "final", "appendable", "mutable",
        "language_binding", "transfer_mode", "data_representation",
        "allowed_data_representation", "default_nested"
    ];

    private bool TryParseInterface(string declarations, IdlInput input, int baseOffset, ref int position)
    {
        var @interface = InterfacePattern.Match(declarations[position..]);
        if (!@interface.Success)
        {
            return false;
        }

        context.Diagnostics?.Add(new IdlDiagnostic("DDSG0103", input, baseOffset + position, $"The interface '{@interface.Groups["name"].Value}' is ignored because it is not a DDS service."));

        position += @interface.Length;

        return true;
    }


    private bool TryParseModule(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var module = ModulePattern.Match(declarations.Substring(position));
        if (!module.Success)
        {
            return false;
        }

        var openBrace = position + module.Length - 1;
        var closeBrace = FindClosingBrace(declarations, openBrace);
        if (closeBrace < 0)
        {
            throw new IdlException(input, baseOffset + position, "Unterminated module declaration.");
        }

        var moduleName = context.Qualify(module.Groups[1].Value, currentNamespace);
        Parse(
            declarations.Substring(openBrace + 1, closeBrace - openBrace - 1),
            input,
            baseOffset + openBrace + 1,
            moduleName);

        position = closeBrace + 1;
        while (position < declarations.Length && char.IsWhiteSpace(declarations[position]))
        {
            position++;
        }

        if (position >= declarations.Length || declarations[position] != ';')
        {
            throw new IdlException(input, baseOffset + closeBrace, "Module declaration must end with a semicolon.");
        }

        position++;

        return true;
    }




    /// <summary>Finds the closing brace matching an opening brace.</summary>
    private static int FindClosingBrace(string text, int openingBrace)
    {
        var depth = 0;
        for (var index = openingBrace; index < text.Length; index++)
        {
            if (text[index] == '{')
            {
                depth++;
            }

            if (text[index] == '}' && --depth == 0)
            {
                return index;
            }
        }

        return -1;
    }
}
