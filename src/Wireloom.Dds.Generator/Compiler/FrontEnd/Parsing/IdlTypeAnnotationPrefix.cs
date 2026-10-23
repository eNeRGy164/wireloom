using Wireloom.Compiler.FrontEnd.Semantic;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Captures one ordered type annotation list before parsing its declaration.</summary>
internal sealed class IdlTypeAnnotationPrefix
{
    /// <summary>Gets the first character after the annotation list.</summary>
    internal int DeclarationStart { get; private set; }

    /// <summary>Gets whether the aggregate is a topic.</summary>
    internal bool IsTopic { get; private set; }

    /// <summary>Gets the effective local or inherited member-ID policy.</summary>
    internal bool UseHashIds { get; private set; }

    /// <summary>Gets whether a member-ID policy was explicitly supplied.</summary>
    internal bool HasLocalAutoId { get; private set; }

    /// <summary>Gets unknown applications interleaved with type annotations.</summary>
    internal List<IdlAnnotationApplicationSyntax> UnknownApplications { get; } = [];

    /// <summary>Gets the explicit extensibility kind, when present.</summary>
    internal IdlExtensibilityKind? Extensibility { get; private set; }

    /// <summary>Reads type annotations with shared ordering and duplicate checks.</summary>
    internal static IdlTypeAnnotationPrefix Read(string text, int position, IdlInput input, IdlParseContext context, int baseOffset, bool useHashIds, bool hasLocalAutoId)
    {
        var result = new IdlTypeAnnotationPrefix { DeclarationStart = position, UseHashIds = useHashIds, HasLocalAutoId = hasLocalAutoId };
        var applications = IdlSyntaxReader.ReadAnnotationApplications(
            text[position..],
            context.MapOffset(baseOffset + position));
        var compositionViolation = IdlAnnotationCatalog.FindCompositionViolation(applications, IdlAnnotationTarget.Declaration);
        if (compositionViolation is { } violation)
        {
            throw new IdlException(input, violation.Application.Offset, violation.Message);
        }

        while (true)
        {
            var remaining = text[result.DeclarationStart..];
            var mappedOffset = context.MapOffset(baseOffset + result.DeclarationStart);
            var application = IdlSyntaxReader.ReadAnnotationApplications(remaining, mappedOffset).FirstOrDefault();
            if (application is null)
            {
                return result;
            }

            var name = application.Name;
            var applicationText = application.Text;
            var applicationLength = application.Length;

            if (IdlAnnotationCatalog.IsKnown(name)
                && !IdlAnnotationCatalog.IsRecognizedUnsupported(name))
            {
                try
                {
                    IdlAnnotationCatalog.ValidateTarget(application, IdlAnnotationTarget.Declaration);
                    IdlAnnotationCatalog.ValidateUniqueNamedArguments(application);
                }
                catch (FormatException exception)
                {
                    throw new IdlException(input, application.Offset, exception.Message);
                }
            }

            var annotation = IdlGrammar.TypePrefixAnnotationPattern.Match(applicationText);
            if (!annotation.Success)
            {
                if (IdlAnnotationCatalog.IsKnown(name) || IdlAnnotationCatalog.IsRecognizedUnsupported(name))
                {
                    return result;
                }

                result.UnknownApplications.Add(application);
                result.DeclarationStart += applicationLength;
                continue;
            }

            if (annotation.Groups["topic"].Success)
            {
                result.IsTopic = true;
            }
            else if (annotation.Groups["autoid"].Success)
            {
                if (result.HasLocalAutoId)
                {
                    throw new IdlException(input, context.MapOffset(baseOffset + result.DeclarationStart), "Duplicate @autoid annotation.");
                }

                result.HasLocalAutoId = true;
                result.UseHashIds = !annotation.Groups["autoidValue"].Success
                    || string.Equals(annotation.Groups["autoidValue"].Value, "HASH", StringComparison.OrdinalIgnoreCase);
            }
            else if (annotation.Groups["extensibility"].Success || annotation.Groups["fullExtensibility"].Success)
            {
                if (result.Extensibility is not null)
                {
                    throw new IdlException(input, context.MapOffset(baseOffset + result.DeclarationStart), "Duplicate extensibility annotation.");
                }

                var value = annotation.Groups["fullExtensibilityValue"].Success
                    ? annotation.Groups["fullExtensibilityValue"].Value
                    : annotation.Groups["extensibility"].Value;
                result.Extensibility = IdlParseContext.ParseExtensibility(value);
            }

            result.DeclarationStart += annotation.Length;
        }
    }
}
