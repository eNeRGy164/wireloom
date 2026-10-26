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
    internal IdlDeclarationParser(
        IdlSymbolTable symbols,
        CancellationToken cancellationToken,
        Func<byte[], byte[]> computeMd5,
        ICollection<IdlDiagnostic>? diagnostics = null)
    {
        context = new IdlParseContext(symbols, cancellationToken, computeMd5, diagnostics);
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
        HashSet<int>? escapedIdentifierOffsets = null,
        bool inheritedHashIds = false)
    {
        if (input.SequenceSize <= 0)
        {
            throw new IdlException(input, 0, "SequenceSize must be a positive Int32 value.");
        }

        if (input.StringSize <= 0)
        {
            throw new IdlException(input, 0, "StringSize must be a positive Int32 value.");
        }

        // Always replace the mapping: sequential files must never inherit the
        // previous parse's origin table.
        context.SetSourceOrigins(sourceOrigins);
        context.SetEscapedIdentifierOffsets(escapedIdentifierOffsets);

        var position = 0;
        IdlAnnotationApplicationSyntax? pendingAutoId = null;
        IdlAnnotationApplicationSyntax? pendingTypedefDefaultApplication = null;

        while (position < declarations.Length)
        {
            context.CancellationToken.ThrowIfCancellationRequested();

            if (char.IsWhiteSpace(declarations[position]))
            {
                position++;
                continue;
            }

            if (IdlSyntaxReader.TryReadAnnotationApplication(declarations[position..], out var capturedName, out var capturedLength))
            {
                var captured = new IdlAnnotationApplicationSyntax(
                    capturedName,
                    declarations.Substring(position, capturedLength),
                    context.MapOffset(baseOffset + position),
                    capturedLength);
                try
                {
                    IdlAnnotationCatalog.ValidateUniqueNamedArguments(captured);
                    if (IdlAnnotationCatalog.IsKnown(captured.Name)
                        && !IdlAnnotationCatalog.IsRecognizedUnsupported(captured.Name))
                    {
                        var target = captured.Name.Equals("default", StringComparison.OrdinalIgnoreCase)
                            ? IdlAnnotationTarget.Typedef
                            : IdlAnnotationTarget.Declaration;
                        IdlAnnotationCatalog.ValidateTarget(captured, target);
                    }
                }
                catch (FormatException exception)
                {
                    throw new IdlException(input, captured.Offset, exception.Message);
                }

                switch (captured.Name.ToLowerInvariant())
                {
                    case "autoid":
                        if (pendingAutoId is not null)
                        {
                            throw new IdlException(input, captured.Offset, "Duplicate @autoid annotation.");
                        }

                        pendingAutoId = captured;
                        position += capturedLength;
                        continue;

                    case "default":
                        if (pendingTypedefDefaultApplication is not null)
                        {
                            throw new IdlException(input, captured.Offset, "Duplicate @default annotation.");
                        }

                        pendingTypedefDefaultApplication = captured;
                        position += capturedLength;
                        continue;

                    case "language_binding":
                    case "transfer_mode":
                    case "data_representation":
                    case "allowed_data_representation":
                    case "default_nested":
                        position += capturedLength;
                        continue;
                }
            }

            if (pendingTypedefDefaultApplication is null && TryParseServiceInterface(declarations, input, baseOffset, ref position, currentNamespace))
            {
                RejectPendingAutoId();
                continue;
            }

            if (IdlSyntaxReader.TryReadAnnotationApplication(declarations[position..], out var name, out var annotationLength)
                && (!KnownDeclarationAnnotationNames.Contains(name) || IdlAnnotationCatalog.IsRecognizedUnsupported(name)))
            {
                if (UnsupportedAnnotationNames.Contains(name) || IdlAnnotationCatalog.IsRecognizedUnsupported(name))
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

            if (pendingTypedefDefaultApplication is not null && !TypedefPattern.IsMatch(declarations[position..]))
            {
                throw new IdlException(
                    input,
                    pendingTypedefDefaultApplication.Offset,
                    "@default is only supported on typedef declarations and members.");
            }

            var effectiveHashIds = pendingAutoId is null ? inheritedHashIds : UsesHashIds(pendingAutoId);
            if (TryParseModule(declarations, input, baseOffset, currentNamespace, sourceOrigins, escapedIdentifierOffsets, inheritedHashIds, pendingAutoId, ref position))
            {
                pendingAutoId = null;
                continue;
            }

            if (TryParseInterface(declarations, input, baseOffset, ref position, currentNamespace: currentNamespace) ||
                constantParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position) ||
                enumParser.TryParse(declarations, input, baseOffset, currentNamespace, ref position) ||
                typedefParser.TryParse(declarations, input, baseOffset, currentNamespace, pendingTypedefDefaultApplication, ref position))
            {
                RejectPendingAutoId();
                pendingAutoId = null;
                pendingTypedefDefaultApplication = null;
                continue;
            }

            if (unionParser.TryParse(declarations, input, baseOffset, currentNamespace, effectiveHashIds, ref position, pendingAutoId))
            {
                pendingAutoId = null;
                continue;
            }

            position += structParser.Parse(declarations, input, baseOffset, currentNamespace, position, effectiveHashIds, pendingAutoId);
            pendingAutoId = null;
        }

        RejectPendingAutoId();
        if (pendingTypedefDefaultApplication is not null)
        {
            throw new IdlException(input, pendingTypedefDefaultApplication.Offset, "@default requires a following typedef declaration.");
        }

        void RejectPendingAutoId()
        {
            if (pendingAutoId is not null)
            {
                throw new IdlException(input, pendingAutoId.Offset, "@autoid requires a module, struct, valuetype, or union declaration.");
            }
        }
    }

    private static bool UsesHashIds(IdlAnnotationApplicationSyntax application)
    {
        var value = application.ParseArguments().SingleOrDefault()?.Expression ?? "HASH";
        return string.Equals(value, "HASH", StringComparison.OrdinalIgnoreCase);
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
        var serviceApplication = IdlSyntaxReader.ReadAnnotationApplications(
            declarations[position..],
            context.MapOffset(baseOffset + position))
            .FirstOrDefault();
        if (serviceApplication is null
            || !serviceApplication.Name.Equals("service", StringComparison.OrdinalIgnoreCase)
            || serviceApplication.ParseArguments().Single().Expression != "\"DDS\"")
        {
            return false;
        }

        var annotationOffset = position;
        var declarationPosition = position + serviceApplication.Length;
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
        HashSet<int>? escapedIdentifierOffsets,
        bool inheritedHashIds,
        IdlAnnotationApplicationSyntax? localAutoId,
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
        var moduleShortName = module.Groups[1].Value;
        var moduleDeclaration = new IdlModuleDeclaration(moduleShortName, currentNamespace);

        if (localAutoId is not null)
        {
            moduleDeclaration.SetAnnotationApplications([localAutoId]);
        }

        context.Declarations.Add(moduleDeclaration);
        var moduleName = context.Qualify(moduleDeclaration.Name, moduleDeclaration.Namespace);
        Parse(
            declarations.Substring(openBrace + 1, closeBrace - openBrace - 1),
            input,
            baseOffset + openBrace + 1,
            moduleName,
            sourceOrigins,
            escapedIdentifierOffsets,
            moduleDeclaration.ResolveMemberIdHashPolicy(inheritedHashIds));

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
