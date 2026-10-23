using System.Text.RegularExpressions;
using Wireloom.Compiler.FrontEnd.Semantic;

using static Wireloom.Compiler.FrontEnd.Parsing.IdlGrammar;

namespace Wireloom.Compiler.FrontEnd.Parsing;

/// <summary>Parses IDL enum declarations.</summary>
internal sealed class IdlEnumDeclarationParser
{
    private readonly IdlParseContext context;

    internal IdlEnumDeclarationParser(IdlParseContext context) =>
        this.context = context;

    internal bool TryParse(string declarations, IdlInput input, int baseOffset, string? currentNamespace, ref int position)
    {
        var prefix = IdlTypeAnnotationPrefix.Read(declarations, position, input, context, baseOffset, useHashIds: false, hasLocalAutoId: false);
        var enumDeclaration = EnumPattern.Match(declarations[prefix.DeclarationStart..]);
        if (!enumDeclaration.Success)
        {
            return false;
        }

        context.ReportUnknownTypePrefixApplications(input, prefix.UnknownApplications);

        if (prefix.IsTopic || prefix.HasLocalAutoId)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + position), "@topic and @autoid are not applicable to enum declarations.");
        }

        if (prefix.Extensibility == IdlExtensibilityKind.Mutable)
        {
            throw new IdlException(input, context.MapOffset(baseOffset + position), "DDS-XTypes enumeration types support FINAL or APPENDABLE extensibility.");
        }

        position = prefix.DeclarationStart;

        var enumName = enumDeclaration.Groups["name"].Value;
        var qualified = context.Qualify(enumName, currentNamespace);
        context.RecordIdentifier(input, baseOffset + position + enumDeclaration.Groups["name"].Index, enumName, currentNamespace);
        context.EnsureNewName(input, baseOffset + position, qualified);
        context.EnsureGeneratedCompanionNames(input, baseOffset + position, enumName, currentNamespace, includeUnmanaged: false);
        var enumMembers = new List<IdlEnumMember>();
        var enumHasDefaultLiteral = false;
        var nextValue = 0L;
        var enumBody = enumDeclaration.Groups["body"];
        foreach (var (memberOffset, memberLength) in IdlSyntaxReader.SplitTopLevel(enumBody.Value, ','))
        {
            var rawMember = enumBody.Value.Substring(memberOffset, memberLength);
            var trimmedMember = rawMember.Trim();
            var leadingWhitespace = rawMember.Length - rawMember.TrimStart().Length;
            var sourceOffset = baseOffset + position + enumBody.Index + memberOffset + leadingWhitespace;

            var annotationOffset = 0;
            var hasDefaultLiteralApplication = false;
            string? annotatedValue = null;

            var applications = IdlSyntaxReader.ReadAnnotationApplications(trimmedMember, context.MapOffset(sourceOffset));
            var compositionViolation = IdlAnnotationCatalog.FindCompositionViolation(applications, IdlAnnotationTarget.EnumLiteral);
            if (compositionViolation is { } violation)
            {
                throw new IdlException(input, violation.Application.Offset, violation.Message);
            }

            while (IdlSyntaxReader.TryReadAnnotationApplication(trimmedMember[annotationOffset..], out var annotationName, out var annotationLength))
            {
                var applicationText = trimmedMember.Substring(annotationOffset, annotationLength);
                var application = new IdlAnnotationApplicationSyntax(
                    annotationName,
                    applicationText,
                    context.MapOffset(sourceOffset + annotationOffset),
                    annotationLength);

                if (IdlAnnotationCatalog.IsKnown(annotationName)
                    && !IdlAnnotationCatalog.IsRecognizedUnsupported(annotationName))
                {
                    try
                    {
                        IdlAnnotationCatalog.ValidateTarget(application, IdlAnnotationTarget.EnumLiteral);
                        IdlAnnotationCatalog.ValidateUniqueNamedArguments(application);
                    }
                    catch (FormatException exception)
                    {
                        throw new IdlException(input, application.Offset, exception.Message);
                    }
                }

                if (annotationName.Equals("default_literal", StringComparison.OrdinalIgnoreCase))
                {
                    if (hasDefaultLiteralApplication || applicationText.Contains('('))
                    {
                        throw new IdlException(input, application.Offset, "Malformed or duplicate @default_literal annotation.");
                    }

                    hasDefaultLiteralApplication = true;
                }
                else if (annotationName.Equals("value", StringComparison.OrdinalIgnoreCase))
                {
                    if (annotatedValue is not null)
                    {
                        throw new IdlException(input, application.Offset, "Duplicate @value annotation.");
                    }

                    try
                    {
                        var arguments = application.ParseArguments();
                        if (arguments.Count != 1 || arguments[0].Name is not null and not "value"
                            || !Regex.IsMatch(arguments[0].Expression, "^-?[0-9]+$"))
                        {
                            var argumentOffset = arguments.Count == 1
                                ? context.MapOffset(sourceOffset + annotationOffset + arguments[0].RelativeOffset)
                                : application.Offset;
                            throw new IdlException(input, argumentOffset, "Malformed enum member: @value requires one signed decimal value.");
                        }

                        annotatedValue = arguments[0].Expression;
                    }
                    catch (FormatException exception)
                    {
                        throw new IdlException(input, application.Offset, exception.Message);
                    }
                }
                else if (IdlAnnotationCatalog.IsRecognizedUnsupported(annotationName))
                {
                    throw new IdlException(input, application.Offset, $"Annotation '@{annotationName}' is not supported on enum literals.");
                }
                else if (!IdlAnnotationCatalog.IsKnown(annotationName))
                {
                    context.Diagnostics?.Add(new IdlDiagnostic(
                        "DDSG0101",
                        input,
                        application.Offset,
                        $"Annotation '@{annotationName}' is not recognized and will be ignored."));
                }
                else
                {
                    throw new IdlException(input, application.Offset, $"Annotation '@{annotationName}' is not supported on enum literals.");
                }

                annotationOffset += annotationLength;
            }

            var memberSyntax = trimmedMember[annotationOffset..].TrimStart();
            var member = EnumMemberPattern.Match(memberSyntax);
            if (!member.Success)
            {
                throw new IdlException(input, context.MapOffset(sourceOffset), $"Malformed enum member '{memberSyntax}'; expected an identifier, an optional prefix @value(<signed decimal>), or an explicit signed decimal value.");
            }

            if ((annotatedValue is not null || member.Groups["value"].Success) && member.Groups["explicit"].Success)
            {
                throw new IdlException(input, context.MapOffset(sourceOffset), "Combined @value and explicit enum values are unsupported.");
            }

            var memberNameOffset = sourceOffset + annotationOffset + memberSyntax.IndexOf(member.Groups["name"].Value, StringComparison.Ordinal);
            context.RecordIdentifier(input, memberNameOffset, member.Groups["name"].Value, currentNamespace);
            var value = annotatedValue is null
                ? ParseEnumValue(input, context.MapOffset(sourceOffset), member, nextValue)
                : ParseAnnotatedEnumValue(input, context.MapOffset(sourceOffset), annotatedValue);
            var isDefaultLiteral = hasDefaultLiteralApplication || member.Groups["defaultLiteral"].Success;
            if (isDefaultLiteral && enumHasDefaultLiteral)
            {
                throw new IdlException(input, context.MapOffset(sourceOffset), "An enum may contain at most one @default_literal.");
            }

            enumHasDefaultLiteral |= isDefaultLiteral;
            var hasExplicitValue = annotatedValue is not null || member.Groups["value"].Success || member.Groups["explicit"].Success;
            enumMembers.Add(new IdlEnumMember(member.Groups["name"].Value, value, hasExplicitValue, isDefaultLiteral));

            nextValue = value + 1;
        }

        var parsedEnum = new IdlEnum(enumName, currentNamespace, enumMembers, prefix.Extensibility ?? IdlExtensibilityKind.Extensible);
        context.Symbols.AddEnum(qualified, parsedEnum);
        context.Declarations.Add(new IdlEnumDeclaration(parsedEnum, Path.GetFileName(input.Path)));

        position += enumDeclaration.Length;

        return true;
    }

    private static int ParseEnumValue(IdlInput input, int sourceOffset, Match member, long nextValue)
    {
        if (member.Groups["explicit"].Success || member.Groups["value"].Success)
        {
            var valueText = member.Groups["explicit"].Success ? member.Groups["explicit"].Value : member.Groups["value"].Value;
            if (!int.TryParse(valueText, out var value))
            {
                throw new IdlException(input, sourceOffset, "Enum value must be a signed Int32 decimal.");
            }

            return value;
        }

        if (nextValue is < int.MinValue or > int.MaxValue)
        {
            throw new IdlException(input, sourceOffset, "Implicit enum value exceeds the Int32 range.");
        }

        return (int)nextValue;
    }

    private static int ParseAnnotatedEnumValue(IdlInput input, int sourceOffset, string valueText)
    {
        if (!int.TryParse(valueText, out var value))
        {
            throw new IdlException(input, sourceOffset, "Enum value must be a signed Int32 decimal.");
        }

        return value;
    }
}
