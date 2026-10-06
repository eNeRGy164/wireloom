using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;
using Wireloom.Compiler.FrontEnd.Preprocessing;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses top-level IDL declarations and coordinates semantic binding.</summary>
internal sealed class IdlDeclarationParser
{
    private readonly IdlParseContext context;
    private readonly IdlConstantParser constantParser;
    private readonly IdlTypedefParser typedefParser;
    private readonly IdlUnionParser unionParser;
    private readonly IdlStructDeclarationParser structParser;
    private readonly IdlEnumDeclarationParser enumParser;

    /// <summary>Initializes a declaration parser with symbol and diagnostic services.</summary>
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

    /// <summary>Parses declarations from an IDL input.</summary>
    public void Parse(
        string declarations,
        IdlInput input,
        int baseOffset,
        string? currentNamespace,
        IReadOnlyList<SourceOriginSpan>? sourceOrigins = null)
    {
        // Always replace the mapping: sequential files must never inherit the
        // previous parse's origin table.
        context.SetSourceOrigins(sourceOrigins);

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

            if (TryParseServiceInterface(declarations, input, baseOffset, ref position))
            {
                continue;
            }

            var unknownAnnotation = UnknownAnnotationPattern.Match(declarations[position..]);
            if (unknownAnnotation.Success && !KnownDeclarationAnnotationNames.Contains(unknownAnnotation.Groups["name"].Value))
            {
                var name = unknownAnnotation.Groups["name"].Value;
                if (UnsupportedAnnotationNames.Contains(name))
                {
                    context.Diagnostics?.Add(new IdlDiagnostic("DDSG0102", input, context.MapOffset(baseOffset + position), $"Annotation '{name}' is recognized but unsupported and will be ignored."));
                    throw new IdlException(
                        input,
                        context.MapOffset(baseOffset + position),
                        $"Annotation '@{name}' is not supported in this context.");
                }

                context.Diagnostics?.Add(new IdlDiagnostic("DDSG0101", input, context.MapOffset(baseOffset + position), $"Annotation '@{name}' is not recognized and will be ignored."));
                position += unknownAnnotation.Length;
                continue;
            }

            if (TryParseInterface(declarations, input, baseOffset, ref position) ||
                TryParseModule(declarations, input, baseOffset, currentNamespace, sourceOrigins, ref position) ||
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

    }

    /// <summary>Binds all parsed declarations.</summary>
    internal void Bind() => context.BindDeclarations();

    /// <summary>Validates all parsed declarations after binding completes with the requested strictness.</summary>
    internal void Validate(bool strict) => context.Validate(strict);

    private static readonly HashSet<string> UnsupportedAnnotationNames =
        ["position", "bit_bound", "service"];

    private static readonly HashSet<string> KnownDeclarationAnnotationNames =
    [
        "topic", "autoid", "nested", "final", "appendable", "mutable",
        "language_binding", "transfer_mode", "data_representation",
        "allowed_data_representation", "default_nested"
    ];

    private bool TryParseServiceInterface(string declarations, IdlInput input, int baseOffset, ref int position)
    {
        var serviceAnnotation = ServiceAnnotationPattern.Match(declarations[position..]);
        if (!serviceAnnotation.Success || serviceAnnotation.Groups["value"].Value != "DDS")
        {
            return false;
        }

        var annotationOffset = position;
        var declarationPosition = position + serviceAnnotation.Length;
        List<IdlDiagnostic>? annotationWarnings = null;
        while (declarationPosition < declarations.Length)
        {
            var interveningAnnotation = UnknownAnnotationPattern.Match(declarations[declarationPosition..]);
            if (!interveningAnnotation.Success)
            {
                break;
            }

            var name = interveningAnnotation.Groups["name"].Value;
            if (KnownDeclarationAnnotationNames.Contains(name) || UnsupportedAnnotationNames.Contains(name))
            {
                break;
            }

            annotationWarnings ??= [];
            annotationWarnings.Add(new IdlDiagnostic(
                "DDSG0101",
                input,
                context.MapOffset(baseOffset + declarationPosition),
                $"Annotation '@{name}' is not recognized and will be ignored."));
            declarationPosition += interveningAnnotation.Length;
        }

        if (!InterfacePattern.IsMatch(declarations[declarationPosition..]))
        {
            // Leave the annotation in place so the normal unsupported-context
            // diagnostic is reported for @service on anything other than an
            // interface declaration.
            position = annotationOffset;
            return false;
        }

        if (annotationWarnings is not null)
        {
            foreach (var warning in annotationWarnings)
            {
                context.Diagnostics?.Add(warning);
            }
        }

        position = declarationPosition;
        return TryParseInterface(declarations, input, baseOffset, ref position, isDdsService: true, annotationOffset: annotationOffset);
    }

    private bool TryParseInterface(
        string declarations,
        IdlInput input,
        int baseOffset,
        ref int position,
        bool isDdsService = false,
        int? annotationOffset = null)
    {
        var @interface = InterfacePattern.Match(declarations[position..]);
        if (!@interface.Success)
        {
            return false;
        }

        var interfaceName = @interface.Groups["name"].Value;
        var message = isDdsService
            ? $"The DDS service interface '{interfaceName}' is ignored because service interfaces are not emitted for C#."
            : $"The interface '{interfaceName}' is ignored because it is not a DDS service.";
        context.Diagnostics?.Add(new IdlDiagnostic(
            "DDSG0103",
            input,
            context.MapOffset(baseOffset + (annotationOffset ?? position)),
            message));

        position += @interface.Length;

        return true;
    }


    private bool TryParseModule(
        string declarations,
        IdlInput input,
        int baseOffset,
        string? currentNamespace,
        IReadOnlyList<SourceOriginSpan>? sourceOrigins,
        ref int position)
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
            throw new IdlException(input, context.MapOffset(baseOffset + position), "Unterminated module declaration.");
        }

        var moduleName = context.Qualify(module.Groups[1].Value, currentNamespace);
        Parse(
            declarations.Substring(openBrace + 1, closeBrace - openBrace - 1),
            input,
            baseOffset + openBrace + 1,
            moduleName,
            sourceOrigins);

        position = closeBrace + 1;
        while (position < declarations.Length && char.IsWhiteSpace(declarations[position]))
        {
            position++;
        }

        if (position >= declarations.Length || declarations[position] != ';')
        {
            throw new IdlException(input, context.MapOffset(baseOffset + closeBrace), "Module declaration must end with a semicolon.");
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
            if (IsLiteralStart(text, index))
            {
                var quote = text[index++];
                while (index < text.Length)
                {
                    var character = text[index++];
                    if (character == '\\' && index < text.Length)
                    {
                        index++;
                    }
                    else if (character == quote)
                    {
                        break;
                    }
                }

                continue;
            }

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

    private static bool IsLiteralStart(string text, int index)
    {
        if (text[index] == '"')
        {
            return true;
        }

        if (text[index] != '\'')
        {
            return false;
        }

        return HasCharacterLiteralPrefix(text, index)
            || !LooksLikeIdentifierApostrophe(text, index);
    }

    private static bool HasCharacterLiteralPrefix(string text, int quoteIndex)
    {
        if (quoteIndex == 0)
        {
            return false;
        }

        var prefixCharacter = text[quoteIndex - 1];
        if (prefixCharacter is 'L' or 'u' or 'U')
        {
            return true;
        }

        return prefixCharacter == '8'
            && quoteIndex > 1
            && text[quoteIndex - 2] == 'u';
    }

    private static bool LooksLikeIdentifierApostrophe(string text, int apostropheIndex)
    {
        var hasIdentifierCharacterBefore = apostropheIndex > 0
            && char.IsLetterOrDigit(text[apostropheIndex - 1]);
        var hasIdentifierCharacterAfter = apostropheIndex + 1 < text.Length
            && char.IsLetterOrDigit(text[apostropheIndex + 1]);

        return hasIdentifierCharacterBefore && hasIdentifierCharacterAfter;
    }
}
