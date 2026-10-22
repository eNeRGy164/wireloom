using Wireloom.Compiler.FrontEnd.Preprocessing;
using Wireloom.Compiler.FrontEnd.Semantic;
using Wireloom.Compiler.FrontEnd.Symbols;

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
        IReadOnlyList<SourceOriginSpan>? sourceOrigins = null,
        bool inheritedHashIds = false)
    {
        // Always replace the mapping: sequential files must never inherit the
        // previous parse's origin table.
        context.SetSourceOrigins(sourceOrigins);

        var position = 0;
        bool? pendingHashIdPolicy = null;
        var pendingHashIdOffset = baseOffset;
        string? pendingTypedefDefault = null;
        int? pendingTypedefDefaultOffset = null;

        while (position < declarations.Length)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            if (char.IsWhiteSpace(declarations[position]))
            {
                position++;
                continue;
            }

            var autoIdAnnotation = AutoIdAnnotationPattern.Match(declarations[position..]);
            if (autoIdAnnotation.Success)
            {
                if (pendingHashIdPolicy is not null)
                {
                    throw new IdlException(input, context.MapOffset(baseOffset + position), "Duplicate @autoid annotation.");
                }

                pendingHashIdPolicy = !autoIdAnnotation.Groups["value"].Success
                    || string.Equals(autoIdAnnotation.Groups["value"].Value, "HASH", StringComparison.OrdinalIgnoreCase);
                pendingHashIdOffset = context.MapOffset(baseOffset + position);
                position += autoIdAnnotation.Length;
                continue;
            }

            if (IdlSyntaxReader.TryReadAnnotationValue(declarations[position..], "default", out var typedefDefault, out var typedefDefaultLength))
            {
                if (pendingTypedefDefault is not null)
                {
                    throw new IdlException(input, context.MapOffset(baseOffset + position), "Duplicate @default annotation.");
                }

                pendingTypedefDefault = typedefDefault;
                pendingTypedefDefaultOffset = context.MapOffset(baseOffset + position);
                position += typedefDefaultLength;
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

            if (pendingTypedefDefault is null && TryParseServiceInterface(declarations, input, baseOffset, ref position, currentNamespace))
            {
                RejectPendingAutoId();
                continue;
            }

            if (IdlSyntaxReader.TryReadAnnotationApplication(declarations[position..], out var name, out var annotationLength)
                && !KnownDeclarationAnnotationNames.Contains(name))
            {
                if (UnsupportedAnnotationNames.Contains(name))
                {
                    context.Diagnostics?.Add(new IdlDiagnostic("DDSG0102", input, context.MapOffset(baseOffset + position), $"Annotation '{name}' is recognized but unsupported and will be ignored."));
                    throw new IdlException(
                        input,
                        context.MapOffset(baseOffset + position),
                        $"Annotation '@{name}' is not supported in this context.");
                }

                context.Diagnostics?.Add(new IdlDiagnostic("DDSG0101", input, context.MapOffset(baseOffset + position), $"Annotation '@{name}' is not recognized and will be ignored."));
                position += annotationLength;
                continue;
            }

            if (pendingTypedefDefault is not null && !TypedefPattern.IsMatch(declarations[position..]))
            {
                throw new IdlException(
                    input,
                    pendingTypedefDefaultOffset ?? context.MapOffset(baseOffset + position),
                    "@default is only supported on typedef declarations and members.");
            }

            var effectiveHashIds = pendingHashIdPolicy ?? inheritedHashIds;
            if (TryParseModule(declarations, input, baseOffset, currentNamespace, sourceOrigins, effectiveHashIds, ref position))
            {
                pendingHashIdPolicy = null;
                continue;
            }

            if (TryParseInterface(declarations, input, baseOffset, ref position, currentNamespace: currentNamespace) ||
                constantParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position) ||
                enumParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position) ||
                typedefParser.TryParse(declarations, input, baseOffset, currentNamespace, pendingTypedefDefault, ref position))
            {
                RejectPendingAutoId();
                pendingHashIdPolicy = null;
                pendingTypedefDefault = null;
                pendingTypedefDefaultOffset = null;
                continue;
            }

            if (unionParser.TryParse(declarations, input, baseOffset, currentNamespace, effectiveHashIds, ref position, hasLocalAutoId: pendingHashIdPolicy is not null))
            {
                pendingHashIdPolicy = null;
                continue;
            }

            position += structParser.Parse(declarations, input, baseOffset, currentNamespace, position, effectiveHashIds, hasLocalAutoId: pendingHashIdPolicy is not null);
            pendingHashIdPolicy = null;

        }

        RejectPendingAutoId();
        if (pendingTypedefDefault is not null)
        {
            throw new IdlException(input, pendingTypedefDefaultOffset ?? context.MapOffset(baseOffset), "@default requires a following typedef declaration.");
        }

        void RejectPendingAutoId()
        {
            if (pendingHashIdPolicy is not null)
            {
                throw new IdlException(input, pendingHashIdOffset, "@autoid requires a module, struct, valuetype, or union declaration.");
            }
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
        "topic", "autoid", "nested", "final", "appendable", "mutable", "extensibility",
        "language_binding", "transfer_mode", "data_representation",
        "allowed_data_representation", "default_nested"
    ];

    private bool TryParseServiceInterface(string declarations, IdlInput input, int baseOffset, ref int position, string? currentNamespace)
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
            if (!IdlSyntaxReader.TryReadAnnotationApplication(declarations[declarationPosition..], out var name, out var annotationLength))
            {
                break;
            }

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
            declarationPosition += annotationLength;
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
        return TryParseInterface(declarations, input, baseOffset, ref position, isDdsService: true, annotationOffset: annotationOffset, currentNamespace: currentNamespace);
    }

    private bool TryParseInterface(
        string declarations,
        IdlInput input,
        int baseOffset,
        ref int position,
        bool isDdsService = false,
        int? annotationOffset = null, string? currentNamespace = null)
    {
        var @interface = InterfacePattern.Match(declarations[position..]);
        if (!@interface.Success)
        {
            return false;
        }

        var interfaceName = @interface.Groups["name"].Value;
        context.RecordIdentifier(input, baseOffset + position + @interface.Groups["name"].Index, interfaceName, currentNamespace);
        var message = isDdsService
            ? $"The DDS service interface '{interfaceName}' is ignored because service interfaces are not emitted for C#."
            : $"The interface '{interfaceName}' is ignored because it is not a DDS service.";
        context.Diagnostics?.Add(new IdlDiagnostic("DDSG0103", input, context.MapOffset(baseOffset + (annotationOffset ?? position)), message));

        var openBrace = position + @interface.Length - 1;
        var closeBrace = IdlSyntaxReader.FindClosingDelimiter(declarations, openBrace, '{', '}');
        if (closeBrace < 0)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + position), "Unterminated interface declaration.");
        }

        var identifiers = new IdlInterfaceIdentifierParser(
            context, input, declarations.Substring(openBrace + 1, closeBrace - openBrace - 1), baseOffset + openBrace + 1, isDdsService: isDdsService);
        identifiers.Parse(context.Qualify(interfaceName, currentNamespace));

        position = closeBrace + 1;
        while (position < declarations.Length && char.IsWhiteSpace(declarations[position]))
        {
            position++;
        }

        if (position >= declarations.Length || declarations[position] != ';')
        {
            throw new IdlException(input, context.MapOffset(baseOffset + closeBrace), "Interface declaration must end with a semicolon.");
        }

        position++;

        return true;
    }


    private bool TryParseModule(
        string declarations,
        IdlInput input,
        int baseOffset,
        string? currentNamespace,
        IReadOnlyList<SourceOriginSpan>? sourceOrigins,
        bool inheritedHashIds,
        ref int position)
    {
        var module = ModulePattern.Match(declarations.Substring(position));
        if (!module.Success)
        {
            return false;
        }

        var openBrace = position + module.Length - 1;
        var closeBrace = IdlSyntaxReader.FindClosingDelimiter(declarations, openBrace, '{', '}');
        if (closeBrace < 0)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + position), "Unterminated module declaration.");
        }

        context.RecordIdentifier(input, baseOffset + position + module.Groups[1].Index, module.Groups[1].Value, currentNamespace);
        var moduleName = context.Qualify(module.Groups[1].Value, currentNamespace);
        Parse(
            declarations.Substring(openBrace + 1, closeBrace - openBrace - 1),
            input,
            baseOffset + openBrace + 1,
            moduleName,
            sourceOrigins,
            inheritedHashIds);

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
}
